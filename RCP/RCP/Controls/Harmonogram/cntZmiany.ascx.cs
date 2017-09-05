using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Controls.Harmonogram
{
    public partial class cntZmiany : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
#if OKT
                cbFillFree.Checked = true;
#else 
                cbFillFree.Checked = false;
#endif
            }
        }

        public int Min
        {
            get { return Convert.ToInt32(hidMin.Value); }
            set { hidMin.Value = value.ToString(); }
        }

        public int Max
        {
            get { return Convert.ToInt32(hidMax.Value); }
            set { hidMax.Value = value.ToString(); }
        }

        protected void dsShifts_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            //bool chevronsVisible = rpShiftGroups.Items.Count > 6;
            //leftChevron.Visible = rightChevron.Visible = chevronsVisible;
        }
    }
}