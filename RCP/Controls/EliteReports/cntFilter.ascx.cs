using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

using System.Configuration;
using HRRcp.Controls;

namespace HRRcp.Controls.EliteReports
{
    public abstract class FilterControl
    {
        public abstract void Bind();
        public abstract String GetId();
        public abstract String GetFilter(String Token);
        public abstract String GetValue();
        public abstract void ClearFilter();
    }
    public class DropDownList_F : FilterControl
    {
        private DropDownList DDL;
        private String ID;
        private Boolean Choose;
        public DropDownList_F(DropDownList DdlParam, String Id, String Command, String ValueField, String TextField, Boolean ChooseString, EventHandler SelectedIndexChanged)
        {
            DDL = DdlParam;
            DDL.DataValueField = ValueField;
            DDL.DataTextField = TextField;
            SqlDataSource ds = new SqlDataSource(ConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString, Command);
            
            DDL.DataBound += new EventHandler(DDLFilter_DataBound);
            DDL.DataSource = ds;
            if (SelectedIndexChanged != null)
            {
                DDL.SelectedIndexChanged += new EventHandler(SelectedIndexChanged);
                DDL.AutoPostBack = true;
            }
            DDL.Visible = true;
            Choose = ChooseString;
            ID = Id;
        }
        public DropDownList_F(DropDownList DdlParam, String Id, String Command, String ValueField, String TextField, Boolean ChooseString)
        {
            DDL = DdlParam;
            DDL.DataValueField = ValueField;
            DDL.DataTextField = TextField;
            SqlDataSource ds = new SqlDataSource(ConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString, Command);
            DDL.DataBound += new EventHandler(DDLFilter_DataBound);
            DDL.DataSource = ds;
            DDL.Visible = true;
            Choose = ChooseString;
            ID = Id;
        }
        public override void Bind()
        {
            DDL.DataBind();
        }
        protected void DDLFilter_DataBound(object sender, EventArgs e)
        {
            if(Choose)
                DDL.Items.Insert(0, new ListItem(L.p("Wybierz..."), String.Empty));
        }
        public override String GetId()
        {
            return ID;
        }
        public override String GetFilter(String Token)
        {
            if (String.IsNullOrEmpty(DDL.SelectedValue))
                return String.Empty;
            return String.Format("{0}='{1}'", Token, DDL.SelectedValue);
        }
        public override String GetValue()
        {
            return DDL.SelectedValue;
        }
        public override void ClearFilter()
        {
            DDL.SelectedIndex = 0;
        }
    }
    public class DateEdit_F : FilterControl
    {
        private DateEdit DE;
        private String ID;
        public DateEdit_F(DateEdit DeParam, String Id)
        {
            DE = DeParam;
            DE.Visible = true;

            ID = Id;
        }
        public override void Bind()
        {

        }
        public override String GetId()
        {
            return ID;
        }
        public override String GetFilter(String Token)
        {
            if (DE.Date == null)
                return String.Empty;
            return String.Format("{0}='{1}'", Token, DE.Date);
        }
        public override string GetValue()
        {
            return null;//return DE.DateStr;
        }
        public override void ClearFilter()
        {
            DE.Date = null;
        }
    }
    public class DateEditRange_F : FilterControl
    {
        private DateEdit DE1, DE2;
        private String ID;
        public DateEditRange_F(DateEdit De1Param, DateEdit De2Param, String Id)
        {
            DE1 = De1Param;
            DE2 = De2Param;
            DE1.Visible = true;
            DE2.Visible = true;

            ID = Id;
        }
        public override void Bind()
        {

        }
        public override String GetId()
        {
            return ID;
        }
        public override String GetFilter(String Token)
        {
            if (DE1.Date == null && DE2.Date == null)
                return String.Empty;
            if (DE1.Date == null)
                return String.Format("{0} >= '{1}' and {0} <= '{2}'", Token, DateTime.MinValue, DE2.Date);
            if (DE2.Date == null)
                return String.Format("{0} >= '{1}' and {0} <= '{2}'", Token, DE1.Date, DateTime.MaxValue);
            //jak nie ma nulli : DDDDD
            return String.Format("{0} >= '{1}' and {0} <= '{2}'", Token, DE1.Date, DE2.Date);
        }
        public override string GetValue()
        {
            return null;//throw new NotImplementedException();
        }
        public override void ClearFilter()
        {
            DE1.Date = null;
            DE2.Date = null;
        }
    }
    public class TextBox_F : FilterControl
    {
        private TextBox TB;
        private String ID;
        public TextBox_F(TextBox TbParam, String Id)
        {
            TB = TbParam;
            TB.Visible = true;

            ID = Id;
        }
        public override void Bind()
        {
            //throw new NotImplementedException();
        }
        public override String GetId()
        {
            return ID;
        }
        public override String GetFilter(String Token)
        {
            if (String.IsNullOrEmpty(TB.Text))
                return String.Empty;

            return String.Format("{0} like '%{1}%'", Token, TB.Text);

        }
        public override string GetValue()
        {
            return null;//throw new NotImplementedException();
        }
        public override void ClearFilter()
        {
            TB.Text = String.Empty;
        }

    }
    public class CheckBox_F : FilterControl
    {
        private CheckBox CB;
        private String ID;
        public CheckBox_F(CheckBox CbParam, String Id)
        {
            CB = CbParam;
            CB.Visible = true;

            ID = Id;
        }
        public override String GetId()
        {
            return ID;
        }
        public override void Bind()
        {
            //throw new NotImplementedException();
        }
        public override String GetFilter(String Token)
        {
            return String.Format("{0} = {1}", Token, CB.Checked ? 1 : 0);
        }
        public override string GetValue()
        {
            return null;//throw new NotImplementedException();
        }

