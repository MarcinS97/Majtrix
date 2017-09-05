using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Portal
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SetNoCache();
                string search = Request.QueryString["s"];
                TextBox tb = Master.FindControl("tbSearch") as TextBox;
                if (tb != null)
                    tb.Text = search;
                int id = Log.Info(Log.SEARCH, search, null);
                cntSearch.Search(search, id);
                //hidSearch.Value = search;
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Wyszukaj");
        }
    }
}
