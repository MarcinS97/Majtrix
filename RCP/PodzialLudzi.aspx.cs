using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.Controls.Reports;
using HRRcp.Controls.PodzialLudzi;
using System.Data;
using System.Data.SqlClient;

namespace HRRcp
{
    public partial class PodzialLudzi : System.Web.UI.Page
    {
        const string title = "Podział Ludzi";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool adm = App.User.HasRight(AppUser.rPodzialLudziAdm);
                bool PL = App.User.HasRight(AppUser.rPodzialLudzi);
                if (adm || PL)
                {
                    Tools.SetNoCache();

                    bool err = true;
                    string[] par = cntReport2.GetParams();
                    if (par != null && par.Length >= 3)
                    {
                        string d1 = par[0];
                        string d2 = par[1];
                        int oid = Tools.StrToInt(par[2], -1);
                        if (oid > 0 && Tools.DateIsValid(d1) && Tools.DateIsValid(d2))
                        {
                            DataOd = d1;
                            DataDo = d2;
                            Okres ok = new Okres((DateTime)Tools.StrToDateTime(d2));

                            bool s0, s1, s2, s3;
                            s0 = ok.StatusPL == cntPodzialLudzi.stNone;
                            s1 = ok.StatusPL == cntPodzialLudzi.stOpen;
                            s2 = ok.StatusPL == cntPodzialLudzi.stOpenNoImport;
                            s3 = ok.StatusPL == cntPodzialLudzi.stClosed;

                            string mies = d2.Substring(0, 7);
                            lbTitle.Text = String.Format("Podział Ludzi {0} - {1}", mies, cntPodzialLudzi.GetStatus(ok.StatusPL));
                            if (DateTime.Today > ok.DateTo)
                                deNaDzien.Date = ok.DateTo;
                            else
                                deNaDzien.Date = DateTime.Today;        // import splitów można robić w dowolnym momencie

                            ok.Next();                                  // nie używać dalej ok bo to już następny !!! 
                            bool noo = ok.Status != Okres.stClosed;     // && ok.StatusPL == cntPodzialLudzi.stNone;     // następny okres otwarty i nie ma jeszcze splitów ??? <<< wystarczy ze otwarty, PRZEJRZEC ustawianie DataDo w imporcie !!!

                            bool edS = App.User.HasRight(AppUser.rPodzialLudziEditS);   // to powinno uwzględniać jeszcze akceptację splitów 
                            bool a = noo && adm;                        // tylko jak następny okres jest otwarty to mogę importować
                            bool imp = a && s1; //(s0 || s1); 20160228 
                            bool fte = a && (s1 || s2);                 // importuj FTE, stanowisko itp. + nowe osoby
                            bool bck = a && (s0 || s1 || s2);           // na s0 moge zrobic backup "sprzed"
                            bool exp = a && (s1 || s2 || s3);
                            bool ed  = a && (s1 || s2) || noo && edS && (s1 || s2);

                            if (imp)
                                Tools.MakeConfirmButton(btImport, 
                                    String.Format("Potwierdź przeliczenie splitów i import danych za miesiąc {0}.\\n\\nUwaga !!!\\nDotychczasowe dane zostaną nadpisane.", mies));
                            if (fte)
                                Tools.MakeConfirmButton(btImportFTE, 
                                    String.Format("Potwierdź aktualizację danych za miesiąc {0}.\\nSplity nie będą nadpisywane.", mies));
                            if (bck)
                                Tools.MakeConfirmButton(btBackup, 
                                    String.Format("Potwierdź wykonanie backupu splitów za miesiąc {0}.", mies));
                            if (exp)
                                Tools.MakeConfirmButton(btExport, 
                                    String.Format("Potwierdź przeliczenie kosztów za miesiąc {0}.", mies));

                            /*  20160227
                            if (imp)
                                Tools.MakeButton(btImport, String.Format("javascript:var c = confirm('{0}');if (c) showAjaxProgress();return c;",
                                    String.Format("Potwierdź przeliczenie splitów i import danych za miesiąc {0}.\\n\\nUwaga !!!\\nDotychczasowe dane zostaną nadpisane.", mies)));
                            if (fte)
                                Tools.MakeButton(btImportFTE, String.Format("javascript:var c = confirm('{0}');if (c) showAjaxProgress();return c;",
                                    String.Format("Potwierdź aktualizację danych za miesiąc {0}.\\nSplity nie będą nadpisywane.", mies)));
                            if (bck)
                                Tools.MakeButton(btBackup, String.Format("javascript:var c = confirm('{0}');if (c) showAjaxProgress();return c;",
                                    String.Format("Potwierdź wykonanie backupu splitów za miesiąc {0}.", mies)));
                            if (exp)
                                Tools.MakeButton(btExport, String.Format("javascript:var c = confirm('{0}');if (c) showAjaxProgress();return c;",
                                    String.Format("Potwierdź przeliczenie kosztów za miesiąc {0}.", mies)));
                             */


                            btBackup.Visible = bck;
                            deNaDzien.Visible = imp;
                            btImport.Visible = imp;
                            btImportFTE.Visible = fte;  
                            btExport.Visible = exp;
                            cntPodzialLudzi.Prepare(d1, d2, ed, imp, deNaDzien);
                            err = false;
                        }
                    }
                    if (err)
                        AppError.Show(title, "Niepoprawne parametry wywołania.");
                }
                else
                    App.ShowNoAccess(title, App.User);
            }
        }

        /*
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool adm = App.User.HasRight(AppUser.rPodzialLudziAdm);
                bool PL = App.User.HasRight(AppUser.rPodzialLudzi);
                if (adm || PL)
                {
                    Tools.SetNoCache();

                    //App_Code.App.CheckccPrawaSplity(cntReport1);
                    //cntReport1.SQL1 = null;
                    //cntReport1.SQL3 = null;
                    //cntReport1.ExportCSV(null, true);

                    bool err = true;
                    string[] par = cntReport2.GetParams();
                    if (par != null && par.Length >= 3)
                    {
                        string d1 = par[0];
                        string d2 = par[1];
                        int oid = Tools.StrToInt(par[2], -1);
                        if (oid > 0 && Tools.DateIsValid(d1) && Tools.DateIsValid(d2))
                        {
                            Okres ok = new Okres((DateTime)Tools.StrToDateTime(d2));
                            string mies = d2.Substring(0, 7);
                            lbTitle.Text = String.Format("Podział Ludzi {0} - {1}", mies, cntPodzialLudzi.GetStatus(ok.StatusPL));
                            bool imp = false;
                            bool ed = false;
                            bool edS = App.User.HasRight(AppUser.rPodzialLudziEditS);
                            bool noimp = ok.StatusPL == cntPodzialLudzi.stOpenNoImport;   //
                            if (adm || edS)
                            {
                                bool open = ok.Status == Okres.stOpen;   // to powinno uwzględniać jeszcze akceptację splitów 
                                imp = adm && open;
                                ed = edS && open;
                                if (imp)
                                {
                                    /*
                                                 d1                            d2
                                                 1234567890123456789012345678901
                                         -----P------P------P------P------P------P------P---
                                    today:  A      B             C                   D
                                      
                                     * A - blokada
                                     * B - @do = B
                                     * C - @do = bow(C) - 1
                                     * D - @do = d2
                                     
                                     * /
                                    
                                    DateTime dt1 = (DateTime)Tools.StrToDateTime(d1);    
                                    DateTime dt2 = (DateTime)Tools.StrToDateTime(d2);    
                                    if (DateTime.Today < dt1)   // A 
                                    {
                                        imp = false;
                                        ed = false;
                                    }
                                    else
                                    {
                                        /*  poniedziałek
                                        DateTime day;
                                        if (DateTime.Today > dt2) day = dt2;     // D
                                        else                    
                                        {
                                            day = Tools.bow(DateTime.Today, DayOfWeek.Monday).AddDays(-1);    // C
                                            if (day < dt1) day = DateTime.Today;    // B
                                        }
                                        deNaDzien.Date = day;
                                        * /
                                        deNaDzien.Date = DateTime.Today;   // import splitów można robić w dowolnym momencie
                                    }
                                }
                            }
                            if (imp)
                            {
                                //Tools.MakeConfirmButton(btImport1, String.Format("Potwierdź przeliczenie splitów i import danych za miesiąc {0}.\\n\\nUwaga !!!\\nDotychczasowe dane zostaną nadpisane.", mies));
                                Tools.MakeButton(btImport, String.Format("javascript:var c = confirm('{0}');if (c) showAjaxProgress();return c;",
                                    String.Format("Potwierdź przeliczenie splitów i import danych za miesiąc {0}.\\n\\nUwaga !!!\\nDotychczasowe dane zostaną nadpisane.", mies)));
                                Tools.MakeButton(btBackup, String.Format("javascript:var c = confirm('{0}');if (c) showAjaxProgress();return c;",
                                    String.Format("Potwierdź wykonanie backupu splitów za miesiąc {0}.", mies)));
                            }

                            btImport.Visible = imp && !noimp;
                            btBackup.Visible = imp;
                            deNaDzien.Visible = imp;
                            cntPodzialLudzi.Prepare(d1, d2, ed);
                            err = false;
                        }
                    }
                    if (err)
                        AppError.Show(title, "Niepoprawne parametry wywołania.");
                }
                else
                    App.ShowNoAccess(title, App.User);
            }
        }
         */

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(title);
        }


        //-----------------------------------------
        protected void btExcel_Click(object sender, EventArgs e)
        {
            string filename = String.Format("{0} - {1}", lbTitle.Text, cntPodzialLudzi.SelectedPodzial);
            //Report.ExportExcel(hidReport.Value, filename, null);

            Report.ExportCSV(filename, cntPodzialLudzi.DataSource, null, null, true, false);
        }

        private static void ShowImportError(int err)
        {
            switch (err)
            {
                case 0:
                    Tools.ShowMessageLog(Log.PL_IMPORT, "Import Podział Ludzi - OK", "Import zakończony poprawnie.");
                    break;
                case -1:
                    Tools.ShowError("Niepoprawna data importu.");
                    break;
                default:
                    Tools.ShowErrorLog(Log.PL_IMPORT, "Import Podział Ludzi - ERROR", String.Format("Wystąpił błąd podczas importu danych. Kod: {0}", err));
                    break;
            }
        }

        protected void btImport_Click(object sender, EventArgs e)
        {
            int err = Import(3);
            ShowImportError(err);
        }

        protected void btImportFTE_Click(object sender, EventArgs e)
        {
            int err = Import(2);
            switch (err)
            {
                case 0:
                    Tools.ShowMessageLog(Log.PL_IMPORT, "Import Podział Ludzi FTE - OK", "Import zakończony poprawnie.");
                    break;
                case -1:
                    Tools.ShowError("Niepoprawna data importu.");
                    break;
                default:
                    Tools.ShowErrorLog(Log.PL_IMPORT, "Import Podział Ludzi FTE - ERROR", String.Format("Wystąpił błąd podczas importu danych. Kod: {0}", err));
                    break;
            }
        }

        private bool CheckNaDzien()
        {
            object o = deNaDzien.Date;
            if (o == null) Tools.ShowMessage("Niepoprawna data.");
            else if (!Tools.DateIsValid(DataOd) || !Tools.DateIsValid(DataDo)) Tools.ShowMessage("Niepoprawne parametry wywołania (okres od-do).");
            else
            {
                DateTime dt = (DateTime)o;
                DateTime dt1 = Tools.StrToDateTime(DataOd, DateTime.MinValue);
                DateTime dt2 = Tools.StrToDateTime(DataDo, DateTime.MinValue);
                if (dt < dt1 || dt2 < dt) Tools.ShowMessage("Data musi być z zakresu {0} - {1}.", DataOd, DataDo);
                else return true;
            }
            return false;
        }

        private static bool DoExport(Okres ok)
        {
            Log.Info(Log.PL_PRZELICZ, "Podział Ludzi - Przeliczenie danych", ok.OdDoStr);
            return Asseco.ExportRCP(ok, false, false, false, true, false);
        }

        protected void btExport_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.MinValue;
            Okres ok = null;
            bool dok = Tools.DateIsValid(DataDo);
            if (dok) 
            {
                dt = Tools.StrToDateTime(DataDo, DateTime.MinValue);
                if (dt == DateTime.MinValue) dok = false;
                else
                {
                    ok = new Okres(dt);
                    dok = ok != null;
                }
            }
            if (dok)
            {
                if (DoExport(ok))
                    Tools.ShowMessage("Dane z okresu {0} zostały przeliczone.", ok.OdDoStr);
                else
                    Tools.ShowMessage("Wystąpił błąd podczas przeliczania danych z okresu:\\n{0}.", ok.OdDoStr);
            }
            else
                Tools.ShowMessage("Niepoprawna data.");
        }

        /*
        protected void btExport_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.MinValue;
            Okres ok = null;
            bool dok = Tools.DateIsValid(DataDo);
            if (dok) 
            {
                dt = Tools.StrToDateTime(DataDo, DateTime.MinValue);
                if (dt == DateTime.MinValue) dok = false;
                else
                {
                    ok = new Okres(dt);
                    dok = ok != null;
                }
            }
            if (dok)
            {
                Log.Info(Log.PL_PRZELICZ, "Podział Ludzi - Przeliczenie danych", ok.OdDoStr);
                if (Asseco.ExportRCP(ok, false, false, false, true, false))
                    Tools.ShowMessage("Dane z okresu {0} zostały przeliczone.", ok.OdDoStr);
                else
                    Tools.ShowMessage("Wystąpił błąd podczas przeliczania danych z okresu:\\n{0}.", ok.OdDoStr);
            }
            else 
                Tools.ShowMessage("Niepoprawna data.");
        }
         */

        protected void btBackup_Click(object sender, EventArgs e)
        {
            string name;
            int err = Backup(out name);
            switch (err)
            {
                case 0:
                    Tools.ShowMessageLog(Log.PL_IMPORT, "Backup Podział Ludzi - OK", String.Format("Backup wykonany poprawnie. Nazwa: {0}.", name));
                    break;
                default:
                    Tools.ShowErrorLog(Log.PL_IMPORT, "Backup Podział Ludzi - ERROR", String.Format("Wystąpił błąd podczas wykonywania backupu danych. Nazwa: {1} Kod: {0}", err, name));
                    break;
            }
        }

        //-----------------------------------------
        public string DataOd
        {
            set { ViewState["dataOd"] = value; }
            get { return Tools.GetStr(ViewState["dataOd"]); }
        }

        public string DataDo
        {
            set { ViewState["dataDo"] = value; }
            get { return Tools.GetStr(ViewState["dataDo"]); }
        }
        
        //-----------------------------------------
        public static DataSet GetSplity(string pracId, string kierId, DateTime naDzien)   // dla importu w kontrolce cntsplityWsp2
        {
            DataSet ds = db.getDataSet(SQL.GetSplity(pracId, kierId, naDzien));

            /*
            DataSet ds = db.getDataSet(String.Format(@"
declare @pracId int
declare @kid int
set @pracId = {0}
set @kid = {1}

----- SplityWsp -----
--drop table #www

declare	@od datetime
declare	@do datetime
set @do = '{2}'
set @od = dbo.bom(@do)

declare @cnt int
set @cnt = DATEDIFF(DD, @od, @do) + 1 - (select COUNT(*) from Kalendarz where Data between @od and @do) 

declare @ccSurplus int
select @ccSurplus = Id from CC where cc = '499'

select D.IdPracownika, D.IdSplitu, D.IdCC, ROUND(D.SumWsp / D.Dni, 4) as Wsp
into #www
from
(
select 
	R.IdPracownika, SPL.Id as IdSplitu, W.IdCC, SUM(ISNULL(W.Wsp, 0)) as SumWsp, 
	(
	select count(*) from dbo.GetDates2(@od, @do) D2
	inner join Przypisania R2 on D2.Data between R2.Od and ISNULL(R2.Do, '20990909') and R2.Status = 1 and R2.IdPracownika = R.IdPracownika		-- okres zatrudnienia
	left join Kalendarz K2 on K2.Data = D2.Data	
	where K2.Data is null -- dni robocze    
	) as Dni
from dbo.GetDates2(@od, @do) D
left join Kalendarz K on K.Data = D.Data   -- powinno iść po zmianach bo wolne za święto
inner join Przypisania R on D.Data between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1		-- okres zatrudnienia

	and (@pracId is null or R.IdPracownika = @pracId)

left join Pracownicy P on P.Id = R.IdPracownika
left join Splity SPL on SPL.GrSplitu = P.KadryId and @do between SPL.DataOd and ISNULL(SPL.DataDo, '20990909')
left join SplityWspP W on W.IdPrzypisania = R.Id
where K.Data is null and P.KadryId < 80000 and P.Status >= -1
group by R.IdPracownika, SPL.Id, W.IdCC
) D
order by IdPracownika, IdSplitu, IdCC

-- brak wsp lub niedomiar do 1
insert into #www
select IdPracownika, IdSplitu, @ccSurplus, ROUND(1 - SUM(Wsp), 4)  
from #www group by IdPracownika, IdSplitu having ROUND(SUM(Wsp), 4) <> 1

delete from #www where IdCC is null

----- SplityWsp - import -----
--select * from SplityWsp
--insert into SplityWsp
--select * from #www W

------------------------------------
----- format dla cntSplityWsp2 -----
select 
--W.Id, 
null as Id,
W.IdCC, CC.cc + ' - ' + CC.Nazwa as cc, convert(varchar, W.Wsp) as Wsp,
case when @kid = -99 or R.Id is not null then 1 else 0 end as MojeCC,
dbo.fn_GetccPrawaKierList(CC.Id, 1, ',') as KierList
--from SplityWsp W
from #www W
inner join CC on CC.Id = W.IdCC and CC.Surplus = 0
left join ccPrawa R on R.IdCC = W.IdCC and R.UserId = @kid
                ", pracId, kierId, Tools.DateToStrDb(naDzien)));
            */
            return ds;
        }


        //-----------------------------------------
        private int Import(int mode)   // 3 - all, 2 - aktualizacja danych, 1 - import splitów
        {
            const string PL = "PodzialLudzi.Import - ";
            Log.Info(Log.PL_IMPORT, 0, PL + "START", null, Log.OK);
            //-----------------------------
            string dOd = cntPodzialLudzi.DataOd;
            string dDo = cntPodzialLudzi.DataDo;
            DateTime day;
            int err = -1;
            object o = deNaDzien.Date;
            if (o != null)
            {
                day = (DateTime)o;
                err = DoImport(PL, mode, dOd, dDo, day);
                if (err == 0) cntPodzialLudzi.DataBind();
            }
            //-----------------------------
            if (err == 0)
                Log.Info(Log.PL_IMPORT, 0, PL + "OK", null, Log.OK);
            else
                Log.Error(Log.PL_IMPORT, 0, PL + "ERROR", String.Format("Kod: {0}", err));
            return err;
        }
        
        public static int ImportOnOpen(int mode, string dOd, string dDo, DateTime naDzien)   // 3 - all, 2 - aktualizacja danych
        {
            const string PL = "PodzialLudzi.ImportOnOpen - ";
            Log.Info(Log.PL_IMPORT, 0, PL + "START", null, Log.OK);
            //-----------------------------
            int err = DoImport(PL, mode, dOd, dDo, naDzien);
            //-----------------------------
            if (err == 0)
                Log.Info(Log.PL_IMPORT, 0, PL + "OK", null, Log.OK);
            else
                Log.Error(Log.PL_IMPORT, 0, PL + "ERROR", String.Format("Kod: {0}", err));
            ShowImportError(err);
            return err;
        }

        public static int AutoImport(int logParentId)  // import automatyczny do uruchomienia w Service
        {
            const string PL = "PodzialLudzi.AutoImport - ";
            Log.Info(Log.PL_IMPORT, logParentId, PL + "START", null, Log.OK);
            //-----------------------------
            int err = 0;
            string data = SQL.GetData();
            DataRow dr = db.getDataRow(data);
            if (dr != null)
            {
                DateTime dod = (DateTime)db.getDateTime(dr, "Od");
                DateTime ddo = (DateTime)db.getDateTime(dr, "Do");
                string sod = Tools.DateToStr(dod);
                string sdo = Tools.DateToStr(ddo);

                DateTime naDzien = (DateTime)db.getDateTime(dr, "NaDzien");
                int impSplity   = db.getInt(dr, "ImpSplity", 0);
                bool expAsseco  = db.getBool(dr, "ExpAsseco", false);
                bool backup     = db.getBool(dr, "DoBackup", false);

                Log.Info(Log.PL_IMPORT, logParentId, PL + "PARAMETRY", String.Format("Od: {0} Do: {1} NaDzien: {2} Backup: {3} Splity: {4} Export: {5}", 
                    sod, sdo, Tools.DateToStrDb(naDzien), backup ? 1 : 0, impSplity, expAsseco ? 1 : 0), Log.OK);

                if (backup)
                {
                    string name;
                    err = Backup(out name, sod, sdo);
                }
                if (err == 0 && impSplity != 0)
                {
                    err = DoImport(PL, impSplity, sod, sdo, naDzien);
                }
                if (err == 0 && expAsseco)
                {
                    err = -4;
                    Okres ok = new Okres(ddo);
                    if (ok != null)
                        if (DoExport(ok))
                            err = 0;
                }
            }
            else
                err = -5;
            //-----------------------------
            if (err == 0)
                Log.Info(Log.PL_IMPORT, logParentId, PL + "OK", null, Log.OK);
            else
                Log.Error(Log.PL_IMPORT, logParentId, PL + "ERROR", String.Format("Kod: {0}", err));
            return err;
        }

        private static int DoImport(string logInfo, int mode, string dOd, string dDo, DateTime naDzien)   // mode: 1 (I), 2 (II), 3 (I+II)
        {
            DateTime dt1 = (DateTime)Tools.StrToDateTime(dOd);
            DateTime dt2 = (DateTime)Tools.StrToDateTime(dDo);

            if (naDzien < dt1 || naDzien > dt2)
                return -1;
            else
            {
                dDo = Tools.DateToStrDb(naDzien);
                DataSet ds = db.getDataSet(SQL.Import(mode, dOd, dDo));     // tu jest import
                DataRow dr = db.getRow(ds);                                 // kody błędów
                int err = db.getInt(dr, 0, -1);
                string msg = db.getValue(dr, 1);
                int step = db.getInt(dr, 2, 0);
                if (err == 0)
                {
                    Log.Info(Log.PL_IMPORT, logInfo + "OK", String.Format("step: {0}", step));
                    if (mode == 1 || mode == 3)
                    {
                        int ret = execProc("importPLSplity", null, dt1, dt2);
                        if (ret != 0) return -2;
                    }
                    if (mode == 2 || mode == 3)
                    {
                        int ret = execProc("importPLFTE", null, dt1, dt2);
                        if (ret != 0) return -3;
                    }
                    return 0; // ok !!!
                }
                else
                {
                    Log.Error(Log.PL_IMPORT, logInfo + "ERROR", String.Format("err: {0} step: {1} msg: {2}", err, step, msg));
                    return -4;
                }
            }
        }




















































        private int x_Import(int mode)   // 1 - all, 2 - aktualizacja danych
        {
            string dOd = cntPodzialLudzi.DataOd;
            string dDo = cntPodzialLudzi.DataDo;
            DateTime dt1 = (DateTime)Tools.StrToDateTime(dOd);
            DateTime dt2 = (DateTime)Tools.StrToDateTime(dDo);
            DateTime day;
            object o = deNaDzien.Date;
            if (o == null) return -1;
            else
            {
                day = (DateTime)o;
                if (day < dt1 || day > dt2)
                    return -1;
            }
            dDo = Tools.DateToStrDb(day);




            //return 0;
            bool ok = true;
            if (mode == 1)
            {
                //ok = db.execSQL(SQL.ImportPL1(dOd, dDo));


                /*
                ok = db.execSQL(String.Format(@"
declare	@od datetime
declare	@do datetime
set @od = '{0}'	-- dla cc: zakres od bo mogą być cc wyłączone 	, jak działa wszystko ok, to taka sam jak @p1
set @do = '{1}'

declare @cnt int
set @cnt = DATEDIFF(DD, @od, @do) + 1 - (select COUNT(*) from Kalendarz where Data between @od and @do) 

declare @ccSurplus int
select @ccSurplus = Id from CC where cc = '499'

----- PodzialLudziImport -----
update PodzialLudziImport set OkresDo = DATEADD(D, -1, @od) where OkresOd = DATEADD(M, -1, @od) and OkresDo is null
delete from PodzialLudziImport where OkresOd = @od and OkresDo is null

--select * from PodzialLudziImport where KadryId = '00001' order by OkresOd desc

insert into PodzialLudziImport
select @od as Miesiac, P.KadryId, P.Nazwisko + ' ' + P.Imie as Pracownik, S.Nazwa as Stanowisko, 
PS.Grupa as TypImport, PS.Klasyfikacja as Class, null as Grade, 1 as FTE, 

case when PO.Id is null then cast(P.EtatL as float) / P.EtatM 
else cast(PO.EtatL as float) / PO.EtatM 
end as Head, 

null as CC, 

@od as OkresOd, null as OkresDo, --case when RR.Id is null then null else dbo.eom(@od) end as OkresDo, 
0 as Status, 
P.DataZatr, convert(varchar(10), P.DataZwol, 20), A.Area, POS.Position, null, ISNULL(PO.Stawka, P.Stawka) as Brutto, 1 as CHwKosztach
from Pracownicy P
left join Przypisania R on R.IdPracownika = P.Id and @do between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1
left join PracownicyStanowiska PS on PS.IdPracownika = P.Id and @do between PS.Od and ISNULL(PS.Do, '20990909')
left join Stanowiska S on S.Id = PS.IdStanowiska
left join Commodity C on C.Id = R.IdCommodity
left join Area A on A.Id = R.IdArea
left join Position POS on POS.Id = R.IdPosition
left join OkresyRozl RR on @od between RR.DataOd and RR.DataDo and RR.Archiwum = 1
left join PracownicyOkresy PO on PO.Id = P.Id and PO.IdOkresu = RR.Id
where P.KadryId < 80000 and P.Status >= -1 and P.Id in (select IdPracownika from Przypisania where Od <= @do and @od <= ISNULL(Do, '20990909') and Status = 1)

--kontrola TypImport !!!
--select * from PodzialLudziImport where TypImport is null and DataZatr <> '20141201'


----- Splity -----
--select * from Splity
--update Splity set DataDo = DATEADD(D, -1, @od) where DataOd = DATEADD(M, -1, @od) and DataDo is null
update Splity set DataDo = DATEADD(D, -1, @od) where DataDo is null 
and Id in  
(
select S.Id 
from PodzialLudziImport I 
outer apply (select top 1 * from Splity where GrSplitu = I.KadryId and DataOd < @od order by DataOd desc) S
where I.OkresOd = @od
) 


delete from SplityWsp where IdSplitu in 
    (select Id from Splity where DataOd = @od and DataDo is null 
        and GrSplitu not in 
            (select GrSplitu from CC where GrSplitu is not null) 
    ) 
delete from Splity where DataOd = @od and DataDo is null
    and GrSplitu not in 
        (select GrSplitu from CC where GrSplitu is not null) 


insert into Splity 
select I.KadryId, 'Split ' + I.KadryId + ' ' + I.Pracownik, @od, null, 0
from PodzialLudziImport I
where OkresOd = @od

----- SplityWsp -----
--drop table #www
select D.IdPracownika, D.IdSplitu, D.IdCC, ROUND(D.SumWsp / D.Dni, 4) as Wsp
into #www
from
(
select 
	R.IdPracownika, SPL.Id as IdSplitu, W.IdCC, SUM(ISNULL(W.Wsp, 0)) as SumWsp, 
	(
	select count(*) from dbo.GetDates2(@od, @do) D2
	inner join Przypisania R2 on D2.Data between R2.Od and ISNULL(R2.Do, '20990909') and R2.Status = 1 and R2.IdPracownika = R.IdPracownika		-- okres zatrudnienia
	left join Kalendarz K2 on K2.Data = D2.Data	
	where K2.Data is null -- dni robocze    
	) as Dni
from dbo.GetDates2(@od, @do) D
left join Kalendarz K on K.Data = D.Data   -- powinno iść po zmianach bo wolne za święto
inner join Przypisania R on D.Data between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1		-- okres zatrudnienia
left join Pracownicy P on P.Id = R.IdPracownika
left join Splity SPL on SPL.GrSplitu = P.KadryId and @do between SPL.DataOd and ISNULL(SPL.DataDo, '20990909')
left join SplityWspP W on W.IdPrzypisania = R.Id
where K.Data is null and P.KadryId < 80000 and P.Status >= -1
group by R.IdPracownika, SPL.Id, W.IdCC
) D
order by IdPracownika, IdSplitu, IdCC

-- brak wsp lub niedomiar do 1
insert into #www
select IdPracownika, IdSplitu, @ccSurplus, ROUND(1 - SUM(Wsp),4)  
from #www group by IdPracownika, IdSplitu having ROUND(SUM(Wsp),4) <> 1

delete from #www where IdCC is null

----- SplityWsp - import -----
--select * from SplityWsp
insert into SplityWsp
select IdSplitu, IdCC, Wsp from #www W

----- DataSplitow -----
--update OkresyRozl set DataImportu = '{1}' where DataOd = '{0}' 
update OkresyRozl set DataSplitow = '{1}' where DataOd = '{0}' 
---------------------------------------
-- KONIEC
---------------------------------------
                    ", dOd, dDo));
                    */
                if (ok)
                {
                    int ret = execProc("importPLSplity", null, dt1, dt2);
                    ok = ret == 0;
                }
            }
            if (!ok) return -2;
















            //----- II cześć -----
            if (mode == 1 || mode == 2)
            {
                //ok = db.execSQL(SQL.ImportPL2(dOd, dDo));
                /*
                ok = db.execSQL(String.Format(@"
declare @od datetime
declare @do datetime
declare @dzis datetime
declare @boy datetime
declare @nom int

--declare @boy2 datetime
--set @boy2 = DATEADD(Y, -2, @boy)  -- do liczenia ostatni dzień w kosztach
--set @boy2 = @boy

set @od = '{0}'
set @dzis = '{1}'

--set @od = '20150201'
--set @dzis = '20150216'

set @do = dbo.eom(@od)
set @boy = dbo.boy(@od)

declare @stmt nvarchar(max)
--->>>---------------------------
---------------------------------
-- pracownicy > 50 lat - wg daty do - koniec miesiąca chociaz lp_fn_BasePracExLow rownie dobrze byloby wywolac z GETDATE() bo nie ma warunków na zatrudniony
---------------------------------
IF OBJECT_ID('tempdb..#prac50') IS NOT NULL DROP TABLE #prac50
create table #prac50(
	LpLogo varchar(20) not null,
	DataUrodzenia datetime not null,
	Wiek int not null,
	Wiek2 int not null
)

set @stmt = '
select * 
--into #prac50 
from openquery(JGBHR01, ''
declare @data datetime
set @data = ''''' + convert(varchar(10), @do, 20) + '''''

select distinct LpLogo, DataUrodzenia, Wiek, YEAR(@data) - YEAR(DataUrodzenia) as Wiek2 
from sl_hr_jabilgs.dbo.lp_fn_BasePracExLow(@data)
where YEAR(@data) - YEAR(DataUrodzenia) > 50

'')'
insert into #prac50
exec (@stmt)

--select * from #prac50
---------------------------------
-- chorobowe do dodania z poprzedniego zatrudnienia
---------------------------------
IF OBJECT_ID('tempdb..#poprzCH') IS NOT NULL DROP TABLE #poprzCH
create table #poprzCH(
	LpLogo varchar(20) not null,
	DniCH int not null
)

set @stmt = '
select * 
--into #poprzCH 
from openquery(JGBHR01, ''
declare @data datetime
set @data = ''''' + convert(varchar(10), @do, 20) + '''''

select H.LpLogo, sum(S.Dni) as DniCH
from sl_hr_jabilgs.dbo.lp_vv_HistoriaZatrudnieniaExt H
left join sl_hr_jabilgs.dbo.lp_vv_HistoriaZatrudnieniaWykEx S on S.lp_HistoriaZatrudnieniaId = H.lp_HistoriaZatrudnieniaId
where S.NieobecnoscLimitowanaTyp = ''''WynZaChorobe''''
and S.Dni is not null
and YEAR(H.DataZw) = YEAR(@data)
group by H.LpLogo

'')'
insert into #poprzCH
exec (@stmt)

--select * from #poprzCH
---------------------------------
-- Absencja (U+Z), KodZUS = 151 i pozostałe 
---------------------------------
--declare @stmt nvarchar(max)

IF OBJECT_ID('tempdb..#abs151') IS NOT NULL DROP TABLE #abs151
-- na podstawie importu absencji w Asseco.cs
create table #abs151(
	[Typ] [varchar](10) NOT NULL,
	[Id] [int] NOT NULL,
	[LpLogo] [char](10) NOT NULL,
	[DataOd] [datetime] NOT NULL,
	[DataDo] [datetime] NOT NULL,
	[Kod] [int] NOT NULL,
	[IleDni] [int] NULL,
	[Godziny] [float] NULL,
	[Zalegly] [bit] NOT NULL,
	[Planowany] [bit] NOT NULL,
	[NaZadanie] [bit] NOT NULL,
	[Rok] [int] NOT NULL,
	[Miesiac] [int] NOT NULL,
	[Korekta] [bit] NOT NULL,
	[IdKorygowane] [int] NULL,
	[Aktywny] [bit] NOT NULL,
	KodZUS varchar(10) null,
	KodZUSChoroby varchar(10) null,
	Symbol varchar(20) null,
    placiZUS bit not null default 0
)

declare @kodyZUS varchar(500)
set @kodyZUS = ISNULL((select Nazwa from Kody where Typ = 'ABS.ZUS.PLACI' and Kod = 1 and Aktywny = 1), '-1')

declare @kodyABSDL varchar(500)
set @kodyABSDL = ISNULL((select Nazwa from Kody where Typ = 'ABS.ZUS.PLACI' and Kod = 2 and Aktywny = 1), '-1')

set @stmt = '
select * 
from openquery(JGBHR01, ''

select ''''U'''' as Typ,lp_UrlopyId as Id,LpLogo,DataOd,DataDo,T.lp_UrlopyTypId,Dni,Godziny,Zalegly,Planowany,NaZadanie,Rok,Miesiac,Korekta,lp_UrlopyIdK as IdKorygowane,U.Aktywny
,null as KodZUS, null as KodZUSChoroby, null as Symbol, 0 as placiZUS
from sl_hr_jabilgs.dbo.lp_urlopy U
left join sl_hr_jabilgs.dbo.lp_urlopytyp T with (nolock) on T.UrlopTyp = U.UrlopTyp
where DataOd >= ''''20111101''''

union all

select ''''Z'''' as Typ,lp_ZasilkiId as Id,LpLogo,DataOd,DataDo,T.lp_ZasilkiTypId as Kod,LDni as IleDni,LDni*8 as Godziny,0 as Zalegly,0 as Planowany,0 as NaZadanie,Rok,Miesiac,cast(Korekta as bit) as Korekta,lp_ZasilkiIdK as IdKorygowane,Z.Aktywny 
,Z.KodZUS, Z.KodZUSChoroby, null as Symbol, 
case when Z.KodZUS in (' + @kodyZUS + ') then 1 else 0 end as placiZUS
from sl_hr_jabilgs.dbo.lp_Zasilki Z
left join sl_hr_jabilgs.dbo.lp_zasilkityp T with (nolock) on T.ZasilekTyp = Z.ZasilekTyp
where DataOd >= ''''20111101''''
'')'
insert into #abs151
exec (@stmt)

-- aktualizacja:1:pozostawienie ostatniego rekordu z korekty -----
delete from #abs151 where Id in 
(select A.Id from #abs151 A 
left outer join #abs151 K on K.Typ = A.Typ and K.Id= A.IdKorygowane
where (A.Korekta = 1 or A.Id in (select distinct IdKorygowane from #abs151 where Korekta = 1))
and (A.Korekta = 0 or A.Id <> (select MAX(Id) from #abs151 where Typ = A.Typ and IdKorygowane = A.IdKorygowane)))

delete from #abs151 where IleDni <= 0 

update #abs151 set Symbol = AK.Symbol
from #abs151 A
left join AbsencjaKody AK on AK.Kod = A.Kod

update #abs151 set placiZUS = 1
where Typ = 'U' 
and Symbol in (select items from dbo.SplitStr(@kodyABSDL,',')) 


--select * from #abs151 
---------------------------------
-- chorobowe 33 i 14 jak wiek > 50 lat w następnym roku,  -- ostatnia data w kosztach
---------------------------------
IF OBJECT_ID('tempdb..#chor33') IS NOT NULL DROP TABLE #chor33

;WITH cte AS
(
	select ROW_NUMBER() OVER (PARTITION BY P.Id ORDER BY D.Data) AS rownum,
		P.Id as IdPracownika, P.KadryId as Logo, P.Nazwisko + ' ' + P.Imie as Pracownik,
		D.Data, PP.Wiek2, ISNULL(PC.DniCH, 0) as DniCH,
		case when PP.Wiek2 is null then 33 else 14 end as LimitDni	
	from dbo.GetDates2(@boy, @do) D
	cross join Pracownicy P
	--left join Absencja A on A.IdPracownika = P.Id and D.Data between A.DataOd and A.DataDo
	--left join AbsencjaKody AK on AK.Kod = A.Kod
    left join #abs151 A on A.LpLogo = P.KadryId and D.Data between A.DataOd and A.DataDo
	left join #prac50 PP on PP.LpLogo = P.KadryId
	left join #poprzCH PC on PC.LpLogo = P.KadryId
	--where AK.Symbol = 'CH'
    where A.Symbol = 'CH' and A.placiZUS = 0
)
SELECT IdPracownika, Logo, Pracownik, 
case when DniCH >= LimitDni then DATEADD(D, -1, Data) else Data end as Data,  -- jak limit jest wyczerpany to dzień wcześniej !!!
Data as Data1, 
Wiek2, DniCH, LimitDni, rownum
into #chor33
FROM cte
WHERE (DniCH < LimitDni and DniCH + rownum = LimitDni)  -- musimy policzyć
   or (DniCH >= LimitDni and rownum = 1)				-- już jest wyczerpany

--select * from #chor33
---------------------------------
-- FTE
---------------------------------
IF OBJECT_ID('tempdb..#fte') IS NOT NULL DROP TABLE #fte

select @nom = DniPrac from CzasNom where Data = @od
declare @dodzis int
set @dodzis = DATEDIFF(D, @od, @dzis) + 1

select D.* 
,ROUND(cast(DniZatr as float) / DniRob, 2) as FTE,
--,ISNULL(I.FTE, 0) as FTE_PL
--,case when I.FTE is null then 'X' else '' end as BrakPL
--,I.Head as HeadPL,
case 
when DataZwol is not null and DataZwol <= @do then convert(varchar(10), DataZwol, 20) --'Z'
when DniZatr = 0 then (
	select top 1 ISNULL(K.Symbol, 'ERROR') from Absencja A 
	left join AbsencjaKody K on K.Kod = A.Kod
	where A.IdPracownika = D.IdPracownika and @od between A.DataOd and A.DataDo	 -- symbol na datę początku okresu
)
else 'A'
end as DataZwolStatus,

case when 
	(
	select top 1 0 from dbo.GetDates2(@boy, dbo.MinDate2(@dzis,ISNULL(D.DataZwol,'20990909'))) DD   -- jak zatrudniony po boy to w kosztach wiec nie robi boy, jak nic nie znajdzie to ciągłość
	left join #abs151 A on A.LpLogo = D.Logo and DD.Data between (A.DataOd) and (A.DataDo)
	where A.Symbol is null or A.placiZUS = 0	-- nie ma absencji lub jest płatna to w kosztach
	) is null									-- ma ciągłość absencji niepłatnych 
or @dzis > ISNULL(D.[Ostatni dzień w kosztach],'20990909')  -- lub przekroczył 33/14 dni  
then 0 else 1 end as CHwKosztach				-- 0-ch niepłatne, 1-w kosztach

into #fte

from
(
select P.Id as IdPracownika, P.KadryId as Logo, P.Nazwisko + ' ' + P.Imie as Pracownik,
SUM(case when D.Data between P.DataZatr and ISNULL(P.DataZwol, '20990909') 
    and (A.Symbol is null 
     or (A.Symbol = 'CH' and A.placiZUS = 0 and D.Data <= ISNULL(CH.Data, '20990909'))
     or (A.Symbol != 'CH' and A.placiZUS = 0)) 
	and AA.Symbol is null

    --and (ISNULL(A.Symbol,'') not in ('CH') or D.Data <= ISNULL(CH.Data, '20990909') and ISNULL(A.KodZUS,0) != 151)	
	--and  ISNULL(A.Symbol,'') not in ('UB','WY','M','R')   -- bezpłatny, wychowawczy, macierzyński, rehabilitacja

    and K.Data is null then 1 else 0 end) as DniZatr,
SUM(case when R.Id is not null 
    and (A.Symbol is null 
     or (A.Symbol = 'CH' and A.placiZUS = 0 and D.Data <= ISNULL(CH.Data, '20990909'))
     or (A.Symbol != 'CH' and A.placiZUS = 0)) 
	and AA.Symbol is null

    --and (ISNULL(A.Symbol,'') not in ('CH') or D.Data <= ISNULL(CH.Data, '20990909') and ISNULL(A.KodZUS,0) != 151)
	--and  ISNULL(A.Symbol,'') not in ('UB','WY','M','R')
	
    and K.Data is null then 1 else 0 end) as DniZatr1,
@nom as DniRob,
SUM(case when K.Data is null then 1 else 0 end) as DniRob1,
count(*) as DniKal,
MIN(P.DataZatr) as DataZatr,
MIN(P.DataZwol) as DataZwol,

SUM(case when A.Symbol in ('CH') then 1 else 0 end) as DniChor,

MIN(CH.Data) as [Ostatni dzień w kosztach]  -- liczone wg chorobowego 33 - jak nie ma 

from dbo.GetDates2(@od, @do) D
cross join Pracownicy P
left join Przypisania R on R.IdPracownika = P.Id and D.Data between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1
left join Kalendarz K on K.Data = D.Data

--left join Absencja A on A.IdPracownika = P.Id and D.Data between (A.DataOd) and (A.DataDo)
--left join AbsencjaKody AK on AK.Kod = A.Kod
left join #abs151 A on A.LpLogo = P.KadryId and D.Data between (A.DataOd) and (A.DataDo)


outer apply (select top 1 * from #abs151 where LpLogo = P.KadryId and DataOd <= D.Data and @od <= DataDo and KodZUS = 313 and KodZUSChoroby = 'B' order by DataOd desc) AA  --plomba FTE=0 dla ciąż, ostatnia CH, kończące się w okresie, po zawsze jest M dlatego zadziała 

 
left join #chor33 CH on CH.IdPracownika = P.Id
where P.Id in 
(
select IdPracownika from Przypisania where ISNULL(Do, '20990909') >= @od and Od <= @do and Status = 1 
)
and P.KadryId < 80000 and P.Status >= -1
group by P.Id, P.Nazwisko, P.Imie, P.KadryId
) D

--left join PodzialLudziImport I on I.KadryId = D.Logo and @od between I.OkresOd and ISNULL(I.OkresDo, '20990909')	-- do porównania
--where IdPracownika = 58

order by D.Pracownik

--->>>---------------

---------------------
--uzupełnienie danych
---------------------
update PodzialLudziImport set DataZatr = F.DataZatr, DataZwolStatus = F.DataZwolStatus, OstatniDzienWKosztach = F.[Ostatni dzień w kosztach], FTE = F.FTE, CHwKosztach = F.CHwKosztach
--select * 
--select F.*,I.*
from PodzialLudziImport I
left join Pracownicy P on P.KadryId = I.KadryId
left join #fte F on F.IdPracownika = P.Id
where I.OkresOd = @od
--and I.KadryId = '01121'
--order by I.KadryId

--and ([Ostatni dzień w kosztach] is not null or F.FTE != 1)
--and I.FTE != F.FTE
--and A.Id is not null

--select * from #poprzCH
--select * from #prac50
--select * from #chor33
--select * from #abs151 
--select * from #fte

--update OkresyRozl set DataImportu = '{1}' where DataOd = '{0}' 
--update OkresyRozl set DataSplitow = '{1}' where DataOd = '{0}' 
                    ", dOd, dDo));
                */
                if (ok)    
                {
                    int ret = execProc("importPLFTE", null, dt1, dt2);
                    ok = ret == 0;
                }
            }
            if (ok) cntPodzialLudzi.DataBind();
            return ok ? 0 : -3;
        }

        //-----------------------------------------------------------------------------------
        public static int execProc(string proc, string pids, DateTime dOd, DateTime dDo)
        {
            int success = 0;
            try
            {
                SqlCommand cmd = new SqlCommand(proc, db.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300;

                var ppids = cmd.Parameters.Add("@pracIds", SqlDbType.VarChar);
                if (String.IsNullOrEmpty(pids))
                    ppids.Value = DBNull.Value;
                else
                    ppids.Value = pids;
                var pod = cmd.Parameters.Add("@od", SqlDbType.DateTime);
                pod.Value = dOd;
                var pdo = cmd.Parameters.Add("@do", SqlDbType.DateTime);
                pdo.Value = dDo;

                var ret = cmd.Parameters.Add("@ret", SqlDbType.Int);
                ret.Direction = ParameterDirection.ReturnValue;

                success = cmd.ExecuteNonQuery();  // 1 sukces
                int r = (int)ret.Value;
                if (r != 0)
                    Log.Error(Log.SQL, String.Format("exec {0}({1},{2},{3})", proc, pids, dOd, dDo), String.Format("return {0}", r));
                return r;
            }
            catch (Exception ex)
            {
                Log.Error(Log.SQL, String.Format("exec {0}({1},{2},{3}) exception", proc, pids, dOd, dDo), ex.Message);
                return -99;
                //throw;
            }
        }

        /*
         do rozwiązania - w SP nie działa #temptable, trzeba zamienić na zmienne lokalne @table, wywala sie cos z exec insert
         
         */

        private int Import_1(int mode)   // 1 - all, 2 - aktualizacja danych
        {
            string pids = null;  // lista id pracowników oddzielona przecinkami
            string dOd = cntPodzialLudzi.DataOd;
            string dDo = cntPodzialLudzi.DataDo;
            DateTime dt1 = (DateTime)Tools.StrToDateTime(dOd);
            DateTime dt2 = (DateTime)Tools.StrToDateTime(dDo);
            DateTime day;
            object o = deNaDzien.Date;
            if (o == null) return -1;
            else
            {
                day = (DateTime)o;
                if (day < dt1 || day > dt2)
                    return -1;
            }
            dDo = Tools.DateToStrDb(day);

            bool ok = true;
            if (mode == 1)
            {
                //int ret = Tools.StrToInt(db.getScalar(String.Format(@"select dbo.importPLSplity({0},'{1}','{2}')", db.nullStrParam(pids), dOd, dDo)), -1);
                int ret = execProc("importPLSplity", null, dt1, dt2);
                ok = ret == 0;
            }
            if (!ok) return -2;
            //----- II cześć -----
            if (mode == 1 || mode == 2)
            {
                //int ret = Tools.StrToInt(db.getScalar(String.Format(@"select dbo.importPLFTE({0},'{1}','{2}')", db.nullStrParam(pids), dOd, dDo)), -1);
                int ret = execProc("importPLFTE", null, dt1, dt2);
                ok = ret == 0;
            }
            if (ok) cntPodzialLudzi.DataBind();
            return ok ? 0 : -3;
        }






        //-----------------------------------------------------------------------------------
        private int Backup(out string name)
        {
            string dOd = cntPodzialLudzi.DataOd;
            string dDo = cntPodzialLudzi.DataDo;
            return Backup(out name, dOd, dDo);
        }

        private static int Backup(out string name, string dOd, string dDo)
        {
            const string bDB = "HR_TMP";

            name = dDo.Substring(0, 7).Replace("-", "") + "_" + Tools.DateTimeToStr(DateTime.Now).Replace("-", "").Replace(":", "").Replace(" ", "_");

            Log.Info(Log.PL_BACKUP, "Podział Ludzi - Backup danych ", name);
            bool ok = db.execSQL(SQL.Backup(dOd, name, bDB));

            /*
            bool ok = db.execSQL(String.Format(@"
select * into {2}..PodzialLudziImport_{1} from PodzialLudziImport 
--where OkresOd = '{0}'

select * into {2}..Splity_{1} from Splity 
--where DataOd = '{0}'

select * into {2}..SplityWsp_{1} from SplityWsp 
--where IdSplitu in (select Id from Splity where DataOd = '{0}')            
            ", dOd, name, bDB));            
            */

            return ok ? 0 : -1;
        }
    }
}












