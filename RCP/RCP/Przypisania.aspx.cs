using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP
{
    public partial class Przypisania : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HasFormAccess)   //App.User.HasAccess sprawdzane w MasterPage3
                {
                    Tools.SetNoCache();
                    //----------------------

                    if (App.User.IsAdmin)
                    {
                        cntPrzesunieciaAdm.Visible = true;
                        cntPrzesunieciaAdm.Prepare();
                    }
                    else
                    {
                        cntPrzesuniecia.Visible = true;
                        cntPrzesuniecia.Prepare();
                    }

                    /*
                    string pracId = SelectPracIdPrzes;
                    if (!String.IsNullOrEmpty(pracId))
                    {
                        SelectPracIdPrzes = null;
                        cntPrzesuniecia.SelectPrac(pracId);
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

                return db.SqlMenuHasRights(5080, App.User);
                //return App.User.HasRight(AppUser.rPrzesuniecia) || App.User.HasRight(AppUser.rPrzesunieciaAcc) || App.User.HasRight(AppUser.rPrzesunieciaAdm); 
            }
        }

        //------------------------
        private string SelectPracIdPrzes
        {
            set { ViewState["pidprzes"] = value; }
            get { return Tools.GetStr(ViewState["pidprzes"]); }
        }

    }
}