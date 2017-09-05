using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.Controls;
using System.Data;
using System.Text;
using HRRcp.Scorecards.App_Code;
using HRRcp.RCP.App_Code;

namespace HRRcp.RCP.Controls
{
    public partial class cntPlanPracyZmiany2 : System.Web.UI.UserControl
    {
        public event EventHandler DataSaved;

        bool v = false;
        bool adm, kier, ed;

        protected void Page_Load(object sender, EventArgs e)
        {
            adm = App.User.IsAdmin;
            kier = App.User.IsKierownik;
            ed = App.User.HasRight(AppUser.rPlanPracy);
            if (!IsPostBack)
            {
                string uid = App.User.Id;
                hidKierId.Value = uid;          // zawsze w kontekscie zalogowanego usera
                hidUserId.Value = uid;          // zawsze w kontekscie zalogowanego usera
                Adm = adm ? cntPlanPracy2.moAdmin : cntPlanPracy2.moKier;

                paLegenda.Visible = Lic.ppPrint;        // stare drukowanie planu pracy
                paTitle.Visible = Lic.ppPrint;
                btPrint.Visible = Lic.ppPrint;

                ddlKier.DataSourceID = adm ? dsKierAll.ID : dsKier.ID;  // filtr kierownik
                ddlKier.DataBind();
                if (ddlKier.Items.Count == 1)                           // tylko kieras
                {
                    ddlKier.Visible = false;   
                    Tools.AddClass(paSearch, "left");
                    Prepare(uid);
                }
                else if (adm && kier)
                {
                    Tools.SelectItem(ddlKier, uid);   // admin będący kierownikiem
                    Prepare(uid);
                }

                lnkEditSchemesModal.Visible = adm;
                btnEditZmiany.Visible = adm;
                //bool toAcc = Tools.GetStr(Request.QueryString["p"], "0") == "1";
                //btnAccept.Visible = ed && toAcc;
                //btnReject.Visible = ed && toAcc;

                SetEditMode(false);
                DoSearch(true);     // ustawienie filtra domyślnego
            }
            else
            {
                v = Visible;        // jak !IsPostBack i Visible tzn ze startuje od formatki z kontrolką
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (Visible && Visible != v)   // nastąpiła zmiana, kontrolka jak nie jest widoczna to zwarac Visible = false niezaleznie czy jest ustawiane czy nie
                PrepareSearch();
            base.OnPreRender(e);
        }
        //---------------------------
        public void Prepare()
        {
            cntSelectOkres.Prepare(DateTime.Today, true);

            /*
            cntPlanPracy.Prepare(hidKierId.Value, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, cntSelectOkres.OkresId, cntSelectOkres.Status, cntSelectOkres.IsArch);
            SetPrintHeader();
             */ 
        }

        public void Prepare(string kierId)
        {
            //hidKierId.Value = kierId;
            
            cntPlanPracy.PracId = null;
            //cntPlanPracy.Prepare(kierId, cntSelectOkres.DateFrom, cntSelectOkres.DateTo);
            //cntPlanPracy.Prepare(kierId);
            ddlVer.DataBind();
            cntPlanPracy.Version = ddlVer.SelectedValue;
            cntPlanPracy.Prepare(kierId, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, cntSelectOkres.OkresId, cntSelectOkres.Status, cntSelectOkres.IsArch);
            //cntPlanPracy.DataBind();
            SetPrintHeader();
        }

        /*
        public void Prepare(string kierId, DateTime nadzien, bool restoreFromSession)
        {
            hidKierId.Value = kierId;

            cntSelectOkres.Prepare(nadzien, restoreFromSession);
            cntPlanPracy.Prepare(kierId, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, cntSelectOkres.OkresId, cntSelectOkres.Status, cntSelectOkres.IsArch);
            SetPrintHeader();
        }
        */

        public void SetEditMode(bool edit)
        {
            bool ed = App.User.HasRight(AppUser.rPlanPracy);
            bool em = ed && edit;
            bool nem = ed && !edit;

            cntPlanPracy.EditMode = em;
            cntSelectZmiana.EditMode = em;
            //Schematy.EditMode = em;

            btSetScheme.Enabled = edit;
            

            btEditPP.Visible = nem;
            btSavePP.Visible = em;
            btCancelPP.Visible = em;

            if (Lic.HarmAcc)
            {
                btnCopyFromModal.Visible = ed;
                btnSendToAccConfirm.Visible = ed;
                btSetScheme.Visible = ed;

                btnCopyFromModal.Enabled = em;
                btCheckPP.Enabled = em;
                btnSendToAccConfirm.Visible = nem;
                btnPrintHar.Enabled = !edit;
                btnPrintList.Enabled = !edit;
                btSetScheme.Enabled = em;
                
                //Schematy.EditMode = em;
            }
            if (Lic.ppPrint)    // wydruk z kontrolki
            {
                btPrint.Visible = !edit;
            }

            //Schematy.DataBind();  //T: czemu to tu ?

            //cntPlanPracy.PracCheckbox = !edit;

            //paZmianySchematy.Visible = edit;          
            //paZmianyTools.Visible = edit;
            //lbZmianyQ.Visible = !edit;
            //lbZmianyE.Visible = edit;
            //lbPlanQ.Visible = !edit;
            //lbPlanE.Visible = edit;

            cntSelectOkres.Enabled = !edit;
            ddlKier.Enabled = !edit;
            //tbSearch.Enabled = edit;
            //btClear.Enabled = edit;
        }
        //----------------------------
        private void SetPrintHeader()
        {
            if (Lic.ppPrint)
            {
                string okres = cntSelectOkres.FriendlyName(1);
                string kid = cntPlanPracy.IdKierownika;
                if (String.IsNullOrEmpty(kid))
                {
                    repHeader.Caption = String.Format("Plan pracy: {0}", okres);
                }
                else
                {
                    string kier = AppUser.GetNazwiskoImie(cntPlanPracy.IdKierownika);
                    repHeader.Caption = String.Format("Plan pracy: {0}, przełożony: {1}", okres, kier);
                }
            }
        }
        //---------------------------
        protected void OnSelectZmiana(object sender, EventArgs e)
        {
            cntPlanPracy.Zmiana = cntSelectZmiana.SelectedZmiana;
        }

        protected void btCheckPP_Click(object sender, EventArgs e)
        {
            //bool empty
            if (cntPlanPracy.Check())
                Tools.ShowMessage("Plan pracy poprawny.");
        }

        protected void btEditPP_Click(object sender, EventArgs e)
        {
            SetEditMode(true);
        }

        protected void btSavePP_Click(object sender, EventArgs e)
        {
            Save();
        }

        bool Save()
        {
            bool good = cntPlanPracy.Check(); 
            if (good)
            {
                cntPlanPracy.Update();
                SetEditMode(false);
                if (DataSaved != null)
                    DataSaved(this, EventArgs.Empty);
            }
            return good;
        }

        protected void btCancelPP_Click(object sender, EventArgs e)
        {
            cntPlanPracy.DataBind();
            SetEditMode(false);
        }

        protected void cntSelectOkres_Changed(object sender, EventArgs e)
        {
            bool v;     // czy okres jest dostępny do akceptacji 
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            DateTime dt = DateTime.Parse(cntSelectOkres.DateTo);
            if (dt < settings.SystemStartDate)
            {
                v = false;
                lbOkresStatus.Text = "Brak danych";
            }
            else
            {
                v = cntSelectOkres.Status != Okres.stClosed;
                if (cntSelectOkres.Status == Okres.stClosed)
                    lbOkresStatus.Text = "Okres rozliczeniowy zamknięty";
            }
            btEditPP.Visible = ed && v;
            lbOkresStatus.Visible = !v;
            cntPlanPracy.OkresClosed = !v;
            
            SetPrintHeader();


            cntPlanPracy.Commodity = ddlCom.SelectedValue;
            cntPlanPracy.DataBind();
        }
        //-----------------------------
        public SelectOkres OkresRozl
        {
            get { return cntSelectOkres; }
        }

        public int Adm
        {
            set { cntPlanPracy.Adm = value; }
            get { return cntPlanPracy.Adm; }
        }
        //-----------------------------
        protected void ddlPrac_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cntPlanPracy.PracId = ddlPrac.SelectedValue;
        }

