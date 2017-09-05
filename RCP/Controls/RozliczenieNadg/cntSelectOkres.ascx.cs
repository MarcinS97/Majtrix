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
    public partial class cntSelectOkres : System.Web.UI.UserControl
    {
        public event EventHandler OkresChanged;
        
        public const int okOpened = 0;
        public const int okClosed = 1;

        private bool FEnabled = true;
        //▌▐¦‖│| ►◄

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void Prepare(DateTime data, bool getFromSession)
        {
            if (getFromSession)
            {
                DateTime? dt = SesOkres;
                if (dt != null) data = (DateTime)dt;
            }

            DataRow dr = GetOkres(data);
            if (dr == null)
            {
                dr = GetOkresLastBefore(data);
                if (dr == null)
                    dr = GetOkresFirstAfter(data);
            }
            FillData(dr);
        }

        //--------------------------------
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

        public static DataRow GetOkres(DateTime d)
        {
            return db.getDataRow(String.Format("select * from OkresyRozliczeniowe where '{0}' between DataOd and DataDo", Tools.DateToStrDb(d)));
        }

        //-------+=====+=====+=====+-----
        //-----|---------------|------|--
        // FirstAfter              LastBefore
        public static DataRow GetOkresLastBefore(DateTime d)
        {
            return db.getDataRow(String.Format("select top 1 * from OkresyRozliczeniowe where DataDo < '{0}' order by DataOd desc", Tools.DateToStrDb(d)));
        }

        public static DataRow GetOkresFirstAfter(DateTime d)
        {
            return db.getDataRow(String.Format("select top 1 * from OkresyRozliczeniowe where DataOd > '{0}' order by DataOd", Tools.DateToStrDb(d)));
        }

        //+=====+=====+=====+=====+=====+-----
        //---^--|------------------------ +3
        //---------------------^---------
        public static DataRow GetNext(DateTime d, int cnt)  // d - dowolna data z biezacego okresu
        {
            DataSet ds = db.getDataSet(String.Format("select top {1} * from OkresyRozliczeniowe where DataOd > '{0}' order by DataOd", Tools.DateToStrDb(d), cnt));
            int c = db.getCount(ds);
            if (c > 0)
                return db.getRow(ds, c - 1);  // ostatni
            else
                return null;
        }

        //+=====+=====+=====+=====+=====+-----
        //------------------------|--^--|----- -3
        //---------^--------------------------
        public static DataRow GetPrev(DateTime d, int cnt)  // d - dowolna data z biezacego okresu
        {
            DataSet ds = db.getDataSet(String.Format("select top {1} * from OkresyRozliczeniowe where DataDo < '{0}' order by DataOd desc", Tools.DateToStrDb(d), cnt));
            int c = db.getCount(ds);
            if (c > 0)
                return db.getRow(ds, c - 1);  // ostatni
            else
                return null;
        }

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
            Prepare(DateTime.Parse("1900-01-01"), false);
            if (prevD != OkresOd) 
                TriggerOkresChanged();
        }

        protected void btEnd_Click(object sender, EventArgs e)
        {
            DateTime prevD = OkresDo;
            Prepare(DateTime.Today, false);
            if (prevD != OkresDo)
                TriggerOkresChanged();
        }

        private void Jump(int cnt)
        {
            DateTime dt;
            DateTime prevD = OkresOd;
            if (cnt < 0)
            {
                DataRow dr = GetPrev(prevD, -cnt);
                if (dr != null)
                    FillData(dr);
            }
            else if (cnt > 0)
            {
                DataRow dr = GetNext(prevD, cnt);
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