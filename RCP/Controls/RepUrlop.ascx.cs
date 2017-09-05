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
    public partial class RepUrlop : System.Web.UI.UserControl
    {
        SqlConnection con = null;
        Ustawienia settings;

        bool withMe = true;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
        }

        public string GetLineClass(object ja)
        {
            if (withMe)
                return "it" + (ja.ToString() == "1" ? " ja" : null);
            else
                return "it";
        }
        //-----------------------------------------------------------
        private void InitPath(string kierId)
        {
            string kn;
            //if (String.IsNullOrEmpty(kierId)) kn = "Wszyscy pracownicy";
            if (kierId == "-100") kn = "Wszyscy pracownicy";
            else if (kierId == "0") kn = "Poziom główny";
            else kn = Worker.GetNazwiskoImie(kierId);
            cntPath.Prepare(kn, kierId, "0");
        }

        public void Prepare(string kierId, string dTo)
        {
            DateFrom = String.Format("{0}-01-01", dTo.Substring(0,4));
            DateTo = dTo;
            KierId = kierId;
            InitPath(kierId);
            SqlDataSource1.SelectCommand = GetSql();
        }

        public void DataBindZoom(string kierId)   // daty musza byc ustawione !!!
        {
            KierId = kierId;
            SqlDataSource1.SelectCommand = GetSql();
            lvUrlopy.DataBind();
        }

        public static string _GetSql(string dataOd, string dataDo, string select, string where)  // do Wniosków - 
        {
#if SIEMENS
            const string sql = @"
select P.Id,P.Nazwisko + ' ' + P.Imie as NazwiskoImie,P.KadryId,D.Nazwa as Dzial,P.Kierownik,
    --P.IdKierownika, 
    P.KierId as IdKierownika, 
    K.Nazwisko + ' ' + K.Imie as KierownikNI,
    round(U.UrlopNom / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNom, 
    


    --round(U.UrlopNomRok / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNomRok, 
    round(U.UrlopNom / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNomRok, 



    round(U.UrlopZaleg / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopZaleg,
    
    (select ISNULL(sum(A.IleDni),0) 
		 from Absencja A 
		 where A.IdPracownika=P.Id 
            --and A.Kod in (510, 511) 
	        and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYWYP' and Aktywny = 1), '510,511'), ','))
            AND A.DataOd >= '{1}' AND A.DataDo <= dbo.eoy('{1}')) as UrlopWyk,

    ((select ISNULL(sum(A.IleDni),0) 
		 from Absencja A 
		 where A.IdPracownika=P.Id 
            --and A.Kod in (510, 511) 
	        and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYWYP' and Aktywny = 1), '510,511'), ','))
            AND A.DataOd >= '{1}' AND A.DataDo <= '{2}') +
	 ISNULL((select DATEDIFF(DAY, A.DataOd, '{2}') + 1 - (select COUNT(*) from Kalendarz where A.DataOd <= Data and Data <= '{2}')
		 from Absencja A 
		 where A.IdPracownika=P.Id 
            --and A.Kod in (510, 511) 
	        and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYWYP' and Aktywny = 1), '510,511'), ','))
            AND A.DataOd >= '{1}' AND A.DataOd <= '{2}' and '{2}' < A.DataDo),0)
    ) as WykDoDn,
    ((select ISNULL(sum(A.IleDni),0) 
		 from Absencja A 
		 where A.IdPracownika=P.Id 
            --and A.Kod in (510) 
	        and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYZAD' and Aktywny = 1), '510'), ','))
            AND A.DataOd >= '{1}' AND A.DataDo <= '{2}') +
	 ISNULL((select DATEDIFF(DAY, A.DataOd, '{2}') + 1 - (select COUNT(*) from Kalendarz where A.DataOd <= Data and Data <= '{2}')
		 from Absencja A 
		 where A.IdPracownika=P.Id 
            --and A.Kod in (510) 
	        and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYZAD' and Aktywny = 1), '510'), ','))
            AND A.DataOd >= '{1}' AND A.DataOd <= '{2}' and '{2}' < A.DataDo),0)
    ) as NaZadanie
{4}
--from Pracownicy P
--left outer join Pracownicy K on K.Id = P.IdKierownika
from VPrzypisaniaNaDzis P
left outer join Pracownicy K on K.Id = P.KierId
left outer join Dzialy D ON D.Id = P.IdDzialu
left outer join UrlopZbior U on U.Rok = '{0}' and U.KadryId = P.KadryId
where P.Status >= 0 {3}
order by P.Kierownik desc, NazwiskoImie
";
#else
            const string sql = @"
select P.Id,P.Nazwisko + ' ' + P.Imie as NazwiskoImie,P.KadryId,D.Nazwa as Dzial,P.Kierownik,
    --P.IdKierownika, 
    P.KierId as IdKierownika, 
    K.Nazwisko + ' ' + K.Imie as KierownikNI,
    round(U.UrlopNom / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNom, 
    


    --round(U.UrlopNomRok / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNomRok, 
    round(U.UrlopNom / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNomRok, 



    round(U.UrlopZaleg / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopZaleg,
    round(U.UrlopWyk / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopWyk,    
    ((select ISNULL(sum(A.IleDni),0) 
		 from Absencja A 
		 where A.IdPracownika=P.Id 
            --and A.Kod in (7,19,1000090080,1000) 
	        and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYWYP' and Aktywny = 1), '7,19,1000090080,1000'), ','))
            AND A.DataOd >= '{1}' AND A.DataDo <= '{2}') +
	 ISNULL((select DATEDIFF(DAY, A.DataOd, '{2}') + 1 - (select COUNT(*) from Kalendarz where A.DataOd <= Data and Data <= '{2}')
		 from Absencja A 
		 where A.IdPracownika=P.Id 
            --and A.Kod in (7,19,1000090080,1000) 
	        and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYWYP' and Aktywny = 1), '7,19,1000090080,1000'), ','))
            AND A.DataOd >= '{1}' AND A.DataOd <= '{2}' and '{2}' < A.DataDo),0)
    ) as WykDoDn,
    ((select ISNULL(sum(A.IleDni),0) 
		 from Absencja A 
		 where A.IdPracownika=P.Id 
            --and A.Kod in (19,1000) 
	        and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYZAD' and Aktywny = 1), '19,1000'), ','))
            AND A.DataOd >= '{1}' AND A.DataDo <= '{2}') +
	 ISNULL((select DATEDIFF(DAY, A.DataOd, '{2}') + 1 - (select COUNT(*) from Kalendarz where A.DataOd <= Data and Data <= '{2}')
		 from Absencja A 
		 where A.IdPracownika=P.Id 
            --and A.Kod in (19,1000) 
	        and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYZAD' and Aktywny = 1), '19,1000'), ','))
            AND A.DataOd >= '{1}' AND A.DataOd <= '{2}' and '{2}' < A.DataDo),0)
    ) as NaZadanie
{4}
--from Pracownicy P
--left outer join Pracownicy K on K.Id = P.IdKierownika
from VPrzypisaniaNaDzis P
left outer join Pracownicy K on K.Id = P.KierId
left outer join Dzialy D ON D.Id = P.IdDzialu
left outer join UrlopZbior U on U.Rok = '{0}' and U.KadryId = P.KadryId
where P.Status >= 0 {3}
order by P.Kierownik desc, NazwiskoImie
";
#endif
            return String.Format(sql, dataOd.Substring(0, 4), dataOd, dataDo, where, select);
        }















        public string GetSql()
        {
            /*
select P.Id,P.Nazwisko + ' ' + P.Imie as NazwiskoImie,P.KadryId,D.Nazwa as Dzial,P.Kierownik,
	U.UrlopNom / 8 as UrlopNom, 
    U.UrlopZaleg / 8 as UrlopZaleg,
    U.UrlopWyk / 8 as UrlopWyk,
    sum(ISNULL(A.IleDni, 0)) as WykDoDn
from Pracownicy P
left outer join Dzialy D ON D.Id = P.IdDzialu
left outer join UrlopZbior U on U.Rok = {0} and U.KadryId = P.KadryId
left outer join Absencja A on A.IdPracownika=P.Id and (A.Kod=7 or A.Kod=19 1000090080, 1000) AND 
	(A.DataOd >= '{1}') AND (A.DataDo <= '{2}')
where P.Status >= 0 {3}
group by P.Id, P.Nazwisko, P.Imie, P.KadryId, D.Nazwa, P.Kierownik,
	U.UrlopNom, U.UrlopZaleg, U.UrlopWyk
order by P.Kierownik desc, NazwiskoImie
             */
            /*
            const string sql_1 = @"
select P.Id,P.Nazwisko + ' ' + P.Imie as NazwiskoImie,P.KadryId,D.Nazwa as Dzial,P.Kierownik,
	round(U.UrlopNom / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNom, 
    round(U.UrlopZaleg / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopZaleg,
    round(U.UrlopWyk / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopWyk,
    (select ISNULL(sum(A.IleDni),0) from Absencja A where A.IdPracownika=P.Id and (A.Kod=7 or A.Kod=19 1000090080, 1000) AND (A.DataOd >= '{1}') AND (A.DataDo <= '{2}')) as WykDoDn,
    (select ISNULL(sum(A.IleDni),0) from Absencja A where A.IdPracownika=P.Id and (A.Kod=19 1000) AND (A.DataOd >= '{1}') AND (A.DataDo <= '{2}')) as NaZadanie
from Pracownicy P
left outer join Dzialy D ON D.Id = P.IdDzialu
left outer join UrlopZbior U on U.Rok = {0} and U.KadryId = P.KadryId
where P.Status >= 0 {3}
order by P.Kierownik desc, NazwiskoImie
";
      ,(select ISNULL(sum(A.IleDni),0) from Absencja A where A.IdPracownika=P.Id and (A.Kod=7 or A.Kod=19, 1000090080,1000) AND (A.DataOd >= '{1}') AND (A.DataDo <= '{2}')) as WykDoDn1
      ,(select ISNULL(sum(A.IleDni),0) from Absencja A where A.IdPracownika=P.Id and (A.Kod=19,1000) AND (A.DataOd >= '{1}') AND (A.DataDo <= '{2}')) as NaZadanie1
            */
#if SIEMENS
            const string sql = @"
select P.Id,P.Nazwisko + ' ' + P.Imie as NazwiskoImie,P.KadryId,D.Nazwa as Dzial,P.Kierownik,
    --P.IdKierownika, 
    P.KierId as IdKierownika, 
    K.Nazwisko + ' ' + K.Imie as KierownikNI,
	round(U.UrlopNom / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNom, 
    


    --round(U.UrlopNomRok / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNomRok, 
    round(U.UrlopNom / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNomRok, 



    round(U.UrlopZaleg / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopZaleg,

    (select ISNULL(sum(A.IleDni),0) 
		 from Absencja A 
		 where A.IdPracownika=P.Id 
            --and A.Kod in (510, 511) 
	        and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYWYP' and Aktywny = 1), '510,511'), ','))
            AND A.DataOd >= '{1}' AND A.DataDo <= dbo.eoy('{1}')) as UrlopWyk,

    ((select ISNULL(sum(A.IleDni),0) 
		 from Absencja A 
		 where A.IdPracownika=P.Id 
            --and A.Kod in (510, 511) 
	        and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYWYP' and Aktywny = 1), '510,511'), ','))
            AND A.DataOd >= '{1}' AND A.DataDo <= '{2}') +
	 ISNULL((select DATEDIFF(DAY, A.DataOd, '{2}') + 1 - (select COUNT(*) from Kalendarz where A.DataOd <= Data and Data <= '{2}')
		 from Absencja A 
		 where A.IdPracownika=P.Id 
            --and A.Kod in (510, 511) 
            and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYWYP' and Aktywny = 1), '510,511'), ','))
            AND A.DataOd >= '{1}' AND A.DataOd <= '{2}' and '{2}' < A.DataDo),0)
    ) as WykDoDn,
    ((select ISNULL(sum(A.IleDni),0) 
		 from Absencja A 
		 where A.IdPracownika=P.Id 
            --and A.Kod in (510) 
	        and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYZAD' and Aktywny = 1), '510'), ','))
            AND A.DataOd >= '{1}' AND A.DataDo <= '{2}') +
	 ISNULL((select DATEDIFF(DAY, A.DataOd, '{2}') + 1 - (select COUNT(*) from Kalendarz where A.DataOd <= Data and Data <= '{2}')
		 from Absencja A 
		 where A.IdPracownika=P.Id 
            --and A.Kod in (510) 
	        and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYZAD' and Aktywny = 1), '510'), ','))
            AND A.DataOd >= '{1}' AND A.DataOd <= '{2}' and '{2}' < A.DataDo),0)
    ) as NaZadanie
{4}
--from Pracownicy P
--left outer join Pracownicy K on K.Id = P.IdKierownika
from VPrzypisaniaNaDzis P
left outer join Pracownicy K on K.Id = P.KierId
left outer join Dzialy D ON D.Id = P.IdDzialu
left outer join UrlopZbior U on U.Rok = '{0}' and U.KadryId = P.KadryId
where P.Status >= 0 {3}
order by {5} P.Kierownik desc, NazwiskoImie
";
#else
            const string sql = @"
select P.Id,P.Nazwisko + ' ' + P.Imie as NazwiskoImie,P.KadryId,D.Nazwa as Dzial,P.Kierownik,
    --P.IdKierownika, 
    P.KierId as IdKierownika, 
    K.Nazwisko + ' ' + K.Imie as KierownikNI,
	round(U.UrlopNom / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNom, 
    


    --round(U.UrlopNomRok / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNomRok, 
    round(U.UrlopNom / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNomRok, 



    round(U.UrlopZaleg / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopZaleg,

    round(U.UrlopWyk / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopWyk,    

    ((select ISNULL(sum(A.IleDni),0) 
		 from Absencja A 
		 where A.IdPracownika=P.Id 
            --and A.Kod in (7,19,1000090080,1000) 
	        and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYWYP' and Aktywny = 1), '7,19,1000090080,1000'), ','))
            AND A.DataOd >= '{1}' AND A.DataDo <= '{2}') +
	 ISNULL((select DATEDIFF(DAY, A.DataOd, '{2}') + 1 - (select COUNT(*) from Kalendarz where A.DataOd <= Data and Data <= '{2}')
		 from Absencja A 
		 where A.IdPracownika=P.Id 
            --and A.Kod in (7,19,1000090080,1000) 
	        and A.Kod in (select items from dbo.SplitInt(ISNULL((select Parametr from Kody where Typ = 'RURLOPYWYP' and Aktywny = 1), '7,19,1000090080,1000'), ','))
            AND A.DataOd >= '{1}' AND A.DataOd <= '{2}' and '{2}' < A.DataDo),0)
    ) as WykDoDn,
    ((select ISNULL(sum(A.IleDni),0) 
		 from Absencja A 
		 where A.IdPracownika=P.Id and A.Kod in (19,1000) AND A.DataOd >= '{1}' AND A.DataDo <= '{2}') +
	 ISNULL((select DATEDIFF(DAY, A.DataOd, '{2}') + 1 - (select COUNT(*) from Kalendarz where A.DataOd <= Data and Data <= '{2}')
		 from Absencja A 
		 where A.IdPracownika=P.Id and A.Kod in (19,1000) AND A.DataOd >= '{1}' AND A.DataOd <= '{2}' and '{2}' < A.DataDo),0)
    ) as NaZadanie
{4}
--from Pracownicy P
--left outer join Pracownicy K on K.Id = P.IdKierownika
from VPrzypisaniaNaDzis P
left outer join Pracownicy K on K.Id = P.KierId
left outer join Dzialy D ON D.Id = P.IdDzialu
left outer join UrlopZbior U on U.Rok = '{0}' and U.KadryId = P.KadryId
where P.Status >= 0 {3}
order by {5} P.Kierownik desc, NazwiskoImie
";
#endif
            if (String.IsNullOrEmpty(KierId)) return null;
            else if (KierId == "-100")//"ALL")                                                                          
                return String.Format(sql, DateFrom.Substring(0, 4), DateFrom, DateTo, 
                    "", 
                    ",0 as Ja", 
                    "");
            else
                //return String.Format(sql, DateFrom.Substring(0, 4), DateFrom, DateTo, " and P.IdKierownika = " + KierId);
                if (withMe)
                    return String.Format(sql, DateFrom.Substring(0, 4), DateFrom, DateTo, 
                        String.Format(" and (P.KierId = {0} or P.Id = {0})", KierId), 
                        string.Format(",case when P.Id = {0} then 1 else 0 end as Ja", KierId), 
                        "Ja desc,"); 
                else
                    return String.Format(sql, DateFrom.Substring(0, 4), DateFrom, DateTo, 
                        String.Format(" and (P.KierId = {0})", KierId), 
                        ",0 as Ja", 
                        "");
        }

















        //--------------
        public static string GetPracUrlopSql(string pracId, DateTime dOd, DateTime dDo)
        {
            const string sql = @"
declare @pracId int
declare @rok int
set @pracId = {0}
set @rok = {1}
select 
	round((U.UrlopNom + U.UrlopZaleg - U.UrlopWyk) / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as Limit, 
	round(U.UrlopNom / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNom, 
    


    --round(U.UrlopNomRok / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNomRok, 
    round(U.UrlopNom / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNomRok, 



    round(U.UrlopZaleg / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopZaleg,
    round(U.UrlopWyk / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopWyk    
from Pracownicy P
left outer join UrlopZbior U on U.Rok = @rok and U.KadryId = P.KadryId
where P.Id = @pracId
                ";
            return String.Format(sql, pracId, dOd.Year);
        }
        //-----------------------------------------------------------
        private void PrepareViewHeader(Control cnt)   //ListView
        {
            bool all = KierId == "-100";
            Tools.SetControlVisible(cnt, "thKier", all);
        }

        private void PrepareView(Control cnt)
        {
            bool all = KierId == "-100";
            Tools.SetControlVisible(cnt, "tdKier", all);

        }
        //-----------------------------------------------------------
        protected void lv_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "SubItems":
                    string par = e.CommandArgument.ToString();  // kierId

                    //LinkButton lbt = (LinkButton)e.Item.FindControl("lbtPracownik");
                    LinkButton lbt = (LinkButton)e.CommandSource;
                    if (lbt != null && !String.IsNullOrEmpty(par))
                    {
                        string name = lbt.Text;
                        cntPath.AddPath(name, par, null);
                        DataBindZoom(par);
                    }
                    break;
                case "zoom":
                    string pid = e.CommandArgument.ToString();  // kierId
                    //cntUrlop.Show(pid, "divZoom", Parent.Parent.ClientID);
                    zoomUrlopy.Show(pid, Parent.Parent.ClientID);  // updatePanel
                    break;
            }
        }
        //------------
        protected void OnSelectPath(object sender, EventArgs e)
        {
            DataBindZoom(cntPath.SelParam);
        }
        //-------------
        protected void lv_LayoutCreated(object sender, EventArgs e)
        {

        }

        protected void lv_DataBinding(object sender, EventArgs e)
        {   
        }

        protected void lv_DataBound(object sender, EventArgs e)
        {
            PrepareViewHeader(lvUrlopy);
        }

        protected void lv_PreRender(object sender, EventArgs e)    // dzięki temu nie znikają zaznaczenia krzyżowe
        {
        }
        //-------------------------------------------
        protected void lv_ItemCreated(object sender, ListViewItemEventArgs e)
        {
        }

        protected void lv_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                PrepareView(e.Item);

                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                DataRowView drv = (DataRowView)dataItem.DataItem;
                string pracId = drv["Id"].ToString();
                //----- kierownik / pracownik ------
                bool ja;
                if (withMe)
                    ja = db.getInt(drv["Ja"], 0) == 1;
                else
                    ja = false;
                bool kier = Base.getBool(drv["Kierownik"], false) && !ja;
                Tools.SetControlVisible(e.Item, "PracownikLabel", !kier);
                Tools.SetControlVisible(e.Item, "lbtPracownik", kier);
                //----- wartości -----
                object o1 = drv["UrlopNom"];
                object o2 = drv["UrlopZaleg"];
                object o3 = drv["UrlopWyk"];
                object o4 = drv["WykDoDn"];
                
                double d1 = db.isNull(o1) ? 0 : Convert.ToDouble(o1);
                double d2 = db.isNull(o2) ? 0 : Convert.ToDouble(o2);
                double d3 = db.isNull(o3) ? 0 : Convert.ToDouble(o3);
                double d4 = db.isNull(o4) ? 0 : Convert.ToDouble(o4);
                string dd = Worktime.Round05(d1 + d2 - d3, 2).ToString().Replace(".", ",");
                if (dd.EndsWith(".00")) dd = dd.Remove(dd.Length - 3);
                Tools.SetText(e.Item, "lbDoWyk", dd);

                dd = Worktime.Round05(d1 + d2 - d4, 2).ToString().Replace(".", ",");
                if (dd.EndsWith(".00")) dd = dd.Remove(dd.Length - 3);
                Tools.SetText(e.Item, "lbDoWykNaDzien", dd);
            }
        }

        //----------
        protected void lv_Unload(object sender, EventArgs e)
        {
        }

        //-----------------------------------------------------------------
        public ListView List
        {
            get { return lvUrlopy; }
        }

        public string KierId
        {
            get { return hidKierId.Value; }
            set { hidKierId.Value = value; }
        }
      
        public string DateFrom
        {
            get { return hidFrom.Value; }
            set { hidFrom.Value = value; }
        }

        public string DateTo
        {
            get { return hidTo.Value; }
            set { hidTo.Value = value; }
        }
    }
}