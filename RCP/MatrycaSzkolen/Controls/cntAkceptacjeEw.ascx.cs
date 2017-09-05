using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Controls
{
    public partial class cntAkceptacjeEw : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void bntAccept_Click(object sender, EventArgs e)
        {
            Button btn = (sender as Button);
            string id = btn.CommandArgument;
            if(!String.IsNullOrEmpty(id))
            {
                btnAcceptConfirm.CommandArgument = id;
                Tools.ShowConfirm("Czy na pewno chcesz zaakceptować?", btnAcceptConfirm);
            }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            LinkButton btn = (sender as LinkButton);
            string split = btn.CommandArgument;

            string id = split.Split(';')[0];
            string typ = split.Split(';')[1];

            if(typ == "0")
                App.Redirect(String.Format("MatrycaSzkolen/AnkietaP.aspx?p={0}", id));
            else if(typ == "1" || typ == "2")
                App.Redirect(String.Format("MatrycaSzkolen/AnkietaK.aspx?p={0}", id));
            else
            {

            }

        
        }

        protected void bntAcceptConfirm_Click(object sender, EventArgs e)
        {
            string id = btnAcceptConfirm.CommandArgument;

            if(!String.IsNullOrEmpty(id))
            {
                db.Execute(dsAccept, id);
                Log.Info(1337, "Zaakceptowano ankietę przez HR", id);
                Tabs.Reload();
                gvList.DataBind();
            }
        }

        protected void Tabs_SelectTab(object sender, EventArgs e)
        {
            SelectedTab = Tabs.SelectedValue;
        }

        public String SelectedTab
        {
            get { return hidSelectedTab.Value; }
            set { hidSelectedTab.Value = value; }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            Button btn = (sender as Button);
            string id = btn.CommandArgument;
            if (!String.IsNullOrEmpty(id))
            {
                btnRejectConfirm.CommandArgument = id;
                Tools.ShowConfirm("Czy na pewno chcesz odrzucić?", btnRejectConfirm);
            }
        }

        protected void btnRejectConfirm_Click(object sender, EventArgs e)
        {
            string id = btnRejectConfirm.CommandArgument;

            if (!String.IsNullOrEmpty(id))
            {
                db.Execute(dsReject, id);
                Log.Info(1337, "Odrzucono ankietę przez HR", id);
                Tabs.Reload();
                gvList.DataBind();
            }
        }
    }
}