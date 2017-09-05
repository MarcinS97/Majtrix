using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP
{
    public partial class Zastepstwa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HasFormAccess)   //App.User.HasAccess sprawdzane w MasterPage3
                {
                    Tools.SetNoCache();
                    //----------------------
                    bool adm = App.User.IsAdmin;
                    cZastepstwa.Visible = !adm;
                    cZastepstwaAdm.Visible = adm;
                }
                else
                    App.ShowNoAccess();
            }
        }

        public static bool HasFormAccess
        {
            get
            {
                return db.SqlMenuHasRights(5090, App.User);
                //return App.User.IsKierownik || App.User.IsAdmin;
            }
        }
    }
}