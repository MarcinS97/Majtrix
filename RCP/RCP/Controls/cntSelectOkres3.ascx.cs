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

/*
 do pozniejszego przerobienia na bootstrap buttons
 */

namespace HRRcp.Controls
{
    public partial class cntSelectOkres3 : System.Web.UI.UserControl
    {
        public event EventHandler OkresChanged;
        private string FControlID = null;
        private string FParentID = null;
        private bool FEnabled = true;
        private bool FStoreInSession = false;
        private bool FForStruktura = false;

        //▌▐¦‖│| ►◄

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        //-----------
        private void TriggerOkresChanged()
        {
            if (FStoreInSession) StoreSession();
            if (OkresChanged != null) OkresChanged(this, EventArgs.Empty);   // i dobrze ze jest wywoływane przed Prepare - PP acc ustawia widoczność btZeruj
            //----- parent control -----
            if (!String.IsNullOrEmpty(FParentID))
            {
                HtmlControl cnt = (HtmlControl)Parent.FindControl(FParentID);
                if (cnt != null)
                    if (IsArch)
                        Tools.AddClass(cnt, "okres_navigator_arch");
                    else
                        Tools.RemoveClass(cnt, "okres_navigator_arch");
            }
            //----- controled control -----
            if (!String.IsNullOrEmpty(FControlID))
            {
                string[] ctlIDs = FControlID.Split(',');
                for (int i = 0; i < ctlIDs.Count(); i++)
                {
                    string ctlID = ctlIDs[i];
                    if (!String.IsNullOrEmpty(ctlID))
                    {
                        UserControl uc = (UserControl)Parent.FindControl(ctlID);
                        if (uc != null)
                        {
                            if (uc is PlanPracy) ((PlanPracy)uc)._Prepare(DateFrom, DateTo, OkresId, Status, IsArch);   //aaa Prepare
                            else if (uc is RepNadgodziny) ((RepNadgodziny)uc).Prepare(DateFrom, DateTo, OkresId, Status, IsArch, StawkaNocna);
                            else if (uc is RepNadgodziny3) ((RepNadgodziny3)uc)._Prepare(DateFrom, DateTo, OkresId, Status, IsArch, StawkaNocna);
                            else if (uc is RcpControl) ((RcpControl)uc).Prepare(DateFrom, DateTo);
                            else if (uc is RcpControl_tool) ((RcpControl_tool)uc).Prepare(DateFrom, DateTo);
                            else if (uc is StrukturaControl) ((StrukturaControl)uc).PrepareOkres(OkresId);
                            //else if (uc is HRRcp.RCP.Controls.Harmonogram.cntHarmonogram) ((HRRcp.RCP.Controls.Harmonogram.cntHarmonogram)uc).PrepareOkres(OkresId);
                        }
                    }
                }
            }
        }
        //--------------
        private void StoreSession()
        {
            Session[ID + "_od"] = DateFrom;
            Session[ID + "_do"] = DateTo;
        }

        private bool GetFromSession(out DateTime d1, out DateTime d2)
        {
            d1 = DateTime.MinValue;
            d2 = DateTime.MinValue;
            object o1 = Session[ID + "_od"];
            object o2 = Session[ID + "_do"];
            if (o1 != null && o2 != null)
                return DateTime.TryParse(o1.ToString(), out d1) &&
                       DateTime.TryParse(o2.ToString(), out d2);
            else
                return false;
        }
        //--------------
        protected void btBegin_Click(object sender, EventArgs e)
        {
            string prevD = DateFrom;
            Okres ok = Okres.First(null);
            FillData(ok);
            if (prevD != DateFrom) TriggerOkresChanged();
        }

        protected void btEnd_Click(object sender, EventArgs e)
        {
            string prevD = DateFrom;
            //Okres ok = Okres.Last(null);
            Okres ok = Okres.Current(null);
            FillData(ok);
            if (prevD != DateFrom) TriggerOkresChanged();
        }
        
        private void Jump(int month)
        {
            DateTime dt;
            string prevD;
            if (month < 0)
            {
                prevD = DateFrom;
                if (!DateTime.TryParse(DateFrom, out dt)) dt = DateTime.Today;
                dt = dt.AddMonths(month);
            }
            else
            {
                prevD = DateTo;
                if (!DateTime.TryParse(DateTo, out dt)) dt = DateTime.Today;
                dt = dt.AddMonths(month);
            }
            Prepare(dt, false);
            if (prevD != DateFrom) TriggerOkresChanged();
        }

