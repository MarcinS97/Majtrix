using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Web.Security;

namespace HRRcp.Portal
{
    public partial class PracPassChangeForm : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
#if PORTAL
            //this.MasterPageFile = App.PortalMaster;
            this.MasterPageFile = App.GetMasterPage();
#endif
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string login = AppUser.GetLogin();
                hidUniqueId.Value = Tools.UniqueId();
                Tools.SetNoCache();

                AppUser user = AppUser.CreateOrGetSession();
                bool f = user.NeedPassChange();
                ltForce.Visible = f;
                ltComplexity.Text = AppUser.GatPassComplexityMsg(App.PassType);
                btBack.Visible = !f;
            }
            tbPass0.ID = "tbPass0" + hidUniqueId.Value;
            tbPass.ID = "tbPass" + hidUniqueId.Value;

            rfvLogin.ControlToValidate = tbPass0.ID;
            rfvPass.ControlToValidate = tbPass.ID;
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("PracPassChange");
        }

        private bool x_CheckLogin(AppUser user, string pesel, string pass)
        {
            return true;
        }

        protected void btOk_Click(object sender, EventArgs e)
        {
            AppUser user = AppUser.CreateOrGetSession();
            string newpass = tbPass0.Text;
            if(!IsOldPasswordCorrect(user))
                Tools.ShowMessage("Obecne hasło nie zgadza się.");
            else if (newpass != tbPass.Text)
                Tools.ShowMessage("Wprowadzone hasła nie są zgodne.");
            else
            {
                if (AppUser.CheckPass(newpass, App.PassType))
                {
                    if (AppUser.UpdatePass(user.Id, newpass, true, db.conP))
                    {
                        Log.Info(Log.CHANGEPASS, "Zmiana hasła", null);
                        App.User.Reload(true);
                        Tools.ShowMessage2("Hasło zostało zmienione.", btGo);
                        NextPage = Tools.GetStr(Session[PracLoginForm.ses_okpage], App.PortalPracForm);
#if KWITEK
                        Response.Redirect(App.KwitekForm);
#elif PORTAL
                        //Response.Redirect("~/" + App.PortalPracForm);
                        
                        string okPage = Tools.GetStr(Session[PracLoginForm.ses_okpage], App.PortalPracForm);
                        Session[PracLoginForm.ses_okpage] = null;
                        App.Redirect(okPage);
                        
#else
#endif
                    }
                    else
                    {
                        Tools.ShowMessage("Wystąpił błąd podczas zmiany hasła.");
                        Log.Error(Log.PASSRESET, "Wystąpił błąd podczas zmiany hasła", user.NazwiskoImie + ' ' + user.NR_EW);
                    }
                }
            }
        }

        private bool IsOldPasswordCorrect(AppUser user)
        {
            string oldPass = db.Select.Scalar(db.conP ,dsGetOldPassword.SelectCommand, user.Id);
            var aaa = db.selectScalar(dsGetOldPassword.SelectCommand, user.Id);
            string inputOldPass = tbOldPassword.Text;
            string newPassHash = FormsAuthentication.HashPasswordForStoringInConfigFile(inputOldPass, AppUser.hashMethod);
            return newPassHash == oldPass;
        }

        private string NextPage
        {
            set { ViewState["npage"] = value; }
            get { return Tools.GetStr(ViewState["npage"]); }
        }

        protected void btGo_Click(object sender, EventArgs e)
        {
            string okPage = NextPage;
            Session[PracLoginForm.ses_okpage] = null;
            App.Redirect(okPage);
        }

        /*
        protected void btOk_Click(object sender, EventArgs e)
        {
            if (tbPass0.Text != tbPass.Text)
                Tools.ShowMessage("Wprowadzone hasła nie są zgodne.");
            else
            {
                AppUser user = AppUser.CreateOrGetSession();
                if (AppUser.UpdatePass(user.Id, tbPass0.Text, true))
                {
                    Tools.ShowMessage("Hasło zostało zmienione.");
                
                    if (App.IsApp(App._Kwitek))
                        Response.Redirect(App.KwitekForm);
                    else if (App.IsApp(App._Portal))
                    {
                        //Response.Redirect("~/" + App.PortalPracForm);
                        string okPage = Tools.GetStr(Session[PracLoginForm.ses_okpage], App.PortalPracForm);
                        Session[PracLoginForm.ses_okpage] = null;
                        App.Redirect(okPage);
                    }
                }
                else
                {
                    Tools.ShowMessage("Wystąpił błąd podczas zmiany hasła.");
                    Log.Error(Log.PASSRESET, "Wystąpił błąd podczas zmiany hasła", user.NazwiskoImie + ' ' + user.NR_EW);
                }
            }
        }
        */
    }
}
