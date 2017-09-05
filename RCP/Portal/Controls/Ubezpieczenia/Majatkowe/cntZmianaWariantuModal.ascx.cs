using HRRcp.App_Code;
using HRRcp.RCP.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls.WnioskiMajatkowe
{
    public partial class cntZmianaWariantuModal : System.Web.UI.UserControl
    {
        public event EventHandler Saved;
        protected void Page_Load(object sender, EventArgs e)
        {
            PrepareCalendar();
            Tools.ExecuteJavascript("cntZmianaWariantuModal();");
        }

        private void PrepareCalendar()
        {
            DataOd.MinViewMode = 1;


            //DataRow drOldData = db.Select.Row(dsData, RequestId);
            //string oldDataOd = db.getValue(drOldData, "DataOd");

            DateTime now = DateTime.Now;//Convert.ToDateTime(oldDataOd);
            now = now.AddMonths(1);
            DataOd.StartDate = now.ToShortDateString();
        }

        public void Show(String RequestId)
        {
            this.RequestId = RequestId;
            cntModal.Show();

            DataRow drData = db.Select.Row(dsData, RequestId);
            lblPESEL.Text = App.User.Nick;
            //lblPracodawca.Text = db.getValue(drData, "Pracodawca");
            dbField.FillData(this, drData, 0, 0, 0, dbField.moQuery);
            cntWarianty.ExcludedId = db.getValue(drData, "ParId", null);
            //cntWarianty.Prepare(db.getValue(drData, "ParId"), db.getBool(drData, "Plus", false), true);
            cntWarianty.Prepare(db.getValue(drData, "Rodzaj"),  db.getValue(drData, "ParId"), db.getValue(drData, "PlusId"), true);
            lblDate.Text = db.getValue(drData, "CurrentDate", "");
            litSkladka.Text = db.getValue(drData, "Skladka", "");
            litSkladkaPlus.Text = db.getValue(drData, "SkladkaPlus", "");
            litSuma.Text = db.getValue(drData, "SumaUbezpieczenia", "");
            litSumaPlus.Text = db.getValue(drData, "SumaUbezpieczeniaPlus", "");
            litSkladkaSum.Text = db.getValue(drData, "SkladkaSum", "");
            bool plus = db.getBool(drData, "Plus", false);
            cbSkladkaPlus.Checked = plus;

        }

        public void Hide()
        {
            cntModal.Close();
        }

        //private bool ValidateDataRozp()
        //{
        //    return true;
        //}

        private bool ValidateWarianty(DataRow drOldData)
        {
            string parId = cntWarianty.GetSelectedParId();
            
            if (String.IsNullOrEmpty(parId))
            {
                Tools.ShowMessage("Proszę wybrać wariant ubezpieczenia!");
                return false;
            }

            string plusId = cntWarianty.GetSelectedPlusId();

            string oldParId  = db.getValue(drOldData, "ParId");
            string oldPlusId = db.getValue(drOldData, "PlusId");

            if(parId == oldParId && plusId == oldPlusId)
            {
                Tools.ShowMessage("Wariant ubezpieczenia musi być różny od poprzedniego.");
                return false;
            }

            return true;
        }

        private bool ValidateDates(DataRow drOldData)
        {
            string dataOd = DataOd.DateStr;
            if (String.IsNullOrEmpty(DataOd.DateStr))
            {
                Tools.ShowMessage("Proszę podać datę rozpoczęcia ubezpieczenia.");
                return false;
            }

            DateTime dtOd;
            try
            {
                dtOd = Convert.ToDateTime(dataOd);
            }
            catch
            {
                Tools.ShowMessage("Niepoprawny format daty!");
                return false;
            }

            DateTime nextMonth = DateTime.Now.AddMonths(1);
            nextMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);

            DateTime dtOdOld;
            try
            {
                dtOdOld = Convert.ToDateTime(db.getValue(drOldData, "DataOd"));
            }
            catch
            {
                Tools.ShowMessage("Nie można pobrać daty poprzedniego ubezpieczenia. Skontaktuj się z administratorem.");
                return false;
            }
           
            if (dtOd < nextMonth || dtOd <= dtOdOld || dtOd.Day != 1)
            {
                Tools.ShowMessage("Niepoprawna data ubezpieczenia!");
                return false;
            }




            DataTable dt = db.Select.Table(dsCheckIfAlreadyExists, DataOd.DateStr, RequestId, App.User.Id);   
            if (dt.Rows.Count > 0)
            {
                Tools.ShowMessage("Istnie już ubezpieczenie na ten adres i datę.");
                return false;
            }
            return true;
        }

        bool Validate()
        {
            DataRow drOldData = db.Select.Row(dsData, RequestId);

            //if (!ValidateDataRozp())
            //    return false;

            if (!ValidateDates(drOldData))
                return false;

            if (!ValidateWarianty(drOldData))
                return false;
            
            return true;
        }

        protected void btnSaveConfirm_Click(object sender, EventArgs e)
        {
            if(Validate())
                Tools.ShowConfirm("Czy na pewno chcesz zmienić wariant ubezpieczenia? Wiążę się to ze stworzeniem nowego wniosku.", btnSave);
        }

        public void Save()
        {
            String parId = cntWarianty.GetSelectedParId();
            bool plus = cntWarianty.GetSelectedPlus();
            String plusId = cntWarianty.GetSelectedPlusId();

            //DataRow drVariantParameters = db.Select.Row(dsVariantParameters, parId);
            DataRow drVariantParameters = db.Select.Row(dsVariantParameters, parId, db.nullParam(plusId));

            string sumaUbezpieczenia, skladka, sumaUbezpieczeniaPlus, skladkaPlus;
            string dataOd = DataOd.DateStr;

            sumaUbezpieczenia = db.getValue(drVariantParameters, "Suma");
            sumaUbezpieczeniaPlus = db.getValue(drVariantParameters, "SumaPlus");
            skladka = db.getValue(drVariantParameters, "Skladka");
            skladkaPlus = db.getValue(drVariantParameters, "SkladkaPlus");


            if (db.Execute(dsSave, RequestId, parId, plus ? "1" : "0", sumaUbezpieczenia, skladka, sumaUbezpieczeniaPlus, skladkaPlus, db.strParam(dataOd), db.nullParam(plusId)))
            {
                Log.Info(1337, String.Format("Zmieniono wariant ubezpieczenia mieszkaniowego o Id = {0}. ZglaszajacyId = {1}, DataRozpoczecia = {2} ", RequestId, App.User.Id, dataOd), "");
                SendMails(RequestId);

                //DataTable dt = db.Select.Table(dsCheckIfNext, App.User.Id, dataOd);
                //if (dt.Rows.Count > 0) // jak ubezpieczamy kolejne mieszkanie to tylko zmieniamy kwote w assecco
                //{
                //    if (!Ubezpieczenia.Ubezpieczenia.UpdateSkladnikKwota(App.User.NR_EW, Ubezpieczenia.Ubezpieczenia.GetSkladkaSumZmianaWariantu(App.User.Id, dataOd, RequestId, GetSkladkaSum(skladka, skladkaPlus)), dataOd))
                //        Mailing.EventUbezpieczeniaMajatkowe(Mailing.maUBEZP_ERR, RequestId);
                //}
                //else
                //{
                //    if (!Ubezpieczenia.Ubezpieczenia.InsertSkladnik(App.User.NR_EW, Ubezpieczenia.Ubezpieczenia.GetSkladkaSumZmianaWariantu(App.User.Id, dataOd, RequestId, GetSkladkaSum(skladka, skladkaPlus)), dataOd))
                //        Mailing.EventUbezpieczeniaMajatkowe(Mailing.maUBEZP_ERR, RequestId);
                //}
                //bool skladnikGood = false;
                //skladnikGood = db.ExecuteClientData("UB_SKLADNIK_UPD", App.User.NR_EW, dataOd);
                //skladnikGood &= db.ExecuteClientData("UB_SKLADNIK_INS", App.User.NR_EW, GetSkladkaSum(skladka, skladkaPlus), dataOd);
                //if (!skladnikGood)
                //{
                //    Tools.ShowMessage("Aktualizacja składnika płacowego nie powiodła się. Skontaktuj się z działem HR.");
                //    Mailing.EventUbezpieczeniaMajatkowe(Mailing.maUBEZP_ERR, RequestId);
                //}
            }
            else
                Tools.ShowMessage("Wystąpił błąd! Administrator został powiadomiony o tym zdarzeniu.");

        }

        public void SendMails(string updatedRequestId)
        {
            // dla zmieniającego
            Mailing.EventUbezpieczeniaMajatkowe(Mailing.maUBEZP_ZMIANA_POTW, updatedRequestId);
            // dla adminów
            Mailing.EventUbezpieczeniaMajatkowe(Mailing.maUBEZP_ZMIANA_POTW_ADM, updatedRequestId);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                Save();
                if (Saved != null)
                    Saved(sender, e);
                Hide();
            }

        }

        public string GetSkladkaSum(string skladka, string skladkaPlus)
        {
            double sum = 0, skladkaF = 0, skladkaPF = 0;
            try
            {
                skladkaF = Convert.ToDouble(skladka);
            }
            catch { }
            try
            {
                skladkaPF = Convert.ToDouble(skladkaPlus);
            }
            catch { }
            sum = skladkaF + skladkaPF;
            return sum.ToString();
        }

        public String RequestId
        {
            get { return (String)ViewState["vRequestId"]; }
            set { ViewState["vRequestId"] = value; }
        }

    }
}