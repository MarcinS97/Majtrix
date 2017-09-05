using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Controls
{
    public partial class cntAkceptacjeOceny : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            string arg = (sender as Button).CommandArgument;
            int index = Convert.ToInt32(arg);


            GridViewRow row = gvList.Rows[index];
            string id = gvList.DataKeys[index].Value.ToString();
            string ocena = Tools.GetText(row, "tbOcena");
            string uwagi = Tools.GetText(row, "tbUwagi");


            if (!String.IsNullOrEmpty(id))
            {
                btnAcceptConfirm.CommandArgument = id + ";" + ocena + ";" + uwagi;
                Tools.ShowConfirm("Czy na pewno chcesz zaakceptować?", btnAcceptConfirm);
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            string arg = (sender as Button).CommandArgument;
            int index = Convert.ToInt32(arg);


            GridViewRow row = gvList.Rows[index];
            string id = gvList.DataKeys[index].Value.ToString();
            string ocena = Tools.GetText(row, "tbOcena");
            string uwagi = Tools.GetText(row, "tbUwagi");
            if (!String.IsNullOrEmpty(id))
            {
                btnRejectConfirm.CommandArgument = id + ";" + ocena + ";" + uwagi;
                Tools.ShowConfirm("Czy na pewno chcesz odrzucić?", btnRejectConfirm);
            }
        }

        protected void btnAcceptConfirm_Click(object sender, EventArgs e)
        {
            Accept(btnAcceptConfirm.CommandArgument);
        }

        protected void btnRejectConfirm_Click(object sender, EventArgs e)
        {
            Reject(btnRejectConfirm.CommandArgument);
        }

        void Accept(string parameters)
        {
            string id = parameters.Split(';')[0];
            string ocena = parameters.Split(';')[1];
            string uwagi = parameters.Split(';')[2];

            db.Execute(dsAccept, id, db.nullStrParam(ocena), db.nullStrParam(uwagi), App.User.Id);
            Reload();
        }

        void Reject(string parameters)
        {
            string id = parameters.Split(';')[0];
            string ocena = parameters.Split(';')[1];
            string uwagi = parameters.Split(';')[2];

            db.Execute(dsReject, id, db.nullStrParam(ocena), db.nullStrParam(uwagi));
            Reload();
        }
        
        void Reload()
        {
            gvList.DataBind();
        }
    }
}