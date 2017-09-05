using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.MatrycaSzkolen.Controls.Uprawnienia
{
    public partial class cntPracownikUprawnienia : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(GridView1, "GridView2", false, 0, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TranslatePage();
            }
        }

        public void Prepare(string pracId, string typ, string kwal, string awp, string data, string monit)
        {
            hidPracId.Value = pracId;
            hidTyp.Value = typ;
            hidKwal.Value = kwal;
            hidData.Value = data;
            hidMonit.Value = monit;
            Tools.SelectMenu(tabFilter, awp);
            Visible = true;
        }

        private void TranslatePage()
        {
            L.p(tabFilter);
            L.p(hidUnlimited);
        }

        public void Cancel()
        {
            Visible = false;
        }

        protected void GridView1Cmd_Click(object sender, EventArgs e)
        {
            /*
            string[] par = Tools.GetLineParams(GridView1CmdPar.Value);
            switch (par[0])
            {
                case "rank":
                    //if (Ocena != null) Ocena(this, EventArgs.Empty);
                    break;
            }
             */ 
        }

        public string CmdPar
        {
            get { return GridView1CmdPar.Value; }
        }

    }
}