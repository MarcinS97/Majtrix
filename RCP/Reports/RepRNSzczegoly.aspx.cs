using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Reports
{
    public partial class RepRNSzczegoly : System.Web.UI.Page
    {
        //protected void Page_PreInit(object sender, EventArgs e)
        //{
        //    this.MasterPageFile = App.GetMasterPage();
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            // spr czy kierownik i czy podgląd swoich ludzi <<<<<<<<<<<<<<<<<<<<<

            App.CheckPrawaRozlNadg();
            if (!Report.CheckParametersCount(4, 4) ||
                !Report.CheckParameterData(1) ||
                !Report.CheckParameterData(2) ||
                !Report.CheckParameterToken(3, "drN|P50|P100|NOC|W50|W100|O50|O100|*") ||
                !Report.CheckParameterInt(4, 1, int.MaxValue))
                App.ShowBadRepParameters("Raport - Rozliczenie nadgodzin");
            if (!IsPostBack)
            {
                /*
                if (Tools.GetStr(Request.QueryString["p3"]) != "2")
                    cntReport1.SQL3 = null;
                else
                    cntReport1.SQL4 = null;
                 */ 
            }
        }
    }
}
