<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Scorecards/Report.Master" AutoEventWireup="true" CodeBehind="RapRozdzielnik2.aspx.cs" Inherits="HRRcp.Scorecards.Reports.RapRozdzielnik2" %>

<%@ Register Src="~/Controls/Reports/cntReport2.ascx" TagPrefix="leet" TagName="Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">


</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">


    <div class="report_page RepCCUprawnienia">
        <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
	    <Triggers>
		    <asp:PostBackTrigger ControlID="btnExportXML" />
	    </Triggers>
        <ContentTemplate>
        <div class="filters">
            <div class="filter">
                <asp:Label ID="lblDataOd" runat="server" Text="Miesiąc rozliczeniowy: " />
                <asp:DropDownList ID="ddlYears" runat="server" DataValueField="Id" DataTextField="Name" DataSourceID="dsYears" OnSelectedIndexChanged="btnFilter_Click" />
                <asp:SqlDataSource ID="dsYears" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="select distinct Data as Id, left(Data, 7) as Name from scListaPlac order by Data desc" />
            </div>
            
     <%--       select distinct DataOd as Id, left(convert(varchar, DataOd, 20), 7) as Name from OkresyRozl order by Id desc--%>
            
            <asp:Button ID="btnFilter" runat="server" Text="Filtruj" OnClick="btnFilter_Click" CssClass="button100" />
            
            <%--<asp:Button ID="btnPDF" runat="server" Text="PDF" OnClick="DownloadPDF" style="float: right;" CssClass="button100" />--%>
            <asp:Button ID="btnExportXML" runat="server" Text="Export XML" OnClick="ExportXML" style="float: right;" CssClass="button100" />
            <asp:Button ID="btnCheckErrors" runat="server" OnClick="CheckErrors" Text="Sprawdź błędy" style="float: right;" CssClass="button150" />
            <asp:Button ID="Button1" runat="server" Text="Szczegóły" visible="false" style="float: right;" CssClass="button150" PostBackUrl="~/Raport.aspx?p=nNUhKpC%2bb4kieItB4g4v2A%3d%3d"/>
        </div>

        <leet:Report
                    ID="Rep1"
                    runat="server"
                    CssClass="none"
                    Title="Rozdzielnik kosztowy"
                    Title2="Miesiąc rozliczeniowy: 2015-10"
                    DivClass="report_page RepCCUprawnienia"
                    Pager="false"
                    Charts="None"
                    SQL="
--declare @pracId int = 2329
declare @date datetime = '@SQL1'--'20150801'

/*
select * into #override from dbo.SplitInt((select Parametr from Kody where Aktywny = 1 and Typ = 'SCROZDZWYNOVERRIDE'), ',')
declare @override nvarchar(16) = (select Nazwa from Kody where Typ = 'SCROZDZWYNOVERRIDE')
*/ --stara plomba

select k1.Nazwa, k2.items into #override from Kody k1
outer apply (select * from dbo.SplitInt((select Parametr from Kody k2 where k1.Nazwa = k2.Nazwa and k2.Aktywny = 1 and k2.Typ = 'SCROZDZWYNOVERRIDE'), ',')) k2
where k1.Aktywny = 1 and k1.Typ = 'SCROZDZWYNOVERRIDE'

--zebranie wspolczynnikow pierwszego stopnia

select
  oro.DataOd
, prz.IdPracownika
, SUM(ppoa.Nominal) as Nominal
, SUM(goa.Godziny) as Godziny
, SUM(ISNULL(d.CzasNieprod, 0)) as NP
, SUM(ISNULL(d.PracaInnyArkusz, 0)) as PracaInnyArkusz
, NULL as ARKM
, NULL as PGIO
, NULL as NIEPROD
into #aaa
from OkresyRozl oro
left join Przypisania prz on oro.DataOd &lt;= ISNULL(prz.Do, '20990909') and oro.DataDo &gt; prz.Od
--left join Absencja a on a.IdPracownika = prz.IdPracownika and oro.DataOd &lt;= a.DataDo and oro.DataDo &gt; a.DataOd --and dz.Data between a.DataOd and a.DataDo
--left join AbsencjaKody ak on a.Kod = ak.Kod
left join PlanPracy pp on pp.IdPracownika = prz.IdPracownika and pp.Data between oro.DataOd and oro.DataDo--and dz.Data = pp.Data
left join Zmiany Z on Z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
left join scDni d on d.Data = pp.Data and d.IdPracownika = prz.IdPracownika and d.IdTypuArkuszy = prz.IdCommodity
outer apply (select ISNULL(case when Z.Typ in (1,2,3) then 0 else case when Z.Od &gt; Z.Do then 24 else 0 end + DATEDIFF(HOUR, Z.Od, Z.Do) end, 0) as Nominal,
  ISNULL(case when Z.Typ in (1,2,3) then 0 else cast(pp.CzasZm as float) end, 0) / 3600 as vCzasZm,
  CONVERT(float, ISNULL(pp.n50, 0) + ISNULL(pp.n100, 0))/3600 as Nadgodziny) ppoa 
outer apply (select case when ppoa.Nominal &gt; ppoa.vCzasZm then ppoa.Nominal - ppoa.vCzasZm else 0 end as GodzNieob) pp2oa
outer apply (select ppoa.Nominal + ppoa.Nadgodziny - pp2oa.GodzNieob as Godziny) goa
where oro.DataOd = @date --and prz.IdPracownika = @pracId
group by oro.Dataod, prz.IdPracownika
order by oro.DataOd

