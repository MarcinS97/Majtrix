using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Web.UI.WebControls;
using System.DirectoryServices;
using System.Threading;

using HRRcp.App_Code;
using HRRcp.Controls.Reports;
using System.Configuration;
using System.IO;
/*
update multiple tables from ListView
http://www.aspdotnetcodes.com/SQLDataSource_StoredProcedure_Multiple_Tables_Update.aspx 

ListView - grouping items przez dodanie nagłówków OnChange w tabeli
http://www.4guysfromrolla.com/articles/091708-1.aspx
 */

/*
select A.ECUserId,
case 
	when A.InOut = 1 and B.InOut = 1 then null
	else A.Czas
end as TimeIn,
case 
	when A.InOut = 0 and B.InOut = 0 then null
	else B.Czas
end as TimeOut,

ISNULL(case 
	when A.InOut = B.InOut then '0 00:00:00'
	else convert(varchar,DATEDIFF(SECOND, A.Czas, B.czas)/86400) + ' ' + convert(varchar, B.Czas - A.Czas, 8)
end, '0 00:00:00') as CzasPracy,	

case 
	when A.InOut = B.InOut then NULL
	else convert(varchar,DATEDIFF(SECOND, A.Czas, B.czas)/86400) + ' ' + convert(varchar, B.Czas - A.Czas, 8)
end as CzasPracy1,	

case 
	when A.InOut = 1 and B.InOut = 1 then null
	else A.ECReaderID
end as ReaderIn,

case 
	when A.InOut = 0 and B.InOut = 0 then null
	else B.ECReaderID
end as ReaderOut,

*
from RCPEventsCache A left outer join RCPEventsCache B
on B.ECUniqueID = (select top 1 C.ECUniqueID 
					from RCPEventsCache C 
					where C.ECUserId=A.ECUserId 
					and C.InOut is not null
					and C.ECReaderId in (6,7)
					--and A.ECReaderId in (29,1029)
					and C.ECUniqueID > A.ECUniqueID)
where 
A.ECUserID=222 and 
A.InOut is not null
and A.ECReaderId in (6,7)
--and A.ECReaderId in (29,1029)
and not (A.InOut = 1 and B.InOut = 0)
order by A.ECUserId, A.ECUniqueID


*/

namespace HRRcp.App_Code
{
    public class App
    {
        public const string Rcp = "RCP";
        public const string Kwitek = "Kwitek";
        public const string Portal = "Portal";
        public const string MS = "MS";         //Matryca Szkoleń

        public const string DefaultForm = "Default.aspx";
        public const string FirstLoginForm = "LoginForm.aspx";
        public const string LoginForm = "Login.aspx";
        public const string _StartForm = "StartForm.aspx";
        public const string AdminForm = "AdminForm.aspx";
        public const string UstawieniaForm = "UstawieniaForm.aspx";
        public const string KierownikForm = "KierownikForm.aspx";
        public const string WynikiForm = "WynikiForm.aspx";
        public const string WynikiKierForm = "WynikiKierForm.aspx";
        public const string RaportyForm = "Raporty.aspx";

        public const string KwitekForm = "Kwitek.aspx";
        public const string KwitekAdminForm = "AdminKwitek.aspx";
        //public const string KwitekDetaleForm = "KwitekDetale.aspx";
        public const string UrlopForm = "Urlop.aspx";
        public const string UrlopAdminForm = "AdminUrlop.aspx";
        public const string PracLoginForm = "PracLogin.aspx";

        public const string PortalMaster = "~/Portal.Master";

        public const string MasterPage = "~/MasterPage.Master";
        public const string MasterPage2 = "~/MasterPage2.Master";
        public const string MasterPage3 = "~/MasterPage3.Master";

        public const string MasterPageRCP = "~/RCP/RCP.Master";

        /* nowy master */
        public const string PortalMaster2 = "~/Portal2.Master";

        public const string PortalMaster3       = "~/Portal3.Master";
        public const string PortalReportMaster3 = "~/Portal3Report.Master";

        //public const string PortalStartForm = _StartForm;
        public const string PortalStartForm = "~/Portal/StartForm.aspx";
        
        public const string PortalPracForm = "Portal.aspx";
        public const string PortalKierForm = "PortalKier.aspx";
        public const string PortalAdminForm = "Portal/Administracja.aspx";
        public const string PortalWnioskiUrlopwe = "Portal/WnioskiUrlopowe.aspx";
        public const string PortalAktualnosciForm = "Portal/Aktualnosci.aspx";
        public const string PortalGazetkaForm = "Portal/Newsletter.aspx";

        public const string WnioskiUrlopoweForm = "WnioskiUrlopowe.aspx";
#if PORTAL
        public const string WnioskiUrlopoweWpiszForm = "~/Portal/WnioskiUrlopoweWpisz.aspx";
#else
        public const string WnioskiUrlopoweWpiszForm = "WnioskiUrlopoweWpisz.aspx";
#endif

        public const string PodzialLudziAdmForm = "PodzialLudziAdm.aspx";

        public const string SzkoleniaBHP = "SzkoleniaBHP/Rejestr.aspx";
        public const string SzkoleniaBHPAdm = "SzkoleniaBHP/Admin.aspx";

        public const string BadaniaWst = "BadaniaWstepne/Rejestr.aspx";
        public const string BadaniaWstAdm = "BadaniaWstepne/RejestrAdmin.aspx";

        public const string Ogloszenia = "Portal/Ogloszenia.aspx";

        public const string PrzypiszRCP = "Kiosk/PrzypiszRCP.aspx";


        public const string MainSvc = "main.asmx";


        public const string PracPassChangeForm = "PracPassChange.aspx";




