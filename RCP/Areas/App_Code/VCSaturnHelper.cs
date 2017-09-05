using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*using HRRcp.SaturnVicimWS;*/
using HRRcp.VCSaturnService;
using System.Data.SqlClient;
using System.Data;

namespace HRRcp.App_Code
{

    public class BetterReturn
    {
        public int Kod { get; set; }
        public object Obiekt { get; set; }
        public BetterReturn(int _int, object _obj)
        {
            Kod = _int;
            Obiekt = _obj;
        }
    }
    public class VCSaturnHelper
    {
        public const int typLog = 99;
        public const string syncStanowiska = "Synchronizacja stanowisk";
        public const string syncPracownicy = "Synchronizacja pracowników";
        public const string syncKodyAbsencji = "Synchronizacja kodów absencji";
        public const string syncAbsencje = "Synchronizacja absencji";
        public const string syncGodziny = "Synchronizacja godzin";
        public const string syncBadania = "Synchronizacja badań";
        public const string syncSzkolenia = "Synchronizacja szkoleń";
        public const string syncUprawnienia = "Synchronizacja uprawnień";
        public const string syncKomorekOrganizacyjnych = "Synchronizacja komórek organizacyjnych";
        public const string syncMPK = "Synchronizacja MPK";

        public static void SynchronizacjaWszystkiego()
        {
            BetterReturn bt0 = VCSaturnHelper.SynchronizacjaStanowisk();
            BetterReturn bt7 = VCSaturnHelper.SynchronizacjaKomorekOrgazacyjnych();
            BetterReturn bt8 = VCSaturnHelper.SynchronizacjaMPK();
            BetterReturn bt1 = VCSaturnHelper.SynchronizacjaKodowAbsencji();
            BetterReturn bt2 = VCSaturnHelper.SynchronizacjaAbsencji();
            BetterReturn bt3 = VCSaturnHelper.SynchronizacjaPracownikow();
            BetterReturn bt4 = VCSaturnHelper.SynchronizacjaSlownikaGodzin();
            BetterReturn bt5 = VCSaturnHelper.SynchronizacjaBadan();
            BetterReturn bt6 = VCSaturnHelper.SynchronizacjaSzkolen();
        }

        public static BetterReturn SynchronizacjaMPK()
        {
            string retMsg = "";
            SaturnServiceSoapClient client = new SaturnServiceSoapClient();

            try
            {


                Log.Info(typLog, "START", syncMPK);
                Saturn.OrgCellsDataTable MPK = client.GetMPKWeb(ref retMsg);

                int i = 0;
                db.Execute("delete from IMPSAT_MPK");
                foreach (var item in MPK)
                {
                    i++;
                    db.CopyRecord(item, db.con, "IMPSAT_MPK");
                }

                Log.Info(typLog, string.Format("KONIEC zaimportowano {0} rekordów.", i), syncMPK);
            }
            catch (Exception e)
            {
                Log.Error(typLog, e.ToString(), syncMPK);
            }
            return new BetterReturn(0, string.Format("{0} - ok", syncMPK));
        }


        public static BetterReturn SynchronizacjaStanowisk()
        {
            string retMsg = "";
            SaturnServiceSoapClient client = new SaturnServiceSoapClient();

            try
            {


                Log.Info(typLog, "START", syncStanowiska);
                Slowniki.StanowiskaDataTable listaStanowisk = client.GetJobPositionsDict(true, ref retMsg);

                int i = 0;
                db.Execute("delete from IMPSAT_Stanowiska");
                foreach (var item in listaStanowisk)
                {
                    i++;
                    db.CopyRecord(item, db.con, "IMPSAT_Stanowiska");
                }

                Log.Info(typLog, string.Format("KONIEC zaimportowano {0} rekordów.", i), syncStanowiska);
            }
            catch (Exception e)
            {
                Log.Error(typLog, e.ToString(), syncStanowiska);
            }
            return new BetterReturn(0, string.Format("{0} - ok", syncStanowiska));
        }



