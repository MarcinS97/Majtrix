using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class KwitekForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
#if PORTAL      //testy
                if (!App.User.HasRight(AppUser.rKwitekAdm))
                    App.ShowNoAccess("Kwitek", App.User);

#endif
                //Wyplaty1.KadryId = App.User.NR_EW;
                //string nr = Wyplaty1.KadryId;
                //Wyplaty1.KadryId = nr;


                Wyplaty1.Prepare();
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Kwitek płacowy Form");
        }
        //--------------------------
        protected void Wyplaty1_SelectedChanged(object sender, EventArgs e)
        {            
            WyplataDetale1._Prepare(Wyplaty1.SelectedListaId); 
        }
    }
}