        /*
        private void Jump(int month)
        {
            string prevD = DateFrom;
            DateTime dt;
            if (!DateTime.TryParse(DateFrom, out dt)) dt = DateTime.Today;
            dt = dt.AddMonths(month);
            Prepare(dt, false);
            if (prevD != DateFrom) TriggerOkresChanged();
        }
         */
        /*
        protected void btBegin_Click(object sender, EventArgs e)
        {
            string oid = Base.getScalar("select top 1 id from OkresyRozl order by DataOd");
            if (oid != OkresId || String.IsNullOrEmpty(oid))
            {
                Prepare(oid);
                TriggerOkresChanged();
            }
        }
         
        protected void btEnd_Click(object sender, EventArgs e)
        {
            string oid = null;
            if (oid != OkresId || String.IsNullOrEmpty(oid))
            {
                Prepare(oid);
                TriggerOkresChanged();
            }
        }
        
        private void Jump(int month)
        {
            string prevId = OkresId;
            DateTime dt;
            if (!DateTime.TryParse(DateFrom, out dt)) dt = DateTime.Today;
            dt = dt.AddMonths(month);
            Prepare(dt);
            if (OkresId != prevId || String.IsNullOrEmpty(prevId)) TriggerOkresChanged();
        }
         */

        protected void btMinusQ_Click(object sender, EventArgs e)
        {
            Jump(-3);
        }

        protected void btPrev_Click(object sender, EventArgs e)
        {
            Jump(-1);
        }

        protected void btNext_Click(object sender, EventArgs e)
        {
            Jump(1);
        }

        protected void btPlusQ_Click(object sender, EventArgs e)
        {
            Jump(3);
        }
        //-------------------
        public string GetStrukturaStatus()
        {
            return IsArch ? "archiwum" : "struktura bieżąca";
        }

        public void FillData(Okres ok)
        {
            hidOkresId.Value = ok.Id.ToString();
            hidStatus.Value = ok.Status.ToString();
            IsArch = ok.IsArch();
            StawkaNocna = ok.StawkaNocna;
            DateFrom = Base.DateToStr(ok.DateFrom);
            DateTo = Base.DateToStr(ok.DateTo);

            if (FForStruktura)
            {
                string st = GetStrukturaStatus();
                if (String.IsNullOrEmpty(st))
                    lbStatus.Text = null;
                else
                    lbStatus.Text = "<br />" + st;
            }
        }

        public void Prepare(DateTime dt, bool restoreFromSession)
        {
            if (FStoreInSession && restoreFromSession)
            {
                DateTime d1, d2;
                if (GetFromSession(out d1, out d2))
                    dt = d1;
            }
            Okres ok = new Okres(dt);
            FillData(ok);
        }

        public void _Prepare(string okresId)
        {
            if (String.IsNullOrEmpty(okresId)) Prepare(DateTime.Now, false);
            else
            {
                Okres ok = new Okres(null, okresId);
                FillData(ok);
            }
        }

        /*
        public void Prepare(string okresId)
        {
            if (String.IsNullOrEmpty(okresId)) Prepare(DateTime.Now, false);
            else
            {
                DataRow dr = Base.getDataRow("select * from OkresyRozl where Id = " + okresId);
                if (dr == null) Prepare(DateTime.Now, false);
                else
                {
                    hidOkresId.Value = okresId;
                    hidStatus.Value = Base.getValue(dr, "Status");
                    
                    
                    IsArch = IsArch(okresId);
                    StawkaNocna = ok.StawkaNocna;


                    DateFrom = Base.DateToStr(Base.getDateTime(dr, "DataOd"));  // musi być 
                    DateTo = Base.DateToStr(Base.getDateTime(dr, "DataDo"));
                }
            }
        }
         */

        public string FriendlyName(int typ)
        {
            DateTime dt;
            if (DateTime.TryParse(DateTo, out dt))
                switch (typ)
                {
                    case 1:
                    default:
                        return Tools.DateFriendlyName(typ, dt);
                    case 2:
                        string ret = Tools.DateFriendlyName(typ, dt);
                        if (Status == Okres.stClosed)
                            return ret;
                        else
                            return ret + " (okres rozliczeniowy otwarty)";
                    case 3:
                        ret = Tools.DateFriendlyName(typ, dt);
                        if (Status == Okres.stClosed)
                            return ret + " (okres rozliczeniowy zamknięty)";
                        else
                            return ret;
                    case 4:
                        ret = Tools.DateFriendlyName(typ, dt);
                        if (Status == Okres.stClosed)
                            return ret + " (okres rozliczeniowy zamknięty)";
                        else
                            return ret + " (okres rozliczeniowy otwarty)";
                }
            else return null;
        }

