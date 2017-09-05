using AjaxControlToolkit;
using HRRcp.App_Code;
using HRRcp.BadaniaWstepne.Controls;
using HRRcp.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace HRRcp.BadaniaWstepne.Templates
{
    public abstract class CrTemplateCreator : ICrTemplateCreator
    {
        public abstract IndexedBasicTemplate.DCreateFunc CreateTemplate(CrTemplateArgs Args);
        public virtual IndexedBasicTemplate.DCreateFunc CreateTemplateReadOnly(CrTemplateArgs Args)
        {
            return CreateTemplate(Args);
        }
        public virtual IndexedBasicTemplate.DCreateFunc CreateValidator(CrTemplateArgs Args)
        {
            return null;
        }
        static public string ToSqlValue(object Value)
        {
            if (Value == null || (Value is string && string.IsNullOrEmpty((string)Value)))
                return "NULL";
            return ToSqlValue(Value, Value.GetType());
        }
        static public string ToSqlValue(object Value, Type type)
        {
            if (Value == null || (Value is string && string.IsNullOrEmpty((string)Value)))
                return "NULL";
            
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return Value.ToString();
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    


                    return Value.ToString().Replace(",", ".");
                case TypeCode.String:
                    return string.Format("'{0}'", Value);
                case TypeCode.DateTime:
                    return "'" + Tools.DateToStr((DateTime)Value) + "'";
                case TypeCode.Boolean:
                    return ((bool)Value) ? "1" : "0";
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        return ToSqlValue(Value, Nullable.GetUnderlyingType(type));
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
        public class UpdInsArgs
        {
            public CrColumn Column { get; private set; }
            public CrTemplate Template { get; private set; }

            public UpdInsArgs(CrColumn Column, CrTemplate Template)
            {
                this.Column = Column;
                this.Template = Template;
            }

            public ICrTemplateCreator Creator
            {
                get { return Template.Creator; }
            }
        }
        public class UpdInsArgsCnt : UpdInsArgs
        {
            public Control Container { get; private set; }
            public UpdInsArgsCnt(CrColumn Column, CrTemplate Template, Control Container)
                : base(Column, Template)
            {
                this.Container = Container;
            }

            public object Value
            {
                get { return Template.ValueGetter(Container); }
                set { Template.ValueSetter(Container, value); }
            }
            public string SqlValue
            {
                get { return ToSqlValue(Template.ValueGetter(Container)); }
            }
        }
        public virtual void SetControlValue(UpdInsArgsCnt Args, DataRowView Item)
        {
            var Value = Item[Args.Column.SqlName];
            Args.Template.ValueSetter(Args.Container, (Value is DBNull) ? null : Value);
        }
        public virtual void Updating(UpdInsArgsCnt Args, IDictionary Values)
        {
            if (!Args.Template.isReadOnly)
            {
                string Column = Args.Column.SqlName;
                object Value = Args.Template.ValueGetter(Args.Container);
                Values.Add(Column, ToSqlValue(Value));
            }
        }
        public virtual void Updated(UpdInsArgsCnt Args, ListViewUpdatedEventArgs e) { }
        public virtual void Inserting(UpdInsArgsCnt Args, IDictionary Values)
        {
            if (!Args.Template.isReadOnly)
            {
                string Column = Args.Column.SqlName;
                object Value = Args.Template.ValueGetter(Args.Container);
                if (Value != null && (!(Value is string) || !string.IsNullOrEmpty((string)Value)))
                    Values.Add(Column, ToSqlValue(Value));
            }
        }
        public virtual void Inserted(UpdInsArgsCnt Args, DataRow Item) { }
        public virtual void Deleting(UpdInsArgsCnt Args, DataRow Item) { }
        public virtual void Deleted(UpdInsArgsCnt Args, DataRow Item) { }
        public virtual string GetFilterValue(Control container, CrTemplateArgs Args, int Index)
        {
            return "";
        }
        protected Y _FindCnt<Y>(Control cnt, int Index, string text)
            where Y : Control
        {
            return cnt.FindControl(text + Index) as Y;
        }
        protected RequiredFieldValidator _FindReqFieldVal(Control cnt, int Index)
        {
            return _FindCnt<RequiredFieldValidator>(cnt, Index, "rfv");
        }
        protected RequiredFieldValidator CreateReqVieldVal(string cntToVal, int Index)
        {
            RequiredFieldValidator rfv = new RequiredFieldValidator();
            rfv.ID = "rfv" + Index;
            rfv.SetFocusOnError = true;
            rfv.Display = ValidatorDisplay.Dynamic;
            rfv.ValidationGroup = "vg1";
            rfv.ControlToValidate = cntToVal + Index;
            rfv.ErrorMessage = "Błąd";
            rfv.CssClass = "error";
            rfv.Enabled = true;
            return rfv;
        }
        public virtual string Validate(Control container, CrTemplateArgs Args, int Index)
        {
            return null;
        }
        public virtual bool IsEmpty(Control container, CrTemplateArgs Args, int Index)
        {
            return false;
        }

        public static CrTC0_Label CrTC0_Label = new CrTC0_Label();
        public static CrTC1_TextBox CrTC1_TextBox = new CrTC1_TextBox();
        //public static CrTC1_TextBoxNum CrTC1_TextBoxNum = new CrTC1_TextBoxNum();
        
        public static CrTC2_DropDownList CrTC2_DropDownList = new CrTC2_DropDownList();
        public static CrTC3_DDLTextBox CrTC3_DDLTextBox = new CrTC3_DDLTextBox();
        public static CrTC4_DateEdit CrTC4_DateEdit = new CrTC4_DateEdit();
        public static CrTC5_MultiLineTextBox CrTC5_MultiLineTextBox = new CrTC5_MultiLineTextBox();
        public static CrTC6_CheckBox CrTC6_CheckBox = new CrTC6_CheckBox();
        public static CrTC7_DateZakres CrTC7_DateZakres = new CrTC7_DateZakres();
        public static CrTC8_SplityWsp CrTC8_SplityWsp = new CrTC8_SplityWsp();
        public static CrTC9_DDLBOOL CrTC9_DDLBOOL = new CrTC9_DDLBOOL();
        public static CrTC10_DataLabel CrTC10_DataLabel = new CrTC10_DataLabel();
        public static CrTC11_AsyncFileUpload CrTC11_AsyncFileUpload = new CrTC11_AsyncFileUpload();
        public static CrTC12_DateEditZakr CrTC12_DateEditZakr = new CrTC12_DateEditZakr();
    }

    public abstract class CrTemplateCreator<T> : CrTemplateCreator, ICrTemplateCreator<T>
    {
        public abstract T GetValue(Control container, CrTemplateArgs Args, int Index);
        public abstract void SetValue(Control container, CrTemplateArgs Args, int Index, T Value);
        public virtual T GetValueReadOnly(Control container, CrTemplateArgs Args, int Index)
        {
            throw new InvalidOperationException();
        }
        public virtual void SetValueReadOnly(Control container, CrTemplateArgs Args, int Index, T Value)
        {
            SetValue(container, Args, Index, Value);
        }
        public CrTemplate<T> Compile(CrTemplateArgs Args, int Index, Func<string, Control> LoadCnt, bool isReadOnly)
        {
            return new CrTemplate<T>(this, Args, Index, LoadCnt, isReadOnly);
        }
    }

    public class CrTC0_Label : CrTemplateCreator<object>
    {
        public override IndexedBasicTemplate.DCreateFunc CreateTemplate(CrTemplateArgs Args)
        {
            return (a, b, c) =>
            {
                Label l = new Label();
                l.ID = "l" + c;
                a.Controls.Add(l);
            };
        }
        public override object GetValue(Control container, CrTemplateArgs Args, int Index)
        {
            return _FindCnt<Label>(container, Index, "l").Text;
        }
        public override void SetValue(Control container, CrTemplateArgs Args, int Index, object Value)
        {
            if (Args[0] != null && Value != null && Value.ToString().Length > (int)Args[0])
                Value = Value.ToString().Substring(0, (int)Args[0]) + "...";
            _FindCnt<Label>(container, Index, "l").Text = (Value != null) ? Value.ToString() : "";
        }
    }
    

    
    
    
    
    
    
    
    
    
    
    public class CrTC1_TextBox : CrTemplateCreator<object>
    {
        public IndexedBasicTemplate.DCreateFunc CreateTemplateEx(CrTA1_TextBox Args)
        {
            return (a, b, i) =>
            {
                TextBox tb = new TextBox();
                tb.ID = "tb" + i;
                tb.ValidationGroup = "vg1";
                if (Args.MaxLength > 0)
                    tb.MaxLength = Args.MaxLength;
                a.Controls.Add(tb);
                if (!string.IsNullOrEmpty(Args.AllowedChars))
                {
                    FilteredTextBoxExtender ftb = new FilteredTextBoxExtender();
                    ftb.TargetControlID = tb.ID;
                    ftb.ValidChars = (string)Args.AllowedChars;
                    a.Controls.Add(ftb);
                }
            };
        }
        public override IndexedBasicTemplate.DCreateFunc CreateTemplate(CrTemplateArgs Args)
        {
            return CreateTemplateEx((CrTA1_TextBox)(Args));
        }
        public override IndexedBasicTemplate.DCreateFunc CreateTemplateReadOnly(CrTemplateArgs Args)
        {
            return (a, b, c) =>
            {
                TextBox tb = new TextBox();
                tb.ID = "tb" + c;
                tb.Enabled = false;
                a.Controls.Add(tb);
            };
        }
        public override IndexedBasicTemplate.DCreateFunc CreateValidator(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                var reqVal = CreateReqVieldVal("tb", i);
                a.Controls.Add(reqVal);
            };
        }
        public override object GetValue(Control container, CrTemplateArgs Args, int Index)
        {
            TextBox cnt = _FindCnt<TextBox>(container, Index, "tb");
            if (cnt != null)
            {
                object allowed = Args.Args[1];
                if (allowed != null && allowed.ToString().StartsWith("0123456789"))
                    return cnt.Text.Replace(",", ".");       // powinno to być w GetSqlValue, a tu powinno zwracać float/int
            }
            return cnt.Text;

            //return _FindCnt<TextBox>(container, Index, "tb").Text;
        }
        public override void SetValue(Control container, CrTemplateArgs Args, int Index, object Value)
        {
            _FindCnt<TextBox>(container, Index, "tb").Text = (Value != null) ? Value.ToString() : "";
        }
        public override string Validate(Control container, CrTemplateArgs Args, int Index)
        {
            var tb = _FindCnt<TextBox>(container, Index, "tb");
            var args2 = (CrTA1_TextBox)Args;
            if (args2.MaxLength != 0 && tb.Text.Length > args2.MaxLength)
                return "Za długa wartość.";
            if (args2.AllowedChars != null && tb.Text.Any(a => !args2.AllowedChars.Contains(a)))
                return "Wartość zawiera niedozwolone znaki.";
            return null;
        }
        public override bool IsEmpty(Control container, CrTemplateArgs Args, int Index)
        {
            var tb = _FindCnt<TextBox>(container, Index, "tb");
            return string.IsNullOrEmpty(tb.Text);
        }
    }

   
    
    
    
    
    
    
    public class CrTC2_DropDownList : CrTemplateCreator<int?>
    {
        public override IndexedBasicTemplate.DCreateFunc CreateTemplate(CrTemplateArgs Args2)
        {
            return (a, b, i) =>
            {
                CrTA6_DDL Args = (CrTA6_DDL)Args2;
                DropDownList ddl = new DropDownList();
                ddl.ID = "ddl" + i;
                ddl.DataSource = Args.DataSource;//.Cast<DataRow>().Select(c => new { Text = c["Text"], Value = c["Value"].ToString() });
                ddl.AppendDataBoundItems = true;
                ddl.DataBinding += (c, d) =>
                {
                    DropDownList ddl2 = (c as DropDownList);
                    ddl2.Items.Clear();
                    if (Args.canNull)
                    {
                        ddl2.Items.Insert(0, new ListItem("wybierz ...", ""));
                    }
                    if (Args.DefValue != null)
                    {
                        if (ddl2.Items.Count != 0)
                        {
                            if (string.IsNullOrEmpty(ddl2.SelectedValue) && ddl2.SelectedIndex == 0)
                            {
                                if (Args.DefValue is string)
                                {
                                    ddl2.SelectedValue = (string)Args.DefValue;
                                }
                                else if (Args.DefValue is int && (int)Args.DefValue != 0)
                                {
                                    ddl2.SelectedValue = ddl2.Items[(int)Args.DefValue].Value;
                                }
                            }
                        }
                    }
                };
                ddl.DataTextField = "Text";
                ddl.DataValueField = "Value";
                ddl.DataBind();
                a.Controls.Add(ddl);
            };
        }

        public override IndexedBasicTemplate.DCreateFunc CreateTemplateReadOnly(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                Label l = new Label();
                l.ID = "l" + i;
                a.Controls.Add(l);
            };
        }
        public override IndexedBasicTemplate.DCreateFunc CreateValidator(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                var reqVal = CreateReqVieldVal("ddl", i);
                a.Controls.Add(reqVal);
            };
        }
        public override int? GetValue(Control container, CrTemplateArgs Args, int Index)
        {
            string sw = _FindCnt<DropDownList>(container, Index, "ddl").SelectedValue;
            return !string.IsNullOrEmpty(sw) ? (int?)int.Parse(sw) : null;
        }
        public override void SetValue(Control container, CrTemplateArgs Args, int Index, int? Value2)
        {
            string Value = Value2.HasValue ? Value2.Value.ToString() : "";
            DropDownList ddl = _FindCnt<DropDownList>(container, Index, "ddl");
            var Item = ddl.Items.FindByValue(Value);
            if(Item == null)
            {
                object obj = (DataBinder.Eval(container.NamingContainer, "DataItem") as DataRowView)[(string)Args[0]];
                string strVal = (obj is DBNull) ? null : obj.ToString();
                ddl.Items.Insert(ddl.Items.Count, new ListItem(strVal, Value));
                ddl.DataBinding += (a, b) =>
                {
                    ddl.Items.Add(new ListItem(strVal, Value));
                };
            }
            ddl.SelectedValue = Value;
        }
        public override void SetValueReadOnly(Control container, CrTemplateArgs Args, int Index, int? Value2)
        {
            object obj = (DataBinder.Eval(container.NamingContainer, "DataItem") as DataRowView)[(string)Args[0]];
            string Value = (obj is DBNull) ? null : obj.ToString();
            if (string.IsNullOrEmpty(Value))
                _FindCnt<Label>(container, Index, "l").Text = "";
            else
                _FindCnt<Label>(container, Index, "l").Text = Value;
        }
        public override bool IsEmpty(Control container, CrTemplateArgs Args, int Index)
        {
            string sw = _FindCnt<DropDownList>(container, Index, "ddl").SelectedValue;
            return string.IsNullOrEmpty(sw);
        }
    }
    public class CrTC3_DDLTextBox : CrTemplateCreator<string>
    {
        public override string Validate(Control container, CrTemplateArgs Args, int Index)
        {
            var tb = _FindCnt<TextBox>(container, Index, "tb");
            var args2 = (CrTA2_DDLTextBox)Args;
            if (args2.MaxLength != 0 && tb.Text.Length > args2.MaxLength)
                return "Za długa wartość.";
            if (args2.AllowedChars != null && tb.Text.Any(a => !args2.AllowedChars.Contains(a)))
                return "Wartość zawiera niedozwolone znaki.";
            return null;
        }
        public IndexedBasicTemplate.DCreateFunc CreateTemplateEx(CrTA2_DDLTextBox Args)
        {
            return (a, b, i) =>
            {
                TextBox tb = new TextBox();
                tb.ID = "tb" + i;
                if (Args.MaxLength > 0)
                    tb.MaxLength = Args.MaxLength;
                a.Controls.Add(tb);
                if (!string.IsNullOrEmpty(Args.AllowedChars))
                {
                    FilteredTextBoxExtender ftb = new FilteredTextBoxExtender();
                    ftb.TargetControlID = tb.ID;
                    ftb.ValidChars = (string)Args.AllowedChars;
                    a.Controls.Add(ftb);
                }
                Literal lt = new Literal();
                lt.Text = "<br />";
                a.Controls.Add(lt);
                DropDownList ddl = new DropDownList();
                ddl.ID = "ddl" + i;
                ddl.DataSource = Args.DataSource;// Args.DataSources[0].Cast<DataRow>().Select(c => new { Text = c["Text"], Value = c["Value"].ToString() });
                ddl.AppendDataBoundItems = true;
                ddl.DataBinding += (c, d) =>
                {
                    DropDownList ddl2 = (c as DropDownList);
                    ddl2.Items.Clear();
                    if (Args.canNull)
                    {
                        ddl2.Items.Insert(0, new ListItem("wybierz ...", ""));
                    }
                    if(Args.DefValue != null)
                    {
                        if (ddl2.Items.Count != 0)
                        {
                            if (string.IsNullOrEmpty(ddl2.SelectedValue) && ddl2.SelectedIndex == 0)
                            {
                                if (Args.DefValue is string)
                                {
                                    ddl2.SelectedValue = (string)Args.DefValue;
                                }
                                else if (Args.DefValue is int && (int)Args.DefValue != 0)
                                {
                                    ddl2.SelectedValue = ddl2.Items[(int)Args.DefValue].Value;
                                }
                            }
                        }
                    }
                };
                ddl.DataTextField = "Text";
                ddl.DataValueField = "Value";
                ddl.AutoPostBack = true;
                ddl.SelectedIndexChanged += (c, d) =>
                {
                    tb.Text = ddl.SelectedItem.Text;
                };
                ddl.DataBind();
                a.Controls.Add(ddl);
            };
        }

        public override IndexedBasicTemplate.DCreateFunc CreateTemplate(CrTemplateArgs Args)
        {
            return CreateTemplateEx(new CrTA2_DDLTextBox(Args));
        }

        public override IndexedBasicTemplate.DCreateFunc CreateTemplateReadOnly(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                TextBox tb = new TextBox();
                tb.ID = "tb" + i;
                tb.Enabled = false;
                a.Controls.Add(tb);
            };
        }
        public override IndexedBasicTemplate.DCreateFunc CreateValidator(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                var reqVal = CreateReqVieldVal("tb", i);
                a.Controls.Add(reqVal);
            };
        }
        public override string GetValue(Control container, CrTemplateArgs Args, int Index)
        {
            return _FindCnt<TextBox>(container, Index, "tb").Text;
        }
        public override void SetValue(Control container, CrTemplateArgs Args, int Index, string Value)
        {
            DropDownList ddl = _FindCnt<DropDownList>(container, Index, "ddl");
            ddl.ClearSelection();
            ddl.SelectedIndex = 0;
            _FindCnt<TextBox>(container, Index, "tb").Text = Value;
        }
        public override void SetValueReadOnly(Control container, CrTemplateArgs Args, int Index, string Value)
        {
            _FindCnt<TextBox>(container, Index, "tb").Text = Value;
        }
        public override bool IsEmpty(Control container, CrTemplateArgs Args, int Index)
        {
            var tb = _FindCnt<TextBox>(container, Index, "tb");
            return string.IsNullOrEmpty(tb.Text);
        }
    }
    public class CrTC4_DateEdit : CrTemplateCreator<DateTime?>
    {
        public override IndexedBasicTemplate.DCreateFunc CreateTemplate(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                DateEdit de = (DateEdit)b("~/Controls/DateEdit.ascx");
                de.ID = "de" + i;
                a.Controls.Add(de);
            };
        }
        public override IndexedBasicTemplate.DCreateFunc CreateTemplateReadOnly(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                Label l = new Label();
                l.ID = "l" + i;
                a.Controls.Add(l);
            };
        }
        public override IndexedBasicTemplate.DCreateFunc CreateValidator(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {//de90$tbDate
                var de = (DateEdit)a.Parent.Controls[1].Controls[0];
                de.ValidationGroup = "vg1";
                //var req = (RequiredFieldValidator)de.FindControl("RequiredFieldValidator1");
                //req.ID = "rfv" + i;
                //a.Controls.Add(req);
            };
        }
        public override DateTime? GetValue(Control container, CrTemplateArgs Args, int Index)
        {
            return (DateTime?)_FindCnt<DateEdit>(container, Index, "de").Date;
        }
        public override void SetValue(Control container, CrTemplateArgs Args, int Index, DateTime? Value)
        {
            if(Value.HasValue)
                _FindCnt<DateEdit>(container, Index, "de").Date = Value.Value;
            else
                _FindCnt<DateEdit>(container, Index, "de").Date = null;
        }
        public override void SetValueReadOnly(Control container, CrTemplateArgs Args, int Index, DateTime? Value)
        {
            if (Value.HasValue)
                _FindCnt<Label>(container, Index, "l").Text = Tools.DateToStr(Value.Value);
            else
                _FindCnt<Label>(container, Index, "l").Text = "";
        }
        public override string Validate(Control container, CrTemplateArgs Args, int Index)
        {
            var de = _FindCnt<DateEdit>(container, Index, "de");
            return string.IsNullOrEmpty(de.EditBox.Text) || de.IsValid ? null :"Niepoprawny format";
        }
        public override bool IsEmpty(Control container, CrTemplateArgs Args, int Index)
        {
            var de = _FindCnt<DateEdit>(container, Index, "de");
            return string.IsNullOrEmpty(de.EditBox.Text);
        }
    }
    public class CrTC5_MultiLineTextBox : CrTemplateCreator<string>
    {
        public override string Validate(Control container, CrTemplateArgs Args, int Index)
        {
            var tb = _FindCnt<TextBox>(container, Index, "mtb");
            var args2 = (CrTA3_MultiLineTextBox)Args;
            if (args2.MaxLength != 0 && tb.Text.Length > args2.MaxLength)
                return "Za długa wartość.";
            if (args2.AllowedChars != null && tb.Text.Any(a => !args2.AllowedChars.Contains(a)))
                return "Wartość zawiera niedozwolone znaki.";
            return null;
        }
        public IndexedBasicTemplate.DCreateFunc CreateTemplateEx(CrTA3_MultiLineTextBox Args)
        {
            return (a, b, i) =>
            {
                TextBox tb = new TextBox();
                tb.ID = "mtb" + i;
                tb.MaxLength = Args.MaxLength;
                tb.TextMode = TextBoxMode.MultiLine;
                tb.Rows = Args.MaxRows;
                Tools.FixMultiLineMaxLen(tb);
                a.Controls.Add(tb);
                if (!string.IsNullOrEmpty(Args.AllowedChars))
                {
                    FilteredTextBoxExtender ftb = new FilteredTextBoxExtender();
                    ftb.TargetControlID = tb.ID;
                    ftb.ValidChars = (string)Args.AllowedChars;
                    a.Controls.Add(ftb);
                }
            };
        }
        public IndexedBasicTemplate.DCreateFunc CreateTemplateReadOnlyEx(CrTA3_MultiLineTextBox Args)
        {
            return (a, b, i) =>
            {
                TextBox tb = new TextBox();
                tb.ID = "mtb" + i;
                tb.MaxLength = Args.MaxLength;
                tb.TextMode = TextBoxMode.MultiLine;
                tb.Rows = Args.MaxRows;
                tb.Enabled = false;
                Tools.FixMultiLineMaxLen(tb);
                a.Controls.Add(tb);
            };
        }
        public override IndexedBasicTemplate.DCreateFunc CreateTemplateReadOnly(CrTemplateArgs Args)
        {
            return CreateTemplateReadOnlyEx(new CrTA3_MultiLineTextBox(Args));
        }
        public override IndexedBasicTemplate.DCreateFunc CreateTemplate(CrTemplateArgs Args)
        {
            return CreateTemplateEx(new CrTA3_MultiLineTextBox(Args));
        }
        public override IndexedBasicTemplate.DCreateFunc CreateValidator(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                var reqVal = CreateReqVieldVal("mtb", i);
                a.Controls.Add(reqVal);
            };
        }
        public override string GetValue(Control container, CrTemplateArgs Args, int Index)
        {
            return _FindCnt<TextBox>(container, Index, "mtb").Text;
        }
        public override void SetValue(Control container, CrTemplateArgs Args, int Index, string Value)
        {
            _FindCnt<TextBox>(container, Index, "mtb").Text = Value ?? "";
        }
        public override bool IsEmpty(Control container, CrTemplateArgs Args, int Index)
        {
            var tb = _FindCnt<TextBox>(container, Index, "mtb");
            return string.IsNullOrEmpty(tb.Text);
        }
    }
    public class CrTC6_CheckBox : CrTemplateCreator<bool?>
    {
        public override IndexedBasicTemplate.DCreateFunc CreateTemplate(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                CheckBox cb = new CheckBox();
                cb.ID = "cb" + i;
                a.Controls.Add(cb);
            };
        }
        public override IndexedBasicTemplate.DCreateFunc CreateTemplateReadOnly(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                CheckBox cb = new CheckBox();
                cb.ID = "cb" + i;
                cb.Enabled = false;
                a.Controls.Add(cb);
            };
        }
        public override bool? GetValue(Control container, CrTemplateArgs Args, int Index)
        {
            return _FindCnt<CheckBox>(container, Index, "cb").Checked;
        }
        public override void SetValue(Control container, CrTemplateArgs Args, int Index, bool? Value)
        {
            _FindCnt<CheckBox>(container, Index, "cb").Checked = Value ?? false;
        }
    }
    public class CrTC7_DateZakres : CrTemplateCreator<KeyValuePair<DateTime?, DateTime?>?>
    {
        public override IndexedBasicTemplate.DCreateFunc CreateTemplate(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                cntDateZakr dr = (cntDateZakr)b("~/BadaniaWstepne/Controls/cntDateZakr.ascx");
                dr.Clear();
                dr.ID = "dz" + i;
                a.Controls.Add(dr);
            };
        }
        public override IndexedBasicTemplate.DCreateFunc CreateTemplateReadOnly(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                TextBox tb = new TextBox();
                tb.ID = "tb" + i;
                tb.Enabled = false;
                a.Controls.Add(tb);
            };
        }
        public override KeyValuePair<DateTime?, DateTime?>? GetValue(Control container, CrTemplateArgs Args, int Index)
        {
            return _FindCnt<cntDateZakr>(container, Index, "dz").Value;
        }
        public override string GetFilterValue(Control container, CrTemplateArgs Args, int Index)
        {
            return _FindCnt<cntDateZakr>(container, Index, "dz").FilterExp;
        }
        public override void SetValue(Control container, CrTemplateArgs Args, int Index, KeyValuePair<DateTime?, DateTime?>? Value)
        {
            if (Value.HasValue)
                _FindCnt<cntDateZakr>(container, Index, "dz").Value = Value.Value;
            else
                _FindCnt<cntDateZakr>(container, Index, "dz").Value = new KeyValuePair<DateTime?, DateTime?>(null, null);
        }
        public override void SetValueReadOnly(Control container, CrTemplateArgs Args, int Index, KeyValuePair<DateTime?, DateTime?>? Value2)
        {
            KeyValuePair<DateTime?, DateTime?> Value = (Value2.HasValue) ? Value2.Value : new KeyValuePair<DateTime?, DateTime?>(null, null);

            string tmp;
            if (Value.Key.HasValue && Value.Value.HasValue)
                tmp = Tools.DateToStr(Value.Key.Value) + " - " + Tools.DateToStr(Value.Value.Value);
            else if(Value.Key.HasValue)
                tmp = "od " + Tools.DateToStr(Value.Key.Value);
            else if(Value.Value.HasValue)
                tmp = "do " + Tools.DateToStr(Value.Value.Value);
            else
                tmp = "";
            _FindCnt<Label>(container, Index, "l").Text = tmp;
        }
    }
    public class CrTC8_SplityWsp : CrTemplateCreator<string>
    {
        public override IndexedBasicTemplate.DCreateFunc CreateTemplate(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                cntSplityWsp2 sw = (cntSplityWsp2)b("~/BadaniaWstepne/Controls/cntSplityWsp2.ascx");
                sw.ID = "sw" + i;
                sw.TableName = "SplityWspB"; // TODO: dodac do tabeli type, a do cntSplityWsp2 property type
                sw.IdPrzypColName = "IdPrzypisania";
                sw.Mode = cntSplityWsp2.FModes.Editable;
                a.Controls.Add(sw);
            };
        }
        public override IndexedBasicTemplate.DCreateFunc CreateTemplateReadOnly(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                cntSplityWsp2 sw = (cntSplityWsp2)b("~/BadaniaWstepne/Controls/cntSplityWsp2.ascx");
                sw.ID = "sw" + i;
                sw.TableName = "SplityWspB";
                sw.IdPrzypColName = "IdPrzypisania";
                sw.Mode = cntSplityWsp2.FModes.ReadOnly;
                a.Controls.Add(sw);
            };
        }
        public override string GetValue(Control container, CrTemplateArgs Args, int Index)
        {
            return _FindCnt<cntSplityWsp2>(container, Index, "sw").IdPrzypisania;
        }
        public override string Validate(Control container, CrTemplateArgs Args, int Index)
        {
            var sw = _FindCnt<cntSplityWsp2>(container, Index, "sw");
            if (sw.Count != 0 && !sw._Validate())
            {
                return "Niepoprawna wartość.";
            }
            return null;
        }
        public override bool IsEmpty(Control container, CrTemplateArgs Args, int Index)
        {
            var sw = _FindCnt<cntSplityWsp2>(container, Index, "sw");
            return sw.Count == 0;
        }
        public override void SetValue(Control container, CrTemplateArgs Args, int Index, string Value)
        {
            _FindCnt<cntSplityWsp2>(container, Index, "sw").IdPrzypisania = Value;
        }
        public override void SetControlValue(CrTemplateCreator.UpdInsArgsCnt Args, DataRowView Item)
        {
            Args.Template.ValueSetter(Args.Container, Item["id"].ToString()); // __MARK2 tak samo w DataLabel
        }
        public override void Updating(CrTemplateCreator.UpdInsArgsCnt Args, IDictionary Values) { }
        public override void Updated(CrTemplateCreator.UpdInsArgsCnt Args, ListViewUpdatedEventArgs e)
        {
            _FindCnt<cntSplityWsp2>(Args.Container, Args.Column.Sort, "sw").Update();
        }
        public override void Inserting(CrTemplateCreator.UpdInsArgsCnt Args, IDictionary Values) { }
        public override void Inserted(CrTemplateCreator.UpdInsArgsCnt Args, DataRow Item)
        {
            var cnt = _FindCnt<cntSplityWsp2>(Args.Container, Args.Column.Sort, "sw");
            cnt.IdPrzypisania = Item["id"].ToString();
            cnt.Update();
        }
        public override void Deleted(CrTemplateCreator.UpdInsArgsCnt Args, DataRow Item)
        {
            string sql = @"DELETE SplityWspB where IdPrzypisania = {0}";
            db.execSQL(string.Format(sql, Item["id"]));
        }
    }
    public class CrTC9_DDLBOOL : CrTemplateCreator<bool?>
    {
        public override IndexedBasicTemplate.DCreateFunc CreateTemplate(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                DropDownList ddl = new DropDownList();
                ddl.ID = "ddl" + i;
                ddl.AppendDataBoundItems = true;
                ddl.Items.Insert(0, new ListItem("wybierz ...", ""));
                ddl.Items.Insert(1, new ListItem("Tak", "1"));
                ddl.Items.Insert(2, new ListItem("Nie", "0"));
                ddl.DataTextField = "Text";
                ddl.DataValueField = "Value";
                ddl.DataBind();
                a.Controls.Add(ddl);
            };
        }

        public override IndexedBasicTemplate.DCreateFunc CreateTemplateReadOnly(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                Label l = new Label();
                l.ID = "l" + i;
                a.Controls.Add(l);
            };
        }

        public override bool? GetValue(Control container, CrTemplateArgs Args, int Index)
        {
            string str = _FindCnt<DropDownList>(container, Index, "ddl").SelectedValue;
            return (str == "") ? null : (bool?)(str == "1");
        }
        public override void SetValue(Control container, CrTemplateArgs Args, int Index, bool? Value)
        {
            _FindCnt<DropDownList>(container, Index, "ddl").SelectedValue = !Value.HasValue ? "" : (Value.Value) ? "1" : "0";
        }
        public override void SetValueReadOnly(Control container, CrTemplateArgs Args, int Index, bool? Value)
        {
            _FindCnt<Label>(container, Index, "l").Text = !Value.HasValue ? "" : (Value.Value) ? "Tak" : "Nie";
        }
    }
    public class CrTC10_DataLabel : CrTemplateCreator<int?>
    {
        public override IndexedBasicTemplate.DCreateFunc CreateTemplate(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                Label l = new Label();
                l.ID = "l" + i;
                a.Controls.Add(l);
            };
        }
        public override void SetControlValue(CrTemplateCreator.UpdInsArgsCnt Args, DataRowView Item)
        {
            Args.Template.ValueSetter(Args.Container, Item["id"]); // bez (Value is DBNull) bo 'id'
        }
        public override int? GetValue(Control container, CrTemplateArgs Args, int Index)
        {
            throw new NotImplementedException();
        }
        public override void SetValue(Control container, CrTemplateArgs Args2, int Index, int? Value2)
        {
            Label l = _FindCnt<Label>(container, Index, "l");
            if(Value2.HasValue)
            {
                int Value = Value2.Value;
                CrTA4_DataLabel Args = (CrTA4_DataLabel)Args2;
                IEnumerable<string> DataSource = db.getDataSet(string.Format(Args.Sql, Value)).Tables[0].Rows.Cast<DataRow>().Select(a => a[0].ToString());
                l.Text = string.Join(Args.Seperator, DataSource.ToArray());
            }
            else
            {
                l.Text = "";
            }
        }
    }
    public class CrTC11_AsyncFileUpload : CrTemplateCreator<string>
    {
        public override IndexedBasicTemplate.DCreateFunc CreateTemplate(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                HtmlGenericControl gc = new HtmlGenericControl("div");

                Label l = new Label();
                l.ID = "imgLoader" + i;
                l.CssClass = "fileupload";
                l.Style.Add("display", "none");
                HiddenField hf = new HiddenField();
                hf.ID = "HFID" + i;
                HiddenField hfVal = new HiddenField();
                hfVal.ID = "HFVALUE" + i;
                Image img2 = new Image();
                Page pg = HttpContext.Current.Handler as Page;
                img2.ImageUrl = pg.ResolveUrl("~/images/uploading.gif");
                img2.AlternateText = "";
                l.Controls.Add(img2);
                AsyncFileUpload fu = new AsyncFileUpload();
                fu.ID = "fu" + i;
                fu.CssClass = "fileupload";
                fu.ToolTip = "Wybierz plik";
                fu.UploadingBackColor = System.Drawing.Color.FromArgb(0x8F, 0xDB, 0x3C);
                fu.CompleteBackColor = System.Drawing.Color.FromArgb(0x8F, 0xDB, 0x3C);
                fu.UploaderStyle = AsyncFileUpload.UploaderStyleEnum.Modern;
                fu.ThrobberID = "imgLoader" + i;
                fu.UploadedComplete += new EventHandler<AsyncFileUploadEventArgs>((c, d) =>
                {
                    var IItem = a;
                    string ext = Path.GetExtension(d.FileName);

                    string path = HttpContext.Current.Request.MapPath((string)Args[0] + "$$${0}");
                    string spath = HttpContext.Current.Request.MapPath((string)Args[0] + "{0}");
                    int fi;
                    for (fi = 0; File.Exists(string.Format(path + ".png", fi)) || File.Exists(string.Format(spath + ".png", fi)); fi = new Random().Next(999999999)) { }
                    path = string.Format(path, fi);
                    spath = string.Format(path, fi);
                    //hf.Value = fi.ToString();
                    string js = string.Format("top.document.getElementById('{0}').value = '{1}';", hf.ClientID, fi);
                    Tools.ExecOnStart2("_BadWstSaveId", js);

                    string org = string.Format("{0}_{1}", path, ext);
                    fu.SaveAs(org);
                    if (ext != ".png")
                    {
                        string org_;
                        using (var bm = new System.Drawing.Bitmap(org))
                        {
                            org_ = org;
                            org = string.Format("{0}_{1}", path, ".png");
                            bm.Save(org);
                        }
                        File.Delete(org_);
                    }

                    string mm = string.Format("{0}.png", path);
                    using (var bm = new System.Drawing.Bitmap(org))
                    {
                        Tools.ResizeBitmap(bm, 320, 0).Save(mm, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    string url = (HttpContext.Current.Handler as Page).ResolveUrl((string)Args[0] + string.Format("$$${0}.png", fi)) + "?time=" + DateTime.Now.ToString();
                    
                    js = string.Format(@"{{ 
                        var tmpImg = top.document.getElementById('{0}');
                        var tmpHF = top.document.getElementById('{2}');
                        tmpHF.value = '{3}';
                        tmpImg.src = '{1}';
                        $(tmpImg).show();
                    }}", a.FindControl("__imgtest").ClientID, url, hfVal.ClientID, string.Format("{0}.png", fi));
                    Tools.ExecOnStart2("_BadWstReloadImg", js);
                });
                fu.UploadedFileError += new EventHandler<AsyncFileUploadEventArgs>((c, d) =>
                {
                    Tools.ShowMessage("Wystąpił błąd podczas ładowania pliku.");
                });
                //gc.Controls.Add(l);
                gc.Controls.Add(hfVal);
                gc.Controls.Add(hf);
                gc.Controls.Add(fu);
                gc.Controls.Add(l);
                a.Controls.Add(gc);
            };
        }
        //public override void SetControlValue(CrTemplateCreator.UpdInsArgsCnt Args, DataRowView Item)
        //{
        //    AsyncFileUpload fu = _FindCnt<AsyncFileUpload>(Args.Container, Args.Column.Sort, "fu");
        //    if (fu != null)
        //    {
        //        _FindCnt<HiddenField>(Args.Container, Args.Column.Sort, "HFID").Value = Item["id"].ToString();
        //    }
        //    base.SetControlValue(Args, Item);
        //}
        public override IndexedBasicTemplate.DCreateFunc CreateTemplateReadOnly(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                Literal l1 = new Literal();
                l1.Text = "<div class=\"img1\"><div class=\"img2\">";
                Literal l2 = new Literal();
                l2.Text = "</div></div>";
                Image img = new Image();
                img.ID = "__imgtest";
                a.Controls.Add(l1);
                a.Controls.Add(img);
                a.Controls.Add(l2);
            };
        }
        public override string GetValue(Control container, CrTemplateArgs Args, int Index)
        {
            HiddenField hf = _FindCnt<HiddenField>(container, Index, "HFVALUE");
            return (hf == null) ? null : hf.Value;
        }
        public override void SetValue(Control container, CrTemplateArgs Args, int Index, string Value)
        {
            Image img = (Image)container.FindControl("__imgtest");
            HiddenField hf = _FindCnt<HiddenField>(container, Index, "HFVALUE");

            if(hf != null)
            {
                hf.Value = Value;
            }
            if (img != null)
            {
                if (string.IsNullOrEmpty(Value))
                {
                    img.Style.Add("display", "none");
                    //img.Visible = false;
                }
                else
                {
                    img.Style.Remove("display");
                    img.ImageUrl = (HttpContext.Current.Handler as Page)
                        .ResolveUrl((string)Args[0] + Value) + "?time=" + DateTime.Now.ToString();
                }
            }
        }
        public override void Inserted(CrTemplateCreator.UpdInsArgsCnt Args, DataRow Item)
        {
            Image img = (Image)Args.Container.FindControl("__imgtest");
            HiddenField hf = _FindCnt<HiddenField>(Args.Container, Args.Column.Sort, "HFID");

            if (!String.IsNullOrEmpty(hf.Value))
            {
                string pbase = HttpContext.Current.Request.MapPath((string)Args.Template.Args[0] + "{0}" + hf.Value + "{1}" + ".png");

                File.Move(string.Format(pbase, "$$$", ""),
                    string.Format(pbase, "", ""));
                File.Move(string.Format(pbase, "$$$", "_"),
                    string.Format(pbase, "", "_"));
            }
            base.Inserted(Args, Item);
        }
        public override void Updated(CrTemplateCreator.UpdInsArgsCnt Args, ListViewUpdatedEventArgs e)
        {
            Image img = (Image)Args.Container.FindControl("__imgtest");
            HiddenField hf = _FindCnt<HiddenField>(Args.Container, Args.Column.Sort, "HFID");

            if (!String.IsNullOrEmpty(hf.Value))
            {
                string pbase = HttpContext.Current.Request.MapPath((string)Args.Template.Args[0] + "{0}" + hf.Value + "{1}" + ".png");

                File.Move(string.Format(pbase, "$$$", ""),
                    string.Format(pbase, "", ""));
                File.Move(string.Format(pbase, "$$$", "_"),
                    string.Format(pbase, "", "_"));
            }
            base.Updated(Args, e);
        }
    }
    public class CrTC12_DateEditZakr : CrTemplateCreator<DateTime?>
    {
        public override IndexedBasicTemplate.DCreateFunc CreateTemplate(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                DateEdit de = (DateEdit)b("~/Controls/DateEdit.ascx");
                de.ID = "de" + i;
                a.Controls.Add(de);
            };
        }
        public override IndexedBasicTemplate.DCreateFunc CreateTemplateReadOnly(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {
                Label l = new Label();
                l.ID = "l" + i;
                a.Controls.Add(l);
            };
        }
        public override IndexedBasicTemplate.DCreateFunc CreateValidator(CrTemplateArgs Args)
        {
            return (a, b, i) =>
            {//de90$tbDate
                var de = (DateEdit)a.Parent.Controls[1].Controls[0];
                de.ValidationGroup = "vg1";
                //var req = (RequiredFieldValidator)de.FindControl("RequiredFieldValidator1");
                //req.ID = "rfv" + i;
                //a.Controls.Add(req);
            };
        }
        public override DateTime? GetValue(Control container, CrTemplateArgs Args, int Index)
        {
            return (DateTime?)_FindCnt<DateEdit>(container, Index, "de").Date;
        }
        public override void SetValue(Control container, CrTemplateArgs Args, int Index, DateTime? Value)
        {
            if (Value.HasValue)
                _FindCnt<DateEdit>(container, Index, "de").Date = Value.Value;
            else
                _FindCnt<DateEdit>(container, Index, "de").Date = null;
        }
        public override void SetValueReadOnly(Control container, CrTemplateArgs Args, int Index, DateTime? Value)
        {
            if (Value.HasValue)
                _FindCnt<Label>(container, Index, "l").Text = Tools.DateToStr(Value.Value);
            else
                _FindCnt<Label>(container, Index, "l").Text = "";
        }
        public override string Validate(Control container, CrTemplateArgs Args, int Index)
        {
            var de = _FindCnt<DateEdit>(container, Index, "de");
            return string.IsNullOrEmpty(de.EditBox.Text) || de.IsValid ? null : "Niepoprawny format";
        }
        public override bool IsEmpty(Control container, CrTemplateArgs Args, int Index)
        {
            var de = _FindCnt<DateEdit>(container, Index, "de");
            return string.IsNullOrEmpty(de.EditBox.Text);
        }
        //public override void Inserting(UpdInsArgsCnt Args, IDictionary Values)
        //{
        //    if (!Args.Template.isReadOnly)
        //    {
        //        string Column = "Zakr";
        //        object Value = cntMenuZakres.GetZakrFromDate((DateTime)Args.Template.ValueGetter(Args.Container));
        //        if (Value != null && (!(Value is string) || !string.IsNullOrEmpty((string)Value)))
        //            Values.Add(Column, ToSqlValue(Value));
        //    }
        //}
        //public override void Updating(CrTemplateCreator.UpdInsArgsCnt Args, IDictionary Values)
        //{
        //    if (!Args.Template.isReadOnly)
        //    {
        //        string Column = "Zakr";
        //        object Value = cntMenuZakres.GetZakrFromDate((DateTime)Args.Template.ValueGetter(Args.Container));
        //        Values.Add(Column, ToSqlValue(Value));
        //    }
        //}
    }
}