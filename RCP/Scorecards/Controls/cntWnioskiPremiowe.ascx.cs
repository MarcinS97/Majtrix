using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Scorecards.Controls
{
    public partial class cntWnioskiPremiowe : System.Web.UI.UserControl
    {
        EventHandler Changed;

        private int FMode = moMoje;
        const int moMoje = 0;
        const int moToAcc = 1;
        const int moAccepted = 2;
        const int moRejected = 3;
        const int moAll = 99;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //-------------------
        public int Mode
        {
            set { FMode = value; }
            get { return FMode; }
        }
    }
}