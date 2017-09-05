using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal
{
    public partial class PracLogin : System.Web.UI.Page
    {
#if PORTAL
        public const string ses_okpage = "pokpage";
#else
        public const string ses_okpage = "okpage";
#endif

        protected void Page_PreInit(object sender, EventArgs e)
        {
#if PORTAL
#if KDR || PORTAL
            this.MasterPageFile = App.GetMasterPage();    
#else
            this.MasterPageFile = App.PortalMaster;
#endif
            //this.MasterPageFile = App.PortalMaster;    //setmasterpage
#endif
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string login = AppUser.GetLogin();
                hidUniqueId.Value = Tools.UniqueId();
                Tools.SetNoCache();
#if PORTAL
                lbInfo1.Text = "Witamy w Portalu Pracownika";
                lbInfo2.Text = "W Portalu dostępne są podstawowe informacje dotyczące Pracowników oraz narzędzia wykorzystywane w pracy.";
    #if SPX
                if (App.User.IsKiosk)
                    lbInfo3.Text = "Proszę zaloguj się podając swój numer ewidencyjny i hasło. Możesz też zalogować się umieszczając swoją kartę identyfikacyjną w czytniku poniżej ekranu. W przypadku problemów podczas logowania, skontaktuj się z działem HR.";
                else
                    lbInfo3.Text = "Proszę zaloguj się podając swój numer ewidencyjny i hasło.<br />W przypadku problemów podczas logowania, skontaktuj się z działem HR.";
                
                //lbLogin.Text = "Nr ewid.:"; 20161219
    #else
                lbInfo3.Text = "Proszę zaloguj się podając swój numer PESEL oraz hasło takie jak do Kwitka Płacowego.<br />W przypadku problemów podczas logowania, skontaktuj się z działem HR.";
        #if CO || PRON
                lbInfo3.Visible = false;
                lbInfo3b.Visible = true;
                tbLogin.Attributes["Placeholder"] = "Login";
                bool a = App.User.HasRight(AppUser.rPortalLoginAllowed);
                if (!a)
                {
                    tbLogin.Text = login;
                    lbLogin.Text = login;
                }
                tbLogin.Visible = a;
                lbLogin.Visible = !a;
             
                // na razie IQOR też jest włączony, później dać, że jak IQOR to num i 11 znaków
                tbLogin.MaxLength = 25;
                ftbLogin.Enabled = false;
        #endif
    #endif
#endif
            }
            tbLogin.ID = "tbLogin" + hidUniqueId.Value;
            tbPass.ID = "tbPass" + hidUniqueId.Value;
            ftbLogin.TargetControlID = tbLogin.ID;

            rfvLogin.ControlToValidate = tbLogin.ID;
            rfvPass.ControlToValidate = tbPass.ID;

            if (!IsPostBack)
            {
#if CO || PRON
                if (App.User.HasRight(AppUser.rPortalLoginAllowed))
                    tbLogin.Focus();
                else
                    tbPass.Focus();
#else
                tbLogin.Focus();
#endif
                //Page.SetFocus(btOk1337);
                Page.Form.DefaultButton = btOk1337.UniqueID;
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("PracLogin");
        }

        public static void Show()
        {
            //string pn = HttpContext.Current.Request.Url.AbsolutePath;
            //string pn = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath;
            string pn = HttpContext.Current.Request.Url.AbsoluteUri; // TOMEK JA NIE WIEM CZY TO BĘDZIE DZIAŁAć DOBRZE ALE DZIĘKI TEMU SĄ PARAMETRY TEŻ PRZEKAZYWANE

            //string pg = Path.GetFileName(pn);            
            HttpContext.Current.Session[ses_okpage] = pn;
            //App.Redirect(App.PracLoginForm);

            App.Redirect("~/Portal/PracLogin.aspx");
        }

        private bool x_CheckLogin(AppUser user, string pesel, string pass)
        {
            return true;
        }

        protected void btOk_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbLogin.Text.Trim()))
#if CO || PRON
                Tools.ShowMessage("Proszę podać swój login.");
#else
                Tools.ShowMessage("Proszę podać swój numer Pesel.");
#endif
            else if (String.IsNullOrEmpty(tbPass.Text.Trim()))
                Tools.ShowMessage("Proszę podać swoje hasło.");
            else
            {
                AppUser user = AppUser.CreateOrGetSession();
                if (!user._CheckPassLogin(tbLogin.Text, tbPass.Text, true))
                {
#if CO || PRON
                    string msg = "Wystąpił błąd podczas logowania użytkownika.<br><br>Proszę sprawdzić czy wpisany login i hasło są poprawne lub skontaktować się z działem HR.";
#else
                    string msg = "Wystąpił błąd podczas logowania użytkownika.<br><br>Proszę sprawdzić czy wpisany numer PESEL jest poprawny lub skontaktować się z działem HR.";
#endif
                    AppError.Show("Logowanie", msg,
                        //user.ImieNazwiskoOrLogin + " o numerze PESEL: " + tbLogin.Text + " nie został znalezniony w bazie.<br><br>Proszę sprawdzić czy wpisany numer PESEL jest poprawny lub skontaktować się z działem HR.", 
                            null, null,
                            AppError.btBack
                        );
                }
                else
                {
#if KWITEK
                    App.KwitekKadryId = user.NR_EW;
                    App.KwitekPracId = user.Id;
                    if (user.NeedPassChange())
                        App.Redirect(App.PracPassChangeForm);
                    else
                        Response.Redirect(App.KwitekForm);
#endif
#if PORTAL
                    App.KwitekKadryId = user.NR_EW;
                    App.KwitekPracId = user.Id;

                    if (user.NeedPassChange())
                        App.Redirect(App.PracPassChangeForm);
                    else
                    {
                        string okPage = Tools.GetStr(Session[ses_okpage], App.DefaultForm);
                        Session[ses_okpage] = null;
                        App.Redirect(okPage);
                    }
#endif
                }
            }
        }
    }
}