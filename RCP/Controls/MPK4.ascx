<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MPK4.ascx.cs" Inherits="HRRcp.Controls.MPK4" %>


<%@ Register src="TimeEdit.ascx" tagname="TimeEdit" tagprefix="uc1" %>
<%@ Register Src="~/Controls/Reports/cntReport2.ascx" TagPrefix="uc1" TagName="cntReport2" %>


<asp:HiddenField ID="hidPlanId" runat="server" />
<asp:HiddenField ID="hidPracId" runat="server" />
<asp:HiddenField ID="hidData" runat="server" />
<asp:HiddenField ID="hidCzasZm3" runat="server" />
<asp:HiddenField ID="hidNadgD3" runat="server" />
<asp:HiddenField ID="hidNadgN3" runat="server" />
<asp:HiddenField ID="hidNocne3" runat="server" />

<asp:HiddenField ID="hidSQL1" runat="server" />
<asp:HiddenField ID="hidSQL2" runat="server" />

<uc1:cntReport2 runat="server" ID="cntReport2" GridVisible="false" CssClass="report_page noborder report-header" SQL="
declare @pracId int = @SQL1

/*
declare @od datetime = '2016-12-20 07:58'
declare @do datetime = '2016-12-20 16:16'

declare @cp_od  datetime  = '20161220 08:00'
declare @cp_do  datetime  = '20161220 16:00'

declare @noc_od datetime  = /*'20161220 13:00'*/ (select top 1 NocneOd from Ustawienia)
declare @noc_do datetime  = /*'20161221 22:00'*/ (select top 1 NocneDo from Ustawienia)

declare @nadg_od datetime = '20161220 16:00'
declare @nadg_do datetime = '20161220 16:20'
*/

declare @od datetime = NULLIF((select items from dbo.SplitStr('@SQL2', '|') a where a.idx = 0), 'NULL')
declare @do datetime = NULLIF((select items from dbo.SplitStr('@SQL2', '|') a where a.idx = 1), 'NULL')

declare @cp_od  datetime  = NULLIF((select items from dbo.SplitStr('@SQL2', '|') a where a.idx = 2), 'NULL')
declare @cp_do  datetime  = NULLIF((select items from dbo.SplitStr('@SQL2', '|') a where a.idx = 3), 'NULL')

declare @noc_od datetime  = (select top 1 NocneOd from Ustawienia)
declare @noc_do datetime  = (select top 1 NocneDo from Ustawienia)

declare @nadg_od datetime = NULLIF((select items from dbo.SplitStr('@SQL2', '|') a where a.idx = 4), 'NULL')
declare @nadg_do datetime = NULLIF((select items from dbo.SplitStr('@SQL2', '|') a where a.idx = 5), 'NULL')

/*********************************************************************************************************************************************
 *                                                                                                                                           *
 *                                                                                                                                           *
 *                                                                                                                                           *
 *                                                                                                                                           *
 *                                                                                                                                           *
 *********************************************************************************************************************************************/

select
  ROW_NUMBER() over (order by rcp.Czas, x.s) Kolejnosc
, rcp.Czas
, x.s
, x.WeWy
, sr.IdStrefy
, s.IdCC
, s.Nazwa
, r.Name Komentarz
into #aaa
from RCP rcp
cross join (select '' a, 1 s, 0 WeWy union select '-', 0, 1) x
inner join StrefyReaders sr on x.a + CONVERT(varchar, rcp.ECReaderId) in (select items from dbo.SplitStr(sr.Readers, ','))
inner join Strefy s on s.Id = sr.IdStrefy and s.Aktywna = 1
left join Readers r on r.Id = rcp.ECReaderId
/*where dbo.getdate(rcp.Czas) = '20161220' and rcp.ECUserId = 1*/
where rcp.ECUserId = @pracId and rcp.Czas between @od and @do
order by rcp.Czas, x.s

/*********************************************************************************************************************************************/

/*
select * from #aaa
*/

/*********************************************************************************************************************************************
 *                                                                                                                                           *
 *                                                                                                                                           *
 *  ????                                                                                                                                     *
 *                                                                                                                                           *
 *                                                                                                                                           *
 *********************************************************************************************************************************************/

select
  FLOOR((ROW_NUMBER() over (order by a.Czas, a.s) - 1) / 2) Grupa /* to skoda tez wymyslil */
, *
, case when Czas &gt;   @cp_od and Czas &lt;   @cp_do then 1 else 0 end CzasPracy
, case when Czas &gt;  @noc_od and Czas &lt;  @noc_do then 1 else 0 end Noc
, case when Czas &gt; @nadg_od and Czas &lt; @nadg_do then 1 else 0 end Nadgodziny
, 0 dev
into #bbb
from
(
	select
	  *
	/*, CONVERT(varchar, NULL) Alert*/
	from #aaa
	union
	select
	  0
	, b.Czas
	, a.WeWy s
	, ABS(a.WeWy - 1) WeWy /* skoda wymyslil jak odwracac arytmetycznie zera i jedynki */
	, a.IdStrefy
	, a.IdCC
	, a.Nazwa
	/*, 'a' Alert*/
	, '????'
	from #aaa a
	inner join #aaa b on b.Kolejnosc = a.Kolejnosc + 1 and a.WeWy = b.WeWy and a.WeWy = 0
	union
	select
	  0
	, a.Czas
	, a.WeWy s
	, ABS(a.WeWy - 1) WeWy
	, b.IdStrefy
	, b.IdCC
	, b.Nazwa
	/*, 'b'*/
	, '????'
	from #aaa a
	inner join #aaa b on b.Kolejnosc = a.Kolejnosc + 1 and a.WeWy = b.WeWy and a.WeWy = 1
	/*union
	select
	  0
	, case when x.x = 0 then b.Czas else a.Czas end
	, x.x
	, ABS(x.x - 1)
	, NULL
	, NULL
	, 'Poza strefą'
	, 'c'
	from #aaa a
	inner join #aaa b on b.Kolejnosc = a.Kolejnosc + 1 and a.WeWy != b.WeWy and a.Czas != b.Czas and a.IdStrefy != b.IdStrefy
	cross join (select 0 x union select 1) x*/
) a
order by a.Czas, a.s

/*********************************************************************************************************************************************/