//-------------------------------------------------------------------------------------
// 20160227
//-------------------------------------------------------------------------------------
/*
I czesc

  
  
            //----- aktualizacja danych -----
            if (mode == 2)
            {
                ok = db.execSQL(String.Format(@"
declare	@od datetime
declare	@do datetime
set @od = '{0}'	-- dla cc: zakres od bo mogą być cc wyłączone 	, jak działa wszystko ok, to taka sam jak @p1
set @do = '{1}'

declare @cnt int
set @cnt = DATEDIFF(DD, @od, @do) + 1 - (select COUNT(*) from Kalendarz where Data between @od and @do) 

declare @ccSurplus int
select @ccSurplus = Id from CC where cc = '499'





----- PodzialLudziImport -----
update PodzialLudziImport set OkresDo = DATEADD(D, -1, @od) where OkresOd = DATEADD(M, -1, @od) and OkresDo is null
delete from PodzialLudziImport where OkresOd = @od and OkresDo is null

--select * from PodzialLudziImport where KadryId = '00001' order by OkresOd desc

insert into PodzialLudziImport
select @od as Miesiac, P.KadryId, P.Nazwisko + ' ' + P.Imie as Pracownik, S.Nazwa as Stanowisko, 
PS.Grupa as TypImport, PS.Klasyfikacja as Class, null as Grade, 1 as FTE, 

case when PO.Id is null then cast(P.EtatL as float) / P.EtatM 
else cast(PO.EtatL as float) / PO.EtatM 
end as Head, 

null as CC, 

@od as OkresOd, null as OkresDo, --case when RR.Id is null then null else dbo.eom(@od) end as OkresDo, 
0 as Status, 
P.DataZatr, convert(varchar(10), P.DataZwol, 20), A.Area, POS.Position, null, ISNULL(PO.Stawka, P.Stawka) as Brutto
from Pracownicy P
left join Przypisania R on R.IdPracownika = P.Id and @do between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1
left join PracownicyStanowiska PS on PS.IdPracownika = P.Id and @do between PS.Od and ISNULL(PS.Do, '20990909')
left join Stanowiska S on S.Id = PS.IdStanowiska
left join Commodity C on C.Id = R.IdCommodity
left join Area A on A.Id = R.IdArea
left join Position POS on POS.Id = R.IdPosition
left join OkresyRozl RR on @od between RR.DataOd and RR.DataDo and RR.Archiwum = 1
left join PracownicyOkresy PO on PO.Id = P.Id and PO.IdOkresu = RR.Id
where P.KadryId < 80000 and P.Status >= -1 and P.Id in (select IdPracownika from Przypisania where Od <= @do and @od <= ISNULL(Do, '20990909') and Status = 1)

--kontrola TypImport !!!
--select * from PodzialLudziImport where TypImport is null and DataZatr <> '20141201'









----- Splity -----
--select * from Splity
--update Splity set DataDo = DATEADD(D, -1, @od) where DataOd = DATEADD(M, -1, @od) and DataDo is null
update Splity set DataDo = DATEADD(D, -1, @od) where DataDo is null 
and Id in  
(
select S.Id 
from PodzialLudziImport I 
outer apply (select top 1 * from Splity where GrSplitu = I.KadryId and DataOd < @od order by DataOd desc) S
where I.OkresOd = @od
) 


delete from SplityWsp where IdSplitu in 
    (select Id from Splity where DataOd = @od and DataDo is null 
        and GrSplitu not in 
            (select GrSplitu from CC where GrSplitu is not null) 
    ) 
delete from Splity where DataOd = @od and DataDo is null
    and GrSplitu not in 
        (select GrSplitu from CC where GrSplitu is not null) 


insert into Splity 
select I.KadryId, 'Split ' + I.KadryId + ' ' + I.Pracownik, @od, null, 0
from PodzialLudziImport I
where OkresOd = @od

----- SplityWsp -----
--drop table #www
select D.IdPracownika, D.IdSplitu, D.IdCC, ROUND(D.SumWsp / D.Dni, 4) as Wsp
into #www
from
(
select 
	R.IdPracownika, SPL.Id as IdSplitu, W.IdCC, SUM(ISNULL(W.Wsp, 0)) as SumWsp, 
	(
	select count(*) from dbo.GetDates2(@od, @do) D2
	inner join Przypisania R2 on D2.Data between R2.Od and ISNULL(R2.Do, '20990909') and R2.Status = 1 and R2.IdPracownika = R.IdPracownika		-- okres zatrudnienia
	left join Kalendarz K2 on K2.Data = D2.Data	
	where K2.Data is null -- dni robocze    
	) as Dni
from dbo.GetDates2(@od, @do) D
left join Kalendarz K on K.Data = D.Data   -- powinno iść po zmianach bo wolne za święto
inner join Przypisania R on D.Data between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1		-- okres zatrudnienia
left join Pracownicy P on P.Id = R.IdPracownika
left join Splity SPL on SPL.GrSplitu = P.KadryId and @do between SPL.DataOd and ISNULL(SPL.DataDo, '20990909')
left join SplityWspP W on W.IdPrzypisania = R.Id
where K.Data is null and P.KadryId < 80000 and P.Status >= -1
group by R.IdPracownika, SPL.Id, W.IdCC
) D
order by IdPracownika, IdSplitu, IdCC

-- brak wsp lub niedomiar do 1
insert into #www
select IdPracownika, IdSplitu, @ccSurplus, ROUND(1 - SUM(Wsp),4)  
from #www group by IdPracownika, IdSplitu having ROUND(SUM(Wsp),4) <> 1

delete from #www where IdCC is null

----- SplityWsp - import -----
--select * from SplityWsp
insert into SplityWsp
select IdSplitu, IdCC, Wsp from #www W

----- DataSplitow -----
--update OkresyRozl set DataImportu = '{1}' where DataOd = '{0}' 
update OkresyRozl set DataSplitow = '{1}' where DataOd = '{0}' 
---------------------------------------
-- KONIEC
---------------------------------------
                    ", dOd, dDo));
            }
            if (!ok) return -2;



*/
//-------------------------------------------------------------------------------------

