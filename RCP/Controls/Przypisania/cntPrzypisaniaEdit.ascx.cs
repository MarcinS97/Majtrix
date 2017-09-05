using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.Przypisania
{
    public partial class cntPrzypisaniaEdit : System.Web.UI.UserControl
    {
        public enum TMode { ADM, KIER };
        TMode FMode = TMode.KIER;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(string kierId, DateTime data)
        {
            Data = Tools.DateToStr(data);
            cntStruktura1.Prepare(kierId, data);
            cntStruktura2.Prepare(null, data, kierId, 0);
        }

        public void InitScript()
        {
            cntStruktura1.InitScript();
            cntStruktura2.InitScript();
        }

        public void Reload()
        {
            string kierId = cntStruktura1.RootId;
            string sel1 = cntStruktura1.SelectedPrzId;
            string sel2 = cntStruktura2.SelectedPrzId;
            cntStruktura1.Prepare(kierId, Data, sel1, 3);
            cntStruktura2.Prepare(null, Data, sel2, 3);
            UpdatePanel3.Update();
            UpdatePanel1.Update();
        }

        public void Reload1()
        {
            string kierId = cntStruktura1.RootId;
            string sel1 = cntStruktura1.SelectedPrzId;
            /*
            if (cntParametry2.TreeChanged)
                cntStruktura1.Prepare(kierId, Data, sel1, 3);
            else
                cntStruktura1.Select(sel1, 3, false);
            */
            cntStruktura1.Prepare(kierId, Data, sel1, 3);
            UpdatePanel3.Update();

            //cntParametry2.SetKierownik(cntStruktura2.SelectedNode);  // zaznaczam ponownie kierownika
            //UpdatePanel2.Update();
        }
        //--------------------
        private void Clear()
        {
            /*
            deFrom.DateStr = null;
            deTo.DateStr = null;
            ddlCC.SelectedIndex = 0;
            */
        }

        private bool Validate()
        {
            return false;
        }

        protected void btZapisz_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                Clear();
            }
        }
        //--------------------
        protected void cntStruktura1_SelectedChanged(object sender, EventArgs e)
        {
            cntParametry1.Prepare(cntStruktura1.SelectedPracId, Data);
            cntParametry2.SetPracownik(cntParametry1);
            if (!String.IsNullOrEmpty(cntStruktura1.SelectedPracId) && AppUser.GetIsKierownik(cntStruktura1.SelectedPracId) && !App.User.IsAdmin)
                Tools.ShowWarning("Nie masz uprawnień do przeniesienia kierownika. Skontaktuj się z administratorem.");
            /*
            if (String.IsNullOrEmpty(cntStruktura1.SelectedPracId) || !AppUser.GetIsKierownik(cntStruktura1.SelectedPracId) || App.User.IsAdmin)
            {
                cntParametry1.Prepare(cntStruktura1.SelectedPracId, Data);
                //cntParametry2.SelPracId = cntStruktura1.SelectedPracId;
                //cntParametry2.SetPracownik(cntStruktura1.SelectedNode);
                cntParametry2.SetPracownik(cntParametry1);
            }
            else
                Tools.ShowError("Nie masz uprawnień do przeniesienia kierownika. Skontaktuj się z administratorem.");
             */ 
        }

        protected void cntStruktura2_SelectedChanged(object sender, EventArgs e)
        {
            //cntParametry2.SelKierId = cntStruktura2.SelectedPracId;
            cntParametry2.SetKierownik(cntStruktura2.SelectedNode);
        }

        protected void cntParamerty2_StructureChanged(object sender, EventArgs e)
        {
            //cntStruktura1.Reload();
            Reload1();
        }

        protected void cnt_MoveSettings(object sender, EventArgs e)
        {
            cntParametry2.SetParams(cntParametry1);
        }
        //--------------------
        public string Data
        {
            get { return hidData.Value; }
            set { hidData.Value = value; }
        }

        public TMode Mode
        {
            get { return FMode; }
            set 
            { 
                FMode = value;
                cntParametry2.Mode = FMode == TMode.ADM ? cntPrzypisaniaParametry.TMode.ADDADM : cntPrzypisaniaParametry.TMode.ADDKIER; 
            }
        }
    }
}