        /*
        public void Prepare(string okresId)
        {
            if (String.IsNullOrEmpty(okresId)) Prepare(DateTime.Now);
            else
            {
                DataRow dr = Base.getDataRow("select * from OkresyRozl where Id = " + okresId);
                if (dr == null) Prepare(DateTime.Now);
                else
                {
                    hidOkresId.Value = okresId;
                    hidStatus.Value = Base.getValue(dr, "Status");
                }
            }
        }

        public void Prepare(DateTime dt)
        {
            //SqlConnection con = Base.Connect();
            dt = dt.Date;  // !!! ważne - odcinam time !!!

            DataRow dr = Base.getDataRow(String.Format(
                "select * from OkresyRozl where '{0}' between DataOd and DataDo", 
                    Base.DateTimeToStr(dt)));
            if (dr != null)
            {
                hidOkresId.Value = Base.getValue(dr, "Id");
                hidStatus.Value = Base.getValue(dr, "Status");
            }
            DateTime dOd, dDo;
            if (dr == null ||
                !Base.getDateTime(dr, "DataOd", out dOd) ||
                !Base.getDateTime(dr, "DataDo", out dDo))   // brak lub problem z datami
            {
                Ustawienia ust = Ustawienia.CreateOrGetSession();
                int defDo = ust.OkresDo;
                int day = dt.Day;
                //if (defDo >= 31)
                if (defDo >= 28)  //inaczej sie nie pozbieram ...
                {
                    dOd = Tools.bom(dt);
                    dDo = Tools.eom(dt);
                }

                else if (day > defDo)
                {
                    dOd = dt.AddDays(-day + defDo + 1); // następny dzień
                    dDo = dOd.AddMonths(1).AddDays(-1);  // + miesiac - dzień
                }
                else
                {
                    dDo = dt.AddDays(-day + defDo);     // ostatni dzień
                    dOd = dDo.AddMonths(-1).AddDays(1); // minus miesiąc + dzień
                }

                /*
                else if (day > defDo)    
                {
                    dOd = dt.AddDays(-day + defDo + 1); // następny dzień
                    dDo = dt.AddMonths(1);              // + miesiac
                }
                else                    
                {
                    dDo = dt.AddDays(-day + defDo);     // ostatni dzień
                    dOd = dDo.AddMonths(-1).AddDays(1); // minus miesiąc + dzień
                }
                 aa * / 
                hidOkresId.Value = null;
                hidStatus.Value = null;
            }
            DateFrom = Base.DateToStr(dOd);
            DateTo = Base.DateToStr(dDo);
            //Base.Disconnect(con);
        }

        public void Prepare(string okresId)
        {
            if (String.IsNullOrEmpty(okresId)) Prepare(DateTime.Now);
            else
            {
                DataRow dr = Base.getDataRow("select * from OkresyRozl where Id = " + okresId);
                if (dr == null) Prepare(DateTime.Now);
                else
                {
                    hidOkresId.Value = okresId;
                    hidStatus.Value = Base.getValue(dr, "Status");
                }
            }
        }
         */
        //-------------------
        public string DateFrom
        {
            get { return hidFrom.Value; }
            set 
            { 
                hidFrom.Value = value;
                lbFrom.Text = value;
            }
        }

        public string DateTo
        {
            get { return hidTo.Value; }
            set 
            { 
                hidTo.Value = value;
                lbTo.Text = value;
            }
        }

        public string OkresId
        {
            get { return hidOkresId.Value; }
            set { _Prepare(value); }
        }

        public int Status
        {
            get { return Int32.Parse(hidStatus.Value); }
            //set { hidStatus.Value = value.ToString(); }
        }

        public bool IsArch
        {
            get { return Tools.GetViewStateBool(ViewState[ID + "_okarch"], false); }
            set { ViewState[ID + "_okarch"] = value; }
        }

        public double StawkaNocna
        {
            get { return Tools.GetViewStateDouble(ViewState[ID + "_oksn"], -1); }
            set { ViewState[ID + "_oksn"] = value; }
        }

        public string ControlID
        {
            get { return FControlID; }
            set { FControlID = value; }
        }

        public string ParentID     // ustawia atrybut navigator_arch
        {
            get { return FParentID; }
            set { FParentID = value; }
        }

        public bool Enabled
        {
            get { return FEnabled; }
            set
            {
                FEnabled = value;
                btBegin.Enabled = FEnabled;
                btMinusQ.Enabled = FEnabled;
                btPrev.Enabled = FEnabled;
                btNext.Enabled = FEnabled;
                btPlusQ.Enabled = FEnabled;
                btEnd.Enabled = FEnabled;
                lbFrom.Enabled = FEnabled;
                lbTo.Enabled = FEnabled;
            }
        }

        public bool ForStruktura
        {
            get { return FForStruktura; }
            set { FForStruktura = value; }
        }
        
        public bool StoreInSession
        {
            get { return FStoreInSession; }
            set { FStoreInSession = value; }
        }
    }
}