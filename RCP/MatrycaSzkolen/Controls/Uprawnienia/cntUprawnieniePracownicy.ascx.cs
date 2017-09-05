using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;

// 1 Uprawnienie -> Pracownicy

namespace HRRcp.MatrycaSzkolen.Controls.Uprawnienia
{
    public partial class cntUprawnieniePracownicy : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(GridView1, "table", false, 0, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TranslatePage();
            }
        }

        public void _Prepare(string kierId, string ccId, string uprId, bool showSub, string status, string awp, string data, string monit)
        {
            hidKierId.Value = kierId;// == "-99" ? null : kierId;
            hidCC.Value = ccId;
            hidUprId.Value = uprId;
            //hidStrList.Value = strList;
            hidShowSub.Value = showSub ? "1" : "0";
            hidStatus.Value = status;
            hidData.Value = data;
            hidMonit.Value = monit;
            Tools.SelectMenu(tabFilter, awp);
            Visible = true;
        }

        private void TranslatePage()
        {
            L.p(tabFilter);
            L.p(hidUnlimited);
        }

        public void Cancel()
        {
            Visible = false;
        }

        protected void GridView1Cmd_Click(object sender, EventArgs e)
        {
            /*
            string[] par = Tools.GetLineParams(GridView1CmdPar.Value);
            switch (par[0])
            {
                case "rank":
                    //if (Ocena != null) Ocena(this, EventArgs.Empty);
                    break;
            }
             */
        }

        public string CmdPar
        {
            get { return GridView1CmdPar.Value; }
        }

        protected void tabFilter_MenuItemClick(object sender, MenuEventArgs e)
        {

        }
        //--------------------------
        public int RowsCount
        {
            set { ViewState["gvrows"] = value; }
            get { return Tools.GetInt(ViewState["gvrows"], 0); }
        }

        private string GetRowsCount(int filter, int total)
        {
            return filter == total ? total.ToString() : String.Format("{0}/{1}", filter, total);
        }

        private int GetFilteredRowsCount()
        {
            DataView dv = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
            return dv.Count;
        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            RowsCount = e.AffectedRows;
            lbCount.Text = e.AffectedRows.ToString();
            //lbCount.Text = GetRowsCount(e.AffectedRows, e.AffectedRows);
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            /*
            //int cnt = ((DataView)gvUprawnienia.DataSource).Count;            
            //int cnt = gvUprawnienia.
            int cnt;
            if (String.IsNullOrEmpty(tbSearch.Text.Trim()))
                cnt = RowsCount;
            else
                cnt = GetFilteredRowsCount();
            lbCount.Text = GetRowsCount(cnt, RowsCount);
            */ 
        }
    }
}