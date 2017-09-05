<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SQL.ascx.cs" Inherits="HRRcp.Controls.PodzialLudzi.SQL" %>

<asp:HiddenField ID="hidVer" runat="server" Value="1.0"/>
 
<asp:SqlDataSource ID="dsGetData" runat="server"
    SelectCommand="
select top 1 
O.DataOd as Od, O.DataDo as Do, NaDzien,
    
--case O.StatusPL when 1 then 1  --testy bez FTE
case O.StatusPL when 1 then 3  --tylko jak otwarty
    else 0 
    end as ImpSplity,

--0 as ExpAsseco, --testy bez Exportu
1 as ExpAsseco,
1 as DoBackup,

O.StatusPL 
from OkresyRozl O 
outer apply (select case when GETDATE() &gt; O.DataDo then O.DataDo else dbo.getdate(GETDATE()) end as NaDzien) O1
--outer apply (select '20160302' as NaDzien) O1     --testy

where O.StatusPL &gt; 0 and O.Status = 0 and GETDATE() between O.DataOd and DATEADD(D,15,O.DataDo) --dodatkowe zabezpieczenie - tylko przez 15 dni moze automat przeliczac poprzedni miesiac
order by O.DataOd desc
    ">
</asp:SqlDataSource>

<%--
select top 1 
O.DataOd as Od, O.DataDo as Do, 
case when DATEADD(D, -1, O1.NaDzien) = O.DataDo then DATEADD(D, -1, O1.NaDzien)  --za ostatni dzień w miesiącu
    else O1.NaDzien
    end as NaDzien, 
    
--case O.StatusPL when 1 then 1  --testy bez FTE
case O.StatusPL when 1 then 3  --tylko jak otwarty
    else 0 
    end as ImpSplity,

--0 as ExpAsseco, --testy bez Exportu
1 as ExpAsseco,
1 as DoBackup,

O.StatusPL 
from OkresyRozl O 
outer apply (select dbo.getdate(GETDATE()) as NaDzien) O1
--outer apply (select '20160302' as NaDzien) O1     --testy

where O.StatusPL &gt; 0 and (O.StatusPL != 3 or DATEADD(D, -1, O1.NaDzien) between O.DataOd and O.DataDo) --zeby sie w nocy przeliczyl
order by O.DataOd
--%>

<asp:SqlDataSource ID="dsBackup" runat="server"
    SelectCommand="
select * into {2}..PodzialLudziImport_{1} from PodzialLudziImport 
--where OkresOd = '{0}'

select * into {2}..Splity_{1} from Splity 
--where DataOd = '{0}'

select * into {2}..SplityWsp_{1} from SplityWsp 
--where IdSplitu in (select Id from Splity where DataOd = '{0}')            
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsGetSplity" runat="server"
    SelectCommand="
declare @pracId int
declare @kid int
set @pracId = {0}
set @kid = {1}

----- SplityWsp -----
--drop table #www

declare	@od datetime
declare	@do datetime
declare	@_do datetime
set @do = '{2}'
set @od = dbo.bom(@do)



set @_do = @do              -- do końca okresu SPR
set @do = dbo.eom(@do)



declare @cnt int
set @cnt = DATEDIFF(DD, @od, @do) + 1 - (select COUNT(*) from Kalendarz where Data between @od and @do) 

declare @ccSurplus int
--select @ccSurplus = Id from CC where cc = '499'
select top 1 @ccSurplus = Id from CC where Surplus = 1



/*----- bierze tylko z dni -----
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
where K.Data is null and P.KadryId between 0 and 80000-1 and P.Status &gt;= -1
group by R.IdPracownika, SPL.Id, W.IdCC
) D
order by IdPracownika, IdSplitu, IdCC
-----*/



