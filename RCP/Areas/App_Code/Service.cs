using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;
using HRRcp.Controls;
using HRRcp.Controls.Przypisania;

/*
dodatkowa kolumna w Scheduler
 * ConStr - przygotowanie dsMail odbywa sie po połączeniu z tym connection string, HRConnectionString (db.con) domyślne
dodatkowe parametry w dsMail zwracanym przez Scheduler'a
 * ReportConStr - connection string dla raportów do SqlMenu, HRConnectionString (db.con) domyślne
 * ReportId - id raportu 
 * RepotName - nazwa pliku raportu
 * ReportOne - 1 jeżeli 
 * 
 
 
 */

namespace HRRcp.App_Code
{
    public class Service
    {
        public static bool inProgress = false;  // to z opisu będzie instancja zmiennej widziana we wszystkich sesjach/procesach PRP

        public Service()
        {
        }

        public static void ExecuteStatic(int mode, string par)
        {
            Service S = new Service();
            S.Execute(mode, par);
        }

        public void Execute(int mode, string par)
        {
            Uri r = HttpContext.Current.Request.UrlReferrer;
            string host = HttpContext.Current.Request.UserHostName;
            string ip = HttpContext.Current.Request.UserHostAddress;
            //string Referrer = (r != null ? r.ToString() + " " : "") + (char)13 + host + " (" + ip + ")";
            string Referrer = (r != null ? r.ToString() + " " + (char)13 : "") + (host == ip ? host : host + " (" + ip + ")");
            if (!inProgress)        // na raz tylko jedna procedura chodzi ...
            {
                inProgress = true;
                try
                {
                    int ev = Log.Info(Log.SERVICE, String.Format("Service start, mode: {0}, par: {1}", mode, db.nullParam(par)), Referrer, Log.PENDING);
                    try
                    {
                        //---------------------------------
                        switch (mode)
                        {
                            default:
                            case 0:
                                SqlConnection con = Base.Connect();
                                //ROGER.ImportROGER(con, ev);  nie działa stąd import, tylko program w Delphi wciaga
#if PORTAL
    #if SPX                     //!!! dla instalacji bez RCP, jeżeli z RCP to tam sie wszystko odbywa
                                //CheckDataIntegrity(con, ev);
                                //CheckMonits(con, ev);
                                CheckZastepstwa(con, ev);
                                //CheckWnioskiUrlopowe(con, ev);
                                //CheckJobs(con, ev);
                                CheckExecAuto(con, ev);
                                //CheckSupervisorActions(con, ev);
    #endif
#elif SCARDS
                                //----- SCARDS ---------------------------- alternatywnie można wszystko skonfigurować po stronie RCP i tak jest teraz zrobione ...
                                CheckDataIntegrity(con, ev);
                                CheckZastepstwa(con, ev);               // zastepstwa sa wspolne z rcp wiec tu trzeba wyłączyć aktywnosc maili
                                CheckJobs(con, ev);
                                CheckExecAuto(con, ev);
                                CheckSupervisorActions(con, ev);
#else
                                //----- RCP -------------------------------
                                CheckDataIntegrity(con, ev);
                                //CheckMonits(con, ev);
                                CheckOkres(con, ev, null, false);
                                CheckZastepstwa(con, ev);
                                CheckPrzypisania(con, ev);
                                CheckWnioskiUrlopowe(con, ev);
                                CheckJobs(con, ev);

                                CheckPodzialLudzi(con, ev);
                                
                                CheckExecAuto(con, ev);
                                
                                CheckSupervisorActions(con, ev);

                                VCSaturnHelper.SynchronizacjaWszystkiego();
#endif
#if IPO
                                //----- iPO -------------------------------
                                dla iPO jest osobna aplikacja póki co
                                UpdateWaluty(con, ev);
#endif
                                Base.Disconnect(con);
                                break;
                            case 1:
#if SIEMENS || KDR
                                //cntImportALARMUS.ImportData2();
                                AutoID.ImportData();
#endif
                                break;
                            case 2:
                                con = Base.Connect();
                                if (String.IsNullOrEmpty(par))
                                    CheckExecAuto(con, ev);
                                else
                                    CheckExecAuto(con, ev, par);
                                Base.Disconnect(con);
                                break;
                            
                            case 3:
#if VICIM
                                VCSaturnHelper.SynchronizacjaWszystkiego();
#endif
                                break;
                        }
                        //---------------------------------
                        Log.Update(ev, Log.OK);
                        Log.Info(Log.SERVICE, ev, String.Format("Service stop, mode: {0}, par: {1}", mode, db.nullParam(par)), null, Log.OK);
                    }
                    catch (Exception ex)
                    {
                        Log.Update2(ev, ex.Message, Log.ERROR);
                        Log.Error(Log.SERVICE, ev, String.Format("Service stop, mode: {0}, par: {1} - Error", mode, db.nullParam(par)), ex.Message);
                    }
                }
                finally
                {
                    inProgress = false;
                }
            }
            else
                Log.Info(Log.SERVICE, String.Format("Service pending, mode: {0}", mode), Referrer, Log.ERROR_BLOCKED);
        }

