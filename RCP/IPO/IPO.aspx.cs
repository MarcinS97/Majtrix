using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;
using HRRcp.IPO.Controls;
using HRRcp.IPO.App_Code;

namespace HRRcp.IPO
{
    public partial class IPO : System.Web.UI.Page
    {
        private const string active_tab = "ipo_active_tab";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie
        private Dictionary<string, Menu> menuDict = new Dictionary<string, Menu>(); // id roli, menu
        AppUser user;
        
        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
            if (menuDict.Count == 0)
            {
                menuDict.Add("1", zamawiajacyMenu);
                menuDict.Add("2", akceptujacyMenu);
                menuDict.Add("3", kupiecMenu);
                menuDict.Add("4", notyfikowanyMenu);
                menuDict.Add("5", administratorMenu);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            user = AppUser.CreateOrGetSession();
            
            DataSet roleDataSet = IPO_db.getDataSet(@"SELECT DISTINCT 
	                                    CASE 
		                                    WHEN IPO_Role.IdParent IS NULL THEN IPO_Role.Id
		                                    WHEN IPO_Role.IdParent IS NOT NULL THEN IPO_Role.IdParent
	                                    END AS Id,
	                                    CASE 
		                                    WHEN IPO_Role.IdParent IS NULL THEN IPO_Role.Nazwa
		                                    WHEN IPO_Role.IdParent IS NOT NULL THEN 'Akceptujący'
	                                    END AS Nazwa
                                    FROM IPO_ccPrawa 
                                    JOIN IPO_Role ON IPO_ccPrawa.RolaId = IPO_Role.Id
                                    WHERE IPO_ccPrawa.UserId = "+user.Id+" ORDER BY Id");
            bool zamawiajacyVisible = false,
                 akceptujacyVisible = false,
                 kupiecVisible = false,
                 notyfikowanyVisible = false,
                 administratorVisible = false;
            foreach (DataRow dr in roleDataSet.Tables[0].Rows)
            {
                string s = dr.ItemArray[0].ToString();
                if ("1".Equals(s))
                {
                    zamawiajacyVisible = true;
                }
                else if ("2".Equals(s))
                {
                    akceptujacyVisible = true;
                }
                else if ("3".Equals(s))
                {
                    kupiecVisible = true;
                }
                else if ("4".Equals(s))
                {
                    notyfikowanyVisible = true;
                }
                else if ("5".Equals(s))
                {
                    administratorVisible = true;
                }
            }
            Tools.SetControlVisible(UpdatePanel1, "zamawiajacyButton", zamawiajacyVisible);
            Tools.SetControlVisible(UpdatePanel1, "akceptujacyButton", akceptujacyVisible);
            Tools.SetControlVisible(UpdatePanel1, "kupiecButton", kupiecVisible);
            Tools.SetControlVisible(UpdatePanel1, "notyfikowanyButton", notyfikowanyVisible);
            Tools.SetControlVisible(UpdatePanel1, "administratorButton", administratorVisible);

            if (zamawiajacyVisible || akceptujacyVisible || kupiecVisible || notyfikowanyVisible || administratorVisible)
            {
                if ("0".Equals(selectedRole.Value))
                {
                    selectedRole.Value = roleDataSet.Tables[0].Rows[0].ItemArray[0].ToString();
                    if ("1".Equals(selectedRole.Value))
                    {
                        Tools.AddClass(zamawiajacyButton, "selectedRole");
                    }
                    else if ("2".Equals(selectedRole.Value))
                    {
                        Tools.AddClass(akceptujacyButton, "selectedRole");
                    }
                    else if ("3".Equals(selectedRole.Value))
                    {
                        Tools.AddClass(kupiecButton, "selectedRole");
                    }
                    else if ("4".Equals(selectedRole.Value))
                    {
                        Tools.AddClass(notyfikowanyButton, "selectedRole");
                    }
                    else if ("5".Equals(selectedRole.Value))
                    {
                        Tools.AddClass(administratorButton, "selectedRole");
                    }
                    SelectTab();
                }
            }
            else
            {
                AppError.Show("Użytkownik nie ma przypisanej żadnej roli w iPO");
            }
            //rolaIPODataSource.SelectParameters["UserId"].DefaultValue = user.Id;
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Wystąpił nieoczekiwany błąd w iPO");
        }

        protected void tabAdmin_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectTab();
        }

