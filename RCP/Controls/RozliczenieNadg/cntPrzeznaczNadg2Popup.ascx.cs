using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class cntPrzeznaczNadg2Popup : System.Web.UI.UserControl
    {
        public event EventHandler Changed;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                paModalPopup.Attributes["style"] = "display: none;";
        }

        public void Show(string pracId, string okresOd, string okresDo, int status)
        {
            cntPrzeznaczNadg21.Prepare(pracId, okresOd, okresDo, status);
            string title = String.Format("Rozliczenie nadgodzin - {0}", AppUser.GetNazwiskoImieNREW(pracId));
            Tools.ShowDialog(this, paModalPopup.ClientID, 1100, btCancel, title);  // 1100
        }

        public string CurrentAlgorytm = null;
        //public string CurrentStrefa = null;
        public string CurrentOdDo = null;

        protected void btCancel_Click(object sender, EventArgs e)
        {
            if (cntPrzeznaczNadg21.Updated)
                if (Changed != null)
                    Changed(this, EventArgs.Empty);
            cntPrzeznaczNadg21.Clear();
        }
    }
}