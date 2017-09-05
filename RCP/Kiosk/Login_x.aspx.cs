using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;
using HRRcp.App_Code;

namespace HRRcp.Kiosk
{
    public partial class Login_x : System.Web.UI.Page
    {
        string formKartaRCP = Tools.GetStr(ConfigurationSettings.AppSettings["kartaRCP"], "kartaRCP");

        protected void Page_Load(object sender, EventArgs e)
        {
            App.User._LoginAsUserId("-1");

            App.User.CheckPassLoginTest(App.User.NR_EW);
            App.KwitekKadryId = App.User.NR_EW;
            App.KwitekPracId = App.User.Id;
            AppUser.x_IdKarty = App.User.NR_EW;

            //App.Redirect(App.PortalStartForm);
            App.Redirect(App.DefaultForm);
















            if (!IsPostBack)
            {
                if (App._PrzypisywanieRCP)
                {
                    //----- -----
                    object oKartaRCP = Request.Form[formKartaRCP];
                    string kartaRCP = oKartaRCP == null ? null : oKartaRCP.ToString();
                    string pid = App.SelectedPracId;
                    //----- -----
                    Log.Info(Log.PRZYPISZRCP, "Przypisywanie RCP - Akceptacja/Odczyt karty",
                        String.Format("karta RCP: {0} PracId: {1}\n-------------------------\n", 
                                       kartaRCP, pid) +
                        GetPOSTStr());
                    //----- -----
                    App._KartaRCP = kartaRCP;
                    Response.Redirect(App.PrzypiszRCP);  // parametry ???
                }
                else
                {
                    //----- -----
                    Log.Info(Log.LOGIN, "Login.aspx", GetPOSTStr());
                    //----- -----

                    object oKartaRCP = Request.Form[formKartaRCP];
                    object oLogin = Request.Form["login"];
                    object oPass = Request.Form["pass"];
                    if (oKartaRCP != null)
                    {
                        if (AppUser.LoginKarta(oKartaRCP.ToString()))
                            Response.Redirect(App._StartForm);
                        else
                            //App.ShowPracNoAccess(true);
                            App.ShowNoAccess("KioskLogin", AppUser.CreateOrGetSession());

                    
                    }
                    else if (oLogin != null && oPass != null)
                    {
                        string login = oLogin.ToString();
                        string pass = oPass.ToString();
                        /*
    --where HashBytes('SHA1', Nazwisko + ' ' + Imie + CHAR(13) + convert(varchar, Id_Pracownicy)) = 
    --where LTRIM(RTRIM(Nazwisko)) + ' ' + LTRIM(RTRIM(Imie)) = '{0}' and convert(varchar, Id_Pracownicy) = '{1}'
                        */
                        DataRow dr = db.getDataRow(String.Format(@"
select * from Pracownicy where 
    (Nr_Ewid = '{0}' and Pass = master.dbo.fn_varbintohexstr(HashBytes('SHA1', Nr_Ewid + '{1}'))) 
    or
    ((case when APT = 0 then 
         ISNULL(Nr_Ewid, convert(varchar,Id_Pracownicy))
    else ISNULL(Nr_Ewid, '') + '/' + convert(varchar,Id_Pracownicy)
    end) = '{0}' and Pass <> '' and Pass = '{1}')"
                            // wejście dla administratora - przekazanie sha1 jako hasła; Nr_ewid jest nvarchar !!!
                            , login, pass));
                        if (dr != null)
                        {
                            string pid = db.getValue(dr, "Id_Pracownicy");
                            //if (AppUser.Login(pid))
                            if (App.User.LoginKartaByPracId(pid))
                                Response.Redirect(App._StartForm);
                            else
                                //App.ShowPracNoAccess(true);
                                App.ShowNoAccess("KioskLogin", AppUser.CreateOrGetSession());

                            /*
                            Tools.ShowError("Błąd autoryzacji użytkownika\\n" +
                                String.Format("Użytkownik: {0} [{1}] nie ma uprawnień zezwalających na dostęp do systemu.",
                                    Pracownik.NazwiskoImie, Pracownik.IdKarty));
                            */
                        }
                        else
                            //App.ShowPracNoAccess(false);
                            App.ShowNoAccess("KioskLogin", AppUser.CreateOrGetSession());
                        /*
                        Tools.ShowError("Błąd autoryzacji użytkownika\\n" +
                            "Użytkownik nie ma uprawnień zezwalających na dostęp do systemu.");
                        */
                    }

                    /*
                    if (s == null)
                        Log.Write("login", "no post params");
                    else
                        Log.Write("login", s.Replace("<br />", " "));
                    */
                    //if (IsReaderLogin)
                }
            }
        }

        public static string GetPOSTStr()
        {
            int cnt = HttpContext.Current.Request.Form.Count;
            string s = null;
            for (int i = 0; i < cnt; i++)
            {
                string key = HttpContext.Current.Request.Form.Keys[i].ToString();
                string value = HttpContext.Current.Request.Form[i];
                bool skip = key.Contains("ToolkitScriptManager") ||
                            key.Contains("hidLetters") ||
                            key.Contains("AdmLogControl") ||
                            key == "__EVENTTARGET" ||
                            key == "__EVENTARGUMENT" ||
                            key == "__LASTFOCUS" ||
                            key == "__VIEWSTATE" ||
                            key == "__VIEWSTATEENCRYPTED" ||
                            key == "__PREVIOUSPAGE" ||
                            key == "__EVENTVALIDATION"
                            ;
                bool pass = key == "pass" ||
                            key.Contains("tbPass");
                if (!skip)
                    s += String.Format("{0}: [{1}]\n", key, pass ? "********" : value);
            }
            return s;
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            //AppError.Show(App.KioskErrorForm, "Kiosk - Logowanie użytkownika");
            AppError.Show("KioskLogin", "Kiosk - Logowanie użytkownika");
        }
    }
}
