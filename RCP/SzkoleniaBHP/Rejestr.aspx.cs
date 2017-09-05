using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;
//using HRRcp.Uprawnienia.Controls;

namespace HRRcp.SzkoleniaBHP
{
    public partial class Rejestr : System.Web.UI.Page
    {
        const string FormName = "Szkolenia BHP";
        const string defTyp = "1";

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

                    //string typ = Tools.GetStr(Request.QueryString["t"], defTyp);
                    string typ = "1024"; // BHP

                    int t = Tools.StrToInt(typ, -1);
                    if (t != -1)
                    {
                        typ = t.ToString(); // na wszelki wypadek...
                        DataRow dr = db.getDataRow("select * from UprawnieniaTypy where Id = " + typ);
                        if (dr == null)
                        {
                            typ = defTyp;
                            SetTitle(L.p("Uprawnienia produkcyjne"), "");
                        }
                        else
                        {
                            SetTitle(db.getValue(dr, L.Lang == L.lngPL ? "TypNazwa" : "TypNazwaEN"), "");
                        }
                        cntUprawnienia1.Typ = typ;


                        Info.SetHelp("HSZKOLBHP");
                        /*RCP
                        switch (typ)
                        {
                            case cntUprawnienia.utPassSpaw: // "4":
                                Info.SetHelp(Info.HELP_PASSSPAW);
                                break;
                            case cntUprawnienia.utSzkolenia:
                                Info.SetHelp(Info.HELP_SZKOLENIA);
                                break;
                            case cntUprawnienia.utStatusSamokontroli:
                                Info.SetHelp(Info.HELP_STATSAM);
                                break;
                            default:
                                Info.SetHelp(Info.HELP_UPRAWNIENIA);
                                break;
                        }
                         */ 
                    }
                    else AppError.Show(FormName, L.p("Niepoprawne parametry wywołania."));
                }
                else App.ShowNoAccess(FormName, App.User);
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(FormName);
        }

        /*
                switch (typ)
                {
                    default:
                    case "1": SetTitle(L.p("Uprawnienia produkcyjne"), ""); typ = "1"; break;         // na razie na sztywno ...
                    case "2": SetTitle(L.p("Uprawnienia nieprodukcyjne"), ""); break;
                    case "4": SetTitle(L.p("Uprawnienia spawaczy"), ""); break;
                }
         */

        public static bool HasFormAccess
        {
            get { return Lic.SzkoleniaBHP && App.User.HasRight(AppUser.rSzkoleniaBHP); }
        }
        
        private void SetTitle(string title, string icon)
        {
            //PageTitle1.Title = title;
            //if (!String.IsNullOrEmpty(icon))
            //    PageTitle1.Ico = icon;

            cntUprawnienia1.Title = title;
        }
    }
}
