using HRRcp.App_Code;
using HRRcp.Controls.Portal;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls
{
    public partial class cntArticleEdit : System.Web.UI.UserControl
    {
        public const string ImagesPath = "~/Portal/Artykuly/";  
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Request.UrlReferrer != null)
                    Referer = Request.UrlReferrer.ToString();
                else
                    Referer = null;

            }
        }

        public void Prepare(String Grupa)
        {
            this.Grupa = Grupa;
            FillData(Grupa, null);
            lblTitle.Text = "Nowy artykuł";
        }

        public void Prepare(String Grupa, String ArticleId/*, EMode Mode*/)
        {
            this.ArticleId = ArticleId;
            this.Grupa = Grupa;
            FillData(Grupa, ArticleId);
            lblTitle.Text = "Edycja artykułu";
            //this.Mode = Mode;
        }

        public String ArticleId
        {
            get { return hidArticleId.Value; }
            set { hidArticleId.Value = value; }
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
                    //Tekst1Editor.Content = s;

                    //htmlEditor.InnerHtml = s;
                    htmlEditor.Text = s;
                    DateEdit1.Date = db.getDateTime(dr, "DataPublikacji");
                    AktywnyCheckBox.Checked = db.getBool(dr, "Widoczny", false);
                    WydrukCheckBox.Checked = db.getBool(dr, "Wydruk", false);
                    ddlPozycja.SelectedValue = db.getValue(dr, "Pozycja");
                    return true;
                }
            }
            return false;
        }

        //public enum EMode { Insert, Edit };
        //private EMode _mode = EMode.Insert;
        //public EMode Mode
        //{
        //    get 
        //    {
        //        if (ViewState["vMode"] == null)
        //            ViewState["vMode"] = EMode.Insert;
        //        return (EMode)ViewState["vMode"]; 
        //    }
        //    set { ViewState["vMode"] = value; }
        //}

        public String PrepareHtmlToSave(String html)
        {
            String newHtml = html;

            int idx = newHtml.IndexOf("data:image"), startIdx = 0, tempIdx = 0;
            while(idx != -1)
            {
                // znaleźliśmy img
                startIdx = idx;
                tempIdx = idx;
                while (newHtml[tempIdx] != '\"')
                    tempIdx++;

                string data = newHtml.Substring(startIdx, tempIdx - startIdx);
                string base64img = data.Substring(data.IndexOf(',') + 1);
                string imageUrl = SaveImage(base64img);
                newHtml = newHtml.Replace(data, imageUrl);
                idx = newHtml.IndexOf("data:image", idx + 1);
            }

            return newHtml;
        }

        public String SaveImage(string base64img)
        {
            Page page = HttpContext.Current.Handler as Page;
            string name = string.Format(@"{0}.jpg", DateTime.Now.Ticks);
            string path = ImagesPath + name;
            String resolvedPath = page.Server.MapPath(ImagesPath + name);
            File.WriteAllBytes(resolvedPath, Convert.FromBase64String(base64img.Replace(@"\n", "")));
            return page.ResolveUrl(path);
        }

        private int Update()
        {
            String html = htmlEditor.Text;//hidEditorData.Value;
            html = PrepareHtmlToSave(html);
            //Tools.ShowMessage(html);
            //Update(html);
            string aId = ArticleId;
            if (String.IsNullOrEmpty(aId))
            {
                //string typ = String.Format("ART{0}", db.getScalar(String.Format("select count(*) + 1 from {0}..Teksty", App.dbPORTAL)));              // <<<< transakcja i zalokowanie, albo dodatkowa tabele z numeratorem, na razie ok ze względu na niewielką ilość zmian
                string typ = String.Format("ART{0}", db.getScalar(String.Format("select max(Id) + 1 from {0}..Teksty", App.dbPORTAL)));              // <<<< transakcja i zalokowanie, albo dodatkowa tabele z numeratorem, na razie ok ze względu na niewielką ilość zmian
                int id = db.insert(String.Format("{0}..Teksty", App.dbPORTAL),
                    true, true, 0, "Typ,Opis,Grupa,Tekst,Tekst2,Widoczny,DataPublikacji,IdAutora,Wydruk,Pozycja",
                    db.strParam(typ),
                    db.strParam(""),     // not null
                    db.strParam(Grupa),
                    db.strParam(""),     // i tak później aktualizacja   
                    db.strParam(""),     // i tak później aktualizacja   
                    AktywnyCheckBox.Checked ? 1 : 0,
                    db.nullStrParam(DateEdit1.DateStr),
                    App.User.OriginalId,
                    WydrukCheckBox.Checked ? 1 : 0,
                    db.nullParam(ddlPozycja.SelectedValue)
                    );
                if (id > 0)
                {
                    string ipfx = id.ToString("X8");
                    //string html = Tekst1Editor.Content.Replace(ImagePrefix, ipfx);
                    string text = HttpUtility.HtmlDecode(Tools.RemoveHtmlTags(html));
                    bool ok = db.update(String.Format("{0}..Teksty", App.dbPORTAL),
                        0, "Tekst,Tekst2", "Id=" + id.ToString(),
                        db.strParam(db.sqlPut(html)),
                        db.strParam(db.sqlPut(text))
                        );
                    //if (ok) RenameFiles(ImagePrefix, ipfx);
                }
                return id;
            }
            else
            {
                string text = HttpUtility.HtmlDecode(Tools.RemoveHtmlTags(html));
                bool ok = db.update(String.Format("{0}..Teksty", App.dbPORTAL),
                    0, "Tekst,Tekst2,Widoczny,DataPublikacji,IdAutora,Wydruk,Pozycja", "Id=" + aId,
                    db.strParam(db.sqlPut(html)),
                    db.strParam(db.sqlPut(text)),
                    AktywnyCheckBox.Checked ? 1 : 0,
                    db.nullStrParam(DateEdit1.DateStr),
                    App.User.OriginalId,
                    WydrukCheckBox.Checked ? 1 : 0,
                    db.nullParam(ddlPozycja.SelectedValue)
                    );
                return ok ? Tools.StrToInt(aId, -1) : -1;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = Update();
            if (id >= 0)
                Back();
            else
                Tools.ShowMessage("Wystąpił błąd podczas zapisywania artykułu");

        }

        private void Back()
        {
            string url = Referer;
            if (String.IsNullOrEmpty(url))
                Response.Redirect(App.PortalPracForm);
            else
                Response.Redirect(cntArticles4.AddEditModeUrl(url));
        }

        public string Referer
        {
            set { ViewState["ref"] = value; }
            get { return Tools.GetStr(ViewState["ref"]); }
        }

        public string Grupa
        {
            set { ViewState["grup"] = value; }
            get { return Tools.GetStr(ViewState["grup"]); }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Back();
        }

        public void Delete()
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
        protected void btnDeleteConfirm_Click(object sender, EventArgs e)
        {
            Tools.ShowConfirm("Potwierdzasz usunięcie rekordu danych ?", btnDelete);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }
    }
}