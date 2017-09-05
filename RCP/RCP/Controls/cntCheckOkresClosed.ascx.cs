using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.RCP.Controls
{
    public partial class cntCheckOkresClosed : System.Web.UI.UserControl
    {
        public int CheckShow(DateTime Od, DateTime Do, String IdKierownika)
        {
            try
            {
                hidData.Value = Tools.DateToStrDb(Od) + "|" + Tools.DateToStrDb(Do) + "|" + IdKierownika;
                cntReport.SQL = String.Format(dsGetKierNotClosedNew.SelectCommand, Tools.DateToStrDb(Od), Tools.DateToStrDb(Do), IdKierownika);
                int ret = cntReport.Execute();
                if (ret != 0)
                {
                    cntModal.Show(false);
                    return 1;
                }
                else return 0;
            }
            catch
            {
                return -1;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnExportCSV_Click(object sender, EventArgs e)
        {
            String a = hidData.Value.Split('|')[0];
            String b = hidData.Value.Split('|')[1];
            String c = hidData.Value.Split('|')[2];
            Report.ExportCSV(String.Format("Weryfikacja_Zamkniecia_{0}", b), String.Format(dsGetKierNotClosedNew.SelectCommand, a, b, c), null, null, true);
        }
    }
}