using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class AcceptControl4 : System.Web.UI.UserControl
    {
        public event EventHandler AcceptChanges;
        public event EventHandler CancelChanges;
        public event EventHandler NadgodzinyWnioskiModalShow;

        int FAdm = PlanPracy.moAdmin;

        const string nodata = "brak danych";
        const string zerodata = "brak";

        AppUser user;

        const string ses_active_tab = "tabPPAcc";

        bool klasNadg = false; // zakładka klasyfikacji naadgodzin

        protected void Page_Load(object sender, EventArgs e)
        {
            user = AppUser.CreateOrGetSession();
            klasNadg = user.HasRight(AppUser.rTester);

            if (!IsPostBack)
            {
                Ustawienia settings = Ustawienia.CreateOrGetSession();

                trWorktimeAll.Visible = false;

                if (Lic.PodzialKosztow == false) Tools.RemoveMenu(tabAccept, tabPodzialMPK);

                /*
                if (user.IsAdmin)   // kierownik, który nie jest adminiem nie widzi kontrolek wyboru zaokrąleń !!!
                //if (false)
                {
                    App.FillTimeRound(ddlTimeRound, cntRcp.Round, settings.Zaokr.ToString());
                    lbTimeRound.Visible = true;
                    ddlTimeRound.Visible = true;
                }
                else
                {
                    lbTimeRound.Visible = false;
                    ddlTimeRound.Visible = false;
                }
                */
 




                //lbNocneOdDo.Text = String.Format("({0}-{1})", settings.NocneOdSec / 3600, settings.NocneDoSec / 3600);
                lbNocneOdDo.Text = App.GetNocneOdDo;

//                DataSet ds = Base.getDataSet(@"
//select 'brak zmiany (plan)' as Nazwa, null as Id, 0 as Sort, 0 as Kolejnosc 
//    union 
//select 'brak zmiany' as Nazwa, -1 as Id, 1 as Sort, 0 as Kolejnosc
//    union 
//select Symbol + ISNULL(' - ' + Nazwa, '') + 
//    case when Od <> Do then ' ' + LEFT(convert(varchar, Od, 8),5) + ' - ' + LEFT(convert(varchar, Do, 8),5) else '' end +
//    case when HideZgoda = 0 and ZgodaNadg = 1 then ' - ZGODA NADG.' else '' end
//    as Nazwa, Id, 2 as Sort, Kolejnosc 
//from Zmiany 
//where Visible=1 and Widoczna=1
//order by Sort, Kolejnosc, Nazwa
//                ");
                //--where Widoczna=1 - okres przejsciowy zmiany z marginesem

                /* 20161101
                DataSet ds = db.Select.Set(dsZmiany, null);

                //select Symbol + ' - ' + Nazwa as Nazwa, Id from Zmiany
                Tools.BindData(ddlZmiana, ds, "Nazwa", "Id", false, null);
                Tools.BindData(ddlZmiana0, ds, "Nazwa", "Id", false, null);
                */
                //ddlZmiana0.Items.AddRange(ddlZmiana.Items.OfType<ListItem>().ToArray());
#if SIEMENS
                trComm.Visible = false;
                if (Lic.ScoreCards) trArkusz.Visible = true;
                lbZmianaBrak.Text = "Wymagana zgoda na nadgodziny";
#elif DBW
                trGrupa.Visible = true;
                trClass.Visible = true;
                ltClass.Text = "Firma:";
#elif VICIM
                trComm.Visible = false;
                trGrupa.Visible = false;
                trClass.Visible = false;
#else
                trCC.Visible    = true;
                trComm.Visible  = true;
#endif
                if (!klasNadg)
                    Tools.RemoveMenu(tabAccept, tabRozlNadg);  //<<<< póki co 
            }
        }

        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {
            RenderAlerty();
            cntReport2.SQL1 = PracId;
            cntReport2.SQL2 = Data;
            cntReport2.DataBind();
            cntReport2.Prepare();
            base.OnPreRender(e);
        }
        //-----------------------------------------
        const string tabCzasPracy   = "pgCzasPracy";
        const string tabPodzialMPK  = "pgPodzialKosztow";
        const string tabRozlNadg    = "pgNadgodziny";

        private bool CheckMPKEdit()
        {
            if (cntMPK.InEditMode)
            {
                if (tabAccept.SelectedValue != tabPodzialMPK)
                {
                    Tools.SelectMenu(tabAccept, tabPodzialMPK);
                    SelectTab();
                }
                Tools.ShowMessage("Proszę zakończyć edycję podziału kosztów.");
                return false;
            }
            else if (klasNadg && cntPrzeznaczNadg.InEditMode)
            {
                if (tabAccept.SelectedValue != tabRozlNadg)
                {
                    Tools.SelectMenu(tabAccept, tabRozlNadg);
                    SelectTab();
                }
                /*
                if (!tabAccept.Items[2].Selected)
                {
                    tabAccept.Items[2].Selected = true;
                    SelectTab();
                }
                 */ 
                Tools.ShowMessage("Proszę zakończyć edycję klasyfikacji nadgodzin.");
                return false;
            }
            else
                return true;
        }

        public bool doAccept = false;//20160301

        private void DoAccept(bool errAcc)
        {
            if (Update(true, errAcc))   // jak nie moze zaakceptować to i tak powinien zamknąć bo zapisał i Anuluj nie cofnie już zmian
            {
                doAccept = true;
                if (AcceptChanges != null)
                    AcceptChanges(this, EventArgs.Empty);
                doAccept = false;
            }
        }

        protected void btAccept_Click(object sender, EventArgs e)
        {
            if (CheckMPKEdit())
                if (Validate())
                    DoAccept(false);   // false - nie akceptuj chyba ze bez błędu, jak błąd to się wyswietli pytania o potwierdzenie akceptacji mimo błędów (przekroczenie 11h i 35h)
        }

        protected void btAccept2_Click(object sender, EventArgs e)
        {
            DoAccept(true);   // akceptuj zawsze mimo błędów przekroczenia 11h i 35h - II krok, po wyświetleniu warning'a
        }
        //----------------
        protected void btCloseAcc_Click(object sender, EventArgs e)
        {
            cntMPK.InEditMode = false;
            cntPrzeznaczNadg.InEditMode = false;
            object o = ViewState["-acc"];
            if (o != null && o.ToString() == Base.bTRUE) // było cofnięcie akceptacji - w takim przypadku anuluj tez musi odswieżyć dane
            {
                if (AcceptChanges != null)                  // wyzej tylko odswieza i zamyka wiec prosciej tak niz nowe zdarzenie do odswiezenia
                    AcceptChanges(this, EventArgs.Empty);
            }
            else
                if (CancelChanges != null)
                    CancelChanges(this, EventArgs.Empty);
        }

        protected void btSaveAcc_Click(object sender, EventArgs e)
        {
            if (CheckMPKEdit())
                if (Validate())
                {
                    Update(false, false);     // nie akceptuje i nie akceptuje mimo bledu (chyba 1 false to obejmuje)
                    if (AcceptChanges != null)
                        AcceptChanges(this, EventArgs.Empty);
                }
        }

        protected void btUnlock_Click(object sender, EventArgs e)
        {
            //UpdateAcc(false);
            UnlockAcc();
            //Prepare(PracId, Data, hidFrom.Value, hidTo.Value);  
            Refresh(true);
            ViewState["-acc"] = Base.bTRUE;
            /*
            if (CancelChanges != null)
                CancelChanges(this, EventArgs.Empty);
             */
        }

        protected void btWniosekNadg_Click(object sender, EventArgs e)
        {
#if RCP

            int N50,N100,NNoc;

            if (teNadgNoc.Time.TotalSeconds > 0)
            {
                N100 = Convert.ToInt32(teNadgNoc.Time.TotalSeconds);
                NNoc = Convert.ToInt32(teNadgNoc.Time.TotalSeconds);
            }
            else 
            {
                N100 = (int)ViewState["nadg100NaWniosek"];
                NNoc = (int)ViewState["nadgNocNaWniosek"];
            }

            if (teNadgDzien.Time.TotalSeconds > 0) N50 = Convert.ToInt32(teNadgDzien.Time.TotalSeconds);
            else N50 = (int)ViewState["nadg50NaWniosek"];

       


            double nadg = 0, nDzien = 0, nNoc = 0;
            if (teNadgDzien.Seconds != null)
                nDzien = (int)teNadgDzien.Seconds / 3600;

            if (teNadgNoc.Seconds != null)
                nNoc = (int)teNadgNoc.Seconds / 3600;


           
            




            /*cntNadgodzinyWnioskiModal.Show(PracId, Data, Worktime.Round05(nDzien + nNoc, 2).ToString(), null, null, null, RCP.Controls.cntNadgodzinyWnioskiModal.EType.None, null, null);            // Data, Ilości*/
            if (NadgodzinyWnioskiModalShow != null)
                NadgodzinyWnioskiModalShow(new NadgodzinyWnioskiParams { PracId = PracId, Data = Data, Wt = Worktime.Round05(nDzien + nNoc, 2).ToString(), N50 = N50, N100 = N100, NNoc = NNoc, Type = RCP.Controls.cntNadgodzinyWnioskiModal.EType.None }, EventArgs.Empty);
#endif
        }

        public class NadgodzinyWnioskiParams
        {
            public String PracId;
            public String Data;
            public String Wt;
            public int N50, N100, NNoc;
            public RCP.Controls.cntNadgodzinyWnioskiModal.EType Type;
        }

//        protected void btWniosekNadg_Click(object sender, EventArgs e)
//        {
//#if RCP
//            cntNadgodzinyWnioskiModal.Show(PracId, Data, null, null, null, null, RCP.Controls.cntNadgodzinyWnioskiModal.EType.None, null, null);            // Data, Ilości
//#endif
//        }
        //------------------------------
        protected void ddlTimeRound_SelectedIndexChanged(object sender, EventArgs e)
        {



            //cntRcp.Round = ddlTimeRound.SelectedValue;



        }

        private string FillZmianaTime(string sel)
        {

            return sel;
        }

        protected void ddlZmiana_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            Tools.SelectItem(ddl == ddlZmiana ? ddlZmiana0 : ddlZmiana, FillZmianaTime(ddl.SelectedValue));   // aktualizuję drugi ddl
            Refresh(false);
        }

        //niewykorzytywane
        protected void ddlZmianaTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            Tools.SelectItem(ddl == ddlZmianaTime ? ddlZmianaTime0 : ddlZmianaTime, ddl.SelectedValue);   // aktualizuję drugi ddl
            Refresh(false);
        }
        //-----------------
        public void x_Prepare(string pracId, string data, string fromTime, string toTime, 
                            string strefaId, string algRCP,
                            int zaokr, int zaokrType, int breakMin, int breakMin2, int marginMin)
        {
            PracId = pracId;
            Data = data;
            hidFrom.Value = fromTime;
            hidTo.Value = toTime;
            FillData(fromTime, toTime, strefaId, algRCP, zaokr, zaokrType, breakMin, breakMin2, marginMin, true);
        }

        public void Prepare(string pracId, string data, string fromTime, string toTime, string kierId)  // kierownik, w którego kontekscie oglądam pracowników
        {
            PracId = pracId;
            Data = data;
            //ViewAs = viewAs;
            KierId = kierId;
            hidFrom.Value = fromTime;
            hidTo.Value = toTime;
            FillData(fromTime, toTime, null, null, -1, -1, -1, -1, -1, true);
        }

        public void Refresh(bool setZmiana)   // po zmienie zmiany
        { 
            FillData(hidFrom.Value, hidTo.Value, null, null, -1, -1, -1, -1, -1, setZmiana);
        }

        private void FillStatus(DataRow wtdr, bool closed, bool acc)
        {
            string kierId = Base.getValue(wtdr, "IdKierownikaAcc");
            DateTime? dt = Base.getDateTime(wtdr, "DataAcc");
            string status, hint;
            if (closed)
                if (acc) status = "Zaakceptowany - zablokowany";      // musi być zaakceptowane !
                else     status = "Niezaakceptowany - zablokowany";
            else
                if (acc) status = "Zaakceptowany";
                else
                {
                    if (String.IsNullOrEmpty(kierId) && dt == null)
                        status = "Nie modyfikowany";                 // nowy wpis
                    else
                        status = "Zmodyfikowany, przed akceptacją";  // zmodyfikowany
                }

            hint = String.Format("{0}, {1}", 
                !String.IsNullOrEmpty(kierId) ? AppUser.GetNazwiskoImie(kierId) : null,
                dt != null ? Tools.DateTimeToStr((DateTime)dt) : null);

            lbStatus.Text = status;
            lbStatus.ToolTip = hint;
        }

        private string GetAbsencjaNazwa(string aid)
        {
            if (!String.IsNullOrEmpty(aid)) 
                 return db.getScalar("select ISNULL(Symbol, 'X') + ' - ' + Nazwa from AbsencjaKody where Kod = " + aid);
            else return null;
        }

        private string GetTime(object dt, string format)
        {
            if (Base.isNull(dt))
                return null;
            else
                return ((DateTime)dt).ToString(format);
        }

        private string GetTimeRCP(object dt)
        {
            if (Base.isNull(dt))
                return null;
            else
                return Base.TimeToStr((DateTime)dt);
        }

        private string GetTimeSec(object sec, string format, string nullValue)
        {
            if (Base.isNull(sec))
                return nullValue;
            else
            {
                if (format.Contains(':'))
                    return DateTime.MinValue.AddSeconds((int)sec).ToString(format);
                else
                {
                    string t = DateTime.MinValue.AddSeconds((int)sec).ToString(format + ":00");
                    return t.Substring(0, t.Length - 3);
                }
            }
        }
        //----------------------
        private string GetZmiana(string zmId, int wym)
        {

            if (String.IsNullOrEmpty(zmId))
            {
                /* na liscie brak zmiany - PLAN jest -1|0 */
                zmId = "-1";
                wym = 0;
            }
            return db.Select.Scalar(dsZmiany, db.NULL, db.NULL, db.NULL, db.NULL, zmId, wym, 0);
        }    
            
        /*
        private string GetZmiana(string zmId)
        {
            if (!String.IsNullOrEmpty(zmId))
            {
                DataRow zdr = db.getDataRow("select * from Zmiany where Id = " + zmId);
                if (zdr != null)
                {
                    DateTime dtOd = db.getDateTime(zdr, "Od", DateTime.MinValue);
                    DateTime dtDo = db.getDateTime(zdr, "Do", DateTime.MinValue);
                    if (dtOd == dtDo)
                        return String.Format("{0} - {1}",
                                db.getValue(zdr, "Symbol"),
                                db.getValue(zdr, "Nazwa"));
                    else
                    return String.Format("{0} - {1} {2} - {3}",
                            db.getValue(zdr, "Symbol"),
                            db.getValue(zdr, "Nazwa"),
                            dtOd.ToString("H:mm"),
                            dtDo.ToString("H:mm"));
                }
                else
                    return null;
            }
            else
                return "brak zmiany";
        }
        */

        //--------------------------------------------------------------------------------------------------
        private void GetCCList(string przId, out string cclist, out string ccwsp, int maxlen)
        {
            if (!String.IsNullOrEmpty(przId))  // jeżeli nowy pracownik to może jeszcze nie być przypisany
            {
                /*
select cc, 
cc + ' - ' + Nazwa + ' (' + CONVERT(varchar, W.Wsp) + ')',
'(' + CONVERT(varchar, W.Wsp) + ') ' + cc + ' - ' + Nazwa,
cc + ' - ' + CONVERT(varchar, W.Wsp) + ' (' +  + Nazwa + ')'
from SplityWspP W
left outer join CC on CC.Id = W.IdCC
where W.IdPrzypisania = {0}

--cc + ' - ' + Nazwa + ' (' + CONVERT(varchar, W.Wsp) + ')',
--'(' + CONVERT(varchar, W.Wsp) + ') ' + cc + ' - ' + Nazwa,
--cc + ' - ' + CONVERT(varchar, W.Wsp) + ' (' + Nazwa + ')'
cc + ' - (' + case when round(W.Wsp, 4) = 1 then '1.0' else CONVERT(varchar, round(W.Wsp, 4)) end + ') ' + Nazwa,

cc + ' - ' + Nazwa + 
REPLICATE(' ', (select max(len(cc + Nazwa)) from CC) - len(cc + Nazwa)) +
'   ( x ' + case when round(W.Wsp, 4) = 1 then '1.0' else CONVERT(varchar, round(W.Wsp, 4)) end + ')'
                 */
                DataSet ds = db.getDataSet(String.Format(@"
select 
cc, 
cc + ' - ' + Nazwa,
cc + ' - ' + Nazwa + ' ( x ' + case when round(W.Wsp, 4) = 1 then '1.0' else CONVERT(varchar, round(W.Wsp, 4)) end + ' )'
from SplityWspP W
left outer join CC on CC.Id = W.IdCC
where W.IdPrzypisania = {0}
order by cc
                ", przId));
                if (db.getCount(ds) == 1)
                {
                    DataRow dr = db.getRow(ds);
                    cclist = db.getValue(dr, 1);
                }
                else
                {
                    cclist = db.Join(ds, 0, ",");
                    if (!String.IsNullOrEmpty(cclist))  //40
                        if (cclist.Length > maxlen)
                            cclist = cclist.Substring(0, maxlen - 5) + "…";
                }
                ccwsp = db.Join(ds, 2, "\n");
            }
            else
            {
                cclist = "brak cc";
                ccwsp = null;
            }
        }

        private void SelectZmiana(string planzm, string planwym, string korzm, string korwym, int pracWymiar)
        {
            DataSet ds = db.Select.Set(dsZmiany
                , db.nullParam(planzm), db.nullParam(planwym)
                , db.nullParam(korzm), db.nullParam(korwym)
                , db.NULL, db.NULL
                , pracWymiar
                );
            if (String.IsNullOrEmpty(planzm))
            {
                planzm = "-1";
                planwym = "0";
            }
            string sel = String.IsNullOrEmpty(korzm) ? Tools.SetLineParams(planzm, planwym) : Tools.SetLineParams(korzm, korwym);
            Tools.BindData(ddlZmiana, ds, "Nazwa", "Id", false, sel);
            Tools.BindData(ddlZmiana0, ds, "Nazwa", "Id", false, sel);
        }

        /*
        private void SelectZmiana(string zmid)
        {
            Tools.SelectItem(ddlZmiana, zmid);
            Tools.SelectItem(ddlZmiana0, zmid);
        }
        */

        private void FillData(string fromTime, string toTime, 
        
                              string x_strefaId, string x_algRCP,    // mogą byc null - sam pobierze
                              
                              int zaokr, int zaokrType, int breakMM, int breakMM2, int marginMM,
                              bool setZmiana)
        {
            //const string plan = " (plan)";
            //const string korekta = " (korekta)";
            const string _plan = " <span class=\"comment\">(plan)</span>";
            const string korekta = " <span class=\"comment\">(korekta)</span>";
            
            //SqlConnection con = Base.Connect();

            //----- is locked -----
            Okres ok = new Okres(db.con, DateTime.Parse(toTime));              // lub pobrac te informacje z PlanPracyNavigator'a            
            int? okresId = ok.IsArch() ? (int?)ok.Id : null;   //>>> zmienić na Okres.GetId




            //DataRow pdr = Base.getRow(Worker.GetPracInfo1(con, 1, okresId, null, PracId, true));
            DataRow pdr = Base.getRow(Worker.GetPracInfo2(1, Data, okresId, null, PracId, true, true, true));




            //----- rcp, strefa, algorytm -----------
            string rcpId = Base.getValue(pdr, "RcpId");
            
            /* 20131111
            if (String.IsNullOrEmpty(strefaId)) strefaId = Base.getValue(pdr, "StrefaId");  // !!! po zrobieniu historii zmian stref, dać null
            if (String.IsNullOrEmpty(strefaId)) strefaId = "0"; 
            if (String.IsNullOrEmpty(algRCP)) algRCP = Base.getValue(pdr, "Algorytm");      // !!! j.w.
            //if (String.IsNullOrEmpty(algRCP)) algRCP = "0";   20130330 wyłaczam bo nie wyświetli alertu jak nie ma algorytmu
            */
            string _strefaId    = db.getValue(pdr, "RcpStrefaId");
            string _algRCP      = db.getValue(pdr, "RcpAlgorytm");
            int przerWlicz      = db.getInt(pdr, "PrzerwaWliczona", 0);
            int przerNiewlicz   = db.getInt(pdr, "PrzerwaNiewliczona", 0);
            int wymiar          = db.getInt(pdr, "WymiarCzasu", 0);
            int etatL           = db.getInt(pdr, "EtatL", -1);
            int etatM           = db.getInt(pdr, "EtatM", -1); 

            DataRow adr;
            string algPar;
            string algNazwa;
            if (!String.IsNullOrEmpty(_algRCP))
            {
                adr = db.getDataRow("select * from Kody where Typ='ALG' and Kod=" + _algRCP);
                algPar = db.getValue(adr, "Parametr");
                algNazwa = db.getValue(adr, "Nazwa");
            }
            else
            {
                adr = null;
                algPar = null;
                algNazwa = null;
            }
            //----- parametry kierownika ------------
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            //KierParams kp = new KierParams(Base.getValue(pdr, "IdKierownika"), settings);
            KierParams kp = new KierParams(Base.getValue(pdr, "KierId"), settings);
            if (zaokr == -1)
            {
                zaokr = settings.Zaokr;
                zaokrType = settings.ZaokrType;
            }
            if (breakMM == -1)  // domyslnie wtedy pozostałe też
            {
                breakMM = kp.Przerwa;
                breakMM2 = kp.Przerwa2;
                marginMM = kp.Margines;
            }

            //----- z przypisań -------
            /*
            DataRow drPrz = db.getDataRow(String.Format(@"
select R.Id,
    R.IdKierownika, K.Nazwisko + ' ' + K.Imie as Kierownik,
	S.Nazwa as RcpStrefa
from Przypisania R 
left outer join Pracownicy K on K.Id = R.IdKierownika
left outer join Strefy S on S.Id = R.RcpStrefaId
where R.Status = 1 and '{0}' between R.Od and ISNULL(R.Do, '20990909') and R.IdPracownika = {1}", 
            Data, PracId));
            */

            string przId = db.getValue(pdr, "PrzId");
            //string pkId = db.getValue(pdr, "IdKierownika");       // kier z przypisania, powinno być... chyba ze pracownik zwolniony i po dacie Do z przypisania
            string pkId = db.getValue(pdr, "KierId"); 
            string pknazwisko = db.getValue(pdr, "KierownikNI");



            //bool isInnyKier = !String.IsNullOrEmpty(pkid) && pkid != KierId; // App.User.Id;
            bool isInnyKier = pkId != KierId; // App.User.Id;
            //bool kierCanEdit = !isInnyKier && (ViewAs == PlanPracy.asKier || ViewAs == PlanPracy.asParent);

            

            bool jaNoEdit = Lic.zastJaNoEdit && PracId == App.User.OriginalId;    // nie mogę edytować siebie


            //----- korekta zmiany ------------------
            string _kzmid = null;
            string kwym = null;
            if (!setZmiana)
            {
                //kzmid = ddlZmiana.SelectedValue;
                _kzmid = Tools.GetLineParam(ddlZmiana.SelectedValue, 0);
                kwym = Tools.GetLineParam(ddlZmiana.SelectedValue, 1);
            }

            // null - wg planu pracy
            // -1 - brak zmiany
            // nn - zmiana (korekata)
            //
            //----- pobieranie danych ---------------
            Worktime2 wt2;
            DataSet ds = Worktime._GetWorktime(
                            PracId, 
                            
                            _algRCP, algPar, rcpId, true,
                            
                            fromTime, toTime, Data, 
                            
                            _strefaId, 
                            
                            zaokr, zaokrType,
                            settings.NocneOdSec, settings.NocneDoSec,
                            breakMM, breakMM2,



                            //!setZmiana && String.IsNullOrEmpty(kzmid) ? "-1" : kzmid,  //20131111
                            !setZmiana && String.IsNullOrEmpty(_kzmid) ? "P" : _kzmid,  //20131111 - jeżeli nie setZmiana i null to zmiana z planu (nie korekta)
                            //kzmid,

                            Tools.StrToInt(kwym, 28800), /* wymiar */

                            true,
                            out wt2);   // -1 tak samo jak brak zmiany z pp
            DataRow wtdr = Base.getDataRow(ds);
            //----- analiza rcp -----
            //if (wt2 != null && (algRCP == "2" || algRCP == "12"))    // rcpId == null -> wt2 == null, dla algorytmów sumy w strefie
            //if (wt2 != null)    // rcpId == null -> wt2 == null, dla algorytmów sumy w strefie
            
            
            
            
            /* 20131111            
            if (wt2 != null && _algRCP != Worktime2.algObecnosc8h)  //"3")    
            {
                //int? idx1 = Base.getInt(wtdr, "idx1rcp");
                //int? idx2 = Base.getInt(wtdr, "idx2rcp");
                cntRcpAnalize.Prepare(wt2.GetRcpAnalize(/*idx1, idx2* /), _algRCP);
            }
            else cntRcpAnalize.Prepare(null, _algRCP);
            */

            if (wt2 != null)
                cntRcpAnalize.Prepare(wt2.GetRcpAnalize(/*idx1, idx2*/), _algRCP, wtdr);




            DateTime day = (DateTime)Base.getDateTime(wtdr, "Data");        // musi być 
            //----- status --------------------------
            //bool closed = ok.Status == Okres.stClosed;                    // okres rozliczeniowy zamkniety
            Closed = ok.Status == Okres.stClosed || day <= ok.LockedTo;     // okres rozliczeniowy zamkniety lub zamkniety tydzien
            bool closed = Closed;
            bool accepted = Base.getBool(wtdr, "Akceptacja", false);        // czy dane za dzień zaakceptowane
            //----- is locked -----
            FillStatus(wtdr, closed, accepted);                
            bool dayAccepted = kp.DataAccDo != null ? day <= kp.DataAccDo : false;
            //----- tabelka z danymi pracownika -----
            //ReadOnly = closed || accepted;
            //ReadOnly = accepted;    //20130401 - admin moze odblokowac poszczególne dni
            //ReadOnly = accepted || (isInnyKier && !App.User.IsAdmin);    //z przypisań - czy inny kierownik, jak admin to nie read only
            
            ReadOnly = accepted || ((isInnyKier || jaNoEdit) && !App.User.IsAdmin);    //z przypisań - czy inny kierownik, jak admin to nie read only




            //!!! ustawić strefaId i algRCP na wartości na dzień Data !!! jak będzie obsługa w czasie !!!
                        
            
            DateTime dt;
            string dn = null;
            if (DateTime.TryParse(Data, out dt))
            {
                int d = (int)dt.DayOfWeek;  // 0-sunday, 1-monday .. 6-saturday
                if (0 <= d && d <= 6)
                    //dn = String.Format(" ({0})", Tools.DayName[d + 1]);
                    dn = String.Format(" {0}", Tools.DayName[d + 1]);
            }




            /*
            Title31.SubValue1 = Base.getValue(pdr, "NazwiskoImie");
            if (!String.IsNullOrEmpty(pkid) && pkid != App.User.Id)
            {
                Title31.SubText2 = "Kierownik:";
                Title31.SubText3 = "Data:";
                Title31.SubValue2 = pknazwisko;
                Title31.SubValue3 = Data + dn;
            }
            else
            {
                Title31.SubText2 = "Data:";
                Title31.SubValue2 = Data + dn;
                Title31.SubText3 = null;
                Title31.SubValue3 = null;            
            }
            */

            string pkn = null;
            if (isInnyKier)
                if (String.IsNullOrEmpty(przId))
                    pkn = " <span class=\"error\">(brak przypisania)</span>";
                else
                    pkn = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"t1\">kierownik:</span> " + pknazwisko;
            
            
            //----- testy -----
            if (App.User.HasRight(AppUser.rSuperuser))
                pkn += String.Format(" [{0}, {1}]", KierId, AppUser.GetNazwiskoImieNREW(KierId));
            //----- testy -----


            lbData.Text = Data;
            lbDataDzien.Text = dn;
            lbKadryId.Text = db.getValue(pdr, "KadryId");
            lbPracownik.Text = db.getValue(pdr, "NazwiskoImie");
            lbPracownik.ToolTip = pkn;

            trEtat.Visible = true;
            trWymiar.Visible = true;
            trPrzerwaWliczona.Visible = true;
            trPrzerwaNiewliczona.Visible = true;
            lbWymiarT.Text             = Worktime.SecToTime(wymiar, 1);
            lbEtat.Text               = etatL != -1 && etatM != -1 ? String.Format("{0}/{1}", etatL, etatM) : "?";
            lbPrzerwaWliczona.Text    = Worktime.SecToTime(przerWlicz, 1);
            lbPrzerwaNiewliczona.Text = Worktime.SecToTime(przerNiewlicz, 1);


            //lbDzial.Text = Base.getValue(pdr, "Dzial") + " (" + Base.getValue(pdr, "CCInfoMix") + ")";
            lbDzial.Text = Base.getValue(pdr, "Dzial");
            lbStanowisko.Text = Base.getValue(pdr, "Stanowisko");

            lbGrupa.Text = db.getValue(pdr, "Grupa");
            lbClass.Text = db.getValue(pdr, "Klasyfikacja");
            lbGrade.Text = db.getValue(pdr, "Grade");

            trKier.Visible = isInnyKier;
            if (isInnyKier)
            {
                if (String.IsNullOrEmpty(przId))
                    lbKier.Text = "brak przypisania";
                else
                    lbKier.Text = pknazwisko;
            }

            string cclist, ccwsp;
            GetCCList(przId, out cclist, out ccwsp, 50);
            lbCC.Text = cclist;
            lbCC.ToolTip = ccwsp; 

            /*
            if (!String.IsNullOrEmpty(strefaId))
                lbStrefaRCP.Text = Base.getScalar(con, "select Nazwa from Strefy where Id=" + strefaId);
            */
            lbStrefaRCP.Text = db.getValue(pdr, "RcpStrefa");
            lbAlgorytm.Text = algNazwa;

            lbCommodity.Text = db.getValue(pdr, "Commodity");
            lbArea.Text = db.getValue(pdr, "Area");
            lbPosition.Text = db.getValue(pdr, "Position");

#if SIEMENS
            if (Lic.ScoreCards) lbArkusz.Text = db.getValue(pdr, "Commodity");
#endif
            //----- zmiana -----   
            string planZmId = Base.getValue(wtdr, "IdZmiany");

            IdZmianyPlan = planZmId;

            //zzz
            int wymiarPlan = db.getInt(wtdr, "Wymiar", 28800);   // 
            int wymiarKor  = db.getInt(wtdr, "WymiarKorekta", 28800);   // 

            WymiarPlan = wymiarPlan.ToString();



            if (setZmiana)
            {
                _kzmid = Base.getValue(wtdr, "IdZmianyKorekta");          // jesli null to IdZmiany, jesli -1 to nie ma
            }
            bool isZmiana = !String.IsNullOrEmpty(planZmId) || (!String.IsNullOrEmpty(_kzmid) && _kzmid != "-1");
            
            //string _planZm = GetZmiana(planZmId, wymiarPlan);





            ddlZmiana.Visible  = !ReadOnly;
            ddlZmiana0.Visible = !ReadOnly;
            lbZmiana.Visible   = ReadOnly;
            lbZmiana0.Visible  = ReadOnly;

            bool zg = db.getBool(wtdr, "ZgodaNadg", false);
            bool hide = db.getBool(wtdr, "HideZgoda", true);
            lbZmianaZgoda.Visible = !hide && zg && isZmiana;
            lbZmianaBrak.Visible = !hide && !zg && isZmiana;

            if (!ReadOnly)
            {
                if (setZmiana)
                {
                    SelectZmiana(planZmId, wymiarPlan.ToString(), _kzmid, wymiarKor.ToString(), wymiar);                            // jak null to zaznaczy 0 cz. ok
                }
            }
            else
            {
                string zm;
                if (String.IsNullOrEmpty(_kzmid))
                    zm = GetZmiana(planZmId, wymiarPlan) /*+ plan*/; /* PLAN jest dokladany w SQL */
                else
                    zm = GetZmiana(_kzmid, wymiarKor) + korekta;
                lbZmiana.Text  = zm;
                lbZmiana0.Text = zm;    
            }

            /* 20161101
            if (!ReadOnly)
            {
                if (setZmiana)
                {
                    ddlZmiana.Items[0].Text = planZm + " (plan)";   // to uaktualni oba ddl
                    ddlZmiana0.Items[0].Text = planZm + " (plan)";   // juz nie ...
                    SelectZmiana(kzmid);                            // jak null to zaznaczy 0 cz. ok
                }
            }
            else
            {
                if (String.IsNullOrEmpty(kzmid))
                    lbZmiana.Text = planZm + plan;
                else
                    lbZmiana.Text = GetZmiana(kzmid) + korekta;
            }
            */
            /* II wersja 
            string zmid = Base.getValue(wtdr, "IdZmiany");
            string symbol = Base.getValue(wtdr, "Symbol");
            bool isZmiana = !String.IsNullOrEmpty(symbol);
            hidZmianaId.Value = zmid;

            ddlZmiana.Visible = !ReadOnly;
            lbZmiana.Visible = ReadOnly;
            if (!ReadOnly)
            {
                string kzmid = Base.getValue(wtdr, "IdZmianyKorekta");      // jesli null to IdZmiany, jesli -1 to nie ma
                //foreach (ListItem li in ddlZmiana.Items)
                //    if (!String.IsNullOrEmpty(li.Value))
                //        if (li.Value.StartsWith("d"))
                //        {
                //            li.Value = li.Value.Substring(1);
                //            break;      // tylko 1 jest zaznaczone
                //        }
                string sel;
                if (String.IsNullOrEmpty(kzmid))    // nie skorygowana
                    if (String.IsNullOrEmpty(zmid)) // brak zmiany
                        sel = "-1";
                    else sel = zmid;
                else sel = kzmid;
                ListItem defZmiana = Tools.SelectItem(ddlZmiana, sel, zmid, true, null, " (plan)");
                //if (defZmiana != null) defZmiana.Value = "d" + defZmiana.Value;
            }
            else
            {
                if (String.IsNullOrEmpty(symbol))
                    lbZmiana.Text = null;
                else
                    lbZmiana.Text = String.Format("{0} - {1} {2} - {3}", 
                                        symbol, 
                                        Base.getValue(wtdr, "NazwaZmiany"),
                                        Base.getDateTime(wtdr, "ZmianaOd", DateTime.MinValue).ToString("H:mm"),
                                        Base.getDateTime(wtdr, "ZmianaDo", DateTime.MinValue).ToString("H:mm"));
            }
             
             */
            /*
            string idZmiany = String.IsNullOrEmpty(kzmid) ? zmid : kzmid;
            string nazwaZmiany = 

            bool isZmiana = !String.IsNullOrEmpty(zmid);
            if (isZmiana)
            {
                DataRow zdr = Base.getDataRow(con, "select * from Zmiany where Id = " + zmid);
                if (zdr != null)
                {
                    lbZmiana.Text = Base.getValue(zdr, "Symbol") + " " + Base.getValue(zdr, "Nazwa");
                }
            }
            */
            //----- tabelka z czasem pracy -----
            /*
            string _tin = Base.getValue(wtdr, "TimeIn");
            string tout = Base.getValue(wtdr, "TimeOut");
            lbTimeIn.Text = String.IsNullOrEmpty(_tin) ? nodata : _tin.Substring(11);       // odcinam datę, tak najprościej
            lbTimeOut.Text = String.IsNullOrEmpty(tout) ? nodata : tout.Substring(11);
            */
            DateTime? tin2 = Base.getDateTime(wtdr, "TimeIn");
            DateTime? tout2 = Base.getDateTime(wtdr, "TimeOut");
            string t1 = tin2 == null ? nodata : Base.TimeToStr((DateTime)tin2);
            string t2 = tout2 == null ? nodata : Base.TimeToStr((DateTime)tout2);
            lbTimeIn.Text = t1;
            lbTimeOut.Text = t2;

            /****/
            Label lb2 = (Label)cntRcpAnalize.FindControl("ListView1").FindControl("SumCzasPrac");



            /****/

            bool b = true;
            switch (_algRCP)
            {
                case Worktime2.algBezLiczenia:      //"0":
                    lbWorktimeAll.Text = zerodata;
                    /****/
                    if (lb2 != null) lb2.Text = zerodata;
                    /****/
                    break;
                case Worktime2.algWeWy:             //"1":
                case Worktime2.algWeWyNadg:         //"11":
                    string c1 = Base.getValue(wtdr, "Czas1");
                    lbWorktimeAll.Text = String.IsNullOrEmpty(c1) ? nodata : String.Format("{0} ({1})", Base.getValue(wtdr, "Czas1R"), c1);
                    /****/
                    if (lb2 != null) lb2.Text = String.IsNullOrEmpty(c1) ? nodata : c1;
                    /****/
                    b = false;   // z przerwami
                    break;
                case Worktime2.algSuma:             //"2":
                case Worktime2.algSumaNadg:         //"12":
                    string c2 = Base.getValue(wtdr, "Czas2");
                    lbWorktimeAll.Text = String.IsNullOrEmpty(c2) ? nodata : String.Format("{0} ({1})", Base.getValue(wtdr, "Czas2R"), c2);
                    /****/
                    if (lb2 != null) lb2.Text = String.IsNullOrEmpty(c2) ? nodata : c2;
                    /****/
                    break;
                default:   //3 
                    c1 = Base.getValue(wtdr, "Czas1");
                    //lbWorktimeAll.Text = String.Format("{0} ({1})", algPar, String.IsNullOrEmpty(c1) ? Base.getValue(wtdr, "Czas2") : c1);
                    lbWorktimeAll.Text = String.IsNullOrEmpty(c1) ? nodata : String.Format("{0} ({1})", Base.getValue(wtdr, "Czas1R"), c1);
                    /****/
                    if (lb2 != null) lb2.Text = String.IsNullOrEmpty(c1) ? nodata : c1;
                    /****/
                    b = false;
                    break;
            }

            if (isZmiana)
            {
                int c3 = db.getInt(wtdr, "czasZm2", 0) +
                         db.getInt(wtdr, "nadgDzien2", 0) +
                         db.getInt(wtdr, "nadgNoc2", 0);
                if (lb2 != null) lb2.Text = Worktime.SecToTime(c3, 0);
                lbWStrefie.Text = String.Format("{0} ({1})",
                                                Worktime.SecToTimePP(c3, zaokr, zaokrType, false),
                                                Worktime.SecToTime(c3, 0));
            }
            else
            {
                lbWStrefie.Text = null;
            }
            /*
            drDay["czasZm2"]        = _czasZm;
            drDay["przerwaZm2"]     = przerwaZm;
            drDay["przerwaZm2nom"]  = przerwaZmNom;
            drDay["nadgDzien2"]     = nadgDzien;
            drDay["nadgNoc2"]       = nadgNoc;
            drDay["przerwaN2"]      = przerwaNadg;
            drDay["przerwaN2nom"]   = przerwaNadgNom;
            drDay["nocne2"]   
            */


            lbLacznieBezPrzerw.Visible = b;
            lbLacznieZPrzerwami.Visible = !b;

            if (przerWlicz > 0 || przerNiewlicz > 0)
            {
                string sep = przerWlicz > 0 && przerNiewlicz > 0 ? ", " : null;
                string przer = null;
                
                /*
                if (przerWlicz > 0)
                    przer = "+ " + Worktime.SecToTime(przerWlicz, 1) + " przerwa wliczona";
                if (przerNiewlicz > 0)
                {
                    if (!String.IsNullOrEmpty(przer)) przer += "<br />";
                    przer += "– " + Worktime.SecToTime(przerNiewlicz, 1) + " przerwa niewliczona";
                }
                */

                if (przerNiewlicz > 0)
                {
                    przer += "– " + Worktime.SecToTime(przerNiewlicz, 1) + " przerwa niewliczona";
                }
                if (przerWlicz > 0)
                {
                    if (!String.IsNullOrEmpty(przer)) przer += "<br />";
                    przer += "+ " + Worktime.SecToTime(przerWlicz, 1) + " przerwa wliczona";
                }

                lbPrzerwy.Text = przer;
                lbPrzerwy.Visible = true;
            }
            else
            {
                lbPrzerwy.Text = null;
                lbPrzerwy.Visible = false;
            }


            /*
            object timeIn = wtdr["TimeIn"];
            object timeOut = wtdr["TimeOut"];
            object zmOd = wtdr["ZmianaOd"];
            object zmDo = wtdr["ZmianaDo"];
            object czas1 = wtdr["Czas1sec"];
            object czas2 = wtdr["Czas2sec"];
            */
            object kCzasIn    = wtdr["CzasIn"];                    // wartości zatrzaśnięte
            object kCzasOut   = wtdr["CzasOut"];
            object kCzasZm    = wtdr["CzasZm"];
            object kNadgDzien = wtdr["NadgodzinyDzien"];
            object kNadgNoc   = wtdr["NadgodzinyNoc"];
            object kNocne     = wtdr["Nocne"];




            //zzz
            //object kWymiar = wtdr["Wymiar"];
            //object kWymiarKorekta = wtdr["WymiarKorekta"];





            /****/
            object kNadg50 = wtdr["n50"];
            object kNadg100 = wtdr["n100"];
            
            /****/

            double n50 = db.getDouble(wtdr, "n50", 0);
            double n100 = db.getDouble(wtdr, "n100", 0);




            bool k_CzasIn    = Base.getBool(wtdr, "k_CzasIn", false);
            bool k_CzasOut   = Base.getBool(wtdr, "k_CzasOut", false);
            bool k_CzasZm    = Base.getBool(wtdr, "k_CzasZm", false);
            bool k_NadgDzien = Base.getBool(wtdr, "k_NadgodzinyDzien", false);
            bool k_NadgNoc   = Base.getBool(wtdr, "k_NadgodzinyNoc", false);
            bool k_Nocne     = Base.getBool(wtdr, "k_Nocne", false);
            bool acc = Base.getBool(wtdr, "Akceptacja", false);
            bool exists = !Base.isNull(wtdr, "PPId");

            DateTime today = DateTime.Today;


            /****/
            /*bool k_Nadg50 = Base.getBool(wtdr, "k_Nadgodziny50", false);
            bool k_Nadg100 = Base.getBool(wtdr, "k_Nadgodziny100", false);*/
            /****/



            //vvv


            int wtAlert = 0;
            //string wtime, ztime, otime, xtime;  // czas łaczny, czas zmiany, nadgodziny, nocne
            /*
            bool isWTime = Worktime.SolveWorktime(con, algRCP, Base.getValue(adr, "Parametr"), zmOd, zmDo,
                Int32.Parse(breakMM),
                Int32.Parse(marginMM),
                timeIn, timeOut, czas1, czas2,
                zaokr, out wtime, out ztime, out otime, out xtime, ref wtAlert);
            */
            int wtime, _ztime, otimeD, otimeN, _ntime;  // czas łaczny, czas zmiany, nadgodziny, nocne
            int rzt, rnD, rnN, rN;
            //int before6;

            
            

            bool isWTime = Worktime.SolveWorktime2(wtdr, _algRCP, algPar, 
                breakMM, breakMM2,
                marginMM, zaokr, zaokrType,
                false,
                out wtime, out _ztime, out otimeD, out otimeN, out _ntime,
                out rzt, out rnD, out rnN, out rN,
                //out before6,
                day < today, ref wtAlert);




            //if (accepted)       // 20140629 dodaję alerty zapisane w bazie związane z weryfikacją <<< zawsze
                wtAlert |= db.getInt(wtdr, "Alerty", 0) & Worktime.alMask2;

            
            
            /* jak probuje zaakceptowac to nie okreslona zmiana i jak brak czasu to jest to niejednoznaczne wiec nie daje
            if (isZmiana)
                lbWorktime.Text = _ztime != -1 ? Worktime.SecToTime(_ztime, zaokr) : nodata;
            else
                lbWorktime.Text = zerodata;
            */
            string _zerodata = isZmiana ? zerodata : nodata;
            lbWorktime.Text = _ztime != -1 ? Worktime.SecToTime(_ztime, zaokr) : nodata;    // brak danych
            lbNadgDzien.Text = otimeD > 0 ? Worktime.SecToTime(otimeD, zaokr) : _zerodata;   // brak
            lbNadgNoc.Text = otimeN > 0 ? Worktime.SecToTime(otimeN, zaokr) : _zerodata;
            lbNocne.Text = _ntime > 0 ? Worktime.SecToTime(_ntime, zaokr) : _zerodata;





            

            /* dkftw */
            //if (String.IsNullOrEmpty(kzmid)) lbZmiana0.Text = planZm + plan;
            //else lbZmiana0.Text = GetZmiana(kzmid) + korekta;

            int _n50 = 0;
            int _n100 = 0;
            PlanPracy.TimeSumator ts = null;
            string _zmianaId = (String.IsNullOrEmpty(_kzmid)) ? planZmId : _kzmid;

            DateTime? _czasIn0 = Base.getDateTime(wtdr, "TimeIn");

            bool k_CzasIn0 = Base.getBool(wtdr, "k_CzasIn", false);
            DateTime? vCzasIn0 = Base.getDateTime(wtdr, "CzasIn");

            DateTime? kin0 = k_CzasIn0 ? (DateTime?)vCzasIn0 : _czasIn0;

            int d50, d100;
            bool zeroZm;
            PlanPracy.SolveNadgodziny(ref ts, (DateTime)Tools.StrToDateTime(Data), _zmianaId,
                db.getInt(wtdr, "czasZm2", 0),
                db.getInt(wtdr, "nadgDzien2", 0),
                db.getInt(wtdr, "nadgNoc2", 0),
                db.getInt(wtdr, "nocne2", 0),
                out _n50, out _n100,
                false,
                kin0,
                out d50, out d100, out zeroZm);

            //lb500.Text = _n50 > 0 ? Worktime.SecToTime(_n50, zaokr) : _zerodata;
            //lb1000.Text = _n100 > 0 ? Worktime.SecToTime(_n100, zaokr) : _zerodata;
            string sn50  = _n50  > 0 ? Worktime.SecToTime(_n50, zaokr) : _zerodata;
            string sn100 = _n100 > 0 ? Worktime.SecToTime(_n100, zaokr) : _zerodata;
            cntRcpAnalize.nadg50  = sn50;
            cntRcpAnalize.nadg100 = sn100;


            /*lbNadg50.Text = _n50 > 0 ? Worktime.SecToTime(_n50, zaokr) : _zerodata;
            lbNadg100.Text = _n100 > 0 ? Worktime.SecToTime(_n100, zaokr) : _zerodata;*/

            /****/


            ViewState["nadgNocNaWniosek"] = db.getInt(wtdr, "nadgNoc2", 0);

            if (n50 > 0) ViewState["nadg50NaWniosek"] =  Convert.ToInt32(n50);
            else ViewState["nadg50NaWniosek"] = _n50;
            if (n100 > 0) ViewState["nadg100NaWniosek"] = Convert.ToInt32(n100);
            else ViewState["nadg100NaWniosek"] = _n100;
            








            //----- formaty -----
            string ftime = "H:mm";
            string ftimeF = "(hh:mm)";
            string fsec;
            string fsecF;
            if (settings.Zaokr == 0)
            {
                fsec = "H:mm:ss";
                fsecF = "(hh:mm:ss)";
            }
            else if (settings.Zaokr >= 60)
            {
                fsec = "H";
                fsecF = "(hh)";
            }
            else
            {
                fsec = "H:mm";
                fsecF = "(hh:mm)";
            }
            //----- edycja -----
            lbTimeInVal.Visible    = ReadOnly;
            lbTimeOutVal.Visible   = ReadOnly;
            lbWorktimeVal.Visible  = ReadOnly;
            lbNadgDzienVal.Visible = ReadOnly;
            lbNadgNocVal.Visible   = ReadOnly;
            lbNocneVal.Visible     = ReadOnly;
            lbUwagi.Visible        = ReadOnly;
            teTimeIn.Visible       = !ReadOnly;
            teTimeOut.Visible      = !ReadOnly;
            teWorktime.Visible     = !ReadOnly;
            teNadgDzien.Visible    = !ReadOnly;
            teNadgNoc.Visible      = !ReadOnly;
            teNocne.Visible        = !ReadOnly;
            tbUwagi.Visible        = !ReadOnly;

            /*lbNadg50Val.Visible    = ReadOnly;
            lbNadg100Val.Visible   = ReadOnly;
            teNadg50.Visible       = !ReadOnly;
            teNadg100.Visible      = !ReadOnly;*/

            /*
            lbWymiarVal.Visible = ReadOnly;
            teWymiar.Visible = !ReadOnly;
            lbWymiar.Text = TimeEdit.SecToTimeStr(db.getInt(wtdr, "Wymiar", 0), "H:mm");
            */

            Color cs = Color.Silver;
            Color ck = Color.Black;
            if (ReadOnly)
            {
                lbZmienNa.Text = "Dane zaakceptowane";
                lbTimeInVal.Text    = k_CzasIn  ? GetTime(kCzasIn,  ftime) + korekta : GetTimeRCP(kCzasIn); //Worktime.SecToTime(kTimeIn, 0);
                lbTimeOutVal.Text   = k_CzasOut ? GetTime(kCzasOut, ftime) + korekta : GetTimeRCP(kCzasOut);
                lbWorktimeVal.Text  = GetTimeSec(kCzasZm,    fsec, nodata)   + (k_CzasZm    ? korekta : null);
                lbNadgDzienVal.Text = GetTimeSec(kNadgDzien, fsec, zerodata) + (k_NadgDzien ? korekta : null);
                lbNadgNocVal.Text   = GetTimeSec(kNadgNoc,   fsec, zerodata) + (k_NadgNoc   ? korekta : null);
                lbNocneVal.Text     = GetTimeSec(kNocne,     fsec, zerodata) + (k_Nocne     ? korekta : null);

                /*
                lbWymiarVal.Text = GetTimeSec(kWymiarKorekta, fsec, GetTimeSec(kWymiar, fsec, zerodata));
                */

                /*lbNadg50Val.Text    = GetTimeSec(kNadg50,    fsec, zerodata) + (k_Nadg50    ? korekta : null);
                lbNadg100Val.Text   = GetTimeSec(kNadg100,   fsec, zerodata) + (k_Nadg100   ? korekta : null);*/

                lbUwagi.Text = Base.getValue(wtdr, "Uwagi");
            }
            else
            {
                lbZmienNa.Text = "Zmień na ...";
                teTimeIn.Format     = ftime;
                teTimeOut.Format    = ftime;
                teWorktime.Format   = fsec;
                teNadgDzien.Format  = fsec;
                teNadgNoc.Format    = fsec;

                /*
                teWymiar.Format     = fsec;
                */

                /*teNadg50.Format = fsec;
                teNadg100.Format = fsec;*/

                teNocne.Format      = fsec;
                teTimeIn.Opis       = ftimeF;
                teTimeOut.Opis      = ftimeF;
                teWorktime.Opis     = fsecF;
                teNadgDzien.Opis    = fsecF;
                teNadgNoc.Opis      = fsecF;

                /*
                teWymiar.Opis       = fsecF;
                */

                /*teNadg50.Opis       = fsecF;
                teNadg100.Opis      = fsecF;*/

                teNocne.Opis        = fsecF;
                teTimeIn.TimeStr    = k_CzasIn    ? GetTime(kCzasIn,  ftime) : null;
                teTimeOut.TimeStr   = k_CzasOut   ? GetTime(kCzasOut, ftime) : null;
                teWorktime.TimeStr  = k_CzasZm    ? GetTimeSec(kCzasZm,    fsec, null) : null;
                teNadgDzien.TimeStr = k_NadgDzien ? GetTimeSec(kNadgDzien, fsec, null) : null;
                teNadgNoc.TimeStr   = k_NadgNoc   ? GetTimeSec(kNadgNoc,   fsec, null) : null;
                teNocne.TimeStr     = k_Nocne     ? GetTimeSec(kNocne,     fsec, null) : null;
                tbUwagi.Text = Base.getValue(wtdr, "Uwagi");

                /*teNadg50.TimeStr = k_Nadg50 ? GetTimeSec(kNadg50, fsec, null) : null;
                teNadg100.TimeStr = k_Nadg100 ? GetTimeSec(kNadg100, fsec, null) : null;*/

                /* * * * * * * * * * * * * * * *
                 * 
                 *  lbWymiar.Text = TimeEdit.SecToTimeStr(db.getInt(wtdr, "Wymiar", 0), "H:mm");
                 *  teWymiar.Seconds = db.getInt(wtdr, "WymiarKorekta", 0);
                 *  lbWymiarVal.Text = TimeEdit.SecToTimeStr(db.getInt(wtdr, "Wymiar", 0), "H:mm");
                 *  
                 * * * * * * * * * * * * * * * */

                //teWymiar.TimeStr = /*(wtdr["WymiarKorekta"] == null) ? null :*/ GetTimeSec(kWymiarKorekta, fsec, null);
            }

            Color cc = ReadOnly ? cs : ck;
            lbTimeIn.ForeColor      = cc;
            lbTimeOut.ForeColor     = cc;
            lbWorktimeAll.ForeColor = cc;
            lbWorktime.ForeColor    = cc;
            lbNadgDzien.ForeColor   = cc;
            lbNadgNoc.ForeColor     = cc;
            lbNocne.ForeColor       = cc;

            /*lbNadg50.ForeColor = cc;
            lbNadg100.ForeColor = cc;*/

            //----- absencja -----
            bool isAbsencja = false;
            string aid = Base.getValue(wtdr, "AbsencjaKod");    // absencja z KP
            lbAbsencja.CssClass = null;
            //----- absencja kier ----- //20130429
            string _kaid = Base.getValue(wtdr, "AbsencjaKodKier");
            string waid = Base.getValue(wtdr, "AbsencjaKodWniosek");
            if (ddlAbsencja.Items.Count == 0)
            {
                DataSet ads = db.getDataSet("select Kod, ISNULL(Symbol, 'X') + ' - ' + Nazwa as Nazwa from AbsencjaKody where Widoczny = 1 order by Symbol, Nazwa");
                Tools.BindData(ddlAbsencja, ads, "Nazwa", "Kod", true, _kaid);
            }
            else
                Tools.SelectItem(ddlAbsencja, _kaid);
            //-----             
            if (String.IsNullOrEmpty(aid))                      // nie ma absencji z KP                 jezeli okres jest zamkniety to biore z KP
            {
                aid = _kaid; // Base.getValue(wtdr, "AbsencjaKodKier");
                if (ReadOnly)
                {
                    if (!String.IsNullOrEmpty(aid))
                    {
                        lbAbsencja.Text = GetAbsencjaNazwa(aid);
                        lbAbsencja.Visible = true;
                        if (!Lic.kierAbs)
                            lbAbsencja.CssClass = "absencja";  // przekreślenie
                    }
                    else
                        lbAbsencja.Visible = false;
                    ddlAbsencja.Visible = false;
                    isAbsencja = false;
                }
                else
                {
                    isAbsencja = !String.IsNullOrEmpty(aid);
                    lbAbsencja.Visible = false;
                    ddlAbsencja.Visible = true;
                }

                /* 20130429
                if (closed)                                     // jeżeli miesiąc zamkniety a nie ma absencji z KP to nie pokazuję absencji Kier
                {
                    if (!String.IsNullOrEmpty(aid))
                    {
                        lbAbsencja.Text = GetAbsencjaNazwa(con, aid);
                        lbAbsencja.Visible = true;
                        lbAbsencja.CssClass = "absencja";
                    }
                    else
                        lbAbsencja.Visible = false;
                    ddlAbsencja.Visible = false;
                    isAbsencja = false;
                }
                else
                {
                    isAbsencja = !String.IsNullOrEmpty(aid); 
                    lbAbsencja.Visible = ReadOnly;
                    ddlAbsencja.Visible = !ReadOnly;
                    if (ReadOnly)
                    {
                        lbAbsencja.Text = GetAbsencjaNazwa(con, aid);
                    }
                    else
                    {
                        // 20130429
                        //DataSet ads = Base.getDataSet(con, "select Kod, ISNULL(Symbol, 'X') + ' - ' + Nazwa as Nazwa from AbsencjaKody where Widoczny = 1 order by Symbol, Nazwa");
                        //Tools.BindData(ddlAbsencja, ads, "Nazwa", "Kod", true, aid);
                    }
                }
                */ 
            }
            else                                                // jest absencja Z KP - nie wyswietlam absencji do edycji
            {
                lbAbsencja.Visible = true;
                ddlAbsencja.Visible = false;
                lbAbsencja.Text = GetAbsencjaNazwa(aid);
                isAbsencja = true;
            }
            //------ alerty ------
            /*  póki co poźniej wypada dać osobne alerty po zmianach Kierownika
            if (!isWTime) isWTime = !kCzasZm.Equals(DBNull.Value);
            if (DateTime.Parse(Data) < today) // tylko wczesniejsze i && miesiac nie zamknięty - spr. do 6 rano ...
            {
                if (isZmiana && !isWTime && !isAbsencja) wtAlert |= Worktime.alNoWork;
                if (isWTime && !isZmiana) wtAlert |= Worktime.alNoShift;
                if (isWTime && isAbsencja) wtAlert |= Worktime.alWorkAbsence;
            }
            */
            //----- alert - spr niezgodności zatrzasniete/rcp - na podstawie Okres.cs, można kiedyś uwspólnić ------
            bool lockError = !closed && (Base.getInt(wtdr, "Alerty", 0) & Worktime.alLockError) != 0;   // tylko w otwatym okresie
            if (!closed)    // dla okresu zamkniętego nie ma sensu już tego sprawdzać
            {
                if (lockError)               //jest zapisany alert od róznicy wartości; ale jeszcze raz weryfikacja
                {
                    DateTime? vCzasIn = !Base.isNull(kCzasIn) ? (DateTime?)kCzasIn : null;          // trochę to przekombinowane ...
                    DateTime? vCzasOut = !Base.isNull(kCzasOut) ? (DateTime?)kCzasOut : null;
                    int vCzas = Base.getInt(wtdr, "Czas", 0);                 // dane z RCP
                    int vCzasZm = !Base.isNull(kCzasZm) ? (int)kCzasZm : -1;
                    int vNadgDzien = !Base.isNull(kNadgDzien) ? (int)kNadgDzien : 0;
                    int vNadgNoc = !Base.isNull(kNadgNoc) ? (int)kNadgNoc : 0;
                    int vNocne = !Base.isNull(kNocne) ? (int)kNocne : 0;

                    bool f0 = vCzas != wtime;
                    bool f1 = !k_CzasIn && vCzasIn != tin2;
                    bool f2 = !k_CzasOut && vCzasOut != tout2;
                    bool f3 = !k_CzasZm && vCzasZm != _ztime;
                    bool f4 = !k_NadgDzien && vNadgDzien != otimeD;
                    bool f5 = !k_NadgNoc && vNadgNoc != otimeN;
                    bool f6 = !k_Nocne && vNocne != _ntime;

                    lockError = f0 || f1 || f2 || f3 || f4 || f5 || f6;     // nadpisuję - na szybko jest to do przyjecia, jak z weryfikacji wyjdzie ze wszystko ok to nie wyswietlam wartosci poprzednich i alertu 
                    //----- wartości poprzednie -----
                    const string bylo = "&nbsp;było: ";
                    if (lockError && !acc)
                    {
                        if (f0)
                        {
                            string czas; 
                            int? c1 = Base.getInt(wtdr, "Czas");
                            if (c1 == null) 
                                czas = nodata;
                            else 
                                czas = String.Format("{0} ({1})", 
                                            Worktime.SecToTimePP((int)c1, zaokr, zaokrType, false),
                                            Worktime.SecToTime((int)c1, 0));
                            lbWorktimeAllVal.Text = bylo + czas;
                            lbWorktimeAllVal.Visible = true;
                        }
                        if (f1)
                        {
                            string tin = GetTimeRCP(kCzasIn);
                            lbTimeInVal.Text = bylo + (String.IsNullOrEmpty(tin) ? nodata : tin);
                            lbTimeInVal.Visible = true;
                        }
                        if (f2)
                        {
                            string tout = GetTimeRCP(kCzasOut);
                            lbTimeOutVal.Text = bylo + (String.IsNullOrEmpty(tout) ? nodata : tout);
                            lbTimeOutVal.Visible = true;
                        }                            
                        if (f3)
                        {
                            lbWorktimeVal.Text = bylo + GetTimeSec(kCzasZm, fsec, nodata);
                            lbWorktimeVal.Visible = true;
                        }
                        if (f4)
                        {
                            lbNadgDzienVal.Text = bylo + GetTimeSec(kNadgDzien, fsec, zerodata);
                            lbNadgDzienVal.Visible = true;
                        }
                        if (f5)
                        {
                            lbNadgNocVal.Text = bylo + GetTimeSec(kNadgNoc, fsec, zerodata);
                            lbNadgNocVal.Visible = true;
                        }
                        if (f6)
                        {
                            lbNocneVal.Text = bylo + GetTimeSec(kNocne, fsec, zerodata);
                            lbNocneVal.Visible = true;
                        }
                    }
                    if (lockError) wtAlert |= Worktime.alLockError;
                }
            }    
            //----- alerty modyfikacji kierownika -----
            /*
            int? wt = k_CzasZm ? (int?)kCzasZm : null;
            int? otD = k_NadgDzien ? (int?)kNadgDzien : null;
            int? otN = k_NadgNoc ? (int?)kNadgNoc : null;
            int? nt = k_Nocne ? (int?)kNocne : null;
            wtAlert |= Worktime.GetKierAlert(wtdr, wt, otD, otN, nt);   // sprawdzał tylko wartości wprowadzone przez kier a trzeba wszystko czy się zgadza
            */            
            int? wt = k_CzasZm ? (int?)kCzasZm : _ztime;
            int? otD = k_NadgDzien ? (int?)kNadgDzien : otimeD;
            int? otN = k_NadgNoc ? (int?)kNadgNoc : otimeN;
            int? nt = k_Nocne ? (int?)kNocne : _ntime;
            DateTime? kin = k_CzasIn ? (DateTime?)kCzasIn : tin2;
            DateTime? kout = k_CzasOut ? (DateTime?)kCzasOut : tout2;



            wtAlert |= Worktime.GetKierAlert(PracId, Data, wtdr, kin, kout, wt, otD, otN, nt);
            


            //hidAlerty.Value = wtAlert.ToString();
            Alerty = wtAlert;

            string ppid = Base.getValue(wtdr, "PPId");
            hidPPId.Value = ppid;
            //--------------------
            //bool mpk = cntMPK.Prepare(ppid, Data, wt, otD, otN, nt, ReadOnly);
            /*bool mpk = cntMPK.Prepare(ppid, PracId, Data, wt, otD, otN, nt, ReadOnly);*/
            bool mpk = cntMPK.Prepare(ppid, PracId, Data, wt, otD, otN, nt, ReadOnly, wt2.GetRcpAnalize());

            MenuItem m = Tools.GetMenuItem(tabAccept, tabPodzialMPK);
            if (m != null)
                if (ReadOnly && !mpk)
                {
                    m.Text = "Podział kosztów - brak";
                    m.Enabled = false;
                }
                else
                {
                    m.Text = "Podział kosztów";
                    m.Enabled = true;
                }
            //-----
            if (klasNadg)
            {
                bool pnad = cntPrzeznaczNadg.Prepare(ppid, PracId,
                                                     Data, hidFrom.Value, hidTo.Value,
                                                     wt, otD, otN, nt, ReadOnly);
                m = Tools.GetMenuItem(tabAccept, tabRozlNadg);
                if (m != null)                    
                    if (ReadOnly && !pnad)
                    {
                        m.Text = "Rozliczenie nadgodzin - brak";
                        m.Enabled = false;
                    }
                    else
                    {
                        m.Text = "Rozliczenie nadgodzin";
                        m.Enabled = true;
                    }
            }
            //-----
            //tabAccept.Items[0].Selected = true;
            Tools.SelectMenuFromSession(tabAccept, ses_active_tab, tabCzasPracy);
            SelectTab();
            //--------------------
            cntRcp.Prepare(PracId, rcpId, fromTime, toTime, _strefaId);
            //--------------------
            bool ja = PracId == App.User.Id;                               // tylko do wykrycia czy to jest pierwsza linia pracowników - ja > pokaż dane kierownika
                    //|| PracId == App.User.OriginalId;                     // to samo co jaNoEdit ale tu do czego innego służy - do odblokowywania, nie wiem czy to uwspólniać <<< nie

            //btAccept.Visible = !accepted && !closed && dayAccepted;
            //btAccept.Visible = !accepted && !closed && day < today;     // we wszystkie dni do wczoraj można zaakceptować od razu; akceptacja zatrzaśnie wartości

            bool a = ((!isInnyKier && !jaNoEdit) || user.IsAdmin) &&
                      !accepted && 
                      //(!closed || ok._Current()) &&             // zawsze jezeli nie zaakceptowane to trzeba zaakceptować
                      day < today;     // we wszystkie dni do wczoraj można zaakceptować od razu; akceptacja zatrzaśnie wartości; current - jezeli zostały odblokowane przez admina
            btAccept.Visible = a;
            btAccept2.Visible = a;
            lbInfoJa.Visible = !a && !closed && jaNoEdit;


            //btSaveAcc.Visible = !ReadOnly;                            // ReadOnly = closed || accepted
            btSaveAcc.Visible = !ReadOnly && !lockError;                // na szybko takie rozwiązanie - nie ma klawisza zapisz, mozna tylko zaakceptować
            btCloseAcc.Visible = !ReadOnly;
            //btUnlock.Visible = accepted && !closed;
            //btUnlock.Visible = accepted && (!closed || user.IsAdmin || (user.HasRight(AppUser.rUnlockPP) && ok.Status != Okres.stClosed && !_ja));  //uprawnienie do odlokowywania dni w otwartym okresie rozliczeniowym przy zamknietym tygodniu
            btUnlock.Visible = accepted && ((!isInnyKier && !jaNoEdit) || user.IsAdmin) && 
                              (!closed || user.IsAdmin || (user.HasRight(AppUser.rUnlockPP) && ok.Status != Okres.stClosed && !ja));  //uprawnienie do odlokowywania dni w otwartym okresie rozliczeniowym przy zamknietym tygodniu
            btCloseAcc1.Visible = ReadOnly;

#if RCP && false
            btWniosekNadg.Visible = a && App.User.HasRight(AppUser.rRozlNadg);
#endif
            //--------------------
            //Base.Disconnect(con);
        }

        private void RenderAlerty()
        {
            //int acode = Tools.StrToInt(hidAlerty.Value, 0);
            int acode = Alerty;
            if (acode == 0)
            {
                ltAlerty.Text = "Brak alertów";
                Tools.RemoveClass(tdAlerty, "alerty");
            }
            else
            {
                ltAlerty.Text = "<ul class=\"alerty\"><li>";
                ltAlerty.Text += String.Join("</li><li>", Worktime.GetAlertMsg(acode).ToArray());
                ltAlerty.Text += "</li></ul>";
                Tools.AddClass(tdAlerty, "alerty");
            }
        }
        //-----------------
        public bool Validate()
        {
            bool v1 = teTimeIn.Validate();
            bool v2 = teTimeOut.Validate();
            bool v3 = teWorktime.Validate();
            bool v4 = teNadgDzien.Validate();
            bool v5 = teNadgNoc.Validate();
            bool v6 = teNocne.Validate();

            /****/
            /*bool v7 = teNadg50.Validate();
            bool v8 = teNadg100.Validate();*/
            /****/

            if (v1 && v2 && v3 && v4 && v5 && v6 /*&& v7 && v8*/)
                return true;
            else
            {
                //Tools.ShowMessage("Błąd formatu czasu pracy.");
                return false;
            }
        }

        private string kRound(object sec, int round, int rtype, ref bool rounded, out int s)
        {
            s = 0;
            if (sec != null)
            {
                s = Worktime.RoundSec((int)sec, round, rtype);
                if (s != (int)sec) rounded = true;
                return s.ToString();
            }
            return Base.NULL;
        }


        private void UnlockAcc()
        {
            string ppId = hidPPId.Value;
            bool c = Closed;   // tylko admin bedzie miec wówczas prawo odblokowania, nie aktualizauje PP, tylko zapisuje do logu
            bool b = Okres._CofnijAcc(null, ppId, 0, user.OriginalId, !c, c, null);
        }
        /*
        public void UpdateAcc(bool acc)
        {
            string ppId = hidPPId.Value;
            bool b = Base.execSQL(Base.updateSql("PlanPracy", 1,
                "Akceptacja, IdKierownikaAcc, DataAcc",
                "Id={0}",
                ppId,
                acc ? "1" : "0",
                user.Id, "GETDATE()"
                ));     // true jesli dodał
        }
        */
        public bool Update(bool accepted, bool accepted2)  
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            string ppId = hidPPId.Value;
            string _tIn, tOut;
            _PrepareInOut(Data, out _tIn, out tOut);

            bool rounded = false;
            int zm, nd, nn, noc;            
            string szm = kRound(teWorktime.Seconds, settings.Zaokr, settings.ZaokrType, ref rounded, out zm);
            string snd = kRound(teNadgDzien.Seconds, settings.Zaokr, settings.ZaokrType, ref rounded, out nd);
            string snn = kRound(teNadgNoc.Seconds, settings.Zaokr, settings.ZaokrType, ref rounded, out nn);
            string snoc = kRound(teNocne.Seconds, settings.Zaokr, settings.ZaokrType, ref rounded, out noc);

            /* pobieranie z timeeditow w swietnej nowej wersji */
            /*int __n50, __n100;
            string s__n50 = kRound(teNadg50.Seconds, settings.Zaokr, settings.ZaokrType, ref rounded, out __n50);
            string s__n100 = kRound(teNadg100.Seconds, settings.Zaokr, settings.ZaokrType, ref rounded, out __n100);*/

            //----- 50 i 100 -----
            int n50 = 0;    // będzie naliczane przy akceptacji, póki co zeruję
            int n100 = 0;
            /*
            int zm = Worktime.RoundSec(db.ISNULL(teWorktime.Seconds, 0), settings.Zaokr, settings.ZaokrType);
            int nd = Worktime.RoundSec(db.ISNULL(teNadgDzien.Seconds, 0), settings.Zaokr, settings.ZaokrType);
            int nn = Worktime.RoundSec(db.ISNULL(teNadgNoc.Seconds, 0), settings.Zaokr, settings.ZaokrType);
            int noc = Worktime.RoundSec(db.ISNULL(teNocne.Seconds, 0), settings.Zaokr, settings.ZaokrType);
            PlanPracy.TimeSumator ts = null;
            string zmianaId = ddlZmiana.SelectedValue;
            PlanPracy.SolveNadgodziny(ref ts, (DateTime)Tools.StrToDateTime(Data), zmianaId, zm, nd, nn, noc, out n50, out n100); 
            */

            string selzm = Tools.GetLineParam(ddlZmiana.SelectedValue, 0);
            string selwym = Tools.GetLineParam(ddlZmiana.SelectedValue, 1);

            if (String.IsNullOrEmpty(IdZmianyPlan) && selzm == "-1" || IdZmianyPlan == selzm && WymiarPlan == selwym)
            {
                selzm = db.NULL;
                selwym = db.NULL;
            }


            if (!String.IsNullOrEmpty(ppId))
            {
                bool b;
                b = Base.execSQL(Base.updateSql2("PlanPracy", 1,
                    "IdKierownikaAcc, DataAcc, " +
                    "IdZmianyKorekta, WymiarKorekta, CzasIn, CzasOut, CzasZm, NadgodzinyDzien, NadgodzinyNoc, Nocne, Absencja, k_CzasIn, k_CzasOut, k_CzasZm, k_NadgodzinyDzien, k_NadgodzinyNoc, k_Nocne, Uwagi, n50, n100, Alerty",
                    //"CzasIn, CzasOut, CzasZm, Nadgodziny, Nocne, Absencja, k_CzasIn, k_CzasOut, k_CzasZm, k_Nadgodziny, k_Nocne, Uwagi",
                    //asAccepted ? ",Akceptacja=1" : null, 
                    ",Akceptacja=0",
                    "Id={0}",
                    ppId,
                    user.OriginalId, "GETDATE()",   //data modyfikacji jesli nie zaakceptowany
                    //Base.nullParam(ddlZmiana.SelectedValue),
                    //Base.nullParam(teWymiar.Seconds),
                    Base.nullParam(selzm),
                    Base.nullParam(selwym),
                    _tIn, tOut,
                    szm, snd, snn, snoc,
                    //kRound(teWorktime.Seconds, settings.Zaokr, settings.ZaokrType, ref rounded),
                    //kRound(teNadgDzien.Seconds, settings.Zaokr, settings.ZaokrType, ref rounded),
                    //kRound(teNadgNoc.Seconds, settings.Zaokr, settings.ZaokrType, ref rounded),
                    //kRound(teNocne.Seconds, settings.Zaokr, settings.ZaokrType, ref rounded),
                    Base.nullParam(ddlAbsencja.SelectedValue),
                    teTimeIn.IsEntered01,
                    teTimeOut.IsEntered01,
                    teWorktime.IsEntered01,
                    teNadgDzien.IsEntered01,
                    teNadgNoc.IsEntered01,
                    teNocne.IsEntered01,
                    /*teNadg50.IsEntered01,
                    teNadg100.IsEntered01,*/
                    Base.strParam(Base.sqlPut(tbUwagi.Text.TrimEnd(), 200)),
                    n50,
                    n100,
                    Alerty
                ));     // true jesli dodał

                /*
                if (accepted)
                else  // aktualizuje tylko pola - jak jest niezgodnosc danych rcp to powinien przy zapisie nie nadpisywać a to mozna zrobic albo przechowując w ViewState czy dana wartość była z ręki wprowadzona a teraz jest pusto i wtedy zapisać wartość z rcp albo wyłączyć mozliwosć zapisu - będzie tylko akceptacja 
                {
                }
                */
            }
            else
            {
                int id = Base.insertSQL(Base.insertSql2("PlanPracy", 0,
                    "IdPracownika, Data, " +
                    "IdKierownikaAcc, DataAcc, " +
                    "IdZmianyKorekta, WymiarKorekta, CzasIn, CzasOut, CzasZm, NadgodzinyDzien, NadgodzinyNoc, Nocne, Absencja, k_CzasIn, k_CzasOut, k_CzasZm, k_NadgodzinyDzien, k_NadgodzinyNoc, k_Nocne, Uwagi, n50, n100, Alerty",
                    //asAccepted ? ",Akceptacja" : null, asAccepted ? ",1" : null,
                    ",Akceptacja", ",0",
                    PracId, Base.strParam(Data),
                    user.OriginalId, "GETDATE()",
                    //Base.nullParam(ddlZmiana.SelectedValue),
                    //Base.nullParam(teWymiar.Seconds),
                    Base.nullParam(selzm),
                    Base.nullParam(selwym),
                    _tIn, tOut,
                    szm, snd, snn, snoc,
                    //kRound(teWorktime.Seconds, settings.Zaokr, settings.ZaokrType, ref rounded),
                    //kRound(teNadgDzien.Seconds, settings.Zaokr, settings.ZaokrType, ref rounded),
                    //kRound(teNadgNoc.Seconds, settings.Zaokr, settings.ZaokrType, ref rounded),
                    //kRound(teNocne.Seconds, settings.Zaokr, settings.ZaokrType, ref rounded),
                    Base.nullParam(ddlAbsencja.SelectedValue),  
                    teTimeIn.IsEntered01,
                    teTimeOut.IsEntered01,
                    teWorktime.IsEntered01,
                    teNadgDzien.IsEntered01,
                    teNadgNoc.IsEntered01,
                    teNocne.IsEntered01,
                    /*teNadg50.IsEntered01,
                    teNadg100.IsEntered01,*/
                    Base.strParam(Base.sqlPut(tbUwagi.Text.TrimEnd(), 200)),
                    n50,
                    n100,
                    Alerty
                ));
                hidPPId.Value = id.ToString();  // nie zamyka panelu jak alert i wchodził ponownie do inserta mimo ze miał zapisane
            }
            if (accepted)    // przełożony akceptuje   //vvv
            {
                int err = Okres.AkceptujOneDay(PracId, hidFrom.Value, hidTo.Value, Data, user.OriginalId, accepted2);  // zwraca kody alertów

                int err1 = err & ~Worktime.alMask2;     // błędy blokujące, jeżeli AkceptujOneDay pozwoli na zapis to 0 
                int err2 = err & Worktime.alMask2;      // błędy nieblokujące, wymagają potwierdzenia akceptacji

                if (err1 != 0)      // spr tylko blokujące
                {
                    string msg = "\\n- " + String.Join("\\n- ", Worktime.GetAlertMsg(err).ToArray());   // niech da wszystkie
                    Tools.ShowMessage("Nie można zaakceptować czasu pracy:" + msg);
                    return false;
                }
                else if (err2 != 0)  // nieprawidłowości do akceptacji
                {
                    string msg = "\\n- " + String.Join("\\n- ", Worktime.GetAlertMsg2(err2).ToArray());   // niech da wszystkie
                    if (accepted2)
                    {
                        string prac = AppUser.GetNazwiskoImieNREW(PracId);
                        Log.Info(Log.WTERRORACC, "Akceptacja nieprawidłowości w czasie pracy", String.Format("{0} {1}: {2}", PracId, prac, Data) + msg);
                    }
                    else
                    {
                        Tools.ShowConfirm("Uwaga!\\nZnaleziono nieprawidłowości w akceptowanym czasie pracy:" + msg + "\\n\\nCzy potwierdzasz akceptację?", btAccept2, null);
                        return false;
                    }
                }
            }
            if (rounded) Tools.ShowMessage("Uwaga!\\nWprowadzone czasy zostały zaokrąglone zgodnie z obowiązujacymi zasadami.");
            return true;
        }

        /*20120304
        public void Update(bool asAccepted)  
        {
            string ppId = hidPPId.Value;
            string tIn, tOut;
            PrepareInOut(Data, tbTimeIn.Text, tbTimeOut.Text, out tIn, out tOut);

            if (!String.IsNullOrEmpty(ppId))
            {
                bool b = Base.execSQL(Base.updateSql2("PlanPracy", 1,
                    "IdKierownikaAcc, DataAcc, " +
                    "CzasIn, CzasOut, CzasZm, Nadgodziny, Nocne, Absencja, k_CzasIn, k_CzasOut, k_CzasZm, k_Nadgodziny, k_Nocne, Uwagi",
                    asAccepted ? ",Akceptacja=1" : null, 
                    "Id={0}",
                    ppId,
                    user.Id, "GETDATE()",
                    tIn, tOut,
                    PrepareWorktime(tbWorktime.Text, null),
                    PrepareWorktime(tbOvertimes.Text, null),
                    PrepareWorktime(tbNocne.Text, null),
                    Base.nullParam(ddlAbsencja.SelectedValue),
                    String.IsNullOrEmpty(tbTimeIn.Text) ? "0" : "1",
                    String.IsNullOrEmpty(tbTimeOut.Text) ? "0" : "1",
                    String.IsNullOrEmpty(tbWorktime.Text) ? "0" : "1",
                    String.IsNullOrEmpty(tbOvertimes.Text) ? "0" : "1",
                    String.IsNullOrEmpty(tbNocne.Text) ? "0" : "1",
                    Base.strParam(Base.sqlPut(tbUwagi.Text.TrimEnd(), 200))
                ));     // true jesli dodał
            }
            else
                Base.execSQL(Base.insertSql2("PlanPracy", 0,
                    "IdPracownika, Data, " +
                    "IdKierownikaAcc, DataAcc, " +
                    "CzasIn, CzasOut, CzasZm, Nadgodziny, Nocne, Absencja, k_CzasIn, k_CzasOut, k_CzasZm, k_Nadgodziny, k_Nocne, Uwagi",
                    ",Akceptacja", ",1",
                    PracId, Base.strParam(Data),
                    user.Id, "GETDATE()",
                    tIn, tOut,
                    PrepareWorktime(tbWorktime.Text, null),
                    PrepareWorktime(tbOvertimes.Text, null),
                    PrepareWorktime(tbNocne.Text, null),
                    Base.nullParam(ddlAbsencja.SelectedValue),
                    String.IsNullOrEmpty(tbTimeIn.Text) ? "0" : "1",
                    String.IsNullOrEmpty(tbTimeOut.Text) ? "0" : "1",
                    String.IsNullOrEmpty(tbWorktime.Text) ? "0" : "1",
                    String.IsNullOrEmpty(tbOvertimes.Text) ? "0" : "1",
                    String.IsNullOrEmpty(tbNocne.Text) ? "0" : "1",
                    Base.strParam(Base.sqlPut(tbUwagi.Text.TrimEnd(), 200))
                ));
        }

         
         */

        /* stara
        public void Update()
        {
            string ppId = hidPPId.Value;
            string tIn, tOut;
            PrepareInOut(Data, tbTimeIn.Text, tbTimeOut.Text, out tIn, out tOut);


            bool k_CzasIn = !String.IsNullOrEmpty(tbTimeIn.Text);
            bool k_CzasOut = !String.IsNullOrEmpty(tbTimeOut.Text)
            bool k_CzasZm = !String.IsNullOrEmpty(tbWorktime.Text)
            bool k_Nadgodziny = !String.IsNullOrEmpty(tbOvertimes.Text)
            bool k_Nocne = !String.IsNullOrEmpty(tbNocne.Text)


            if (!String.IsNullOrEmpty(ppId))
            {
                string set = null;
                if (!k_CzasIn) set += "CzasIn=" + (czasIn == null ? Base.NULL : Base.strParam(Base.DateTimeToStr(czasIn))) + ",";
                if (!k_CzasOut) set += "CzasOut=" + (czasOut == null ? Base.NULL : Base.strParam(Base.DateTimeToStr(czasOut))) + ",";
                if (!k_CzasZm) set += "CzasZm=" + PrepareWorktime(tbWorktime.Text, null) + ",";
                if (!k_Nadgodziny) set += "Nadgodziny=" + PrepareWorktime(tbOvertimes.Text, null) + ",";
                if (!k_Nocne) set += "Nocne=" + PrepareWorktime(tbNocne.Text, null) + ",";

                bool b = Base.execSQL(con, "update PlanPracy set " + set +
                                    Base.updateParam("Czas", Base.nullParam(wt)) +
                                    Base.updateParam("IdKierownikaAcc", accKierId) +
                                    Base.updateParam("DataAcc", "GETDATE()") +
                                    Base.updateParamLast("Akceptacja", "1") +
                    "where IdPracownika = " + pracId + " and Data = " + Base.strParam(Base.DateToStr(date)));

                
                
                bool b = Base.execSQL(Base.updateSql("PlanPracy", 1,
                    "IdKierownikaAcc, DataAcc, " +
                    "CzasIn, CzasOut, CzasZm, Nadgodziny, Nocne, Absencja, k_CzasIn, k_CzasOut, k_CzasZm, k_Nadgodziny, k_Nocne, Uwagi",
                    "Id={0}",
                    ppId,
                    user.Id, "GETDATE()",
                    tIn, tOut,
                    PrepareWorktime(tbWorktime.Text, null),
                    PrepareWorktime(tbOvertimes.Text, null),
                    PrepareWorktime(tbNocne.Text, null),
                    Base.nullParam(ddlAbsencja.SelectedValue),
                    String.IsNullOrEmpty(tbTimeIn.Text) ? "0" : "1",
                    String.IsNullOrEmpty(tbTimeOut.Text) ? "0" : "1",
                    String.IsNullOrEmpty(tbWorktime.Text) ? "0" : "1",
                    String.IsNullOrEmpty(tbOvertimes.Text) ? "0" : "1",
                    String.IsNullOrEmpty(tbNocne.Text) ? "0" : "1",
                    Base.strParam(Base.sqlPut(tbUwagi.Text.TrimEnd(), 200))
                ));     // true jesli dodał
            }
            else
            {
                //----- insert -----
                string fields = null;
                string values = null;
                if (!k_CzasIn)
                {
                    fields += "CzasIn,";
                    values += (czasIn == null ? Base.NULL : Base.strParam(Base.DateTimeToStr(czasIn))) + ",";
                }
                if (!k_CzasOut)
                {
                    fields += "CzasOut,";
                    values += (czasOut == null ? Base.NULL : Base.strParam(Base.DateTimeToStr(czasOut))) + ",";
                }
                if (!k_CzasZm)
                {
                    fields += "CzasZm,";
                    values += (zt == null ? Base.NULL : zt.ToString()) + ",";
                }
                if (!k_Nadgodziny)
                {
                    fields += "Nadgodziny,";
                    values += (ot == null ? Base.NULL : ot.ToString()) + ",";
                }
                if (!k_Nocne)
                {
                    fields += "Nocne,";
                    values += (nt == null ? Base.NULL : nt.ToString()) + ",";
                }
                Base.execSQL(con, "insert into PlanPracy (" +
                              fields + "IdPracownika,Data,Czas,IdKierownikaAcc,DataAcc,Akceptacja) values (" +
                              values +
                                Base.insertParam(pracId) +
                                Base.insertStrParam(Base.DateToStr(date)) +
                                Base.insertParam(Base.nullParam(wt)) +
                                Base.insertParam(accKierId) +
                                Base.insertParam("GETDATE()") +
                                Base.insertParamLast("1") + ")");

                Base.execSQL(Base.insertSql("PlanPracy", 0,
                    "IdPracownika, Data, " +
                    "IdKierownikaAcc, DataAcc, " +
                    "CzasIn, CzasOut, CzasZm, Nadgodziny, Nocne, Absencja, k_CzasIn, k_CzasOut, k_CzasZm, k_Nadgodziny, k_Nocne, Uwagi",
                    PracId, Base.strParam(Data),
                    user.Id, "GETDATE()",
                    tIn, tOut,
                    PrepareWorktime(tbWorktime.Text, null),
                    PrepareWorktime(tbOvertimes.Text, null),
                    PrepareWorktime(tbNocne.Text, null),
                    Base.nullParam(ddlAbsencja.SelectedValue),
                    String.IsNullOrEmpty(tbTimeIn.Text) ? "0" : "1",
                    String.IsNullOrEmpty(tbTimeOut.Text) ? "0" : "1",
                    String.IsNullOrEmpty(tbWorktime.Text) ? "0" : "1",
                    String.IsNullOrEmpty(tbOvertimes.Text) ? "0" : "1",
                    String.IsNullOrEmpty(tbNocne.Text) ? "0" : "1",
                    Base.strParam(Base.sqlPut(tbUwagi.Text.TrimEnd(), 200))
                    ));
            }
        }



        private static void UpdatePP(SqlConnection con, string pracId, DateTime date, DataRow dr,
                             DateTime? czasIn, DateTime? czasOut, int? wt, int? zt, int? ot, int? nt,
                             string accKierId)
        {
            bool k_CzasIn = Base.getBool(dr, "k_CzasIn", false);
            bool k_CzasOut = Base.getBool(dr, "k_CzasOut", false);
            bool k_CzasZm = Base.getBool(dr, "k_CzasZm", false);
            bool k_Nadgodziny = Base.getBool(dr, "k_Nadgodziny", false);
            bool k_Nocne = Base.getBool(dr, "k_Nocne", false);
            bool acc = Base.getBool(dr, "Akceptacja", false);
            bool exists = !Base.isNull(dr, "PPId");

            if (!exists)
            {
                //----- insert -----
                string fields = null;
                string values = null;
                if (!k_CzasIn)
                {
                    fields += "CzasIn,";
                    values += (czasIn == null ? Base.NULL : Base.strParam(Base.DateTimeToStr(czasIn))) + ",";
                }
                if (!k_CzasOut)
                {
                    fields += "CzasOut,";
                    values += (czasOut == null ? Base.NULL : Base.strParam(Base.DateTimeToStr(czasOut))) + ",";
                }
                if (!k_CzasZm)
                {
                    fields += "CzasZm,";
                    values += (zt == null ? Base.NULL : zt.ToString()) + ",";
                }
                if (!k_Nadgodziny)
                {
                    fields += "Nadgodziny,";
                    values += (ot == null ? Base.NULL : ot.ToString()) + ",";
                }
                if (!k_Nocne)
                {
                    fields += "Nocne,";
                    values += (nt == null ? Base.NULL : nt.ToString()) + ",";
                }
                Base.execSQL(con, "insert into PlanPracy (" +
                              fields + "IdPracownika,Data,Czas,IdKierownikaAcc,DataAcc,Akceptacja) values (" +
                              values +
                                Base.insertParam(pracId) +
                                Base.insertStrParam(Base.DateToStr(date)) +
                                Base.insertParam(Base.nullParam(wt)) +
                                Base.insertParam(accKierId) +
                                Base.insertParam("GETDATE()") +
                                Base.insertParamLast("1") + ")");
            }
            else
                if (!acc)  // akceptuję tylko jeżeli nie są jeszcze zaakaceptowane
                {
                    //----- update -----
                    string set = null;
                    if (!k_CzasIn) set += "CzasIn=" + (czasIn == null ? Base.NULL : Base.strParam(Base.DateTimeToStr(czasIn))) + ",";
                    if (!k_CzasOut) set += "CzasOut=" + (czasOut == null ? Base.NULL : Base.strParam(Base.DateTimeToStr(czasOut))) + ",";
                    if (!k_CzasZm) set += "CzasZm=" + (zt == null ? Base.NULL : zt.ToString()) + ",";
                    if (!k_Nadgodziny) set += "Nadgodziny=" + (ot == null ? Base.NULL : ot.ToString()) + ",";
                    if (!k_Nocne) set += "Nocne=" + (nt == null ? Base.NULL : nt.ToString()) + ",";

                    Base.execSQL(con, "update PlanPracy set " + set +
                                        Base.updateParam("Czas", Base.nullParam(wt)) +
                                        Base.updateParam("IdKierownikaAcc", accKierId) +
                                        Base.updateParam("DataAcc", "GETDATE()") +
                                        Base.updateParamLast("Akceptacja", "1") +
                        "where IdPracownika = " + pracId + " and Data = " + Base.strParam(Base.DateToStr(date)));
                }
        }
        */



        //-----------------
        private string TimeToDateTimeStr(string date, string time)
        {
            DateTime dt = DateTime.Parse(date);
            TimeSpan ts = TimeSpan.Parse(time);
            dt = dt.Add(ts);
            return Base.DateTimeToStr(dt);
        }

        private bool _PrepareInOut(string date, out string tIn, out string tOut)  // wartosci czasów muszą być po walidacji i poprawne !!!
        {
            tIn = Base.NULL;
            tOut = Base.NULL;
            bool ret = true; 
            bool fIn = teTimeIn.IsEntered;
            bool fOut = teTimeOut.IsEntered;
            if (fIn || fOut)
            {
                TimeSpan tsIn, tsOut;
                if (fIn)
                {
                    tsIn = teTimeIn.Time;
                    if (tsIn == TimeSpan.MinValue) ret = false;
                }
                else
                    if (lbTimeIn.Text != nodata)
                        TimeEdit.ToTimeSpan(lbTimeIn.Text, out tsIn); // jak błąd to MinValue
                    else tsIn = TimeSpan.MinValue;
                if (fOut)
                {
                    tsOut = teTimeOut.Time;
                    if (tsOut == TimeSpan.MinValue) ret = false;
                }
                else
                    if (lbTimeOut.Text != nodata)
                        TimeEdit.ToTimeSpan(lbTimeOut.Text, out tsOut); // jak błąd to MinValue
                    else tsOut = TimeSpan.MinValue;
                //----- in i out mam wprowadzone lub z rcp - spr przewinięcie -----
                if (tsIn != TimeSpan.MinValue && tsOut != TimeSpan.MinValue)
                {
                    DateTime dtIn = DateTime.Parse(date);
                    DateTime dtOut = tsIn < tsOut ? dtIn : dtIn.AddDays(1);    // zakładam czas pracy < 24h
                    if (fIn)
                        tIn = Base.strParam(Base.DateTimeToStrHHMM(dtIn.Add(tsIn)));
                    if (fOut)
                        tOut = Base.strParam(Base.DateTimeToStrHHMM(dtOut.Add(tsOut)));
                }
                else // któryś nie jest podany - przyjmuję datę dzisiejszą do obu
                {
                    DateTime dt = DateTime.Parse(date);
                    if (tsIn != TimeSpan.MinValue && fIn)
                        tIn = Base.strParam(Base.DateTimeToStrHHMM(dt.Add(tsIn)));
                    if (tsOut != TimeSpan.MinValue && fOut)
                        tOut = Base.strParam(Base.DateTimeToStrHHMM(dt.Add(tsOut)));
                }
            }
            return ret;
        }


        /*
        private void PrepareInOut(string date, out string tIn, out string tOut)  // wartosci czasów muszą być po walidacji i poprawne !!!
        {
            bool fIn = teTimeIn.IsEntered;
            bool fOut = teTimeOut.IsEntered;
            if (fIn && fOut)
            {
                TimeSpan tsIn = TimeSpan.Parse(teTimeIn.TimeStr);
                TimeSpan tsOut = TimeSpan.Parse(teTimeOut.TimeStr);
                DateTime dtIn = DateTime.Parse(date);
                DateTime dtOut;
                if (tsIn < tsOut)
                    dtOut = dtIn;
                else
                    dtOut = dtIn.AddDays(1);    // zakładam czas pracy < 24h
                tIn = Base.strParam(date + " " + teTimeIn.TimeStr);
                tOut = Base.strParam(Base.DateToStr(dtOut) + " " + teTimeOut.TimeStr);
            }
            else 
            {
                if (fIn) tIn = Base.strParam(date + " " + teTimeIn.TimeStr);
                else tIn = Base.NULL;
                if (fOut) tOut = Base.strParam(date + " " + teTimeOut.TimeStr);  // moze być data niepoprawna !!! - przewinięcie - uwzglednić zmienę !!!
                else tOut = Base.NULL;
            }
        }
         */

        /*
        private void PrepareInOut(string date, string timeIn, string timeOut, out string tIn, out string tOut)
        {
            bool fIn = !String.IsNullOrEmpty(timeIn);
            bool fOut = !String.IsNullOrEmpty(timeOut);
            if (fIn && fOut)
            {
                TimeSpan tsIn = TimeSpan.Parse(timeIn);
                TimeSpan tsOut = TimeSpan.Parse(timeOut);
                DateTime dtIn = DateTime.Parse(date);
                DateTime dtOut;
                if (tsIn < tsOut)
                    dtOut = dtIn;
                else
                    dtOut = dtIn.AddDays(1);    // zakładam czas pracy < 24h
                tIn = Base.strParam(date + " " + timeIn);
                tOut = Base.strParam(Base.DateToStr(dtOut) + " " + timeOut);
            }
            else 
            {
                if (fIn) tIn = Base.strParam(date + " " + timeIn);
                else tIn = Base.NULL;
                if (fOut) tOut = Base.strParam(date + " " + timeOut);  // moze być data niepoprawna !!! - przewinięcie - uwzglednić zmienę !!!
                else tOut = Base.NULL;
            }
        }
         */

        private string PrepareWorktime(string wt, string zaokr)  // zaokr na razie bez, wt - [h]
        {
            int hh;
            if (!String.IsNullOrEmpty(wt))
                if (Int32.TryParse(wt, out hh))
                    return (hh * 3600).ToString();
            return Base.NULL; 
        }

        private string PrepareWorktime_old(string wt, string zaokr)  // zaokr na razie bez
        {
            if (String.IsNullOrEmpty(wt)) return Base.NULL;
            else
            {
                int hh;
                TimeSpan ts;
                if (Int32.TryParse(wt, out hh))
                    ts = new TimeSpan(hh, 0, 0);
                else
                    ts = TimeSpan.Parse(wt);    // moze sie wywalic !!!
                return Base.strParam(ts.ToString());       // co z ustawieniami narodowymi ???
            }
        }

        //------------------------------------------------------------------
        private void SelectTab()
        {
            Tools.SelectTab(tabAccept, mvAccept, ses_active_tab, false);

            /*
            switch (tabAccept.SelectedValue)
            {
                case "0":
                    mvCzasPodzial.SetActiveView(pgCzasPracy);
                    break;
                case "1":
                    mvCzasPodzial.SetActiveView(pgKorekta);
                    break;
                case "2":
                    mvCzasPodzial.SetActiveView(pgPodzialKosztow);
                    break;
                case "3":
                    mvCzasPodzial.SetActiveView(pgNadgodziny);
                    break;
                case "4":
                    mvCzasPodzial.SetActiveView(pgWnioskiNadg);
                    break;
                case "5":
                    mvCzasPodzial.SetActiveView(pgDanePracownika);
                    break;
                case "6":
                    mvCzasPodzial.SetActiveView(pgRCP);
                    break;
            }
            */ 
        }
        
        protected void tabAccept_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectTab();
        }

        protected void pgPodzialKosztow_Activate(object sender, EventArgs e)
        {
            if (ReadOnly)
                cntMPK.PrepareReadOnly();
            else
                cntMPK.Prepare((int?)teWorktime.Seconds, (int?)teNadgDzien.Seconds, (int?)teNadgNoc.Seconds, (int?)teNocne.Seconds);
        }

        protected void pgNadgodziny_Activate(object sender, EventArgs e)
        {
            if (ReadOnly)
                cntPrzeznaczNadg.PrepareReadOnly();
            else
                cntPrzeznaczNadg.Prepare((int?)teWorktime.Seconds, (int?)teNadgDzien.Seconds, (int?)teNadgNoc.Seconds, (int?)teNocne.Seconds);
        }

        //-----------------
        public string PracId
        {
            get { return hidPracId.Value; }
            set { hidPracId.Value = value; }
        }

        public string Data
        {
            get { return hidData.Value; }
            set { hidData.Value = value; }
        }

        public int Alerty
        {
            set { hidAlerty.Value = value.ToString(); }
            get { return Tools.StrToInt(hidAlerty.Value, 0); }
        }
        
        public bool ReadOnly
        {
            get { return Tools.GetViewStateBool(ViewState["readonly"], true); }
            set { ViewState["readonly"] = value; }
        }

        /*
        public int ViewAs
        {
            get { return Tools.GetInt(ViewState["viewas"], PlanPracy.asKier); }
            set { ViewState["viewas"] = value; }
        }
        */

        public string KierId
        {
            get { return Tools.GetStr(ViewState["kierid"]); }
            set { ViewState["kierid"] = value; }
        }

        public bool Closed
        {
            get { return Tools.GetViewStateBool(ViewState["closed"], true); }
            set { ViewState["closed"] = value; }
        }

        public int Adm
        {
            set { FAdm = value; }
            get { return FAdm; }
        }

        public string IdZmianyPlan
        {
            get { return Tools.GetStr(ViewState["planZmId"]); }
            set { ViewState["planZmId"] = value; }
        }

        public string WymiarPlan
        {
            get { return Tools.GetStr(ViewState["planWymiar"]); }
            set { ViewState["planWymiar"] = value; }
        }

        // Potrzebne do inicjalizacji nagodzinami nocnymi.
        public int? NadgNPP
        {
            get { return (int?)ViewState["nadgnpp"]; }
            set { ViewState["nadgnpp"] = value; }
        }

        public int? NadgN
        {
            get { return ViewState["nadgn"] != null ? (int?)ViewState["nadgn"] : NadgNPP; }
            set { ViewState["nadgn"] = value; }
        }

        protected void pgWnioskiNadg_Activate(object sender, EventArgs e)
        {

        }

        protected void pgRCP_Activate(object sender, EventArgs e)
        {

        }

        protected void pgKorekta_Activate(object sender, EventArgs e)
        {

        }

        protected void pgDanePracownika_Activate(object sender, EventArgs e)
        {

        }

        protected void pgCzasPracy_Activate(object sender, EventArgs e)
        {
            //cntRcpAnalize.nadg50 = 
        }
    }
}