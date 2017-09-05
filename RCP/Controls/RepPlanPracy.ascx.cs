using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class RepPlanPracy : System.Web.UI.UserControl
    {
        public event EventHandler SelectDay;

        SqlConnection con = null;

        int[] Months = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};

        int FMode = PlanPracy.moKartaRoczna;

        private string cntPrefix;
        public PlanPracyLineHeader LineHeader;

        Ustawienia settings;
        PlanPracy.TimeSumator sumator;
        RepNadgodziny.TimeSumator3 total;
        /*
        DataSet dsK;  // kalendarz
        int ilDniPrac, ilGodzPrac;
        int etat;  // n/8
        */

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
        }
        //-------------

        protected void btSelectCell_Click(object sender, EventArgs e)
        {
            /*
            switch (FMode)
            {
                default:
                case moZmiany:
                    UpdateDays();
                    break;
                case moAccept:
                    if (SelectDay != null) SelectDay(this, EventArgs.Empty);
                    break;
            }
             */ 
        }
        //--------------------------------------------------------------------------------

        public void Prepare(string rok, string pracId)
        {
            Rok = rok;
            PracId = pracId;
            //lvPlanPracy.DataSourceID = null;
            //lvPlanPracy.DataSource = Months;
            lvPlanPracy.DataBind();

            RepPracInfo pi = (RepPracInfo)lvPlanPracy.FindControl("cntRepPracInfo");
            if (pi != null) pi.Prepare(null, pracId);
        }


        //--------
        private void MakeDayClickable()
        {
            /*
            //HtmlTable tb = (HtmlTable)lvPlanPracy.TemplateControl.FindControl("itemPlaceholderContainer"); // sprobowac dac na ondatabind
            HtmlTable tb = (HtmlTable)lvPlanPracy.FindControl("lvOuterTable"); // sprobowac dac na ondatabind
            if (tb != null)
                switch (FMode)
                {
                    default:
                    case PlanPracy.moZmiany:
                        if (EditMode) Tools.AddClass(tb, "clickable");
                        else Tools.RemoveClass(tb, "clickable");
                        break;
                    case PlanPracy.moAccept:
                        Tools.AddClass(tb, "clickable");
                        break;
                }
             */ 
        }

        //-----------------------------------------------------------------------------
        public bool IsWolny(DataSet dsK, DateTime date, out int rodzaj)  // spr czy jest wpisany w kalendarz, sobota, niedziela, święto, -1 jak nie jest to dzień wolny
        {
            foreach (DataRow dr in dsK.Tables[0].Rows)
            {
                DateTime dt = Base.getDateTime(dr, "Data", DateTime.MinValue);
                if (dt == date)
                {
                    rodzaj = Base.getInt(dr, "Rodzaj", -1);
                    return true;
                }
            }
            rodzaj = -1;
            return false;
        }

        /*
            +wtime = Worktime.RoundSec(sumator.sumWTime, zaokrSum, zaokrSumType);                                // całkowity 
            otime50 = sumator.GetSum(sumator.sumZTime, "150", true);                                            // nadgodziny 50 ze zmiany
            otime100 = sumator.GetSum(sumator.sumZTime, "200", true);                                           // nadgodziny 100 ze zmiany
            ztime = Worktime.RoundSec(sumator.GetSum(sumator.sumZTime), zaokrSum, zaokrSumType);                // pozostały czas ze zmiany (tu są absencje !!!)
            pptime = Worktime.RoundSec(sumator.GetSum(sumator.sumZmiany), zaokrSum, zaokrSumType);              // zaplanowany
            otime50 += sumator.GetSum(sumator.sumOTime, "150", true);                                           // nadgodziny 50
            otime100 += sumator.GetSum(sumator.sumOTime, "200", true);                                          // nadgodziny 100
            otime50 = Worktime.RoundSec(otime50, zaokrSum, zaokrSumType);                                      // zaokr. nadgodziny 50
            otime100 = Worktime.RoundSec(otime100, zaokrSum, zaokrSumType);                                    // zaokr. nadgodziny 100
            orest = Worktime.RoundSec(sumator.GetSum(sumator.sumOTime), zaokrSum, zaokrSumType);                // reszta nadgodzin
            ntime = Worktime.RoundSec(sumator.sumNTime, zaokrSum, zaokrSumType);                                // czas nocny
            htime = Worktime.RoundSec(sumator.sumHTime, zaokrSum, zaokrSumType);                                // czas w niedziele i święta
        */

        // zmiana S
        public string GetSumy(PlanPracy.TimeSumator ts, int zaokrSum, int zaokrSumType, int _ilGodzPrac, int ilDyzury, int ilWolneZaNadg)
        {
            int wtime = Worktime.RoundSec(sumator.sumWTime, zaokrSum, zaokrSumType);                                // łączny czas pracy
            //int ztime = Worktime.RoundSec(sumator.GetSum(sumator.sumZTime), zaokrSum, zaokrSumType);
            //int pptime = Worktime.RoundSec(sumator.GetSum(sumator.sumZmiany), zaokrSum, zaokrSumType);
            
            //int otime50 = Worktime.RoundSec(sumator.GetSum(sumator.sumOTime, "150", true), zaokrSum, zaokrSumType); // nadg 50
            //int otime100 = Worktime.RoundSec(sumator.GetSum(sumator.sumOTime, "200", true), zaokrSum, zaokrSumType);// nadg 100
            int otime50 = sumator.GetSum(sumator.sumZTime, "150", true);                                            // nadgodziny 50 ze zmiany
            int otime100 = sumator.GetSum(sumator.sumZTime, "200", true);                                           // nadgodziny 100 ze zmiany
            //int ztime = Worktime.RoundSec(sumator.GetSum(sumator.sumZTime), zaokrSum, zaokrSumType);                // pozostały czas ze zmiany (tu są absencje !!!)
            //int pptime = Worktime.RoundSec(sumator.GetSum(sumator.sumZmiany), zaokrSum, zaokrSumType);              // zaplanowany
            otime50 += sumator.GetSum(sumator.sumOTime, "150", true);                                           // nadgodziny 50
            otime100 += sumator.GetSum(sumator.sumOTime, "200", true);                                          // nadgodziny 100
            otime50 = Worktime.RoundSec(otime50, zaokrSum, zaokrSumType);                                      // zaokr. nadgodziny 50
            otime100 = Worktime.RoundSec(otime100, zaokrSum, zaokrSumType);                                    // zaokr. nadgodziny 100
            
            int orest = Worktime.RoundSec(sumator.GetSum(sumator.sumOTime), zaokrSum, zaokrSumType);                // reszta nadg.
            int ntime = Worktime.RoundSec(sumator._sumNTime, zaokrSum, zaokrSumType);                                // czas nocny
            int htime = Worktime.RoundSec(sumator.sumHTime, zaokrSum, zaokrSumType);                                // niedziele i święta

            total.SumTimes(_ilGodzPrac, wtime, otime50, otime100, orest, htime, ntime, ilDyzury, ilWolneZaNadg);

            return 
#if NOMWYM
                    Worktime.SecToHourStr(_ilGodzPrac)
#else
                    _ilGodzPrac.ToString() 
#endif
                    + "|" +                // czas nominalny
                    Worktime.SecToHourStr(wtime) + "|" +         // zmiany + nadgodziny
                    Worktime.SecToHourStr(otime50) + (orest > 0 ? "<br />? " + Worktime.SecToHourStr(orest) : null) + "|" + // powinno być 0 ! jezeli zmiany sa poprawnie skonfigurowane
                    Worktime.SecToHourStr(otime100) + "|" +
                    Worktime.SecToHourStr(htime) + "|" +        // czas wolny
                    Worktime.SecToHourStr(ntime) + "|" +
                    ilDyzury.ToString() + "|" +                 
                    ilWolneZaNadg.ToString();
        }


        /*
        public string GetSumy(PlanPracy.TimeSumator ts, int zaokrSum, int zaokrSumType, int ilGodzPrac)   
        {
            int wtime = Worktime.RoundSec(sumator.sumWTime, zaokrSum, zaokrSumType);                                // łączny czas pracy
            //int ztime = Worktime.RoundSec(sumator.GetSum(sumator.sumZTime), zaokrSum, zaokrSumType);
            //int pptime = Worktime.RoundSec(sumator.GetSum(sumator.sumZmiany), zaokrSum, zaokrSumType);
            int otime50 = Worktime.RoundSec(sumator.GetSum(sumator.sumOTime, "150", true), zaokrSum, zaokrSumType); // nadg 50
            int otime100 = Worktime.RoundSec(sumator.GetSum(sumator.sumOTime, "200", true), zaokrSum, zaokrSumType);// nadg 100
            int orest = Worktime.RoundSec(sumator.GetSum(sumator.sumOTime), zaokrSum, zaokrSumType);                // reszta nadg.
            int ntime = Worktime.RoundSec(sumator.sumNTime, zaokrSum, zaokrSumType);                                // czas nocny
            int htime = Worktime.RoundSec(sumator.sumHTime, zaokrSum, zaokrSumType);                                // niedziele i święta

            total.SumTimes(ilGodzPrac, wtime, otime50, otime100, orest, htime, ntime);

            return  ilGodzPrac.ToString() + "|" +                // czas nominalny
                    Worktime.SecToHourStr(wtime) + "|" +         // zmiany + nadgodziny
                    Worktime.SecToHourStr(otime50) + (orest > 0 ? "<br />? " + Worktime.SecToHourStr(orest) : null) + "|" + // powinno być 0 ! jezeli zmiany sa poprawnie skonfigurowane
                    Worktime.SecToHourStr(otime100) + "|" +
                    Worktime.SecToHourStr(htime) + "|" +         // czas wolny
                    Worktime.SecToHourStr(ntime) + "|" +
                    "0";    // Dyżury, póki co na sztywno wg wytycznych
        }
        */

        //-----------------------------------------------------------
        protected void lvPlanPracy_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            /*
            switch (e.CommandName)
            {
                case "SubItems":
                    string par = e.CommandArgument.ToString();  // kierId
                    LinkButton lbt = (LinkButton)e.Item.FindControl("lbtPracownik");
                    if (lbt != null && !String.IsNullOrEmpty(par))
                    {
                        string name = lbt.Text;
                        cntPath.AddPath(name, par, null);
                        IdKierownika = par;
                        Reload();
                    }
                    break;
            }
             */ 
        }
        //-------------
        protected void lvPlanPracy_LayoutCreated(object sender, EventArgs e)
        {
            LineHeader = (PlanPracyLineHeader)lvPlanPracy.FindControl("PlanPracyLineHeader");
            
            cntPrefix = btSelectCell.ClientID.Substring(0, btSelectCell.ClientID.Length - btSelectCell.ID.Length);
        }

        private bool _GetOkresyDays(string rok, out int firstDay, out int lastDay)
        {
            if (!String.IsNullOrEmpty(rok))
            {
                DataRow dr = db.getDataRow("select MIN(DAY(DataOd)), MAX(DAY(DataDo)) from OkresyRozl where YEAR(DataDo) = " + rok);
                if (!db.isNull(dr[0]) && !db.isNull(dr[1]))
                {
                    firstDay = db.getInt(dr, 0, 1);
                    lastDay = db.getInt(dr, 1, 31);
                    if (firstDay == 1 && lastDay < 31) lastDay = 31;  //20160410 jak był 1 miesiąc wpisany to mogło zwaracać 30 i było index out of bounds
                    return true;
                }
            }
            firstDay = 0;
            lastDay = 0;
            return false;
        }

        int maxDaysCount = 31;  // do Item DataBind

        protected void lvPlanPracy_DataBinding(object sender, EventArgs e)
        {
            if (LineHeader != null)
            {
                //----- parametry ------
                settings = Ustawienia.CreateOrGetSession();
                //----- data i sumator -----
                int firstDay, lastDay, cnt;
                if (_GetOkresyDays(Rok, out firstDay, out lastDay))
                {
                    cnt = LineHeader.DataBind(firstDay, lastDay);
                }
                else
                {
                    LineHeader.DataBind(settings.OkresDo);
                    cnt = 31;
                }
                Tools.SetColSpan(lvPlanPracy, "thHeader1", cnt + 1);
                Tools.SetColSpan(lvPlanPracy, "tdsumy", cnt + 1);
                maxDaysCount = cnt;

                sumator = new PlanPracy.TimeSumator();
                total = new RepNadgodziny.TimeSumator3(7+2);
                //dsK = null;

                /* dla kazdego okresu moze byc inne ...
                RepPracInfo pi = (RepPracInfo)lvPlanPracy.FindControl("cntRepPracInfo");
                if (pi != null) _etat = pi.Etat;
                else _etat = 0;
                */
            }
        }

        protected void lvPlanPracy_DataBound(object sender, EventArgs e)
        {
#if NOMWYM
            Tools.SetText(lvPlanPracy, "lbSumNominalny", Worktime.SecToHourStr(total.GetInt(0)));                  // (dni w miesiącu - wolne) * 8 ~ z PP bez korekt ? moze być różne bo za święto w niedziele trzeba dac wolne... - najlepiej było by wziąć z KP!
#else
            Tools.SetText(lvPlanPracy, "lbSumNominalny", total.GetInt(0).ToString());                  // (dni w miesiącu - wolne) * 8 ~ z PP bez korekt ? moze być różne bo za święto w niedziele trzeba dac wolne... - najlepiej było by wziąć z KP!
#endif
            Tools.SetText(lvPlanPracy, "lbSumSumaryczny", Worktime.SecToHourStr(total.Sum[1]));     // zmiany + nadgodziny
            int orest = total.GetInt(4);                                                          // powinno być 0 ! jezeli zmiany sa poprawnie skonfigurowane 
            Tools.SetText(lvPlanPracy, "lbSumNadg50", Worktime.SecToHourStr(total.Sum[2]) + (orest > 0 ? "<br />? " + Worktime.SecToHourStr(orest) : null));
            Tools.SetText(lvPlanPracy, "lbSumNadg100", Worktime.SecToHourStr(total.Sum[3]));
            Tools.SetText(lvPlanPracy, "lbSumNiedzieleSwieta", Worktime.SecToHourStr(total.Sum[5]));
            Tools.SetText(lvPlanPracy, "lbSumNocny", Worktime.SecToHourStr(total.Sum[6]));

            Tools.SetText(lvPlanPracy, "lbSumDyzury", total.GetInt(7).ToString());
            Tools.SetText(lvPlanPracy, "lbSumZaNadg", total.GetInt(8).ToString());





            /*
            HtmlTableCell td = (HtmlTableCell)lvPlanPracy.FindControl("thColSelect");
            if (td != null) td.Visible = false;
              */  

                /*
                switch (FMode)
                {
                    default:
                    case moZmiany:
                        td.Visible = EditMode;
                        break;
                    case moAccept:
                        td.Visible = false;
                        break;
                }
                 */

            MakeDayClickable();

            //RefreshLetterPager();
        }

        protected void lvPlanPracy_PreRender(object sender, EventArgs e)    // dzięki temu nie znikają zaznaczenia krzyżowe
        {
            /*
            switch (FMode)
            {
                default:
                case PlanPracy.moZmiany:
                    if (EditMode)
                    {
                        foreach (ListViewItem item in lvPlanPracy.Items)
                        {
                            PlanPracyLine2 ppl = (PlanPracyLine2)item.FindControl("PlanPracyLine");
                            CheckBox cb = (CheckBox)item.FindControl("cbPrac");
                            if (cb != null && ppl != null)
                                SetLineSelected(item, ppl, cb.Checked);
                        }
                    }
                    break;
                case PlanPracy.moAccept:
                    break;
            }
             */ 
        }
        //-------------------------------------------
        protected void lvPlanPracy_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            PlanPracyLine2 ppl = (PlanPracyLine2)e.Item.FindControl("PlanPracyLine");
            if (ppl != null)
            {
                ppl.Mode = FMode;
                ppl.cntPrefix = cntPrefix;
                ppl.LineHeader = LineHeader;
            }
        }

        public static string MonthR(int month)
        {
            string[] mr = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII" };
            return mr[month - 1];
        }

        protected void lvPlanPracy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            PlanPracyLine2 pl = (PlanPracyLine2)e.Item.FindControl("PlanPracyLine");
            Label lb = (Label)e.Item.FindControl("MonthLabel");
            if (pl != null && lb != null)
            {
                //----- okres rozliczeniowy -----
                DateTime okresOd, okresDo, lastDay;
                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                
                int rok = Tools.StrToInt(Rok, -1);
                /*
                int month = (int)dataItem.DataItem;
                if (settings.OkresDo > 28)          // od 1-końca miesiąca
                {
                    okresOd = new DateTime(rok, month, 1);
                    okresDo = okresOd.AddMonths(1).AddDays(-1);
                    //lb.Text = month.ToString();  
                    lb.Text = MonthR(month);  
                }
                else                                // od w środku miesiąca ...
                {
                    okresDo = new DateTime(rok, month, settings.OkresDo);
                    okresOd = okresDo.AddMonths(-1).AddDays(1);
                    int month1 = okresOd.Month;
                    //int month1 = (month + 10) % 12 + 1; 
                    lb.Text = String.Format("{0} / {1}", MonthR(month1), MonthR(month));
                }
                */

                DataRowView drv = (DataRowView)dataItem.DataItem;
                int month = db.getInt(drv["Month"], 0);
                okresOd = (DateTime)drv["OkresOd"];
                okresDo = (DateTime)drv["OkresDo"];
                lb.Text = MonthR(month);  

                lastDay = Tools.eom(okresOd);
                //----- pierwsze uruchomienie ------
                if (con == null)
                {
                    con = Base.Connect();
                }
                //----- pobieranie danych ------
                string dod = Base.DateToStr(okresOd);
                string ddo = Base.DateToStr(okresDo);
                DataSet dsK = Base.getDataSet(con, String.Format(    // nie mam jak juz podlinkowac tego do wt.GetWorktime chyba zeby zmienic ...
                    "select Data, Rodzaj from Kalendarz where Data between '{0}' and '{1}'",
                    dod, ddo));



                int _ilDniPrac = Worktime._GetIloscDniPrac(con, dod, ddo);
                // czy ilość dni w miesiącu ?






                //----- z RepNadgodziny3 -----
                int _ilDniPracMies = Worktime._GetIloscDniPracMies(con, ddo);

#if NOMWYM
                int ilDniPrac2Sec, ilDniPracMies2Sec;
                RepNadgodziny3.GetPracDniPracSec(PracId, dod, ddo, _ilDniPracMies, out ilDniPrac2Sec, out ilDniPracMies2Sec);
#else
                int ilDniPrac2, ilDniPracMies2;
                RepNadgodziny3._GetPracDniPrac(PracId, dod, ddo, _ilDniPracMies, out ilDniPrac2, out ilDniPracMies2);
#endif
                //-----------------------------







                DataRow drP = Worker.GetData(con, Okres.GetId(con, okresDo), PracId);
                int etat = Worker.GetEtat(drP);
                if (etat == 0) etat = 8;            //zeby było
#if NOMWYM
                int ilGodzPracSec = ilDniPrac2Sec;  //jak błąd to etat = 0
#else
                int ilGodzPrac = ilDniPrac2 * etat;  //jak błąd to etat = 0
#endif






                DataSet ds = Worktime.GetWorktime1(con, PracId, null,            // bez worktime, tylko z PP
                        Base.DateToStr(okresOd), Base.DateToStr(okresDo), null, 
                        null, -1, -1, -1, -1, -1, -1, false);
                
                
                
                
                
                
                //----- wolne za nadgodziny -----
                string wolneZaNadg = db.getScalar(String.Format(@"
select count(*) from dbo.GetDates2('{0}','{1}') D
join Absencja A on D.Data between A.DataOd and A.DataDo and A.Kod = {3} and A.IdPracownika = {2}
left outer join Kalendarz K on K.Data = D.Data
where K.Data is null", dod, ddo, PracId, 

#if SIEMENS
                     519                //<<<< sprawdzić !!! czy nie powinno to sie liczyc tak jak w RepNAdgodziny3 ???
#else
                     1000090083
#endif
                     ));
                int ilWolneZaNadg = Tools.StrToInt(wolneZaNadg, 0);






                //----- wypełnianie informacji w wierszu kolejne dni -----
                int cnt = ds.Tables[0].Rows.Count;
                int ofs = 0;

                string[] days = new string[maxDaysCount]; 
                
                DateTime today = DateTime.Today;
                sumator.Reset();
                bool absPraca = false;
                bool allAccepted = true;

                if (cnt == 0) allAccepted = false;
                else
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        DataRow dr = Base.getDataRow(ds, i);
                        DateTime date = Base.getDateTime(dr, "Data", DateTime.MinValue);  // musi być                 

                        //string info = pl.LineHeader.HeaderData[i];  // holiday, today
                        string info = null;
                        string absencja = null;

                        //----- zmiana -----
                        string zmianaId = Base.getValue(dr, "ZmianaId");
                        bool isZmiana = !String.IsNullOrEmpty(zmianaId);

                        //----- wolne -----
                        int _wolne;
                        bool _isHoliday = IsWolny(dsK, date, out _wolne);    // -1 pracujacy lub bład, 0 sobota 1 niedziela 2 swieto

                        //----- absencja ------
                        string absKod = Base.getValue(dr, "AbsencjaKod");       // z systemu KP
                        int absGodzPracy = Base.getInt(dr, "GodzinPracy", 0);
                        bool isAbsencja = !String.IsNullOrEmpty(absKod);
                        bool showAbsencja = false;
                        if (isAbsencja)
                        {
                            info += PlanPracy.maAbsencja; // "u";
                            string absSymbol = Base.getValue(dr, "AbsencjaSymbol");
                            string absNazwa = Tools.FirstUpper(Base.getValue(dr, "AbsencjaNazwa").ToLower());
                            absencja = (!String.IsNullOrEmpty(absSymbol) ? absSymbol : "X") + " - " + absNazwa;
                            absencja = absencja.Replace(Tools.LineParamSeparator, ' '); // na wszelki wypadek
                            showAbsencja = !_isHoliday || Base.getBool(dr, "DniWolne", false) || isZmiana;      // jak ktos jest chory i nie ma zaplanowanej zmiany to trzeba pokazac chociaz jak ma wolne bo pracował na nockę to też pokaże - czy powinien ???
                        }
                        //----- poprawki - akceptacja -----
                        int ztime, otimeD, otimeN, ntime, wtime;
                        bool isAcc = Base.getBool(dr, "Akceptacja", false);   //poprawnie zamknięty okres ma wszystko poakceptowane !!! więc nie trzeba sprawdzać - miga bo akceptacja z ręki wpisana ...
                        if (isAcc)
                        {
                            ztime = Base.getInt(dr, "CzasZm", 0);
                            otimeD = Base.getInt(dr, "NadgodzinyDzien", 0);
                            otimeN = Base.getInt(dr, "NadgodzinyNoc", 0);
                            ntime = Base.getInt(dr, "Nocne", 0);
                            wtime = ztime + otimeD + otimeN;        // to pokazuje czas łączny w dniu, a nie czas za absencje
                        }
                        else
                        {
                            ztime = 0;
                            otimeD = 0;
                            otimeN = 0;
                            ntime = 0;
                            wtime = 0;
                            allAccepted = false;
                        }

                        //----- absencje jako czas pracy -----
                        if (isAbsencja)
                        {
                            if (ztime + otimeD + otimeN > 0) absPraca = true;
                            if (absGodzPracy > 0 && !_isHoliday)    // liczę jako czas pracy jeśli nie było pracy i dzień nie jest wolny (bez sobot, niedziel i świąt) 
                                //if (absGodzPracy > 0 && isZmiana)     // liczę jako czas pracy jeśli nie było pracy i na dzień jest zaplanowana/skorygowana zmiana - więc i w niedziele i święta zgodnie z ostatnimi ustaleniami 2012-05-09 <<< ale ze system KP nie obsługuje więc wyłączam i po staremu zeby sie zgadzało ...
                                if (ztime == 0)                     // praca ma priorytet
                                    ztime = ((etat > 0 ? etat : 8) * 3600 * absGodzPracy) / 8;  //>>> jak błąd etatu to policzy 8, a moze powienien dać 0 ?
                        }
                        /* stare
                        if (isAbsencja && wolne == -1)       // nie ma dnia wolnego
                            if (ztime == 0) ztime = (etat > 0 ? etat : 8) * 3600;
                            else absPraca = true;           // na razie nie wyswietlam o tym info ...
                        */
                        //----- dzień wolny ------
                        if (_isHoliday)
                            info += PlanPracy.maHoliday;
                        //----- prezentacja -------
                        string wt = wtime > 0 ? Worktime.SecToTimePP(wtime, 0, null, true) : null;  // jeżeli jest absencja i nie zaakceptowany to absencja bez czasu
                        days[i + ofs] = Tools.SetLineParams(6, zmianaId,
                                                           Base.getValue(dr, "Symbol"),
                                                           Base.getValue(dr, "Kolor"),
                                                           info, wt,
                                                           showAbsencja ? absencja : null);         // nie pokazuję absencji w dni wolne chyba ze mam
                        //----- sumy -----
                        int t50, t100;  // w danym dniu 
                        int d50, d100;
                        bool zeroZm;
                        //int before6 = Worktime.GetBefore6(dr["CzasIn"], ztime);

                        sumator._SumTimes2(date, Base.getValue(dr, "ZmianaId"), _wolne >= 1, ztime, otimeD, otimeN, ntime, out t50, out t100, isAbsencja, dr["CzasIn"], out d50, out d100, out zeroZm);  // zmiana podlinkowana - null jak nie ma; wolne >= 1 - niedziele i święta bez sobót
                        
                        //----- dziura -----
                        if (date == lastDay)  // uzupełniam następne komórki na pusto
                        {
                            info = PlanPracy.maNoDay;
                            ofs = 31 - lastDay.Day;
                            for (int k = 1; k <= ofs; k++)
                                days[i + k] = Tools.SetLineParams(6, null, null, null, info, null, null);
                        }
                    }
                    if (allAccepted && cnt < _ilDniPrac)   // jak jest plan to musi być akceptacja
                        allAccepted = false;    // brak planu, a tylko 1 dzień zaakceptowany
                }

                if (!allAccepted)
                {
                    for (int i = 0; i < 31; i++)
                    {
                        string p1, p2, p3, info, p5, abs;
                        Tools.GetLineParams(days[i], out p1, out p2, out p3, out info, out p5, out abs); 
                        days[i] = Tools.SetLineParams(6, null, null, null, info, null, abs);
                    }
                    sumator.Reset();
                }
                pl.PracId = PracId;
                pl.Zmiany = String.Join(",", days);
