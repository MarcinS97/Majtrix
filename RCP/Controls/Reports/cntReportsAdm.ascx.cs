using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HRRcp.App_Code;
using System.Collections.Specialized;

namespace HRRcp.Controls
{
    public partial class cntReportsAdm : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //Tools.PrepareDicListView(ListView1, 0);
            Tools.PrepareDicListView(ListView1, 3);
            Tools.PrepareSorting(ListView1, 1, 20);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //-----
        private bool Update(Control item, IOrderedDictionary values, bool insert)
        {
            string sql = Tools.GetText(ListView1, "tbSql");
            values["SQL"] = sql;
            return true;
        }

        protected void ListView1_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !Update(e.Item, e.Values, true);
        }

        protected void ListView1_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !Update(ListView1.EditItem, e.NewValues, true);
        }

        protected void ListView1_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.InsertItem)
            {
                Tools.SetText2(ListView1, "tbSql", null);
            }
        }
        //------------------
        protected void ListView1_DataBinding(object sender, EventArgs e)
        {
        }

        protected void ListView1_DataBound(object sender, EventArgs e)
        {

        }

        private string GetSql(int itemIndex)
        {
            return Tools.GetText(ListView1.Items[itemIndex], "hidSql");
        }

        private void SetSqlEditor(bool visible, bool enabled, bool set, string sql)
        {
            Tools.SetControlVisible(ListView1, "trSql", visible);
            TextBox tbSql = ListView1.FindControl("tbSql") as TextBox;
            if (tbSql != null)
            {
                if (visible)
                {
                    tbSql.ReadOnly = !enabled;
                    if (set) tbSql.Text = sql;
                }
                else
                {
                    tbSql.ReadOnly = true;
                    tbSql.Text = null;
                }
            }
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListViewItemType.DataItem:
                    CheckBox cb = e.Item.FindControl("cbSelect") as CheckBox;
                    Button bt = e.Item.FindControl("btSelect") as Button;
                    if (cb != null && bt != null)
                    {
                        cb.Visible = true;
                        cb.Attributes["onclick"] = String.Format("javascript:doClick('{0}');", bt.ClientID);
                    }
                    //-----
                    /*
                    int idx = ((ListViewDataItem)e.Item).DisplayIndex;
                    if (idx == ListView1.SelectedIndex || idx == ListView1.EditIndex)
                        SetSqlEditor(true, idx == ListView1.EditIndex, Tools.GetText(e.Item, "hidSql"));
                    */ 
                    break;
                case ListViewItemType.InsertItem:
                    //SetSqlEditor(true, true, null);
                    break;
                case ListViewItemType.EmptyItem:
                    break;
            }
        }
        //-----
        /*
        private TextBox tbSql
        {
            get { return ListView1.FindControl("tbSql") as TextBox; }
        }

        
        private void ShowSqlEditor(ListViewItem item, bool visible, bool editable, bool set)
        {
            Tools.SetControlVisible(ListView1, "trSql", visible);
            TextBox tbSql = ListView1.FindControl("tbSql") as TextBox;
            if (tbSql != null) 
            {
                tbSql.ReadOnly = !editable;
                if (set)
                    tbSql.Text = Tools.GetText(item, "hidSql");
            }
        }
        */
        protected void ListView1_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            if (ListView1.EditIndex != -1 || ListView1.InsertItemPosition != InsertItemPosition.None)
            {
                //e.Cancel = true;
                //Tools.ShowMessage("Proszę zakończyć edycję");  // usunąć jeszcze zaznaczenie wlasnie kliknietegi checkboxa od selected
                ListView1.EditIndex = -1;
                ListView1.InsertItemPosition = InsertItemPosition.None;
            }
        }

        protected void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool v = ListView1.SelectedIndex != -1;
            SetSqlEditor(v, false, true, GetSql(ListView1.SelectedIndex));
            
            
            /*
            Tools.SetControlVisible(ListView1, "trSql", v);
            //HtmlTableRow tr = ListView1.FindControl("trSql") as HtmlTableRow;
            
            TextBox tbSql = ListView1.FindControl("tbSql") as TextBox;
            //HiddenField hid = ListView1.Items[ListView1.SelectedIndex].FindControl("hidSql") as HiddenField;

            if (tbSql != null)
            {
                tbSql.Text = v ? Tools.GetText(ListView1.Items[ListView1.SelectedIndex], "hidSql") : null;
                tbSql.ReadOnly = true;
            }
            */ 
        }

        private TextBox _EnableSqlEditing(bool enable)
        {
            TextBox tbSql = ListView1.FindControl("tbSql") as TextBox;
            if (tbSql != null) tbSql.ReadOnly = !enable;
            return tbSql;
        }
        
        protected void ListView1_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            ListView1.SelectedIndex = e.NewEditIndex;
            SetSqlEditor(true, true, true, GetSql(e.NewEditIndex));

            //ShowSqlEditor(ListView1.Items[e.NewEditIndex], true, true, true);
        }

        protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "NewRecord":
                    SetSqlEditor(true, true, true, null);

                    /*
                    TextBox tbSql = EnableSqlEditing(true);
                    if (tbSql != null) tbSql.Text = null;
                    */ 
                    break;

            }
        }

        protected void ListView1_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            SetSqlEditor(true, false, true, GetSql(e.ItemIndex));

            //EnableSqlEditing(false);
        }
        //-----
        protected void ListView1_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            SetSqlEditor(true, false, false, null);

            //ShowSqlEditor(true, false);
            //EnableSqlEditing(false);
        }

        protected void ListView1_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            SetSqlEditor(true, false, false, null);

            //ShowSqlEditor(true, false);
            //EnableSqlEditing(false);
        }

        protected void ListView1_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            bool s = ListView1.SelectedIndex != -1;
            string sql = s ? GetSql(ListView1.SelectedIndex) : null;
            SetSqlEditor(s, false, true, sql);
        }
        //-----
        protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView1.DataBind();
        }

        protected void SqlDataSource3_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {

        }

    }
}