/* przed liczenie FTE z 20150211 KodZusChoroby B, ale już z KodZusChoroby
declare @od datetime
declare @do datetime
declare @boy datetime
declare @nom int

--declare @boy2 datetime
--set @boy2 = DATEADD(Y, -2, @boy)  -- do liczenia ostatni dzień w kosztach
--set @boy2 = @boy

set @od = '{0}'

set @do = dbo.eom(@od)
set @boy = dbo.boy(@od)

declare @stmt nvarchar(max)


--->>>---------------------------


---------------------------------
-- pracownicy > 50 lat - wg daty do - koniec miesiąca chociaz lp_fn_BasePracExLow rownie dobrze byloby wywolac z GETDATE() bo nie ma warunków na zatrudniony
---------------------------------
IF OBJECT_ID('tempdb..#prac50') IS NOT NULL DROP TABLE #prac50
create table #prac50(
	LpLogo varchar(20) not null,
	DataUrodzenia datetime not null,
	Wiek int not null,
	Wiek2 int not null
)

set @stmt = '
select * 
--into #prac50 
from openquery(JGBHR01, ''
declare @data datetime
set @data = ''''' + convert(varchar(10), @do, 20) + '''''

select distinct LpLogo, DataUrodzenia, Wiek, YEAR(@data) - YEAR(DataUrodzenia) as Wiek2 
from sl_hr_jabilgs.dbo.lp_fn_BasePracExLow(@data)
where YEAR(@data) - YEAR(DataUrodzenia) > 50

'')'
insert into #prac50
exec (@stmt)

--select * from #prac50
---------------------------------
-- chorobowe do dodania z poprzedniego zatrudnienia
---------------------------------
IF OBJECT_ID('tempdb..#poprzCH') IS NOT NULL DROP TABLE #poprzCH
create table #poprzCH(
	LpLogo varchar(20) not null,
	DniCH int not null
)

set @stmt = '
select * 
--into #poprzCH 
from openquery(JGBHR01, ''
declare @data datetime
set @data = ''''' + convert(varchar(10), @do, 20) + '''''

select H.LpLogo, sum(S.Dni) as DniCH
from sl_hr_jabilgs.dbo.lp_vv_HistoriaZatrudnieniaExt H
left join sl_hr_jabilgs.dbo.lp_vv_HistoriaZatrudnieniaWykEx S on S.lp_HistoriaZatrudnieniaId = H.lp_HistoriaZatrudnieniaId
where S.NieobecnoscLimitowanaTyp = ''''WynZaChorobe''''
and S.Dni is not null
and YEAR(H.DataZw) = YEAR(@data)
group by H.LpLogo

'')'
insert into #poprzCH
exec (@stmt)

--select * from #poprzCH
---------------------------------
-- Absencja (U+Z), KodZUS = 151 i pozostałe 
---------------------------------
--declare @stmt nvarchar(max)

IF OBJECT_ID('tempdb..#abs151') IS NOT NULL DROP TABLE #abs151
-- na podstawie importu absencji w Asseco.cs
create table #abs151(
	[Typ] [varchar](10) NOT NULL,
	[Id] [int] NOT NULL,
	[LpLogo] [char](10) NOT NULL,
	[DataOd] [datetime] NOT NULL,
	[DataDo] [datetime] NOT NULL,
	[Kod] [int] NOT NULL,
	[IleDni] [int] NULL,
	[Godziny] [float] NULL,
	[Zalegly] [bit] NOT NULL,
	[Planowany] [bit] NOT NULL,
	[NaZadanie] [bit] NOT NULL,
	[Rok] [int] NOT NULL,
	[Miesiac] [int] NOT NULL,
	[Korekta] [bit] NOT NULL,
	[IdKorygowane] [int] NULL,
	[Aktywny] [bit] NOT NULL,
	KodZUS varchar(10) null,
	KodZUSChoroby varchar(10) null,
	Symbol varchar(20) null,
    placiZUS bit not null default 0
)

declare @kodyZUS varchar(500)
set @kodyZUS = ISNULL((select Nazwa from Kody where Typ = 'ABS.ZUS.PLACI' and Kod = 1 and Aktywny = 1), '-1')

declare @kodyABSDL varchar(500)
set @kodyABSDL = ISNULL((select Nazwa from Kody where Typ = 'ABS.ZUS.PLACI' and Kod = 2 and Aktywny = 1), '-1')

set @stmt = '
select * 
from openquery(JGBHR01, ''

select ''''U'''' as Typ,lp_UrlopyId as Id,LpLogo,DataOd,DataDo,T.lp_UrlopyTypId,Dni,Godziny,Zalegly,Planowany,NaZadanie,Rok,Miesiac,Korekta,lp_UrlopyIdK as IdKorygowane,U.Aktywny
,null as KodZUS, null as KodZUSChoroby, null as Symbol, 0 as placiZUS
from sl_hr_jabilgs.dbo.lp_urlopy U
left join sl_hr_jabilgs.dbo.lp_urlopytyp T with (nolock) on T.UrlopTyp = U.UrlopTyp
where DataOd >= ''''20111101''''

union all

select ''''Z'''' as Typ,lp_ZasilkiId as Id,LpLogo,DataOd,DataDo,T.lp_ZasilkiTypId as Kod,LDni as IleDni,LDni*8 as Godziny,0 as Zalegly,0 as Planowany,0 as NaZadanie,Rok,Miesiac,cast(Korekta as bit) as Korekta,lp_ZasilkiIdK as IdKorygowane,Z.Aktywny 
,Z.KodZUS, Z.KodZUSChoroby, null as Symbol, 
case when Z.KodZUS in (' + @kodyZUS + ') then 1 else 0 end as placiZUS
from sl_hr_jabilgs.dbo.lp_Zasilki Z
left join sl_hr_jabilgs.dbo.lp_zasilkityp T with (nolock) on T.ZasilekTyp = Z.ZasilekTyp
where DataOd >= ''''20111101''''
'')'
insert into #abs151
exec (@stmt)

-- aktualizacja:1:pozostawienie ostatniego rekordu z korekty -----
delete from #abs151 where Id in 
(select A.Id from #abs151 A 
left outer join #abs151 K on K.Typ = A.Typ and K.Id= A.IdKorygowane
where (A.Korekta = 1 or A.Id in (select distinct IdKorygowane from #abs151 where Korekta = 1))
and (A.Korekta = 0 or A.Id <> (select MAX(Id) from #abs151 where Typ = A.Typ and IdKorygowane = A.IdKorygowane)))

delete from #abs151 where IleDni <= 0 

update #abs151 set Symbol = AK.Symbol
from #abs151 A
left join AbsencjaKody AK on AK.Kod = A.Kod

update #abs151 set placiZUS = 1
where Typ = 'U' 
and Symbol in (select items from dbo.SplitStr(@kodyABSDL,',')) 


--select * from #abs151 
---------------------------------
-- chorobowe 33 i 14 jak wiek > 50 lat w następnym roku,  -- ostatnia data w kosztach
---------------------------------
IF OBJECT_ID('tempdb..#chor33') IS NOT NULL DROP TABLE #chor33

;WITH cte AS
(
	select ROW_NUMBER() OVER (PARTITION BY P.Id ORDER BY D.Data) AS rownum,
		P.Id as IdPracownika, P.KadryId as Logo, P.Nazwisko + ' ' + P.Imie as Pracownik,
		D.Data, PP.Wiek2, ISNULL(PC.DniCH, 0) as DniCH,
		case when PP.Wiek2 is null then 33 else 14 end as LimitDni	
	from dbo.GetDates2(@boy, @do) D
	cross join Pracownicy P
	--left join Absencja A on A.IdPracownika = P.Id and D.Data between A.DataOd and A.DataDo
	--left join AbsencjaKody AK on AK.Kod = A.Kod
    left join #abs151 A on A.LpLogo = P.KadryId and D.Data between A.DataOd and A.DataDo
	left join #prac50 PP on PP.LpLogo = P.KadryId
	left join #poprzCH PC on PC.LpLogo = P.KadryId
	--where AK.Symbol = 'CH'
    where A.Symbol = 'CH' and A.placiZUS = 0
)
SELECT IdPracownika, Logo, Pracownik, 
case when DniCH >= LimitDni then DATEADD(D, -1, Data) else Data end as Data,  -- jak limit jest wyczerpany to dzień wcześniej !!!
Data as Data1, 
Wiek2, DniCH, LimitDni, rownum
into #chor33
FROM cte
WHERE (DniCH < LimitDni and DniCH + rownum = LimitDni)  -- musimy policzyć
   or (DniCH >= LimitDni and rownum = 1)				-- już jest wyczerpany

--select * from #chor33
---------------------------------
-- FTE
---------------------------------
IF OBJECT_ID('tempdb..#fte') IS NOT NULL DROP TABLE #fte

select @nom = DniPrac from CzasNom where Data = @od

select D.* 
,ROUND(cast(DniZatr as float) / DniRob, 2) as FTE,
--,ISNULL(I.FTE, 0) as FTE_PL
--,case when I.FTE is null then 'X' else '' end as BrakPL
--,I.Head as HeadPL,
case 
when DataZwol is not null and DataZwol <= @do then convert(varchar(10), DataZwol, 20) --'Z'
when DniZatr = 0 then (
	select top 1 ISNULL(K.Symbol, 'ERROR') from Absencja A 
	left join AbsencjaKody K on K.Kod = A.Kod
	where A.IdPracownika = D.IdPracownika and @od between A.DataOd and A.DataDo	 -- symbol na datę początku okresu
)
else 'A'
end as DataZwolStatus

into #fte

from
(
select P.Id as IdPracownika, P.KadryId as Logo, P.Nazwisko + ' ' + P.Imie as Pracownik,
SUM(case when D.Data between P.DataZatr and ISNULL(P.DataZwol, '20990909') 
    and (A.Symbol is null 
     or (A.Symbol = 'CH' and A.placiZUS = 0 and D.Data <= ISNULL(CH.Data, '20990909'))
     or (A.Symbol != 'CH' and A.placiZUS = 0)) 

    --and (ISNULL(A.Symbol,'') not in ('CH') or D.Data <= ISNULL(CH.Data, '20990909') and ISNULL(A.KodZUS,0) != 151)	
	--and  ISNULL(A.Symbol,'') not in ('UB','WY','M','R')   -- bezpłatny, wychowawczy, macierzyński, rehabilitacja

    and K.Data is null then 1 else 0 end) as DniZatr,
SUM(case when R.Id is not null 
    and (A.Symbol is null 
     or (A.Symbol = 'CH' and A.placiZUS = 0 and D.Data <= ISNULL(CH.Data, '20990909'))
     or (A.Symbol != 'CH' and A.placiZUS = 0)) 

    --and (ISNULL(A.Symbol,'') not in ('CH') or D.Data <= ISNULL(CH.Data, '20990909') and ISNULL(A.KodZUS,0) != 151)
	--and  ISNULL(A.Symbol,'') not in ('UB','WY','M','R')
	
    and K.Data is null then 1 else 0 end) as DniZatr1,
@nom as DniRob,
SUM(case when K.Data is null then 1 else 0 end) as DniRob1,
count(*) as DniKal,
min(P.DataZatr) as DataZatr,
MIN(P.DataZwol) as DataZwol,

SUM(case when A.Symbol in ('CH') then 1 else 0 end) as DniChor,

MIN(CH.Data) as [Ostatni dzień w kosztach]
from dbo.GetDates2(@od, @do) D
cross join Pracownicy P
left join Przypisania R on R.IdPracownika = P.Id and D.Data between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1
left join Kalendarz K on K.Data = D.Data

--left join Absencja A on A.IdPracownika = P.Id and D.Data between (A.DataOd) and (A.DataDo)
--left join AbsencjaKody AK on AK.Kod = A.Kod
left join #abs151 A on A.LpLogo = P.KadryId and D.Data between (A.DataOd) and (A.DataDo)

left join #chor33 CH on CH.IdPracownika = P.Id
where P.Id in 
(
select IdPracownika from Przypisania where ISNULL(Do, '20990909') >= @od and Od <= @do and Status = 1 
)
and P.KadryId < 80000 and P.Status >= -1
group by P.Id, P.Nazwisko, P.Imie, P.KadryId
) D

--left join PodzialLudziImport I on I.KadryId = D.Logo and @od between I.OkresOd and ISNULL(I.OkresDo, '20990909')	-- do porównania
--where IdPracownika = 58

order by D.Pracownik

--->>>---------------

---------------------
--uzupełnienie danych
---------------------
update PodzialLudziImport set DataZatr = F.DataZatr, DataZwolStatus = F.DataZwolStatus, OstatniDzienWKosztach = F.[Ostatni dzień w kosztach], FTE = F.FTE
--select * 
--select F.*,I.*
from PodzialLudziImport I
left join Pracownicy P on P.KadryId = I.KadryId
left join #fte F on F.IdPracownika = P.Id
where I.OkresOd = @od

--and I.KadryId = '01738'
--and ([Ostatni dzień w kosztach] is not null or F.FTE != 1)
--and I.FTE != F.FTE
--and A.Id is not null

/*
select * from #poprzCH
select * from #prac50
select * from #chor33
select * from #abs151 
select * from #fte
* /
--update OkresyRozl set DataImportu = '{1}' where DataOd = '{0}' 
--update OkresyRozl set DataSplitow = '{1}' where DataOd = '{0}' 
 */

