using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls.Portal
{
    public partial class x_dbField : System.Web.UI.UserControl
    {
        public event EventHandler Changed;
        
        public enum TTyp { lb, tb, ddl, check, date, dt }
        
        private TTyp FTyp = TTyp.lb;
        string FStVisible = null;           // "11111,11111,11111"
        string FTypVisible = null;          // "1,3,7"
        string FFields = null;              // "Field" lub "edField,qField" tez dla ddl
        string FValueField = null;          // dla ddl
        string FDataSourceID = null;
        string FControls = null;
        string FFormat = null;              // w Fields po przecinku pola, Format {0} 
        int FMin = 0;
        int FMax = 0;
        bool FRq = false;

        const string cssMultiLine = "dbFieldMultiLine";  

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.AddClass(paField, ID);
                if (FTyp == TTyp.tb && tbValue.TextMode == TextBoxMode.MultiLine)
                    Tools.AddClass(paField, cssMultiLine);   
            }
        }
        //----------------------------
        public static void FillData(Control cnt, DataRow dr, int typ, int vindex, int index, bool queryMode)  // wnioski: id=typ, vindex=status, index=osoba
        {
            /*
            foreach (Control c in cnt.Controls)
                if (c is dbField)
                    ((dbField)c).FillData(dr, typ, vindex, index, queryMode);
                else if (c is WnButton)
                    ((WnButton)c).SetVisible(typ, vindex, index, queryMode);
                else if (c is WnVisible)
                    ((WnVisible)c).SetVisible(typ, vindex, index);
                else if (c.HasControls())
                    FillData(c, dr, typ, vindex, index, queryMode);
             */
        }

        public static void SetEditVisible(Control cnt, int typ, int vindex, int index, bool editMode)  // wnioski: id=typ, vindex=status, index=osoba
        {
            /*
            foreach (Control c in cnt.Controls)
                if (c is dbField)
                    ((dbField)c).SetEditVisible(typ, vindex, index, editMode);
                else if (c is WnButton)
                    ((WnButton)c).SetVisible(typ, vindex, index, false);
                else if (c is WnVisible)
                    ((WnVisible)c).SetVisible(typ, vindex, index);
                else if (c.HasControls())
                    SetEditVisible(c, typ, vindex, index, editMode);
             */ 
        }

        public static bool Validate(Control cnt)
        {
            bool valid = true;
            foreach (Control c in cnt.Controls)
                if (c is dbField)
                {
                    dbField dbf = c as dbField;
                    if (dbf.Visible)
                        if (!dbf.Validate()) 
                            valid = false;
                }
                else if (c.HasControls())
                    if (!Validate(c))
                        valid = false;
            return valid;
        }

        public static void ClearErrors(Control cnt)
        {
            foreach (Control c in cnt.Controls)
                if (c is dbField)
                {
                    dbField dbf = c as dbField;
                    dbf.SetError(false);
                }
                else if (c.HasControls())
                    ClearErrors(c);
        }

        public static int dbInsert(Control cnt, string table, string fields, params object[] values)
        {
            GetFieldsValues(cnt, ref fields, ref values);
            return db.insert(table, true, true, 0, fields, values);
        }

        public static bool dbUpdate(Control cnt, string table, string where, string fields, params object[] values)
        {
            GetFieldsValues(cnt, ref fields, ref values);
            return db.update(table, 0, fields, where, values);
        }

        public static void GetFieldsValues(Control cnt, ref string fields, ref object[] values)
        {
            /*
            foreach (Control c in cnt.Controls)
                if (c is dbField)
                {
                    dbField dbf = c as dbField;
                    if (dbf.Editable)
                    {
                        //----- fields -----
                        string F1, F2;
                        dbf.getFields(dbf.Fields, out F1, out F2);
                        if (String.IsNullOrEmpty(fields))
                            fields = F1;
                        else
                            fields += "," + F1;
                        //----- values -----
                        int idx = values.Length;
                        Array.Resize(ref values, idx + 1);  // później zoptymalizować !!!
                        values[idx] = dbf.dbValue;
                    }
                }
                else if (c.HasControls())
                    GetFieldsValues(c, ref fields, ref values);
             */ 
        }
        //---------------------------
        public void AddRqStar(bool add)
        {
            const string star = "<span class=\"star\">*</span>";   //<<< też inaczej to rozwiązać np trzymać na FLabel i podstawiać
            string lb = lbLabel.Text.TrimEnd();
            if (add)
            {
                if (!lb.Contains(star))
                    if (lb.EndsWith(":"))
                        lbLabel.Text = lb.Substring(0, lb.Length - 1) + star + ":";
                    else
                        lbLabel.Text += star;
            }
            else
                if (lb.Contains(star))
                    lbLabel.Text = lb.Replace(star, "");
        }

        public bool FillData(DataRow dr, int typ, int vindex, int index, int mode)  //0 - normal, 1 - 
        {
            //----- visible, edit -----
            bool visible;
            bool edit = false;
            if (typVisible(FTypVisible, typ))
            {
                switch (FTyp)
                {
                    case TTyp.ddl:
                        string F1, F2;
                        getFields(Fields, out F1, out F2);
                        ddlValue.DataSourceID = FDataSourceID;
                        ddlValue.DataValueField = F1;
                        ddlValue.DataTextField = F2;
                        //ddlValue.DataBind();
                        break;
                }
                if (!String.IsNullOrEmpty(FStVisible))
                {
                    if (Visible)   //???????? spr !!!!
                    {
                        string[] vis = FStVisible.Split(',');
                        char v = getVisible(vindex, index, out visible, out edit, vis);
                        SetValue(dr, visible, edit, Fields);
                    }
                    else
                    {
                        SetValue(dr, false, false, Fields);
                        visible = false;
                    }
                }
                else
                {
                    SetValue(dr, true, false, Fields);
                    visible = true;
                }
            }
            else
            {
                visible = false;
            }
            //----- required -----
            AddRqStar(edit && FRq);
            //----- other controls -----
            if (String.IsNullOrEmpty(FControls))
                setVisible(Parent, FControls, visible);

            SetError(false);
            paField.Visible = visible;
            return visible;
        }

        private void SetEditVisible(int typ, int vindex, int index, bool editMode)
        {
            //----- visible, edit -----
            bool visible;
            bool edit = false;
            if (typVisible(FTypVisible, typ))
            {
                if (!String.IsNullOrEmpty(FStVisible))
                {
                    if (Visible)   //???????? spr !!!!
                    {
                        string[] vis = FStVisible.Split(',');
                        char v = getVisible(vindex, index, out visible, out edit, vis);
                        if (visible)
                        {
                            bool ed = edit || editMode && v == '3';
                            switch (FTyp)
                            {
                                case TTyp.tb:
                                    lbValue.Visible = !ed;
                                    tbValue.Visible = ed;
                                    break;
                                case TTyp.ddl:
                                    lbValue.Visible = !ed;
                                    ddlValue.Visible = ed;
                                    break;
                                case TTyp.dt:
                                case TTyp.date:
                                    lbValue.Visible = !ed;
                                    deValue.Visible = ed;
                                    break;
                                case TTyp.check:
                                    cbValue.Enabled = ed;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        visible = false;
                    }
                }
                else
                {
                    visible = true;
                }
            }
            else
            {
                visible = false;
            }
            //----- other controls -----
            if (String.IsNullOrEmpty(FControls))
                setVisible(Parent, FControls, visible);

            SetError(false);
            paField.Visible = visible;
        }

        public bool Validate()
        {
            switch (FTyp)
            {
                case TTyp.tb:
                    if (tbValue.Visible && FRq && String.IsNullOrEmpty(Value.Trim()))
                    {
                        SetError(true);
                        return false;
                    }
                    else
                        SetError(false);
                    break;
                case TTyp.ddl:
                    string sel = Value;
                    if (ddlValue.Visible && FRq && String.IsNullOrEmpty(Value))
                    {
                        SetError(true);
                        return false;
                    }
                    else
                        SetError(false);
                    break;
                case TTyp.dt:
                case TTyp.date:
                    if (deValue.Visible && (FRq && String.IsNullOrEmpty(Value.Trim()) || !deValue.IsValid))
                    {
                        SetError(true);
                        return false;
                    }
                    else
                        SetError(false);
                    break;
            }
            return true;
        }

        public void SetError(bool error)
        {
            if (error)
                Tools.AddClass(lbLabel, "error");
            else
                Tools.RemoveClass(lbLabel, "error");
        }
        //-----------------------------------------------------------
        private string getFieldName(string id)  // prefix z małych liter lub 1:1
        {
            char[] a = id.ToCharArray();
            for (int i = 0; i < a.Length; i++)
            {
                char c = a[i];
                if (c >= 'A' && c <= 'Z')
                    return id.Substring(i);
            }
            return id;
        }

        private int getFields(string fields, out string F1, out string F2)  // F1-edit, F2-query
        {
            string[] fa = fields.Split(',');
            F1 = fa.Length > 0 ? fa[0] : null;
            F2 = fa.Length > 1 ? fa[1] : F1;
            return fa.Length;
        }

        public static bool fieldExists(DataRow dr, string field)  //<<<< inaczej powinno być zrobione, ale na szybko ...
        {
            try
            {
                object o = dr[field];
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string getValue(DataRow dr, string field)  //<<<< inaczej powinno być zrobione, ale na szybko ...
        {
            try
            {
                object o = dr[field];
                return o != null ? o.ToString() : null;
            }
            catch
            {
                return null;
            }
        }

        public static object getValueObj(DataRow dr, string field)  
        {
            try
            {
                return dr[field];
            }
            catch
            {
                return null;
            }
        }

        public static bool typVisible(string typVis, int typ)
        {
            if (!String.IsNullOrEmpty(typVis))
            {
                string t = typ.ToString();
                string[] ta = typVis.Split(',');
                for (int i = 0; i < ta.Length; i++)
                    if (ta[i].Trim() == t)
                        return true;
                return false;
            }
            else 
                return true;
        }

        public static void setVisible(Control cnt, string cnames, bool visible)
        {
            if (!String.IsNullOrEmpty(cnames))
            {
                string[] cntA = cnames.Split(',');
                foreach (string cntN in cntA)
                {
                    Control c = cnt.FindControl(cntN.Trim());
                    if (c != null)
                        c.Visible = visible;
                }
            }
        }
        //-----------
        protected void TriggerChanged()//object sender, EventArgs e)
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }

        //----- SetValue ---------------------------------------------------------------------------------------------
        private void SetValue(Label lb, bool visible, string value)
        {
            lb.Text = value;
            lb.Visible = visible; 
        }

        private void SetValue(TextBox tb, bool visible, string value)
        {
            tb.Text = value;
            tb.Visible = visible;
            if (tb.TextMode == TextBoxMode.MultiLine)
                Tools.FixMultiLineMaxLen(tb);
        }

        private void SetValue(DropDownList ddl, bool visible, string value)
        {
            Tools.SelectItem(ddl, value);
            ddl.Visible = visible;
            
            bool c = Changed != null;
            ddl.AutoPostBack = c;
            //if (c) ddl.SelectedIndexChanged += new EventHandler(TriggerChanged);
        }

        private void _SetValue(DateEdit de, bool visible, string value)
        {
            de.DateStr = value;
            de.Visible = visible;
        }

        private void SetValue(HtmlGenericControl div, DateEdit de, bool visible, string value)
        {
            de.DateStr = value;
            div.Visible = visible;
        }

        private void SetValue(CheckBox cb, bool visible, bool edit, bool check)
        {
            cb.Checked = check;
            cb.Visible = visible; 
            cb.Enabled = edit;
        }

        private void SetValue(DataRow dr, bool visible, bool edit, string fields)  // lista pól oddzielona przecinkiem !!! 0-id, 1-napis dla ddl
        {
            bool q = visible && !edit;
            bool e = visible && edit;
            if (q && FTyp == TTyp.lb && !String.IsNullOrEmpty(FFormat))
            {
                //----- tryb formatted (tylko query) -----
                string[] fa = fields.Split(',');
                object[] va = new object[fa.Length];
                for (int i = 0; i < fa.Length; i++)
                    va[i] = getValueObj(dr, fa[i]);
                string v = String.Format(FFormat, va);
                SetValue(lbValue, q, v);
            }
            else
            {
                //----- tryb !formatted -----
                string F1, F2;
                int len = getFields(fields, out F1, out F2);
                paField.Visible = visible;
                bool d = len > 0 && dr != null && fieldExists(dr, F1) && (len == 1 || fieldExists(dr, F2));
                bool vq = d && q;
                bool ve = d && e;
                switch (FTyp)
                {
                    case TTyp.lb:
                        SetValue(lbValue, q, getValue(dr, F2));
                        break;
                    case TTyp.tb:
                        SetValue(lbValue, q, getValue(dr, F2));
                        SetValue(tbValue, e, getValue(dr, F1));
                        break;
                    case TTyp.ddl:
                        SetValue(lbValue, q, getValue(dr, F2));
                        string sel = getValue(dr, F1);
                        tmpValue = sel;  
                        if (ve) ddlValue.DataBind();
                        SetValue(ddlValue, e, ve ? sel : null);
                        break;
                    case TTyp.date:
                        SetValue(lbValue, q, Tools.DateToStr(getValueObj(dr, F2)));
                        SetValue(paDateEdit, deValue, e, Tools.DateToStr(getValueObj(dr, F1)));
                        break;
                    case TTyp.dt:
                        string s = null;
                        if (vq)
                        {
                            DateTime? dt = db.getDateTime(dr, F2);
                            if (!db.isNull(dt))
                            {
                                s = String.Format("{0} <span class=\"time\">{1}</span>",
                                        Tools.DateToStr(dt),
                                        Tools.TimeToStr((DateTime)dt));
                            }
                            tmpValue = Tools.DateTimeToStr(dt);
                        }
                        SetValue(lbValue, q, s);
                        SetValue(paDateEdit, deValue, e, Tools.DateToStr(getValueObj(dr, F1)));  // <<<na razie to samo co data
                        break;
                    case TTyp.check:
                        SetValue(lbValue, false, null);
                        SetValue(cbValue, visible, edit, db.getBool(dr, F1, false));
                        break;
                }
            }
        }

        /*
        private void SetValue(DataRow dr, bool visible, bool edit, string fields)  // lista pól oddzielona przecinkiem !!! 0-id, 1-napis dla ddl
        {
            bool q = visible && !edit;
            bool e = visible && edit;
            if (q && FTyp == TTyp.lb && !String.IsNullOrEmpty(FFormat))
            {
                //----- tryb formatted (tylko query) -----
                string[] fa = fields.Split(',');
                object[] va = new object[fa.Length];
                for (int i = 0; i < fa.Length; i++)
                    va[i] = getValueObj(dr, fa[i]);
                string v = String.Format(FFormat, va);
                SetValue(lbValue, q, v);
            }
            else
            {
                //----- tryb !formatted -----
                string F1, F2;
                int len = getFields(fields, out F1, out F2);
                paField.Visible = visible;
                bool d = len > 0 && dr != null && fieldExists(dr, F1) && (len == 1 || fieldExists(dr, F2));
                bool vq = d && q;
                bool ve = d && e;
                switch (FTyp)
                {
                    case TTyp.lb:
                        SetValue(lbValue, q, vq ? db.getValue(dr, F2) : null);
                        break;
                    case TTyp.tb:
                        SetValue(lbValue, q, vq ? db.getValue(dr, F2) : null);
                        SetValue(tbValue, e, ve ? db.getValue(dr, F1) : null);
                        break;
                    case TTyp.ddl:
                        SetValue(lbValue, q, vq ? db.getValue(dr, F2) : null);
                        string sel = getValue(dr, F1);
                        tmpValue = sel;
                        if (ve) ddlValue.DataBind();
                        SetValue(ddlValue, e, ve ? sel : null);
                        break;
                    case TTyp.date:
                        SetValue(lbValue, q, vq ? Tools.DateToStr(db.getDateTime(dr, F2)) : null);
                        SetValue(paDateEdit, deValue, e, ve ? Tools.DateToStr(db.getDateTime(dr, F1)) : null);
                        break;
                    case TTyp.dt:
                        string s = null;
                        if (vq)
                        {
                            DateTime? dt = db.getDateTime(dr, F2);
                            if (!db.isNull(dt))
                            {
                                s = String.Format("{0} <span class=\"time\">{1}</span>",
                                        Tools.DateToStr(dt),
                                        Tools.TimeToStr((DateTime)dt));
                            }
                            tmpValue = Tools.DateTimeToStr(dt);
                        }
                        SetValue(lbValue, q, s);
                        SetValue(paDateEdit, deValue, e, ve ? Tools.DateToStr(db.getDateTime(dr, F1)) : null);  // <<<na razie to samo co data
                        break;
                    case TTyp.check:
                        SetValue(lbValue, false, null);
                        SetValue(cbValue, visible, edit, d && visible ? db.getBool(dr, F1, false) : false);
                        break;
                }
            }
        }
         */

        public static bool _SetVisible(ref bool paVisible, Button bt, int vindex, int index, params string[] vis)  // ostatni vis to question
        {
            if (vis.Length > 0)
            {
                bool visible, edit;
                string question = vis[vis.Length - 1];
                char v = getVisible(vindex, index, out visible, out edit, vis);
                bt.Visible = visible;
                if (visible)
                {
                    if (!String.IsNullOrEmpty(question))
                        Tools.MakeConfirmButton(bt, question);  
                    paVisible = true;
                    return true;
                }
            }
            return false;
        }

        public static char getVisible(int vindex, int index, out bool visible, out bool edit, params string[] vis) // zazwyczaj vindex to tryb/osoba, index to status
        {
            if (0 <= vindex && 0 < vis.Length)
            {
                string v = vis[vindex];
                char[] va = v.ToCharArray();
                if (0 <= index && index < va.Length)
                {
                    char vas = va[index];
                    visible = vas != '0';   // 3 też tędy
                    edit = vas == '2';
                    return vas;
                }
            }
            visible = false;
            edit = false;
            return (char)0;
        }

        public static char getVisible(int vindex, int index, out bool visible, out bool edit, string stvis) // zazwyczaj vindex to tryb/osoba, index to status
        {
            string[] vis = stvis.Split(',');
            return getVisible(vindex, index, out visible, out edit, vis);
        }

        //---------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------
        public string StVisible
        {
            set { FStVisible = value; }
            get { return FStVisible; }
        }

        public string TypVisible
        {
            set { FTypVisible = value; }
            get { return FTypVisible; }
        }

        public string Fields
        {
            set { FFields = value; }
            get 
            {
                if (String.IsNullOrEmpty(FFields))
                {
                    string f = getFieldName(ID);
                    if (String.IsNullOrEmpty(FValueField))
                        return f;
                    else
                        return f + "," + FValueField;
                }
                else
                    return FFields;  
            }
        }

        public bool Editable
        {
            get { return tbValue.Visible || deValue.Visible || (cbValue.Visible && cbValue.Enabled); }  // lub najpierw case po typ
        }

        public string Format    // na razie tylko dla Fields
        {
            set { FFormat = value; }
            get { return FFormat; }
        }

        public string StControls    // inne kontrolki visible
        {
            set { FControls = value; }
            get { return FControls; }
        }
        //---------    
        public string Label         // pomyslec o dodaniu EdLabel
        {
            set 
            { 
                lbLabel.Text = value;
                lbLabel.Visible = !String.IsNullOrEmpty(value);
            }
            get { return lbLabel.Text; }
        }
        
        public string CssClass
        {
            set 
            {
                string css = String.Format("dbField {0} {1}", ID, value).Trim(); 
                if (FTyp == TTyp.tb && tbValue.TextMode == TextBoxMode.MultiLine)
                    css += " " + cssMultiLine;
                paField.Attributes["class"] = css;
            }
            get { return paField.Attributes["class"]; }
        }

        public TTyp Type            // 
        {
            set { FTyp = value; }
            get { return FTyp; }
        }

        public bool Rq              // pole wymagane
        {
            set { FRq = value; }
            get { return FRq; }
        }

        public string _ValidationGroup   // na razie bez zastosowania ...
        {
            set 
            {
                switch (FTyp)
                {
                    case TTyp.lb:
                        break;
                    case TTyp.tb:
                        tbValue.ValidationGroup = value;
                        break;
                    case TTyp.ddl:
                        ddlValue.ValidationGroup = value;
                        break;
                    case TTyp.date:
                        deValue.EditBox.ValidationGroup = value;
                        break;
                    case TTyp.dt:
                        deValue.EditBox.ValidationGroup = value;
                        break;
                    case TTyp.check:
                        break;
                }
            }
            get 
            {
                switch (FTyp)
                {
                    case TTyp.tb:
                        return tbValue.ValidationGroup;
                    case TTyp.ddl:
                        return ddlValue.ValidationGroup;
                    case TTyp.date:
                        return deValue.EditBox.ValidationGroup;
                    case TTyp.dt:
                        return deValue.EditBox.ValidationGroup;
                    default:
                        return null;
                }
            }
        }

        public string ValidationGroup  
        {
            set
            {
                rqValidator.Enabled = !String.IsNullOrEmpty(value);
                rqValidator.ValidationGroup = value;
                tbValue.ValidationGroup = value;
            }
            get { return rqValidator.ValidationGroup; }
        }
        
        //----- ddl -----
        public string DataSourceID
        {
            set { FDataSourceID = value; }
            get { return FDataSourceID; }
        }

        public string ValueField
        {
            set { FValueField = value; }
            get { return FValueField; }
        }
        //----- ed -----
        public TextBoxMode TextMode
        {
            set { tbValue.TextMode = value; }
            get { return tbValue.TextMode; }
        }

        public int Rows
        {
            set { tbValue.Rows = value; }
            get { return tbValue.Rows; }
        }

        public int MaxLength
        {
            set { tbValue.MaxLength = value; }
            get { return tbValue.MaxLength; }
        }

        public string ValidChars
        {
            set
            {
                FilteredTextBoxExtender1.ValidChars = value;
                FilteredTextBoxExtender1.Enabled = !String.IsNullOrEmpty(value);
            }
            get { return FilteredTextBoxExtender1.ValidChars; }
        }

        public int Min
        {
            set { FMin = value; }
            get { return FMin; }
        }

        public int Max
        {
            set { FMax = value; }
            get { return FMax; }
        }

        //------- -----
        public DateEdit DeValue
        {
            get { return deValue; }
        }

        public TextBox TbValue
        {
            get { return tbValue; }
        }

        public DropDownList DdlValue
        {
            get { return ddlValue; }
        }

        public CheckBox CbValue
        {
            get { return cbValue; }
        }

        public Label LbValue
        {
            get { return lbValue; }
        }
        //---------------
        public int? AsInt
        {
            set 
            {
                string v = value == null ? null : value.ToString();
                lbValue.Text = v;
                tbValue.Text = v; 
            }
            get { return Tools.StrToInt(tbValue.Text); }
        }

        public double? AsDouble
        {
            set 
            {
                string v = value == null ? null : value.ToString();
                lbValue.Text = v;    
                tbValue.Text = v; 
            }
            get { return Tools.StrToDouble(tbValue.Text); }
        }

        public string AsString
        {
            set 
            {
                lbValue.Text = value;
                tbValue.Text = value; 
            }
            get { return tbValue.Text; }
        }

        public DateTime? AsDate
        {
            set 
            { 
                deValue.Date = value; 
            }
            get { return (DateTime?)deValue.Date; }
        }
        //----------------
        public string tmpValue  // jak nie jest widoczny to tu jest przechowywany lub text ddl
        {
            set { ViewState["tvalue"] = value; }
            get { return Tools.GetStr(ViewState["tvalue"]); }
        }

        //-----------
        public string GetValue(int no, bool asParam)
        {
            switch (FTyp)
            {
                case TTyp.lb:   // num ?
                    string v = lbValue.Text;
                    return asParam ? db.nullStrParam(v) : v;
                case TTyp.tb:   // num ?
                    v = tbValue.Visible ? tbValue.Text : lbValue.Text;
                    if (asParam)
                    {
                        if (tbValue.MaxLength != 0)
                            return db.nullStrParam(db.sqlPut(v, tbValue.MaxLength));
                        else
                            return db.nullStrParam(db.sqlPut(v));
                    }
                    else
                        return v;
                case TTyp.ddl:
                    if (no == 1)
                        v = ddlValue.Visible ? ddlValue.SelectedValue : tmpValue; //ddlSelectedValue;
                    else
                        v = ddlValue.Visible ? (ddlValue.SelectedItem != null ? ddlValue.SelectedItem.Text : null) : lbValue.Text;
                    return asParam ? db.nullStrParam(v) : v;
                case TTyp.check:
                    return cbValue.Checked ? "1" : "0";
                case TTyp.date:
                    v = DeValue.Visible ? DeValue.DateStr : lbValue.Text; // lbValue.Text;                    
                    return asParam ? db.nullStrParam(v) : v;
                case TTyp.dt:
                    v = DeValue.Visible ? DeValue.DateStr : tmpValue; // lbValue.Text;                    
                    return asParam ? db.nullStrParam(v) : v;
                default:
                    return null;
            }
        }

        public void SetValue(string value)
        {
            switch (FTyp)
            {
                case TTyp.lb:   // num ?
                    lbValue.Text = value;
                    break;
                case TTyp.tb:   // num ?
                    lbValue.Text = value;
                    tbValue.Text = value;
                    break;
                case TTyp.ddl:
                    tmpValue = value;
                    Tools.SelectItem(ddlValue, value);
                    lbValue.Text = ddlValue.SelectedValue;
                    break;
                case TTyp.check:
                    cbValue.Checked = value == "1";
                    break;
                case TTyp.date:
                    lbValue.Text = value;
                    deValue.DateStr = value;
                    break;
                case TTyp.dt:
                    tmpValue = value;
                    lbValue.Text = value;   // tu jest rzobijane na date i czas w span
                    deValue.DateStr = value;
                    break;
                default:
                    lbValue.Text = value;
                    break;
            }
        }

        public string Value   // ddl - selectedId
        {
            get { return GetValue(1, false); }
            set { SetValue(value); }
        }

        public string Value2  // ddl - selectedText
        {
            get { return GetValue(2, false); } 
        }

        public string dbValue
        {
            get { return GetValue(1, true); }
        }

        protected void ddlValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            TriggerChanged();
        }

        /*
        public string Value   // ddl - selectedId
        {
            get
            {
                switch (FTyp)
                {
                    case TTyp.lb: 
                        return lbValue.Text;
                    case TTyp.tb: 
                        return tbValue.Visible ? tbValue.Text : lbValue.Text;                        
                    case TTyp.ddl: 
                        return ddlValue.Visible ? ddlValue.SelectedValue : tmpValue; //ddlSelectedValue;
                    case TTyp.check: 
                        return cbValue.Checked ? "1" : "0";
                    case TTyp.date:
                        return DeValue.Visible ? DeValue.DateStr : lbValue.Text; // lbValue.Text;                    
                    case TTyp.dt:
                        return DeValue.Visible ? DeValue.DateStr : tmpValue; // lbValue.Text;                    
                    default:
                        return null;
                }
            }
        }

        public string Value2  // ddl - selectedText
        {
            get
            {
                switch (FTyp)
                {
                    case TTyp.ddl:
                        return ddlValue.Visible ? (ddlValue.SelectedItem != null ? ddlValue.SelectedItem.Text : null) : lbValue.Text; 
                    default:
                        return Value;
                }
            }
        }
         */ 
    }

}




















