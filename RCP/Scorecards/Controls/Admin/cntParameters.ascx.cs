using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards.Controls.Admin
{
    public partial class cntParameters : System.Web.UI.UserControl
    {
        //private const string active_tab = "mnScWn";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie
        private const string active_tab = "mnScParams";  //T

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SelectMenuFromSession(mnLeft, active_tab);
                SelectMenu(false);
                //Tools.ExecOnStart2("resize2", "resize();");
            }
        }

        public void Prepare()
        {
        }
        //----------------
        private void SelectMenu(bool setHelp)
        {
            Tools.SelectTab(mnLeft, mvWnioski, active_tab, setHelp);
        }

        protected void mnLeft_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectMenu(true);
        }
        //-----------------
        protected void ddlKier_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string kid = ddlKier.SelectedValue;
            //PrepareReport();
        }

        protected void cntSelectRokMiesiac_NextAll(object sender, EventArgs e)
        {
        }

        protected void cntSelectRokMiesiac_BackAll(object sender, EventArgs e)
        {
            //DateTime? dt = db.getScalar<DateTime>("SELECT TOP 1 miesiac FROM ccLimity where isLast = 1 AND Limit IS NOT NULL ORDER BY miesiac");

            /*
            if (dt.HasValue)
            {
                SRM.Rok = dt.Value.Year;
                SRM.Miesiac = dt.Value.Month;
            }
            else
            {
                SRM.SelectNow();
            }
            */
        }

        protected void cntSelectRokMiesiac_ValueChanged(object sender, EventArgs e)
        {
        }
    }
}