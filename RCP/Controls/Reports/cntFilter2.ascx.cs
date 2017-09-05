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
    public partial class cntFilter2 : System.Web.UI.UserControl
    {
        public event EventHandler Clear;
        public event EventHandler Hide;
        public event EventHandler Filter;
        public event EventHandler Edit;
        public event EventHandler EndEdit;
        public event EventHandler SelectedChanged;

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
                bool sup = App.User.HasRight(AppUser.rSuperuser);
                //bool sup = Page.User.IsInRole(Utils.rRaportyAdm);
                bool c2 = FColumns == 2;
                btEdit.Visible = sup;
                if (sup) paFilter.Visible = true;
                SetVisibleFilter2(c2);
            }
        }

        private void SetButtons()
        {

        }

        private void SetVisibleFilter2(bool visible)
        {
            tdCol1.RowSpan = visible ? 2 : 1;
            tdSep1.Visible = visible;
            tdCol2.Visible = visible;
        }

        private void TriggerClear()
        {
            if (Clear != null)
                Clear(this, EventArgs.Empty);
        }

        private void TriggerHide()
        {
            if (Hide != null)
                Hide(this, EventArgs.Empty);
        }

        private void TriggerFilter()
        {
            if (Filter != null)
                Filter(this, EventArgs.Empty);
        }

        private void TriggerEdit()
        {
            if (Edit != null)
                Edit(this, EventArgs.Empty);
        }

        private void TriggerEndEdit()
        {
            if (EndEdit != null)
                EndEdit(this, EventArgs.Empty);
        }

        private void TriggerSelectedChanged()
        {
            if (SelectedChanged != null)
                SelectedChanged(this, EventArgs.Empty);
        }
        //-----------------------
        public bool ApplyTo(SqlDataSource ds)  // czy się coś zmieniło
        {
            bool b1 = cntFilterFields1.ApplyTo(ds);
            bool b2 = cntFilterFields2.ApplyTo(ds);
            return b1 || b2;
        }

        public string ApplyTo(string sql)
        {
            //sql = cntFilterFields1.ApplyTo(sql);
            //sql = cntFilterFields2.ApplyTo(sql);
            return sql;
        }
        //-----------------------
        protected bool IsTyp(object data, params int[] typy)
        {
            DataRowView drv = (DataRowView)data;
            int typ = (int)drv["Typ"];
            return typy.Any(a => a == typ);
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
            /*
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //DataRowView drv = e.Item.DataItem as DataRowView;
                cntField ff = e.Item.FindControl("cntField") as cntField;
                if (ff != null) ff.Prepare(e.Item.DataItem);
            }
             */ 
        }

        protected void rpFilter_Load(object sender, EventArgs e)
        {

        }
        //----------------------------

        private void Move(int dir)
        {
            bool ok = cntFilterFields1.Move(dir);
            if (!ok)
                ok = cntFilterFields2.Move(dir);
            if (ok)
                SetUpDnEnabled();
        }
        //----------------------------
        protected void btWyszukaj_Click(object sender, EventArgs e)
        {
            // po prostu robi postback
            TriggerFilter();
        }

        protected void btClear_Click(object sender, EventArgs e)
        {
            cntFilterFields1.Clear();
            cntFilterFields2.Clear();
            TriggerClear();
            //foreach (Control cnt in paFilter.Controls)
            //{
            //    if (cnt is TextBox) (cnt as TextBox).Text = null;
            //    else if (cnt is DateEdit) (cnt as DateEdit).Date = null;
            //    else if (cnt is DropDownList) Tools.SelectItem(cnt as DropDownList, null);
            //    else if (cnt is CheckBox) (cnt as CheckBox).Checked = false;
            //}
        }

        protected void btUp_Click(object sender, EventArgs e)
        {
            Move(-1);
        }

        protected void btDn_Click(object sender, EventArgs e)
        {
            Move(1);
        }

        private void SetUpDnEnabled()
        {
            int sel = cntFilterFields1.List.SelectedIndex;
            int last = -1;
            if (sel != -1)
                last = cntFilterFields1.List.Items.Count - 1;
            else
            {
                sel = cntFilterFields2.List.SelectedIndex;
                if (sel != -1)
                    last = cntFilterFields2.List.Items.Count - 1;
            }
            //btFirst.Enabled = sel > 0;
            btUp.Enabled = sel > 0;
            btDn.Enabled = sel < last;
            //btLast.Enabled = sel < last;
        }

        private void SetEditMode(bool edit)
        {
            EditMode = edit;
            cntFilterFields1.EditMode = edit;
            cntFilterFields2.EditMode = edit;
            btEdit.Visible = !edit;
            btEndEdit.Visible = edit;
            btAddField.Visible = edit;
            btUp.Visible = edit;
            btDn.Visible = edit;
            if (edit) SetUpDnEnabled();
            else
            {
                cntFilterFields1.List.SelectedIndex = -1;
                cntFilterFields2.List.SelectedIndex = -1;
            }
        }

        protected void btEdit_Click(object sender, EventArgs e)
        {
            SetEditMode(true);
            TriggerEdit();
        }

        protected void btEndEdit_Click(object sender, EventArgs e)
        {
            SetEditMode(false);
            TriggerEndEdit();
        }

        protected void btAddField_Click(object sender, EventArgs e)
        {
            cntFilterFields1.Insert();
        }

        protected void btHide_Click(object sender, EventArgs e)
        {
            TriggerHide();
        }
        //-----------------------------
        private void SetButtonsVisible(bool set)
        {
            if (set)
            {
                btWyszukaj.Visible = true;
                btClear.Visible = true;
                //btHide.Visible = true;
            }
        }

        public bool Present = false;  // troche plomba ...

        protected void cntFilterFields1_DataBound(object sender, EventArgs e)
        {
            paFilter.Visible = true;
            bool f1 = cntFilterFields1.List.Items.Count > 0;
            if (f1) Present = true;
            SetButtonsVisible(f1);
        }

        protected void cntFilterFields2_DataBound(object sender, EventArgs e)
        {
            paFilter.Visible = true;
            bool f2 = cntFilterFields2.List.Items.Count > 0;
            if (f2) Present = true;
            SetButtonsVisible(f2);
            SetVisibleFilter2(f2);
        }

        protected void cntFilterFields1_SelectedChanged(object sender, EventArgs e)
        {
            cntFilterFields2.List.SelectedIndex = -1;
            SetUpDnEnabled();
            TriggerSelectedChanged();           
        }

        protected void cntFilterFields2_SelectedChanged(object sender, EventArgs e)
        {
            cntFilterFields1.List.SelectedIndex = -1;
            SetUpDnEnabled();
            TriggerSelectedChanged();           
        }
        //-----------------------------
        int FColumns = 1;

        public int Columns
        {
            set { FColumns = value; }
            get { return FColumns; }
        }

        public string ReportId
        {
            set 
            {
                cntFilterFields1.ReportId = value;
                cntFilterFields2.ReportId = value; 
            }
            get { return cntFilterFields1.ReportId; }
        }

        public bool EditMode
        {
            set { ViewState["editm"] = value; }
            get { return Tools.GetBool(ViewState["editm"], false); }
        }
    }
}