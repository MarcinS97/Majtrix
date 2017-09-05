<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntScorecard.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Spreadsheets.cntScorecard" %>

<%@ Register Src="~/Scorecards/Controls/Spreadsheets/cntCosmicInfo.ascx" TagPrefix="leet" TagName="CosmicInfo" %>
<%@ Register Src="~/Scorecards/Controls/Spreadsheets/cntCosmicInfo2.ascx" TagPrefix="leet" TagName="CosmicInfo2" %>
<%@ Register Src="~/Scorecards/Controls/Spreadsheets/cntTasks.ascx" TagPrefix="leet" TagName="Tasks" %>
<%@ Register Src="~/Scorecards/Controls/Spreadsheets/cntDays.ascx" TagPrefix="leet" TagName="Days" %>
<%@ Register Src="~/Scorecards/Controls/Spreadsheets/cntValues.ascx" TagPrefix="leet" TagName="Values" %>   
<%@ Register Src="~/Scorecards/Controls/Spreadsheets/cntCoolStuff.ascx" TagPrefix="leet" TagName="CoolStuff" %>

<%@ Register Src="~/Scorecards/Controls/Spreadsheets/cntHeader.ascx" TagPrefix="leet" TagName="Header" %>

<div id="ctScorecard" runat="server" class="cntScorecard">
    <asp:HiddenField ID="hidScorecardTypeId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidEmployeeId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidDate" runat="server" Visible="false" />
    <asp:HiddenField ID="hidOutsideJob" runat="server" Visible="false" />
    <asp:HiddenField ID="hidObserverId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidTeamLeader" runat="server" Visible="false" />

    <leet:Header ID="Header" runat="server" OnUnAccept="UnAccepted" />
    
    <asp:Button ID="btnBackToMotherScorecard" runat="server" Text="Powrót" Visible="false" CssClass="button100 backButton" OnClick="BackToMotherScorecard" />

    <div class="sc">
        <div id="row1" class="row1">
            <leet:CosmicInfo ID="CosmicInfo" runat="server" />
            <div id="scrollbox3" class="scrollbox3 cnt">
                <leet:Tasks ID="Tasks" runat="server" Visible="true" OnEmptyTasks="EmptyTasks" />
            </div>
            <leet:CosmicInfo2 ID="CosmicInfo2" runat="server" />
        </div>
        <div class="row2">
            <leet:Days ID="Days" runat="server" Visible="true" OnOutsideClick="OutsideClick" />
            <div id="scrollbox1" class="scrollbox1 cnt">
                <leet:Values ID="Values" runat="server" Visible="true" />
            </div>
            <leet:CoolStuff ID="CoolStuff" runat="server" OnRowDeleted="RowDeleted" Visible="true" />
            <div id="daySelectorWrapper" style="display: none;" >
                <div id="daySelector">
                </div>
            </div>
        </div>
    </div>
</div>

<asp:Button ID="btnAccept" runat="server" Text="Accept" OnClick="Accept" style="display: none;" />
<asp:Button ID="btnAcceptAll" runat="server" Text="Accept" OnClick="AcceptAll" style="display: none;" />


<asp:SqlDataSource ID="dsDays" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" OnSelected="dsDays_Selected"
        SelectCommand="
--declare @date datetime = GETDATE()
--declare @typark int = 
--declare @pracId int = 
--declare @oj bit = 1
--declare @observerId int = 
--declare @tl int = 

declare @kanapkaCzas as float = ISNULL(CONVERT(float, (select Kod from Kody where Typ = 'SCCZAS' and Aktywny = 1)) / 60, 6)

declare @od as datetime = dbo.bom(@date)
declare @do as datetime = dbo.eom(@date)

declare 
    @colsH nvarchar(max), 
    @colsD nvarchar(max), 
    @stmt nvarchar(max),
	@stmtv nvarchar(max)


select distinct c.Id, cc.cc, c.Nazwa, c.Czas, tac1.QC into #c from scTypyArkuszyCzynnosci tac1 --zmienic parametry z przypisan
	left join scCzynnosci c on c.Id = tac1.IdCzynnosci
	left join CC cc on cc.Id = c.IdCC
	where tac1.IdTypuArkuszy = @typark and tac1.Od &lt;= @do and @od &lt;= ISNULL(tac1.Do, '20990909')

select @colsH = isnull(@colsH + ',', '') + '[' + convert(varchar, Id) + ']',
       --@colsD = isnull(@colsD + ',', '') + '[' + convert(varchar, Lp) + '] as [' + convert(varchar, Lp) + '|' + convert(varchar, DATEPART(DW, DATEADD(D, Lp, '20010101'))) + ']'  
       @colsD = isnull(@colsD + ',', '') + '[' + convert(varchar, Id) + ']'  
    from #c asd order by asd.Nazwa

if @colsH is null set @colsH = '[0]'
if @colsD is null set @colsD = '[0]'

select @stmtv = '
declare @typark as int = ' + CONVERT(varchar, @typark) + '
declare @pracId as int = ' + CONVERT(varchar, @pracId) + '
declare @date as datetime = ''' + CONVERT(varchar, @date) + '''
declare @oj as bit = ' + CONVERT(varchar, @oj) + ';
declare @ObserverId as int = ' + CONVERT(varchar, @observerId) + ';
declare @tl as bit = ' + CONVERT(varchar, @tl) + ';

declare @kanapkaCzas as float = ' + CONVERT(varchar, @kanapkaCzas) + ';
declare @od as datetime = dbo.bom(@date)
declare @do as datetime = dbo.eom(@date)

'

