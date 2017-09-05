using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Adm
{
    public partial class OkresyRozliczeniowe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HasFormAccess)   //App.User.HasAccess sprawdzane w MasterPage3
                {
                    Tools.SetNoCache();
                    //----------------------
                    PrepareConfirmButtons();
                }
                else
                    App.ShowNoAccess();
            }
        }

        public static bool HasFormAccess
        {
            get 
            {

                return db.SqlMenuHasRights(5140, App.User);
                //return App.User.IsAdmin; 
            }
        }

        // SKOPIOWANE Z ADMIN FORMA !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //
        //
        //
        //

        private void PrepareConfirmButtons()
        {
            Okres ok = Okres.FirstToClose(null);
            if (ok != null)
            {
                Button2.Text = String.Format("Zamknięcie miesiąca ({0})", Base.MonthName(ok.DateTo.Month));
                Tools.MakeConfirmButton(Button2, String.Format(
                    "Potwierdź zamknięcie okresu rozliczeniowego:\\n{0}.",
                    ok.OdDoStr));
            }
            else
            {
                ok = Okres.Current(null);
                Button2.Text = String.Format("Zamknięcie miesiąca ({0})", Base.MonthName(ok.DateTo.Month));
                Tools.MakeInfoButton(Button2, String.Format(
                    "Zamknięcie okresu rozliczeniowego {0} niemożliwe przed datą końca okresu.",
                    ok.OdDoStr));
            }

            Okres ok2 = Okres.LastClosed(null);
            Button4.Enabled = ok2 != null;
            if (ok2 != null)
            {
                Button4.Text = String.Format("Odblokowanie miesiąca ({0})", Base.MonthName(ok2.DateTo.Month));
                Tools.MakeConfirmButton(Button4, String.Format(
                    "Potwierdź odblokowanie okresu rozliczeniowego:\\n{0}.",
                    ok2.OdDoStr));
            }
            else
                Tools.MakeConfirmButton(Button4, null);
            //-------------------
            lbInfo.Text = "Okres rozliczeniowy zamknięty do: " +
                (ok.LockedTo < ok.DateFrom ? "odblokowany" : Tools.DateToStr(ok.LockedTo));
            /*
            lbInfo.Text += "<br />" + String.Format("{0} - {1}, {2}, {3} - {4}", 
                Tools.DateToStr(ok.DateFrom), 
                Tools.DateToStr(ok.DateTo), 
                Tools.DateToStr(ok.LockedTo), 
                Tools.DateToStr(ok.NextLockTo), 
                Tools.DateToStr(ok.UnlockTo)); /**/
            btWeekClose.Text = String.Format("Zamknięcie tygodnia do: {0}", Tools.DateToStr(ok.NextLockTo));
            btWeekClose.Enabled = ok.NextLockTo < DateTime.Today && ok.NextLockTo < ok.DateTo;
            Tools.MakeConfirmButton(btWeekClose, String.Format(
                "Potwierdź zamknięcie tygodnia do: {0}",
                Tools.DateToStr(ok.NextLockTo)));
            if (ok.LockedTo < ok.DateFrom)
            {
                //btWeekOpen.Text = String.Format("Odblokowanie tygodnia od: {0}", Tools.DateToStr(ok.DateFrom));
                btWeekOpen.Text = "Odblokowanie tygodnia";
                btWeekOpen.Enabled = false;
            }
            else
            {
                //btWeekOpen.Text = String.Format("Odblokowanie tygodnia od: {0}", Tools.DateToStr(ok.UnlockTo));
                btWeekOpen.Text = "Odblokowanie tygodnia";
                btWeekOpen.Enabled = true;
                Tools.MakeConfirmButton(btWeekOpen, String.Format(
                    //"Potwierdź odblokowanie tygodnia od: {0}",
                    "Potwierdź odblokowanie tygodnia.\\nOkres rozliczeniowy zostanie odblokowany od dnia: {0}.",
                    Tools.DateToStr(ok.UnlockTo)));
            }
            //---------------------
            /*
            ok = GetOkresToExport();
            Tools.MakeConfirmButton(Button5, String.Format("Potwierdź eksport danych czasu pracy za okres: {0}", ok.OdNalDoStr));
            ok.Next();
            Tools.MakeConfirmButton(Button1, String.Format("Potwierdź eksport planu pracy za okres: {0}", ok.OdDoStr));
            */
            Tools.MakeConfirmButton(Button6, "Potwierdź import danych z systemu Kadrowo-Płacowego.");
        }

        protected void ProgressTimer_Tick(object sender, EventArgs e)
        {
            ///ShowProgress(false);
        }
        //------------------------------------
        const string iprocAD = "iprocAD";
        const string iprocKP = "iprocKP";
        const string importInfo = "Trwa import danych.\\nProszę zobaczyć stan importu w podglądzie zdarzeń.";

        private bool ImportInProgress()
        {
            return Session[iprocAD] != null || Session[iprocKP] != null;  // powinno sie sprawdzać Log i czas, ale na razie tak wystarczy
        }

        protected void btImport_Click(object sender, EventArgs e)
        {
            /*
            //int cnt = App.ImportData_test();
            string cnt = App.ImportData_test();
            Label1.Text = cnt;
            */
            if (ImportInProgress()) Tools.ShowMessage(importInfo);
            else
            {
                int id1 = Log.Info(Log.APP_IMPORTAD, "Import AD", "", Log.PENDING);   // zmianna do statusu postępu AD
                Session[iprocAD] = App.ExecProcess(id1, 1);
                int id2 = Log.Info(Log.t2APP_IMPORTKP, "Import KP", "", Log.PENDING);
                Session[iprocKP] = App.ExecProcess(id2, 2);
                ///ShowProgress(true);
                //ProgressTimer.Enabled = true;
            }
        }

        /*
        protected void btImport_Click(object sender, EventArgs e)
        {
            int cnt = App.ImportData();
            cntPracToVerify.DataBind();
            if (cnt > 0)
            {
                cntPracownicy.List.DataBind();
                Tools.ShowMessage("Import danych zakończony poprawnie.\\nIlość nowych pracowników: " + cnt.ToString() + ".");
            }
            else Tools.ShowMessage("Wystąpił błąd podczas importu danych.");
        }
         */

        protected void btStart_Click(object sender, EventArgs e)
        {
            /*aa
            if (ImportInProgress()) Tools.ShowMessage(importInfo);
            else
            {
                bool ok = App.StartVerify();
                cntPracToVerify.DataBind();
                if (ok)
                {
                    cntPracownicy.List.DataBind();
                    Tools.ShowMessage("Uruchomienie weryfikacji przebiegło poprawnie.");
                }
                else Tools.ShowMessage("Wystąpił błąd podczas uruchamiania weryfikacji.");
            }
             */
        }

        protected void cntImportALARMUS1_ImportFinished(object sender, EventArgs e)
        {
            if (cntImportALARMUS1.UpdateReaders)
            {
                ///StrefyControl1.DataBind();
            }
        }

        protected void cntImportABSENCJA_ImportFinished(object sender, EventArgs e)
        {
        }

        //------------------------------------------------
        
        protected void ImportFinished(object sender, EventArgs e)
        {
            // err ?
            ///cntStruktura.PrepareOkres(null);
            //cntStruktura.Prepare(null);
            ///cntPracownicy.List.DataBind();
        }
       
        //----- KP -------------------------------------------
       
        public static bool importInProgress = false;  // to z opisu będzie instancja zmiennej widziana we wszystkich sesjach/procesach PRP

        protected void Button3_Click(object sender, EventArgs e)
        {
            if (!importInProgress)
                try
                {
                    importInProgress = true;

                    const string info = "Import danych z systemu KP";
                    int lid = Log.Info(Log.t2APP_IMPORTKP, info, null, Log.PENDING);

                    OdbcConnection conKP = Base.odbcConnect(KP.conStrKP);
                    SqlConnection con = Base.Connect();

                    int c1 = KP.ImportABSENCJA(conKP, con);
                    int c2 = KP.ImportKODYABS(conKP, con);
                    int c3 = KP.ImportKALENDAR(conKP, con);
                    int c4 = KP.ImportCZASNOM(conKP, con);
                    int c5 = KP.ImportSTAWKI(conKP, con);    // moze dac pozniej przed zamknieciem miesiaca zeby sie wykonalo ?
                    int c6 = KP.ImportZBIOR(conKP, con);

                    Base.Disconnect(con);
                    Base.Disconnect(conKP);

                    if (c1 >= 0 && c2 >= 0 && c3 >= 0 && c4 >= 0 && c5 >= 0 && c6 >= 0)
                    {
                        Log.Update(lid, Log.OK);
                        Log.Info(Log.t2APP_IMPORTKP, info, "zakończony OK", Log.OK);
                        Tools.ShowMessage("Import zakończony.");
                    }
                    else
                    {
                        Log.Update(lid, Log.ERROR);
                        Log.Error(Log.t2APP_IMPORTKP, info, String.Format("Wystapił błąd podczas importu danych, kody: {0} {1} {2} {3} {4} {5}", c1, c2, c3, c4, c5, c6), Log.OK);
                        Tools.ShowMessage("Wystąpił błąd podczas importu. Szczegóły błędu znajdują się w logu systemowym.");
                    }
                }
                finally
                {
                    importInProgress = false;
                }
            else
                Tools.ShowMessage("Trwa już import danych z systemu KP.\\n\\n" + App.User.Imie + ", poczekaj na jego zakończenie.");
        }

        //----- ASSECO ------------------------------------------
        protected void Button6_Click(object sender, EventArgs e)
        {
#if !DBW && !VICIM && !VC
    #if SIEMENS
            Komax.ImportAll();
    #else
            Asseco.ImportAll();
    #endif
#else
            try
            {
                db.execProc_2("execute rcp_kp_import");
            }
            catch
            {
                Tools.ShowError("Wystąpił błąd podczas importu danych z systemu KP.");
            }
#endif
        }

        /*
        protected void Button6_Click(object sender, EventArgs e)
        {
            if (!importInProgress)
                try
                {
                    importInProgress = true;

                    const string info = "Import danych z systemu Asseco HR";
                    int lid = Log.Info(Log.IMPORT_ASSECO, info, null, Log.PENDING);

                    SqlConnection conAsseco = db.Connect(Asseco.ASSECO);

                    int c1 = Asseco.ImportABSENCJA(conAsseco);
                    int c2 = Asseco.ImportKODYABS(conAsseco);
                    int c3 = Asseco.ImportKALENDAR(conAsseco);
                    int c4 = Asseco.ImportCZASNOM(conAsseco);
                    int c5 = Asseco.ImportSTAWKI(conAsseco);    // moze dac pozniej przed zamknieciem miesiaca zeby sie wykonalo ?
                    int c6 = Asseco.ImportZBIOR(conAsseco);

                    db.Disconnect(conAsseco);

                    if (c1 >= 0 && c2 >= 0 && c3 >= 0 && c4 >= 0 && c5 >= 0 && c6 >= 0)
                    {
                        Log.Update(lid, Log.OK);
                        Log.Info(Log.IMPORT_ASSECO, info, "zakończony OK", Log.OK);
                        Tools.ShowMessage("Import zakończony.");
                    }
                    else
                    {
                        Log.Update(lid, Log.ERROR);
                        Log.Error(Log.IMPORT_ASSECO, info, String.Format("Wystapił błąd podczas importu danych, kody: {0} {1} {2} {3} {4} {5}", c1, c2, c3, c4, c5, c6), Log.OK);
                        //Log.Error(Log.IMPORT_ASSECO, info, String.Format("Wystapił błąd podczas importu danych, kody: {0} {1}", c1, c2), Log.OK);
                        Tools.ShowMessage("Wystąpił błąd podczas importu. Szczegóły błędu znajdują się w logu systemowym.");
                    }
                }
                finally
                {
                    importInProgress = false;
                }
            else
                Tools.ShowMessage("Trwa już import danych.\\n\\n" + user.Imie + ", poczekaj na jego zakończenie.");
        }
        
         */




        public Okres GetOkresToExport()
        {
            //return new Okres(DateTime.Today.AddDays(-15));
            return new Okres(DateTime.Today.AddDays(-10));  // do 10 musza pojsc wypłaty wiec nie ma sensu dłuzej 
        }

        //-----------------------
        private static bool execProc(string proc, out int err, out string msg, params object[] par)     //  string logInfo, int mode, string dOd, string dDo, DateTime naDzien)   // mode: 1 (I), 2 (II), 3 (I+II)
        {
            int success = 0;
            string log = null;
            err = 0;
            msg = null;
            try
            {
                SqlCommand cmd = new SqlCommand(proc, db.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;

                for (int i = 0; i < par.Length; i++)
                {
                    object o = par[i];
                    log += String.Format(", {{{0}}}", i);
                    cmd.Parameters.AddWithValue(String.Format("p{0}", i + 1), par[i] ?? DBNull.Value);
                }
                if (!String.IsNullOrEmpty(log)) log = log.Substring(2);

                var perr = cmd.Parameters.Add("@err", SqlDbType.Int);
                perr.Direction = ParameterDirection.ReturnValue;
                var pmsg = cmd.Parameters.Add("@msg", SqlDbType.NVarChar, 2000);
                pmsg.Direction = ParameterDirection.ReturnValue;

                success = cmd.ExecuteNonQuery();  // 1 sukces
                err = (int)perr.Value;
                msg = (string)pmsg.Value;
                if (err != 0)
                    Log.Error(Log.SQL, String.Format("exec {0}({1}) err:{2} msg:{3}", proc, String.Format(log, par), err, msg));
                return err == 0;
            }
            catch (Exception ex)
            {
                err = -1;
                Log.Error(Log.SQL, String.Format("exec {0}({1}) err:{2} msg:{3}", proc, String.Format(log, par), err, msg), ex.Message);
                return false;
                //throw;
            }
        }

        private bool ExportKP(string proc, bool plan)
        {
            modalConfirmOkres.Close();  // może być tu

            //Tools.Delay(5000);   testy
            //Okres ok = GetOkresToExport();
            //if (plan) ok.Next();  // zawsze następny ... docelowo zmienić na pytanie o podanie daty ...
            string oid = ddlExpZaOkres.SelectedValue;
            Okres ok = new Okres(db.con, oid);

            if (ok != null)
            {
                bool go = true;
#if IQOR
                go = Asseco.ExportRCP(ok, false, true, true, true, true);
                if (go)
                    Tools.ShowMessage("Dane z okresu rozliczeniowego {0} zostały wyeksportowane poprawnie.", ok.OdDoStr);
                else
                    Tools.ShowMessage("Wystąpił błąd podczas eksportu danych z okresu rozliczeniowego:\\n{0}.", ok.OdDoStr);
                return go;
#elif VICIM
                go = (plan) ? VCSaturnHelper.ExportPlan(ok) : VCSaturnHelper.ExportRCP(ok);
                if (go)
                    Tools.ShowMessage("Dane z okresu rozliczeniowego {0} zostały wyeksportowane poprawnie.", ok.OdDoStr);
                else
                    Tools.ShowMessage("Wystąpił błąd podczas eksportu danych z okresu rozliczeniowego:\\n{0}.", ok.OdDoStr);
                return go;
#elif DBW
                int err = 0;
                string msg = null;
                string oddo;
                if (plan)
                {
                    oddo = ok.OdDoStr;
                }
                else
                {
                    //dod = ok.DataNaliczeniaPrev.AddDays(1);
                    //ddo = ok.DataNaliczenia;
                    //oddo = ok.OdDoNaliczeniaStr;
                    DateTime dod = ok.DataNaliczeniaPrev.AddDays(1);
                    DateTime ddo = ok.DateTo;
                    oddo = ok.OdNalDoStr;
                    try
                    {
                        Worktime2.GetRcpData(Tools.DateToStr(dod), Tools.DateToStr(ddo), null, null);
                    }
                    catch (Exception ex)
                    {
                        //Log!!!
                        err = -2;
                        go = false;
                        Log.Error(Log.EXPORT_ASSECO, String.Format("ExportKP.GetRcpData({0}, {1})", Tools.DateToStr(dod), Tools.DateToStr(ddo)), ex.Message, Log.OK);
                    }
                }
                if (go) go = execProc(proc, out err, out msg, ok.DateFrom, ok.DateTo, ok.DataNaliczeniaPrev.AddDays(1), ok.DataNaliczenia);
                if (go)
                    Tools.ShowMessage("Dane z okresu {0} zostały wyeksportowane poprawnie.\\n{1}", oddo, msg);
                else
                    Tools.ShowMessage("Wystąpił błąd podczas eksportu danych z okresu:\\n{0}, kod błędu: {1}.\\n{2}", oddo, err, msg);
                return go;
#else
                return false;
#endif
            }
            else
            {
                //Tools.ShowMessage("Brak zamkniętego okresu rozliczeniowego.");
                Tools.ShowMessage("Nieprawidłowy okres rozliczeniowy.");
                return false;
            }
        }
        
        protected void Button1_Click(object sender, EventArgs e)
        {
            //ExportKP("exportKPplan", true);
            ShowConfirmExport(true);
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            //ExportKP("exportKP", false);
            ShowConfirmExport(false);
        }

        private void ShowConfirmExport(bool plan)
        {
            lbExpZaOkresPlan.Visible = plan;
            lbExpZaOkresAcc.Visible = !plan;
            btExpPlan.Visible = plan;
            btExpAcc.Visible = !plan;

            Okres ok = GetOkresToExport();
            if (plan) ok.Next();
            //Tools.SelectItemByParam(ddlExpZaOkres, 0, ok.Id.ToString());
            if (ddlExpZaOkres.Items.Count == 0) ddlExpZaOkres.DataBind();
            Tools.SelectItem(ddlExpZaOkres, ok.Id.ToString());

            modalConfirmOkres.Show(false);
        }

        protected void btExpPlan_Click(object sender, EventArgs e)
        {
            ExportKP("exportKPplan", true);
        }

        protected void btExpAcc_Click(object sender, EventArgs e)
        {
            ExportKP("exportKP", false);
        }

        protected void cntOkresy_Changed(object sender, EventArgs e)
        {
            ddlExpZaOkres.DataBind();
        }

        //------------------------------------
        private string GetKierNotClosedSql()
        {
#if DBW || IQOR || VICIM
            return dsGetKierNotClosed.SelectCommand;
#else
            return dsGetKierNotClosedOld.SelectCommand;
#endif
        }

        Boolean CheckClosedShowReport(DateTime Od, DateTime Do, String IdKierownika)
        {
            

            return false;
        }

        protected void Button2_Click(object sender, EventArgs e)    // zamykanie miesiąca
        {
            Okres ok = Okres.FirstToClose(null);
            if (ok != null)
            {
#if DBW || VICIM
                int Closed = cntCheckOkresClosed.CheckShow(ok.DataNaliczeniaPrev.AddDays(1), ok.DataNaliczenia, "0");
                
                /*
                Boolean Closed = false;
                DataSet ds = db.Select.Set(GetKierNotClosedSql(), Tools.DateToStrDb(ok.DataNaliczeniaPrev), hidData.Value = Tools.DateToStrDb(ok.DateTo), 0);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    cntReport.SQL = String.Format(GetKierNotClosedSql(), Tools.DateToStrDb(ok.DateTo), Tools.DateToStrDb(ok.DataNaliczeniaPrev), 0);
                    cntModal.Show(false);
                }
                else Closed = true;
                */

                if (Closed == -1)
                    Tools.ShowError("Wystąpił błąd podczas uruchamiania procedury sprawdzającej.");

                if (Closed == 0)
#else
                if (ok.CheckClose(ok.DateTo, GetKierNotClosedSql()))
#endif
                    if (ok._Close())
                    {
                        Tools.ShowMessage("Okres rozliczeniowy {0} został zamknięty.", ok.OdDoStr);
                        cntOkresy.DataBind();
                        PrepareConfirmButtons();
                    }
                    else
                        Tools.ShowMessage("Wystąpił błąd podczas zamykania okresu rozliczeniowego:\\n{0}.", ok.OdDoStr);
            }
            else
            {
                ok = Okres.Current(null);
                Tools.ShowMessage("Zamknięcie okresu rozliczeniowego {0} niemożliwe przed datą końca okresu.", ok.OdDoStr);
            }
        }

        protected void Button4_Click(object sender, EventArgs e)    // odblokowywanie miesiąca
        {
            Okres ok = Okres.LastClosed(null);
            if (ok != null)
                if (ok._Reopen())
                {
                    Tools.ShowMessage("Okres rozliczeniowy {0} został odblokowany.", ok.OdDoStr);
                    cntOkresy.DataBind();
                    PrepareConfirmButtons();
                }
                else
                    Tools.ShowMessage("Wystąpił błąd podczas odblokowywania okresu rozliczeniowego:\\n{0}.", ok.OdDoStr);
            else Tools.ShowMessage("Brak zamkniętego okresu rozliczeniowego.");
        }
        //--------------------
        protected void btWeekClose_Click(object sender, EventArgs e)    // zamknięcie/zablokowanie tygodnia
        {
            Okres ok = Okres.FirstToClose(null);
            if (ok == null) ok = Okres.Current(null);
            DateTime dt = ok.NextLockTo;

#if DBW || VICIM
            int Closed = cntCheckOkresClosed.CheckShow(ok.DataNaliczeniaPrev.AddDays(1), dt, "0");
            /*DataSet ds = db.Select.Set(GetKierNotClosedSql(), hidData.Value = Tools.DateToStrDb(dt), Tools.DateToStrDb(ok.DataNaliczeniaPrev), 0);
            if (ds.Tables[0].Rows.Count == 0)
            {
                cntReport.SQL = GetKierNotClosedSql();
                cntModal.Show(false);
            }
            else Closed = true;
            */

            if (Closed == -1)
                Tools.ShowError("Wystąpił błąd podczas uruchamiania procedury sprawdzającej.");

            if (Closed == 0)
#else
            if (ok.CheckClose(dt, GetKierNotClosedSql()))
#endif
                if (ok.SetLockedTo(dt))
                {
                    bool go = true;
#if IQOR
                    go = Asseco.ExportRCP(ok, true, true, true, true, false);  // bez struktury
#elif DBW || VICIM
                    go = true;  // <<<<<<, tu uruchomienie eksportu
#endif
                    if (go)
                    {
                        Tools.ShowMessage("Okres rozliczeniowy {0} został zamknięty do dnia: {1}.", ok.OdDoStr, Tools.DateToStr(ok.LockedTo));
                        cntOkresy.DataBind();
                        PrepareConfirmButtons();
                    }
                    else Tools.ShowMessage("Wystąpił błąd podczas eksportu danych z okresu rozliczeniowego:\\n{0}.", ok.OdDoStr);
                }
                else Tools.ShowMessage("Wystąpił błąd podczas zamykania tygodnia:\\n{0} do dnia: {1}.", ok.OdDoStr, Tools.DateToStr(dt));
        }

        protected void btWeekOpen_Click(object sender, EventArgs e)     // odblokowywanie tygodnia
        {
            Okres ok = Okres.FirstToClose(null);
            if (ok == null) ok = Okres.Current(null);
            DateTime dt = ok.UnlockTo;
            if (ok.SetLockedTo(dt.AddDays(-1)))
            {
                Tools.ShowMessage("Okres rozliczeniowy {0} został odblokowany od dnia: {1}.", ok.OdDoStr, Tools.DateToStr(ok.LockedTo.AddDays(1)));
                cntOkresy.DataBind();
                PrepareConfirmButtons();
            }
            else
                Tools.ShowMessage("Wystąpił błąd podczas odblokowywania tygodnia:\\n{0} od dnia: {1}.", ok.OdDoStr, Tools.DateToStr(dt));
        }
        //-----------------------------------------

        protected void btAbsDl_Click(object sender, EventArgs e)     // odblokowywanie tygodnia
        {
            AbsencjeDlugotrwale.Show();
        }

        //-----------------------
        protected void btPodzial_Click(object sender, EventArgs e)
        {
            App.Redirect(App.PodzialLudziAdmForm);
        }

    }
}