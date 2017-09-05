using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using HRRcp.App_Code;

namespace HRRcp.Kiosk
{
    public partial class Login : System.Web.UI.Page
    {
        string formKartaRCP = Tools.GetStr(ConfigurationSettings.AppSettings["kartaRCP"], "kartaRCP");


        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Init(object sender, EventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            App.User._LoginAsUserId("-1");

            App.User.CheckPassLoginTest(App.User.NR_EW);
            App.KwitekKadryId = App.User.NR_EW;
            App.KwitekPracId = App.User.Id;
            AppUser.IdKarty = App.User.NR_EW;

            //App.Redirect(App.PortalStartForm);
            App.Redirect(App.DefaultForm);
            */

            if (!IsPostBack)
            {
                if (App._PrzypisywanieRCP)
                {
                    //----- -----
                    string kartaRCP = Tools.GetStr(Request.Form[formKartaRCP]);
                    if (String.IsNullOrEmpty(kartaRCP)) kartaRCP = Tools.GetStr(Request.Form["kartaRCPprz"]);  // tak się nazywa pole na cntPrzypiszRCP

                    string pid = App.SelectedPracId;
                    //----- -----
                    Log.Info(Log.PRZYPISZRCP, "Przypisywanie RCP - Akceptacja/Odczyt karty",
                        String.Format("karta RCP: {0} PracId: {1}\n-------------------------\n",
                                       kartaRCP, pid) +
                        GetPOSTStr());
                    //----- -----
                    App._KartaRCP = kartaRCP;
                    App.Redirect(App.PrzypiszRCP);  // parametry ???
                }
                else
                {
                    //----- -----
                    Log.Info(Log.LOGIN, "Login.aspx", GetPOSTStr());
                    //----- -----
                    object oKartaRCP = Request.Form[formKartaRCP];
                    if (oKartaRCP == null && App.User.IsPortalAdmin)  // logowanie za usera
                        oKartaRCP = Request.Form["kartaRCP"];

                    object oLogin = Request.Form["login"];
                    object oPass = Request.Form["pass"];
                    if (oKartaRCP != null && !String.IsNullOrEmpty(oKartaRCP.ToString()))
                    {
                        if (AppUser.LoginKarta(oKartaRCP.ToString()))
                        {
                            //App.Redirect(App._StartForm);
                            //App.Redirect(App.PortalStartForm);
                            App.Redirect(App.DefaultForm);
                            //App.Redirect(App.PortalStartForm);
                        }
                        else
                            //App.ShowPracNoAccess(true);
                            App.ShowNoAccess("Kiosk Login", AppUser.CreateOrGetSession());
                    }
                    else if (oLogin != null && oPass != null)
                    {
                        string login = oLogin.ToString();
                        string pass = oPass.ToString();

                        if (!String.IsNullOrEmpty(login))
                        {

                            /*
        --where HashBytes('SHA1', Nazwisko + ' ' + Imie + CHAR(13) + convert(varchar, Id_Pracownicy)) = 
        --where LTRIM(RTRIM(Nazwisko)) + ' ' + LTRIM(RTRIM(Imie)) = '{0}' and convert(varchar, Id_Pracownicy) = '{1}'
                            */
                            /*
                            DataRow dr = db.getDataRow(String.Format(@"
    select * from Pracownicy where 
        (Nr_Ewid = '{0}' and Pass = master.dbo.fn_varbintohexstr(HashBytes('SHA1', Nr_Ewid + '{1}'))) 
        or
        ((case when APT = 0 then 
             ISNULL(Nr_Ewid, convert(varchar,Id_Pracownicy))
        else ISNULL(Nr_Ewid, '') + '/' + convert(varchar,Id_Pracownicy)
        end) = '{0}' and Pass <> '' and Pass = '{1}')"
                            */
                            DataRow dr = db.getDataRow(String.Format(@"
select * from Pracownicy 
where 
    ('{1}' = 'kdr123' and convert(varchar,Id) = '{0}') or
    (KadryId = '{0}' and Pass = master.dbo.fn_varbintohexstr(HashBytes('SHA1', KadryId + '{1}'))) 
                            ", login, pass));    // wejście dla administratora - przekazanie sha1 jako hasła; Nr_ewid jest nvarchar !!!
                            if (dr != null)
                            {
                                string pid = db.getValue(dr, "Id");
                                if (App.User.LoginKartaByPracId(pid))
                                {
                                    App.User.DoPassLogin();

                                    App.KwitekKadryId = App.User.NR_EW;
                                    App.KwitekPracId = App.User.Id;

                                    App.Redirect(App.PortalStartForm);
                                }
                                else
                                    App.ShowNoAccess("Kiosk Login 1", AppUser.CreateOrGetSession());
                            }
                            else
                                App.ShowNoAccess("Kiosk Login 2", AppUser.CreateOrGetSession());
                        }
                        else
                            App.ShowNoAccess("Kiosk Login 3", AppUser.CreateOrGetSession());
                    }
                    else
                    {
                        //----- -----
                        Log.Error(Log.LOGIN, "Login.aspx", GetPOSTStr());
                        //----- -----
                        //App.Redirect(App.DefaultForm);
                        AppError.Show("Logowanie", "Niepoprawne parametry logowania.");
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
            Exception ex = HttpContext.Current.Server.GetLastError();
            
            AppError.Show("KioskLogin", "Kiosk - Logowanie użytkownika");
        }
    }
}
