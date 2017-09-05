using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP
{
    public partial class AkceptacjaCzasuPracy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HasFormAccess)   //App.User.HasAccess sprawdzane w MasterPage3
                {
                    Tools.SetNoCache();
                    //----------------------
                    cntPlanPracyAccept.Prepare();
                }
                else
                    App.ShowNoAccess();
            }
        }

        public static bool HasFormAccess
        {
            get
            {
                return db.SqlMenuHasRights(5040, App.User);
                //return App.User.IsKierownik || App.User.IsAdmin; 
            }
        }
        //-------------------------
        //protected void ddlKierownicy2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    PlanPracyAccept.Prepare(ddlKierownicy2.SelectedValue);
        //}
    }
}