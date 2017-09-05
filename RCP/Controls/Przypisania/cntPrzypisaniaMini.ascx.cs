using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.Przypisania
{
    public partial class cntPrzypisaniaMini : System.Web.UI.UserControl
    {
        public event EventHandler Select;
        //public int SelectedId = -1;

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvPrzypisaniaMini, 0);
            Tools.PrepareSorting(lvPrzypisaniaMini, 1, 4);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public int Prepare(string pracId, string dataOd)
        {
            hidPracId.Value = pracId;
            hidDataOd.Value = String.IsNullOrEmpty(dataOd) ? Tools.DateToStr(DateTime.Today) : dataOd;
            DataPager dp = Tools.Pager(lvPrzypisaniaMini);
            if (dp != null)
            {
                lvPrzypisaniaMini.DataBind();
                dp.SetPageProperties(0, dp.PageSize, true);
                return dp.TotalRowCount;
            }
            else
            {
                lvPrzypisaniaMini.DataBind();
                return lvPrzypisaniaMini.Items.Count;
            }
        }

        private void TriggerSelected()
        {
            if (Select != null)
            {
                //SelectedId = (int)lvPrzypisaniaMini.SelectedDataKey.Value;
                Select(this, EventArgs.Empty);
            }
        }

        protected void lvPrzypisaniaMini_SelectedIndexChanged(object sender, EventArgs e)
        {
            TriggerSelected();
        }

        public ListView List
        {
            get { return lvPrzypisaniaMini; }
        }

        public int SelectedId
        {
            get { return lvPrzypisaniaMini.SelectedIndex == -1 || lvPrzypisaniaMini.SelectedDataKey == null ? -1 : (int)lvPrzypisaniaMini.SelectedDataKey.Value; }
        }

        protected void lvPrzypisaniaMini_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Tools.OnClick(e.Item, "trLine", "SelectButton");
            }
        }

        protected void lvPrzypisaniaMini_PagePropertiesChanged(object sender, EventArgs e)
        {
            lvPrzypisaniaMini.SelectedIndex = -1;
            TriggerSelected();
        }
    }
}