select @stmt = @stmtv + '
select
d.Id
, dz.Data as Date
, LEFT(CONVERT(nvarchar, dz.Data, 20), 10) as Data
, case when k.Rodzaj = 2 then 1 else 0 end as Data2
, case when k.Data is null then 0 else  1 end as Data3
, CONVERT(varchar, ISNULL(case when SUM(/*ppoa.Nominal*/goa.Godziny) &gt; 0 then prod.Parametr else 0 end, 0) * 100) + ''%'' as CelProd
, CONVERT(varchar, ISNULL(case when SUM(/*ppoa.Nominal*/goa.Godziny) &gt; 0 then qc.Parametr else 0 end, 0) * 100) + ''%'' as CelJak
--KRECHA
, SUM(ppoa.Nominal) as Nominal--ppoa.Nominal * COUNT(prz.IdPracownika) as Nominal
, ROUND(SUM(pp2oa.GodzNieob),3) as GodzNieob
, dbo.cat(ak.Symbol, '','', 0) as KodNieob
, dbo.cat(ak.Nazwa, '','', 0) as NazwaNieob
, ROUND(d.CzasNieprod,3) as CzasNieprod
, PracaInnyArkusz   --, 0) as PracaInnyArkusz
, REPLACE(CONVERT(nvarchar, ROUND(case when @oj = 0 then ISNULL(case when SUM(ISNULL(ppoa.Nominal, 0)) = SUM(ISNULL(pp2oa.GodzNieob, 0)) then 0 + SUM(ISNULL(ppoa.Nadgodziny, 0)) - ISNULL(d.CzasNieprod, 0) - tloa.TLPrac else SUM(ISNULL(ppoa.Nominal, 0)) - SUM(ISNULL(pp2oa.GodzNieob, 0)) - ISNULL(d.PracaInnyArkusz, 0) - ISNULL(d.CzasNieprod, 0) - tloa.TLPrac + SUM(ISNULL(ppoa.Nadgodziny, 0)) end, 0) else ISNULL(/*d*/SUM(poa.CzasProd), 0) end, /*3*/2)), '','', ''.'') as War1
, REPLACE(CONVERT(nvarchar, ROUND(case when @oj = 0 then ISNULL(case when SUM(ISNULL(ppoa.Nominal, 0)) = SUM(ISNULL(pp2oa.GodzNieob, 0)) then 0 + SUM(ISNULL(ppoa.Nadgodziny, 0)) - ISNULL(d.CzasNieprod, 0) - tloa.TLPrac - (ISNULL(s.Parametr, 0) * SUM(aoa.Alive)) else /*case when SUM(ISNULL(ppoa.Nominal, 0)) + SUM(ISNULL(ppoa.Nadgodziny, 0)) - SUM(ISNULL(pp2oa.GodzNieob, 0)) - ISNULL(d.PracaInnyArkusz, 0) - ISNULL(d.CzasNieprod, 0) - tloa.TLPrac - (ISNULL(s.Parametr, 0) * SUM(aoa.Alive)) &lt; 0 then 0 else*/ SUM(ISNULL(ppoa.Nominal, 0)) + SUM(ISNULL(ppoa.Nadgodziny, 0)) - SUM(ISNULL(pp2oa.GodzNieob, 0)) - ISNULL(d.PracaInnyArkusz, 0) - ISNULL(d.CzasNieprod, 0)  - tloa.TLPrac - (ISNULL(s.Parametr, 0) * SUM(aoa.Alive)/*(COUNT(distinct prz.IdPracownika) + SUM(ISNULL(d.Korekta, 0)))*/) /*end*/ end, 0) else ISNULL(/*d*/SUM(poa.CzasProd), 0) end, /*3*/2)), '','', ''.'') as War2
, ROUND(SUM(ppoa.Nadgodziny),3) as  Nadgodziny
, COUNT(prz.IdPracownika) as TeamSize
, ISNULL(Korekta, 0) as Korekta
, d.IloscBledow--, 0) as IloscBledow
, d.Uwagi
, case when d.Id is not null then 1 else 0 end as Bang
, tloa.TLPrac
, case when @oj = 0 then
    case when COUNT(prz.Id) is not null then
        case when
            case when @oj = 0 then ISNULL(case when SUM(ISNULL(ppoa.Nominal, 0)) = SUM(ISNULL(pp2oa.GodzNieob, 0)) then 0 + SUM(ISNULL(ppoa.Nadgodziny, 0)) else SUM(ISNULL(ppoa.Nominal, 0)) - SUM(ISNULL(pp2oa.GodzNieob, 0)) /*- ISNULL(d.PracaInnyArkusz, 0) - ISNULL(d.CzasNieprod, 0)*/ + SUM(ISNULL(ppoa.Nadgodziny, 0)) end, 0) else ISNULL(/*d*/SUM(poa.CzasProd), 0) end != 0
            then
            1
            else
            0
        end
        else 0
    end
    else
        case when @oj = 1 then
            case when d.Id is not null then 1
                else 0
            end
            else 0
        end
    end as State
, case when SUM(pgiooa.c) &gt; 0

and case when @oj = 0 then
    case when COUNT(prz.Id) is not null then
        case when
            case when @oj = 0 then ISNULL(case when SUM(ISNULL(ppoa.Nominal, 0)) = SUM(ISNULL(pp2oa.GodzNieob, 0)) then 0 + SUM(ISNULL(ppoa.Nadgodziny, 0)) else SUM(ISNULL(ppoa.Nominal, 0)) - SUM(ISNULL(pp2oa.GodzNieob, 0)) /*- ISNULL(d.PracaInnyArkusz, 0) - ISNULL(d.CzasNieprod, 0)*/ + SUM(ISNULL(ppoa.Nadgodziny, 0)) end, 0) else ISNULL(/*d*/SUM(poa.CzasProd), 0) end != 0
            then
            1
            else
            0
        end
        else 0
    end
    else
        case when @oj = 1 then
            case when d.Id is not null then 1
                else 0
            end
            else 0
        end
    end = 1