        protected void ddlKier_SelectedIndexChanged(object sender, EventArgs e)
        {
            //hidKierId.Value = ddlKier.SelectedValue;
            //Session["admKier"] = ddlKier.SelectedValue;
            Prepare(ddlKier.SelectedValue);
            ddlVer.Visible = false;//true; juan nie robimy wersji
            ddlVer.DataBind();
        }

        protected void btScheme_Click(object sender, EventArgs e)
        {
            //if (String.IsNullOrEmpty(Schematy.SelectedValue))
            //{
            //    Tools.ShowError("Proszę wybrać schemat!");
            //}
            //else
            {
                string emp = "", days = "";
                //bool good = cntPlanPracy.GetEmpDays(ref emp, ref days);
                string[] empl = cntPlanPracy.GetCheckedEmployees(false);
                if (empl.Length > 0)
                {
                    emp = String.Join(",", empl);
                    cntSetSchemesModal.Show();
                    hidSchemeEmp.Value = emp;//.Substring(1);
                    //hidSchemeDays.Value = days.Substring(1);
                    //if (days.Length > 1)
                    //{
                    //    string[] spl = days.Substring(1).Split(',');

                    //    if (spl.Length == 2)
                    //    {
                    //        string firstDate = spl[0];
                    //        string lastDate = spl[0];
                    //        if (spl.Length > 1)
                    //            lastDate = spl[spl.Length - 1];

                    //        deLeft.Date = firstDate;
                    //        deRight.Date = lastDate;
                    //    }
                    //}
                    //up2.Update();
                    //Tools.ShowBSDialog();
                }
                else
                {
                    Tools.ShowError("Nie wybrano żadnego pracownika!");
                }
            }
        }