        protected void zamawiajacy_OnClick(object sender, EventArgs e)
        {
            Tools.AddClass(zamawiajacyButton, "selectedRole");
            Tools.RemoveClass(akceptujacyButton, "selectedRole");
            Tools.RemoveClass(kupiecButton, "selectedRole");
            Tools.RemoveClass(notyfikowanyButton, "selectedRole");
            Tools.RemoveClass(administratorButton, "selectedRole");
            selectedRole.Value = "1";
            SelectTab();
        }
        protected void akceptujacy_OnClick(object sender, EventArgs e)
        {
            Tools.RemoveClass(zamawiajacyButton, "selectedRole");
            Tools.AddClass(akceptujacyButton, "selectedRole");
            Tools.RemoveClass(kupiecButton, "selectedRole");
            Tools.RemoveClass(notyfikowanyButton, "selectedRole");
            Tools.RemoveClass(administratorButton, "selectedRole");
            selectedRole.Value = "2";
            SelectTab();
        }
        protected void kupiec_OnClick(object sender, EventArgs e)
        {
            Tools.RemoveClass(zamawiajacyButton, "selectedRole");
            Tools.RemoveClass(akceptujacyButton, "selectedRole");
            Tools.AddClass(kupiecButton, "selectedRole");
            Tools.RemoveClass(notyfikowanyButton, "selectedRole");
            Tools.RemoveClass(administratorButton, "selectedRole");
            selectedRole.Value = "3";
            SelectTab();
        }
        protected void notyfikowany_OnClick(object sender, EventArgs e)
        {
            Tools.RemoveClass(zamawiajacyButton, "selectedRole");
            Tools.RemoveClass(akceptujacyButton, "selectedRole");
            Tools.RemoveClass(kupiecButton, "selectedRole");
            Tools.AddClass(notyfikowanyButton, "selectedRole");
            Tools.RemoveClass(administratorButton, "selectedRole");
            selectedRole.Value = "4";
            SelectTab();
        }
        protected void administrator_OnClick(object sender, EventArgs e)
        {
            Tools.RemoveClass(zamawiajacyButton, "selectedRole");
            Tools.RemoveClass(akceptujacyButton, "selectedRole");
            Tools.RemoveClass(kupiecButton, "selectedRole");
            Tools.RemoveClass(notyfikowanyButton, "selectedRole");
            Tools.AddClass(administratorButton, "selectedRole");
            selectedRole.Value = "5";
            SelectTab();
        }


        private void SelectTab()
        {
            foreach (string s in menuDict.Keys)
                menuDict[s].Visible = false;

            if (menuDict.ContainsKey(selectedRole.Value))
            {
                zakladkiMultiView.Visible = true;
                Menu m = menuDict[selectedRole.Value];
                m.Visible = true;

                Session[active_tab] = m.SelectedValue;
                Tools.SelectTab(m, zakladkiMultiView, active_tab, false);
            }
            else
            {
                zakladkiMultiView.Visible = false;
            }
        }

        protected void rolaIPODropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SelectTab();
        }

        protected void rolaIPODropDownList_OnDataBound(object sender, EventArgs e)
        {
            SelectTab();
        }

        protected void TabOnActivate(object sender, EventArgs e)
        {
            View v = sender as View;
            foreach (Control c in v.Controls)
            {
                Type t = c.GetType();
                if ("HRRcp.IPO.Controls.cntZamowienia".Equals(t.BaseType.FullName))
                {
                    cntZamowienia z = c as cntZamowienia;
                    z.refresh();
                }
            }

        }
    }
}
