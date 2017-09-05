using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class PracInfo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(SqlConnection con, string onDay, DataRow pdr)
        {
            /*
            //!!! ustawić strefaId i algRCP na wartości na dzień Data !!!
            lbDay.Text = onDay;
            lbPracownik.Text = Base.getValue(pdr, "NazwiskoImie");
            lbDzial.Text = Base.getValue(pdr, "Dzial");
            lbStanowisko.Text = Base.getValue(pdr, "Stanowisko");
            if (!String.IsNullOrEmpty(strefaId))
                lbStrefaRCP.Text = Base.getScalar(con, "select Nazwa from Strefy where Id=" + strefaId);

            DataRow adr;
            if (!String.IsNullOrEmpty(algRCP))
            {
                adr = Base.getDataRow(con, "select * from Kody where Typ='ALG' and Kod=" + algRCP);
                lbAlgorytm.Text = Base.getValue(adr, "Nazwa");
            }
            else adr = null;

            string zmid = Base.getValue(wtdr, "IdZmiany");
            bool isZmiana = !String.IsNullOrEmpty(zmid);
            if (isZmiana)
            {
                DataRow zdr = Base.getDataRow(con, "select * from Zmiany where Id = " + zmid);
                if (zdr != null)
                {
                    lbZmiana.Text = Base.getValue(zdr, "Symbol") + " " + Base.getValue(zdr, "Nazwa");
                }
            }
             * */
        }

    }
}