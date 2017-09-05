using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.Kwitek
{
    public partial class KwitekHeader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private string GetSql()
        {
            return String.Format(@"
select top 1
LpLogo, Imie, Nazwisko, Pesel, Rok, Miesiac, 
NrListy, KategoriaListy, Data, Zamknieta
from lp_vv_NaliczeniaNagExtSkl where
LpLogo = '{0}' order by Data desc", App.KwitekKadryId);
        }
    }
}