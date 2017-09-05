using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Controls.Adm
{
    public partial class cntOkresyTypy : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvOkresyTypy, Tools.ListViewMode.Bootstrap);
        }

        protected void Delete(String Id)
        {
            if (String.IsNullOrEmpty(Id))
                throw new Exception("Brak wybranego okresu!");

            Boolean CanDelete = db.Select.Boolean(dsCheckIfCanDelete, Id);
            if (!CanDelete)
                throw new Exception("Istnieją już okresy rozliczeniowe przypisane pod podany typ. Przed usunięciem typu okresu usuń istniejące okresy.");

            db.Execute(dsDelete, Id);
            lvOkresyTypy.DataBind();

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            String Id = Tools.GetCommandArgument(sender);
            try
            {
                Delete(Id);
            }
            catch (Exception ex)
            {
                Tools.ShowError(ex.Message);
            }
        }

        protected void btnDeleteConfirm_Click(object sender, EventArgs e)
        {
            String Id = Tools.GetCommandArgument(sender);
            btnDelete.CommandArgument = Id;
            Tools.ShowConfirm("Potwierdzasz usunięcie rekordu danych?", btnDelete);
        }
    }
}