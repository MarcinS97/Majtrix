using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.Adm
{
    public partial class cntStrukturaAdm : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    
        //----- struktura ---------------------------------------
        protected void ShowCzas1()
        {
            string pid = cntStruktura.SelectedPracId;
            string _id = cntStruktura._SelectedRcpId;   //<<< nie jest używany po uzaleznieniu od czasu
            //if (String.IsNullOrEmpty(id)) id = "-1";
            //cntPracInfo.Prepare(pid);
            if (!String.IsNullOrEmpty(pid))
            {
                lbPracName.Text = cntStruktura.SelectedNI;
                string strefaId, algId, algPar;
                Worktime.GetStrefaRCP2(pid, cntSelectOkres.DateTo, out strefaId, out algId, out algPar);
                cntRCPStruct.Prepare(pid, _id, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, strefaId);
            }
        }

        protected void cntStruktura_SelectedChanged(object sender, EventArgs e)    // selectOkres
        {
            ShowCzas1();
        }

        protected void btRefresh1_Click(object sender, EventArgs e)
        {
            ShowCzas1();
        }
    }
}