/*----- bierze z PodzialKosztow.CzasZm -----*/
select P.Id as IdPracownika, S.Id as IdSplitu, W.IdCC, ROUND(SUM(ISNULL(W.Wsp,0)) / SS.CzasZmPlan, 4) as Wsp  
into #www
from Pracownicy P
outer apply (select 8 * 3600 * P.EtatL / P.EtatM as WymiarEtat) P1 
left join Splity S on S.GrSplitu = P.KadryId /*P.GrSplitu*/ and @do between S.DataOd and ISNULL(S.DataDo, '20990909')
outer apply 
	(
	select (sum(P2.WymiarEtat)) as CzasZmPlan  -- suma sekund
	from dbo.GetDates2(@od, @do) D2
	outer apply (select P1.WymiarEtat) P2	-- trzeba tak
	inner join Przypisania R2 on D2.Data between R2.Od and ISNULL(R2.Do, '20990909') and R2.Status = 1 and R2.IdPracownika = P.Id	-- okres zatrudnienia
	left join Kalendarz K2 on K2.Data = D2.Data	
	where K2.Data is null -- dni robocze    
	) SS   
cross join dbo.GetDates2(@od, @do) D
left join Kalendarz K on K.Data = D.Data -- powinno iść po zmianach bo wolne za święto
left join PlanPracy PP on PP.IdPracownika = P.Id and PP.Data = D.Data and PP.Akceptacja = 1
inner join Przypisania R on R.IdPracownika = P.Id and D.Data between R.Od and ISNULL(R.Do,'20990909') and R.Status = 1
outer apply 
	(
	select ISNULL(sum(PK.CzasZm), 0) as CzasZmPK -- liczę wg etatu chyba że jest czas na zmianie rozpisany
	from PodzialKosztow PK 
	where PP.Akceptacja = 1 and PK.IdPracownika = R.IdPracownika and PK.Data = D.Data and PK.CzasZm is not null	-- tylko rozpiska czasu na zmianie
	) PK
outer apply (select case when 1 = 1 then P1.WymiarEtat else PP.CzasZm end as CzasZmPlan) PP1 -- czas zaplanowany, powinien być = ilości godzin zmiany dla niepełnoetatowców, póki co Etat
outer apply (select case when PP1.CzasZmPlan &gt; PK.CzasZmPK then PP1.CzasZmPlan - PK.CzasZmPK else 0 end as CzasZmMac) PP2 -- czas zaplanowany - czas podzielony z zabezpieczeniem 
outer apply 
	(
	select --Id, IdPrzypisania, 
		IdCC, Wsp * PP2.CzasZmMac as Wsp
	from SplityWspP 
	where IdPrzypisania = R.Id
		union all
	select IdMPK, PK.CzasZm as Wsp
	from PodzialKosztow PK 
	where PP.Akceptacja = 1 and PK.IdPracownika = R.IdPracownika and PK.Data = D.Data and PK.CzasZm is not null	-- tylko rozpiska czasu na zmianie
	) W
where K.Data is null  
and (
	@pracId is null and P.KadryId between 0 and 80000-1 and P.Status &gt;= -1
 or P.Id = @pracId
	)
group by P.Id, S.Id, W.IdCC, SS.CzasZmPlan
order by IdPracownika, IdSplitu, IdCC
/*------*/


-- brak wsp lub niedomiar do 1
insert into #www
select IdPracownika, IdSplitu, @ccSurplus, ROUND(1 - SUM(Wsp), 4)  
from #www group by IdPracownika, IdSplitu having ROUND(SUM(Wsp), 4) != 1

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
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsOpenSplity" runat="server"
    SelectCommand="
declare @od datetime
declare @od1 datetime
declare @do1 datetime
set @od = '{0}'
set @od1 = DATEADD(M, -1, @od)
set @do1 = dbo.eom(@od1)

/*
drop table #sss
drop table #ddd
select * into #ddd from (select 
(select max(Id) from Splity) MaxSplity,
(select max(Id) from SplityWsp) MaxSplityWsp) D
*/
/*
select * into HR_TMP..Splity_20160228 from Splity
select * into HR_TMP..SplityWsp_20160228 from SplityWsp

select * from VSplity3 where GrSplitu in (select GrSplitu from CC where Grupa = 1) and DataOd = '20160101' order by GrSplitu, cc
select * from VSplity3 where GrSplitu in (select GrSplitu from CC where Grupa = 1) and DataOd = '20160201' order by GrSplitu, cc
select * from VSplity3 where GrSplitu in (select GrSplitu from CC where Grupa = 1) and DataOd = '20160301' order by GrSplitu, cc

delete from Splity where Id &gt; (select MaxSplity from #ddd)
delete from SplityWsp where Id &gt; (select MaxSplityWsp from #ddd)
update Splity set DataDo = null where Id in (select Id from #sss)
*/

