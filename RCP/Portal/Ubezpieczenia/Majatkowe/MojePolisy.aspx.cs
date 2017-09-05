using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Ubezpieczenia.Majatkowe
{
    public partial class MojePolisy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cntLista_Show(object sender, EventArgs e)
        {
            ListView lv = sender as ListView;

            String id = lv.SelectedValue.ToString();
            cntWniosekModal.Show(id);
        }

        protected void cntWniosekModal_Saved(object sender, EventArgs e)
        {
            upMain.Update();
            cntLista.DataBind();
        }
    }
}