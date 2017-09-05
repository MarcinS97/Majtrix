using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards.Controls
{
    public partial class cntScorecardSelect : System.Web.UI.UserControl
    {
        public event EventHandler DateChanged;
        public event EventHandler EmployeeChanged;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //cntSelectRokMiesiac.DataBind();
                //----- zawężenie zakresu widocznych przełożonych -----
                //if (!App.User.IsScAdmin)
                //{
                //    ddlKier.DataBind();
                //    if (ddlKier.Items.Count > 2)    // wybierz... i ja zawsze jest
                //        tdKier.Visible = true;
                //}
                //else
                //    tdKier.Visible = true;
            }
        }

        public void Prepare(bool prevEnabled, bool nextEnabled, string employeeId)
        {
            //btnPreviousEmployee.Enabled = prevEnabled;
            //btnNextEmployee.Enabled = nextEnabled;
            //lblCurrentEmployee.Text = db.getScalar(String.Format(@"select Nazwisko + ' ' + Imie from Pracownicy where Id = {0}", employeeId));
        }

        protected void ChangeEmployeeLeft(object sender, EventArgs e)
        {
            if (EmployeeChanged != null) EmployeeChanged("-1", EventArgs.Empty);
        }

        protected void ChangeEmployeeRight(object sender, EventArgs e)
        {
            if (EmployeeChanged != null) EmployeeChanged("1", EventArgs.Empty);
        }

        protected void cntSelectRokMiesiac_NextAll(object sender, EventArgs e)
        {
            //cntSelectRokMiesiac.SelectNow();
            if (DateChanged != null) DateChanged(sender, EventArgs.Empty);
        }

        protected void cntSelectRokMiesiac_BackAll(object sender, EventArgs e)
        {
            //DateTime? dt = db.getScalar<DateTime>("SELECT TOP 1 miesiac FROM ccLimity where isLast = 1 AND Limit IS NOT NULL ORDER BY miesiac");
            //cntSelectRokMiesiac.Rok = 2015;
            //cntSelectRokMiesiac.Miesiac = 6;

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
            if (DateChanged != null) DateChanged(sender, EventArgs.Empty);
        }

        protected void cntSelectRokMiesiac_ValueChanged(object sender, EventArgs e)
        {
            if (DateChanged != null) DateChanged(sender, EventArgs.Empty);
        }

        public String SelectedDate
        {
            get
            {
                return null;
                //return String.Format(@"{0}-{1}-01", cntSelectRokMiesiac.Rok, cntSelectRokMiesiac.Miesiac);
            }
        }

    }
}