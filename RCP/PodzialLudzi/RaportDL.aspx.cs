using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.Controls;

namespace HRRcp
{
    public partial class RaportDL : System.Web.UI.Page
    {
        const string title = "Ilości DL";

        //protected void Page_Init(object sender, EventArgs e)
        //{
        //    if (IsPostBack)
        //        Tools.CheckSessionExpired();
        //}

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        if (cntRaport.Prepare())
        //        {
        //            Tools.SetNoCache();
        //        }
        //        else
        //            App.ShowNoAccess(title, null);
        //    }
        //}

        //protected void Page_Error(object sender, System.EventArgs e)
        //{
        //    AppError.Show(title);
        //}




        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(GridView1, null, true, 20, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool adm = App.User.HasRight(AppUser.rPodzialLudziAdm);
                bool PM = App.User.HasRight(AppUser.rPodzialLudziPM);
                if (adm || PM)
                {
                    bool all = false;
                    //if (App.User.HasRight(AppUser.rRaportyAll))
                    //    all = Tools.StrToInt(Tools.GetStr(Request.QueryString["a"]), -1) == 1;   // przy okazji kontrola parametru
                    hidAll.Value = all ? "1" : "0";
                    string userId = App.User.Id;
                    hidUserId.Value = userId;
                    paKier.Visible = adm;

                    if (adm)
                    {
                        ddlPM.DataBind();
                        Tools.SelectItem(ddlPM, userId);
                    }
                    string kid = adm ? ddlPM.SelectedValue : App.User.Id;
                    hidKierId.Value = string.IsNullOrEmpty(kid) ? "-99" : kid;

                    DateTime eom = Tools.eom(DateTime.Today);
                    deOd.Date = Tools.bom(DateTime.Today);
                    deDo.Date = DateTime.Today > eom ? eom : DateTime.Today;

                    Tools.SetNoCache();
                }
                else App.ShowNoAccess(title, null);
            }
            ReportMaster rm = Page.Master as ReportMaster;
            rm.SetBtExcel(btExcel_Click);
        }

        protected void btWyszukaj_Click(object sender, EventArgs e)
        {
            // po prostu robi postback
        }

        protected void btClear_Click(object sender, EventArgs e)
        {
            foreach (Control cnt in paFilter.Controls)
            {
                if (cnt is TextBox) (cnt as TextBox).Text = null;
                else if (cnt is DateEdit) (cnt as DateEdit).Date = null;
                else if (cnt is DropDownList) Tools.SelectItem(cnt as DropDownList, null);
                else if (cnt is CheckBox) (cnt as CheckBox).Checked = false;
            }
        }

        protected void btExcel_Click(object sender, EventArgs e)
        {
            string filename = String.Format("Ilosc_DL_na_CC_{0}", Tools.DateToStrDb(DateTime.Now));
            Report.ExportCSV(filename, SqlDataSource1, null, null, true, false);
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {

        }

        protected void GridView1_PageIndexChanged(object sender, EventArgs e)
        {

        }
        //-----
        public int RowsCount
        {
            set { ViewState["gvrows"] = value; }
            get { return Tools.GetInt(ViewState["gvrows"], 0); }
        }

        private void SetPager(bool allonly)
        {
            string ln = ddlLines.SelectedValue;
            if (ln == "all")
            {
                //GridView1.PageSize = Int32.MaxValue;//RowsCount;  RowCount moze byc = 0
                int r = RowsCount;
                GridView1.PageSize = r == 0 ? Int32.MaxValue : RowsCount;  //RowCount moze byc = 0, MaxValue wywala sie z OutOfMenory ...
            }
            else if (!allonly)
            {
                GridView1.PageSize = Tools.StrToInt(ln, 25);
            }
        }

        protected void ddlLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPager(false);
        }

        private string GetRowsCount(int filter, int total)
        {
            return filter == total ? total.ToString() : String.Format("{0}/{1}", filter, total);
        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            RowsCount = e.AffectedRows;
            lbCount.Text = GetRowsCount(e.AffectedRows, e.AffectedRows);
            SetPager(true);
        }

        protected void ddlStr_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DropDownList ddl = sender as DropDownList;
            //string sel = ddl.SelectedValue;
            //string fld = ddl.DataValueField;
        }

        protected void ddlPM_SelectedIndexChanged(object sender, EventArgs e)
        {
            string kid = ddlPM.SelectedValue;
            hidKierId.Value = string.IsNullOrEmpty(kid) ? "-99" : kid;
        }

        protected void ddlCC_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void MoveDates1(int m)
        {
            if (deOd.IsValid)
                deOd.Date = ((DateTime)deOd.Date).bom().AddMonths(m);
            else if (String.IsNullOrEmpty(deOd.DateStr))
                deOd.Date = Tools.bom(DateTime.Today);

            if (deDo.IsValid)
                deDo.Date = ((DateTime)deDo.Date).eom().AddMonths(m);
            else if (String.IsNullOrEmpty(deDo.DateStr))
                deDo.Date = Tools.eom(DateTime.Today);
        }

        private void MoveDates(int m)
        {
            DateTime d;
            if (deOd.IsValid)
                d = ((DateTime)deOd.Date).AddMonths(m);
            else if (deDo.IsValid)
                d = ((DateTime)deDo.Date).AddMonths(m);
            else
                d = DateTime.Today.bom();
            deOd.Date = d.bom();
            deDo.Date = d.eom();
        }

        protected void btPrev_Click(object sender, EventArgs e)
        {
            MoveDates(-1);
        }

        protected void btNext_Click(object sender, EventArgs e)
        {
            MoveDates(1);
        }
    }
}
