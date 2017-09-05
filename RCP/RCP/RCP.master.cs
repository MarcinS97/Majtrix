using HRRcp.App_Code;
using HRRcp.RCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HRRcp.RCP.Adm;

namespace HRRcp
{
    public partial class MasterPage3RCP : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                App.SetApp(App.Rcp);
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AppUser user = AppUser.CreateOrGetSession();
                ShowLoggedUser(user);
                lbAppTitle.Text = Tools.GetAppName();
                ddlLogin.Visible = user.IsSuperuser;
                hidUserRights.Value = db.PrepareRightsForCheckRightsExpression(user); //user.Rights.PadRight(1000, '0') + ((user.IsAdmin) ? "1" : "0") + ((user.IsKierownik) ? "1" : "0") + ((user.IsRaporty) ? "1" : "0");
                //hidAdditionalUserRights.Value = (((user.IsAdmin) ? 1 : 0) + ((user.IsKierownik) ? 2 : 0) + ((user.IsRaporty) ? 4 : 0)).ToString();
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

        //----------------
        enum mType { Other, Option, Divider };

        private mType getCntType(Control cnt)
        {
            if (cnt is HtmlGenericControl)
            {
                HtmlGenericControl tag = cnt as HtmlGenericControl;
                if (tag.TagName.ToLower() == "li" )
                {
                    string role = tag.Attributes["role"];
                    if (role != null && role.ToLower() == "separator")
                        return mType.Divider;
                    else
                        return mType.Option;
                }
            }
            return mType.Other;            
        }

        private void CheckMenu(HtmlGenericControl mnTopOption)
        {
            // wycięcie zduplikowanych dividerów
            // sprawdzenie czy są opcje, jak nie to usunięcie menu górnego           
            bool div = false;
            int cnt = 0;
            Control lastdiv = null;     // pierwszy z ostatnich
            foreach (Control m in mnTopOption.Controls)
            {
                string id = m.ID;
                mType typ = getCntType(m);
                switch (typ)
                {
                    case mType.Option:
                        if (m.Visible)
                        {
                            div = false;
                            lastdiv = null;
                            cnt++;
                        }
                        break;
                    case mType.Divider:
                        if (m.Visible)
                        {
                            if (div)
                                m.Visible = false;
                            else
                            {
                                div = true;
                                lastdiv = m;
                            }
                            cnt++;
                        }
                        break;
                }
            }
            if (lastdiv != null)
                lastdiv.Visible = false;
            if (cnt == 0)
                mnTopOption.Visible = false;
        }

        private void SetMenuRights()
        {
#if false
            //----- Start -----
            mnSTART.Visible             = Start.HasFormAccess;
            //----- Pracownicy -----
            opPLANPRACY.Visible         = Harmonogram.HasFormAccess;
            opPLANPRACYACC.Visible      = HarmonogramAkceptacja.HasFormAccess;  
            opCZASPRACYACC.Visible      = AkceptacjaCzasuPracy.HasFormAccess;
            opROZLICZNADG.Visible       = RozliczanieNadgodzin.HasFormAccess;
            opWNIOSKINADG.Visible       = WnioskiNadgodziny.HasFormAccess;
            opPRZYPISANIA.Visible       = Przypisania.HasFormAccess;            
            opPRACOWNICY.Visible        = HRRcp.RCP.Pracownicy.HasFormAccess;
            opWNIOSKIURLOPOWE.Visible   = WnioskiUrlopowe.HasFormAccess;
            opZASTEPSTWA.Visible        = Zastepstwa.HasFormAccess;
            opUSTAWIENIA.Visible        = false;// UstawieniaForm.HasFormAccess;             
            CheckMenu(mnPRACOWNICY);
            //----- Moduły -----
            opPODZIALLUDZI.Visible      = PodzialLudziAdm.HasFormAccess;
            poPLANURLOPOW.Visible       = HRRcp.RCP.PlanUrlopow.HasFormAccess;
            opSZKOLENIABHP.Visible      = HRRcp.SzkoleniaBHP.Rejestr.HasFormAccess;
            opBADANIA.Visible           = HRRcp.BadaniaWstepne.Rejestr.HasFormAccess;
            opMATRYCASZKOLEN.Visible    = Lic.MatrycaSzkolen && !String.IsNullOrEmpty(App.wwwMS);
            opSCORECARDS.Visible        = Lic.ScoreCards && !String.IsNullOrEmpty(App.wwwSCORECARDS);
            opPORTAL.Visible            = Lic.Portal && !String.IsNullOrEmpty(App.wwwPORTAL);
            CheckMenu(mmMODULY);
            //----- Raporty -----
            mnRAPORTY.Visible           = RaportyForm.HasFormAccess;
            //----- Administracja -----
            mnADMINISTRACJA.Visible     = App.User.IsAdmin;
            //opADMPRACOWNICY.Visible = true;
            //opADMPRZYPISANIA.Visible = true;
            //opADMWNIOSKIURLOPOWE.Visible = true;
            //opADMZASTEPSTWA.Visible = true;
            opADMOKRESYROZL.Visible     = OkresyRozliczeniowe.HasFormAccess;
            opADMSTREFYRCP.Visible      = StrefyRCP.HasFormAccess;
            opADMZMIANY.Visible         = Zmiany.HasFormAccess;
            opADMSLOWNIKI.Visible       = Slowniki.HasFormAccess;
            opRAPORTYADM.Visible        = AdminRaporty.HasFormAccess;
            opADMUSTAWIENIA.Visible     = UstawieniaForm.HasFormAccess;
            opADMLOG.Visible            = LogForm.HasFormAccess;
            CheckMenu(mnADMINISTRACJA);
#endif
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
                lbUser.Text = Tools.PrepareName(user.OriginalImieNazwisko);
                if (user.IsOriginalUser)
                {
                    if (Master is MasterPage3)
                        ((MasterPage3)Master).ShowZastInfo(false, user);
                }
                else
                {
                    if (Master is MasterPage3)
                        ((MasterPage3)Master).ShowZastInfo(true, user);
                }
            }
            else
                if (Master is MasterPage3)
                    ((MasterPage3)Master).ShowZastInfo(false, user);
            
            btLogout.Visible = /*user.IsKiosk &&*/ user.IsPassLogged_VC;
        }

        protected void lnkRedirect_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            string cmd = (lb.CommandArgument);
            if (!String.IsNullOrEmpty(cmd))
            {
                string url = "";
                if (Tools.IsUrl(cmd, out url))
                {
                    //App.Redirect(url);
                }
                else if (cmd.Contains("REPORT"))
                {
                    string repId = cmd.Split(':')[1];
                    url = GetUrl(repId);
                }

                App.Redirect(url);
            }
        }

        string GetUrl(string id)
        {
            string p = Tools.SetLineParams(3, id, null, null, null, null, null);   // jak za krótki string to dołoży #zera na końcu - dlatego pusty parametr, zeby nie trzeba było ich odcinać
            string e = Report.EncryptQueryString(p, Grid.key, Grid.salt);
            return ResolveUrl(String.Format("/Redirect.aspx?p={0}&f={1}", e, "RaportF.aspx"));
        }

        protected void btLogout_Click(object sender, EventArgs e)
        {
            /*if (!App.User.IsOriginalUser)
                //App.LoginAsUser(null);   // to robi redirect, w sumie moze moze tak dzialac - wyloguje z zastepowanego?
                App.User.LoginAsUser(null);*/
            App.User.DoPassLogout();
            App.User.Reload(false);
            App.Redirect("Default.aspx");
        }

        //--------------
    }
}