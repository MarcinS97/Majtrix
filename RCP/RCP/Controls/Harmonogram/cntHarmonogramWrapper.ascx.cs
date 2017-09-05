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
using System.Web.UI.HtmlControls;

namespace HRRcp.RCP.Controls.Harmonogram
{
    /* 20170220 Ogarnąłem tu syf trochę - Skoda */
    public partial class cntHarmonogramWrapper : System.Web.UI.UserControl
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
#if OKT
                msEntities.Visible = true;
                ddlStanowiska.Visible = false;
#endif

#if VICIM || VC || KDD
                msKlasyfikacje.Visible = false;
                ddlDzialy.Visible = true;
                ddlStanowiska.Visible = true;
                ddlDzialy.AutoPostBack = true;
                ddlStanowiska.AutoPostBack = true;
                ddlKier.AutoPostBack = true;
#endif

                string uid = App.User.Id;//"118"; //App.User.Id; //TODO
                hidKierId.Value = uid;          // zawsze w kontekscie zalogowanego usera
                hidUserId.Value = uid;          // zawsze w kontekscie zalogowanego usera
                //Adm = adm ? cntPlanPracy2.moAdmin : cntPlanPracy2.moKier;


                ddlKier.DataSourceID = adm ? dsKierAll.ID : dsKier.ID;  // filtr kierownik
                ddlKier.DataBind();
                if (ddlKier.Items.Count == 1)                           // tylko kieras
                {
                    ddlKier.Visible = false;
                    Tools.AddClass(paSearch, "left");
                    Prepare(uid);
                }

                int idx = -1;
                string sid = App.SesSelKierId;
                if (!String.IsNullOrEmpty(sid))
                {
                    idx = Tools.SelectItem2(ddlKier, sid);      // admin będący kierownikiem
                }
                if (idx == -1 && kier)
                {
                    idx = Tools.SelectItem2(ddlKier, uid);             // admin będący kierownikiem
                }
                Prepare(ddlKier.SelectedValue);
                
                
                //else if (adm && kier)
                //{
                //    Tools.SelectItem(ddlKier, uid);   // admin będący kierownikiem
                //    Prepare(uid);
                //}
                //else
                //{
                //    Prepare(ddlKier.SelectedValue);
                //}

#if OKT && !KDD
                ddlKier.Visible = false;
                ddlKier.SelectedValue = null;
#endif


#if DBW 
                /* 20161128 22:53 - kill me*/
                if (adm)
                {
                    ddlKier.SelectedValue = "0";
                    Prepare();
                }
#endif


                lnkEditSchemesModal.Visible = adm;
                btnEditZmiany.Visible = adm;

