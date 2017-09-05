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
    public partial class cntPlanRoczny2 : System.Web.UI.UserControl
    {
        public event EventHandler SelectDay;
        public event EventHandler DataSaved;

        int[] Months = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};

        public const int moQuery    = 0;
        public const int moEditable = 1;
        int FMode = moQuery;

        public const int cm14 = 0;              // min 14 dni
        public const int cm14all = 1;           // min 14 dni i całość trzeba zaplanować 
        int FCheckMode = cm14all;

        public const int stPlanowanie = 0;
        public const int stKorekta      = 1;
        public const int stBlokada      = 2;    // archiwum
        public const int stNiedostepny  = 3;    // przyszły

        private string cntPrefix;
        public PlanPracyLineHeader LineHeader;

        //Ustawienia settings;
        //PlanPracy.TimeSumator sumator;
        //RepNadgodziny.TimeSumator3 total;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitButtons();
            }
            else
            {
                InitScript();
            }
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
        private bool IsAdmin
        {
            get { return App.User.IsAdmin; }
        }

        private bool ShowHistory
        {
            set { paAdminOptions.Visible = value; }
            get { return paAdminOptions.Visible; }
        }

        public void Prepare(string rok, string pracId)
        {
            _Rok = rok + "-01-01";
            PracId = pracId;
            Czas = Tools.DateTimeToStr(DateTime.Now);
            //lbRok.Visible = true;
            //lbRok.Text = "Plan urlopów na rok: " + rok;                 

            //lvPlanPracy.DataSourceID = null;
            //lvPlanPracy.DataSource = Months;
            
            //----- admin options -----
            ShowHistory = IsAdmin;
            
            GetParams();

            PracInfoReload();
            lvPlanPracy.DataBind();
            EnableButtons();
            
            InitScript();
        }

        private void Reload(bool enableButtons)
        {
            Reload(Tools.DateTimeToStr(DateTime.Now), enableButtons);
        }

        private void Reload(string czas, bool enableButtons)
        {
            Czas = czas;
            PracInfoReload();
            lvPlanPracy.DataBind();
            if (enableButtons) EnableButtons();
        }

        /*
        w PlanUrlopowParam musi byc jeden wpis z datą blokady ale można też na każdy rok - jak znajdzie to weźmie, jak nie to ostatni
        mogą być tam też ustawienia dla konkretnych pracowników
         */

        public static void x_GetStatus(DateTime rok, DateTime data, string pracId, out int status, out string stOpis, out DateTime planStart, out DateTime planStop)
        {
            DataRow dr = db.getDataRow(String.Format(@"
declare @pracId int
declare @rok datetime
declare @data datetime
set @pracId = {0}
set @rok = '{1}'
set @data = '{2}'   -- dziś

select PlanStart, PlanStop, 
case 
	when YEAR(Data) < YEAR(PlanStart) then 3
	when Data between PlanStart and PlanStop then 0
	when Data > PlanStop and Data <= dbo.eoy(PlanStart) then 1
else 2
end as Status
from 
(
select 
	ISNULL(P.Rok,      ISNULL(DATEADD(YEAR, DATEDIFF(YEAR, B.Rok, @rok), B.Rok),      D.Rok)) as PlanStart, 
	ISNULL(P.PlanStop, ISNULL(DATEADD(YEAR, DATEDIFF(YEAR, B.Rok, @rok), B.PlanStop), D.PlanStop)) as PlanStop,
	dbo.getdate(@data) as Data
-- D.RokA, D.StopA, B.Rok as RokB, B.PlanStop as StopB, P.Rok as RokP, P.PlanStop as StopP 
from (select dbo.boy(@rok) as Rok, dbo.eom(dbo.boy(@rok)) as PlanStop) as D		-- domyślne
outer apply (select top 1 * from PlanUrlopowParam where IdPracownika is null and Rok <= @rok order by Rok desc) as B    -- ostatni wpis ustawień dla całości
left join PlanUrlopowParam P on @pracId is not null and P.IdPracownika = @pracId and @rok between P.Rok and dbo.eoy(P.Rok)	-- ustawienia indywidualne pracownika w roku
) as PPP
                ", db.nullParam(pracId), Tools.DateToStrDb(rok), Tools.DateToStrDb(data)));
            status = db.getInt(dr, "Status", stNiedostepny);
            planStart = (DateTime)db.getDateTime(dr, "PlanStart");
            planStop = (DateTime)db.getDateTime(dr, "PlanStop");
            //stOpis = status == stPlanowanie ? String.Format("Planowanie (do: {0})", Tools.DateToStr(planStop)) : GetStatus(status);
            stOpis = GetStatus(status, planStop);
        }

        public static void _GetStatus(DateTime rok, DateTime data, string pracId, out int status, out string stOpis, out DateTime planStart, out DateTime planStop)
        {
            DataRow dr = db.getDataRow(String.Format(@"
declare @pracId int
declare @rok datetime
declare @data datetime
set @pracId = {0}
set @rok = '{1}'
set @data = '{2}'   -- dziś

select *,
case 
	when @data < PlanStart then 3						--stNiedostepny 
	when @data between PlanStart and PlanStop then 0	--stPlanowanie
	when @data between PlanStop and dbo.eoy(Rok) then 1 --stKorekta
else 2													--stBlokada    when @data > dbo.eoy(Rok) then 2					
end as Status,
case 
	when @data < PlanStart then 'Niedostępny'
	when @data between PlanStart and PlanStop then 'Planowanie'
	when @data between PlanStop and dbo.eoy(Rok) then 'Korekta'
else 'Archiwum'
end as StatusNazwa
from
(
    select 
        DATEADD(YEAR, dY, Rok) as Rok,
        DATEADD(YEAR, dY, PlanStart) as PlanStart,
        DATEADD(YEAR, dY, PlanStop) as PlanStop,
        dbo.eoy(DATEADD(YEAR, dY, Rok)) as RokEnd,
        Min14
    from
    (
        select top 1 *, DATEDIFF(YEAR, Rok, @rok) as dY
        from PlanUrlopowParam
        where Rok <= @rok order by Rok desc
    ) D
) D
                ", db.nullParam(pracId), Tools.DateToStrDb(rok), Tools.DateToStrDb(data)));
            if (dr != null)
            {
                status = db.getInt(dr, "Status", stNiedostepny);
                planStart = (DateTime)db.getDateTime(dr, "PlanStart");
                planStop = (DateTime)db.getDateTime(dr, "PlanStop");
                //stOpis = status == stPlanowanie ? String.Format("Planowanie (do: {0})", Tools.DateToStr(planStop)) : GetStatus(status);
                stOpis = GetStatus(status, planStop);
            }
            else   // jak wymuszam date wcześniejsza ...
            {
                status = stNiedostepny;
                planStart = rok;
                planStop = rok.AddMonths(1).AddDays(-1);
                stOpis = "Niedostępny";
            }
        }

        public static string GetStatus(int status, DateTime planStop)
        {
            switch (status)
            {
                case stPlanowanie:
                    //return "Planowanie";
                    return String.Format("Planowanie (do: {0})", Tools.DateToStr(planStop));
                case stKorekta:
                    //return "Plan zatwierdzony";
                    //return String.Format("Plan zatwierdzony ({0})", Tools.DateToStr(planStop.AddDays(1)));
                    return String.Format("Plan zatwierdzony ({0})", Tools.DateToStr(planStop));
                case stBlokada:
                    return "Plan archiwalny";
                case stNiedostepny:
                    return "Plan niedostępny";
                default:
                    return "Zablokowany do edycji";
            }
        }

        private void GetParams()
        {
            int status;
            string opis;
            DateTime planStart, planStop;
            _GetStatus((DateTime)Tools.StrToDateTime(_Rok), DateTime.Today, PracId, out status, out opis, out planStart, out planStop);
            Editable = status == stPlanowanie || status == stKorekta;
            bool kor = status == stKorekta;// DateTime.Today > stop;
            Korekta = kor;
            //cntSelectUrlop.SelectedInfo = kor ? "k" : null;

            lbOkresStatus.Text = opis;
            lbOkresStatus.Visible = true;

            lbRok.Text = AppUser.GetNazwiskoImieNREW(PracId);
            lbRok.Visible = true;

            btEditPP.Text = kor ? "Korekta" : "Edycja";
        }

        private cntPracInfo2 PracInfoReload()  // musi byc wywolywana przed lvPlanPracy.DataBind()
        {
            cntPracInfo2 pi = (cntPracInfo2)lvPlanPracy.FindControl("cntPracInfo");
            if (pi != null)
            {
                pi.Prepare(PracId, _Rok);
                hidUmowaOd.Value = pi.UmowaOd;
                hidUmowaDo.Value = pi.UmowaDo;
            }
            return pi;
        }

        public void InitScript()
        {
            if (String.IsNullOrEmpty(cntPrefix)) cntPrefix = getCntPrefix();
            int kor = Korekta ? 1 : 0;
            Tools.ExecOnStart2("scpuinit", String.Format("initPlanUrlopow('{0}',{1},{2});", cntPrefix, kor, FCheckMode));
        }

        public void InitButtons()
        {
            if (String.IsNullOrEmpty(cntPrefix)) cntPrefix = getCntPrefix();
            Tools.MakeButton(btSavePP, String.Format("return checkPlanUrlopow('{0}',{1});", cntPrefix, App.User.IsAdmin ? "1" : "0"));
        }

        private void EnableButtons()    // tylko na starcie przy wyłaczonym editMode powinno się ustawić
        {
            bool e = FMode != moQuery;
            bool p = Portal;
            //tbNavigator.Visible = e;
            tbNavigator.Visible = true;
            bool em = EditMode;         // na wszelki wypadek 
            btBack.Visible = e && !em && !p;
            btEditPP.Visible = e && (Editable || App.User.HasRight(AppUser.rPlanUrlopowEditPo)) && !em && !p;
        }

        private void SetEditMode(bool edit)
        {
            //cntPlanPracy.EditMode = edit;
            bool p = Portal;

            cntSelectUrlop.EditMode = edit;
            lbZmianyQ.Visible = !edit;
            lbZmianyE.Visible = edit;
            lbPlanQ.Visible = !edit;
            lbPlanE.Visible = edit;

            btEditPP.Visible = !edit && !p;
            //btCheckPP.Visible = edit;
            btSavePP.Visible = edit && !p;
            btCancelPP.Visible = edit && !p;
            //cntSelectOkres.Enabled = !edit;

            btBack.Visible = !edit && !p;

            /* coś nie działało ...
            if (edit)
                Tools.MakeConfirmButton(btBack, "Potwierdź wyjście bez zapisu.");
            else
                Tools.MakeConfirmButton(btBack, null);
             */
            //----- admin options -----
            if (ShowHistory)
            {
                //cbKorekta.Visible = edit;   nie ma mozliwosci - wszystko po dacie jest korektą
                //cbKorekta.Checked = Korekta;
                if (edit)
                {
                    int idx = ddlHistory.SelectedIndex;
                    if (idx != 0)
                    {
                        ddlHistory.SelectedIndex = 0;  // zawsze jest brak danych
                        Reload(false);
                    }
                }
                ddlHistory.Enabled = !edit;
            }
            MakeDayClickable(edit);
        }
        //--------
        private void MakeDayClickable(bool edit)
        {
            //HtmlTable tb = (HtmlTable)lvPlanPracy.TemplateControl.FindControl("itemPlaceholderContainer"); // sprobowac dac na ondatabind
            HtmlTable tb = (HtmlTable)lvPlanPracy.FindControl("lvOuterTable"); // sprobowac dac na ondatabind
            if (tb != null)
            {
                if (edit) Tools.AddClass(tb, "clickable");
                else Tools.RemoveClass(tb, "clickable");
            }
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
        // zmiana S
        public string GetSumy(PlanPracy.TimeSumator ts, int zaokrSum, int zaokrSumType, int ilGodzPrac, int ilDyzury, int ilWolneZaNadg)
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
            int ntime = Worktime.RoundSec(sumator.sumNTime, zaokrSum, zaokrSumType);                                // czas nocny
            int htime = Worktime.RoundSec(sumator.sumHTime, zaokrSum, zaokrSumType);                                // niedziele i święta

            total.SumTimes(ilGodzPrac, wtime, otime50, otime100, orest, htime, ntime, ilDyzury, ilWolneZaNadg);

            return ilGodzPrac.ToString() + "|" +                // czas nominalny
                    Worktime.SecToHourStr(wtime) + "|" +         // zmiany + nadgodziny
                    Worktime.SecToHourStr(otime50) + (orest > 0 ? "<br />? " + Worktime.SecToHourStr(orest) : null) + "|" + // powinno być 0 ! jezeli zmiany sa poprawnie skonfigurowane
                    Worktime.SecToHourStr(otime100) + "|" +
                    Worktime.SecToHourStr(htime) + "|" +        // czas wolny
                    Worktime.SecToHourStr(ntime) + "|" +
                    ilDyzury.ToString() + "|" +                 
                    ilWolneZaNadg.ToString();
        }
        */

        /*
        public string xxxGetSumy(PlanPracy.TimeSumator ts, int zaokrSum, int zaokrSumType, int ilGodzPrac)   
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
            //cntPrefix = btSelectCell.ClientID.Substring(0, btSelectCell.ClientID.Length - btSelectCell.ID.Length);
            cntPrefix = getCntPrefix();
        }

        private string getCntPrefix()
        {
            return btSelectCell.ClientID.Substring(0, btSelectCell.ClientID.Length - btSelectCell.ID.Length);
        }

        int maxDaysCount = 37;  // do Item DataBind

        protected void lvPlanPracy_DataBinding(object sender, EventArgs e)
        {
            if (LineHeader != null)
            {
                LineHeader.DataBind(1, maxDaysCount);
                Tools.SetColSpan(lvPlanPracy, "thHeader1", maxDaysCount + 1);
                Tools.SetColSpan(lvPlanPracy, "tdsumy", maxDaysCount + 1);

                //Tools.SetText(lvPlanPracy, "MonthLabel", _Rok.Substring(0, 4));
                Tools.SetText(lvPlanPracy, "MonthLabel", String.Format("Miesiąc '{0}", _Rok.Substring(0, 4)));
            }
        }

        protected void lvPlanPracy_DataBound(object sender, EventArgs e)
        {
            //Tools.SetText(lvPlanPracy, "lbSumNominalny", total.GetInt(0).ToString());                  // (dni w miesiącu - wolne) * 8 ~ z PP bez korekt ? moze być różne bo za święto w niedziele trzeba dac wolne... - najlepiej było by wziąć z KP!
            //Tools.SetText(lvPlanPracy, "lbSumSumaryczny", Worktime.SecToHourStr(total.Sum[1]));     // zmiany + nadgodziny





            //MakeDayClickable();
        }

        protected void lvPlanPracy_PreRender(object sender, EventArgs e)    // dzięki temu nie znikają zaznaczenia krzyżowe
        {
        }
        //-------------------------------------------
        protected void lvPlanPracy_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            PlanPracyLine2 ppl = (PlanPracyLine2)e.Item.FindControl("PlanPracyLine");
            if (ppl != null)
            {
                //ppl.Mode = PlanPracy.moPlanUrlopowRok;   // jest w aspx ustawione
                ppl.cntPrefix = cntPrefix;
                ppl.LineHeader = LineHeader;
            }
        }

        public static string MonthR(int month)
        {
            return Tools.MonthName[month];            
            //string[] mr = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII" };
            //return mr[month - 1];
        }



        int cnt1 = 0;


        protected void lvPlanPracy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            cnt1++;


            PlanPracyLine2 pl = (PlanPracyLine2)e.Item.FindControl("PlanPracyLine");
            Label lb = (Label)e.Item.FindControl("MonthLabel");
            if (pl != null && lb != null)
            {
                //----- miesiąc -----
                DateTime miesOd, miesDo;
                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                
                DataRowView drv = (DataRowView)dataItem.DataItem;
                int month = db.getInt(drv["Month"], 0);
                miesOd = (DateTime)drv["MiesiacOd"];
                miesDo = (DateTime)drv["MiesiacDo"];
                lb.Text = MonthR(month);

                //----- wypełnianie informacji w wierszu kolejne dni -----
                string[] days = new string[maxDaysCount];
                
                for (int i = 0; i < maxDaysCount; i++)
                {
                    string data = drv[i.ToString()].ToString();
                    if (String.IsNullOrEmpty(data))
                    {
                        string info = PlanPracy.maNoDay;
                        days[i] = Tools.SetLineParams(9, null, null, null, info, null, null, null, null, null, null);
                    }
                    else
                    {
                        /*
                             0 convert(varchar(10), C.Day, 20) + ''|'' +
                             1 isnull(convert(varchar, K.Rodzaj), '''') + ''|'' + 
                             2 isnull(K.Opis, '''') + ''|'' + 
		     
                             3 isnull(convert(varchar, PU.KodUrlopu), '''') + ''|'' + 
                             4 isnull(AK.Symbol, '''') + ''|'' + 
                             5 isnull(AK.Kolor, '''') + ''|'' + 
                             6 isnull(AK.Nazwa, '''') + ''|'' + 
		     
                             7 convert(varchar, ISNULL(PU.Korekta, 0)) + ''|'' +
		     
                             8 isnull(AK2.Symbol, '''') + ''|'' +
                             9 isnull(AK2.Kolor, '''') + ''|'' + 
                            10 isnull(AK2.Nazwa, '''') + ''|'' + 
                            11 convert(varchar, ISNULL(AK2.WyborPU, 0))
                            12 jest umowa 0/1
                            13 wniosek urlopowy zamiast AK.Symbol
                        */

                        string[] D = data.Split('|');   // 0-data 1-kod 2-symbol 3-kolor 4-rodzaj 5-opis 6-korekta
                        string info = null;
                        string wolne  = D[1];           // rodzaj dnia 0,1,2
                        string nazwaD = D[2].Replace(',', ' ');
                        string nazwaP = D[6].Replace(',', ' ');
                        bool korekta  = D[7] == "1";
                        string nazwaA = D[10].Replace(',', ' ');

                        //bool umowa = D[12] == "1";
                        bool _umowa   = D[12] == "1";
                        bool poumowie = D[12] == "2";
                        bool wniosek  = D[13] != "";

                        bool planAbs  = !String.IsNullOrEmpty(D[3]);
                        string planInfo;
                        
                        if (korekta || planAbs)
                            planInfo = "\n" + (korekta ? "Korekta: " : "Plan: ") + (planAbs ? nazwaP : "brak");
                        else
                            planInfo = null;

                        string hint = D[0] +    // data cr nazwa dnia wolnego cr nazwa absencji
                            (String.IsNullOrEmpty(nazwaD) ? null : "\n" + nazwaD) +
                            planInfo +
                            (String.IsNullOrEmpty(D[8]) ? null : "\n" + (wniosek ? "Wniosek: " : "Absencja: ") + nazwaA);

                        if (!String.IsNullOrEmpty(wolne))
                            info += PlanPracy.maHoliday;
                        if (korekta)
                            info += PlanPracy.maUrlopKorekta;
                        
                        //if (!umowa)
                        if (!_umowa && !poumowie)
                            info += PlanPracy.maNoDay;

                        if (poumowie)
                            info += PlanPracy.maPoUmowie;

                        if (wniosek)
                            info += PlanPracy._maAbsencjaKier;  // tak jak w przypadku absencji kierownika - kursywa

                        string dno = Tools.Right(D[0], 2);      // nr dnia - data
                        if (dno.StartsWith("0"))
                            dno = dno.Substring(1);

                        //string kolor = D[11] == "1" ? null : D[9];      // kolor - jeżeli nie jest wybierana w PU  

                        days[i] = Tools.SetLineParams(D[3],   // 0 kod
                                                      D[4],   // 1 symbol 
                                                      D[5],   // 2 kolor
                                                      info,   // 3 info
                                                      hint,   // 4 hint 
                                                      dno,    // 5 numer dnia ~ absencja w pp
                                                      D[3],   // 6 dla znacznika modyfikacji = kod oryginalny
                                                      D[8],   // 7 absencja: symbol
                                                      D[9]);  // 8 absencja: kolor
                                                      //kolor); // 8 absencja: kolor - jeżeli nie jest wybierana w PU - żeby UW bylo bez koloru, kolor dla UŹ 
                    } 
                }
                pl.PracId = PracId;
                pl.Zmiany = String.Join(",", days);
                pl.Sumy = String.Format("{0}|{1}", 
                    drv["37"].ToString(),
                    drv["38"].ToString()
                    );
            }
        }














        //----- update zmiany -------------------------------------------------
        public int Update()  // Plan Pracy - zmiany
        {
            string pracId = PracId;
            AppUser user = AppUser.CreateOrGetSession();
            
            SqlTransaction tr = db.con.BeginTransaction("PUPD1");

            DateTime boy = (DateTime)Tools.StrToDateTime(_Rok);   // musi byc
            DateTime data;
            string czas = Tools.DateTimeToStr(DateTime.Now);  // z dokładnościa do sekund powinno wystarczyć chociaż tu trzeba pomyśleć nad jakims zabezpieczeniem - moze w triggerze ???
            for (int idx = 0; idx < lvPlanPracy.Items.Count; idx++)
            {
                ListViewItem item = lvPlanPracy.Items[idx];
                PlanPracyLine2 pl = (PlanPracyLine2)item.FindControl("PlanPracyLine");
                if (pl != null)
                {
                    string[] dni = pl.Zmiany.Split(',');
                    bool lineMod = false;
                    for (int i = 0; i < dni.Count(); i++)
                    {
                        string[] daydata = Tools.GetLineParams(dni[i]);
                        bool modified = daydata[6] != daydata[0]; 
                        if (modified)
                        {
                            lineMod = true;
                            int day = Tools.StrToInt(daydata[5], -1);
                            if (day != -1)
                            {
                                data = boy.AddMonths(idx).AddDays(day - 1);
                                string kod = daydata[0];

                                string korekta = Korekta ? "1" : "0";

                                int id = db.insert(tr, db.insertCmd("PlanUrlopow", 0,
                                    "IdPracownika,Data,KodUrlopu,Korekta,Od,AutorId,Status",
                                    pracId,
                                    db.strParam(Tools.DateToStrDb(data)),
                                    db.nullParam(kod),
                                    korekta,
                                    //"GETDATE()",          // zapisjuje sie z ms dla kazdego z rekorów - tak nie pogrupuje po czasi zeby modyfikacje pokazac
                                    db.strParam(czas),
                                    App.User.OriginalId,
                                    0
                                    ), true, true);
                                if (id == -1)
                                {
                                    tr.Rollback();
                                    return -1;
                                }

                                daydata[6] = daydata[0];
                                dni[i] = Tools.SetLineParams(daydata);
                            }
                            else
                            {
                                tr.Rollback();
                                return -2;
                            }
                        }
                    }
                    if (lineMod)  // trzeba uaktualinić ze juz zapisane
                        pl.Zmiany = String.Join(",", dni);
                }
            }
            tr.Commit();
            return 0;
        }
        /*
select 
	distinct
	IdPracownika, 
	DATEADD(ms, -DATEPART(ms, Od), Od)

from PlanUrlopow 
order by 1 desc


select 
	distinct
	IdPracownika, 
	case when DATEPART(ms, Od) >= 500 then
		 DATEADD(ms, 1000-DATEPART(ms, Od), Od)
	else DATEADD(ms, -DATEPART(ms, Od), Od)
	end  
from PlanUrlopow 
order by 1 desc
         
         */


        //----- weryfikacja -----
        public bool Check()
        {
            /*
            const string zmfmt = "Dzień: {0}, zmiana: {1} ({2}-{3})\\n";
            const string brfmt = "Przerwa: {0}h\\n";
            const int defPrzerwa11 = 11;
            const int defPrzerwa24 = 24;
            const int defPrzerwa35 = 35;
            const int s86400 = 86400;  // doba
            const int s3600 = 3600;    // godzina

            if (FMode == moZmiany)  // na wszelki wypadek, mozna by sprawdzic jeszcze czy okres nie jest aby zamkniety, bo jezeli jestem w trakcie edycji, a hr zamknie okres to i tak nadpiszę !!!
            {
                TimeSumator zz = new TimeSumator(); // do wyszukiwania zmian
                string msg = null;
                //int przerwaD = -1;
                //int przerwaW = -1;
                int czasNom = Worktime.GetIloscDniPrac(null, DateFrom, DateTo) * 8; // il dni w okresie a nie w mies 

                //----- pracownicy -----
                foreach (ListViewItem item in lvPlanPracy.Items)
                {
                    string alg;
                    Tools.GetControlValue(item, "hidAlgorytm", out alg);    // moze być null
                    if (alg != "0")                                         // nie sprawdzam jak nie mam 
                    {
                        string pracId;
                        PlanPracyLine2 pl = (PlanPracyLine2)item.FindControl("PlanPracyLine");
                        if (pl != null)
                        {
                            if (Tools.GetControlValue(item, "hidPracId", out pracId))
                            {
                                List<string> zm = new List<string>();   // tu będą tylko istniejące zmiany
                                string[] zmiany = pl.Zmiany.Split(',');

                                int _daysCount = 0;
                                int przerwa7 = 0;
                                bool zmianaZmian = false;
                                int _zmCnt = 0;  //ilosc zaplanowanych zmian - dni pracujących - to nie to samo co czas !!! (bo moze byc zmiana "tylko nadgodziny" zaplanowana)
                                int _czas = 0;   //godz.
                                DateTime _prevData7 = DateFromIdx(0);
                                DateTime prevData = DateTime.MinValue;
                                string prevZm = null;
                                bool isPrevZmiana = false;
                                DateTime prevZmOd = DateTime.MinValue;
                                DateTime prevZmDo = DateTime.MinValue;
                                //----- fix jak pierwszy dzień okresu jest wolny -----
                                int fix = 0;
                                bool isHoliday = Tools.GetLineParam(zmiany[0], 3).Contains(maHoliday);
                                if (isHoliday)
                                {
                                    bool nextHoliday = Tools.GetLineParam(zmiany[1], 3).Contains(maHoliday);
                                    if (!nextHoliday)
                                    {
                                        string symbol, nazwa;
                                        int czasZm, czasOd, czasDo;
                                        string zid = Tools.GetLineParam(zmiany[0], 0);
                                        bool isZmiana = zz.FindZmiana(zid, out symbol, out nazwa, out czasZm, out czasOd, out czasDo);
                                        if (isZmiana)   // potrzebne do poprawnej kontroli czasu nominalnego
                                        {
                                            DateTime data = DateFromIdx(0);
                                            _zmCnt++;
                                            DateTime zmOd = data.AddSeconds(czasOd);
                                            DateTime zmDo = data.AddSeconds(czasDo + (czasDo < czasOd ? s86400 : 0));
                                            int zt = Worktime.CountTime((DateTime)zmOd, (DateTime)zmDo);  //sek
                                            _czas += zt / 3600;  //godziny, zmiany zawsze są w godzinach                                        
                                            _daysCount++;
                                        }
                                        fix = 1;
                                    }
                                }
                                //----- kolejne dni -----
                                //for (int _i = 0; _i < zmiany.Count(); _i++)
                                for (int _i = fix; _i < zmiany.Count(); _i++)
                                {
                                    DateTime data = DateFromIdx(_i);
                                    //----- reset parametrów przed pierwszym dniem tygodnia rozliczeniowego -----
                                    if ((_i - fix) % 7 == 0)
                                    {
                                        _daysCount = 0;
                                        _prevData7 = data;
                                        przerwa7 = 0;
                                        zmianaZmian = false;
                                    }
                                    //----- ustawienie patrametrów -----
                                    string zid = Tools.GetLineParam(zmiany[_i], 0);
                                    string symbol, nazwa;
                                    int czasZm, czasOd, czasDo;
                                    DateTime zmOd = DateTime.MinValue;
                                    DateTime zmDo = DateTime.MinValue;
                                    bool isZmiana = zz.FindZmiana(zid, out symbol, out nazwa, out czasZm, out czasOd, out czasDo);
                                    if (isZmiana)
                                    {
                                        _zmCnt++;
                                        zmOd = data.AddSeconds(czasOd);
                                        zmDo = data.AddSeconds(czasDo + (czasDo < czasOd ? s86400 : 0));
                                        int zt = Worktime.CountTime((DateTime)zmOd, (DateTime)zmDo);  //sek
                                        _czas += zt / 3600;  //godziny, zmiany zawsze są w godzinach                                        
                                        _daysCount++;
                                    }
                                    //----- 11h -----
                                    if (isZmiana)
                                    {
                                        if (prevZmDo != DateTime.MinValue)
                                        {
                                            int sec = Worktime.CountDateTimeSec(prevZmDo, zmOd);
                                            if (sec < defPrzerwa11 * s3600)
                                            {
                                                string prev = String.Format(zmfmt, Base.DateToStr(prevData), prevZm, prevZmOd.Hour, prevZmDo.Hour);
                                                string prz = String.Format(brfmt, Worktime.SecToHour(sec));
                                                string curr = String.Format(zmfmt, Base.DateToStr(data), symbol, Worktime.SecToHour(czasOd), Worktime.SecToHour(czasDo));
                                                msg = "Pracownik: " + Worker.GetNazwiskoImie(pracId) + "\\n" +
                                                      prev +
                                                      curr +
                                                      prz + "\\n" +
                                                      String.Format("Przerwa dobowa krótsza niż {0}h.", defPrzerwa11);
                                                Tools.ShowMessage(msg);
                                                return false;       // >>>>>>>>>>>>>>>
                                            }
                                        }
                                    }
                                    //----- 35h lub 24h -----
                                    int p;
                                    if (isZmiana)
                                    {
                                        if (isPrevZmiana && symbol != prevZm) zmianaZmian = true;
                                        if (przerwa7 == 0 || !isPrevZmiana) // String.IsNullOrEmpty(prevZm))  // reset co 7 dni ???
                                            p = Worktime.CountDateTimeSec(_prevData7, zmOd);
                                        else
                                            p = Worktime.CountDateTimeSec(prevZmDo, zmOd);
                                        if (p > przerwa7) przerwa7 = p;
                                    }
                                    if (((_i - fix) + 1) % 7 == 0)   // ostatni dzień z 7
                                    {
                                        if (!isZmiana)      // nie ma zmiany 
                                        {
                                            if (przerwa7 == 0 || !isPrevZmiana) // String.IsNullOrEmpty(prevZm))  // do końca dnia
                                                p = Worktime.CountDateTimeSec(_prevData7, data.AddDays(1));  // -1 sek ???
                                            else
                                                p = Worktime.CountDateTimeSec(prevZmDo, data.AddDays(1));
                                            if (p > przerwa7) przerwa7 = p;
                                        }
                                        if (zmianaZmian)
                                        {
                                            if (przerwa7 < defPrzerwa24 * s3600)
                                            {
                                                msg = "Pracownik: " + Worker.GetNazwiskoImie(pracId) + "\\n" +
                                                      String.Format("Okres: {0} - {1} (7 dni)\\n\\n", Base.DateToStr(_prevData7), Base.DateToStr(data)) +
                                                      String.Format("Przerwa tygodniowa krótsza niż {0}h (jest zmiana zmiany).", defPrzerwa24);
                                                Tools.ShowMessage(msg);
                                                return false;       // >>>>>>>>>>>>>>>
                                            }
                                        }
                                        else
                                            if (przerwa7 < defPrzerwa35 * s3600)
                                            {
                                                msg = "Pracownik: " + Worker.GetNazwiskoImie(pracId) + "\\n" +
                                                      String.Format("Okres: {0} - {1} (7 dni)\\n\\n", Base.DateToStr(_prevData7), Base.DateToStr(data)) +
                                                      String.Format("Przerwa tygodniowa krótsza niż {0}h (zmiana zmiany nie występuje).", defPrzerwa35);
                                                Tools.ShowMessage(msg);
                                                return false;       // >>>>>>>>>>>>>>>
                                            }
                                    }
                                    //----- 5 dni na 7-----
                                    if (((_i - fix) + 1) % 7 == 0)   // ostatni dzień z 7   !!!! tu sprawdzić z fix czy nie psuje
                                    {
                                        //if (daysCount > 5)
                                        if (_daysCount > 6)
                                        {
                                            msg = "Pracownik: " + Worker.GetNazwiskoImie(pracId) + "\\n" +
                                                  String.Format("Okres: {0} - {1} (7 dni)\\nZaplanowana ilość dni pracy: {2}\\n\\n", Base.DateToStr(_prevData7), Base.DateToStr(data), _daysCount) +
                                                //"Przekroczono dopuszczalny czas pracy (5 dni).";
                                                  "Przekroczono dopuszczalny maksymalny czas pracy (6 dni).";
                                            Tools.ShowMessage(msg);
                                            return false;       // >>>>>>>>>>>>>>>
                                        }
                                    }
                                    //----- wolne za 6 dni ciurkiem -----

                                    //----- naruszenie doby pracowniczej (next day nie mozna zacząć wcześniej) ----- 
                                    if (isZmiana && prevZmDo != DateTime.MinValue)      // jest zmiana i zmiana wczesniejsza
                                    {
                                        int sec1 = Worktime.CountDateTimeSec(prevZmOd, zmOd);
                                        if (sec1 < s86400)   // nie może zacząć wcześniej niż doba upłynie
                                        {
                                            string prev = String.Format(zmfmt, Base.DateToStr(prevData), prevZm, prevZmOd.Hour, prevZmDo.Hour);
                                            string curr = String.Format(zmfmt, Base.DateToStr(data), symbol, Worktime.SecToHour(czasOd), Worktime.SecToHour(czasDo));
                                            msg = "Pracownik: " + Worker.GetNazwiskoImie(pracId) + "\\n" +
                                                  prev +
                                                  curr + "\\n" +
                                                  "Naruszenie doby pracowniczej.";
                                            Tools.ShowMessage(msg);
                                            return false;       // >>>>>>>>>>>>>>>
                                        }
                                    }
                                    //----- dzień wolny za niedzielę -----

                                    //----- ustawienia parametrów -----
                                    if (isZmiana)
                                    {
                                        isPrevZmiana = true;
                                        prevData = data;
                                        prevZm = symbol;
                                        prevZmOd = zmOd;
                                        prevZmDo = zmDo;
                                    }
                                    //----- error info -----
                                    if (!String.IsNullOrEmpty(msg))
                                    {
                                        Tools.ShowMessage(msg);
                                        return false;           //>>>>>>>>>>>>>>>>
                                    }
                                }
                                //----- czas nominalny -----    // UWAGA - póki co nie sprawdzam tych nie wklikanych 
                                if (_zmCnt > 0 && _czas != czasNom)    // tu wyłapię też takich, dla których K nie ustawiał zmian
                                {
                                    Tools.ShowMessage(
                                          "Pracownik: {0}\\n" +
                                          "Czas zaplanowany ({1}h) nie zgadza się z czasem nominalnym ({2}h).",
                                          Worker.GetNazwiskoImie(pracId), _czas, czasNom);
                                    return false;       // >>>>>>>>>>>>>>> 
                                }
                            }
                        }
                    }
                }
            }
            */



            return true;  // wszystko ok
        }
        
        
        
        
        
        
        
        //----------
        protected void lvPlanPracy_Unload(object sender, EventArgs e)
        {
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

        //----------------
        protected void OnSelectUrlop(object sender, EventArgs e)
        {
            SelectedUrlop = cntSelectUrlop.SelectedUrlop;
        }

        protected void cbKorekta_CheckedChanged(object sender, EventArgs e)
        {
            if (IsAdmin)
                Korekta = cbKorekta.Checked;
        }

        protected void ddlHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlHistory.SelectedIndex == 0)
                Reload(false);
            else
                Reload(ddlHistory.SelectedValue, false);
        }

        protected void btCheckPP_Click(object sender, EventArgs e)
        {
            /*
            //bool empty
            if (cntPlanPracy.Check())
                Tools.ShowMessage("Plan pracy poprawny.");
             * */
        }

        protected void btEditPP_Click(object sender, EventArgs e)
        {
            SetEditMode(true);
        }

        protected void btSavePP_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                Update();
                SetEditMode(false);

                if (ShowHistory) ddlHistory.DataBind();
                Reload(Tools.DateTimeToStr(DateTime.Now.AddSeconds(1)), false);

                if (DataSaved != null)
                    DataSaved(this, EventArgs.Empty);
            }
        }

        protected void btCancelPP_Click(object sender, EventArgs e)
        {
            SetEditMode(false);
            Reload(false);
        }
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

        public string _Rok
        {
            get { return hidRokBoy.Value; }
            set { hidRokBoy.Value = value; }
        }

        public string Czas
        {
            get { return hidCzas.Value; }
            set { hidCzas.Value = value; }
        }

        public bool EditMode
        {
            get { return cntSelectUrlop.EditMode; }  // nazwa powinna byc zmieniona na selecturlop/absencja
            set
            {
                cntSelectUrlop.EditMode = value;
                //lvPlanPracy.DataBind();
                //PrepareDataSource();
                DataBind();
            }
        }

        public bool Editable
        {
            set { ViewState["edit"] = value; }
            get { return Tools.GetBool(ViewState["edit"], false); }
        }

        public bool Korekta
        {
            set { ViewState["kor"] = value; }
            get { return Tools.GetBool(ViewState["kor"], false); }
        }

        public string SelectedUrlop
        {
            set { hidSelUrlop.Value = value; }   // odwołania są też w js
            get { return hidSelUrlop.Value; }
        }

        public bool Portal
        {
            set { ViewState["portal"] = value; }
            get { return Tools.GetBool(ViewState["portal"], false); }
        }

    }
}


