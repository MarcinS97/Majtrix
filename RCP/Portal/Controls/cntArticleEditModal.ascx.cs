using HRRcp.App_Code;
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
    public partial class cntArticleEditModal : System.Web.UI.UserControl
    {
        public const string ImagesPath = "~/Portal/Artykuly/";

        public event EventHandler Saved;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                //Tools.ExecuteJavascript("prepareArticleEditor('#" + htmlEditor.ClientID + "');");
                btnSaveConfirm.OnClientClick = "$('.hidEditorData').val(tinyMCE.get('" + htmlEditor.ClientID + "').getContent());return true;";
            }
        }

        public void ShowEdit(String group, String articleId)
        {
            cntModal.Show(false);
            ArticleId = articleId;
            Group = group;
            FillData(Group, ArticleId);
        }
        
        public void ShowInsert(String group)
        {
            cntModal.Show();
            ArticleId = null;
            Group = group;
            FillData(Group, null);
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
                    s = System.Web.HttpUtility.HtmlDecode(s);
                    htmlEditor.Text = s;
                    DateEdit1.Date = db.getDateTime(dr, "DataPublikacji");
                    AktywnyCheckBox.Checked = db.getBool(dr, "Widoczny", false);
                    WydrukCheckBox.Checked = db.getBool(dr, "Wydruk", false);
                    ddlPozycja.SelectedValue = db.getValue(dr, "Pozycja");
                    tbTitle.Text = db.getValue(dr, "Tytul");
                    return true;
                }
            }
            return false;
        }

        public void Close()
        {
            cntModal.Close();
        }
        public String PrepareHtmlToSave(String html)
        {
            String newHtml = html;

            int idx = newHtml.IndexOf("data:image"), startIdx = 0, tempIdx = 0;
            while (idx != -1)
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
            //String html = htmlEditor.Text;//
            String html = hidEditorData.Value;
            html = PrepareHtmlToSave(html);
            //Tools.ShowMessage(html);
            //Update(html);
            string aId = ArticleId;
            if (String.IsNullOrEmpty(aId))
            {
                //string typ = String.Format("ART{0}", db.getScalar(String.Format("select count(*) + 1 from {0}..Teksty", App.dbPORTAL)));              // <<<< transakcja i zalokowanie, albo dodatkowa tabele z numeratorem, na razie ok ze względu na niewielką ilość zmian
                string typ = String.Format("ART{0}", db.getScalar(String.Format("select max(Id) + 1 from {0}..Teksty", App.dbPORTAL)));              // <<<< transakcja i zalokowanie, albo dodatkowa tabele z numeratorem, na razie ok ze względu na niewielką ilość zmian
                int id = db.insert(String.Format("{0}..Teksty", App.dbPORTAL),
                    true, true, 0, "Typ,Opis,Grupa,Tekst,Tekst2,Widoczny,DataPublikacji,IdAutora,Wydruk,Pozycja,Tytul",
                    db.strParam(typ),
                    db.strParam(""),     // not null
                    db.strParam(Group),
                    db.strParam(""),     // i tak później aktualizacja   
                    db.strParam(""),     // i tak później aktualizacja   
                    AktywnyCheckBox.Checked ? 1 : 0,
                    db.nullStrParam(DateEdit1.DateStr),
                    App.User.OriginalId,
                    WydrukCheckBox.Checked ? 1 : 0,
                    db.nullParam(ddlPozycja.SelectedValue),
                    db.nullStrParam(tbTitle.Text)
                    );
                if (id > 0)
                {
                    string ipfx = id.ToString("X8");
                    //string html = Tekst1Editor.Content.Replace(ImagePrefix, ipfx);
                    string text = HttpUtility.HtmlDecode(Tools.RemoveHtmlTags(html));
                    bool ok = db.update(String.Format("{0}..Teksty", App.dbPORTAL),
                        0, "Tekst,Tekst2,Tytul", "Id=" + id.ToString(),
                        db.strParam(db.sqlPut(html)),
                        db.strParam(db.sqlPut(text)),
                        db.nullStrParam(tbTitle.Text)
                        );
                    //if (ok) RenameFiles(ImagePrefix, ipfx);
                }
                return id;
            }
            else
            {
                string text = HttpUtility.HtmlDecode(Tools.RemoveHtmlTags(html));
                bool ok = db.update(String.Format("{0}..Teksty", App.dbPORTAL),
                    0, "Tekst,Tekst2,Widoczny,DataPublikacji,IdAutora,Wydruk,Pozycja,Tytul", "Id=" + aId,
                    db.strParam(db.sqlPut(html)),
                    db.strParam(db.sqlPut(text)),
                    AktywnyCheckBox.Checked ? 1 : 0,
                    db.nullStrParam(DateEdit1.DateStr),
                    App.User.OriginalId,
                    WydrukCheckBox.Checked ? 1 : 0,
                    db.nullParam(ddlPozycja.SelectedValue),
                    db.nullStrParam(tbTitle.Text)
                    );
                return ok ? Tools.StrToInt(aId, -1) : -1;
            }
        }

        protected void btnSaveConfirm_Click(object sender, EventArgs e)
        {
            Tools.ShowConfirm("Czy na pewno chcesz zapisać artykuł?", btnSave);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // save
            Update();
            if (Saved != null)
                Saved(null, EventArgs.Empty);
            Close();
        }

        public String ArticleId
        {
            get { return Tools.GetStr(ViewState["vArticleId"]); }
            set { ViewState["vArticleId"] = value; }
        }

        public String Group
        {
            get { return Tools.GetStr(ViewState["vGroup"]); }
            set { ViewState["vGroup"] = value; }
        }
    }
}