                SetEditMode(false);
                DoSearch(true);     // ustawienie filtra domyślnego
            }
            else
            {
                v = Visible;        // jak !IsPostBack i Visible tzn ze startuje od formatki z kontrolką
            }
        }

        public void Prepare()
        {
            Prepare(ddlKier.SelectedValue);
        }

        public void Prepare(string kierId)
        {
#if OKT
            kierId = null;
#endif

            cntSelectOkres.Prepare(DateTime.Today, true);
            if (cntHarmonogram.Prepare(cntSelectOkres.DateFrom, cntSelectOkres.DateTo, cntSelectOkres.OkresId, kierId))
                divTools.Visible = true;
            else
                divTools.Visible = false;
        }

        public void SetEditMode(bool edit)
        {
            bool ed = App.User.HasRight(AppUser.rPlanPracy);
            bool em = ed && edit;
            bool nem = ed && !edit;

            cntHarmonogram.Editable = edit;
            cntZmiany.Visible = edit;

            /* ikonki mode'a*/
            iModeEdit.Visible = edit;
            iModePreview.Visible = !edit;

            btSetScheme.Enabled = edit;


            btEditPP.Visible = nem;
            btSavePP.Visible = em;
            btCancelPP.Visible = em;


            if (Lic.HarmAcc)
            {
                btnCopyFromModal.Visible = ed;
                btnSendToAccConfirm.Visible = ed;
                btSetScheme.Visible = ed;
                btnSendToAccConfirm.Visible = nem;

                SetItemEnabled(btnCopyFromModal, em);
                SetItemEnabled(btnPrintHar, !edit);
                SetItemEnabled(btnPrintList, !edit);
                SetItemEnabled(btSetScheme, em);

#if OKT
                liPrintEmptyHar.Visible = true;
                SetItemEnabled(lbtnPrintEmptyHar, !edit);
#endif

            }

            cntSelectOkres.Enabled = !edit;
            ddlKier.Enabled = !edit;
            ddlCom.Enabled = !edit;

            btnFilter.Enabled = !edit;
            btnClearFilter.Enabled = !edit;

            msEntities.Enabled = !edit;
            ddlKlasyfikacja.Enabled = !edit;
            msKlasyfikacje.Enabled = !edit;
            ddlStanowiska.Enabled = !edit;
            ddlDzialy.Enabled = !edit;

            switch (Mode)
            {
                case EMode.Acc:
                    btnAcc.Visible = btnAccConfirm.Visible = btnReject.Visible = btnRejectConfirm.Visible = !edit; /* w keeeeeeeperze nie ma odrzucania? */
                    btnSendToAcc.Visible = btnSendToAccConfirm.Visible = false;
                    break;
                case EMode.Plan:
                    btnAcc.Visible = btnAccConfirm.Visible = btnReject.Visible = btnRejectConfirm.Visible = false;
                    btnSendToAcc.Visible = btnSendToAccConfirm.Visible = !edit;
                    break;
            }

        }

        protected void SetItemEnabled(LinkButton lnk, bool b)
        {
            lnk.Enabled = b;
            HtmlGenericControl li = lnk.Parent as HtmlGenericControl;
            li.Attributes["class"] = ((b) ? "" : "disabled");
        }

        protected void btCheckPP_Click(object sender, EventArgs e)
        {
            if (cntHarmonogram.Check(true))
                Tools.ShowMessage("Plan pracy poprawny.");
        }

        protected void btEditPP_Click(object sender, EventArgs e)
        {
            SetEditMode(true);
        }

        protected void btSavePP_Click(object sender, EventArgs e)
        {
            Save(false);
        }

        bool Save(bool keepEditMode)
        {
            bool good = true;
            if (!keepEditMode)
                SetEditMode(false);
            cntHarmonogram.Prepare();
            return good;
        }

        protected void btCancelPP_Click(object sender, EventArgs e)
        {
            SetEditMode(false);
            Prepare();
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
            //cntPlanPracy.OkresClosed = !v;
            Prepare();

            //cntPlanPracy.Commodity = ddlCom.SelectedValue;
            //cntPlanPracy.DataBind();
        }
        //-----------------------------
        public SelectOkres OkresRozl
        {
            get { return cntSelectOkres; }
        }

        protected void ddlKier_SelectedIndexChanged(object sender, EventArgs e)
        {
            //hidKierId.Value = ddlKier.SelectedValue;
            //Session["admKier"] = ddlKier.SelectedValue;

            string kid = ddlKier.SelectedValue;
            App.SesSelKierId = kid;
            Prepare(kid);
        }

        protected void btScheme_Click(object sender, EventArgs e)
        {
            String emp = String.Empty;
            String[] empl = cntHarmonogram.GetSelectedEmployeesArray(true);
            if (empl.Length > 0)
            {
                emp = String.Join(",", empl);
                cntSetSchemesModal.Show();
                hidSchemeEmp.Value = emp;//.Substring(1);

                deLeft.Date = Tools.bom(Tools.StrToDateTime(cntSelectOkres.DateFrom).Value);
                deRight.Date = Tools.eom(Tools.StrToDateTime(cntSelectOkres.DateTo).Value);
            }
            else
            {
                Tools.ShowError("Nie wybrano żadnego pracownika!");
            }

        }

        protected void btnSaveScheme_Click(object sender, EventArgs e)
        {
            string emp = hidSchemeEmp.Value;//, days = "";
            string dateLeft = deLeft.DateStr;
            string dateRight = deRight.DateStr;
            string scheme = ddlScheme.SelectedValue; //Schematy.SelectedValue;//ddlSchemes.SelectedValue;
            string mod = ddlPamietamTeDawneDni.SelectedValue;
            if (String.IsNullOrEmpty(emp))
                Tools.ShowError("Nie wybrano żadnego pracownika!");
            else if (String.IsNullOrEmpty(dateLeft) || String.IsNullOrEmpty(dateRight))
                Tools.ShowError("Proszę podać datę!");
            else if (String.IsNullOrEmpty(scheme))
                Tools.ShowError("Proszę wybrać schemat!");
            else if (Save(true))
            {
                db.Execute(dsApplyScheme, scheme, App.User.OriginalId, db.strParam(emp), db.strParam(dateLeft), db.strParam(dateRight), mod);

                Tools.CloseBSDialog();
                cntHarmonogram.Prepare();
            }
        }

        protected void btnCopyFromModal_Click(object sender, EventArgs e)
        {
            //string emp = "", days = "";
            //bool good = cntPlanPracy.GetEmpDays(ref emp, ref days);

            string emp = String.Join(",", cntHarmonogram.GetSelectedEmployeesArray(true));

            if (true)
            {
                //hidCopyDays.Value = days.Substring(1);

                //upCopyW.Update();
                cntCopyModal.Update();
                cntCopyModal.Show();

                deCopyLeft.Date = cntSelectOkres.DateFrom;
                deCopyRight.Date = cntSelectOkres.DateTo;

                hidCopyEmp.Value = emp;//.Substring(1);
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
            else if (Save(true))
            {
                db.Execute(dsCopyFrom, empId, App.User.OriginalId, db.strParam(emp), /*db.strParam(days)*/db.strParam(dateLeft), db.strParam(dateRight));
                Tools.CloseBSDialog();
                cntHarmonogram.Prepare();

            }
        }

        #region PRINT

        protected void btnPrintHar_Click(object sender, EventArgs e)
        {
            string[] emp = cntHarmonogram.GetSelectedEmployeesArray(true);
            bool good = true;
            if (emp.Length <= 0)
                good = false;
            else
                good = PrintKier2(String.Join(",", emp), false);

            if (!good)
                Tools.ShowMessage("Proszę wybrać pracowników!");
        }

        bool PrintKier2(string emp, bool empty)
        {
            string title = String.Format("<span class='table-title'>HARMONOGRAM PRACY {1} <span class='okres'>{0}</span></span>", cntSelectOkres.FriendlyName(1), db.Select.Scalar(@"
select 
	dbo.cat(dd.Nazwa, ',', 1)
from
(
	select distinct d.Nazwa
	from Dzialy d
	inner join PracownicyStanowiska ps on ps.IdDzialu = d.Id and ps.IdPracownika in (select items from dbo.SplitInt('{0}', ','))
) dd
", emp));
#if OKT
            string sign = "";
#else
            string sign = "<div class='signature'>Data i podpis kierownika</div>";
#endif
            if (String.IsNullOrEmpty(emp))
                return false;

#if OKT
            DataSet ds = db.Select.Set(dsPrintHarmonogramKeeeper
                , db.nullStrParam(ddlKier.SelectedValue)
                , db.strParam(cntSelectOkres.DateFrom)
                , db.strParam(cntSelectOkres.DateTo)
                , db.strParam(emp)
                , empty ? "1" : "0");
#else
            DataSet ds = db.Select.Set(dsPrint, db.strParam(ddlKier.SelectedValue), db.strParam(cntSelectOkres.DateFrom), db.strParam(cntSelectOkres.DateTo), db.strParam(emp));
#endif
            StringBuilder sb = new StringBuilder();

            DataTable dtHeader = ds.Tables[0];
            DataTable dtRows = ds.Tables[1];

            int counter = 0, breakIndex = 15;
            string pageClass = "";
#if OKT
            breakIndex = /*40;*/43; //20170305
            pageClass = "keeeper";
#endif
            foreach (DataRow dr in dtRows.Rows)
            {

                if ((counter % breakIndex) == 0) /* print header */
                {
                    if (counter > 0)
                    {
                        sb.AppendFormat("</table>{0}</div>", sign);
                    }

                    sb.AppendFormat("<div class='break pdf-harmonogram {1}'>{0}", title, pageClass);

#if OKT
                    if (counter == 0)
                    {
                        sb.Append(getLegend());
                    }
#endif
                    sb.AppendFormat("<table class='pdf-harmonogram-table'>");
                    foreach (DataRow drh in dtHeader.Rows)
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
#if OKT
#else
            PDF.Options = "-O landscape";
#endif
            if (PDF.Download(PDF.Prepare(sb.ToString(), Request), Server, Response, Request, "Harmonogram") != 0)
                return false;
            return true;
        }

        public String ListPageDEMP(DataTable dt, DataRow drPrac, string comp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='list-page break'>");
            sb.AppendFormat(@"
<div class='dempPrint'>
        <table>
            <tr>
                <th colspan='2'>{0}</th>
                <th colspan='2'>
                    <img src='../styles/User/demp_logo.png'> 
                    <span class='tit'>LISTA OBECNOŚCI PRACOWNIKA DEMP</span>
                </th>
            </tr>
            <tr>
                <td colspan='3'>{1}</td>
                <td>DBW POLSKA CIGACICE, GORZYKOWO 1A</td>
            </tr>

            <tr>
                <td></td>
                <td>DZIEN</td>
                <td>ZMIANA</td>
                <td>PODPIS PRACOWNIKA</td>
            </tr>", cntSelectOkres.FriendlyName(1).ToUpper().Replace("'", ""), db.getStr(drPrac["Pracownik"]));

            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (i > 0)
                {
                    string hol = db.getValue(dr, 0);
                    sb.AppendFormat("<tr class='{0}'>", String.IsNullOrEmpty(hol) ? "" : "hol");
                    sb.AppendFormat("<td>{0}</td>", i.ToString());

                    string dataSplit = db.getValue(dr, "Data");
                    string dataName = dataSplit.Split('|')[0];

                    sb.AppendFormat("<td>{0}</td>", dataName.ToUpper());

                    string zmianaSplit = db.getValue(dr, "Zmiana");

                    string zmianaName = zmianaSplit.Split('|')[0];

                    sb.AppendFormat("<td>{0}</td>", zmianaName);
                    sb.AppendFormat("<td></td>");
                    sb.Append("</tr>");
                }
                i++;
            }


            sb.AppendFormat(@"  
            <tr>
                <td class='noborder' colspan='2'></td>
                <td>Suma godzin</td>
                <td></td>
            </tr>


            <tr class='noborder'>
                <td colspan='4'></td>
            </tr>

            <tr>
                <td colspan='2' class='noborder'></td>
                <td>ZMIANA I</td>
                <td>6:00 - 14:00</td>
            </tr>
            <tr>
                <td colspan='2' class='noborder'></td>
                <td>ZMIANA II</td>
                <td>14:00 - 22:00</td>
            </tr>
            <tr>
                <td colspan='2' class='noborder'></td>
                <td>ZMIANA III</td>
                <td>22:00 - 6:00</td>
            </tr>

            <tr>
                <td colspan='4'>PREMIA MOTYWACYJNA: .........................................</td>
            </tr>
        </table>
    </div>");



            sb.Append("%%FOOTER%%</div>");
            return sb.ToString();
        }

        public String ListPageDEMPZlecenie(DataTable dt, DataRow drPrac, string comp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='list-page break'>");
            sb.AppendFormat(@"
<div class='dempZlecPrint'>
        		<table>
			<tr class='tr0'>
				<td class='td0' colspan='3'>
					Dla zleceniobiorcy --- Potwierdzenie oddania karty
				</td>
				<td class='td1' colspan='2' rowspan='6'>
					<span>Karta zleceń</span>
                    <br />
                    <img src='../styles/User/demp_zlec_logo.png'> 
				</td>
				<td class='td2' colspan='2' rowspan='2'>
					Imię i Nazwisko:
				</td>
				<td class='td3' colspan='2' rowspan='2'>
					{1}
				</td>
			</tr>
			<tr class='tr1'>
				<td class='td0' colspan='3'>

				</td>
			</tr>
			<tr class='tr2'>
				<td class='td0' colspan='3'>
					Osoba przyjmująca kartę:
				</td>
				<td class='td1' colspan='2' rowspan='2'>
					Miesiąc:
				</td>
				<td class='td2' colspan='2' rowspan='2'>
					{0}
				</td>
			</tr>
			<tr class='tr3'>
				<td class='td0' colspan='3'>
					Data złożenia karty:
				</td>
			</tr>
			<tr class='tr4'>
				<td class='td0' colspan='3'>
					Nazwisko i imię właściciela karty:
				</td>
				<td class='td1' colspan='2' rowspan='2'>
					Miejsce świadczenia zleceń
				</td>
				<td class='td2' colspan='2' rowspan='2'>

				</td>

			</tr>
			<tr class='tr5'>
				<td class='td0' colspan='3'>
					Miesiąc, którego dotyczy karta:
				</td>
			</tr>
			<tr class='tr6'>
				<td class='td0' colspan='3'>
					MARKET, KTÓREGO DOTYCZY KARTA:
				</td>
				<td class='td1' colspan='6' rowspan='2'>
					UWAGA!!!
					<br /> KARTĘ ZLECEŃ DEMP NALEŻY ZŁOŻYĆ U PRZEDSTAWICIELA DEMP DO OSTATNIEGO DNIA MIESIĄCA, KTÓREGO TA KARTA DOTYCZY
					!!!
				</td>
			</tr>
			<tr class='tr7'>
				<td class='td0'>
					✄
				</td>
				<td class='td1' colspan='2'>
					ILOŚĆ GODZIN NA KARCIE:
				</td>
			</tr>
			<tr class='tr8 tr-header'>
				<td>
					Data
				</td>
				<td>
					Rozpoczęcie zlecenia
				</td>
				<td>
					Przerwa
				</td>
				<td>
					Zakończenie zlecenia
				</td>
				<td>
					Ilość godzin
				</td>
				<td>
					HALA
				</td>
				<td>
					INNE
				</td>
				<td>
					INNE
				</td>
				<td>
					Podpis potwierdzający wykonanie zlecenia
				</td>
			</tr>
", cntSelectOkres.FriendlyName(1).ToUpper().Replace("'", ""), db.getStr(drPrac["Pracownik"]));

            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {



                if (i > 0)
                {
                    string hol = db.getValue(dr, 0);
                    sb.AppendFormat("<tr class='{0}'>", String.IsNullOrEmpty(hol) ? "" : "hol");
                    sb.AppendFormat("<td>{0}</td>", i.ToString());
                    sb.AppendFormat(@"<td></td>
                                <td></td>
				                <td></td>
				                <td></td>
				                <td></td>
				                <td></td>
				                <td></td>
				                <td></td>");
                    //        string dataSplit = db.getValue(dr, "Data");
                    //        string dataName = dataSplit.Split('|')[0];

                    //        sb.AppendFormat("<td>{0}</td>", dataName.ToUpper());

                    //        string zmianaSplit = db.getValue(dr, "Zmiana");

                    //        string zmianaName = zmianaSplit.Split('|')[0];

                    //        sb.AppendFormat("<td>{0}</td>", zmianaName);
                    //        sb.AppendFormat("<td></td>");
                    sb.Append("</tr>");
                }
                i++;
            }


            sb.AppendFormat(@"  
<tr class='tr-footer'>
				<td class='td0' colspan='3'>
					Premia motywacyjna.............
				</td>
				<td class='td1'>
					RAZEM
				</td>
				<td class='td2'>
				</td>
				<td class='td3'>
				</td>
				<td class='td4'>
				</td>
				<td class='td5'>
				</td>
				<td class='td6'>
				</td>
			</tr>


		</table>
    </div>");



            sb.Append("%%FOOTER%%</div>");
            return sb.ToString();
        }

        public String ListPageJail(DataTable dt, DataRow drPrac, string comp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='list-page break'>JAIL");


            sb.Append("%%FOOTER%%</div>");
            return sb.ToString();
        }

        public String ListPageSPS(DataTable dt, DataRow drPrac, string comp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='list-page break'>");

            sb.AppendFormat(@"  <div class='spsPrint'>
            <table>
                <tr class='tr0'>
                    <td class='td0' colspan='2'>
                        SPS 
                    </td>
                    <td class='td1' colspan='3'>
                        {0}
                    </td>
                    <td class='td2'>
                        SPS
                    </td>
                </tr>
                <tr class='tr1'>
                    <td class='td0' rowspan='3'>
                        <div>
                            <span class='rotate'>dzień miesiąca</span>
                        </div>
                    </td>
                    <td class='td1'>
                        <div>
                            <span class='rotate'>imię i nazwisko</span>
                        </div>
                    </td>
                    <td class='td2' colspan='4'>
                            {1}
                    </td>
                </tr>
                <tr class='tr2'>
                    <td class='td0' rowspan='2'>
                        <div>
                            <span class='rotate'>dzień tygodnia</span>
                        </div>
                    </td>
                    <td class='td1' colspan='2'>
                        Godziny
                    </td>
                    <td class='td2' rowspan='2'>
                        podpis pracownika
                    </td>
                    <td class='td3' rowspan='2'>
                        podpis osoby nadzorującej
                    </td>
                </tr>
                <tr class='tr3'>
                    <td class='td0'>
                        od
                    </td>
                    <td class='td1'>
                        do  
                    </td>
                </tr>", cntSelectOkres.FriendlyName(1).ToUpper().Replace("'", ""), db.getStr(drPrac["Pracownik"]));


            //foreach
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (i > 0)
                {
                    string hol = db.getValue(dr, 0);
                    sb.AppendFormat("<tr class='{0}'>", String.IsNullOrEmpty(hol) ? "" : "hol");
                    sb.AppendFormat("<td>{0}</td>", i.ToString());

                    string dataSplit = db.getValue(dr, "Data");
                    string dataName = dataSplit.Split('|')[0];

                    sb.AppendFormat("<td>{0}</td>", dataName.ToUpper());

                    string zmianaSplit = db.getValue(dr, "Zmiana");

                    string zmianaName = zmianaSplit.Split('|')[0];

                    sb.AppendFormat("<td></td>");
                    sb.AppendFormat("<td></td>");
                    sb.AppendFormat("<td></td>");
                    sb.AppendFormat("<td></td>");
                    sb.Append("</tr>");
                }
                i++;
            }



            sb.AppendFormat(@"<tr class='trLast'>
                    <td class='td0' colspan='5'>
                        PREMIA MOTYWACYJNA.......        
                    </td>
                    <td>
                    </td>
                </tr>

            </table>
        </div>");


            sb.Append("%%FOOTER%%</div>");
            return sb.ToString();
        }

        public String ListPageDBW(DataTable dt, DataRow drPrac, string comp)
        {
            StringBuilder sb = new StringBuilder();
            string title = "LISTA OBECNOŚCI - EWIDENCJA CZASU PRACY";
            string subTitle1 = "<span class='label'>Pracodawca:</span><span class='pracodawca'>{0}</span>";
            string subTitle2 =
#if DBW
                "<span class='label'>Lokalizacja:</span><span class='lokalizacja'>Górzykowo 1A, 66-131 Cigacice</span>";
#else
 ""
#endif
;

            sb.Append("<div class='list-page break'>");

            sb.AppendFormat("<span class='lista-ewid-title'>{0}</span>", title);
            if (!String.IsNullOrEmpty(comp))
                sb.AppendFormat("<span class='lista-ewid-subtitle1'>{0}</span>", String.Format(subTitle1, comp));
            sb.AppendFormat("<span class='lista-ewid-subtitle2'>{0}</span>", subTitle2);

            sb.AppendFormat("<div class='lista-ewid-header'><span class='prac'><span class='imienazw'>Imię i nazwisko pracownika:</span><span class='imienazwprac'>{1}</span></span><span class='miesiac'>miesiąc:</span><span class='miesiac2'>{0}</span></div>", cntSelectOkres.FriendlyName(1), db.getStr(drPrac["Pracownik"]));
            sb.Append("<table class='table-ewid'>");
            sb.Append("<tr class='tr-header'><td class='col1' colspan='3'></td><td class='col2' colspan='4'>UZUPEŁNIA PRACOWNIK</td><td class='col3' colspan='4'>UZUPEŁNIA PRZEŁOŻONY</td></tr>");
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append(PrintRow(dr));
            }

#if DBW
            sb.Append("<tr class='tr-footer'><td colspan='7'>PREMIA MOTYWACYJNA  TAK/NIE</td><td></td><td></td><td></td><td></td></tr>");
#else
            sb.Append("<tr class='tr-footer'><td colspan='7'>&nbsp;</td><td></td><td></td><td></td><td></td></tr>");
#endif
            sb.Append("</table>");

#if DBW
            sb.Append("<span class='funn'>*niepotrzebne skreślić</span>");
#endif
            sb.Append("<span class='facc'>Potwierdza przełożony</span>");
            sb.Append("<span class='facc2'>Niniejszym potwierdzam wykonanie pracy oraz powyższą ewidencję czasu pracy:</span>");

            sb.Append("<span class='fin'>imię i nazwisko</span>");
            sb.Append("<span class='fdata'>data potwierdzenia</span>");
            sb.Append("<span class='fpodpis'>podpis</span>");

            sb.Append("%%FOOTER%%</div>");
            return sb.ToString();
        }

        public String ListPageKeeeper(DataTable dt, DataRow drPrac, string comp)
        {
            StringBuilder sb = new StringBuilder();
            string title = "LISTA OBECNOŚCI - EWIDENCJA CZASU PRACY";
            string subTitle1 = "<span class='label'>Pracodawca:</span><span class='pracodawca'>{0}</span>";
            string subTitle2 = "";

            sb.Append("<div class='list-page break keeeper'>");

            sb.AppendFormat("<span class='lista-ewid-title'>{0}</span>", title);
            if (!String.IsNullOrEmpty(comp))
                sb.AppendFormat("<span class='lista-ewid-subtitle1'>{0}</span>", String.Format(subTitle1, comp));
            sb.AppendFormat("<span class='lista-ewid-subtitle2'>{0}</span>", subTitle2);

            sb.AppendFormat("<div class='lista-ewid-header'><span class='prac'><span class='imienazw'>Imię i nazwisko pracownika:</span><span class='imienazwprac'>{1}</span></span><span class='miesiac'>miesiąc:</span><span class='miesiac2'>{0}</span></div>", cntSelectOkres.FriendlyName(1), db.getStr(drPrac["Pracownik"]));
            sb.Append("<table class='table-ewid'>");
            //sb.Append("<tr class='tr-header'><td class='col1' colspan='3'></td><td class='col2' colspan='4'>UZUPEŁNIA PRACOWNIK</td><td class='col3' colspan='4'>UZUPEŁNIA PRZEŁOŻONY</td></tr>");
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append(PrintRow(dr));
            }

            sb.Append("<tr class='tr-footer'><td colspan='5'>&nbsp;</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");

            sb.Append("</table>");

            //sb.Append("<span class='facc'>Potwierdza przełożony</span>");
            //sb.Append("<span class='facc2'>Niniejszym potwierdzam wykonanie pracy oraz powyższą ewidencję czasu pracy:</span>");

            sb.Append("<span class='fin'>imię i nazwisko</span>");
            sb.Append("<span class='fdata'>data potwierdzenia</span>");
            sb.Append("<span class='fpodpis'>podpis</span>");

            sb.Append("%%FOOTER%%</div>");
            return sb.ToString();
        }

        bool PrintList(string[] emp)
        {

#if KDD
            //				<th>Trasa przejazdu / linia autobusowa</th>

            PDF PDF2 = new PDF();
            string kartaKierowcy = @"

    <style>
        .dempPrint table, th, td {
            border: 1px solid black;
            vertical-align: middle;
            height: 30px;
            text-align: center;
			border-collapse: collapse;
			padding: 5px;
        }

        .dempPrint th {
            font-size: 20px;
        }

    </style>
    <div class='dempPrint' style = 'border-right:10px;'>
        <table>
            <tr>
				<th>Kierowca</th>
				<th>Trasa przejazdu / linia</th>
				<th>Godz. wyjazdu</th>
				<th>Stan licznika</th>
				<th>Godz. przyjazdu</th>
				<th>Stan licznika</th>
				<th>Podpis</th>
            </tr>
            <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
            <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
            <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
            <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
            <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
            <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
            <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
            <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
            <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
            <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
            <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
            <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
            <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
            <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
			<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>      
            <tr>
                <td clospan=7>Uwagi:</td>
            </tr>
        </table>
    </div>

";
            PDF2.Download(PDF2.Prepare(kartaKierowcy, Request), Server, Response, Request, "Lista obecności");


            return true;
#endif
            StringBuilder sb = new StringBuilder();
            int counter = 0;

            DataTable dtPrac = db.Select.Table(dsPracOrder, String.Join(",", emp), db.strParam(cntSelectOkres.DateTo));

            List<String> trump = new List<String>();

            foreach (DataRow dr in dtPrac.Rows)
            {
                trump.Add(db.getValue(dr, 0));
            }

            emp = trump.ToArray();

            sb.Append("<div class='lista-ewid'>");
            foreach (string pracId in emp)
            {
#if OKT
                DataTable dt = db.Select.Table(dsPrint2Keeeper, db.strParam(cntSelectOkres.DateFrom), db.strParam(cntSelectOkres.DateTo), pracId);
#else
                DataTable dt = db.Select.Table(dsPrint2, db.strParam(cntSelectOkres.DateFrom), db.strParam(cntSelectOkres.DateTo), pracId);
#endif
                DataRow drPrac = db.Select.Row(dsPrint2Prac, pracId);
                String comp = db.Select.Scalar(dsPrint2Comp, pracId, db.strParam(cntSelectOkres.DateTo));
                if (dt != null)
                {
                    switch (comp)
                    {
                        case "DBW":
                        case "EURICA":
                        case "ARESZT":
                        default:
#if OKT
                            sb.Append(ListPageKeeeper(dt, drPrac, comp));
#else
                            sb.Append(ListPageDBW(dt, drPrac, comp));
#endif
                            break;
                        case "DEMP":
                            sb.Append(ListPageDEMP(dt, drPrac, comp));
                            break;
                        /*case "DEMP (umowa - zlecenie)":
                            sb.Append(ListPageDEMP2(dt, drPrac, comp));
                            break;*/
                        case "XARESZT":
                            sb.Append(ListPageJail(dt, drPrac, comp));
                            break;
                        case "SPS":
                            sb.Append(ListPageSPS(dt, drPrac, comp));
                            break;
                        case "DEMP-ZLECENIE":
                            sb.Append(ListPageDEMPZlecenie(dt, drPrac, comp));
                            break;
                    }

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
            string[] emp = cntHarmonogram.GetSelectedEmployeesArray(true);
            bool good = true;
            if (emp.Length <= 0)
                good = false;
            else
                good = PrintList(emp);

            if (!good)
                Tools.ShowMessage("Proszę wybrać pracowników!");
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

                    if (css.Contains("^"))
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

        protected string getLegend()
        {
            string ret = String.Format(@"
            <table class='keeeper legendTable'>
                <tr>
                    <td class='wn'>WN</td>
                    <td class='legendDesc'> - wolne za niedziele</td>
                    <td class='ws'>WS</td>
                    <td class='legendDesc'> - wolne za święto</td>
                    <td class='dw'>DW</td>
                    <td class='legendDesc'> - dodatkowy wolny dzień dla pracownika</td>
                </tr>
            </table>");

            return ret;
        }

        protected void lbtnPrintEmptyHar_Click(object sender, EventArgs e)
        {
            string[] emp = cntHarmonogram.GetSelectedEmployeesArray(true);
            bool good = true;
            if (emp.Length <= 0)
                good = false;
            else
                good = PrintKier2(String.Join(",", emp), true);

            if (!good)
                Tools.ShowMessage("Proszę wybrać pracowników!");
        }

        #endregion

        protected void btnSendToAccConfirm_Click(object sender, EventArgs e)
        {
            string[] emp = cntHarmonogram.GetSelectedEmployeesArray(true);

            if (emp.Length <= 0)
            {
                Tools.ShowMessage("Proszę wybrać pracowników!");
            }
            else if (!cntHarmonogram.Check(false))
            {
            }
            else
            {
                Tools.ShowConfirm("Czy na pewno chcesz wysłać do akceptacji?", btnSendToAcc);
            }
        }

        protected void btnSendToAcc_Click(object sender, EventArgs e)
        {
            string[] emp = cntHarmonogram.GetSelectedEmployeesArray(true);

            if (emp.Length <= 0)
            {
                Tools.ShowMessage("Proszę wybrać pracowników!");
            }
            else if (Save(false))
            {
                if (cntHarmonogram.Check(false))
                {
                    Log.Info(1337, String.Format("Wysłano do akceptacji pracowników - {0} w dacie {1} przez {2}", String.Join(",", emp), cntSelectOkres.DateFrom, App.User.Id), "");
                    db.Execute(dsSendToAcc, String.Join(",", emp), db.strParam(cntSelectOkres.DateFrom), App.User.Id);
                }
                else
                {

                }
                //XAILING
                //RCPMailing.EventRCP(RCPMailing.maRCP_PP_ACC, cntHarmonogram.IdKierownika, String.Join(",", emp), db.Select.Scalar(@"
                //select
                //  r.IdKierownika
                //from Przypisania r
                //where r.IdPracownika = {0} and GETDATE() between r.Od and ISNULL(r.Do, '20990909')
                //", cntHarmonogram.IdKierownika));
            }
            else
            {
                Tools.ShowMessage("Błąd!");
            }
            cntHarmonogram.Prepare();
        }

        protected void btnAccConfirm_Click(object sender, EventArgs e)
        {
            string[] emp = cntHarmonogram.GetSelectedEmployeesArray(true);
            string err = cntHarmonogram.Check(false, false);
            if (emp.Length <= 0)
            {
                Tools.ShowMessage("Proszę wybrać pracowników!");
            }
            else if (!string.IsNullOrEmpty(err))
            {
                if (App.User.HasRight(AppUser.rPlanPracyAccMimoBl))
                {
                    Tools.ShowConfirm("Uwaga znaleziono błędy w harmonogramie. <br /><br />" + err + " <br /><br /><span class=\"error\"> Masz uprawnienie do akceptacji harmonogramu z błędami. Czy na pewno chcesz zaakceptować?</span> ", "Uwaga!",btnAcc);

                    Log.Info(1337, "Mimo błędów podjęto próbę akceptacji", err);
                }
                else
                {
                    Tools.ShowMessage(err);
                }
            }
            else
            {
                Tools.ShowConfirm("Czy na pewno chcesz zaakceptować?", btnAcc);
            }
        }

        protected void btnAcc_Click(object sender, EventArgs e)
        {
            string[] emp = cntHarmonogram.GetSelectedEmployeesArray(true);
            Log.Info(1337, "Akceptacja", "");
            string err = cntHarmonogram.Check(false, false);


            if (emp.Length <= 0)
            {
                Tools.ShowMessage("Proszę wybrać pracowników!");
            }
            else if (!string.IsNullOrEmpty(err) && !App.User.HasRight(AppUser.rPlanPracyAccMimoBl))
            {
                Tools.ShowMessage(err);
            }
            else if (Save(false))
            {
                string empj = String.Join(",", emp);

                Log.Info(1337, String.Format("Zaakceptowano pracowników - {0} w okresie {1} - {2}", empj, cntSelectOkres.DateFrom, cntSelectOkres.DateTo), "");
                db.Execute(dsAccept, empj, db.strParam(cntSelectOkres.DateFrom), db.strParam(cntSelectOkres.DateTo));
                //db.Execute(dsSendToAcc, db.strParam(String.Join(",", emp)), db.strParam(cntSelectOkres.DateFrom), App.User.Id);
                // TODO QUERY DO AKCEPTACJI
            }
            else
            {
                Tools.ShowMessage("Błąd!");
            }
            cntHarmonogram.Prepare();
        }

        protected void btnReject_Click1(object sender, EventArgs e)
        {
            string emp = hidRejectEmp.Value;


            if (String.IsNullOrEmpty(emp))
            {
                Tools.ShowMessage("Proszę wybrać pracowników!");
            }
            else if (Save(false))
            {
                Log.Info(1337, String.Format("Odrzucono pracowników - {0} {1} - {2}, powód: {3}", emp, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, tbReject.Text), "");
                db.Execute(dsReject, emp, db.strParam(cntSelectOkres.DateFrom), db.strParam(cntSelectOkres.DateTo), db.nullStrParam(tbReject.Text));
                cntRejectModal.Close();
            }
            else
            {
                Tools.ShowMessage("Błąd!");
            }
            cntHarmonogram.Prepare();
        }

        protected void btnRejectConfirm_Click(object sender, EventArgs e)
        {
            string[] emp = cntHarmonogram.GetSelectedEmployeesArray(true);

            if (emp.Length <= 0)
            {
                Tools.ShowMessage("Proszę wybrać pracowników!");
            }
            else
            {
                cntRejectModal.Show();
                hidRejectEmp.Value = String.Join(",", emp);
            }
        }

        #region Search


        private string SetFilterExpr(bool resetPager)
        {
            cntHarmonogram.Search = tbSearch.Text;
            cntHarmonogram.Prepare();
            //cntPlanPracy.Search = tbSearch.Text;
            return null;

        }

        private void DoSearch(bool init)  //init = !IsPostback, w SteFilter był ResetLetterPager który w !IsPostback szukał pagera czym powodował bind lvPracownicy, ustawiał się LetterPager i zaraz był ustawiany do zerowania - się nie wyświetlał 
        {
            SetFilterExpr(!init);
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

        protected void btnEditZmiany_Click(object sender, EventArgs e)
        {
            cntModalZmiany.Show();
        }

        protected void ddlCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            cntHarmonogram.IdCommodity = ddlCom.SelectedValue;
            Prepare();
        }

        protected void btnAddOkresModal_Click(object sender, EventArgs e)
        {
            cntModalOkres.Show();
        }

        protected void ddlDzialy_SelectedIndexChanged(object sender, EventArgs e)
        {
            cntHarmonogram.IdDzialu = ddlDzialy.SelectedValue;
        }
    
        protected void ddlStanowiska_SelectedIndexChanged(object sender, EventArgs e)
        {
            cntHarmonogram.IdStanowiska = ddlStanowiska.SelectedValue;
        }

        protected void ddlKlasyfikacja_SelectedIndexChanged(object sender, EventArgs e)
        {
            cntHarmonogram.IdKlasyfikacji = ddlKlasyfikacja.SelectedValue;
        }

        public enum EMode { Plan, Acc };
        private EMode _mode = EMode.Plan;
        public EMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        protected void cntSchematy_Changed(object sender, EventArgs e)
        {
            ddlScheme.DataBind();
        }

        protected void btnCheckPreview_Click(object sender, EventArgs e)
        {

        }

        protected void btnSearchX_Click(object sender, EventArgs e)
        {
            DoSearch(false);
        }

        protected void btClear_Click(object sender, EventArgs e)
        {
            tbSearch.Text = "";
            DoSearch(false);
        }

        protected void dsPrac2_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@ids"].Value = cntHarmonogram.GetNotSelectedEmployeesString();
        }

        protected void cntHarmonogram_ParametryPracownikaShow(object sender, EventArgs e)
        {

            String EmployeeId = sender as String;
            if (!String.IsNullOrEmpty(EmployeeId))
            {
                cntParametryPracownika.Show(EmployeeId, cntSelectOkres.DateFrom, cntSelectOkres.DateTo);
            }
            else
            {
                Tools.ShowMessage("Błąd");
            }
        }

        protected void cntParametryPracownika_Saved(object sender, EventArgs e)
        {
            Prepare();
        }

        protected void cntModalZmiany_EditClick(object sender, EventArgs e)
        {
            string id = sender as String;
            if (String.IsNullOrEmpty(id))
            {
                cntZmianyEditModal.Show();
            }
            else
            {
                cntZmianyEditModal.Show(Int32.Parse(id));
            }
        }

        protected void cntZmianyEditModal_Saved(object sender, EventArgs e)
        {

            cntZmianyList.DataBind();
        }

        protected void msEntities_SelectedIndexChanged(object sender, EventArgs e)
        {
            cntHarmonogram.Entities = msEntities.SelectedItems;
        }

        protected void msKlasyfikacje_SelectedIndexChanged(object sender, EventArgs e)
        {
            cntHarmonogram.Klasyfikacje = msKlasyfikacje.SelectedItems;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Prepare();
        }

        protected void btnClearFilter_Click(object sender, EventArgs e)
        {
            msEntities.SelectedValue = null;
            msKlasyfikacje.SelectedValue = null;
            cntHarmonogram.Entities = null;
            cntHarmonogram.Klasyfikacje = null;
            Prepare();
        }

    }
}