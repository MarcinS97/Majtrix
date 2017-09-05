using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Controls
{
    public partial class cntListaZgloszen : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ModalButton_Click(object sender, EventArgs e)
        {
            String Id = (sender as Button).CommandArgument;
            cntKartaZgloszenie2.IdZgloszenia = Id;
            cntKartaZgloszenie2.Prepare();
            cntModal.Show(false);
        }
    }
}