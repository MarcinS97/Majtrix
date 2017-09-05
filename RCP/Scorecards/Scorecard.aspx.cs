using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.Scorecards.App_Code;

namespace HRRcp.Scorecards
{
    public partial class Scorecard : System.Web.UI.Page
    {
        private const String _OutsideJob = "1";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadScorecard();
            }
        }

        protected void SaveScorecard(object sender, EventArgs e)
        {
            ScorecardActual.Save();
        }

        protected void LoadScorecard()
        {
            String EmployeeId = null;
            String ScorecardTypeId = null;
            String Date = null;
            Boolean InEdit = false;
            String ObserverId = null;

            // !!!!! BEZPIECZEŃSTWO !!!!!
            if (Grid.cryptParams)
            {
                String p = Tools.GetStr(Request.QueryString["p"]);
                if (!String.IsNullOrEmpty(p))
                {
                    p = Report.DecryptQueryString(p, Grid.key, Grid.salt);
                    if (!String.IsNullOrEmpty(p))
                    {
                        String[] par = Tools.GetLineParams(p);
                        EmployeeId = par[0];
                        ScorecardTypeId = par[1];
                        Date = par[2];
                        InEdit = Convert.ToBoolean(par[3]);
                        ObserverId = par[4];
                    }
                }
            }
            else
            {
                EmployeeId = Request.Params["p1"];
                ScorecardTypeId = Request.Params["p2"];
                Date = Request.Params["p3"];
                InEdit = Convert.ToBoolean(Request.Params["p4"]);
                ObserverId = Request.Params["p5"];
            }




            ScorecardMenu2.ToggleItems(InEdit);

            Boolean Ok = !String.IsNullOrEmpty(EmployeeId) && !String.IsNullOrEmpty(ScorecardTypeId) && !String.IsNullOrEmpty(Date);

            if (Ok)
            {
                ScorecardActual.InEdit = InEdit;
                String Genre = db.getScalar(String.Format("select s.Id from scTypyArkuszy ta left join scSlowniki s on ta.Rodzaj = s.Id where ta.Id = {0} and s.Typ = '{1}' and s.Aktywny = 1", ScorecardTypeId, Dictionaries.Types.Spreadsheets));
                ScorecardActual.Prepare(EmployeeId, ScorecardTypeId, Date, _OutsideJob, Genre, ObserverId, !InEdit, ObserverId);
                ScorecardActual.Visible = true;
                PrepareTitle(EmployeeId, Date);
            }
            else
            {
                ScorecardActual.Visible = false;
            }
        }

        protected void PrepareTitle(String EmployeeId, String Date)
        {
            String Name = String.Empty;
            if (EmployeeId != null)
            {
                if(EmployeeId.StartsWith("-")) Name = db.Select.Scalar("select Nazwa from scTypyArkuszy where Id = abs({0})", EmployeeId);
                else Name = db.Select.Scalar(@"select Nazwisko + ' ' + Imie from Pracownicy where Id = {0}", EmployeeId);
                PageTitle1.Title = String.Format(@"Praca na inny arkusz, Scorecard: {0}, {1}", Name, Date);
            }
        }
    }
}
