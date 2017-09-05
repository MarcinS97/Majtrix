using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP
{
    public partial class Harmonogram2 : System.Web.UI.Page
    {
        //public new int FormId = 1;
        //public new String FormTitle = "Harmonogram";

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                if (HasFormAccess)   //App.User.HasAccess sprawdzane w MasterPage3
                {
                    Tools.SetNoCache();
                    //----------------------
                    //cntPlanPracyZmiany.Prepare(null, DateTime.Today, true);
                    cntPlanPracyZmiany.Prepare();

                    /*
                    PlanPracyAccept.InitScripts();

                    string pracId = SelectPracIdPP;
                    if (!String.IsNullOrEmpty(pracId))
                    {
                        SelectPracIdPP = null;
                        string kierId = GetKierId(pracId);
                        if (!String.IsNullOrEmpty(kierId))
                        {
                            Tools.SelectItem(ddlKierownicy, kierId);
                            ddlKierownicy_SelectedIndexChanged(ddlKierownicy, EventArgs.Empty);
                        }
                    }
                     */
                }
                else
                    App.ShowNoAccess();
            }
        }

        public static bool HasFormAccess
        {
            get 
            {
                return db.SqlMenuHasRights(5020, App.User);
                //return App.User.IsKierownik || App.User.IsAdmin; 
            } // App.User.HasRight(AppUser.rPlanPracy); }
        }
    }
}