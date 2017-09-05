using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Controls.Uprawnienia
{

    public partial class cntTransfer : System.Web.UI.UserControl
    {
        public event EventHandler Transfered;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            string kwalId = ddlKwalifikacje.SelectedValue;
            string grupaId = ddlGrupy.SelectedValue;
            if (!String.IsNullOrEmpty(kwalId))
            {
                db.Execute(dsTransfer, UprId, kwalId, db.nullParam(grupaId));
            }
            this.Visible = false;
            if (Transfered != null)
                Transfered(sender, e);
        }


        public string UprId
        {
            get { return hidUprId.Value; }
            set { hidUprId.Value = value; }
        }

    }
}