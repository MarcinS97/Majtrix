using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class RcpAnalizeControl : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
        }
        //------------------
        public void Prepare(DataSet dsAnalize, string alg, DataRow wtdr)
        {
            ListView1.DataSource = dsAnalize;
            ListView1.DataBind();

            int s1 = 0;
            int s2 = 0;
            if (dsAnalize != null)
            {
                foreach (DataRow dr in Base.getRows(dsAnalize))
                {
                    int czas = Base.getInt(dr, "CzasSec", 0);
                    switch (Base.getInt(dr, "Typ", -1))
                    {
                        case Worktime2.aeBreak:
                        case Worktime2.aePrzerwaWliczona:
                            s1 += czas;
                            s2 += czas;
                            break;
                        case Worktime2._aeBreakNoCount:
                        case Worktime2.aePrzerwaNiewliczona:
                            s1 += Math.Abs(czas);   // moze byc < 0 jak przerwa niewliczona ustawiona dla pracownika np. palacz
                            break;
                        case Worktime2.aeBreakOver:
                        case Worktime2.aePrzerwaWliczonaNadg:
                            s1 += czas;
                            s2 += czas;
                            break;
                        case Worktime2.aeBreakOverNoCount:
                            s1 += czas;
                            break;
                    }
                }
            }
            //bool v = s1 != 0 || s2 != 0 || alg == Worktime.algSuma || alg == Worktime.algSumaNadg;    //20140101
            bool v = alg == Worktime.algSuma || alg == Worktime.algSumaNadg;
            Tools.SetControlVisible(ListView1, "trPrzerwa1", v);
            Tools.SetControlVisible(ListView1, "trPrzerwa2", v);
            if (v)
            {
                Label lb = (Label)ListView1.FindControl("SumPrzerwLabel");
                if (lb != null) lb.Text = Worktime.SecToTime(s1, 0);
                lb = (Label)ListView1.FindControl("SumPrzerwNomLabel");
                if (lb != null) lb.Text = Worktime.SecToTime(s2, 0);
            }

            Boolean isAbsencjaGodz = /*!db.isNull(wtdr["AbsencjaId"]) &&*/ db.getInt(wtdr, "AbsencjaDni", -1) == 0; /* wyswietlaj tylko wtedy kiedy dni sa rowne zero */

            Tools.SetControlVisible(ListView1, "trAbsGodz", isAbsencjaGodz);

            if (isAbsencjaGodz)
            {
                /*Label lb = (Label)ListView1.FindControl("lbAbsGodz");
                if (lb != null) lb.Text = Worktime.SecToTime(db.getInt(wtdr, "AbsencjaGodzin", 0) * 3600, 0);*/
                Tools.SetText2(ListView1, "lbAbsGodz", Worktime.SecToTime(db.getInt(wtdr, "AbsencjaGodzin", 0) * 3600, 0));
                Tools.SetText2(ListView1, "ltAbsGodzNazwa", db.getValue(wtdr, "AbsencjaNazwa"));
            }

        }

        public string nadg50
        {
            set { Tools.SetText(ListView1, "lbNadg50", value); }
            get { return Tools.GetText(ListView1, "lbNadg50"); }
        }

        public string nadg100
        {
            set { Tools.SetText(ListView1, "lbNadg100", value); }
            get { return Tools.GetText(ListView1, "lbNadg100"); }
        }
    }
}