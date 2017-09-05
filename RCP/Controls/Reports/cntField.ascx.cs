using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;
using System.ComponentModel;
using AjaxControlToolkit;

namespace HRRcp.Controls.Reports
{
    public partial class cntField : System.Web.UI.UserControl
    {
        public event EventHandler Changed;

        public const int tTextBox       = 1;
        public const int tMultiline     = 2;
        public const int _tDropDownList  = 3;
        public const int tDropDownEdit  = 4;
        public const int tDate          = 5;
        public const int tDateRange     = 6;
        public const int tTitle         = 7;
        public const int tCheckBox      = 8;
        public const int tMultiSelect   = 9;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //base.OnPreRender(e);



            /*
            RcpMasterPage mp = null;
            System.Web.UI.MasterPage m = Page.Master;
            if (m is RcpMasterPage) mp = m as RcpMasterPage;
            else if (m.Master != null)
            {
                m = m.Master;
                if (m is RcpMasterPage) mp = m as RcpMasterPage;
            }
            if (mp != null)
            {
                ToolkitScriptManager tsm = mp.GetToolkitScriptManager();
                if (tsm != null)
                {
                    tsm.RegisterExtenderControl(tbValueFTB, Page);
                }
            }
             */ 
        }

        //protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        //{
        //    ToolkitScriptManager1.RegisterExtenderControl(HoverMenuExtender1, Page);
        //    base.OnPreRender(e);
        //}

        //-----------------------------
        //public void Prepare(int typ, 

