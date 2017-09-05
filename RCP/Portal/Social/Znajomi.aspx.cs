using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Social
{
    public partial class Znajomi : System.Web.UI.Page
    {

        public static class Tab
        {
            public const string Search = "1";
            public const string My = "2";
            public const string Inv = "3";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cntSqlTabs_SelectTab(object sender, EventArgs e)
        {
            String selectedTab = cntSqlTabs.SelectedValue;
            View v = Tools.SetViewById(mvViews, selectedTab);

            switch (selectedTab)
            {
                case Tab.Search:
                    cntFriendsListSearch.Prepare();
                    break;
                case Tab.My:
                    cntFriendsListMy.Prepare();
                    break;
                case Tab.Inv:
                    cntFriendsListInvAcc.Prepare();
                    cntFriendsListInvSent.Prepare();
                    break;
            }



        }
    }
}