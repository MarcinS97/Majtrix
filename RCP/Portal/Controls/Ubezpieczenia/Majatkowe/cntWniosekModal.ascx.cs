using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls.WnioskiMajatkowe
{
    public partial class cntWniosekModal : System.Web.UI.UserControl
    {
        public event EventHandler Saved;

        protected void Page_Load(object sender, EventArgs e)
        {
            PrepareCalendar();
            Tools.ExecuteJavascript("cntWniosekModal();");
        }

        void PrepareCalendar()
        {
            DataOd.DeValue.MinViewMode = 1;
            DateTime now = DateTime.Now;
            now = now.AddMonths(1);
            DataOd.DeValue.StartDate = now.ToShortDateString();
        }

        public void Show(String Id)
        {
            cntModal.Show(false);
            Prepare(Id);
        }

        public void Close()
        {
            cntModal.Close();
        }

        #region PREPARE AND INIT

        private void Prepare(String Id)
        {
            this.RequestId = Id;
            this.Edit = String.IsNullOrEmpty(Id);
            //DataRow dr = db.Select.Row(dsPrac, App.User.Id);
            PrepareUserData();
            if (Edit)
                PrepareEdit();
            else
                PreparePreview(Id);
        }

        private void PrepareUserData()
        {
            hidUserId.Value = App.User.Id;
            lbZglaszajacy.Text = App.User.NazwiskoImie;
            lblPESEL.Text = App.User.Nick;
        }

        private void PrepareEdit()
        {
            dbField.FillData(this, null, 0, 0, 0, dbField.moEdit);
            divFormularz.Attributes["class"] = "wn-formularz edit";
            InitEditEmail();
            InitEditAdresUbezp();
            InitEditAdresKoresp();
            InitEditWlasnoc();
            InitEditCesja();
            InitEditRodzaj();
            InitEditPracodawca();
            InitEditButtons();
            InitEditWarianty();
        }

        private void InitEditEmail()
        {
            Email.Value = App.User.EMail;
        }

        private void InitEditAdresUbezp()
        {
            tbAdresUbezp.Visible = true;
            trAddrUbezp0.Visible = true;
            trAddrUbezp1.Visible = false;
            trAddrUbezp2.Visible = false;
            ddlAdresyUbezp.DataBind();
            ddlAdresyUbezp.SelectedValue = null;

            lblAdresUbezpUlicaDomLok.Visible = lblAdresUbezpKodMiasto.Visible = false;
            tbAdresUbezpUlica.Visible = tbAdresUbezpDom.Visible = tbAdresUbezpLokal.Visible = tbAdresUbezpKod.Visible = tbAdresUbezpMiasto.Visible = true;

            tbAdresUbezpDom.Text = tbAdresUbezpKod.Text = tbAdresUbezpLokal.Text = tbAdresUbezpMiasto.Text =
                tbAdresUbezpUlica.Text = String.Empty;

        }

        private void InitEditAdresKoresp()
        {
            divAddrQuestion.Visible = true;
            rblKoresp.Enabled = true;
            rblKoresp.Visible = true;
            rblKoresp.SelectedValue = "1";
            tbAdresKor.Visible = false;
            ddlAdresyKor.DataBind();
            ddlAdresyKor.SelectedValue = null;

            lblAdresKorUlicaDomLok.Visible = lblAdresKorKodMiasto.Visible = false;
            tbAdresKorUlica.Visible = tbAdresKorDom.Visible = tbAdresKorLokal.Visible = tbAdresKorKod.Visible = tbAdresKorMiasto.Visible = true;

            tbAdresKorDom.Text = tbAdresKorKod.Text = tbAdresKorLokal.Text = tbAdresKorMiasto.Text =
                tbAdresKorUlica.Text = String.Empty;
        }

        private void InitEditButtons()
        {
            btnSave.Visible = btnSaveConfirm.Visible = true;
        }

        private void InitEditWarianty()
        {
            cntWarianty.Prepare(Rodzaj.RblValue.SelectedValue, null, null, Edit);
        }

        private void InitEditPracodawca()
        {
            Pracodawca.DdlValue.DataBind();
            if (Pracodawca.DdlValue.Items.Count == 2)
                Pracodawca.DdlValue.Items.RemoveAt(0);
        }

        private void InitEditCesja()
        {
            rblCesja.Enabled = true;
            tbCesja.Visible = false;
            rblCesja.SelectedValue = "0";
        }

        private void InitEditRodzaj()
        {
            Rodzaj.RblValue.SelectedIndex = 0;
        }

        private void InitEditWlasnoc()
        {
            WlasnoscOpis.Visible = false;
        }

        private void PreparePreview(String id)
        {
            DataRow dr = db.Select.Row(dsData, id);
            dbField.FillData(this, dr, 0, 0, 0, dbField.moQuery);
            divFormularz.Attributes["class"] = "wn-formularz preview";

            InitPreviewStatus(db.getInt(dr, "Status", 0), dr);
            InitPreviewAdresUbezp(db.getValue(dr, "AdresUbezpUlicaDomLokal"), db.getValue(dr, "AdresUbezpKodMiasto"));
            InitPreviewAdresKor(db.getBool(dr, "AdresKorSame", true), db.getValue(dr, "AdresKorUlicaDomLokal"), db.getValue(dr, "AdresKorKodMiasto"));
            InitPreviewCesja(db.getBool(dr, "Cesja", false));
            InitPreviewWarianty(dr);
            InitPreviewButtons();
        }

        private void InitPreviewAdresUbezp(string adresUbezpUlicaDomLok, string adresUbezpKodMiasto)
        {
            tbAdresUbezp.Visible = true;

            trAddrUbezp0.Visible = false;
            trAddrUbezp1.Visible = true;
            trAddrUbezp2.Visible = true;

            tbAdresUbezpUlica.Visible = tbAdresUbezpDom.Visible = tbAdresUbezpLokal.Visible = tbAdresUbezpKod.Visible = tbAdresUbezpMiasto.Visible = false;
            lblAdresUbezpUlicaDomLok.Visible = lblAdresUbezpKodMiasto.Visible = true;

            lblAdresUbezpUlicaDomLok.Text = adresUbezpUlicaDomLok;
            lblAdresUbezpKodMiasto.Text = adresUbezpKodMiasto;
        }

        private void InitPreviewStatus(int status, DataRow dr)
        {
            if (status == -1)
            {
                string status2 = db.getValue(dr, "StatusNazwa2");
                DateTime? dt = db.getDateTime(dr, "DataZakonczenia");
                string data = dt != null ? String.Format(" ({0})", Tools.DateToStr((DateTime)dt)) : null;
                ltStatus.Text = status2 + data;
                tStatus.Visible = true;
                btnRestore.Visible = btnRestoreConfirm.Visible = false;//true; NAD PRZYWRACANIEM TRZEBA POMYŚLEć
            }
            else
            {
                tStatus.Visible = false;
                btnRestore.Visible = btnRestoreConfirm.Visible = false;
            }
        }

        private void InitPreviewAdresKor(bool same, string adresKorUlicaDomLok, string adresKorKodMiasto)
        {
            rblKoresp.SelectedValue = (same) ? "1" : "0";
            tbAdresKor.Visible = !same;
            trAddrKor0.Visible = same;
            trAddrKor1.Visible = !same;
            trAddrKor2.Visible = !same;
            divAddrQuestion.Visible = same;
            rblKoresp.Enabled = false;

            tbAdresKorUlica.Visible = tbAdresKorDom.Visible = tbAdresKorLokal.Visible = tbAdresKorKod.Visible = tbAdresKorMiasto.Visible = false;
            lblAdresKorUlicaDomLok.Visible = lblAdresKorKodMiasto.Visible = true;
            lblAdresKorUlicaDomLok.Text = adresKorUlicaDomLok;
            lblAdresKorKodMiasto.Text = adresKorKodMiasto;
        }

        private void InitPreviewCesja(bool cesja)
        {
            tbCesja.Visible = cesja;
            rblCesja.SelectedValue = cesja ? "1" : "0";
            rblCesja.Enabled = false;
        }

        private void InitPreviewButtons()
        {
            btnSave.Visible = btnSaveConfirm.Visible = false;
        }

        private void InitPreviewWarianty(DataRow dr)
        {
            // NIE MAM POJĘCIA CO TU SIĘ DZIEJE
            cntWarianty.Prepare(db.getValue(dr, "Rodzaj"), db.getValue(dr, "ParId"), db.getValue(dr, "PlusId"), Edit);
            cntWarianty.Prepare(db.getValue(dr, "Rodzaj"), db.getValue(dr, "ParId"), db.getValue(dr, "PlusId"), Edit);
        }

        #endregion

        #region VALIDATION

        public bool Validate()
        {
            if (!ValidateNrTel())
                return false;

            if (!ValidateEmail())
                return false;

            if (!ValidatePracodawca())
                return false;

            if (!ValidateAdresUbezp())
                return false;

            if (!ValidatePowierzchnia())
                return false;

            if (!ValidateAdresKor())
                return false;

            if (!ValidateRodzaj())
                return false;

            if (!ValidateStatusLok())
                return false;

            //if (!ValidateDataRozp())
            //    return false;

            if (!ValidateCesja())
                return false;

            if (!ValidateWarianty())
                return false;

            if (!ValidateCheckboxes())
                return false;

            if (!ValidateDates())
                return false;

            return true;
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

        private bool ValidatePracodawca()
        {
            if (String.IsNullOrEmpty(Pracodawca.Value))
            {
                Tools.ShowMessage("Proszę wybrać pracodawcę.");
                return false;
            }
            return true;
        }

        private bool ValidateAdresUbezp()
        {
            if (String.IsNullOrEmpty(ddlAdresyUbezp.SelectedValue))
            {
                Tools.ShowMessage("Proszę wybrać adres ubezpieczenia.");
                return false;
            }

            if (ddlAdresyUbezp.SelectedValue == "-1")
            {
                if (String.IsNullOrEmpty(tbAdresUbezpKod.Text))
                {
                    Tools.ShowMessage("Proszę podać kod pocztowy ubezpieczanego lokalu.");
                    return false;
                }

                if (String.IsNullOrEmpty(tbAdresUbezpMiasto.Text))
                {
                    Tools.ShowMessage("Proszę podać miasto ubezpieczanego lokalu.");
                    return false;
                }

                if (String.IsNullOrEmpty(tbAdresUbezpDom.Text))
                {
                    Tools.ShowMessage("Proszę podać nr domu ubezpieczanego lokalu.");
                    return false;
                }

            }

            return true;
        }

        private bool ValidatePowierzchnia()
        {
            if (String.IsNullOrEmpty(PowierzchniaLokalu.Value))
            {
                Tools.ShowMessage("Proszę podać kod pocztowy ubezpieczanego lokalu.");
                return false;
            }
            return true;
        }

        private bool ValidateAdresKor()
        {
            if (rblKoresp.SelectedValue == "0")
            {
                if (String.IsNullOrEmpty(ddlAdresyKor.SelectedValue))
                {
                    Tools.ShowMessage("Proszę wybrać adres ubezpieczenia.");
                    return false;
                }

                if (ddlAdresyKor.SelectedValue == "-1")
                {
                    if (String.IsNullOrEmpty(tbAdresKorKod.Text))
                    {
                        Tools.ShowMessage("Proszę podać kod pocztowy do adresu korespondencyjnego.");
                        return false;
                    }

                    if (String.IsNullOrEmpty(tbAdresKorMiasto.Text))
                    {
                        Tools.ShowMessage("Proszę podać miasto do adresu korespondencyjnego.");
                        return false;
                    }

                    if (String.IsNullOrEmpty(tbAdresKorDom.Text))
                    {
                        Tools.ShowMessage("Proszę podać nr domu do adresu korespondencyjnego.");
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ValidateRodzaj()
        {
            if (Rodzaj.RblValue.SelectedItem == null)
            {
                Tools.ShowMessage("Proszę wybrać rodzaj lokalu!");
                return false;
            }
            return true;
        }

        private bool ValidateStatusLok()
        {
            if (Wlasnosc.RblValue.SelectedItem == null)
            {
                Tools.ShowMessage("Proszę wybrać status lokalu!");
                return false;
            }
            if (Wlasnosc.RblValue.SelectedItem.Value == "-1")
            {
                if (String.IsNullOrEmpty(WlasnoscOpis.Value))
                {
                    Tools.ShowMessage("Proszę podać opis lokalu mieszkalnego.");
                    return false;
                }
            }
            return true;
        }

        //private bool ValidateDataRozp()
        //{
          
        //    return true;
        //}

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

        private bool ValidateWarianty()
        {
            String parId = cntWarianty.GetSelectedParId();
            if (String.IsNullOrEmpty(parId))
            {
                Tools.ShowMessage("Proszę wybrać wariant ubezpieczenia!");
                return false;
            }
            return true;
        }

        private bool ValidateCheckboxes()
        {
            if (!PermAdm.CbValue.Checked)
            {
                Tools.ShowMessage("Proszę o wyrażenie zgody dostępu do danych");
                return false;
            }

            if (!PermCudzy.CbValue.Checked)
            {
                Tools.ShowMessage("Proszę o wyrażenie wszystkich wymaganych zgód.");
                return false;
            }

            if (!PermAssistance.CbValue.Checked)
            {
                Tools.ShowMessage("Proszę o wyrażenie wszystkich wymaganych zgód.");
                return false;
            }

            if (!ZgodaNaPotracenie.CbValue.Checked)
            {
                Tools.ShowMessage("Proszę o wyrażenie zgody na potrącenie składki z wynagrodzenia!");
                return false;
            }




            return true;
        }

        private bool ValidateDates()
        {
            string dataOd = DataOd.Value;
            if (String.IsNullOrEmpty(dataOd))
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
            
            if(dtOd < nextMonth || dtOd.Day != 1)
            {
                Tools.ShowMessage("Niepoprawna data ubezpieczenia!");
                return false;
            }

            Adres adresUbezp = GetUbezpAdres();
            DataTable dt = db.Select.Table(dsCheckIfAlreadyExists, dataOd, adresUbezp.Ulica, adresUbezp.Dom, adresUbezp.Lokal, adresUbezp.Kod, adresUbezp.Miasto, App.User.Id);
            if (dt.Rows.Count > 0)
            {
                Tools.ShowMessage("Istnie już ubezpieczenie na ten adres i datę.");
                return false;
            }

            return true;
        }

        #endregion

        public String GetEditClass()
        {
            return "wn-formularz " + (Edit ? "edit" : "preview");
        }

        class Adres
        {
            public Adres() { }
            public Adres(String Miasto, String Ulica, String Kod, String Dom, String Lokal)
            {
                this.Miasto = Miasto;
                this.Ulica = Ulica;
                this.Kod = Kod;
                this.Dom = Dom;
                this.Lokal = Lokal;
            }
            public Adres(DataRow drAdres)
            {
                this.Miasto = db.getValue(drAdres, "AdresMiasto");
                this.Ulica = db.getValue(drAdres, "AdresUlica");
                this.Kod = db.getValue(drAdres, "AdresKod");
                this.Dom = db.getValue(drAdres, "AdresDom");
                this.Lokal = db.getValue(drAdres, "AdresLokal");
            }
            public Adres(Adres Adr)
            {
                this.Miasto = Adr.Miasto;
                this.Ulica = Adr.Ulica;
                this.Kod = Adr.Kod;
                this.Dom = Adr.Dom;
                this.Lokal = Adr.Lokal;
            }
            public String Miasto { get; set; }
            public String Ulica { get; set; }
            public String Kod { get; set; }
            public String Dom { get; set; }
            public String Lokal { get; set; }
        }

        Adres GetUbezpAdres()
        {
            if (String.IsNullOrEmpty(ddlAdresyUbezp.SelectedValue))
                return null;

            Adres adr = null;
            if (ddlAdresyUbezp.SelectedValue == "-1")
                adr = new Adres(tbAdresUbezpMiasto.Text, tbAdresUbezpUlica.Text, tbAdresUbezpKod.Text, tbAdresUbezpDom.Text, tbAdresUbezpLokal.Text);
            else
            {
                DataRow drAdres = db.Select.Row(dsAdres, ddlAdresyUbezp.SelectedValue);
                adr = new Adres(drAdres);
            }
            return adr;
        }

        Adres GetKorAdres()
        {
            if (rblKoresp.SelectedValue == "1")
                return new Adres(GetUbezpAdres());
            else
            {
                string val = ddlAdresyKor.SelectedValue;
                switch (val)
                {
                    case "-1":
                        return new Adres(tbAdresKorMiasto.Text, tbAdresKorUlica.Text, tbAdresKorKod.Text, tbAdresKorDom.Text, tbAdresKorLokal.Text);
                        break;
                    default:
                        DataRow drAdres = db.Select.Row(dsAdres, val);
                        return new Adres(drAdres);
                }

            }
            return null;
        }

        #region SAVE

        protected void btnSaveConfirm_Click(object sender, EventArgs e)
        {
            if (Validate())
                Tools.ShowConfirm("Czy na pewno chcesz wysłać wniosek?", btnSave);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                Save();
                if (Saved != null)
                    Saved(sender, e);
                this.Close();
            }
        }

        public void Save()
        {
            String parId = cntWarianty.GetSelectedParId();
            String plusId = cntWarianty.GetSelectedPlusId();

            bool plus = !String.IsNullOrEmpty(plusId);
            bool same = (rblKoresp.SelectedValue == "1");
            bool cesja = (rblCesja.SelectedValue == "1");

            Adres adresUbezp = GetUbezpAdres();
            Adres adresKor = GetKorAdres();

            DataRow drVariantParameters = db.Select.Row(dsVariantParameters, parId, db.nullParam(plusId));
            string sumaUbezpieczenia, skladka, sumaUbezpieczeniaPlus, skladkaPlus;

            sumaUbezpieczenia = db.getValue(drVariantParameters, "Suma");
            sumaUbezpieczeniaPlus = db.getValue(drVariantParameters, "SumaPlus");
            skladka = db.getValue(drVariantParameters, "Skladka");
            skladkaPlus = db.getValue(drVariantParameters, "SkladkaPlus");

            int identity = dbField.dbInsert(db.conP, this, "poWnioskiMajatkowe"
                , @"ZglaszajacyId, ParId, PlusId, Plus, Imie, Nazwisko, Narodowosc, DataUrodzenia, SumaUbezpieczenia, Skladka, SumaUbezpieczeniaPlus, SkladkaPlus, StatusName, Status, AdresKorSame, AdresKorType
                , AdresUbezpType, PESEL, Cesja, AdresUbezpDom, AdresUbezpKod, AdresUbezpLokal, AdresUbezpMiasto, AdresUbezpUlica, AdresKorDom, AdresKorKod, AdresKorLokal, AdresKorMiasto, AdresKorUlica"
                , App.User.Id, parId, db.nullParam(plusId), plus ? "1" : "0", db.strParam(App.User.Imie), db.strParam(App.User.Nazwisko), db.strParam("PL"), db.nullParam(null), sumaUbezpieczenia, skladka
                , sumaUbezpieczeniaPlus, skladkaPlus, db.strParam(Wlasnosc.RblValue.SelectedItem.Text), 0, same ? "1" : "0", db.nullParam(ddlAdresyKor.SelectedValue), db.nullParam(ddlAdresyUbezp.SelectedValue)
                , db.strParam(App.User.Nick), cesja ? "1" : "0", db.nullStrParam(adresUbezp.Dom), db.nullStrParam(adresUbezp.Kod), db.nullStrParam(adresUbezp.Lokal), db.nullStrParam(adresUbezp.Miasto)
                , db.nullStrParam(adresUbezp.Ulica), db.nullStrParam(adresKor.Dom), db.nullStrParam(adresKor.Kod), db.nullStrParam(adresKor.Lokal), db.nullStrParam(adresKor.Miasto), db.nullStrParam(adresKor.Ulica));

            if (identity != -1)
            {

                db.Execute(dsCosmos, identity);
                Log.Info(1337, String.Format("Dodano wniosek o ubezpieczenie mieszkaniowe o Id = {0}. ZglaszajacyId = {1}, DataRozpoczecia = {2} ", identity, App.User.Id, DataOd.Value), "");
                string createdId = identity.ToString();
                SendMails(createdId);

                //DataTable dt = db.Select.Table(dsCheckIfNext, App.User.Id, DataOd.Value, createdId);
                //if (dt.Rows.Count > 0) // jak ubezpieczamy kolejne mieszkanie to tylko zmieniamy kwote w assecco
                //{
                //    if (!Ubezpieczenia.Ubezpieczenia.UpdateSkladnikKwota(App.User.NR_EW, Ubezpieczenia.Ubezpieczenia.GetSkladkaSumWniosek(App.User.Id, DataOd.Value), DataOd.Value))
                //        Mailing.EventUbezpieczeniaMajatkowe(Mailing.maUBEZP_ERR, createdId);
                //}
                //else
                //{
                //    if (!Ubezpieczenia.Ubezpieczenia.InsertSkladnik(App.User.NR_EW, Ubezpieczenia.Ubezpieczenia.GetSkladkaSumWniosek(App.User.Id, DataOd.Value), DataOd.Value))
                //        Mailing.EventUbezpieczeniaMajatkowe(Mailing.maUBEZP_ERR, createdId);
                //}

            }
            else
                Tools.ShowMessage("Wystąpił błąd! Administrator został powiadomiony o tym zdarzeniu.");

        }

        public void SendMails(string createdId)
        {
            // dla składającego
            Mailing.EventUbezpieczeniaMajatkowe(Mailing.maUBEZP_POTW, createdId);
            // dla adminów
            Mailing.EventUbezpieczeniaMajatkowe(Mailing.maUBEZP_POTW_ADM, createdId);
        }

        #endregion

        //public double GetSkladkaSum(string skladka, string skladkaPlus)
        //{
        //    double sum = 0, skladkaF = 0, skladkaPF = 0;
        //    try
        //    {
        //        skladkaF = Convert.ToDouble(skladka);
        //    }
        //    catch { }
        //    try
        //    {
        //        skladkaPF = Convert.ToDouble(skladkaPlus);
        //    }
        //    catch { }
        //    sum = skladkaF + skladkaPF;
        //    return sum;
        //}

        protected void ddlAdresyUbezp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = (sender as DropDownList).SelectedValue;
            if (!String.IsNullOrEmpty(val))
            {
                switch (val)
                {
                    case "-1":
                        ToggleAddrUbezp(true);
                        break;
                    default:
                        ToggleAddrUbezp(false);
                        break;
                }
            }
            else
            {
                ToggleAddrUbezp(false);
            }
        }

        void ToggleAddrUbezp(bool b)
        {
            trAddrUbezp1.Visible = b;
            trAddrUbezp2.Visible = b;
            if (!b)
            {
                tbAdresUbezpDom.Text = "";
                tbAdresUbezpKod.Text = "";
                tbAdresUbezpLokal.Text = "";
                tbAdresUbezpMiasto.Text = "";
                tbAdresUbezpUlica.Text = "";
            }
        }

        protected void rblKoresp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = (sender as RadioButtonList).SelectedValue;

            if (val == "1")
            {
                tbAdresKor.Visible = false;
                tbAdresKorUlica.ValidationGroup = "";
                tbAdresKorDom.ValidationGroup = "";
                tbAdresKorMiasto.ValidationGroup = "";
                tbAdresKorKod.ValidationGroup = "";
            }
            else
            {
                tbAdresKor.Visible = true;
                trAddrKor0.Visible = true;
                trAddrKor1.Visible = false;
                trAddrKor2.Visible = false;
                tbAdresKorUlica.ValidationGroup = "vgSave";
                tbAdresKorDom.ValidationGroup = "vgSave";
                tbAdresKorMiasto.ValidationGroup = "vgSave";
                tbAdresKorKod.ValidationGroup = "vgSave";
            }
        }

        protected void ddlAdresyKor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = (sender as DropDownList).SelectedValue;
            if (!String.IsNullOrEmpty(val))
            {
                switch (val)
                {
                    case "-1":
                        ToggleAddrKor(true);
                        break;
                    default:
                        ToggleAddrKor(false);
                        break;
                }
            }
            else
            {
                ToggleAddrKor(false);
            }
        }

        void ToggleAddrKor(bool b)
        {
            trAddrKor1.Visible = b;
            trAddrKor2.Visible = b;
            if (!b)
            {
                tbAdresKorDom.Text = "";
                tbAdresKorKod.Text = "";
                tbAdresKorLokal.Text = "";
                tbAdresKorMiasto.Text = "";
                tbAdresKorUlica.Text = "";
            }
        }

        protected void Wlasnosc_Changed(object sender, EventArgs e)
        {
            WlasnoscOpis.Visible = (Wlasnosc.Value == "-1");
        }

        protected void rblCesja_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = (sender as RadioButtonList).SelectedValue;
            tbCesja.Visible = (val == "1");
        }

        public bool IsCesjaVisible()
        {
            return rblCesja.SelectedValue == "1";
        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            db.Execute(dsRestore, RequestId);
            if (Saved != null)
                Saved(sender, e);
            this.Close();
        }

        protected void btnRestoreConfirm_Click(object sender, EventArgs e)
        {
            Tools.ShowConfirm("Czy na pewno chcesz przywrócić polisę?", btnRestore);
        }

        protected void Rodzaj_Changed(object sender, EventArgs e)
        {
            cntWarianty.Update(Rodzaj.RblValue.SelectedValue);
        }

        public String RequestId
        {
            get { return ViewState["vRequestId"] as String; }
            set { ViewState["vRequestId"] = value; }
        }

        public Boolean Edit
        {
            get { return Tools.GetViewStateBool(ViewState["vEdit"], false); }
            set { ViewState["vEdit"] = value; }
        }

    }
}




//, db.nullStrParam(adresUbezpDom) 
//, db.nullStrParam(adresUbezpKod)
//, db.nullStrParam(adresUbezpLokal)
//, db.nullStrParam(adresUbezpMiasto)
//, db.nullStrParam(adresUbezpUlica)
//, db.nullStrParam(adresKorDom)
//, db.nullStrParam(adresKorKod)
//, db.nullStrParam(adresKorLokal)
//, db.nullStrParam(adresKorMiasto)
//, db.nullStrParam(adresKorUlica)
//--, AdresUbezpDom
//--, AdresUbezpKod

//, AdresUbezpType
//--, AdresUbezpLokal
//--, AdresUbezpMiasto
//--, AdresUbezpUlica
//--, AdresKorDom
//--, AdresKorKod
//--, AdresKorLokal
//--, AdresKorMiasto
//--, AdresKorUlica

/*, AdresUbezpDom
, AdresUbezpKod
, AdresUbezpLokal
, AdresUbezpMiasto
, AdresUbezpUlica
, AdresKorDom
, AdresKorKod
, AdresKorLokal
, AdresKorMiasto
, AdresKorUlica*/


