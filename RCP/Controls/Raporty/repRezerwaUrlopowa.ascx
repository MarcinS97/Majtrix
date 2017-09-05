<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="repRezerwaUrlopowa.ascx.cs" Inherits="HRRcp.Controls.Raporty.repRezerwaUrlopowa" %>
<%@ Register src="~/Controls/Reports/cntReport2.ascx" tagname="cntReport" tagprefix="uc1" %>
<%@ Register src="~/Controls/SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc1" %>

<table class="okres_navigator printoff">
    <tr>
        <td class="colmiddle">
            <uc1:SelectOkres ID="cntSelectOkres" OnOkresChanged="cntSelectOkres_Changed" runat="server" StoreInSession="true" />
        </td>
    </tr>
</table>

<uc1:cntReport ID="cntReport1" runat="server" 
    CssClass="RepRezerwaUrlopowa"
    HeaderVisible="true"
    SQL="
declare 
    @od datetime,
    @do datetime,
    @zal int
set @od = '@SQL1'
set @do = '@SQL2'
--set @do = dbo.eom(@od)
set @zal = 9

declare @nom int 
select @nom = DniPrac from CzasNom where Data = @od

select @od as Data, @do as DataDo, P.Id, P.KadryId, P.Pracownik, Grupa, Klasyfikacja, 
--(CAST(WymiarRok2 as float) / 12 - PlanU) * (Stawka / @nom) as Rezerwa
--(ISNULL(UrlopZalegDni,0) + dbo.GetWymiar12a(@od, DataZatr, ISNULL(DataZwiekszenia,'20990909')) - PlanU) * (Stawka / @nom) as Rezerwa
--,(ISNULL(UrlopZalegDni,0) + dbo.GetWymiar12a(@od, DataZatr, ISNULL(DataZwiekszenia,'20990909')) - PlanU) as RezerwaIlosc
((ISNULL(UrlopZalegDni,0) * case when MONTH(@od) &lt;= @zal then MONTH(@od) else @zal end / @zal) + dbo.GetWymiar12a(@od, DataZatr, ISNULL(DataZwiekszenia,'20990909')) - PlanU) * (Stawka / @nom) * 1.2 as Rezerwa
,((ISNULL(UrlopZalegDni,0) * case when MONTH(@od) &lt;= @zal then MONTH(@od) else @zal end / @zal) + dbo.GetWymiar12a(@od, DataZatr, ISNULL(DataZwiekszenia,'20990909')) - PlanU) as RezerwaIlosc
,case when MONTH(@od) &lt;= @zal then MONTH(@od) else @zal end as ZalWsp
,UrlopZalegDni, DataZwiekszenia, dbo.GetWymiar12a(@od, DataZatr, DataZwiekszenia) as Wymiar12, PlanU, Datazatr, DataZwol, Stawka

into #ppp
from  
(
select P.Id, P.KadryId, P.Nazwisko + ' ' + P.Imie as Pracownik, ISNULL(PO.Stawka, P.Stawka) as Stawka, P.DataZatr, P.DataZwol,
PS.Grupa, PS.Klasyfikacja,
UZ.WymiarRok, UZ.DataZwiekszenia, UZ.UrlopZalegDni,
--case when @do &lt; UZ.DataZwiekszenia then 20 else 26 end as WymiarRok2,
PUboy.PlanU
--PU.PlanU
from Pracownicy P 
left join OkresyRozl RR on @do between RR.DataOd and RR.DataDo and RR.Archiwum = 1
left join PracownicyOkresy PO on PO.Id = P.Id and PO.IdOkresu = RR.Id

--outer apply (select top 1 * from UrlopZbior where KadryId = P.KadryId and Rok &lt;= DATEPART(YEAR, @do) order by Rok desc) UZ
left join UrlopZbior UZ on UZ.KadryId = P.KadryId and UZ.Rok = DATEPART(YEAR, @od)

outer apply (select count(*) as PlanU from PlanUrlopow where IdPracownika = P.Id and Data between dbo.boy(@od) and @do and Do is null and KodUrlopu is not null) PUboy
outer apply (select count(*) as PlanU from PlanUrlopow where IdPracownika = P.Id and Data between @od and @do and Do is null and KodUrlopu is not null) PU
outer apply (select top 1 * from PracownicyStanowiska where IdPracownika = P.Id and Od &lt;= @do order by Od desc) PS
outer apply (select top 1 * from Splity where GrSplitu = P.KadryId and DataOd &lt;= @do and @od &lt;= ISNULL(DataDo, '20990909')) S
where P.Id in (select IdPracownika from Przypisania where Od &lt;= @do and @od &lt;= ISNULL(Do, '20990909') and Status = 1)
and P.Status &gt;= -1 and P.KadryId &lt; 80000
and S.Id is not null

--and P.KadryId = '01949'

) P

