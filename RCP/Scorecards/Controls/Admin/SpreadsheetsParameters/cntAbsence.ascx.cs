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
    public partial class cntAbsence : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvAbsence, 0);
            Tools.PrepareSorting2(lvAbsence, 3, 10);
        } 

        public void Prepare(String ScorecardTypeId, String TL)
        {
            this.ScorecardTypeId = ScorecardTypeId;
            this.TL = TL;
            lvAbsence.DataBind();
        }

        protected void lvAbsence_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !UpdateItem(lvAbsence.EditItem, e.OldValues, e.NewValues, e);
        }

        protected void lvAbsence_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !UpdateItem(e.Item, null, e.Values, e);
        }

        private bool UpdateItem(ListViewItem Item, IOrderedDictionary OldValues, IOrderedDictionary Values, EventArgs e)
        {
            try
            {
                RadioButtonList rblCurrency = Item.FindControl("rblCurrency") as RadioButtonList;
                Boolean Percent = rblCurrency.SelectedValue == "0";
                int Curr = !Percent ? 1337 : 0;
                
                Values["Ile"] = Double.Parse(Tools.GetText(Item, "tbIle").Replace(',', '.'), CultureInfo.InvariantCulture) / (Percent ? 100 : 1);
                Values["DlaIlu"] = Double.Parse(Tools.GetText(Item, "tbDlaIlu").Replace(',', '.'), CultureInfo.InvariantCulture);
                Values["curr"] = Curr;
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