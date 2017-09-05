using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls.Ubezpieczenia.Majatkowe
{
    public partial class cntWypowiedzenieModal : System.Web.UI.UserControl
    {
        public event EventHandler Saved;
        protected void Page_Load(object sender, EventArgs e)
        {
            PrepareCalendar();
            Tools.ExecuteJavascript("cntWypowiedzenieModal();");
        }

        private void PrepareCalendar()
        {
            DataZakonczenia.DeValue.MinViewMode = 1;
            DateTime now = DateTime.Now;
            now = now.AddMonths(1);
            DataZakonczenia.DeValue.StartDate = now.ToShortDateString();
        }

        public void Show(String RequestId)
        {
            cntModal.Show();
            this.RequestId = RequestId;
            Prepare();
        }

        public void Hide()
        {
            cntModal.Close();
        }

        void Prepare()
        {
            DataRow drData = db.Select.Row(dsData, RequestId);
            dbField.FillData(this, drData, 0, 0, 0, dbField.moEdit);
        }

        public bool ValidatePowod()
        {
            if (!OdrzProblemyFinansowe.CbValue.Checked && !OdrzZakupInnego.CbValue.Checked && !OdrzBrakSatysfakcji.CbValue.Checked && !OdrzOczekiwania.CbValue.Checked && !OdrzInnaPrzyczyna.CbValue.Checked)
            {
                Tools.ShowMessage("Proszę podać powód odrzucenia.");
                return false;
            }

            if (OdrzOczekiwania.CbValue.Checked && String.IsNullOrEmpty(OdrzOczekiwaniaUwagi.Value))
            {
                Tools.ShowMessage("Proszę podać dlaczego produkt nie spełnia oczekiwań.");
                return false;
            }

            if (OdrzInnaPrzyczyna.CbValue.Checked && String.IsNullOrEmpty(OdrzInnaPrzyczynaUwagi.Value))
            {
                Tools.ShowMessage("Proszę podać inną przyczynę odrzucenia.");
                return false;
            }

            return true;
        }

        public bool ValidateDates(DataRow drData)
        {
            string dataOd = db.getValue(drData, "DataOd");
            string dataDo = DataZakonczenia.Value;
            if (String.IsNullOrEmpty(dataDo))
            {
                Tools.ShowMessage("Proszę podać datę zakończenia ubezpieczenia.");
                return false;
            }
            DateTime dtDo;
            try
            {
                dtDo = Convert.ToDateTime(dataDo);
            }
            catch
            {
                Tools.ShowMessage("Niepoprawny format daty!");
                return false;
            }



            //DateTime nextMonth = DateTime.Now.AddMonths(1);
            //nextMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);




            if (dtDo.Day != DateTime.DaysInMonth(dtDo.Year, dtDo.Month))
            {
                Tools.ShowMessage("Niepoprawna data ubezpieczenia. Data powinna być ostatnim dniem miesiąca.");
                return false;
            }

            DateTime dtOd;
            try
            {
                dtOd = Convert.ToDateTime(db.getValue(drData, "DataOd"));
            }
            catch
            {
                Tools.ShowMessage("Nie można pobrać daty poprzedniego ubezpieczenia. Skontaktuj się z administratorem.");
                return false;
            }

            if(dtDo <= dtOd)
            {

                Tools.ShowMessage("Data zakończenia polisy nie może być wcześniejsza niż data rozpoczęcia!");
                return false;
            }

            return true;
        }

        public bool Validate()
        {
            DataRow drData = db.Select.Row(dsData, RequestId);

            if (!ValidatePowod())
                return false;

            if (!ValidateDates(drData))
                return false;

            return true;
        }

        protected void btnSaveConfirm_Click(object sender, EventArgs e)
        {
            if (Validate())
                Tools.ShowConfirm("Czy na pewno chcesz zakończyć polisę?", btnSave);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                Save();
                Hide();
                if (Saved != null)
                    Saved(sender, e);
            }
        }

        public void Save()
        {
            if (dbField.dbUpdate(db.conP, this, "poWnioskiMajatkowe", "Id=" + RequestId, "Status,SkladkaAssecoId", "-1", db.NULL))
            {
                Log.Info(1337, String.Format("Zakończono ubezpieczenie mieszkaniowe o Id = {0} przez użytkownika: {1} na datę zakończenia: {2}", RequestId, App.User.Id, DataZakonczenia.Value), "");
                SendMails();

                //string skladkaSum = Ubezpieczenia.GetSkladkaSumWypowiedzenie(App.User.Id, DataZakonczenia.Value, RequestId);
                //bool err = false;
                //if (String.IsNullOrEmpty(skladkaSum))
                //    err = !Ubezpieczenia.UpdateSkladnik(App.User.NR_EW, DataZakonczenia.Value);
                //else
                //    err = !Ubezpieczenia.InsertSkladnik(App.User.NR_EW, skladkaSum, DataZakonczenia.Value);

                //if (err)
                //    Mailing.EventUbezpieczeniaMajatkowe(Mailing.maUBEZP_ERR, RequestId);








                //if (db.ExecuteClientData("UB_SKLADNIK_UPD", App.User.NR_EW, DataZakonczenia.Value)) { }
                //else
                //{
                //    Tools.ShowErrorLog(1337, "Zamknięcie składnika płacowego nie powiodło się.", "Zamknięcie składnika płacowego nie powiodło się. Skontaktuj się z działem HR.");
                //    Mailing.EventUbezpieczeniaMajatkowe(Mailing.maUBEZP_ERR, RequestId);
                //}
            }
            else
                Tools.ShowMessage("Wystąpił błąd! Administrator został powiadomiony o tym zdarzeniu.");
        }

        void SendMails()
        {
            // dla kończącego
            Mailing.EventUbezpieczeniaMajatkowe(Mailing.maUBEZP_KONIEC_POTW, RequestId);
            // dla adminów
            Mailing.EventUbezpieczeniaMajatkowe(Mailing.maUBEZP_KONIEC_POTW_ADM, RequestId);
        }

        public String RequestId
        {
            get { return (String)ViewState["vRequestId"]; }
            set { ViewState["vRequestId"] = value; }
        }


    }
}