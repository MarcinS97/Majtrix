using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards
{
    public partial class Arkusze : System.Web.UI.Page
    {
        private const string active_tab = "atabScArk";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie
        private const string FormName = "Scorecards - Arkusze";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (App.User.IsScAdmin)
                {
                    Tools.SetNoCache();
                    Tools.EnableUpload();
                    Tools.SelectMenuFromSession(tabAdmin, active_tab);
                    SelectTab();

                    //Tools.ExecOnStart2("resize2", "resize();");
                }
                else
                    App.ShowNoAccess(FormName, null);
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(FormName);
        }
        //----------------------------------        
        protected void tabAdmin_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectTab();
        }

        private void SelectTab()
        {
            Session[active_tab] = tabAdmin.SelectedValue;
            Tools.SelectTab(tabAdmin, mvArkusze, active_tab, false);
        }

        //------------------------------------------------

        protected void ActivateCzynnosciArkusze(object sender, EventArgs e)
        {
            SpreadsheetsTasks.DataBind();
        }

        protected void ActivateParametry(object sender, EventArgs e)
        {
            SpreadsheetsParameters.DataBind();
        }
    }
}