        public void Prepare(object rpDataItem)
        {
            const string gflt = "gflt";
            const string fText = "Text";
            const string fValue = "Value";

            DataRowView drv = (DataRowView)rpDataItem;
            int typ = (int)drv["Typ"];
            Typ = typ;
            bool rq = db.getBool(drv["Required"], false);
            bool apost = db.getBool(drv["AutoRefresh"], false);
            string lb1 = drv["Label1"].ToString();
            string tt1 = drv["ToolTip1"].ToString();
            RetPar1 = drv["RetValue1"].ToString();
            RetPar2 = drv["RetValue2"].ToString();

            bool tr = FMode == TMode.tr;
            bool td = tr || FMode == TMode.td;
            bool span = typ == tTitle;
            bool td1 = td && span;
            bool td2 = td && !span;
            ltTr.Visible       = tr;
            ltTrc.Visible      = tr;
            ltTd1.Visible      = td2;
            ltTd1title.Visible = td1;
            ltTd1c.Visible     = td;
            ltTd2.Visible      = td2;
            ltTd2c.Visible     = td2;
            lbLabelTd.Visible  = td;
            lbLabel1.Visible   = !td;
            lbSpace.Visible    = !td;

            if (td)
            {
                lbLabelTd.Text = lb1;
                lbLabelTd.ToolTip = tt1;
            }
            else
            {
                lbLabel1.Text = lb1;
                lbLabel1.ToolTip = tt1;
            }

            switch (typ)
            {
                case tTextBox:
                    tbValue.Visible = true;
                    tbValue.Text = GetInitValue(drv);
                    tbValue.AutoPostBack = apost;
                    rfvValue.Enabled = rq;
                    rfvValue.Visible = rq;
                    break;
                case tMultiline:
                    tbValue.Visible = true;
                    tbValue.Text = GetInitValue(drv);
                    tbValue.AutoPostBack = apost;
                    tbValue.TextMode = TextBoxMode.MultiLine;
                    tbValue.Rows = 3;
                    rfvValue.Enabled = rq;
                    rfvValue.Visible = rq;
                    break;
                case _tDropDownList:
                    ddlValue.Visible = true;
                    string sql = db.getStr(drv["LookupSql"]);
                    if (!String.IsNullOrEmpty(sql))
                    {
                        string sel = GetInitValue(drv);
                        try
                        {
                            DataSet ds = db.getDataSet(PrepareSql(sql));
                            Tools.BindData(ddlValue, ds, fText, fValue, false, sel);
                        }
                        catch (Exception ex)
                        {
                            ddlValue.Items.Add(ex.Message);
                        }
                    }
                    ddlValue.AutoPostBack = apost;
                    rfvValue.Enabled = rq;
                    rfvValue.Visible = rq;
                    rfvValue.ControlToValidate = ddlValue.ID;
                    break;
                case tMultiSelect:
                    msValue.Visible = true;
                    sql = db.getStr(drv["LookupSql"]);
                    if (!String.IsNullOrEmpty(sql))
                    {
                        string sel = GetInitValue(drv);
                        try
                        {
                            DataSet ds = db.getDataSet(PrepareSql(sql));
                            Tools.BindData(msValue, ds, fText, fValue, false, sel);
                        }
                        catch (Exception ex)
                        {
                            msValue.Items.Add(ex.Message);
                        }
                    }
                    msValue.AutoPostBack = apost;
                    rfvValue.Enabled = rq;
                    rfvValue.Visible = rq;
                    rfvValue.ControlToValidate = msValue.ID;
                    break;
                case tDropDownEdit:
                    paDropDownEdit.Visible = true;
                    tbValue.Visible = true;
                    sql = db.getStr(drv["LookupSql"]);
                    if (!String.IsNullOrEmpty(sql))
                    {
                        string sel = GetInitValue(drv);
                     
                        DataSet ds = db.getDataSet(PrepareSql(sql));
                        Tools.BindData(ddlEditValue, ds, fText, fValue, false, sel);
                        
                        if (!String.IsNullOrEmpty(sel) && ddlEditValue.SelectedItem != null) tbValue.Text = ddlEditValue.SelectedItem.Text;
                        ddlEditValue.Attributes["OnChange"] = String.Format("javascript:ddlUpdateText('{0}','{1}');", ddlEditValue.ClientID, tbValue.ClientID);
                    }
                    tbValue.AutoPostBack = apost;
                    ddlEditValue.AutoPostBack = apost;
                    rfvValue.Enabled = rq;
                    rfvValue.Visible = rq;
                    break;
                case tDate:
                    deValue.Visible = true;
                    deValue.Date = GetInitValueDT(drv);
                    deValue.AutoPostBack = apost;
                    if (rq) deValue.ValidationGroup = gflt;
                    break;
                case tDateRange:
                    paDateRange.Visible = true;
                    GetInitValueDT(drv, deOd, deDo);
                    deOd.AutoPostBack = apost;
                    deDo.AutoPostBack = apost;
                    if (rq)
                    {
                        deOd.ValidationGroup = gflt;
                        deDo.ValidationGroup = gflt;
                    }
                    break;
                case tTitle:
                    //lbValue.Visible = true;                        
                    //lbValue.Text = GetInitValue(drv);
                    //Tools.AddClass(lbValue, "title");
                    break;
                case tCheckBox:
                    cbValue.Visible = true;
                    cbValue.Checked = GetInitValueBool(drv);
                    //cb.Text = 
                    cbValue.AutoPostBack = apost;
                    if (rq) cbValue.ValidationGroup = gflt;
                    break;
            }
        }

        private string nullStrParam(string par)
        {
            if (String.IsNullOrEmpty(par))
                return null;
            else
                return db.strParam(par);
        }

        private bool addParam(SqlDataSource ds, string name, DbType typ, string value)
        {
            foreach(Parameter p in ds.SelectParameters)
                if (p.Name == name)
                {
                    bool b = p.DefaultValue != value;
                    p.DefaultValue = value;
                    return b;
                }
            ds.SelectParameters.Add(new Parameter(name, typ, value));
            return true;
        }

