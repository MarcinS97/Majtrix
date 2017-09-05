using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using AjaxControlToolkit;
using HRRcp.App_Code;

namespace HRRcp.Controls.Portal
{
    public partial class cntPlikiPliki : System.Web.UI.UserControl
    {
        const int moQuery = 0;
        const int moEdit = 1;

        int FModeSet = -1;
        int FMode = moQuery;

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterPostBackControls();
        }


        private void RegisterPostBackControls()
        {
            foreach (ListViewItem item in lvPliki.Items)
            {
                LinkButton lbt = item.FindControl("LinkButton1") as LinkButton;
                UpdatePanel up = Tools.FindUpdatePanel(this);
                if (lbt != null && up != null)
                {
                    //ScriptManager.GetCurrent(Page).RegisterPostBackControl(lbt);

                    PostBackTrigger trigger = new PostBackTrigger();
                    trigger.ControlID = lbt.UniqueID;
                    up.Triggers.Add(trigger);
                    ScriptManager.GetCurrent(Page).RegisterPostBackControl(lbt);            
                }
            }
        }


        protected void lvPliki_DataBound(object sender, EventArgs e)
        {
            RegisterPostBackControls();
        }

        //---------------
        public void ShowInsert(bool visible)
        {
            lvPliki.InsertItemPosition = visible ? InsertItemPosition.LastItem : InsertItemPosition.None;
        }

        public string GetPath(object path)
        {
            return path.ToString();
        }

        public string GetLink(object cmd)
        {
            return Tools.Substring(cmd.ToString(), 16, 9999);
        }

        public string GetFileName(object cmd)
        {
            if (db.isNull(cmd))
                return null;
            else
                return Tools.Substring(Path.GetFileName(cmd.ToString()), 16, 9999);
        }

        public bool IsEditable()
        {
            return Mode == moEdit;
        }

        public InsertItemPosition GetInsertItemPosition()
        {
            if (Mode == moEdit)
                return InsertItemPosition.LastItem;
            else
                return InsertItemPosition.None;
        }

        public bool IsWydruk
        {
            get { return Lic.portalPrint; }
        }
        //----------------------------------
        public bool IsPrintVisible(object value)
        {
            //return false;

            return
                Lic.portalPrint &&
                App.User.IsKiosk &&
                Tools.GetBool(value, false);
        }

        //-----------------------------------
        const string PlikiPath      = @"~/portal/pliki/";
        const string PlikiPathRel   = @"../../portal/pliki/";
        
        protected void FileUploadComplete(object sender, EventArgs e)
        {
            UploadFile(sender, e, "img", "tbImg");
        }

        private string GetFileName(string id, string fname)
        {
            string iid;
            if (String.IsNullOrEmpty(id))    //create
                iid = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
            else
                iid = Tools.StrToInt(id, 0).ToString("X8");

            return String.Format("{0}_{1}", iid, fname);
        }

        private string GetIco(string fname)
        {
            string ext = Path.GetExtension(fname).ToLower();  // potem zmienić na baze !!!
            switch (ext)
            {
                case ".pdf":
                    return "../../images/fileext/pdf.png";
                case ".doc":
                case ".docx":
                    return "../../images/fileext/doc.png";
                case ".xls":
                case ".xlsx":
                    return "../../images/fileext/xls.png";
                case ".png":
                case ".jpg":
                case ".jpeg":
                case ".gif":
                    return "../../images/fileext/img.png";
                default:
                    return "../../images/fileext/dok.png";
            }
        }

