using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP
{
    public partial class Pracownicy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HasFormAccess)   //App.User.HasAccess sprawdzane w MasterPage3
                {
                    Tools.SetNoCache();
                    //----------------------
                }
                else
                    App.ShowNoAccess();
            }
        }

        public static bool HasFormAccess
        {
            //get { return App.User.IsKierownik || App.User.IsAdmin; }
            get 
            { 
                //return App.User.IsAdmin; 
                return db.SqlMenuHasRights(5070, App.User);
            }
        }

        //---------------------------
        protected void cntPracownicy2_Command(object sender, CommandEventArgs e)
        {
        }

        protected void cntPracownicy2_SelectedChanged(object sender, EventArgs e)    // selectOkres
        {
            string pracId = cntPracownicy2.SelectedPracId;
            App.SetSesSelPracId(pracId);

            //SelectPracIdPrzes = pracId;
            //SelectPracIdPP = pracId;
            //SelectPracIdAcc = pracId;
            //SelectPracIdRozl = pracId;
            //SelectPracIdPlanUrlop = pracId;
        }

    }
}