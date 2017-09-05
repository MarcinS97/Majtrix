using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls
{
    public partial class PlanPracyLine : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void DataBind(string dFrom, string dTo)
        {
            FromDate = dFrom;
            ToDate = dTo;
            Repeater1.DataBind();
        }


        //cbAccept.Attributes["onclick"] = "showAItemEdit(!this.checked, '" + tdEdit.ClientID + "');";

        public string FromDate
        {
            get { return hidFrom.Value; }
            set { hidFrom.Value = value; }
        }

        public string ToDate
        {
            get { return hidTo.Value; }
            set { hidTo.Value = value; }
        }
        
        public string PracId
        {
            get { return hidPracId.Value; }
            set { hidPracId.Value = value; }
        }

        protected void Repeater1_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
        
        }
    }
}