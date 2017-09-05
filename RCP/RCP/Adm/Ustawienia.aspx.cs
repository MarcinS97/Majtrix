using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Adm
{
    public partial class UstawieniaForm : System.Web.UI.Page
    {
        private const string active_tab = "atabU"; 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HasFormAccess)   //App.User.HasAccess sprawdzane w MasterPage3
                {
                    Tools.SetNoCache();
                    //----------------------
                    Tools.SelectMenuFromSession(tabUstawienia, active_tab);
                    SelectTab();

                    if (!App.User.IsSuperuser)
                        Tools.RemoveMenu(tabUstawienia, "vScheduler");  // scheduler
                }
                else
                    App.ShowNoAccess();
            }
        }

        public static bool HasFormAccess
        {
            get 
            {
                return db.SqlMenuHasRights(5180, App.User);
                //return App.User.IsAdmin; 
            }
        }
        //-----------------------------------
        protected void tabUstawienia_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectTab();
        }

        private void SelectTab()
        {
            Tools.SelectTab(tabUstawienia, mvUstawienia, active_tab, false);
        }

    }
}