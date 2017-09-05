using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Portal
{
    public partial class ContentForm : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
            //this.MasterPageFile = "~/Portal/Ubezpieczenia.master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string grupa = Tools.GetStr(HttpContext.Current.Request.QueryString["p"]);
                if (String.IsNullOrEmpty(grupa))
                    App.Redirect(App.DefaultForm);
                else
                {
                    cntArticles.Grupa = grupa;
                    //cntArticles.List.DataBind();
                }
            }
        }
    }
}
