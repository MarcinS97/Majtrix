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

namespace HRRcp.MatrycaSzkolen.Controls
{
    public partial class cntKartaZgloszenie : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Prepare();

                rpParti.DataSource = new string[] { "1", "2", "3", "4", "5", "6", "7", "8"};
                rpParti.DataBind();

#if VEOLIA
                lbDataSzkolenia.InnerText = "Planowana data szkolenia";
                ddlOrganizator.DataSourceID = "dsFirmy";
                ddlOrganizator.DataTextField = "Text";
                ddlOrganizator.DataValueField = "Value";
                trKosztyDodatkowe.Visible = false;
                ll5.Visible = false;
#endif
            }
        }

        void Prepare()
        {
            tbZglaszajacy.Text = App.User.NazwiskoImie;
            deDataZgloszenia.Date = DateTime.Now;
        }

        Dictionary<String, String> GetData()
        {
            Dictionary<String, String> Data = new Dictionary<String, String>();
            Data["Zglaszajacy"] = tbZglaszajacy.Text;
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
            foreach(RepeaterItem item in rpParti.Items)
            {
                prac = Tools.GetText(item, "tbImieNazwisko");
                DateEdit de = item.FindControl("deDataUrodzenia") as DateEdit;
                if (de.Date != null)
                    data = ((DateTime)de.Date).ToShortDateString();
                else
                    data = string.Empty;

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
    }
}