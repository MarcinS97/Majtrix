using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls.RozliczenieNadg
{
    public partial class cntRozlNadgSuma : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(string pracId, string okresOd, string okresDo)
        {
            hidPracId.Value = pracId;
            hidOkresOd.Value = okresOd;
            hidOkresDo.Value = okresDo;
            lvSumy.DataBind();
        }

        public void Update()
        {
            lvSumy.DataBind();
        }

        protected string GetRowClass(object sort)
        {
            int s = (int)sort;
            switch (s)
            {
                case 0:
                    return "suma";
                default:
                    return "miesiac mc" + s.ToString();
            }
        }
    }
}