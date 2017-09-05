using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Ubezpieczenia.Majatkowe
{
    public partial class Warunki2 : System.Web.UI.Page
    {
        string typ = null;
        const string tuOgolne       = "OWU_";
        const string tuSzczegolne   = "SWU_";

        protected void Page_Load(object sender, EventArgs e)
        {
            const string WARUNKI = "WARUNKI_";

            typ = Request.QueryString["t"];
            string[] t = hidTypy.Value.Split('|');
            if (typ.IsAny(t))
            {
                cntPDFViewer.Grupa = WARUNKI + typ; // = "6515_OWU_prev.pdf";
                btSendMail.Enabled = true;
            }
            else
            {
                typ = null;
                btSendMail.Enabled = false;
            }
            /*
            switch(typ)
            {
                case tuOgolne:
                case tuSzczegolne:
                    cntPDFViewer.Grupa = WARUNKI + typ; // = "6515_OWU_prev.pdf";
                    btSendMail.Enabled = true;
                    break;
                default:
                    typ = null;
                    btSendMail.Enabled = false;
                    break;
            }
            */ 
            //litData.Text = String.Format("<embed src=\"{0}\" class=\"viewer\" >", path + fileName);
            if (!IsPostBack)
            {
                btSendMail.ToolTip = App.User.EMail;
            }
        }

        protected void btSendMail_Click(object sender, EventArgs e)
        {

            string tematS = "Szczególne warunki ubezpieczenia";   
            string tematO = "Ogólne warunki ubezpieczenia";
            string temat = "Warunki ubezpieczenia";
            string tresc = @"Dzień dobry

W załączeniu przesyłamy warunki ubezpieczenia.

Pozdrawiamy serdecznie.
Mail został wysłany automatycznie, prosimy na niego nie odpowiadać.
";
            string file;
            cntPDFViewer.GetGazetka(out file);
            file = MapPath(file);
            FileStream f = new FileStream(file, FileMode.Open);
            //if (Mailing.SendMail2(App.User.EMail, null, null, typ == tuOgolne ? tematO : tematS, tresc, f, Path.GetFileName(file), null) == 0)
            if (Mailing.SendMail2(App.User.EMail, null, null, typ.StartsWith(tuOgolne) ? tematO : typ.StartsWith(tuSzczegolne) ? tematS : temat, tresc, f, Path.GetFileName(file), null) == 0)
                Tools.ShowMessage("Mail na adres {0}  został wysłany poprawnie.", App.User.EMail);
            else
                Tools.ShowMessage("Wystąpił błąd podczas wysyłki maila na adres {0}.", App.User.EMail);
        }
    }
}