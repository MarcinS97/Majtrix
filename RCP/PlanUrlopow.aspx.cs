using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
//using HRRcp.Controls.PlanUrlopow;

namespace HRRcp
{
    public partial class PlanUrlopow : System.Web.UI.Page
    {
        const string FormName = "Plan Urlopów";
        const string key  = "6578414901";
        const string salt = "4587752215";   // na razie, potem dac losowe i zapisywane w sesji


        protected void Page_PreInit(object sender, EventArgs e)
        {
#if PORTAL
            this.MasterPageFile = App.GetMasterPage();
#endif
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (App.User.HasAccess)
                {
                    Tools.SetNoCache();
                    bool ok = false;
                    string pracId = null;
                    string data = null;

                    string p = Tools.GetStr(Request.QueryString["p"]);
                    if (!String.IsNullOrEmpty(p))
                    {
                        if (App.User.HasRight(AppUser.rPlanUrlopow))
                        {
                            string p3, p4, p5, p6;
                            p = Report.DecryptQueryString(p, key, salt);
                            Tools.GetLineParams(p, out pracId, out data, out p3, out p4, out p5, out p6);
                            ok = !String.IsNullOrEmpty(pracId) && !String.IsNullOrEmpty(data);
                        }
                        else App.ShowNoAccess(FormName, App.User);
                    }
#if PORTAL
                    else
                    {
                        //if (App.User.IsPassLogged)
                        //pracId = App.KwitekPracId;
                        pracId = App.User.Id;
                        data = DateTime.Today.Year.ToString();
                        ok = true;
                        cntPlanRoczny.Portal = true;
                    }
#endif
                    if (ok)
                    {
                        //lbTitle.Text = String.Format("Plan urlopów na rok {1} - {0}", AppUser.GetNazwiskoImieNREW(pracId), data);
                        lbTitle.Text = String.Format("Plan urlopów na rok {0}", data);
                        cntPlanRoczny.Prepare(data, pracId);
                    }
                    else AppError.Show(FormName, "Niepoprawne parametry wywołania.");
                }
                else App.ShowNoAccess(FormName, App.User);
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(FormName);
        }

        public static void Show(string pracId, string data)
        {
            string p = Tools.SetLineParams(3, pracId, data.Substring(0, 4), "", null, null, null);   // jak za krótki string to dołoży #zera na końcu - dlatego pusty parametr, zeby nie trzeba było ich odcinać
            string e = Report.EncryptQueryString(p, key, salt);
            //string d = Report.DecryptQueryString(e, key, salt);
            HttpContext.Current.Response.Redirect("~/PlanUrlopow.aspx?p=" + e);
        }
        //---------------------------------------------
    }
}
