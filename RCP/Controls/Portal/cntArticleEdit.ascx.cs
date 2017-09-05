using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;
using HRRcp.Controls.Portal;
using System.Globalization;
using System.IO;
using AjaxControlToolkit;
using AjaxControlToolkit.HTMLEditor;

namespace HRRcp.Controls
{
    public partial class cntArticleEdit : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.MakeConfirmDeleteRecordButton(DeleteButton);
                if (Request.UrlReferrer != null)
                    Referer = Request.UrlReferrer.ToString();
                else
                    Referer = null;

                WydrukCheckBox.Visible = Lic.portalPrint;
            }
        }

        public void Create(string grupa)
        {
            Grupa = grupa;
            ArticleId = null;
            ImagePrefix = null;
            ImagePrefix = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
            FillData(grupa, null);
            lbEdit.Visible = false;
            lbInsert.Visible = true;
        }

        public void Edit(string grupa, string aId)
        {
            Grupa = grupa;
            ArticleId = aId;
            ImagePrefix = null;
            FillData(grupa, aId);
            lbEdit.Visible = true;
            lbInsert.Visible = false;
        }

        private void Back()
        {
            string url = Referer;
            if (String.IsNullOrEmpty(url))
                Response.Redirect(App.PortalPracForm);
            else
                Response.Redirect(cntArticles4.AddEditModeUrl(url));
        }

        private bool FillData(string grupa, string aId)
        {
            if (String.IsNullOrEmpty(aId))
            {
                DateEdit1.Date = DateTime.Today;
                AktywnyCheckBox.Checked = true;
                WydrukCheckBox.Checked = false;
                return true;
            }
            else
            {
                DataRow dr = db.getDataRow(String.Format("select * from {0}..Teksty where Id = {1}", App.dbPORTAL, aId));
                if (dr != null)
                {
                    string s = db.getValue(dr, "Tekst");
                    //s = s.Replace("&lt;", "<").Replace("&gt;", ">");
                    s = System.Web.HttpUtility.HtmlDecode(s);
                    Tekst1Editor.Content = s;

                    DateEdit1.Date = db.getDateTime(dr, "DataPublikacji");
                    AktywnyCheckBox.Checked = db.getBool(dr, "Widoczny", false);
                    WydrukCheckBox.Checked = db.getBool(dr, "Wydruk", false);
                    return true;
                }
            }
            return false;
        }

        private int Update()
        {
            string aId = ArticleId;
            if (String.IsNullOrEmpty(aId))
            {
                //string typ = String.Format("ART{0}", db.getScalar(String.Format("select count(*) + 1 from {0}..Teksty", App.dbPORTAL)));              // <<<< transakcja i zalokowanie, albo dodatkowa tabele z numeratorem, na razie ok ze względu na niewielką ilość zmian
                string typ = String.Format("ART{0}", db.getScalar(String.Format("select max(Id) + 1 from {0}..Teksty", App.dbPORTAL)));              // <<<< transakcja i zalokowanie, albo dodatkowa tabele z numeratorem, na razie ok ze względu na niewielką ilość zmian
                int id = db.insert(String.Format("{0}..Teksty", App.dbPORTAL), 
                    true, true, 0, "Typ,Opis,Grupa,Tekst,Tekst2,Widoczny,DataPublikacji,IdAutora,Wydruk",  
                    db.strParam(typ), 
                    db.strParam(""),     // not null
                    db.strParam(Grupa),
                    db.strParam(""),     // i tak później aktualizacja   
                    db.strParam(""),     // i tak później aktualizacja   
                    AktywnyCheckBox.Checked ? 1 : 0,
                    db.nullStrParam(DateEdit1.DateStr),
                    App.User.OriginalId,
                    WydrukCheckBox.Checked ? 1 : 0
                    );
                if (id > 0)
                {
                    string ipfx = id.ToString("X8");
                    string html = Tekst1Editor.Content.Replace(ImagePrefix, ipfx);
                    string text = HttpUtility.HtmlDecode(Tools.RemoveHtmlTags(html));
                    bool ok = db.update(String.Format("{0}..Teksty", App.dbPORTAL),
                        0, "Tekst,Tekst2", "Id=" + id.ToString(),
                        db.strParam(db.sqlPut(html)),
                        db.strParam(db.sqlPut(text))
                        );
                    if (ok) RenameFiles(ImagePrefix, ipfx);
                }
                return id;
            }
            else
            {
                string html = Tekst1Editor.Content;
                string text = HttpUtility.HtmlDecode(Tools.RemoveHtmlTags(html));
                bool ok = db.update(String.Format("{0}..Teksty", App.dbPORTAL),
                    0, "Tekst,Tekst2,Widoczny,DataPublikacji,IdAutora,Wydruk", "Id=" + aId, 
                    db.strParam(db.sqlPut(html)),
                    db.strParam(db.sqlPut(text)),
                    AktywnyCheckBox.Checked ? 1 : 0,
                    db.nullStrParam(DateEdit1.DateStr),
                    App.User.OriginalId,
                    WydrukCheckBox.Checked ? 1 : 0
                    );
                return ok ? Tools.StrToInt(aId, -1) : -1;
            }
        }

        //------------------------
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            bool ok = false;
            string aId = ArticleId;
            
            if (!String.IsNullOrEmpty(aId))
                ok = db.execSQL(String.Format("delete from {0}..Teksty where Id = {1}", App.dbPORTAL, aId));

            if (ok)
                Back();
            else
                Tools.ShowMessage("Wystąpił błąd podczas usuwania artykułu");
        }

        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            int id = Update();
            if (id >= 0)
                Back();
            else
                Tools.ShowMessage("Wystąpił błąd podczas zapisywania artykułu");
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Back();
        }

        //------------------------------------------------
        const string ArtykulyPath = @"~/portal/artykuly/";
        //const string ArtykulyUrl = @"../../portal/artykuly/";
        const string ArtykulyUrl = @"/portal/artykuly/";

        protected void FileUploadComplete(object sender, EventArgs e)
        {
            UploadFile(sender, e, "img", "tbImg");
        }

        private string GetFileName(string fname)
        {
            string ipfx;
            string aId = ArticleId;
            if (String.IsNullOrEmpty(aId))    //create
            {
                ipfx = ImagePrefix;
                if (String.IsNullOrEmpty(ipfx))
                {
                    ipfx = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);   // to zdaje sie nie ustawia bo na w IFrame?
                    ImagePrefix = ipfx;
                }
            }
            else
                ipfx = Tools.StrToInt(aId, 0).ToString("X8");

            return String.Format("{0}_{1}", ipfx, fname);
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

        private void UploadFile(object sender, EventArgs e, string v, string tb)
        {
            AsyncFileUploadEventArgs args = e as AsyncFileUploadEventArgs;
            AsyncFileUpload fu = sender as AsyncFileUpload;
            Editor ed = fu.Parent.FindControl("Tekst1Editor") as Editor;

            if (args != null && fu != null && ed != null)
            {
                string fileName = Path.GetFileName(fu.PostedFile.FileName);
                string savePath = Server.MapPath(ArtykulyPath);    //@ oznacza nie interpretuj - pozwala na umieszczenie \ na końcu
                string fn = GetFileName(fileName);   // dokładam id_ lub prefix z datą jak insert
                fu.SaveAs(savePath + fn);



                string cmd = App.wwwPORTAL + ArtykulyUrl + fn;  //<<<, pozniej dac pobranie sciezki www
                
                
                
                //string cmd = Tools.GetAppPath(ArtykulyUrl + fn);
                //string ico = GetIco(fileName);
                string img = String.Format("<img alt=\"\" class=\"{1}\" src=\"{0}\"/>", cmd, "img_left img30");
                //string img = String.Format("<img alt='' class='{1}' src='{0}'/>", cmd, "img_left");
                Tools.ExecOnStart2("edinsert", String.Format("ajaxEditorInsertText('{0}','{1}');", ed.ClientID, Tools.ToScript(img)));
            }
        }

        protected void FileUploadError(object sender, AsyncFileUploadEventArgs e)
        {
            //Log.Info(Log.t2APP, "UPLOAD_ERROR", String.Format("File: {0} Msg: {1}", e.FileName, e.StatusMessage));
            Tools.ShowMessage("Wystąpił błąd podczas ładowania pliku.");
        }
        //----------------------------------
        private void RenameFiles(string oldprefix, string newprefix)
        {
            string savePath = Server.MapPath(ArtykulyPath);    //@ oznacza nie interpretuj - pozwala na umieszczenie \ na końcu

            DirectoryInfo diMyFolder = new DirectoryInfo(savePath);
            FileInfo[] rgFiles = diMyFolder.GetFiles(oldprefix + "*.*");
            int len = oldprefix.Length;

            foreach (FileInfo fi in rgFiles)
            {
                string fname = newprefix + fi.Name.Remove(0, len);
                File.Move(fi.FullName, savePath + fname);
            }
        }

        //----------------------------------

        public string Grupa
        {
            set { ViewState["grup"] = value; }
            get { return Tools.GetStr(ViewState["grup"]); }
        }

        public string ArticleId
        {
            set { ViewState["artid"] = value; }
            get { return Tools.GetStr(ViewState["artid"]); }
        }

        public string ImagePrefix
        {
            set { ViewState["imgpfx"] = value; }
            get { return Tools.GetStr(ViewState["imgpfx"]); }
        }

        public string Referer
        {
            set { ViewState["ref"] = value; }
            get { return Tools.GetStr(ViewState["ref"]); }
        }
    }
}