--sumy

select
  DataOd
, IdPracownika
, oa.Suma
, (Godziny - PracaInnyArkusz - NP) / oa.Suma as ARKM
, PracaInnyArkusz / oa.Suma as PGIO
, NP / oa.Suma as NIEPROD
into #bbb from #aaa
outer apply (select case when Godziny = 0 then 1 else Godziny end as Suma) oa

--zebranie wspolczynnikow drugiego stopnia (mpk z czynnosci)

select
  oro.DataOd
, prz.IdPracownika
, c.IdCC as IdCC
, SUM(w.Ilosc) as Ilosc
, SUM(ISNULL(w.Ilosc, 0) * c.Czas) as Czas
, CAST(NULL as float) as Suma
, oa.M as M
into #ccc
from OkresyRozl oro
left join scWartosci w on w.Data between oro.DataOd and oro.DataDo
left join scCzynnosci c on c.Id = w.IdCzynnosci
left join Przypisania prz on w.Data between prz.Od and ISNULL(prz.Do, '20990909') and (w.IdPracownika = 0 - prz.IdCommodity or prz.IdPracownika = w.IdPracownika)
outer apply (select case when w.IdTypuArkuszy = prz.IdCommodity then 1 else 2 end as M) oa
where oro.Status = 1 and w.Ilosc != 0 and oro.DataOd = @date --and prz.IdPracownika = @pracId
group by DataOd, w.IdPracownika, prz.IdPracownika, oa.M, c.IdCC--, s.Wsp
order by DataOd

--zebranie wspolczynnikow drugiego stopnia (mpk macierzyste)

select
  oro.DataOd
, prz.IdPracownika
, swp.IdCC
, SUM(ISNULL(swp.Wsp, 0)) as Wsp
, CAST(NULL as float) as Suma
into #www
from OkresyRozl oro
left join Przypisania prz on oro.DataOd &lt;= ISNULL(prz.Do, '20990909') and oro.DataDo &gt; prz.Od
left join PlanPracy pp on pp.IdPracownika = prz.IdPracownika and pp.Data between oro.DataOd and oro.DataDo--and dz.Data = pp.Data
left join SplityWspP swp on swp.IdPrzypisania = prz.Id
where oro.DataOd = @date --and prz.IdPracownika = @pracId
group by oro.DataOd, prz.IdPracownika, swp.IdCC

--sumy

update #ccc set Suma = B.Sum
--select *
from #ccc A
outer apply (
select SUM(Czas) as Sum
from #ccc
where DataOd = A.DataOd and M = A.M and IdPracownika = A.IdPracownika) B

--select * from #ccc where IdPracownika = 2448

--sumy

update #www set Suma = B.Sum
--select *
from #www A
outer apply (
select SUM(Wsp) as Sum
from #www
where DataOd = A.DataOd and IdPracownika = A.IdPracownika) B

--join mpk z czynnosci do dni

select
  b.DataOd
, b.IdPracownika
, b.ARKM
, b.PGIO
, b.NIEPROD
, c.IdCC
, c.Ilosc
, c.Czas
, c.Czas/c.Suma as Wsp0
, c.Czas/c.Suma * case when c.M = 1 then
  ARKM else case when c.M = 2 then
  PGIO else -1
  end
  end as Wsp
, c.M
into #fff
from #bbb b
left join #ccc c on b.DataOd = c.DataOd and b.IdPracownika = c.IdPracownika
where c.Suma != 0

--join mpk macierzystych do dni

--select
--  b.DataOd
--, b.IdPracownika
--, b.ARKM
--, b.PGIO
--, b.NIEPROD
--, c.IdCC
--, c.Ilosc
--, c.Czas
--, c.Czas/c.Suma as Wsp0
--, c.Czas/c.Suma * case when c.M = 1 then
--  ARKM else case when c.M = 2 then
--  PGIO else -1
--  end
--  end as Wsp
--, c.M
----into #fff
--from #bbb b
--left join #ccc c on b.DataOd = c.DataOd and b.IdPracownika = c.IdPracownika
--where c.Suma != 0 and b.IdPracownika = 2448

select
  b.DataOd
, b.IdPracownika
, b.ARKM
, b.PGIO
, b.NIEPROD
, w.IdCC as IdCC
, NULL as Ilosc
, NULL as Czas
, w.Wsp/w.Suma as Wsp0
, w.Wsp/w.Suma * NIEPROD as Wsp
, 0 as M
into #nnn
from #bbb b
left join #www w
on b.DataOd = w.DataOd and b.IdPracownika = w.IdPracownika
where w.Suma != 0

--asd

insert into #nnn select DataOd, asd.IdPracownika, ARKM, PGIO, NIEPROD, wsp.IdCC, null, null, wsp.Wsp, (1 - ROUND(SUM(asd.Wsp), 6)) * wsp.Wsp, 0 from (select * from #fff union select * from #nnn) asd
outer apply (select top 1 * from Przypisania where Status = 1 and IdPracownika = asd.IdPracownika order by Od desc) prz
left join SplityWspP wsp on wsp.IdPrzypisania = prz.Id
group by DataOd, asd.IdPracownika, ARKM, PGIO, NIEPROD, wsp.IdCC, wsp.Wsp having ROUND(SUM(asd.Wsp), 6) not in (0, 1)

--/asd

