using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class Splity : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //-------------------------------
        private void SetPageSize(DataPager dp)  // wywoływana na zmianę w ddl wyboru - musi przybindować
        {
            DropDownList ddl = (DropDownList)lvSplity.FindControl("ddlLines");
            if (ddl != null)
            {
                if (ddl.SelectedValue == "all")
                    dp.SetPageProperties(0, dp.TotalRowCount, true);
                else
                {
                    int size = Tools.StrToInt(ddl.SelectedValue, 10);
                    if (size == 0) size = 10;
                    dp.SetPageProperties((dp.StartRowIndex / size) * size, size, true);
                }
            }
        }

        protected void ddlLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["pagesize"] = ((DropDownList)sender).SelectedValue;  // jak Brak danych to nie ustawia i trzeba samemu
            DataPager dp = (DataPager)lvSplity.FindControl("DataPager1");
            SetPageSize(dp);
        }
    }
}