then 1 else 0 end as Lupka
--, d.*
, ' + @colsD + ' from
(
	select * from
	(
        select dz.Data, CONVERT(varchar, coa.Id) + ''|'' + ISNULL(CONVERT(varchar, w.Ilosc), '''') as Ilosc, coa.Id
		from dbo.GetDates2(@od, @do) as dz
		outer apply (
			select distinct c.* from scTypyArkuszyCzynnosci tac --zmienic parametry z przypisan
			left join scCzynnosci c on c.Id = tac.IdCzynnosci
			where tac.IdTypuArkuszy = @typark and tac.Od &lt;= @do and @od &lt;= ISNULL(tac.Do, ''20990909'')
		) coa
		left join scWartosci w on coa.Id = w.IdCzynnosci and w.IdTypuArkuszy = @typark and w.IdPracownika = @pracId and w.Data = dz.Data
	) as cz
	PIVOT
	(
		MAX(cz.Ilosc) FOR cz.Id IN (' + @colsH + ')
	) as PV
) as dz
left join Kalendarz k on dz.Data = k.Data
left join scDni d on d.Data = dz.Data and d.IdTypuArkuszy = @typark and d.IdPracownika = @pracId
--left join scTypyArkuszy ta on ta.Id = @typark
--left join scSlowniki s on s.Id = ta.Rodzaj and s.Typ = ''ARK''
left join
(
select * from Przypisania prz where ((prz.IdPracownika = @pracId and IdCommodity = @typark) or (@pracId &lt; 0 and IdCommodity = @typark /*and prz.IdKierownika = @ObserverId*/))
union all
select * from Przypisania prz2 where @pracId &lt; 0 and prz2.IdPracownika = @ObserverId and dbo.GetRight(@observerId, 57)/*@tl*/ = 1
) prz on (dz.Data between prz.Od and ISNULL(prz.Do, ''20990909'') and prz.Status = 1)
left join PlanPracy pp on pp.IdPracownika = prz.IdPracownika and dz.Data = pp.Data
left join Absencja a on /*@pracId &gt; 0 and */a.IdPracownika = /*@pracId*/prz.IdPracownika and /*a.DataOd &lt; @do and a.DataDo &gt; @od --*/dz.Data between a.DataOd and a.DataDo
left join AbsencjaKody ak on a.Kod = ak.Kod


--outer apply (select case when ISNULL(ISNULL(pp.IdZmianyKorekta, pp.IdZmiany), -1) != -1 then 8 else 0 end as Nominal, CONVERT(float, ISNULL(pp.n50, 0) + ISNULL(pp.n100, 0))/3600 as Nadgodziny) ppoa --do zmiany n i s
--outer apply (select ppoa.Nominal - (ISNULL(CONVERT(float, pp.CzasZm), 0)/3600) as GodzNieob) pp2oa
left join Zmiany Z on Z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
--left join Pracownicy P on P.Id = @pracId
--left join OkresyRozl OK on OK.DataOd = @od
--left join PracownicyOkresy PO on PO.Id = @pracId and PO.IdOkresu = OK.Id 
--outer apply (select case when OK.Id is null then cast(P.EtatL as float) * 8 / P.EtatM else cast(PO.EtatL as float) * 8 / PO.EtatM end as Etat) E 
outer apply (select ISNULL(case when Z.Typ in (1,2,3) then 0 else case when Z.Od &gt; Z.Do then 24 else 0 end + (CAST(DATEDIFF(MINUTE, Z.Od, Z.Do) as float) / 60) end, 0) as Nominal, ISNULL(case when Z.Typ in (1,2,3) then 0 else cast(pp.CzasZm as float) end, 0) / 3600 as vCzasZm, CONVERT(float, ISNULL(pp.n50, 0) + ISNULL(pp.n100, 0))/3600 as Nadgodziny) ppoa 
outer apply (select case when ppoa.Nominal &gt; ppoa.vCzasZm then ppoa.Nominal - ppoa.vCzasZm else 0 end as GodzNieob) pp2oa
outer apply (select ppoa.Nominal + ppoa.Nadgodziny - pp2oa.GodzNieob as Godziny) goa
outer apply (select case when /*ak.Symbol is null*/ goa.Godziny &gt;= @kanapkaCzas then 1 else 0 end as Alive) as aoa

left join scParametry prod on prod.IdTypuArkuszy = @typark and (dz.Data between prod.Od and ISNULL(prod.Do, ''20990909'')) and prod.Typ = ''PROD'' and prod.TL = case when @pracId &lt; 0 then 0 else @tl end
left join scParametry qc on qc.IdTypuArkuszy = @typark and (dz.Data between qc.Od and ISNULL(qc.Do, ''20990909'')) and qc.Typ = ''QC'' and qc.TL = case when @pracId &lt; 0 then 0 else @tl end
left join scParametry s on s.IdTypuArkuszy = @typark and (dz.Data between s.Od and ISNULL(s.Do, ''20990909'')) and s.Typ = ''SANDWICH'' and s.TL = case when @pracId &lt; 0 then 0 else @tl end
left join scParametry tp on tp.IdTypuArkuszy = @typark and (dz.Data between tp.Od and ISNULL(s.Do, ''20990909'')) and tp.Typ = ''TLPRAC'' and tp.TL = case when @pracId &lt; 0 then 0 else @tl end and tp.Parametr2 = @pracId

--outer apply (select SUM(ISNULL(dn.CzasProd, 0)) as PracaInnyArkusz from scDni dn where dn.Data = dz.Data and d.IdTypuArkuszy != @typark and d.IdPracownika = @pracId) dnoa
outer apply (select SUM(PracaInnyArkusz) as CzasProd from scDni q where q.Data = dz.Data and q.IdPracownika = @pracId) poa
outer apply (select COUNT(*) as c from scWartosci w where w.Data = dz.Data and IdPracownika = @pracId and IdTypuArkuszy != @typark) pgiooa
outer apply (select case when goa.Godziny &gt;= ISNULL(tp.Parametr, 0) then ISNULL(tp.Parametr, 0) else 0 end TLPrac) tloa

group by d.Id, k.Data, dz.Data, ' + @colsH + ', k.Rodzaj, prod.Parametr, qc.Parametr, s.Parametr, tp.Parametr, tloa.TLPrac, /*ak.Symbol, ak.Nazwa,*/ d.CzasNieprod, d.PracaInnyArkusz, d.Korekta, d.IloscBledow, d.Uwagi
order by Date
'

exec sp_executesql @stmt

--select * from #c order by Nazwa

drop table #c
">
        <SelectParameters>
            <asp:ControlParameter Name="typark" ControlID="hidScorecardTypeId" PropertyName="Value"
                Type="String" />
            <asp:ControlParameter Name="pracId" ControlID="hidEmployeeId" PropertyName="Value"
                Type="String" />
            <asp:ControlParameter Name="date" ControlID="hidDate" PropertyName="Value" Type="String" />
            <asp:ControlParameter Name="oj" ControlID="hidOutsideJob" PropertyName="Value" Type="String" />
            <asp:ControlParameter Name="ObserverId" ControlID="hidObserverId" PropertyName="Value" Type="String" />
            <asp:ControlParameter Name="tl" ControlID="hidTeamLeader" PropertyName="Value" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>

<div class="hidRazemPremia">
    <asp:HiddenField ID="hidRazemPremia" runat="server" Value="5" /> 
</div>


<asp:SqlDataSource ID="dsRazemPremia" runat="server" SelectCommand="
declare @typark int = {0} --6
declare @pracId int = {1} --2166
declare @date datetime = {2} --'2014-08-01'
--declare @tl bit = {3} -- 0
declare @tl bit = case when @pracId &lt; 0 then {3} else case when {5} = 1 then 1 else {3} end end
--declare @ObserverId int = {4} -- 2329
declare @observerId int = case when @pracId &lt; 0 then {4} else case when {5} = 0 then {4} else @pracId end end

declare @diesel bit = (select top 1 case when @observerId = IdKierownika then 1 else 0 end as Cosmos from Przypisania where dbo.eom(@date) between Od and isnull(Do, '20990909') and IdPracownika = @pracId order by Od)
declare @kanapkaCzas as float = ISNULL(CONVERT(float, (select Kod from Kody where Typ = 'SCCZAS' and Aktywny = 1)) / 60, 6)
declare @nominalactual float

declare @_PREMIA as float
declare @_PROD as float
declare @_QC as float
declare @_ABS as float
declare @_PGIO as float

declare @p_prod as float
declare @p_qc as float
declare @p_abs as float

declare @v_prod as float
declare @v_qc as float
declare @v_abs as float
declare @v_pgio as float

declare @qc as int, @prod as int

declare @sum_err as float
declare @sum_count as float
declare @sum_pgio as float
declare @sum_sand as float
declare @sum_nominal as float
declare @sum_prodtime as float
declare @sum_prod as float

--bang

--ULTYMATYWNA METODA ROZWIAZUJACA WSZYSTKO ZWIAZANE Z WNIOSKAMI PREMIOWYMI
--UPDATE 1 DONE
--UPDATE 2 DONE
declare @wniosekId int
set @wniosekId = (select Id from scWnioski w where w.IdTypuArkuszy = @typark and w.Data = @date and w.IdPracownika = @observerId)
if @wniosekId is null begin --pierwsze uruchomienie systemu, powinno odpalic sie tylko raz ew. przy jakims purge'u danych

    declare @bilans int
    declare @oldwniosekId int
    declare @hajs int
    
    declare @hco int
    select @hco = HeadcountOverride from scWnioski where Id = @wniosekId

    select @bilans = ISNULL(BilansOtwarcia, 0), @oldwniosekId = Id from scWnioski where Data = DATEADD(m, -1, @date) and IdPracownika = @observerId and IdTypuArkuszy = @typark
    select @hajs = (select top 1 Parametr from scParametry p where @typark = p.IdTypuArkuszy and p.Typ = 'PREM' and DATEADD(m, -1, @date) between p.Od and ISNULL(p.Do, '20990909') and p.TL = 0)
    select @hajs = @hajs * ISNULL(@hco, (select COUNT(*) from scPremie where _do is null and IdWniosku = @oldwniosekId and IdPracownika != @observerId and ISNULL(Czas, ISNULL(CzasPracy, 0)) &gt; 0 and IdPracownika in
        (select IdPracownika from Przypisania where IdKierownika = @observerId and dbo.eom(DATEADD(m, -1, @date)) between Od and ISNULL(Do, '20990909'))))

    select @bilans = @bilans + @hajs - (select SUM(ISNULL(PremiaUznaniowa, 0)) from scPremie where IdWniosku = @oldwniosekId and _do is null)

	insert into scWnioski (IdTypuArkuszy, IdPracownika, Data, DataWyplaty, BilansOtwarcia, Status, Kacc, Pacc, DataUtworzenia) values (@typark, @observerId, @date, DATEADD(m, 1, @date), @bilans, 0, -1, -1, GETDATE())
	set @wniosekId = SCOPE_IDENTITY()
    --if @wniosekId is null set @wniosekId = @@IDENTITY
	end

select @qc = QC, @prod = Produktywnosc from scTypyArkuszy where Id = @typark
declare @ziomkip int = (select IloscPracownikow from scWnioski where Id = @wniosekId)
declare @od as datetime = CONVERT(datetime, Convert(varchar, YEAR(@date)) + '-' + convert(varchar, MONTH(@date)) + '-01' )
declare @do as datetime = DATEADD(D, -1, DATEADD(M,1,@od))

--REEEEEEEEEEEEEEEEEEEEE
select @nominalactual = SUM(Nominal) from dbo.GetDates2(@od, @do) dz
left join PlanPracy pp on pp.IdPracownika = @pracId and dz.Data = pp.Data
left join Zmiany Z on Z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
outer apply (select ISNULL(case when Z.Typ in (1,2,3) then 0 else case when Z.Od &gt; Z.Do then 24 else 0 end + (CAST(DATEDIFF(MINUTE, Z.Od, Z.Do) as float) / 60) end, 0) as Nominal, ISNULL(case when Z.Typ in (1,2,3) then 0 else cast(pp.CzasZm as float) end, 0) / 3600 as vCzasZm, CONVERT(float, ISNULL(pp.n50, 0) + ISNULL(pp.n100, 0))/3600 as Nadgodziny) ppoa

select @prod = case when case when @pracId &lt; 0 then 0 else @tl end = 1 then 9 else 11 end

select ak.Symbol, SUM(
	DATEDIFF(DAY,
		case when @od &lt; a.DataOd then a.DataOd else @od end,
		case when @do &gt; a.DataDo then a.DataDo else @do end
	) + 1
) as Ilosc into #abs from Absencja a
left join AbsencjaKody ak on ak.Kod = a.Kod
outer apply (select k.Parametr from Kody k where k.Aktywny = 1 and k.Typ = 'SCABSENCJE') oa
where a.IdPracownika = @pracId and a.DataOd &lt;= @do and a.DataDo &gt;= @od and ak.Kod in (select items from dbo.SplitInt(oa.Parametr, ','))
group by ak.Symbol

--NAJWAZNIEJSZE MIEJSCE W CALYM WZORZE: SUMY

select --top 31
	dz.Data as date
, ISNULL(d.IloscBledow, 0) as err
, ISNULL(d.PracaInnyArkusz, 0) as pgio
, ISNULL(d.CzasNieprod, 0) as CzasNieprod
, s.Parametr
, SUM(ISNULL(ppoa.Nominal, 0)) as nominal--ISNULL(ppoa.Nominal, 0) * COUNT(distinct prz.IdPracownika) as nominal
, ISNULL(case when SUM(ISNULL(ppoa.Nominal, 0)) = SUM(ISNULL(pp2oa.GodzNieob, 0)) then 0 + SUM(ISNULL(ppoa.Nadgodziny, 0)) - ISNULL(d.CzasNieprod, 0) - tloa.TLPrac else SUM(ISNULL(ppoa.Nominal, 0)) - SUM(ISNULL(pp2oa.GodzNieob, 0)) - ISNULL(d.PracaInnyArkusz, 0) - ISNULL(d.CzasNieprod, 0) - tloa.TLPrac + SUM(ISNULL(ppoa.Nadgodziny, 0)) end, 0) as prodtime
, ISNULL(case when SUM(ISNULL(ppoa.Nominal, 0)) = SUM(ISNULL(pp2oa.GodzNieob, 0)) then 0 + SUM(ISNULL(ppoa.Nadgodziny, 0)) - ISNULL(d.CzasNieprod, 0) - tloa.TLPrac - (ISNULL(s.Parametr, 0) * SUM(aoa.Alive)) else case when SUM(ISNULL(ppoa.Nominal, 0)) + SUM(ISNULL(ppoa.Nadgodziny, 0)) - SUM(ISNULL(pp2oa.GodzNieob, 0)) - ISNULL(d.PracaInnyArkusz, 0) - ISNULL(d.CzasNieprod, 0) - tloa.TLPrac - (ISNULL(s.Parametr, 0) * SUM(aoa.Alive)/*(COUNT(distinct prz.IdPracownika) + SUM(ISNULL(d.Korekta, 0)))*/) &lt; 0 then 0 else SUM(ISNULL(ppoa.Nominal, 0)) + SUM(ISNULL(ppoa.Nadgodziny, 0)) - SUM(ISNULL(pp2oa.GodzNieob, 0)) - ISNULL(d.PracaInnyArkusz, 0) - ISNULL(d.CzasNieprod, 0) - tloa.TLPrac - (ISNULL(s.Parametr, 0) * SUM(aoa.Alive)/*(COUNT(distinct prz.IdPracownika) + SUM(ISNULL(d.Korekta, 0)))*/) end end, 0) as sand
into #ccc
from dbo.GetDates2(@od, @do) dz
left join scDni d on dz.Data = d.Data and d.IdTypuArkuszy = @typark and d.IdPracownika = @pracId
left join
(
select * from Przypisania prz where ((prz.IdPracownika = @pracId and IdCommodity = @typark) or (@pracId &lt; 0 and IdCommodity = @typark /*and prz.IdKierownika = @ObserverId*/))
union all
select * from Przypisania prz2 where @pracId &lt; 0 and prz2.IdPracownika = @ObserverId and dbo.GetRight(@observerId, 57)/*@tl*/ = 1
) prz on (dz.Data between prz.Od and ISNULL(prz.Do, '20990909') and prz.Status = 1)
left join Absencja a on a.IdPracownika = prz.IdPracownika and /*a.DataOd &lt; @do and a.DataDo &gt; @od --*/dz.Data between a.DataOd and a.DataDo
left join AbsencjaKody ak on a.Kod = ak.Kod
left join PlanPracy pp on pp.IdPracownika = prz.IdPracownika and dz.Data = pp.Data
-- uwzglednic ETAT!!!
--outer apply (select case when ISNULL(ISNULL(pp.IdZmianyKorekta, pp.IdZmiany), -1) != -1 then 8 else 0 end as Nominal, CONVERT(float, ISNULL(pp.n50, 0) + ISNULL(pp.n100, 0))/3600 as Nadgodziny) ppoa --do zmiany n i s
--outer apply (select ppoa.Nominal - (ISNULL(CONVERT(float, pp.CzasZm), 0)/3600) as GodzNieob) pp2oa
left join Zmiany Z on Z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
outer apply (select ISNULL(case when Z.Typ in (1,2,3) then 0 else case when Z.Od &gt; Z.Do then 24 else 0 end + (CAST(DATEDIFF(MINUTE, Z.Od, Z.Do) as float) / 60) end, 0) as Nominal, ISNULL(case when Z.Typ in (1,2,3) then 0 else cast(pp.CzasZm as float) end, 0) / 3600 as vCzasZm, CONVERT(float, ISNULL(pp.n50, 0) + ISNULL(pp.n100, 0))/3600 as Nadgodziny) ppoa 
outer apply (select case when ppoa.Nominal &gt; ppoa.vCzasZm then ppoa.Nominal - ppoa.vCzasZm else 0 end as GodzNieob) pp2oa
outer apply (select ppoa.Nominal + ppoa.Nadgodziny - pp2oa.GodzNieob as Godziny) goa
outer apply (select case when /*ak.Symbol is null*/ goa.Godziny &gt;= @kanapkaCzas then 1 else 0 end as Alive) as aoa
left join scParametry s on s.IdTypuArkuszy = @typark and (dz.Data between s.Od and ISNULL(s.Do, '20990909')) and s.Typ = 'SANDWICH' and s.TL = case when @pracId &lt; 0 then 0 else @tl end
left join scParametry tp on tp.IdTypuArkuszy = @typark and (dz.Data between tp.Od and ISNULL(s.Do, '20990909')) and tp.Typ = 'TLPRAC' and tp.TL = case when @pracId &lt; 0 then 0 else @tl end and tp.Parametr2 = @pracId
outer apply (select case when goa.Godziny &gt;= ISNULL(tp.Parametr, 0) then ISNULL(tp.Parametr, 0) else 0 end TLPrac) tloa

group by dz.Data, d.IloscBledow, d.PracaInnyArkusz, d.CzasNieprod, s.Parametr, tloa.TLPrac, d.Korekta
order by dz.Data

select 
  @sum_err = serr
, @sum_pgio = spgio
, @sum_nominal = snominal
, @sum_prodtime = sprodtime
, @sum_sand = ssand
, @sum_count = count
, @sum_prod = prod
from
(
select 
	  ISNULL(SUM(D.err),0) as serr
	, ISNULL(SUM(D.pgio),0) as spgio
	, ISNULL(SUM(D.nominal),0) as snominal
	, ISNULL(SUM(D.prodtime),0) as sprodtime
	, ISNULL(SUM(D.sand),0) as ssand
from #ccc D
) D
outer apply (select 
	  ISNULL(SUM(wqc.Ilosc), 0) as count
	, ISNULL(SUM(w.Ilosc * c.Czas), 0) / 60 as prod
--from dbo.GetDates2(@od, @do) dz
--left join scWartosci w on dz.Data = w.Data and w.IdPracownika = @pracId and w.IdTypuArkuszy = @typark
from scWartosci w 
left join scCzynnosci c on c.Id = w.IdCzynnosci
left join scTypyArkuszyCzynnosci tac on tac.IdCzynnosci = w.IdCzynnosci and (w.Data between tac.Od and ISNULL(tac.Do, '20990909')) and tac.IdTypuArkuszy = @typark
left join scWartosci wqc on wqc.Id = w.Id and tac.QC = 1
where w.Data between @od and @do and w.IdPracownika = @pracId and w.IdTypuArkuszy = @typark
) S

select @p_prod = case when @sum_sand = 0 then 0 else @sum_prod / @sum_sand end
select @p_qc = case when @sum_count = 0 then 0 else 1 - (@sum_err / @sum_count) end
select @p_abs = ISNULL(SUM(Ilosc), 0) from #abs 
drop table #abs
drop table #ccc

--whereowanie bunnkrow

select top 1 @v_prod = Ile from scProduktywnosc p where p.IdTypuArkuszy = @typark and (@date between p.Od and ISNULL(p.Do, '20990909')) and p.OkresProbny = 0 and p.Rodzaj = @prod and TL = case when @pracId &lt; 0 then 0 else @tl end
	and DlaIlu &lt;= @p_prod order by DlaIlu desc

select top 1 @v_qc = Ile from scQC qc where qc.IdTypuArkuszy = @typark and (@date between qc.Od and ISNULL(qc.Do, '20990909')) and qc.Rodzaj = @qc and TL = case when @pracId &lt; 0 then 0 else @tl end
	and DlaIlu &lt;= @p_qc order by DlaIlu desc

select top 1 @v_abs = Ile from scAbsencje a where a.IdTypuArkuszy = @typark and (@date between a.Od and ISNULL(a.Do, '20990909')) and TL = case when @pracId &lt; 0 then 0 else @tl end
	and DlaIlu &lt;= @p_abs order by DlaIlu desc

select @v_pgio = Parametr from scParametry where IdTypuArkuszy = @typark and (@date between Od and ISNULL(Do, '20990909')) and Typ = 'PGIO' and TL = case when @pracId &lt; 0 then 0 else @tl end

--wyliczenie poszczegolnych kosmosow

select @_PROD = case when @sum_nominal = 0 then 0 else (@v_prod * case when @pracId &lt; 0 then ISNULL(@ziomkip, 1) else 1 end) / /*@sum_nominal*/isnull(nullif(@nominalactual, 0), @sum_nominal) * @sum_prodtime end
select @_QC = ROUND(@_PROD * (@v_qc - 1), 2)
select @_ABS = case when @v_abs &gt;= 1337 then @v_abs - 1337 else @_PROD * (@v_abs - 1) end
select @_PGIO = @sum_pgio * @v_pgio

--suma ze wszystkiego

--select
--  @sum_err
--, @sum_pgio
--, @sum_nominal
--, @sum_prodtime
--, @sum_sand
--, @sum_count
--, @sum_prod

--select @p_prod, @p_qc, @p_abs

--select @v_prod, @v_qc, @v_abs, @v_pgio

--select @_PROD, @_QC, @_ABS, @_PGIO

select @_PREMIA = ISNULL(@_PROD, 0) + ISNULL(@_QC, 0) + case when @pracId &lt; 0 or @diesel = 0 or @sum_prodtime = 0 then 0 else ISNULL(@_ABS, 0) end + ISNULL(@_PGIO, 0)

select @_PREMIA as Premia, @sum_prod as GodzProd, @sum_sand as CzasPracy, @sum_count as IloscSztuk, @sum_err as IloscBledow
" />

<asp:SqlDataSource ID="dsRequest" runat="server" SelectCommand="
declare @pracId int = {2}
declare @tl bit = case when @pracId &lt; 0 then {5} else case when {10} = 1 then 1 else {5} end end
declare @typark int = {0}
declare @date datetime = {1}

declare @observerId int = case when @pracId &lt; 0 then {3} else case when {10} = 0 then {3} else @pracId end end

declare @prem float = {4}
declare @prod float = {6}
declare @prac float = {7}
declare @amount int = {8}
declare @errors int = {9}

declare @sumsville float
declare @kanapka float
declare @pgio float

declare @kanapkaCzas as float = ISNULL(CONVERT(float, (select Kod from Kody where Typ = 'SCCZAS' and Aktywny = 1)) / 60, 6)

declare @od as datetime = dbo.bom(@date)
declare @do as datetime = dbo.eom(@date)


--torebka
--update scDni set Akceptacja = 1, DataAkceptacji = GETDATE(), IdAkceptujacego = @observerId where IdTypuArkuszy = @typark and Data = @date and IdPracownika = @pracId
--powyzsze chyba nawet do niczego nie sluzy

--ULTYMATYWNA METODA ROZWIAZUJACA WSZYSTKO ZWIAZANE Z WNIOSKAMI PREMIOWYMI
--UPDATE 1 DONE
declare @wniosekId int
set @wniosekId = (select Id from scWnioski w where w.IdTypuArkuszy = @typark and w.Data = @date and w.IdPracownika = @observerId)
if @wniosekId is null begin --pierwsze uruchomienie systemu, powinno odpalic sie tylko raz ew. przy jakims purge'u danych

    declare @bilans int
    declare @oldwniosekId int
    declare @hajs int

    select @bilans = ISNULL(BilansOtwarcia, 0), @oldwniosekId = Id from scWnioski where Data = DATEADD(m, -1, @date) and IdPracownika = @observerId and IdTypuArkuszy = @typark
    select @hajs = (select top 1 Parametr from scParametry p where @typark = p.IdTypuArkuszy and p.Typ = 'PREM' and DATEADD(m, -1, @date) between p.Od and ISNULL(p.Do, '20990909') and p.TL = 0)
    select @hajs = @hajs * (select COUNT(*) from scPremie where _do is null and IdWniosku = @oldwniosekId and IdPracownika != @observerId and ISNULL(Czas, ISNULL(CzasPracy, 0)) &gt; 0 and IdPracownika in
        (select IdPracownika from Przypisania where IdKierownika = @observerId and dbo.eom(DATEADD(m, -1, @date)) between Od and ISNULL(Do, '20990909')))

    select @bilans = @bilans + @hajs - (select SUM(ISNULL(PremiaUznaniowa, 0)) from scPremie where IdWniosku = @oldwniosekId and _do is null)

	insert into scWnioski (IdTypuArkuszy, IdPracownika, Data, DataWyplaty, BilansOtwarcia, Status, Kacc, Pacc, DataUtworzenia) values (@typark, @observerId, @date, DATEADD(m, 1, @date), @bilans, 0, -1, -1, GETDATE())
	set @wniosekId = SCOPE_IDENTITY()
    --if @wniosekId is null set @wniosekId = @@IDENTITY
	end

--if (select COUNT(*) from scPremie p where p.IdPracownika = @pracId and p.IdWniosku = @wniosekId) = 0
if (select COUNT(*) from scPremie where IdWniosku = @wniosekId and IdPracownika = @pracId) = 0
	insert into scPremie (/*Id, */IdPracownika, IdWniosku, _od, Akceptacja, PremiaMiesieczna, TL, GodzProd, CzasPracy, IloscSztuk, IloscBledow)
		values (/*CONVERT(int, RAND() * 10000),*/ @pracId, @wniosekId, GETDATE(), NULL, @prem, @tl, @prod, @prac, @amount, @errors)
		
		
		if @pracId &lt; 0 begin

			select @kanapka = ISNULL(s.Parametr, 0) from scParametry s where s.IdTypuArkuszy = @typark and (@date between s.Od and ISNULL(s.Do, '20990909')) and s.Typ = 'SANDWICH' and  s.TL = 0--@tl
		    
		    select
  p.Id
--, SUM(ppoa.Nominal) as nominal
, SUM(ppoa.Nominal + ppoa.Nadgodziny - pp2oa.GodzNieob - aoa.Alive * ISNULL(@kanapka, 0)) as kosmos
--, aoa.Alive
into #a
from --dbo.GetDates2(@od, @do) dz
/*left join*/ --Przypisania prz --on (dz.Data between prz.Od and ISNULL(prz.Do, '20990909') and prz.Status = 1)
(
select * from Przypisania prz where (IdCommodity = @typark /*and prz.IdKierownika = @ObserverId*/)
union all
select * from Przypisania prz2 where prz2.IdPracownika = @ObserverId and dbo.GetRight(@observerId, 57)/*@tl*/ = 1
) prz
left join Pracownicy p on p.Id = prz.IdPracownika
left join PlanPracy pp on pp.IdPracownika = prz.IdPracownika /*and dz.Data = pp.Data --*/and pp.Data between @od and @do
left join Absencja a on a.IdPracownika = prz.IdPracownika and pp.Data between a.DataOd and a.DataDo--a.DataOd &lt; @do and a.DataDo &gt; @od --dz.Data between a.DataOd and a.DataDo
left join AbsencjaKody ak on a.Kod = ak.Kod
--left join scDni d on d.IdPracownika = @pracId and d.Data between @od and @do
--outer apply (select case when ISNULL(ISNULL(pp.IdZmianyKorekta, pp.IdZmiany), -1) != -1 then 8 else 0 end as Nominal, CONVERT(float, ISNULL(pp.n50, 0) + ISNULL(pp.n100, 0))/3600 as Nadgodziny) ppoa --do zmiany n i s
--outer apply (select ppoa.Nominal - (ISNULL(CONVERT(float, pp.CzasZm), 0)/3600) as GodzNieob) pp2oa
left join Zmiany Z on Z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
outer apply (select ISNULL(case when Z.Typ in (1,2,3) then 0 else case when Z.Od &gt; Z.Do then 24 else 0 end + (CAST(DATEDIFF(MINUTE, Z.Od, Z.Do) as float) / 60) end, 0) as Nominal, ISNULL(cast(pp.CzasZm as float), 0) / 3600 as vCzasZm, CONVERT(float, ISNULL(pp.n50, 0) + ISNULL(pp.n100, 0))/3600 as Nadgodziny) ppoa 
outer apply (select case when ppoa.Nominal &gt; ppoa.vCzasZm then ppoa.Nominal - ppoa.vCzasZm else 0 end as GodzNieob) pp2oa
outer apply (select ppoa.Nominal + ppoa.Nadgodziny - pp2oa.GodzNieob as Godziny) goa
outer apply (select case when /*ak.Symbol is null*/ goa.Godziny &gt;= @kanapkaCzas then 1 else 0 end as Alive) as aoa
where /*(prz.IdKierownika = @observerId or (prz.IdPracownika = @observerId and @tl = 1)) and */prz.Status = 1 and @date between prz.Od and ISNULL(prz.Do, '20990909') /*and prz.IdCommodity = @typark*/
group by p.Id--, aoa.Alive

/*		    select
--  p.Id
dz.Data
--, SUM(ppoa.Nominal) as nominal
, SUM(ppoa.Nominal + ppoa.Nadgodziny - pp2oa.GodzNieob - aoa.Alive * ISNULL(@kanapka, 0)) - ISNULL(d.CzasNieprod, 0) - ISNULL(d.PracaInnyArkusz, 0) as kosmos
--, aoa.Alive
into #b
from dbo.GetDates2(@od, @do) dz
left join --Przypisania prz --on (dz.Data between prz.Od and ISNULL(prz.Do, '20990909') and prz.Status = 1)
(
select * from Przypisania prz where (IdCommodity = @typark and prz.IdKierownika = @ObserverId)
union all
select * from Przypisania prz2 where prz2.IdPracownika = @ObserverId and dbo.GetRight(@observerId, 57)/*@tl*/ = 1
) prz on prz.Status = 1 and dz.Data between prz.Od and ISNULL(prz.Do, '20990909')
left join Pracownicy p on p.Id = prz.IdPracownika
left join PlanPracy pp on pp.IdPracownika = prz.IdPracownika and dz.Data = pp.Data --and pp.Data between @od and @do
left join Absencja a on a.IdPracownika = prz.IdPracownika and pp.Data between a.DataOd and a.DataDo--a.DataOd &lt; @do and a.DataDo &gt; @od --dz.Data between a.DataOd and a.DataDo
left join AbsencjaKody ak on a.Kod = ak.Kod
left join scDni d on d.IdPracownika = @pracId and d.Data = dz.Data--between @od and @do
--outer apply (select case when ISNULL(ISNULL(pp.IdZmianyKorekta, pp.IdZmiany), -1) != -1 then 8 else 0 end as Nominal, CONVERT(float, ISNULL(pp.n50, 0) + ISNULL(pp.n100, 0))/3600 as Nadgodziny) ppoa --do zmiany n i s
--outer apply (select ppoa.Nominal - (ISNULL(CONVERT(float, pp.CzasZm), 0)/3600) as GodzNieob) pp2oa
left join Zmiany Z on Z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
outer apply (select ISNULL(case when Z.Typ in (1,2,3) then 0 else case when Z.Od &gt; Z.Do then 24 else 0 end + (CAST(DATEDIFF(MINUTE, Z.Od, Z.Do) as float) / 60) end, 0) as Nominal, ISNULL(case when Z.Typ in (1,2,3) then 0 else cast(pp.CzasZm as float) end, 0) / 3600 as vCzasZm, CONVERT(float, ISNULL(pp.n50, 0) + ISNULL(pp.n100, 0))/3600 as Nadgodziny) ppoa 
outer apply (select case when ppoa.Nominal &gt; ppoa.vCzasZm then ppoa.Nominal - ppoa.vCzasZm else 0 end as GodzNieob) pp2oa
outer apply (select ppoa.Nominal + ppoa.Nadgodziny - pp2oa.GodzNieob as Godziny) goa
outer apply (select case when /*ak.Symbol is null*/ goa.Godziny &gt;= @kanapkaCzas then 1 else 0 end as Alive) as aoa
--where /*(prz.IdKierownika = @observerId or (prz.IdPracownika = @observerId and @tl = 1)) and */prz.Status = 1 and @date between prz.Od and ISNULL(prz.Do, '20990909') /*and prz.IdCommodity = @typark*/
group by dz.Data, d.CzasNieprod, d.PracaInnyArkusz--p.Id--, aoa.Alive*/

--select @pgio = ISNULL(SUM(d.PracaInnyArkusz), 0) from scDni d where d.Data = @date and d.IdTypuArkuszy = @typark

select @sumsville = SUM(ISNULL(kosmos, 0)) /*- (COUNT(*) * @kanapka) - @pgio*/ from #a

--select @prem / @sumsville * kosmos from #a

declare @ziomkip int = (select IloscPracownikow from scWnioski where Id = @wniosekId)


--HORROR

declare @abs nvarchar(MAX)
select @abs = k.Parametr from Kody k where k.Aktywny = 1 and k.Typ = 'SCABSENCJE'

select
  prem.Id EmployeeId
, ROUND(aoa.AbsencjaKorekta, 0) as AbsencjaKorekta
into #c
from #a prem
outer apply
(
	select COUNT(*) as Ilosc from dbo.GetDates2(dbo.bom(@date), dbo.eom(@date)) d
	inner join Absencja a on d.Data between a.DataOd and a.DataDo and a.Kod in (select items from dbo.SplitInt(@abs, ',')) and a.IdPracownika = prem.Id
	inner join PlanPracy pp on pp.Data = d.Data and pp.IdPracownika = prem.Id and ISNULL(ISNULL(pp.IdZmianyKorekta, pp.IdZmiany), -1) != -1
	left join Zmiany z on z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
	where z.Typ not in (1, 2, 3)
) oa
outer apply
(
	select top 1 case when ISNULL(Ile, 1) &gt;= 1337 then ISNULL(Ile, 1)  - 1337 else (ISNULL(Ile, 1) - 1) * ISNULL((@prem) / @sumsville * ISNULL(kosmos, 0), 0) end as AbsencjaKorekta from scAbsencje a where a.IdTypuArkuszy = @typark and (@date between a.Od and ISNULL(a.Do, '20990909')) and TL = 0
		and DlaIlu &lt;= ISNULL(oa.Ilosc, 0) order by DlaIlu desc
)
aoa

--/HORROR

--SANITY CHECK
declare @sane bit
select @sane = case when (select COUNT(*) from scPremie where IdWniosku = @wniosekId and IdPracownika &gt; 0) = 0 then 1 else 0 end

if @sane = 1
insert into scPremie select /*CONVERT(int, RAND() * 10000) + Id,*/null, Id, @wniosekId, ISNULL((@prem /** ISNULL(@ziomkip, 1)*/) / @sumsville * ISNULL(kosmos, 0), 0), null, null, null, null, GETDATE(), null, null, @tl, null, null, null, null, null, kosmos, AbsencjaKorekta from #a a left join #c c on a.Id = c.EmployeeId

drop table #a
--drop table #b
drop table #c
		    
		    end
		
		
--	else update scPremie set PremiaMiesieczna = {4} where IdPracownika = @pracId and IdWniosku = @wniosekId
--NIE MA DEAKCEPTACJI (ANI W SYSTEMIE ANI TAKIEGO SLOWA)
" />

<asp:SqlDataSource ID="dsTeamLeader" runat="server" SelectCommand="
declare @date datetime = {0}
declare @pracId int = {1}
declare @typark int = {2}

declare @WPI float

declare @sanity bit = case when (select COUNT(*) from Przypisania where IdCommodity = @typark and IdKierownika = @pracId and @date between Od and ISNULL(Do, '20990909')) = 0 then 0 else 1 end

select @WPI = Parametr from /*Przypisania p
left join*/ scParametry par /*on p.IdCommodity = par.IdTypuArkuszy and @date between par.Od and ISNULL(par.Do, '20990909')*/
where /*p.IdKierownika = @pracId and @date between p.Od and ISNULL(p.Do, '20990909') and p.Status = 1 and*/ par.Typ = 'WPI' and par.TL = 1 and par.IdTypuArkuszy = @typark and par.Parametr2 = @pracId and par.Parametr2 = @pracId and @date between par.Od and ISNULL(par.Do, '20990909')

select case when ISNULL(@WPI, 0) &gt; 0 and @sanity = 1 then 1 else 0 end
" />


<asp:SqlDataSource ID="dsCanAccept" runat="server" SelectCommand="
declare @typark int = {0}
declare @pracId int = {1}
declare @date datetime = {2}
declare @ObserverId int = {3}
declare @tl bit = 0

declare @od datetime = dbo.bom(@date)
declare @do datetime = dbo.eom(@date)


declare @kanapkaCzas as float = ISNULL(CONVERT(float, (select Kod from Kody where Typ = 'SCCZAS' and Aktywny = 1)) / 60, 6)


select LEFT(CONVERT(nvarchar, d.Data, 20), 10) as Data from
(
	select d.Data, case when SUM(ISNULL(d.PracaInnyArkusz, 0)) &gt; 0 then 1 else 0 end as a, case when SUM(ISNULL(w.Ilosc, 0)) &gt; 0 then 1 else 0 end as b from scDni d
	left join scWartosci w on w.IdTypuArkuszy != @typark and w.IdPracownika = @pracId and w.Data = d.Data
	where d.IdTypuArkuszy = @typark and d.IdPracownika = @pracId and d.Data between @od and @do
	group by d.Data
) d where d.a != d.b

select LEFT(CONVERT(nvarchar, d.Data, 20), 10) as Data, d.CzasNieprod, d.Nominal from 
(
	select d.Data, d.CzasNieprod, SUM(goa.Godziny - (aoa.Alive * ISNULL(s.Parametr, 0))) - ISNULL(d.PracaInnyArkusz, 0) as Nominal from dbo.GetDates2(@od, @do) dz
	left join scDni d on dz.Data = d.Data and d.IdTypuArkuszy = @typark and d.IdPracownika = @pracId
	left join
	(
		select * from Przypisania prz where ((prz.IdPracownika = @pracId and IdCommodity = @typark) or (@pracId &lt; 0 and IdCommodity = @typark /*and prz.IdKierownika = @ObserverId*/))
		union all
		select * from Przypisania prz2 where @pracId &lt; 0 and prz2.IdPracownika = @ObserverId and dbo.GetRight(@observerId, 57)/*@tl*/ = 1
	) prz on (dz.Data between prz.Od and ISNULL(prz.Do, '20990909') and prz.Status = 1)
	left join PlanPracy pp on pp.IdPracownika = prz.IdPracownika and dz.Data = pp.Data
	left join Zmiany Z on Z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
	left join scParametry s on s.IdTypuArkuszy = @typark and (dz.Data between s.Od and ISNULL(s.Do, '20990909')) and s.Typ = 'SANDWICH' and s.TL = case when @pracId &lt; 0 then 0 else @tl end
	outer apply (select ISNULL(case when Z.Typ in (1,2,3) then 0 else case when Z.Od &gt; Z.Do then 24 else 0 end + (CAST(DATEDIFF(MINUTE, Z.Od, Z.Do) as float) / 60) end, 0) as Nominal, ISNULL(case when Z.Typ in (1,2,3) then 0 else cast(pp.CzasZm as float) end, 0) / 3600 as vCzasZm, CONVERT(float, ISNULL(pp.n50, 0) + ISNULL(pp.n100, 0))/3600 as Nadgodziny) ppoa
	outer apply (select case when ppoa.Nominal &gt; ppoa.vCzasZm then ppoa.Nominal - ppoa.vCzasZm else 0 end as GodzNieob) pp2oa
	outer apply (select ppoa.Nominal + ppoa.Nadgodziny - pp2oa.GodzNieob as Godziny) goa
    outer apply (select case when /*ak.Symbol is null*/ goa.Godziny &gt;= @kanapkaCzas then 1 else 0 end as Alive) as aoa
	group by d.Data, d.CzasNieprod, d.PracaInnyArkusz
) d
where d.CzasNieprod &gt; d.Nominal
" />