        public static BetterReturn SynchronizacjaKomorekOrgazacyjnych()
        {
            string retMsg = "";
            SaturnServiceSoapClient client = new SaturnServiceSoapClient();

            try
            {


                Log.Info(typLog, "START", syncKomorekOrganizacyjnych);
                Slowniki.KomorkiOrgDataTable komorkiOrg = client.GetCellsDict(null, null, null, ref retMsg);


                int i = 0;
                db.Execute("delete from IMPSAT_KomorkiOrg");
                foreach (var item in komorkiOrg)
                {
                    i++;
                    db.CopyRecord(item, db.con, "IMPSAT_KomorkiOrg");
                }

                Log.Info(typLog, string.Format("KONIEC zaimportowano {0} rekordów.", i), syncKomorekOrganizacyjnych);
            }
            catch (Exception e)
            {
                Log.Error(typLog, e.ToString(), syncKomorekOrganizacyjnych);
            }
            return new BetterReturn(0, string.Format("{0} - ok", syncKomorekOrganizacyjnych));
        }

        public static BetterReturn SynchronizacjaPracownikow()
        {
            string retMsg = "";
            SaturnServiceSoapClient client = new SaturnServiceSoapClient();

            try
            {


                Log.Info(typLog, "START", syncPracownicy);
                WebService.EmployeesDataTable listaPracownikow = client.GetEmployees(DateTime.Now, null, ref retMsg);

                int i = 0;
                db.Execute("delete from IMPSAT_Pracownicy");
                foreach (var item in listaPracownikow)
                {
                    i++;
                    db.CopyRecord(item, db.con, "IMPSAT_Pracownicy");
                }

                Log.Info(typLog, string.Format("KONIEC zaimportowano {0} rekordów.", i), syncPracownicy);
            }
            catch (Exception e)
            {
                Log.Error(typLog, e.ToString(), syncPracownicy);
            }
            return new BetterReturn(0, string.Format("{0} - ok", syncPracownicy));
        }

        public static BetterReturn SynchronizacjaKodowAbsencji()
        {
            string retMsg = "";
            SaturnServiceSoapClient client = new SaturnServiceSoapClient();

            try
            {


                Log.Info(typLog, "START", syncKodyAbsencji);
                Slowniki.AbsencjeDataTable listaKodow = client.GetAbsDict(ref retMsg);

                int i = 0;
                db.Execute("delete from IMPSAT_AbsencjeDict");
                foreach (var item in listaKodow)
                {
                    i++;
                    db.CopyRecord(item, db.con, "IMPSAT_AbsencjeDict");
                }

                Log.Info(typLog, string.Format("KONIEC zaimportowano {0} rekordów.", i), syncKodyAbsencji);
            }
            catch (Exception e)
            {
                Log.Error(typLog, e.ToString(), syncKodyAbsencji);
            }
            return new BetterReturn(0, string.Format("{0} - ok", syncKodyAbsencji));
        }

