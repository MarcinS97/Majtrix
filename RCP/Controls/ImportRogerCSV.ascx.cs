using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class ImportRogerCSV : System.Web.UI.UserControl
    {
        public event EventHandler ImportFinished;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btImport_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string fileName = FileUpload1.FileName;
                string savePath = Server.MapPath(@"uploads\") + fileName;  //@ oznacza nie interpretuj - pozwala na umieszczenie \ na końcu
                FileUpload1.SaveAs(savePath);
                int err = ImportData(savePath);
                switch (err)
                {
                    case 0:
                        Tools.ShowMessage("Import zakończony.");
                        break;
                    case -1:
                        Tools.ShowMessage("Niepoprawna struktura pliku.");
                        break;
                    default:
                        Tools.ShowMessage("Wystąpił błąd podczas importu struktury.");
                        break;
                }
                if (ImportFinished != null)
                    ImportFinished(this, EventArgs.Empty);
            }
            else
            {
                Tools.ShowMessage("Brak pliku do importu.");  // załatwia to javascript, ale na wszelki wypadek, script dlatego ze postbacktrigger musi być na buttonie i msg pojawiał sie po przeładowaniu strony przed jej wyswietleniem
            }
        }
        //--------------------------------------------------------
        private void CreateTempTable(SqlConnection con)
        {
            Base.execSQL(con,
                "CREATE TABLE tmpImportRCP (" +
                    "[Czas] [datetime] NOT NULL," +
                    "[RcpId] [int] NOT NULL," +
                    "[Nazwa1] [nvarchar](200) NULL," +
                    "[Nazwa2] [nvarchar](200) NULL," +
                    "[Typ] [nvarchar](50) NULL)");
        }

        private string PrepareId(string id)
        {
            if (id.ToUpper().StartsWith("PL04"))
                id = id.Substring(4);   // odcinam PL04
            while (id.Length < 5)       // dopełniam 0 z przodu do 5 znaków tak jak to jest w kadrach
                id = "0" + id;
            return id;
        }

        private bool CheckFormat(string line, out string[] sep, out bool quota)
        {   //separatory: , ; tab "," ";" "tab"
            string[] separatory = new string[] { "\";\"", "\",\"", "\"\t\"", ";", ",", "\t" };
            bool[] isQuota = new bool[] { true, true, true, false, false, false };
            for (int i = 0; i < separatory.Length; i++)
            {
                sep = new string[1];
                sep[0] = separatory[i];
                string[] values = line.Split(sep, StringSplitOptions.None);
                if (values.Length >= 6)
                {
                    quota = isQuota[i];
                    return true;
                }
            }
            sep = null;
            quota = false;
            return false;
        }

        // -1 niepoprawny format
        private int ImportFromFile(SqlConnection con, string fileName)
        {
            string line;
            int err = 0;
            if (File.Exists(fileName))
            {
                Encoding enc = Tools.GetFileEncoding(fileName, Encoding.GetEncoding(1250));

                StreamReader file = null;
                try
                {
                    file = new StreamReader(fileName, enc);
                    int cnt = 0;
                    //separatory: , ; tab "," ";" "tab"
                    //string[] sep = new string[] { "\",\"" };
                    string[] sep = new string[] { ";" };
                    //string[] sep;
                    bool quota = false;
                    while ((line = file.ReadLine()) != null)
                    {
                        if (cnt == 0) 
                        {
                            if (!CheckFormat(line, out sep, out quota))  // pierwszą linia 
                            {
                                err = -1;
                                break;
                            }
                        }
                        //----- bez nagłówka -----
                        if (quota && line.StartsWith("\"") && line.EndsWith("\""))
                            line = line.Substring(1, line.Length - 2);  // odcinam " z początku i z końca
                        string[] values = line.Split(sep, StringSplitOptions.None);
                        if (values.Length == 12)                        // poprawny plik, na razie tylko tak sprawdzam
                        {
                            string data = values[0].Trim();
                            string czas = values[1].Trim();
                            string code = values[2].Trim();
                            int c = Base.StrToIntDef(code.Substring(1, 3), -1);
                            string nazwa1 = values[4].Trim();
                            string rcpId = values[5].Trim();
                            string nazwa2 = values[9].Trim();
                            string typ = values[10].Trim();

                            Base.execSQL(con, String.Format(
                                "insert into tmpImportRCP values ('{0}',{1},'{2}','{3}','{4}',-1,{5})",
                                    data + ' ' + czas, rcpId, nazwa1, nazwa2, typ, c));
                        }
                        else
                        {
                            err = -1;
                            break;
                        }
                        cnt++;
                    }
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }
            }
            return err;
        }

        // łaczę tekstowo po RCP.Name z tmpImportRcp.Nazwa2 - uwzgledniam problem z brakiem pl w Readers i case sensitive
        public bool CheckTextLinkedOk(SqlConnection con)
        {
            string cnt = Base.getScalar(con,
                "select count(*) from tmpImportRCP C " +
                //"C.Czas, C.RcpId, C.Nazwa1, C.Nazwa2, R.Name, R.Id from tmpImportRCP C
                "left outer join Readers R on " +
                "convert(varbinary(400), R.Name) = " +
                "convert(varbinary(400), REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(C.Nazwa2 collate Polish_CS_AS,'Ś','O'),'ł','3'),'ś','o'),'ę','e'),'ć','a'),'ż','?'),'ą','1')) " +
                "where R.Id is null and RcpId < 100000000");
            return cnt == "0";
        }

        public int GetFirstECUniqueId(SqlConnection con, int count)
        {
            const int min = 1000000;
            const int max = 18000000;
            int id = min;
            string sid = Base.getScalar(con, String.Format(
                "select max(ECUniqueId) + 1 from RCP where ECUniqueId between {0} and {1}", min, max));
            if (!String.IsNullOrEmpty(sid))
                id = Base.StrToIntDef(sid, -1);
            if (id == -1 || max - id < count)
                return -1;   // nie ma miejsca
            else
                return id;
        }

        public void DeleteTmpTable(SqlConnection con)
        {
            Base.execSQL(con, "delete from tmpImportRCP");
        }

        public int ImportData(string fileName)
        {
            int lid = Log.Info(Log.t2APP_IMPORTRCP, "Import danych RCP z plików CSV", "START", Log.PENDING);
            SqlConnection con = Base.Connect();
            DeleteTmpTable(con);
            int err = ImportFromFile(con, fileName);
            if (err == 0)
            {
                try
                {
                    if (CheckTextLinkedOk(con))
                    {
                        DataSet ds = Base.getDataSet(con,
                            "select C.Czas, C.RcpId, R.Id, C.ECCode, " +
                            "case when C.Typ = 'Służb.' then 1 when C.Typ = 'Pryw.' then 0 else -1 end as Duty " +
                            "from tmpImportRCP C " +
                            "left outer join Readers R on " +
                            "convert(varbinary(400), R.Name) = " +
                            "convert(varbinary(400), REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(C.Nazwa2 collate Polish_CS_AS,'Ś','O'),'ł','3'),'ś','o'),'ę','e'),'ć','a'),'ż','?'),'ą','1')) " +
                            "where RcpId < 100000000");

                        int ecuid = GetFirstECUniqueId(con, Base.getRows(ds).Count);
                        if (ecuid != -1)
                        {
                            int startid = ecuid;
                            string door = "-1";
                            string inout = Base.NULL;
                            foreach (DataRow dr in Base.getRows(ds))
                            {
                                string czas = Base.getValue(dr, 0);
                                string rcpId = Base.getValue(dr, 1);
                                string rogerId = Base.getValue(dr, 2);
                                string uid = Base.getScalar(con, String.Format(
                                    "select ECUniqueId from RCP where Czas='{0}' and ECUserId={1} and ECReaderId={2}",
                                    czas, rcpId, rogerId));
                                if (String.IsNullOrEmpty(uid))  //dodaję bo nie ma
                                {
                                    string code = Base.getValue(dr, 3);
                                    string duty = Base.getValue(dr, 4);
                                    Base.execSQL(con, Base.insertSql("RCP", 0,
                                        "ECUniqueId,Czas,ECCode,ECUserId,ECReaderId,ECDoorType,InOut,Duty",
                                        ecuid, Base.strParam(czas), code, rcpId, rogerId, door, inout, duty
                                        ));
                                    ecuid++;
                                }
                            }
                            Log.Update(lid, Log.OK);
                            Log.Info(Log.t2APP_IMPORTRCP, "Import danych RCP z plików CSV", String.Format("END ECUniqueId: {0} - {1}", startid, ecuid - 1), Log.OK);
                        }
                        else
                        {
                            Log.Update(lid, Log.ERROR);
                            Log.Error(Log.t2APP_IMPORTRCP, "Import danych RCP z plików CSV", "Brak miejsca w zakresie numeracji odczytów RCP", Log.OK);
                            Tools.ShowMessage("Brak miejsca w zakresie numeracji odczytów RCP. Import przerwany.");
                        }
                    }
                    else
                    {
                        Log.Update(lid, Log.ERROR);
                        Log.Error(Log.t2APP_IMPORTRCP, "Import danych RCP z plików CSV", "Istnieją odczyty, którym nie można przypisać rejestratorów po nazwach", Log.OK);
                        Tools.ShowMessage("Istnieją odczyty, którym nie można przypisać rejestratorów po nazwach - sprawdź w 'importRogerCSV.sql'. Import przerwany.");
                    }
                }
                catch (Exception ex)
                {
                    Log.Update(lid, Log.ERROR);
                    Log.Error(Log.t2APP_IMPORTSTRUCT, lid, "Import danych RCP z plików CSV", ex.Message);
                    if (con != null)
                        Base.Disconnect(con);
                    throw;
                }
            }
            if (con != null)
                Base.Disconnect(con);
            if (ImportFinished != null)
                ImportFinished(this, EventArgs.Empty);
            return err;
        }


        public int ImportData_txtlink(string fileName)
        {
            int lid = Log.Info(Log.t2APP_IMPORTRCP, "Import danych RCP z plików CSV", "START", Log.PENDING);
            SqlConnection con = Base.Connect();
            DeleteTmpTable(con);
            int err = ImportFromFile(con, fileName);
            if (err == 0)
            {
                try
                {
                    if (CheckTextLinkedOk(con))
                    {
                        DataSet ds = Base.getDataSet(con,
                            "select C.Czas, C.RcpId, R.Id, C.ECCode, " +
                            "case when C.Typ = 'Służb.' then 1 when C.Typ = 'Pryw.' then 0 else -1 end as Duty " +
                            "from tmpImportRCP C " +
                            "left outer join Readers R on " +
                            "convert(varbinary(400), R.Name) = " +
                            "convert(varbinary(400), REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(C.Nazwa2 collate Polish_CS_AS,'Ś','O'),'ł','3'),'ś','o'),'ę','e'),'ć','a'),'ż','?'),'ą','1')) " +
                            "where RcpId < 100000000");

                        int ecuid = GetFirstECUniqueId(con, Base.getRows(ds).Count);
                        if (ecuid != -1)
                        {
                            int startid = ecuid;
                            string door = "-1";
                            string inout = Base.NULL;
                            foreach (DataRow dr in Base.getRows(ds))
                            {
                                string czas = Base.getValue(dr, 0);
                                string rcpId = Base.getValue(dr, 1);
                                string rogerId = Base.getValue(dr, 2);
                                string uid = Base.getScalar(con, String.Format(
                                    "select ECUniqueId from RCP where Czas='{0}' and ECUserId={1} and ECReaderId={2}",
                                    czas, rcpId, rogerId));
                                if (String.IsNullOrEmpty(uid))  //dodaję bo nie ma
                                {
                                    string code = Base.getValue(dr, 3);
                                    string duty = Base.getValue(dr, 4);
                                    Base.execSQL(con, Base.insertSql("RCP", 0,
                                        "ECUniqueId,Czas,ECCode,ECUserId,ECReaderId,ECDoorType,InOut,Duty",
                                        ecuid, Base.strParam(czas), code, rcpId, rogerId, door, inout, duty
                                        ));
                                    ecuid++;
                                }
                            }
                            Log.Update(lid, Log.OK);
                            Log.Info(Log.t2APP_IMPORTRCP, "Import danych RCP z plików CSV", String.Format("END ECUniqueId: {0} - {1}", startid, ecuid - 1), Log.OK);
                        }
                        else
                        {
                            Log.Update(lid, Log.ERROR);
                            Log.Error(Log.t2APP_IMPORTRCP, "Import danych RCP z plików CSV", "Brak miejsca w zakresie numeracji odczytów RCP", Log.OK);
                            Tools.ShowMessage("Brak miejsca w zakresie numeracji odczytów RCP. Import przerwany.");
                        }
                    }
                    else
                    {
                        Log.Update(lid, Log.ERROR);
                        Log.Error(Log.t2APP_IMPORTRCP, "Import danych RCP z plików CSV", "Istnieją odczyty, którym nie można przypisać rejestratorów po nazwach", Log.OK);
                        Tools.ShowMessage("Istnieją odczyty, którym nie można przypisać rejestratorów po nazwach - sprawdź w 'importRogerCSV.sql'. Import przerwany.");
                    }
                }
                catch (Exception ex)
                {
                    Log.Update(lid, Log.ERROR);
                    Log.Error(Log.t2APP_IMPORTSTRUCT, lid, "Import danych RCP z plików CSV", ex.Message);
                    if (con != null)
                        Base.Disconnect(con);
                    throw;
                }
            }
            if (con != null)
                Base.Disconnect(con);
            if (ImportFinished != null)
                ImportFinished(this, EventArgs.Empty);
            return err;
        }
    
    }
}