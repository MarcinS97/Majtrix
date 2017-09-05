using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls.Ubezpieczenia.Majatkowe
{
    public partial class cntWariantyAdm : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvWarianty, Tools.ListViewMode.Bootstrap);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                CheckSelection();
            }
        }

        private void CheckSelection()
        {
            if(String.IsNullOrEmpty(ddlRodzaje.SelectedValue))
            {
                notSelectedText.Visible = true;
                lvWarianty.Visible = false;
            }
            else
            {
                notSelectedText.Visible = false;
                lvWarianty.Visible = true;
            }
        }

        protected void ddlRodzaje_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSelection();
        }
    }
}