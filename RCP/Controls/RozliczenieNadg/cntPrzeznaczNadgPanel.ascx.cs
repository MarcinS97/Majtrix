using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class cntPrzeznaczNadgPanel : System.Web.UI.UserControl
    {
        int FMode = moKier;
        const int moKier = 0;
        const int moAdm = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                switch (FMode)
                {
                    default:
                    case moKier:
                        cntStruktura1.Prepare(App.User.Id, DateTime.Today);
                        break;
                    case moAdm:
                        cntStruktura1.Prepare(null, DateTime.Today);
                        break;
                }
                //cntStruktura1
            }
        }

        public void Prepare()
        {
        }

        protected void cntStruktura1_SelectedChanged(object sender, EventArgs e)
        {

            cntKartotekaNadg.Prepare(cntStruktura1.SelectedPracId, "20131001", "20131231", 0);
        }

        public int Mode
        {
            set { FMode = value; }
            get { return FMode; }
        }
    }
}