        protected void btnSaveScheme_Click(object sender, EventArgs e)
        {
            string emp = hidSchemeEmp.Value;//, days = "";
            string dateLeft = deLeft.DateStr;
            string dateRight = deRight.DateStr;
            string scheme = ddlScheme.SelectedValue; //Schematy.SelectedValue;//ddlSchemes.SelectedValue;
            if (String.IsNullOrEmpty(emp))
                Tools.ShowError("Nie wybrano żadnego pracownika!");
            else if (String.IsNullOrEmpty(dateLeft) || String.IsNullOrEmpty(dateRight))
                Tools.ShowError("Proszę podać datę!");
            else if (String.IsNullOrEmpty(scheme))
                Tools.ShowError("Proszę wybrać schemat!");
            else if (Save())
            {
                db.Execute(dsApplyScheme, scheme, App.User.OriginalId, db.strParam(emp), db.strParam(dateLeft), db.strParam(dateRight));

                Tools.CloseBSDialog();
                cntPlanPracy.DataBind();
            }
        }

        protected void btnCopyFromModal_Click(object sender, EventArgs e)
        {
            string emp = "", days = "";
            bool good = cntPlanPracy.GetEmpDays(ref emp, ref days);

            if (good)
            {
                //hidCopyDays.Value = days.Substring(1);

                //upCopyW.Update();
                cntCopyModal.Update();
                cntCopyModal.Show();
                hidCopyEmp.Value = emp.Substring(1);
                ddlPrac2.DataBind();
                //Tools.ShowBSDialog("copyModal");
            }
            else
                Tools.ShowError("Proszę wybrać daty i pracowników!");

        }

        protected void btnCopyFrom_Click(object sender, EventArgs e)
        {
            string emp = hidCopyEmp.Value;
            string days = hidCopyDays.Value;
            string empId = ddlPrac2.SelectedValue;
            string dateLeft = deCopyLeft.DateStr;
            string dateRight = deCopyRight.DateStr;
            if (String.IsNullOrEmpty(emp))
                Tools.ShowError("Proszę zaznaczyć pracowników!");
            else if (String.IsNullOrEmpty(dateLeft) || String.IsNullOrEmpty(dateRight))
                Tools.ShowError("Proszę wybrać datę!");
            else if (String.IsNullOrEmpty(empId))
                Tools.ShowError("Proszę wybrać pracownika!");
            else if (Save())
            {
                db.Execute(dsCopyFrom, empId, App.User.OriginalId, db.strParam(emp), /*db.strParam(days)*/db.strParam(dateLeft), db.strParam(dateRight));

                //x up1.Update();
                cntPlanPracy.DataBind();

                Tools.CloseBSDialog();
            }
        }

