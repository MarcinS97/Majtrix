using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Reports
{
    public partial class RepCCPracownicyCC : System.Web.UI.Page
    {
        const string sesid = "_praccc_sel";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SelectOkres1.Prepare(DateTime.Today, true);


                App.CheckccPrawaSplity(cntReport2);
                if (App.User.HasRight(AppUser.rRepPodzialCCKwoty))
                {
                    tabWyniki.Visible = true;
                    App.CheckccPrawaSplity(cntReport3);

                    Tools.SelectMenuFromSession(tabWyniki, sesid);
                    SelectTab();
                }
                else
                {
                    tabWyniki.Visible = true;
                    mvWyniki.SetActiveView(View1);
                }
            }
            DataBind();
        }

        protected void tabWyniki_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectTab();
            Session[sesid] = tabWyniki.SelectedValue;
        }

        private void SelectTab()
        {
            int idx = Tools.StrToInt(tabWyniki.SelectedValue, -1);
            mvWyniki.ActiveViewIndex = idx;
        }
        //------------------
        public object GetOkres()
        {
            return SelectOkres1.DateFrom + " - " + SelectOkres1.DateTo;
        }
    }
}
