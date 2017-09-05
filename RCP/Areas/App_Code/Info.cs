using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Web.UI.HtmlControls;

using System.Data;
using System.Data.SqlClient;
using HRRcp.Controls;

namespace HRRcp.App_Code
{
    public class Info
    {
        public const string sesInfoTyp = "info_typ";
        public const string sesInfoBack = "info_back";
        public static string infoForm = "InfoForm.aspx";

        // UWAGA !!! ograniczenie długości do 10 znaków !!!, pierwsza litera - prefiks grupy H I L?
        //----- HELP ------
        public const string HELP_ADMMANAGER = "HAMANAGER";   // na przyszłość
        public const string HELP_ADMSTRUKTURA = "HASTRUCT";
        public const string HELP_ADMPRACOWNICY = "HAPRAC";
        public const string HELP_ADMSTREFY = "HASTREFY";
        public const string HELP_ADMDZIALY = "HADZIALY";
        public const string HELP_ADMZMIANY = "HAZMIANY";
        public const string HELP_ADMPARAMETRY = "HAPARAMS";

        public const string HELP_ADMOKRESY          = "HAOKRESY";
        public const string HELP_ADMSTAWKI          = "HASTAWKI";
        public const string HELP_ADMALGORYTMY       = "HAALG";
        public const string HELP_ADMKODYABS         = "HAKODYABS";
        public const string HELP_ADMKIERPARAMS      = "HAKIERPAR";
        public const string HELP_ADMMPK             = "HAMPK";
        public const string HELP_ADMABSDL           = "HAABSDL";
        public const string HELP_ADMPUADM           = "HAPUADM";

        public const string HELP_ADMPLANPRACY       = "HAPP";
        public const string HELP_ADMACCEPTPP        = "HAACCEPTPP";
        public const string HELP_ADMRAPORTY         = "HARAPORTY";

        public const string HELP_ADMLOG             = "HALOG";
        public const string HELP_ADMTEKSTY          = "HATEKSTY";
        public const string HELP_ADMMAILE           = "HAMAILE";
        public const string HELP_ADMSYSPARAMS       = "HASYSPAR";
        public const string HELP_ADMZASTEPSTWA      = "HAZASTEP";
        public const string HELP_ADMODDELEGOWANIA   = "HADELEG";
        public const string HELP_ADMWNIOSKIURLOPOWE = "HAWNURLOP";
        public const string HELP_ADMROZLNADG        = "HAROZLNADG";
        public const string HELP_ADMPLANURLOP       = "HAPLURLOP";
                                                
        public const string HELP_KIERPLANPRACY      = "HKPP";
        public const string HELP_KIERACCPP          = "HKACCPP";
        public const string HELP_KIERROZLNADG       = "HKROZLNADG";

        public const string HELP_KIERPRACOWNICY     = "HKPRAC";
        public const string HELP_KIERPARAMS         = "HKPARAMS";
        public const string HELP_KIERRAPORTY        = "HKRAPORTY";

        public const string HELP_ZASTEPSTWA         = "HZASTEP";
        public const string HELP_ODDELEGOWANIA      = "HDELEG";
        public const string HELP_WNIOSKIURLOPOWE    = "HWNURLOP";

        public const string HELP_START              = "HSTART";

        public const string REGULAMIN = "REGULAMIN";
        public const string PANELPRAC = "PANPRAC";
        
        public const string HELP_INFO = "HINFO";
        public const string HELP_ERR  = "HERR";
        public const string NOHELP    = "HNOHELP";        // help domyslny jak nie znajdzie
        
        //----- INFO ----- informacje - otwierają sie w masterpage
        //public const string INFO_NOTCHECKED  = "INOTCHECK";     // pracownik nie jest weryfikowany
        //public const string INFO_PACCEPT = "IPACCEPT";          // po akceptacji ankiety przez P
        //----------------------------------------------------
        public static string Info1
        {
            set { HttpContext.Current.Session["infoFormI1"] = value; }
            get { return Tools.GetStr(HttpContext.Current.Session["infoFormI1"]); }
        }

        public static string InfoEx
        {
            set { HttpContext.Current.Session["infoFormI2"] = value; }
            get { return Tools.GetStr(HttpContext.Current.Session["infoFormI2"]); }
        }

        public static int BtBack
        {
            set { HttpContext.Current.Session["infoFormBb"] = value; }
            get { return Tools.GetInt(HttpContext.Current.Session["infoFormBb"], ibNone); }
        }

        public static void Show(string info, string infoEx, int back)
        {
            Info1  = info;
            InfoEx = infoEx;
            BtBack = back;
            App.Redirect(infoForm);
        }
        //----------------------------------------------------
        public static void ShowInfo(string typ, int back)    // info z masterpage
        {
            HttpContext.Current.Response.Redirect(infoForm + SetInfoParamsLine(typ, back));
        }

