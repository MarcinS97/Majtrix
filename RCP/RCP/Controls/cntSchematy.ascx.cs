using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP
{
    public partial class cntSchematy : System.Web.UI.UserControl
    {
        const string _defaultClass = "list-group-item";
        const string _activeClass = "list-group-item active";

        public event EventHandler Selected;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void lnkScheme_Click(object sender, EventArgs e)
        {
            LinkButton btn = (sender as LinkButton);
            string id = btn.CommandArgument;
            if (!String.IsNullOrEmpty(id))
            {
                btn.CssClass = _activeClass;
                SelectedValue = id;
                DeselectAll();
                if (Selected != null)
                    Selected(sender, e);
            }
        } 

        void DeselectAll()
        {
            foreach(RepeaterItem item in rpSchemes.Items)
            {
                string id = Tools.GetText(item, "hidId");
                if(!String.IsNullOrEmpty(id))
                {
                    if(id != SelectedValue)
                    {
                        LinkButton btn = item.FindControl("lnkScheme") as LinkButton;
                        if(btn != null)
                        {
                            btn.CssClass = _defaultClass;
                        }
                    }

                }
            }
        }

        public String GetClass(object oid)
        {
            if(oid != null)
            {
                string id = oid.ToString();
                return "list-group-item" + ((id == SelectedValue) ? " active" : "");
            }
            return "";
        }
        public bool EditMode
        {
            get { return Tools.GetViewStateBool(ViewState["vEditMode"], false); }
            set { ViewState["vEditMode"] = value; }
        }

        public String SelectedValue
        {
            get { return hidSelectedValue.Value; }
            set { hidSelectedValue.Value = value; }
        }
    }
}