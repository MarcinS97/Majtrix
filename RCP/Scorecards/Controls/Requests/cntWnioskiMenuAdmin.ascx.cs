using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards.Controls.Requests
{
    public partial class cntWnioskiMenuAdmin : System.Web.UI.UserControl
    {
        //private const string active_tab = "mnScWn";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie
        private const string _active_tab = "mnScWnAdm"; //T zeby sie nie myliło bo było jeszcze tak samo nazwane w cntParameters

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Boolean Admin = App.User.IsScAdmin;                
                hidObserverId.Value = App.User.Id;
                ddlSuperior.DataBind();
                ChangeObserver(ddlSuperior.SelectedValue);
                //if (Admin)
                //{
                //    tdAdmin.Visible = true;
                //    ChangeObserver(ddlSuperior.SelectedValue, "0");
                //}
                //else
                //{
                //    tdAdmin.Visible = false;
                //    ChangeObserver(App.User.Id, null);
                //}


                Tools.SelectMenuFromSession(mnLeft, _active_tab);
                SelectMenu(false);


                //cntSelectRokMiesiac.DataBind();
                ////----- zawężenie zakresu widocznych przełożonych -----
                //if (!App.User.IsScAdmin)
                //{
                //    ddlKier.DataBind();
                //    if (ddlKier.Items.Count > 2)    // wybierz... i ja zawsze jest
                //        tdKier.Visible = true;
                //}
                //else
                //    tdKier.Visible = true;
                //Tools.ExecOnStart2("resize2", "resize();");

                //if (App.User.HasRight(AppUser.rScorecardsTL)) HideShowToAcc(false);
                //if (!App.User.HasRight(AppUser.rScorecardsWnAcc) && !App.User.IsScAdmin) HideToAcc();
                //if (/*!App.User.IsSuperuser*/true) Tools.RemoveMenu(mnLeft, "vALL");
                //SelectMenuTexts();
            }
        }

        public void Prepare()
        {
        }

        //----------------
        public static void ResetSelectMenu()
        {
            HttpContext.Current.Session[_active_tab] = null;
        }
        
        private void SelectMenu(bool setHelp)
        {
            int indx = Tools.SelectTab(mnLeft, mvWnioski, _active_tab, setHelp);

        }

        protected void mnLeft_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectMenu(true);
            HideRequests();
        }

        void HideRequests()
        {
            // bo się nie chowały
            cntWnioskiPremioweAccKier.HideRequest();
            cntWnioskiPremioweAccdKier.HideRequest();
            cntWnioskiPremioweAccZarz.HideRequest();
            cntWnioskiPremioweAccdZarz.HideRequest();
            cntWnioskiPremioweAll.HideRequest();
            cntWnioskiPremioweHR.HideRequest();
            cntWnioskiPremioweNew.HideRequest();
            cntWnioskiPremioweRej.HideRequest();
        }


        //-----------------
        protected void ddlSuperior_SelectedIndexChanged(object sender, EventArgs e)
        {
            //String Upr = (ddlSuperior.SelectedValue == "0") ? rblUpr.SelectedValue : null;
            //rblUpr.Visible = !String.IsNullOrEmpty(Upr);
            ChangeObserver(ddlSuperior.SelectedValue);
        }

        void ChangeObserver(String Id)
        {
            //cntWnioskiPremioweAccKier.Prepare(Id, deDate.DateStr);
            //cntWnioskiPremioweAccZarz.Prepare(Id, deDate.DateStr);
            //cntWnioskiPremioweAll.Prepare(Id, deDate.DateStr);
            //cntWnioskiPremioweHR.Prepare(Id, deDate.DateStr);
            //cntWnioskiPremioweNew.Prepare(Id, deDate.DateStr);
            //cntWnioskiPremioweRej.Prepare(Id, deDate.DateStr);
            //cntWnioskiPremioweAccdKier.Prepare(Id, deDate.DateStr);
            //cntWnioskiPremioweAccdZarz.Prepare(Id, deDate.DateStr);

            cntWnioskiPremioweAccKier.Prepare(Id, ddlMiesiac.SelectedValue);
            cntWnioskiPremioweAccZarz.Prepare(Id, ddlMiesiac.SelectedValue);
            cntWnioskiPremioweAll.Prepare(Id, ddlMiesiac.SelectedValue);
            cntWnioskiPremioweHR.Prepare(Id, ddlMiesiac.SelectedValue);
            cntWnioskiPremioweNew.Prepare(Id, ddlMiesiac.SelectedValue);
            cntWnioskiPremioweRej.Prepare(Id, ddlMiesiac.SelectedValue);
            cntWnioskiPremioweAccdKier.Prepare(Id, ddlMiesiac.SelectedValue);
            cntWnioskiPremioweAccdZarz.Prepare(Id, ddlMiesiac.SelectedValue);

            HideRequests();
        }

        protected void cntSelectRokMiesiac_NextAll(object sender, EventArgs e)
        {
            //cntSelectRokMiesiac.SelectNow();
        }

        protected void cntSelectRokMiesiac_BackAll(object sender, EventArgs e)
        {
            //DateTime? dt = db.getScalar<DateTime>("SELECT TOP 1 miesiac FROM ccLimity where isLast = 1 AND Limit IS NOT NULL ORDER BY miesiac");
            //cntSelectRokMiesiac.Rok = 2015;
            //cntSelectRokMiesiac.Miesiac = 6;

            /*
            if (dt.HasValue)
            {
                SRM.Rok = dt.Value.Year;
                SRM.Miesiac = dt.Value.Month;
            }
            else
            {
                SRM.SelectNow();
            }
            */
        }

        protected void cntSelectRokMiesiac_ValueChanged(object sender, EventArgs e)
        {

        }

        protected void BindRequests()
        {

        }

        protected void ObserverChanged(object sender, EventArgs e)
        {
            //HideShowToAcc(sender.ToString() != "0"); 
        }

        protected void Changed(object sender, EventArgs e)
        {
            Tools.SetUpdated(ViewState);
        }

        protected void Activate1(object sender, EventArgs e)
        {
            if (Tools.IsUpdated(ViewState, 0x0001))
                cntWnioskiPremioweAll.Prepare();
            cntWnioskiPremioweAll.HideRequest();
        }

        protected void Activate2(object sender, EventArgs e)
        {
            if (Tools.IsUpdated(ViewState, 0x0002))
                cntWnioskiPremioweNew.Prepare();
            cntWnioskiPremioweNew.HideRequest();
        }

        protected void Activate3(object sender, EventArgs e)
        {
            if (Tools.IsUpdated(ViewState, 0x0004))
                cntWnioskiPremioweAccKier.Prepare();
            cntWnioskiPremioweAccKier.HideRequest();
        }

        protected void Activate4(object sender, EventArgs e)
        {
            if (Tools.IsUpdated(ViewState, 0x0008))
                cntWnioskiPremioweAccdKier.Prepare();
            cntWnioskiPremioweAccdKier.HideRequest();
        }

        protected void Activate5(object sender, EventArgs e)
        {
            if (Tools.IsUpdated(ViewState, 0x00010))
                cntWnioskiPremioweAccZarz.Prepare();
            cntWnioskiPremioweAccZarz.HideRequest();
        }

        protected void Activate6(object sender, EventArgs e)
        {
            if (Tools.IsUpdated(ViewState, 0x00020))
                cntWnioskiPremioweAccdZarz.Prepare();
            cntWnioskiPremioweAccdZarz.HideRequest();
        }

        protected void Activate7(object sender, EventArgs e)
        {
            if (Tools.IsUpdated(ViewState, 0x0030))
                cntWnioskiPremioweHR.Prepare();
            cntWnioskiPremioweHR.HideRequest();
        }

        protected void Activate8(object sender, EventArgs e)
        {
            if (Tools.IsUpdated(ViewState, 0x0040))
                cntWnioskiPremioweRej.Prepare();
            cntWnioskiPremioweRej.HideRequest();
        }

        protected void rblUpr_SelectedIndexChanged(Object sender, EventArgs e)
        {
            ChangeObserver(ddlSuperior.SelectedValue);
        }

        protected void btClear_Click(object sender, EventArgs e)
        {
            ddlSuperior.SelectedIndex = 0;
            //deDate.Date = null;
            ddlMiesiac.SelectedIndex = 0;
            ChangeObserver(ddlSuperior.SelectedValue);
            HideRequests();
        }

        void SelectMenuTexts()
        {
            //mnLeft.Items[0].Text = String.Format("Wszystkie wnioski ({0})", cntWnioskiPremioweAll.ItemsCount.ToString());
            //mnLeft.Items[1].Text = String.Format("W przygotowaniu ({0})", cntWnioskiPremioweNew.ItemsCount.ToString());
            //mnLeft.Items[2].Text = String.Format("Do akceptacji Kierownika ({0})", cntWnioskiPremioweAccKier.ItemsCount.ToString());
            //mnLeft.Items[3].Text = String.Format("Cofnięte przez Kierownika ({0})", cntWnioskiPremioweAccdKier.ItemsCount.ToString());
            //mnLeft.Items[4].Text = String.Format("Do akceptacji Zarządu ({0})", cntWnioskiPremioweAccZarz.ItemsCount.ToString());
            //mnLeft.Items[5].Text = String.Format("Cofnięte przez Zarząd ({0})", cntWnioskiPremioweAccdZarz.ItemsCount.ToString());
            //mnLeft.Items[6].Text = String.Format("Zaakceptowane - HR ({0})", cntWnioskiPremioweHR.ItemsCount.ToString());
            //mnLeft.Items[7].Text = String.Format("Odrzucone ({0})", cntWnioskiPremioweRej.ItemsCount.ToString());
        }
    }
}