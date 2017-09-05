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
using System.Web.Services;

namespace HRRcp
{
    public partial class test : System.Web.UI.Page
    {
        AppUser user;

        protected void Page_Load(object sender, EventArgs e)        
        {
            user = AppUser.CreateOrGetSession();
            if (!IsPostBack)
            {
                if (user.HasAccess && user.IsAdmin)
                {
                    Log.Info(Log.t2APP_INFO, "test.aspx", null);
                    //string S = null;
                    ddlLogins.DataSource = Base.getDataSet(@"
select Nazwisko + ' ' + Imie + ISNULL(' ' + Login, '') as Nazwisko, Login 
from Pracownicy 
--where Kierownik=1
where Status != -1
--order by Login
order by DataZwol, Kierownik desc, Nazwisko
                    ");
                    ddlLogins.DataTextField = "Nazwisko";
                    ddlLogins.DataValueField = "Login";
                    ddlLogins.DataBind();
                    string login = user.Login;
                    if (ddlLogins.Items.Count > 0)
                    {
                        ListItem li = ddlLogins.Items.FindByValue(login);
                        if (li == null) li = ddlLogins.Items.FindByValue("WojciowT");
                        if (li == null) li = ddlLogins.Items[0];
                        if (li != null)
                        {
                            li.Selected = true;
                            //user.Login = li.Value;
                            //S = li.Text;
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
                }
                else App.ShowNoAccess("test.aspx", user);




                //Button14.Attributes["onclick"] = "btclick(this);return false;";
            }
            else
            {
                int x = 0;
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

        protected void Button9_Click(object sender, EventArgs e)
        {
            OdbcConnection conKP = Base.odbcConnect(KP.conStrKP);
            SqlConnection con = Base.Connect();

            KP.ImportNickPass(conKP, con);

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
            if (ok._CheckClose(ok.DateTo))   // !!! po starmu 
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

        protected void LinkButton5_Click(object sender, EventArgs e)
        {
            App.LoginAsUser(ddlLogins.SelectedValue);
        }

        protected void Button10_Click(object sender, EventArgs e)  // zamknij okres bez sprawdzania
        {
            Tools.ShowMessage("test z ' apostrofem");
        }

        protected void Button11_Click(object sender, EventArgs e)  // zamknij okres bez sprawdzania
        {
            Tools.ShowMessage(Tools.ToScript("test z ' apostrofem"));
        }







        //----------------------------------


        [WebMethod]
        public static string GetCurrentDate(string par)
        {
            return DateTime.Now.ToLongDateString() + par;
        }

        protected void btAbsDl_Click(object sender, EventArgs e)
        {
            AbsencjeDlugotrwale.Show();
        }

        protected void sattest_Click(object sender, EventArgs e)
        {
            /*
            SaturnService.SaturnServiceSoapClient sssc = new SaturnService.SaturnServiceSoapClient();
            sssc.Open();

            String err = null;

            SaturnService.WebService.EmployeesDataTable a = sssc.GetEmployee(null, "2", ref err);
            sssc.Close();
             */
        }



        //----------------------------------
    }
}