        protected void btnPrintHar_Click(object sender, EventArgs e)
        {
            string[] emp = cntPlanPracy.GetCheckedEmployees(true);


            bool good = true;
            if (emp.Length <= 0)
                good = false;
            else
                good = PrintKier2(String.Join(",", emp));
            
            if(!good)
                Tools.ShowMessage("Błąd pobierania");
        }

        bool PrintKier2(string emp)
        {
            string title = String.Format("<span class='table-title'>HARMONOGRAM PRACY {0}</span>", cntSelectOkres.FriendlyName(1));
            string sign = "<div class='signature'>Data i podpis kierownika</div>";

            if (String.IsNullOrEmpty(emp))
                return false;

            DataSet ds = db.Select.Set(dsPrint, db.strParam(ddlKier.SelectedValue), db.strParam(cntSelectOkres.DateFrom), db.strParam(cntSelectOkres.DateTo), db.strParam(emp));
            StringBuilder sb = new StringBuilder();
            

            DataTable dtHeader = ds.Tables[0];
            DataTable dtRows = ds.Tables[1];

            int counter = 0, breakIndex = 15;
            foreach (DataRow dr in dtRows.Rows)
            {
                if((counter % breakIndex) == 0) /* print header */
                {
                    if (counter > 0)
                    {
                        sb.AppendFormat("</table>{0}</div>", sign);
                    }
                    sb.AppendFormat("<div class='break pdf-harmonogram'>{0}<table class='pdf-harmonogram-table'>", title);

                    foreach(DataRow drh in dtHeader.Rows)
                    {
                        sb.Append(PrintRow(drh));
                    }
                }
                sb.Append(PrintRow(dr));
                counter++;
            }
            sb.AppendFormat("</table>{0}</div>", sign);
            PDF PDF = new PDF();
            PDF.CssClasses[0] = "../styles/master3.css";
            PDF.Options = "-O landscape";
            if (PDF.Download(PDF.Prepare(sb.ToString(), Request), Server, Response, Request, "Harmonogram") != 0)
                return false;
            return true;
        }

