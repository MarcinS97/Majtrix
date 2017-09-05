using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;
using HRRcp.Controls;

namespace HRRcp.MatrycaSzkolen.Controls.Przypisania
{
    public partial class cntPrzypisaniaParametry : System.Web.UI.UserControl
    {
        /*
        public delegate void TGetData(object sender, ListViewInsertEventArgs e);
        public event TGetData GetData;
        */
        public event EventHandler StructureChanged;
        public event EventHandler MoveSettings;

        public enum TMode { 
            INFO,           // informacje
            ADDKIER,        // dodaj w panelu kierownika
            ADDADM,         // dodaj w panelu adm
            EDITKIER,       // edytuj
            EDITADM,        // edytuj
            ACCEPTKIER,     // akceptacja
            ACCEPTADM       // akceptacja
        }
        private TMode FMode = TMode.INFO;

        //----------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void Prepare(string pracId, string data)
        {
            PracId = pracId;
            if (String.IsNullOrEmpty(pracId))
            {
                Prepare(null, false);   // jak nic nie jest zaznaczone
                paPrzypisaniaMini.Visible = false;
            }
            else
            {
                NazwiskoImieNr = AppUser.GetNazwiskoImieNREW(pracId);
                Data = data;

                switch (FMode)
                {
                    case TMode.INFO:
                        /*
                        cntPrzypisaniaMini pm = lvPrzypisaniaParametry.FindControl("cntPrzypisaniaMini1") as cntPrzypisaniaMini;
                        //if (pm != null) pm.Prepare(PracId, null);   // na dziś
                        //if (pm != null) pm.Prepare(PracId, Data);   // na dziś
                        if (pm != null)
                        {
                            if (pm.Prepare(PracId, "20130101"))  // testy - wszystkie od ...
                            {
                                pm.List.SelectedIndex = 0;
                            }
                        }
                        */
                        //cntPrzypisaniaMini1.Prepare(PracId, null);   // na dziś
                        //cntPrzypisaniaMini1.Prepare(PracId, Data);   // na dziś

                        paPrzypisaniaMini.Visible = true;
                        if (cntPrzypisaniaMini1.Prepare(PracId, "20000101") > 0)  // testy - wszystkie od ...
                        {
                            cntPrzypisaniaMini1.List.SelectedIndex = 0;
                            Prepare(cntPrzypisaniaMini1.SelectedId.ToString(), false);   // musi być
                        }
                        else
                        {
                            Prepare(null, false);   // ??? jest pracownik, ale nie ma przypisań ... raczej niemożliwe tu
                        }
                        break;
                    default:
                        paPrzypisaniaMini.Visible = false;
                        break;
                }
            }

            lvPrzypisaniaParametry.DataBind();
            if (lvPrzypisaniaParametry.Items.Count == 0)
            {
                //NazwiskoImieNr = null;
                DataOd = null;
                DataDo = null;
                //DataZatr = null;
            }
            else
            {
            }
        }

        public void Prepare(string przId, bool edit)
        {
            hidPrzId.Value = przId;
            lvPrzypisaniaParametry.DataBind();
            if (edit)
                lvPrzypisaniaParametry.EditIndex = 0;
        }

        private void EnableButtons()
        {
            bool wn = AsWniosek;
            bool p = !String.IsNullOrEmpty(SelPracId);
            bool e = p && !String.IsNullOrEmpty(SelKierId);
            Tools.SetControlVisible(lvPrzypisaniaParametry.InsertItem, "btPrzesun", !wn);
            Tools.SetControlVisible(lvPrzypisaniaParametry.InsertItem, "btWnioskuj", wn);
            Button btP = (Button)Tools.SetControlEnabled(lvPrzypisaniaParametry.InsertItem, "btPrzesun", e);
            Button btW = (Button)Tools.SetControlEnabled(lvPrzypisaniaParametry.InsertItem, "btWnioskuj", e);
            if (e && btP != null && btW != null)
                if (wn) Tools.MakeConfirmButton(btW, "Potwierdź złożenie wniosku o przesunięcie pracownika.");
                else    Tools.MakeConfirmButton(btP, "Potwierdź przesunięcie pracownika.");
            Tools.SetControlEnabled(lvPrzypisaniaParametry.InsertItem, "btMoveSettings", p);
        }

        protected void cntPrzypisaniaMini_Select(object sender, EventArgs e)
        {
            int id = ((cntPrzypisaniaMini)sender).SelectedId;
            if (id != -1)
                Prepare(id.ToString(), false);
        }
        //-----------------------------
        public void SetParams(cntPrzypisaniaParametry cntPrac)
        {
            //SetParams(cntPrac.StrefaRcp, cntPrac.CC, cntPrac.Commodity, cntPrac.Area, cntPrac.Position);
            SetParams(cntPrac.StrefaRcp, cntPrac.Splity, cntPrac.Commodity, cntPrac.Area, cntPrac.Position);
        }