        public void Execute2()   // TESTER
        {
            Uri r = HttpContext.Current.Request.UrlReferrer;
            string host = HttpContext.Current.Request.UserHostName;
            string ip = HttpContext.Current.Request.UserHostAddress;
            //string Referrer = (r != null ? r.ToString() + " " : "") + (char)13 + host + " (" + ip + ")";
            string Referrer = (r != null ? r.ToString() + " " + (char)13 : "") + (host == ip ? host : host + " (" + ip + ")");
            if (!inProgress)        // na raz tylko jedna procedura chodzi ...
            {
                inProgress = true;
                try
                {
                    int ev = Log.Info(Log.SERVICE, "Service start - tester", Referrer, Log.PENDING);
                    try
                    {
                        //---------------------------------
                        SqlConnection con = Base.Connect();
                        Base.Disconnect(con);
                        //---------------------------------
                        Log.Update(ev, Log.OK);
                        Log.Info(Log.SERVICE, ev, "Service stop - tester", null, Log.OK);
                    }
                    catch (Exception ex)
                    {
                        Log.Update2(ev, ex.Message, Log.ERROR);
                        Log.Error(Log.SERVICE, ev, "Service stop - tester - Error", ex.Message);
                    }
                }
                finally
                {
                    inProgress = false;
                }
            }
            else
                Log.Info(Log.SERVICE, "Service pending - tester", Referrer, Log.ERROR_BLOCKED);
        }
        //------------------------------------------------
        private void CheckDataIntegrity(SqlConnection con, int ParentId)
        {
        }
        //------------------

