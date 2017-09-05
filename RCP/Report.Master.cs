using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.Controls.Reports;

namespace HRRcp
{
    public partial class ReportMaster : System.Web.UI.MasterPage
    //public partial class ReportMaster : RcpMasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Tools.SetNoCache();

                AppUser user = App.User;    //AppUser.CreateOrGetSession();
                if (user.IsRaporty)
                {
                    Info.SetHelp(Info.HELP_ADMRAPORTY);

                    /*
                    Prepare();

                    string rep = (string)HttpContext.Current.Request.QueryString["p"];
                    if (!String.IsNullOrEmpty(rep))
                    {
                        Tools.SelectMenu(tabWyniki, "1");
                        SelectTab();
                    }
                    else
                    {
                        Tools.SelectMenu(tabWyniki, "0");
                        //Tools.SelectMenuFromSession(tabWyniki, active_tab);
                        SelectTab();
                    }
                    */
                    
                    lbPrintTime.Text = Base.DateTimeToStr(DateTime.Now) + " " + user.ImieNazwisko;
                    lbPrintVersion.Text = Tools.GetAppVersion();
                    Page.Error +=new EventHandler(Page_Error);
                }
                else
                    App.ShowNoAccess("Raporty", user);
            }
        }

        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {
            /*
            switch (tabWyniki.SelectedValue)
            {
                case "0":
                    ((MasterPage)Master).SetWideJs(2);  //wide2
                    break;
                default:
                    ((MasterPage)Master).SetWideJs(1);  //wide
                    break;
            }
             */ 
            base.OnPreRender(e);
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Raporty2", AppError.btBack);
        }






        protected void TreeView1_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            if (e.Node.NavigateUrl.ToString().IndexOf("#") > 0) // Or use string.Contains()
            {
                e.Node.NavigateUrl = e.Node.NavigateUrl.Remove(1, 1);
                e.Node.Target = "_blank";
            }
        }
        //-----------------------------------------
        public void SetBtExcel(EventHandler ev)
        {
            btRepExcel1.OnClientClick = null;
            btRepExcel2.OnClientClick = null;

            btRepExcel1.Click -= btExcel_Click;
            btRepExcel1.Click += new EventHandler(ev);
            btRepExcel2.Click -= btExcel_Click;
            btRepExcel2.Click += new EventHandler(ev);
        }

        //-----------------------------------------
        protected void btExcel_Click(object sender, EventArgs e)
        {
            int ccc = Controls.Count;
            
            string filename = null;
            foreach (Control cnt in ContentPlaceHolderReport.Controls)
                if (cnt is cntReport)
                {
                    filename = ((cntReport)cnt).Title;
                    break;
                }
            
            if (String.IsNullOrEmpty(filename))
            {
                filename = hidReportTitle.Value;
                if (String.IsNullOrEmpty(filename))
                    //filename = "report.csv";
                    filename = "report";
            }
            
            App_Code.Report.ExportExcel(hidReport.Value, filename, null);
        }
    }
}
