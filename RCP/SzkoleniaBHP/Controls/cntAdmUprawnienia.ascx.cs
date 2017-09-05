using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Uprawnienia.Controls
{
    public partial class cntAdmUprawnienia : System.Web.UI.UserControl
    {
        const bool editable = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvTypy, 0);
            Tools.PrepareDicListView(lvKwalifikacje, 0);
            Tools.PrepareDicListView(lvUprawnienia, 0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!editable)
                    lvTypy.InsertItemPosition = InsertItemPosition.None;
            }
        }

        public InsertItemPosition GetInsertItemPositionKwal()
        {
            if (!editable)
                return InsertItemPosition.None;

            if (lvTypy.SelectedIndex != -1)
                return InsertItemPosition.LastItem;
            else
                return InsertItemPosition.None;
        }

        public InsertItemPosition GetInsertItemPositionUpr()
        {
            if (!editable)
                return InsertItemPosition.None;

            if (lvTypy.SelectedIndex != -1)
                return InsertItemPosition.LastItem;
            else
                return InsertItemPosition.None;
        }
    }
}     