using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Portal
{
    public partial class OgloszeniaForm : System.Web.UI.Page
    {
        const string FormName = "Portal - Ogłoszenia";

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
            if (!IsPostBack)
                if (App.User.HasRight(AppUser.rPortalTmp))
                {
                    Tools.SetNoCache();
                    Tools.EnableUpload();
                    Prepare(null);
                }
                else
                    App.ShowNoAccess(FormName, App.User);
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(FormName);
        }

        public void Prepare(string cmd)
        {
            ((PortalMasterPage3)App.Master).SearchButton.OnClientClick = String.Format("return checkSearchOgloszenia(this,'{0}');", btSearch.ClientID);
        }

        public void Search(string s)
        {
            cntOgloszeniaBoard.Search(s);
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            Search(((PortalMasterPage3)App.Master).SearchTextBox.Text);
        }
        //---------------------
    }
}