--select * from #ppp 
/*
--- spr null ----
select A.*, P.Status, P.DataZatr, P.DataZwol, P.* from #ppp A 
left join Splity S on S.GrSplitu = A.KadryId and A.Data between S.DataOd and ISNULL(S.DataDo, '20990909')
left join Pracownicy P on P.Id = A.Id
where S.Id is null
order by A.Pracownik
--------
*/

--drop table #ddd

select 
cc, 
Nazwa [Nazwa:C], 
ROUND(ISNULL(DL, 0),2) DL,--[DL:NN2S;w100], 
ROUND(ISNULL(BUIL, 0),2) BUIL,--[BUIL:NN2S;w100], 
ROUND(ISNULL([SG&A], 0),2) [SG&A],--[SG&A:NN2S;w100],
ROUND(ISNULL(DL,0),2) + ROUND(ISNULL(BUIL,0),2) + ROUND(ISNULL([SG&A],0),2) Suma--[Suma:NN2S;w100]
into #ddd
from 
(
select 
ISNULL(C1.cc, CC.cc) as cc,
ISNULL(C1.Nazwa, CC.Nazwa) as Nazwa,
R.Grupa,
sum(R.Rezerwa * ISNULL(W1.Wsp, ISNULL(W.Wsp, 1))) as Kwota
from #ppp R 
left outer join Splity S on S.GrSplitu = R.KadryId and @do between S.DataOd and ISNULL(S.DataDo, @do)  -- macierzyste 'wg podzialu'
left outer join SplityWsp W on W.IdSplitu = S.Id 
left outer join CC on CC.Id = W.IdCC

left outer join Splity S1 on S1.GrSplitu = CC.GrSplitu and @do between S1.DataOd and ISNULL(S1.DataDo, @do)
left outer join SplityWsp W1 on W1.IdSplitu = S1.Id
left outer join CC C1 on C1.Id = W1.IdCC
group by
    ISNULL(C1.cc, CC.cc),
    ISNULL(C1.Nazwa, CC.Nazwa),
    R.Grupa
) as D
PIVOT 
(
    sum(Kwota) for D.Grupa in (DL,BUIL,[SG&A])
) as PV
order by 1


select CC.cc [CC], 
CC.Nazwa [Nazwa:C], 
ISNULL(D.DL,0) [DL:NN2S;w100], 
ISNULL(D.BUIL,0) [BUIL:NN2S;w100], 
ISNULL(D.[SG&A],0) [SG&A:NN2S;w100], 
ISNULL(Suma,0) [Suma:NN2S;w100]
from CC
left join #ddd D on D.cc = CC.cc
where CC.cc in ('001','002','003','004','005','007','009','010','012','013','014','015','018','020','021','022','023','025','026','027','028','029','030','035','036','038','042','043','323','399','499','720','760','780','790','801','802','804','807','809','810','811','812','832','855','910','930','944','950','970','975','990')
order by CC.cc
"/>



<%--
<uc1:cntReport ID="cntReport1" runat="server" 
    CssClass="RepRezerwaUrlopowa"
    HeaderVisible="true"
    SQL="
declare 
    @od datetime,
    @do datetime
set @od = '@SQL1'
set @do = '@SQL2'
--set @do = dbo.eom(@od)

