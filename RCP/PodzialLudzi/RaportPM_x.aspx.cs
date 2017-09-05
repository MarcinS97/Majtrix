using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class RaportPM_x : System.Web.UI.Page
    {
        int FMode = moKier;
        const int moKier = 0;
        const int moAdm  = 1;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool adm = App.User.HasRight(AppUser.rPodzialLudziAdm);
                if (App.User.HasRight(AppUser.rPodzialLudzi) || adm)
                {
                    Tools.SetNoCache();
                    string d1 = Tools.GetStr(Request.QueryString["p1"]);
                    string d2 = Tools.GetStr(Request.QueryString["p2"]);

                    paKier.Visible = adm;
                    if (adm) paKier.DataBind();
                    bool all = App.User.HasRight(AppUser.rRepPodzialCCAll);
                    FillTabs(all || adm ? ALL : App.User.Id);  // tu się wywoła SelectedChanged
                    PrepareReport();
                }
                else
                {
                    App.ShowNoAccess("Podział Ludzi", App.User);
                }                
            }
        }
        //---------------------------------
        const string ALL = "-99";

        private void FillTabs(string kid)
        {
            cntSqlTabs1.Prepare(String.Format(@"
declare @kid int
set @kid = {0}
select K.Nazwa + '|' + case when W.Id is not null or @kid = -99 then '$$$' else '' end as Value, K.Nazwa as Text, 1 as Sort, K.Lp 
from Kody K
left join ccPrawa R on R.IdCC = 0 and R.CC = K.Nazwa and R.UserId = @kid
left join ccPrawa W on W.IdCC = 0 and W.CC = K.Nazwa + '$$$' and W.UserId = @kid
where K.Typ = 'PRACGRUPA'
and (@kid = -99 or R.Id is not null)
order by Sort, Lp, Text
                ", 
                kid), false);

            //if (cntSqlTabs1.Tabs.Items.Count > 1)
            //    cntSqlTabs1.Tabs.Items.Add(new MenuItem("Wszystkie klasyfikacje", "-99"));
            
            if (cntSqlTabs1.Tabs.Items.Count == 0)
                cntReport1.Title3 = "Brak uprawnień do klasyfikacji";
            else
                cntReport1.Title3 = kid == ALL ? "Cała klasyfikacja" : ("Klasyfikacja: " + App.GetccPrawaList2(kid));            
        }


        /*
        private void FillTabs(string kid)
        {
            cntSqlTabs1.Prepare(String.Format(@"
select K.Nazwa as Value, K.Nazwa as Text, 1 as Sort, K.Lp 
from Kody K
left join ccPrawa R on R.IdCC = 0 and R.CC = K.Nazwa and R.UserId = {0}
where Typ = 'PRACGRUPA'
and ({0} = -99 or R.Id is not null)
{1}
order by Sort, Lp, Text
                ", 
                kid, 
                kid == ALL ? @"
union all 
select '-99' as Value, 'Wszystkie klasyfikacje' as Text, 2 as Sort, null as Lp 
                " : null), false);  // bez triggera 
            
            if (cntSqlTabs1.Tabs.Items.Count == 0)
                cntReport1.Title3 = "Brak uprawnień do klasyfikacji";
            else
                cntReport1.Title3 = kid == ALL ? "Cała klasyfikacja" : ("Klasyfikacja: " + App.GetccPrawaList2(kid));            
        }
         */

        /*
         * SQL1 - kier Id
         * SQL2 - 
         * SQL3 - Class/TypImport
         * SQL4 - data od
         * SQL5 - data do
         */

        private void PrepareReport()
        {
            bool adm = App.User.HasRight(AppUser.rPodzialLudziAdm);
            bool all = App.User.HasRight(AppUser.rRepPodzialCCAll);
            bool kwoty = App.User.HasRight(AppUser.rRepPodzialCCKwoty);


            if (adm)
                cntReport1.SQL1 = ddlPM.SelectedValue;
            else
                cntReport1.SQL1 = all ? ALL : App.User.Id;
            //-----
            if (kwoty)
                cntReport1.SQL2 = cntReport1.P2;
            else
                cntReport1.SQL2 = cntReport1.P1;
            //-----
            cntReport1.SQL3 = cntSqlTabs1.SelectedValue;            
            //-----            
            //string[] d = Tools.GetLineParams(ddlWeek.SelectedValue);
            if (ddlNaDzien.Items.Count == 0)
            {
                ddlNaDzien.DataBind();
            }
            string[] d = Tools.GetLineParams(ddlNaDzien.SelectedValue);
            if (d.Length >= 2)
            {
                cntReport1.SQL4 = d[1];
                cntReport1.SQL5 = d[2];
            }
            else
            {
                cntReport1.SQL4 = "20140101";
                cntReport1.SQL5 = "20140131";
            }
            //-----
            string klas = cntSqlTabs1.Tabs.SelectedValue;
            if (klas == ALL)
                klas = "Wszystkie klasyfikacje";
            cntReport1.Title = String.Format("Podział Ludzi - {0}", klas);
            cntReport1.Prepare();
            cntReport1.GridDataBind();
        }
        //---------------------------------
        protected void ddlPM_SelectedIndexChanged(object sender, EventArgs e)
        {
            string kid = ddlPM.SelectedValue;
            FillTabs(kid);
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
    }
}