        public void SetParams(string idStrefaRCP, cntSplityWsp splity, string idComm, string idArea, string idPos)
        {
            if (lvPrzypisaniaParametry.InsertItem != null)
            {
                if (IsEditStrefa)   // tylko jak adm i jest pusto to się zainicjuje
                {
                    string sid = Tools.GetDdlSelectedValue(lvPrzypisaniaParametry.InsertItem, "ddlStrefa");
                    if (String.IsNullOrEmpty(sid))
                        Tools.SelectItem(lvPrzypisaniaParametry.InsertItem, "ddlStrefa", idStrefaRCP);
                }
                cntSplityWsp sp = lvPrzypisaniaParametry.InsertItem.FindControl("cntSplityWsp1") as cntSplityWsp;
                if (sp != null)
                    sp.Assign(splity);
                Tools.SelectItem(lvPrzypisaniaParametry.InsertItem, "ddlCommodity", idComm);
                Tools.SelectItem(lvPrzypisaniaParametry.InsertItem, "ddlArea", idArea);
                Tools.SelectItem(lvPrzypisaniaParametry.InsertItem, "ddlPosition", idPos);
            }
        }
        /*
        public void SetParams(string idStrafaRCP, string idCC, string idComm, string idArea, string idPos)
        {
            if (lvPrzypisaniaParametry.InsertItem != null)
            {
                Tools.SelectItem(lvPrzypisaniaParametry.InsertItem, "ddlStrefa", idStrafaRCP);
                Tools.SelectItem(lvPrzypisaniaParametry.InsertItem, "ddlCC", idCC);
                Tools.SelectItem(lvPrzypisaniaParametry.InsertItem, "ddlCommodity", idComm);
                Tools.SelectItem(lvPrzypisaniaParametry.InsertItem, "ddlArea", idArea);
                Tools.SelectItem(lvPrzypisaniaParametry.InsertItem, "ddlPosition", idPos);
            }
        }
        */
        //----------------------------
        public void SetPracownik(string pracId, string Nazwisko, DateTime? dtFrom)
        {
            SelPracId = pracId;
            if (lvPrzypisaniaParametry.InsertItem != null)
            {
                Label lbPrac = lvPrzypisaniaParametry.InsertItem.FindControl("lbPracownik") as Label;
                Label lbHint = lvPrzypisaniaParametry.InsertItem.FindControl("lbPracownikHint") as Label;
                DateEdit de = lvPrzypisaniaParametry.InsertItem.FindControl("deFrom") as DateEdit;
                bool p = !String.IsNullOrEmpty(pracId);
                if (lbPrac != null && lbHint != null && de != null)
                {
                    lbPrac.Visible = p;
                    lbHint.Visible = !p;
                    lbPrac.Text = p ? Nazwisko : null;
                    de.Date = p ? dtFrom : null;
                }
                //----- ostatni RcpStrefaId -----
                if (IsEditStrefa)
                {
                    string idStrefaRCP = p ? GetLastRcpStrefaId(pracId) : null;
                    Tools.SelectItem(lvPrzypisaniaParametry.InsertItem, "ddlStrefa", idStrefaRCP);
                }
                //----------
                EnableButtons();
            }
        }

        public void SetPracownik(TreeNode ptn)
        {
            SetPracownik(cntStruktura.GetPracId(ptn), ptn.Text, null);
        }

        public void SetPracownik(cntPrzypisaniaParametry cnt)
        {
            //DateTime dt = cnt.dtFrom.Add(1);
            DateTime? dt = null;
            if (cnt.DataOd == null)   // nie ma oddelegowania
                dt = cnt.DataZatr;
            else
            {
                if (cnt.DataDo == null)
                    dt = DateTime.Today;
                else
                    dt = ((DateTime)cnt.DataDo).AddDays(1);
            }
            string pracId = cnt.PracId;
            SetPracownik(pracId, cnt.NazwiskoImieNr, dt);
        }




        /*
                    if (ptn.Parent != null)
                        lbKierOd.Text = ptn.Parent.Text;
                    else
                        lbKierOd.Text = "Główny poziom struktury";
                    lbKierOd.CssClass = "value";
        */

        /*
        public void SetKierownik(cntPrzypisaniaParametry cntPrac, TreeNode ktn)
        {
            string kierId = cntStruktura.GetPracId(ktn);
            _SelKierId = kierId;
            SelKierText = ktn != null ? ktn.Text ? null;

            //AsWniosek = !App.User.IsAdmin && !WMojejPodstrukturze(App.User.Id, kierId);
            bool wn = !String.IsNullOrEmpty(kierId) && FMode != TMode.ADDADM && kierId != App.User.Id && !WMojejPodstrukturze(App.User.Id, kierId);  // !czyszcze && !panel admina && !ja && !w podstrukturze
            AsWniosek = wn;
            if (lvPrzypisaniaParametry.InsertItem != null)
            {
                Label lbKierDo = lvPrzypisaniaParametry.InsertItem.FindControl("lbKierDo") as Label;
                Label lbHint = lvPrzypisaniaParametry.InsertItem.FindControl("lbKierDoHint") as Label;
                if (lbKierDo != null && lbHint != null)
                {
                    bool k = !String.IsNullOrEmpty(kierId);
                    lbKierDo.Visible = k;
                    lbHint.Visible = !k;
                    lbKierDo.Text = k ? ktn.Text : null;
                }
                /*
                if (wn)
                    SetParams(null, null, null, null);
                else
                    SetParams(cntPrac.CC, cntPrac.Commodity, cntPrac.Area, cntPrac.Position);
                 * /
                EnableButtons();
            }
        }

        */