        // >>>> maile sie nie wysyłają dla nie istniejącego okresu - dołożyć dopisanie po zamknięciu starego z automatu !!!
        // >>>> jak Monit DniPrzed = 0 to też nie wysle maili o konieczności zaakceptowania !!!
        public static void CheckOkres(SqlConnection con, int ParentId, DateTime? day, bool test)
        {
            try
            {
                const bool workDays = true;         // wysyłka tylko w dni pracujące <<< później dać do ustawień
                bool onOkresMonit = false;
                bool onOkresClose = false;
                Ustawienia settings = Ustawienia.CreateOrGetSession();
                DateTime d = day != null ? (DateTime)day : DateTime.Today;

                Log.Info(Log.t2APP_MONITOKRESENDING, ParentId, "Service.CheckOkres", Tools.DateToStr(d), Log.OK);

                if (settings.MonitDniPrzed >= 0)     // 0-wysle 20, 1-wysle 19, 
                {
                    Okres ok = new Okres(d);
                    //DateTime okDateFrom = ok.DateFrom;
                    //DateTime okDateTo = ok.DateTo;
                    DateTime okDateFrom = ok.DataNaliczeniaPrev.AddDays(1);
                    DateTime okDateTo = ok.DataNaliczenia;
                    //----- 1. Przypomnienie o zbliżającym sie końcu -----
                    int dni;
                    if (workDays)
                        dni = Worktime.GetIloscDniPrac(con, d, okDateTo) - 1;
                    else
                        dni = Tools.GetDaysCount(d, okDateTo) - 1; // jak 20-20 to 1 dzień, 0 nie powinno nigdy wystąpić bo 21-20 sie nie zdarzy bo weźmie następny okres rozliczeniowy

                    if (0 <= dni && dni <= settings.MonitDniPrzed)  // czyli dla monit=0 ostatniego dnia o 1:00 wysle, dla 1-19 i 20
                        if (d > okDateFrom)         // jakby to był pierwszy dzień to go pomijam, ale zakładam, ze i tak będzie klika dni przed końcem
                        {
                            onOkresMonit = true;                                            // jakbym wysłał ...
                            if (!workDays || Worktime.GetRodzajDnia(d) == Worktime.dayPracujacy)   // spr czy pracujący lub w każdy
                            {
                                Log.Info(Log.t2APP_MONITOKRESENDING, ParentId, "Service.CheckOkres: Monit zbliżającego się końca okresu rozliczeniowego", null, Log.OK);
                                if (!test)
                                {
                                    DataSet ds = ok._GetKierNotClosed(con, d.AddDays(-1));       // spr do dnia wczorajszego
                                    foreach (DataRow dr in Base.getRows(ds))
                                        if (Base.getValue(dr, "Id") != "0")                     // wysyłam do kierowników
                                        {
                                            string n = Base.getValue(dr, "KierownikNI");
                                            int id = Log.Info(Log.t2APP_MONITOKRESENDING, ParentId, "Monit zbliżającego się końca okresu rozliczeniowego", n, Log.PENDING);
                                            Mailing.EventOkres(Mailing.maOKRESENDING, dr, ok);
                                            Log.Update(id, Log.OK);
                                        }
                                }
                            }
                        }
                    //----- 2. Info o zamknieciu - przypomnienie o acc ----- 1 i 2 powinny się w sumie wykluczać i tak to chyba z warunków nawet wynika ...
                    if (ok.Prev())   // bieżący nigdy nie moze być zamkniętym, biorę prev i spr czy zamknięty - jesli nie to trzeba monitować, jesli zamkniety to wszystko poszło ok
                    {
                        if (ok.Status != Okres.stClosed)    // jeżeli jest zamknięty to nie mam co sprawdzać
                        {
                            onOkresClose = true;
                            if (!workDays || Worktime.GetRodzajDnia(d) == Worktime.dayPracujacy)
                            {
                                Log.Info(Log.t2APP_MONITOKRESENDING, ParentId, "Service.CheckOkres: Monit zakończenia okresu rozliczeniowego", null, Log.OK);
                                if (!test)
                                {
                                    DataSet ds = ok._GetKierNotClosed(con, okDateTo);
                                    foreach (DataRow dr in Base.getRows(ds))
                                        if (Base.getValue(dr, "Id") != "0")
                                        {
                                            string n = Base.getValue(dr, "KierownikNI");
                                            int id = Log.Info(Log.t2APP_MONITOKRESACC, ParentId, "Monit zakończenia okresu rozliczeniowego", n, Log.PENDING);
                                            Mailing.EventOkres(Mailing.maOKRESACC, dr, ok);
                                            Log.Update(id, Log.OK);
                                        }
                                }
                            }
                        }
                    }
                    else Log.Error(Log.t2APP_MONITOKRESACC, ParentId, "Błąd podczas pobierania danych zamkniętego okresu rozliczeniowego", ok.OdDoStr);
                }
                //----- 3. przypomnienie o konieczności zaakceptowania tygodnia -> piątek
                Okres ok7 = new Okres(d);
                DayOfWeek weekMonitDay = DayOfWeek.Friday;   // odczyt z konfiguracji później
                if (!onOkresMonit)
                {
                    bool send = false;
                    if (workDays)
                    {
                        int dd = (int)weekMonitDay - (int)d.DayOfWeek;
                        if (dd < 0) dd += 7;
                        bool dayPrac;
                        DateTime dMonit = d.AddDays(dd);
                        do
                        {
                            dayPrac = Worktime.GetRodzajDnia(dMonit) == Worktime.dayPracujacy;
                            if (dMonit == d && dayPrac)
                            {
                                send = true;
                                break;
                            }
                            dMonit = dMonit.AddDays(-1);
                        }
                        while (dMonit >= d && !dayPrac);      // kręci się póki true, wychodzi jak są równe bądź będzie jakiś dzień pracujący po
                    }
                    else
                        send = d.DayOfWeek == weekMonitDay;
                    if (send)
                    {
                        Log.Info(Log.t2APP_MONITOKRESENDING, ParentId, "Service.CheckOkres: Monit zbliżającego się zamknięcia tygodnia", null, Log.OK);
                        if (!test)
                        {
                            DataSet ds = ok7._GetKierNotClosed(con, d.AddDays(-1));      // spr do dnia wczorajszego <<< sprawdza poprzedni dzień a wysyła o 2 w nocy - nie załapia sie ci, ktorzy poakceptowali od razu po zmianie i spłynięciu danych = niewiele
                            foreach (DataRow dr in Base.getRows(ds))
                                if (Base.getValue(dr, "Id") != "0")                     // wysyłam do kierowników
                                {
                                    string n = Base.getValue(dr, "KierownikNI");
                                    int id = Log.Info(Log.t2APP_MONITWEEKENDING, ParentId, "Monit zbliżającego się zamknięcia tygodnia", n, Log.PENDING);
                                    Mailing.EventOkres(Mailing.maWEEKENDING, dr, ok7);
                                    Log.Update(id, Log.OK);
                                }
                        }
                    }
                }
                //----- 4. przypomnienie, że nie poakceptował -> pon, wt...
                if (!onOkresClose && d > ok7.NextLockTo)        // next to niedziela - w poniedziałek o 1:00 wysyłam maile
                {
                    if (!workDays || Worktime.GetRodzajDnia(d) == Worktime.dayPracujacy)
                    {
                        //DateTime dLock = ok7.NextLockTo;
                        DateTime dLock = ok7.GetProperLockedTo(d);  //
                        Log.Info(Log.t2APP_MONITOKRESENDING, ParentId, "Service.CheckOkres: Monit zamknięcia tygodnia: " + Tools.DateToStr(dLock), null, Log.OK);
                        if (!test)
                        {
                            DataSet ds = ok7._GetKierNotClosed(con, dLock);
                            foreach (DataRow dr in Base.getRows(ds))
                                if (Base.getValue(dr, "Id") != "0")
                                {
                                    string n = Base.getValue(dr, "KierownikNI");
                                    int id = Log.Info(Log.t2APP_MONITWEEKACC, ParentId, "Monit zamknięcia tygodnia: " + Tools.DateToStr(dLock), n, Log.PENDING);
                                    Mailing.EventOkres(Mailing.maWEEKACC, dr, ok7);
                                    Log.Update(id, Log.OK);
                                }
                        }
                    }
                }
                //----- 5. brak planu pracy ---------
                const int checkNoPPDays = 10;
                Okres okPP = new Okres(d.AddDays(checkNoPPDays));
                if (!workDays || Worktime.GetRodzajDnia(d) == Worktime.dayPracujacy)   // spr czy pracujący lub w każdy
                {
                    Log.Info(Log.MONITOKRESNOPP, ParentId, "Service.CheckOkres: Monit brak planu pracy", null, Log.OK);
                    if (!test)
                    {
                        DataSet ds = okPP.GetKierBrakPlanuPracy(con);               // dla głównego poziomu nikt nie dostanie maili, wypadałoy sprawdzić też bieżący okres
                        foreach (DataRow dr in Base.getRows(ds))
                            if (Base.getValue(dr, "Id") != "0")                     // wysyłam do kierowników
                            {
                                string n = Base.getValue(dr, "KierownikNI");
                                int id = Log.Info(Log.MONITOKRESNOPP, ParentId, "Monit braku planu pracy", n, Log.PENDING);
                                Mailing.EventOkres(Mailing.maNOPP, dr, okPP);
                                Log.Update(id, Log.OK);
                            }
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(Log.t2APP_MONITOKRESENDING, ParentId, "Service.CheckOkres", ex.Message);
            }
        }

        //------------------
        private void CheckZastepstwa(SqlConnection con, int ParentId)
        {
            string id, id1, id2, d1, d2;
            //----- startujące zastępstwa -----
            DataSet ds = Zastepstwa.GetStartujace();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Zastepstwa.GetData(dr, out id, out id1, out id2, out d1, out d2);
                Log.Info(Log.ZASTEPSTWO, ParentId, Mailing.zaStart, id, Log.OK);
                Mailing.EventZastepstwo(Mailing.maZAST_ADD, id, id1, id2, d1, d2, Mailing.zaStart);
            }
            //----- wygasające zastępstwa -----
            ds = Zastepstwa.GetWygasajace();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Zastepstwa.GetData(dr, out id, out id1, out id2, out d1, out d2);
                Log.Info(Log.ZASTEPSTWO, ParentId, Mailing.zaStop, id, Log.OK);
                Mailing.EventZastepstwo(Mailing.maZAST_END, id, id1, id2, d1, d2, Mailing.zaStop);
            }
            //----- przypomnienie o wygasającym zastępstwie -----
            ds = Zastepstwa.GetMonity();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Zastepstwa.GetData(dr, out id, out id1, out id2, out d1, out d2);
                Log.Info(Log.ZASTEPSTWO, ParentId, Mailing.zaMonit, id, Log.OK);
                Mailing.EventZastepstwo(Mailing.maZAST_MONIT, id, id1, id2, d1, d2, Mailing.zaMonit);
            }