        public static BetterReturn SynchronizacjaAbsencji()
        {
            string retMsg = "";
            SaturnServiceSoapClient client = new SaturnServiceSoapClient();

            try
            {
                /*int Mth = DateTime.Now.Month;
                int lastMont = DateTime.Now.AddMonths(-1).Month;

                int Yr = DateTime.Now.Year;
                int lastYr = DateTime.Now.AddMonths(-1).Year;

                Log.Info(typLog, "START", syncAbsencje);
                WebService.EmpAbsencesDataTable listaAbsencjiLast = client.GetEmpAbsences(lastMont, lastYr, null, ref retMsg);
                WebService.EmpAbsencesDataTable listaAbsencji = client.GetEmpAbsences(Mth, Yr, null, ref retMsg);


                int i = 0;
                db.Execute("delete from IMPSAT_Absencje");

                foreach (var item in listaAbsencjiLast)
                {
                    i++;
                    db.CopyRecord(item, db.con, "IMPSAT_Absencje");
                }
                foreach (var item in listaAbsencji)
                {
                    i++;
                    db.CopyRecord(item, db.con, "IMPSAT_Absencje");
                }



                Log.Info(typLog, string.Format("KONIEC zaimportowano {0} rekordów.", i), syncAbsencje);*/


                Log.Info(typLog, "START", syncAbsencje);
                int CurrentYear = DateTime.Now.Year;
                int j = 0;
                for (int i = 0; i < 12; ++i)
                {
                    WebService.EmpAbsencesDataTable listaAbsencji = client.GetEmpAbsences(i, CurrentYear, null, ref retMsg);
                    foreach (var item in listaAbsencji)
                    {
                        ++j;
                        db.CopyRecord(item, db.con, "IMPSAT_Absencje");
                    }
                }
                Log.Info(typLog, string.Format("KONIEC zaimportowano {0} rekordów.", j), syncAbsencje);

            }
            catch (Exception e)
            {
                Log.Error(typLog, e.ToString(), syncAbsencje);
            }
            return new BetterReturn(0, string.Format("{0} - ok", syncAbsencje));
        }

        public static BetterReturn SynchronizacjaBadan()
        {
            string retMsg = "";
            SaturnServiceSoapClient client = new SaturnServiceSoapClient();

            try
            {
                Log.Info(typLog, "START Słownik", syncBadania);
                Slowniki.BadaniaLekarskieSlownikDataTable listBadan = client.GetMedExamsDict(ref retMsg);
                Log.Info(typLog, "Koniec Słownik", retMsg);

                int i = 0;
                db.Execute("delete from IMPSAT_BadaniaDict");

                foreach (var item in listBadan)
                {
                    i++;
                    db.CopyRecord(item, db.con, "IMPSAT_BadaniaDict");
                }

                Log.Info(typLog, "START Przypisania", syncBadania);
                Kadry.BadaniaLekarskieDataTable badania = client.GetEmpMedicalExams(null, null, null, ref retMsg);
                Log.Info(typLog, "Koniec Słownik", retMsg);

                db.Execute("delete from IMPSAT_Badania");
                foreach (var item in badania)
                {
                    i++;
                    db.CopyRecord(item, db.con, "IMPSAT_Badania");
                }

                Log.Info(typLog, string.Format("KONIEC zaimportowano {0} rekordów.", i), syncBadania);
            }
            catch (Exception e)
            {
                Log.Error(typLog, e.ToString(), syncAbsencje);
            }
            return new BetterReturn(0, string.Format("{0} - ok", syncBadania));
        }

        public static BetterReturn SynchronizacjaSzkolen()
        {
            string retMsg = "";
            SaturnServiceSoapClient client = new SaturnServiceSoapClient();

            try
            {
                Log.Info(typLog, "START", syncSzkolenia);
                Slowniki.SzkoleniaSlownikDataTable listSzkolen = client.GetCoursesDict(ref retMsg);
                Log.Info(typLog, retMsg, syncSzkolenia);

                int i = 0;
                db.Execute("delete from IMPSAT_SzkoleniaDict");

                foreach (var item in listSzkolen)
                {
                    i++;
                    db.CopyRecord(item, db.con, "IMPSAT_SzkoleniaDict");
                }

                Log.Info(typLog, "START Przypisania", syncSzkolenia);
                Kadry.SzkoleniaDataTable szkolenia = client.GetEmpCourses(null, null, null, ref retMsg);
                Log.Info(typLog, retMsg, syncSzkolenia);

                db.Execute("delete from IMPSAT_Szkolenia");
                foreach (var item in szkolenia)
                {
                    i++;
                    db.CopyRecord(item, db.con, "IMPSAT_Szkolenia");
                }



                Log.Info(typLog, string.Format("KONIEC zaimportowano {0} rekordów.", i), syncSzkolenia);
            }
            catch (Exception e)
            {
                Log.Error(typLog, e.ToString(), syncAbsencje);
            }
            return new BetterReturn(0, string.Format("{0} - ok", syncSzkolenia));
        }

