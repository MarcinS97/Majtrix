using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.Controls.Reports;

using HRRcp.Scorecards.App_Code;
using AjaxControlToolkit;

namespace HRRcp.Scorecards
{
    public partial class ReportMaster : System.Web.UI.MasterPage
    //public partial class ReportMaster : RcpMasterPage
    {
        private const string defFilename = "report";

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
                    btnPDF.Visible = btnPDF2.Visible = Lic.PDF;
                    
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
                else if (cnt is cntReport2)
                {
                    filename = ((cntReport2)cnt).Title;
                }
                else if (cnt is HRRcp.Controls.EliteReports.cntReport)
                {
                    filename = ((HRRcp.Controls.EliteReports.cntReport)cnt).Title;
                }
            
            if (String.IsNullOrEmpty(filename))
            {
                filename = hidReportTitle.Value;
                if (String.IsNullOrEmpty(filename))
                    filename = defFilename;
            }
            
            HRRcp.App_Code.Report.ExportExcel(hidReport.Value, filename, null);
        }

        //JUAN

        public String GetFilename(Control cnt)
        {
            if (cnt is cntReport)
            {
                return ((cntReport)cnt).Title;
            }
            else if (cnt is cntReport2)
            {
                return ((cntReport2)cnt).Title;
            }
            else if (cnt is HRRcp.Controls.EliteReports.cntReport)
            {
                return ((HRRcp.Controls.EliteReports.cntReport)cnt).Title;
            }
            return null;
        }

        public IEnumerable<Control> GetAllControls(Control root)
        {
            foreach (Control control in root.Controls)
            {
                foreach (Control child in GetAllControls(control))
                {
                    yield return child;
                }
            }
            yield return root;
        }




        //------------------------------------------
        public static void SetExtVisible(Control cnt)  //przeszukuje dzieci
        {
            if (cnt is ExtenderControlBase)
            {
                //cnt.Visible = false;
                //cnt.Parent.Controls.Remove(cnt);
            }
            else
                foreach (Control cc in cnt.Controls)
                    SetExtVisible(cc);
        }


        public static void RemoveExtenders(Control cnt)
        {
            List<Control> list = new List<Control>();
            GetExtenders(cnt, ref list);
            foreach (Control c in list)
                c.Parent.Controls.Remove(c);
        }

        public static void GetExtenders(Control cnt, ref List<Control> list)  //przeszukuje dzieci
        {
            if (cnt is ExtenderControlBase)
                list.Add(cnt);
            else
                foreach (Control cc in cnt.Controls)
                    GetExtenders(cc, ref list);
        }






        protected void btnPDF_Click(object sender, EventArgs e)
        {
            List<Control> Controls = new List<Control>();
            foreach (Control Cnt in GetAllControls(ContentPlaceHolderReport))
            {
                if (Cnt is HRRcp.Controls.EliteReports.cntReport || Cnt is HRRcp.Controls.Reports.cntReport || Cnt is HRRcp.Controls.Reports.cntReport2 || Cnt is HRRcp.Controls.Reports.cntReport3 || 
                    (Cnt is WebControl && (((WebControl)Cnt).Attributes["pdf"] ?? "").ToLower() == "true" || Cnt.ID == "pdfwrapper" )                    
                    )
                {
                    if (Cnt.Visible)
                    {
                        RemoveExtenders(Cnt);
                        Controls.Add(Cnt);
                    }
                }
            }
            if (Controls.Count > 0)
            {
                PDF PDF = new PDF();

                List<PDF.ReplaceObject> ReplaceObjects = new List<PDF.ReplaceObject>();
                ReplaceObjects.Add(new PDF.ReplaceObject("_hap", "<img src='../Scorecards/images/Happy64px.png' style='border-width:0px; width: 16px;'>"));
                ReplaceObjects.Add(new PDF.ReplaceObject("_sad", "<img src='../Scorecards/images/Sad64px.png' style='border-width:0px; width: 16px;'>"));
                ReplaceObjects.Add(new PDF.ReplaceObject("_spook", "<img src='../Scorecards/images/Skeleton64px.png' style='border-width:0px; width: 16px;'>"));
                PDF.ReplaceObjects = ReplaceObjects;

                if (PDF.Download(Controls, Server, Response, Request, db.RemovePL(Tools.PrepareFilename(GetFilename(Controls[0])))) != 0)
                    Tools.ShowMessage("Błąd pobierania");
            }
            else
                Tools.ShowMessage("Błąd pobierania");
         }
    }
}
