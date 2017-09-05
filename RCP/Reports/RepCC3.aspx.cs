using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Reports
{
    public partial class RepCC3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SetNoCache();

                string d1 = Tools.GetStr(Request.QueryString["p1"]);
                string d2 = Tools.GetStr(Request.QueryString["p2"]);

                cntSqlTabs1.Prepare(String.Format(@"
select distinct Class as Value, Class as Text, 1 as Sort from PodzialLudziImport where Miesiac = '{0}'
union all 
select 'all' as Value, 'Wszystkie klasyfikacje' as Text, 2 as Sort 
order by Sort, Text
                    ", d1, d2));
            }
        }

        protected void cntSqlSelectTabs1_SelectTab(object sender, EventArgs e)
        {
            App.CheckccPrawa(cntReport1);

            cntReport1.SQL3 = cntSqlTabs1.SelectedValue;
            string klas = cntSqlTabs1.Tabs.SelectedValue;
            if (klas == "all")
                klas = "Wszystkie klasyfikacje";
            cntReport1.Title = String.Format("Podział czasu pracy na CC - {0}", klas);
            cntReport1.Prepare();
            cntReport1.GridDataBind();
        }
    }
}
