using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.RozliczenieNadg
{
    public partial class cntRozlNadg : System.Web.UI.UserControl
    {
        const string ses_filter = "rn_filter";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SelectMenuFromSession(tabFilter, ses_filter);
                SelectFilter();
            }
        }

        public void Prepare(string pracId, string okresOd, string okresDo, string mies)   // mies jak np. okres 1 mies to tu jest data początku
        {
            //DateTime? dt = Tools.StrToDateTime(okresOd);
            //cntSelectOkres.Prepare(dt != null ? (DateTime)dt : DateTime.Today, pracId, false);
            //cntRozlNadgSuma.Prepare(pracId, okresOd, okresDo);
            //cntPrzeznaczNadg.Prepare(pracId, okresOd, okresDo);

            DateTime? dt = Tools.StrToDateTime(mies);
            cntSelectOkres.Prepare(dt != null ? (DateTime)dt : DateTime.Today, pracId, false);
            okresOd = cntSelectOkres.OkresOdStr;
            okresDo = cntSelectOkres.OkresDoStr;
            cntRozlNadgSuma.Prepare(pracId, okresOd, okresDo);
            cntPrzeznaczNadg.Prepare(pracId, okresOd, okresDo, cntSelectOkres.Status);
        }




        //------------------------------------------
        private bool showAccDate()
        {
            bool v;     // czy okres jest dostępny do akceptacji 
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            DateTime dt = cntSelectOkres.OkresDo;
            if (dt < settings.SystemStartDate)
            {
                v = false;
                lbOkresStatus.Text = "Brak danych";
            }
            else
            {
                v = cntSelectOkres.Status != cntSelectOkres3.okClosed;
                if (cntSelectOkres.Status == cntSelectOkres3.okClosed)
                    lbOkresStatus.Text = "Okres rozliczeniowy zamknięty";
            }
            //btAccept.Visible = v;
            lbOkresStatus.Visible = !v;
            //cntPlanPracy.OkresClosed = !v;
            return v;
        }

        protected void cntSelectOkres_Changed(object sender, EventArgs e)
        {
            showAccDate();
            string pracId = cntPrzeznaczNadg.PracId;
            string okresOd = cntSelectOkres.OkresOdStr;
            string okresDo = cntSelectOkres.OkresDoStr;
            cntRozlNadgSuma.Prepare(pracId, okresOd, okresDo);
            cntPrzeznaczNadg.Prepare(pracId, okresOd, okresDo, cntSelectOkres.Status);
        }

        protected void cntPrzeznaczNadg_Changed(object sender, EventArgs e)
        {
            cntRozlNadgSuma.Update();
        }

        //--------
        private void SelectFilter()
        {
            Session[ses_filter] = tabFilter.SelectedValue;
            cntPrzeznaczNadg.SetFilter(tabFilter.SelectedValue);
        }
        
        protected void tabFilter_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectFilter();
        }
    }
}