        public void SetKierownik(TreeNode ktn)
        {
            string kierId = cntStruktura.GetPracId(ktn);
            SelKierId = kierId;
            SelKierText = ktn != null ? ktn.Text : null;

            //AsWniosek = !App.User.IsAdmin && !WMojejPodstrukturze(App.User.Id, kierId);
            bool wn = !String.IsNullOrEmpty(kierId) && FMode != TMode.ADDADM && kierId != App.User.Id && !WMojejPodstrukturze(App.User.Id, kierId);  // !czyszcze && !panel admina && !ja && !w podstrukturze
            AsWniosek = wn;

            ShowKierownik();
        }

        private void ShowKierownik()
        {
            if (lvPrzypisaniaParametry.InsertItem != null)
            {
                Label lbKierDo = lvPrzypisaniaParametry.InsertItem.FindControl("lbKierDo") as Label;
                Label lbHint = lvPrzypisaniaParametry.InsertItem.FindControl("lbKierDoHint") as Label;
                if (lbKierDo != null && lbHint != null)
                {
                    bool k = !String.IsNullOrEmpty(SelKierId);
                    lbKierDo.Visible = k;
                    lbHint.Visible = !k;
                    lbKierDo.Text = k ? SelKierText : null;
                }
                EnableButtons();
            }
        }

        //-----------------------------
        private bool WMojejPodstrukturze(string kid, string pid)
        {
            string dt = Data;
            if (String.IsNullOrEmpty(dt))
                dt = Tools.DateToStr(DateTime.Today);
            DataRow dr = db.getDataRow(String.Format("select Id from dbo.fn_GetSubPrzypisania({0}, '{1}') where IdPracownika = {2}", kid, dt, pid));
            return dr != null;
        }

        protected void lvPrzypisaniaParametry_ItemCreated(object sender, ListViewItemEventArgs e)
        {   
            if (e.Item.ItemType == ListViewItemType.InsertItem)
            {
                ShowKierownik();
                Tools.SetControlVisible(e.Item, "paStrefa", IsEditStrefa);
            }
            
            
            /*
            int lim = Tools.GetListItemMode(e, lvPrzypisaniaParametry);
            switch (lim)
            {
                case Tools.limInsert:
                    break;
            }
            */


#if SIEMENS
            Tools.SetControlVisible(e.Item, "paCommodity", Lic.ScoreCards);
            if (Lic.ScoreCards) Tools.SetText(e.Item, "lb6", "Arkusz:");
            Tools.SetControlVisible(e.Item, "paArea", false);
#elif DBW
            /*Tools.SetControlVisible(e.Item, "paStrefa", false);*/
            Tools.SetControlVisible(e.Item, "paCC", false);
            Tools.SetControlVisible(e.Item, "paCommodity", false);
            Tools.SetControlVisible(e.Item, "paArea", false);
#elif VICIM
            Tools.SetControlVisible(e.Item, "paCommodity", false);
            Tools.SetControlVisible(e.Item, "paArea", false);
#endif
            if (Lic.OddelegAutoPowrot) Tools.SetControlVisible(e.Item, "paBack", Lic.OddelegAutoPowrot);
        }

        public static DateTime? GetDateTime(object dt)
        {
            return db.isNull(dt) ? null : (DateTime?)dt;
        }

        bool first = true;

        protected void lvPrzypisaniaParametry_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.EmptyItem)
            {

            }

