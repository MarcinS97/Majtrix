using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using HRRcp.App_Code;


//UWAGA - do testów zmienić kontrolkę na TimeEdit2 i typy w kodzie


namespace HRRcp.Controls
{
    public partial class MPK2 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        public bool Prepare(string planId, string pracId, string data, int? czasZmPP, int? nadgDPP, int? nadgNPP, int? nocnePP, bool noedit)
        {
            _PlanId = planId;
            PracId = pracId;
            Data = data;
            CzasZmPP = czasZmPP == -1 ? 0 : czasZmPP;
            NadgDPP = nadgDPP;
            NadgNPP = nadgNPP;
            NocnePP = nocnePP;
            ReadOnly = noedit;
            lvMPK.InsertItemPosition = noedit ? InsertItemPosition.None : InsertItemPosition.LastItem;
            lvMPK.DataBind();
            return lvMPK.Items.Count > 0;
        }

        public void Prepare(int? czasZm, int? nadgD, int? nadgN, int? nocne)
        {
            CzasZm = czasZm;
            NadgD = nadgD;
            NadgN = nadgN;
            Nocne = nocne;
            ShowCzasPracy();
        }

        public void PrepareReadOnly()
        {
            CzasZm = CzasZmPP;
            NadgD = NadgDPP;
            NadgN = NadgNPP;
            Nocne = NocnePP;
            ShowCzasPracy();
        }

        public void Update()
        {
            lvMPK.DataBind();
        }

        /*
        public void Prepare(string czasZm, string nadgD, string nadgN, string nocne)
        {
            SetCzasPracy(czasZm, nadgD, nadgN, nocne);
        }
        */
        //-------------------------------------
        public void InitItem(ListView lv, ListViewItemEventArgs e, bool create)
        {
            bool select, edit, insert;
            int lim = Tools.GetListItemMode(e, lv, out select, out edit, out insert);
            if (create)  // item create
            {
                switch (lim)
                {
                    case Tools.limSelect:
                        if (ReadOnly)
                        {
                            Tools.SetControlVisible(e.Item, "tdControl", false);
                            HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("tdLastCol");
                            Tools.AddClass(td, "lastcol");
                        }
                        else
                        {
                            Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                            Tools.SetButton(e.Item, "EditButton", "Edytuj");
                            Tools.SetButton(e.Item, "DeleteButton", "Usuń");
                            //SetControlVisible(e.Item, "DeleteButton", false);
                            //Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                        }
                        break;
                    case Tools.limEdit:
                        Button bt = Tools.SetButton(e.Item, "UpdateButton", "Zapisz");
                        if (bt != null)
                            bt.ValidationGroup = "vge";
                        Tools.SetButton(e.Item, "CancelButton", "Anuluj");
                        Tools.SetButton(e.Item, "DeleteButton", "Usuń");
                        Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                        break;
                    case Tools.limInsert:
                        bt = Tools.SetButton(e.Item, "InsertButton", "Dodaj");
                        if (bt != null)
                            bt.ValidationGroup = "vgi";
                        Tools.SetButton(e.Item, "CancelButton", "Czyść");
                        Tools.SetControlVisible(e.Item, "CancelButton", false);
                        break;
                }
            }
            else
            {
                switch (lim)
                {
                    case Tools.limEdit:
                        ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                        DataRowView drv = (DataRowView)dataItem.DataItem;
                        Tools.SelectItem(e.Item, "ddlMPK", drv["IdMPK"]);
                        break;
                }
            }
        }

        protected void lvMPK_LayoutCreated(object sender, EventArgs e)
        {
        }

        protected void lvMPK_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            InitItem(lvMPK, e, true);
        }

