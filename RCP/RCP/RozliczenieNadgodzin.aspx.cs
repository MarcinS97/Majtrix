using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.Controls.RozliczenieNadg;

namespace HRRcp.RCP
{
    public partial class RozliczenieNadgodzin : System.Web.UI.Page
    {
        const string FormName = "Rozliczenie nadgodzin";
        const string key  = "1587654322";
        const string salt = "4561234865";   // na razie, potem dac losowe i zapisywane w sesji


        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (App.User.HasAccess && App.User.HasRight(AppUser.rRozlNadg))
                {
                    bool ok = false;
                    string p = Tools.GetStr(Request.QueryString["p"]);
                    if (!String.IsNullOrEmpty(p))
                    {
                        Tools.SetNoCache();
                        string pracId, okresOd, okresDo, p4, mies, p6;
                        p = Report.DecryptQueryString(p, key, salt);
                        Tools.GetLineParams(p, out pracId, out okresOd, out okresDo, out p4, out mies, out p6);
                        ok = !String.IsNullOrEmpty(pracId) && !String.IsNullOrEmpty(okresOd) && !String.IsNullOrEmpty(okresDo);
                        if (ok)
                        {
                            //lbTitle.Text = String.Format("Rozliczenie nadgodzin - {0}", AppUser.GetNazwiskoImieNREW(pracId));
                            PageTitle.Title = String.Format("Rozliczenie nadgodzin - {0}", AppUser.GetNazwiskoImieNREW(pracId));
                            cntRozlNadg.Prepare(pracId, okresOd, okresDo, mies);
                        }
                    }
                    if (!ok) AppError.Show(FormName, "Niepoprawne parametry wywołania.");
                }
                else App.ShowNoAccess(FormName, App.User);
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(FormName);
        }

        public static void Show(string pracId, string okresOd, string okresDo)
        {
            string p = Tools.SetLineParams(4, pracId, okresOd, okresDo, Tools.DateToStr(DateTime.Today), null, null);  // data jako wypełniacz bo 0 dodawał
            string e = Report.EncryptQueryString(p, key, salt);
            //string d = Report.DecryptQueryString(e, key, salt);
            HttpContext.Current.Response.Redirect("~/RCP/RozliczenieNadgodzin.aspx?p=" + e);
        }
        //---------------------------------------------
    }
}
