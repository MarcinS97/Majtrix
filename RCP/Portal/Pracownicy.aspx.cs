using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;
using HRRcp.Portal.Controls;
 
namespace HRRcp.Portal
{
    public partial class PortalPracownicy : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Prepare();
                cntSqlContent1.cntLine.NoDataInfo = "wybierz pracownika...";
            }
        }

        private void Prepare()
        {
            DataSet ds = db.getDataSet(String.Format(@"
declare @rootId int
set @rootId = {0}
select 
--'-1|-1|-1' as Id, 
'0|0|null' as Id, 
'wybierz ...' as Pracownik, null as SortPath
union
select KadryId + '|' + convert(varchar, IdPracownika) as Id,
--select KadryId + '|' + convert(varchar, IdPracownika) + '|' + ISNULL(KadryId2, 'null') as Id,
replicate('&nbsp;', (Hlevel - 1) * 4) +
Nazwisko + ' ' + Imie + ' (' + ISNULL(KadryId, '???') + ')' as Pracownik, SortPath
from dbo.fn_GetTree(@rootId, GETDATE())    
order by SortPath", App.User.Id));

            foreach (DataRow dr in db.getRows(ds))
                dr[1] = HttpUtility.HtmlDecode(dr[1].ToString());

            Tools.BindData(ddlPracownicy, ds, "Pracownik", "Id", false, null);


            string grupa = Tools.GetStr(Request.QueryString["p1"]);
            if (!String.IsNullOrEmpty(grupa))
                if (grupa.IsAny("OCENAK"))
                    cntSqlContent1.Grupa = grupa;
        }

        protected void ddlPracownicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cntPlanUrlopow.Prepare(ddlKierownicy4.SelectedValue);
            cntSqlContent1.ReloadCurrent();
        }

        protected void cntSqlContent1_SelectTab(object sender, EventArgs e)
        {
            string p1, p2, p3;
            Tools.GetLineParams(ddlPracownicy.SelectedValue, out p1, out p2, out p3);
            if (p2 != "0")
                p3 = db.getScalar("select ISNULL(KadryId2,'null') from Pracownicy where Id = " + p2);
#if PRON
            p3 = p2;
#endif
            cntSqlContent2 sc = (cntSqlContent2)sender;
            switch (sc.Typ)
            {
                case cntSqlContent2.moLines:
                    sc.cntLine.SQL1 = p1;
                    sc.cntLine.SQL2 = p2;
                    sc.cntLine.SQL3 = p3;
                    break;
                case cntSqlContent2.moScreen:
                    sc.cntScreen.SQL1 = p1;
                    sc.cntScreen.SQL2 = p2;
                    sc.cntScreen.SQL3 = p3;
                    break;
            }

            string s = sc.Tabs.SelectedValue;
            if (!String.IsNullOrEmpty(s) && sc.Tabs.Items.Count > 0)
            {
                bool t1 = s == sc.Tabs.Items[0].Value;
                paPracownicy.Visible = !t1;
                if (t1)
                    Tools.AddClass(sc.Panel, "noTopPadding");
                else
                    Tools.RemoveClass(sc.Panel, "noTopPadding");
            }
            else
                paPracownicy.Visible = true;
        }
    }
}


//--"\xA0"
