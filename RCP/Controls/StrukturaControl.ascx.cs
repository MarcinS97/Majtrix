using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class StrukturaControl : System.Web.UI.UserControl
    {
        public event EventHandler SelectedChanged;

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        //-----------------
        public void Prepare(string rootId)
        {
            RootId = rootId;
            SqlConnection con = Base.Connect();
            tvStructure.Nodes.Clear();
            getWorkersTreeChilds(con, null, rootId);
            Base.Disconnect(con);
        }

        public void PrepareIfEmpty(string rootId)
        {
            if (tvStructure.Nodes.Count == 0)
                Prepare(rootId);
        }

        public void PrepareOkres(string okresId)
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


        /*
        public void InitScript()
        {
            const string vs = "structureScripts";
            object o = ViewState[vs];
            if (o == null)
            {
                ViewState[vs] = "1";
                Tools.ExecOnStart2(vs, String.Format(
                    "structScrollTopIndex = {0};" +
                    "tvStructureId = '{1}';" +
                    "paStructureId = '{2}';" +
                    "Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(structOnBeginRq);" +
                    "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(structOnEndRq);",
                    FScrollIndex, tvStructure.ClientID, paStructure.ClientID));
            }
            else
            {
                Tools.ExecOnStart2(vs, String.Format(
                    "structScrollTopIndex = {0};" +
                    "tvStructureId = '{1}';" +
                    "paStructureId = '{2}';",
                    FScrollIndex, tvStructure.ClientID, paStructure.ClientID));
            }
        }
         */ 
        //-----------------
        // UWAGA !!! dorobić zabezpieczenie zapętlonej struktury !!! np p1.k1 - k1.k2 - k2.p1 lub k1.k1
        int depth = 0;

        private void getWorkersTreeChilds(SqlConnection con, TreeNode node, string id)
        {                                                                      //0            1  2         3     4
            const string select = "select RTRIM(Nazwisko) + ' ' + RTRIM(Imie) as ImieNazwisko,Id,Kierownik,RcpId,Status from ";
            string from = String.IsNullOrEmpty(OkresId) ? "Pracownicy " : "PracownicyOkresy ";
            string where;
            bool isRoot = String.IsNullOrEmpty(id);
            if (isRoot)
                //where = "where (IdKierownika is null or IdKierownika = 0 or Id = IdKierownika) and Status >= 0";   // zabezpieczenie jak sam jestem swoim kierownikiem póki co - wyrzucę na górę
                where = "where (IdKierownika is null or IdKierownika = 0 or Id = IdKierownika) and (Status >= 0 or Status = " + App.stPomin + " and Kierownik = 1)";   // zabezpieczenie jak sam jestem swoim kierownikiem póki co - wyrzucę na górę; uwzględniam jeśli pominiety kierownik -> kierownika nie można pominąć !!!
                //where = "where (IdKierownika is null or IdKierownika = 0 or Id = IdKierownika) and Status <> " + App.stOld;   // zabezpieczenie jak sam jestem swoim kierownikiem póki co - wyrzucę na górę
            else
                //where = "where IdKierownika = " + id + " and Id <> IdKierownika and Status >= 0";
                where = "where IdKierownika = " + id + " and Id <> IdKierownika and (Status >= 0 or Status = " + App.stPomin + " and Kierownik = 1)";
                //where = "where IdKierownika = " + id + " and Id <> IdKierownika and Status <> " + App.stOld;
            if (!String.IsNullOrEmpty(OkresId))
                where += " and IdOkresu = " + OkresId;
            DataSet ds = Base.getDataSet(con, select + from + where + " order by Kierownik desc, Nazwisko,Imie");            
            foreach (DataRow dr in Base.getRows(ds))
            {
                string pracId = Base.getValue(dr, 1);
                bool kier = Base.getBool(dr, 2, false);
#if SIEMENS
                string rcpId = pracId; 
#else
                string rcpId = Base.getValue(dr, 3);
#endif
                string name = Base.getValue(dr, 0) +
                              (String.IsNullOrEmpty(rcpId) ? " (brak RCP)" : "") +
                              (kier ? "+" : "-");

                /* debug 
                int status = Base.getInt(dr, 4, -5);
                if (status < 0)
                    status = status;
                */

                TreeNode n = new TreeNode(name, Tools.SetLineParams(2, pracId, rcpId, null, null, null, null));
                if (node == null)
                    tvStructure.Nodes.Add(n);
                else
                    node.ChildNodes.Add(n);
            }
            if (node == null)
            {
                depth = 0;
                foreach (TreeNode n in tvStructure.Nodes)
                {
                    if (n.Text.EndsWith("+"))       // tylko kierowników sprawdzam
                        getWorkersTreeChilds(con, n, Tools.GetLineParam(n.Value, 0));
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
                            getWorkersTreeChilds(con, n, Tools.GetLineParam(n.Value, 0));
                        n.Text = n.Text.Remove(n.Text.Length - 1);
                    }
                depth--;
            }
            //----- archiwum -----
            if (isRoot)
            {
                TreeNode arch = new TreeNode("ARCHIWUM", Tools.SetLineParams(2, null, null, null, null, null, null));
                tvStructure.Nodes.Add(arch);

                where = "where Status = -1 ";
                if (!String.IsNullOrEmpty(OkresId))
                    where += " and IdOkresu = " + OkresId;
                ds = Base.getDataSet(con, select + from + where + " order by Nazwisko,Imie");
                foreach (DataRow dr in Base.getRows(ds))
                {
                    string pracId = Base.getValue(dr, 1);
                    bool kier = Base.getBool(dr, 2, false);
                    string rcpId = Base.getValue(dr, 3);
                    string name = Base.getValue(dr, 0) +
                                  (String.IsNullOrEmpty(rcpId) ? " (brak RCP)" : "");
                    TreeNode n = new TreeNode(name, Tools.SetLineParams(2, pracId, rcpId, null, null, null, null));
                    arch.ChildNodes.Add(n);
                }
                if (arch.ChildNodes.Count == 0) arch.Text += " - brak danych";
                Tools.CollapseDepth1(tvStructure);
                arch.Collapse();
            }
        }

        /*
        private void getWorkersTreeChilds(SqlConnection con, TreeNode node, string id)
        {
            //Base.execSQL(tr "update Pracownicy set Kierownik=0 where Id not in (select distinct IdKierownika from Pracownicy where IdKierownika is not null)");
            //Base.execSQL(tr, "update Pracownicy set Kierownik=1 where Id in (select distinct IdKierownika from Pracownicy where IdKierownika is not null)");
            SqlCommand sqlCmd = new SqlCommand(
                "select RTRIM(Nazwisko) + ' ' + RTRIM(Imie) as ImieNazwisko, " +
                    "Id, Kierownik, RcpId" +
                " from Pracownicy where IdKierownika " + 
                    (String.IsNullOrEmpty(id) ? 
                        " is null or IdKierownika = 0 or Id = IdKierownika" :   // zabezpieczenie jak sam jestem swoim kierownikiem póki co - wyrzucę na górę
                        " = " + id + " and Id <> IdKierownika") +                                               
                " order by Kierownik desc, Nazwisko,Imie", con);
            SqlDataReader dataRdr = sqlCmd.ExecuteReader();
            while (dataRdr.Read())
            {
                string pracId = dataRdr.GetInt32(1).ToString();
                bool kier = dataRdr.GetBoolean(2);
                string rcpId = dataRdr.IsDBNull(3) ? null : dataRdr.GetInt32(3).ToString();
                string name = dataRdr.GetString(0) +
                              (String.IsNullOrEmpty(rcpId) ? " (brak RCP)" : "") +
                              (kier ? "+" : "-");

                TreeNode n = new TreeNode(name, Tools.SetLineParams(2, pracId, rcpId, null, null, null, null));
                if (node == null)
                    tvStructure.Nodes.Add(n);
                else
                    node.ChildNodes.Add(n);
            }
            dataRdr.Close();

            if (node == null)
            {
                depth = 0;
                foreach (TreeNode n in tvStructure.Nodes)
                {
                    if (n.Text.EndsWith("+"))       // tylko kierowników sprawdzam
                        getWorkersTreeChilds(con, n, Tools.GetLineParam(n.Value, 0));
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
                            getWorkersTreeChilds(con, n, Tools.GetLineParam(n.Value, 0));
                        n.Text = n.Text.Remove(n.Text.Length - 1);
                    }
                depth--;
            }
        }
        */

        private string GetPracId(TreeNode tn)
        {
            if (tn != null)
                return Tools.GetLineParam(tn.Value, 0);
            else
                return null;
        }

        private string GetRcpId(TreeNode tn)
        {
            if (tn != null)
                return Tools.GetLineParam(tn.Value, 1);
            else
                return null;
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
                    //getWorkerInfo(pracId, panel);
                    TriggerSelectedChanged();
                }
                else
                {
                    //Tools.SetError(lbNotFound, "Nie znaleziono.");
                    //Tools.HideLabelAfter(lbNotFound, 5000);
                    //Tools.ShowMessage("Nie znaleziono.");
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

        /*
        private bool getNextWorker(ref TreeNode tn)   // false jesli już nie ma
        {
            int i;
            for (i = 0; i <= 1; i++)        
            {
                if (i == 1) return false; // zapęliliśmy się
                tn = nextNode(tn, true);
                if (tn != null)
                    return !isStartItem(tn);
                else
                    tn = null;
            }
            return false; // i tak wyjdzie, tu return tylko dla porządku 
        }
        */

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

        public void searchReset()   // kiedy np usuwamy lub przenosimy pracownika
        {
            PrevSearch = null;
        }
        //------------------------
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

        public string SelectedRcpId
        {
            get 
            {
                if (tvStructure.SelectedNode != null)
                    return GetRcpId(tvStructure.SelectedNode);
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
    }
}