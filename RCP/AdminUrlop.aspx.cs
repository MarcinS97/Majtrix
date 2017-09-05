using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class AdminUrlopForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Prepare();
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Urlop AdminForm");
        }
        //--------------------------
        public void Prepare()
        {
            Urlop1.Prepare(null, true);
            UpdatePanel1.Update();
        }
    }
}
