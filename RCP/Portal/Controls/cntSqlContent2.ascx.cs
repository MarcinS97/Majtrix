using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;
using HRRcp.Controls.Reports;

namespace HRRcp.Portal.Controls
{
    public partial class cntSqlContent2 : System.Web.UI.UserControl
    {
        public event EventHandler SelectTab;

        public const int moLines    = 1;
        public const int moScreen   = 2;
        public const int moMDLines  = 3;    // master lines - details lines  <<< rozwój :)
        public const int moMDScreen = 4;    // master lines - details lines


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string grupa = Grupa;
                if (!String.IsNullOrEmpty(grupa))
                    LoadTabs();
                if (App.User.HasRight(AppUser.rSuperuser))
                    paEdit.Visible = true;





                paEdit2.Visible = Lic.wnZmianaDanych;
            }
        }

        public void Prepare(string grupa)
        {
            Grupa = grupa;
            LoadTabs();
        }

        public void LoadTabs()
        {
            const string item = "<div class=\"tabCaption\"><div class=\"tabLeft\"><div class=\"tabRight\">{0}</div></div></div>";
#if CO || PRON  // docelowo wszystkie tędy, tylko na razie IQOR jest włączony z CO
            DataSet ds = db.Select.Set(dsTabs.SelectCommand, App.dbPORTAL, Grupa, App.User.Rights);
#else
            DataSet ds = db.getDataSet(String.Format("select * from {0}..SqlContent where Grupa = '{1}' and Aktywny = 1 order by Kolejnosc, MenuText", App.dbPORTAL ,Grupa));
#endif
            tabContent.Items.Clear();
            foreach (DataRow dr in db.getRows(ds))
                tabContent.Items.Add(new MenuItem(String.Format(item, db.getValue(dr, "MenuText")), db.getValue(dr, "Id")));
            if (tabContent.Items.Count > 0)
            {
                tabContent.Items[0].Selected = true;
                DoSelectTab();
            }
        }

        private void TriggerSelectTab()
        {
            if (SelectTab != null)
                SelectTab(this, EventArgs.Empty);
        }

        //---- parametry do OnSelectTab
        public int Typ = -1;

        private void DoSelectTab()
        {
            string id = tabContent.SelectedValue;
            DataRow dr = db.getDataRow(String.Format("select * from {0}..SqlContent where Id = {1}", App.dbPORTAL, id));
            string constr = db.getValue(dr, "ConStr");
            string sql = db.getValue(dr, "Sql");
            int typ = db.getInt(dr, "Typ", moLines);


            //!!!!! uwaga plomba dla iQora dopóki się nie zaktualizauje bazy danych
            string empty = null;
            try { db.getValue(dr, "EmptyInfo"); }
            catch (Exception ex) {}
            if (String.IsNullOrEmpty(empty)) empty = "Brak danych";
            
            Typ = typ;
            cntMasterLines.Visible = false;
            cntMasterScreen.Visible = false;




            hidTabId.Value = id;
            hidTabType.Value = typ.ToString();





            switch (typ)
            {
                default:
                case moLines:
                    cntMasterLines.Visible = true;
                    cntMasterLines.SQL = sql;
                    cntMasterLines.ConStr = constr;
                    cntMasterLines.NoDataInfo = empty;
                    TriggerSelectTab();
                    cntMasterLines.Prepare();
                    break;
                case moScreen:
                    cntMasterScreen.Visible = true;
                    cntMasterScreen.SQL = sql;
                    cntMasterScreen.ConStr = constr;
                    cntMasterScreen.NoDataInfo = empty;
                    TriggerSelectTab();
                    cntMasterScreen.Prepare();
                    break;
            }
        }

        protected void tabContent_MenuItemClick(object sender, MenuEventArgs e)
        {
            DoSelectTab();
        }

        public void ReloadCurrent()
        {
            DoSelectTab();
        }
        //------------------------------------------
        public string Grupa
        {
            set { ViewState["grupa"] = value; }
            get { return Tools.GetStr(ViewState["grupa"]); }
        }

        public cntReport2 cntLine
        {
            get { return cntMasterLines; }
        }

        public cntDetails cntScreen
        {
            get { return cntMasterScreen; }
        }

        public Menu Tabs
        {
            get { return tabContent; }
        }

        public HtmlGenericControl Panel
        {
            get { return paSqlContent; }
        }

        protected void btEdit_Click(object sender, EventArgs e)
        {
            cntSqlEdit1.Show(tabContent.SelectedValue);
            btEdit.Visible = false;
        }

        protected void cntSqlEdit1_Close(object sender, EventArgs e)
        {
            cntSqlEdit1.Hide();
            btEdit.Visible = true;
        }




        //------------------------------------
        protected void btWniosek_Click(object sender, EventArgs e)
        {

            Response.Redirect(String.Format(@"Wnioski_Dane_Osobowe.aspx?Id={1}&Type={0}",hidTabType.Value, hidTabId.Value));
   
        }

    }
}