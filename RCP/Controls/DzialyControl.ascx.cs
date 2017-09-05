using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class DzialyControl : System.Web.UI.UserControl
    {
        public event EventHandler Changed;

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvDzialy, 0);
            Tools.PrepareSorting(lvDzialy, 1, 8);
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string GetStatus(object status)
        {
            if(status == null) {
                status = 0;
            }
            string st = status.ToString();
            switch (st)
            {
                case "-1": return "Archiwum";
                case "0": return "";
                case "1": return "Nowy";
                default: return st;
            }
        }

        public string GetSortHeader(params string[] par)
        {
            const int linecnt = 1;
            int cnt = par.Count() / linecnt;
            string[] th = new string[cnt];
            for (int i = 0; i < cnt; i++)
            {
                int idx = i * linecnt;
                th[i] = par[idx];
            }
            return String.Join("</th><th>", th);
        }

        public void GetSortHeader(PlaceHolder ph, params string[] par)
        {
            const int linecnt = 1;
            int cnt = par.Count() / linecnt;
            string[] th = new string[cnt];
            for (int i = 0; i < cnt; i++)
            {
                int idx = i * linecnt;
                th[i] = par[idx];
            }
        }

        private void TriggerChanged()
        {
            cntStanowiskaAdm1.DataBind();
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }

        protected void lvDzialy_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            string id = Tools.GetDataKey(lvDzialy, e);
            DataRow dr = db.getDataRow("select top 1 Id from Pracownicy where IdDzialu = " + id);
            if (dr != null) Tools.ShowError("Usunięcie niemożliwe. Istnieją pracownicy przypisani do działu.");            
            else
            {
                dr = db.getDataRow("select top 1 IdOkresu from PracownicyOkresy where IdDzialu = " + id);
                if (dr != null) Tools.ShowError("Usunięcie niemożliwe. Istnieją pracownicy przypisani do działu (ARCHIWUM).");
                else
                {
                    dr = db.getDataRow("select top 1 Id from Stanowiska where IdDzialu = " + id);
                    if (dr != null) Tools.ShowError("Usunięcie niemożliwe. Istnieją stanowiska w dziale.");
                }
            }
            e.Cancel = dr != null;
        }

        private bool Update(ListViewItem item, IOrderedDictionary values)
        {
            CheckBox cb = item.FindControl("cbAktywny") as CheckBox;
            if (cb != null)
                values["Status"] = cb.Checked ? 0 : -1;
            values["KierStrefaId"] = Tools.GetDdlSelectedValueInt(item, "ddlKierStrefa");
            values["KierAlgorytm"] = Tools.GetDdlSelectedValueInt(item, "ddlKierAlgorytm");
            values["PracStrefaId"] = Tools.GetDdlSelectedValueInt(item, "ddlPracStrefa");
            values["PracAlgorytm"] = Tools.GetDdlSelectedValueInt(item, "ddlPracAlgorytm");
            return true;
        }

        protected void lvDzialy_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !Update(lvDzialy.EditItem, e.NewValues);
        }

        protected void lvDzialy_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !Update(e.Item, e.Values);
        }

        protected void lvDzialy_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            TriggerChanged();
        }

        protected void lvDzialy_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            TriggerChanged();
        }

        protected void lvDzialy_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            TriggerChanged();
        }

        //----- sortowanie -----

        /*
        const string sesSortId = "_sort";
        const int maxSortCol = 8;
        private int FDefSortColumn = 1;

        protected void lvDzialy_LayoutCreated(object sender, EventArgs e)
        {
            int sort = Sort;
            Report.ShowSort(lvDzialy, sort, sort > 0);
        }

        protected void lvDzialy_Sorting(object sender, ListViewSortEventArgs e)
        {
            int sort;
            Report.ShowSort(sender, e, maxSortCol, FDefSortColumn, out sort);
            Sort = sort;  // unikalne co do kontrolki
        }

        public int Sort
        {
            get
            {
                object s = Session[lvDzialy.ClientID + sesSortId + sesSortId];
                //object s = ViewState[lvDzialy.ClientID + sesSortId];
                if (s != null)
                {
                    int sort = (int)s;
                    if (1 <= sort && sort <= maxSortCol)
                        return sort;
                }
                return FDefSortColumn;
            }
            set 
            {
                Session[lvDzialy.ClientID + sesSortId + sesSortId] = value;
                //ViewState[lvDzialy.ClientID + sesSortId + sesSortId] = value;
            }
        }

        public int DefSort
        {
            get { return FDefSortColumn; }
            set { FDefSortColumn = value; }
        }
        //----------------------
 
        */


    }
}