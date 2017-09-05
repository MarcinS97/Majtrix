using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Adm
{
    public partial class PracownicyHarm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HasFormAccess)   //App.User.HasAccess sprawdzane w MasterPage3
                {
                    Tools.SetNoCache();
                    //----------------------
                    cntPracownicyHarm.Prepare();
                }
                else
                    App.ShowNoAccess();
            }
        }

        public static bool HasFormAccess
        {
            get
            {

                return App.User.IsAdmin;
                //return db.SqlMenuHasRights(5040, App.User);  //!!!!!!!!!!!!!!!!!!!!!!!! zmienić 
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