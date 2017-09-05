using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls.PodzialLudzi
{
    public partial class cntKalkulator : System.Web.UI.UserControl
    {
        protected const string cmdAdd = "add";
        protected const string cmdRem = "rem";
        protected const string cmdMonth = "month";
        protected const string cmdConfirm = "confirm";

        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(gvPracownicy);
            Grid.Prepare(gvSelected);
            //Grid.Prepare(gvResultHeader);
            Grid.Prepare(gvResultPodzial);
            Grid.Prepare(gvResultPracownicy);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlMiesiac.DataBind();
                PrepareSearch();
                Tools.MakeConfirmButton(btClearSel, "Potwierdź usunięcie wszystkich wybranych pracowników.");
            }
            tbKwota.Text = tbKwota.Text.Replace(',', '.');
            lbtRecord.OnClientClick = String.Format("startRecording('{0}','{1}'); return false;", tbSearch.ClientID, "formRecord");
        }

        private void StartRecording()
        {
            Tools.ExecOnStart2("record", String.Format("if (recActive) startRecording('{0}','{1}');", tbSearch.ClientID, "formRecord"));
        }

        private void ClearStartRecording()
        {
            tbSearch.Text = null; // leży poza update panel wiec niby nic nie da, ale sie SqlDatasource nie pobindują
            Tools.ExecOnStart2("record", String.Format("if (recActive) {{ $('#{0}').val('').focus(); startRecording('{0}','{1}'); }}", tbSearch.ClientID, "formRecord"));
        }

        //protected override void OnPreRender(EventArgs e)
        //{
        //    if (!IsPostBack)
        //        PrepareSearch();
        //    base.OnPreRender(e);
        //}

        public bool Prepare()
        {
            if (App.User.HasRight(AppUser.rPodzialLudziKalkulator))
            {
                return true;
            }
            return false;
        }
        //-----
        private void PrepareSearch()
        {
            ////btClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('');doClick('{1}');return false;", tbSearch.ClientID, btSearch.ClientID);
            btnClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('').focus();return false;", tbSearch.ClientID);
            Tools.ExecOnStart2("searchtrigprac", String.Format("startSearch('{0}','{1}');", tbSearch.ClientID, btSearch.ClientID));
            tbSearch.Focus();
        }

        public void FocusSearch()
        {
            tbSearch.Focus();
        }

        private void ClearSearch(bool setFocus)
        {
            Tools.ExecOnStart2("clearsearch", String.Format(@"$('#{0}').val(''){1};", tbSearch.ClientID, setFocus ? ".focus()" : ""));            
        }
        //-----------------------
        protected bool startsWith(string cmd, out string rest, params string[] tokenlist)
        {
            foreach (string token in tokenlist)
            {
                if ((cmd + " ").StartsWith(token + " "))
                {
                    rest = cmd.Substring(token.Length).TrimStart();
                    return true;
                }
            }
            rest = null;
            return false;
        }

        protected void ShowLines(string lines)
        {
            string sel = null;
            if (lines.IsAny("wszystko", "all"))
            {
                ListItem li = ddlLines.Items.FindByValue("all");
                if (li != null)
                    li.Selected = true;
            }
            else
            {
                int ln = Tools.StrToInt(lines.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0], -1);
                foreach (ListItem item in ddlLines.Items)
                    if (Tools.StrToInt(item.Value, -2) >= ln)
                    {
                        item.Selected = true;
                        break;
                    }
            }
        }

        protected void Sortuj(string by)
        {
            string[] bb = by.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            SortDirection dir = SortDirection.Ascending;
            string sort = null;
            for (int i = 0; i < bb.Length; i++)
            {
                string s = bb[i];
                if (s.IsAny("rosnąco", "ascending", "asc"))
                    dir = SortDirection.Ascending;
                else if (s.IsAny("malejąco", "descending", "desc"))
                    dir = SortDirection.Descending;
                else if (s.IsAny("numer", "numerze", "logo"))
                    sort = "[Nr ew.]";
                else if (s.IsAny("pracownik", "pracowniku", "nazwisko", "nazwisku", "imię"))
                    sort = "[Pracownik]";
            }
            if (!String.IsNullOrEmpty(sort))
                gvPracownicy.Sort(sort, dir);
        }

        protected virtual bool Recognize(string cmd)
        {
            return false;
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            string cmd = tbSearch.Text.Trim().ToLower();
            if (Recognize(cmd))
                ClearStartRecording();
            else
            {
                string[] p = cmd.Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
                bool num = p.All(n => char.IsDigit(n, 0));
                if (num)
                    tbSearch.Text = String.Join("", p);
            }
        }

        //--------------
        public int RowsCount
        {
            set { ViewState["gvrows"] = value; }
            get { return Tools.GetInt(ViewState["gvrows"], 0); }
        }

        public int RowsCountSel
        {
            set { ViewState["gvrowssel"] = value; }
            get { return Tools.GetInt(ViewState["gvrowssel"], 0); }
        }

        protected void ddlLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ln = ddlLines.SelectedValue;
            if (ln == "all")
            {
                //gvPracownicy.AllowPaging = false;
                gvPracownicy.PageSize = RowsCount;
            }
            else
            {
                //gvPracownicy.AllowPaging = true;
                gvPracownicy.PageSize = Tools.StrToInt(ln, 20);
            }
        }
        //--------------
        protected void ddlMiesiac_DataBound(object sender, EventArgs e)
        {
            foreach (ListItem li in ddlMiesiac.Items)
            {
                string sel = Tools.GetLineParam(li.Value, 1);
                if (sel == "x")
                {
                    li.Selected = true;
                    li.Value = li.Value.Substring(0, li.Value.Length - 2);
                    break;
                }
            }
        }

        protected void tabResult_MenuItemClick(object sender, MenuEventArgs e)
        {
            Tools.SelectTab(tabResult, mvResult, null, false);
        }

        //--------------
        protected void dsPracownicy_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            RowsCount = e.AffectedRows;
            lbCount.Text = e.AffectedRows.ToString();

            if (e.AffectedRows != 1)
                StartRecording();
        }

        protected void gvPracownicy_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (cbAutoAdd.Checked && !String.IsNullOrEmpty(tbSearch.Text) && RowsCount == 1)
                {
                    //DataRow row = ((DataRowView)e.Row.DataItem).Row;
                    //string id = row.Field<String>(0);
                    string id = db.getValue(((DataRowView)e.Row.DataItem).Row, 0);

                    string par = String.Format("add|{0}", id);
                    gvPracownicyCmdPar.Value = par;
                    Tools.ExecOnStart2("autoadd", String.Format(@"
setTimeout(function(){{
    var v = $('#{1}').val();
    if (v == '{2}')
        doClick('{0}');
}}, 10);
                    ", gvPracownicyCmd.ClientID, gvPracownicyCmdPar.ClientID, par));   // można dać animację
                }
            }
        }

        protected void UpdateKwota()
        {
            int cnt = RowsCountSel;
            if (cnt > 0)
                lbKwotaOs.Text = Math.Round((Tools.StrToDouble(tbKwota.Text, 0.0) / cnt), 4).ToString();
            else
                lbKwotaOs.Text = "0";
        }
        //-----
        protected void PrevMonth()
        {
            DateTime? dt = Tools.StrToDateTime(ddlMiesiac.SelectedValue);
            if (dt != null)
                SelectMonth(((DateTime)dt).AddMonths(-1));
        }

        protected void NextMonth()
        {
            DateTime? dt = Tools.StrToDateTime(ddlMiesiac.SelectedValue);
            if (dt != null)
                SelectMonth(((DateTime)dt).AddMonths(1));
        }

        protected void SelectMonth(DateTime dt)
        {
            if (dt.Day != 1)
                dt = dt.AddDays(-dt.Day + 1);
            string v = DateToStr(dt);
            string t = SelectItem(ddlMiesiac, v);
            if (String.IsNullOrEmpty(t))
            {
                ddlMiesiac.Items.Insert(0, new ListItem(v.Substring(0, 7), v));
                ddlMiesiac.Items[0].Selected = true;
            }
        }
        //-----
        protected void tbKwota_TextChanged(object sender, EventArgs e)
        {
            UpdateKwota();
            DoUpdate = true;
        }

        protected void dsSelected_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            RowsCountSel = e.AffectedRows;
            lbCountSel.Text = e.AffectedRows.ToString();
            btClearSel.Enabled = e.AffectedRows > 0;
            UpdateKwota();
        }

        protected void dsResultPracownicy_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
        }

        protected void dsResultPodzial_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
        }

        //--------------
        public string LastCmd
        {
            set { ViewState["lastcmd"] = value; }
            get { return Tools.GetStr(ViewState["lastcmd"]); }
        }

        public string LastId
        {
            set { ViewState["lastid"] = value; }
            get { return Tools.GetStr(ViewState["lastid"]); }
        }
        
        protected void PracAdd(string pid)
        {
            if (!gvPracownicySelected.Value.StartsWith(","))
                gvPracownicySelected.Value = "," + gvPracownicySelected.Value;
            gvPracownicySelected.Value += pid + ",";
            gvSelectedSelected.Value = gvPracownicySelected.Value;
            DoUpdate = true;
            LastCmd = cmdAdd;
            LastId = pid;
        }

        protected void PracRem(string pid)
        {
            gvPracownicySelected.Value = gvPracownicySelected.Value.Replace(String.Format(",{0},", pid), ",");
            if (gvPracownicySelected.Value == ",")
                gvPracownicySelected.Value = null;
            gvSelectedSelected.Value = gvPracownicySelected.Value;
            DoUpdate = true;
            LastCmd = cmdRem;
            LastId = pid;
        }

        protected void ClearAll()
        {
            gvPracownicySelected.Value = null;
            DoUpdate = true;
        }

        protected void gvPracownicyCmd_Click(object sender, EventArgs e)
        {
            string[] par = Tools.GetLineParams(gvPracownicyCmdPar.Value);
            switch (par[0])
            {
                case cmdAdd:
                    PracAdd(par[1]);
                    if (RowsCount == 1)
                    {
                        ClearSearch(true);
                        StartRecording();
                    }
                    break;
            }    
        }

        protected void gvSelectedCmd_Click(object sender, EventArgs e)
        {
            string[] par = Tools.GetLineParams(gvSelectedCmdPar.Value);
            switch (par[0])
            {
                case cmdRem:
                    PracRem(par[1]);
                    break;
            }    
        }

        protected void btClearSel_Click(object sender, EventArgs e)
        {
            ClearAll();
        }
        //---------------
        private string CsvName
        {
            get
            {
                string nazwa = tbNazwa.Text;
                if (String.IsNullOrEmpty(nazwa))
                    nazwa = "Podział"; 
                return String.Format("{0} - {1}", nazwa, ddlMiesiac.SelectedValue);
            }
        }

        protected void ExportCsv()
        {
            Tools.ExecOnStart2("excel", String.Format("doClick('{0}');", btExcelPodz.ClientID));
        }

        protected void btExcelPodz_Click(object sender, EventArgs e)
        {
            //Report.ExportCSV(CsvName, dsResultPodzial, null, null, true, false);
            Report.ExportCSV(CsvName, dsResultHeader, null, dsResultPodzial, null, dsSelected);//, dsResultPracownicy);    dsSep - trochę nieoptymalne, ale ...
        }

        protected void btExcelPrac_Click(object sender, EventArgs e)
        {
            //Report.ExportCSV(CsvName, dsResultPracownicy, null, null, true, false);
            Report.ExportCSV(CsvName, dsResultHeader, null, dsResultPodzial, null, dsResultPracownicy);
        }

        protected void btExcelSel_Click(object sender, EventArgs e)
        {
            Report.ExportCSV(CsvName, dsSelected, null, null, true, false);
        }
        //---------------
        private bool DoUpdate
        {
            set { ViewState["update"] = value; }
            get { return Tools.GetBool(ViewState["update"], false); }
        }

        protected void vPodzial_Activate(object sender, EventArgs e)
        {
            if (DoUpdate)
            {
                DoUpdate = false;  // druga zakładka jak jest widoczna to się sama odświeży
                gvResultPodzial.DataBind();
            }
        }

        protected void vPracownicy_Activate(object sender, EventArgs e)
        {
            if (DoUpdate)
            {
                DoUpdate = false;  // druga zakładka jak jest widoczna to się sama odświeży
                gvResultPracownicy.DataBind();
            }
        }
        //---------------
        protected string SelectItem(ListControl lc, string selectedValue)
        {
            return Tools.SelectItem(lc, selectedValue);
        }

        protected string DateToStr(DateTime dt)
        {
            return Tools.DateToStr(dt);
        }

        protected void SelectTab(View view)
        {
            int ret = Tools.SelectMenu(tabResult, view.ID);
            mvResult.SetActiveView(view);    
        }

        protected void ShowPaste()
        {
            cbPasteClear.Checked = false;
            cntModalPaste.Show(true);
            //tbPaste.Focus();   nie działa :(
            //Tools.ExecOnStart2("pastefocus", String.Format("$('#{0}').focus();", tbPaste.ClientID));
            Tools.ExecOnStart2("pastefocus", String.Format("setTimeout(function(){{$('#{0}').focus();}}, 1);", tbPaste.ClientID));
        }

        protected void btPasteShow_Click(object sender, EventArgs e)
        {
            ShowPaste();
        }

        protected string[] GetNumbers(string s)
        {
            return Regex.Matches(s, "(-?[0-9]+)").OfType<Match>().Select(m => m.Value.ToString()).ToArray();
        }

        private bool Paste()
        {
            bool ret = false;
            string[] num = GetNumbers(tbPaste.Text);
            string nn = String.Join(",", num);
            if (!String.IsNullOrEmpty(nn))
            {
                DataSet ds = db.Select.Set(dsPasteCheck.SelectCommand, nn);
                string err = db.Join(ds, "Error", ", ", 0, 20, false);
                string sel = db.Join(ds, "Id", ",", 0, 500, false);   // jakieś ograniczenie powinno być
                if (!String.IsNullOrEmpty(sel))
                {
                    if (cbPasteClear.Checked || String.IsNullOrEmpty(gvPracownicySelected.Value))
                        gvPracownicySelected.Value = "," + sel + ",";
                    else
                        gvPracownicySelected.Value += sel + ",";
                    ret = true;
                }
                if (!String.IsNullOrEmpty(err))
                {
                    Tools.ShowWarning("Nie znaleziono pracowników o następujących numerach ewidencyjnych:\n{0}", err);
                }
            }
            cntModalPaste.Close();
            return ret;
        }

        protected void btPaste_Click(object sender, EventArgs e)
        {
            if (Paste())
                DoUpdate = true;
        }
        //---------------
        protected const int cmDeleteAll= 1;

        public int ConfirmCmd
        {
            set { ViewState["conficmd"] = value; }
            get { return Tools.GetInt(ViewState["conficmd"], -1); }
        }
        
        protected void ShowConfirm(int cmd, string question)
        {
            ConfirmCmd = cmd;
            lbConfirm.Text = question;
            modalConfirm.Show(false);
        }

        protected bool IsConfirm()
        {
            //return modalConfirm.Visible;
            return LastCmd == cmdConfirm;
        }

        protected void Confirm(bool ok)
        {
            switch (ConfirmCmd)
            {
                case cmDeleteAll:
                    if (ok)
                        ClearAll();
                    modalConfirm.Close();
                    LastCmd = null;
                    break;
            }
        }

        protected void btConfirmOk_Click(object sender, EventArgs e)
        {
            Confirm(true);
        }

        //---------------
    }
}