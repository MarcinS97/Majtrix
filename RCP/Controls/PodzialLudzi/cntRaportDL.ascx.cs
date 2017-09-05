using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.PodzialLudzi
{
    public partial class cntRaportDL : System.Web.UI.UserControl
    {
        int FMode = moUser;
        const int moUser = 0;
        const int moAdm = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool adm = App.User.HasRight(AppUser.rPodzialLudziAdm);
                //xpaKierWr.Visible = ((FMode == moAdm) && adm);
                //paKier.Visible = adm;
                tdKier.Visible = adm;
                cntSelectRokMiesiac.DataBind();
                PrepareReport();
            }
        }

        public bool Prepare()
        {
            bool adm = App.User.HasRight(AppUser.rPodzialLudziAdm);
            //bool PL = App.User.HasRight(AppUser.rPodzialLudzi);
            bool PM = App.User.HasRight(AppUser.rPodzialLudziPM);
            if (adm || PM)
            {
                string d1 = Tools.GetStr(Request.QueryString["p1"]);
                string d2 = Tools.GetStr(Request.QueryString["p2"]);

                if (adm)
                {
                    ddlPM.DataBind();
                    Tools.SelectItem(ddlPM, App.User.Id);
                }
                PrepareReport();
                return true;
            }
            else
                return false;  // brak dostępu
        }

        //---------------------------------
        const string ALL = "-99";

        private void PrepareReport()
        {
            bool adm = App.User.HasRight(AppUser.rPodzialLudziAdm);
            //bool all = App.User.HasRight(AppUser.rRepPodzialCCAll);
            //bool kwoty = App.User.HasRight(AppUser.rRepPodzialCCKwoty);
            /*
            switch (FMode)
            {
                default:
                case moKier:
                    cntReport1.SQL1 = App.User.Id;
                    break;
                case moAdm:
                    cntReport1.SQL1 = ddlPM.SelectedValue;
                    break;
            }
            */
            //string kid = ((FMode == moAdm) && adm) ? ddlPM.SelectedValue : App.User.Id;
            string kid = adm ? ddlPM.SelectedValue : App.User.Id;
            cntReport1.SQL1 = string.IsNullOrEmpty(kid) ? "-99" : kid;
            
            cntReport1.SQL4 = string.Format("{0}-{1}-1", cntSelectRokMiesiac.Rok, cntSelectRokMiesiac.Miesiac);
            //cntReport1.SQL5 = string.Format("{0}-{1}-31", cntSelectRokMiesiac.Rok, cntSelectRokMiesiac.Miesiac);
            //-----
            //string[] d = Tools.GetLineParams(ddlWeek.SelectedValue);
            //if (ddlNaDzien.Items.Count == 0)
            //{
            //    ddlNaDzien.DataBind();
            //}
            //string[] d = Tools.GetLineParams(ddlNaDzien.SelectedValue);
            //if (d.Length >= 2)
            //{
            //    cntReport1.SQL4 = d[1];
            //    cntReport1.SQL5 = d[2];
            //}
            //else
            //{
            //    cntReport1.SQL4 = "20140101";
            //    cntReport1.SQL5 = "20140131";
            //}
            //-----

            cntReport1.Prepare();
            cntReport1.GridDataBind();
        }
        //---------------------------------
        protected void ddlPM_SelectedIndexChanged(object sender, EventArgs e)
        {
            string kid = ddlPM.SelectedValue;
            PrepareReport();
        }

        protected void ddlNaDzien_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrepareReport();
        }

        protected void cntSqlSelectTabs1_SelectTab(object sender, EventArgs e)
        {
            PrepareReport();
        }
        //----------------------------------
        public int Mode
        {
            set { FMode = value; }
            get { return FMode; }
        }

        protected void cntSelectRokMiesiac_NextAll(object sender, EventArgs e)
        {
            cntSelectRokMiesiac SRM = (sender as cntSelectRokMiesiac);
            SRM.SelectNow();
        }

        protected void cntSelectRokMiesiac_BackAll(object sender, EventArgs e)
        {
            cntSelectRokMiesiac SRM = (sender as cntSelectRokMiesiac);

            DateTime? dt = db.getScalar<DateTime>("SELECT TOP 1 miesiac FROM ccLimity where isLast = 1 AND Limit IS NOT NULL ORDER BY miesiac");

            if (dt.HasValue)
            {
                SRM.Rok = dt.Value.Year;
                SRM.Miesiac = dt.Value.Month;
            }
            else
            {
                SRM.SelectNow();
            }
        }

        protected void cntSelectRokMiesiac_ValueChanged(object sender, EventArgs e)
        {
            PrepareReport();
        }
    }
}