--select * from #fff where IdPracownika = 2448

--select * from (select *
--from #fff where Wsp != 0 union 
--select *
--from #nnn where Wsp != 0) aoe where IdPracownika = 2448

--select
--IdPracownika, SUM(Wsp0), M
---- CC.cc
----, aoe.Ilosc
----, aoe.Czas 
----, convert(varchar, round(aoe.Wsp0 * 100, 2)) + '%' as Wsp0
----, convert(varchar, round(aoe.Wsp * 100, 2)) + '%' as Wsp
--from
--(select *
--from #fff where Wsp != 0 union 
--select *
--from #nnn where Wsp != 0) aoe
--group by DataOd, IdPracownika, M
----left join CC on CC.Id = aoe.IdCC

----
--select
-- CC.cc
--, aoe.Ilosc
--, aoe.Czas
--, Wsp0
--, Wsp
--from #fff aoe
--left join CC on aoe.IdCC = CC.Id
--where Wsp != 0 and M = 1

--select
-- CC.cc
--, aoe.Ilosc
--, aoe.Czas
--, Wsp0
--, Wsp
--from #fff aoe
--left join CC on aoe.IdCC = CC.Id
--where Wsp != 0 and M = 2

--select
-- CC.cc
--, aoe.Ilosc
--, aoe.Czas
--, Wsp0
--, Wsp
--from
--#nnn aoe
--left join CC on aoe.IdCC = CC.Id
--where Wsp != 0
--

select
  aoe.IdPracownika
, CC.Id as IdCC
, CONVERT(varchar(10), dbo.eom(@date), 20) as Date
, /*dbo.eom(@date)*/CONVERT(varchar(10), GETDATE(), 20) as [Reporting Date]
, k.Parametr as [Exact GL account code]
, /*CC.cc*/lp.CC as [Cost Center]
--, lp.CC as cc2
, k.Nazwa as [Description]
, k2.Parametr as [Creditor]
, CC.Nazwa as Project
--, p.Nazwisko + ' ' + p.Imie + isnull(' (' + p.KadryId + ')', '') as Pracownik
, CAST(NULL as float) as Debit
, CONVERT(float, lp.Brutto) as Credit
, 'PLN' as Currency
, 'KUP' as [Costunit code]
, 0 as Type
, SUM(case when CC.cc != lp.CC then aoe.Czas else 0 end) Czas
, SUM(case when CC.cc = lp.CC then aoe.Czas else 0 end) Czas2
, CAST('' as nvarchar(16)) CCM
into #uuu
from
(select *
from #fff where Wsp != 0 union 
select *
from #nnn where Wsp != 0) aoe
left join Pracownicy p on p.Id = aoe.IdPracownika
left join scListaPlac lp on lp.Data = aoe.DataOd and lp.KadryId = p.KadryId
left join CC on CC.cc = lp.CC
left join Kody k on k.Typ = 'SCROZDZWYN'
left join Kody k2 on k2.Typ = 'SCROZDZCRED'
where lp.CC not in (select c4.cc from (select * from #fff where Wsp != 0 union select * from #nnn where Wsp != 0) asd left join CC c4 on c4.Id = asd.IdCC where aoe.IdPracownika = asd.IdPracownika)
group by aoe.IdPracownika, lp.Brutto, lp.CC, CC.Id, CC.Nazwa, k.Parametr, k.Nazwa, k2.Parametr
--union all

insert into #uuu select
  aoe.IdPracownika
, aoe.IdCC
, CONVERT(varchar(10), dbo.eom(@date), 20) as Date
, /*dbo.eom(@date)*/CONVERT(varchar(10), GETDATE(), 20) as [Reporting Date]
, k.Parametr as [Exact GL account code]
, CC.cc as [Cost Center]
--, lp.CC as cc2
, k.Nazwa as [Description]
, k2.Parametr as [Creditor]
, CC.Nazwa as Project
--, p.Nazwisko + ' ' + p.Imie + isnull(' (' + p.KadryId + ')', '') as Pracownik
, convert(float, case when CC.cc != lp.CC then SUM(aoe.Wsp) * lp.Brutto else NULL end) as Debit
, case when CC.cc = lp.CC then lp.Brutto - SUM(aoe.Wsp) * lp.Brutto else NULL end as Credit
, 'PLN' as Currency
, 'KUP' as [Costunit code]
, 0 as Type
, SUM(case when CC.cc != lp.CC then aoe.Czas else 0 end) Czas
, SUM(case when CC.cc = lp.CC then aoe.Czas else 0 end) Czas2
, lp.CC
from
(select *
from #fff where Wsp != 0 union 
select *
from #nnn where Wsp != 0) aoe
left join CC on CC.Id = aoe.IdCC
left join Pracownicy p on p.Id = aoe.IdPracownika
left join scListaPlac lp on lp.Data = aoe.DataOd and lp.KadryId = p.KadryId
left join Kody k on k.Typ = 'SCROZDZWYN'
left join Kody k2 on k2.Typ = 'SCROZDZCRED'
group by aoe.IdPracownika, lp.Brutto, lp.CC, aoe.IdCC, CC.cc, CC.Nazwa, k.Parametr, k.Nazwa, k2.Parametr
--union all

insert into #uuu select
  aoe.IdPracownika
, CC.Id as IdCC
, CONVERT(varchar(10), dbo.eom(@date), 20) as Date
, /*dbo.eom(@date)*/CONVERT(varchar(10), GETDATE(), 20) as [Reporting Date]
, k.Parametr as [Exact GL account code]
, /*CC.cc*/lp.CC as [Cost Center]
--, lp.CC as cc2
, k.Nazwa as [Description]
, k2.Parametr as [Creditor]
, CC.Nazwa as Project
--, p.Nazwisko + ' ' + p.Imie + isnull(' (' + p.KadryId + ')', '') as Pracownik
, CAST(NULL as float) as Debit
, CONVERT(float, lp.ZUS) as Credit
, 'PLN' as Currency
, 'KUP' as [Costunit code]
, 1 as Type
, SUM(case when CC.cc != lp.CC then aoe.Czas else 0 end) Czas
, SUM(case when CC.cc = lp.CC then aoe.Czas else 0 end) Czas2
, ''
from
(select *
from #fff where Wsp != 0 union 
select *
from #nnn where Wsp != 0) aoe
left join Pracownicy p on p.Id = aoe.IdPracownika
left join scListaPlac lp on lp.Data = aoe.DataOd and lp.KadryId = p.KadryId
left join CC on CC.cc = lp.CC
left join Kody k on k.Typ = 'SCROZDZNARZ'
left join Kody k2 on k2.Typ = 'SCROZDZCRED'
where lp.CC not in (select c4.cc from (select * from #fff where Wsp != 0 union select * from #nnn where Wsp != 0) asd left join CC c4 on c4.Id = asd.IdCC where aoe.IdPracownika = asd.IdPracownika)
group by aoe.IdPracownika, lp.ZUS, lp.CC, CC.Id, CC.Nazwa, k.Parametr, k.Nazwa, k2.Parametr
--union all

insert into #uuu select
  aoe.IdPracownika
, aoe.IdCC
, CONVERT(varchar(10), dbo.eom(@date), 20) as Date
, /*dbo.eom(@date)*/CONVERT(varchar(10), GETDATE(), 20) as [Reporting Date]
, k.Parametr as [Exact GL account code]
, CC.cc as [Cost Center]
--, lp.CC as cc2
, k.Nazwa as [Description]
, k2.Parametr as [Creditor]
, CC.Nazwa as Project
--, p.Nazwisko + ' ' + p.Imie + isnull(' (' + p.KadryId + ')', '') as Pracownik
, case when CC.cc != lp.CC then SUM(aoe.Wsp) * lp.ZUS else NULL end as Debit
, case when CC.cc = lp.CC then lp.ZUS - SUM(aoe.Wsp) * lp.ZUS else NULL end as Credit
, 'PLN' as Currency
, 'KUP' as [Costunit code]
, 1 as Type
, SUM(case when CC.cc != lp.CC then aoe.Czas else 0 end) Czas
, SUM(case when CC.cc = lp.CC then aoe.Czas else 0 end) Czas2
, lp.CC
from
(select *
from #fff where Wsp != 0 union 
select *
from #nnn where Wsp != 0) aoe
left join CC on CC.Id = aoe.IdCC
left join Pracownicy p on p.Id = aoe.IdPracownika
left join scListaPlac lp on lp.Data = aoe.DataOd and lp.KadryId = p.KadryId
left join Kody k on k.Typ = 'SCROZDZNARZ'
left join Kody k2 on k2.Typ = 'SCROZDZCRED'
group by aoe.IdPracownika, lp.ZUS, lp.CC, aoe.IdCC, CC.cc, CC.Nazwa, k.Parametr, k.Nazwa, k2.Parametr

--select * from 
--(select IdPracownika, ISNULL(SUM(Debit), 0) as a , ISNULL(SUM(Credit), 0) as b, case when ROUND(ISNULL(SUM(Debit), 0), 3) = ROUND(ISNULL(SUM(Credit), 0), 3) then 1 else 0 end as asd, Description from #uuu u group by Description, IdPracownika) asd
--where asd.asd = 0

--select * from #uuu where IdPracownika = 2175

--[Date:C], [Reporting Date], [Exact GL account code], [Cost Center], [Description], [Creditor], [Project], [Debit:S], [Credit:S], [Currency], [Costunit code]

/* update #uuu set [Exact GL account code] = @override where [Cost Center] in (select * from #override) and Type = 0 */ --stara plomba

update #uuu set [Exact GL account code] = o.Nazwa/*select u.[Exact GL account code], o.Nazwa*/ from #uuu u inner join #override o on u.Type = 0 and u.[Cost Center] = o.items

/*
select distinct
  u.IdCC
, u.Type
, aoe.Czas2
into #uuu2
from #uuu u
outer apply
(
	select SUM(/*case when u.CCM = '' then ISNULL(Czas2, 0) else*/ ISNULL(Czas, 0) /*end*/) Czas2
	from
	(
		select u3.[Cost Center], u3.Type, u3.Czas from #uuu u3 where ISNULL(u.Debit, 0) != 0
		/*union all
		select u3.[Cost Center], u3.Type, u3.Czas2 from #uuu u3 where /*ISNULL(u.Credit, 0) != 0 /*????*/ */ CCM = ''*/
	) u2
	where (u2.[Cost Center] = u.CCM /*or (u.CCM = '' and u.[Cost Center] = u2.[Cost Center])*/) and u2.Type = u.Type
) aoe
--where ISNULL(u.Debit, 0) != 0

select * from #uuu where [Cost Center] = '6000'
select * from #uuu where [CCM] = ''

select * from #uuu2
*/

/*
select * from #uuu

select a.IdCC, a.Type, SUM(b.Czas) from #uuu a
left join #uuu b on a.CCM = b.[Cost Center]
where a.Debit is not null
group by a.IdCC, a.Type
order by a.IdCC
--to query to porazka
*/




/* OSTATECZNOSC */
select
  aoe.IdPracownika
, aoe.IdCC
, CONVERT(varchar(10), dbo.eom(@date), 20) as Date
, CC.cc as [Cost Center]
, CC.Nazwa as Project
, case when CC.cc != lp.CC then SUM(aoe.Wsp) else 0 end Wsp
, lp.Brutto as Hajs1
, lp.ZUS as Hajs2
, lp.CC as CCM
, convert(float, case when CC.cc != lp.CC then SUM(aoe.Wsp) * lp.Brutto else NULL end) as Debit
, convert(float, case when CC.cc != lp.CC then SUM(aoe.Wsp) * lp.ZUS else NULL end) as Debit2
, SUM(aoe.Czas) Czas
into #zzz
from
(select * from #fff where Wsp != 0 union select * from #nnn where Wsp != 0) aoe
left join CC on CC.Id = aoe.IdCC
left join Pracownicy p on p.Id = aoe.IdPracownika
left join scListaPlac lp on lp.Data = aoe.DataOd and lp.KadryId = p.KadryId
group by aoe.IdPracownika, lp.Brutto, lp.ZUS, lp.CC, aoe.IdCC, CC.cc, CC.Nazwa

selecT CC.Id, SUM(Czas) Czas700 into #horror from #zzz
left join CC on CC.cc = CCM
where Debit is not null --and CC.Id = 67
group by CC.Id




/*
select IdCC, Type, SUM(Czas2) Czas into #uuu3 from #uuu2 group by IdCC, Type

select * from #uuu3
*/

select IdCC [pid:-]
, @date [TrueDate:-]
, Type [Type:-]
, Date [Date:C], [Reporting Date], [Exact GL account code], [Cost Center] [Cost Center], Description, Creditor, Project, SUM(Czas) [Czas debetowy:NS;c3], ISNULL(z.Czas700, 0) [Czas kredytowy:NS;c2], SUM(ISNULL(Debit, 0)) as [Debit:NN2S;c3|../Raport 38 @pid @TrueDate @Type], SUM(ISNULL(Credit, 0)) as [Credit:NN2S;c2|../Raport 39 @pid @TrueDate @Type], [Currency], [Costunit code]
from #uuu u
left join #horror z on u.IdCC = z.Id
where Debit != 0 or Credit != 0
group by Date, [Reporting Date], [Exact GL account code], [Cost Center], Description, Creditor, Project, Currency, [Costunit code], IdCC, Type, z.Czas700
order by [Exact GL account code], [Cost Center]

--select Description, SUM(Debit), SUM(Credit), SUM(Debit) - SUM(Credit) from #uuu group by Description

drop table #aaa
drop table #bbb
drop table #ccc
drop table #fff
drop table #www
drop table #nnn

drop table #uuu
drop table #zzz
drop table #horror
--drop table #uuu2
--drop table #uuu3

drop table #override

"

 />
 
 </ContentTemplate>
 </asp:UpdatePanel>
 
</div>
 <asp:SqlDataSource id="dsXML" runat="server" SelectCommand=
 "
 --declare @pracId int = 2329
declare @date datetime = '{0}'--'20150801'

/*
select * into #override from dbo.SplitInt((select Parametr from Kody where Aktywny = 1 and Typ = 'SCROZDZWYNOVERRIDE'), ',')
declare @override nvarchar(16) = (select Nazwa from Kody where Typ = 'SCROZDZWYNOVERRIDE')
*/ --stara plomba

select k1.Nazwa, k2.items into #override from Kody k1
outer apply (select * from dbo.SplitInt((select Parametr from Kody k2 where k1.Nazwa = k2.Nazwa and k2.Aktywny = 1 and k2.Typ = 'SCROZDZWYNOVERRIDE'), ',')) k2
where k1.Aktywny = 1 and k1.Typ = 'SCROZDZWYNOVERRIDE'

--zebranie wspolczynnikow pierwszego stopnia

select
  oro.DataOd
, prz.IdPracownika
, SUM(ppoa.Nominal) as Nominal
, SUM(goa.Godziny) as Godziny
, SUM(ISNULL(d.CzasNieprod, 0)) as NP
, SUM(ISNULL(d.PracaInnyArkusz, 0)) as PracaInnyArkusz
, NULL as ARKM
, NULL as PGIO
, NULL as NIEPROD
into #aaa
from OkresyRozl oro
left join Przypisania prz on oro.DataOd &lt;= ISNULL(prz.Do, '20990909') and oro.DataDo &gt; prz.Od
--left join Absencja a on a.IdPracownika = prz.IdPracownika and oro.DataOd &lt;= a.DataDo and oro.DataDo &gt; a.DataOd --and dz.Data between a.DataOd and a.DataDo
--left join AbsencjaKody ak on a.Kod = ak.Kod
left join PlanPracy pp on pp.IdPracownika = prz.IdPracownika and pp.Data between oro.DataOd and oro.DataDo--and dz.Data = pp.Data
left join Zmiany Z on Z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
left join scDni d on d.Data = pp.Data and d.IdPracownika = prz.IdPracownika and d.IdTypuArkuszy = prz.IdCommodity
outer apply (select ISNULL(case when Z.Typ in (1,2,3) then 0 else case when Z.Od &gt; Z.Do then 24 else 0 end + DATEDIFF(HOUR, Z.Od, Z.Do) end, 0) as Nominal,
  ISNULL(case when Z.Typ in (1,2,3) then 0 else cast(pp.CzasZm as float) end, 0) / 3600 as vCzasZm,
  CONVERT(float, ISNULL(pp.n50, 0) + ISNULL(pp.n100, 0))/3600 as Nadgodziny) ppoa 
outer apply (select case when ppoa.Nominal &gt; ppoa.vCzasZm then ppoa.Nominal - ppoa.vCzasZm else 0 end as GodzNieob) pp2oa
outer apply (select ppoa.Nominal + ppoa.Nadgodziny - pp2oa.GodzNieob as Godziny) goa
where oro.DataOd = @date --and prz.IdPracownika = @pracId
group by oro.Dataod, prz.IdPracownika
order by oro.DataOd

--sumy

select
  DataOd
, IdPracownika
, oa.Suma
, (Godziny - PracaInnyArkusz - NP) / oa.Suma as ARKM
, PracaInnyArkusz / oa.Suma as PGIO
, NP / oa.Suma as NIEPROD
into #bbb from #aaa
outer apply (select case when Godziny = 0 then 1 else Godziny end as Suma) oa

--zebranie wspolczynnikow drugiego stopnia (mpk z czynnosci)

select
  oro.DataOd
, prz.IdPracownika
, c.IdCC as IdCC
, SUM(w.Ilosc) as Ilosc
, SUM(ISNULL(w.Ilosc, 0) * c.Czas) as Czas
, CAST(NULL as float) as Suma
, oa.M as M
into #ccc
from OkresyRozl oro
left join scWartosci w on w.Data between oro.DataOd and oro.DataDo
left join scCzynnosci c on c.Id = w.IdCzynnosci
left join Przypisania prz on w.Data between prz.Od and ISNULL(prz.Do, '20990909') and (w.IdPracownika = 0 - prz.IdCommodity or prz.IdPracownika = w.IdPracownika)
outer apply (select case when w.IdTypuArkuszy = prz.IdCommodity then 1 else 2 end as M) oa
where oro.Status = 1 and w.Ilosc != 0 and oro.DataOd = @date --and prz.IdPracownika = @pracId
group by DataOd, w.IdPracownika, prz.IdPracownika, oa.M, c.IdCC--, s.Wsp
order by DataOd

--zebranie wspolczynnikow drugiego stopnia (mpk macierzyste)

select
  oro.DataOd
, prz.IdPracownika
, swp.IdCC
, SUM(ISNULL(swp.Wsp, 0)) as Wsp
, CAST(NULL as float) as Suma
into #www
from OkresyRozl oro
left join Przypisania prz on oro.DataOd &lt;= ISNULL(prz.Do, '20990909') and oro.DataDo &gt; prz.Od
left join PlanPracy pp on pp.IdPracownika = prz.IdPracownika and pp.Data between oro.DataOd and oro.DataDo--and dz.Data = pp.Data
left join SplityWspP swp on swp.IdPrzypisania = prz.Id
where oro.DataOd = @date --and prz.IdPracownika = @pracId
group by oro.DataOd, prz.IdPracownika, swp.IdCC

--sumy

update #ccc set Suma = B.Sum
--select *
from #ccc A
outer apply (
select SUM(Czas) as Sum
from #ccc
where DataOd = A.DataOd and M = A.M and IdPracownika = A.IdPracownika) B

--select * from #ccc where IdPracownika = 2448

--sumy

update #www set Suma = B.Sum
--select *
from #www A
outer apply (
select SUM(Wsp) as Sum
from #www
where DataOd = A.DataOd and IdPracownika = A.IdPracownika) B

--join mpk z czynnosci do dni

select
  b.DataOd
, b.IdPracownika
, b.ARKM
, b.PGIO
, b.NIEPROD
, c.IdCC
, c.Ilosc
, c.Czas
, c.Czas/c.Suma as Wsp0
, c.Czas/c.Suma * case when c.M = 1 then
  ARKM else case when c.M = 2 then
  PGIO else -1
  end
  end as Wsp
, c.M
into #fff
from #bbb b
left join #ccc c on b.DataOd = c.DataOd and b.IdPracownika = c.IdPracownika
where c.Suma != 0

--join mpk macierzystych do dni

--select
--  b.DataOd
--, b.IdPracownika
--, b.ARKM
--, b.PGIO
--, b.NIEPROD
--, c.IdCC
--, c.Ilosc
--, c.Czas
--, c.Czas/c.Suma as Wsp0
--, c.Czas/c.Suma * case when c.M = 1 then
--  ARKM else case when c.M = 2 then
--  PGIO else -1
--  end
--  end as Wsp
--, c.M
----into #fff
--from #bbb b
--left join #ccc c on b.DataOd = c.DataOd and b.IdPracownika = c.IdPracownika
--where c.Suma != 0 and b.IdPracownika = 2448

select
  b.DataOd
, b.IdPracownika
, b.ARKM
, b.PGIO
, b.NIEPROD
, w.IdCC as IdCC
, NULL as Ilosc
, NULL as Czas
, w.Wsp/w.Suma as Wsp0
, w.Wsp/w.Suma * NIEPROD as Wsp
, 0 as M
into #nnn
from #bbb b
left join #www w
on b.DataOd = w.DataOd and b.IdPracownika = w.IdPracownika
where w.Suma != 0

--asd

insert into #nnn select DataOd, asd.IdPracownika, ARKM, PGIO, NIEPROD, wsp.IdCC, null, null, wsp.Wsp, (1 - ROUND(SUM(asd.Wsp), 6)) * wsp.Wsp, 0 from (select * from #fff union select * from #nnn) asd
outer apply (select top 1 * from Przypisania where Status = 1 and IdPracownika = asd.IdPracownika order by Od desc) prz
left join SplityWspP wsp on wsp.IdPrzypisania = prz.Id
group by DataOd, asd.IdPracownika, ARKM, PGIO, NIEPROD, wsp.IdCC, wsp.Wsp having ROUND(SUM(asd.Wsp), 6) not in (0, 1)

--/asd

--select * from #fff where IdPracownika = 2448

--select * from (select *
--from #fff where Wsp != 0 union 
--select *
--from #nnn where Wsp != 0) aoe where IdPracownika = 2448

--select
--IdPracownika, SUM(Wsp0), M
---- CC.cc
----, aoe.Ilosc
----, aoe.Czas 
----, convert(varchar, round(aoe.Wsp0 * 100, 2)) + '%' as Wsp0
----, convert(varchar, round(aoe.Wsp * 100, 2)) + '%' as Wsp
--from
--(select *
--from #fff where Wsp != 0 union 
--select *
--from #nnn where Wsp != 0) aoe
--group by DataOd, IdPracownika, M
----left join CC on CC.Id = aoe.IdCC

----
--select
-- CC.cc
--, aoe.Ilosc
--, aoe.Czas
--, Wsp0
--, Wsp
--from #fff aoe
--left join CC on aoe.IdCC = CC.Id
--where Wsp != 0 and M = 1

--select
-- CC.cc
--, aoe.Ilosc
--, aoe.Czas
--, Wsp0
--, Wsp
--from #fff aoe
--left join CC on aoe.IdCC = CC.Id
--where Wsp != 0 and M = 2

--select
-- CC.cc
--, aoe.Ilosc
--, aoe.Czas
--, Wsp0
--, Wsp
--from
--#nnn aoe
--left join CC on aoe.IdCC = CC.Id
--where Wsp != 0
--

select
  aoe.IdPracownika
, CC.Id as IdCC
, CONVERT(varchar(10), dbo.eom(@date), 20) as Date
, /*dbo.eom(@date)*/CONVERT(varchar(10), GETDATE(), 20) as [Reporting Date]
, k.Parametr as [Exact GL account code]
, /*CC.cc*/lp.CC as [Cost Center]
--, lp.CC as cc2
, k.Nazwa as [Description]
, k2.Parametr as [Creditor]
, CC.Nazwa as Project
--, p.Nazwisko + ' ' + p.Imie + isnull(' (' + p.KadryId + ')', '') as Pracownik
, CAST(NULL as float) as Debit
, CONVERT(float, lp.Brutto) as Credit
, 'PLN' as Currency
, 'KUP' as [Costunit code]
, 0 as Type
into #uuu
from
(select *
from #fff where Wsp != 0 union 
select *
from #nnn where Wsp != 0) aoe
left join Pracownicy p on p.Id = aoe.IdPracownika
left join scListaPlac lp on lp.Data = aoe.DataOd and lp.KadryId = p.KadryId
left join CC on CC.cc = lp.CC
left join Kody k on k.Typ = 'SCROZDZWYN'
left join Kody k2 on k2.Typ = 'SCROZDZCRED'
where lp.CC not in (select c4.cc from (select * from #fff where Wsp != 0 union select * from #nnn where Wsp != 0) asd left join CC c4 on c4.Id = asd.IdCC where aoe.IdPracownika = asd.IdPracownika)
group by aoe.IdPracownika, lp.Brutto, lp.CC, CC.Id, CC.Nazwa, k.Parametr, k.Nazwa, k2.Parametr
--union all

insert into #uuu select
  aoe.IdPracownika
, aoe.IdCC
, CONVERT(varchar(10), dbo.eom(@date), 20) as Date
, /*dbo.eom(@date)*/CONVERT(varchar(10), GETDATE(), 20) as [Reporting Date]
, k.Parametr as [Exact GL account code]
, CC.cc as [Cost Center]
--, lp.CC as cc2
, k.Nazwa as [Description]
, k2.Parametr as [Creditor]
, CC.Nazwa as Project
--, p.Nazwisko + ' ' + p.Imie + isnull(' (' + p.KadryId + ')', '') as Pracownik
, convert(float, case when CC.cc != lp.CC then SUM(aoe.Wsp) * lp.Brutto else NULL end) as Debit
, case when CC.cc = lp.CC then lp.Brutto - SUM(aoe.Wsp) * lp.Brutto else NULL end as Credit
, 'PLN' as Currency
, 'KUP' as [Costunit code]
, 0 as Type
from
(select *
from #fff where Wsp != 0 union 
select *
from #nnn where Wsp != 0) aoe
left join CC on CC.Id = aoe.IdCC
left join Pracownicy p on p.Id = aoe.IdPracownika
left join scListaPlac lp on lp.Data = aoe.DataOd and lp.KadryId = p.KadryId
left join Kody k on k.Typ = 'SCROZDZWYN'
left join Kody k2 on k2.Typ = 'SCROZDZCRED'
group by aoe.IdPracownika, lp.Brutto, lp.CC, aoe.IdCC, CC.cc, CC.Nazwa, k.Parametr, k.Nazwa, k2.Parametr
--union all

insert into #uuu select
  aoe.IdPracownika
, CC.Id as IdCC
, CONVERT(varchar(10), dbo.eom(@date), 20) as Date
, /*dbo.eom(@date)*/CONVERT(varchar(10), GETDATE(), 20) as [Reporting Date]
, k.Parametr as [Exact GL account code]
, /*CC.cc*/lp.CC as [Cost Center]
--, lp.CC as cc2
, k.Nazwa as [Description]
, k2.Parametr as [Creditor]
, CC.Nazwa as Project
--, p.Nazwisko + ' ' + p.Imie + isnull(' (' + p.KadryId + ')', '') as Pracownik
, CAST(NULL as float) as Debit
, CONVERT(float, lp.ZUS) as Credit
, 'PLN' as Currency
, 'KUP' as [Costunit code]
, 1 as Type
from
(select *
from #fff where Wsp != 0 union 
select *
from #nnn where Wsp != 0) aoe
left join Pracownicy p on p.Id = aoe.IdPracownika
left join scListaPlac lp on lp.Data = aoe.DataOd and lp.KadryId = p.KadryId
left join CC on CC.cc = lp.CC
left join Kody k on k.Typ = 'SCROZDZNARZ'
left join Kody k2 on k2.Typ = 'SCROZDZCRED'
where lp.CC not in (select c4.cc from (select * from #fff where Wsp != 0 union select * from #nnn where Wsp != 0) asd left join CC c4 on c4.Id = asd.IdCC where aoe.IdPracownika = asd.IdPracownika)
group by aoe.IdPracownika, lp.ZUS, lp.CC, CC.Id, CC.Nazwa, k.Parametr, k.Nazwa, k2.Parametr
--union all

insert into #uuu select
  aoe.IdPracownika
, aoe.IdCC
, CONVERT(varchar(10), dbo.eom(@date), 20) as Date
, /*dbo.eom(@date)*/CONVERT(varchar(10), GETDATE(), 20) as [Reporting Date]
, k.Parametr as [Exact GL account code]
, CC.cc as [Cost Center]
--, lp.CC as cc2
, k.Nazwa as [Description]
, k2.Parametr as [Creditor]
, CC.Nazwa as Project
--, p.Nazwisko + ' ' + p.Imie + isnull(' (' + p.KadryId + ')', '') as Pracownik
, case when CC.cc != lp.CC then SUM(aoe.Wsp) * lp.ZUS else NULL end as Debit
, case when CC.cc = lp.CC then lp.ZUS - SUM(aoe.Wsp) * lp.ZUS else NULL end as Credit
, 'PLN' as Currency
, 'KUP' as [Costunit code]
, 1 as Type
from
(select *
from #fff where Wsp != 0 union 
select *
from #nnn where Wsp != 0) aoe
left join CC on CC.Id = aoe.IdCC
left join Pracownicy p on p.Id = aoe.IdPracownika
left join scListaPlac lp on lp.Data = aoe.DataOd and lp.KadryId = p.KadryId
left join Kody k on k.Typ = 'SCROZDZNARZ'
left join Kody k2 on k2.Typ = 'SCROZDZCRED'
group by aoe.IdPracownika, lp.ZUS, lp.CC, aoe.IdCC, CC.cc, CC.Nazwa, k.Parametr, k.Nazwa, k2.Parametr

--select * from 
--(select IdPracownika, ISNULL(SUM(Debit), 0) as a , ISNULL(SUM(Credit), 0) as b, case when ROUND(ISNULL(SUM(Debit), 0), 3) = ROUND(ISNULL(SUM(Credit), 0), 3) then 1 else 0 end as asd, Description from #uuu u group by Description, IdPracownika) asd
--where asd.asd = 0

--select * from #uuu where IdPracownika = 2175

--[Date:C], [Reporting Date], [Exact GL account code], [Cost Center], [Description], [Creditor], [Project], [Debit:S], [Credit:S], [Currency], [Costunit code]

/* update #uuu set [Exact GL account code] = @override where [Cost Center] in (select * from #override) and Type = 0 */ --stara plomba

update #uuu set [Exact GL account code] = o.Nazwa/*select u.[Exact GL account code], o.Nazwa*/ from #uuu u inner join #override o on u.Type = 0 and u.[Cost Center] = o.items

select Date, [Reporting Date] as ReportingDate, [Exact GL account code] as ExactGLaccountcode, [Cost Center] as CostCenter, Description, Creditor, Project, SUM(ISNULL(Debit, 0)) as [Debit], SUM(ISNULL(Credit, 0)) as [Credit], [Currency], [Costunit code] as CostunitCode
from #uuu
where Debit != 0 or Credit != 0
group by Date, [Reporting Date], [Exact GL account code], [Cost Center], Description, Creditor, Project, Currency, [Costunit code], IdCC
order by [Exact GL account code], [Cost Center]

--select Description, SUM(Debit), SUM(Credit), SUM(Debit) - SUM(Credit) from #uuu group by Description

drop table #aaa
drop table #bbb
drop table #ccc
drop table #fff
drop table #www
drop table #nnn

drop table #uuu

drop table #override
 "
 />

</asp:Content>