        private void UploadFile(object sender, EventArgs e, String v, string tb)
        {
            AsyncFileUploadEventArgs args = e as AsyncFileUploadEventArgs;
            AsyncFileUpload fu = sender as AsyncFileUpload;
            HiddenField hid = fu.Parent.FindControl("hidId") as HiddenField;
            Image img = fu.Parent.FindControl("Image1") as Image;
            HiddenField hidIco = fu.Parent.FindControl("hidImage") as HiddenField;
            //HiddenField hidCmd = fu.Parent.FindControl("hidCommand") as HiddenField;
            HiddenField hidPar1 = fu.Parent.FindControl("hidPar1") as HiddenField;
            TextBox tbNapis = fu.Parent.FindControl("FileNameTextBox") as TextBox;
            TextBox tbCommand = fu.Parent.FindControl("CommandTextBox") as TextBox;

            //if (args != null && fu != null && hid != null && img != null && tbCommand != null && hidPar1 != null && tbNapis != null && hidIco != null)
            if (Tools.IsAllNotNull(args, fu, hid, img, tbCommand, hidPar1, tbNapis, hidIco))
            {
                string fileName = Path.GetFileName(fu.PostedFile.FileName);
                string savePath = Server.MapPath(PlikiPath);    //@ oznacza nie interpretuj - pozwala na umieszczenie \ na końcu

                string fn = GetFileName(hid.Value, fileName);   // dokładam id_
                fu.SaveAs(savePath + fn);

                string cmd = PlikiPath + fn;  // link
                string ico = GetIco(fileName);

                //img.ImageUrl = ico;
                //hidIco.Value = ico;
                //hidCmd.Value = cmd;
                //hidPar1.Value = fu.ContentType;

                // __NEW
                string ext = Path.GetExtension(fileName).ToLower();
                //if (ext == ".pdf")  
                if (Lic.PDF2PNG && ext == ".pdf")   //20160514 nie uruchamiał w iqor
                {
                    Pdf2Img.AddPNGImages(App.PDFPNGPath, savePath + fn);
                }
                // __END

                string napis = null;
                if (String.IsNullOrEmpty(tbNapis.Text))
                    napis = String.Format("top.document.getElementById('{0}').value='{1}';", tbNapis.ClientID, fileName);

                Tools.ExecOnStart2("eico", String.Format(@"
top.document.getElementById('{0}').src='{1}';
top.document.getElementById('{2}').value='{3}';
top.document.getElementById('{4}').value='{5}';
top.document.getElementById('{6}').value='{7}';
{8}", 
                 img.ClientID, ico,
                 hidIco.ClientID, ico,
                 //hidCmd.ClientID, cmd,
                 tbCommand.ClientID, cmd,

                 hidPar1.ClientID, fu.ContentType,
                 napis));
                  
            }
        }

        protected void FileUploadError(object sender, AsyncFileUploadEventArgs e)
        {
            Tools.ShowMessage("Wystąpił błąd podczas ładowania pliku.");
        }        
        


        //----------------------------------
        /*
        public static void UploadFileToDB(bool hasFile, string fileName, int fileSize, byte[] documentBinary, int wniosekID, int type, int? idPrzelozeni)
        {
            if (wniosekID != -1)
            {
                if (hasFile)
                {
                    string fileExtension = Path.GetExtension(fileName);
                    string documentType = string.Empty;

                    switch (fileExtension.ToLower())
                    {
                        case ".pdf":
                            documentType = "application/pdf";
                            break;
                        case ".xls":
                        case ".xlsx":
                            documentType = "application/vnd.ms-excel";
                            break;
                        case ".doc":
                        case ".docx":
                            documentType = "application/vnd.ms-word";
                            break;
                        case ".gif":
                            documentType = "image/gif";
                            break;
                        case ".png":
                            documentType = "image/png";
                            break;
                        case ".jpg":
                            documentType = "image/jpg";
                            break;
                        default:
                            documentType = "application/octet-stream";
                            break;
                    }




                    //SqlParameter DocName = new SqlParameter("@Name", SqlDbType.VarChar, 50);
                    //DocName.Value = fileName.ToString();

                    //SqlParameter Type = new SqlParameter("@DataType", SqlDbType.VarChar, 50);
                    //Type.Value = documentType.ToString();

                    //string sql = "";

                    //if (idPrzelozeni != null)
                    //{
                    //    sql = String.Format("INSERT INTO [wnZalacznik](ID_Wniosek, DataCzas, Data, DataType, Name, Type, ID_Przelozeni) VALUES({0}, GETDATE(), @Data, @DataType, @Name, {1}, {2})", wniosekID, type, idPrzelozeni);
                    //}
                    //else
                    //{
                    //    sql = String.Format("INSERT INTO [wnZalacznik](ID_Wniosek, DataCzas, Data, DataType, Name, Type) VALUES({0}, GETDATE(), @Data, @DataType, @Name, {1})", wniosekID, type);
                    //}

                    //SqlParameter uploadedDocument = new SqlParameter("@Data", SqlDbType.Binary, fileSize);
                    //uploadedDocument.Value = documentBinary;

                    //db.execSQL(sql, uploadedDocument, Type, DocName);
                }
            }
        }
        
        protected void AsyncFileUpload1_UploadedComplete(object sender, EventArgs e)
        {
            if (((AsyncFileUpload)sender).HasFile)
            {
                //UploadFileToDB((AsyncFileUpload)sender, wniosekID, 1);
                
                //int wniosekID = Tools.StrToInt(hidWniosekId.Value, -99);
                //if (wniosekID != -99)
                //{

                    
                //    wnCommon.UploadFileToDB((AsyncFileUpload)sender, wniosekID, 1);
                //    //wnCommon.Dalej(wniosekID, AppUser.CreateOrGetSession());
                    
                    
                //    //xWnioskiList.DataBind();
                //    //xResponse.Redirect(App.WnioskiForm);
                //}
                //else Tools.ShowError("Błąd podczas dodawania pliku załącznika.");   // <<< TODO na później - się nie pokazuje bo control jest w IFrame - trzeba dodać clientUploadCompete i zrobić doClick(xx)
            }



            else Tools.ShowError("Proszę wybrać plik załącznika.");                 // j.w.
        }

        protected void AsyncFileUpload1_UploadedFileError(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            Tools.ShowMessage("Wystąpił błąd podczas ładowania pliku.");
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            //WnioskiList.DataBind();
            Tools.ShowMessage("Załącznik został dodany.");
        }
        */





