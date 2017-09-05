using HRRcp.App_Code;
using HRRcp.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Wnioski.PracaZdalna
{
    public partial class Kier : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HasFormAccess)
                {
                    Tools.SetNoCache();
                    //----------------------
                }
                else
                    App.ShowNoAccess();
            }
        }

        protected void btnNewRequest_Click(object sender, EventArgs e)
        {
            //cntWniosekModal.Show();
            cntWniosekUrlopowy1.PrepareNew(cntWniosekUrlopowy.wtPZ, cntWniosekUrlopowy.osKierownik);
        }

        protected void cntWniosekUrlopowy1_Close(object sender, EventArgs e)
        {
            //cntWnioskiUrlopowe2.Prepare(true);

            cntWnioskiUrlopowe2.List.DataBind();  
            UpdatePanel1.Update();
        }

        protected void cntWnioskiUrlopowe2_Show(object sender, EventArgs e)
        {
            int wnid = ((cntWnioskiUrlopowe)sender).ShowWniosekId;
            cntWniosekUrlopowy1.Show(wnid, cntWniosekUrlopowy.osPracownik, false);
        }

        public static bool HasFormAccess
        {
            get
            {
                return App.User.HasRight(AppUser.rWnioskiZdalna);
            }
        }
    }
}