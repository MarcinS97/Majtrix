using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls.Adm
{
    public partial class cntStanowiskaAdm : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvStanowiska, 0);
            Tools.PrepareSorting(lvStanowiska, 1, 8);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lvStanowiska_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            string id = Tools.GetDataKey(lvStanowiska, e);
            DataRow dr = db.getDataRow("select top 1 Id from Pracownicy where IdStanowiska = " + id);
            if (dr != null) Tools.ShowError("Usunięcie niemożliwe. Istnieją pracownicy mający przypisane stanowisko.");
            else
            {
                dr = db.getDataRow("select top 1 IdOkresu from PracownicyOkresy where IdStanowiska = " + id);
                if (dr != null) Tools.ShowError("Usunięcie niemożliwe. Istnieją pracownicy mający przypisane stanowisko (ARCHIWUM).");
            }
            e.Cancel = dr != null;
        }

        protected void lvStanowiska_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.InsertItem)
            {
                Tools.SetChecked(e.Item, "AktywneCheckBox", true);
            }
        }

        protected void lvStanowiska_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem && lvStanowiska.EditIndex != -1)
            {
                if (lvStanowiska.EditIndex == Tools.GetDisplayIndex(e))
                {
                    DataRowView drv = Tools.GetDataRowView(e);
                    string dzial = drv["IdDzialu"].ToString();
                    
                    Tools.SelectItem(e.Item, "ddlDzial", dzial);
                }
            }
        }
    }
}