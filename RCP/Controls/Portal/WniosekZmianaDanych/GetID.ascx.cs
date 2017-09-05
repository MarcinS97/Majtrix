using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HRRcp.App_Code;

namespace HRRcp.Controls.WnioseZmianaDanych
{
    public partial class GetID : System.Web.UI.UserControl
    {
        bool super_variable = false;
        public EventHandler TurboClickUltimate2;
        
        public void GetId(String Id) {
            sqlcontent1.DoSelectTab_wn(Id);
        
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            sqlcontent1.TurboClick3 += new EventHandler(TurboClickUltimate);
            
           
        }

        protected void TurboClickUltimate(object sender, EventArgs e)
        {
            if (TurboClickUltimate2 != null)
            {
               // Tools.CloseDialog("DivZoom_Report");
                if (super_variable!=true)
                TurboClickUltimate2(sender, null);
                super_variable = true;
            }
                 }
    }
}