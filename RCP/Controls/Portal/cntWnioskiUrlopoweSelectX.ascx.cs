using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls.Portal
{
    public partial class cntWnioskiUrlopoweSelectX : System.Web.UI.UserControl
    {
        public event EventHandler Select;
        public string SelectedTyp = null;

        protected void Page_Load(object sender, EventArgs e)
        {


        }
        
        public int Mode
        {
            set;
            get;
        }

    }
}