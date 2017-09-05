using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards
{
    public partial class Raporty : System.Web.UI.Page
    {
        const string FormName = "Raporty i zestawienia";
        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Raport.F = true;   // na czas testów

                bool rap = App.User.IsRaporty;
                bool all = false; //App.User.IsAdmin;
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
                    App.ShowNoAccess(FormName, App.User);
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(null, FormName);
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
                        Raport.Show(id);
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
