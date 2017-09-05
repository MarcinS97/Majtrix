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


//save uploaded images to db
//http://code.msdn.microsoft.com/Upload-Files-Asynchronously-829691df

//przyspieszenie editora
//https://ajaxcontroltoolkit.codeplex.com/workitem/26791


namespace HRRcp.Controls.Portal
{
    public partial class cntArticles3 : System.Web.UI.UserControl
    {
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
            
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                paEditButton.Visible = CanEdit;
                DataPager dp = lvArtykuly.FindControl("DataPager1") as DataPager;
                if (dp != null)
                {
                    if (FPageSize < 1)
                        dp.Visible = false;
                    else
                        dp.PageSize = FPageSize;
                }
            }
            
            
            //ShowInsert(FMode == moEdit);
            /*
            if (Request.QueryString["preview"] == "1" && !string.IsNullOrEmpty(Request.QueryString["fileId"]))
            {
                var fileId = Request.QueryString["fileId"];
                var fileContents = (byte[])Session["fileContents_" + fileId];
                var fileContentType = (string)Session["fileContentType_" + fileId];

                if (fileContents != null)
                {
                    Response.Clear();
                    Response.ContentType = fileContentType;
                    Response.BinaryWrite(fileContents);
                    Response.End();
                }
            }
            */ 
        }

        protected void x_htmlEditorExtender2_OnImageUploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            //Log.Info(Log.t2APP, "UPLOAD_IN2", String.Format("FieldId: {0} FileName: {1} FileSize: {2} PostedUrl: {3} ContentType: {4}", e.FileId, e.FileName, e.FileSize, e.PostedUrl, e.ContentType));

            /*
            switch (e.State)
            {
                case AjaxFileUploadState.Failed:
                    break;
                case AjaxFileUploadState.Unknown:
                    break;
                case AjaxFileUploadState.Success:
                    break;
            }
            */

            if (e.ContentType.Contains("jpg") ||
                e.ContentType.Contains("gif") ||
                e.ContentType.Contains("png") ||
                e.ContentType.Contains("jpeg"))
            {
                //Session["fileContentType_" + e.FileId] = e.ContentType;
                //Session["fileContents_" + e.FileId] = e.GetContents();

                //string fn = e.FileName;
                string ext = Path.GetExtension(e.FileName);
                string fn = e.FileId + ext;

                string fullpath = Server.MapPath(ArtykulyPath) + fn;
                ((AjaxFileUpload)sender).SaveAs(fullpath);
                e.PostedUrl = Page.ResolveUrl(ArtykulyPath + fn);
            }

            // Set PostedUrl to preview the uploaded file.         
            //e.PostedUrl = string.Format("?preview=1&fileId={0}", e.FileId);

            //Log.Info(Log.t2APP, "UPLOAD_OUT2", String.Format("PostedUrl: {0}", e.PostedUrl));
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

        public InsertItemPosition GetInsertItemPosition()
        {
            if (IsEditable)
                return InsertItemPosition.FirstItem;
            else
                return InsertItemPosition.None;
        }
        //----------------------------------
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        






        /*
        protected void btnsubmit_click(object sender, EventArgs e)
        {
            TextBox tb = lvArtykuly.InsertItem.FindControl("tbArtykul") as TextBox;
            if (tb != null)
            {
                // Retrieve the html contents from htmleditor extender
                string htmlContents = System.Web.HttpUtility.HtmlDecode(tb.Text);
            }
        }
        */

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
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


        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        /*
        
            //Log.Info(Log.t2APP, "UPLOAD_IN2", String.Format("FieldId: {0} FileName: {1} FileSize: {2} PostedUrl: {3} ContentType: {4}", e.FileId, e.FileName, e.FileSize, e.PostedUrl, e.ContentType));

            //switch (e.State)
            //{
            //    case AjaxFileUploadState.Failed:
            //        break;
            //    case AjaxFileUploadState.Unknown:
            //        break;
            //    case AjaxFileUploadState.Success:
            //        break;
            //}
        
            if (e.ContentType.Contains("jpg") ||
                e.ContentType.Contains("gif") ||
                e.ContentType.Contains("png") ||
                e.ContentType.Contains("jpeg"))
            {
                //Session["fileContentType_" + e.FileId] = e.ContentType;
                //Session["fileContents_" + e.FileId] = e.GetContents();

                //string fn = e.FileName;
                string ext = Path.GetExtension(e.FileName);
                string fn = e.FileId + ext;

                string fullpath = Server.MapPath(ArtykulyPath) + fn;
                ((AjaxFileUpload)sender).SaveAs(fullpath);
                e.PostedUrl = Page.ResolveUrl(ArtykulyPath + fn);
            }

            // Set PostedUrl to preview the uploaded file.         
            //e.PostedUrl = string.Format("?preview=1&fileId={0}", e.FileId);

            //Log.Info(Log.t2APP, "UPLOAD_OUT2", String.Format("PostedUrl: {0}", e.PostedUrl));

        */


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



            //Image img = fu.Parent.FindControl("Image1") as Image;
            //HiddenField hidIco = fu.Parent.FindControl("hidImage") as HiddenField;
            //HiddenField hidCmd = fu.Parent.FindControl("hidCommand") as HiddenField;
            //HiddenField hidPar1 = fu.Parent.FindControl("hidPar1") as HiddenField;
            //TextBox tbNapis = fu.Parent.FindControl("FileNameTextBox") as TextBox;

            if (args != null && fu != null && hid != null)  // && ed != null)// && img != null && hidCmd != null && hidPar1 != null && tbNapis != null && hidIco != null)
            {
                string fileName = Path.GetFileName(fu.PostedFile.FileName);
                string savePath = Server.MapPath(ArtykulyPath);    //@ oznacza nie interpretuj - pozwala na umieszczenie \ na końcu

                string fn = GetFileName(hid.Value, fileName);   // dokładam id_
                fu.SaveAs(savePath + fn);

                string cmd = ArtykulyPath + fn;  // link
                //string ico = GetIco(fileName);
                //string img = String.Format("<img alt=\"\" class=\"{1}\" src=\"{0}\"/>", cmd, "img_left");
                string img = String.Format("<img alt='' class='{1}' src='{0}'/>", cmd, "img_left");

                //ed.Content = img + ed.Content;
                //Tools.ExecOnStart2("edrefresh", String.Format("ajaxEditorUpdate('{0}');",ed.ClientID));

                Tools.ExecOnStart2("edinsert", String.Format("ajaxEditorInsertText('{0}','{1}');", ed.ClientID, "aaaaaaaaa "));



                //InsertImg(fu.Parent, cmd);





                //img.ImageUrl = ico;
                //hidIco.Value = ico;
                //hidCmd.Value = cmd;
                //hidPar1.Value = fu.ContentType;

                /*
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
                 hidCmd.ClientID, cmd,
                 hidPar1.ClientID, fu.ContentType,
                 napis));
                  */

                //Log.Info(Log.t2APP, "UPLOAD_OUT", String.Format("File: {0} Path: {1} fn: {2} ico: {3}", fileName, savePath, fn, ico));

            }
        }

        protected void FileUploadError(object sender, AsyncFileUploadEventArgs e)
        {
            Log.Info(Log.t2APP, "UPLOAD_ERROR", String.Format("File: {0} Msg: {1}", e.FileName, e.StatusMessage));
            Tools.ShowMessage("Wystąpił błąd podczas ładowania pliku.");
        }


        //----------------------------------
        protected void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
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
                        

                        if (setEditor)
                            Tools.ExecOnStart2("setedit", String.Format("setCurrentAjaxEditor('{0}');", ed.ClientID));

                    }
                    else
                        Tools.SetText2(e.Item, "Literal1", s);
                    break;
                case ListViewItemType.EmptyItem:
                    Tools.SetControlVisible(e.Item, "InsertButton", IsEditable);
                    break;
            }
        }

        private bool Update(Control item, IOrderedDictionary values, bool insert)
        {
            if (!insert)
            {
                //zzstring s = db.sqlPut(Tools.GetText(item, "tbArtykul"));
                //string s = Tools.GetText(item, "tbArtykul");

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
            paEditButton.Visible = !show;
            if (show)
                Mode = moEdit;
            else
                Mode = moQuery;
            lvArtykuly.DataBind();
            Tools.SetControlVisible(lvArtykuly, "paTopButtons", IsEditable);
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
            get { return FGrupa; }
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

        public int PageSize
        {
            set { FPageSize = value; }
            get { return FPageSize; }
        }

        protected void lvArtykuly_DataBound(object sender, EventArgs e)
        {
            Tools.ExecOnStart2("aresize", "resize();");
        }

        private void SetTopButtonsVisible(bool visible)
        {
            Tools.SetControlVisible(lvArtykuly, "paTopButtons", visible);
        }

        protected void lvArtykuly_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
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

        bool setEditor = false;
        protected void lvArtykuly_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            SetTopButtonsVisible(false);
            setEditor = true;
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
    }
}