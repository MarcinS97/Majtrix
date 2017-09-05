using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    //public partial class MasterPage : System.Web.UI.MasterPage
    public partial class KwitekAdminMasterPage : RcpMasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.InitSessionExpired();
                App.SetApp(App.Kwitek);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AppUser user = AppUser.CreateOrGetSession();

                //----- zabezpieczenie przed zastępstwem -----
                if (!user.IsOriginalUser)
                {
                    App.LoginAsUser(null);
                    user.DoPassLogout();
                }
                //----- logowanie -----
                if ((Parent is AdminKwitekForm || Parent is AdminUrlopForm) && App.User.IsAdmin && App.User.HasRight(AppUser.rKwitekAdm))
                {
                    if (!String.IsNullOrEmpty(user.Id))
                        lbUser.Text = Tools.PrepareName(user.ImieNazwisko);

                    if (user.IsAdmin && user.Login.ToLower() == "wojciowt")
                    {
                        lbtTest.Visible = true;
                        lbtTest2.Visible = true;
                    }

                    lbChangePass.Visible = true;

                    MainMenu.Visible = true;
                    string s;
                    if (Parent is AdminKwitekForm || Parent is KwitekForm) s = "1";
                    else if (Parent is AdminUrlopForm || Parent is UrlopForm) s = "2";
                    else s = null;
                    Tools._SelectMenu(MainMenu, s, "mainMenu");

                    DropDownList1.Visible = true;

                    bool ak = Parent is AdminKwitekForm;
                    tdKwitekDo.Visible = ak;
                    if (ak)
                    {
                        Ustawienia settings = Ustawienia.CreateOrGetSession();
                        deKwitekDo.Date = settings.KwitekDo;
                        Tools.MakeConfirmButton(btSetKwitekDo, "Potwierdź zmianę daty udostępnienia list.");
                    }
                    Info.SetHelp(Info.PANELPRAC);
                }
                else
                {
                    HttpContext.Current.Response.Redirect(App.KwitekForm);
                }
            }
        }

        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {
            if (!IsPostBack)
            {
                lbProgram.Text = "Panel Pracownika";
                FooterControl1.AppName = "Panel Pracownika";
                FooterControl1.AppVer = "1.2.0.0";
                FooterControl1.RegulaminButton.Visible = false;
            }
            base.OnPreRender(e);
        }
        
        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Panel Pracownika");
        }
        //------------------------------
        private int mnIndex = 0;

        protected string GetMenuIndex()
        {
            mnIndex++;
            int ret = 5 - MainMenu.Items.Count + mnIndex;
            return ret.ToString();
        }
        //--------------------------------------------------------------------------------
        private void ChangePrac()
        {
            App.KwitekKadryId = DropDownList1.SelectedValue;
            App.KwitekPracId = db.getScalar("select Id from Pracownicy where KadryId = " + DropDownList1.SelectedValue);
            if (Parent is AdminKwitekForm) ((AdminKwitekForm)Parent).Prepare();
            if (Parent is AdminUrlopForm) ((AdminUrlopForm)Parent).Prepare();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangePrac();
        }

        protected void DropDownList1_DataBound(object sender, EventArgs e)
        {
            Tools.SelectItem((DropDownList)sender, App.KwitekKadryId);
            ChangePrac();
        }

        //------------------------------

        public override void Redirect(string page)
        {
            if (String.IsNullOrEmpty(page))
                page = MainMenu.Items[0].Value;
            switch (page)
            {
                case "1": 
                    Response.Redirect(App.KwitekAdminForm);
                    break;
                case "2": 
                    Response.Redirect(App.UrlopAdminForm);
                    break;
                case "3":
                    /*
                    Log.Info(Log.PRACLOGIN, "Wylogowanie pracownika", String.Format("ID: {0}, {1}", App.User.Id, App.User.NazwiskoImie), Log.OK);
                    App.User.DoPassLogout();
                    App.User.Reload(true);
                    */
                    Response.Redirect(App.KwitekForm);
                    break;
            }
        }

        protected void MainMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            Redirect(MainMenu.SelectedValue);
        }

        protected void btHelp_Click(object sender, EventArgs e)
        {
            if (cntHelp.Visible)
                cntHelp.Show(false, true);
            else
                cntHelp.Show(true, false);
        }

        protected void lbtEndZastepstwo_Click(object sender, EventArgs e)
        {
            App.LoginAsUser(null);
        }

        //---------------------------------
        //---------------------------------
        public void SetWide(bool wide)
        {
            divContent.Attributes["class"] = wide ? "center_wide" : "center";
        }

        public override void SetWideJs(bool wide)
        {
            Tools.ExecOnStart("setClassById", "'" + divContent.ClientID + "'," + (wide ? "'center_wide'" : "'center'"));
        }

        public void SetWideJs(int wide)
        {
            string css;
            switch (wide)
            {
                default:
                case 0:
                    css = "center";
                    break;
                case 1:
                    css = "center_wide";
                    break;
                case 2:
                    css = "center_wide2";
                    break;
            }
            Tools.ExecOnStart("setClassById", String.Format("'{0}','{1}'", divContent.ClientID, css));
        }

        public void SetBodyScrollBar(bool visible)
        {
            if (visible)
                //Tools.RemoveClass(htmlbody, "noscroll");
                Tools.ExecOnStart2("bodyscroll", "$('body').removeClass('noscroll');");   // uruchamia jak jest, bez -2 dokłada ();
            else
                //Tools.AddClass(htmlbody, "noscroll");
                Tools.ExecOnStart2("bodyscroll", "$('body').addClass('noscroll');");
        }

        protected void btSetKwitekDo_Click(object sender, EventArgs e)
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            if (settings.UpdateKwitekDo(deKwitekDo.DateStr))
                if (Parent is AdminKwitekForm) ((AdminKwitekForm)Parent).UpdateListy();
        }
    }
}