/* 20150210 kodchoroby B
--declare @stmt nvarchar(max)

IF OBJECT_ID('tempdb..#abs151') IS NOT NULL DROP TABLE #abs151
-- na podstawie importu absencji w Asseco.cs
create table #abs151(
	[Typ] [varchar](10) NOT NULL,
	[Id] [int] NOT NULL,
	[LpLogo] [char](10) NOT NULL,
	[DataOd] [datetime] NOT NULL,
	[DataDo] [datetime] NOT NULL,
	[Kod] [int] NOT NULL,
	[IleDni] [int] NULL,
	[Godziny] [float] NULL,
	[Zalegly] [bit] NOT NULL,
	[Planowany] [bit] NOT NULL,
	[NaZadanie] [bit] NOT NULL,
	[Rok] [int] NOT NULL,
	[Miesiac] [int] NOT NULL,
	[Korekta] [bit] NOT NULL,
	[IdKorygowane] [int] NULL,
	[Aktywny] [bit] NOT NULL,
	KodZUS varchar(10) null,
	Symbol varchar(20) null,
    placiZUS bit not null default 0
)

declare @kodyZUS varchar(500)
set @kodyZUS = ISNULL((select Nazwa from Kody where Typ = 'ABS.ZUS.PLACI' and Kod = 1 and Aktywny = 1), '-1')

declare @kodyABSDL varchar(500)
set @kodyABSDL = ISNULL((select Nazwa from Kody where Typ = 'ABS.ZUS.PLACI' and Kod = 2 and Aktywny = 1), '-1')

set @stmt = '
select * 
from openquery(JGBHR01, ''

select ''''U'''' as Typ,lp_UrlopyId as Id,LpLogo,DataOd,DataDo,T.lp_UrlopyTypId,Dni,Godziny,Zalegly,Planowany,NaZadanie,Rok,Miesiac,Korekta,lp_UrlopyIdK as IdKorygowane,U.Aktywny
,null as KodZUS, null as Symbol, 0 as placiZUS
from sl_hr_jabilgs.dbo.lp_urlopy U
left join sl_hr_jabilgs.dbo.lp_urlopytyp T with (nolock) on T.UrlopTyp = U.UrlopTyp
where DataOd >= ''''20111101''''

union all

select ''''Z'''' as Typ,lp_ZasilkiId as Id,LpLogo,DataOd,DataDo,T.lp_ZasilkiTypId as Kod,LDni as IleDni,LDni*8 as Godziny,0 as Zalegly,0 as Planowany,0 as NaZadanie,Rok,Miesiac,cast(Korekta as bit) as Korekta,lp_ZasilkiIdK as IdKorygowane,Z.Aktywny 
,Z.KodZUS, null as Symbol, 
case when Z.KodZUS in (' + @kodyZUS + ') then 1 else 0 end as placiZUS
from sl_hr_jabilgs.dbo.lp_Zasilki Z
left join sl_hr_jabilgs.dbo.lp_zasilkityp T with (nolock) on T.ZasilekTyp = Z.ZasilekTyp
where DataOd >= ''''20111101''''
'')'
insert into #abs151
exec (@stmt)

 
 */




