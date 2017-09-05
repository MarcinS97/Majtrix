using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using HRRcp.App_Code;
using HRRcp.Controls.RozliczenieNadg;

namespace HRRcp.Controls
{
    public partial class PlanPracyRozliczenie : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //paAccept.DefaultButton="tmpBtModalPopup" pomaga na Enter który klikał w cntSelectOkres mimo ze popup był na wierzchu
            if (IsPostBack)
                if (AccPanelVisible)
                {
                    //paAccept_ModalPopupExtender.Show();
                }
            //----- data akceptacji -----
            if (!IsPostBack)
            {
                //Tools.MakeConfirmButton(btAccept, "Potwierdź akceptację czasu pracy do podanej daty.");
                //----- sprawdzanie -----
                //AppUser user = AppUser.CreateOrGetSession();
                //btCheck.Visible = user.IsAdmin;
                btCheck.Visible = false;
#if RCP
                lbPlanQ.Visible = false;
#endif
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
            cntPlanPracy.Prepare(kierId, cntSelectOkres.OkresOdStr, cntSelectOkres.OkresDoStr, 
                null, //cntSelectOkres.OkresId, 
                cntSelectOkres.Status, 
                false //cntSelectOkres.IsArch
                );
        }

        public void Prepare(string kierId)
        {
            _InitScripts();
            cntSelectOkres.Prepare(DateTime.Today, true);
            cntPlanPracy.Prepare(kierId);
        }

        public void Prepare()
        {
            _InitScripts();
            cntSelectOkres.Prepare(DateTime.Today, true);
            cntPlanPracy.Prepare(null);
        }

        public void PrepareV3()
        {
            _InitScripts();
            cntSelectOkres.Prepare(DateTime.Today, true);
            cntPlanPracy.Prepare(cntSelectOkres.OkresOdStr, cntSelectOkres.OkresDoStr);
        }




        //----- ---------------------
        public void _InitScripts()
        {
            /* 20170108 wydaje się nie być potrzebne
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
            */ 
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
        protected void cntPlanPracy_ShowRozliczenie(object sender, EventArgs e)
        {
            //cntPrzeznaczNadg2Popup.Show(cntPlanPracy.SelPracId, cntPlanPracy.DateFrom, cntPlanPracy.DateTo);
            RozliczenieNadgodzin.Show(cntPlanPracy.SelPracId, cntPlanPracy.DateFrom, cntPlanPracy.DateTo);
        }

        //---------------------------
        
        protected void cntAccept_CancelChanges(object sender, EventArgs e)
        {
            AccPanelVisible = false;
        }

        protected void cntAccept_AcceptChanges(object sender, EventArgs e)
        {
            AccPanelVisible = false;
            //DataBind();  // 20120622 wywala sie viewstate dla ludzi bez rcpid; to długo trwa, pomyslec nad optymalizacja !!!
            cntPlanPracy.DataBind();
        }

        public bool CheckAll(out string msg)
        {
            msg = "";
            return true;
        }

        protected void btAcceptPP_Click(object sender, EventArgs e)
        {
            /*
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
                            else  * /
                        {
                            //----- zatwierdzam !!! od początku okresu do dt bo mogli się pojawić inni pracownicy z nie zatwierdzonym planem przez poprzedniego kierownika
                            AppUser user = AppUser.CreateOrGetSession();
                            int err = Okres.AkceptujOkres(kp.KierId, user.OriginalId, kp,
                                                    cntPlanPracy.DateFrom, cntPlanPracy.DateTo,
                                                    dt);
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
            */
        }

        protected void btCheckPP_Click(object sender, EventArgs e)  // to samo co w test tylko dla 1 kier
        {
            Okres ok = new Okres(cntSelectOkres.OkresDo);
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
            DateTime dt = cntSelectOkres.OkresDo;
            if (dt < settings.SystemStartDate)
            {
                v = false;
                lbOkresStatus.Text = "Brak danych";
            }
            else
            {
                v = cntSelectOkres.Status != cntSelectOkres.okClosed;
                if (cntSelectOkres.Status == cntSelectOkres.okClosed)
                    lbOkresStatus.Text = "Okres rozliczeniowy zamknięty";
            }
            btAccept.Visible = v;
            
            btAccept.Visible = false;

            lbOkresStatus.Visible = !v;
            cntPlanPracy.OkresClosed = !v;
            return v;
        }

        protected void cntSelectOkres_Changed(object sender, EventArgs e)
        {
            showAccDate();


            cntPlanPracy.Prepare(cntSelectOkres.OkresOdStr, cntSelectOkres.OkresDoStr);   

        }

        protected void cntPrzeznaczNadg_Changed(object sender, EventArgs e)
        {
            cntPlanPracy.DataBind();
        }
        //---------------------------
        public HRRcp.Controls.RozliczenieNadg.cntSelectOkres SelectOkres
        {
            get { return cntSelectOkres; }
        }

        public bool AccPanelVisible
        {
            get { return (string)ViewState[ID + "_accVisible"] == "1"; }
            set 
            {
                ViewState[ID + "_accVisible"] = value ? "1" : "0";
                /*
                if (value)
                {
                    //cntSelectOkres.Enabled = false;
                    paAccept_ModalPopupExtender.Show();
                    //paAccept_ModalPopupExtender.Focus();
                    ((MasterPage)Page.Master).SetBodyScrollBar(false);
                }
                else
                {
                    //cntSelectOkres.Enabled = true;
                    paAccept_ModalPopupExtender.Hide();
                    ((MasterPage)Page.Master).SetBodyScrollBar(true);
                }
                //paAccept.DefaultButton="tmpBtModalPopup" pomaga na Enter który klikał w cntSelectOkres mimo ze popup był na wierzchu
                */ 
            }   
        }
        
        //---------------------------
        public string Search
        {
            set;
            get;
        }
    }
}