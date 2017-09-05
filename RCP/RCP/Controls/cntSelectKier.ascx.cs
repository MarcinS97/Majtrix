using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Controls
{
    public partial class cntSelectKier : System.Web.UI.UserControl
    {
        public event EventHandler DataBound;
        public event EventHandler SelectedIndexChanged;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string uid = App.User.Id;
                bool adm = App.User.IsAdmin;
                bool kier = App.User.IsKierownik;
                
                hidKierId.Value = uid;          // zawsze w kontekscie zalogowanego usera
                hidUserId.Value = uid;          // zawsze w kontekscie zalogowanego usera

                ddlKier.DataSourceID = adm ? dsKierAll.ID : dsKier.ID;  // filtr kierownik



                /*
                ddlKier.DataBind();
                if (ddlKier.Items.Count == 1)                           // tylko kieras
                {
                    ddlKier.Visible = false;
                    TriggerDataBound();
                }
                else
                {
                    int idx = -1;
                    string sid = App.SesSelKierId;
                    if (!String.IsNullOrEmpty(sid))
                    {
                        idx = Tools.SelectItem2(ddlKier, sid);   // admin będący kierownikiem
                        if (idx != -1) TriggerDataBound();
                    }
                    if (idx == -1 && kier)
                    {
                        Tools.SelectItem(ddlKier, uid);   // admin będący kierownikiem
                        TriggerDataBound();
                    }
                }
                //else if (adm && kier)
                //{
                //    Tools.SelectItem(ddlKier, uid);   // admin będący kierownikiem
                //    TriggerDataBound();
                //}   
                 */

            }
        }
        //------------------------
        private void TriggerDataBound()
        {
            if (DataBound != null)
                DataBound(ddlKier, EventArgs.Empty);
        }

        private void TriggerSelectedChanged()
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(ddlKier, EventArgs.Empty);
        }

        protected void ddlKier_DataBound(object sender, EventArgs e)
        {
            if (ddlKier.Items.Count == 1)                       // tylko kieras
                ddlKier.Visible = false;
            else                                                // admin lub podlegli kierownicy
            {
                string uid = App.User.Id;
                bool adm = App.User.IsAdmin;
                bool kier = App.User.IsKierownik;

                int idx = -1;
                string sid = App.SesSelKierId;
                if (!String.IsNullOrEmpty(sid))
                {
                    idx = Tools.SelectItem2(ddlKier, sid);      // admin będący kierownikiem
                }
                if (idx == -1 && kier)
                {
                    Tools.SelectItem(ddlKier, uid);             // admin będący kierownikiem
                }
            }
            TriggerDataBound();
        }

        protected void ddlKier_SelectedIndexChanged(object sender, EventArgs e)
        {
            App.SesSelKierId = ddlKier.SelectedValue;           // tylko przy zmianie z ręki ustawiam ses
            TriggerSelectedChanged();
        }

        //------------------------
        public DropDownList List
        {
            get { return ddlKier; }
        }

        public string SelectedValue
        {
            get { return ddlKier.SelectedValue; }
        }

    }
}