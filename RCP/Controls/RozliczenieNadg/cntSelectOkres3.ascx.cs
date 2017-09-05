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

namespace HRRcp.Controls.RozliczenieNadg
{
    public partial class cntSelectOkres3 : System.Web.UI.UserControl
    {
        public event EventHandler OkresChanged;
        
        public const int okOpened = 0;
        public const int okClosed = 1;

        private bool FEnabled = true;
        //▌▐¦‖│| ►◄

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void Prepare(DateTime data, string pracId, bool getFromSession)
        {
            if (getFromSession)
            {
                DateTime? dt = SesOkres;
                if (dt != null) data = (DateTime)dt;
            }
            PracId = pracId;
            DataRow dr = GetOkres(data, pracId, 0);
            /*
            if (dr == null)
            {
                dr = GetOkresLastBefore(data, pracId, 0);
                if (dr == null)
                    dr = GetOkresFirstAfter(data, pracId, 0);
            }
            */ 
            FillData(dr);
        }

        //--------------------------------
        /*
        public static bool xGetOkres(DateTime d, out DateTime dataOd, out DateTime dataDo, out int status)
        {
            DataRow dr = GetOkres(d);
            if (dr == null)
            {
                dr = GetOkresLastBefore(d);
                if (dr == null)
                    dr = GetOkresFirstAfter(d);
            }
            if (dr != null)
            {
                dataOd = (DateTime)db.getDateTime(dr, "DataOd");
                dataDo = (DateTime)db.getDateTime(dr, "DataDo");
                status = (int)db.getInt(dr, "Status");
                return true;
            }
            else
            {
                dataOd = Tools.bom(d);  // nie powinno miec miejsca jezeli wygenerowana tabela
                dataDo = Tools.eom(d);
                status = okOpened;
                return false;
            }
        }
        */

        public DataRow GetOkres(DateTime d, string pracId, int cnt)
        {
            //return db.getDataRow(String.Format("select * from OkresyRozliczeniowe where '{0}' between DataOd and DataDo", Tools.DateToStrDb(d)));
            return db.getDataRow(String.Format(dsOkres.SelectCommand, Tools.DateToStrDb(d), pracId, cnt));
        }

        /*
        //-------+=====+=====+=====+-----
        //-----|---------------|------|--
        // FirstAfter              LastBefore
        public DataRow GetOkresLastBefore(DateTime d, string pracId)
        {
            return db.getDataRow(String.Format(dsOkresLastBefore.SelectCommand, Tools.DateToStrDb(d), pracId));
        }

        public DataRow GetOkresFirstAfter(DateTime d, string pracId)
        {
            return db.getDataRow(String.Format(dsOkresFirstAfter.SelectCommand, Tools.DateToStrDb(d), pracId));
        }

        //+=====+=====+=====+=====+=====+-----
        //---^--|------------------------ +3
        //---------------------^---------
        public DataRow GetNext(DateTime d, string pracId, int cnt)  // d - dowolna data z biezacego okresu
        {
            DataSet ds = db.getDataSet(String.Format(dsOkresNext.SelectCommand, Tools.DateToStrDb(d), cnt, pracId));
            int c = db.getCount(ds);
            if (c > 0)
                return db.getRow(ds, c - 1);  // ostatni
            else
                return null;
        }

        //+=====+=====+=====+=====+=====+-----
        //------------------------|--^--|----- -3
        //---------^--------------------------
        public DataRow GetPrev(DateTime d, string pracId, int cnt)  // d - dowolna data z biezacego okresu
        {
            DataSet ds = db.getDataSet(String.Format(dsOkresPrev.SelectCommand, Tools.DateToStrDb(d), cnt, pracId));
            int c = db.getCount(ds);
            if (c > 0)
                return db.getRow(ds, c - 1);  // ostatni
            else
                return null;
        }
        */
        //------------------------------
        private void FillData(DataRow dr)
        {
            string[] StatusNazwa = {"Otwarty", "Zamknięty"};

            int st;
            if (dr != null)
            {
                OkresOd = (DateTime)db.getDateTime(dr, "DataOd");
                OkresDo = (DateTime)db.getDateTime(dr, "DataDo");
                st = (int)db.getInt(dr, "Status");
            }
            else
            {
                OkresOd = Tools.bom(DateTime.Today);  // nie powinno miec miejsca jezeli wygenerowana tabela
                OkresDo = Tools.eom(DateTime.Today);
                st = okOpened;
            }
            Status = st;
            IloscMiesiecy = db.getInt(dr, "iloscMiesiecy", 1);

            lbFrom.Text = OkresOdStr;
            lbTo.Text = OkresDoStr;
            //lbStatus.Text = "<br />" + StatusNazwa[st];
        }
        //------------------------------
        private void TriggerOkresChanged()
        {
            if (OkresChanged != null)
                OkresChanged(this, EventArgs.Empty);   // i dobrze ze jest wywoływane przed Prepare - PP acc ustawia widoczność btZeruj
        }