        bool PrintList(string[] emp)
        {
            StringBuilder sb = new StringBuilder();
            string title = "LISTA OBECNOŚCI - EWIDENCJA CZASU PRACY";
            string subTitle1 = "<span class='label'>Pracodawca:</span><span class='pracodawca'>{0}</span>";
            string subTitle2 = "<!--<span class='label'>Lokalizacja:</span><span class='lokalizacja'>Górzykowo 1A, 66-131 Cigacice</span>-->";
            int counter = 0;
            
            sb.Append("<div class='lista-ewid'>");
            foreach(string pracId in emp)
            {
                DataTable dt = db.Select.Table(dsPrint2, db.strParam(cntSelectOkres.DateFrom), db.strParam(cntSelectOkres.DateTo), pracId);
                DataRow drPrac = db.Select.Row(dsPrint2Prac, pracId);
                DataRow drComp = db.Select.Row(dsPrint2Comp, pracId, db.strParam(cntSelectOkres.DateTo));
                if(dt != null)
                {
                    sb.Append("<div class='list-page break'>");
                        
                    sb.AppendFormat("<span class='lista-ewid-title'>{0}</span>", title);
                    if(drComp != null)
                        sb.AppendFormat("<span class='lista-ewid-subtitle1'>{0}</span>", String.Format(subTitle1, drComp["Klasyfikacja"]));
                    sb.AppendFormat("<span class='lista-ewid-subtitle2'>{0}</span>", subTitle2);

                    sb.AppendFormat("<div class='lista-ewid-header'><span class='prac'><span class='imienazw'>Imię i nazwisko pracownika:</span><span class='imienazwprac'>{1}</span></span><span class='miesiac'>miesiąc:</span><span class='miesiac2'>{0}</span></div>", cntSelectOkres.FriendlyName(1), db.getStr(drPrac["Pracownik"]));
                    sb.Append("<table class='table-ewid'>");
                    sb.Append("<tr class='tr-header'><td class='col1' colspan='3'></td><td class='col2' colspan='4'>UZUPEŁNIA PRACOWNIK</td><td class='col3' colspan='4'>UZUPEŁNIA PRZEŁOŻONY</td></tr>");
                    foreach(DataRow dr in dt.Rows)
                    {
                        sb.Append(PrintRow(dr));
                    }

#if DBW
                    /*|| VICIM || VC*/
                    sb.Append("<tr class='tr-footer'><td colspan='7'>PREMIA MOTYWACYJNA  TAK/NIE</td><td></td><td></td><td></td><td></td></tr>");
#else
                    sb.Append("<tr class='tr-footer'><td colspan='7'>&nbsp;</td><td></td><td></td><td></td><td></td></tr>");
#endif
                    sb.Append("</table>");

                    sb.Append("<span class='funn'>*niepotrzebne skreślić</span>");
                    sb.Append("<span class='facc'>Potwierdza przełożony</span>");
                    sb.Append("<span class='facc2'>Niniejszym potwierdzam wykonanie pracy oraz powyższą ewidencję czasu pracy:</span>");

                    sb.Append("<span class='fin'>imię i nazwisko</span>");
                    sb.Append("<span class='fdata'>data potwierdzenia</span>");
                    sb.Append("<span class='fpodpis'>podpis</span>");
                    
                    sb.Append("%%FOOTER%%</div>");
                }
                counter++;
            }
            sb.Append("</div>"); 
            
            PDF PDF = new PDF();
            PDF.CssClasses[0] = "../styles/master3.css";
            //PDF.Options = "-O landscape";
            if (PDF.Download(PDF.Prepare(sb.ToString(), Request), Server, Response, Request, "Lista obecności") != 0)
                return false;
            
            return true;
        }

        protected void btnPrintList_Click(object sender, EventArgs e)
        {
            string[] emp = cntPlanPracy.GetCheckedEmployees(true);
            bool good = true;
            if (emp.Length <= 0)
                good = false;
            else
                good = PrintList(emp);

            if (!good)
                Tools.ShowMessage("Błąd pobierania");
        }

