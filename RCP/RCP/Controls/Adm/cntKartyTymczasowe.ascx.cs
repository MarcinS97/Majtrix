using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls
{
    public partial class cntKartyTymczasowe : System.Web.UI.UserControl
    {
        const int moWydanie  = 1;
        const int moZwrot    = 2;
        const int moHistoria = 3;
        
        protected void Page_Init(object sender, EventArgs e)
        {
            //Grid.Prepare(gvPracownicy, "GridView1", true, 25, true);
            Grid.Prepare(gvPracownicy);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidUserId.Value = App.User.Id;  // original ?
                PrepareSearch();
            }
        }

        protected void gvPracownicyCmd_Click(object sender, EventArgs e)
        {
            string[] par = Tools.GetLineParams(gvPracownicyCmdPar.Value);
            //string[] par = Grid.DecryptParams(gvPracownicyCmdPar.Value);
            switch (par[0])
            {
                case "ajax":
                    if (par.Length >= 5)  // cmd,typ,pid,par,ret
                    {
                        switch (Tools.StrToInt(par[1], -1))
                        {
                            case 1:
                                gvPracownicy.DataBind();
                                break;
                        }
                    }
                    break;
                case "action":
                    if (par.Length >= 3)
                    {
                        string pid  = par[1];
                        string pkid = par[2];
                        PracId = pid;
                        PracKartyId = pkid;
                        int mode = Mode;
                        paWydanie.Visible = mode == moWydanie;
                        paZwrot.Visible   = mode == moZwrot;
                        btWydaj.Visible   = mode == moWydanie;
                        btZwrot.Visible   = mode == moZwrot;
                        DataRow dr = AppUser.GetData(pid);
                        string nrew = db.getValue(dr, "KadryId");
                        lbPracownik.Text = String.Format("{0} {1}", db.getValue(dr, "Nazwisko"), db.getValue(dr, "Imie")); 
                        lbNrEwid.Text = nrew;
                        cntAvatar.NrEw = nrew;
                        switch (mode)
                        {
                            case moWydanie:
                                ltTitle.Text = "Wydawanie identyfikatora tymczasowego";
                                if (tbNrKarty.Visible)
                                    tbNrKarty.Text = null;
                                else
                                    ddlNrKarty.DataBind();  // odświeży co jest już wydane
                                
                                cntModalEdit.Show(false);

                                if (tbNrKarty.Visible)
                                    tbNrKarty.Focus();
                                else
                                    ddlNrKarty.Focus();                                
                                break;
                            case moZwrot:
                                ltTitle.Text = "Zwrot identyfikatora tymczasowego";
                                string prac, karta, data;
                                GetInfo(pkid, out prac, out karta, out data);
                                lbNrKarty.Text = karta;
                                deData.Date = DateTime.Today;
                                cntModalEdit.Show(false);
                                deData.EditBox.Focus();
                                break;
                        }
                    }
                    break;
            }
        }

        private bool GetInfo(string pkid, out string prac, out string karta, out string data)
        {
            DataSet ds = db.selectSQL(dsKartyTmp.DeleteCommand, pkid);
            if (db.getCount(ds) > 0)
            {
                DataRow dr = db.getRow(ds);
                prac = db.getValue(dr, "Pracownik");
                karta = db.getValue(dr, "Karta");
                data = db.getValue(dr, "Data");
                return true;
            }
            else
            {
                prac = null;
                karta = null;
                data = null;
                return false;
            }
        }

        private bool MoznaWydac(string rcpid)
        {
            DataSet ds = db.selectSQL(dsKartyTmp.SelectCommand, rcpid); 
            if (db.getCount(ds) > 0)
            {
                DataRow dr = db.getRow(ds);
                string prac  = db.getValue(dr, "Pracownik");
                string karta = db.getValue(dr, "Karta");
                string data  = db.getValue(dr, "Data");
                Tools.ShowError("Wydanie niemożliwe.\nKarta {0} została już wydana {1} pracownikowi {2}.\n\nPrzed ponownym wydaniem, proszę dokonać zwrotu karty.", karta, data, prac);
                return false;
            }
            else
                return true;
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            bool ok = true;
            int mode = Mode;
            switch (mode)
            {
                case moWydanie:
                    string tid, rcpid, nrkarty;
                    Tools.GetLineParams(ddlNrKarty.SelectedValue, out tid, out rcpid, out nrkarty);
                    if (MoznaWydac(rcpid))
                    {
                        ok = db.execSQL(dsKartyTmp.InsertCommand
                            , PracId
                            , rcpid
                            , nrkarty
                            , App.User.OriginalId);
                        if (ok)
                        {
                            cntModalEdit.Close();
                            gvPracownicy.DataBind();
                        }
                    }
                    break;
                case moZwrot:
                    ok = db.execSQL(dsKartyTmp.UpdateCommand, PracId, PracKartyId, deData.Date);
                    if (ok) 
                    {
                        cntModalEdit.Close();
                        gvPracownicy.DataBind();
                    }
                    break;
            }
            if (!ok)
                Tools.ShowError("Wystąpił błąd podczas zapisu.");
        }

        private int Mode
        {
            get { return Tools.StrToInt(tabMode.SelectedValue, -1); }
        }

        private string PracId
        {
            set { ViewState["pracid"] = value; }
            get { return Tools.GetStr(ViewState["pracid"]); }
        }

        private string PracKartyId
        {
            set { ViewState["pkid"] = value; }
            get { return Tools.GetStr(ViewState["pkid"]); }
        }
        
        //----- FILTER ----------------------------------
        private void PrepareSearch()
        {
            lbtClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('').focus();return false;", tbSearch.ClientID, btSearch.ClientID);
            Tools.ExecOnStart2("searchtrigprac", String.Format("startSearch('{0}','{1}');", tbSearch.ClientID, btSearch.ClientID));
            tbSearch.Focus();
        }

        public void FocusSearch()
        {
            tbSearch.Focus();
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            //podstback
        }

        protected void gvPracownicy_Load(object sender, EventArgs e)
        {
        }
        //-----------------------------------       
        public int RowsCount
        {
            set { ViewState["gvrows"] = value; }
            get { return Tools.GetInt(ViewState["gvrows"], 0); }
        }

        protected void ddlLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ln = ddlLines.SelectedValue;
            if (ln == "all")
            {
                //gvPracownicy.AllowPaging = false;
                gvPracownicy.PageSize = RowsCount;
            }
            else
            {
                //gvPracownicy.AllowPaging = true;
                gvPracownicy.PageSize = Tools.StrToInt(ln, 25);
            }
        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            RowsCount = e.AffectedRows;
            lbCount.Text = e.AffectedRows.ToString();
        }

        protected void tabMode_MenuItemClick(object sender, MenuEventArgs e)
        {

        }
    }
}