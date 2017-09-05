using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Collections.Specialized;
using System.Globalization;

namespace HRRcp.Scorecards.Controls.Admin.SpreadsheetsParameters
{
    public partial class cntProductivity : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvProductivity, 0);
            Tools.PrepareSorting2(lvProductivity, 4, 10);
        }

        public void Prepare(String ScorecardTypeId, String TL)
        {
            this.ScorecardTypeId = ScorecardTypeId;
            this.TL = TL;
            ddlProductivity.DataBind();
            PrepareTitle();
            lvProductivity.DataBind();
        }

        protected void PrepareTitle()
        {
            lblTitle.Text = String.Format("Produktywność - {0}", ddlProductivity.SelectedItem.Text);
        }

        protected void ddlProductivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrepareTitle();
        }

        protected void lvProductivity_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            //e.NewValues["Rodzaj"] = Tools.GetDdlSelectedValueInt(lvProductivity.EditItem, "ddlGenre");
            e.Cancel = !UpdateItem(lvProductivity.EditItem, e.OldValues, e.NewValues, e);
        }

        protected void lvProductivity_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            //e.Values["Rodzaj"] = Tools.GetDdlSelectedValueInt(lvProductivity.InsertItem, "ddlGenre");
            e.Cancel = !UpdateItem(e.Item, null, e.Values, e);
        }

        private bool UpdateItem(ListViewItem Item, IOrderedDictionary OldValues, IOrderedDictionary Values, EventArgs e)
        {
            try
            {
                Values["Ile"] = Double.Parse(Tools.GetText(Item, "tbIle").Replace(',', '.'), CultureInfo.InvariantCulture);
                Values["DlaIlu"] = Double.Parse(Tools.GetText(Item, "tbDlaIlu").Replace(',', '.'), CultureInfo.InvariantCulture) / 100;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public String ScorecardTypeId
        {
            get { return hidScorecardTypeId.Value; }
            set { hidScorecardTypeId.Value = value; }
        }

        public String TL
        {
            get { return hidTL.Value; }
            set { hidTL.Value = value; }
        }
    }
}