#if NOMWYM
                pl.Sumy = GetSumy(sumator, settings.ZaokrSum, settings.ZaokrSumType, ilGodzPracSec, 0, ilWolneZaNadg);   // tu bierze sumy
#else
                pl.Sumy = GetSumy(sumator, settings.ZaokrSum, settings.ZaokrSumType, ilGodzPrac, 0, ilWolneZaNadg);   // tu bierze sumy
#endif
            }
        }

        //----------
        protected void lvPlanPracy_Unload(object sender, EventArgs e)
        {
            if (con != null) Base.Disconnect(con);
        }


        //-----------------------------------------------------------------
        private object TimeToIntSec(object t)
        {
            if (t == null || t.Equals(DBNull.Value)) return null;
            else
            {
                DateTime dt = (DateTime)t;
                int sec = (dt.Hour * 60 * 60) + (dt.Minute * 60) + dt.Second;
                return sec;
            }
        }

        /*
        public DateTime DateFromIdx(int idx)
        {
            return DateTime.Parse(DateFrom).AddDays(idx);
        }
        */

        //----------------
        public int Mode
        {
            get { return FMode; }
            set { FMode = value; }
        }


        public ListView List
        {
            get { return lvPlanPracy; }
        }

        public string PracId
        {
            get { return hidPracId.Value; }
            set { hidPracId.Value = value; }
        }

        public string Rok
        {
            get { return hidRok.Value; }
            set { hidRok.Value = value; }
        }
    }
}




/*
declare @year int
set @year = 2013 
select DAY(D.Data) as Month, R.Id, 
ISNULL(R.DataOd, DATEADD(MONTH, DAY(D.Data)-1, DATEADD(YEAR, @year-1900, 0))) as OkresOd, 
ISNULL(R.DataDo, DATEADD(DAY, -1, DATEADD(MONTH, DAY(D.Data), DATEADD(YEAR, @year-1900, 0)))) as OkresDo
from dbo.GetDates2('20120101','20120112') D
left outer join OkresyRozl R on R.DataDo between 
DATEADD(MONTH, DAY(D.Data)-1, DATEADD(YEAR, @year-1900, 0)) and 
DATEADD(DAY, -1, DATEADD(MONTH, DAY(D.Data), DATEADD(YEAR, @year-1900, 0))) */