using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class AdmSettingsControl : System.Web.UI.UserControl
    {
        Ustawienia settings;

        protected void Page_Load(object sender, EventArgs e)
        {
            settings = Ustawienia.CreateOrGetSession();
            if (!IsPostBack)
            {
                FillData();
                SetEditMode(false);
            }
        }

        protected void btEdit_Click(object sender, EventArgs e)
        {
            FillData();   // odczytujemy wartosci z bazy na wypadek gdyby byly przez kogos zmienione
            SetEditMode(true);
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            if (CheckAndUpdate())
                SetEditMode(false);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            SetEditMode(false);
            FillData();   //odczytujemy warosci z bazy - przywracamy edity
        }
        //---------------------
        private void FillData()
        {
            tbADController.Text = settings.ADKontroler;
            tbADPath.Text = settings.ADPath;
        }

        private void SetEditMode(bool fEdit)
        {
            Tools.SetEdit(tbADController, fEdit);
            Tools.SetEdit(tbADPath, fEdit);
            btCancel.Visible = fEdit;
            btSave.Visible = fEdit;
            btEdit.Visible = !fEdit;
        }

        private void clearErrorMarkers()
        {
            //Tools.SetErrorMarker(lbColCzasZast, false);
        }

        private bool CheckAndUpdate()
        {
            clearErrorMarkers();
            int err = settings.Update(tbADController.Text, tbADPath.Text); // walidacja wartośc w środku !!!
            switch (err)
            {
                case 0: return true;  // ok!
                default: 
                    Tools.ShowMessage("Błąd podczas zapisu do bazy.");
                    break;
            }
            return false;
        }
        //-----------------------------------------

    }
}