        protected void lvMPK_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            InitItem(lvMPK, e, false);
        }

        protected void lvMPK_DataBound(object sender, EventArgs e)
        {
            bool ro = ReadOnly;
            Tools.SetControlVisible(lvMPK, "thControl", !ro);
            Tools.SetControlVisible(lvMPK, "thControl1", !ro);
            HtmlTableCell th = (HtmlTableCell)lvMPK.FindControl("thLastCol");
            HtmlTableCell th1 = (HtmlTableCell)lvMPK.FindControl("thLastCol1");
            if (th != null && th1 != null)
                if (ro)
                {
                    Tools.AddClass(th, "lastcol");
                    Tools.AddClass(th1, "lastcol");
                }
                else
                {
                    Tools.RemoveClass(th, "lastcol");
                    Tools.RemoveClass(th1, "lastcol");
                }
            Tools.SetText(lvMPK, "lbNocneOdDo", App.GetNocneOdDo);
        }
        //-----------------------
        private bool UpdateItem(EventArgs ea, ListViewItem item, IOrderedDictionary oldValues, IOrderedDictionary values)
        {
            values["IdMPK"] = Tools.GetDdlSelectedValueInt(item, "ddlMPK");
            return true;
        }

        protected void lvMPK_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !UpdateItem(e, e.Item, null, e.Values);
        }

        protected void lvMPK_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !UpdateItem(e, lvMPK.EditItem, e.OldValues, e.NewValues);
        }

        protected void lvMPK_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            //lvMPK.InsertItemPosition = InsertItemPosition.None;
        }

        protected void lvMPK_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            //lvMPK.InsertItemPosition = InsertItemPosition.LastItem;
        }

        protected void lvMPK_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            //lvMPK.InsertItemPosition = InsertItemPosition.LastItem;
        }
        //-------------
        private void ShowCzasPracy()
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            Tools.SetText(lvMPK, "lbCzasZm", CzasZm == null ? null : Worktime.SecToTime((int)CzasZm, settings.Zaokr));
            Tools.SetText(lvMPK, "lbNadgD", NadgD == null ? null : Worktime.SecToTime((int)NadgD, settings.Zaokr)); 
            Tools.SetText(lvMPK, "lbNadgN", NadgN == null ? null : Worktime.SecToTime((int)NadgN, settings.Zaokr));
            Tools.SetText(lvMPK, "lbNocne", Nocne == null ? null : Worktime.SecToTime((int)Nocne, settings.Zaokr)); 
        }

        /*
        private void SetCzasPracy(string czasZm, string nadgD, string nadgN, string nocne)
        {
            Tools.SetText(lvMPK, "lbCzasZm", String.IsNullOrEmpty(czasZm) ? hidCzasZm.Value : czasZm);
            Tools.SetText(lvMPK, "lbNadgD", String.IsNullOrEmpty(nadgD) ? hidNadgD.Value : nadgD);
            Tools.SetText(lvMPK, "lbNadgN", String.IsNullOrEmpty(nadgN) ? hidNadgN.Value : nadgN);
            Tools.SetText(lvMPK, "lbNocne", String.IsNullOrEmpty(nocne) ? hidNocne.Value : nocne);
        }

        private void InitCzasPracy(int? czasZm, int? nadgD, int? nadgN, int? nocne)
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            //wartości startowe
            hidCzasZm.Value = czasZm == null ? null : Worktime.SecToTime((int)czasZm, settings.Zaokr);
            hidNadgD.Value = nadgD == null ? null : Worktime.SecToTime((int)nadgD, settings.Zaokr);
            hidNadgN.Value = nadgN == null ? null : Worktime.SecToTime((int)nadgN, settings.Zaokr);
            hidNocne.Value = nocne == null ? null : Worktime.SecToTime((int)nocne, settings.Zaokr);
            SetCzasPracy(null, null, null, null);
        }
        */
        //---------------------
        /*
        public bool IsValid(string excludeId, IOrderedDictionary values)
        {
            if (String.IsNullOrEmpty(PlanId)) return false;
            {
                string excl = String.IsNullOrEmpty(excludeId) ? "" : " and Id <> " + excludeId;
                DataRow dr = db.getDataRow(String.Format(
                    //      0                     1                                 2                               3 
                    "select sum(CzasZm) as SumZm, sum(NadgodzinyDzien) as SumNadgD, sum(NadgodzinyNoc) as SumNadgN, sum(Nocne) as Nocne " +
                    "from PodzialKosztow where IdPlanPracy = {0}{1}",
                    PlanId, excl));
                int zm = db.getInt(dr, 0, 0) + (values == null ? 0 : db.ISNULL(values["CzasZm"], 0));
                int nd = db.getInt(dr, 1, 0) + (values == null ? 0 : db.ISNULL(values["NadgodzinyDzien"], 0));
                int nn = db.getInt(dr, 2, 0) + (values == null ? 0 : db.ISNULL(values["NadgodzinyNoc"], 0));
                int noc = db.getInt(dr, 3, 0) + (values == null ? 0 : db.ISNULL(values["Nocne"], 0));
                bool c1 = zm <= CzasZm;
                bool c2 = nd <= NadgD;
                bool c3 = nn <= NadgN;
                bool c4 = noc <= Nocne;
                bool c5 = nn <= noc;

                if (c1 && c2 && c3 && c4 && c5) return true;
                else
                {
                    string pp = null;
                    if (!c1) pp += "\\n- czas na zmianie";
                    if (!c2) pp += "\\n- nadgodziny w dzień";
                    if (!c3) pp += "\\n- nadgodziny w nocy";
                    if (!c4) pp += "\\n- czas pracy w nocy";
                    if (!c5) pp += "\\n- nadgodziny w nocy większe od czasu pracy w nocy";
                    Tools.ShowMessage("Przekroczony czas pracy:{0}", pp);
                    return false;
                }
            }
        }
        */

        public static int IsValid(string pracId, string data, string excludeId,     // exclude - którą pozycje pominąć przy sumowaniu, null jak wszystkie wziąć
                                  int kCzasZm, int kNadgD, int kNadgN, int kNocne,  // wartości rcp lub skorygowane przez kierownik
                                  int? czasZm, int? nadgD, int? nadgN, int? nocne)  // wartość wprowadzane
        {
            string excl = String.IsNullOrEmpty(excludeId) ? "" : " and Id <> " + excludeId;
            DataRow dr = db.getDataRow(String.Format(
                //      0                     1                                 2                               3 
                "select sum(CzasZm) as SumZm, sum(NadgodzinyDzien) as SumNadgD, sum(NadgodzinyNoc) as SumNadgN, sum(Nocne) as Nocne " +
                "from PodzialKosztow where IdPracownika={0} and Data='{1}'{2}",
                pracId, data, excl));
            int zm = db.getInt(dr, 0, 0) + (czasZm == null ? 0 : (int)czasZm);
            int nd = db.getInt(dr, 1, 0) + (nadgD == null ? 0 : (int)nadgD);
            int nn = db.getInt(dr, 2, 0) + (nadgN == null ? 0 : (int)nadgN);
            int noc = db.getInt(dr, 3, 0) + (nocne == null ? 0 : (int)nocne); 
            bool c1 = zm <= kCzasZm;
            bool c2 = nd <= kNadgD;
            bool c3 = nn <= kNadgN;
            bool c4 = noc <= kNocne;
            bool c5 = nn <= noc;  
            int ret = 0;
            if (!c1) ret |= 0x0001;
            if (!c2) ret |= 0x0002;
            if (!c3) ret |= 0x0004;
            if (!c4) ret |= 0x0008;
            if (!c5) ret |= 0x0010;
            return ret;
        }

        public int IsValid(string excludeId, int? czasZm, int? nadgD, int? nadgN, int? nocne)
        {
            return IsValid(PracId, Data, excludeId, 
                           db.ISNULL(CzasZm, 0), db.ISNULL(NadgD, 0), db.ISNULL(NadgN, 0), db.ISNULL(Nocne, 0), 
                           czasZm, nadgD, nadgN, nocne);
        }

        /*
        public static int IsValid(string pracId, string data, string excludeId, 
                                  int kCzasZm, int kNadgD, int kNadgN, int kNonce,
                                  int? czasZm, int? nadgD, int? nadgN, int? nocne)
        {
            string excl = String.IsNullOrEmpty(excludeId) ? "" : " and Id <> " + excludeId;
            DataRow dr = db.getDataRow(String.Format(
                //      0                     1                                 2                               3 
                "select sum(CzasZm) as SumZm, sum(NadgodzinyDzien) as SumNadgD, sum(NadgodzinyNoc) as SumNadgN, sum(Nocne) as Nocne " +
                "from PodzialKosztow where IdPracownika={0} and Data='{1}'{2}",
                PracId, Data, excl));
            int zm = db.getInt(dr, 0, 0) + (czasZm == null ? 0 : (int)czasZm);
            int nd = db.getInt(dr, 1, 0) + (nadgD == null ? 0 : (int)nadgD);
            int nn = db.getInt(dr, 2, 0) + (nadgN == null ? 0 : (int)nadgN);
            int noc = db.getInt(dr, 3, 0) + (nocne == null ? 0 : (int)nocne); 
            bool c1 = zm <= CzasZm;
            bool c2 = nd <= NadgD;
            bool c3 = nn <= NadgN;
            bool c4 = noc <= Nocne;
            bool c5 = nn <= noc;  
            int ret = 0;
            if (!c1) ret |= 0x0001;
            if (!c2) ret |= 0x0002;
            if (!c3) ret |= 0x0004;
            if (!c4) ret |= 0x0008;
            if (!c5) ret |= 0x0010;
            return ret;
        }
         */

        private bool kRound(TimeEdit2 te, ref bool rounded)
        {
            if (te.IsEntered && te.Seconds != null)
            {
                int ss = (int)te.Seconds;
                int s = Worktime.RoundSec(ss, 30, 2);
                if (s != ss) rounded = true;
                te.Seconds = s;
                return true;
            }
            else 
                return false;
        }

        private bool kRound(TimeEdit te, ref bool rounded)
        {
            if (te.IsEntered && te.Seconds != null)
            {
                int ss = (int)te.Seconds;
                int s = Worktime.RoundSec(ss, 30, 2);
                if (s != ss) rounded = true;
                te.Seconds = s;
                return true;
            }
            else
                return false;
        }

        private bool IsValid(ListViewItem item, string id)
        {
            bool c0;
            string cc = Tools.GetDdlSelectedValue(item, "ddlMPK");
            if (!String.IsNullOrEmpty(id) && Tools.GetText(item, "hidMPK") == cc) 
                c0 = true;
            else
            {       
                DataRow dr = db.getDataRow(String.Format(
                    "select Id from PodzialKosztow where IdPracownika={0} and Data='{1}' and IdMPK={2}",
                    PracId, Data, cc));
                c0 = dr == null;   // nie jest już wybrane
            }
            /*
            TimeEdit2 te1 = (TimeEdit2)item.FindControl("teCzasZm");
            TimeEdit2 te2 = (TimeEdit2)item.FindControl("teNadgD");
            TimeEdit2 te3 = (TimeEdit2)item.FindControl("teNadgN");
            TimeEdit2 te4 = (TimeEdit2)item.FindControl("teNocne");
            /**/
            /**/
            TimeEdit te1 = (TimeEdit)item.FindControl("teCzasZm");
            TimeEdit te2 = (TimeEdit)item.FindControl("teNadgD");
            TimeEdit te3 = (TimeEdit)item.FindControl("teNadgN");
            TimeEdit te4 = (TimeEdit)item.FindControl("teNocne");
            /**/
            bool r = false;
            bool c1 = kRound(te1, ref r);   // IsEntered, ref rounded
            bool c2 = kRound(te2, ref r);
            bool c3 = kRound(te3, ref r);
            bool c4 = kRound(te4, ref r);
            bool c = !c1 && !c2 && !c3 && !c4;
            int err = 0;
            //----- weryfikacja - jak są dane -----
            if (Tools.StrToDateTime(Data, DateTime.MaxValue) <= DateTime.Today)  // jak błędna konwersja to ma byc weryfikacja
                err = IsValid(id, (int?)te1.Seconds, (int?)te2.Seconds, (int?)te3.Seconds, (int?)te4.Seconds);
            bool v1 = te1.Validate();
            bool v2 = te2.Validate();
            bool v3 = te3.Validate();
            bool v4 = te4.Validate(); 
            if (v1 && v2 && v3 && v4)
                if (c)
                {
                    te1.SetError(true, "Błąd");
                    te2.SetError(true, "Błąd");
                    te3.SetError(true, "Błąd");
                    te4.SetError(true, "Błąd");
                }
                else
                {
                    te1.SetError((err & 0x0001) > 0, "Przekroczenie");
                    te2.SetError((err & 0x0002) > 0, "Przekroczenie");
                    te3.SetError((err & 0x0004) > 0, "Przekroczenie");
                    if ((err & 0x0008) > 0)      te4.SetError(true, "Przekroczenie");
                    else if ((err & 0x0010) > 0) te4.SetError((err & 0x0018) > 0, "Brak");
                    else                         te4.SetError(false, null);
                }
            CustomValidator cv = (CustomValidator)item.FindControl("cvMPK");
            if (!c0) cv.ErrorMessage = "Powtórzone CC";
            else cv.ErrorMessage = null;
            return c0 && !c && err == 0;
        }

        protected void ddlMPK_ValidateInsert(object source, ServerValidateEventArgs args)
        {
            args.IsValid = IsValid(lvMPK.InsertItem, null);
        }

        protected void ddlMPK_ValidateEdit(object source, ServerValidateEventArgs args)
        {
            args.IsValid = IsValid(lvMPK.EditItem, lvMPK.DataKeys[lvMPK.EditIndex].Value.ToString());
        }
        //---------------------
        public bool ReadOnly
        {
            get { return Tools.GetViewStateBool(ViewState["readonly"], true); }
            set { ViewState["readonly"] = value; }
        }

        public bool InEditMode
        {
            get { return lvMPK.EditIndex != -1; }
            set 
            {
                if (value)
                    lvMPK.EditIndex = lvMPK.SelectedIndex;
                else
                    lvMPK.EditIndex = -1;
            }  
        }
        
        public string _PlanId
        {
            get { return hidPlanId.Value; }
            set { hidPlanId.Value = value; }
        }

        public string PracId
        {
            get { return hidPracId.Value; }
            set { hidPracId.Value = value; }
        }

        public string Data
        {
            get { return hidData.Value; }
            set { hidData.Value = value; }
        }
        //-----
        public int? CzasZmPP
        {
            get { return (int?)ViewState["czaszmpp"]; }
            set { ViewState["czaszmpp"] = value; }
        }

        public int? NadgDPP
        {
            get { return (int?)ViewState["nadgdpp"]; }
            set { ViewState["nadgdpp"] = value; }
        }

        public int? NadgNPP
        {
            get { return (int?)ViewState["nadgnpp"]; }
            set { ViewState["nadgnpp"] = value; }
        }

        public int? NocnePP
        {
            get { return (int?)ViewState["nocnepp"]; }
            set { ViewState["nocnepp"] = value; }
        }
        //-----
        public int? CzasZm
        {
            get { return ViewState["czaszm"] != null ? (int?)ViewState["czaszm"] : CzasZmPP; }
            set { ViewState["czaszm"] = value; }
        }

        public int? NadgD
        {
            get { return ViewState["nadgd"] != null ? (int?)ViewState["nadgd"] : NadgDPP; }
            set { ViewState["nadgd"] = value; }
        }

        public int? NadgN
        {
            get { return ViewState["nadgn"] != null ? (int?)ViewState["nadgn"] : NadgNPP; }
            set { ViewState["nadgn"] = value; }
        }

        public int? Nocne
        {
            get { return ViewState["nocne"] != null ? (int?)ViewState["nocne"] : NocnePP; }
            set { ViewState["nocne"] = value; }
        }
    }
}