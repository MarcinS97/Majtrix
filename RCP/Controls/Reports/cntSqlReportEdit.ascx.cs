using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls.Reports
{
    public partial class cntSqlReportEdit : System.Web.UI.UserControl
    {
        public event EventHandler Save;
        public event EventHandler Cancel;

        const string zoomId = "cntSqlReportEditZoom";
        const string tbRaporty = "SqlMenu";
        const string tbFiltry  = "SqlFields";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public bool Show(string repId, string grupa)
        {
            const int width = 1100;

            ReportId = repId;
            Grupa = grupa;
            paEdit.Visible = true;
            if (!String.IsNullOrEmpty(repId))
            {
                Id.ReadOnly = true;    
                DataSet ds = db.getDataSet(String.Format("select * from {1} where Id = {0}", repId, tbRaporty));
                if (db.getCount(ds) > 0)
                {
                    Tools.SetData(this, ds);
                    Tools.ShowDialog(this, zoomId, width, btCancel, true, "Edytuj");
                    return true;
                }
                else
                {
                    Tools.ShowErrorLog(Log.RAPORTY, "Brak definicji raportu.", repId);
                    return false;
                }
            }
            else
            {
                Id.ReadOnly = false;
                Id.Text = db.getScalar(String.Format("select ISNULL((select max(Id) + 1 from {0}), 1)", tbRaporty));
                Tools.ShowDialog(this, zoomId, width, btCancel, true, "Dodaj raport");
                return true;
            }
        }

        public bool Delete(string repId)
        {
            bool ok = db.execSQL(String.Format(@"
delete from {2} where IdRaportu = {0}
delete from {1} where Id = {0}
                ", repId, tbRaporty, tbFiltry));
            return ok;
        }
        //-------------------
        private object[] AsParams(object[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                object o = data[i];
                if (o is String)
                    data[i] = db.nullStrParam(o.ToString().Replace("'", "''"));
                else if (o is Boolean)
                    data[i] = (bool)o ? 1 : 0;
            }
            return data;
        }

        private bool DoSave()
        {
            const string insFields = "Grupa,Id,MenuText,ToolTip,Sql,Aktywny,SqlParams,Par1,Par2,Rights,Mode,Javascript,Class";
            const string updFields = "MenuText,ToolTip,Sql,Aktywny,SqlParams,Par1,Par2,Rights,Mode,Javascript,Class";
            //const string updFields = "Sql";
            string repId = ReportId;
            bool ok;
            if (String.IsNullOrEmpty(repId))
            {
                object[] data = Tools.GetData(this, insFields, Grupa);
                ok = db.insert(tbRaporty, 0, insFields, AsParams(data));
            }
            else
            {
                object[] data = Tools.GetData(this, updFields);
                ok = db.update(tbRaporty, 0, updFields, "Id=" + repId, AsParams(data));
            }
            return ok;
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            if (DoSave())
            {
                paEdit.Visible = false;
                Tools.CloseDialog(zoomId);
                if (Save != null)
                    Save(this, EventArgs.Empty);
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            paEdit.Visible = false;
            Tools.CloseDialog(zoomId);
            if (Cancel != null)
                Cancel(this, EventArgs.Empty);
        }
        //-------------------
        public string ReportId
        {
            set { ViewState["repid"] = value; }
            get { return Tools.GetStr(ViewState["repid"]); }
        }

        public string Grupa
        {
            set { ViewState["repgrp"] = value; }
            get { return Tools.GetStr(ViewState["repgrp"]); }
        }

    }
}