/* 20150122 - kodZUS tylko 151
declare @od datetime
declare @do datetime
declare @boy datetime
declare @nom int

set @od = '{0}'

set @do = dbo.eom(@od)
set @boy = dbo.boy(@od)

declare @stmt nvarchar(max)

---------------------------------
-- pracownicy > 50 lat - wg daty do - koniec miesiąca chociaz lp_fn_BasePracExLow rownie dobrze byloby wywolac z GETDATE() bo nie ma warunków na zatrudniony
---------------------------------
IF OBJECT_ID('tempdb..#prac50') IS NOT NULL DROP TABLE #prac50
create table #prac50(
	LpLogo varchar(20) not null,
	DataUrodzenia datetime not null,
	Wiek int not null,
	Wiek2 int not null
)

set @stmt = '
select * 
--into #prac50 
from openquery(JGBHR01, ''
declare @data datetime
set @data = ''''' + convert(varchar(10), @do, 20) + '''''

select distinct LpLogo, DataUrodzenia, Wiek, YEAR(@data) - YEAR(DataUrodzenia) as Wiek2 
from sl_hr_jabilgs.dbo.lp_fn_BasePracExLow(@data)
where YEAR(@data) - YEAR(DataUrodzenia) > 50

'')'
insert into #prac50
exec (@stmt)

--select * from #prac50
---------------------------------
-- chorobowe do dodania z poprzedniego zatrudnienia
---------------------------------
IF OBJECT_ID('tempdb..#poprzCH') IS NOT NULL DROP TABLE #poprzCH
create table #poprzCH(
	LpLogo varchar(20) not null,
	DniCH int not null
)

set @stmt = '
select * 
--into #poprzCH 
from openquery(JGBHR01, ''
declare @data datetime
set @data = ''''' + convert(varchar(10), @do, 20) + '''''

select H.LpLogo, sum(S.Dni) as DniCH
from sl_hr_jabilgs.dbo.lp_vv_HistoriaZatrudnieniaExt H
left join sl_hr_jabilgs.dbo.lp_vv_HistoriaZatrudnieniaWykEx S on S.lp_HistoriaZatrudnieniaId = H.lp_HistoriaZatrudnieniaId
where S.NieobecnoscLimitowanaTyp = ''''WynZaChorobe''''
and S.Dni is not null
and YEAR(H.DataZw) = YEAR(@data)
group by H.LpLogo

'')'
insert into #poprzCH
exec (@stmt)

--select * from #poprzCH
---------------------------------
-- Absencja, KodZUS = 151
---------------------------------
--declare @stmt nvarchar(max)

IF OBJECT_ID('tempdb..#abs151') IS NOT NULL DROP TABLE #abs151
-- na podstawie importu absencji w Asseco.cs
create table #abs151(
	[Typ] [varchar](10) NOT NULL,
	[Id] [int] NOT NULL,
	[LpLogo] [char](10) NOT NULL,
	[DataOd] [datetime] NOT NULL,
	[DataDo] [datetime] NOT NULL,
	[Kod] [int] NOT NULL,
	[IleDni] [int] NULL,
	[Godziny] [float] NULL,
	[Zalegly] [bit] NOT NULL,
	[Planowany] [bit] NOT NULL,
	[NaZadanie] [bit] NOT NULL,
	[Rok] [int] NOT NULL,
	[Miesiac] [int] NOT NULL,
	[Korekta] [bit] NOT NULL,
	[IdKorygowane] [int] NULL,
	[Aktywny] [bit] NOT NULL,
	KodZUS varchar(10) null,
	Symbol varchar(20) null,
    placiZUS bit not null default 0
)

declare @kodyZUS varchar(500)
select @kodyZUS = Nazwa from Kody where Typ = 'ABS.ZUS.PLACI' and Aktywny = 1
if @kodyZUS is null 
	set @kodyZUS = '-1'

set @stmt = '
select * 
from openquery(JGBHR01, ''

select ''''U'''' as Typ,lp_UrlopyId as Id,LpLogo,DataOd,DataDo,T.lp_UrlopyTypId,Dni,Godziny,Zalegly,Planowany,NaZadanie,Rok,Miesiac,Korekta,lp_UrlopyIdK as IdKorygowane,U.Aktywny
,null as KodZUS, null as Symbol, 0 as placiZUS
from sl_hr_jabilgs.dbo.lp_urlopy U
left join sl_hr_jabilgs.dbo.lp_urlopytyp T with (nolock) on T.UrlopTyp = U.UrlopTyp
where DataOd >= ''''20111101''''

union all

select ''''Z'''' as Typ,lp_ZasilkiId as Id,LpLogo,DataOd,DataDo,T.lp_ZasilkiTypId as Kod,LDni as IleDni,LDni*8 as Godziny,0 as Zalegly,0 as Planowany,0 as NaZadanie,Rok,Miesiac,cast(Korekta as bit) as Korekta,lp_ZasilkiIdK as IdKorygowane,Z.Aktywny 
,Z.KodZUS, null as Symbol, 
case when Z.KodZUS in (' + @kodyZUS + ') then 1 else 0 end as placiZUS
from sl_hr_jabilgs.dbo.lp_Zasilki Z
left join sl_hr_jabilgs.dbo.lp_zasilkityp T with (nolock) on T.ZasilekTyp = Z.ZasilekTyp
where DataOd >= ''''20111101''''
'')'
insert into #abs151
exec (@stmt)

-- aktualizacja:1:pozostawienie ostatniego rekordu z korekty -----
delete from #abs151 where Id in 
(select A.Id from #abs151 A 
left outer join #abs151 K on K.Typ = A.Typ and K.Id= A.IdKorygowane
where (A.Korekta = 1 or A.Id in (select distinct IdKorygowane from #abs151 where Korekta = 1))
and (A.Korekta = 0 or A.Id <> (select MAX(Id) from #abs151 where Typ = A.Typ and IdKorygowane = A.IdKorygowane)))

delete from #abs151 where IleDni <= 0 

update #abs151 set Symbol = AK.Symbol
from #abs151 A
left join AbsencjaKody AK on AK.Kod = A.Kod

--select * from #abs151 
---------------------------------
-- chorobowe 33 i 14 jak wiek > 50 lat w następnym roku,  -- ostatnia data w kosztach
---------------------------------
IF OBJECT_ID('tempdb..#chor33') IS NOT NULL DROP TABLE #chor33

;WITH cte AS
(
	select ROW_NUMBER() OVER (PARTITION BY P.Id ORDER BY D.Data) AS rownum,
		P.Id as IdPracownika, P.KadryId as Logo, P.Nazwisko + ' ' + P.Imie as Pracownik,
		D.Data, PP.Wiek2, ISNULL(PC.DniCH, 0) as DniCH,
		case when PP.Wiek2 is null then 33 else 14 end as LimitDni	
	from dbo.GetDates2(@boy, @do) D
	cross join Pracownicy P
	--left join Absencja A on A.IdPracownika = P.Id and D.Data between A.DataOd and A.DataDo
	--left join AbsencjaKody AK on AK.Kod = A.Kod
    left join #abs151 A on A.IdPracownika = P.Id and D.Data between A.DataOd and A.DataDo
	left join #prac50 PP on PP.LpLogo = P.KadryId
	left join #poprzCH PC on PC.LpLogo = P.KadryId
	--where AK.Symbol = 'CH'
    where A.Symbol = 'CH' and A.placiZUS = 0
)
SELECT IdPracownika, Logo, Pracownik, 
case when DniCH >= LimitDni then DATEADD(D, -1, Data) else Data end as Data,  -- jak limit jest wyczerpany to dzień wcześniej !!!
Data as Data1, 
Wiek2, DniCH, LimitDni, rownum
into #chor33
FROM cte
WHERE (DniCH < LimitDni and DniCH + rownum = LimitDni)  -- musimy policzyć
   or (DniCH >= LimitDni and rownum = 1)				-- już jest wyczerpany

--select * from #chor33
---------------------------------
-- FTE
---------------------------------
IF OBJECT_ID('tempdb..#fte') IS NOT NULL DROP TABLE #fte

select @nom = DniPrac from CzasNom where Data = @od

select D.* 
,ROUND(cast(DniZatr as float) / DniRob, 2) as FTE,
--,ISNULL(I.FTE, 0) as FTE_PL
--,case when I.FTE is null then 'X' else '' end as BrakPL
--,I.Head as HeadPL,
case 
when DataZwol is not null and DataZwol <= @do then convert(varchar(10), DataZwol, 20) --'Z'
when DniZatr = 0 then (
	select ISNULL(K.Symbol, 'ERROR') from Absencja A 
	left join AbsencjaKody K on K.Kod = A.Kod
	where A.IdPracownika = D.IdPracownika and @od between A.DataOd and A.DataDo	 -- symbol na datę początku okresu
)
else 'A'
end as DataZwolStatus

into #fte

from
(
select P.Id as IdPracownika, P.KadryId as Logo, P.Nazwisko + ' ' + P.Imie as Pracownik,
SUM(case when D.Data between P.DataZatr and ISNULL(P.DataZwol, '20990909') 
    and (A.Symbol is null 
     or (A.Symbol = 'CH' and A.placiZUS = 0 and D.Data <= ISNULL(CH.Data, '20990909'))
     or (A.Symbol != 'CH' and A.placiZUS = 0)) 

    --and (ISNULL(A.Symbol,'') not in ('CH') or D.Data <= ISNULL(CH.Data, '20990909') and ISNULL(A.KodZUS,0) != 151)	
	--and  ISNULL(A.Symbol,'') not in ('UB','WY','M','R')   -- bezpłatny, wychowawczy, macierzyński, rehabilitacja

    and K.Data is null then 1 else 0 end) as DniZatr,
SUM(case when R.Id is not null 
    and (A.Symbol is null 
     or (A.Symbol = 'CH' and A.placiZUS = 0 and D.Data <= ISNULL(CH.Data, '20990909'))
     or (A.Symbol != 'CH' and A.placiZUS = 0)) 

    --and (ISNULL(A.Symbol,'') not in ('CH') or D.Data <= ISNULL(CH.Data, '20990909') and ISNULL(A.KodZUS,0) != 151)
	--and  ISNULL(A.Symbol,'') not in ('UB','WY','M','R')
	
    and K.Data is null then 1 else 0 end) as DniZatr1,
@nom as DniRob,
SUM(case when K.Data is null then 1 else 0 end) as DniRob1,
count(*) as DniKal,
min(P.DataZatr) as DataZatr,
MIN(P.DataZwol) as DataZwol,

SUM(case when A.Symbol in ('CH') then 1 else 0 end) as DniChor,

MIN(CH.Data) as [Ostatni dzień w kosztach]
from dbo.GetDates2(@od, @do) D
cross join Pracownicy P
left join Przypisania R on R.IdPracownika = P.Id and D.Data between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1
left join Kalendarz K on K.Data = D.Data

--left join Absencja A on A.IdPracownika = P.Id and D.Data between (A.DataOd) and (A.DataDo)
--left join AbsencjaKody AK on AK.Kod = A.Kod
left join #abs151 A on A.LpLogo = P.KadryId and D.Data between (A.DataOd) and (A.DataDo)

left join #chor33 CH on CH.IdPracownika = P.Id
where P.Id in 
(
select IdPracownika from Przypisania where ISNULL(Do, '20990909') >= @od and Od <= @do and Status = 1 
)
and P.KadryId < 80000 and P.Status >= -1
group by P.Id, P.Nazwisko, P.Imie, P.KadryId
) D

--left join PodzialLudziImport I on I.KadryId = D.Logo and @od between I.OkresOd and ISNULL(I.OkresDo, '20990909')	-- do porównania
--where IdPracownika = 58

order by D.Pracownik

---------------------
--uzupełnienie danych
---------------------
update PodzialLudziImport set DataZatr = F.DataZatr, DataZwolStatus = F.DataZwolStatus, OstatniDzienWKosztach = F.[Ostatni dzień w kosztach], FTE = F.FTE
--select * 
--select F.*,I.*
from PodzialLudziImport I
left join Pracownicy P on P.KadryId = I.KadryId
left join #fte F on F.IdPracownika = P.Id
where I.OkresOd = @od

--and I.KadryId = '01738'
--and ([Ostatni dzień w kosztach] is not null or F.FTE != 1)
--and I.FTE != F.FTE
--and A.Id is not null

/*
select * from #poprzCH
select * from #prac50
select * from #chor33
select * from #abs151 
select * from #fte
* /

update OkresyRozl set DataImportu = '{1}' where DataOd = '{0}' 

 */






