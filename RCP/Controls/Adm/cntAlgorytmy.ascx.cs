using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls.Adm
{
    public partial class cntAlgorytmy: System.Web.UI.UserControl
    {
        private const bool strefaVisible = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(ListView1, 0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //----------------------------------
        public bool GetCurrent(out string algorytm, /*out string strefa, */ out int wymiarCzasu, out int przWliczSec, out int przNiewliczSec, out string oddo)
        {
            string data = DateTime.Today.ToStringDb();
            DataRow dr = db.getDataRow(String.Format(@"
select top 1 A.Nazwa, null, --S.Nazwa, 
R.Od, R.Do,
R.WymiarCzasu, R.PrzerwaWliczona, R.PrzerwaNiewliczona
from PracownicyParametry R 
left outer join Kody A on A.Kod = R.RcpAlgorytm
--left outer join Strefy S on S.Id = R.RcpStrefaId
where R.IdPracownika = {0} and {1} between R.Od and ISNULL(R.Do, '20990909')
                ", PracId, data));
            if (dr != null)
            {
                algorytm = db.getValue(dr, 0);
                //strefa = db.getValue(dr, 1);
                oddo = Tools.GetOdDo(dr[2], dr[3]);
                wymiarCzasu = db.getInt(dr, 4, 28800);
                przWliczSec = db.getInt(dr, 5, 0);
                przNiewliczSec = db.getInt(dr, 6, 0);
                return true;
            }
            else
            {
                algorytm = null;
                //strefa = null;
                oddo = null;
                wymiarCzasu = 28800;
                przWliczSec = 0;
                przNiewliczSec = 0;
                return false;
            }
        }
        //----------------------------------
        protected void ListView1_LayoutCreated(object sender, EventArgs e)
        {

        }

        private void PrepareItem(ListViewItemEventArgs e, bool create)
        {
            bool select, edit, insert;
            int lim = Tools.GetListItemMode(e, ListView1, out select, out edit, out insert);
            switch (lim)
            {
                case Tools.limSelect:
                    if (!create)
                    {
                    }
                    break;
                case Tools.limEdit:
                    if (!create)
                    {
                        DataRowView drv = Tools.GetDataRowView(e);
                        Tools.SelectItem(e.Item, "ddlAlgorytm", drv["RcpAlgorytm"]);
                        if (strefaVisible)
                            Tools.SelectItem(e.Item, "ddlStrefa", drv["RcpStrefaId"]);
                    }
                    break;
                case Tools.limInsert:
                    if (create)
                    {
                        TimeEdit te = e.Item.FindControl("teWymiar") as TimeEdit;
                        if (te != null) te.Seconds = 28800;
                        te = e.Item.FindControl("tePrzerwaWliczona") as TimeEdit;
                        if (te != null) te.Seconds = 0;
                        te = e.Item.FindControl("tePrzerwaNiewliczona") as TimeEdit;
                        if (te != null) te.Seconds = 0;
                    }
                    break;
            }

            if (e.Item.ItemType == ListViewItemType.DataItem || e.Item.ItemType == ListViewItemType.InsertItem)
            {
                Tools.SetControlVisible(e.Item, "tdStrefa", strefaVisible);
            }
        }

        protected void ListView1_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            PrepareItem(e, true);
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            PrepareItem(e, false);
        }

        protected void ListView1_DataBound(object sender, EventArgs e)
        {
            DataPager dp = Tools.Pager(ListView1);
            if (dp != null)
            {
                Tools.SetControlVisible(ListView1, "trPager1", dp.TotalRowCount > dp.PageSize);
                dp.Visible = dp.TotalRowCount > dp.PageSize;
            }

            Tools.SetControlVisible(ListView1, "thStrefa", strefaVisible);
        }
        //----------------------------------
        private bool Validate(ListViewItem item, IOrderedDictionary values, string id)
        {
            return cntKartyRcp.Validate("PracownicyParametry", "RcpAlgorytm", 0, PracId, values["Od"], values["Do"], id, null);
        }
        //----------------------------------
        protected void ListView1_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !Validate(e.Item, e.Values, null);
        }

        protected void ListView1_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !Validate(ListView1.EditItem, e.NewValues, Tools.GetDataKey((ListView)sender, e));
        }
        //------
        protected void ListView1_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            Updated = true;
        }

        protected void ListView1_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            Updated = true;
        }

        protected void ListView1_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            Updated = true;
        }
        //----------------------------------
        public string PracId
        {
            set 
            {
                ViewState["pracid"] = value;
                hidPracId.Value = value;
                Updated = false;
                ListView1.InsertItemPosition = InsertItemPosition.None;
                ListView1.EditIndex = -1;
                ListView1.SelectedIndex = -1;
            }
            get { return Tools.GetStr(ViewState["pracid"]); }
        }

        public bool Updated
        {
            set { ViewState["updated"] = value; }
            get { return Tools.GetBool(ViewState["updated"], false); }
        }

    }
}