-- splity do zamknięcia z poprzedniego okresu wg Grup splitów
select S.Id, CC.GrSplitu, S.Nazwa 
into #sss
from CC 
inner join Splity S on S.GrSplitu = CC.GrSplitu and @od1 = S.DataOd and S.DataDo is null 
where CC.Grupa = 1 --and CC.AktywneOd &lt;= @do1 and @od1 &lt;= ISNULL(CC.Aktywnedo,'20990909') -- nie jest istotne czy aktywne
--select * from #sss

-- zamykam z poprzedniego miesiąca
update Splity set DataDo = @do1 where Id in (select Id from #sss)
--select * from Splity where Id in (select Id from #sss)

-- dodaję Splity
insert into Splity 
select S.GrSplitu, S.Nazwa, @od, null, 0 
from #sss S
inner join CC on CC.GrSplitu = S.GrSplitu and @od between CC.AktywneOd and ISNULL(CC.Aktywnedo,'20990909')

-- dodaję współczynniki
insert into SplityWsp 
select N.Id, W.IdCC, W.Wsp
from Splity N
inner join #sss S on S.GrSplitu = N.GrSplitu 
left join SplityWsp W on W.IdSplitu = S.Id
where N.DataOd = @od and N.DataDo is null
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsImport" runat="server"
    SelectCommand="
BEGIN TRANSACTION;
BEGIN TRY
-------------------------------
-------------------------------
declare @step int
set @step = 0

declare @mode int
declare	@od datetime
declare	@do datetime
declare @_do datetime
set @mode = {0}
set @od = '{1}'	-- dla cc: zakres od bo mogą być cc wyłączone 	, jak działa wszystko ok, to taka sam jak @p1
set @do = '{2}'



set @_do = @do
set @do = dbo.eom(@do)  -- koniec okresu SPR



----- Część I Splity-----------
if @mode = 1 or @mode = 3 begin
-------------------------------
declare @cnt int
set @cnt = DATEDIFF(DD, @od, @do) + 1 - (select COUNT(*) from Kalendarz where Data between @od and @do) 

declare @ccSurplus int
--select @ccSurplus = Id from CC where cc = '499'
select top 1 @ccSurplus = Id from CC where Surplus = 1

set @step = 1
----- PodzialLudziImport -----
update PodzialLudziImport set OkresDo = DATEADD(D, -1, @od) where OkresOd = DATEADD(M, -1, @od) and OkresDo is null
delete from PodzialLudziImport where OkresOd = @od and OkresDo is null

--select * from PodzialLudziImport where KadryId = '00001' order by OkresOd desc

set @step = 2

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
left join Przypisania R on R.IdPracownika = P.Id and @_do between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1
left join PracownicyStanowiska PS on PS.IdPracownika = P.Id and @_do between PS.Od and ISNULL(PS.Do, '20990909')
left join Stanowiska S on S.Id = PS.IdStanowiska
left join Commodity C on C.Id = R.IdCommodity
left join Area A on A.Id = R.IdArea
left join Position POS on POS.Id = R.IdPosition
left join OkresyRozl RR on @od between RR.DataOd and RR.DataDo and RR.Archiwum = 1
left join PracownicyOkresy PO on PO.Id = P.Id and PO.IdOkresu = RR.Id
where P.KadryId &lt; 80000 and P.Status &gt;= -1 and P.Id in (select IdPracownika from Przypisania where Od &lt;= @_do and @od &lt;= ISNULL(Do, '20990909') and Status = 1)

--kontrola TypImport !!!
--select * from PodzialLudziImport where TypImport is null and DataZatr != '20141201'

set @step = 3

----- Splity -----
--select * from Splity
--update Splity set DataDo = DATEADD(D, -1, @od) where DataOd = DATEADD(M, -1, @od) and DataDo is null
update Splity set DataDo = DATEADD(D, -1, @od) where DataDo is null 
and Id in  
(
select S.Id 
from PodzialLudziImport I 
outer apply (select top 1 * from Splity where GrSplitu = I.KadryId and DataOd &lt; @od order by DataOd desc) S
where I.OkresOd = @od
) 

set @step = 4

delete from SplityWsp where IdSplitu in 
    (select Id from Splity where DataOd = @od and DataDo is null 
        and GrSplitu not in 
            (select GrSplitu from CC where GrSplitu is not null) 
    ) 

set @step = 5
    
delete from Splity where DataOd = @od and DataDo is null
    and GrSplitu not in 
        (select GrSplitu from CC where GrSplitu is not null) 

set @step = 6

insert into Splity 
select I.KadryId, 'Split ' + I.KadryId + ' ' + I.Pracownik, @od, null, 0
from PodzialLudziImport I
where OkresOd = @od

set @step = 7
----- SplityWsp -----



/*----- bierze z dni -----
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
where K.Data is null and P.KadryId &lt; 80000 and P.Status &gt;= -1
group by R.IdPracownika, SPL.Id, W.IdCC
) D
order by IdPracownika, IdSplitu, IdCC
-----*/

/*----- bierze z PodzialKosztow.CzasZm, to samo co w  -----*/
declare @pracId int
set @pracId = null

select P.Id as IdPracownika, S.Id as IdSplitu, W.IdCC, ROUND(SUM(ISNULL(W.Wsp,0)) / SS.CzasZmPlan, 4) as Wsp  
into #www
from Pracownicy P
outer apply (select 8 * 3600 * P.EtatL / P.EtatM as WymiarEtat) P1 
left join Splity S on S.GrSplitu = P.KadryId /*P.GrSplitu*/ and @do between S.DataOd and ISNULL(S.DataDo, '20990909')
outer apply 
	(
	select (sum(P2.WymiarEtat)) as CzasZmPlan  -- suma sekund
	from dbo.GetDates2(@od, @do) D2
	outer apply (select P1.WymiarEtat) P2	-- trzeba tak
	inner join Przypisania R2 on D2.Data between R2.Od and ISNULL(R2.Do, '20990909') and R2.Status = 1 and R2.IdPracownika = P.Id	-- okres zatrudnienia
	left join Kalendarz K2 on K2.Data = D2.Data	
	where K2.Data is null -- dni robocze    
	) SS   
cross join dbo.GetDates2(@od, @do) D
left join Kalendarz K on K.Data = D.Data -- powinno iść po zmianach bo wolne za święto
left join PlanPracy PP on PP.IdPracownika = P.Id and PP.Data = D.Data and PP.Akceptacja = 1
inner join Przypisania R on R.IdPracownika = P.Id and D.Data between R.Od and ISNULL(R.Do,'20990909') and R.Status = 1
outer apply 
	(
	select ISNULL(sum(PK.CzasZm), 0) as CzasZmPK -- liczę wg etatu chyba że jest czas na zmianie rozpisany
	from PodzialKosztow PK 
	where PP.Akceptacja = 1 and PK.IdPracownika = R.IdPracownika and PK.Data = D.Data and PK.CzasZm is not null	-- tylko rozpiska czasu na zmianie
	) PK
outer apply (select case when 1 = 1 then P1.WymiarEtat else PP.CzasZm end as CzasZmPlan) PP1 -- czas zaplanowany, powinien być = ilości godzin zmiany dla niepełnoetatowców, póki co Etat
outer apply (select case when PP1.CzasZmPlan &gt; PK.CzasZmPK then PP1.CzasZmPlan - PK.CzasZmPK else 0 end as CzasZmMac) PP2 -- czas zaplanowany - czas podzielony z zabezpieczeniem 
outer apply 
	(
	select --Id, IdPrzypisania, 
		IdCC, Wsp * PP2.CzasZmMac as Wsp
	from SplityWspP 
	where IdPrzypisania = R.Id
		union all
	select IdMPK, PK.CzasZm as Wsp
	from PodzialKosztow PK 
	where PP.Akceptacja = 1 and PK.IdPracownika = R.IdPracownika and PK.Data = D.Data and PK.CzasZm is not null	-- tylko rozpiska czasu na zmianie
	) W
where K.Data is null  
and (
	@pracId is null and P.KadryId between 0 and 80000-1 and P.Status &gt;= -1
 or P.Id = @pracId
	)
group by P.Id, S.Id, W.IdCC, SS.CzasZmPlan
order by IdPracownika, IdSplitu, IdCC
/*-----*/


set @step = 8

-- brak wsp lub niedomiar do 1
insert into #www
select IdPracownika, IdSplitu, @ccSurplus, ROUND(1 - SUM(Wsp),4)  
from #www group by IdPracownika, IdSplitu having ROUND(SUM(Wsp),4) != 1

set @step = 9

delete from #www where IdCC is null

set @step = 10

----- SplityWsp - import -----
--select * from SplityWsp
insert into SplityWsp
select IdSplitu, IdCC, Wsp from #www W

set @step = 11

----- DataSplitow -----
--update OkresyRozl set DataImportu = '{1}' where DataOd = '{0}' 
--update OkresyRozl set DataSplitow = '{1}' where DataOd = '{0}' 
update OkresyRozl set DataSplitow = @_do where DataOd = @od 

set @step = 12

end



/**/



----- Część II FTE ------------
if @mode = 2 or @mode = 3 begin
-------------------------------

--declare @od datetime
--declare @do datetime
declare @dzis datetime
declare @boy datetime
declare @nom int

--declare @boy2 datetime
--set @boy2 = DATEADD(Y, -2, @boy)  -- do liczenia ostatni dzień w kosztach
--set @boy2 = @boy

set @dzis = @_do

--set @od = '20150201'
--set @dzis = '20150216'

set @do = dbo.eom(@od)
set @boy = dbo.boy(@od)

declare @stmt nvarchar(max)

set @step = 13
---------------------------------
-- pracownicy +50 lat - wg daty do - koniec miesiąca chociaz lp_fn_BasePracExLow rownie dobrze byloby wywolac z GETDATE() bo nie ma warunków na zatrudniony
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
where YEAR(@data) - YEAR(DataUrodzenia) &gt; 50

'')'
insert into #prac50
exec (@stmt)

set @step = 14

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

set @step = 15

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
where DataOd &gt;= ''''20111101''''

union all

select ''''Z'''' as Typ,lp_ZasilkiId as Id,LpLogo,DataOd,DataDo,T.lp_ZasilkiTypId as Kod,LDni as IleDni,LDni*8 as Godziny,0 as Zalegly,0 as Planowany,0 as NaZadanie,Rok,Miesiac,cast(Korekta as bit) as Korekta,lp_ZasilkiIdK as IdKorygowane,Z.Aktywny 
,Z.KodZUS, Z.KodZUSChoroby, null as Symbol, 
case when Z.KodZUS in (' + @kodyZUS + ') then 1 else 0 end as placiZUS
from sl_hr_jabilgs.dbo.lp_Zasilki Z
left join sl_hr_jabilgs.dbo.lp_zasilkityp T with (nolock) on T.ZasilekTyp = Z.ZasilekTyp
where DataOd &gt;= ''''20111101''''
'')'
insert into #abs151
exec (@stmt)