        //----------------------------------

        public void DownloadFile(string file, string fileName, string dataType)
        {
            bool ok = false;
            HttpResponse response = HttpContext.Current.Response;
            try
            {
                response.ClearContent();
                response.AddHeader("Content-Disposition", String.Format("attachment; filename=\"{0}\"", fileName));  // numer hex

                //header('Content-type: application/pdf', true);  <<< duplicate content disposition jak , w nazwie pliku -> dodanie "" i sugestia zeby dodać content-type

                //response.WriteFile(Server.MapPath(fileName));
                response.WriteFile(file);
                response.ContentType = dataType;
                ok = true;
            }
            catch (Exception ex)  
            {
                //Tools.ShowErrorLog(Log._PORTAL, ex.Message, "Wystąpił błąd podczas pobierania pliku.");  nie moze byc bo response laduje do pliku
                AppError.Show("Pobieranie pliku", ex);
            }
            if (ok) response.End();   // to wywala exception ze thread aborted i to jest ok, wiec nie moge przechwycic
        }


        public static void HandleCommand(string id)   // wykorzystywana przy wyszukaj
        {
            SqlConnection con = db.Connect(db.PORTAL);
            DataRow dr = db.getDataRow(con, "select * from SqlMenu where Id = " + id);
            db.Disconnect(con);

            string cmd = db.getValue(dr, "Command");
            if (!String.IsNullOrEmpty(cmd) && cmd.Length > 2)
            {
                string url;
                //----- redirect -----
                if (Tools.IsUrl(cmd, out url))
                {
                    string grp = db.getValue(dr, "Par1");
                    if (!String.IsNullOrEmpty(grp))
                        HttpContext.Current.Session["lmenugrp"] = grp;   // symulacja wyboru menu
                    App.Redirect(url);
                }
                else
                //----- download -----
                {
                    string fname = db.getValue(dr, "MenuText").Replace('/', '_').Replace('\\', '_').Replace('?', ' ').Replace(':', '_').Replace('*', '_');//.Replace('','_');
                    string mime = db.getValue(dr, "Par1");

                    string e1 = Path.GetExtension(cmd).ToLower();
                    string e2 = Path.GetExtension(fname).ToLower();
                    if (e1 != e2) fname += e1;

                    //Tools.Substring(Path.GetFileName(fileName), 9, 9999)

                    Tools.DownloadFile(cmd, fname, mime);
                }
            }
        }

        protected void lvPliki_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CancelInsert":
                    ShowInsert(false);
                    break;
                
                
                
                case "Delete":
                    string id = Tools.GetDataKey(lvPliki, e);
                    SqlConnection con = db.Connect(db.PORTAL);
                    DataRow dr = db.getDataRow(con, "select * from SqlMenu where Id = " + id); //e.CommandArgument.ToString());
                    db.Disconnect(con);
                    
                    if (db.getValue(dr, "Par1") == "application/pdf")
                    {
                        string fPath = Server.MapPath(db.getValue(dr, "Command"));    // tu jest plik pdf
                        //DeletePNGImages(fPath);   // jakby miał usuwać wystarczy odremować i moze dodać jeszcze usuniecie pliku pdf, UWAGA !!! trzeba sprawdzac czy lik nie jest jeszcze gdzieś wykorzysany
                    }
                    break;
                
                
                
                case "View":

