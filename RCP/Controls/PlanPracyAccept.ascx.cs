using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class PlanPracyAccept : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //paAccept.DefaultButton="tmpBtModalPopup" pomaga na Enter który klikał w cntSelectOkres mimo ze popup był na wierzchu
            if (IsPostBack)
                if (AccPanelVisible)
                {
                    paAccept_ModalPopupExtender.Show();
                }
            //----- data akceptacji -----
            if (!IsPostBack)
            {
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
        //---------------------------
        protected void OnSelectDay(object sender, EventArgs e)
        {
            PlanPracy pp = (PlanPracy)sender;
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
                    Tools.ShowMessage("Okres rozliczeniowy jest zamknięty.");  // na wszelki wypadek, nie powinno się zdarzyć bo chronologia...
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
                        }
                    }
                }
            }
        }

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
                    paAccept_ModalPopupExtender.Show();
                    //paAccept_ModalPopupExtender.Focus();
#if RCP
#else
                    ((MasterPage)Page.Master).SetBodyScrollBar(false);
#endif
                }
                else
                {
                    //cntSelectOkres.Enabled = true;
                    paAccept_ModalPopupExtender.Hide();
#if RCP
#else
                    ((MasterPage)Page.Master).SetBodyScrollBar(true);
#endif
                }
                //paAccept.DefaultButton="tmpBtModalPopup" pomaga na Enter który klikał w cntSelectOkres mimo ze popup był na wierzchu
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
     }
}