        protected void btBegin_Click(object sender, EventArgs e)
        {
            DateTime prevD = OkresOd;

            Ustawienia settings = Ustawienia.CreateOrGetSession();
            DateTime dt;
            DataRow dr = db.getDataRow("select DataZatr from Pracownicy where Id = " + PracId);
            dt = db.getDateTime(dr, 0, settings.SystemStartDate);
            if (dt < settings.SystemStartDate) dt = settings.SystemStartDate;

            //Prepare(DateTime.Parse("1900-01-01"), PracId, false);
            Prepare(dt, PracId, false);
            if (prevD != OkresOd) 
                TriggerOkresChanged();
        }

        protected void btEnd_Click(object sender, EventArgs e)
        {
            DateTime prevD = OkresDo;
            Prepare(DateTime.Today, PracId, false);
            if (prevD != OkresDo)
                TriggerOkresChanged();
        }

        private void Jump(int cnt)
        {
            DateTime dt;
            DateTime prevD = OkresOd;
            
            cnt = cnt * IloscMiesiecy;

            if (cnt < 0)
            {
                //DataRow dr = GetPrev(prevD, PracId, -cnt);
                DataRow dr = GetOkres(prevD, PracId, cnt);
                if (dr != null)
                    FillData(dr);
            }
            else if (cnt > 0)
            {
                //DataRow dr = GetNext(prevD, PracId, cnt);
                DataRow dr = GetOkres(prevD, PracId, cnt);
                if (dr != null)
                    FillData(dr);
            }
            if (prevD != OkresOd) 
                TriggerOkresChanged();
        }

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
        
        //------------------------------
        public DateTime OkresOd
        {
            set { ViewState["okod"] = value; }
            get { return Tools.GetDateTime(ViewState["okod"], Tools.bom(DateTime.Today)); }
        }

        public DateTime OkresDo
        {
            set { ViewState["okdo"] = value; }
            get { return Tools.GetDateTime(ViewState["okdo"], Tools.eom(DateTime.Today)); }
        }

        public string OkresOdStr
        {
            get { return Tools.DateToStr(OkresOd); }
        }

        public string OkresDoStr
        {
            get { return Tools.DateToStr(OkresDo); }
        }

        public string OkresOdStrDb
        {
            get { return Tools.DateToStrDb(OkresOd); }
        }

        public string OkresDoStrDb
        {
            get { return Tools.DateToStrDb(OkresDo); }
        }

        public int Status
        {
            set { ViewState["status"] = value; }
            get { return Tools.GetInt(ViewState["status"], okOpened); }
        }

        public string PracId
        {
            set { ViewState["pracid"] = value; }
            get { return Tools.GetStr(ViewState["pracid"]); }
        }

        public int IloscMiesiecy
        {
            set { ViewState["ilmies"] = value; }
            get 
            { 
                int m = Tools.GetInt(ViewState["ilmies"], 1);
                if (m < 1) m = 1;
                return m;
            }
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

        public string SesOkresId
        {
            get;
            set;
        }

        public DateTime? SesOkres
        {
            set { Session[SesOkresId ?? ID] = value; }
            get
            {
                object o = Session[SesOkresId ?? ID];
                //return o != null ? (DateTime)o : null;
                if (o != null)
                    return (DateTime)o;
                else
                    return null;
            }
        }
    }
}