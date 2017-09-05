using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class RepCzasPracy : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(DataSet dsKierownicy)
        {
            Tools.BindData(ddlKierownicy, dsKierownicy, "NI", "Id", true, null);
            cntSelectOkres.Prepare(DateTime.Today, true);
            repNadgodziny._Prepare(null, null, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, cntSelectOkres.OkresId, cntSelectOkres.Status, cntSelectOkres.IsArch, cntSelectOkres.StawkaNocna, RepNadgodziny3.nmoRcp);
        }

        public void Prepare(string kierId)
        {
            ddlKierownicy.Visible = false;
            lbKierownik.Visible = false;
            if (String.IsNullOrEmpty(kierId)) kierId = App.User.Id;
            cntSelectOkres.Prepare(DateTime.Today, true);
            repNadgodziny._Prepare(null, kierId, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, cntSelectOkres.OkresId, cntSelectOkres.Status, cntSelectOkres.IsArch, cntSelectOkres.StawkaNocna, RepNadgodziny3.nmoRcp);
            //Execute();
        }
        //------------------
        private void SetHeader()
        {
            repHeader.Caption = "Porównanie czasu pracy z RCP za miesiąc " + cntSelectOkres.FriendlyName(4);
            if (String.IsNullOrEmpty(ddlKierownicy.SelectedValue) || ddlKierownicy.SelectedValue == "-100") //"ALL")
                repHeader.Caption2 = null;
            else
                if (ddlKierownicy.Visible)
                    repHeader.Caption2 = ddlKierownicy.SelectedItem.Text.Replace(App.OldMarker, "");
                else
                    repHeader.Caption2 = AppUser.GetNazwiskoImie(repNadgodziny.KierId);
        }

        public void Execute()
        {
            SetHeader();
            if (ddlKierownicy.Visible)
                repNadgodziny._DataBindKier(ddlKierownicy.SelectedValue);
            else
                repNadgodziny._DataBindKier(repNadgodziny.KierId);
        }

        protected void ddlKierownicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            Execute();
        }

        protected void cntSelectOkres_Changed(object sender, EventArgs e)
        {
            SetHeader();
        }
        //------------------
        public RepHeader Header
        {
            get { return repHeader; }
        }

    }
}