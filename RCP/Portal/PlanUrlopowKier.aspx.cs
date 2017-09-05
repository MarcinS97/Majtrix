using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Portal
{
    public partial class PlanUrlopowKier : System.Web.UI.Page
    {
        const string FormName = "Portal - Plan Urlopów Przełożony";

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                if (App.User.HasRight(AppUser.rPlanUrlopow))
                {
                    //ddl jeszcze
                    cntPlanUrlopow.Prepare(App.User.Id, DateTime.Now, true);
                }
                else
                    App.ShowNoAccess(FormName, App.User);
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(FormName);
        }

        protected void ddlKierownicy4_SelectedIndexChanged(object sender, EventArgs e)
        {
            cntPlanUrlopow.Prepare(ddlKierownicy4.SelectedValue);
        }
    }
}
