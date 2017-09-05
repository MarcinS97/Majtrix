using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class RepPracInfo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void Prepare(SqlConnection con, string pracId)
        {
            PracId = pracId;   // >>> zmienic na GetPracInfo1
            DataRow dr = Base.getDataRow(con, 
                "select P.Imie + ' ' + P.Nazwisko as ImieNazwisko, " +
                    "P.KadryId, " + 
	                "D.Nazwa as Dzial, " +
	                "S.Nazwa as Stanowisko, " +
	                "P.DataZatr, " +
                    //"convert(varchar, P.EtatL) + '/' + convert(varchar, P.EtatM) as Etat, " +
                    "P.EtatL, P.EtatM, " +
                    "Info " +
                "from Pracownicy P " +
                "left outer join Dzialy D on D.Id = P.IdDzialu " +
                "left outer join Stanowiska S on S.Id = P.IdStanowiska " + 
                "where P.Id = " + pracId);
            DateTime? dt = Base.getDateTime(dr, "DataZatr");
            int? etatL = Base.getInt(dr, "EtatL");
            int? etatM = Base.getInt(dr, "EtatM");
            string etat = null;
            if (etatL != null && etatM != null)
            {
                if ((int)etatL > 0 && (int)etatM > 0)
                    Etat = 8 * (int)etatL / (int)etatM;
                else
                    Etat = 0;
                if (etatL == etatM) etat = "pełny";
                else etat = etatL.ToString() + "/" + etatM.ToString();
            }
            else Etat = 0;
            lbImieNazwisko.Text = Base.getValue(dr, "ImieNazwisko");
            lbNrEw.Text = Base.getValue(dr, "KadryId");
            lbDataZatrudnienia.Text = dt == null ? null : Base.DateToStr(dt);
            lbDzial.Text = Base.getValue(dr, "Dzial");
            lbStanowisko.Text = Base.getValue(dr, "Stanowisko");
            //lbWymiarEtatu.Text = Base.getValue(dr, "Etat");
            lbWymiarEtatu.Text = etat;
            lbInformacje.Text = Base.getValue(dr, "Info");
        }
        //--------------------------
        public string PracId
        {
            get { return hidPracId.Value; }
            set { hidPracId.Value = value; }
        }

        public int Etat
        {
            get { return Tools.GetViewStateInt(ViewState[ID + "_etat"], 0); }
            set { ViewState[ID + "_etat"] = value; }
        }
    }
}