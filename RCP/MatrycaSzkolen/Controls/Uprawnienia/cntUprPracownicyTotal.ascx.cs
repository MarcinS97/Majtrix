using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;

// wszystkie Uprawnienia i wszyscy pracownicy

namespace HRRcp.MatrycaSzkolen.Controls.Uprawnienia
{
    public partial class cntUprPracownicyTotal : System.Web.UI.UserControl
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

        public void _Prepare(string typ, string kwal, string kierId, string ccId, bool showSub, string status, string awp, string data, string monit)
        {
            hidKierId.Value = kierId;
            hidCC.Value = ccId;
            hidShowSub.Value = showSub ? "1" : "0";
            hidTyp.Value = typ;
            hidKwal.Value = kwal;
            //hidStrList.Value = strList;
            hidStatus.Value = status;
            hidData.Value = data;
            hidMonit.Value = monit;
            Tools.SelectMenu(tabFilter, awp);
            Visible = true;
        }

        public void Cancel()
        {
            Visible = false;
        }

        private void TranslatePage()
        {
            L.p(tabFilter);
            L.p(hidUnlimited);
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
        //----------------------------------
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