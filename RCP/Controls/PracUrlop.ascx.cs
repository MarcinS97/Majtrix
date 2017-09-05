using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class PracUrlop : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(string pracId)
        {
            PracId = pracId;
            gvUrlopy.DataBind();
        }

        /*
        public void Show(string pracId, string panelId, string parent)
        {
            Prepare(pracId);
            string nazwisko = db.getScalar("select Nazwisko + ' ' + Imie + ' (' + KadryId + ')' from Pracownicy where Id = " + pracId);
            string up = Parent.Parent.ClientID; // update panel tu jest 
            up = parent;
            Tools.MakeButton(btClose, "javascript: $('#divZoom').dialog('close'); return true;");
            Tools.ExecOnStart2("scZoomUrlop", "zoomPracUrlop('Urlopy pracownika: " + nazwisko + "','" + up + "');");
        }
        */
        public string PracId
        {
            get { return hidPracId.Value; }
            set { hidPracId.Value = value; }
        }
    }
}