                    break;
                case "Download":
                    con = db.Connect(db.PORTAL);
                    dr = db.getDataRow(con, "select * from SqlMenu where Id = " + e.CommandArgument.ToString());
                    db.Disconnect(con);

                    string cmd = db.getValue(dr, "Command");
                    if (!String.IsNullOrEmpty(cmd) && cmd.Length > 2)
                    {
                        string url;
                        if (Tools.IsUrl(cmd, out url))
                            App.Redirect(url);
                        else
                        {
                            string fname = db.getValue(dr, "MenuText").Replace('/', '_').Replace('\\', '_').Replace('?', ' ').Replace(':', '_').Replace('*', '_');//.Replace('','_');
                            string mime = db.getValue(dr, "Par1");

                            string e1 = Path.GetExtension(cmd).ToLower();
                            string e2 = Path.GetExtension(fname).ToLower();
                            if (e1 != e2) fname += e1;

                            //Tools.Substring(Path.GetFileName(fileName), 9, 9999)


                            //DownloadFile(cmd, fname, mime);
                            //if (Lic.portalPrint && App.User.IsKiosk)
                            if (Lic.portalPrint)
                            {
                                if (mime == "application/pdf" || mime.Substring(0, 5) == "image")
                                {
                                    Session["DocID"] = e.CommandArgument.ToString();
                                    Response.Redirect("PDFViewer.aspx");
                                }
                                else DownloadFile(cmd, fname, mime);
                            }
                            else DownloadFile(cmd, fname, mime);
                        }
                    }
                    break;
            }
        }

        /*

                case "SelectFile":
                    FileUpload fu = lvPliki.EditItem.FindControl("FileUpload1") as FileUpload;
                    if (fu != null)
                    {
                        string ff = fu.PostedFile.FileName;
                    }
                    break;
                case "SelectFile2":


                    fu = lvPliki.EditItem.FindControl("FileUpload1") as FileUpload;
                    if (fu != null)
                    {
                        if (fu.HasFile)
                        {
                            string fileName = fu.FileName;
                            //string savePath = Server.MapPath(@"uploads\") + fileName;  //@ oznacza nie interpretuj - pozwala na umieszczenie \ na końcu
                            //FileUpload1.SaveAs(savePath);
                        }
                        else
                        {
                            Tools.ShowMessage("Brak pliku do importu.");  // załatwia to javascript, ale na wszelki wypadek, script dlatego ze postbacktrigger musi być na buttonie i msg pojawiał sie po przeładowaniu strony przed jej wyswietleniem
                        }
                    }

                    //string fff = FileUpload1.PostedFile.FileName;

                    break;
         */


        protected void lvPliki_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");





                /* 20150312 wyłączamy bo jest pdf2png
                if (((ListViewDataItem)e.Item).DisplayIndex == lvPliki.EditIndex)
                {
                    if (Lic.portalPrint)
                    {
                        //Tools.SetControlVisible(e.Item, "paUploadImg", true); jest w aspx ustawiane
                        UpadatePgLiteral(e.Item);
                    }
                }
                 */ 
                 
                 
                 
                /*
                UpdatePanel up = Tools.FindUpdatePanel(this);
                LinkButton lb = Tools.FindLinkButton(e.Item, "LinkButton1");
                if (up != null && lb != null)
                {
                    PostBackTrigger trigger = new PostBackTrigger();
                    trigger.ControlID = lb.UniqueID;
                    up.Triggers.Add(trigger);
                    ScriptManager.GetCurrent(Page).RegisterPostBackControl(lb);
                }
                 */ 
            }
        }
        //---
        private bool Update(IOrderedDictionary values, bool create)
        {
            /*
            //string cmd = values["Command"].ToString();
            //values["Command"] = cmd;

            string ext = Path.GetExtension(cmd);
            string img = db.getScalar("select * from Kody where Typ = 'FILEEXT' and Kod = " + db.strParam(ext));
            if (String.IsNullOrEmpty(img))
            {
                img = db.getScalar("select * from Kody where Typ = 'FILEEXT' and Kod = 'DEFAULT'");
            }
            values["Image"] = img;
            */
            return true;
        }

        string plikId = null;

        private void UpdateFileName(IOrderedDictionary values)
        {
            string fn = Tools.GetStr(values["Command"]);
    
            string url;
            if (!Tools.IsUrl(fn, out url) && fn.Length > 2)
            {
                string fn2 = PlikiPath + GetFileName(plikId, Tools.Substring(Path.GetFileName(fn), 16, 9999));  // odcinam yyyyMMdd_HHmmss_
                File.Move(Server.MapPath(fn), Server.MapPath(fn2));
                SqlConnection con = db.Connect(db.PORTAL);
                db.execSQL(con, String.Format("update SqlMenu set Command = {0} where Id = {1}", db.nullStrParam(fn2), plikId));
                db.Disconnect(con);
            }
        }

        protected void lvPliki_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !Update(e.Values, true);
        }

        protected void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            plikId = e.Command.Parameters["@Id"].Value.ToString();
        }

        protected void lvPliki_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            if (e.AffectedRows > 0)
            {
                UpdateFileName(e.Values);
                ShowInsert(false);
            }
        }

        protected void lvPliki_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !Update(e.NewValues, false);
        }

        protected void lvPliki_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {

        }
        //----------------------------------
        public int ParentId
        {
            set { hidParentId.Value = value.ToString(); }
            get { return Tools.StrToInt(hidParentId.Value, -1); }
        }

        public string Grupa
        {
            set { hidGrupa.Value = value; }
            get { return hidGrupa.Value; }
        }

        //public int Mode
        //{
        //    set { FMode = value; }
        //    get { return FMode; }
        //}

        public int Mode
        {
            set
            {
                FMode = value;
                ViewState["amode"] = value;
            }
            get
            {
                if (FModeSet == -1)
                {
                    FMode = Tools.GetInt(ViewState["amode"], moQuery);
                    FModeSet = 1;
                }
                return FMode;
            }
        }


        //----------------------------------------------------------------



        /* 20150312 wyłaczamy bo jest pdf2png
        string[] GetAllPages(string name)
        {
            return Directory.GetFiles(Server.MapPath(PlikiPath), Path.GetFileNameWithoutExtension(name) + "*.png");
        }

        int GetLastPageNumber(string name)
        {
            return (from file in GetAllPages(name) select Tools.GetPageNum(file)).DefaultIfEmpty(0).Max();
        }

        string GetNextFileName(string name)
        {
            return string.Format("{2}{0}_{1}.png", name, GetLastPageNumber(name) + 1, Server.MapPath(PlikiPath));
        }

        protected void DeleteAllPages(object sender, EventArgs e)
        {
            string fileNameBase = ((sender as Control).Parent.FindControl("CommandTextBox") as TextBox).Text;
            string name = Server.MapPath(PlikiPath) + fileNameBase;
            foreach (var Item in GetAllPages(name))
            {
                File.Delete(Item);
            }
            UpadatePgLiteral((sender as Control).Parent);
        }

        void UpadatePgLiteral(Control c)
        {
            Label lt = c.FindControl("LLPagesCount") as Label;
            if (lt != null)
            {
                string FileName = (c.FindControl("CommandTextBox") as TextBox).Text;
                string name = Path.GetFileNameWithoutExtension(FileName);
                int lastPg = GetLastPageNumber(name);

                Action<string> setLTText = t =>
                    Tools.ExecOnStart2("uPgL", string.Format(@"top.document.getElementById('{0}').innerHTML = '{1}';", lt.ClientID, t));

                if (lastPg > 0)
                {
                    setLTText(string.Format("Dokument zawiera stron: {0}", lastPg));
                }
                else
                {
                    setLTText("Brak stron. Wyslij pierwsza strone.");
                }
            }
        }

        void PgLiteralError(Control c, string text)
        {
            Label lt = c.FindControl("LLPagesCount") as Label;
            if (lt != null)
            {
                lt.Text = text;
            }
        }


        protected void UploadFileNewPage(object sender, EventArgs e)
        {
            AsyncFileUpload fu = (sender as AsyncFileUpload);
            string ext = Path.GetExtension(fu.PostedFile.FileName);
            string fn = ((sender as Control).Parent.FindControl("CommandTextBox") as TextBox).Text;
            string SavePath = GetNextFileName(Path.GetFileNameWithoutExtension(fn));
            if (ext != ".png")
            {
                SavePath = Path.ChangeExtension(SavePath, ext);
            }
            fu.SaveAs(SavePath);

            if (ext == ".png" || Tools.ChangeImgFormatToPNG(SavePath) != null)
            {
                UpadatePgLiteral((sender as Control).Parent);
            }
            else
            {
                PgLiteralError((sender as Control).Parent, "Nieobsługiwany format pliku");
            }
        }
         */
 




    }
}
