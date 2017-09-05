using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.SzkoleniaBHP.Controls
{
    public partial class cntStartBHP : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(GridView1);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public int Prepare()
        {
            if (App.User.HasRight(AppUser.rSzkoleniaBHPAdm))
                hidKierId.Value = "-99";
            else
                hidKierId.Value = App.User.Id;
            GridView1.DataBind();
            return GridView1.Rows.Count;
        }
    }
}