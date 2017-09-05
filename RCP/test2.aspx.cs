using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.IO;

using System.DirectoryServices;
using System.Net;

using HRRcp.App_Code;

namespace HRRcp
{
    public partial class test2 : System.Web.UI.Page
    {
        AppUser user;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = AppUser.CreateOrGetSession();
            if (!IsPostBack)
            {
                if (user.HasAccess && user.IsAdmin)
                {
                    Log.Info(Log.t2APP_INFO, "test2.aspx", null);

                    cntSplityWsp1.IdPrzypisania = "21495";

                    string S = null;
                    ddlLogins.DataSource = Base.getDataSet("select Login + ' - ' + Nazwisko + ' ' + Imie as Nazwisko, Login from Pracownicy order by Login");
                    ddlLogins.DataTextField = "Nazwisko";
                    ddlLogins.DataValueField = "Login";
                    ddlLogins.DataBind();
                    string login = user.Login;
                    if (ddlLogins.Items.Count > 0)
                    {
                        ListItem li = ddlLogins.Items.FindByValue(login);
                        if (li == null) li = ddlLogins.Items.FindByValue("WojciowT");
                        if (li == null) li = ddlLogins.Items.FindByValue("tomekw");
                        if (li == null) li = ddlLogins.Items[0];
                        if (li != null)
                        {
                            li.Selected = true;
                            user.Login = li.Value;
                            S = li.Text;
                        }
                    }
                    cntSelectOkres.Prepare(DateTime.Today, true);
                    DataSet ds = Base.getDataSet(
                        "select 'Wszyscy kierownicy' as NI, -1 as Id, 0 as Sort union " +
                        "select Nazwisko + ' ' + Imie as NI, Id, 1 as Sort from Pracownicy where Kierownik = 1 union " +
                        "select 'Poziom główny struktury' as NI, 0 as Id, 2 as Sort " +
                        "order by Sort, NI");
                    Tools.BindData(ddlKierownicy, ds, "NI", "Id", true, null);
                    Tools.MakeConfirmButton(Button8, "UWAGA !!!\\nSPRAWDŹ OKRES, KTÓRY ZAMYKASZ NA NAWIGATORZE POWYŻEJ !!!\\n\\nPotwierdź zamknięcie miesiąca bez sprawdzenia.");
                    Tools.MakeConfirmButton(Button12, "Potwierdź import");
                    Tools.MakeConfirmButton(Button14, "Potwierdź export");
                    Tools.MakeConfirmButton(Button16, "Potwierdź export");
                    Tools.MakeConfirmButton(Button15, "Potwierdź import");

                    Tools.MakeConfirmButton(Button11, "Potwierdź export");
                    Tools.MakeConfirmButton(Button13, "Potwierdź export");
                }
                else App.ShowNoAccess("test.aspx", user);
            }
        }
        //----------------------------------------
        protected void ddlUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //user.Login = ddlLogins.SelectedValue;
            App.LoginAsUser(ddlLogins.SelectedValue);
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            App.ShowNoAccess("test", null);
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("LoginForm.aspx");
        }


        //----------------------------------------

        protected void Button21_Click(object sender, EventArgs e)
        {
            int cnt = ROGER.ImportROGER();
            Tools.ShowMessage("Import zakończony.");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ROGER.ImportReaders();
            Tools.ShowMessage("Import zakończony.");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            OdbcConnection conKP = Base.odbcConnect(KP.conStrKP);
            SqlConnection con = Base.Connect();

            KP.ImportPROJECTS(conKP, con);
            KP.ImportPracownicy(con);
            KP.UpdatePracownicyRcpId(conKP, con);
            KP.UpdatePracownicyPRP(con);

            Base.Disconnect(con);
            Base.Disconnect(conKP);
            Tools.ShowMessage("Import zakończony.");
        }

        protected void Button4_Click(object sender, EventArgs e) // update RcpId z KP
        {
            OdbcConnection conKP = Base.odbcConnect(KP.conStrKP);
            SqlConnection con = Base.Connect();

            KP.UpdatePracownicyRcpId(conKP, con);

            Base.Disconnect(con);
            Base.Disconnect(conKP);
            Tools.ShowMessage("Import zakończony.");
        }

        protected void Button5_Click(object sender, EventArgs e) // update RcpId z ROGER
        {
            ROGER.UpdateRcpId();
            Tools.ShowMessage("Import zakończony.\\nNastępnie: przypisanie i weryfikacja w pliku:\\nImport RcpId z ROGERów.sql");
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            OdbcConnection conKP = Base.odbcConnect(KP.conStrKP);
            SqlConnection con = Base.Connect();

            KP.ImportABSENCJA(conKP, con);
            KP.ImportKODYABS(conKP, con);
            KP.ImportKALENDAR(conKP, con);

            Base.Disconnect(con);
            Base.Disconnect(conKP);
            Tools.ShowMessage("Import zakończony.");
        }

        protected void FormView1_PageIndexChanging(object sender, FormViewPageEventArgs e)
        {

        }

        protected void Button7_Click(object sender, EventArgs e)  // spr mozliwosci zamknięcie miesiąca
        {
            Okres ok = new Okres(Base.StrToDateTime(cntSelectOkres.DateTo));
            if (ok._CheckClose(ok.DateTo))
                Tools.ShowMessage("Okres gotowy do zamknięcia.");
            else
                Tools.ShowMessage("Nie można zamknąć okresu.");

        }

        protected void Button6_Click(object sender, EventArgs e)  // analiza zatrzaśniętych wartości
        {
            Okres ok = new Okres(Base.StrToDateTime(cntSelectOkres.DateTo));
            StringWriter sw = Report.StartExportTXT("rcp analiza.txt");
            DataSet ds;
            string kid = ddlKierownicy.SelectedValue;
            if (kid == "-1")
            {
                if (ok.IsArch())
                    ds = Base.getDataSet("select Id, KadryId, Nazwisko + ' ' + Imie as KierNI from PracownicyOkresy where IdOkresu = " + ok.Id + " and Kierownik = 1");
                else
                    ds = Base.getDataSet("select Id, KadryId, Nazwisko + ' ' + Imie as KierNI from Pracownicy where Kierownik = 1");
            }
            else
            {
                if (ok.IsArch())
                    ds = Base.getDataSet("select Id, KadryId, Nazwisko + ' ' + Imie as KierNI from PracownicyOkresy where IdOkresu = " + ok.Id + " and Id = " + kid);
                else
                    ds = Base.getDataSet("select Id, KadryId, Nazwisko + ' ' + Imie as KierNI from Pracownicy where Id = " + kid);
            }

            foreach (DataRow dr in Base.getRows(ds))
            {
                sw.WriteLine(String.Format("{0} {1}", Base.getValue(dr, "KadryId"), Base.getValue(dr, "KierNI")));
                string kierId = Base.getValue(dr, "Id");
                KierParams kp = new KierParams(kierId);
                Okres._Akceptuj(kierId, null, null, kp, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, null, null,
                               true, false, sw, false);
            }
            Report.EndExportTXT(sw);
        }

        protected void Button10_Click(object sender, EventArgs e)  // aktualizacja alertów i n50 n100 na pp
        {
            Okres ok = new Okres(Base.StrToDateTime(cntSelectOkres.DateTo));
            DataSet ds;
            string kid = ddlKierownicy.SelectedValue;
            if (kid == "-1")
            {
                if (ok.IsArch())
                    ds = Base.getDataSet("select Id, KadryId, Nazwisko + ' ' + Imie as KierNI from PracownicyOkresy where IdOkresu = " + ok.Id + " and Kierownik = 1");
                else
                    ds = Base.getDataSet("select Id, KadryId, Nazwisko + ' ' + Imie as KierNI from Pracownicy where Kierownik = 1");
            }
            else
            {
                if (ok.IsArch())
                    ds = Base.getDataSet("select Id, KadryId, Nazwisko + ' ' + Imie as KierNI from PracownicyOkresy where IdOkresu = " + ok.Id + " and Id = " + kid);
                else
                    ds = Base.getDataSet("select Id, KadryId, Nazwisko + ' ' + Imie as KierNI from Pracownicy where Id = " + kid);
            }

            foreach (DataRow dr in Base.getRows(ds))
            {
                string kierId = Base.getValue(dr, "Id");
                KierParams kp = new KierParams(kierId);
                Okres._Akceptuj(kierId, null, null, kp, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, null, null,
                               false,
                               true,
                               null,
                               false);
            }
            Tools.ShowMessage("Koniec.");
        }

        protected void Button8_Click(object sender, EventArgs e)  // zamknij okres bez sprawdzania
        {
            Okres ok = new Okres(Base.StrToDateTime(cntSelectOkres.DateTo));
            if (ok._Close())
            {
                Tools.ShowMessage("Okres rozliczeniowy {0} został zamknięty.", ok.OdDoStr);
            }
            else
                Tools.ShowMessage("Wystąpił błąd podczas zamykania okresu rozliczeniowego:\\n{0}.", ok.OdDoStr);
        }

        protected void Button9_Click(object sender, EventArgs e)  // zamknij okres bez sprawdzania
        {
            Okres ok = new Okres(Base.StrToDateTime(cntSelectOkres.DateTo));
            if (ok != null)
                if (Asseco.ExportRCP(ok, false, cbDaneRCP.Checked, cbSumyRCP.Checked, cbDaneMPK.Checked, cbStruct.Checked))
                    Tools.ShowMessage("Dane z okresu rozliczeniowego {0} zostały wyeksportowane poprawnie.", ok.OdDoStr);
                else
                    Tools.ShowMessage("Wystąpił błąd podczas eksportu danych z okresu rozliczeniowego:\\n{0}.", ok.OdDoStr);
            else Tools.ShowMessage("Brak okresu rozliczeniowego.");
        }

        protected void btTestMailingOkres_Click(object sender, EventArgs e)
        {
            Service.CheckOkres(db.con, 0, (DateTime)deDay.Date, true);
        }

        protected void Button22_Click(object sender, EventArgs e)
        {
            Mailing.SendMail2("tomasz_wojciow@jabil.com", "tomasz_wojciow@jabil.com", "tomasz_wojciow@jabil.com", "testowy mail z cc i bcc", "testowy mail z cc i bcc - treść");
        }

        protected void Button11_Click(object sender, EventArgs e)  // spr mozliwosci zamknięcie miesiąca
        {
            Worktime2.GetRcpData(cntSelectOkres.DateFrom, cntSelectOkres.DateTo, null, null);
            Tools.ShowMessage("Koniec");
        }

        protected void Button13_Click(object sender, EventArgs e)  // spr mozliwosci zamknięcie miesiąca
        {
            Worktime2.GetRcpAnalize(cntSelectOkres.DateFrom, cntSelectOkres.DateTo, dsRcpAnalize.SelectCommand, "tmpRcpAnalize", null);
            Tools.ShowMessage("Koniec");
        }
        //---------------------
        protected void Button12_Click(object sender, EventArgs e)  // spr mozliwosci zamknięcie miesiąca
        {
            try
            {
                DateTime dt = DateTime.Parse(ddlRoki.SelectedValue + "-01-01");

                const string info = "test2.Import danych z systemu Asseco HR";
                int lid = Log.Info(Log.IMPORT_ASSECO, info, null, Log.PENDING);

                SqlConnection conAsseco = db.Connect(Asseco.ASSECO);

                //int c1 = Asseco.ImportABSENCJA(conAsseco);
                //int c2 = Asseco.ImportKODYABS(conAsseco);
                //int c3 = Asseco.ImportKALENDAR(conAsseco);
                //int c4 = Asseco.ImportCZASNOM(conAsseco);
                //int c5 = Asseco.ImportSTAWKI(conAsseco);    // moze dac pozniej przed zamknieciem miesiaca zeby sie wykonalo ?
                int c6 = Asseco.ImportZBIOR(conAsseco, dt);
                //int c7 = Asseco.ImportUMOWY(conAsseco);       
                int c8 = Asseco.ImportLIMITY(conAsseco);        //20160417 włączone, tu się aktualizaują limity dodatkowego urlopu zaległego
                //int c9 = Asseco.ImportSTANOWISKA(conAsseco);

                db.Disconnect(conAsseco);
                if (c6 >= 0 && c8 >= 0)// && ok1)
                {
                    Log.Update(lid, Log.OK);
                    Log.Info(Log.IMPORT_ASSECO, info, "zakończony OK", Log.OK);
                    Tools.ShowMessage("Import zakończony.");
                }
                else
                {
                    Log.Update(lid, Log.ERROR);
                    Log.Error(Log.IMPORT_ASSECO, info, String.Format("Wystąpił błąd podczas importu danych, kody: {0} {1}", c6, c8), Log.OK);
                    //Log.Error(Log.IMPORT_ASSECO, info, String.Format("Wystapił błąd podczas importu danych, kody: {0} {1}", c1, c2), Log.OK);
                    Tools.ShowMessage("Wystąpił błąd podczas importu. Szczegóły błędu znajdują się w logu systemowym.");
                }
            }
            finally
            {
            }
        }
        //-----
        protected void Button14_Click(object sender, EventArgs e)  // zamknij okres bez sprawdzania
        {
            Okres ok = new Okres(Base.StrToDateTime(cntSelectOkres.DateTo));
            if (ok != null)
                if (Asseco.ExportRCP(ok, false, false, false, true, false))  // tylko DaneMPK
                    Tools.ShowMessage("Dane z okresu rozliczeniowego {0} zostały wyeksportowane poprawnie.", ok.OdDoStr);
                else
                    Tools.ShowMessage("Wystąpił błąd podczas eksportu danych z okresu rozliczeniowego:\\n{0}.", ok.OdDoStr);
        }
        //-----
        protected void Button16_Click(object sender, EventArgs e)  // zamknij okres bez sprawdzania
        {
            Okres ok = new Okres(Base.StrToDateTime(cntSelectOkres.DateTo));
            if (ok != null)
                if (Asseco.ExportRCP(ok, true, true, true, true, false))
                    Tools.ShowMessage("Dane z okresu rozliczeniowego {0} zostały wyeksportowane poprawnie.", ok.OdDoStr);
                else
                    Tools.ShowMessage("Wystąpił błąd podczas eksportu danych z okresu rozliczeniowego:\\n{0}.", ok.OdDoStr);
        }
        //-----
        protected void Button15_Click(object sender, EventArgs e)  // zamknij okres bez sprawdzania
        {
            PodzialLudzi.AutoImport(0);
        }

        protected void Button17_Click(object sender, EventArgs e)
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

            /*BetterReturn bt5 = VCSaturnHelper.EksportHarmonogramu();*/
        }

        protected void Button18_Click(object sender, EventArgs e)
        {
            VCSaturnHelper.Test();
        }
    }
}
