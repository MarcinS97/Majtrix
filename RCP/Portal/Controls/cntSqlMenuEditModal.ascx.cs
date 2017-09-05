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
    public partial class cntSqlMenuEditModal : System.Web.UI.UserControl
    {
        public event EventHandler Saved;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Show()
        {
            cntModal.Show();
            this.MenuId = null;
            dbField.FillData(this, null, 0, 0, 0, dbField.moEdit);
            btnDeleteConfirm.Visible = btnDelete.Visible = false;
        }

        public void Show(String Id)
        {
            cntModal.Show();
            this.MenuId = Id;
            DataRow dr = db.Select.Row(dsData, MenuId);
            dbField.FillData(this, dr, 0, 0, 0, dbField.moEdit);
            btnDeleteConfirm.Visible = btnDelete.Visible = true;
        }

        public void Hide()
        {
            cntModal.Close();
        }

        public String MenuId
        {
            get { return ViewState["vMenuId"] as String; }
            set { ViewState["vMenuId"] = value; }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(MenuId)) //insert
            {
                dbField.dbInsert(db.conP, this, "SqlMenu", null, null);

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

        public void SetGrupa(String val)
        {
            Grupa.Value = val;
        }
    }
}