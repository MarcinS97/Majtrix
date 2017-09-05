using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Controls.Adm
{

    public partial class cntZmianyList : System.Web.UI.UserControl
    {
        public event EventHandler EditClick;

        protected void Page_Load(object sender, EventArgs e)
        {
            rpZmiany.DataBind();
        }
        public string GetColorNoHash(string color)
        {
            try
            {
                return color.StartsWith("#") ? color.Substring(1) : color;
            }
            catch
            {
                return "Transparent";
            }
        }
        public Color GetColorNull(string color)
        {
            try
            {
                //return ColorTranslator.FromHtml(color.StartsWith("#") ? color : "#" + color);
                return ColorTranslator.FromHtml(color);
            }
            catch
            {
                return Color.Transparent;
            }
        }

        protected void EditClickTrigger(object sender, EventArgs e)
        {
            if (EditClick != null) EditClick(sender, e);
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            //cntModalZmianyEdit.Show(false);
            string id = (sender as LinkButton).CommandArgument;
           
            EditClickTrigger(id,e);

        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            EditClickTrigger(null, e);
        }
    }
}