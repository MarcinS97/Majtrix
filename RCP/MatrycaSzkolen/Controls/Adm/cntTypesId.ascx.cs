using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Controls.Adm
{
    public partial class cntTypesId : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvTypes, 1337);
        }

        public String Title
        {
            set { lblTitle.InnerText = value; }
        }

        public String Subtitle
        {
            set { lblSubtitle.InnerText = value; }
        }

        public String TableName
        {
            get { return hidTableName.Value; }
            set { hidTableName.Value = value; }
        }
    }
}