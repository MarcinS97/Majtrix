using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Portal.Controls
{
    public partial class zoomUrlopy : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Show(string pracId, string parentUpdatePanel)
        {
            cntModal.Show();
            cntUrlop.Prepare(pracId);
            string nazwisko = db.getScalar("select Nazwisko + ' ' + Imie + ' (' + KadryId + ')' from Pracownicy where Id = " + pracId);
            cntModal.Title = "Urlopy pracownika: " + nazwisko;
            //Tools.MakeButton(btClose, "javascript:$('#divZoom').dialog('close');return true;");
            //Tools.ShowDialog("divZoom", parentUpdatePanel, "Urlopy pracownika: " + nazwisko);

        }

        public void Show(string pracId)
        {
            cntModal.Show();
            cntUrlop.Prepare(pracId);
            string nazwisko = db.getScalar("select Nazwisko + ' ' + Imie + ' (' + KadryId + ')' from Pracownicy where Id = " + pracId);
            cntModal.Title = "Urlopy pracownika: " + nazwisko;
            //Tools.ShowDialog(this, "divZoom", null, btClose, "Urlopy pracownika: " + nazwisko);
        }
    }
}