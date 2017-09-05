using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Controls.Harmonogram
{
    public partial class cntPracownicyEditModal : System.Web.UI.UserControl
    {
        public event EventHandler Saved;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void ShowEdit(String EmployeeId)
        {
            this.EmployeeId = EmployeeId;
            cntModal.Title = "Edytuj pracownika";
            cntModal.Show();
            Prepare();
        }

        void Prepare()
        {
            if (String.IsNullOrEmpty(EmployeeId))
                return;
            DataRow dr = db.Select.Row(dsData, EmployeeId, ddlMiesiac.SelectedValue);
            tbImie.Text = db.getValue(dr, "Imie");
            tbNazwisko.Text = db.getValue(dr, "Nazwisko");
            tbIdentyfikator.Text = db.getValue(dr, "KadryId");
            Tools.SelectItem(ddlDzial, db.getValue(dr, "IdDzialu"));
            Tools.SelectItem(ddlTypOkresu, db.getValue(dr, "TypOkresu"));
            tbKlasyfikacja.Text = db.getValue(dr, "Klasyfikacja");
            tbFunkcja.Text = db.getValue(dr, "Rodzaj");
            tbKod.Text = db.getValue(dr, "Kod");
            tbKodOpis.Text = db.getValue(dr, "Opis");
            deDataZatrudnienia.Date = db.getValue(dr, "DataZatr");

            NrEwid = db.getValue(dr, "KadryId");

        }

        public void ShowInsert()
        {
            this.EmployeeId = null;
            cntModal.Title = "Dodaj pracownika";
            cntModal.Show();
        }

        public void Close()
        {
            cntModal.Close();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        public void Save()
        {

            String nazwisko = tbNazwisko.Text;
            String imie = tbImie.Text;
            String identyfikator = tbIdentyfikator.Text;
            String dzial = ddlDzial.SelectedItem.Text;
            String funkcja = tbFunkcja.Text;
            String kod = tbKod.Text;
            String kodOpis = tbKodOpis.Text;
            String agencja = tbKlasyfikacja.Text;

            //object data = deDataZatrudnienia.Date;
            //String dataZatr = (data == null) ? null : data.ToString();

            String dataZatr = deDataZatrudnienia.DateStr as String;


            // zmiana nr ewid

            if (IsUpdate && NrEwid != identyfikator)
            {
                DataTable dt = db.Select.Table(dsNrEwid, identyfikator);
                if (dt.Rows.Count == 0)
                {
                    Log.Info(Log.HARMONOGRAM,
                        "Zmiana nr ewidencyjnego",
                        String.Format("Pracownik: {0} stary: {1} nowy: {2}", EmployeeId, NrEwid, identyfikator));

                    if (!db.Execute(dsNrEwid.UpdateCommand, identyfikator, EmployeeId))
                    {
                        Tools.ShowError("Wystąpił błąd przy zmianie identyfikatora.");
                        return;
                    }
                }
                else
                {
                    string prac = db.getValue(dt.Rows[0], "Pracownik");
                    Tools.ShowError("Istnieje już pracownik z podanym identyfikatorem: {0}", prac);
                    return;
                }
            }

            // procedura importu

            if (db.Execute(dsSave, nazwisko, imie, identyfikator, dzial, funkcja, kod, kodOpis, agencja, db.nullStrParam(dataZatr), ddlMiesiac.SelectedValue, ddlTypOkresu.SelectedValue))
            {
                Close();
                Tools.ShowMessage("Dane zostały zapisane.");
                if (Saved != null)
                    Saved(null, EventArgs.Empty);
            }
            else
                Tools.ShowError("Wystąpił błąd!");
        }

        private bool IsUpdate
        {
            get { return !String.IsNullOrEmpty(EmployeeId); }
        }

        public String NrEwid
        {
            get { return Tools.GetStr(ViewState["vNrEwid"]); }
            set { ViewState["vNrEwid"] = value; }
        }

        public String EmployeeId
        {
            get { return Tools.GetStr(ViewState["vEmployeeId"]); }
            set { ViewState["vEmployeeId"] = value; }
        }

        protected void ddlMiesiac_SelectedIndexChanged(object sender, EventArgs e)
        {
            Prepare();
        }

        protected void btnSaveConfirm_Click(object sender, EventArgs e)
        {
            string newNr = tbIdentyfikator.Text;
            DataTable dt = db.Select.Table(dsNrEwid, newNr);
            if (dt.Rows.Count == 0)
            {
                Save();
            }   
            else
            {
                string prac = db.getValue(dt.Rows[0], "Pracownik");
                Tools.ShowConfirm(String.Format("Istnieje już pracownik z podanym identyfikatorem: {0}. Czy chcesz kontynuować?", prac), btnSave);
            }
        }
    }
}