/*
select * from #bbb
*/

/*********************************************************************************************************************************************
 *                                                                                                                                           *
 * Rozpoczęcie/zakończenie czasu pracy                                                                                                       *
 *                                                                                                                                           *
 * dev=1                                                                                                                                     *
 *                                                                                                                                           *
 *********************************************************************************************************************************************/

select
  FLOOR((ROW_NUMBER() over (order by a.Czas, a.s) - 1) / 2) Grupa
, a.Kolejnosc
, a.Czas
, a.s
, a.WeWy
, a.IdStrefy
, a.IdCC
, a.Nazwa

, a.Komentarz

, a.CzasPracy
, a.Noc
, a.Nadgodziny

, a.dev
into #ccc
from
(
	select
	  *
	from #bbb
	union
	select
	  -1 Grupa
	, 0 Kolejnosc
	, case when a.CzasPracy = 0 then @cp_od else @cp_do end Czas
	, x.x s
	, ABS(x.x - 1) WeWy
	, case when x.x = 0 then a.IdStrefy   else b.IdStrefy   end IdStrefy
	, case when x.x = 0 then a.IdCC       else b.IdCC       end IdCC
	, case when x.x = 0 then a.Nazwa      else b.Nazwa      end Nazwa

	, '# Rozpoczęcie/zakończenie czasu pracy' Komentarz

	, case when x.x = 0 then a.CzasPracy  else b.CzasPracy  end CzasPracy
	, case when y.y = @noc_od or y.y = @noc_do then x.x else case when y.y between @noc_od and @noc_do then 1 else 0 end end Noc
	, case when y.y = @nadg_od or y.y = @nadg_do then x.x else case when y.y between @nadg_od and @nadg_do then 1 else 0 end end Nadgodziny

	, 1 dev
	from #bbb a
	inner join #bbb b on b.Grupa = a.Grupa and a.WeWy = 0 and b.WeWy = 1 and a.CzasPracy != b.CzasPracy
	cross join (select 0 x union select 1) x
	outer apply (select case when a.CzasPracy = 0 then @cp_od else @cp_do end y) y
) a
order by a.Czas, a.s

/*********************************************************************************************************************************************/

/*
select * from #ccc
*/

/*********************************************************************************************************************************************
 *                                                                                                                                           *
 * Rozpoczęcie/zakończenie nocy                                                                                                              *
 *                                                                                                                                           *
 * dev=2                                                                                                                                     *
 *                                                                                                                                           *
 *********************************************************************************************************************************************/

select
  FLOOR((ROW_NUMBER() over (order by a.Czas, a.s) - 1) / 2) Grupa
, a.Kolejnosc
, a.Czas
, a.s
, a.WeWy
, a.IdStrefy
, a.IdCC
, a.Nazwa

, a.Komentarz

, a.CzasPracy
, a.Noc Noc
, a.Nadgodziny

, a.dev
into #ddd
from
(
	select
	  *
	from #ccc
	union
	select
	  -1 Grupa
	, 0 Kolejnosc
	, case when a.Noc = 0 then @noc_od else @noc_do end Czas
	, x.x s
	, ABS(x.x - 1) WeWy
	, case when x.x = 0 then a.IdStrefy   else b.IdStrefy   end IdStrefy
	, case when x.x = 0 then a.IdCC       else b.IdCC       end IdCC
	, case when x.x = 0 then a.Nazwa      else b.Nazwa      end Nazwa

	, '# Rozpoczęcie/zakończenie czasu pracy w nocy' Komentarz

	, case when y.y = @cp_od or y.y = @cp_do then x.x else case when y.y between @cp_od and @cp_do then 1 else 0 end end CzasPracy
	, case when x.x = 0 then a.Noc        else b.Noc        end Noc
	, case when y.y = @nadg_od or y.y = @nadg_do then x.x else case when y.y between @nadg_od and @nadg_do then 1 else 0 end end Nadgodziny

	, 2 dev
	from #ccc a
	inner join #ccc b on b.Grupa = a.Grupa and a.WeWy = 0 and b.WeWy = 1 and a.Noc != b.Noc
	cross join (select 0 x union select 1) x
	outer apply (select case when a.Noc = 0 then @noc_od else @noc_do end y) y
) a
order by a.Czas, a.s

/*********************************************************************************************************************************************/

/*
select * from #ddd
*/

/*********************************************************************************************************************************************
 *                                                                                                                                           *
 * Rozpoczęcie/zakończenie nadgodzin                                                                                                         *
 *  nadgodzin                                                                                                                                *
 * dev=3                                                                                                                                     *
 *                                                                                                                                           *
 *********************************************************************************************************************************************/

select
  FLOOR((ROW_NUMBER() over (order by a.Czas, a.s) - 1) / 2) Grupa
, a.Kolejnosc
, a.Czas
, a.s
, a.WeWy
, a.IdStrefy
, a.IdCC
, a.Nazwa

, a.Komentarz

, a.CzasPracy
, a.Noc
, a.Nadgodziny

, a.dev
into #zzz
from
(
	select
	  *
	from #ddd
	union
	select
	  -1 Grupa
	, 0 Kolejnosc
	, case when a.Nadgodziny = 0 then @nadg_od else @nadg_do end Czas
	, x.x s
	, ABS(x.x - 1) WeWy
	, case when x.x = 0 then a.IdStrefy   else b.IdStrefy   end IdStrefy
	, case when x.x = 0 then a.IdCC       else b.IdCC       end IdCC
	, case when x.x = 0 then a.Nazwa      else b.Nazwa      end Nazwa

	, '# Rozpoczęcie/zakończenie nadgodzin' Komentarz

	, case when y.y = @cp_od or y.y = @cp_do then x.x else case when y.y between @cp_od and @cp_do then 1 else 0 end end CzasPracy
	, case when y.y = @noc_od or y.y = @noc_do then x.x else case when y.y between @noc_od and @noc_do then 1 else 0 end end Noc
	, case when x.x = 0 then a.Nadgodziny else b.Nadgodziny end Nadgodziny

	, 3 dev
	from #ddd a
	inner join #ddd b on b.Grupa = a.Grupa and a.WeWy = 0 and b.WeWy = 1 and a.Nadgodziny != b.Nadgodziny
	cross join (select 0 x union select 1) x
	outer apply (select case when a.Nadgodziny = 0 then @nadg_od else @nadg_do end y) y
) a
order by a.Czas, a.s

