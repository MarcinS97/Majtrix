using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls
{
    public partial class x_AcceptControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void FormView1_PageIndexChanging(object sender, FormViewPageEventArgs e)
        {

        }
        //-----------------
        public void Prepare(string pracId, string data)
        {
            PracId = pracId;
            Data = data;
            fvData.DataBind();
        }

        //-----------------
        public string PracId
        {
            get { return hidPracId.Value; }
            set { hidPracId.Value = value; }
        }

        public string Data
        {
            get { return hidData.Value; }
            set { hidData.Value = value; }
        }
    }
}