        public static BetterReturn SynchronizacjaSlownikaGodzin()
        {
            string retMsg = "";
            SaturnServiceSoapClient client = new SaturnServiceSoapClient();

            try
            {


                Log.Info(typLog, "START", syncGodziny);
                Slowniki.GodzinySlownikDataTable listaKodow = client.GetHoursDict(true, ref retMsg);

                int i = 0;
                db.Execute("delete from IMPSAT_GodzinyDict");
                foreach (var item in listaKodow)
                {
                    i++;
                    db.CopyRecord(item, db.con, "IMPSAT_GodzinyDict");
                }

                Log.Info(typLog, string.Format("KONIEC zaimportowano {0} rekordów.", i), syncGodziny);
            }
            catch (Exception e)
            {
                Log.Error(typLog, e.ToString(), syncGodziny);
            }
            return new BetterReturn(0, string.Format("{0} - ok", syncGodziny));
        }

        public static Boolean ExportPlan(Okres o)
        {
            Boolean go = true;
            SaturnServiceSoapClient client = new SaturnServiceSoapClient();
            String retMsg = String.Empty;

            Log.Add(0, 79636652, -1, App.User.Id, App.User.Login, null, null, null, null, null, "START", retMsg, 0);

            try
            {
                DataTable pp = db.Select.Table("select KadryId, Od, Do, Holiday from dbo.exportKPplan('{0}', '{1}')", o.DateFrom, o.DateTo);
                foreach (DataRow dr in pp.Rows)
                {
                    try
                    {
                        int contractId = client.GetEmpContractId(((DateTime)dr[1]).Date, (String)dr[0], ref retMsg).FirstOrDefault();
                        int contractCondId = client.GetEmpContractCondId(((DateTime)dr[1]).Date, (String)dr[0], ref retMsg).FirstOrDefault();
                        client.InsertEmpHarmonogram((string)dr[0], (DateTime)dr[1], (DateTime)dr[2], (Boolean)dr[3], contractId, ref retMsg);
                        Log.Add(0, 79636652, 0, App.User.Id, App.User.Login, null, null, null, null, null, (String)dr[0] + "|" + ((DateTime)dr[1]).ToString() + "|" + ((DateTime)dr[2]).ToString(), retMsg, 0);
                    }
                    catch
                    {
                        Log.Add(0, 79636652, 1, App.User.Id, App.User.Login, null, null, null, null, null, (String)dr[0] + "|" + ((DateTime)dr[1]).ToString() + "|" + ((DateTime)dr[2]).ToString(), retMsg, 0);
                        go = false;
                    }
                }
                Log.Add(0, 79636652, -2, App.User.Id, App.User.Login, null, null, null, null, null, "END", retMsg, 0);
            }
            catch (Exception e)
            {
                /* log? */
                Log.Add(0, 79636652, 2, App.User.Id, App.User.Login, null, null, null, null, null, "ERROR", retMsg, 0);
                go = false;
            }

            return go;
        }

