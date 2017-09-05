using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards.Controls
{
    public partial class cntWnioskiMenu : System.Web.UI.UserControl
    {
        private const string _active_tab = "mnScWn2";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Boolean Admin = App.User.IsScAdmin;
                hidObserverId.Value = App.User.Id;
                ddlSuperior.DataBind();
                //if (Admin)
                //{
                //    tdAdmin.Visible = true;
                //    ChangeObserver(ddlSuperior.SelectedValue, "0");
                //}
                //else
                {
                    tdAdmin.Visible = false;
                    ChangeObserver(App.User.Id, null);

                }
                
                
                //T - to wystarczy przeniesc za HideToAcc i nie trzeba zerowac sesji - jak nie znajdzie to zaznaczy pierwszą opcję
                //Tools.SelectMenuFromSession(mnLeft, _active_tab);
                //SelectMenu(false);


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
                if (!App.User.HasRight(AppUser.rScorecardsWnAcc)) HideToAcc();   //T: Admin ma swój panel
                if (/*!App.User.IsSuperuser*/true) Tools.RemoveMenu(mnLeft, "vALL");


                Tools.SelectMenuFromSession(mnLeft, _active_tab);
                SelectMenu(false);
            }
        }

        protected void HideToAcc()
        {
            Tools.RemoveMenu(mnLeft, "vTOACC");
            Tools.RemoveMenu(mnLeft, "vACC");
            Tools.RemoveMenu(mnLeft, "vWYJ");
            Tools.RemoveMenu(mnLeft, "vREJ");
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

            // bo się nie chowały
            cntWnioskiPremioweMy.HideRequest();
            cntWnioskiPremioweAcc.HideRequest();
            cntWnioskiPremioweRej.HideRequest();
            cntWnioskiPremioweWyj.HideRequest();
            cntWnioskiPremioweToAcc.HideRequest();
        }
        //-----------------
        protected void ddlSuperior_SelectedIndexChanged(object sender, EventArgs e)
        {
            String Upr = (ddlSuperior.SelectedValue == "0") ? rblUpr.SelectedValue : null;
            rblUpr.Visible = !String.IsNullOrEmpty(Upr);
            ChangeObserver(ddlSuperior.SelectedValue, Upr);
        }

        void ChangeObserver(String Id, String Upr)
        {
            cntWnioskiPremioweAcc.Prepare(Id, Upr);
            cntWnioskiPremioweMy.Prepare(Id, Upr);
            cntWnioskiPremioweRej.Prepare(Id, Upr);
            cntWnioskiPremioweToAcc.Prepare(Id, Upr);
            cntWnioskiPremioweWyj.Prepare(Id, Upr);
            
            cntWnioskiPremioweAcc.HideRequest();
            cntWnioskiPremioweMy.HideRequest();
            cntWnioskiPremioweRej.HideRequest();
            cntWnioskiPremioweToAcc.HideRequest();
            cntWnioskiPremioweWyj.HideRequest();
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
                cntWnioskiPremioweMy.Prepare();
            cntWnioskiPremioweMy.HideRequest();
        }

        protected void Activate2(object sender, EventArgs e)
        {
            if (Tools.IsUpdated(ViewState, 0x0002))
                cntWnioskiPremioweToAcc.Prepare();
            cntWnioskiPremioweToAcc.HideRequest();
        }

        protected void Activate3(object sender, EventArgs e)
        {
            if (Tools.IsUpdated(ViewState, 0x0004))
                cntWnioskiPremioweAcc.Prepare();
            cntWnioskiPremioweAcc.HideRequest();
        }

        protected void Activate4(object sender, EventArgs e)
        {
            if (Tools.IsUpdated(ViewState, 0x0008))
                cntWnioskiPremioweRej.Prepare();
            cntWnioskiPremioweRej.HideRequest();
        }

        protected void Activate5(object sender, EventArgs e)
        {
            //if (Tools.IsUpdated(ViewState, 0x0010))
            //    cntWnioskiPremioweAll.Prepare();
            //cntWnioskiPremioweAll.HideRequest();
        }

        protected void Activate6(object sender, EventArgs e)
        {
            if (Tools.IsUpdated(ViewState, 0x0020))
                cntWnioskiPremioweWyj.Prepare();
            cntWnioskiPremioweWyj.HideRequest();
        }

        protected void rblUpr_SelectedIndexChanged(Object sender, EventArgs e)
        {
            ChangeObserver(ddlSuperior.SelectedValue, rblUpr.SelectedValue);
        }
    }
}