using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.Portal;

namespace HRRcp.Controls.Portal
{
    public partial class UbezpieczeniaMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
#if PORTAL

                //litTitle.Text = PortalMasterPage3.LeftMenuSelectedItem + SelectedItem;
                if (SelectedItem != null)
                {
                    if (Parent is UbezpieczeniaForm)
                    {
                        ((UbezpieczeniaForm)Parent).Prepare(SelectedItem);

                    }
                }


#elif true

#else
                ((PortalMasterPage3)Master).paRight.Visible = false;
                Tools.AddClass(((PortalMasterPage3)Master).ttbPortal, "fullwidth-left");
#endif
            }
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "click":
                    string c = e.CommandArgument.ToString();
                    SelectedItem = c;
                    //Session["ubezpnaszybko"] = c;
                    if (Parent is UbezpieczeniaForm)
                    {
                        ((UbezpieczeniaForm)Parent).Prepare(c);
                        upMain.Update();
                        Repeater1.DataBind();
                    }
                    else
                    {
                        App.Redirect("Portal/Ubezpieczenia.aspx");
                        //string url;
                        //if (Tools.IsUrl(c, out url))
                        //    App.Redirect(url);
                        //else
                        //{
                        //    url = Tools.GetRedirectUrl(c);
                        //    App.Redirect(url);
                        //    switch (c)
                        //    {
                        //        case "":
                        //            break;
                        //    }
                        //}
                    }
                    break;
            }
        }

        public static String SelectedItem
        {
            get { return (String)HttpContext.Current.Session["sesSelectedUbezpItem"]; }
            set { HttpContext.Current.Session["sesSelectedUbezpItem"] = value; }
        }
    }
}