        public static void ShowInfo2(string typ, int back)   // info bez masterpage
        {
            HttpContext.Current.Response.Redirect(infoForm + SetInfoParamsLine(typ, back));
        }
        //----------------------------------------------------
        public static string GetInfoText(string typ)
        {
            return Base.getScalar("select Tekst from Teksty where Typ = " + Base.strParam(typ));
        }

        public static DataRow GetInfo(string typ)
        {
            return db.getDataRow("select * from Teksty where Typ = " + db.strParam(typ));
        }

        public static bool SetInfo(string typ, string opis, string tekst)
        {
            DataRow dr = GetInfo(typ);
            if (dr == null)
                return db.insert("Teksty", 0, "Typ,Opis,Tekst",
                    db.strParam(db.sqlPut(typ, 25)),
                    db.strParam(db.sqlPut(opis, 200)),
                    db.strParam(db.sqlPut(tekst)));
            else
                return db.update("Teksty", 1, "Opis,Tekst",
                    "Typ={0}",
                    db.strParam(typ),
                    db.strParam(db.sqlPut(opis, 200)),
                    db.strParam(db.sqlPut(tekst)));
        }

        //--------
        public const int ibNone = 0;
        public const int ibBack = 1;
        //public const int ibPanelK = 2;

        public static bool PrepareBackButton(Button bt, int ib)   // true jak visible
        {
            switch (ib)
            {
                case ibNone:
                    bt.Visible = false;
                    return false;
                case ibBack:
                    Tools.MakeBackButton(bt);
                    break;
                    /*
                case ibPanelK:
                    bt.PostBackUrl = Mailing.formKierownik;
                    break;
                     */
                default:
                    break;
            }
            return true;
        }
        
        public static void SetInfoParams(string typ, int back)
        {
            HttpContext.Current.Session[sesInfoTyp] = typ;
            HttpContext.Current.Session[sesInfoBack] = back.ToString();
        }

        public static bool GetInfoParams(out string typ, out int back)
        {
            typ = (string)HttpContext.Current.Session[sesInfoTyp];
            if (!String.IsNullOrEmpty(typ))
            {
                HttpContext.Current.Session[sesInfoTyp] = null;
                back = Base.StrToIntDef((string)HttpContext.Current.Session[sesInfoBack], ibBack);
                HttpContext.Current.Session[sesInfoBack] = null;
                return true;
            }
            else
            {
                typ = null;
                back = ibNone;
                return false;
            }
        }
        //-------
        public static string SetInfoParamsLine(string typ, int back)
        {
            return "?t=" + typ + "&b=" + back.ToString();
        }

        public static bool GetInfoParamsLine(out string typ, out int back)
        {
            typ = (string)HttpContext.Current.Request.QueryString["t"];
            if (String.IsNullOrEmpty(typ))
            {
                back = ibBack;
                return false;
            }
            else
            {
                back = Base.StrToIntDef(HttpContext.Current.Request.QueryString["b"], ibBack);
                return true;
            }
        }
        //-------
        public static void SetHelp(string context, bool show)  // do testowania w Ustawieniach - Teksty
        {
            SetHelp(context, show, false);
        }

        public static void SetHelp(string context, bool show, bool store)  // do testowania w Ustawieniach - Teksty
        {
            Page page = HttpContext.Current.Handler as Page;
            System.Web.UI.MasterPage mp = page.Master;
            if (mp.Master != null) mp = mp.Master;    //20130304 nested master reports
            if (mp != null)
            {
                //HelpControl hc = (HelpControl)mp.FindControl("cntHelp");
                Help hc = (Help)mp.FindControl("cntHelp");
                if (hc != null)
                {
                    if (store && hc.HelpContext != context) hc.StoreHelpContext();  // nie robię store jak deugi raz klikam w Regulamin bo by mi przechował Regulamin a nie help
                    hc.HelpContext = context;
                    if (show)
                        hc.Show(true, false);   //pokaż, nie przywracaj
                }
            }
        }

        public static void SetHelp()
        {
            SetHelp(App.FormName, false, false);
        }

        public static void SetHelp(string context)
        {
            SetHelp(context, false, false);
        }

        //----------------------------------------------------
        private static void CheckUpdateInfo(SqlConnection con, string typ, string opis, string info)
        {
            DataRow dr = Base.getDataRow(con, 
                "select Typ, Opis from Teksty " +
                "where Typ = " + Base.strParam(typ));
            if (dr == null)
            {
                if (String.IsNullOrEmpty(info))
                    info = opis;
                Base.execSQL(con,
                    "insert into Teksty(" +
                        "Typ, Opis, Tekst) " +
                        "values (" +
                            Base.strParam(typ) + "," +
                            Base.strParam(opis) + "," +
                            Base.strParam(info) + ")");
            }
            else
                if (dr[1].ToString() != opis)
                    Base.execSQL(con,
                        "update Teksty set " +
                            Base.updateStrParamLast("Opis", opis) +
                        "where Typ = " + Base.strParam(dr[0].ToString()));
        }