/*
public static bool GetValue(ref bool paVisible, Control cnt, DataRow dr, int vindex, int index, params string[] vis)  // ostatni vis to lista pól oddzielona przecinkiem !!!
{
    if (vis.Length > 0)
    {
        bool visible, edit;
        char v = getVisible(vindex, index, out visible, out edit, vis);  // nadmiarowo o 1 element ale to nic ...
        string fields = vis[vis.Length - 1];
        string[] fa = fields.Split(',');
        if (fa.Length > 0)
        {
            //----- panel -----
            Control pa = cnt.FindControl("pa" + fa[0]);
            if (pa != null) pa.Visible = visible;
            //----- edycja -----
            Control ed = cnt.FindControl("ed" + fa[0]);
            if (ed != null)
            {
                ed.Visible = visible && edit;
                if (dr != null)
                {
                    if (fieldExists(dr, fa[0]))
                    {
                        if (ed is TextBox) ((TextBox)ed).Text = db.getValue(dr, fa[0]);
                        else if (ed is DropDownList) Tools.SelectItem((DropDownList)ed, db.getValue(dr, fa[0]));
                        else if (ed is DateEdit) ((DateEdit)ed).Date = db.getDateTime(dr, fa[0]);
                        else if (ed is CheckBox)
                        {
                            CheckBox cb = ed as CheckBox;
                            cb.Checked = db.getBool(dr, fa[0], false);
                            if (!edit) cb.Enabled = false;
                        }
                    }
                    else
                        if (!edit && ed is CheckBox)
                        {
                            CheckBox cb = ed as CheckBox;
                            cb.Enabled = false;
                        }
                }
            }
            //----- podgląd -----
            Control lb = cnt.FindControl("lb" + fa[0]);
            if (lb != null)
            {
                bool vv = visible && !edit;
                lb.Visible = vv;
                if (vv && dr != null)
                {
                    if (fa.Length > 1)
                    {
                        if (fieldExists(dr, fa[1]))
                            ((Label)lb).Text = db.getValue(dr, fa[1]);
                    }
                    else
                    {
                        if (fieldExists(dr, fa[0]))
                            ((Label)lb).Text = db.getValue(dr, fa[0]);
                    }
                }
            }
        }
        if (visible) paVisible = true;
        return visible;
    }
    return false;
}
 */

/*
public static bool GetValue(DataRow dr, int vindex, int index, params string[] vis)  // ostatni vis to lista pól oddzielona przecinkiem !!!
{
    if (vis.Length > 0)
    {
        bool visible, edit;
        char v = getVisible(vindex, index, out visible, out edit, vis);  // nadmiarowo o 1 element ale to nic ...
        string fields = vis[vis.Length - 1];
        GetValue(dr, visible, edit, fields);
        return visible;
    }
    return false;
}

public static void GetValue(DataRow dr, string fields)  // lista pól oddzielona przecinkiem !!!
{
    GetValue(dr, true, false, fields);
}
*/