/*********************************************************************************************************************************************/

/*
select * from #zzz
*/

/*********************************************************************************************************************************************
 *                                                                                                                                           *
 *                                                                                                                                           *
 * Obóz koncentracyjny                                                                                                                       *
 *                                                                                                                                           *
 *                                                                                                                                           *
 *********************************************************************************************************************************************/

select
  a.Nazwa
, a.IdCC
, a.Czas We
, b.Czas Wy
, DATEDIFF(SECOND, a.Czas, b.Czas) Czas
, case when a.IdStrefy != b.IdStrefy then 1 else 0 end ErrorLevel
, case when a.IdStrefy != b.IdStrefy then 'Uwaga! Wyjście na: ''' + b.Nazwa + '''' else '' end Alert
, a.CzasPracy
, a.Noc
, a.Nadgodziny
, a.Komentarz
into #owari
from #zzz a
inner join #zzz b on b.Grupa = a.Grupa and b.WeWy = 1
where a.WeWy = 0 and a.Czas != b.Czas

/*********************************************************************************************************************************************/

/*********************************************************************************************************************************************
 *                                                                                                                                           *
 *                                                                                                                                           *
 *                                                                                                                                           *
 *                                                                                                                                           *
 *                                                                                                                                           *
 *********************************************************************************************************************************************/

select
  a.Nazwa Strefa
, ISNULL(cc.cc, 'mac.') MPK
, RIGHT(CONVERT(varchar, a.We, 20), 8) Od
, RIGHT(CONVERT(varchar, a.Wy, 20), 8) Do
/*, a.Czas*/
, dbo.ToTimeHMMSS(a.Czas) Czas
, a.Komentarz [Czytnik / zdarzenie - Czas Od]
, a.Alert
/*, case when a.CzasPracy  = 1 then 'x' else '' end Zmiana
, case when a.Noc		 = 1 then 'x' else '' end Noc
, case when a.Nadgodziny = 1 then 'x' else '' end Nadgodziny*/
from #owari a
left join CC cc on cc.Id = a.IdCC
order by a.We

/*
select * into #final from
(
	select
		a.IdCC
	, SUM(a.Czas) Zmiana
	, 0 NadgDzien
	, 0 NadgNoc
	, 0 Nocne
	from #owari a
	where a.CzasPracy = 1 and a.Noc = 0
	group by a.IdCC
	union all
	select
		a.IdCC
	, SUM(a.Czas) Zmiana
	, 0 NadgDzien
	, 0 NadgNoc
	, SUM(a.Czas) Nocne
	from #owari a
	where a.CzasPracy = 1 and a.Noc = 1
	group by a.IdCC
	union all
	select
		a.IdCC
	, 0 Zmiana
	, SUM(a.Czas) NadgDzien
	, 0 NadgNoc
	, 0 Nocne
	from #owari a
	where a.Nadgodziny = 1 and a.Noc = 0
	group by a.IdCC
	union all
	select
		a.IdCC
	, 0 Zmiana
	, 0 NadgDzien
	, SUM(a.Czas) NadgNoc
	, SUM(a.Czas) Nocne
	from #owari a
	where a.Nadgodziny = 1 and a.Noc = 1
	group by a.IdCC
) a

/*
select
  a.IdCC
/*, dbo.ToTimeHMM(a.Zmiana) ZmianaHMM*/
/*, dbo.ToTimeHMM(a.NadgDzien) NadgDzienHMM*/
/*, dbo.ToTimeHMM(a.NadgNoc) NadgNocHMM*/
/*, dbo.ToTimeHMM(a.Nocne) NocneHMM*/
, SUM(a.Zmiana) CzasZm
, SUM(a.NadgDzien) n50
, SUM(a.NadgNoc) n100
, SUM(a.Nocne) Noc
/*into #final*/
from #final a
group by a.IdCC
order by a.IdCC
*/
/*
select f.*
from #final f
inner join Przypisania r on r.IdPracownika = @pracId and @do between r.Od and ISNULL(r.Do, '20990909')
inner join SplityWspP swp on swp.IdPrzypisania = r.Id
where f.IdCC is null*/
/*union
select *
from #final f
where f.IdCC is not null*/

/*
select
  a.IdCC
, SUM(a.Zmiana) CzasZm
, SUM(a.NadgDzien) n50
, SUM(a.NadgNoc) n100
, SUM(a.Nocne) Noc
from #final a
group by a.IdCC
*/

/*insert PodzialKosztow (IdPracownika, Data, IdMPK, CzasZm, NadgodzinyDzien, NadgodzinyNoc, Nocne)*/
select
  @pracId IdPracownika
, dbo.getdate(@od) Data
, a.IdCC
, SUM(a.CzasZm) CzasZm
, SUM(a.NadgodzinyDzien) NadgodzinyDzien
, SUM(a.NadgodzinyNoc) NadgodzinyNoc
, SUM(a.Nocne) Nocne
from
(
	select
	  swp.IdCC
	, a.Zmiana    * swp.Wsp CzasZm
	, a.NadgDzien * swp.Wsp NadgodzinyDzien
	, a.NadgNoc   * swp.Wsp NadgodzinyNoc
	, a.Nocne     * swp.Wsp Nocne
	from #final a
	inner join Przypisania r on r.IdPracownika = @pracId and @od between r.Od and ISNULL(r.Do, '20990909')
	inner join SplityWspP swp on swp.IdPrzypisania = r.Id
	where a.IdCC is null
	union
	select
	  a.IdCC
	, a.Zmiana
	, a.NadgDzien
	, a.NadgNoc
	, a.Nocne
	from #final a
	where a.IdCC is not null
) a
group by a.IdCC
order by a.IdCC
*/
drop table #aaa
drop table #bbb
drop table #ccc
drop table #ddd
drop table #zzz
drop table #owari
/*drop table #final*/
" />

<asp:SqlDataSource ID="dsFill" runat="server" SelectCommand="
declare @pracId int = @SQL1

/*
declare @od datetime = '2016-12-20 07:58'
declare @do datetime = '2016-12-20 16:16'

declare @cp_od  datetime  = '20161220 08:00'
declare @cp_do  datetime  = '20161220 16:00'

declare @noc_od datetime  = /*'20161220 13:00'*/ (select top 1 NocneOd from Ustawienia)
declare @noc_do datetime  = /*'20161221 22:00'*/ (select top 1 NocneDo from Ustawienia)

declare @nadg_od datetime = '20161220 16:00'
declare @nadg_do datetime = '20161220 16:20'
*/

declare @od datetime = NULLIF((select items from dbo.SplitStr('@SQL2', '|') a where a.idx = 0), 'NULL')
declare @do datetime = NULLIF((select items from dbo.SplitStr('@SQL2', '|') a where a.idx = 1), 'NULL')

declare @cp_od  datetime  = NULLIF((select items from dbo.SplitStr('@SQL2', '|') a where a.idx = 2), 'NULL')
declare @cp_do  datetime  = NULLIF((select items from dbo.SplitStr('@SQL2', '|') a where a.idx = 3), 'NULL')

declare @noc_od datetime  = (select top 1 NocneOd from Ustawienia)
declare @noc_do datetime  = (select top 1 NocneDo from Ustawienia)

declare @nadg_od datetime = NULLIF((select items from dbo.SplitStr('@SQL2', '|') a where a.idx = 4), 'NULL')
declare @nadg_do datetime = NULLIF((select items from dbo.SplitStr('@SQL2', '|') a where a.idx = 5), 'NULL')

/*********************************************************************************************************************************************
 *                                                                                                                                           *
 *                                                                                                                                           *
 *                                                                                                                                           *
 *                                                                                                                                           *
 *                                                                                                                                           *
 *********************************************************************************************************************************************/

select
  ROW_NUMBER() over (order by rcp.Czas, x.s) Kolejnosc
, rcp.Czas
, x.s
, x.WeWy
, sr.IdStrefy
, s.IdCC
, s.Nazwa
into #aaa
from RCP rcp
cross join (select '' a, 1 s, 0 WeWy union select '-', 0, 1) x
inner join StrefyReaders sr on x.a + CONVERT(varchar, rcp.ECReaderId) in (select items from dbo.SplitStr(sr.Readers, ','))
inner join Strefy s on s.Id = sr.IdStrefy and s.Aktywna = 1
/*where dbo.getdate(rcp.Czas) = '20161220' and rcp.ECUserId = 1*/
where rcp.ECUserId = @pracId and rcp.Czas between @od and @do
order by rcp.Czas, x.s

/*********************************************************************************************************************************************/

/*
select * from #aaa
*/

/*********************************************************************************************************************************************
 *                                                                                                                                           *
 *                                                                                                                                           *
 *  ????                                                                                                                                     *
 *                                                                                                                                           *
 *                                                                                                                                           *
 *********************************************************************************************************************************************/

select
  FLOOR((ROW_NUMBER() over (order by a.Czas, a.s) - 1) / 2) Grupa /* to skoda tez wymyslil */
, *
, case when Czas &gt;   @cp_od and Czas &lt;   @cp_do then 1 else 0 end CzasPracy
, case when Czas &gt;  @noc_od and Czas &lt;  @noc_do then 1 else 0 end Noc
, case when Czas &gt; @nadg_od and Czas &lt; @nadg_do then 1 else 0 end Nadgodziny
, 0 dev
into #bbb
from
(
	select
	  *
	/*, CONVERT(varchar, NULL) Alert*/
	from #aaa
	union
	select
	  0
	, b.Czas
	, a.WeWy s
	, ABS(a.WeWy - 1) WeWy /* skoda wymyslil jak odwracac arytmetycznie zera i jedynki */
	, a.IdStrefy
	, a.IdCC
	, a.Nazwa
	/*, 'a' Alert*/
	from #aaa a
	inner join #aaa b on b.Kolejnosc = a.Kolejnosc + 1 and a.WeWy = b.WeWy and a.WeWy = 0
	union
	select
	  0
	, a.Czas
	, a.WeWy s
	, ABS(a.WeWy - 1) WeWy
	, b.IdStrefy
	, b.IdCC
	, b.Nazwa
	/*, 'b'*/
	from #aaa a
	inner join #aaa b on b.Kolejnosc = a.Kolejnosc + 1 and a.WeWy = b.WeWy and a.WeWy = 1
	/*union
	select
	  0
	, case when x.x = 0 then b.Czas else a.Czas end
	, x.x
	, ABS(x.x - 1)
	, NULL
	, NULL
	, 'Poza strefą'
	, 'c'
	from #aaa a
	inner join #aaa b on b.Kolejnosc = a.Kolejnosc + 1 and a.WeWy != b.WeWy and a.Czas != b.Czas and a.IdStrefy != b.IdStrefy
	cross join (select 0 x union select 1) x*/
) a
order by a.Czas, a.s

/*********************************************************************************************************************************************/

/*
select * from #bbb
*/

/*********************************************************************************************************************************************
 *                                                                                                                                           *
 * Rozpoczęcie/zakończenie czasu pracy                                                                                                       *
 *                                                                                                                                           *
 * dev=1                                                                                                                                     *
 *                                                                                                                                           *
 *********************************************************************************************************************************************/

select
  FLOOR((ROW_NUMBER() over (order by a.Czas, a.s) - 1) / 2) Grupa
, a.Kolejnosc
, a.Czas
, a.s
, a.WeWy
, a.IdStrefy
, a.IdCC
, a.Nazwa

, a.CzasPracy
, a.Noc
, a.Nadgodziny

, a.dev
into #ccc
from
(
	select
	  *
	from #bbb
	union
	select
	  -1 Grupa
	, 0 Kolejnosc
	, case when a.CzasPracy = 0 then @cp_od else @cp_do end Czas
	, x.x s
	, ABS(x.x - 1) WeWy
	, case when x.x = 0 then a.IdStrefy   else b.IdStrefy   end IdStrefy
	, case when x.x = 0 then a.IdCC       else b.IdCC       end IdCC
	, case when x.x = 0 then a.Nazwa      else b.Nazwa      end Nazwa

	, case when x.x = 0 then a.CzasPracy  else b.CzasPracy  end CzasPracy
	, case when y.y = @noc_od or y.y = @noc_do then x.x else case when y.y between @noc_od and @noc_do then 1 else 0 end end Noc
	, case when y.y = @nadg_od or y.y = @nadg_do then x.x else case when y.y between @nadg_od and @nadg_do then 1 else 0 end end Nadgodziny

	, 1 dev
	from #bbb a
	inner join #bbb b on b.Grupa = a.Grupa and a.WeWy = 0 and b.WeWy = 1 and a.CzasPracy != b.CzasPracy
	cross join (select 0 x union select 1) x
	outer apply (select case when a.CzasPracy = 0 then @cp_od else @cp_do end y) y
) a
order by a.Czas, a.s

/*********************************************************************************************************************************************/

/*
select * from #ccc
*/

/*********************************************************************************************************************************************
 *                                                                                                                                           *
 * Rozpoczęcie/zakończenie nocy                                                                                                              *
 *                                                                                                                                           *
 * dev=2                                                                                                                                     *
 *                                                                                                                                           *
 *********************************************************************************************************************************************/

select
  FLOOR((ROW_NUMBER() over (order by a.Czas, a.s) - 1) / 2) Grupa
, a.Kolejnosc
, a.Czas
, a.s
, a.WeWy
, a.IdStrefy
, a.IdCC
, a.Nazwa

, a.CzasPracy
, a.Noc Noc
, a.Nadgodziny

, a.dev
into #ddd
from
(
	select
	  *
	from #ccc
	union
	select
	  -1 Grupa
	, 0 Kolejnosc
	, case when a.Noc = 0 then @noc_od else @noc_do end Czas
	, x.x s
	, ABS(x.x - 1) WeWy
	, case when x.x = 0 then a.IdStrefy   else b.IdStrefy   end IdStrefy
	, case when x.x = 0 then a.IdCC       else b.IdCC       end IdCC
	, case when x.x = 0 then a.Nazwa      else b.Nazwa      end Nazwa

	, case when y.y = @cp_od or y.y = @cp_do then x.x else case when y.y between @cp_od and @cp_do then 1 else 0 end end CzasPracy
	, case when x.x = 0 then a.Noc        else b.Noc        end Noc
	, case when y.y = @nadg_od or y.y = @nadg_do then x.x else case when y.y between @nadg_od and @nadg_do then 1 else 0 end end Nadgodziny

	, 2 dev
	from #ccc a
	inner join #ccc b on b.Grupa = a.Grupa and a.WeWy = 0 and b.WeWy = 1 and a.Noc != b.Noc
	cross join (select 0 x union select 1) x
	outer apply (select case when a.Noc = 0 then @noc_od else @noc_do end y) y
) a
order by a.Czas, a.s

/*********************************************************************************************************************************************/

/*
select * from #ddd
*/

/*********************************************************************************************************************************************
 *                                                                                                                                           *
 * Rozpoczęcie/zakończenie nadgodzin                                                                                                         *
 *  nadgodzin                                                                                                                                *
 * dev=3                                                                                                                                     *
 *                                                                                                                                           *
 *********************************************************************************************************************************************/

select
  FLOOR((ROW_NUMBER() over (order by a.Czas, a.s) - 1) / 2) Grupa
, a.Kolejnosc
, a.Czas
, a.s
, a.WeWy
, a.IdStrefy
, a.IdCC
, a.Nazwa

, a.CzasPracy
, a.Noc
, a.Nadgodziny

, a.dev
into #zzz
from
(
	select
	  *
	from #ddd
	union
	select
	  -1 Grupa
	, 0 Kolejnosc
	, case when a.Nadgodziny = 0 then @nadg_od else @nadg_do end Czas
	, x.x s
	, ABS(x.x - 1) WeWy
	, case when x.x = 0 then a.IdStrefy   else b.IdStrefy   end IdStrefy
	, case when x.x = 0 then a.IdCC       else b.IdCC       end IdCC
	, case when x.x = 0 then a.Nazwa      else b.Nazwa      end Nazwa

	, case when y.y = @cp_od or y.y = @cp_do then x.x else case when y.y between @cp_od and @cp_do then 1 else 0 end end CzasPracy
	, case when y.y = @noc_od or y.y = @noc_do then x.x else case when y.y between @noc_od and @noc_do then 1 else 0 end end Noc
	, case when x.x = 0 then a.Nadgodziny else b.Nadgodziny end Nadgodziny

	, 3 dev
	from #ddd a
	inner join #ddd b on b.Grupa = a.Grupa and a.WeWy = 0 and b.WeWy = 1 and a.Nadgodziny != b.Nadgodziny
	cross join (select 0 x union select 1) x
	outer apply (select case when a.Nadgodziny = 0 then @nadg_od else @nadg_do end y) y
) a
order by a.Czas, a.s

/*********************************************************************************************************************************************/

/*
select * from #zzz
*/

/*********************************************************************************************************************************************
 *                                                                                                                                           *
 *                                                                                                                                           *
 * Obóz koncentracyjny                                                                                                                       *
 *                                                                                                                                           *
 *                                                                                                                                           *
 *********************************************************************************************************************************************/

select
  a.Nazwa
, a.IdCC
, a.Czas We
, b.Czas Wy
, DATEDIFF(SECOND, a.Czas, b.Czas) Czas
, case when a.IdStrefy != b.IdStrefy then 1 else 0 end ErrorLevel
, case when a.IdStrefy != b.IdStrefy then 'Uwaga! Wyjście na: ''' + b.Nazwa + '''' else '' end Alert
, a.CzasPracy
, a.Noc
, a.Nadgodziny
into #owari
from #zzz a
inner join #zzz b on b.Grupa = a.Grupa and b.WeWy = 1
where a.WeWy = 0 and a.Czas != b.Czas

/*********************************************************************************************************************************************/

/*********************************************************************************************************************************************
 *                                                                                                                                           *
 *                                                                                                                                           *
 *                                                                                                                                           *
 *                                                                                                                                           *
 *                                                                                                                                           *
 *********************************************************************************************************************************************/
/*
select
  a.Nazwa Strefa
, cc.cc MPK
, RIGHT(CONVERT(varchar, a.We, 20), 8) Od
, RIGHT(CONVERT(varchar, a.Wy, 20), 8) Do
/*, a.Czas*/
, dbo.ToTimeHMM(a.Czas) Czas
, a.Alert
/*, case when a.CzasPracy  = 1 then 'x' else '' end Zmiana
, case when a.Noc		 = 1 then 'x' else '' end Noc
, case when a.Nadgodziny = 1 then 'x' else '' end Nadgodziny*/
from #owari a
left join CC cc on cc.Id = a.IdCC
order by a.We
*/

select * into #final from
(
	select
		a.IdCC
	, SUM(a.Czas) Zmiana
	, 0 NadgDzien
	, 0 NadgNoc
	, 0 Nocne
	from #owari a
	where a.CzasPracy = 1 and a.Noc = 0
	group by a.IdCC
	union all
	select
		a.IdCC
	, SUM(a.Czas) Zmiana
	, 0 NadgDzien
	, 0 NadgNoc
	, SUM(a.Czas) Nocne
	from #owari a
	where a.CzasPracy = 1 and a.Noc = 1
	group by a.IdCC
	union all
	select
		a.IdCC
	, 0 Zmiana
	, SUM(a.Czas) NadgDzien
	, 0 NadgNoc
	, 0 Nocne
	from #owari a
	where a.Nadgodziny = 1 and a.Noc = 0
	group by a.IdCC
	union all
	select
		a.IdCC
	, 0 Zmiana
	, 0 NadgDzien
	, SUM(a.Czas) NadgNoc
	, SUM(a.Czas) Nocne
	from #owari a
	where a.Nadgodziny = 1 and a.Noc = 1
	group by a.IdCC
) a

/*
select
  a.IdCC
/*, dbo.ToTimeHMM(a.Zmiana) ZmianaHMM*/
/*, dbo.ToTimeHMM(a.NadgDzien) NadgDzienHMM*/
/*, dbo.ToTimeHMM(a.NadgNoc) NadgNocHMM*/
/*, dbo.ToTimeHMM(a.Nocne) NocneHMM*/
, SUM(a.Zmiana) CzasZm
, SUM(a.NadgDzien) n50
, SUM(a.NadgNoc) n100
, SUM(a.Nocne) Noc
/*into #final*/
from #final a
group by a.IdCC
order by a.IdCC
*/
/*
select f.*
from #final f
inner join Przypisania r on r.IdPracownika = @pracId and @do between r.Od and ISNULL(r.Do, '20990909')
inner join SplityWspP swp on swp.IdPrzypisania = r.Id
where f.IdCC is null*/
/*union
select *
from #final f
where f.IdCC is not null*/

/*
select
  a.IdCC
, SUM(a.Zmiana) CzasZm
, SUM(a.NadgDzien) n50
, SUM(a.NadgNoc) n100
, SUM(a.Nocne) Noc
from #final a
group by a.IdCC
*/

insert PodzialKosztow (IdPracownika, Data, IdMPK, CzasZm, NadgodzinyDzien, NadgodzinyNoc, Nocne)
select
  @pracId IdPracownika
, dbo.getdate(@od) Data
, a.IdCC
, SUM(a.CzasZm) CzasZm
, SUM(a.NadgodzinyDzien) NadgodzinyDzien
, SUM(a.NadgodzinyNoc) NadgodzinyNoc
, SUM(a.Nocne) Nocne
from
(
	select
	  swp.IdCC
	, a.Zmiana    * swp.Wsp CzasZm
	, a.NadgDzien * swp.Wsp NadgodzinyDzien
	, a.NadgNoc   * swp.Wsp NadgodzinyNoc
	, a.Nocne     * swp.Wsp Nocne
	from #final a
	inner join Przypisania r on r.IdPracownika = @pracId and @od between r.Od and ISNULL(r.Do, '20990909')
	inner join SplityWspP swp on swp.IdPrzypisania = r.Id
	where a.IdCC is null
	union
	select
	  a.IdCC
	, a.Zmiana
	, a.NadgDzien
	, a.NadgNoc
	, a.Nocne
	from #final a
	where a.IdCC is not null
) a
group by a.IdCC
order by a.IdCC

drop table #aaa
drop table #bbb
drop table #ccc
drop table #ddd
drop table #zzz
drop table #owari
drop table #final
"
    UpdateCommand="Uwaga! Przeniesienie danych MPK wiążę się z usunięciem istniejących już dla tego dnia rekordów. Czy na pewno chcesz kontynuować?"
    DeleteCommand="delete from PodzialKosztow where IdPracownika = @pracId and Data = dbo.getdate('@data')"
    InsertCommand="Czy na pewno chcesz usunąć dane MPK?|Brak danych do usunięcia."
    >
</asp:SqlDataSource>

<asp:Button ID="btnFillConfirm" runat="server" Text="Przenieś do Podziału Kosztów" OnClick="btnFillConfirm_Click" CssClass="btn-fill" />
<asp:Button ID="btnFill" runat="server" CssClass="hidden" OnClick="btnFill_Click" />
<asp:Button ID="btnDeleteAllConfirm" runat="server" Text="Wyczyść Podział Kosztów" OnClick="btnDeleteAllConfirm_Click" CssClass="btn-fill" />
<asp:Button ID="btnDeleteAll" runat="server" CssClass="hidden" OnClick="btnDeleteAll_Click" />

<asp:ListView ID="lvMPK" runat="server" DataSourceID="SqlDataSource1" 
    DataKeyNames="Id" InsertItemPosition="LastItem" 
    onitemcreated="lvMPK_ItemCreated" onitemdatabound="lvMPK_ItemDataBound" 
    oniteminserting="lvMPK_ItemInserting" onitemupdating="lvMPK_ItemUpdating" 
    ondatabound="lvMPK_DataBound" onlayoutcreated="lvMPK_LayoutCreated" 
    onitemcanceling="lvMPK_ItemCanceling" onitemediting="lvMPK_ItemEditing" 
    onitemupdated="lvMPK_ItemUpdated" ondatabinding="lvMPK_DataBinding">
    <ItemTemplate>
        <tr class="it">
            <td class="col1">
                <asp:Label ID="MPKLabel" runat="server" Text='<%# Eval("MPK") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="CzasZmLabel" runat="server" Text='<%# Eval("CzasZmT") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="NadgodzinyDzienLabel" runat="server" Text='<%# Eval("NadgodzinyDzienT") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="NadgodzinyNocLabel" runat="server" Text='<%# Eval("NadgodzinyNocT") %>' />
            </td>
            <td id="tdLastCol" runat="server" class="col2">
                <asp:Label ID="NocneLabel" runat="server" Text='<%# Eval("NocneT") %>' />
            </td>
            <td id="tdControl" runat="server" class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" class="tbEnterMPKedt">
            <tr>
                <td>
                    Brak podziału
                    <asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Podziel na centra kosztowe" Visible="false"/>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <InsertItemTemplate>
        <tr id="trSumI" runat="server" class="it" visible="true">
            <td class="col1">
                <asp:Label ID="lbMPKPrz" runat="server" Text="wg przypisania"/>
            </td>
            <td class="col2">
                <asp:Label ID="lbCzasZmPrz" runat="server" />
            </td>
            <td class="col2">
                <asp:Label ID="lbNadgDPrz" runat="server" />
            </td>
            <td class="col2">
                <asp:Label ID="lbNadgNPrz" runat="server" />
            </td>
            <td id="tdLastCol" runat="server" class="col2">
                <asp:Label ID="lbNocnePrz" runat="server" />
            </td>
            <td id="tdControl" runat="server" class="control">
            </td>
        </tr>
        <tr class="iit">
            <td class="col1">
                <asp:DropDownList ID="ddlMPK" runat="server" DataSourceID="SqlDataSource2" DataTextField="CC" DataValueField="Id" ></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi" 
                    ControlToValidate="ddlMPK"
                    CssClass="t4n error"
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvMPK" runat="server" Display="Dynamic"
                    ValidationGroup="vgi"
                    ControlToValidate="ddlMPK"
                    OnServerValidate="ddlMPK_ValidateInsert"
                    CssClass="t4n error"
                    ErrorMessage="Powtórzone CC">
                </asp:CustomValidator>
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teCzasZm" runat="server" Seconds='<%# Bind("CzasZm") %>' Format="hh:mm:ss" />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNadgD" runat="server" Seconds='<%# Bind("NadgodzinyDzien") %>' Format="hh:mm:ss" />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNadgN" runat="server" Seconds='<%# Bind("NadgodzinyNoc") %>' Format="hh:mm:ss" />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNocne" runat="server" Seconds='<%# Bind("Nocne") %>' Format="hh:mm:ss" />
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" ValidationGroup="vgi"/>
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="col1">
                <asp:HiddenField ID="hidMPK" runat="server" Value='<%# Eval("IdMPK") %>' />
                <asp:DropDownList ID="ddlMPK" DataSourceID="SqlDataSource2" runat="server" DataTextField="CC" DataValueField="Id"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vge" 
                    ControlToValidate="ddlMPK"
                    CssClass="t4n error"
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvMPK" runat="server" Display="Dynamic"
                    ValidationGroup="vge"
                    ControlToValidate="ddlMPK"
                    OnServerValidate="ddlMPK_ValidateEdit"
                    CssClass="t4n error"
                    ErrorMessage="Powtórzone CC">
                </asp:CustomValidator>
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teCzasZm" runat="server" Seconds='<%# Bind("CzasZm") %>' />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNadgD" runat="server" Seconds='<%# Bind("NadgodzinyDzien") %>' />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNadgN" runat="server" Seconds='<%# Bind("NadgodzinyNoc") %>'/>
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNocne" runat="server" Seconds='<%# Bind("Nocne") %>'/>
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vge" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1 tbEnterMPK hoverline narrow">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th id="Th2" runat="server">
                                Czas pracy / Centrum kosztowe<br />
                                <%--<span class="t4n">czas z dokładnością do 30 min.</span>--%>
                                </th>
                            <th id="Th1" runat="server">
                                Zmiana</th>
                            <th runat="server">
                                Nadg.<br />w dzień</th>
                            <th runat="server">
                                Nadg.<br />w nocy</th>
                            <th id="thLastCol" runat="server">
                                Praca w<br />nocy (<asp:Label ID="lbNocneOdDo" runat="server" Text="22-6"></asp:Label>)</th>
                            <th id="thControl" class="control" runat="server">&nbsp;</th>
                        </tr>
                        <tr runat="server" class="it total">
                            <td class="col1">Czas łączny <span class="t4">(hh:mm:ss)</span></td>
                            <td class="col2"><asp:Label ID="lbCzasZm" runat="server" ></asp:Label></td>
                            <td class="col2"><asp:Label ID="lbNadgD" runat="server" ></asp:Label></td>
                            <td class="col2"><asp:Label ID="lbNadgN" runat="server" ></asp:Label></td>
                            <td id="thLastCol1" class="col2" runat="server"><asp:Label ID="lbNocne" runat="server" ></asp:Label></td>
                            <td id="thControl1" class="control" runat="server">&nbsp;</td>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                        <tr id="trSum" runat="server" class="it" visible="false">
                            <td class="col1">
                                <asp:Label ID="lbMPKPrz" runat="server" Text="wg przypisania"/>
                            </td>
                            <td class="col2">
                                <asp:Label ID="lbCzasZmPrz" runat="server" />
                            </td>
                            <td class="col2">
                                <asp:Label ID="lbNadgDPrz" runat="server" />
                            </td>
                            <td class="col2">
                                <asp:Label ID="lbNadgNPrz" runat="server" />
                            </td>
                            <td id="tdLastCol" runat="server" class="col2 lastcol">
                                <asp:Label ID="lbNocnePrz" runat="server" />
                            </td>
                        </tr>                        
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>


<%--
<br />
--%>
<asp:Button ID="btWgPrzypisania" runat="server" CssClass="button" Text="Dodaj/uaktualnij wg przypisania" onclick="btWgPrzypisania_Click" Visible="false"/>
<asp:SqlDataSource ID="dsWgPrzypisania" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"    
    UpdateCommand="
declare @id int, 
	@szm int, @snd int, @snn int, @snoc int, @noclimit int,
	@szm0 int, @snd0 int, @snn0 int, @snoc0 int

select @szm = ISNULL(sum(P.CzasZm),0), @snd = ISNULL(sum(P.NadgodzinyDzien),0), @snn = ISNULL(sum(P.NadgodzinyNoc),0), @snoc = ISNULL(sum(P.Nocne),0)
from PodzialKosztow P
where P.IdPracownika = @IdPracownika and P.Data = @Data and IdMPK != 0

--set @szm0 = case when @czasZm is null then null when @czasZm &gt; @szm then @czasZm - @szm else 0 end 
set @szm0 = null
set @snd0 = case when @nadgD is null then null when @nadgD &gt; @snd then @nadgD - @snd else 0 end
set @snn0 = case when @nadgN is null then null when @nadgN &gt; @snn then @nadgN - @snn else 0 end
set @noclimit = case when @nocne &gt; @snoc then @nocne - @snoc else 0 end   -- limit
set @snoc0 = case when @snn0 &gt; @noclimit then @noclimit else @snn0 end    -- tylko z nn

select * from PodzialKosztow P where P.IdPracownika = @IdPracownika and P.Data = @Data 
select @czasZm as czasZm, @nadgD nadgD, @nadgN nadgN, @nocne nocne, 
	@szm as szm, @snd snd, @snn snn, @snoc snoc, @noclimit as noclimit, @szm0 as szm0, @snd0 snd0, @snn0 snn0, @snoc0 snoc0

select @id = Id from PodzialKosztow where IdPracownika = @IdPracownika and Data = @Data and IdMPK = 0

if @id is null 
	insert into PodzialKosztow values (@IdPracownika, @Data, 0, 0, @szm0, @snd0, @snn0, @snoc0, null)
	--select 'INSERT', @IdPracownika, @Data, 0, 0, @szm0, @snd0, @snn0, @snoc0, null
else
	update PodzialKosztow set /*CzasZm = null,*/ NadgodzinyDzien = @snd0, NadgodzinyNoc = @snn0, Nocne = @snoc0 where Id = @id
	--select 'UPDATE', IdPracownika, Data, IdPlanPracy, IdMPK, @snd0, @snn0, @snoc0, Uwagi, Id from PodzialKosztow where Id = @id
    ">
    <UpdateParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="datetime" />
        <asp:ControlParameter ControlID="hidCzasZm3" Name="czasZm" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidNadgD3" Name="nadgD" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidNadgN3" Name="nadgN" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidNocne3" Name="nocne" PropertyName="Value" Type="Int32" />
    </UpdateParameters>
</asp:SqlDataSource>

<%--
SELECT P.Typ, P.Id, P.IdMPK, 
    P.MPK, 
    P.CzasZm, P.NadgodzinyDzien, P.NadgodzinyNoc, P.Nocne,
    dbo.ToTimeHMM(P.CzasZm) as CzasZmT, 
    dbo.ToTimeHMM(P.NadgodzinyDzien) as NadgodzinyDzienT, 
    dbo.ToTimeHMM(P.NadgodzinyNoc) as NadgodzinyNocT, 
    dbo.ToTimeHMM(P.Nocne) as NocneT, 
    P.Uwagi,
	1 as Sort 
FROM VPodzialKosztow P
WHERE IdPracownika = @IdPracownika and Data = @Data 
order by Typ, Id
--%>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"     
    SelectCommand="
SELECT P.Id, P.IdMPK as IdMPK, 
    case when P.IdMPK = 0 then 'wg przypisania'
    else CC.cc + ' - ' + CC.Nazwa 
    end as MPK, 
    P.CzasZm, P.NadgodzinyDzien, P.NadgodzinyNoc, P.Nocne,
    dbo.ToTimeHMMSS(P.CzasZm) as CzasZmT, 
    dbo.ToTimeHMMSS(P.NadgodzinyDzien) as NadgodzinyDzienT, 
    dbo.ToTimeHMMSS(P.NadgodzinyNoc) as NadgodzinyNocT, 
    dbo.ToTimeHMMSS(P.Nocne) as NocneT, 
    P.Uwagi 
FROM PodzialKosztow P
left join CC on CC.Id = P.IdMPK
WHERE IdPracownika = @IdPracownika and Data = @Data 
ORDER BY P.Id
    "
    DeleteCommand="DELETE FROM [PodzialKosztow] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [PodzialKosztow] ([IdPlanPracy], [Data], [IdPracownika], [IdMPK], [CzasZm], [NadgodzinyDzien], [NadgodzinyNoc], [Nocne], [Uwagi]) VALUES (@IdPlanPracy, @Data, @IdPracownika, @IdMPK, @CzasZm, @NadgodzinyDzien, @NadgodzinyNoc, @Nocne, @Uwagi)" 
    UpdateCommand="UPDATE [PodzialKosztow] SET [IdMPK] = @IdMPK, [CzasZm] = @CzasZm, [NadgodzinyDzien] = @NadgodzinyDzien, [NadgodzinyNoc] = @NadgodzinyNoc, [Nocne] = @Nocne, [Uwagi] = @Uwagi WHERE [Id] = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="datetime" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="IdMPK" Type="Int32" />
        <asp:Parameter Name="CzasZm" Type="Int32" />
        <asp:Parameter Name="NadgodzinyDzien" Type="Int32" />
        <asp:Parameter Name="NadgodzinyNoc" Type="Int32" />
        <asp:Parameter Name="Nocne" Type="Int32" />
        <asp:Parameter Name="Uwagi" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="hidPlanId" Name="IdPlanPracy" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="datetime" />
        <asp:Parameter Name="IdMPK" Type="Int32" />
        <asp:Parameter Name="CzasZm" Type="Int32" />
        <asp:Parameter Name="NadgodzinyDzien" Type="Int32" />
        <asp:Parameter Name="NadgodzinyNoc" Type="Int32" />
        <asp:Parameter Name="Nocne" Type="Int32" />
        <asp:Parameter Name="Uwagi" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null as Id, 'wybierz ...' as CC, 0 as Sort
union all 
--xselect 0 as Id, dbo.fn_GetPracLastCC(@IdPracownika, @Data, 5, ',') as CC, 1 as Sort 
--select 0 as Id, 'wg przypisania' as CC, 1 as Sort 
--union all 
SELECT Id, cc + ' - ' + Nazwa as CC, 2 as Sort FROM [CC] 
WHERE @Data between AktywneOd and ISNULL(AktywneDo, @Data) ORDER BY Sort, cc
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