            DataRowView drv;
            int lim = Tools.GetListItemMode(e, lvPrzypisaniaParametry, out drv);
            switch (lim)
            {
                case Tools.limSelect:
                    if (first)
                    {
                        first = false;
                        //NazwiskoImieNr = drv["Pracownik"].ToString();
                        DataZatr  = GetDateTime(drv["DataZatr"]);
                        DataOd    = GetDateTime(drv["Od"]);
                        DataDo    = GetDateTime(drv["DoM"]);
                        StrefaRcp = drv["RcpStrefaId"].ToString();
                        
                        x_CC        = drv["IdCC"].ToString();

                        Commodity = drv["IdCommodity"].ToString();
                        Area      = drv["IdArea"].ToString();
                        Position  = drv["IdPosition"].ToString();
                    }
                    
                    
                    /*
                    cntPrzypisaniaMini pm = e.Item.FindControl("cntPrzypisaniaMini1") as cntPrzypisaniaMini;
                    //if (pm != null) pm.Prepare(PracId, null);   // na dziś
                    //if (pm != null) pm.Prepare(PracId, Data);   // na dziś
                    if (pm != null) pm.Prepare(PracId, "20130101");  // testy - wszystkie od ...
                    */
                    
                    
                    break;
                case Tools.limEdit:
                    int status = (int)drv["status"];
                    bool ed = FMode == TMode.EDITADM;

                    Tools.SetControlVisible(e.Item, "lbOd", !ed);
                    Tools.SetControlVisible(e.Item, "lbDo", !ed);
                    Tools.SetControlVisible(e.Item, "deOd", ed);
                    Tools.SetControlVisible(e.Item, "deDo", ed);

                    break;
            }
            if (e.Item.ItemType == ListViewItemType.DataItem)
                Tools.SetControlVisible(e.Item, "paStrefa", IsEditStrefa);
        }

        protected void lvPrzypisaniaParametry_DataBound(object sender, EventArgs e)
        {
            if (lvPrzypisaniaParametry.Items.Count == 0)
            {
                bool nodata = String.IsNullOrEmpty(PracId);
                Tools.SetControlVisible(lvPrzypisaniaParametry.Controls[0], "lbNoData", nodata);
                Tools.SetControlVisible(lvPrzypisaniaParametry.Controls[0], "lbNoDataPracLabel", !nodata);
                Label lbPrac = Tools.SetControlVisible(lvPrzypisaniaParametry.Controls[0], "lbNoDataPracName", !nodata) as Label;
                if (!nodata && lbPrac != null)
                    lbPrac.Text = NazwiskoImieNr;
            }
        }
        //-----------------------------
        private bool SetError(ListViewItem item, string lbName, bool error)
        {
            Label lb = item.FindControl(lbName) as Label;
            if (lb != null)
                if (error)
                    Tools.AddCss(lb, "error");
                else
                    Tools.RemoveCss(lb, "error");
            return error;
        }

        private void ShowError(ListViewItem item, bool prac, bool kier, bool deOd, bool deDo, bool idStrefaRcp, bool idCC, bool idComm, bool idArea, bool idPos)
        {
            SetError(item, "lb1", prac);
            SetError(item, "lb2", kier);
            SetError(item, "lb3", deOd);
            SetError(item, "lb4", deDo);
            SetError(item, "lb5", idCC);
            SetError(item, "lb6", idComm);
            SetError(item, "lb7", idArea);
            SetError(item, "lb8", idPos);
            SetError(item, "lb9", idStrefaRcp);
        }

        private void ClearError(ListViewItem item)
        {
            ShowError(item, false, false, false, false, false, false, false, false, false);
        }

        //private bool Validate(ListViewItem item, string pracId, string kierId, DateEdit deOd, DateEdit deDo, int? idStrefaRcp, int? idCC, int? idComm, int? idArea, int? idPos)
        private bool Validate(ListViewItem item, string pracId, string kierId, DateEdit deOd, DateEdit deDo, int? idStrefaRcp, cntSplityWsp cntSplit, int? idComm, int? idArea, int? idPos, out string msg)
        {
            bool e1 = false;
            bool e2 = false;
            bool e3 = false;
            bool e4 = false;
            bool e5 = false;
            bool e6 = false;
            bool e7 = false;
            bool e8 = false;
            bool e9 = false;
            msg = null;
            if (String.IsNullOrEmpty(pracId)) e1 = true;
            if (String.IsNullOrEmpty(kierId)) e2 = true;
            if (pracId == kierId)  // i tu zapętlenie struktury sprawdzić
            { 
                e1 = true; 
                e2 = true; 
            }

            if (deOd.IsValid) 
            {
                DateTime dod = (DateTime)deOd.Date;
                if (deDo.IsValid) 
                    if ((DateTime)deDo.Date < dod) 
                    {
                        e3 = true;
                        e4 = true;
                    }
                if (!e3)    // dotyczy zamkniętego okresu
                {
                    Okres ok = Okres.LastAccessible(db.con);
                    if (dod < ok.DateFrom)
                    {
                        msg = "Przesunięcie nie może rozpoczynać się w zamkniętym okresie rozliczeniowym.";
                        e3 = true;
                    }
                }
                if (!e3)  // zachodzi na inne przesunięcie
                {
                    DateTime lastOd;
                    DateTime? lastDo;
                    DataRow drLast = GetLastPrzypisanie(pracId, out lastOd, out lastDo);
                    if (dod <= lastOd)
                    {
                        msg = "Okres przesunięcia koliduje z już istniejącym.";
                        e3 = true;
                    }
                }
            }
            else e3 = true;

#if SIEMENS
            if (cntSplit != null && !cntSplit._Validate()) e5 = true;
            if (IsEditStrefa && idStrefaRcp == null) e9 = true;
#elif MS
#elif DBW
            if (IsEditStrefa && idStrefaRcp == null) e9 = true;
#elif VICIM
            if (cntSplit != null && !cntSplit._Validate()) e5 = true;
            if (IsEditStrefa && idStrefaRcp == null) e9 = true;
#else
            if (cntSplit != null && !cntSplit._Validate()) e5 = true;
            if (idComm == null) e6 = true;
            if (idArea == null) e7 = true;
            if (idPos  == null) e8 = true;
            if (IsEditStrefa && idStrefaRcp == null) e9 = true;
#endif

            ShowError(item, e1, e2, e3, e4, e9, e5, e6, e7, e8);
            return !(e1 || e2 || e3 || e4 || e5 || e6 || e7 || e8 || e9);
        }

        private bool _Validate(ListViewItem item, string pracId, string kierId, DateEdit deOd, DateEdit deDo, out string msg)   // jak jest to wniosek to pola mogą być niewypełnione
        {
            return Validate(item, pracId, kierId, deOd, deDo, -1, null, -1, -1, -1, out msg);
        }

        private void TriggerStructureChanged()
        {
            if (StructureChanged != null)
                StructureChanged(this, EventArgs.Empty);
        }

        private void TriggerMoveSettings()
        {
            if (MoveSettings != null)
                MoveSettings(this, EventArgs.Empty);
        }
        //-----------------------------
        bool accepted = false;
        DateTime prevDo = DateTime.MinValue;  // do SqlDataSource_Inserting
        cntSplityWsp split = null;
        string pracId = null;

        public static DataRow GetLastPrzypisanie(string pracId)
        {
            return db.getDataRow(String.Format(@"select top 1 * from Przypisania where IdPracownika = {0} and Status = 1 order by Od desc", pracId));
        }

        public static DataRow GetLastPrzypisanie(string pracId, out DateTime dOd, out DateTime? dDo)
        {
            DataRow dr = GetLastPrzypisanie(pracId);
            if (dr != null)
            {
                dOd = (DateTime)db.getDateTime(dr, "Od");
                dDo = db.getDateTime(dr, "Do");
            }
            else
            {
                dOd = DateTime.MinValue;
                dDo = null;
            }
            return dr;
        }

        public static string GetLastRcpStrefaId(string pracId)
        {
            DataRow dr = db.getDataRow(String.Format("select top 1 RcpStrefaId from Przypisania where IdPracownika = {0} and Status = 1 and Od <= GETDATE() order by Od desc", pracId));
            if (dr != null)
                return db.getValue(dr, 0);
            else
                return null;
        }

        protected void lvPrzypisaniaParametry_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            //ClearError(e.Item);
            string msg = null;

            DateEdit deOd = e.Item.FindControl("deFrom") as DateEdit;
            DateTime? dod = (DateTime?)deOd.Date;
            DateEdit deDo = e.Item.FindControl("deTo") as DateEdit;
            DateTime? ddo = (DateTime?)deDo.Date;
            pracId = SelPracId;

            e.Values["Od"] = dod;
            e.Values["Do"] = null;
            e.Values["DoMonit"] = ddo;

            if (Lic.OddelegAutoPowrot)
            {
                CheckBox cb = e.Item.FindControl("cbAutoReturn") as CheckBox;
                e.Values["AutoPowrot"] = cb.Checked;
            }

            int? idStrefaRCP;
            if (IsEditStrefa)
                idStrefaRCP = Tools.GetDdlSelectedValueInt(e.Item, "ddlStrefa");
            else
                idStrefaRCP = Tools.StrToInt(GetLastRcpStrefaId(pracId));    // jak nie ma edycji to bierzemy ostatni

            int? idCC = Tools.GetDdlSelectedValueInt(e.Item, "ddlCC");
            int? idComm = Tools.GetDdlSelectedValueInt(e.Item, "ddlCommodity");
            int? idArea = Tools.GetDdlSelectedValueInt(e.Item, "ddlArea");
            int? idPos = Tools.GetDdlSelectedValueInt(e.Item, "ddlPosition");

            e.Values["RcpStrefaId"] = idStrefaRCP;

            //e.Values["IdCC"] = idCC;

            e.Values["IdCommodity"] = idComm;
            e.Values["IdArea"] = idArea;
            e.Values["IdPosition"] = idPos;

            string kid = SelKierId;
            if (!String.IsNullOrEmpty(pracId) && !String.IsNullOrEmpty(kid))
            {
                e.Values["IdPracownika"] = pracId;
                e.Values["IdKierownika"] = kid;
                e.Values["IdKierownikaRq"] = App.User.Id; // po tym wyszukuję "moje wnioski"
                if (!App.User.IsOriginalUser)
                    e.Values["IdKierownikaRqZast"] = App.User.OriginalId;   // zastępstwo
                e.Values["DataRq"] = DateTime.Now;
                e.Values["UwagiRq"] = db.sqlPut(Tools.GetText(e.Item, "tbUwagi"), 200);
                //if (FMode == TMode.ADDADM || WMojejPodstrukturze(App.User.Id, kid))
                split = e.Item.FindControl("cntSplityWsp1") as cntSplityWsp;
                if (!AsWniosek)   // przesunięcie w podstrukturze
                {
                    if (Validate(e.Item, pracId, kid, deOd, deDo, idStrefaRCP, split, idComm, idArea, idPos, out msg))
                    {
                        e.Values["IdKierownikaAcc"] = App.User.OriginalId; // App.User.Id;
                        e.Values["DataAcc"] = DateTime.Now;
                        e.Values["UwagiAcc"] = null;
                        e.Values["Status"] = 1;
                        e.Values["Typ"] = cntPrzypisania.tySubstr;  // 1
                        prevDo = ((DateTime)dod).AddDays(-1);
                        accepted = true;

                        Log.Info(Log.PRZESUNIECIA, "Przesunięcie pracownika", Log.GetValues(null, e.Values));
                        return;
                    }
                }
                else      // wniosek
                {
                    if (_Validate(e.Item, pracId, kid, deOd, deDo, out msg))
                    {
                        e.Values["Status"] = 0;
                        e.Values["Typ"] = cntPrzypisania.tyWniosek;     // 0

                        string kaccId = App.FindKierUp(kid, DateTime.Today, AppUser.rPrzesunieciaAcc);


                        accepted = false;
                        
                        Log.Info(Log.PRZESUNIECIA, "Wniosek o przesunięcie pracownika", Log.GetValues(null, e.Values));
                        return;
                    }
                }
            }
            e.Cancel = true;
            Tools.ShowError("Niepoprawne parametry." + (String.IsNullOrEmpty(msg) ? null : "\\n" + msg));
        }

        public static bool UpdatePrevDateTo(SqlTransaction tr, string pracId, DateTime dtTo)
        {
            return db.execSQL(tr, String.Format(@"
update Przypisania set Do = {1} 
where Id = (select top 1 Id from Przypisania where IdPracownika = {0} and Status = 1 and Od <= {1} order by Od desc)
                ", pracId, db.strParam(Tools.DateToStr(dtTo))));
        }

        public static bool UpdatePrevDateTo(string pracId, DateTime dtTo)
        {
            return db.execSQL(String.Format(@"
update Przypisania set Do = {1} 
where Id = (select top 1 Id from Przypisania where IdPracownika = {0} and Status = 1 and Od <= {1} order by Od desc)
                ", pracId, db.strParam(Tools.DateToStr(dtTo))));
        }

        public static bool UpdatePrevDateToNull(string pracId, DateTime dataOd)
        {
            return db.execSQL(String.Format(@"
update Przypisania set Do = null 
where Id = (select top 1 Id from Przypisania where IdPracownika = {0} and Status = 1 and Od < {1} order by Od desc)
                ", pracId, db.strParam(Tools.DateToStr(dataOd))));
        }


        /*
        public static bool UpdatePrevDateTo(SqlTransaction tr, string pracId, DateTime dtTo)
        {
            return db.execSQL(tr,
                db.updateCmd("Przypisania", 1, "Do",
                    "IdPracownika = {0} and Status = 1 and Od <= {1} and Do is null",
                    pracId, db.strParam(Tools.DateToStr(dtTo))));
        }

        public static bool _UpdatePrevDateTo(string pracId, DateTime dtTo)  // <<<<<<< pozamieniać pozniej na transakcje !!!!
        {
            return db.update("Przypisania", 1, "Do",
                    "IdPracownika = {0} and Status = 1 and Od <= {1} and Do is null",
                    pracId, db.strParam(Tools.DateToStr(dtTo)));
        }
        public static bool UpdatePrevDateToNull(string pracId)
        {
            return db.execSQL(String.Format(@"
update Przypisania set Do = null where Id = 
(select top 1 Id from Przypisania where IdPracownika = {0} and Status = 1 order by Od desc)
                ", pracId));
        }

        */

        //-------------------------------------------
        int przesId = -1;  // ustawiany w SqlDataSource.Inserted

        public bool TreeChanged = false;   // zmiana dotyczy też drzewka, a nie tylko danych pracownika

        protected void lvPrzypisaniaParametry_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            TreeChanged = prevDo < DateTime.Today;
            TriggerStructureChanged();

            string pid = przesId.ToString();
            if (accepted)
            {
                //UpdatePrevDateTo(SelPracId, dtTo);
                //if (prevDo < DateTime.Today)
                //TriggerStructureChanged();

                Mailing.EventPrzesuniecie(Mailing.maPRZES_K, pid);
                Mailing.EventPrzesuniecie(Mailing.maPRZES_P, pid);

                Tools.ShowMessage("Pracownik został przeniesiony.");
            }
            else
            {
                Mailing.EventPrzesuniecie(Mailing.maPRZES_WN, pid);

                Tools.ShowMessage("Wniosek o przeniesienie pracownika został złożony i czeka na akceptację.");
            }
        }

        protected void lvPrzypisaniaParametry_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "MoveSettings":
                    TriggerMoveSettings();
                    break;
                case "Cancel1":
                    SetParams(null, null, null, null, null);
                    break;
            }
        }
        //-------------
        protected void SqlDataSource1_Inserting(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Command.Connection.Open();
            e.Command.Transaction = e.Command.Connection.BeginTransaction();
            SqlTransaction tr = (SqlTransaction)e.Command.Transaction;

            //if (split != null && prevDo != DateTime.MinValue)  // kontrolka jest wskazana, po tym sprawdzam
            if (!AsWniosek && prevDo != DateTime.MinValue)
            {
                UpdatePrevDateTo(tr, pracId, prevDo);   // musi byc przed bo key violation jest jak do=null w OnInserted, jak cos pojdzie nie tak wypadałoby sprawdzić ale update jak nie znajdzie zwaraca false            
            }
            
            /*
            DbCommand command = e.Command;
            DbConnection cx  = command.Connection;    
            cx.Open();    
            DbTransaction tx = cx.BeginTransaction();
            command.Transaction = tx;
             */
        }


 //       nie mozna rzutowac typu dbnull na inne typy przu dodawania przesuniecia do tego samego kierownika IX_Przesuniecia - duplicate


        protected void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            SqlTransaction tr = (SqlTransaction)e.Command.Transaction;
            try
            {
                przesId = db.getIdentity(tr, true);
                if (split != null && split.Insert(tr, przesId))
                    tr.Commit();
                else
                {
                    tr.Rollback();
                    Tools.ShowErrorLog(Log.PRZESUNIECIA, String.Format("Splity.Insert - Pracownik: {0}", pracId),
                        "Wystąpił błąd podczas zmiany przypisania pracownika.");
                }
            }
            catch (Exception ex)
            {
                tr.Rollback();
                Tools.ShowErrorLog(Log.PRZESUNIECIA, String.Format("Pracownik: {0}", pracId),
                    "Wystąpił błąd podczas zmiany przypisania pracownika.\\n" + ex.Message);
            }
        }


        /*
                protected void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
                {
                    SqlTransaction tr = (SqlTransaction)e.Command.Transaction;
                    if (split != null && prevDo != DateTime.MinValue)  // kontrolka jest wskazana, po tym sprawdzam
                    {
                        try
                        {
                            przesId = db.getIdentity(tr, true);
                            //UpdatePrevDateTo(tr, pracId, dtTo);            // powinno pójść w tej samej transakcji - mozna dodać w SqlDataSource.                        
                            if (split.Insert(tr, przesId))
                                tr.Commit();
                            else
                            {
                                tr.Rollback();
                                Tools.ShowErrorLog(Log.PRZESUNIECIA, String.Format("Splity.Insert - Pracownik: {0}", pracId),
                                    "Wystąpił błąd podczas przesuwania pracownika.");
                            }
                        }
                        catch (Exception ex)
                        {
                            tr.Rollback();
                            Tools.ShowErrorLog(Log.PRZESUNIECIA, String.Format("Pracownik: {0}", pracId),
                                "Wystąpił błąd podczas przesuwania pracownika.\\n" + ex.Message);
                        }
                    }
                    else
                    {
                        tr.Commit();
                    }
                }
         */

        /*
        bool OtherProcessSucceeded = true;

        if (OtherProcessSucceeded)
        {
            e.Command.Transaction.Commit();
            Response.Write("The record was updated successfully");
        }
        else
        {
            e.Command.Transaction.Rollback();
            Response.Write("The record was not updated");
        }
        */

            /*
            DbCommand command = e.Command;
            DbTransaction tx = command.Transaction;

            bool OtherProcessSucceeded = true;

            if (OtherProcessSucceeded) {
                tx.Commit();
                Label2.Text="The record was updated successfully!";
            }
            else {
                tx.Rollback();
                Label2.Text="The record was not updated.";
            }
             */

        /*
    protected void SqlDataSource1_Inserting(object sender, SqlDataSourceCommandEventArgs e)
    {
        DbCommand command = e.Command;
        DbConnection cx = command.Connection;
        cx.Open();
        DbTransaction tx = cx.BeginTransaction();
        command.Transaction = tx;
    }

    protected void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
    {
        DbCommand command = e.Command;
        DbTransaction tx = command.Transaction;
        try
        {
            int id = db.getIdentity((SqlTransaction)tx, true);
            switch (kTyp)
            {
                case App.ptWprowadzone:
                    App.AddDoRozdania((SqlTransaction)tx, kDlaId, kIlosc);
                    break;
                case App.ptDoRozdania:
                    App.AddDoRozdania((SqlTransaction)tx, kOdId, -kIlosc);
                    App.AddDoRozdania((SqlTransaction)tx, kDlaId, kIlosc);
                    break;
                case App.ptNaNagrody:
                    App.AddDoRozdania((SqlTransaction)tx, kOdId, -kIlosc);
                    App.AddPunkty((SqlTransaction)tx, kDlaId, kIlosc);
                    break;
                case App.ptWymienione:
                    App.AddPunkty((SqlTransaction)tx, kOdId, kIlosc);
                    break;
                    /*
                case App.ptKorekta:
                    break;
                    zzz* / 
                default:
                    break;
            }
            tx.Commit();
            Mailing.EventPunkty(id.ToString());
            App.ShowMessage("Korekta została dodana.\\n \\nUwaga!\\nBieżące ustawienia filtru danych i sortowania mogą powodować, że nie jest widoczna.");
        }
        /*
        catch (DbException exDb)
        {
            Console.WriteLine("DbException.GetType: {0}", exDb.GetType());
            Console.WriteLine("DbException.Source: {0}", exDb.Source);
            Console.WriteLine("DbException.ErrorCode: {0}", exDb.ErrorCode);
            Console.WriteLine("DbException.Message: {0}", exDb.Message);
        }
         zzz* /
        catch (Exception ex)
        {
            tx.Rollback();
            App.ShowMessage("Wystąpił błąd podczas wprowadzania korekty.\\n" + ex.Message);
            db.LogError(String.Format("Błąd podczas wprowadzania korekty, id={0}, punkty={1}, powód='{2}'", kId, kIlosc, kPowod), 
                        ex.Message);
        }
    }
    
         */

        //-----------------------------
        public string BezTerminu(object o)
        {
            if (db.isNull(o))
                return "bez terminu";
            else
                return Tools.DateToStr(o);
        }
        //-----------------------------
        public string Data              // na dzień
        {
            get 
            {
                /*
                if (String.IsNullOrEmpty(hidData.Value))
                    hidData.Value = Tools.DateToStr(DateTime.Today);
                */
                return hidData.Value; 
            }
            set { hidData.Value = value; }
        }

        public string PracId            // 
        {
            get { return hidPracId.Value; }
            set { hidPracId.Value = value; }
        }

        public string NazwiskoImieNr    // out
        {
            get { return Tools.GetViewStateStr(ViewState["nazwisko"]); }
            set { ViewState["nazwisko"] = value; }
        }

        public DateTime? DataOd            // out
        {
            get { return (DateTime?)ViewState["dataod"]; }
            set { ViewState["dataod"] = value; }
        }

        public DateTime? DataDo            // out
        {
            get { return (DateTime?)ViewState["datado"]; }
            set { ViewState["datado"] = value; }
        }

        public DateTime? DataZatr          // out
        {
            get { return (DateTime?)ViewState["datazatr"]; }
            set { ViewState["datazatr"] = value; }
        }

        public string StrefaRcp
        {
            get { return Tools.GetViewStateStr(ViewState["strefa"]); }
            set { ViewState["strefa"] = value; }
        }

        public string x_CC
        {
            get { return Tools.GetViewStateStr(ViewState["cc"]); }
            set { ViewState["cc"] = value; }
        }

        public cntSplityWsp Splity
        {
            get { return lvPrzypisaniaParametry.Items[0].FindControl("cntSplityWsp1") as cntSplityWsp; }
        }

        public string Commodity
        {
            get { return Tools.GetViewStateStr(ViewState["commodity"]); }
            set { ViewState["commodity"] = value; }
        }

        public string Area
        {
            get { return Tools.GetViewStateStr(ViewState["area"]); }
            set { ViewState["area"] = value; }
        }

        public string Position
        {
            get { return Tools.GetViewStateStr(ViewState["position"]); }
            set { ViewState["position"] = value; }
        }
        //-----
        public string SelPracId
        {
            get { return hidSelPracId.Value; }
            set { hidSelPracId.Value = value; }
        }

        public string SelKierId
        {
            //get { return hidSelKierId.Value; }
            //set { hidSelKierId.Value = value; }
            get { return Tools.GetStr(ViewState["selkierid"]); }
            set { ViewState["selkierid"] = value; }
        }

        public string SelKierText
        {
            get { return Tools.GetStr(ViewState["selkiertext"]); }
            set { ViewState["selkiertext"] = value; }
        }

        //-----
        public TMode Mode
        {
            get { return FMode; }
            set
            {
                FMode = value;
                if (FMode == TMode.ADDKIER || FMode == TMode.ADDADM)
                    lvPrzypisaniaParametry.InsertItemPosition = InsertItemPosition.FirstItem;
                else
                    lvPrzypisaniaParametry.InsertItemPosition = InsertItemPosition.None;
            }
        }
        
        public bool AsWniosek
        {
            get { return Tools.GetViewStateBool(ViewState["aswniosek"], false); }  // false zeby nie zmieniał domyslnego tekstu, bez ustawienia nie pójdzie
            set { ViewState["aswniosek"] = value; }
        }

        public bool IsEditStrefa
        {
            get { return App.User.IsAdmin; }
        }
    }
}