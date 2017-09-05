using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using HRRcp.App_Code;
using HRRcp.Controls;

namespace HRRcp.RCP.Controls
{
    public partial class cntPlanPracyAccept : System.Web.UI.UserControl
    {
        bool v = false;
        bool adm, kier, ed;

        protected void Page_Load(object sender, EventArgs e)
        {
            adm = App.User.IsAdmin;
            kier = App.User.IsKierownik;
            ed = App.User.HasRight(AppUser.rPlanPracy);


            //paAccept.DefaultButton="tmpBtModalPopup" pomaga na Enter który klikał w cntSelectOkres mimo ze popup był na wierzchu
            if (IsPostBack)
                if (AccPanelVisible)
                {
                    /*paAccept_ModalPopupExtender.Show();*/
                    cntModal.Show(false);
                }
            //----- data akceptacji -----
            if (!IsPostBack)
            {
                string uid = App.User.Id;
                hidKierId.Value = uid;          // zawsze w kontekscie zalogowanego usera
                hidUserId.Value = uid;          // zawsze w kontekscie zalogowanego usera
                Adm = adm ? cntPlanPracy.moAdmin : cntPlanPracy.moKier;

                
                
                
                deDataAccept.Date = DateTime.Today.AddDays(-1);
                //Tools.MakeConfirmButton(btAccept, "Potwierdź akceptację czasu pracy do podanej daty.");
#if MODAL
                Tools.MakeConfirmButton2(btAccept, String.Format(
                    "Potwierdź akceptację czasu pracy do dnia: ' + $('#{0}').val() + '.",
                    deDataAccept.EditBox.ClientID
                    ));
#else
                Tools.MakeConfirmButton2(btAccept, String.Format(
                    "'Potwierdź akceptację czasu pracy do dnia: ' + $('#{0}').val() + '.'",
                    deDataAccept.EditBox.ClientID
                    ));
#endif
                //----- sprawdzanie -----
                //AppUser user = AppUser.CreateOrGetSession();
                //btCheck.Visible = user.IsAdmin;
                btCheck.Visible = false;
                /*
                PostBackTrigger trigger = new PostBackTrigger();
                trigger.ControlID = btCheck.UniqueID;
                 UpdatePanel1.Triggers.Add(trigger);
                ScriptManager.GetCurrent(Page).RegisterPostBackControl(button);
                 */

                /* juz jest na stałe
                bool v2 = App.User.Id == "13" ||        // Chorążak Robert
                          App.User.Id == "14" ||        // Olchowik Marcin
                          App.User.Id == "16" ||        // Seraficki Marcin
                          App.User.Id == "408" ||       // Kruszka Tomek
                          App.User.Id == "42" ||        // Michał Kazubski
                          //App.User.Id == "1" ||         // ja
                          //App.User.Id == "941" ||       // sabina
                          App.User.HasRight(AppUser.rTester) ||
                          App.User.IsAdmin
                              ;


                cntAccept.Visible = !v2;
                cntAccept2.Visible = v2;
                 */
            
            
                //---- RCP-------------------
                ddlKier.DataSourceID = adm ? dsKierAll.ID : dsKier.ID;  // filtr kierownik
                ddlKier.DataBind();
                if (ddlKier.Items.Count == 1)                           // tylko kieras
                {
                    ddlKier.Visible = false;
                    Tools.AddClass(paSearch, "left");
                    //Prepare(uid);
                    Prepare(uid, DateTime.Today, true);
                }
                else
                { 
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
                    Prepare(ddlKier.SelectedValue, DateTime.Today, true);
                }

                //else if (adm && kier)
                //{
                //    Tools.SelectItem(ddlKier, uid);   // admin będący kierownikiem
                //    //Prepare(uid);
                //    Prepare(uid, DateTime.Today, true);
                //}
                //else
                //{
                //    if (ddlKier.SelectedIndex != 0 || !adm) Prepare(ddlKier.SelectedValue);
                //}
                //----- -----
            }
            else
            {
                v = Visible;        // jak !IsPostBack i Visible tzn ze startuje od formatki z kontrolką
            }
        }
        //---------------------------
        public void Prepare(string kierId, DateTime nadzien, bool getFromSession)
        {
            cntSelectOkres.Prepare(nadzien, getFromSession);
            showAccDate();
            cntPlanPracy.Prepare(kierId, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, cntSelectOkres.OkresId, cntSelectOkres.Status, cntSelectOkres.IsArch);
        }