            /*
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            if (settings.MonitZastepstwa > 0)
            {
                ds = Workers.GetZastepstwaMonit(con, settings.MonitZastepstwa);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    z = Workers.GetZastStr(dr);
                    string pId = Base.getValue(dr, "IdPracownika");
                    string zId = Base.getValue(dr, "IdZastepstwa");
                    string data = Base.getValue(dr, "CzasZastepstwa");
                    id = Log.Info(Log.t2APP_ZASTMONIT, ParentId, "Monit zastępstwa", z, Log.PENDING);
                    Mailing.EventZastMonit(pId, zId, data);
                    Mailing.UpdateMonit(con, null, null, pId, Mailing.zaZASTZ_M, 1);
                    Log.Update(id, Log.OK);
                }
            }
            */
        }
        //------------------------------
        private void CheckPrzypisania(SqlConnection con, int ParentId)
        {
            try
            {
                //----- startujące -----
                DataSet ds = cntPrzypisania.GetStartujace();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string pid = db.getValue(dr, "Id");
                    Log.Info(Log.PRZESUNIECIA, ParentId, "Service.CheckPrzypisania1", Mailing.maPRZES_START_K + ":" + pid, Log.OK);
                    Mailing.EventPrzesuniecie(Mailing.maPRZES_START_K, pid);
                    Mailing.EventPrzesuniecie(Mailing.maPRZES_START_P, pid);
                }
                //----- przypomnienie ze się kończy/skończyło -----
                ds = cntPrzypisania.GetWygasajace(3);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string pid = db.getValue(dr, "Id");
                    Log.Info(Log.PRZESUNIECIA, ParentId, "Service.CheckPrzypisania2", Mailing.maPRZES_MONIT + ":" + pid, Log.OK);
                    Mailing.EventPrzesuniecie(Mailing.maPRZES_MONIT, pid);
                }
                //----- przypomnienie o akceptacji/odrzuceniu -----
                ds = cntPrzypisania.GetMonityAcc();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string pid = db.getValue(dr, "Id");
                    Log.Info(Log.PRZESUNIECIA, ParentId, "Service.CheckPrzypisania3", Mailing.maPRZES_WN + ":" + pid, Log.OK);
                    Mailing.EventPrzesuniecie(Mailing.maPRZES_WN, pid);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Log.PRZESUNIECIA, ParentId, "Service.CheckPrzypisania", ex.Message);
            }
        }

        //------------------------------
        private void CheckWnioskiUrlopowe(SqlConnection con, int ParentId)
        {
        }

        //------------------------------
        private void CheckJobs(SqlConnection con, int ParentId)
        {
        }

        //------------------------------
        private void CheckSupervisorActions(SqlConnection con, int ParentId)
        {
            /*
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            string email = settings.GetSupervisorEMail(con);
            if (!String.IsNullOrEmpty(email))
            {
                Log.Info(Log.t2SERVICE, "Supervisor DB Export", null, Log.OK);
                Tools.DBMail(email);
            }
             */ 
        }
        //-------------------------------
        public static void CheckPodzialLudzi(SqlConnection con, int ParentId)   
        {
            try
            {
                PodzialLudzi.AutoImport(ParentId);
            }
            catch (Exception ex)
            {
                Log.Error(Log.PL_IMPORT, ParentId, "Podział Ludzi - ERROR", ex.Message);
            }
        }


        //---------------------------------------------------------------------
        // Mailing Auto
        //---------------------------------------------------------------------
        //public static void CheckExecAuto(SqlConnection con, int ParentId)   // też moze być wykorzystywany jako scheduler
        //{
        //    Log.Info(Log.EXECAUTO, ParentId, "Scheduler - START", null, Log.OK);
        //    DataSet ds = db.getDataSet(con, "select * from Scheduler where Aktywny = 1 order by Kolejnosc");
        //    foreach (DataRow drCmd in db.getRows(ds))
        //    {
        //        DataSet dsMails;

        //        // ZMIANA DB !!!!! Dorzucić kolumne ConStr
        //        // alter table scheduler add ConStr nvarchar(100)
        //        string conStr = db.getValue(drCmd, "ConStr");
        //        if (!string.IsNullOrEmpty(conStr))
        //        {
        //            SqlConnection c = db.Connect(conStr);
        //            dsMails = ExecAutoCommand(c, ParentId, drCmd);
        //            db.Disconnect(c);
        //        }
        //        else
        //        {
        //            dsMails = ExecAutoCommand(con, ParentId, drCmd);
        //        }
        //        if (dsMails != null)
        //            Mailing.SendMail3(dsMails);
        //    }
        //    Log.Info(Log.EXECAUTO, ParentId, "Scheduler - STOP", null, Log.OK);
        //}
        /*
        public static void CheckExecAuto(SqlConnection con, int ParentId)   // też moze być wykorzystywany jako scheduler
        {
            Log.Info(Log.EXECAUTO, ParentId, "Scheduler - START", null, Log.OK);
            DataSet ds = db.getDataSet(con, "select * from Scheduler where Aktywny = 1 order by Kolejnosc");
            foreach (DataRow drCmd in db.getRows(ds))
            {
                DataSet dsMails;
                string conStr = "";
                foreach (var item in drCmd.Table.Columns)
                {
                    string c = item.ToString();
                    if (c == "ConStr") 
                        conStr = db.getValue(drCmd, c);
                }
                if (!string.IsNullOrEmpty(conStr))
                {
                    SqlConnection c = db.Connect(conStr);
                    dsMails = ExecAutoCommand(c, ParentId, drCmd);
                    db.Disconnect(c);
                }
                else
                {
                    dsMails = ExecAutoCommand(con, ParentId, drCmd);
                }
                if (dsMails != null)
                    Mailing.SendMail3(dsMails);
            }
            Log.Info(Log.EXECAUTO, ParentId, "Scheduler - STOP", null, Log.OK);
        }
        */

        public static void CheckExecAuto(SqlConnection con, int ParentId)   // też moze być wykorzystywany jako scheduler
        {
            CheckExecAuto(con, ParentId, null);
        }

        public static void CheckExecAuto(SqlConnection con, int ParentId, string schId)   // też moze być wykorzystywany jako scheduler
        {
            Log.Info(Log.EXECAUTO, ParentId, "Scheduler - START", schId, Log.OK);            

            DataSet ds;
            if (String.IsNullOrEmpty(schId))
                ds = db.getDataSet(con, "select * from Scheduler where Aktywny = 1 order by Kolejnosc");
            else
                ds = db.getDataSet(con, String.Format("select * from Scheduler where Aktywny = 1 and Id = {0}", schId));

            foreach (DataRow drCmd in db.getRows(ds))
            {
                DataSet dsMails; 
                string conStr = "";
                foreach (var item in drCmd.Table.Columns)
                {
                    string c = item.ToString();
                    if (c == "ConStr")           // to jest dodatkowa kolumna w tabeli Scheduler , ale ze nie wszędzie jest założona więc takie zabezpieczenie ...
                        conStr = db.getValue(drCmd, c);
                }
                if (!string.IsNullOrEmpty(conStr))
                {
                    SqlConnection c = db.Connect(conStr);
                    dsMails = ExecAutoCommand(c, ParentId, drCmd);
                    db.Disconnect(c);
                }
                else
                {
                    dsMails = ExecAutoCommand(con, ParentId, drCmd);
                }
                if (dsMails != null)
                    Mailing.SendMail3(dsMails);    // <<<<<< tu może przygotować raport do csv i go wysłać
            }
            Log.Info(Log.EXECAUTO, ParentId, "Scheduler - STOP", null, Log.OK);
        }

        public static DataSet ExecAutoCommand(SqlConnection con, int ParentId, DataRow drCmd)
        {
            string id  = db.getValue(drCmd, "Id");
            //string typ = db.getValue(drCmd, "Typ");
            //string grupa = db.getValue(drCmd, "Grupa");
            string sql   = db.getValue(drCmd, "SQL");
            if (!String.IsNullOrEmpty(sql))
                try
                {
                    Log.Info(Log.EXECAUTO, ParentId, "Scheduler - EXEC", id, Log.OK);
                    DataSet dsMails = db.getDataSet(con, sql);
                    if (dsMails != null && dsMails.Tables.Count == 0)
                        return null;
                    else
                        return dsMails;
                }
                catch (Exception ex)
                {
                    Log.Error(Log.EXECAUTO, ParentId, "Scheduler - ERROR", ex.Message);
                }
            return null;
        }
        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
