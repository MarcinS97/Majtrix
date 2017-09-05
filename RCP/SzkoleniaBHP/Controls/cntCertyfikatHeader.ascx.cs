using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.SzkoleniaBHP.Controls
{
    public partial class cntCertyfikatHeader : System.Web.UI.UserControl
    {
        const int moPracownicy = 0;
        const int moSpawacze = 1;

        int FMode = moPracownicy;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Szkoleniap)
                //{
                //    Label1.Text = "Szkolenie";
                //    Label4.Text = "Szkolenia";
                //}
                //if (Statusyp)
                //{
                //    //Label1.Text = "Status samokontroli";
                //    Label1.Visible = false;
                //    lbUprawnienie.Visible = false;
                //    tdUprawnienie.Visible = false;
                //    //Label4.Text = "Ważności statusów";
                //}
                TranslatePage();
            }
        }

        private void TranslatePage()
        {
            L.p(Label1);
            L.p(Label2);
            L.p(Label3);
            L.p(Label4);
            L.p(Label5);
            L.p(Label6);
            L.p(Label7);
            L.p(Label8);
        }

        public static string GetPola(string typ)
        {
            return db.getScalar(String.Format("select top 1 Pola from Uprawnienia where Typ = {0} and Aktywne = 1 order by Kolejnosc", typ));
        }

        public void Prepare(string typ, string uprId, string pracId, out string pola)
        {
            DataRow drU;
            if (String.IsNullOrEmpty(uprId))
                drU = db.getDataRow(String.Format("select top 1 * from Uprawnienia where Typ = {0} and Aktywne = 1 order by Kolejnosc", typ));
            else
                drU = db.getDataRow("select * from Uprawnienia where Id = " + uprId);
            SymbolUprawnienia = db.getValue(drU, "Symbol");
            NazwaUprawnienia = db.getValue(drU, "Nazwa");
            OkresWaznosciTyp = db.getValue(drU, "OkresWaznosciTyp");
            OkresWaznosci = db.getInt(drU, "OkresWaznosci", -1);
            lbUprawnienie.Text = String.Format("{0} - {1}", db.getValue(drU, "Symbol"), db.getValue(drU, L.Lang == L.lngPL ? "Nazwa" : "NazwaEN"));
            //string typ = db.getValue(drU, "Typ");
            typ = db.getValue(drU, "Typ");
            if (typ == cntUprawnienia.utSzkolenia)
            {
                Label1.Text = L.p("Szkolenie");
                Label4.Text = L.p("Szkolenia");
            }

            bool s = FMode == moSpawacze;
            pola = db.getValue(drU, "Pola");
            //DataRow drP = db.getDataRow("select Nazwisko + ' ' + Imie as Pracownik, * from Pracownicy where Id_Pracownicy = " + pracId);

            DataRow drP = null;
            if (!String.IsNullOrEmpty(pracId))
            {
                drP = db.getDataRow("select Nazwisko + ' ' + Imie as Pracownik, * from Pracownicy where Id = " + pracId);
                lbPracownik.Text = db.getValue(drP, "Pracownik");
                //lbNrEw.Text = db.getValue(drP, "Nr_Ewid");
                lbNrEw.Text = db.getValue(drP, "KadryId");
            }
            trUpr2.Visible = s;
            trUpr3.Visible = s;
            trPrac2.Visible = s;
            if (s)
            {
                lbPoziom.Text = db.getValue(drU, L.Lang == L.lngPL ? "Poziom" : "PoziomEN");
                lbSymbol.Text = db.getValue(drU, "Symbol");
                lbSymbolSpawacza.Text = db.getValue(drP, "Nr_Ewid2");
            }
        }

        public int Mode
        {
            set { FMode = value; }
            get { return FMode; }
        }

        public string SymbolUprawnienia
        {
            set { ViewState["symupr"] = value; }
            get { return Tools.GetStr(ViewState["symupr"]); }
        }

        public string NazwaUprawnienia
        {
            set { ViewState["nazwaupr"] = value; }
            get { return Tools.GetStr(ViewState["nazwaupr"]); }
        }

        public string OkresWaznosciTyp
        {
            set { ViewState["okwazt"] = value; }
            get { return Tools.GetStr(ViewState["okwazt"]); }
        }

        public int OkresWaznosci
        {
            set { ViewState["okwaz"] = value; }
            get { return Tools.GetInt(ViewState["okwaz"], -1); }
        }

        /*
        protected Boolean Szkoleniap
        {
            get
            {
                try
                {
                    return Typ = cntUprawnienia.utSzkolenia;
                    //return Request.QueryString["t"] == cntUprawnienia.utSzkolenia;
                }
                catch
                {
                    return false;
                }
            }
        }
        protected Boolean Statusyp
        {
            get
            {
                try
                {
                    return Request.QueryString["t"] == cntUprawnienia.utStatusSamokontroli;
                }
                catch
                {
                    return false;
                }
            }
        }
        */ 
    }
}