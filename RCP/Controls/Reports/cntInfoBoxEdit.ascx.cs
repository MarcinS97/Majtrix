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
    public partial class cntInfoBoxEdit : System.Web.UI.UserControl
    {
        public event EventHandler Save;
        public event EventHandler Cancel;

        const string zoomId = "cntInfoBoxEditZoom";
        const string tbName = "SqlBoxes";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        public bool Show(string id, string grupa)
        {
            const int width = 1100;

            BoxId = id;
            Grupa = grupa;
            paEdit.Visible = true;
            if (!String.IsNullOrEmpty(id))
            {
                Id.ReadOnly = true;
                DataSet ds = db.select("select CHECKSUM(*), * from {0} where Id = {1}", tbName, id);
                if (db.getCount(ds) > 0)
                {
                    DataRow dr = db.getRow(ds);
                    string nazwa = db.getValue(dr, "Nazwa");
                    Checksum = db.getValue(dr, 0); 
                    Tools.SetData(this, ds);
                    Tools.ShowDialog(this, zoomId, width, btCancel, true, "Edytuj" + (String.IsNullOrEmpty(nazwa) ? null : " - " + nazwa));
                    return true;
                }
                else
                {
                    Tools.ShowErrorLog(Log.RAPORTY, "InfoBox", String.Format("Brak definicji panelu informacyjnego. Id: {0}", id));
                    return false;
                }
            }
            else
            {
                Id.ReadOnly = false;
                string newid = db.getScalar(String.Format("select ISNULL((select max(Id) + 1 from {0}), 1)", tbName));   // uwaga! jakby było jednoczesne dodawanie to ten mechanizm się nie sprawdzi !!!
                Id.Text = newid;
                CssClass.Text = "ibox-" + newid;
                Tools.ShowDialog(this, zoomId, width, btCancel, true, "Dodaj panel informacyjny");
                return true;
            }
        }

        public bool Add(string id, string grupa)
        {
            return Show(null, grupa);
        }

        public bool Delete(string id)
        {
            bool ok = db.execSQL(String.Format(@"delete from {1} where Id = {0}", id, tbName));
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
            const string updFields = "Html1,Html2,HtmlEmpty,Css,Script,Nazwa,CssClass,Sql,Par1,Par2,Command,Aktywny,Mode,Rights,Kolejnosc,NowaLinia,Typ";
            const string insFields = "Grupa,Id," + updFields;
            //const string updFields = "Sql";
            string id = BoxId;
            bool ok;
            if (String.IsNullOrEmpty(id))
            {
                object[] data = Tools.GetData(this, insFields, Grupa);  // jak nie ma kontrolki to pola dodatkowe na początek!
                ok = db.insert(tbName, 0, insFields, AsParams(data));
            }
            else
            {
                object[] data = Tools.GetData(this, updFields);
                ok = db.update(tbName, 0, updFields, "Id=" + id, AsParams(data));
                /*
                - otwarcie transakcji
                if (ChecksumOk(tbName, Checksum)
                    ok = db.update(tbName, 0, updFields, "Id=" + id, AsParams(data));
                else
                {
                    Tools.ShowError("Rekord został w międzyczasie zmodyfikowany. Zapis niemożliwy.");  - albo Potwierdź nadpisanie danych ...
                    ok = false;
                }
                - lub w jednej funkcji
                err = db.update(tbName, 0, updFields, "Id=" + id, Checksum, AsParams(data));
                if (err == -1) Tools.ShowError();
                */
            }
            return ok;
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            if (DoSave())
            {
                paEdit.Visible = false;
                //Tools.CloseDialog(zoomId);
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

        protected void tabDane_MenuItemClick(object sender, MenuEventArgs e)
        {
            Tools.SelectTab(tabDane, mvDane, null, false);
        }
        //-------------------
        public string BoxId
        {
            set { ViewState["iboxid"] = value; }
            get { return Tools.GetStr(ViewState["iboxid"]); }
        }

        public string Grupa
        {
            set { ViewState["iboxgrp"] = value; }
            get { return Tools.GetStr(ViewState["iboxgrp"]); }
        }

        public string Checksum
        {
            set { ViewState["iboxchs"] = value; }
            get { return Tools.GetStr(ViewState["iboxchs"]); }
        }
    }
}