#if IPO
        private void UpdateWaluty(SqlConnection con, int ParentId)
        {


            try
            {
                string filePath1 = "http://www.nbp.pl/kursy/xml/LastA.xml";
                string filePath = db.getScalar("Select Parametr from kody where Typ = 'IPO_WALUTY' and Kod = 1");
                if (String.IsNullOrEmpty(filePath))
                    filePath = filePath1;

                if (!String.IsNullOrEmpty(filePath))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(filePath);

                    DateTime dataPublikacji = DateTime.Parse(ds.Tables[0].Rows[0].ItemArray[1].ToString());
                    String data = Tools.DateToStrDb(dataPublikacji);

                    var transaction = con.BeginTransaction();

                    try
                    {
                        System.Data.SqlClient.SqlCommand deleteCommand = new System.Data.SqlClient.SqlCommand();
                        deleteCommand = con.CreateCommand();
                        deleteCommand.Transaction = transaction;
                        deleteCommand.CommandText = "DELETE FROM IPO_Waluty";
                        deleteCommand.ExecuteNonQuery();
                        bool ok = db.insert("IPO_Waluty", 0, "Nazwa, Symbol, Przelicznik, Kurs, Lp, DataPublikacji",
                                db.strParam("Złoty"),
                                db.strParam("PLN"),
                                1,
                                1,
                                1,
                                db.strParam(data)
                                );
                        if (!ok)
                        {
                            Log.Error(Log.IPO_WALUTY, "Wystąpił błąd podczas ładowania tabeli kursów walut do DB", "Nieudane ładowanie PLN");
                        }
                        for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
                        {
                            string nazwaWaluty = ds.Tables[1].Rows[i].ItemArray[0].ToString();
                            int przelicznikWaluty = Int32.Parse(ds.Tables[1].Rows[i].ItemArray[1].ToString());
                            string symbolWaluty = ds.Tables[1].Rows[i].ItemArray[2].ToString();
                            decimal kursWaluty = Decimal.Parse(ds.Tables[1].Rows[i].ItemArray[3].ToString());

                            System.Data.SqlClient.SqlCommand insertCommand = new System.Data.SqlClient.SqlCommand();

                            insertCommand = con.CreateCommand();
                            insertCommand.Transaction = transaction;
                            insertCommand.CommandText = "INSERT INTO IPO_Waluty(Nazwa, Symbol, Przelicznik, Kurs, Lp, DataPublikacji) VALUES (@nazwaWaluty, @symbolWaluty, @przelicznikWaluty, @kursWaluty, @Lp, @dataPublikacji)";
                            if (symbolWaluty.Equals("EUR") || symbolWaluty.Equals("USD"))
                            {
                                insertCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@Lp", Value = "2" });
                            }
                            else
                            {
                                insertCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@Lp", Value = "3" });
                            }
                            insertCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@nazwaWaluty", Value = nazwaWaluty });
                            insertCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@symbolWaluty", Value = symbolWaluty });
                            insertCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@przelicznikWaluty", Value = przelicznikWaluty });
                            insertCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@kursWaluty", Value = kursWaluty });
                            insertCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@dataPublikacji", Value = dataPublikacji });
                            insertCommand.ExecuteNonQuery();
                            Log.InfoTr(Log.IPO_WALUTY, "Tabela z kursami walut została pomyślnie załadowana", null);
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Log.Error(Log.IPO_WALUTY, "Wystąpił błąd podczas ładowania tabeli kursów walut do DB", ex.Message);
                    }
                   
                }
                else
                    Log.Error(Log.IPO_WALUTY, "Brak adresu url");
            }
            catch (System.Net.WebException ex1)
            {
                Log.Error(Log.IPO_WALUTY, "Wystąpił błąd podczas pobierania kursów walut", ex1.Message);
            }
        }
#endif
        //------------------------------
    }
}
