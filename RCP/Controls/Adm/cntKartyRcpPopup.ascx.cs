using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class cntKartyRcpPopup : System.Web.UI.UserControl
    {
        public event EventHandler Changed;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                paModalPopup.Attributes["style"] = "display: none;";
        }

        public void Show(string pracId)
        {
            cntKartyRcp1.PracId = pracId;
            string title = String.Format("Karty RCP - {0}", AppUser.GetNazwiskoImieNREW(pracId));
            Tools.ShowDialog(this, paModalPopup.ClientID, null, btCancel, title);
        }

        public void Close()
        {
            Tools.CloseDialog(paModalPopup.ClientID);
        }

        public string CurrentRcpId = null;
        public string CurrentNrKarty = null;

        private void Update()
        {
            if (cntKartyRcp1.Updated)
                if (Changed != null)
                {
                    //CurrentRcpId = cntKartyRcp1.GetCurrent();
                    string rcpid, nrkarty, grade;
                    cntKartyRcp1.GetCurrent(out rcpid, out nrkarty);
                    CurrentRcpId = rcpid;
                    CurrentNrKarty = nrkarty;
                    Changed(this, EventArgs.Empty);
                }
            cntKartyRcp1.PracId = null;
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            /*
            CancelChanges = false;
            cntKartyRcp1.Update();
            Update();
            Close();
             */ 
        }

        private bool CancelChanges
        {
            set { ViewState["canch"] = value; }
            get { return Tools.GetBool(ViewState["canch"], false); }
        }

        protected void btCancel2_Click(object sender, EventArgs e)
        {
            if (!CancelChanges && cntKartyRcp1.InEdit)
            {
                //CancelChanges = true;
                //Tools.ShowConfirm("Zapisać zmiany ?", btUpdate, btCancel2);
                Tools.ShowMessage("Proszę zakończyc edycję.");    // na razie tak ...
            }
            else
            {
                CancelChanges = false;
                Update();
                Close();
            }
        }

        /*
                if (cntAlgorytmy.InEdit)
                    Tools.Show



                bool upd;
                if (CheckInEdit)
                    if (cntAlgorytmy.ChecInEdit())
                        Tools.ShowConfirm("Zapisać zmiany ?");

                    upd = !cntAlgorytmy.CheckInEdit(((Button)sender));

                
                
         */

    }
}