        public void Prepare(string kierId)
        {
            cntPlanPracy.Prepare(kierId);
        }

        public void Prepare()
        {
            cntSelectOkres.Prepare(DateTime.Today, true);
            cntPlanPracy.Prepare(cntSelectOkres.DateFrom, cntSelectOkres.DateTo);
            InitScripts();
        }
        //----- ---------------------
        public void InitScripts()
        {
            const string vs = "ppAccScripts";
            object o = ViewState[vs];
            if (o == null)
            {
                ViewState[vs] = "1";
                Tools.ExecOnStart2(vs, 
                    //"setInterval(blinkPPAcc, 500);" +
                    "startBlinkPPAcc();" +
                    "Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(getRcpScrollPos);"
                    );
            }
        }  

        //---------------------------
        /*
        private int DateAccOk()     // -2 błędny format, -1 wcześniejsza (lub równa) niż ostatni zaakceptowany, 0(ok) - pomiedzy ostatni zaakc. a dziś-1, 1 - >= dziś
        {                           // w sumie powinno się to sprawdzać wg bieżącego okresu rozliczeniowego jeszcze
            DateTime minDataDo;
            DateTime dataDo;
            DateTime maxDataDo = DateTime.Today.AddDays(-1);
            if (DateTime.TryParse(deDataAccept.DateStr, out dataDo))
            {
                AppUser user = AppUser.CreateOrGetSession();
                string lastAccData = user.DataAccDo;
                if (DateTime.TryParse(lastAccData, out minDataDo))
                {
                    minDataDo = minDataDo.AddDays(1);
                    if (dataDo <= minDataDo) return -1;
                    else if (dataDo > maxDataDo) return 1;
                    else return 0;
                }
                else 
                    if (dataDo > maxDataDo) return 1;
                    else return 0;          // zakładam ze last data nie istnieje ...
            }
            else return -2;
        }
         */
        //------RCP---------------------

        protected void ddlKier_SelectedIndexChanged(object sender, EventArgs e)
        {
            string kid = ddlKier.SelectedValue;
            App.SesSelKierId = kid;
            Prepare(kid);
        }

        protected void ddlStanowiska_SelectedIndexChanged(object sender, EventArgs e)
        {
            cntPlanPracy.IdStanowiska = ddlStanowiska.SelectedValue; 
        }

        protected void ddlDzialy_SelectedIndexChanged(object sender, EventArgs e)
        {
            cntPlanPracy.IdDzialu = ddlDzialy.SelectedValue;
        }
 

        //---------------------------
        protected void OnSelectDay(object sender, EventArgs e)
        {
            cntPlanPracy pp = (cntPlanPracy)sender;
            if (pp != null && !String.IsNullOrEmpty(pp.ClickPracId))
            {
                //if (cntAccept.Visible) 
                //    cntAccept.Prepare(pp.ClickPracId, Base.DateToStr(pp.ClickDate), pp.DateFrom, pp.DateTo);
                if (cntAccept2.Visible)
                    cntAccept2.Prepare(pp.ClickPracId, Base.DateToStr(pp.ClickDate), pp.DateFrom, pp.DateTo, pp.IdKierownika); 
                AccPanelVisible = true;
                //paAccept_ModalPopupExtender.Show();
            }
        }
        
        protected void cntAccept_CancelChanges(object sender, EventArgs e)
        {
            AccPanelVisible = false;
        }

