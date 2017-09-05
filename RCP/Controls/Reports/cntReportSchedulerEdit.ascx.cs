using HRRcp.App_Code;
using HRRcp.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls.Reports
{
    public partial class cntReportSchedulerEdit : System.Web.UI.UserControl
    {
        public event EventHandler Changed;

        const int moAll = 0;
        const int moPortal = 1;
        int FMode = moAll;

        const string conPORTAL = "PORTAL"; 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                switch (FMode)
                {
                    case moPortal:
                        
                        break;
                }
            }
        }

        //----------------------
        public void FillData(int mode)
        {
            DataRow dr;
            string id = Id;
            string select = dsRaportyScheduler.SelectCommand;
            if (!String.IsNullOrEmpty(id))
                dr = db.Select.Row(con, select, "S.Id", id, DB); 
            else if (!String.IsNullOrEmpty(Grupa))
                dr = db.Select.Row(con, select, "S.Grupa", db.strParam(Grupa), DB); 
            else
                dr = db.Select.Row(con, select, 0, 0, DB);   // pierwszy z listy 
            if (dr != null && String.IsNullOrEmpty(id))
                Id = db.getValue(dr, "Id");
            dbField.FillData(this, dr, 0, 0, 0, mode);

            
            // plomba
            //string rap = db.getValue(dr, "IdRaportu");
            //string typ = db.getValue(dr, "InterwalTyp");
            //Tools.SelectItem(IdRaportu.DdlValue, rap);
            //Tools.SelectItem(InterwalTyp.DdlValue, typ);


            SetIntervalVisible();
        }
        /*
        private DataRow Create(SqlConnection con)
        {
            string id = null;
            bool ok = db.insert(con, out id, "RaportyScheduler", "IdPracownika,IdRaportu,Parametry,Typ,Email,cc,bcc,DataStartu,DataStopu,InterwalTyp,Interwal,InterwalSql,NextStart,Status,Aktywny,LastStart,LastStop,LastError,Grupa", );
            if (ok)
                return db.Select.Row(con, select, "Id", id);
            else
                return null;
        }
        */
        public bool Validate()
        {
            return dbField.Validate(this);
        }

        public DateTime GetNextStart(string ityp, DateTime start, int interval)
        {
            string datastartu = Tools.DateToStrDb(start);  // nie puści jak null
            DataRow dr = db.Select.Row(con, dsInterwaly.UpdateCommand, datastartu, ityp, interval);
            return ((DateTime)db.getDateTime(dr, 0));  // w sql jest podstawienie Next = DataStartu
        }

        public bool Update()
        {
            string where;
            string id = Id;
            /*
            string typ = InterwalTyp.Value;
            string datastartu = Tools.DateToStrDb((DateTime)DataStartu.AsDate);  // nie puści jak null
            int interval = Interwal.AsInt ?? 0;
            DataRow dr = db.Select.Row(dsInterwaly, datastartu, typ, interval);
            string datanext = Tools.DateToStrDb((DateTime)db.getDateTime(dr, 0));  // w sql jest podstawienie Next = DataStartu
            */
            DateTime next = GetNextStart(InterwalTyp.Value, (DateTime)DataStartu.AsDate, Interwal.AsInt ?? 0);
            string datanext = db.strParam(Tools.DateToStrDb(next));

            if (String.IsNullOrEmpty(id))
            {
                int ret = dbField.dbInsert(con, this, "RaportyScheduler", "Grupa,Typ,NextStart,Status", db.strParam(Grupa), cntReportScheduler.typCSV, datanext, 0);
                if (ret > 0)
                {
                    Id = ret.ToString();
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return dbField.dbUpdate(con, this, "RaportyScheduler", "Id=" + id, "NextStart,Status", datanext, 0);
            }
        }

        private void TriggerChanged()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }
        //----------------------
        protected void wbtEdit_Click(object sender, EventArgs e)
        {
            FillData(dbField.moEdit);
        }

        protected void wbtSave_Click(object sender, EventArgs e)
        {
            if (Validate())
                if (Update())
                {
                    FillData(dbField.moQuery);
                    UbezpieczeniaParametry.GetData();
                    TriggerChanged();
                }
        }

        protected void wbtCancel_Click(object sender, EventArgs e)
        {
            FillData(dbField.moQuery);   // przywracam poprzednie wartości, ustawia query
        }

        private bool SetIntervalVisible()
        {
            string typ = InterwalTyp.DdlValue.SelectedValue;
            bool v = cntReportScheduler.IsIntervalVisible(typ);
            Interwal.Visible = v;
            return v;
        }

        protected void InterwalTyp_Changed(object sender, EventArgs e)
        {
            SetIntervalVisible();
        }
        //----------------------
        public int Mode
        {
            set 
            { 
                FMode = value;
                paButtons.Visible = FMode == moAll;
            }
            get { return FMode; }
        }

        private SqlConnection con
        {
            get
            {
                if (Connection == conPORTAL)   // albo od switch'a PORTAL
                    return db.conP;
                else 
                    return db.con;
            }
        }

        private string DB
        {
            get
            {
                if (Connection == conPORTAL)   // albo od switch'a PORTAL
                    return App.dbPORTAL + "..";
                else
                    return null;
            }
        }

        public string Id
        {
            set { ViewState["id"] = value; }
            get { return Tools.GetStr(ViewState["id"]); }
        }

        public string Grupa 
        {
            set { hidGrupa.Value = value; }
            get { return hidGrupa.Value; } 
        }

        public string Connection { set; get; }

    }
}