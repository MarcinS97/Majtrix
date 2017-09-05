using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class cntStanowiskaPopup : System.Web.UI.UserControl
    {
        public event EventHandler Changed;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                paModalPopup.Attributes["style"] = "display: none;";
        }

        public void Show(string pracId)
        {
            cntStanowiska.PracId = pracId;
            string title = String.Format("Przypisania pracownika - {0}", AppUser.GetNazwiskoImieNREW(pracId));
            Tools.ShowDialog(this, paModalPopup.ClientID, 1100, btCancel, title);
        }

        public DataRow Current = null;

        protected void btCancel_Click(object sender, EventArgs e)
        {
            if (cntStanowiska.Updated)
                if (Changed != null)
                {
                    Current = cntStanowiska.GetCurrent();
                    Changed(this, EventArgs.Empty);
                }
            cntStanowiska.PracId = null;
        }
    }
}