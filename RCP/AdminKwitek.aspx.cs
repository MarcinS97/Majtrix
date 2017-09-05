using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class AdminKwitekForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Prepare();
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Kwitek płacowy AdminForm");
        }
        //--------------------------
        public void Prepare()
        {
            Wyplaty1.Prepare();
            Urlop1.Prepare(null, true);
        }

        public void UpdateListy()
        {
            Wyplaty1.Prepare();
        }

        protected void Wyplaty1_SelectedChanged(object sender, EventArgs e)
        {
            WyplataDetale1._Prepare(Wyplaty1.SelectedListaId);
        }

    }
}
