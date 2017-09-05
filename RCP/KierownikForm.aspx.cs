using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class KierownikForm : System.Web.UI.Page
    {
        private const string active_tab = "atabK";
        AppUser user;

        /*
#if SIEMENS
        const bool ppPrint = true;
        const bool wnUrlopowe = false;
#else
        const bool ppPrint = false;
        const bool wnUrlopowe = true;
#endif
        */

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            user = AppUser.CreateOrGetSession();
            if (!IsPostBack)
            {
                Tools.SetNoCache();
#if !KDR
                ((MasterPage)Master).SetWide(true);
#endif
                if (user.IsKierownik)
                {
                    if (!App.User.HasRight(AppUser.rPrzesuniecia) && !App.User.HasRight(AppUser.rPrzesunieciaAcc))
                        Tools.RemoveMenu(tabKierownik, "vPrzesuniecia");
                    if (!App.User.HasRight(AppUser.rWnioskiUrlopowe) || !Lic.wnioskiUrlopowe)
                        Tools.RemoveMenu(tabKierownik, "vWnioskiUrlopowe");
                    if (!App.User.HasRight(AppUser.rRozlNadg) || !Lic.rozlNadg)
                        Tools.RemoveMenu(tabKierownik, "pgRozliczanieNadgodzin");
                    if (!App.User.HasRight(AppUser.rPlanUrlopow) || !Lic.planUrlopow)
                        Tools.RemoveMenu(tabKierownik, "pgPlanUrlopow");
                    if (!App.User.HasRight(AppUser.rPodzialLudzi) || !Lic.podzialLudzi)
                        Tools.RemoveMenu(tabKierownik, "vPodzialLudzi");

                    Tools.SelectMenuFromSession(tabKierownik, active_tab);
                    PrepareDostepniKierownicy();
                    Prepare();
                    SelectTab(true);
                }
                else if (user.HasZastepstwo)    // pracownik, który zastępuje też może się zalogować do RCP - nie ma mieć dostępu do innych opcji 
                {
                    Tools.EnableMenu(tabKierownik, "pgPlanPracy", false);
                    Tools.EnableMenu(tabKierownik, "pgAkceptacjaCzasu", false);
                    Tools.EnableMenu(tabKierownik, "pgStruktura", false);                    
                    Tools.SelectMenu(tabKierownik, "pgZastepstwa");                    
                    Tools.EnableMenu(tabKierownik, "pgUstawienia", false);

                    Tools.RemoveMenu(tabKierownik, "vPrzesuniecia");
                    Tools.RemoveMenu(tabKierownik, "vWnioskiUrlopowe");
                    Tools.RemoveMenu(tabKierownik, "pgRozliczanieNadgodzin");
                    Tools.RemoveMenu(tabKierownik, "pgPlanUrlopow");
                    Tools.RemoveMenu(tabKierownik, "vPodzialLudzi");
                    



                    /*                        
                    Tools.EnableMenu(tabKierownik, "0", false);
                    Tools.EnableMenu(tabKierownik, "1", false);
                    Tools.EnableMenu(tabKierownik, "2", false);                    
                    Tools.SelectMenu(tabKierownik, "4");                    
                    Tools.EnableMenu(tabKierownik, "3", false);
                    Tools.EnableMenu(tabKierownik, "5", false);
                    Tools.EnableMenu(tabKierownik, "6", false);
                    */ 

                    /*
                    tabKierownik.Items[0].Enabled = false;
                    tabKierownik.Items[1].Enabled = false;
                    tabKierownik.Items[2].Enabled = false;
                    tabKierownik.Items[3].Selected = true;
                    tabKierownik.Items[4].Enabled = false;
                    */
                    SelectTab(true);
                }
                else
                    App.ShowNoAccess("Panel Kierownika", user);

                if (Lic.ppPrint)
                    headPrint.Visible = true;
            }
        }

        
        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Kierownik Form");
        }
         
        //--------------------------------
        private void PrepareDostepniKierownicy_new()
        {
            bool a = App.User.HasRight(AppUser.rDostepniKierownicy);
            bool b = App.User.HasRight(AppUser.rPlanPracySwoj);

            if (a || b)
            {
                divDostepniKierownicy.Visible = true;
                string kid = App.User.Id;

                string moiPrac = @"
select {0} as Id, 'Moi pracownicy' as NI, 0 as Sort ";
                string mojCzas =  b ? @"
union
select -{0} as Id, 'Mój czas pracy' as NI, 1 as Sort " : null;
                string dostPrac = a ? @"
union
select top 1 0 as Id, 'Udostępnieni pracownicy' as NI, 2 as Sort 
from DostepniKierownicy D 
where D.IdKierownika = {0} and Pracownicy = 0 
union 
select P.Id, P.Nazwisko + ' ' + P.Imie 
--+ case when D.Edycja = 1 then ' (edycja)' else ' (podgląd)' end 
as NI, 3 as Sort
from DostepniKierownicy D
left outer join Pracownicy P on P.Id = D.IdKierDostepny
where D.IdKierownika = {0} and Pracownicy = 1 " : null;

                DataSet ds = Base.getDataSet(String.Format(
                    moiPrac +
                    mojCzas +
                    dostPrac +
                    "order by Sort, NI", kid));
                
                Tools.BindData(ddlKierownicy2, ds, "NI", "Id", false, kid);
                PlanPracyAccept.Prepare(kid, DateTime.Today, true);

                if (App.User.HasRight(AppUser.rRozlNadg))
                {
                    Tools.BindData(ddlKierownicy3, ds, "NI", "Id", false, kid);
                    PlanPracyRozliczenie.Prepare(kid, DateTime.Today, true);
                }

                if (App.User.HasRight(AppUser.rPlanUrlopow))
                {
                    Tools.BindData(ddlKierownicy4, ds, "NI", "Id", false, kid);
                    cntPlanUrlopow.Prepare(kid, DateTime.Today, true);
                }
            }
        }

        //ddlKierownicy2.Items.Add(new ListItem("Moi pracownicy", kid));
        //ddlKierownicy2.Items.Add(new ListItem("Mój czas pracy", kid));
        //ddlKierownicy2.Items.Add(new ListItem("Udostępnieni pracownicy", kid));
        
        private void PrepareDostepniKierownicy()
        {
            if (App.User.HasRight(AppUser.rDostepniKierownicy))
            {
                divDostepniKierownicy.Visible = true;
                string kid = App.User.Id;

                DataSet ds = Base.getDataSet(String.Format(@"
select {0} as Id, 'Moi pracownicy' as NI, 0 as Sort 
union 
--select 0 as Id, 'Udostępnieni kierownicy' as NI, 1 as Sort 
--union 
select P.Id, P.Nazwisko + ' ' + P.Imie as NI, 2 as Sort
from DostepniKierownicy D
left outer join Pracownicy P on P.Id = D.IdKierDostepny
where D.IdKierownika = {0}
order by Sort, NI", kid));
                Tools.BindData(ddlKierownicy2, ds, "NI", "Id", false, kid);
                PlanPracyAccept.Prepare(kid, DateTime.Today, true);

                if (App.User.HasRight(AppUser.rRozlNadg))
                {
                    Tools.BindData(ddlKierownicy3, ds, "NI", "Id", false, kid);
                    PlanPracyRozliczenie.Prepare(kid, DateTime.Today, true);
                }

                if (App.User.HasRight(AppUser.rPlanUrlopow))
                {
                    Tools.BindData(ddlKierownicy4, ds, "NI", "Id", false, kid);
                    cntPlanUrlopow.Prepare(kid, DateTime.Today, true);
                }
            }
        }
        //-------------------------------
        private void SelectTab(bool setHelp)
        {
            Tools.SelectTab(tabKierownik, mvKierownik, active_tab, setHelp);
        }

        /*
        private void _SelectTab()
        {
            switch (tabKierownik.SelectedValue)
            {
                case "0":
                    Info.SetHelp(Info.HELP_KIERPLANPRACY);
                    mvKierownik.SetActiveView(pgPlanPracy);
                    break;
                case "1":
                    Info.SetHelp(Info.HELP_KIERACCPP);
                    mvKierownik.SetActiveView(pgAkceptacjaCzasu);
                    break;
                case "2":
                    Info.SetHelp(Info.HELP_KIERPRACOWNICY);
                    mvKierownik.SetActiveView(pgStruktura);
                    break;
                case "3":
                    Info.SetHelp(Info.HELP_KIERPARAMS);
                    mvKierownik.SetActiveView(pgUstawienia);
                    break;
                case "4":
                    Info.SetHelp(Info.HELP_ZASTEPSTWA);
                    mvKierownik.SetActiveView(pgZastepstwa);
                    break;
                case "5":
                    Info.SetHelp(Info.HELP_ODDELEGOWANIA);
                    mvKierownik.SetActiveView(vPrzesuniecia);
                    break;
                case "6":
                    //Info.SetHelp(Info.HELP_ODDELEGOWANIA);
                    mvKierownik.SetActiveView(vWnioskiUrlopowe);
                    break;
            }
        }
        */

        private void Prepare()
        {
            cntKierParams.Prepare(user.Id);

            PlanPracyZmiany.Prepare(user.Id, DateTime.Now, true);
            PlanPracyAccept.Prepare(user.Id, DateTime.Now, true);
            if (App.User.HasRight(AppUser.rRozlNadg))
                PlanPracyRozliczenie.Prepare(user.Id, DateTime.Now, true);
            if (App.User.HasRight(AppUser.rPlanUrlopow))
                cntPlanUrlopow.Prepare(user.Id, DateTime.Now, true);

            //cntSelectOkresStruct._Prepare(null);
            cntSelectOkresStruct.OkresId = PlanPracyZmiany.OkresRozl.OkresId;
            SelectOkresRCP.OkresId = PlanPracyZmiany.OkresRozl.OkresId;
        }

        protected void ShowCzas()
        {
            string pid = cntStruktura.SelectedPracId;
            string id = cntStruktura.SelectedRcpId;
            string strefaId, algId, algPar;
            Worktime.GetStrefaRCP2(pid, SelectOkresRCP.DateTo, out strefaId, out algId, out algPar);
            cntRcp.Prepare(pid, id, SelectOkresRCP.DateFrom, SelectOkresRCP.DateTo, strefaId);
        }
        //---------------------------
        protected void tabKierownik_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectTab(true);
            //Session[active_tab] = tabKierownik.SelectedValue;
        }

        protected void pgStruktura_Activate(object sender, EventArgs e)
        {
            cntStruktura.PrepareIfEmpty(user.Id);
            cntStruktura.InitScript();
        }

        protected void pgPrzesuniecia_Activate(object sender, EventArgs e)
        {
            cntPrzesunieciaKier1.Prepare();
        }

        protected void pgPodzialLudzi_Activate(object sender, EventArgs e)
        {
            cntPodzialLudziKier.Prepare();
        }

        protected void vWnioskiUrlopowe_Activate(object sender, EventArgs e)
        {
            paWnioskiUrlopowe.Prepare();
        }

        protected void pgAkceptacjaCzasu_Activate(object sender, EventArgs e)
        {
            PlanPracyAccept.InitScripts();
        }

        protected void pgRozliczanieNadgodzin_Activate(object sender, EventArgs e)
        {
            PlanPracyRozliczenie._InitScripts();
        }

        protected void pgPlanUrlopow_Activate(object sender, EventArgs e)
        {
        }



        //-----
        protected void PlanPracyZmiany_DataSaved(object sender, EventArgs e)
        {
            PlanPracyAccept.DataBind();
        }

        protected void ddlBreak_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Session["acc_break"] = ddlBreak.SelectedValue;
        }

        protected void ddlMargin_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Session["acc_margin"] = ddlMargin.SelectedValue;
        }

        protected void cntStruktura_SelectedChanged(object sender, EventArgs e)
        {
            lbPracName.Text = cntStruktura.SelectedNI;
            ShowCzas();
        }

        protected void btRefresh1_Click(object sender, EventArgs e)
        {
            ShowCzas();
        }

        protected void cntKierParams_Changed(object sender, EventArgs e)
        {
            PlanPracyAccept.DataBind();
        }

        //-----
        protected void ddlKierownicy2_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlanPracyAccept.Prepare(ddlKierownicy2.SelectedValue);
        }

        protected void ddlKierownicy3_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlanPracyRozliczenie.Prepare(ddlKierownicy3.SelectedValue);
        }

        protected void ddlKierownicy4_SelectedIndexChanged(object sender, EventArgs e)
        {
            cntPlanUrlopow.Prepare(ddlKierownicy4.SelectedValue);
        }
        //----------------------------------------
    }
}
