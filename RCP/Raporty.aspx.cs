using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class RaportyForm : System.Web.UI.Page
    {

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

#if !RCP
        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool rap = App.User.IsRaporty;
                bool all = App.User.IsAdmin;
                if (rap || all)
                {
                    if (all)
                        paAll.Visible = true;
                    else
                        lbAllTitle.Visible = false;

                    Info.SetHelp("HRAPORTY2");
                    TranslatePage();
                }
                else
                    App.ShowNoAccess("Raporty", App.User);
            }
        }

        protected void x_Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(null, L.p("Raporty i zestawienia"));
        }

        public static bool HasFormAccess
        {
            get { return App.User.HasRight(AppUser.rRaporty2); }
        }
        //----------------------
        private void TranslatePage()
        {
            L.p(PageTitle1);
            L.p(lbAllTitle);
        }
        //----------------------
        protected void btClick(object sender, EventArgs e)
        {
            //AppError.Show(null, L.p("Brak danych do wykonania zestawienia."), AppError.btBack);
            AppError.Show(L.p("Brak danych do wykonania zestawienia."), AppError.btBack);
        }
        //-----------------------

        protected void cntSqlReports_SelectedChanged(string id, string cmd, string par1)
        {
            if (!String.IsNullOrEmpty(id))
            {
                //Session["lmenugrp"] = String.IsNullOrEmpty(par1) ? null : par1;

                string url;
                if (Tools.IsUrl(cmd, out url))
                    App.Redirect(url);
                else
                    if (!String.IsNullOrEmpty(id))
                    {
                        RaportF.Show(id);
                    }


                /*
                if (cmd.ToLower().StartsWith("url:"))
                {
                    string url = cmd.Substring(4);
                    App.Redirect(url);
                }
                 */
            }
        }

        protected void cntSqlReportsAll_SelectedChanged(string id, string cmd, string par1)
        {
            if (!String.IsNullOrEmpty(id))
            {
                //Session["lmenugrp"] = String.IsNullOrEmpty(par1) ? null : par1;

                string url;
                if (Tools.IsUrl(cmd, out url))
                    App.Redirect(url);
                else
                    if (!String.IsNullOrEmpty(id))
                    {
                        RaportF.Show(id);
                    }


                /*
                if (cmd.ToLower().StartsWith("url:"))
                {
                    string url = cmd.Substring(4);
                    App.Redirect(url);
                }
                 */
            }
        }
    }
}
