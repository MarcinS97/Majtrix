using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class zoomUrlopy : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Show(string pracId, string parentUpdatePanel)
        {
            cntUrlop.Prepare(pracId);
            string nazwisko = db.getScalar("select Nazwisko + ' ' + Imie + ' (' + KadryId + ')' from Pracownicy where Id = " + pracId);
            Tools.MakeButton(btClose, "javascript:$('#divZoom').dialog('close');return true;");
            Tools.ShowDialog("divZoom", parentUpdatePanel, "Urlopy pracownika: " + nazwisko);
        }

        public void Show(string pracId)
        {
            cntUrlop.Prepare(pracId);
            string nazwisko = db.getScalar("select Nazwisko + ' ' + Imie + ' (' + KadryId + ')' from Pracownicy where Id = " + pracId);
            Tools.ShowDialog(this, "divZoom", null, btClose, "Urlopy pracownika: " + nazwisko);
        }
    }
}