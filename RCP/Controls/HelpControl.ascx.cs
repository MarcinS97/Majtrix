using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class HelpControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        //--------------------------------------
        protected void btHelpHide_Click(object sender, EventArgs e)
        {
            Show(false, true);  // przywróć poprzedni po ukryciu jeśli jest jakiś w store
        }
        //--------------------------------------
        private void GetHelpText()
        {
            ltHelp.Text = Info.GetInfoText(hidContext.Value);
        }
        //--------------------------------------
        public bool Show(bool visible, bool restore)
        {
            if (restore && !String.IsNullOrEmpty(hidStoreContext.Value))
            {
                hidContext.Value = hidStoreContext.Value;
                hidStoreContext.Value = null;
            }
            if (visible != Visible)
            {
                Visible = visible;
                if (visible)
                    GetHelpText();
            }
            return visible;
        }

        public void Show(string context)
        {
            hidStoreContext.Value = null;   // nie będzie już potrzebny

            if (Visible && HelpContext == context) 
                Show(false, false);
            else
            {
                HelpContext = context;
                Show(true, false);
            }
        }

        public void StoreHelpContext()
        {
            hidStoreContext.Value = HelpContext;
        }

        //--------------------------------------
        public string HelpContext
        {
            get { return hidContext.Value; }
            set
            {
                if (hidContext.Value != value)
                {
                    hidContext.Value = value;
                    if (this.Visible)
                        GetHelpText();
                }
            }
        }
    }
}