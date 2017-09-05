using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls.Portal
{
    public partial class cntSqlEdit : System.Web.UI.UserControl
    {
        public event EventHandler Close;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //-------------------------------
        public void Prepare(string id)
        {
            Id = id;
            if (!String.IsNullOrEmpty(id))
            {
                DataRow dr = db.getDataRow(String.Format("select * from {0}..SqlContent where Id = {1}", App.dbPORTAL, id));
                if (dr != null)
                {
                    tbSql.Text = db.getValue(dr, "Sql");

                }
            }
        }

        public void Show(string id)
        {
            Prepare(id);
            Visible = true;
        }

        public void Hide()
        {
            Visible = false;
        }
        //-------------------------------
        private void TriggerClose()
        {
            if (Close != null)
                Close(this, EventArgs.Empty);
        }

        private bool Update(string id)
        {
            if (String.IsNullOrEmpty(id))       // insert
            {
                return false;
            }
            else                                // update 
            {
                bool ok = db.update(String.Format("{0}..SqlContent", App.dbPORTAL), 0, "Sql", "Id=" + id, db.nullStrParam(db.sqlPut(tbSql.Text)));
                return ok;
            }
        }


        protected void btSave_Click(object sender, EventArgs e)
        {
            if (Update(Id))
                TriggerClose();
            else
                Tools.ShowError("Wystąpił błąd podczas zapisu.");
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            TriggerClose();
        }

        //--------------------------------
        public string Id
        {
            set { ViewState["sqlid"] = value; }
            get { return Tools.GetStr(ViewState["sqlid"]); }
        }
    }
}