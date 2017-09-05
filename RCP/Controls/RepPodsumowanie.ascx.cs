using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class RepPodsumowanie : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            if (Visible)
                ((MasterPage)Page.Master).SetWideJs(false);                
             */
            if (!IsPostBack)
            {
                FillData();
            }
        }

        protected void zoom_Click(object sender, EventArgs e)
        {
            LinkButton lbt = (LinkButton)sender;
            //if (lbt.Text != "0")
            Response.Redirect(App.WynikiForm + "?p=" + lbt.CommandArgument);
        }

        //---------------------------------
        private void Prepare(SqlConnection con, LinkButton lbt, int id)
        {
            /*
            string cnt = PracownicyControl.GetCount(con, id);
            lbt.Text = cnt;
            lbt.CommandArgument = id.ToString();    // tu przechowam parametr
             */
            //if (cnt == "0") lbt.Enabled = false;

            //Report.MakeLink(lbt,"~/" + App.WynikiForm, id.ToString());  - to nie chodzi jak link do tej samej strony
            //lbt.Command += new CommandEventHandler(zoom_Command);
            //lbt.Click += new EventHandler(zoom_Click);    - to też nie działa trzeb było dac eventy w aspx
        }

        private void FillData()
        {
            SqlConnection con = Base.Connect();
            Prepare(con, LinkButton1, 10);
            Prepare(con, LinkButton9, 30);
            Prepare(con, LinkButton2, 20);
            Prepare(con, LinkButton3, 21);
            Prepare(con, LinkButton4, 22);
            //Prepare(con, LinkButton5, 32);  //2
            //Prepare(con, LinkButton6, 33);  //3
            //Prepare(con, LinkButton7, 34);  //42
            //Prepare(con, LinkButton8, 36);  //6
            Base.Disconnect(con);
        }
    }
}