declare @nom int 
select @nom = DniPrac from CzasNom where Data = @od

select @od as Data, @do as DataDo, P.Id, P.KadryId, P.Pracownik, Grupa, Klasyfikacja, (CAST(WymiarRok2 as float) / 12 - PlanU) * (Stawka / @nom) as Rezerwa
into #ppp
from  
(
select P.Id, P.KadryId, P.Nazwisko + ' ' + P.Imie as Pracownik, ISNULL(PO.Stawka, P.Stawka) as Stawka, 
PS.Grupa, PS.Klasyfikacja,
UZ.WymiarRok, UZ.DataZwiekszenia, 
case when @do &lt; UZ.DataZwiekszenia then 20 else 26 end as WymiarRok2,
PU.PlanU
from Pracownicy P 
left join OkresyRozl RR on @do between RR.DataOd and RR.DataDo and RR.Archiwum = 1
left join PracownicyOkresy PO on PO.Id = P.Id and PO.IdOkresu = RR.Id
outer apply (select top 1 * from UrlopZbior where KadryId = P.KadryId and Rok &lt;= DATEPART(YEAR, @do) order by Rok desc) UZ
outer apply (select count(*) as PlanU from PlanUrlopow where IdPracownika = P.Id and Data between @od and @do and Do is null and KodUrlopu is not null) PU
outer apply (select top 1 * from PracownicyStanowiska where IdPracownika = P.Id and Od &lt;= @do order by Od desc) PS
outer apply (select top 1 * from Splity where GrSplitu = P.KadryId and DataOd &lt;= @do and @od &lt;= ISNULL(DataDo, '20990909')) S
where P.Id in (select IdPracownika from Przypisania where Od &lt;= @do and @od &lt;= ISNULL(Do, '20990909') and Status = 1)
and P.Status &gt;= -1 and P.KadryId &lt; 80000
and S.Id is not null
) P

/*
--select * from #ppp 
--- spr null ----
select A.*, P.Status, P.DataZatr, P.DataZwol, P.* from #ppp A 
left join Splity S on S.GrSplitu = A.KadryId and A.Data between S.DataOd and ISNULL(S.DataDo, '20990909')
left join Pracownicy P on P.Id = A.Id
where S.Id is null
order by A.Pracownik
--------
*/

select 
cc, 
Nazwa [Nazwa:C], 
ROUND(ISNULL(DL, 0),2) [DL:NN2S;w100], 
ROUND(ISNULL(BUIL, 0),2) [BUIL:NN2S;w100], 
ROUND(ISNULL([SG&A], 0),2) [SG&A:NN2S;w100],
ROUND(ISNULL(DL,0),2) + ROUND(ISNULL(BUIL,0),2) + ROUND(ISNULL([SG&A],0),2) [Suma:NN2S;w100]
from 
(
select 
ISNULL(C1.cc, CC.cc) as cc,
ISNULL(C1.Nazwa, CC.Nazwa) as Nazwa,
R.Grupa,
sum(R.Rezerwa * ISNULL(W1.Wsp, ISNULL(W.Wsp, 1))) as Kwota
from #ppp R 
left outer join Splity S on S.GrSplitu = R.KadryId and @do between S.DataOd and ISNULL(S.DataDo, @do)  -- macierzyste 'wg podzialu'
left outer join SplityWsp W on W.IdSplitu = S.Id 
left outer join CC on CC.Id = W.IdCC

left outer join Splity S1 on S1.GrSplitu = CC.GrSplitu and @do between S1.DataOd and ISNULL(S1.DataDo, @do)
left outer join SplityWsp W1 on W1.IdSplitu = S1.Id
left outer join CC C1 on C1.Id = W1.IdCC
group by
    ISNULL(C1.cc, CC.cc),
    ISNULL(C1.Nazwa, CC.Nazwa),
    R.Grupa
) as D
PIVOT 
(
    sum(Kwota) for D.Grupa in (DL,BUIL,[SG&A])
) as PV
order by 1
"/>
--%>

    
    
