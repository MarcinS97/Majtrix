using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using HRRcp.App_Code;

namespace HRRcp.MatrycaSzkolen.Controls.Przypisania
{
    public partial class cntStruktura: System.Web.UI.UserControl
    {
        public event EventHandler SelectedChanged;

        public enum TMode { ALL, KIER }    // wszyscy, tylko kierownicy
        private TMode FMode = TMode.ALL;

        //-----------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /*
                if (FMode == TMode.KIER)
                {
                    tvStructure.LeafNodeStyle.CopyFrom(tvStructure.NodeStyle);
                }
                 */ 
            }
        }
        //-----------------
        public void Prepare(string rootId, DateTime data)
        {
            string d = Tools.DateToStr(data);
            Prepare(rootId, d);
        }

        bool withPrac = true;

        public void Prepare(string rootId, string data)
        {
            RootId = rootId;
            Data = data;

            AppUser user = AppUser.CreateOrGetSession();
            bool v = FMode == TMode.KIER && user.IsAdmin;   // tylko admin moze ustawić nowego kierownika !!! <<< tu się coś sypało z null reference czasem  wiec zmieniam na CreateOrGetSession
            cbShowWorkers.Visible = v;
            switch (FMode)
            {
                case TMode.ALL: 
                    withPrac = true;
                    break;
                case TMode.KIER:
                    withPrac = v && cbShowWorkers.Checked;
                    if (withPrac)
                    {
                        tvStructure.LeafNodeStyle.ForeColor = Color.Black;
                        tvStructure.LeafNodeStyle.Font.Bold = false;
                    }
                    else
                        tvStructure.LeafNodeStyle.CopyFrom(tvStructure.NodeStyle);
                    break;
            }

            tvStructure.Nodes.Clear();
            
            //getTreeChilds(con, null, rootId, data);
            getTreeChilds2(rootId, data, withPrac);
            
            bool isRoot = String.IsNullOrEmpty(rootId);
            if (FMode == TMode.ALL && isRoot)
                getNieprzypisani(null, data);
            
            btShowMe.Visible = isRoot;      // w każdym innym przypadku jest wszystko co pode mną - nie ma mnie na tree
        }

        public void Prepare(string rootId, string data, string selectId, int index)
        {
            Prepare(rootId, data);
            if (!String.IsNullOrEmpty(selectId))
                Select(selectId, index, false);
        }

        public void Prepare(string rootId, DateTime data, string selectId, int index)
        {
            Prepare(rootId, data);
            if (!String.IsNullOrEmpty(selectId))
                Select(selectId, index, false);
        }

        public void PrepareIfEmpty(string rootId, DateTime data)
        {
            if (tvStructure.Nodes.Count == 0)
                Prepare(rootId, data);
        }

        /*
        public void _PrepareOkres(string okresId)
        {
            if (okresId == "-1")
                OkresId = null;
            else
                if (!String.IsNullOrEmpty(okresId) && Okres.IsArch(okresId))
                    OkresId = okresId;
                else 
                    OkresId = null;
            Prepare(RootId);
        }
         */
        public void Reload()
        {
            Prepare(RootId, Data);
        }
        
        //-----------------
        public TreeNode Select(string selectId, int index, bool showNotFound)
        {
            if (searchFor(selectId, index))
            {
                tvStructure.ExpandAll();
                return tvStructure.SelectedNode;
            }
            else
            {
                if (showNotFound)
                    Tools.ExecOnStart("ShowNotFound");  // zeby sie pokazało po wszystkim np po ustawieniu pozycji ...
                //selectItem(FirstNode);
                return null;
            }
        }
        //-----------------
        /*
        public void InitScript()
        {
            const string vs = "structureScripts";
            object o = ViewState[vs];
            if (o == null)
            {
                ViewState[vs] = "1";
                Tools.ExecOnStart2(vs,
                    "tvStructureId = '" + tvStructure.ClientID + "';" +
                    "paStructureId = '" + paStructure.ClientID + "';" +
                    "Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(structOnBeginRq);" +
                    "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(structOnEndRq);"
                    );
            }
        }
        */
        int FScrollIndex = 0;  //do structScrollTop

        public void InitScript()
        {
            const string vs = "structureScripts";
            Tools.ExecOnStart2(vs + FScrollIndex.ToString(), String.Format(@"
tvStructureId[{0}] = '{1}';
paStructureId[{0}] = '{2}';
if (!isScrollSet) {{
    isScrollSet = true;
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(structOnBeginRq);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(structOnEndRq);
}}",
                FScrollIndex, tvStructure.ClientID, paStructure.ClientID));
        }
        
        //-----------------
        // UWAGA !!! dorobić zabezpieczenie zapętlonej struktury !!! np p1.k1 - k1.k2 - k2.p1 lub k1.k1
        int depth = 0;

        private void _getTreeChilds(SqlConnection con, TreeNode node, string id, string data)
        {                                                  //0 1 2 3 4
            const string select = @"select 
ISNULL(RTRIM(P.Nazwisko) + ' ' + RTRIM(P.Imie) + ISNULL(' (' + P.KadryId + ')',''), convert(varchar, R.IdPracownika)) as ImieNazwisko,
R.IdPracownika,
P.Kierownik,P.RcpId,P.Status 
from Przypisania R 
left outer join Pracownicy P on P.Id = R.IdPracownika";
          
            
            bool isRoot = String.IsNullOrEmpty(id) || id == "0";
            if (String.IsNullOrEmpty(id)) id = "0";
            string where = String.Format(" where R.Status = 1 and '{0}' between R.Od and ISNULL(R.Do, '20990909') and R.IdKierownika = {1}", data, id);   // zabezpieczenie jak sam jestem swoim kierownikiem póki co - wyrzucę na górę; uwzględniam jeśli pominiety kierownik -> kierownika nie można pominąć !!!
            if (!withPrac)
                where += String.Format(" and (P.Kierownik=1 or R.IdPracownika in (select distinct IdKierownika from Przypisania where Status = 1 and '{0}' between Od and ISNULL(Do, '20990909') and IdKierownika <> 0))", data);
            
            DataSet ds = Base.getDataSet(con, select + where + " order by P.Kierownik desc, P.Nazwisko,P.Imie");  //<< sortowanie zrobić na nodes            
            foreach (DataRow dr in Base.getRows(ds))
            {
                string pracId = Base.getValue(dr, 1);
                bool kier = Base.getBool(dr, 2, false);
                string rcpId = Base.getValue(dr, 3);
                string name = Base.getValue(dr, 0) +
                              (String.IsNullOrEmpty(rcpId) ? " (brak RCP)" : "") +
                              (kier ? "+" : "-");

                TreeNode n = new TreeNode(name, Tools.SetLineParams(2, pracId, rcpId, null, null, null, null));
                if (node == null)
                    tvStructure.Nodes.Add(n);
                else
                    node.ChildNodes.Add(n);
            }
            if (node == null)   // root
            {
                depth = 0;
                foreach (TreeNode n in tvStructure.Nodes)
                {
                    if (n.Text.EndsWith("+"))       // tylko kierowników sprawdzam
                        _getTreeChilds(con, n, Tools.GetLineParam(n.Value, 0), data);
                    n.Text = n.Text.Remove(n.Text.Length - 1);
                }
            }
            else
            {
                depth++;
                if (depth <= 30)
                    foreach (TreeNode n in node.ChildNodes)
                    {
                        if (n.Text.EndsWith("+"))
                            _getTreeChilds(con, n, Tools.GetLineParam(n.Value, 0), data);
                        n.Text = n.Text.Remove(n.Text.Length - 1);
                    }
                depth--;
            }
        }

        private void getNieprzypisani(TreeNode node, string data)
        {                                                  //0 1    2           3       4   5
            const string _select = @"
declare @data datetime
set @data = '{0}'
select 
ISNULL(RTRIM(P.Nazwisko) + ' ' + RTRIM(P.Imie) + ISNULL(' (' + P.KadryId + ')',''), convert(varchar, R.IdPracownika)) as ImieNazwisko,
P.Id,           
P.Kierownik,    
--P.RcpId,      
PK.RcpId,
PK.NrKarty,
P.Status 
from Pracownicy P 
left outer join Przypisania R on R.IdPracownika = P.Id and @data between R.Od and ISNULL(R.Do, '20990909') and R.Status in (0,1)
outer apply (select top 1 * from PracownicyKarty where IdPracownika = P.Id and @data between Od and ISNULL(Do, '20990909')) PK 
where P.Status >= 0 and R.Id is null";
//left outer join Przypisania R on R.IdPracownika = P.Id 20131025

            DataSet ds = db.getDataSet(String.Format(_select, data) + " order by P.Kierownik desc, P.Nazwisko,P.Imie");  //<< sortowanie zrobić na nodes            
            if (db.getCount(ds) > 0)
            {
                TreeNode arch = new TreeNode("NIEPRZYPISANI", Tools.SetLineParams(2, null, null, null, null, null, null));
                tvStructure.Nodes.Add(arch);

                foreach (DataRow dr in Base.getRows(ds))
                {
                    string pracId = Base.getValue(dr, 1);
                    bool kier = Base.getBool(dr, 2, false);
#if SIEMENS
                    string rcpId = Base.getValue(dr, 4);  // NrKarty
#else
                    string rcpId = Base.getValue(dr, 3);
#endif
                    string name = Base.getValue(dr, 0) +
                                  (String.IsNullOrEmpty(rcpId) ? " (brak RCP)" : "");
                    TreeNode n = new TreeNode(name, Tools.SetLineParams(2, pracId, rcpId, null, null, null, null));
                    arch.ChildNodes.Add(n);
                }
                if (arch.ChildNodes.Count == 0) arch.Text += " - brak danych";
                
                //Tools.CollapseDepth1(tvStructure);
                arch.Expand();
            }
        }

        //-------------------------------------------------------------------------------------------
        private void x_getTreeChilds2(string rootId, string data, bool withPrac)
        {
/*
create view VPracownicySortNazwisko as 
SELECT ROW_NUMBER() OVER(ORDER BY Nazwisko, Imie ASC) AS LP, *  
FROM Pracownicy
*/
            if (String.IsNullOrEmpty(rootId)) rootId = "0";
            if (String.IsNullOrEmpty(data)) data = Tools.DateToStr(DateTime.Today);
            string where = !withPrac ? String.Format(" where Kierownik=1 or IdPracownika in (select distinct IdKierownika from Przypisania where Status = 1 and '{0}' between Od and ISNULL(Do, '20990909') and IdKierownika <> 0)", data) : null;
            Okres ok = new Okres((DateTime)Tools.StrToDateTime(data));  // zakładam, że poprawna !!!
            DataSet ds = db.getDataSet(String.Format(@"
declare 
    @rootId int,
    @data datetime,
    @od datetime,
    @do datetime
set @rootId = {0}
set @data = '{1}'
set @od = {3}
set @do = {4}
--set @rootId = 0
--set @data = GETDATE()

SELECT 
    IdPracownika 
    ,Nazwisko + ' ' + Imie
    ,KadryId
    ,Kierownik 
    ,RcpId
    ,Status
    ,Hlevel
    ,AktywneNaDzien
--FROM dbo.fn_GetTree(@rootId, @data) 
FROM dbo.fn_GetTreeOkres(@rootId, @od, @do, @data)
{2}", rootId, data, where, ok.DateFrom.ToStringDb(), ok.DateTo.ToStringDb()));

            TreeNode node = null;   // lastnode
            int level = 0;          // last level 

            string prevPracId = null;
            int prevLev = -1;

            foreach (DataRow dr in db.getRows(ds))
            {
                string pracId = db.getValue(dr, 0);
                string prac = db.getValue(dr, 1);
                string nrew = db.getValue(dr, 2);
                bool kier = db.getBool(dr, 3, false);
                string rcpId = db.getValue(dr, 4);
                int status = db.getInt(dr, 5, 0);   // status ok
                int lev = db.getInt(dr, 6, 1);      // poziom główny
                bool akt = db.getBool(dr, 7, true); // aktywne na dzień

                if (akt || prevPracId != pracId || prevLev != lev)
                {
                    string name = String.Format("<span class=\"{1}{2}\">{0}</span>", prac +
                                  String.Format(" <span class=\"n\">({0})</span>", nrew) +
                                  (String.IsNullOrEmpty(rcpId) ? " <span class=\"e\">(brak RCP)</span>" : "")
                                  , kier ? "k" : "p", akt ? "" : " x");
                    TreeNode n = new TreeNode(name, Tools.SetLineParams(3, pracId, rcpId, akt ? "1" : "0", null, null, null));
                    if (lev <= level)
                        for (int i = level; i >= lev; i--)
                            if (node != null)   // zabezpieczenie
                                node = node.Parent;
                            else
                                break;

                    if (lev <= 1 || node == null)  // na wszelki wypadek
                        tvStructure.Nodes.Add(n);
                    else
                        node.ChildNodes.Add(n);

                    node = n;
                    level = lev;

                    prevPracId = pracId;
                    prevLev = lev;
                }
            }
        }

        private void getTreeChilds2(string rootId, string data, bool withPrac)
        {
            /*
            create view VPracownicySortNazwisko as 
            SELECT ROW_NUMBER() OVER(ORDER BY Nazwisko, Imie ASC) AS LP, *  
            FROM Pracownicy
            */
            if (String.IsNullOrEmpty(rootId)) rootId = "0";
            if (String.IsNullOrEmpty(data)) data = Tools.DateToStr(DateTime.Today);
            string where = !withPrac ? String.Format(" where Kierownik=1 or IdPracownika in (select distinct IdKierownika from Przypisania where Status = 1 and '{0}' between Od and ISNULL(Do, '20990909') and IdKierownika <> 0)", data) : null;
            Okres ok = new Okres((DateTime)Tools.StrToDateTime(data));  // zakładam, że poprawna !!!
            DataSet ds = db.getDataSet(String.Format(@"
declare 
    @rootId int,
    @data datetime,
    @od datetime,
    @do datetime
set @rootId = {0}
set @data = '{1}'
set @od = {3}
set @do = {4}
--set @rootId = 0
--set @data = GETDATE()

SELECT 
    IdPracownika 
    ,Nazwisko + ' ' + Imie
    ,KadryId
    ,Kierownik 
    ,RcpId
    ,NrKarty
    ,Status
    ,Hlevel
    ,AktywneNaDzien
    ,IdPrzypisania
--FROM dbo.fn_GetTree(@rootId, @data) 
FROM dbo.fn_GetTreeOkres(@rootId, @od, @do, @data)
{2}
order by SortPath", rootId, data, where, ok.DateFrom.ToStringDb(), ok.DateTo.ToStringDb()));

            TreeNode node = null;   // lastnode
            int level = 0;          // last level 

            string prevPracId = null;
            int prevLev = -1;
            bool prevAkt = false;
            TreeNode n = null;

            foreach (DataRow dr in db.getRows(ds))
            {
                string pracId = db.getValue(dr, 0);
                string prac = db.getValue(dr, 1);
                string nrew = db.getValue(dr, 2);
                bool kier = db.getBool(dr, 3, false);
#if SIEMENS
                string rcpId = db.getValue(dr, 5);
#else
                string rcpId = db.getValue(dr, 4);
#endif
                int status = db.getInt(dr, 6, 0);   // status ok
                int lev = db.getInt(dr, 7, 1);      // poziom główny
                bool akt = db.getBool(dr, 8, true); // aktywne na dzień
                string przId = db.getValue(dr, 9);  // id przypisania

                string name = String.Format("<span class=\"{1}{2}\">{0}</span>",
                              prac + String.Format(" <span class=\"n\">({0})</span>", nrew) +
                                    (String.IsNullOrEmpty(rcpId) ? " <span class=\"e\">(brak RCP)</span>" : ""),
                              kier ? "k" : "p",
                              akt ? "" : " x");
                if (prevPracId != pracId || prevLev != lev)
                {
                    n = new TreeNode(name, Tools.SetLineParams(4, pracId, rcpId, akt ? "1" : "0", przId, null, null));
                    if (lev <= level)
                        for (int i = level; i >= lev; i--)
                            if (node != null)   // zabezpieczenie
                                node = node.Parent;
                            else
                                break;

                    if (lev <= 1 || node == null)  // na wszelki wypadek
                        tvStructure.Nodes.Add(n);
                    else
                        node.ChildNodes.Add(n);

                    node = n;
                    level = lev;

                    prevPracId = pracId;
                    prevLev = lev;
                }
                else if (!prevAkt && akt)  // ten sam pracownik, ale aktualny -> nadpisuję 
                {
                    n.Text = name;
                }
                prevAkt = akt;
            }
        }

        /*
        public static void Sort(this TreeView tv)
        {
            TreeNodeCollection T = tv.Nodes.Sort();
            tv.Nodes.Clear();
            tv.Nodes.AddRange(T);
        }

        public static void Sort(this TreeNode tn)
        {
            TreeNodeCollection T = tn.ChildNodes.Sort();
            tn.ChildNodes.Clear();
            tn.ChildNodes.AddRange(T);
        }
        */
        /*
        public void Sort(IComparer comparer)
        {
            ArrayList list = new ArrayList(this.Nodes.Count);
            foreach (TreeNode childNode in this.Nodes)
            {
                list.Add(childNode);
            }
            list.Sort(comparer);

            this.BeginUpdate();
            this.Nodes.Clear();
            foreach (TreeNode childNode in list)
            {
                this.Nodes.Add(childNode);
            }
            this.EndUpdate();
        }
        */

        //-------------------------------------------------------------------------------------------
        public static string GetValueId(TreeNode tn, int index)  // 0 - pracId, 1 - rcpId (nieużywane!), 2 - akt, 3 - przypisanieId
        {
            if (tn != null)
                return Tools.GetLineParam(tn.Value, index);
            else
                return null;
        }

        public static string GetPracId(TreeNode tn)
        {
            return GetValueId(tn, 0);
        }

        public static string GetPrzId(TreeNode tn)
        {
            return GetValueId(tn, 3);
        }

        private string _GetRcpId(TreeNode tn)  // nie musi być wykorzystywane po uzaleznieniu od dat
        {
            return GetValueId(tn, 1);
        }

        private void TriggerSelectedChanged()
        {
            if (SelectedChanged != null)
                SelectedChanged(this, EventArgs.Empty);
        }

        //------------------------
        protected void tvStructure_SelectedNodeChanged(object sender, EventArgs e)
        {
            TriggerSelectedChanged();
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbSearch.Text))
            {
                string pracId;
                
                if (searchFor(tbSearch.Text, out pracId))
                {
                    TriggerSelectedChanged();
                }
                else
                {
                    Tools.ExecOnStart("ShowNotFound");  // zeby sie pokazało po wszystkim np po ustawieniu pozycji ...
                }
                tbSearch.Attributes.Add("onfocus", "javascript:this.select();");  // zaznaczam wszystko
                tbSearch.Focus();
            }
        }

        protected void btExpand_Click(object sender, EventArgs e)
        {
            tvStructure.ExpandAll();
            Tools.ExecOnStart("scroll_tvStructure");
        }

        protected void btCollapse_Click(object sender, EventArgs e)
        {
            if (tvStructure.SelectedNode != null)
            {
                tvStructure.SelectedNode.Selected = false;
                TriggerSelectedChanged();
            }
            //checkButtons();
            if (tvStructure.Nodes.Count > 1)
                tvStructure.CollapseAll();
            else
                Tools.CollapseDepth1(tvStructure);

        }

        protected void btShowMe_Click(object sender, EventArgs e)
        {
            Select(App.User.Id, 0, true);
        }

        protected void cbShowWorkers_CheckedChanged(object sender, EventArgs e)
        {
            Reload();
        }
        //----- SEARCH -------------------
        //private string searchText = "";
        //private string searchPracId;        // ten pracownik jest zaznaczony w momencie rozpoczęcia wyszukiwania

        private void selectItem(TreeNode tn)
        {
            if (tn != null)
            {
                tn.Selected = true;
                TreeNode nn = tn;           // rozwijamy wszystko
                while (nn != null)
                {
                    nn.Expand();
                    nn = nn.Parent;
                }
                TriggerSelectedChanged();
            }
            else
                if (tvStructure.SelectedNode != null)
                {
                    tvStructure.SelectedNode.Selected = false;
                    TriggerSelectedChanged();
                }
            Tools.ExecOnStart("scroll_tvStructure");
        }

        private bool compareWorkerName(TreeNode tn, string search, ref string pracId)
        {
            if (tn != null)
                if (tn.Text.ToLower().Contains(search))
                {
                    pracId = tn.Value;
                    selectItem(tn);
                    return true;
                }
            return false;
        }

        private bool compareWorkerId(TreeNode tn, string pracId)
        {
            if (tn != null)
                if (GetPracId(tn) == pracId)
                {
                    selectItem(tn);
                    return true;
                }
            return false;
        }

        private bool compareValueId(TreeNode tn, string valueId, int index)
        {
            if (tn != null)
                if (GetValueId(tn, index) == valueId)
                {
                    selectItem(tn);
                    return true;
                }
            return false;
        }

        private TreeNode nextNode(TreeNode tn, bool children)
        {
            if (tn == null) return FirstNode;
            else
                if (children && tn.ChildNodes.Count > 0)
                    return tn.ChildNodes[0];
                else
                    if (tn.Parent == null)                                  // root
                    {
                        int idx = tvStructure.Nodes.IndexOf(tn) + 1;
                        if (0 < idx && idx < tvStructure.Nodes.Count)       // -1
                            return tvStructure.Nodes[idx];
                        else
                            return null;                                    // koniec
                    }
                    else                                                    // i w górę o 1
                    {
                        int idx = tn.Parent.ChildNodes.IndexOf(tn) + 1;
                        if (0 < idx && idx < tn.Parent.ChildNodes.Count)    // -1
                            return tn.Parent.ChildNodes[idx];
                        else
                            return nextNode(tn.Parent, false);              // poziom wyżej
                    }
        }

        private bool isStartItem(TreeNode tn)
        {
            return (tn.Value == StartValue);  // porownanie node==node nie wychodzi
        }

        private bool getNextWorker(ref TreeNode tn)     // false jesli już nie ma
        {
            tn = nextNode(tn, true);
            if (tn == null && StartValue != FirstValue) // koniec i jeśli nie zacząłem od początku to zaczynam
                tn = nextNode(tn, true);                // jeszcze raz od początku
            
            if (tn != null)
                return !isStartItem(tn);
            else
                return false;
        }

        public bool searchFor_1(string search, out string pracId)
        {
            if (FirstNode != null)  // pusta lista
            {
                TreeNode tn = tvStructure.SelectedNode;
                if (tn == null) tn = FirstNode;
                pracId = null;
                string loSearch = search.ToLower();

                bool newSearch = search != PrevSearch;
                if (newSearch)  // start parameters
                {
                    PrevSearch = search;
                    if (compareWorkerName(tn, loSearch, ref pracId))
                        return true;
                }

                if (newSearch || String.IsNullOrEmpty(StartValue))
                    StartValue = tn.Value;
                while (getNextWorker(ref tn))
                    if (compareWorkerName(tn, loSearch, ref pracId))
                        return true;
            }
            pracId = null;
            searchReset();
            return false;
        }

        public bool searchFor(string search, out string pracId)
        {
            if (FirstNode != null)  // pusta lista
            {
                pracId = null;
                string loSearch = search.ToLower();

                bool newSearch = search != PrevSearch;

                TreeNode tn;
                if (newSearch || String.IsNullOrEmpty(StartValue))
                {
                    tn = FirstNode;
                    StartValue = tn.Value;
                    PrevSearch = search;
                    if (compareWorkerName(tn, loSearch, ref pracId))
                        return true;
                }
                else
                {
                    tn = tvStructure.SelectedNode;
                    if (tn == null) tn = FirstNode;
                }

                while (getNextWorker(ref tn))
                    if (compareWorkerName(tn, loSearch, ref pracId))
                        return true;
            }
            pracId = null;
            searchReset();
            return false;
        }
        //---
        public bool searchFor(string pracId)
        {
            if (FirstNode != null)  // pusta lista
            {
                TreeNode tn = FirstNode;
                StartValue = tn.Value;
                if (compareWorkerId(tn, pracId))
                    return true;
                while (getNextWorker(ref tn))
                    if (compareWorkerId(tn, pracId))
                        return true;
            }
            return false;
        }

        public bool searchFor(string valueId, int index)
        {
            if (FirstNode != null)  // pusta lista
            {
                TreeNode tn = FirstNode;
                StartValue = tn.Value;
                if (compareValueId(tn, valueId, index))
                    return true;
                while (getNextWorker(ref tn))
                    if (compareValueId(tn, valueId, index))
                        return true;
            }
            return false;
        }
        //-----------------
        public void searchReset()   // kiedy np usuwamy lub przenosimy pracownika
        {
            PrevSearch = null;
        }
        //------------------------
        public TMode Mode
        {
            get { return FMode; }
            set { FMode = value; }
        }

        public int ScrollIndex
        {
            get { return FScrollIndex; }
            set { FScrollIndex = value; }
        }
        //-----
        public string OkresId
        {
            get { return hidOkresId.Value; }
            set { hidOkresId.Value = value; }
        }

        public string RootId
        {
            get { return hidRootId.Value; }
            set { hidRootId.Value = value; }
        }

        public string Data
        {
            get { return hidData.Value; }
            set { hidData.Value = value; }
        }

        public string _SelectedRcpId
        {
            get 
            {
                if (tvStructure.SelectedNode != null)
                    return _GetRcpId(tvStructure.SelectedNode);
                else
                    return null;
            }
        }

        public string SelectedPracId
        {
            get
            {
                if (tvStructure.SelectedNode != null)
                    return GetPracId(tvStructure.SelectedNode);
                else
                    return null;
            }
        }

        public string SelectedPrzId
        {
            get
            {
                if (tvStructure.SelectedNode != null)
                    return GetPrzId(tvStructure.SelectedNode);
                else
                    return null;
            }
        }

        public TreeNode FirstNode
        {
            get 
            {
                if (tvStructure.Nodes.Count > 0)
                    return tvStructure.Nodes[0];
                else
                    return null;
            }
        }

        public string FirstValue
        {
            get
            {
                TreeNode tn = FirstNode;
                if (tn != null)
                    return tn.Value;
                else
                    return null;
            }
        }

        public string PrevSearch
        {
            get { return hidPrevSearch.Value; }
            set { hidPrevSearch.Value = value; }
        }
 
        public string StartValue
        {
            get { return hidStartValue.Value; }
            set { hidStartValue.Value = value; }
        }

        public string SelectedNI
        {
            get 
            {
                if (tvStructure.SelectedNode != null)
                    return tvStructure.SelectedNode.Text;
                else
                    return null;
            }
        }

        public TreeNode SelectedNode
        {
            get { return tvStructure.SelectedNode; }
        }

    }
}