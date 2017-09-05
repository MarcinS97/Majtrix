using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls
{
    public partial class Dzialy: System.Web.UI.UserControl
    {
        public string FDetailsList;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string GetStatus(object status)
        {
            string st = status.ToString();
            switch (st)
            {
                case "0": return "";
                case "1": return "Stary";
                case "2": return "Nowy";
                default: return st;
            }
        }
        //------------------

        private void TriggerSelect()
        {
            if (!String.IsNullOrEmpty(FDetailsList))
            {
                UserControl uc = (UserControl)Parent.FindControl(FDetailsList);
                if (uc != null)
                {
                    if (uc is DzialyStrefy) ((DzialyStrefy)uc).DzialId = SelectedId;
                }
            }
        }

        protected void lvDzialy_SelectedIndexChanged(object sender, EventArgs e)
        {
            TriggerSelect();
        }

        protected void lvDzialy_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

        }
        //-------------------
        public string DetailsList
        {
            get { return FDetailsList; }
            set { FDetailsList = value; }
        }
    
        public string SelectedId
        {
            get
            {
                if (lvDzialy.SelectedIndex >= 0 && lvDzialy.SelectedIndex < lvDzialy.DataKeys.Count)
                    return lvDzialy.DataKeys[lvDzialy.SelectedIndex].Value.ToString();
                else
                    return null;
            }
        }

    }
}