        public const string ScStartForm = "Scorecards/Start.aspx";
        public const string ScScorecardsForm = "Scorecards/Scorecards.aspx";
        public const string ScScorecardForm = "Scorecards/Scorecard.aspx";
        public const string ScWnioskiForm = "Scorecards/Wnioski.aspx";
        public const string ScWnioskiAdminForm = "Scorecards/WnioskiAdmin.aspx";
        public const string ScWniosekPremiowyForm = "Scorecards/WniosekPremiowy.aspx";
        public const string ScUstawieniaForm = "Scorecards/Ustawienia.aspx";
        public const string ScAdminForm = "Scorecards/Administracja.aspx";
        public const string ScParametryForm = "Scorecards/Parametry.aspx";
        public const string ScRaportyForm = "Scorecards/Raporty.aspx";

        public const string ScMaster = "~/Scorecards/Scorecards.Master";
        public const string ScReportMaster = "~/Scorecards/Report.Master";


        public const string MSMaster = "~/MatrycaSzkolen/MS.Master";


        public const string PracownicyHarm = "~/RCP/Adm/PracownicyHarm.aspx";

        public const string KartyTmp = "~/RCP/Adm/KartyPracownicyHarm.aspx";




        //public const string dbPORTAL = "HR_PORTAL";
        //public const string dbPORTAL = "HR_PORTAL_20140621_J";
        //public static string dbPORTAL = Tools.GetStr(ConfigurationSettings.AppSettings["HR_PORTAL"], "HR_PORTAL");

        public const string WWW_PORTAL = "WWW_PORTAL";
        public const string WWW_SCORECARDS = "WWW_SCORECARDS";
        public const string WWW_RCP = "WWW_RCP";
        public const string WWW_MS = "WWW_MS";

        public static string dbPORTAL = db.GetDbName(db.PORTAL);
        public static string wwwPORTAL = Tools.GetStr(ConfigurationSettings.AppSettings[WWW_PORTAL], "/Portal");
        public static string wwwSCORECARDS = Tools.GetStr(ConfigurationSettings.AppSettings[WWW_SCORECARDS], "/");
        public static string wwwRCP = Tools.GetStr(ConfigurationSettings.AppSettings[WWW_RCP], "/");          // Scorecards
        public static string wwwMS = Tools.GetStr(ConfigurationSettings.AppSettings[WWW_MS], "/");           // Matryca Szkoleń

        public static string dbRCP = Tools.GetStr(ConfigurationSettings.AppSettings["HR_DB"], "HR_DB");

        public static string stylePath =
#if SPX
            "~/styles/spx/"
#elif ZELMER
            "~/styles/zelmer/"
#elif OKT
            "~/styles/okt/"
#elif APATOR
            "~/styles/apator/"
#else
 "~/styles/default/"
#endif
;








        public const string ImagesPath = "~/images/";
        public const string ImagesPathPortal = "~/images/portal/";
        public const string ImagesPathUrlopy = "~/images/urlopy/";



        //public const int stPracImport = 1;
        //public const int stPracFirstLogin = 2;

        public const string zmNadgKolejne = "0";
        public const string zmNadgZwykleNocne = "1";
        public const string zmNadgBrakZgody = "-1";
        public const string zmAbsencja = "2";

        public const string OldMarker = "* ";

        //----- statusy pracowników/działów; > 0 to się łąpią do zestawień -----
        public const int stCurrent = 0;     // w bieżącej strukturze
        public const int stNew = 1;         // nowy w bieżącej strukturze
        public const int stOld = -1;        // stary, brak w bieżącej strukturze
        public const int stPomin = -2;      // poza strukturą, jak nie ma być liczony czas to trzeba mu ustawić algorytm na bez liczenia czsu pracy
        public const int stBadaniaWst = -6; // nowy z badań wstępnych

        //----- selektywny update kontrolek Tools.IsUpdated, Tools.SetUpdated -----
        public const string updPrzesuniecia = "uprzes";
        public const int updPrzesunieciaTree = 0x00000001;
        public const int updPrzesunieciaList = 0x00000002;


        //----- PdfToImg
        public const string PDFPNGPath = @"~/PDFTOPNG/PdfToPng_v2.exe";


        //----------------------------------------------------
        public static void Redirect(string url)
        {
            //Log.Info(Log.DEBUG, "REDIRECT", Tools.GetRedirectUrl(url));
            HttpContext.Current.Response.Redirect(Tools.GetRedirectUrl(url));  // dokłada ~/ z przodu jak nie ma
        }
        //----------------------------------------------------
        public static void ShowNoAccess()
        {
            ShowNoAccess(null, null);
        }

        public static void ShowNoAccess(string modName, AppUser user)
        {
            if (String.IsNullOrEmpty(modName))
                modName = App.FormName;
            if (user == null)
                user = AppUser.CreateOrGetSession();
            ShowNoAccess2(modName, user.ImieNazwiskoOrLogin);
        }

        public static void ShowNoAccess2(string modName, string userName)
        {
            AppError.Show(modName, String.Format("Użytkownik {0} nie ma uprawnień zezwalających na dostęp do modułu.", userName));
        }

        public static void LoginAsUser(string login)    // sprawdzenie praw wyżej !!!
        {
            AppUser user = AppUser.CreateOrGetSession();
            user.LoginAsUser(login);
            HttpContext.Current.Response.Redirect("~/" + DefaultForm);
        }

        public static bool LogoutOnError   // ustawiana tylko przy wejściu w panel administracyjny / wnioski
        {
            set { HttpContext.Current.Session["LogoutOnError"] = value ? "1" : null; }
            get { return Tools.GetStr(HttpContext.Current.Session["LogoutOnError"]) == "1"; }
        }

        //----------------------------------------------------
        public static bool NeedPassLogin(AppUser user, int secLevel)   // 1,2
        {
            //return false;
            if (user.IsKiosk) return !user.IsKioskLogged;
            else
            {
                int lt = user.LoginType;
                switch (lt)
                {
                    default:
                    case AppUser.ltPass:
                        return !user.IsPassLogged;
                    case AppUser.ltAuthWinPass:
                        if (secLevel == 2)
                            return !user.IsPassLogged;
                        else
                            return false;
                    case AppUser.ltAuthWin:
                        return false;
                }
            }
        }
        //----------------------------------------------------
        private const string sesapp = "__app__";

