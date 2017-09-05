using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Globalization;
using AjaxControlToolkit;
using HRRcp.App_Code;

namespace HRRcp.Controls.Portal
{
    public partial class cntPliki2 : System.Web.UI.UserControl
    {
        const int moQuery = 0;
        const int moEdit = 1;

        int FModeSet = -1;
        int FMode = moQuery;

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvNaglowki, 2);  // z ImageButtons
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                paEditButton.Visible = CanEdit;
            }
        }

        public string GetPath(object path)
        {
            return path.ToString();
        }

        public bool IsIco(object path)
        {
            return !String.IsNullOrEmpty(path.ToString());
        } 

        public int GetMode()
        {
            return FMode;
        }

        public bool IsEditable
        {
            get { return Mode == moEdit && CanEdit; }
        }

        public bool IsVisible(object par2, int id)
        {
            return db.getInt(par2, 1) == id;
        }

        public bool CanEdit
        {
            get { return App.User.IsPortalAdmin || App.User.HasRight(AppUser.rPortalArticles); }
        }

        public InsertItemPosition GetInsertItemPosition()
        {
            if (FMode == moEdit)
                return InsertItemPosition.LastItem;
            else
                return InsertItemPosition.None;
        }
        //-----------------------------------
        protected void FileUploadComplete(object sender, EventArgs e)
        {
            UploadFile(sender, e, "img", "tbImg");
        }

        private void UploadFile(object sender, EventArgs e, String v, string tb)
        {
            AsyncFileUploadEventArgs args = e as AsyncFileUploadEventArgs;
            AsyncFileUpload fu = sender as AsyncFileUpload;

            if (args != null && fu != null)
            {
                string fileName = Path.GetFileName(fu.PostedFile.FileName);
                string savePath = Server.MapPath(@"~/Portal/Pliki/");  //@ oznacza nie interpretuj - pozwala na umieszczenie \ na końcu
                
                int no = 0;
                string fn = Path.GetFileNameWithoutExtension(fileName);
                string ext = Path.GetExtension(fileName);
                while (File.Exists(savePath + fileName))
                {
                    no++;
                    fileName = String.Format("{0}_{1}{2}", fn, no, ext);
                    if (no > 100) 
                    {
                        fileName = String.Format("{0}_{1}{2}", fn, DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture), ext);
                        break;
                    }
                }
                fu.SaveAs(savePath + fileName);
                Image img = fu.Parent.FindControl("Image1") as Image;
                HiddenField hid = fu.Parent.FindControl("hidImageUrl") as HiddenField;
                if (img != null && hid != null)
                {
                    string file = string.Format(@"../Portal/Pliki/{0}", fileName);
                    Tools.ExecOnStart2("eimg", String.Format("top.document.getElementById('{0}').src='{2}';top.document.getElementById('{1}').value='{2}';", img.ClientID, hid.ClientID, file));
                }
            }
        }

        protected void FileUploadError(object sender, AsyncFileUploadEventArgs e)
        {
            Tools.ShowMessage("Wystąpił błąd podczas ładowania pliku.");
        }        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        //----------------------------
        protected void lvNaglowki_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
            }
        }

        protected void lvNaglowki_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
        }

        protected void lvNaglowki_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {

        }

        protected void lvNaglowki_ItemInserting(object sender, ListViewInsertEventArgs e)
        {

        }

        protected void lvNaglowki_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {

        }

        protected void lvNaglowki_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        protected void lvNaglowki_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {

        }

        protected void lvNaglowki_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "AddFile":
                    cntPlikiPliki pp = e.Item.FindControl("cntPlikiPliki1") as cntPlikiPliki;
                    if (pp != null)
                    {
                        pp.ShowInsert(true);
                    }
                    break;
            }
        }

        protected void lvNaglowki_DataBound(object sender, EventArgs e)
        {
            if (lvNaglowki.Items.Count == 0 && CanEdit && Mode == moQuery)
                paEditButton.Visible = false;
        }

        protected void lvNaglowki_DataBinding(object sender, EventArgs e)
        {

        }

        protected void lvNaglowki_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                cntPlikiPliki pp = e.Item.FindControl("cntPlikiPliki1") as cntPlikiPliki;
                if (pp != null)
                {
                    pp.Mode = FMode;
                }
            }
        }

        //----------------------------------
        private void ShowEdit(bool show)
        {
            paEditButton.Visible = !show;
            if (show)
            {
                Mode = moEdit;
                Tools.AddClass(paPliki, "cntPlikiEdit");
            }
            else
            {
                Mode = moQuery;
                Tools.RemoveClass(paPliki, "cntPlikiEdit");
            }
            lvNaglowki.DataBind();
            Tools.SetControlVisible(lvNaglowki, "paTopButtons", IsEditable);
        }

        protected void btEdit_Click(object sender, EventArgs e)
        {
            ShowEdit(true);
        }

        protected void btCancelEdit_Click(object sender, EventArgs e)
        {
            ShowEdit(false);
        }
        //----------------------------
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

        public string Grupa
        {
            set { hidGrupa.Value = value; }
            get { return hidGrupa.Value; }
        }

   }
}