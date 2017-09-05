using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class cntPlanUrlopow : System.Web.UI.UserControl
    {
        public event EventHandler DataSaved;

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        //---------------------------
        public void Prepare(string kierId, DateTime nadzien, bool restoreFromSession)
        {
            cntSelectOkres.Prepare(nadzien, restoreFromSession);
            UpdatePlanStatus();
            cntPlanPracy.Prepare(kierId, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, cntSelectOkres.OkresId, cntSelectOkres.Status, cntSelectOkres.IsArch);
        }

        public void Prepare(string kierId)
        {
            //cntPlanPracy.Prepare(kierId, cntSelectOkres.DateFrom, cntSelectOkres.DateTo);
            cntPlanPracy.Prepare(kierId);
        }

        public void SetEditMode(bool edit)
        {
            cntPlanPracy.EditMode = edit;
            
            cntSelectZmiana.EditMode = edit;
            lbZmianyQ.Visible = !edit;
            lbZmianyE.Visible = edit;
            lbPlanQ.Visible = !edit;
            lbPlanE.Visible = edit;

            //btEditPP.Visible = !edit;
            btEditPP.Visible = false;   // na razie
            
            btCheckPP.Visible = edit;
            btSavePP.Visible = edit;
            btCancelPP.Visible = edit;
            cntSelectOkres.Enabled = !edit;
        }
        //---------------------------
        protected void OnSelectZmiana(object sender, EventArgs e)
        {
            cntPlanPracy.Zmiana = cntSelectZmiana.SelectedUrlop;
        }

        protected void btCheckPP_Click(object sender, EventArgs e)
        {
            //bool empty
            if (cntPlanPracy.Check())
                Tools.ShowMessage("Plan urlopów poprawny.");
        }

        protected void btEditPP_Click(object sender, EventArgs e)
        {
            SetEditMode(true);
        }

        protected void btSavePP_Click(object sender, EventArgs e)
        {
            if (cntPlanPracy.Check())
            {
                cntPlanPracy.Update();
                SetEditMode(false);
                if (DataSaved != null)
                    DataSaved(this, EventArgs.Empty);
            }
        }

        protected void btCancelPP_Click(object sender, EventArgs e)
        {
            cntPlanPracy.DataBind();
            SetEditMode(false);
        }

        private bool UpdatePlanStatus()
        {
            int status;
            string opis;
            DateTime planStart, planStop;
            cntPlanRoczny.GetStatus((DateTime)Tools.StrToDateTime(cntSelectOkres.DateFrom), DateTime.Today, null, out status, out opis, out planStart, out planStop);
            /*
            switch (status)
            {
                case cntPlanRoczny.stPlanowanie:
                    btEditPP.Text
                    break;
            }
             */
            cntPlanPracy.PlanUrlopowStatus = status;

            lbOkresStatus.Text = opis;
            lbOkresStatus.Visible = true;
            return status == cntPlanRoczny.stPlanowanie || status == cntPlanRoczny.stKorekta;
        }

        protected void cntSelectOkres_Changed(object sender, EventArgs e)
        {
            /*
            bool v;     // czy okres jest dostępny do akceptacji 
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            DateTime dt = DateTime.Parse(cntSelectOkres.DateTo);
            if (dt < settings.SystemStartDate)
            {
                v = false;
                lbOkresStatus.Text = "Brak danych";
            }
            else
            {
                v = cntSelectOkres.Status != Okres.stClosed;
                if (cntSelectOkres.Status == Okres.stClosed)
                    lbOkresStatus.Text = "Okres rozliczeniowy zamknięty";
            }
            btEditPP.Visible = v;
            lbOkresStatus.Visible = !v;
            cntPlanPracy.OkresClosed = !v;
            */

            btEditPP.Visible = false;
            UpdatePlanStatus();
        }

        protected void cntPlanPracy_ShowPlanUrlopow(object sender, EventArgs e)
        {
            PlanUrlopow.Show(cntPlanPracy.SelPracId, cntPlanPracy.DateFrom);
        }
        
        //-----------------------------
        public SelectOkres OkresRozl
        {
            get { return cntSelectOkres; }
        }

        public int Mode
        {
            set { cntPlanPracy.Adm = value; }
            get { return cntPlanPracy.Adm; }
        }
    }
}