set @step = 16

-- aktualizacja:1:pozostawienie ostatniego rekordu z korekty -----
delete from #abs151 where Id in 
(select A.Id from #abs151 A 
left outer join #abs151 K on K.Typ = A.Typ and K.Id= A.IdKorygowane
where (A.Korekta = 1 or A.Id in (select distinct IdKorygowane from #abs151 where Korekta = 1))
and (A.Korekta = 0 or A.Id != (select MAX(Id) from #abs151 where Typ = A.Typ and IdKorygowane = A.IdKorygowane)))

set @step = 17

delete from #abs151 where IleDni &lt;= 0 

set @step = 18

update #abs151 set Symbol = AK.Symbol
from #abs151 A
left join AbsencjaKody AK on AK.Kod = A.Kod

set @step = 19

update #abs151 set placiZUS = 1
where Typ = 'U' 
and Symbol in (select items from dbo.SplitStr(@kodyABSDL,',')) 

set @step = 20

--select * from #abs151 
---------------------------------
-- chorobowe 33 i 14 jak wiek +50 lat w następnym roku,  -- ostatnia data w kosztach
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
case when DniCH &gt;= LimitDni then DATEADD(D, -1, Data) else Data end as Data,  -- jak limit jest wyczerpany to dzień wcześniej !!!
Data as Data1, 
Wiek2, DniCH, LimitDni, rownum
into #chor33
FROM cte
WHERE (DniCH &lt; LimitDni and DniCH + rownum = LimitDni)  -- musimy policzyć
   or (DniCH &gt;= LimitDni and rownum = 1)				-- już jest wyczerpany

--select * from #chor33

set @step = 21

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
when DataZwol is not null and DataZwol &lt;= @do then convert(varchar(10), DataZwol, 20) --'Z'
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
or @dzis &gt; ISNULL(D.[Ostatni dzień w kosztach],'20990909')  -- lub przekroczył 33/14 dni  
then 0 else 1 end as CHwKosztach				-- 0-ch niepłatne, 1-w kosztach

into #fte

from
(
select P.Id as IdPracownika, P.KadryId as Logo, P.Nazwisko + ' ' + P.Imie as Pracownik,
SUM(case when D.Data between P.DataZatr and ISNULL(P.DataZwol, '20990909') 
    and (A.Symbol is null 
     or (A.Symbol = 'CH' and A.placiZUS = 0 and D.Data &lt;= ISNULL(CH.Data, '20990909'))
     or (A.Symbol != 'CH' and A.placiZUS = 0)) 
	and AA.Symbol is null

    --and (ISNULL(A.Symbol,'') not in ('CH') or D.Data &lt;= ISNULL(CH.Data, '20990909') and ISNULL(A.KodZUS,0) != 151)	
	--and  ISNULL(A.Symbol,'') not in ('UB','WY','M','R')   -- bezpłatny, wychowawczy, macierzyński, rehabilitacja

    and K.Data is null then 1 else 0 end) as DniZatr,
SUM(case when R.Id is not null 
    and (A.Symbol is null 
     or (A.Symbol = 'CH' and A.placiZUS = 0 and D.Data &lt;= ISNULL(CH.Data, '20990909'))
     or (A.Symbol != 'CH' and A.placiZUS = 0)) 
	and AA.Symbol is null

    --and (ISNULL(A.Symbol,'') not in ('CH') or D.Data &lt;= ISNULL(CH.Data, '20990909') and ISNULL(A.KodZUS,0) != 151)
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


outer apply (select top 1 * from #abs151 where LpLogo = P.KadryId and DataOd &lt;= D.Data and @od &lt;= DataDo and KodZUS = 313 and KodZUSChoroby = 'B' order by DataOd desc) AA  --plomba FTE=0 dla ciąż, ostatnia CH, kończące się w okresie, po zawsze jest M dlatego zadziała 

 
left join #chor33 CH on CH.IdPracownika = P.Id
where P.Id in 
(
select IdPracownika from Przypisania where ISNULL(Do, '20990909') &gt;= @od and Od &lt;= @do and Status = 1 
)
and P.KadryId &lt; 80000 and P.Status &gt;= -1
group by P.Id, P.Nazwisko, P.Imie, P.KadryId
) D

--left join PodzialLudziImport I on I.KadryId = D.Logo and @od between I.OkresOd and ISNULL(I.OkresDo, '20990909')	-- do porównania
--where IdPracownika = 58

order by D.Pracownik

set @step = 22

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

/*
select * from #poprzCH
select * from #prac50
select * from #chor33
select * from #abs151 
select * from #fte
*/

set @step = 23

end



/**/


-------------------------------
-------------------------------
END TRY
BEGIN CATCH
	select -1 as Error, ERROR_MESSAGE() AS ErrorMessage, @step as Step
	/*
    SELECT 
         ERROR_NUMBER() AS ErrorNumber
        ,ERROR_SEVERITY() AS ErrorSeverity
        ,ERROR_STATE() AS ErrorState
        ,ERROR_PROCEDURE() AS ErrorProcedure
        ,ERROR_LINE() AS ErrorLine
        ,ERROR_MESSAGE() AS ErrorMessage;
	*/
    IF @@TRANCOUNT &gt; 0
        ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT &gt; 0 BEGIN
    COMMIT TRANSACTION;
    select 0 as Error, null as ErrorMessage, @step as Step
END    
    ">
</asp:SqlDataSource>

