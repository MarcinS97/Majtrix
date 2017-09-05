using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls.Wnioski.PracaZdalna
{
    public partial class cntWniosekModal : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Show(String RequestId)
        {
            this.RequestId = RequestId;
            cntModal.Show(false);
        }

        public void Show()
        {
            cntModal.Show(false);
        }

        public void Close()
        {
            cntModal.Close();
        }

        public String RequestId
        {
            get { return ViewState["vRequestId"] as String; }
            set { ViewState["vRequestId"] = value; }
        }
    }
}