        public static Boolean ExportRCP(Okres o)
        {
            Boolean go = true;
            SaturnServiceSoapClient client = new SaturnServiceSoapClient();
            String retMsg = String.Empty;

            Log.Add(0, 79636653, -1, App.User.Id, App.User.Login, null, null, null, null, null, "START", retMsg, 0);

            try
            {
                DataTable pp = db.Select.Table("select KadryId, Od, Do, IdGodziny, Godziny from dbo.exportKPRCP('{0}', '{1}')", o.DateFrom, o.DateTo);
                foreach (DataRow dr in pp.Rows)
                {
                    try
                    {
                        int contractId = client.GetEmpContractId(((DateTime)dr[1]).Date, (string)dr[0], ref retMsg).FirstOrDefault();
                        int contractCondId = client.GetEmpContractCondId(((DateTime)dr[1]).Date, (string)dr[0], ref retMsg).FirstOrDefault();
                        client.InsertEmpAddHours((int)dr[3], (string)dr[0], (DateTime)dr[1], (DateTime)dr[2], null, (decimal)dr[4], 0, contractId, contractCondId, ref retMsg);
                        Log.Add(0, 79636653, 0, App.User.Id, App.User.Login, null, null, null, null, null, (String)dr[0] + "|" + ((DateTime)dr[1]).ToString() + "|" + ((DateTime)dr[2]).ToString(), retMsg, 0);
                    }
                    catch
                    {
                        /*Log.Add(0, 79636652, null, App.User.Id, App.User.Login, "", null, null, null, null, "EXPORT - SATURN - ERR1", retMsg, 0);*/
                        Log.Add(0, 79636653, 1, App.User.Id, App.User.Login, null, null, null, null, null, (String)dr[0] + "|" + ((DateTime)dr[1]).ToString() + "|" + ((DateTime)dr[2]).ToString(), retMsg, 0);
                        go = false;
                    }
                }
                Log.Add(0, 79636653, -2, App.User.Id, App.User.Login, null, null, null, null, null, "END", retMsg, 0);
            }
            catch
            {
                /* log? */
                /*Log.Add(0, 79636652, null, App.User.Id, App.User.Login, "", null, null, null, null, "EXPORT - SATURN - ERR2", retMsg, 0);*/
                Log.Add(0, 79636653, 2, App.User.Id, App.User.Login, null, null, null, null, null, "ERROR", retMsg, 0);
                go = false;
            }

            return go;
        }

        public static void Test()
        {
            SaturnServiceSoapClient client = new SaturnServiceSoapClient();

            String emp = "1138";
            DateTime dt1 = new DateTime(2017, 06, 29);
            DateTime dt2 = new DateTime(2017, 06, 30, 8, 0, 05);
            DateTime dt3 = new DateTime(2017, 06, 30);

            String e = "";

            int[] c1 = client.GetEmpContractId(dt1,      emp, ref e);
            int[] c2 = client.GetEmpContractId(dt2.Date, emp, ref e);
            int[] c3 = client.GetEmpContractId(dt3.Date, emp, ref e);

            int[] c4 = client.GetEmpContractCondId(dt1,  emp, ref e);
            int[] c5 = client.GetEmpContractCondId(dt2,  emp, ref e);
            int[] c6 = client.GetEmpContractCondId(dt3,  emp, ref e);
        }

        /*public static BetterReturn EksportHarmonogramu()
        {
            string retMsg = "";
            SaturnServiceSoapClient client = new SaturnServiceSoapClient();

            try
            {
                DataSet impHarm = db.Select.Set("select * from dbo.exportKPdane ('20161001', '20161231', '20161001', '20161231')");
                foreach (DataRow item in impHarm.Tables[0].Rows)
                {
                    int contractId = client.GetEmpContractId(DateTime.Now, (string)item[1], ref retMsg).FirstOrDefault();
                    int contractCondId = client.GetEmpContractCondId(DateTime.Now, (string)item[1], ref retMsg).FirstOrDefault();
                    int dictHours = client.GetHoursDict(true, ref retMsg).FirstOrDefault().IdGodziny;


                    client.InsertEmpHarmonogram((string)item[1], (DateTime)item[2], (DateTime)item[3], false, contractId, ref retMsg);
                    client.InsertEmpAddHours(dictHours
                        , (string)item[1]
                        , (DateTime)item[2]
                        , (DateTime)item[3]
                        , null
                        , (decimal)item[4]
                        , 0, contractId
                        , contractCondId
                        , ref retMsg);

                }



            }
            catch (Exception e)
            {

            }
            return new BetterReturn(0, string.Format(" ok"));
        }*/



    }


}