        //------------------------------------------------------------
        public static void x_CheckUpdate()
        {
            SqlConnection con = Base.Connect();
            CheckUpdateInfo(con, REGULAMIN,
                        " REGULAMIN. Regulamin Systemu Rejestracji Czasu Pracy",
                        "");

            CheckUpdateInfo(con, HELP_KIERPLANPRACY,
                        "POMOC 001. Panel Kierownika - Plan pracy",
                        "");
            CheckUpdateInfo(con, HELP_KIERACCPP,
                        "POMOC 002. Panel Kierownika - Akceptacja czasu pracy",
                        "");
            CheckUpdateInfo(con, HELP_KIERPRACOWNICY,
                        "POMOC 003. Panel Kierownika - Pracownicy",
                        "");
            CheckUpdateInfo(con, HELP_KIERPARAMS,
                        "POMOC 004. Panel Kierownika - Ustawienia",
                        "");
            CheckUpdateInfo(con, HELP_KIERRAPORTY,
                        "POMOC 005. Panel Kierownika - Raporty",
                        "");
            CheckUpdateInfo(con, HELP_ZASTEPSTWA,
                        "POMOC 006. Panel Kierownika - Zastępstwa",
                        "");
            CheckUpdateInfo(con, HELP_START,
                        "POMOC 007. Formatka startowa",
                        "Lista czynności do wykonania w systemie."); 

            /*CheckUpdateInfo(con, HELP_ADMMANAGER,
                        "POMOC 100. Administracja - Zarządzanie pracą systemu",
                        "");  /**/
            CheckUpdateInfo(con, HELP_ADMSTRUKTURA,
                        "POMOC 101. Administracja - Struktura organizacyjna",
                        "");
            CheckUpdateInfo(con, HELP_ADMPRACOWNICY,
                        "POMOC 102. Administracja - Pracownicy",
                        "");
            CheckUpdateInfo(con, HELP_ADMSTREFY,
                        "POMOC 103. Administracja - Konfiguracja stref RCP",
                        "");
            CheckUpdateInfo(con, HELP_ADMDZIALY,
                        "POMOC 104. Administracja - Konfiguracja Działów (strefy i algorytmy)",
                        "");
            CheckUpdateInfo(con, HELP_ADMZMIANY,
                        "POMOC 105. Administracja - Konfiguracja zmian",
                        "");
            CheckUpdateInfo(con, HELP_ADMPARAMETRY,
                        "POMOC 106. Administracja - Parametry",
                        "");
            CheckUpdateInfo(con, HELP_ADMOKRESY,
                        "POMOC 107. Administracja - Okresy rozliczeniowe",
                        "");
            CheckUpdateInfo(con, HELP_ADMSTAWKI,
                        "POMOC 108. Administracja - Stawki godzinowe",
                        "");
            CheckUpdateInfo(con, HELP_ADMALGORYTMY,
                        "POMOC 109. Administracja - Algorytmy",
                        "");
            CheckUpdateInfo(con, HELP_ADMKODYABS,
                        "POMOC 110. Administracja - Kody absencji",
                        "");
            CheckUpdateInfo(con, HELP_ADMKIERPARAMS,
                        "POMOC 111. Administracja - Ustawienia kierowników",
                        "");

            CheckUpdateInfo(con, HELP_ADMPLANPRACY,
                        "POMOC 112. Administracja - Plan pracy",
                        "");
            CheckUpdateInfo(con, HELP_ADMACCEPTPP,
                        "POMOC 113. Administracja - Akceptacja czasu pracy",
                        "");
            CheckUpdateInfo(con, HELP_ADMRAPORTY,
                        "POMOC 114. Administracja - Raporty",
                        "");

            CheckUpdateInfo(con, HELP_ADMLOG,
                        "POMOC 115. Administracja - Podgląd zdarzeń",
                        ""); 
            CheckUpdateInfo(con, HELP_ADMTEKSTY,
                        "POMOC 116. Administracja - Teksty informacji i pomocy",
                        "");
            CheckUpdateInfo(con, HELP_ADMMAILE,
                        "POMOC 117. Administracja - Powiadomienia mailowe",
                        "");
            CheckUpdateInfo(con, HELP_ADMSYSPARAMS,
                        "POMOC 118. Administracja - Ustawienia systemu",
                        "");

            CheckUpdateInfo(con, HELP_ADMMPK,
                        "POMOC 119. Administracja - Centra kosztowe",
                        "");

            CheckUpdateInfo(con, HELP_INFO,
                        "POMOC 200. Formatka informacyjna",
                        "");
            CheckUpdateInfo(con, HELP_ERR,
                        "POMOC 201. Formatka informacji o błędzie",
                        "");
            CheckUpdateInfo(con, NOHELP,
                        "POMOC 202. Domyślny tekst pomocy",
                        "Pomoc nie jest dostępna.");

            CheckUpdateInfo(con, PANELPRAC,
                        "POMOC 500. Panel Pracownika",
                        "W przypadku pytań lub problemów z użytkowaniem aplikacji prosimy o kontakt z działem HR.");
            Base.Disconnect(con);
        }
    }
}