/* 20150118 bez liczenia kodZUS 151
 
            //----- II cześć -----
            if (!ok) return -2;
            else
                ok = db.execSQL(String.Format(@"
declare @od datetime
declare @do datetime
declare @boy datetime
declare @nom int

set @od = '{0}'

set @do = dbo.eom(@od)
set @boy = dbo.boy(@od)

declare @stmt nvarchar(max)


---------------------------------
-- pracownicy > 50 lat - wg daty do - koniec miesiąca chociaz lp_fn_BasePracExLow rownie dobrze byloby wywolac z GETDATE() bo nie ma warunków na zatrudniony
---------------------------------
IF OBJECT_ID('tempdb..#prac50') IS NOT NULL DROP TABLE #prac50
create table #prac50(
	LpLogo varchar(20) not null,
	DataUrodzenia datetime not null,
	Wiek int not null,
	Wiek2 int not null
)

set @stmt = '
select * 
--into #prac50 
from openquery(JGBHR01, ''
declare @data datetime
set @data = ''''' + convert(varchar(10), @do, 20) + '''''

select distinct LpLogo, DataUrodzenia, Wiek, YEAR(@data) - YEAR(DataUrodzenia) as Wiek2 
from sl_hr_jabilgs.dbo.lp_fn_BasePracExLow(@data)
where YEAR(@data) - YEAR(DataUrodzenia) > 50

'')'
insert into #prac50
exec (@stmt)

--select * from #prac50


---------------------------------
-- chorobowe do dodania z poprzedniego zatrudnienia
---------------------------------
IF OBJECT_ID('tempdb..#poprzCH') IS NOT NULL DROP TABLE #poprzCH
create table #poprzCH(
	LpLogo varchar(20) not null,
	DniCH int not null
)

set @stmt = '
select * 
--into #poprzCH 
from openquery(JGBHR01, ''
declare @data datetime
set @data = ''''' + convert(varchar(10), @do, 20) + '''''

select H.LpLogo, sum(S.Dni) as DniCH
from sl_hr_jabilgs.dbo.lp_vv_HistoriaZatrudnieniaExt H
left join sl_hr_jabilgs.dbo.lp_vv_HistoriaZatrudnieniaWykEx S on S.lp_HistoriaZatrudnieniaId = H.lp_HistoriaZatrudnieniaId
where S.NieobecnoscLimitowanaTyp = ''''WynZaChorobe''''
and S.Dni is not null
and YEAR(H.DataZw) = YEAR(@data)
group by H.LpLogo

'')'
insert into #poprzCH
exec (@stmt)

--select * from #poprzCH

---------------------------------
-- chorobowe 33 i 14 jak wiek > 50 lat w następnym roku,  -- ostatnia data w kosztach
---------------------------------
IF OBJECT_ID('tempdb..#chor33') IS NOT NULL DROP TABLE #chor33

;WITH cte AS
(
	select ROW_NUMBER() OVER (PARTITION BY P.Id ORDER BY D.Data) AS rownum,
		P.Id as IdPracownika, P.KadryId as Logo, P.Nazwisko + ' ' + P.Imie as Pracownik,
		D.Data, PP.Wiek2, ISNULL(PC.DniCH, 0) as DniCH,
		case when PP.Wiek2 is null then 33 else 14 end as LimitDni	
	from dbo.GetDates2(@boy, @do) D
	cross join Pracownicy P
	left join Absencja A on A.IdPracownika = P.Id and D.Data between A.DataOd and A.DataDo
	left join AbsencjaKody AK on AK.Kod = A.Kod
	left join #prac50 PP on PP.LpLogo = P.KadryId
	left join #poprzCH PC on PC.LpLogo = P.KadryId
	where AK.Symbol = 'CH' 
)
SELECT IdPracownika, Logo, Pracownik, 
case when DniCH >= LimitDni then DATEADD(D, -1, Data) else Data end as Data,  -- jak limit jest wyczerpany to dzień wcześniej !!!
Data as Data1, 
Wiek2, DniCH, LimitDni, rownum
into #chor33
FROM cte
WHERE (DniCH < LimitDni and DniCH + rownum = LimitDni)  -- musimy policzyć
   or (DniCH >= LimitDni and rownum = 1)				-- już jest wyczerpany

--select * from #chor33



---------------------------------
-- FTE
---------------------------------
IF OBJECT_ID('tempdb..#fte') IS NOT NULL DROP TABLE #fte

select @nom = DniPrac from CzasNom where Data = @od

select D.* 
,ROUND(cast(DniZatr as float) / DniRob, 2) as FTE,
--,ISNULL(I.FTE, 0) as FTE_PL
--,case when I.FTE is null then 'X' else '' end as BrakPL
--,I.Head as HeadPL,
case 
when DataZwol is not null and DataZwol <= @do then convert(varchar(10), DataZwol, 20) --'Z'
when DniZatr = 0 then (
	select ISNULL(K.Symbol, 'ERROR') from Absencja A 
	left join AbsencjaKody K on K.Kod = A.Kod
	where A.IdPracownika = D.IdPracownika and @od between A.DataOd and A.DataDo
)
else 'A'
end as DataZwolStatus

into #fte

from
(
select P.Id as IdPracownika, P.KadryId as Logo, P.Nazwisko + ' ' + P.Imie as Pracownik,

SUM(case when D.Data between P.DataZatr and ISNULL(P.DataZwol, '20990909') 
	and (ISNULL(AK.Symbol,'') not in ('CH') or D.Data <= ISNULL(CH.Data, '20990909'))
	and ISNULL(AK.Symbol,'') not in ('UB','WY','M','R')   -- bezpłatny, wychowawczy, macierzyński, rehabilitacja
	and K.Data is null then 1 else 0 end) as DniZatr,
SUM(case when R.Id is not null 
	and (ISNULL(AK.Symbol,'') not in ('CH') or D.Data <= ISNULL(CH.Data, '20990909'))
	and ISNULL(AK.Symbol,'') not in ('UB','WY','M','R')
	and K.Data is null then 1 else 0 end) as DniZatr1,
@nom as DniRob,
SUM(case when K.Data is null then 1 else 0 end) as DniRob1,
count(*) as DniKal,
min(P.DataZatr) as DataZatr,
MIN(P.DataZwol) as DataZwol,

SUM(case when AK.Symbol in ('CH') then 1 else 0 end) as DniChor,
MIN(CH.Data) as [Ostatni dzień w kosztach]
from dbo.GetDates2(@od, @do) D
cross join Pracownicy P
left join Przypisania R on R.IdPracownika = P.Id and D.Data between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1
left join Kalendarz K on K.Data = D.Data
left join Absencja A on A.IdPracownika = P.Id and D.Data between (A.DataOd) and (A.DataDo)
left join AbsencjaKody AK on AK.Kod = A.Kod
left join #chor33 CH on CH.IdPracownika = P.Id
where P.Id in 
(
select IdPracownika from Przypisania where ISNULL(Do, '20990909') >= @od and Od <= @do and Status = 1 
)
and P.KadryId < 80000 and P.Status >= -1
group by P.Id, P.Nazwisko, P.Imie, P.KadryId
) D

--left join PodzialLudziImport I on I.KadryId = D.Logo and @od between I.OkresOd and ISNULL(I.OkresDo, '20990909')	-- do porównania
--where IdPracownika = 58

order by D.Pracownik

---------------------
--uzupełnienie danych
---------------------
update PodzialLudziImport set DataZatr = F.DataZatr, DataZwolStatus = F.DataZwolStatus, OstatniDzienWKosztach = F.[Ostatni dzień w kosztach], FTE = F.FTE
--select * 
from PodzialLudziImport I
left join Pracownicy P on P.KadryId = I.KadryId
left join #fte F on F.IdPracownika = P.Id
where I.OkresOd = @od

update OkresyRozl set DataImportu = '{1}' where DataOd = '{0}' 
                    ", dOd, dDo));

            if (ok)
                cntPodzialLudzi.DataBind();

            return ok ? 0 : -3;
        }

        private int Backup(out string name)
        {
            name = Tools.DateTimeToStr(DateTime.Now).Replace("-", "").Replace(":", "").Replace(" ", "_");
            Log.Info(Log.PL_IMPORT, "Backup danych", name);
            string dOd = cntPodzialLudzi.DataOd;
            bool ok = db.execSQL(String.Format(@"
select * into PodzialLudziImport_{1} from PodzialLudziImport 
--where OkresOd = '{0}'

select * into Splity_{1} from Splity 
--where DataOd = '{0}'

select * into SplityWsp_{1} from SplityWsp 
--where IdSplitu in (select Id from Splity where DataOd = '{0}')            
            ", dOd, name));
 */