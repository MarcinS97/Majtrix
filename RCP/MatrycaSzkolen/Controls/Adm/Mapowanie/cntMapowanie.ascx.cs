using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.MatrycaSzkolen.Controls.Adm
{
    public partial class cntMapowanie : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnAdd.Enabled = btnDelete.Enabled = false;
            }
        }

        protected void StanowiskoSelected(object sender, EventArgs e)
        {
            string s = sender.ToString();
            string stanId = s.Split(';')[0];
            string text = s.Split(';')[1];


            StanowiskoId = LinieStanowiska.StanowiskoId = stanId;
            lblConnection.Text = text;
            btnAdd.Enabled = stanId != "-1";
            LinieStanowiska.ClearSelect();
            LinieStanowiska.SetEdit();

            //if (!(LinieStanowiska.GetCount() > 0))
            btnDelete.Enabled = false;

        }

        protected void AddTask(object sender, EventArgs e)
        {
            if(StanowiskoId != "-1")
                LinieStanowiska.AddTasks(StanowiskoId, Linie.SelectedLinie);
            Linie.ClearSelect();
        }

        protected void RemoveTask(object sender, EventArgs e)
        {
            LinieStanowiska.RemoveTask();
            Linie.ClearSelect();
        }

        public string StanowiskoId
        {
            get { return ViewState["vStanowiskoId"] as String; }
            set { ViewState["vStanowiskoId"] = value; }
        }

        protected void LinieStanowiska_Deleted(object sender, EventArgs e)
        {
            if (!(LinieStanowiska.GetCount() > 0))
                btnDelete.Enabled = false;
        }

        protected void LinieStanowiska_Selected(object sender, EventArgs e)
        {
            btnDelete.Enabled = true;
        }
    }
}