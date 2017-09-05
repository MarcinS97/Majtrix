using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Adm
{
    public partial class SzablonyAnkiet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void PrintEmployeeSurveyTemplate(object sender, EventArgs e)
        {
            string text;
            using (var streamReader = new StreamReader(Server.MapPath("~/MatrycaSzkolen/Templates/aou.rtf"), Encoding.GetEncoding(1250)))
            {
                text = streamReader.ReadToEnd();
            }

            text = text.Replace("%Pracownik%", "");
            text = text.Replace("%Stanowisko%", "");
            text = text.Replace("%Temat%", "");
            text = text.Replace("%Organizator%", "");
            text = text.Replace("%Prowadzacy%", "");
            text = text.Replace("%Miejsce%", "");
            text = text.Replace("%CzasTrwania%", "");
            text = text.Replace("%DataSzkolenia%", "");

            text = text.Replace("%Ocena1%", "");
            text = text.Replace("%Ocena2%", "");
            text = text.Replace("%Ocena3%", "");
            text = text.Replace("%Ocena4%", "");
            text = text.Replace("%Ocena5%", "");
            text = text.Replace("%Ocena6%", "");
            text = text.Replace("%Ocena7%", "");
            text = text.Replace("%Ocena8%", "");
            text = text.Replace("%Ocena9%", "");
            text = text.Replace("%Ocena10%", "");
            text = text.Replace("%Ocena11%", "");
            text = text.Replace("%Ocena12%", "");
            text = text.Replace("%Ocena13%", "");
            text = text.Replace("%Ocena14%", "");

            text = text.Replace("%DataUtworzenia%", DateTime.Now.ToShortDateString());
            text = text.Replace("%Uwagi%", "……………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………………….");


            Response.Clear();
            Response.AddHeader("Content-Disposition", String.Format(@"attachment; filename=""{0}""", "aou.rtf"));
            Response.AddHeader("Content-Length", text.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = Encoding.GetEncoding(1250);
            Response.Write(text);
            Response.Flush();
            Response.End();

        }

        protected void PrintSuperiorSurveyTemplate(object sender, EventArgs e)
        {
            string text;
            using (var streamReader = new StreamReader(Server.MapPath("~/MatrycaSzkolen/Templates/aop.rtf"), Encoding.GetEncoding(1250)))
            {
                text = streamReader.ReadToEnd();
            }

            text = text.Replace("%Pracownik%", "");
            text = text.Replace("%Temat%", "");
            text = text.Replace("%DataSzkolenia%", "");
            text = text.Replace("%CelSzkolenia%", "");
            text = text.Replace("%MonitDni%", "");
            text = text.Replace("%DataUtworzenia%", DateTime.Now.ToShortDateString());


            text = text.Replace("%Tekst1%", "……………………………………………………………………………….");
            text = text.Replace("%Tekst2%", "……………………………………………………………………………….");
            text = text.Replace("%Tekst3%", "……………………………………………………………………………….");
            text = text.Replace("%Odp6%", "…………………………………………………………………………………………………………………………………………………………….");

            text = text.Replace("%Odp6%", "…………………………………………………………………………………………………………………………………………………………….");

            text = text.Replace("%Odpowiedz1%", "");
            text = text.Replace("%Odpowiedz2%", "");
            text = text.Replace("%Odpowiedz3%", "");


            Response.Clear();
            Response.AddHeader("Content-Disposition", String.Format(@"attachment; filename=""{0}""", "aop.rtf"));
            Response.AddHeader("Content-Length", text.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = Encoding.GetEncoding(1250);
            Response.Write(text);
            Response.Flush();
            Response.End();

        }
    }
}