using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.IO;
using System.Data;
using System.Configuration;

/*
MasterPage3.Master - Page_Init
RCP.Master - Page_Init
_strona_ - Page_Init
_strona_ - Page_Load
RCP.Master - Page_Load
MasterPage3.Master - Page_Load
*/

namespace HRRcp
{
    public partial class MasterPage3 : RcpMasterPage
    {
        protected void OnPageInit(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void OnPageError(object sender, System.EventArgs e)
        {
            string page = Path.GetFileName(Request.Path);   // powinna być wzięta nazwa formatki ...
            AppError.Show(page);
        }
        //-----
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!(Parent is ErrorForm))
            {
                Page.Init += new EventHandler(OnPageInit);
                Page.Error += new EventHandler(OnPageError);
            }
            //-----
            if (!IsPostBack)
            {
                Tools.InitSessionExpired();
                if (!App.User.HasAccess)
                {
                    App.ShowNoAccess();//App.FormName, App.User);
                }
                if (App.User.IsKomputer) App.Redirect("Login.aspx");
                /*else
                {
                    App.Redirect("Start.aspx");
                    App.Redirect("RCP/Harmonogram.aspx");
                }*/
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();
            if (!IsPostBack)
            {
                Info.SetHelp();
                //----- info baza danych, www, mailing -----
                string info, tooltip;
                if (Tools.IsTestDb(App.ProdDB, App.ProdWWW, out info, out tooltip))
                {
                    lbDbVerInfo.Visible = true;
                    lbDbVerInfo.Text = info;
                    lbDbVerInfo.ToolTip = tooltip;
                }
                //----- footer -----
                bool print = false;
                if (Parent is MasterPage3RCP)
                {
                    if (Parent.Parent is RaportF || Parent.Parent is RaportPDF)
                        print = true;
                }
                if (print)
                {
                    paPrintFooter.Visible = true;
                    lbPrintFooter.Text = String.Format("Wydrukowano z systemu {0} v. {1}", Tools.GetAppName(), Tools.GetAppVersion()); 
                    lbPrintTime.Text = Base.DateTimeToStr(DateTime.Now) + " " + App.User.ImieNazwisko;
                }
            }


            /* clients */
#if OKT
            css_keeeper.Visible = true;
#elif DBW   
            css_dbw.Visible = true;
#elif VICIM
            css_vicim.Visible = true;
#endif



        }

        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {
            if (!IsPostBack)
            {
            }
            base.OnPreRender(e);
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("RCP Master Page");
        }
        //-------------------------
        public static void PrepareMainMenuOnClick(Control menu)
        {
            foreach (Control cnt in menu.Controls)
                if (cnt is LinkButton)
                {
                    LinkButton lbt = cnt as LinkButton;
                    if (!String.IsNullOrEmpty(lbt.CommandArgument))
                        lbt.Click += new EventHandler(MenuClick);
                }
                else
                    PrepareMainMenuOnClick(cnt);
        }

        public static void RemoveDividers(Control menu)
        {

        }

        public static void MenuClick(object sender, EventArgs e)
        {
            string cmd = ((LinkButton)sender).CommandArgument;
            if (!String.IsNullOrEmpty(cmd))
            {
                if (cmd.StartsWith("app:"))
                {
                    string app = cmd.Substring(4);
                    if (!String.IsNullOrEmpty(app))
                    {
                        string url = Tools.GetStr(ConfigurationSettings.AppSettings[app], "/");
                        App.Redirect(url);
                    }
                }
                else
                    App.Redirect(cmd);
            }
        }
        //-------------------------
        public void ShowZastInfo(bool show, AppUser user)
        {
            if (show)
            {
                divZastInfo.Visible = true;
                lbZast.Text = user.ImieNazwisko;
            }
            else
            {
                divZastInfo.Visible = false;
                lbZast.Text = null;
            }
        }
        //-------------------------
        protected string GetAppName
        {
            get { return Tools.GetAppName(); }
        }

        protected string GetAppNameClass
        {
            get { return Tools.GetAppNameShort(); }
        }

        bool IsEmployee(AppUser user)
        {
            return !user.HasRight(AppUser.rMSBHP) && !user.HasRight(AppUser.rMSHR) && !user.HasRight(AppUser.rMSMeister) && !user.HasRight(AppUser.rMSTrener) && !user.IsMSAdmin && !user.IsKierownik;
        }

        protected void lbtEndZastepstwo_Click(object sender, EventArgs e)
        {
            App.LoginAsUser(null);
        }
    }
}
