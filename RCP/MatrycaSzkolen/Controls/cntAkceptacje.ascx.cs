using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Controls
{
    public partial class cntAkceptacje : System.Web.UI.UserControl
    {
        public class Accept
        {
            public const string Szk = "1";
            public const string Ankiety = "2";
            public const string Oceny = "3";
        }

        protected void Types_SelectTab(object sender, EventArgs e)
        {
            SelectedType = Types.SelectedValue;
            SelectView();
        }

        void SelectView()
        {
            View v = mvContent.FindControl("View" + SelectedType) as View;
            if (v != null)
                mvContent.SetActiveView(v);

        }

        public String SelectedType
        {
            get { return hidSelectedType.Value; }
            set { hidSelectedType.Value = value; }
        }

        protected void Types_DataBound(object sender, EventArgs e)
        {
            if (!App.User.HasRight(AppUser.rMSCertyfikatyAcc) && !App.User.IsMSAdmin)
                Tools.RemoveMenu(Types.Tabs, Accept.Szk);
            if (!App.User.HasRight(AppUser.rMSAnkietyAcc) && !App.User.IsMSAdmin)
                Tools.RemoveMenu(Types.Tabs, Accept.Ankiety);
            if (!App.User.HasRight(AppUser.rMSKorektyAcc) && !App.User.IsMSAdmin)
                Tools.RemoveMenu(Types.Tabs, Accept.Oceny);
            if (Types.Tabs.Items.Count > 0)
            {
                //if (Types.Tabs.Items.Count == 1)
                //    divTypes.Visible = false;
                //else
                //    divTypes.Visible = true;

                /* JA ODPOWIADAM - IRSON */

                if (Request.QueryString.Count == 0)
                {
                    Types.Tabs.Items[0].Selected = true;
                    SelectedType = Types.Tabs.SelectedValue;
                    SelectView();
                }
                else
                {
                    Types.Tabs.Items[0].Selected = false;
                    SelectedType = Request.QueryString["p"];
                    Types.Tabs.Items[Int32.Parse(SelectedType) - 1].Selected = true;
                    SelectView();
                }
            }
            else
                App.ShowNoAccess("", App.User);
        }

    }
}