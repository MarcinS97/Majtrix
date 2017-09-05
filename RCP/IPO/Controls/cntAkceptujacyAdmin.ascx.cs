using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;
using HRRcp.IPO.App_Code;


namespace HRRcp.IPO.Controls
{
    public partial class cntAkceptujacyAdmin : System.Web.UI.UserControl
    {

        private const string active_tab = "pgAdministratorzy";
        AppUser user;

        protected void Page_PreInit(Object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                Tools.CheckSessionExpired();
            }
            refresh();
            Tools.SelectTab(IPOTabAdmin, mvAdministracja, active_tab, false);
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {

            base.OnPreRender(e);
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("IPO Administracja Form");
        }

        protected void IPOtabAdmin_MenuItemClick(object sender, MenuEventArgs e)
        {

            SelectTab(e.Item.Text.ToString());
        }

        private void SelectTab(String zakladka)
        {
            Session[active_tab] = IPOTabAdmin.SelectedValue;
            Tools.SelectTab(IPOTabAdmin, mvAdministracja, active_tab, false);

        }
        public void refresh()
        {
            IPOTabAdmin.Items.Clear();
            fillMenu();
        }
        private void fillMenu()
        {
            const string item = "<div class=\"tabCaption\"><div class=\"tabLeft\"><div class=\"tabRight\">{0}</div></div></div>";
            DataSet ds = IPO_db.getDataSet("select distinct(nazwa), Symbol, Id from IPO_ROLE where IPO_ROLE.Aktywny=1 and IdParent=2");
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                     MenuItem submenuitem1 = new MenuItem(String.Format(item, dr[1].ToString()), "pg" + dr[2].ToString());
                    IPOTabAdmin.Items.Add(submenuitem1);
                    if (i ==0)
                    {
                        submenuitem1.Selected = true;
                    }
                    i++;
                
                View subView = new View();
                subView.ID = "pg" + dr[2].ToString();

                UpdatePanel subUpdatePanel = new UpdatePanel();

                subUpdatePanel.ID = "UpdatePanel" + dr[2].ToString();
                
                subUpdatePanel.UpdateMode = UpdatePanelUpdateMode.Conditional;
                
                cntUprawnienia uprawnienia = (cntUprawnienia)LoadControl("~/IPO/Controls/cntUprawnienia.ascx");
               
                uprawnienia.ID = dr[2].ToString();
                uprawnienia.Rola = dr[0].ToString();
                subUpdatePanel.ContentTemplateContainer.Controls.Add(uprawnienia);
                subView.Controls.Add(subUpdatePanel);
                mvAdministracja.Controls.Add(subView);
               //uprawnienia.
            }
        }

    }
}