        protected void cntAccept_AcceptChanges(object sender, EventArgs e)
        {
            AccPanelVisible = false;
            //DataBind();  // 20120622 wywala sie viewstate dla ludzi bez rcpid; to długo trwa, pomyslec nad optymalizacja !!!
            cntPlanPracy.DataBind();
            
            
            //----- ostatni dzień -----
            if (cntAccept2.doAccept)
            {
                //Ustawienia settings = Ustawienia.CreateOrGetSession();
                //KierParams kp = new KierParams(cntPlanPracy.IdKierownika, settings);
                //if (kp.DataAccDo != null && kp.DataAccDo < dt1.AddDays(-1))
                string kid = cntPlanPracy.IdKierownika;
                string dod = cntSelectOkres.DateFrom;
                string ddo = cntSelectOkres.DateTo;
                DataRow dr = db.getDataRow(String.Format(@"
declare @kid int
declare @od datetime
declare @do datetime
set @od = '{0}'
set @do = '{1}'
set @kid = {2}

select top 1 * 
from Przypisania R
outer apply (select * from dbo.GetDates2(dbo.MaxDate2(R.Od, @od), dbo.MinDate2(ISNULL(R.Do, '20990909'), @do))) D
left join PlanPracy PP on PP.IdPracownika = R.IdPracownika and PP.Data = D.Data and PP.Akceptacja = 1
left join PracownicyParametry PR on PR.IdPracownika = R.IdPracownika and D.Data between PR.Od and ISNULL(PR.Do, '20990909')
where R.IdKierownika = @kid and R.Status = 1 and R.Od <= @do and @od <= ISNULL(R.Do, '20990909')
and PR.RcpAlgorytm != 0  -- bez liczenia czasu pracy
and PP.Id is null
                ", dod, ddo, kid));
                if (dr == null)
                {
                    DateTime dt = DateTime.Parse(cntSelectOkres.DateTo);
                    Ustawienia settings = Ustawienia.CreateOrGetSession();
                    KierParams kp = new KierParams(cntPlanPracy.IdKierownika, settings);
                    int err = Okres.AkceptujOkres(kid, App.User.OriginalId, kp,
                                            dod, ddo,
                                            dt, true);
                    cntPlanPracy.DataBind();
                    cntPlanPracy.LineHeader.DataBind();
                    if (err == 0)
                        Tools.ShowMessage(String.Format("Czas pracy od {0} do {1} został zaakceptowany.", dod, Base.DateToStr(dt)));
                    else
                        Tools.ShowMessage(String.Format("Uwaga!\\nNie można zaakceptować wszystkich dni pracy w okresie:\\nod {0} do {1}.\\n\\nProszę wyjaśnić zaznaczone dni i ponownie dokonać akceptacji.", dod, Base.DateToStr(dt)));
                    Log.Info(Log.t2APP, "AcceptLastDay", String.Format("Kier: {0} Okres: {1} - {2} Error: {3}", kid, dod, ddo, err));
                }
            }
        }

        public bool CheckAll(out string msg)
        {
            msg = "";
            return true;
        }

        protected void btAcceptPP_Click(object sender, EventArgs e)
        {
            DateTime dt; 
            if (!DateTime.TryParse(deDataAccept.DateStr, out dt))
                Tools.ShowMessage("Niepoprawny format daty.");
            else
            {
                string msg;
                DateTime dt1 = DateTime.Parse(cntSelectOkres.DateFrom); // muszą być poprawne
                DateTime dt2 = DateTime.Parse(cntSelectOkres.DateTo);

                Okres ok = new Okres(dt1);
                if (ok.Status == Okres.stClosed)
                    Tools.ShowMessage("Okres rozliczeniowy jest zamknięty."); // na wszelki wypadek, nie powinno się zdarzyć bo chronologia...
                else if (dt < dt1 || dt2 < dt) 
                    Tools.ShowMessage("Data spoza zakresu wyświetlanego okresu rozliczeniowego.");
                else if (dt >= DateTime.Today)
                    Tools.ShowMessage("Data musi byc wcześniejsza od dzisiejszej.");
                else 
                {
                    Ustawienia settings = Ustawienia.CreateOrGetSession();
                    KierParams kp = new KierParams(cntPlanPracy.IdKierownika, settings);
                    if (kp.DataAccDo != null && kp.DataAccDo < dt1.AddDays(-1))
                        Tools.ShowMessage("Przed akceptacją bieżącego okresu rozliczeniowego należy zaakceptować okres poprzedni.");
                    else if (dt == dt2 && !CheckAll(out msg))   //furtka do weryfikacji na koniec okresu, póki co AkceptujOkres zwraca tylko sytuacje kiedy jest czas, a nie ma zmiany - nie można naliczyć nadgodzin i czasu pracy
                        Tools.ShowMessage(msg);
                    else
                    {
                        /* można zatwierdzic ponownie na dowolną datę w okresie
                            if (dt != null && dt <= (DateTime)kp.DataAccDo && dt < dt2)
                                Tools.ShowMessage("Czas pracy do wskazanej daty jest już zatwierdzony. Ponowne zatwierdzenie możliwe jest tylko na ostatni dzień okresu rozliczeniowego.");
                            else  */

                        if(Check(true))
                        {
                            //----- zatwierdzam !!! od początku okresu do dt bo mogli się pojawić inni pracownicy z nie zatwierdzonym planem przez poprzedniego kierownika
                            /*AppUser user = AppUser.CreateOrGetSession();
                            int err = Okres.AkceptujOkres(kp.KierId, user.OriginalId, kp,
                                                    cntPlanPracy.DateFrom, cntPlanPracy.DateTo,
                                                    dt, true);
                            cntPlanPracy.DataBind();
                            cntPlanPracy.LineHeader.DataBind();
                            HiddenField s;
                            
                            if (err == 0)
                                Tools.ShowMessage(String.Format("Czas pracy od {0} do {1} został zaakceptowany.", Base.DateToStr(dt1), Base.DateToStr(dt)));
                            else
                                Tools.ShowMessage(String.Format("Uwaga!\\nNie można zaakceptować wszystkich dni pracy w okresie:\\nod {0} do {1}.\\n\\nProszę wyjaśnić zaznaczone dni i ponownie dokonać akceptacji.", Base.DateToStr(dt1), Base.DateToStr(dt)));*/
                        }
                    }
                }
            }
        }
        /*void f()
        {
            //----- zatwierdzam !!! od początku okresu do dt bo mogli się pojawić inni pracownicy z nie zatwierdzonym planem przez poprzedniego kierownika
            AppUser user = AppUser.CreateOrGetSession();
            int err = Okres.AkceptujOkres(kp.KierId, user.OriginalId, kp,
                                    cntPlanPracy.DateFrom, cntPlanPracy.DateTo,
                                    dt, true);
            cntPlanPracy.DataBind();
            cntPlanPracy.LineHeader.DataBind();
            if (err == 0)
                Tools.ShowMessage(String.Format("Czas pracy od {0} do {1} został zaakceptowany.", Base.DateToStr(dt1), Base.DateToStr(dt)));
            else
                Tools.ShowMessage(String.Format("Uwaga!\\nNie można zaakceptować wszystkich dni pracy w okresie:\\nod {0} do {1}.\\n\\nProszę wyjaśnić zaznaczone dni i ponownie dokonać akceptacji.", Base.DateToStr(dt1), Base.DateToStr(dt)));
        }*/
        protected void btCheckPP_Click(object sender, EventArgs e)  // to samo co w test tylko dla 1 kier
        {
            Okres ok = new Okres(Base.StrToDateTime(cntSelectOkres.DateTo));
            bool arch = ok.IsArch();
            string kierId = cntPlanPracy.IdKierownika;

            StringWriter sw = Report.StartExportTXT("rcp analiza.txt");
            DataRow dr = Worker.GetData(kierId, arch);
            sw.WriteLine(String.Format("{0} {1}", Base.getValue(dr, "KadryId"), Base.getValue(dr, "NazwiskoImie")));
            /*
            KierParams kp = new KierParams(kierId);
            Okres.Akceptuj(kierId, null, null, kp, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, null, null,
                           true, sw);
            */ 
            Report.EndExportTXT(sw);
        }

        private bool showAccDate()
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
            lbAccept.Visible = v;
            deDataAccept.Visible = v;
            btAccept.Visible = v;
            lbOkresStatus.Visible = !v;
            if (v)
            {
                DateTime dt1 = DateTime.Parse(cntSelectOkres.DateFrom);
                DateTime accTo = DateTime.Today.AddDays(-1);
                deDataAccept.Date = accTo < dt ? accTo : dt;
            }
            cntPlanPracy.OkresClosed = !v;
            return v;
        }

        protected void cntSelectOkres_Changed(object sender, EventArgs e)
        {
            showAccDate();
        }
        //---------------------------
        public SelectOkres OkresRozl
        {
            get { return cntSelectOkres; }
        }

        public bool AccPanelVisible
        {
            get { return (string)ViewState[ID + "_accVisible"] == "1"; }
            set 
            { 
                ViewState[ID + "_accVisible"] = value ? "1" : "0";
                if (value)
                {
                    //cntSelectOkres.Enabled = false;
                    /*paAccept_ModalPopupExtender.Show();*/
                    cntModal.Show(false);
                    //paAccept_ModalPopupExtender.Focus();
                    Tools.SetBodyScrollBar(false);
                }
                else
                {
                    //cntSelectOkres.Enabled = true;
                    /*paAccept_ModalPopupExtender.Hide();*/
                    cntModal.Close();
                    Tools.SetBodyScrollBar(true);
                }
                //paAccept.DefaultButton="tmpBtModalPopup" pomaga na Enter który klikał w cntSelectOkres mimo ze popup był na wierzchu
                /*
                if (value)
                    cntModal.Show(false);
                else
                    cntModal.Close();
                */
            }   
        }

        public int Adm
        {
            set 
            { 
                cntPlanPracy.Adm = value;
                cntAccept2.Adm = value;
            }
            get { return cntPlanPracy.Adm; }
        }

        public void cntAccept2_OnNadgodzinyWnioskiModalShow(object sender, EventArgs e)
        {
            AcceptControl4.NadgodzinyWnioskiParams par = sender as AcceptControl4.NadgodzinyWnioskiParams;
            cntNadgodzinyWnioskiModal.Show(par.PracId, par.Data, par.Wt, par.N50,par.N100, par.NNoc, par.Type, null, null,null);
        }


        //--------------------------------
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

        /**/
        public bool Check(bool includeNotRequired)
        {
            DataTable dt = db.Select.Table(dsConditions, includeNotRequired ? "0" : "1");
            List<string> errors = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string conditionSQL = db.getValue(dr, "Sql");

                DataRow conditionData = db.Select.Row(conditionSQL, cntPlanPracy.IdKierownika, db.strParam(cntSelectOkres.DateFrom), db.strParam(/*cntSelectOkres.DateTo*/deDataAccept.EditBox.Text));
                int cCode = db.getInt(conditionData, "ErrorLevel", 0);

                if (cCode > 0)
                {
                    string cMsg = db.getValue(conditionData, "ErrorMsg", "");
                    cMsg = cMsg.Replace("\r\n", "<br />");

                    if( db.getBool(dr, "Wymagany", false) )
                    {
                        Tools.ShowMessage(cMsg);
                        return false;
                    }
                    else
                    {
                        errors.Add(cMsg);
                    }
                }
            }

            if(errors.Count == 0)
            {
                AkceptujOkres();
            }
            else
            {
                string msg = "";
                for (int i = 0; i < errors.Count; i++)
                {
                    msg += errors[i];
                    if(i < errors.Count-1)
                        msg += "\n\n\n";
                }
                Tools.ShowConfirm(msg, btAcceptConfirm);
            }

            return true;
        }

