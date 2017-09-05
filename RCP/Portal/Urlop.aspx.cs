using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Portal
{
    public partial class UrlopForm : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            if (!IsPostBack)
            {
                if (!App.User.IsAdmin || !App.User.HasRight(AppUser.rKwitekAdm))
                {
                    DropDownList1.Visible = false;
                    //Urlop1.Prepare(App.User.NR_EW, null);
                }
                else
                {
                    DropDownList1.Visible = true;
                    //Tools.SelectItem(DropDownList1, App.User.NR_EW);
                }

            }
             */
            if (!IsPostBack)
            {
                Urlop1.Prepare(null, true);
                if (Lic.portalPrint)
                    btPrint.Visible = true;
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Urlop Form");
        }
        //--------------------------
        /*
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Urlop1.Prepare(DropDownList1.SelectedValue, null);
        }

        protected void DropDownList1_DataBound(object sender, EventArgs e)
        {
            Urlop1.Prepare(DropDownList1.SelectedValue, null);
        }
         */ 
    }
}