        public bool ApplyTo(SqlDataSource ds)
        {
            int typ = Typ;
            bool b = false;
            switch (typ)
            {
                case tTextBox:
                case tMultiline:
                    if (!String.IsNullOrEmpty(RetPar1)) b = addParam(ds, RetPar1, DbType.String, tbValue.Text);
                    break;
                case _tDropDownList:
                    if (!String.IsNullOrEmpty(RetPar1)) if (addParam(ds, RetPar1, DbType.String, ddlValue.SelectedValue)) b = true;
                    if (!String.IsNullOrEmpty(RetPar2)) if (addParam(ds, RetPar2, DbType.String, ddlValue.SelectedItem.Text)) b = true;
                    break;
                case tMultiSelect:
                    if (!String.IsNullOrEmpty(RetPar1)) if (addParam(ds, RetPar1, DbType.String, msValue.SelectedItems)) b = true;
                    //if (!String.IsNullOrEmpty(RetPar2)) if (addParam(ds, RetPar2, DbType.String, msValue.SelectedItem.Text)) b = true;
                    break;
                case tDropDownEdit:
                    if (!String.IsNullOrEmpty(RetPar1)) b = addParam(ds, RetPar1, DbType.String, tbValue.Text);
                    break;
                case tDate:
                    if (!String.IsNullOrEmpty(RetPar1)) b = addParam(ds, RetPar1, DbType.DateTime, deValue.DateStr);
                    break;
                case tDateRange:
                    if (!String.IsNullOrEmpty(RetPar1)) if (addParam(ds, RetPar1, DbType.DateTime, deOd.DateStr)) b = true;
                    if (!String.IsNullOrEmpty(RetPar2)) if (addParam(ds, RetPar2, DbType.DateTime, deDo.DateStr)) b = true;
                    break;
                case tTitle:
                    break;
                case tCheckBox:
                    if (!String.IsNullOrEmpty(RetPar1)) b = addParam(ds, RetPar1, DbType.Int32, cbValue.Checked ? "1" : "0");
                    break;
            }
            return b;
        }

        public void Clear()
        {
            int typ = Typ;
            switch (typ)
            {
                case tTextBox:
                case tMultiline:
                    tbValue.Text = null;
                    break;
                case _tDropDownList:
                    Tools.SelectItem(ddlValue, null);
                    break;
                case tMultiSelect:
                    Tools.SelectItem(msValue, null);
                    break;
                case tDropDownEdit:
                    tbValue.Text = null;
                    Tools.SelectItem(ddlValue, null);
                    break;
                case tDate:
                    deValue.Date = null;
                    break;
                case tDateRange:
                    deOd.Date = null;
                    deDo.Date = null;
                    break;
                case tTitle:
                    break;
                case tCheckBox:
                    cbValue.Checked = false; //GetDefault;
                    break;
            }
        }

        private void TriggerChanged()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }
        //-----------------------
        private string PrepareSql(string sql)
        {
            // podmiana UserId itp
            //----- zmienne predefiniowane -----
            AppUser user = AppUser.CreateOrGetSession();
            sql = sql.Replace("@UserId", user.Id);//App.User.Id);  cos tu sie wywalalo - jakby za wczesnie i jeszcze nie było zmiennej
            sql = sql.Replace("@KadryId2", db.strParam(user.KadryId2));  // to najpierw
            sql = sql.Replace("@KadryId", db.strParam(user.NR_EW));
            sql = sql.Replace("@Login", db.strParam(user.Login));
            //sql = sql.Replace("@lang", db.strParam(L.Lang));
            sql = sql.Replace("@lang", db.strParam("PL"));
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
        //------------------------
        protected void ddlValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            TriggerChanged();
        }

        protected void msValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            TriggerChanged();
        }
        //------------------------
        public bool EditMode
        {
            set { ViewState["edit"] = value; }
            get { return Tools.GetBool(ViewState["edit"], false); }
        }
        //-----
        public int Typ
        {
            set 
            {
                ViewState["typ"] = value;
            }
            get { return Tools.GetInt(ViewState["typ"], 1); }
        }

        public string Label1
        {
            set { lbLabel1.Text = value; }
            get { return lbLabel1.Text; }
        }

        public string ToolTip1
        {
            set { lbLabel1.ToolTip = value; }
            get { return lbLabel1.ToolTip; }
        }

        public string RetPar1
        {
            set { ViewState["retPar1"] = value; }
            get { return Tools.GetStr(ViewState["retPar1"]); }
        }

        public string RetPar2
        {
            set { ViewState["retPar2"] = value; }
            get { return Tools.GetStr(ViewState["retPar2"]); }
        }


        public string CssClass
        {
            set { paField.Attributes["class"] = value; }
            get { return paField.Attributes["class"]; }
        }

        [Bindable(true, BindingDirection.OneWay)]
        public object Data
        {
            set { Prepare(value); }
        }

        public enum TMode { div, tr, td };
        private TMode FMode = TMode.div;

        public TMode Mode
        {
            set { FMode = value; }
            get { return FMode; }
        }

    }
}