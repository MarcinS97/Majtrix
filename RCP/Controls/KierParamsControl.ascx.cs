using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class KierParamsControl : System.Web.UI.UserControl
    {
        public event EventHandler Changed;   // tak na prawde jak saved
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(string kierId)
        {
            hidKierId.Value = kierId;
            SetEditMode(false);
            FillData();
        }

        public void SetEditMode(bool edit)
        {
            btEdit.Visible = !edit;
            btEdit.Enabled = App.User.HasRight(AppUser.rKierParams);
            btSave.Visible = edit;
            btCancel.Visible = edit;
            ddlPrzerwa.Enabled = edit;
            ddlPrzerwa2.Enabled = edit;
            ddlMargin.Enabled = edit;
        }

        public void FillData()
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            KierParams kp = new KierParams(hidKierId.Value, settings);
            App.FillBreak(ddlPrzerwa, kp.KierPrzerwa.ToString(), settings.Przerwa.ToString(), false);
            App.FillBreak(ddlPrzerwa2, kp.KierPrzerwa2.ToString(), settings.Przerwa2.ToString(), true);
            App.FillBreak(ddlMargin, kp.KierMargines.ToString(), settings.Margines.ToString(), false);
        }

        private void clearErrorMarkers()
        {
            //Tools.SetErrorMarker(lbColCzasZast, false);
        }

        public bool CheckAndUpdate()
        {
            clearErrorMarkers();
            bool ok = KierParams.Update(hidKierId.Value, ddlPrzerwa.SelectedValue, ddlPrzerwa2.SelectedValue, ddlMargin.SelectedValue);
            if (!ok)
                Tools.ShowMessage("Błąd podczas zapisu do bazy.");
            return ok;
        }
        //-----------------------------
        protected void btEdit_Click(object sender, EventArgs e)
        {
            if (App.User.HasRight(AppUser.rKierParams))
            {
                SetEditMode(true);
                FillData();
            }
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            if (App.User.HasRight(AppUser.rKierParams))  // na wszelki wypadek
            {
                if (CheckAndUpdate())
                {
                    SetEditMode(false);
                    if (Changed != null)
                        Changed(this, EventArgs.Empty);
                }
            }
            else
            {
                SetEditMode(false);
                FillData();  // przywracam
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            SetEditMode(false);
            FillData();  // przywracam
        }
        //-----------------------------
        public bool EditMode
        {
            get { return !btEdit.Visible; }
            set { SetEditMode(value); }
        }
    }
}