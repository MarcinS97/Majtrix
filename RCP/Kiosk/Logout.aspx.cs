using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (App._PrzypisywanieRCP)
            {
                //----- -----
                Log.Info2(Log.PRZYPISZRCP, "Przypisywanie RCP - Zdjęcie karty z czytnika", HRRcp.Kiosk.Login.GetPOSTStr());
                //----- -----
                //Tools.Back();
                App.Redirect(App.PrzypiszRCP);  // parametry ???
                
                //HttpContext.Current.Response.Redirect("https://app01.kdrs.pl/spxportal/kiosk/przypiszrcp.aspx");  // parametry ???
            }
            else
            {
                //----- -----
                Log.Info2(Log.LOGIN, "Logout.aspx -----", HRRcp.Kiosk.Login.GetPOSTStr());
                //----- -----

                //Pracownik.Logout();
                AppUser.x_Logout();
                //Session["logout_aspx"] = "1";     // troche plomba admin - po wylogowaniu wchodzi na Default dla kiosku

                AppUser user = AppUser.CreateOrGetSession();    // Logout nie ma masterpage
                user.Reload(true);                              // po zalogowaniu z kiosku na admina zostawały śmieci

                /*zzz
                if (user.IsAdmin || user.HasRight(AppUser.rLogout))
                    AdminLogout = true;             // admin został wylogowany - będziemy przechodzić na 'Kiosk.StartForm' (Default.aspx)
                 */ 
                App.Redirect(App.DefaultForm);
            }
        }

        public static bool AdminLogout
        {
            get 
            {
                object o = HttpContext.Current.Session["logout_aspx"];
                return o != null && o.ToString() == "1"; 
            }
            set { HttpContext.Current.Session["logout_aspx"] = value ? "1" : "0"; }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            //AppError.Show(App.KioskErrorForm, "Kiosk - Wylogowanie użytkownika");
            AppError.Show("KioskLogout", "Kiosk - Wylogowanie użytkownika");
        }
    }
}
