using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class cntTypOkresuRozlPopup : System.Web.UI.UserControl
    {
        public event EventHandler Changed;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                paModalPopup.Attributes["style"] = "display: none;";
            }
        }

        public void Show(string pracId)
        {
            cntTypOkresuRozl.PracId = pracId;
            string title = String.Format("Okres rozliczeniowy - {0}", AppUser.GetNazwiskoImieNREW(pracId));
            //string title = String.Format("Parametry liczenia czasu pracy - {0}", AppUser.GetNazwiskoImieNREW(pracId));
            Tools.ShowDialog(this, paModalPopup.ClientID, 1000, btCancel, title);  // 1100
        }

        public void Close()
        {
            Tools.CloseDialog(paModalPopup.ClientID);
        }

        public string CurrentOkres = null;
        //public string CurrentStrefa = null;
        public string CurrentOdDo = null;

        private bool CheckInEdit
        {
            set { ViewState["chked"] = value; }
            get { return Tools.GetBool(ViewState["chked"], true); }
        }

        private void Update()
        {
            if (cntTypOkresuRozl.Updated)
                if (Changed != null)
                {
                    cntTypOkresuRozl.GetCurrent(
                        out CurrentOkres,
                        out CurrentOdDo);
                    Changed(this, EventArgs.Empty);
                }
            cntTypOkresuRozl.PracId = null;
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            Update();
        }
    }
}