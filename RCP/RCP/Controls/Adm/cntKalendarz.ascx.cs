using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Controls.Adm
{
    public partial class cntKalendarz : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(gvKalendarz, null, false, 12, false, ltTh.Text, ltTd.Text);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                deRok.Date = Tools.boy(DateTime.Today);
                Tools.MakeConfirmDeleteRecordButton(btDelete);
                ShowSwietaStale(false);   // to zainicjuje hidSwStale
            }
        }

        private void ShowEdit(string[] par)
        {
            if (par.Length == 3)
            {
                DataRow dr = db.Select.Row(dsEdit, par[1], par[2]);    // data, idx
                if (dr != null)   // jak null tzn, że poza zakresem dat - nic nie robię
                {
                    DateTime dt = (DateTime)db.getDateTime(dr, "Data");
                    string rodzaj = db.getValue(dr, "Rodzaj");
                    string opis = db.getValue(dr, "Opis");
                    dow = db.getInt(dr, "dow", -1);
                    Data = Tools.DateToStrDb(dt);
                    cntModalEdit.Show(false);
                    if (SwStale)
                        cntModalEdit.Title = String.Format("Edycja świąt stałych: ROK{0}", Tools.DateToStr(dt).Substring(4));
                    else
                        cntModalEdit.Title = String.Format("Edycja: {0} ({1})", Tools.DateToStr(dt), Tools.GetDayName(dt));
                    if (SwStale)
                    {
                        ddlRodzaj.Enabled = false;
                        Tools.SelectItemByParam(ddlRodzaj, 0, "2");   //święto
                        tbOpis.Focus();
                    }
                    else
                    {
                        ddlRodzaj.Enabled = true;
                        Tools.SelectItemByParam(ddlRodzaj, 0, rodzaj);
                    }
                    tbOpis.Text = opis;
                    btDelete.Visible = !String.IsNullOrEmpty(rodzaj);
                }
            }
        }

        protected void gvKalendarzCmd_Click(object sender, EventArgs e)
        {
            string[] par = Tools.GetLineParams(gvKalendarzCmdPar.Value);
            if (par.Length > 0)
            {
                switch (par[0])
                {
                    case "edit":
                        ShowEdit(par);
                        break;
                }
            }
        }

        protected void deRok_DateChanged(object sender, EventArgs e)
        {
            /*
            if (deRok.Date != null)
            {
                DataRow dr = db.selectRow("select top 1 * from Kalendarz where Data >= dbo.boy('{0}')");
            }
             */ 
        }

        protected void gvKalendarz_DataBound(object sender, EventArgs e)
        {
            bool v = false;
            if (deRok.Date != null)
            {
                DateTime dt = (DateTime)deRok.Date;
                DataRow dr = db.selectRow("select top 1 * from Kalendarz where Data between dbo.boy('{0}') and dbo.eoy('{0}')", Tools.DateToStrDb(dt));
                v = dr == null;
                if (v)
                    Tools.MakeConfirmButton(btGenerate, String.Format("Potwierdź wygenerowanie kalendarza na rok {0}.\\n\\nUwaga!!!\\nPo wygenerowaniu, do kalendarza należy wprowadzić święta ruchome.", dt.Year));
                else
                    Tools.MakeConfirmButton(btDeleteKal, String.Format("Potwierdź usunięcie kalendarza na rok {0}.", dt.Year));

            }
            btGenerate.Enabled = v;
            btDeleteKal.Enabled = !v;
        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
        }

        private bool Generate()
        {
            if (deRok.Date != null)
            {
                bool ok = db.Execute(dsGenerate, Tools.DateToStrDb((DateTime)deRok.Date));
                if (!ok)
                    Tools.ShowError("Wystąpił błąd podczas generowania kalendarza.");
                return ok;
            }
            return false;
        }

        private bool DeleteKal()
        {
            if (deRok.Date != null)
            {
                bool ok = db.Execute(dsDelete, Tools.DateToStrDb((DateTime)deRok.Date));
                if (!ok)
                    Tools.ShowError("Wystąpił błąd podczas usuwania kalendarza.");
                return ok;
            }
            return false;
        }

        protected void btGenerate_Click(object sender, EventArgs e)
        {
            if (Generate())
                gvKalendarz.DataBind();
        }

        protected void btDeleteKal_Click(object sender, EventArgs e)
        {
            if (DeleteKal())
                gvKalendarz.DataBind();
        }
        //------------------------------
        protected void ddlRodzaj_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] par = Tools.GetLineParams(ddlRodzaj.SelectedValue);
            if (par.Length == 3)
            {
                int rodzaj = Tools.StrToInt(par[0], -99);
                switch (rodzaj)
                {
                    case 0:
                    case 1:
                        tbOpis.Text = par[2];
                        break;
                }
            }
        }

        private bool Validate()
        {
            bool ok = false;
            string sel = ddlRodzaj.SelectedValue;
            if (String.IsNullOrEmpty(sel))
                Tools.ShowError("Proszę wybrać rodzaj dnia.");
            else
            {
                string[] par = Tools.GetLineParams(sel);
                if (par.Length == 3)
                {
                    ok = false;
                    int rodzaj = Tools.StrToInt(par[0], -99);
                    if (rodzaj == 0 && dow != 5) Tools.ShowError("Wybrany dzień nie jest sobotą.");
                    else if (rodzaj == 1 && dow != 6) Tools.ShowError("Wybrany dzień nie jest niedzielą.");
                    else ok = true;
                }
            }
            return ok;
        }

        private bool Save()
        {
            string rodzaj = Tools.GetLineParam(ddlRodzaj.SelectedValue, 0);
            string opis = tbOpis.Text;

            bool ok = db.update("Kalendarz", String.Format("Data='{0}'", Data), "Rodzaj,Opis", rodzaj, opis);    // najpierw update
            if (!ok)
            {
                string id;
                ok = db.insert(out id, "Kalendarz", "Data,Rodzaj,Opis", Data, rodzaj, opis);                // jak nie ma to insert
            }
            if (!ok)
                Tools.ShowError("Wystąpił błąd podczas zapisu.");
            return ok;
        }

        private bool Delete()
        {
            bool ok = db.delete("Kalendarz", String.Format("Data='{0}'", Data));  // może nie istnieć więc ok = false <<< jak inczej pokazać że błąd ? exception ?
            if (!ok)
                Tools.ShowError("Wystąpił błąd podczas usuwania wpisu.");
            return ok;
        }

        /* nie gasi back modala - rozwiązanie osobny modal na kontrolkę i modal poza nim
        private void Update()
        {
            UpdatePanel up = Tools.FindUpdatePanel(this);
            if (up.UpdateMode == UpdatePanelUpdateMode.Conditional)
                up.Update();
        }
        */
        protected void btSave_Click(object sender, EventArgs e)
        {
            if (Validate())
                if (Save())
                {
                    cntModalEdit.Close();
                    gvKalendarz.DataBind();
                }
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            if (Delete())
            {
                cntModalEdit.Close();
                gvKalendarz.DataBind();
            }
        }
        //------------------------------
        private void ShowSwietaStale(bool swstale)
        {
            SwStale = swstale;
            paFilterKal.Visible = !swstale;
            btSwietaStale.Visible = !swstale;
            btSwietaStaleBack.Visible = swstale;
        }

        protected void btSwietaStale_Click(object sender, EventArgs e)
        {
            ShowSwietaStale(true);
        }

        protected void btSwietaStaleBack_Click(object sender, EventArgs e)
        {
            ShowSwietaStale(false);
        }
        //------------------------------
        private string Data
        {
            set { ViewState["data"] = value; }
            get { return Tools.GetStr(ViewState["data"]); }
        }

        private int dow
        {
            set { ViewState["dow"] = value; }
            get { return Tools.GetInt(ViewState["dow"], -1); }
        }

        private bool SwStale
        {
            set { hidSwStale.Value = value ? "1" : "0"; }
            get { return Tools.StrToInt(hidSwStale.Value, 0) == 1; }
        }
    }
}