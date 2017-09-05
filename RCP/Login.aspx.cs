using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


                string login = AppUser.GetLogin();
                lbLogin.Text = login;
                hidUniqueId.Value = Tools.UniqueId();
                loginInfo.Visible = !String.IsNullOrEmpty(login);
            }
            tbLogin.ID = "tbLogin" + hidUniqueId.Value;
            tbPass.ID = "tbPass" + hidUniqueId.Value;
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Login");
        }

        protected void btOk_Click(object sender, EventArgs e)
        {
            /*aa
            if (String.IsNullOrEmpty(tbLogin.Text.Trim()))
                Tools.ShowMessage("Proszę podać swój login.");
            else if (String.IsNullOrEmpty(tbPESEL.Text.Trim()))
                Tools.ShowMessage("Proszę podać swój numer PESEL.");
            else
            {
                AppUser user = AppUser.CreateOrGetSession();
                user.Login = tbLogin.Text;
                if (!App.CheckFirstLogin(user, tbPESEL.Text))
                    AppError.Show("Logowanie", "Użytkownik " + user.ImieNazwiskoOrLogin + " o numerze PESEL: " + tbPESEL.Text + " nie został znalezniony w bazie.<br><br>Proszę sprawdzić czy wpisany numer PESEL jest poprawny lub skontaktować się z działem HR.", 
                            null, null, 
                            AppError.btBack
                        );
                else
                    Response.Redirect(App.StartForm);
            }
             */

            /* 20170314 VC */

            if (String.IsNullOrEmpty(tbLogin.Text.Trim()))
                Tools.ShowMessage("Proszę podać swój numer Pesel.");
            else if (String.IsNullOrEmpty(tbPass.Text.Trim()))
                Tools.ShowMessage("Proszę podać swoje hasło.");
            else
            {
                AppUser user = AppUser.CreateOrGetSession();
                if (!user._CheckPassLogin(tbLogin.Text, tbPass.Text, true))
                    AppError.Show("Logowanie", "Wystąpił błąd podczas logowania użytkownika.<br><br>Proszę sprawdzić czy wpisane hasło jest poprawne lub skontaktować się z działem HR.",
                        //user.ImieNazwiskoOrLogin + " o numerze PESEL: " + tbLogin.Text + " nie został znalezniony w bazie.<br><br>Proszę sprawdzić czy wpisany numer PESEL jest poprawny lub skontaktować się z działem HR.", 
                            null, null,
                            AppError.btBack
                        );
                else
                    App.Redirect(/*"Start.aspx"*/App.DefaultForm);
            }

        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            AppUser user = AppUser.CreateOrGetSession();
            user.Reload(true);
            Response.Redirect(App.DefaultForm);
        }
    }
}
