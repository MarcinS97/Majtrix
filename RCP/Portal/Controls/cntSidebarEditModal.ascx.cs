using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls
{
    public partial class cntSidebarEditModal : System.Web.UI.UserControl
    {
        public event EventHandler Saved;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //cntIconPicker.Visible = 
            }
        }

        public void Show(string id, string grupa)
        {
            cntModal.Title = "Edycja menu";
            cntModal.Show();
            this.MenuId = id;
            this.Group = grupa;
            btnDelete.Visible = btnDeleteConfirm.Visible = true;
            DataRow dr = db.Select.Row(dsData, MenuId);
            dbField.FillData(this, dr, 0, 0, 0, dbField.moEdit);
            cntIconPicker._SelectedIcon = db.getValue(dr, "Image");
        }
        
        
        public void Show(string grupa)
        {
            cntModal.Title = "Dodaj menu";
            cntModal.Show();
            this.MenuId = null;
            this.Group = grupa;
            btnDelete.Visible = btnDeleteConfirm.Visible = false;
            dbField.FillData(this, null, 0, 0, 0, dbField.moEdit);
        }


        public void Hide()
        {
            cntModal.Close();
        }

        public String Group
        {
            get { return hidGroup.Value; }
            set { hidGroup.Value = value; }
        }

        public String MenuId
        {
            get { return hidMenuId.Value; }
            set { hidMenuId.Value = value; }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(MenuId)) //insert
            {
                dbField.dbInsert(db.conP, this, "SqlMenu", "Grupa,Image", db.strParam("LEFTMENU" + Group), db.nullStrParam(cntIconPicker._SelectedIcon));

            }
            else //update
            {
                dbField.dbUpdate(db.conP, this, "SqlMenu", "Id=" + MenuId, null, null);
            }
            TriggerSaved();
            Hide();
        }

        void TriggerSaved()
        {
            if (Saved != null)
                Saved(null, EventArgs.Empty);
        }

        public void Delete()
        {
            db.Execute(dsDelete, MenuId);
        }

        protected void btnDeleteConfirm_Click(object sender, EventArgs e)
        {
            Tools.ShowConfirm("Potwierdzasz usunięcie rekordu danych?", btnDelete);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Delete();
            TriggerSaved();
            Hide();
        }
    }
}