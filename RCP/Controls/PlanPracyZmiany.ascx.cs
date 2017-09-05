using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class PlanPracyZmiany : System.Web.UI.UserControl
    {
        public event EventHandler DataSaved;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                paLegenda.Visible = Lic.ppPrint;
                paTitle.Visible = Lic.ppPrint;
                btPrint.Visible = Lic.ppPrint;

#if RCP
                ddlKier.Visible = true;
#endif

            }
        }
        //---------------------------
        public void Prepare(string kierId, DateTime nadzien, bool restoreFromSession)
        {
            hidKierId.Value = kierId;

            cntSelectOkres.Prepare(nadzien, restoreFromSession);
            cntPlanPracy.Prepare(kierId, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, cntSelectOkres.OkresId, cntSelectOkres.Status, cntSelectOkres.IsArch);
            SetPrintHeader();
        }

        public void Prepare(string kierId)
        {
            hidKierId.Value = kierId;
            cntPlanPracy.PracId = null;
            //cntPlanPracy.Prepare(kierId, cntSelectOkres.DateFrom, cntSelectOkres.DateTo);
            cntPlanPracy.Prepare(kierId);
            SetPrintHeader();
        }

        public void SetEditMode(bool edit)
        {
            cntPlanPracy.EditMode = edit;
            
            cntSelectZmiana.EditMode = edit;
            lbZmianyQ.Visible = !edit;
            lbZmianyE.Visible = edit;
            lbPlanQ.Visible = !edit;
            lbPlanE.Visible = edit;

            btEditPP.Visible = !edit;
            btCheckPP.Visible = edit;
            btSavePP.Visible = edit;

            if (Lic.HarmAcc)
                btScheme.Visible = edit;


            btCancelPP.Visible = edit;
            cntSelectOkres.Enabled = !edit;

            if (Lic.ppPrint)
                btPrint.Visible = !edit;
        }

        private void SetPrintHeader()
        {
            if (Lic.ppPrint)
            {
                string okres = cntSelectOkres.FriendlyName(1);
                string kid = cntPlanPracy.IdKierownika;
                if (String.IsNullOrEmpty(kid))
                {
                    repHeader.Caption = String.Format("Plan pracy: {0}", okres);
                }
                else
                {
                    string kier = AppUser.GetNazwiskoImie(cntPlanPracy.IdKierownika);
                    repHeader.Caption = String.Format("Plan pracy: {0}, przełożony: {1}", okres, kier);
                }
            }
        }
        //---------------------------
        protected void OnSelectZmiana(object sender, EventArgs e)
        {
            cntPlanPracy.Zmiana = cntSelectZmiana.SelectedZmiana;
        }

        protected void btCheckPP_Click(object sender, EventArgs e)
        {
            //bool empty
            if (cntPlanPracy.Check())
                Tools.ShowMessage("Plan pracy poprawny.");
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

        protected void cntSelectOkres_Changed(object sender, EventArgs e)
        {
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
            
            SetPrintHeader();
        }
        //-----------------------------
        public SelectOkres OkresRozl
        {
            get { return cntSelectOkres; }
        }

        public int Adm
        {
            set { cntPlanPracy.Adm = value; }
            get { return cntPlanPracy.Adm; }
        }

        protected void ddlPrac_SelectedIndexChanged(object sender, EventArgs e)
        {
            cntPlanPracy.PracId = ddlPrac.SelectedValue;
        }

        protected void ddlKier_SelectedIndexChanged(object sender, EventArgs e)
        {
            //hidKierId.Value = ddlKier.SelectedValue;
            Prepare(ddlKier.SelectedValue);
        }

        protected void btScheme_Click(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            db.Execute(dsApplyScheme, ddlSchemes.SelectedValue, ddlPrac2.SelectedValue, db.strParam(deLeft.DateStr), db.strParam(deRight.DateStr));
        }
    }
}