        public static void SetApp(string app)
        {
            HttpContext.Current.Session[sesapp] = app;
        }

        public static bool IsApp(string app)
        {
            object o = HttpContext.Current.Session[sesapp];
            if (o != null && o.ToString() == app)
                return true;
            else
                return false;
        }

#if PORTAL
        public const string APP = "PORTAL: ";
        public const string ID1 = "P";
#elif KWITEK
        public const string APP = "KWITEK: ";
        public const string ID1 = "K";
#elif IPO
        public const string APP = "IPO: ";
        public const string ID1 = "I";
#else
        public const string APP = "RCP: ";
        public const string ID1 = "R";
#endif

        public const int PassType = 1;  // 8 znaków, do kontroli i generowania hasła Portal Kwitek

        //----------------------------------------------------
        public static bool WPodstrukturze(string kid, string pid, DateTime? naDzien)
        {
            if (naDzien == null)
                naDzien = DateTime.Today;
            DataRow dr = db.getDataRow(String.Format("select Id from dbo.fn_GetSubPrzypisania({0}, '{1}') where IdPracownika = {2}", kid, Tools.DateToStrDb((DateTime)naDzien), pid));
            return dr != null;
        }

        public static string FindKierUp(string kid, DateTime? naDzien, int right)
        {
            if (naDzien == null)
                naDzien = DateTime.Today;       // zwraca też kid jezeli ma right
            DataRow dr = db.getDataRow(String.Format(@" 
select top 1 IdPracownika from dbo.fn_GetUpPrzypisania({0}, '{1}')
where dbo.GetRight(IdPracownika, {2}) <> 0
                ", kid, Tools.DateToStrDb((DateTime)naDzien), right));
            return dr != null ? db.getValue(dr, 0) : null;
        }

        public static DataSet FindAdmin(int right)
        {
            return db.getDataSet(String.Format(@"select * from Pracownicy where Admin = 1 and ({0} = -1 or dbo.GetRight(Id, {0}) <> 0)", right));
        }

        public static DataSet FindUser(bool admin, params int[] rights)
        {
            string adm = admin ? "Admin = 1 and " : null;
            int cnt = rights.Count();
            string[] where = new string[cnt];
            for (int i = 0; i < cnt; i++)
                where[i] = String.Format("dbo.GetRight(Id,{0}) <> 0", rights[i]);
            return db.getDataSet(String.Format(@"select * from Pracownicy where {0}{1}", adm, String.Join(" and ", where)));
        }


        //----------------------------------------------------



        public static DataSet FindZast(DataRow dr, params int[] rights)
        {
            String pracId = db.getValue(dr, "Id");
#if R2
            int cnt = rights.Count();
            string[] where = new string[cnt];
            for (int i = 0; i < cnt; i++)
                where[i] = String.Format("dbo.GetRight(p.Id,{0}) <> 0", rights[i]);
            return db.getDataSet(String.Format(@"
select p.* 
from Zastepstwa z
left join Pracownicy p on p.Id = z.IdZastepujacy
where z.IdZastepowany = {0} and dbo.getDate(getdate()) between z.Od and z.Do and {1}
", pracId, String.Join(" and ", where)));
#else
            return db.getDataSet(String.Format(@"
select p.* 
from Zastepstwa z
left join Pracownicy p on p.Id = z.IdZastepujacy
where z.IdZastepowany = {0} and dbo.getDate(getdate()) between z.Od and z.Do
", pracId));
#endif
        }




        //----------------------------------------------------
        public static string GetccPrawaList(string userId)
        {
            DataSet ds = db.getDataSet(String.Format(@"
select CC as Id, CC, 1 as Sort from ccPrawa where UserId = {0} and IdCC = 0
union all " +
"select '<span title=\"' + CC.Nazwa + '\">' + P.CC + '</span>' as Id, P.CC, 2 as Sort " +
@"from ccPrawa P
left outer join CC on CC.Id = P.IdCC 
where P.UserId = {0} and P.IdCC <> 0
order by Sort, CC",
                App.User.Id));
            return db.Join(ds, 0, ", ");
        }

        public static string GetccPrawaList2(string userId)
        {
            DataSet ds = db.getDataSet(String.Format(@"
select CC as Id, CC, 1 as Sort from ccPrawa where UserId = {0} and IdCC = 0
union all " +
"select '<span title=\"' + CC.Nazwa + '\">' + P.CC + '</span>' as Id, P.CC, 2 as Sort " +
@"from ccPrawa P
left outer join CC on CC.Id = P.IdCC 
where P.UserId = {0} and P.IdCC <> 0
order by Sort, CC",
                userId));
            return db.Join(ds, 0, ", ");
        }

        public static bool CheckccPrawa(cntReport rep)
        {
            if (App.User.HasRight(AppUser.rRepPodzialCC))
            {
                if (App.User.HasRight(AppUser.rRepPodzialCCAll))        // uprawnienia - usuwam joiny do uprawnień
                    rep.SQL1 = null;
                if (App.User.HasRight(AppUser.rRepPodzialCCKwoty))      // są kwoty - ustawiam joiny do kwot
                    rep.SQL2 = rep.P2;



                //rep.Prepare();



                return true;
            }
            else
            {
                App.ShowNoAccess("Raporty", App.User);
                return false;
            }
        }

        public static bool CheckccPrawa(cntReport2 rep)
        {
            if (App.User.HasRight(AppUser.rRepPodzialCC))
            {
                if (App.User.HasRight(AppUser.rRepPodzialCCAll))        // uprawnienia - usuwam joiny do uprawnień
                    rep.SQL1 = null;
                if (App.User.HasRight(AppUser.rRepPodzialCCKwoty))      // są kwoty - ustawiam joiny do kwot
                    rep.SQL2 = rep.P2;



                //rep.Prepare();



                return true;
            }
            else
            {
                App.ShowNoAccess("Raporty", App.User);
                return false;
            }
        }
        //-----
        public static bool CheckccPrawaSplity(cntReport rep)
        {
            if (App.User.HasRight(AppUser.rRepPodzialCC))
            {
                if (App.User.HasRight(AppUser.rRepPodzialCCAll))        // uprawnienia - usuwam joiny do uprawnień
                {
                    rep.SQL3 = null;
                }
            }
            return CheckccPrawa(rep);
        }

        public static bool CheckccPrawaSplity(cntReport2 rep)
        {
            if (App.User.HasRight(AppUser.rRepPodzialCC))
            {
                if (App.User.HasRight(AppUser.rRepPodzialCCAll))        // uprawnienia - usuwam joiny do uprawnień
                {
                    rep.SQL3 = null;
                }
            }
            return CheckccPrawa(rep);
        }
        //-----
        public static bool CheckPrawaRapStolowka()
        {
            if (App.User.HasRight(AppUser.rRepStolowka))
            {
                return true;
            }
            else
            {
                App.ShowNoAccess("Raport - Stołówka", App.User);
                return false;
            }
        }

        public static bool CheckPrawaRozlNadg()
        {
            if (App.User.HasRight(AppUser.rRozlNadg))
            {
                return true;
            }
            else
            {
                App.ShowNoAccess("Raport - Rozliczenie nadgodzin", App.User);
                return false;
            }
        }

        public static void ShowBadRepParameters(string modname)
        {
            if (String.IsNullOrEmpty(modname))
                modname = "Raport";
            AppError.Show(modname, "Niepoprawne parametry wywołania.");
        }
        //----------------------------------------------------
        public static string StawkaText(string stawka)          // do formatek <%=
        {
            return stawka + "%";
        }

        public static string GetZmianaId(string planZmId, string korektaZmId)   // korektaZmId =-1 oznacza nadpisanie brakiem zmiany, w sql ISNULL(korekta, zmiana) zalatwi sprawe, bo zmid=-1 nie znajdzie
        {
            if (String.IsNullOrEmpty(korektaZmId))
                return planZmId;
            else
                return korektaZmId == "-1" ? null : korektaZmId;
        }
        /*        
        public static string GetZmianaId(string planZmId, string korektaZmId)
        {
            if (korektaZmId == "-1")
                return planZmId;
            else
                return korektaZmId;
        }
        */

        public static string GetAlgPar(string alg)
        {
            if (!String.IsNullOrEmpty(alg) && alg != "0" && alg != "1" && alg != "2" && alg != "11" && alg != "12")
                return db.getScalar("select Parametr from Kody where Typ='ALG' and Kod=" + alg);  //tak chyba będzie optymalniej niż dołączać do query
            else
                return null;
        }

        public static void FillStawki(DropDownList ddl, string select)
        {
            if (ddl != null)
            {
                DataSet ds = Base.getDataSet("select convert(varchar,Stawka) + '%' as Stawka, Stawka as Value from Stawki where Aktywna=1 order by Value");
                Tools.BindData(ddl, ds, "Stawka", "Value", false, select);
            }
        }

        public static void x_FillStawki1(DropDownList ddl, string select)
        {
            ddl.Items.Add(new ListItem(StawkaText("100"), "100"));
            ddl.Items.Add(new ListItem(StawkaText("150"), "150"));
            ddl.Items.Add(new ListItem(StawkaText("200"), "200"));
            Tools.SelectItem(ddl, select);
        }

        public static void FillTimeRound(DropDownList ddl, string select, string def)
        {
            if (ddl != null)
            {
                ddl.Items.Add(new ListItem("bez zaokrąglania", "0"));
                ddl.Items.Add(new ListItem("do 1 minuty", "1"));
                ddl.Items.Add(new ListItem("do 5 minut", "5"));
                ddl.Items.Add(new ListItem("do 10 minut", "10"));
                ddl.Items.Add(new ListItem("do 15 minut", "15"));
                ddl.Items.Add(new ListItem("do 30 minut", "30"));
                ddl.Items.Add(new ListItem("do pełnej godziny", "60"));
                Tools.SelectItem(ddl, select, def, false);
                //Tools.SetDefaultItem(ddl, def, null, " (domyślne)");
            }
        }

        public static void FillRoundType(DropDownList ddl, string select, string def)
        {
            if (ddl != null)
            {
                ddl.Items.Add(new ListItem("w dól", "0"));
                ddl.Items.Add(new ListItem("od 0,5 w górę", "1"));
                ddl.Items.Add(new ListItem("w górę", "2"));
                /*
                ddl.Items.Add(new ListItem("od 0,5 do 1 minuty i w górę", "3"));
                ddl.Items.Add(new ListItem("od 0,5 do 5 minut i w górę", "4"));
                ddl.Items.Add(new ListItem("od 0,5 do 15 minut i w górę", "5"));
                ddl.Items.Add(new ListItem("od 0,5 do 30 minut i w górę", "6"));
                */
                Tools.SelectItem(ddl, select, def, false);
                //Tools.SetDefaultItem(ddl, def, null, " (domyślny)");
            }
        }

        public static void FillBreak(DropDownList ddl, string select, string defMin, bool max30)
        {
            if (ddl != null)
            {
                ddl.Items.Clear();
                if (!String.IsNullOrEmpty(defMin))
                    ddl.Items.Add(new ListItem("wartość domyślna (" + defMin + ")", ""));
                ddl.Items.Add(new ListItem("0", "0"));
                ddl.Items.Add(new ListItem("5", "5"));
                ddl.Items.Add(new ListItem("10", "10"));
                ddl.Items.Add(new ListItem("15", "15"));
                ddl.Items.Add(new ListItem("20", "20"));
                ddl.Items.Add(new ListItem("25", "25"));
                ddl.Items.Add(new ListItem("30", "30"));
                if (!max30)
                {
                    ddl.Items.Add(new ListItem("35", "35"));
                    ddl.Items.Add(new ListItem("40", "40"));
                    ddl.Items.Add(new ListItem("45", "45"));
                    ddl.Items.Add(new ListItem("50", "50"));
                    ddl.Items.Add(new ListItem("55", "55"));
                    ddl.Items.Add(new ListItem("60", "60"));
                }
                if (String.IsNullOrEmpty(defMin) && String.IsNullOrEmpty(select))   // domyślne dla Ustawień a nie KierParams
                    select = Ustawienia.DefPrzerwa.ToString();
                Tools.SelectItem(ddl, select);
            }
        }

        public static void FillDays(DropDownList ddl, string select)
        {
            if (ddl != null)
            {
                for (int i = 1; i <= 31; i++)
                {
                    string d = i.ToString();
                    ddl.Items.Add(new ListItem(d, d));
                }
                Tools.SelectItem(ddl, select);
            }
        }

        public static void CopyItems(DropDownList ddlSource, DropDownList ddlDest, string select)
        {
            ListItem[] lia = new ListItem[ddlSource.Items.Count];
            ddlSource.Items.CopyTo(lia, 0);
            ddlDest.Items.AddRange(lia);
            if (String.IsNullOrEmpty(select))
                ddlDest.SelectedIndex = ddlSource.SelectedIndex;
            else
                Tools.SelectItem(ddlDest, select);
        }
        //---------------------------------
        /*
        public static string GetStatus(int st)
        {
            switch (st)
            {
                case stCurrent: return "Ok";
                case stNew: return "Nowy";
                case stOld: return "Stary";
                case stPomin: return "Pomiń";
                default: return String.Format("Inny ({0})", st);
            }
        }

        public static void FillStatus(DropDownList ddl, string select)
        {
            if (ddl != null)
            {
                ddl.Items.Clear();
                ddl.Items.Add(new ListItem(GetStatus(stCurrent), stCurrent.ToString()));
                ddl.Items.Add(new ListItem(GetStatus(stNew), stNew.ToString()));
                ddl.Items.Add(new ListItem(GetStatus(stOld), stOld.ToString()));
                ddl.Items.Add(new ListItem(GetStatus(stPomin), stPomin.ToString()));
                Tools.SelectItem(ddl, select);
            }
        }
        */
        public static string GetStatus(int st)
        {
            switch (st)
            {
                //case stCurrent: return "Pracujący";
                case stCurrent: return "Zatrudniony";
                case stNew: return "Nowy";
                case stOld: return "Zwolniony";
                case stPomin: return "Pomiń";
                //case stScalony: return "Scalony";
                default: return String.Format("Inny ({0})", st);
            }
        }

        public static void FillStatus(DropDownList ddl, string select, bool addPomin)
        {
            if (ddl != null)
            {
                ddl.Items.Clear();
                ddl.Items.Add(new ListItem(GetStatus(stNew), stNew.ToString()));
                ddl.Items.Add(new ListItem(GetStatus(stCurrent), stCurrent.ToString()));
                ddl.Items.Add(new ListItem(GetStatus(stOld), stOld.ToString()));
                if (addPomin)
                    ddl.Items.Add(new ListItem(GetStatus(stPomin), stPomin.ToString()));
                Tools.SelectItem(ddl, select);
            }
        }

        public static string GetNocneOdDo
        {
            get
            {
                Ustawienia settings = Ustawienia.CreateOrGetSession();
                return String.Format("{0}-{1}", settings.NocneOdSec / 3600, settings.NocneDoSec / 3600);
            }
        }

        //----- SŁOWNIKI -------------------------------------
        public static string GetSlownik(SqlConnection con, string Typ, string Kod)
        {
            return Base.getScalar(con,
                "select Nazwa from Slowniki where Typ = " + Base.strParam(Typ) +
                " and Kod = " + Base.strParam(Kod));
        }

        public static void BindSlownik(SqlConnection con, ListControl lc, string Typ, bool fAddChooseStr, string selectedValue)
        {
            DataSet ds = Base.getDataSet(con, "select Nazwa, Kod from Slowniki where Typ = " + Base.strParam(Typ) + " order by Nazwa");
            Tools.BindData(lc, ds, "Nazwa", "Kod", fAddChooseStr, selectedValue);
        }

        //----------------------------------------------------
        const string comma = ",";

        public static string GetPracBelowList(string kierId, bool addRoot, int mode)  // dodaj kierId; 0 - pracownicy, 1 - kierownicy, 2 - pracownicy+kierownicy
        {
            string where = mode < 2 ? " and Kierownik = " + mode.ToString() : null;
            DataSet ds = db.getDataSet("select Id from Pracownicy where Status >= 0 and IdKierownika = " + kierId + where);
            if (Base.getCount(ds) > 0)
            {
                string list = comma + db.Join(ds, 0, comma);
                foreach (DataRow dr in db.getRows(ds))
                    list += GetPracBelowList(db.getValue(dr, 0), false, mode);
                if (addRoot)
                    return kierId + list;
                else
                    return list.Substring(1);
            }
            else
                if (addRoot)
                    return kierId;
                else
                    return null;
        }

        public const string select_prac = "select P.Nazwisko + ' ' + P.Imie as Pracownik, P.Id as Id, P.Kierownik from Pracownicy P";

        public static DataSet GetPracBelow(string kierId, bool addRoot, int mode)
        {
            string list = GetPracBelowList(kierId, addRoot, mode);
            if (!String.IsNullOrEmpty(list))
                return db.getDataSet(String.Format(
                    "select Nazwisko + ' ' + Imie as Pracownik, Id, Kierownik from Pracownicy " +
                    "where Id in ({0}) order by Pracownik",
                    list));
            else
                return null;
        }

        public static DataSet GetPracAll(string exceptId, int mode)
        {
            string where;
            if (mode < 2)
                where = " where Kierownik = " + mode.ToString() +
                       (!String.IsNullOrEmpty(exceptId) ? " and Id <> " + exceptId : null);
            else
                where = !String.IsNullOrEmpty(exceptId) ? " where Id <> " + exceptId : null;
            return db.getDataSet(String.Format(
                "select Nazwisko + ' ' + Imie as Pracownik, Id, Kierownik from Pracownicy {0} order by Pracownik",
                where));
        }

        public static DataSet xGetZastPracAll(string exceptId, int mode)
        {
            string where, union;
            if (mode < 2)
            {
                where = "where Kierownik = " + mode.ToString() +
                       (!String.IsNullOrEmpty(exceptId) ? " and Id <> " + exceptId : null);
                if (mode == 0)
                    union = " union select 'Pokaż kierowników' as Pracownik, -1 as Id, 0 as Kierownik, 1 as Sort";
                else
                    union = " union select 'Pokaż pracowników' as Pracownik, 0 as Id, 0 as Kierownik, 1 as Sort";
            }
            else
            {
                where = !String.IsNullOrEmpty(exceptId) ? "where Id <> " + exceptId : null;
                union = null;
            }
            return db.getDataSet(String.Format(
                "select Nazwisko + ' ' + Imie as Pracownik, Id, Kierownik, 0 as Sort from Pracownicy {0}{1} order by Sort, Pracownik",
                where, union));
        }
        //----------------------------------------------------
        public static void ImportAD(SqlConnection con, ProcessClass pc)
        {
            //return;

            //Ustawienia settings = Ustawienia.CreateOrGetSession(); // <<<<<< beda musiały byc przekazane z góry !!!
            if (!String.IsNullOrEmpty(pc.settings.ADKontroler) && !String.IsNullOrEmpty(pc.settings.ADPath))
            {
                DirectoryEntry de = AD.GetADUsers(pc.settings.ADKontroler, pc.settings.ADPath);
                if (de != null)
                {
                    //---- liczenie count bo inaczej sie nie da ----- 
                    int cnt = 0;
                    foreach (DirectoryEntry c in de.Children)
                        if (AD.IsUser(c)) cnt++;
                    pc.Max = cnt;
                    //----- odczyt AD -----
                    Base.execSQL(con, "delete from AD");

                    string Login, Mail, Tel, Nazwisko, Imie, CN, Path, Prop;
                    foreach (DirectoryEntry c in de.Children)
                    {
                        if (AD.GetUserData(c, out Login, out Mail, out Nazwisko, out Imie, out CN))
                        {
                            Path = c.Path;
                            Prop = "";
                            Tel = null;
                            foreach (string propertyName in c.Properties.PropertyNames)
                            {
                                string v = c.Properties[propertyName][0].ToString();
                                if (propertyName.ToLower() == "telephonenumber")
                                {
                                    Tel = Tools.PreparePhoneNo(v);
                                }
                                //zaremowac i spr szybkość, zmienić na list i join
                                Prop += propertyName + ":" + v + "|";
                            }
                            Base.execSQL(con,
                                "insert into AD (Login , Nazwisko, Imie, Email, Tel, CN, Path, Properties) " +
                                "values (" +
                                    Base.insertStrParam(Login) +
                                    Base.insertStrParam(Nazwisko) +
                                    Base.insertStrParam(Imie) +
                                    Base.insertStrParam(Mail) +
                                    Base.insertStrParam(Tel) +
                                    Base.insertStrParam(CN) +
                                    Base.insertStrParam(Path.Replace("'", "''")) +
                                    Base.insertStrParamLast(Prop.Replace("'", "''")) +
                                ")");
                            pc.Progress++;
                        }
                    }
                    //----- koniec -----
                    pc.Progress = pc.Max;
                }
            }
        }

        public static void UpdateFromPRP_AD(SqlConnection con)
        {
            int cnt = Base.execSQLcnt(con,
                "update Pracownicy set Email = Q.Email, Login = Q.Login " +
                "from HR_PRP.dbo.Pracownicy Q left outer join HR_PRP.dbo.Ankiety A on Q.IdPracownika = A.IdPracownika " +
                "where Pracownicy.Login is null" +
                " and '0' + Q.KadryId = Pracownicy.NR_EW" +
                " and Q.Oceniany=1 and Q.IdKierownika is not null and Q.KadryId is not null and A.Id is not null");
            int cnt2 = Base.execSQLcnt(con,
                "update Pracownicy set TelJabil = A.Tel " +
                "from AD A " +
                "where TelJabil is null and Pracownicy.Login = A.Login");
        }

        public static string ImportData_test()
        {
            int id = Log.Info(Log.t2APP_IMPORTKP, "test Import danych", null, Log.PENDING);
            OdbcConnection conKP = Base.odbcConnect(KP.conStrKP);
            SqlConnection con = Base.Connect();

            //!!!!!!!!!!!!!!!! właczyć !!!!!!!!!!!!!!!
            //App.ImportAD(con);
            string cnt = KP.ImportKADRY_test(conKP, con);
            //KP.ImportSLOWNIKI(conKP, con);
            //UpdateFromPRP_AD(con);

            Base.Disconnect(conKP);
            Base.Disconnect(con);
            Log.Update(id, Log.OK);
            Log.Info(Log.t2APP_IMPORTKP, "test Import danych zakończony", null, Log.OK);
            return cnt;
        }

        //------------------------
        public static string x_whereToStart()
        {
            return String.Format(" where Status >= 0");

            /*aa
            return " where Checked=1 and P_Start is null and StanAnkiety=" + Ankieta.stPracownik;  // na wszelki wypadek
             */
        }

        /*aa
        public static bool StartVerify()
        {
            int id = Log.Info(Log.t2APP_STARTWER, "Start weryfikacji", null, Log.PENDING);
            SqlConnection con = Base.Connect();
            int cnt = Base.execSQLcnt(con, 
                "update Pracownicy set " +
                    Base.updateParam("Status", "1") +
                    Base.updateParamLast("P_Start", Base.sqlGetDate("GETDATE()")) +  // nie mlże być updateDateParam bo dokłada '' a nie ma bo sqlGetDate rozwija do left(convert ...,10)!
                whereToStart()
                );
            Base.Disconnect(con);
            Log.Update(id, "Start weryfikacji - ilość: " + cnt.ToString(), Log.OK);
            return cnt > 0;
        }
        */
        //------------------------------------------

        public static void longTester(ProcessClass pc)
        {
            const int cnt = 100;
            pc.Max = cnt;
            for (int i = 1; i <= cnt; i++)
            {
                pc.Progress += 1;
                Tools.Delay(25);
                if (pc.LogId != 0)
                    Log.Update2(pc.LogId, i.ToString(), i == cnt ? Log.OK : Log.PENDING);
            }
        }

        public class ProcessClass
        {
            int Fmax;
            int Fprogress;
            int FLogId;
            int FprocId;
            int Ferror;
            public AppUser user;
            public Ustawienia settings;

            public ProcessClass(int logId, int procId)
            {
                FLogId = logId;
                Fmax = 0;
                Fprogress = -1;
                Ferror = 0;
                FprocId = procId;
                user = AppUser.CreateOrGetSession();
                settings = Ustawienia.CreateOrGetSession();
            }

            public void DoProcess(object par)
            {
                switch (FprocId)
                {
                    case 0:
                        longTester((ProcessClass)par);
                        break;
                    default:
                    case 1:
                        /*
                        Log.Info(user, Log.t2APP_IMPORTAD, "Import AD", "test", Log.OK);
                        longTester((ProcessClass)par);
                        /**/
                        try
                        {
                            SqlConnection con = Base.Connect();
                            ImportAD(con, this);
                            Base.Disconnect(con);
                        }
                        catch (Exception ex)
                        {
                            Ferror = -1;
                            Log.Error(user, Log.APP_IMPORTAD, "Import AD", ex.Message);
                        }
                        /**/
                        break;
                    case 2:
                        try
                        {
                            OdbcConnection conKP = Base.odbcConnect(KP.conStrKP);
                            SqlConnection con = Base.Connect();
                            int cnt = KP.ImportKADRY(conKP, con, this);
                            KP.ImportSLOWNIKI(conKP, con);
                            UpdateFromPRP_AD(con);
                            Base.Disconnect(conKP);
                            Base.Disconnect(con);
                        }
                        catch (Exception ex)
                        {
                            Ferror = -1;
                            Log.Error(user, Log.t2APP_IMPORTKP, "Import KP", ex.Message);
                        }
                        break;
                }
            }

            public int Max
            {
                get { return Fmax; }
                set { Fmax = value; }
            }

            public int Progress
            {
                get
                {
                    if (Fprogress == -1) return 0;
                    else return Fprogress;
                }
                set
                {
                    Fprogress = value;
                    if (Fprogress > Fmax) Fmax = Fprogress;  // zabezpieczenie
                }
            }

            public int LogId
            {
                get { return FLogId; }
                set { FLogId = value; }
            }

            public int ProcId
            {
                get { return FprocId; }
                set { FprocId = value; }
            }

            public int Pcent
            {
                get
                {
                    if (Fmax != 0)
                    {
                        int p = Fprogress * 100 / Max;
                        if (p == 100 && !Finished) p = 99;  // korekta zeby 100 sie pojawilo dopiero jak skończy
                        return p;
                    }
                    else return 0;
                }
            }

            public bool Finished
            {
                get { return Fprogress == Fmax || Ferror != 0; }
            }

            public int Error
            {
                get { return Ferror; }
            }
        }
        //-------
        public static ProcessClass ExecProcess(int logId, int procId)
        {
            ProcessClass pc = new ProcessClass(logId, procId);
            ParameterizedThreadStart ts = new ParameterizedThreadStart(pc.DoProcess);
            Thread thd = new Thread(ts);
            thd.IsBackground = true;
            thd.Start(pc);  // pc - object - parametr uruchomienia - wskazanie na całą klase zeby wszystko było w jednym miejscu
            return pc;
        }
        //-------------------------------------
        public static Page Page
        {
            get { return HttpContext.Current.Handler as Page; }
        }

        public static string FormName
        {
            get { return Path.GetFileName(HttpContext.Current.Request.PhysicalPath); }
        }

        /*
        public static MasterPage Master
        {
            get { return (MasterPage)Page.Master; }
        }
        */

        public static RcpMasterPage Master
        {
            get
            {
                if (Page != null)//MVC Support
                {
                    if (Page.Master != null)
                        if (Page.Master.Master != null)
                            return (RcpMasterPage)Page.Master.Master;
                        else
                            return (RcpMasterPage)Page.Master;
                    else
                        return null;
                }
                else
                    return null;
            }
        }

        public static RcpMasterPage Masterxxx
        {
            get
            {
                //System.Web.UI.MasterPage mp1 = Page.Master;
                //System.Web.UI.MasterPage mp2 = Page.Master.Master;

                Page page = HttpContext.Current.Handler as Page;
                System.Web.UI.MasterPage mp = page.Master;
                RcpMasterPage mp2 = (RcpMasterPage)((System.Web.UI.MasterPage)Page.Master).Master;

                //if (mp.Master != null) mp = mp.Master;    //20130304 nested master reports


                if (Page.Master != null)
                    if (Page.Master.Master != null)
                        return (RcpMasterPage)((System.Web.UI.MasterPage)Page.Master).Master;
                    else
                        return (RcpMasterPage)Page.Master;
                else
                    return null;

                /*
                System.Web.UI.MasterPage mp = Page.Master;
                if (mp.Master != null) mp = mp.Master;    //20130304 nested master reports
                return (RcpMasterPage)mp; 
                 */
            }
        }

        public static AppUser _User
        {
            get { return Master.user; }
        }

        public static AppUser User
        {
            get
            {
                if(Master != null)// MVC Support
                { 
                if (Master.user == null)   //20140901 czasem jest tak ze on activate multiview wykonuje sie przed OnInit w MasterPage
                    Master.user = AppUser.CreateOrGetSession();
                return Master.user;
                }
                else
                {
                    return AppUser.CreateOrGetSession();
                }
            }
        }
        //---------------

        private const string ses_kwitek_kid = "kwitek_kid";
        private const string ses_kwitek_pid = "kwitek_pid";
        private const string ses_kwitek_listaid = "kwitek_listaid";

        public static string KwitekKadryId
        {
            get
            {
                object o = HttpContext.Current.Session[ses_kwitek_kid];
                /*
                if (o == null || String.IsNullOrEmpty(o.ToString()))    // nie ma probuje wziac z usera po loginie
                {
                    AppUser user = AppUser.CreateOrGetSession();
                    KwitekKadryId = user.NR_EW;
                }
                o = HttpContext.Current.Session[ses_kwitek_kid];        // user nie ma loginu
                */
                if (o != null)
                    return o.ToString();
                else
                    return null;
            }
            set
            {
                if (!App.User.IsAdmin || !App.User.HasRight(AppUser.rKwitekAdm))
                    if (!String.IsNullOrEmpty(App.User.NR_EW) && value != App.User.NR_EW)
                    {
                        Log.Info(Log.KWITEK, "PRÓBA USTAWIENIA NR_EW", String.Format("{0} {1} {2}", KwitekKadryId, App.User.NR_EW, value));
                        value = App.User.NR_EW;
                    }
                HttpContext.Current.Session[ses_kwitek_kid] = value;
            }
        }

        public static string KwitekPracId
        {
            get
            {
                return Tools.GetStr(HttpContext.Current.Session[ses_kwitek_pid]);
            }
            set
            {
                HttpContext.Current.Session[ses_kwitek_pid] = value;
            }
        }

        public static string KwitekListaId
        {
            get
            {
                object o = HttpContext.Current.Session[ses_kwitek_listaid];
                if (o == null || String.IsNullOrEmpty(o.ToString()))
                {
                    KwitekListaId = "";
                }
                return HttpContext.Current.Session[ses_kwitek_listaid].ToString();
            }
            set { HttpContext.Current.Session[ses_kwitek_listaid] = value; }
        }
        //-------------------------------
        public static bool _PrzypisywanieRCP   // ustawiana tylko przy wejściu w panel administracyjny / wnioski
        {
            set { HttpContext.Current.Session["PrzypRCP"] = value ? "1" : null; }
            get { return Tools.GetStr(HttpContext.Current.Session["PrzypRCP"]) == "1"; }
        }

        public static string _KartaRCP
        {
            set { HttpContext.Current.Session["przkartarcp"] = value; }
            get { return Tools.GetStr(HttpContext.Current.Session["przkartarcp"]); }
        }

        public static string SelectedPracId
        {
            set { HttpContext.Current.Session["stpracselid"] = value; }
            get { return Tools.GetStr(HttpContext.Current.Session["stpracselid"]); }
        }

        //-----------------------------------
        public static string SesSelPracId
        {
            set { HttpContext.Current.Session["sesselpid"] = value; }
            get { return Tools.GetStr(HttpContext.Current.Session["sesselpid"]); }
        }

        public static string SesSelKierId
        {
            set { HttpContext.Current.Session["sesselkid"] = value; }
            get { return Tools.GetStr(HttpContext.Current.Session["sesselkid"]); }
        }

        public static void SetSesSelPracId(string pracId)
        {
            SesSelPracId = pracId;
            if (!String.IsNullOrEmpty(pracId))
                SesSelKierId = db.getScalar("select KierId from VPrzypisaniaNaDzis where Id = " + pracId);
            else
                SesSelKierId = null;
        }

        //public static bool FirstEnter
        //{
        //    set { HttpContext.Current.Session["isfirstent"] = value; }
        //    get { return Tools.GetBool(HttpContext.Current.Session["isfirstent"], true); }
        //}

        public static string StylePath    // potem z ustawień
        {
            get { return stylePath; }
        }
        //----------------------------------
        static int FMailing = -1;
        public static bool IsMailing   // "0" - brak, inne lub brak wpisu - wysyłka, docelowo mozna dac do settings i sesji
        {
            get
            {
                if (FMailing == -1)
                {
                    string m = Tools.GetStr(ConfigurationSettings.AppSettings["MAILING"]);
                    FMailing = m == "0" ? 0 : 1;
                    //FMailing = Tools.StrToInt(Tools.GetStr(ConfigurationSettings.AppSettings["MAILING"], "1"), 1);
                }
                return FMailing != 0;
            }
        }

        public static string ProdWWW   // folder www
        {
            get
            {
                return Tools.GetStr(ConfigurationSettings.AppSettings["APP_PROD_WWW"],
#if PORTAL
                    "Portal"
#elif IPO
                    "IPO"
#else
 "RCP"
#endif
);
            }
        }

        public static string ProdDB
        {
            get
            {
                return Tools.GetStr(ConfigurationSettings.AppSettings["APP_PROD_DB"],
#if PORTAL
                    "HR_PORTAL2"
#elif IPO
                    "HR_IPO"
#else
 "HR_DB"
#endif
);
            }  // SERVER.DB_NAME można też dać
        }


        #region MASTER

        public const string mstrPortal = "mstrPortal";
        public const string mstrRCP = "mstrRCP";

        public static string GetMasterPage()
        {
#if PORTAL
            return PortalMaster3;
#elif RCP
            return MasterPageRCP;
#else
#if KDR
            return MasterPage2;
#elif DBW
            return MasterPage3;
#elif VICIM
            return MasterPage3;
#elif VC
            return MasterPage3;
#else
            return MasterPage;
#endif
#endif
        }

        public static string GetReportMasterPage()
        {
#if SCARDS
            return ScReportMaster;
#elif MS
            return MSMaster;
#elif PORTAL
            return PortalReportMaster3;
#else
            return GetMasterPage();
#endif
        }
        #endregion
        //----------------------------------------
        public static Global Global
        {
            get { return (Global)HttpContext.Current.ApplicationInstance; }
        }
    }
}
