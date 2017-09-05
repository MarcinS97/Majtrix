using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code; 

namespace HRRcp.Controls
{
    public partial class ReadersSelector : System.Web.UI.UserControl
    {
        const int mAllReaders = 1;
        const int mStreafaReaders = 2;
        int FMode = mAllReaders;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetSelectSql();
        }

        protected string GetSelectSql()
        {
            switch (Mode)
            {
                case mAllReaders: 
                    //return "select Id, Id as ReaderId, Nazwa, Zone, InOut from Readers where Id not in (select ReaderId from StrefyReaders where IdStrefy = " + Base.nullParam(StrefaId) + ")";
                    string sr = StrefaReaders;
                    return "select * from Readers " + (String.IsNullOrEmpty(sr) ? "" : "where Id not in (" + sr + ") ") + 
                           "order by Id";
                case mStreafaReaders:
                    //return "select S.Id, S.ReaderId, R.Nazwa, R.Zone, S.InOut from StrefyReaders S left outer join Readers R on S.ReaderId = R.Id where S.IdStrefy = " + Base.nullParam(StrefaId); 
                    sr = StrefaReaders;
                    if (String.IsNullOrEmpty(sr))
                        return "select * from Readers where Id is null";  // no data
                    else
                    {
                        string rr = Readers;
                        return "select Id, Name, Zone," +
                               " case " +
                               "    when Id in (" + rr + ") then convert(bit, 0)" +
                               "    else convert(bit, 1)" +
                               " end as InOut" +
                               " from Readers where Id in (" + sr + ")" +
                               " order by Id";
                    }
                default:
                    return SqlDataSource1.SelectCommand;
            }
        }

        public void SetSelectSql()
        {
            SqlDataSource1.SelectCommand = GetSelectSql(); 
        }

        protected string GetInOutText(object inout)
        {
            if (!inout.Equals(DBNull.Value))
                switch (Convert.ToInt32(inout))
                {
                    case 0: return "IN";
                    case 1: return "OUT";
                }
            return "-"; // przejście
        }

        public string GetReadersInOut()
        {
            string ret = null;
            foreach (ListViewItem item in lvReaders.Items)
            {
                HiddenField hid = (HiddenField)item.FindControl("hidId");  // musi być 
                DropDownList ddl = (DropDownList)item.FindControl("ddlInOut");
                if (ddl != null)
                    ret += "," + (ddl.SelectedIndex == 0 ? "" : "-") + hid.Value;
                else
                    ret += "," + hid.Value;
            }
            return String.IsNullOrEmpty(ret) ? null : ret.Substring(1);
        }
        //----------------------------
        protected void lvReaders_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (FMode == mStreafaReaders)
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    DropDownList ddl = (DropDownList)e.Item.FindControl("ddlInOut");
                    Label lb = (Label)e.Item.FindControl("InOutLabel");
                    if (lb != null) lb.Visible = false;
                    if (ddl != null)
                    {
                        ddl.Visible = true;
                        ListViewDataItem ditem = (ListViewDataItem)e.Item;
                        DataRowView rowView = (DataRowView)ditem.DataItem;
                        object o = rowView["InOut"];
                        if (!o.Equals(DBNull.Value))
                            ddl.SelectedValue = (bool)rowView["InOut"] ? "1" : "0";
                    }
                }
        }
        //----------------------------
        public int Mode
        {
            get { return FMode; }
            set { FMode = value; }
        }

        public string Readers
        {
            get { return hidReaders.Value; }
            set { hidReaders.Value = value; }
        }

        public string StrefaReaders
        {
            get { return hidReaders.Value.Replace("-", ""); }
        }

        public ListView List
        {
            get { return lvReaders; }
        }

    }
}