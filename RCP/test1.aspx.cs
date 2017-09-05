using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class test1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AppUser user = AppUser.CreateOrGetSession();
            if (!IsPostBack)
            {
                if (user.HasAccess && user.IsAdmin)
                {
                    PrepareKierownicy();
                    Tools.MakeConfirmButton(btPRP, "Potwierdź operację eksportu danych do PRP.");
                }
                else App.ShowNoAccess("test.aspx", user);
            }
        }

        private void PrepareKierownicy()
        {
            DataSet ds = Base.getDataSet("select Nazwisko + ' ' + Imie as NI, Id from Pracownicy where Kierownik = 1 order by Nazwisko, Imie");
            Tools.BindData(ddlKierownicy, ds, "NI", "Id", true, null);
        }

        protected void ddlKierownicy_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        //----------------------------------------------------------------------------------------
        protected void btImportKP_Click(object sender, EventArgs e)
        {
            int err = KP.ImportKP();
            if (err >= 0)
                Tools.ShowMessage("Import zakończnony poprawnie.");
            else
                Tools.ShowMessage("Wystąpił błąd podczas importu.");
        }

        protected void btPRP_Click(object sender, EventArgs e)
        {
            int err = KP.ExportToPRP();
            if (err >= 0) 
                Tools.ShowMessage("Eksport zakończnony poprawnie.");
            else
                Tools.ShowMessage("Wystąpił błąd podczas eksportu.");
        }

    }
}
