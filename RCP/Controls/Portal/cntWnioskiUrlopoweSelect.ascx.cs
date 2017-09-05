using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls.Portal
{
    public partial class cntWnioskiUrlopoweSelect : System.Web.UI.UserControl
    {
        public event EventHandler Select;
        int FMode = moPracownik;
        const int moPracownik = 0;
        const int moKierownik = 1;

        public string SelectedTyp = null;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "select":
                    if (Select != null)
                    {
                        SelectedTyp = e.CommandArgument.ToString();
                        Select(this, EventArgs.Empty);
                    }
                    break;
            }
        }

        public int Mode
        {
            set 
            { 
                FMode = value;
                SqlDataSource1.SelectParameters["mode"].DefaultValue = value.ToString();
            }
            get { return FMode; }
        }
    }
}