        public override void ClearFilter()
        {
            CB.Checked = false;
        }
    }

    public partial class cntFilter : System.Web.UI.UserControl
    {
        protected FilterControl Filter;
        private const String _DDL = "DropDownList";
        private const String _DE = "DateEdit";
        private const String _TB = "TextBox";
        private const String _CB = "CheckBox";
        private const String _DERANGE = "DateEditRange";
        private const String _FILTER = "__FILTER";
        private const String _COMMAND = "__COMMAND";
        private const String _VALUEFIELD = "__VALUEFIELD";
        private const String _TEXTFIELD = "__TEXTFIELD";
        private const String _TOKEN = "__TOKEN";
        private const String _TYPE = "__TYPE";
        private const String _REPORTID = "__REPORTIDFILTER";
        private const String _POSTBACKBUTTONID = "__POSTBACKBUTTONID";
        private const String _CHOOSESTRING = "__CHOOSESTRING";

        protected override void OnLoad(EventArgs e)
        {
            bool OnButtonClick = false;
            if (!String.IsNullOrEmpty(PostBackButtonId))
            {
                Button btn = this.Parent.FindControl(PostBackButtonId) as Button;
                if (btn != null)
                {
                    btn.Click += new EventHandler(UpdateFilter);
                    OnButtonClick = true;
                }
            }
            if (PostBackButtonId == "no_button") OnButtonClick = true;
            switch (Type)
            {
                case _DDL:
                    if (!OnButtonClick) //czy autopostback czy nie
                        Filter = new DropDownList_F(ddlMain, ControlId(_DDL), Command, ValueField, TextField, ChooseString, UpdateFilterPostBack);
                    else
                        Filter = new DropDownList_F(ddlMain, ControlId(_DDL), Command, ValueField, TextField, ChooseString);
                    break;
                case _DE:
                    Filter = new DateEdit_F(deMain, ControlId(_DE));
                    break;
                case _DERANGE:
                    Filter = new DateEditRange_F(deMain, deMain2, ControlId(_DERANGE));
                    break;
                case _TB:
                    Filter = new TextBox_F(tbMain, ControlId(_TB));
                    break;
                case _CB:
                    Filter = new CheckBox_F(cbMain, ControlId(_CB));
                    break;
            }
            base.OnLoad(e);
        }
        public void Clear()
        {
            Filter.ClearFilter();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                if (Filter != null) Filter.Bind();
        }
        protected void UpdateFilter(object sender, EventArgs e)
        {
            cntReport report = this.Parent.FindControl(ReportId) as cntReport;
            report.UpdateFilters(Filter.GetId(), GetFilter(), false);
        }
        protected void UpdateFilterPostBack(object sender, EventArgs e)
        {
            cntReport report = this.Parent.FindControl(ReportId) as cntReport;
            report.UpdateFilters(Filter.GetId(), GetFilter(), true);
        }
        public String GetFilter()
        {
            return Filter.GetFilter(Token);
        }
        public String GetValue()
        {
            return Filter.GetValue();
        }
        public String Command
        {
            get { return ViewState[_COMMAND] as String; }
            set { ViewState[_COMMAND] = value; }
        }
        public String ValueField
        {
            get { return ViewState[_VALUEFIELD] as String; }
            set { ViewState[_VALUEFIELD] = value; }
        }
        public String TextField
        {
            get { return ViewState[_TEXTFIELD] as String; }
            set { ViewState[_TEXTFIELD] = value; }
        }
        public String Token
        {
            get { return ViewState[_TOKEN] as String; }
            set { ViewState[_TOKEN] = value; }
        }
        public String Type
        {
            get { return ViewState[_TYPE] as String; }
            set { ViewState[_TYPE] = value; }
        }
        public String ReportId
        {
            get { return ViewState[_REPORTID] as String; }
            set { ViewState[_REPORTID] = value; }
        }
        public String PostBackButtonId
        {
            get { return ViewState[_POSTBACKBUTTONID] as String; }
            set { ViewState[_POSTBACKBUTTONID] = value; }
        }
        public String Label
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    lblFirst.Visible = true;
                    lblFirst.Text = value;
                }
                else
                    lblFirst.Visible = false;
            }
        }
        public String Label2
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    lblSecond.Visible = true;
                    lblSecond.Text = value;
                }
                else
                    lblSecond.Visible = false;
            }
        }
        public String ControlId(String type)
        {
            return this.ID + "_" + type;
        }
        public Boolean ChooseString
        {
            get
            {
                try
                {
                    return (Boolean)ViewState[_CHOOSESTRING];
                }
                catch
                {
                    return true;
                }
            }
            set { ViewState[_CHOOSESTRING] = value; }
        }


    }
}