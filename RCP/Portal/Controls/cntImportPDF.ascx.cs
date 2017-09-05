using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using AjaxControlToolkit;
using System.IO;
using iTextSharp.text;

namespace HRRcp.Portal.Controls
{
    class EmployeeInfo
    {
        public string pesel;
        public string NIP;
        public string name;
        public string surname;
        public string nr_ew;
        

        public byte[] pdfData;

        public void SavePDF(string fileName)
        {
            PdfReader pdfReader = new PdfReader(pdfData);

            Document doc = new Document();
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));

            doc.Open();
            for(int i=1; i<=pdfReader.NumberOfPages; i++)
            {
                doc.NewPage();
                pdfWriter.DirectContent.AddTemplate(pdfWriter.GetImportedPage(pdfReader, i), 0, 0);
            }
            doc.Close();
        }
        public EmployeeInfo(string pesel="", string NIP="", string name="", string surname="", string id="", byte[] pdfData = null)
        {
            this.pesel = pesel;
            this.NIP = NIP;
            this.name = name;
            this.surname = surname;
            this.nr_ew = id;
            this.pdfData = pdfData;
        }
    }
    class PdfInfo
    {
        public static EmployeeInfo GetEmployeeInfo(string pageText)
        {
            EmployeeInfo info = new EmployeeInfo();
            string line = "";

            for(int i=0; i<pageText.Length; i++)
            {
                if(pageText[i] != '\n' && pageText[i] != '\r')
                    line += pageText[i];
                else
                {
                    if( FindOnStart("NIP", line) )
                        info.NIP = FindNip(line);
                    if( FindOnStart("PESEL", line) )
                        info.pesel = FindPesel(line);
                    if( FindOnStart("Imi i nazwisko", line) )
                    {
                        info.name = FindWord(line, 15, 0);
                        info.surname = FindWord(line, 15, 1);
                    }

                    line = "";
                }
            }

            return info;
        }
        public static EmployeeInfo[] GetEmployeesFromPDF(string path)
        {
            PdfReader pdfReader = new PdfReader(path);
            List<EmployeeInfo> infos = new List<EmployeeInfo>();

            for(int i=1; i<=pdfReader.NumberOfPages; i++)
            {
                string pageText = PdfTextExtractor.GetTextFromPage(pdfReader, i);
                EmployeeInfo info = GetEmployeeInfo(pageText);

                // zapisywanie do EmployeeInfo fragmentu PDF mu odpowiadającemu
                Document doc = new Document();
                MemoryStream memoryStream = new MemoryStream();
                PdfWriter pdfWriter = PdfWriter.GetInstance(doc, memoryStream);
                doc.Open();
                doc.NewPage();
                pdfWriter.DirectContent.AddTemplate(pdfWriter.GetImportedPage(pdfReader, i), 0, 0);
                doc.Close();
                info.pdfData = memoryStream.ToArray();

                // wydobywanie numeru ewidencyjnego
                DataRow row = db.Select.Row(db.conP, "select SqlFindNrEw from PlikiSciezki where ID = 3");
                string command = "";
                if(row != null && row["SqlFindNrEw"] != null)
                {
                    command = (string)row["SqlFindNrEw"];
                }
                if(command != "" && info.pesel != "")
                {
                    row = db.Select.Row(command, db.sqlPut(info.pesel));
                    if(row != null && row["kadryId"] != null)
                    {
                        info.nr_ew = (string)row["kadryId"];
                    }
                }


                infos.Add(info);
            }

            pdfReader.Close();
            return infos.ToArray();
        }
        
        static string FindWord(string value, int startIndex, int wordIndex)
        {
            string buffer = "";

            int wordCounter = 0;
            for(int i=startIndex; i<value.Length; i++)
            {
                if(value[i] != ' ')
                    buffer += value[i];

                if(value[i] == ' ' || i+1 == value.Length)
                {
                    if(buffer != "")
                    {
                        if(wordCounter == wordIndex)
                            return buffer;
                         wordCounter++;
                    }

                    buffer = "";
                }
            }

            return "";
        }
        static string FindPesel(string value)
        {
            for(int i=0; i<value.Length; i++)
            {
                if(value[i]-'0' >= 0 && value[i]-'0' <= 9 && (i+11 == value.Length || value[i+11]-'0' < 0 || value[i+11]-'0' > 9) )
                {
                    string buffer = "";
                    for(int i2=0; i2<11; i2++)
                        buffer += value[i+i2];

                    bool onlyDigits = true;
                    for(int i2=0; i2<buffer.Length; i2++)
                        if(buffer[i2]-'0' < 0 || buffer[i2]-'0' > 9)
                            onlyDigits = false;

                    return onlyDigits ? buffer : "";
                }
            }

            return "";
        }
        static string FindNip(string value)
        {
            for(int i=0; i<value.Length; i++)
            {
                if(value[i]-'0' >= 0 && value[i]-'0' <= 9 && i+12 < value.Length && value[i+3] == '-' && value[i+7] == '-' && value[i+10] == '-')
                {
                    string buffer = "";
                    for(int i2=0; i2<13; i2++)
                        buffer += value[i+i2];
                    return buffer;
                }
            }

            return "";
        }
        static bool FindOnStart(string toFind, string source)
        {
            string buffer = "";
            for(int i=0; i<toFind.Length && i<source.Length; i++)
            {
                buffer += source[i];
            }
            return buffer == toFind;
        }
    }
    public partial class cntImportPDF : System.Web.UI.UserControl
    {
        public event EventHandler Close;
        public string sciezkaOcena;
        public string sciezkaRmua;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {               

                if (hidMode.Value == "1") div1.Visible = true;
                else if (hidMode.Value == "2") div2.Visible = true;
                else if (hidMode.Value == "3")
                {
                    div3.Visible = true;

                    DataRow row = db.Select.Row(db.conP, "select SqlDateList from PlikiSciezki where ID='3'");
                    if (row != null && row["SqlDateList"] != null)
                    {
                        string command = (string)row["SqlDateList"];
                        DataTable table = db.Select.Table(db.conP, 0, command);
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            row = table.Rows[i];
                            DropDownListKwitek.Items.Add(new System.Web.UI.WebControls.ListItem((string)row["Text"], ((DateTime)row["Value"]).ToString()));
                        }
                    }
                }
                else if (hidMode.Value == "4")
                {
                    div4.Visible = true;

                    DataRow row = db.Select.Row(db.conP, "select SqlDateList from PlikiSciezki where ID='4'");
                    if (row != null && row["SqlDateList"] != null)
                    {
                        string command = (string)row["SqlDateList"];
                        DataTable table = db.Select.Table(db.conP, 0, command);
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            row = table.Rows[i];
                            DropDownListPIT.Items.Add(new System.Web.UI.WebControls.ListItem((string)row["Text"], ((DateTime)row["Value"]).ToString()));
                        }
                    }
                }

                
            }

            DataRow rowOcena = db.Select.Row(db.conP, "select * from PlikiSciezki where ID='1'");
            DataRow rowRmua = db.Select.Row(db.conP, "select * from PlikiSciezki where ID='2'");
            sciezkaOcena = (string)rowOcena["Sciezka"];
            sciezkaRmua = (string)rowRmua["Sciezka"];
        }

        protected void btImport_Click_Kwitek(object sender, EventArgs e)
        {
            if (FileUploadKwitek.HasFile)
            {
                string fileName = FileUploadKwitek.FileName;
                string savePath = Server.MapPath(@"~\uploads\") + fileName;  //@ oznacza nie interpretuj - pozwala na umieszczenie \ na końcu
                FileUploadKwitek.SaveAs(savePath);

                DataRow row = db.Select.Row(db.conP, "select Sciezka from PlikiSciezki where ID = 3");
                    string path = "";
                    if(row != null && row["Sciezka"] != null)
                        path = (string)row["Sciezka"];

                if(path != "")
                {
                    EmployeeInfo[] empInfos = PdfInfo.GetEmployeesFromPDF(savePath);
                    for (int i = 0; i < empInfos.Length; i++)
                    {
                        empInfos[i].SavePDF(string.Format("{0}\\{1}_{2}.pdf", Server.MapPath(path), DropDownListKwitek.SelectedItem.Text, empInfos[i].nr_ew));
                    }
                }

                PlikiDoBazy.Zapis(3);
                System.IO.File.Delete(savePath);
            }
            else
            {
                Tools.ShowMessage("Brak pliku do importu.");  // załatwia to javascript, ale na wszelki wypadek, script dlatego ze postbacktrigger musi być na buttonie i msg pojawiał sie po przeładowaniu strony przed jej wyswietleniem
            }
        }


        protected void btImport_Click_PIT(object sender, EventArgs e)
        {

        }


        protected void btImport_Click_Ocena(object sender, EventArgs e)
        {
            if (FileUploadOcena.HasFile)
            {
                string fileName = FileUploadOcena.FileName;
                string savePath = Server.MapPath(@"~\Content\DokumentyWZL\ocena\") + fileName;  //@ oznacza nie interpretuj - pozwala na umieszczenie \ na końcu
                FileUploadOcena.SaveAs(savePath);
               
                PlikiDoBazy.Zapis(1);
            }
            else
            {
                Tools.ShowMessage("Brak pliku do importu.");  // załatwia to javascript, ale na wszelki wypadek, script dlatego ze postbacktrigger musi być na buttonie i msg pojawiał sie po przeładowaniu strony przed jej wyswietleniem
            }
        }


        protected void btImport_Click_RMUA(object sender, EventArgs e)
        {
            if (FileUploadRMUA.HasFile)
            {
                string fileName = FileUploadRMUA.FileName;
                string savePath = Server.MapPath(@"~\Content\DokumentyWZL\RMUA\") + fileName;  //@ oznacza nie interpretuj - pozwala na umieszczenie \ na końcu
                FileUploadRMUA.SaveAs(savePath);
               
                PlikiDoBazy.Zapis(2);
            }
            else
            {
                Tools.ShowMessage("Brak pliku do importu.");  // załatwia to javascript, ale na wszelki wypadek, script dlatego ze postbacktrigger musi być na buttonie i msg pojawiał sie po przeładowaniu strony przed jej wyswietleniem
            }
        }

        public String mode
        {
            get { return hidMode.Value; }
            set { hidMode.Value = value; }
        }

        protected void btPrzetwarzanieOcena_Click(object sender, EventArgs e)
        {
            
            PlikiDoBazy.Zapis(1);
        }

        protected void btPrzetwarzanieRMUA_Click(object sender, EventArgs e)
        {
           // db.execSQL(db.conP, " DELETE FROM PLIKI WHERE IdSciezki=2");
            PlikiDoBazy.Zapis(2);
        }
    }


}