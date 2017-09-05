using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.RCP
{
    public partial class Raporty : System.Web.UI.Page
    {
        const string FormName = "Raporty i zestawienia";
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Raport.F = true;   // na czas testów

                bool rap = App.User.IsRaporty;
                bool all = App.User.IsMSAdmin;
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

        private void TranslatePage()
        {
            L.p(PageTitle1);
            L.p(lbAllTitle);
        }

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

            }
        }

        protected void cntSqlReportsAll_SelectedChanged(string id, string cmd, string par1)
        {
            if (!String.IsNullOrEmpty(id))
            {
                string url;
                if (Tools.IsUrl(cmd, out url))
                    App.Redirect(url);
            }
        }

        public static bool HasFormAccess
        {
            get 
            { 
                //return App.User.IsMSAdmin || App.User.IsRaporty; 
                return db.SqlMenuHasRights(5120, App.User);
            }
        }

        protected void cntSqlReportsMy_Rendered(object sender, EventArgs e)
        {
            bool empty = cntSqlReportsMy.IsEmpty();
            paMy.Visible = !empty;
        }

        protected void cntSqlReportsAll_Rendered(object sender, EventArgs e)
        {
            bool empty = cntSqlReportsAll.IsEmpty();
            paAll.Visible = !empty;
        }
    }
}
