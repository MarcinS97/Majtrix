using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.SzkoleniaBHP.Controls
{
    public partial class cntKierSelect : System.Web.UI.UserControl
    {
        public event EventHandler Changed;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(string rootId)
        {
            Tools.SelectItem(ddlKier, rootId);
        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            TriggerChanged();
        }

        protected void cbShowSub_CheckedChanged(object sender, EventArgs e)
        {
            TriggerChanged();
        }

        private void TriggerChanged()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }

        public string KierId
        {
            get { return ddlKier.SelectedValue; }
        }

        public bool ShowSub
        {
            get { return cbShowSub.Checked; }
        }

    }
}