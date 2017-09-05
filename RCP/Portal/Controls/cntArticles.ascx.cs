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
using AjaxControlToolkit.HTMLEditor;
using HRRcp.Portal;
using HRRcp.Controls;


//save uploaded images to db
//http://code.msdn.microsoft.com/Upload-Files-Asynchronously-829691df



namespace HRRcp.Portal.Controls
{
    public partial class cntArticles : System.Web.UI.UserControl
    {
        const bool newEdit = true;   // edycja od razu dostepna, bez przełączania do Edit mode

        private string FGrupa = null;

        const int moQuery = 0;
        const int moEdit = 1;

        int FModeSet = -1;
        int FMode = moQuery;

        int FPageSize = 3;

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvArtykuly, 0);
        }

        public static string AddEditModeUrl(string url)
        {
            const string par = "?e=1";
            if (url.EndsWith(par))
                return url;
            else
                return url + par;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool ed = CanEdit;
                //paEditButton.Visible = ed;
                upModal.Visible = ed;  //T:optymalizacja
                                
                if (newEdit)
                {
                    btEdit.Visible = false;   // po nowemu - zawsze jest widoczny klawisz dodaj i edytuj/usuń przy artykułach
                    InsertButton.Visible = CanEdit;
                }
                else
                {
                    btEdit.Visible = ed;
                    if (ed && Request.QueryString["e"] == "1")
                        ShowEdit(true);
                }

                DataPager dp = lvArtykuly.FindControl("DataPager1") as DataPager;
                if (dp != null)
                {
                    if (FPageSize < 1)
                        dp.Visible = false;
                    else
                        dp.PageSize = FPageSize;
                }

                /* juan */
                string art_title = Request.QueryString["title"];
                if (!String.IsNullOrEmpty(art_title))
                {
                    this.Title = art_title;
                }
            }
            Tools.ExecuteJavascript(@"
            $(function () {{
                prepareTinyMce('.editor');
            }});");
        }

        protected void x_htmlEditorExtender2_OnImageUploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {

            if (e.ContentType.Contains("jpg") ||
                e.ContentType.Contains("gif") ||
                e.ContentType.Contains("png") ||
                e.ContentType.Contains("jpeg"))
            {
                string ext = Path.GetExtension(e.FileName);
                string fn = e.FileId + ext;

                string fullpath = Server.MapPath(ArtykulyPath) + fn;
                ((AjaxFileUpload)sender).SaveAs(fullpath);
                e.PostedUrl = Page.ResolveUrl(ArtykulyPath + fn);
            }
        }

        //-------------------------------
        public void ShowInsert(bool visible)
        {
            lvArtykuly.InsertItemPosition = visible ? InsertItemPosition.FirstItem : InsertItemPosition.None;
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

        public bool IsEditable
        {
            get { return Mode == moEdit && CanEdit; }
        }

        public bool CanEdit
        {
            get { return App.User.IsPortalAdmin || App.User.HasRight(AppUser.rPortalArticles); }
        }

        public bool IsPrintVisible(object print)
        {
            //return false;            
            return Lic.portalPrint &&
                App.User.IsKiosk &&
                Tools.GetBool(print, false);
        }

        public bool IsWydruk
        {
            get { return Lic.portalPrint; }
        }

        public InsertItemPosition GetInsertItemPosition()
        {
            if (IsEditable)
                return InsertItemPosition.FirstItem;
            else
                return InsertItemPosition.None;
        }
        //----------------------------------





        //------------------------------------------------
        const string ArtykulyPath = @"~/portal/artykuly/";

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




        private void InsertText(Editor ed, string txt)
        {
            Tools.ExecOnStart2("insertText", String.Format("ajaxEditorInsertText('{0}','{1}');", ed.ClientID, Tools.ToScript(txt)));
            //Tools.ExecOnStart2("insertText", "alert(111);");
        }

        private void InsertImg(Control item, string img)
        {
            Button btInsImg = item.FindControl("btInsImg") as Button;
            HiddenField btInsImgPar = item.FindControl("btInsImg_par") as HiddenField;
            if (btInsImg != null && btInsImgPar != null)
            {
                btInsImgPar.Value = img;
                //Tools.ExecOnStart2("insimg", String.Format("doClick('{0}')", btInsImg.ClientID));



                Tools.ExecOnStart2("insimg", String.Format("javascript:window.setTimeout(function(){{doClick('{0}');}}, 500);", btInsImg.ClientID));
                //Tools.ExecOnStart2("insimg", String.Format("javascript:window.setTimeout(alert(222));", btInsImg.ClientID));
            }
        }

        private void UploadFile(object sender, EventArgs e, String v, string tb)
        {
            //Log.Info(Log.t2APP, "UPLOAD_IN", null);

            AsyncFileUploadEventArgs args = e as AsyncFileUploadEventArgs;
            //Log.Info(Log.t2APP, "UPLOAD_IN", String.Format("File: {0} Msg: {1}", args.FileName, args.StatusMessage));

            AsyncFileUpload fu = sender as AsyncFileUpload;
            HiddenField hid = fu.Parent.FindControl("hidId") as HiddenField;
            Editor ed = fu.Parent.FindControl("Tekst1Editor") as Editor;


            if (args != null && fu != null && hid != null)  // && ed != null)// && img != null && hidCmd != null && hidPar1 != null && tbNapis != null && hidIco != null)
            {
                string fileName = Path.GetFileName(fu.PostedFile.FileName);
                string savePath = Server.MapPath(ArtykulyPath);    //@ oznacza nie interpretuj - pozwala na umieszczenie \ na końcu

                string fn = GetFileName(hid.Value, fileName);   // dokładam id_
                fu.SaveAs(savePath + fn);

                string cmd = ArtykulyPath + fn;  // link
                string img = String.Format("<img alt='' class='{1}' src='{0}'/>", cmd, "img_left");

                Tools.ExecOnStart2("edinsert", String.Format("ajaxEditorInsertText('{0}','{1}');", ed.ClientID, "aaaaaaaaa "));
            }
        }

        protected void FileUploadError(object sender, AsyncFileUploadEventArgs e)
        {
            Log.Info(Log.t2APP, "UPLOAD_ERROR", String.Format("File: {0} Msg: {1}", e.FileName, e.StatusMessage));
            Tools.ShowMessage("Wystąpił błąd podczas ładowania pliku.");
        }


        //----------------------------------
        protected void dsArticles_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
        }

        protected void lvArtykuly_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListViewItemType.DataItem:
                    DataRowView drv = Tools.GetDataRowView(e);
                    string s = drv["Tekst"].ToString();
                    //s = s.Replace("&lt;", "<").Replace("&gt;", ">");
                    s = System.Web.HttpUtility.HtmlDecode(s);

                    if (((ListViewDataItem)e.Item).DisplayIndex == lvArtykuly.EditIndex)
                    {
                        Editor ed = e.Item.FindControl("Tekst1Editor") as Editor;
                        if (ed != null)
                            ed.Content = s;
                        //Tools.SetText2(e.Item, "tbArtykul", s);
                    }
                    else
                        Tools.SetText2(e.Item, "Literal1", s);
                    break;
                case ListViewItemType.EmptyItem:
                    Tools.SetControlVisible(e.Item, "TopInsertButton", IsEditable);
                    break;
            }
        }

        private bool Update(Control item, IOrderedDictionary values, bool insert)
        {
            if (!insert)
            {
                Editor ed = item.FindControl("Tekst1Editor") as Editor;
                if (ed != null)
                {
                    string s = ed.Content;
                    values["Tekst"] = s;
                }
            }
            return true;
        }

        protected void lvArtykuly_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !Update(lvArtykuly.EditItem, e.NewValues, false);
        }

        protected void lvArtykuly_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !Update(lvArtykuly.InsertItem, e.Values, true);
        }

        protected void lvArtykuly_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.InsertItem)
            {
                DateEdit de = e.Item.FindControl("DateEdit1") as DateEdit;
                if (de != null) de.Date = DateTime.Today;
                Tools.SetChecked(e.Item, "AktywnyCheckBox", true);
            }
        }
        //----------------------------------
        private void ShowEdit(bool show)
        {
            //paEditButton.Visible = !show;
            btEdit.Visible = !show;
            //btEdit.Visible = !show;
            if (show)
                Mode = moEdit;
            else
                Mode = moQuery;
            lvArtykuly.DataBind();
            paTopButtons.Visible = IsEditable;
            InsertButton.Visible = IsEditable;
            //Tools.SetControlVisible(lvArtykuly, "paTopButtons", IsEditable);
        }

        public ListView List
        {
            get { return lvArtykuly; }
        }

        protected void btEdit_Click(object sender, EventArgs e)
        {
            ShowEdit(true);
        }

        protected void btCancelEdit_Click(object sender, EventArgs e)
        {
            ShowEdit(false);
        }
        //----------------------------------
        public string Grupa
        {
            set
            {
                FGrupa = value;
                hidGrupa.Value = value;
            }
            get
            {
                if (String.IsNullOrEmpty(FGrupa))
                    FGrupa = hidGrupa.Value;
                return FGrupa;
            }
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
                //ViewState["amode"] = value;
                hidEdit.Value = value.ToString();
            }
            get
            {
                if (FModeSet == -1)
                {
                    //FMode = Tools.GetInt(ViewState["amode"], moQuery);
                    FMode = Tools.StrToInt(hidEdit.Value, moQuery);
                    FModeSet = 1;
                }
                return FMode;
            }
        }

        public int PageSize
        {
            set { FPageSize = value; }
            get { return FPageSize; }
        }

        protected void lvArtykuly_DataBound(object sender, EventArgs e)
        {
            //Tools.ExecOnStart2("aresize", "resize();");
        }

        private void SetTopButtonsVisible(bool visible)
        {
            Tools.SetControlVisible(lvArtykuly, "paTopButtons", visible);
        }

        protected void lvArtykuly_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "NewRecord2":
                    ArticleEdit.Create(Grupa);
                    break;
                case "Edit2":
                    ArticleEdit.Edit(Grupa, Tools.GetDataKey(lvArtykuly, e));
                    break;



                case "NewRecord":

                    SetTopButtonsVisible(false);
                    break;
                case "CancelInsert":
                    SetTopButtonsVisible(true);
                    break;
                case "InsImg":
                    Editor ed = e.Item.FindControl("Tekst1Editor") as Editor;
                    HiddenField par = e.Item.FindControl("btInsImg_par") as HiddenField;
                    if (ed != null && par != null)
                    {
                        //string img = String.Format("<img src=\"{0}\" class=\"{1}\" />", par.Value, "artykul_image");
                        //ed.Content = img + ed.Content;



                        ed.Content = "aaaaaaa " + ed.Content;
                    }
                    break;
                case "test":
                    ed = e.Item.FindControl("Tekst1Editor") as Editor;

                    Tools.ExecOnStart2("edinsert1", String.Format("ajaxEditorInsertText('{0}','{1}');", ed.ClientID, "aaaaaaaaa "));

                    break;
            }
        }

        protected void lvArtykuly_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            SetTopButtonsVisible(false);
        }

        protected void lvArtykuly_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            SetTopButtonsVisible(true);
        }

        protected void lvArtykuly_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            SetTopButtonsVisible(true);
        }

        protected void lvArtykuly_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            SetTopButtonsVisible(true);
        }

        protected void lvArtykuly_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            SetTopButtonsVisible(true);
        }

        protected void InsertButton_Click(object sender, EventArgs e)
        {
            //ArticleEdit.Create(Grupa);
            cntArticleEditModal.ShowInsert(Grupa);
        }

        //protected void lbtnBack_Click(object sender, EventArgs e)
        //{
        //    Tools.Back();
        //}

        public String Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        protected void lnkEditArticle_Click(object sender, EventArgs e)
        {
            String articleId = Tools.GetCommandArgument(sender);
            if (!String.IsNullOrEmpty(articleId))
            {
                cntArticleEditModal.ShowEdit("ARTYKULY", articleId);
            }
            else
                Tools.ShowError("Wystąpił błąd!");
        }

        protected void lnkRemoveArticleConfirm_Click(object sender, EventArgs e)
        {
            String articleId = Tools.GetCommandArgument(sender);
            if (!String.IsNullOrEmpty(articleId))
            {
                btnRemoveArticle.CommandArgument = articleId;
                Tools.ShowConfirm("Czy na pewno chcesz usunąć artykuł?", btnRemoveArticle);
            }
            else
                Tools.ShowError("Wystąpił błąd!");
        }

        protected void btnRemoveArticle_Click(object sender, EventArgs e)
        {
            String articleId = Tools.GetCommandArgument(sender);
            if (!String.IsNullOrEmpty(articleId))
            {
                db.Execute(dsRemoveArticle, articleId);
                lvArtykuly.DataBind();
            }
            else
                Tools.ShowError("Wystąpił błąd!");
        }

        protected void cntArticleEditModal_Saved(object sender, EventArgs e)
        {
            //Tools.FindUpdatePanel(this).Update();
            upArticles.Update();
            lvArtykuly.DataBind();
        }


    }
}