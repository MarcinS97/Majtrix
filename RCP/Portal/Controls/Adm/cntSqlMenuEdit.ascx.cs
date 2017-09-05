using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls.Adm
{
    public partial class cntSqlMenuEdit : System.Web.UI.UserControl
    {
        public event EventHandler Save;

        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(gvPola);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.MakeConfirmButton(btPolaDefault, "Potwierdź operację. Dotychczasowe wartości zostaną nadpisane dmyślnymi.");
            }
        }

        private void TriggerSave()
        {
            if (Save != null)
                Save(this, EventArgs.Empty);
        }

        public void Show(string id, string parentid, string grupa)
        {
            Id = id;
            ParentId = parentid;
            Grupa = grupa;
            FillData();
            cntModal.Show(false);
        }

        private void FillData()
        {
            DataRow dr;
            bool ins = String.IsNullOrEmpty(Id);
            ltTitleEdit.Visible = !ins;
            lbTitleNazwa.Visible = !ins;
            ltTitleInsert.Visible = ins;
            btDelete.Visible = !ins;
            if (ins)
            {
                //----- new -----
                dr = db.Select.Row(dsSqlMenu, db.NULL, db.nullParam(ParentId), db.nullStrParam(Grupa));
            }
            else
            {
                //----- update -----
                dr = db.Select.Row(dsSqlMenu, Id, db.NULL, db.NULL);
                lbTitleNazwa.Text = db.getValue(dr, "MenuText");   // można by jeszcze js zapuścić, który by to synchronizował ... 
            }
            string pola = db.getValue(dr, "Pola");
            Pola = pola;
            dbField.ApplyStVisible(this, pola);  // musi być przed FillData
            dbField.FillData(this, dr, 0, 0, 0, dbField.moEdit);

            btDelete.Visible = !ins;
            if (ins) Tools.MakeConfirmDeleteRecordButton(btDelete);
        }

        private bool Validate()
        {
            return dbField.Validate(this);
        }

        private bool DoSave()
        {
            const string table = "SqlMenu";
            int ret = 0;
            string info = null;
            if (String.IsNullOrEmpty(Id))
            {
                ret = dbField.dbInsert(db.conP, this, table, null, null);
                info = db.LastInsertSql;
            }
            else
            {
                ret = dbField.dbUpdate(db.conP, this, table, "Id=" + Id, null, null) ? 1 : -3;
                info = db.LastUpdateSql;
            }
            if (ret >= 0)
            {
                Log.Info(Log.PORTAL_ADMIN, "SqlMenu.Save", info);
                return true;
            }
            else
            {
                Tools.ShowError("Wystąpił błąd podczas zapisu.");
                return false;
            }
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            if (Validate())
                if (DoSave())
                {
                    cntModal.Close();
                    TriggerSave();
                }
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {

        }

        protected void cntSqlTabs_SelectTab(object sender, EventArgs e)
        {

        }

        private string PolaToEdit(string pola)
        {
            return pola.Replace(",", "\r\n");
        }

        private string EditToPola(string edit)
        {
            return edit.Replace("\r\n", ",").Replace("\n", ",");
        }

        protected void btPola_Click(object sender, EventArgs e)
        {
            //gvPola.DataBind();
            if (String.IsNullOrEmpty(Pola))
                tbPola.Text = tbPolaDefault.Text;
            else
                tbPola.Text = PolaToEdit(Pola);
            cntModalPola.Show(false);
        }

        protected void btPolaSave_Click(object sender, EventArgs e)
        {
            string pola = EditToPola(tbPola.Text);  // uwaga - brak zabezpieczenia sql
            Pola = pola;
            bool ok = db.execSQL(db.conP, String.Format(dsPolaUpdate.UpdateCommand, Grupa, pola));
            if (ok)
            {
                cntModalPola.Close();
                FillData();
            }
            else
                Tools.ShowError("Wystąpił błąd podczas zapisu.");
        }

        protected void btPolaDefault_Click(object sender, EventArgs e)
        {
            tbPola.Text = tbPolaDefault.Text;
        }

        //----------
        private string Id
        {
            //set { ViewState["id"] = value; }
            //get { return Tools.GetStr(ViewState["id"]); }
            set { hidId.Value = value; }
            get { return hidId.Value; }
        }

        private string ParentId
        {
            set { ViewState["parentid"] = value; }
            get { return Tools.GetStr(ViewState["parentid"]); }
        }

        private string Grupa
        {
            set { hidGrupa.Value = value; }
            get { return hidGrupa.Value; }
        }

        private string Pola
        {
            set { hidPola.Value = value; }
            get { return hidPola.Value; }
        }

        protected void gvPolaCmd_Click(object sender, EventArgs e)
        {
            string[] p = Tools.GetLineParams(gvPolaCmdPar.Value);
            switch (p[0])
            {
                case "status":
                    if (p.Length == 1 + 3)
                    {
                        int cid = Tools.StrToInt(p[1], -1);
                        string st = p[2].Substring(0, 1);
                        string stnext = p[3].Substring(0, 1);
                        Dictionary<string, string> pp = dbField.GetPola(Pola);
                        string k = db.getScalar(db.conP, String.Format(dsPolaUpdate.SelectCommand, cid));
                        if (pp == null) pp = new Dictionary<string,string>();
                        pp[k] = stnext;
                        string pola = dbField.SetPola(pp);
                        Pola = pola;
                        bool ok = db.execSQL(db.conP, String.Format(dsPolaUpdate.UpdateCommand, Grupa, pola));
                        if (!ok) Tools.ShowError("Wystąpił błąd podczas zapisu.");
                    }
                    break;
            }
        }

    }
}
/*

using HRRcp.App_Code;
using HRRcp.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Scorecards.Odbicia
{
    public partial class cntKafelEdit : System.Web.UI.UserControl
    {
        public event EventHandler Save;
        //public event cntSciezki.SciezkiCommandEventHandler SciezkaCommand;

        public const int moBlueKafel  = 0;
        public const int moGreenKafel = 1;

        private const string tabPAR = "PAR";
        private const string tabWE  = "WE";
        private const string tabWY  = "WY";

        protected void Page_Load(object sender, EventArgs e)
        {
            Tools.ExecOnStart2("dpick", String.Format("prepareCalendar2('{0}');", paKafelEdit.ClientID));
            if (!IsPostBack)
            {
                Tools.MakeConfirmButton(btDelete, "Potwierdź usunięcie kafla.");
                cntSciezkiWe.AddButton.Visible = false;
                cntSciezkiWy.AddButton.Visible = false;
            }
        }

        public void Show(string kid, string parentId, string sctypid)
        {
            KafelId = kid;
            ScTypeId = sctypid;
            ParentId = parentId;
            FillData();
            //if (String.IsNullOrEmpty(parentId))     // kafel niebieski
            if (Mode == moBlueKafel)
            {
                cntSqlTabs.Visible = true;
                SelectTab();
                cntSciezkiWe.KafelId = kid;
                cntSciezkiWy.KafelId = kid;
                cntSciezkiWe.Refresh();
                cntSciezkiWy.Refresh();
                cntModal.Focus(ddlTryb.DdlValue);
            }
            else                                    // kafel zielony
            {
                cntSqlTabs.Visible = false;
                btSciezkaAdd.Visible = false;
                mvData.ActiveViewIndex = 0;
                cntModal.Focus(ddlRodzaj.DdlValue);
            }
            cntModal.Show(false);
        }

        private void FillData()
        {
            DataRow dr;
            bool ins = String.IsNullOrEmpty(KafelId);
            ltTitleEdit.Visible = !ins;
            lbTitleNazwa.Visible = !ins;
            ltTitleInsert.Visible = ins;
            btDelete.Visible = !ins;
            if (ins)
            {
                //----- new -----
                dr = db.Select.Row(dsKafel, db.NULL, db.nullParam(ParentId), db.nullParam(ScTypeId));
            }
            else
            {
                //----- update -----
                dr = db.Select.Row(dsKafel, KafelId, db.NULL, db.NULL);
                lbTitleNazwa.Text = db.getValue(dr, "Nazwa");   // można by jeszcze js zapuścić, który by to synchronizował ... 
            }
            //KafelOd = (DateTime)db.getDateTime(dr, "DataOd");
            string tid = db.getValue(dr, "IdTypuArkuszy");
            dbField.FillData(this, dr, 0, 0, Mode, dbField.moEdit);   // Mode 0 - niebieski, 1 - zielony
        }

        private void TriggerSave()
        {
            if (Save != null)
                Save(this, EventArgs.Empty);
        }

        //private void TriggerSciezkaCommand(string cmd, string sid)
        //{
        //    if (SciezkaCommand != null)
        //        SciezkaCommand(this, new cntSciezki.SciezkiCommandEventArgs(cmd, sid));
        //}

        protected void btDelete_Click(object sender, EventArgs e)
        {
            //DataRow dr = db.Select.Row(dsKafel.InsertCommand, KafelId, db.nullParam(ParentId)); //db.nullParam(ParentId));  wywala się z błędem jak nie znajdzie rekordu
            DataRow dr = db.getDataRow(String.Format(dsKafel.InsertCommand, KafelId, db.nullParam(ParentId)));
            if (dr != null)
                Tools.ShowError("Istnieją rejestracje dla kafla. Usunięcie niemożliwe.");
            else
            {
                bool ok = db.Execute(dsKafel.DeleteCommand, KafelId);
                if (ok)
                {
                    Log.Info(Log.SCARDS_ODBICIA_KAFLE, "Kafel.Delete", KafelId);
                    cntModal.Close();
                    TriggerSave();
                }
                else
                    Tools.ShowError("Wystąpił błąd podczas usuwania kafla.");
            }
        }

        private void SelectTab()
        {
            Tools.SelectTab(cntSqlTabs.Tabs, mvData, "kedit_seltab", false);
            switch (cntSqlTabs.SelectedValue)
            {
                default:
                    btDelete.Visible = !String.IsNullOrEmpty(KafelId);  //insert
                    btSciezkaAdd.Visible = false;
                    break;
                case tabWE:
                case tabWY:
                    btDelete.Visible = false;   // zeby się nie myliło z usunięciem ścieżki
                    btSciezkaAdd.Visible = true;
                    break;
            }
        }

        protected void cntSqlTabs_SelectTab(object sender, EventArgs e)
        {
            SelectTab();
        }

        //------------------------------
        private string KafelId
        {
            set { ViewState["kafid"] = value; }
            get { return Tools.GetStr(ViewState["kafid"]); }
        }

        private string ScTypeId
        {
            set { ViewState["sctypid"] = value; }
            get { return Tools.GetStr(ViewState["sctypid"]); }
        }

        private string ParentId
        {
            set { ViewState["parentid"] = value; }
            get { return Tools.GetStr(ViewState["parentid"]); }
        }

        //private bool Validate()
        //{
        //    DateTime? od = (DateTime)deDataOd.AsDate;
        //    if (od == null) Tools.ShowError("Niepoprawna data");
        //    else
        //    {
        //        if (!String.IsNullOrEmpty(KafelId) && od < KafelOd) Tools.ShowError("Data wcześniejsza niż poprzedni okres obowiązywania danych.");
        //        else
        //            return dbField.Validate(this);
        //    }
        //    return false;
        //}

        private bool Validate()
        {
            return dbField.Validate(this);
        }

        private bool DoSave()
        {
            const string scKafle = "scKafle";
            int ret = 0;
            string info = null;
            if (String.IsNullOrEmpty(KafelId))
            {
                ret = dbField.dbInsert(this, scKafle, null, null);
                info = db.LastInsertSql;
            }
            else
            {
                ret = dbField.dbUpdate(this, scKafle, "Id=" + KafelId, null, null) ? 1 : -3;
                info = db.LastUpdateSql;
            }
            if (ret >= 0)
            {
                Log.Info(Log.SCARDS_ODBICIA_KAFLE, "Kafel.Save", info);
                return true;
            }
            else
            {
                Tools.ShowError("Wystąpił błąd podczas zapisu.");
                return false;
            }
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            if (cntSqlTabs.SelectedValue != tabPAR)
            {
                cntSqlTabs.SelectedValue = tabPAR;  // nie widzi dbFieldów i zwraca null w liście nazw pól do update, na razie takie rozwiązanie ...
                SelectTab();
            }

            if (Validate())
                if (DoSave())
                {
                    cntModal.Close();
                    TriggerSave();
                }
        }

        protected void btSciezkaAdd_Click(object sender, EventArgs e)
        {
            string fromid = cntSqlTabs.SelectedValue == tabWE ? null : KafelId;
            string toid   = cntSqlTabs.SelectedValue == tabWY ? null : KafelId;
            cntSciezkaEdit.Show(cntSciezki.cAdd, null, null, fromid, toid);
        }

        protected void cntSciezkaEdit_Refresh(object sender, EventArgs e)
        {
            cntSciezkiWe.Refresh();
            cntSciezkiWy.Refresh();
        }

        protected void cntSciezkiWe_Command(object sender, HRRcp.Scorecards.Odbicia.cntSciezki.SciezkiCommandEventArgs e)
        {
            cntSciezkaEdit.Show(e.cmd, e.SciezkaId, e.par, null, null);
        }

        protected void cntSciezkiWy_Command(object sender, HRRcp.Scorecards.Odbicia.cntSciezki.SciezkiCommandEventArgs e)
        {
            cntSciezkaEdit.Show(e.cmd, e.SciezkaId, e.par, null, null);
        }
        //-------------
        public int Mode
        {
            get;
            set;
        }

        //public DateTime KafelOd
        //{
        //    set { ViewState["kafelod"] = value; }
        //    get { return Tools.GetDateTime(ViewState["kafelod"], DateTime.Today); }
        //}

    }
}
*/