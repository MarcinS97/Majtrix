using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls.Reports
{
    public partial class cntFilter : System.Web.UI.UserControl
    {
        const int tTextBox      = 1;
        const int tMultiline    = 2;
        const int tDropDownList = 3;
        const int tDropDownEdit = 4;
        const int tDate         = 5;
        const int tDateRange    = 6;
        const int tTitle        = 7;
        const int tCheckBox     = 8;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btEdit.Visible = App.User.HasRight(AppUser.rSuperuser);
            }
        }
        //-----------------------
        private string PrepareSql(string sql)
        {
            // podmiana UserId itp

            return sql;
        }
        //-----------------------
        protected bool IsTyp(object data, params int[] typy)
        {
            DataRowView drv = (DataRowView)data;
            int typ = (int)drv["Typ"];
            return typy.Any(a => a == typ);
        }

        protected bool IsSql(ref string sql)
        {
            string s = Tools.Substring(sql.TrimStart(), 0, 10).ToLower();
            if (s.StartsWith("sql:"))
            {
                sql = sql.TrimStart().Substring(4);
                return true;
            }
            else
                return sql.StartsWith("select") || sql.StartsWith("declare");
        }

        protected string GetInitValue(DataRowView drv)
        {
            int id = (int)drv["Id"];
            string val = drv["InitValue"].ToString();
            if (IsSql(ref val))
            {
                DataRow dr = db.getDataRow(PrepareSql(val));
                return db.getValue(dr, 0);
            }
            else
                return val;
            /*
            cachować w ViewState lub znacznik czy ustawione, 
            druga wartość dla Range
            */
        }

        public static bool Equals(string s, params string[] par)
        {
            return par.Any(a => a == s);
        }

        protected Boolean GetInitValueBool(DataRowView drv)
        {
            int id = (int)drv["Id"];
            string val = drv["InitValue"].ToString();
            if (IsSql(ref val))
            {
                DataRow dr = db.getDataRow(PrepareSql(val));
                return db.getBool(dr, 0, false);
            }
            else
            {
                string v = val.ToLower();
                return Equals(v, "1", "true", "tak", "t", "yes", "y");
            }
        }

        protected DateTime? GetInitValueDT(DataRowView drv)
        {
            int id = (int)drv["Id"];
            string val = drv["InitValue"].ToString();
            if (IsSql(ref val))
            {
                DataRow dr = db.getDataRow(PrepareSql(val));
                return db.getDateTime(dr, 0);
            }
            else
                return Tools.StrToDateTime(val);
        }

        protected void GetInitValueDT(DataRowView drv, DateEdit de1, DateEdit de2)
        {
            int id = (int)drv["Id"];
            string val = drv["InitValue"].ToString();
            if (IsSql(ref val))
            {
                DataRow dr = db.getDataRow(PrepareSql(val));
                de1.Date = db.getDateTime(dr, 0);
                if (dr.ItemArray.Length >= 1)
                    de2.Date = db.getDateTime(dr, 1);
            }
        }
            
        protected string GetCss(object data)
        {
            DataRowView drv = (DataRowView)data;
            string css = db.getStr(drv["CssClass"]);
            return String.IsNullOrEmpty(css) ? null : " " + css; 
        }

        private bool IsRequired(DataRowView drv)
        {
            return db.getBool(drv["Required"], false);
        }

        //-------------------------------
        protected void rpFilter_DataBinding(object sender, EventArgs e)
        {

        }

        protected void rpFilter_ItemCreated(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void rpFilter_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            const string flt = "flt";
            const string tbValue = "tbValue";
            const string rfvValue = "rfvValue";
            const string Text = "Text";
            const string Value = "Value";


            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                int typ = (int)drv["Typ"];
                bool rq = db.getBool(drv["Required"], false);
                bool apost = db.getBool(drv["AutoRefresh"], false);

                //HtmlGenericControl item = e.Item.FindControl();
                //Label lb1 = e.Item.FindControl("Label1") as Label;
                //Label lb2 = e.Item.FindControl("Label2") as Label;
                //Label lb3 = e.Item.FindControl("Label3") as Label;

                switch (typ)
                {
                    case tTextBox:
                        TextBox tb = Tools.SetControlVisible(e.Item, tbValue, true) as TextBox;
                        RequiredFieldValidator rfv = Tools.SetControlEnabled(e.Item, "rfvValue", rq) as RequiredFieldValidator;
                        tb.AutoPostBack = apost;
                        tb.Text = GetInitValue(drv);
                        break;
                    case tMultiline:
                        tb = Tools.SetControlVisible(e.Item, tbValue, true) as TextBox;
                        rfv = Tools.SetControlEnabled(e.Item, rfvValue, rq) as RequiredFieldValidator;
                        tb.TextMode = TextBoxMode.MultiLine;
                        tb.Rows = 3;
                        tb.AutoPostBack = apost;
                        tb.Text = GetInitValue(drv);
                        break;
                    case tDropDownList:
                        DropDownList ddl = Tools.SetControlVisible(e.Item, "ddlValue", true) as DropDownList;
                        rfv = Tools.SetControlEnabled(e.Item, rfvValue, rq) as RequiredFieldValidator;
                        rfv.ControlToValidate = ddl.ID;
                        string sql = db.getStr(drv["LookupSql"]);
                        if (!String.IsNullOrEmpty(sql))
                        {
                            string sel = GetInitValue(drv);
                            DataSet ds = db.getDataSet(PrepareSql(sql));
                            Tools.BindData(ddl, ds, Text, Value, false, sel);
                        }
                        ddl.AutoPostBack = apost;
                        break;
                    case tDropDownEdit:
                        Tools.SetControlVisible(e.Item, "paDropDownEdit", true);
                        rfv = Tools.SetControlEnabled(e.Item, rfvValue, rq) as RequiredFieldValidator;
                        tb = Tools.SetControlVisible(e.Item, tbValue, true) as TextBox;
                        ddl = e.Item.FindControl("ddlEditValue") as DropDownList;
                        sql = db.getStr(drv["LookupSql"]);
                        if (!String.IsNullOrEmpty(sql))
                        {
                            String sel = GetInitValue(drv);
                            DataSet ds = db.getDataSet(PrepareSql(sql));
                            Tools.BindData(ddl, ds, Text, Value, false, sel);
                            if (ddl.SelectedItem != null) tb.Text = ddl.SelectedItem.Text;
                        }
                        tb.AutoPostBack = apost;
                        ddl.AutoPostBack = apost;
                        break;
                    case tDate:
                        DateEdit de = Tools.SetControlVisible(e.Item, "deValue", true) as DateEdit;
                        if (rq) de.ValidationGroup = flt;
                        de.Date = GetInitValueDT(drv);
                        de.AutoPostBack = apost;
                        break;
                    case tDateRange:
                        Tools.SetControlVisible(e.Item, "paDateRange", true);
                        object d1, d2;
                        DateEdit de1 = e.Item.FindControl("deOd") as DateEdit;
                        DateEdit de2 = e.Item.FindControl("deDo") as DateEdit;
                        GetInitValueDT(drv, de1, de2);
                        if (rq)
                        {
                            de1.ValidationGroup = flt;
                            de2.ValidationGroup = flt;
                        }
                        de1.AutoPostBack = apost;
                        de2.AutoPostBack = apost;
                        break;
                    case tTitle:                        
                        Label lb = Tools.SetControlVisible(e.Item, "lbValue", true) as Label;
                        lb.Text = GetInitValue(drv);
                        break;
                    case tCheckBox:
                        CheckBox cb = Tools.SetControlVisible(e.Item, "cbValue", true) as CheckBox;
                        cb.Checked = GetInitValueBool(drv);
                        //cb.Text = 
                        cb.AutoPostBack = apost;
                        break;
                }
            }
        }

        protected void rpFilter_Load(object sender, EventArgs e)
        {

        }
        //----------------------------
        protected void btWyszukaj_Click(object sender, EventArgs e)
        {
            // po prostu robi postback
        }

        protected void btClear_Click(object sender, EventArgs e)
        {
            foreach (Control cnt in paFilter.Controls)
            {
                if (cnt is TextBox) (cnt as TextBox).Text = null;
                else if (cnt is DateEdit) (cnt as DateEdit).Date = null;
                else if (cnt is DropDownList) Tools.SelectItem(cnt as DropDownList, null);
                else if (cnt is CheckBox) (cnt as CheckBox).Checked = false;
            }
        }

        protected void btEdit_Click(object sender, EventArgs e)
        {
            // edycja
        }

    }
}