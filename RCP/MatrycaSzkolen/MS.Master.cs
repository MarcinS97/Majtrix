using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.IO;
using System.Data;

namespace HRRcp.MatrycaSzkolen
{
    public partial class MS : RcpMasterPage
    {
        protected void OnPageInit(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void OnPageError(object sender, System.EventArgs e)
        {
            string page = Path.GetFileName(Request.Path);   // powinna być wzięta nazwa formatki ...
            AppError.Show(page);
        }
        //-----
        protected void Page_Init(object sender, EventArgs e)
        {
            //-------------------------------
            if (!(Parent is ErrorForm))
            {
                Page.Init += new EventHandler(OnPageInit);
                Page.Error += new EventHandler(OnPageError);
            }
            //-------------------------------
            if (!IsPostBack)
            {
                Tools.InitSessionExpired();
                App.SetApp(App.MS);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();
            if(!IsPostBack)
            {
                AppUser user = AppUser.CreateOrGetSession();
                if (!String.IsNullOrEmpty(user.Id))
                    lbUser.Text = Tools.PrepareName(user.ImieNazwisko);
                ShowLoggedUser(user);
                SetMenuRights();


                if(IsEmployee(user))
                {
                    if (Parent is AnkietaPP)
                    {
                        topMenu.Visible = false;
                    }
                    else
                    {
                        DataRow dr = db.Select.Row(@"select top 1 c.*, a.*, a.Id AnkietaId from msAnkiety a left join Certyfikaty c on c.Id = a.IdCertyfikatu 
                                                    where c.IdPracownika = {0} order by a.Status, c.DataUtworzenia desc", user.Id);
                        if(dr == null)
                        {
                            App.ShowNoAccess(null, user);
                        }
                        else
                        {
                            App.Redirect(String.Format("MatrycaSzkolen/AnkietaPP.aspx?p={0}", dr["AnkietaId"]));
                        }
                    }
                }

                if (Parent is RaportF || Parent is RaportPDF)
                {
                    paReportFooter.Visible = true;
                    lbPrintTime.Text = Base.DateTimeToStr(DateTime.Now) + " " + user.ImieNazwisko;
                    lbPrintVersion.Text = Tools.GetAppVersion();
                }

                lbProgram.Text = Tools.GetAppName();
                lbVersion.Text = Tools.GetAppVersion();
                string kdr = "© KDR Solutions Sp. z o.o. '" + DateTime.Today.Year.ToString();
                string www = "http://www.kdrsolutions.pl";
                lbCopyright.Text = kdr;
                aCopyright.Text = kdr;
                aCopyright.NavigateUrl = www;
                aCopyright.ToolTip = www;

                lbAppTitle.Text = Tools.GetAppName();
                ddlLogin.Visible = user.IsSuperuser;
                hidUserRights.Value = db.PrepareRightsForCheckRightsExpression(user); //user.Rights.PadRight(1000, '0') + ((user.IsAdmin) ? "1" : "0") + ((user.IsKierownik) ? "1" : "0") + ((user.IsRaporty) ? "1" : "0");
                //hidAdditionalUserRights.Value = (((user.IsAdmin) ? 1 : 0) + ((user.IsKierownik) ? 2 : 0) + ((user.IsRaporty) ? 4 : 0)).ToString();
                /*imgLogo.ImageUrl = "~/styles/user/logo.png";*/
            }
            PrepareMainMenu();
        }

        private void PrepareMainMenu()
        {
            if (!IsPostBack)
            {
                //SetMenuRights();
                //MasterPage3.RemoveDividers(topMenu);
            }
            MasterPage3.PrepareMainMenuOnClick(topMenu);
        }

        bool IsEmployee(AppUser user)
        {
            return !user.HasRight(AppUser.rMSBadaniaPodglad) && !user.HasRight(AppUser.rMSBHP) && !user.HasRight(AppUser.rMSHR) && !user.HasRight(AppUser.rMSMeister) && !user.HasRight(AppUser.rMSTrener) && !user.IsMSAdmin && !user.IsKierownik;
        }

        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {
            if (!IsPostBack)
            {
            }
            base.OnPreRender(e);
        }   
        
        //-------------------------
        protected void ddlLogin_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            string sel = ddl.SelectedValue;
            switch (sel)
            {
                case "0":
                case "-1":
                    hidLoginTyp.Value = sel;
                    ((DropDownList)sender).DataBind();
                    break;
                default:
                    string[] p = Tools.GetLineParams(sel);
                    App.User.LoginAsUserId2(p[0]);
                    App.Redirect(App.DefaultForm);
                    break;
            }
            /*                      
            //App.User._LoginAsUserId(ddlLogin.SelectedValue);

            string[] p = Tools.GetLineParams(ddlLogin.SelectedValue);
            App.User.LoginAsUserId2(p[0]);

            //App.User.CheckPassLoginTest(App.User.NR_EW);

            //lbUser.Text = Tools.PrepareName(App.User.ImieNazwisko);

            //App.KwitekKadryId = App.User.NR_EW;
            //App.KwitekPracId = App.User.Id;
            //AppUser.x_IdKarty = App.User.NR_EW;

            //App.Redirect(App.PortalStartForm);
            //App.Redirect("IPO/IPO.aspx");

            App.Redirect(App.DefaultForm);
            */
            // żeby wnioski działały poprawnie
            //ViewState["tabsupd"] = null;

            //Session["mnScWn2"] = null;    //T: musi być wywołane przed Redirect powyżej ...
            //Session["mnScWn"] = null;
        }

        protected void ddlLogin_DataBound(object sender, EventArgs e)
        {
            foreach (ListItem item in ((DropDownList)sender).Items)
            {
                string id, css;
                Tools.GetLineParams(item.Value, out id, out css);
                if (!String.IsNullOrEmpty(css))
                    item.Attributes["class"] = css;
            }
            //Tools.SelectItem(ddlLogin, App.User.Id);  // zostaje zmien usera
        }

        protected void lbUser_Click(object sender, EventArgs e)
        {
            App.User.Reload(false);
            App.Redirect(App.DefaultForm);
        }

        private void ShowLoggedUser(AppUser user)
        {
            if (!String.IsNullOrEmpty(user.Id))
            {
                if (user.IsOriginalUser)
                {
                    divZastInfo.Visible = false;
                    lbUser.Text = Tools.PrepareName(user.ImieNazwisko);
                    lbZast.Text = null;
                }
                else
                {
                    divZastInfo.Visible = true;
                    /*
                    lbUser.Text = String.Format("{0}<span class=\"text\">, zastępujesz: </span>{1}",
                        Tools.PrepareName(user.OriginalImieNazwisko),
                        Tools.PrepareName(user.ImieNazwisko));
                    */
                    lbUser.Text = Tools.PrepareName(user.OriginalImieNazwisko);
                    lbZast.Text = user.ImieNazwisko;
                }
            }
            else
                divZastInfo.Visible = false;
        }

        //--------------
        protected void lbtEndZastepstwo_Click(object sender, EventArgs e)
        {
            App.LoginAsUser(null);
        }

        void SetMenuRights()
        {
            //bool pAdm = App.User.HasRight(AppUser.rMSPracownicyAdm);
            //bool pProd = App.User.HasRight(AppUser.rMSPracownicyProd);
            bool roleAdmin = App.User.IsMSAdmin;
            //bool roleMistrz = App.User.HasRight(AppUser.rMSMeister);
            //bool roleTrener = App.User.HasRight(AppUser.rMSTrener);
            //bool roleBHP = App.User.HasRight(AppUser.rMSBHP);
            //bool roleHR = App.User.HasRight(AppUser.rMSHR);

            //bool rSZKACC = App.User.HasRight(AppUser.rMSCertyfikatyAcc);
            //bool rOCENYACC = App.User.HasRight(AppUser.rMSKorektyAcc);
            //bool rANKIETYACC = App.User.HasRight(AppUser.rMSAnkietyAcc);

            //opPRACADM.Visible = Szkolenia.HasFormAccess;  //roleAdmin || pAdm;
            //opPRACPROD.Visible = SzkoleniaP.HasFormAccess; //roleAdmin || pProd;

            //opANKIETY.Visible = Ewaluacja.HasFormAccess;//roleAdmin || roleMistrz || roleTrener || roleHR;
            //opACC.Visible = Akceptacje.HasFormAccess;//roleAdmin || rSZKACC || rOCENYACC || rANKIETYACC;

            //opBADANIA.Visible = opBADANIASEP.Visible = Badania.HasFormAccess;//roleAdmin || roleHR;
            //opSZKOLENIABHP.Visible = opSZKOLENIABHPSEP.Visible = SzkoleniaBHP.HasFormAccess;

            //opODD.Visible = opZASTSEP.Visible = Oddelegowania.HasFormAccess;//roleAdmin || App.User.HasRight(AppUser.rPrzesuniecia) || App.User.HasRight(AppUser.rPrzesunieciaAcc) || App.User.HasRight(AppUser.rPrzesunieciaAdm);

            //mmREP.Visible = roleAdmin || App.User.IsRaporty;
            //mmADM.Visible = roleAdmin;

            //mmPRAC.Visible = user.IsKierownik;
        }


        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("MS Master Page");
        }

        protected void lnkRedirect_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            if (!String.IsNullOrEmpty(lb.CommandArgument))
                App.Redirect(lb.CommandArgument);
        }
    }
}
