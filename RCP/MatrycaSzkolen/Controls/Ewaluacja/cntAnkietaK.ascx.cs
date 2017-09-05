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
    public partial class cntAnkietaK : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Prepare();
            }
        }

        void Prepare()
        {
            bool preview = App.User.HasRight(AppUser.rMSAnkietyPodgladK) || App.User.IsMSAdmin;
            bool edit = App.User.HasRight(AppUser.rMSAnkietyEdycjaK) || App.User.IsMSAdmin;
            if (Request.Params.Count < 1 || (!preview && !edit))
            {
                App.ShowNoAccess(null, App.User);
            }
            else
            {
                DataRow dr = db.Select.Row(dsData, AnkietaId);
                tbPracownik.Text = db.getValue(dr, "PracownikCert", String.Empty);
                tbTematSzkolenia.Text = db.getValue(dr, "TematSzkolenia", String.Empty);
                deDataSzkolenia.Date = db.getDateTime(dr, "DataSzkolenia", DateTime.Now);
                tbCel.Text = db.getValue(dr, "CelSzkolenia", String.Empty);
                tbMonitDni.Text = db.getValue(dr, "MonitDniActual", String.Empty);
                bool monitEdit = db.getBool(dr, "MonitDniEdit", false);


                string oc1 = db.getValue(dr, "Odp1", null);
                string oc2 = db.getValue(dr, "Odp2", null);
                string oc3 = db.getValue(dr, "Odp3", null);
                string oc4 = db.getValue(dr, "Odp4", null);
                string oc5 = db.getValue(dr, "Odp5", null);
                string oc6 = db.getValue(dr, "Odp6", null);


                rbl1.SelectedValue = oc1;
                tbOcena1.Text = oc2;
                tbTekst1.Text = db.getValue(dr, "Tekst1", String.Empty);

                rbl2.SelectedValue = oc3;
                tbOcena2.Text = oc4;
                tbTekst2.Text = db.getValue(dr, "Tekst2", String.Empty);


                rbl3.SelectedValue = oc5;
                tbTekst3.Text = db.getValue(dr, "Tekst3", String.Empty);


                tbOcena3.Text = oc6;

                string status = db.getValue(dr, "Status", null);

                lblStatus.Text = db.getValue(dr, "StatusName", "");
                lblStatus.CssClass = db.getValue(dr, "StatusColor", "");

                lblTraining.Text = db.getValue(dr, "Szkolenie", "");

                SetVisibility(preview, edit, status, monitEdit);
            }
        }

        void SetVisibility(bool preview, bool edit, string status, bool monitEdit)
        {
            if (!String.IsNullOrEmpty(status))
            {
                divStatus0.Visible = (status == "0" && edit);
                divStatus1.Visible = (status == "1" && App.User.IsMSAdmin);
                divRemove.Visible = App.User.IsMSAdmin;
                divRejected.Visible = (status == "-1" && App.User.IsMSAdmin);
                btnPrint.Visible = true;
                SetEnabled(status == "0" && edit, monitEdit);
            }
            else
            {
                divStatus0.Visible = false;
                divStatus1.Visible = false;
                divRemove.Visible = false;
                btnPrint.Visible = false;
                divRejected.Visible = false;
                SetEnabled(false, monitEdit);
            }

        }

        void SetEnabled(bool b, bool monitEdit)
        {
            tbPracownik.Enabled = false;
            tbTematSzkolenia.Enabled = b;
            deDataSzkolenia.ReadOnly = !b;
            tbCel.Enabled = b;
            tbMonitDni.Enabled = monitEdit;

            rbl1.Enabled = rbl2.Enabled = rbl3.Enabled = b;
            tbOcena1.Enabled = tbOcena2.Enabled = tbOcena3.Enabled = b;
            tbTekst1.Enabled = tbTekst2.Enabled = tbTekst3.Enabled = b;
        }


        Dictionary<String, object> GetData()
        {
            Dictionary<String, object> Data = new Dictionary<String, object>();
            Data["Pracownik"] = tbPracownik.Text;
            Data["Temat"] = tbTematSzkolenia.Text;
            Data["DataSzkolenia"] = deDataSzkolenia.Date;
            Data["CelSzkolenia"] = tbCel.Text;
            Data["MonitDni"] = tbMonitDni.Text;

            Data["Odp1"] = rbl1.SelectedValue;
            Data["Odp2"] = tbOcena1.Text;
            Data["Odp3"] = rbl2.SelectedValue;
            Data["Odp4"] = tbOcena2.Text;
            Data["Odp5"] = rbl3.SelectedValue;
            Data["Odp6"] = tbOcena3.Text;

            Data["Tekst1"] = tbTekst1.Text;
            Data["Tekst2"] = tbTekst2.Text;
            Data["Tekst3"] = tbTekst3.Text;
            //Data["Tekst4"] = tbTekst4.Text;
            Data["DataUtworzenia"] = DateTime.Now.ToShortDateString();

            return Data;
        }


        void Save(string id)
        {
            Dictionary<String, object> Data = GetData();

            db.Execute(dsSave
                , db.nullParamStr(Data["Pracownik"])
                , db.nullParamStr(Data["Temat"])
                , db.nullParamStr(Data["DataSzkolenia"])
                , db.nullParamStr(Data["CelSzkolenia"])
                , db.nullParamStr(Data["MonitDni"])
                , db.nullParamStr(Data["Odp1"])
                , db.nullParamStr(Data["Odp2"])
                , db.nullParamStr(Data["Odp3"])
                , db.nullParamStr(Data["Odp4"])
                , db.nullParamStr(Data["Odp5"])
                , db.nullParamStr(Data["Odp6"])
                , db.nullParamStr(Data["Tekst1"])
                , db.nullParamStr(Data["Tekst2"])
                , db.nullParamStr(Data["Tekst3"])
                , id
                );
        }


        void ReplaceRTF(Dictionary<String, object> data, ref string text)
        {
            foreach (KeyValuePair<String, Object> item in data)
            {
                string key = item.Key;
                object o = item.Value;
                string value = string.Empty;
                if (o is DateTime)
                    value = Tools.GetDateTime(o, DateTime.Now).ToShortDateString();
                else
                    value = Tools.GetStr(item.Value, string.Empty);

                if (key == "Odp1") value = (value == "0") ? "Tak" : "Nie";
                else if (key == "Odp3") value = (value == "0") ? "Tak" : "Nie";
                else if (key == "Odp5") value = ((value == "0") ? "Tak" : (value == "1") ? "Nie" : "Nie jest to wymagane");

                text = text.Replace(String.Format("%{0}%", key), value);
            }
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string text;
            using (var streamReader = new StreamReader(Server.MapPath("~/MatrycaSzkolen/Templates/aop.rtf"), Encoding.GetEncoding(1250)))
            {
                text = streamReader.ReadToEnd();
            }

            Dictionary<String, Object> Data = GetData();


            string text1 = "Odpowiedź: %Odp1%, w stopniu: %Odp2%";
            string text2 = "Odpowiedź: %Odp3%, w stopniu: %Odp4%";
            string text3 = "Odpowiedź: %Odp5%";

            ReplaceRTF(Data, ref text1);
            Data["Odpowiedz1"] = text1;

            ReplaceRTF(Data, ref text2);
            Data["Odpowiedz2"] = text2;

            ReplaceRTF(Data, ref text3);
            Data["Odpowiedz3"] = text3;


            using (var streamReader = new StreamReader(Server.MapPath("~/MatrycaSzkolen/Templates/aop.rtf"), Encoding.GetEncoding(1250)))
            {
                text = streamReader.ReadToEnd();
            }

            ReplaceRTF(Data, ref text);


            //foreach (KeyValuePair<String, Object> item in Data)
            //{
            //    string key = item.Key;
            //    object o = item.Value;
            //    string value = string.Empty;
            //    if (o is DateTime)
            //        value = Tools.GetDateTime(o, DateTime.Now).ToShortDateString();
            //    else
            //        value = Tools.GetStr(item.Value, string.Empty);

            //    if (key == "Odp1") value = (value == "0") ? "Tak" : "Nie";
            //    else if (key == "Odp3") value = (value == "0") ? "Tak" : "Nie";
            //    else if (key == "Odp5") value = ((value == "0") ? "Tak" : (value == "1") ? "Nie" : "Nie jest to wymagane");

            //    text = text.Replace(String.Format("%{0}%", key), value);


            //}


            Response.Clear();
            Response.AddHeader("Content-Disposition", String.Format(@"attachment; filename=""{0}""", "aop.rtf"));
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

        public string AnkietaId
        {
            get { return Request.Params[0]; }
        }

        protected void ddlPracownik_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (!String.IsNullOrEmpty(ddlPracownik.SelectedValue))
                //tbPracownik.Text = ddlPracownik.SelectedItem.Text;
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

    }
}