        string PrintRow(DataRow dr)
        {
            StringBuilder sb = new StringBuilder();
            string rowClass = dr["RowClass"].ToString();
            sb.AppendFormat("<tr class='{0}'>", rowClass);
            foreach (string s in dr.ItemArray.Skip(1))
            {
                string data = s;
                string css = "";
                string style = "";
                if (data.Contains("|"))
                {
                    string[] split = data.Split('|');
                    data = split[0];
                    css = split[1];

                    if(css.Contains("^"))
                    {
                        string[] split2 = css.Split('^');
                        css = split2[0];
                        style = split2[1];
                    }

                }

                sb.AppendFormat("<td class='{0}' style='{1}'>", css, style);
                sb.Append(data);
                sb.Append("</td>");
            }
            sb.Append("</tr>");
            return sb.ToString();
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            ListView lv = cntPlanPracy.List;
            foreach (ListViewItem item in lv.Items)  //pracownicy
            {
                string pracId = Tools.GetText(item, "hidPracId");
                if (!String.IsNullOrEmpty(pracId))
                    db.Execute(dsSendToAcc, pracId, db.strParam(cntSelectOkres.DateFrom), "2");
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            ListView lv = cntPlanPracy.List;
            foreach (ListViewItem item in lv.Items)  //pracownicy
            {
                string pracId = Tools.GetText(item, "hidPracId");
                if (!String.IsNullOrEmpty(pracId))
                    db.Execute(dsSendToAcc, pracId, db.strParam(cntSelectOkres.DateFrom), "3");
            }
        }

        protected void btnSendToAccConfirm_Click(object sender, EventArgs e)
        {
            string[] emp = cntPlanPracy.GetCheckedEmployees(false);

            if (emp.Length <= 0)
            {
                Tools.ShowMessage("Proszę wybrać pracowników!");
            }
            else
            {
                Tools.ShowConfirm("Czy na pewno chcesz wysłać do akceptacji?", btnSendToAcc);
            }
        }

        protected void btnSendToAcc_Click(object sender, EventArgs e)
        {
            string[] emp = cntPlanPracy.GetCheckedEmployees(false);

            if(emp.Length <= 0)
            {
                Tools.ShowMessage("Proszę wybrać pracowników!");
            }
            else if (Save())
            {
                db.Execute(dsSendToAcc, db.strParam(String.Join(",", emp)), db.strParam(cntSelectOkres.DateFrom), App.User.Id);
                //XAILING
                RCPMailing.EventRCP(RCPMailing.maRCP_PP_ACC, cntPlanPracy.IdKierownika, String.Join(",", emp), db.Select.Scalar(@"
select
  r.IdKierownika
from Przypisania r
where r.IdPracownika = 11 and GETDATE() between r.Od and ISNULL(r.Do, '20990909')
", cntPlanPracy.IdKierownika));
            }
            else
            {
                Tools.ShowMessage("Błąd!");
            }

            ddlVer.DataBind();
            cntPlanPracy.DataBind();
        }

        #region Search
        //-----
        private void Deselect()
        {
            /*
            SelectedRcpId = null;
            SelectedStrefaId = null;
            if (lvPracownicy.SelectedIndex != -1)
            {
                lvPracownicy.SelectedIndex = -1;
                TriggerSelectedChanged();
            }
             */
        }

        //-----
        private void PrepareSearch()
        {
            //btClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('');doClick('{1}');return false;", tbSearch.ClientID, btSearch.ClientID);
            btClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('').focus();return false;", tbSearch.ClientID, btSearch.ClientID);
            Tools.ExecOnStart2("searchtrigprac", String.Format("startSearch('{0}','{1}');",
                tbSearch.ClientID, btSearch.ClientID));
            //tbSearch.Focus();
        }

        public void FocusSearch()
        {
            tbSearch.Focus();
        }

        protected void tbSearch_TextChanged(object sender, EventArgs e)
        {
        }

        /*
        private string GetSearch(string field, string search)
        {
            bool s1 = search.StartsWith(" ");
            bool s2 = search.EndsWith(" ");
            int len = search.Length;

            if (s1 && s2 && len > 2) return String.Format("{0}='{1}'", field, search.Substring(1, len-2));
            else
                if (s1 && len > 1) 
                    return String.Format("{0} like '{1}%'", field, search.Substring(1, len-1));
                else if (s2 && len > 1) 
                    return String.Format("{0} like '%{1}'", field, search.Substring(0, len-1));
                else
                    return String.Format("{0} like '%{1}%'", field, search.Trim());
        }
        */

        private string SetFilterExpr(bool resetPager)
        {
            cntPlanPracy.Search = tbSearch.Text;
            return null;


            /*
            string filter;
            string f1 = tabFilter.SelectedValue.Trim();
            SqlDataSource1.FilterParameters.Clear();
            if (String.IsNullOrEmpty(tbSearch.Text))
            {
                filter = f1;
            }
            else
            {
                //Tools.ExecOnStart2("searchfocus", String.Format("$('#{0}').focus();", tbSearch.ClientID));
                string f2;
                string[] words = Tools.RemoveDblSpaces(tbSearch.Text.Trim()).Split(' ');   // nie trzeba sprawdzać czy words[i] != ''
                if (words.Length == 1)
                {
                    //string search = tbSearch.Text;
                    //bool s1 = search.StartsWith(" ");
                    //bool s2 = search.EndsWith(" ");
                    //int len = search.Length;
#if SIEMENS
                    f2 = "(Nazwisko like '{0}%' or Imie like '{0}%' or KadryId like '{0}%' or NrKarty like '{0}%' or KierNazwisko like '{0}%' or KierImie like '{0}%')";
#else
                    f2 = "(Nazwisko like '{0}%' or Imie like '{0}%' or KadryId like '{0}%' or RcpIdTxt like '{0}%' or KierNazwisko like '{0}%' or KierImie like '{0}%')";
#endif
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                }
                else if (words.Length == 2)
                {
#if SIEMENS
                    f2 = "(Nazwisko like '{0}%' and Imie like '{1}%' or Nazwisko like '{1}%' and Imie like '{0}%' or KadryId like '{0}%' or KadryId like '{1}%' or NrKarty like '{0}%' or NrKarty like '{1}%' or KierNazwisko like '{0}%' and KierImie like '{1}%' or KierNazwisko like '{1}%' and KierImie like '{0}%')";   // przypadek kiedy szukam po inicjałach wpisując to samo np s s
#else
                    f2 = "(Nazwisko like '{0}%' and Imie like '{1}%' or Nazwisko like '{1}%' and Imie like '{0}%' or KadryId like '{0}%' or KadryId like '{1}%' or RcpIdTxt like '{0}%' or RcpIdTxt like '{1}%' or KierNazwisko like '{0}%' and KierImie like '{1}%' or KierNazwisko like '{1}%' and KierImie like '{0}%')";   // przypadek kiedy szukam po inicjałach wpisując to samo np s s
#endif
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                    SqlDataSource1.FilterParameters.Add("par1", words[1]);
                }
                else
                {
                    string[] exp = new string[words.Length];
                    for (int i = 0; i < words.Length; i++)
                    {
#if SIEMENS
                        exp[i] = String.Format("(Nazwisko like '{{{0}}}%' or Imie like '{{{0}}}%' or KadryId like '{{{0}}}%' or KierNazwisko like '{{{0}}}%' or KierImie like '{{{0}}}%' or NrKarty like '{{{0}}}%')", i);
#else
                        exp[i] = String.Format("(Nazwisko like '{{{0}}}%' or Imie like '{{{0}}}%' or KadryId like '{{{0}}}%' or KierNazwisko like '{{{0}}}%' or KierImie like '{{{0}}}%' or RcpIdTxt like '{{{0}}}%')", i);
#endif
                        SqlDataSource1.FilterParameters.Add(String.Format("par{0}", i), words[i]);
                    }
                    f2 = String.Join(" and ", exp);
                }
                filter = f2 + (String.IsNullOrEmpty(f1) ? null : " and " + f1);
            }
            FilterExpression = filter;

            if (resetPager) Tools.ResetLetterPager(lvPracownicy);       //resetPager nie robić kiedy !IsPostback - w !IsPostback szukał pagera czym powodował bind lvPracownicy, ustawiał się LetterPager i zaraz był ustawiany do zerowania - się nie wyświetlał 
            return filter;
            */
 
        }

        private void DoSearch(bool init)  //init = !IsPostback, w SteFilter był ResetLetterPager który w !IsPostback szukał pagera czym powodował bind lvPracownicy, ustawiał się LetterPager i zaraz był ustawiany do zerowania - się nie wyświetlał 
        {
            SetFilterExpr(!init);
            if (!init)
            {
                //lvPracownicy.DataBind();
                Deselect();
                /*
                if (lvPracownicy.Items.Count == 1) Select(0);
                else if (lvPracownicy.SelectedIndex != -1) Select(-1);
                 */
            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            DoSearch(false);
        }

        protected void cnt_ChangeFilter(object sender, EventArgs e)
        {
            DoSearch(false);
        }
        #endregion

        protected void lnkEditSchemesModal_Click(object sender, EventArgs e)
        {
            //Tools.ShowBSDialog("EditSchemesModal");
            //EditSchemesModal.Show();
            cntModal.Show();
        }

        protected void btnShowSchemeInsert_Click(object sender, EventArgs e)
        {
            cntSchematy.List.InsertItemPosition = InsertItemPosition.LastItem;

            foreach (ListViewItem item in cntSchematy.List.Items)
            {
                Button EditButton = item.FindControl("EditButton") as Button;
                Button DeleteButton = item.FindControl("DeleteButton") as Button;

                if (EditButton != null)
                    EditButton.Visible = false;
                if (DeleteButton != null)
                    DeleteButton.Visible = false;            
            }


        }

        protected void ddlVer_SelectedIndexChanged(object sender, EventArgs e)
        {
            cntPlanPracy.Version = ddlVer.SelectedValue;
            cntPlanPracy.DataBind();
        }

        protected void ddlVer_DataBound(object sender, EventArgs e)
        {
            if (ddlVer.Items.Count <= 0)
                ddlVer.Visible = false;
        }

        protected void btnModal_Click(object sender, EventArgs e)
        {
            //cntModal.Show();
        }

        protected void btnShowPlan_Click(object sender, EventArgs e)
        {

            //PlanPracy1.Prepare(ddlKier.SelectedValue, cntSelectOkres.DateFrom.ToString(), cntSelectOkres.DateTo.ToString(), cntSelectOkres.OkresId, cntSelectOkres.Status, cntSelectOkres.IsArch);
            //PlanPracy1.Version = ddlVer.SelectedValue;
            //PlanPracy1.Visible = true;
            //PlanPracy1.DataBind();
            //cntModalPlan.Show();
        }

        protected void btnEditZmiany_Click(object sender, EventArgs e)
        {
            cntModalZmiany.Show();
        }

        protected void ddlCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            cntPlanPracy.Commodity = ddlCom.SelectedValue;
            cntPlanPracy.DataBind();
        }





        protected void btnAddOkresModal_Click(object sender, EventArgs e)
        {
            //DataRow dr = db.Select.Row(dsNewOkres);
            //string from = db.getValue(dr, "DateFrom");
            //string to = db.getValue(dr, "DateTo");

            cntModalOkres.Show();

            //deFrom.Date = from;
            //deTo.Date = to;

        }

        protected void btnAddOkres_Click(object sender, EventArgs e)
        {
            String NewOkresId = AddOkres();
            bool good = !String.IsNullOrEmpty(NewOkresId);
            good &= AddCzasy(NewOkresId);
            if (good)
            {
                cntModalOkres.Close();
                //lvOkresy.DataBind();
            }
            else
            {
                Tools.ShowMessage("Wystąpił błąd!");
            }

        }

        String AddOkres()
        {
            String DataOd, DataDo, Typ;

            Typ = ddlTypOkresu.SelectedValue;
            if (String.IsNullOrEmpty(Typ))
                return null;

            String OkresRaw = ddlOkresy.SelectedValue;
            if (String.IsNullOrEmpty(OkresRaw))
                return null;

            String[] OkresPar = OkresRaw.Split(';');
            if (OkresPar.Length < 2)
                return null;

            DataOd = OkresPar[0];
            DataDo = OkresPar[1];

            return db.Select.Scalar(dsInsert, db.nullStrParam(DataOd), db.nullStrParam(DataDo), db.nullStrParam(Typ));
        }

        bool AddCzasy(String OkresId)
        {
            foreach (RepeaterItem Item in rpNom.Items)
            {
                String Dni = Tools.GetText(Item, "tbNom");
                String Data = Tools.GetText(Item, "hidDate");

                if (!db.Execute(dsAddCzas, db.strParam(Data), Dni, OkresId))
                    return false;
            }
            return true;
        }

        protected void ddlTypOkresu_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlOkresy.DataBind();
            BindNom();
        }

        protected void ddlOkresy_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindNom();
        }

        class OkresDate
        {
            public String Friendly { get; set; }
            public String Date { get; set; }
        }

        void BindNom()
        {
            String RawDates = ddlOkresy.SelectedValue;
            if (String.IsNullOrEmpty(RawDates))
                return;

            String[] Dates = RawDates.Split(';');
            if (Dates.Length < 2)
                return;

            DateTime From = DateTime.Parse(Dates[0]);
            DateTime To = DateTime.Parse(Dates[1]);

            List<OkresDate> OutputDates = new List<OkresDate>();

            for (var month = From.Date; month.Date <= To.Date; month = month.AddMonths(1))
            {
                OutputDates.Add
                    (
                        new OkresDate
                        {
                            Friendly = Tools.DateFriendlyName(0, month),
                            Date = month.ToShortDateString()
                        }
                    );
            }

            rpNom.DataSource = OutputDates;
            rpNom.DataBind();
        }



    }
}