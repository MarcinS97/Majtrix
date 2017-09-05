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
    public partial class cntZmianaDanychModal : System.Web.UI.UserControl
    {
        public event EventHandler Saved;
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        public void Show(String RequestId)
        {
            this.RequestId = RequestId;
            cntModal.Show();

            DataRow drData = db.Select.Row(dsData, RequestId);

            tbCesja.Visible = true;
            bool cesja = db.getBool(drData, "Cesja", false);
            rblCesja.SelectedValue = (cesja ? "1" : "0");
            
            if(cesja)
            {
                divCesja.Visible = true;
                lblCesja.Text = db.getValue(drData, "CesjaName", "");
            }
            else
            {
                divCesja.Visible = false;
            }
            
            dbField.FillData(this, drData, 0, 0, 0, dbField.moEdit);

            tbCesja.Visible = cesja;
            lblTelefon.Text = db.getValue(drData, "NrTel");
            lblEmail.Text = db.getValue(drData, "Email");
        }

        public void Hide()
        {
            cntModal.Close();
        }


        private bool ValidateNrTel()
        {
            if (String.IsNullOrEmpty(NrTel.Value))
            {
                Tools.ShowMessage("Proszę podać numer telefonu.");
                return false;
            }
            return true;
        }

        private bool ValidateEmail()
        {
            if (String.IsNullOrEmpty(Email.Value))
            {
                Tools.ShowMessage("Proszę podać adres email.");
                return false;
            }

            if (!Email.Value.Contains("@"))
            {
                Tools.ShowMessage("Proszę podać poprawny adres email.");
                return false;
            }
            return true;
        }

        private bool ValidateCesja()
        {
            if (rblCesja.SelectedValue == "1")
            {
                if (String.IsNullOrEmpty(NazwaBanku.Value))
                {
                    Tools.ShowMessage("Proszę podać nazwę banku.");
                    return false;
                }

                if (String.IsNullOrEmpty(RegonBanku.Value))
                {
                    Tools.ShowMessage("Proszę podać REGON banku.");
                    return false;
                }
            }
            return true;
        }

        public bool Validate()
        {
            if (!ValidateNrTel())
                return false;

            if (!ValidateEmail())
                return false;

            if (!ValidateCesja())
                return false;

            return true;
        }
        
        protected void btnSaveConfirm_Click(object sender, EventArgs e)
        {
            if(Validate())
                Tools.ShowConfirm("Czy na pewno chcesz zmienić dane ubezpieczenia? Wiążę się to ze stworzeniem nowego wniosku.", btnSave);
        }

        public void Save()
        {
            bool cesja = rblCesja.SelectedValue == "1";
            string regon = RegonBanku.Value, nazwa = NazwaBanku.Value;
            if (!cesja)
                regon = nazwa = null;

            if (db.Execute(dsSave, RequestId, db.strParam(NrTel.Value), db.strParam(Email.Value), cesja ? "1" : "0", db.nullStrParam(regon), db.nullStrParam(nazwa)))
            {
                Log.Info(1337, String.Format("Zmieniono dane ubezpieczenia mieszkaniowego o Id = {0}. ZglaszajacyId = {1}", RequestId, App.User.Id), "");
                SendMails(RequestId);
            }
            else
                Tools.ShowMessage("Wystąpił błąd! Administrator został powiadomiony o tym zdarzeniu.");
        }

        public void SendMails(string updatedRequestId)
        {
            // dla zmieniającego
            Mailing.EventUbezpieczeniaMajatkowe(Mailing.maUBEZP_ZMIANAD_POTW, updatedRequestId);
            // dla adminów
            Mailing.EventUbezpieczeniaMajatkowe(Mailing.maUBEZP_ZMIANAD_POTW_ADM, updatedRequestId);
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


        protected void rblCesja_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = (sender as RadioButtonList).SelectedValue;
            tbCesja.Visible = (val == "1");

        }
        public String RequestId
        {
            get { return (String)ViewState["vRequestId"]; }
            set { ViewState["vRequestId"] = value; }
        }
    }
}