        void AkceptujOkres()
        {
            DateTime dt;
            if (!DateTime.TryParse(deDataAccept.DateStr, out dt))
                return;
            DateTime dt1 = DateTime.Parse(cntSelectOkres.DateFrom);
            DateTime dt2 = DateTime.Parse(cntSelectOkres.DateTo);
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            KierParams kp = new KierParams(cntPlanPracy.IdKierownika, settings);


            AppUser user = AppUser.CreateOrGetSession();
            int err = Okres.AkceptujOkres(kp.KierId, user.OriginalId, kp,
                                    cntPlanPracy.DateFrom, cntPlanPracy.DateTo,
                                    dt, true);
            cntPlanPracy.DataBind();
            cntPlanPracy.LineHeader.DataBind();
            HiddenField s;

            if (err == 0)
                Tools.ShowMessage(String.Format("Czas pracy od {0} do {1} został zaakceptowany.", Base.DateToStr(dt1), Base.DateToStr(dt)));
            else
                Tools.ShowMessage(String.Format("Uwaga!\\nNie można zaakceptować wszystkich dni pracy w okresie:\\nod {0} do {1}.\\n\\nProszę wyjaśnić zaznaczone dni i ponownie dokonać akceptacji.", Base.DateToStr(dt1), Base.DateToStr(dt)));
        }

        protected void btAcceptConfirm_Click(object sender, EventArgs e)
        {
            AkceptujOkres();
        }

        /**/

     }
}