using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.IO;
using System.Text;
using HRRcp.Controls;
using System.Data;

namespace HRRcp.MatrycaSzkolen.Controls
{
    public partial class cntKartaZgloszenie2 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //try
                //{
                //    IdZgloszenia = Request.QueryString["p"].ToString();
                //}
                //catch
                //{
                //    IdZgloszenia = "";
                //}

                //Prepare();

                //rpParti.DataSource = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
                //rpParti.DataBind();

                //lbDataSzkolenia.InnerText = "Planowana data szkolenia";
                //ddlOrganizator.DataSourceID = "dsFirmy";
                //ddlOrganizator.DataTextField = "Text";
                //ddlOrganizator.DataValueField = "Value";
                //trKosztyDodatkowe.Visible = false;
                //ll5.Visible = false;
            }
        }

        public void Prepare()
        {
            //try
            //{
            //    IdZgloszenia = Request.QueryString["p"].ToString();
            //}
            //catch
            //{
            //    IdZgloszenia = "";
            //}

            //Prepare();

            rpParti.DataSource = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
            rpParti.DataBind();

            lbDataSzkolenia.InnerText = "Planowana data szkolenia";
            ddlOrganizator.DataSourceID = "dsFirmy";
            ddlOrganizator.DataTextField = "Text";
            ddlOrganizator.DataValueField = "Value";
            trKosztyDodatkowe.Visible = false;
            ll5.Visible = false;


            //tbZglaszajacy.Text = App.User.NazwiskoImie;

            //ddlZglaszajacy.SelectedValue = App.User.Id;
            //deDataZgloszenia.Date = DateTime.Now;

            if (!String.IsNullOrEmpty(IdZgloszenia))
            {
                DataRow dr = db.Select.Row(dsHeader, IdZgloszenia);
                ddlZglaszajacy.SelectedValue = db.getValue(dr, "IdPracownika");
                deDataZgloszenia.Date = db.getValue(dr, "DataZgloszenia");
                ddlRodzajSzkolenia.SelectedValue = db.getValue(dr, "RodzajSzkolenia");
                ddlOrganizator.SelectedValue = db.getValue(dr, "Organizator");
                tbTematSzkolenia.Text = db.getValue(dr, "Temat");
                deDataSzkolenia.Date = db.getValue(dr, "DataSzkolenia");
                tbMiejsceSzkolenia.Text = db.getValue(dr, "Miejsce");
                tbKosztSzkolenia.Text = db.getValue(dr, "Koszt");
                ddlZglaszajacy.Enabled = false;
                deDataZgloszenia.ReadOnly = true;
                ddlRodzajSzkolenia.Enabled = false;
                ddlOrganizator.Enabled = false;
                tbTematSzkolenia.Enabled = false;
                deDataSzkolenia.ReadOnly = true;
                tbMiejsceSzkolenia.Enabled = false;
                tbKosztSzkolenia.Enabled = false;

                btnSave.Visible = false;
            }
            else
            {
                ddlZglaszajacy.SelectedValue = App.User.Id;
                deDataZgloszenia.Date = DateTime.Now;
            }
        }

        Dictionary<String, String> GetData()
        {
            Dictionary<String, String> Data = new Dictionary<String, String>();
            Data["Zglaszajacy"] = ddlZglaszajacy.SelectedValue;
            if (deDataZgloszenia.Date != null)
                Data["DataZgloszenia"] = ((DateTime)deDataZgloszenia.Date).ToShortDateString();
            else
                Data["DataZgloszenia"] = null;
            Data["RodzajSzkolenia"] = ddlRodzajSzkolenia.SelectedItem.Text;
            Data["Organizator"] = ddlOrganizator.SelectedItem.Text;
            Data["Temat"] = tbTematSzkolenia.Text;
            if (deDataSzkolenia.Date != null)
                Data["DataSzkolenia"] = ((DateTime)deDataSzkolenia.Date).ToShortDateString();
            else
                Data["DataSzkolenia"] = null;
            Data["Miejsce"] = tbMiejsceSzkolenia.Text;
            Data["CalkowityKoszt"] = tbKosztSzkolenia.Text;
            Data["DodatkowyKoszt"] = tbKosztyDodatkowe.Text;




            return Data;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string text;
            using (var streamReader = new StreamReader(Server.MapPath("~/MatrycaSzkolen/Templates/kzns.rtf"), Encoding.GetEncoding(1250)))
            {
                text = streamReader.ReadToEnd();
            }

            Dictionary<String, String> Data = GetData();


            //ReplaceMarker(text, Data, "%ZGLASZAJACY%", "Zglaszajacy");

            text = text.Replace("%ZGLASZAJACY%", Data["Zglaszajacy"]);
            text = text.Replace("%DATA_ZGLOSZENIA%", Data["DataZgloszenia"]);
            text = text.Replace("%RODZAJ_SZKOLENIA%", Data["RodzajSzkolenia"]);
            text = text.Replace("%ORGANIZATOR%", Data["Organizator"]);
            text = text.Replace("%TEMAT_SZKOLENIA%", Data["Temat"]);
            text = text.Replace("%DATA_SZKOLENIA%", Data["DataSzkolenia"]);
            text = text.Replace("%MIEJSCE_SZKOLENIA%", Data["Miejsce"]);
            text = text.Replace("%KOSZT_SZKOLENIA%", Data["CalkowityKoszt"]);
            text = text.Replace("%KOSZTY_DODATKOWE%", Data["DodatkowyKoszt"]);

            string prac, data;
            int index = 1;
            foreach (RepeaterItem item in rpParti.Items)
            {
                //prac = Tools.GetText(item, "tbImieNazwisko");
                //DateEdit de = item.FindControl("deDataUrodzenia") as DateEdit;
                //if (de.Date != null)
                //    data = ((DateTime)de.Date).ToShortDateString();
                //else
                //    data = string.Empty;

                string pracId = Tools.GetDdlSelectedValue(item, "ddlEmployee");


                if (!String.IsNullOrEmpty(pracId))
                    prac = Tools.GetDdlSelectedText(item, "ddlEmployee");
                else
                    prac = "";

                string cc = Tools.GetDdlSelectedValue(item, "ddlCC");

                if (!String.IsNullOrEmpty(cc))
                    data = Tools.GetDdlSelectedText(item, "ddlCC");
                else
                    data = "";

                text = text.Replace(String.Format("%PRACOWNIK{0}%", index.ToString()), prac);
                text = text.Replace(String.Format("%DATA_UR{0}%", index.ToString()), data);

                index++;
            }


            Response.Clear();
            Response.AddHeader("Content-Disposition", String.Format(@"attachment; filename=""{0}""", "kzns.rtf"));
            Response.AddHeader("Content-Length", text.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = Encoding.GetEncoding(1250);
            Response.Write(text);
            Response.Flush();
            Response.End();



        }

        void ReplaceMarker(string text, Dictionary<String, String> data, string marker, string key)
        {
            string value = string.Empty;
            try
            {
                value = data[key];
            }
            catch { }
            text = text.Replace(marker, value);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> Data = GetData();

            foreach (RepeaterItem item in rpParti.Items)
            {
                string pracId = Tools.GetDdlSelectedValue(item, "ddlEmployee");

                if (!String.IsNullOrEmpty(pracId))
                {
                    string cc = Tools.GetDdlSelectedValue(item, "ddlCC");
                    if (String.IsNullOrEmpty(cc))
                    {
                        Tools.ShowError("Uzupełnij CC!");
                        return;
                    }
                }
            }

            String NewId = db.Select.Scalar(dsHeaderSave
                , db.nullParam(Data["Zglaszajacy"])
                , db.nullStrParam(Data["DataZgloszenia"])
                , db.nullParam(ddlRodzajSzkolenia.SelectedValue)
                , db.nullStrParam(ddlOrganizator.SelectedValue)
                , db.nullStrParam(tbTematSzkolenia.Text)
                , db.nullStrParam(Data["DataSzkolenia"])
                , db.nullStrParam(Data["Miejsce"])
                , db.nullParam(Data["CalkowityKoszt"].Replace(',', '.'))
                );

            if (!String.IsNullOrEmpty(NewId))
            {
                foreach (RepeaterItem item in rpParti.Items)
                {
                    string pracId = Tools.GetDdlSelectedValue(item, "ddlEmployee");

                    if (!String.IsNullOrEmpty(pracId))
                    {
                        string cc = Tools.GetDdlSelectedValue(item, "ddlCC");
                        db.Execute(dsSave, NewId, pracId, db.nullParam(cc));
                    }
                }
            }

            Tools.ShowMessage("Zapisano!");

        }

        public String IdZgloszenia
        {
            get
            {
                return hidIdZgloszenia.Value;
            }
            set
            {
                hidIdZgloszenia.Value = value;
            }
        }

        DataTable dt = null;
        int index = -1;

        protected void rpParti_DataBinding(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(IdZgloszenia))
            {
                index = -1;
                dt = db.Select.Table(dsEmployees, IdZgloszenia);
            }
        }

        protected void rpParti_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (!String.IsNullOrEmpty(IdZgloszenia))
            {
                if (index < dt.Rows.Count && index != -1)
                {
                    Tools.SelectItem(e.Item, "ddlEmployee", db.getValue(dt.Rows[index], "IdPracownika"));
                    Tools.SelectItem(e.Item, "ddlCC", db.getValue(dt.Rows[index], "IdCC"));
                }
                ++index;
                Tools.SetControlEnabled(e.Item, "ddlEmployee", false);
                Tools.SetControlEnabled(e.Item, "ddlCC", false);
            }
        }
    }
}