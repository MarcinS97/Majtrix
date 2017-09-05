using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Controls.Ewaluacja
{
    public partial class cntAnkietaP : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Prepare();
            }
        }

        void Prepare()
        {
            DataRow dr = db.Select.Row(dsData, AnkietaId);
            if (PracView)
            {
                // weryfikujemy czy na pewno mozemy ogladac te ankiete
                PracView = (db.getValue(dr, "IdPracownikaCert") == App.User.Id);
            }


            bool preview = App.User.HasRight(AppUser.rMSAnkietyPodgladP) || App.User.IsMSAdmin || PracView;
            bool edit = App.User.HasRight(AppUser.rMSAnkietyEdycjaP) || App.User.IsMSAdmin || PracView;
                
            if (Request.Params.Count < 1 || (!preview && !edit))
            {
                App.ShowNoAccess(null, App.User);
            }
            else
            {
                tbPracownik.Text = db.getValue(dr, "PracownikCert", String.Empty);
                tbStanowisko.Text = db.getValue(dr, "StanowiskoPrac", String.Empty);
                tbTematSzkolenia.Text = db.getValue(dr, "TematSzkolenia", String.Empty);
                tbOrganizator.Text = db.getValue(dr, "Organizator", String.Empty);
                tbProwadzacy.Text = db.getValue(dr, "ProwadzacySzkolenie", String.Empty);
                tbMiejsce.Text = db.getValue(dr, "MiejsceSzkolenia", String.Empty);
                tbCzasTrwania.Text = db.getValue(dr, "CzasTrwania", String.Empty);
                deDataSzkolenia.Date = db.getDateTime(dr, "DataSzkolenia", DateTime.Now);

                tbOcena1.Text = db.getValue(dr, "Odp1", String.Empty);
                tbOcena2.Text = db.getValue(dr, "Odp2", String.Empty);
                tbOcena3.Text = db.getValue(dr, "Odp3", String.Empty);
                tbOcena4.Text = db.getValue(dr, "Odp4", String.Empty);
                tbOcena5.Text = db.getValue(dr, "Odp5", String.Empty);
                tbOcena6.Text = db.getValue(dr, "Odp6", String.Empty);
                tbOcena7.Text = db.getValue(dr, "Odp7", String.Empty);
                tbOcena8.Text = db.getValue(dr, "Odp8", String.Empty);
                tbOcena9.Text = db.getValue(dr, "Odp9", String.Empty);
                tbOcena10.Text = db.getValue(dr, "Odp10", String.Empty);
                tbOcena11.Text = db.getValue(dr, "Odp11", String.Empty);
                tbOcena12.Text = db.getValue(dr, "Odp12", String.Empty);
                tbOcena13.Text = db.getValue(dr, "Odp13", String.Empty);
                tbOcena14.Text = db.getValue(dr, "Odp14", String.Empty);

                tbUwagi.Text = db.getValue(dr, "Uwagi", String.Empty);

                //ddlPracownik.SelectedValue = db.getValue(dr, "IdPracownika", null);

                hidPracId.Value = db.getValue(dr, "IdPracownika", null);
                hidStanId.Value = db.getValue(dr, "IdStanowiska", null);

                string status = db.getValue(dr, "Status", null);
                lblStatus.Text = db.getValue(dr, "StatusName", "");
                lblStatus.CssClass = db.getValue(dr, "StatusColor", "");
                lblTraining.Text = db.getValue(dr, "Szkolenie", "");

                SetVisibility(preview, edit, status);
            }

        }

        void SetVisibility(bool preview, bool edit, string status)
        {
            if (!String.IsNullOrEmpty(status)) 
            {
                divStatus0.Visible = (status == "0" && edit);
                divStatus1.Visible = (status == "1" && App.User.IsMSAdmin);
                divRejected.Visible = (status == "-1" && App.User.IsMSAdmin);
                divRemove.Visible = App.User.IsMSAdmin;
                btnPrint.Visible = btnPrint2.Visible = true;
                SetEnabled(status == "0" && edit);
            }
            else
            {
                divStatus0.Visible = false;
                divStatus1.Visible = false;
                divRemove.Visible = false;
                divRejected.Visible = false;
                btnPrint.Visible = btnPrint2.Visible = false;
                SetEnabled(false);
            }
        }

        void SetEnabled(bool b)
        {
            tbPracownik.Enabled = false;
            tbStanowisko.Enabled = false;
            tbTematSzkolenia.Enabled = b;
            tbOrganizator.Enabled = b;
            tbProwadzacy.Enabled = b;
            tbMiejsce.Enabled = b;
            tbCzasTrwania.Enabled = b;
            deDataSzkolenia.ReadOnly = !b;

            tbOcena1.Enabled = b;
            tbOcena2.Enabled = b;
            tbOcena3.Enabled = b;
            tbOcena4.Enabled = b;
            tbOcena5.Enabled = b;
            tbOcena6.Enabled = b;
            tbOcena7.Enabled = b;
            tbOcena8.Enabled = b;
            tbOcena9.Enabled = b;
            tbOcena10.Enabled = b;
            tbOcena11.Enabled = b;
            tbOcena12.Enabled = b;
            tbOcena13.Enabled = b;
            tbOcena14.Enabled = b;

            tbUwagi.Enabled = b;

        }

        Dictionary<String, object> GetData()
        {
            Dictionary<String, object> Data = new Dictionary<String, object>();
            Data["Pracownik"] = tbPracownik.Text;
            Data["Stanowisko"] = tbStanowisko.Text;
            Data["Temat"] = tbTematSzkolenia.Text;
            Data["Organizator"] = tbOrganizator.Text;
            Data["Prowadzacy"] = tbProwadzacy.Text;
            Data["Miejsce"] = tbMiejsce.Text;
            Data["CzasTrwania"] = tbCzasTrwania.Text;
            Data["DataSzkolenia"] = deDataSzkolenia.Date;

            Data["Ocena1"] = tbOcena1.Text;
            Data["Ocena2"] = tbOcena2.Text;
            Data["Ocena3"] = tbOcena3.Text;
            Data["Ocena4"] = tbOcena4.Text;
            Data["Ocena5"] = tbOcena5.Text;
            Data["Ocena6"] = tbOcena6.Text;
            Data["Ocena7"] = tbOcena7.Text;
            Data["Ocena8"] = tbOcena8.Text;
            Data["Ocena9"] = tbOcena9.Text;
            Data["Ocena10"] = tbOcena10.Text;
            Data["Ocena11"] = tbOcena11.Text;
            Data["Ocena12"] = tbOcena12.Text;
            Data["Ocena13"] = tbOcena13.Text;
            Data["Ocena14"] = tbOcena14.Text;

            Data["Uwagi"] = tbUwagi.Text;
            
            Data["DataUtworzenia"] = DateTime.Now.ToShortDateString();

            //Data["IdPracownika"] = ddlPracownik.SelectedValue;
            Data["IdPracownika"] = hidPracId.Value;
            Data["IdStanowiska"] = hidStanId.Value;
            return Data;
        }

        
        void Save(string id)
        {
            Dictionary<String, object> Data = GetData();

            db.Execute(dsSave
                , db.nullParamStr(Data["Pracownik"])
                , db.nullParamStr(Data["Stanowisko"])
                , db.nullParamStr(Data["Temat"])
                , db.nullParamStr(Data["Organizator"])
                , db.nullParamStr(Data["Prowadzacy"])
                , db.nullParamStr(Data["Miejsce"])
                , db.nullParamStr(Data["CzasTrwania"])
                , db.nullParamStr(Data["DataSzkolenia"])
                , db.nullParamStr(Data["Ocena1"])
                , db.nullParamStr(Data["Ocena2"])
                , db.nullParamStr(Data["Ocena3"])
                , db.nullParamStr(Data["Ocena4"])
                , db.nullParamStr(Data["Ocena5"])
                , db.nullParamStr(Data["Ocena6"])
                , db.nullParamStr(Data["Ocena7"])
                , db.nullParamStr(Data["Ocena8"])
                , db.nullParamStr(Data["Ocena9"])
                , db.nullParamStr(Data["Ocena10"])
                , db.nullParamStr(Data["Ocena11"])
                , db.nullParamStr(Data["Ocena12"])
                , db.nullParamStr(Data["Ocena13"])
                , db.nullParamStr(Data["Ocena14"])
                , db.nullParamStr(Data["Uwagi"])
                , db.nullParamStr(Data["IdPracownika"])
                , db.nullParamStr(Data["IdStanowiska"])
                , id
                );
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string text;
            using (var streamReader = new StreamReader(Server.MapPath("~/MatrycaSzkolen/Templates/aou.rtf"), Encoding.GetEncoding(1250)))
            {
                text = streamReader.ReadToEnd();
            }

            Dictionary<String, Object> Data = GetData();


            foreach(KeyValuePair<String, Object> item in Data)
            {
                string key = item.Key;
                object o = item.Value;
                string value = string.Empty;
                if (o is DateTime)
                    value = Tools.GetDateTime(o, DateTime.Now).ToShortDateString();
                else
                    value = Tools.GetStr(item.Value, string.Empty);

                text = text.Replace(String.Format("%{0}%", key), value);


            }


            Response.Clear();
            Response.AddHeader("Content-Disposition", String.Format(@"attachment; filename=""{0}""", "aou.rtf"));
            Response.AddHeader("Content-Length", text.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = Encoding.GetEncoding(1250);
            Response.Write(text);
            Response.Flush();
            Response.End();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save(AnkietaId);
        }

        protected void ddlPracownik_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(!String.IsNullOrEmpty(ddlPracownik.SelectedValue))
            //    tbPracownik.Text = ddlPracownik.SelectedItem.Text;
        }

        protected void btnSaveAndBlock_Click(object sender, EventArgs e)
        {
            Tools.ShowConfirm("Czy na pewno chcesz zablokować ankietę do edycji?", btnSaveAndBlockConfirm);
        }

        protected void btnSaveAndBlockConfirm_Click(object sender, EventArgs e)
        {
            Save(AnkietaId);
            db.Execute(dsAccept, AnkietaId);
            Log.Info(1337, "Zaakceptowano ankietę", AnkietaId);
            Prepare();
        }

        protected void btnUnlock_Click(object sender, EventArgs e)
        {
            Tools.ShowConfirm("Czy na pewno chcesz odblokować ankietę?", btnUnlockConfirm);
        }

        protected void btnUnlockConfirm_Click(object sender, EventArgs e)
        {
            Unlock(AnkietaId);
            Prepare();            
        }

        void Unlock(string id)
        {
            db.Execute(dsUnlock, id);
            Log.Info(1337, "Odblokowano ankietę", AnkietaId);
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            Tools.ShowConfirm("Czy na pewno chcesz usunąć ankietę?", btnRemoveConfirm);
        }

        protected void btnRemoveConfirm_Click(object sender, EventArgs e)
        {
            Remove(AnkietaId);
            App.Redirect("MatrycaSzkolen/Ewaluacja.aspx");
        }

        void Remove(string id)
        {
            db.Execute(dsRemove, id);
            Log.Info(1337, "Usunięto ankietę", AnkietaId);
        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            Tools.ShowConfirm("Czy na pewno chcesz przywrócić ankietę?", btnRestoreConfirm);

        }

        protected void btnRestoreConfirm_Click(object sender, EventArgs e)
        {
            Restore(AnkietaId);
        }

        void Restore(string id)
        {
            db.Execute(dsRestore, id);
            Log.Info(1337, "Przywrócono ankietę", AnkietaId);
        }

        public string AnkietaId
        {
            get { return Request.Params[0]; }
        }

        public bool PracView
        {
            get { return Tools.GetViewStateBool(ViewState["vPracView"], false); }
            set { ViewState["vPracView"] = value; }
        }

    }
}