using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.MatrycaSzkolen.Controls.Adm.Mapowanie
{
    public partial class cntStanowiska : System.Web.UI.UserControl
    {
        public event EventHandler StanowiskoSelected;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvStanowiska, 1337);
            Tools.PrepareSorting2(lvStanowiska, 1, 10);
        }

        protected void lvSpreadsheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            TriggerChanged(lvStanowiska.SelectedDataKey.Value);
        }

        void TriggerChanged(object sender)
        {
            if (StanowiskoSelected != null)
                StanowiskoSelected(sender + ";" + GetName(), EventArgs.Empty);

            
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            tbSearch.Text = "";
        }

        public string GetName()
        {
            int indx = lvStanowiska.SelectedIndex;
            if(indx >= 0)
            {
                string text = Tools.GetText(lvStanowiska.Items[indx], "lblName");
                if(!String.IsNullOrEmpty(text))
                    return text;
            }
            return "Powiązania - wybierz szkolenie stanowiskowe";
        }

        protected void cbShow_CheckedChanged(object sender, EventArgs e)
        {
            if (lvStanowiska.Items.Count > 0)
                lvStanowiska.SelectedIndex = -1;
            TriggerChanged(-1);
        }
    }
}