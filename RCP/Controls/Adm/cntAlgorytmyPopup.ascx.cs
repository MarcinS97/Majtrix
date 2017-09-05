using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class cntAlgorytmyPopup : System.Web.UI.UserControl
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
            cntAlgorytmy.PracId = pracId;
            string title = String.Format("Algorytm liczenia czasu pracy - {0}", AppUser.GetNazwiskoImieNREW(pracId));
            //string title = String.Format("Parametry liczenia czasu pracy - {0}", AppUser.GetNazwiskoImieNREW(pracId));
            Tools.ShowDialog(this, paModalPopup.ClientID, 1000, btCancel, title);  // 1100
        }

        public void Close()
        {
            Tools.CloseDialog(paModalPopup.ClientID);
        }

        public string CurrentAlgorytm = null;
        //public string CurrentStrefa = null;
        public string CurrentOdDo = null;
        public int WymiarCzasuSec = 0;
        public int PrzerwaWliczonaSec = 0;
        public int PrzerwaNiewliczonaSec = 0;

        private bool CheckInEdit
        {
            set { ViewState["chked"] = value; }
            get { return Tools.GetBool(ViewState["chked"], true); }
        }

        private void Update()
        {
            if (cntAlgorytmy.Updated)
                if (Changed != null)
                {
                    cntAlgorytmy.GetCurrent(out CurrentAlgorytm,
                        //out CurrentStrefa,
                                            out WymiarCzasuSec,
                                            out PrzerwaWliczonaSec,
                                            out PrzerwaNiewliczonaSec,
                                            out CurrentOdDo);
                    Changed(this, EventArgs.Empty);
                }
            cntAlgorytmy.PracId = null;
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            Update();
        }
    }
}