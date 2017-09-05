<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntHarmonogram.ascx.cs" Inherits="HRRcp.RCP.Controls.Harmonogram.cntHarmonogram" %>

<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/RCP/Controls/Harmonogram/cntParametryPracownika.ascx" TagPrefix="uc1" TagName="cntParametryPracownika" %>
<%@ Register Src="~/RCP/Controls/Harmonogram/cntRightSummary.ascx" TagPrefix="uc1" TagName="cntRightSummary" %>
<%@ Register Src="~/RCP/Controls/Harmonogram/cntErrorsPanel.ascx" TagPrefix="uc1" TagName="cntErrorsPanel" %>



<input type="hidden" id="hidRPN" class="hidRPN" runat="server" />
<input type="hidden" id="hidDataOd" runat="server" class="hidDataOd" />
<input type="hidden" id="hidDataDo" runat="server" class="hidDataDo" />

<asp:HiddenField ID="hidIdKierownika" runat="server" Visible="false" />
<asp:HiddenField ID="hidKlasyfikacja" runat="server" Visible="false" />
<asp:HiddenField ID="hidCommodity" runat="server" Visible="false" />
<asp:HiddenField ID="hidIdStanowiska" runat="server" Visible="false" />
<asp:HiddenField ID="hidIdDzialu" runat="server" Visible="false" />

<asp:HiddenField ID="hidIdKlasyfikacji" runat="server" Visible="false" />
<asp:HiddenField ID="hidKlasyfikacje" runat="server" Visible="false" /> <%--MULTI SELECT--%>

<asp:HiddenField ID="hidSearch" runat="server" Visible="false" />
<%--<asp:HiddenField ID="hidApp" runat="server" Visible="false" />--%>

<input type="hidden" runat="server" id="hidApp" class="hidApp" />


<asp:HiddenField ID="hidEntities" runat="server" Visible="false" />

<input id="hidAllEmployees" type="hidden" runat="server" class="hidAllEmployees" />
<input id="hidSelectedEmployees" type="hidden" runat="server" class="hidSelectedEmployees" />
<input id="hidShift" type="hidden" runat="server" class="hidShifts" />

<input id="hidErrors" type="hidden" runat="server" class="hidErrors" />
<input id="hidInstantErrors" type="hidden" runat="server" class="hidInstantErrors" />

<asp:Button ID="btnTest" runat="server" OnClick="btnTest_Click" Text="TEST" CssClass="btn btn-primary" Visible="false" />

<input type="hidden" id="hidSchedule" runat="server" class="hidSchedule" />



<div id="divIfEmpty" runat="server" visible="false">
    <div class="well well-sm">
        Brak danych
    </div>
</div>

<div id="ctHarmonogram" runat="server" class="cntHarmonogram cntPlanPracy sortable">

    <div id="alert_placeholder" class="alert-placeholder">
    </div>
    <div class="table-scroller" id="divHeader" runat="server">
        <table class="tbHarmonogram tbHeader table">
            <tr>
                <th class="pracname">
                    <a href="#" class="pracname" data-toggle="tooltip" title="Zaznacz / odznacz wszystkich pracowników">Pracownik <asp:Label ID="lblEmployeeCount" runat="server" /></a>
                    <a href="#" class="col-opener" data-toggle="tooltip" title="Pokaż 7 dni z zeszłego miesiąca"><i class="glyphicon glyphicon-menu-left"></i></a>
                </th>
                <th id="thKod" runat="server" class="sortable" data-toggle="tooltip" title="Kod" data-container="body">K</th>
                <th id="thFunk" runat="server" data-toggle="tooltip" title="Funkcja" class="funk sortable" data-container="body">F</th>
                <asp:Repeater ID="rpHeader" runat="server">
                    <ItemTemplate>
                        <th class='<%# Eval("Class") %>' style='<%# Eval("Style")%>' runat="server" title='<%# Eval("Hint") %>'
                            visible='<%# Eval("Visible") %>' data-col-index='<%# Container.ItemIndex - BeforeDays %>' data-date='<%# Eval("Date") %>' data-type='<%# Eval("Type") %>'>
                            <%# Eval("Text") %>
                        </th>
                    </ItemTemplate>
                </asp:Repeater>
                <th id="thRSum" data-toggle="tooltip" data-container="body" title="Ilość zmian Rano" class="rsum sortable sum" runat="server" visible="true">R</th>
                <th id="thPSum" data-toggle="tooltip" data-container="body" class="sortable sum" title="Ilość zmian Popołudnie" runat="server" visible="true">P</th>
                <th id="thNSum" data-toggle="tooltip" data-container="body" class="sortable firstsum sum" title="Ilość zmian Noc">N</th>
                <th id="thNDSum" data-toggle="tooltip" data-container="body" class="sortable sum" title="Ilość niedziel pracujących" runat="server" visible="true">ND</th>
                <th id="th2" data-toggle="tooltip" class="sortable sum" data-container="body" title="Ilość niedziel pracujących w zeszłym miesiącu" runat="server">NP</th>
                <th id="th3" data-toggle="tooltip" class="xfirstsum sortable sum" data-container="body" title="Czas nominalny">NM</th>
                <th id="th4" data-toggle="tooltip" class="sortable sum" data-container="body" title="Czas zaplanowany">RS</th>
                <th id="th5" data-toggle="tooltip" class="sortable" data-container="body" title="Status">ST</th>
            </tr>
        </table>
    </div>
    <div class="table-scroller">
        <table class="tbHarmonogram tbData table">
            <asp:Repeater ID="rpData" runat="server" OnDataBinding="rpData_DataBinding" OnItemDataBound="rpData_ItemDataBound">
                <ItemTemplate>
                    <tr id="row" class='<%# "it data " + GetAdditionalEmployeeClass(QEval("FirmaSort")) %>' data-row-index='<%# Container.ItemIndex %>' data-employee-id='<%# QEval("Id") %>'
                        data-etat='<%# QEval("Etat") %>' data-rodzaj='<%# QEval("Funk") %>'>
                        <td id="tdPracName" class="pracname row-select" runat="server">
                            <asp:HiddenField ID="hidGhost" runat="server" Visible="false" Value='<%# QEval("Ghost") %>' />
                            <asp:HiddenField ID="hidEmployeeId" runat="server" Visible="false" Value='<%# QEval("Id") %>' />
                            <div class="pracname">

                                <div style="display: block;">
                                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="nazwisko" Text="" Enabled="true"
                                        OnClick="PracownikLabel_Click" CommandArgument='<%# QEval("Id") %>' ToolTip="Edytuj parametry pracownika" data-toggle="tooltip">
                                    <i class="fa fa-user"></i>
                                    </asp:LinkButton>
                                    <label class="name" title='<%# QEval("Prac") %>'><%# QEval("Prac") %></label>
                                    <a href="javascript:" class="time-machine pull-right" data-toggle="tooltip" data-container="body" title="Pokaż historię..."><i class="glyphicon glyphicon-time"></i></a>
                                </div>
                                <asp:Label ID="lbNrEw" runat="server" CssClass="line2 nrew" Text='<%# QEval("KadryId") %>' title='<%# QEval("Dzial") %>' data-toggle="tooltip" />
                            </div>
                        </td>
                        <td id="tdKod" runat="server" class="kod">
                            <a href="javascript:" class="kod" title='<%# QEval("KodPracOpis") %>' data-value='<%# QEval("KodPrac") %>'><%# QEval("KodPrac") %></a>
                        </td>
                        <td id="tdFunk" runat="server" class="funk">
                            <a href="javascript:" class="funk" title='<%# QEval("Funk") %>' data-value='<%# QEval("Funk") %>'><%# QEval("Funk") %></a>
                        </td>
                        <asp:Repeater ID="rpDays" runat="server">
                            <ItemTemplate>
                                <td id='<%# Container.ItemIndex %>' class='<%# Eval("Class") + GetStateClass(Eval("State")) %>' style='<%# Eval("Style")%>' data-id='<%# Eval("ShiftId") %>'
                                    data-col-index='<%# Container.ItemIndex - BeforeDays %>' data-name='<%# Eval("Text") %>' data-time='<%# Eval("Time") %>' data-container="body">
                                    <div class="inner-div">
                                        <span class="name"><%# Eval("Text") %></span>
                                        <span class="no time"><%# ((double)Eval("Time") <= 0) ? "" : Eval("Time")  %></span>
                                    </div>
                                </td>
                            </ItemTemplate>
                        </asp:Repeater>
                        <td class="sum rsum" runat="server" visible="true" id="tdRSum">
                            <asp:Label Text='<%# QEval("RSum") %>' runat="server" />
                        </td>
                        <td class="sum psum" runat="server" visible="true" id="tdPSum">
                            <asp:Label Text='<%# QEval("PSum") %>' runat="server" />
                        </td>
                        <td class="sum nsum firstsum">
                            <asp:Label Text='<%# QEval("NSum") %>' runat="server" />
                        </td>
                        <td class="sum ndsum" runat="server" visible="true" id="tdNDSum">
                            <asp:Label Text='<%# QEval("NDSum") %>' runat="server" />
                        </td>
                        <td id="tdNDLSum" runat="server" class="sum ndlsum">
                                <asp:Label Text='<%# QEval("NDLSum") %>' runat="server" />
                        </td>
                        <td id="tdCzasNom" runat="server" class="sum czasnom xfirstsum" data-value='<%# QEval("CzasNom") %>'>
                            <asp:Label Text='<%# QEval("CzasNom") %>' runat="server" />
                            <i id="okresIndicator" runat="server" visible='<%# !Convert.ToBoolean(QEval("IsOkres")) %>' class="fa fa-exclamation okres-not-exist blink-me"
                                data-toggle="tooltip" title="Brak zdefiniowanego okresu rozliczeniowego dla pracownika"></i>

                        </td>
                        <td id="tdCzasActual" runat="server" class='<%# "sum czasactual " + QEval("AllGood") %>'>
                            <asp:Label Text='<%# QEval("CzasActual") %>' runat="server" />
                        </td>
                        <td id="tdStatus" class="status" runat="server" visible="true" title='<%# QEval("StatusName") %>' data-toggle="tooltip" data-container="body">
                            <i class='<%# QEval("StatusClass") %>'></i>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
</div>

<uc1:cntRightSummary runat="server" ID="cntRightSummary" Visible="false" />
<uc1:cntErrorsPanel runat="server" id="cntErrorsPanel" Visible="false" />

<asp:SqlDataSource ID="dsPivot" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
--declare @kierId int = 355
--declare @kierId int = 355
--declare @od datetime = '20170501'
--declare @do datetime = dbo.eom(@od)
--declare @stan int
--declare @dzial int
--declare @com int
--declare @entities varchar(max)
--declare @klasyfikacje varchar(max)
--declare @klas int
--declare @search nvarchar(max)
--declare @app varchar(max) = 'VC'

declare @od3 datetime = dateadd(day, -7, @od)

declare @colsH nvarchar(MAX)
declare @colsD nvarchar(MAX)
declare @stmt nvarchar(MAX)

declare @nadzien datetime = GETDATE()

select 

  @colsH = isnull(@colsH + ',', '') + 'CONVERT(nvarchar(100)
, ISNULL([' + CONVERT(varchar(10), d.Data, 20) + '], '''')) [|' + CONVERT(varchar, DAY(d.Data)) + '&lt;br /&gt; ' + case dbo.dow(d.Data)
    when 0 then 'Pn'
    when 1 then 'Wt'
    when 2 then 'Śr'
    when 3 then 'Cz'
    when 4 then 'Pi'
    when 5 then 'So'
    when 6 then 'Ni'
    end + '|col-select sortable' 
    + case when k.Rodzaj = 0 then ' sobota' else '' end 
    + case when k.Rodzaj = 1 then ' niedziela' else '' end 
    + case when k.Rodzaj = 2 then ' swieto' else '' end 
    + case when k.Rodzaj in (0, 1, 2) then ' holiday' else '' end 
    + case when d.Data &lt; @od then ' before' else ' shift' end
    + case when convert(DATE, d.Data) = convert(DATE, GETDATE()) then ' today' else '' end
    + '|||' + convert(varchar(10), d.Data, 20) + '||' + isnull(convert(varchar, k.Rodzaj), '') + ']'
, @colsD = isnull(@colsD + ',', '') + '[' + CONVERT(varchar(10), d.Data, 20) + ']'
from dbo.getdates2(@od3, @do) d
left join Kalendarz k on k.Data = d.Data

--T
declare @order nvarchar(max) 
select @order = case @app 
	when 'VC' then 'order by p.Nazwisko, p.Imie'
	when 'VICIM' then 'order by p.Nazwisko, p.Imie'
	else 'order by [Dzial||||0], [FirmaSort||||0], p.Nazwisko, p.Imie'
	end

set @stmt = '
declare @kierId int = ' +isnull(CONVERT(varchar, @kierId), 'null') + '
declare @stanf int = ' + isnull(convert(varchar, @stan), 'null')  + '
declare @dzial int = ' + isnull(convert(varchar, @dzial), 'null')  + '
declare @com int = ' + isnull(convert(varchar, @com), 'null')  + '
/*declare @entities nvarchar(200) = ''' + isnull(convert(varchar, @entities) , 'null')  + '''*/

declare @entities nvarchar(max) = ' + ISNULL('''' + CONVERT(varchar, @entities) + '''', 'NULL') + '
declare @klasyfikacje nvarchar(max) = ' + ISNULL('''' + CONVERT(varchar, @klasyfikacje) + '''', 'NULL') + '

declare @klasf nvarchar(200) = ''' + isnull(convert(varchar, @klas), 'null')  + '''
declare @ssearch nvarchar(200) = ''' + isnull(convert(varchar, @search), 'null')  + '''
declare @nadzien datetime = ''' + CONVERT(varchar, @nadzien, 20) + '''
declare @od datetime = ''' + CONVERT(varchar, @od, 20) + '''
declare @do datetime = ''' + CONVERT(varchar, @do, 20) + '''
declare @od3 datetime = ''' + CONVERT(varchar, @od3, 20) + '''
declare @app varchar(50) = ''' + @app + '''

    ' + case @app when 'keeeper' then '

select
  p0.Id
into #zzz
from Pracownicy p0
inner join PracownicyStanowiska ps0 on ps0.IdPracownika = p0.Id and /*@nadzien*/@do between ps0.Od and isnull(ps0.Do, ''20990909'')
inner join
(
    select
        MIN(case when ps255.Klasyfikacja = ''keeeper'' then ''0'' else ''1'' end + p255.Nazwisko + '' '' + p255.Imie + ISNULL('' ('' + p255.KadryId + '')'', '''')) PRACOWNIK
    , ps255.IdDzialu
    from Pracownicy p255
    inner join PracownicyStanowiska ps255 on ps255.IdPracownika = p255.Id and /*@nadzien*/@do between ps255.Od and isnull(ps255.Do, ''20990909'')
    group by ps255.IdDzialu
) a on a.IdDzialu = ps0.IdDzialu and a.PRACOWNIK = case when ps0.Klasyfikacja = ''keeeper'' then ''0'' else ''1'' end + p0.Nazwisko + '' '' + p0.Imie + ISNULL('' ('' + p0.KadryId + '')'', '''')
where ps0.IdDzialu in (select Id from Dzialy where NazwaEN = ''CALC'' /*and Id != @com*/) and @entities is null --and @com in (select Id from Dzialy where NazwaEN = ''CALC'')

    ' when 'DBW' then '

select @kierId Id into #zzz

    ' else '

select -1 Id into #zzz
delete from #zzz

    ' end + '

select
  p.Id [Id||day|color: red !important;|0]
, p.Nazwisko + '' '' + p.Imie 
    /*
    + '' '' 
    + ISNULL(convert(varchar, ps.IdDzialu), ''null'') + ''-''
    + ISNULL(convert(varchar, @dzial),''null'') + ''.'' 
    + ISNULL(convert(varchar, ps.IdStanowiska), ''null'') + ''-''
    + ISNULL(convert(varchar, @stanf),''null'')
    */ 
    --+ @entities
    [Prac]
, p.KadryId + /*case when ps.Klasyfikacja = ''keeeper'' then '''' else*/ ISNULL('' '' + ps.Klasyfikacja, '''') /*end*/ [KadryId|KadryId|||0]
, oak.Kod [KodPrac|Kod]
, oak.Opis [KodPracOpis]
, ISNULL(prp.WymiarCzasu, (isnull(p.EtatL, 1) * 8) / (isnull(p.EtatM, 1))) [Etat||||0]
, ps.Rodzaj [Funk]
, ' + @colsH + '
, ISNULL(lj.R, 0) [RSum]
, ISNULL(lj.P, 0) [PSum]
, ISNULL(lj.N, 0) [NSum]
, ISNULL(lj.ND, 0) [NDSum]
, ISNULL(lj.NDL, 0) [NDLSum]
/*, isnull(ndls.c, 0) [NDLSum]*/

, ISNULL(cn.DniPrac, 0) * ISNULL(prp.WymiarCzasu / 3600, ISNULL(p.EtatL, 1) * 8 / ISNULL(EtatM, 1)) [CzasNom]

, /*(ISNULL(cn.DniPrac, 0) * ISNULL(prp.WymiarCzasu, ISNULL(p.EtatL, 1) * 8 / ISNULL(EtatM, 1))) -*/ ISNULL(lj.IZ2, 0) /*- lj.IZ * ISNULL(prp.WymiarCzasu / 3600, ISNULL(p.EtatL, 1) * 8 / ISNULL(EtatM, 1))*/ [CzasActual]
, hac.Nazwa + ISNULL('' ('' + ha.Uwagi + '')'', '''') [StatusName|Status|||||Status akceptacji]
, hac.Class [StatusClass]
, case when ((ISNULL(cn.DniPrac, 0) * ISNULL(prp.WymiarCzasu / 3600, ISNULL(p.EtatL, 1) * 8 / ISNULL(EtatM, 1))) = (lj.IZ2)) and (lj.IZ2 != 0) then ''all-good'' 
    else case when ((ISNULL(cn.DniPrac, 0) * ISNULL(prp.WymiarCzasu / 3600, ISNULL(p.EtatL, 1) * 8 / ISNULL(EtatM, 1))) &gt; (lj.IZ2)) then ''all-wrong'' else ''all-quite'' end end [AllGood||||0]
, case when p.Id in (select Id from #zzz) then -1 when ps.Klasyfikacja = ''keeeper'' then 0 else 1 end [FirmaSort||||0]
, ISNULL('' '' + d.Nazwa, '''') [Dzial||||0]

, case when orz.Id is not null then 1 else 0 end [IsOkres||||0]

, case when p.Id in (select Id from #zzz) then 1 else 0 end [Ghost||||0]
from Pracownicy p
inner join Przypisania r on 
    (
      r.IdPracownika = p.Id and r.Status = 1 and @od/*3*/ &lt; isnull(r.Do, ''20990909'') and r.Od &lt; @do
      and
      (
        (
          (r.IdKierownika = @kierId or @com is not null or @entities is not null) --- UWAGA UWAGA UWAGA Przypisania czy PracownicyStanowiska
          or
          (@kierId is null and @com is null and @ssearch is not null and @ssearch != ''null'')
          or (@klasyfikacje is not null)
        )
        or r.IdPracownika in (select Id from #zzz) and r.Status = 1 and @od/*3*/ &lt; isnull(r.Do, ''20990909'') and r.Od &lt; @do
      )
    )



    /*zmieniam dla keeepera*/

    ' + case @app when 'keeeper' then '

inner join PracownicyStanowiska ps on  --T:VC

    ' else '

left join PracownicyStanowiska ps on

    ' end + '


    (
        ps.IdPracownika = p.Id and /*@nadzien*/@do between ps.Od and isnull(ps.Do, ''20990909'')
		and
		(
		  (/*ps.IdDzialu = @com*/@entities is null or ps.IdDzialu in (select items from dbo.SplitInt(@entities, '','')) /*@com*/)
		  or ps.IdPracownika in (select Id from #zzz) and /*@nadzien*/@do between ps.Od and isnull(ps.Do, ''20990909'')
		)
    )' + '
left join Dzialy d on d.Id = ps.IdDzialu
/* outer apply ( select top 1 * from rcpHarmonogramAcc where IdPracownika = r.IdPracownika and Data = @Od order by DataUtworzenia desc) ha */  /* odcinamy sie od rcpHarmonogramAcc */
left join
(
	select
	  pp.IdPracownika
	/*, case when ha.Status = 1 then 1 else case when /*CHECK*/SUM/*_AGG(CHECKSUM*/(ISNULL(pp.IdZmiany    , -1)/*)*/)
	                                                = /*CHECK*/SUM/*_AGG(CHECKSUM*/(ISNULL(pp.IdZmianyPlan, -1)/*)*/) then 2 else 0 end end Status*/
    , case when ha.Status = 1 then 1 when ha.Status = -1 then -1 else case when dbo.cat(CONVERT(varchar, ISNULL(pp.IdZmiany    , -1)) + CONVERT(varchar, ISNULL(pp.Wymiar    , -1)), '' '', 0)
	                                              = dbo.cat(CONVERT(varchar, ISNULL(pp.IdZmianyPlan, -1)) + CONVERT(varchar, ISNULL(pp.WymiarPlan, -1)), '' '', 0) then 2 else 0 end end Status
    , ha.Uwagi
	from PlanPracy pp
	outer apply (select top 1 * from rcpHarmonogramAcc where IdPracownika = pp.IdPracownika and Data = @Od order by DataUtworzenia desc) ha
	where pp.Data between @od and @do
	group by pp.IdPracownika, ha.Status, ha.Uwagi
) ha on ha.IdPracownika = p.Id
left join rcpHarmonogramAccStatus hac on hac.Id = isnull(ha.Status, 0)
left join
(
    select * from
    (
        select
          pp.IdPracownika
        , CONVERT(varchar(10), pp.Data, 20) Data
        , z.Symbol + ''||background:'' + z.Kolor + '';|'' + convert(varchar, z.Id) + ''|'' + convert(varchar, isnull(pp.WymiarPlan /*20161031 HALLOWEEN UPDATE*/ / 3600, 8)) + ''|'' + case when r.Id is null then ''0'' else ''1'' end Symbol
        from PlanPracy pp
        left join Zmiany z on z.Id = pp.IdZmianyPlan
        left join Przypisania r on r.IdPracownika = pp.IdPracownika and r.Status = 1 and /*@nadzien*/@do between r.Od and ISNULL(r.Do, ''20990909'') and (r.IdKierownika = @kierId or @kierId is null)
        where pp.Data between @od3 and DATEADD(d, 1, @do)
    ) pp
    PIVOT
    (
        MAX(pp.Symbol) for pp.Data in (' + @colsD + ')
    ) pv
) t on t.IdPracownika = p.Id
left join
(
    select 
      pp.IdPracownika
    , SUM(case when pp.Data &gt;= @od and /*pp.IdZmianyPlan = /*188*//*179*/3*/ z.NazwaEN = ''Z1'' then 1 else 0 end) R
    , SUM(case when pp.Data &gt;= @od and /*pp.IdZmianyPlan = /*190*//*182*/5*/ z.NazwaEN = ''Z3'' then 1 else 0 end) N
    , SUM(case when pp.Data &gt;= @od and /*pp.IdZmianyPlan = /*189*//*181*/4*/ z.NazwaEN = ''Z2'' then 1 else 0 end) P
    , SUM(case when pp.Data &gt;= @od and pp.IdZmianyPlan is not null and k.Rodzaj = 1 then 1 else 0 end) ND
    , SUM(case when pp.Data &lt;  @od and pp.IdZmianyPlan is not null and k.Rodzaj = 1 then 1 else 0 end) NDL
    , SUM(case when pp.Data &gt;= @od and pp.IdZmianyPlan is not null then 1 else 0 end) IZ
    , SUM(case when pp.Data &gt;= @od and pp.IdZmianyPlan is not null then ISNULL(pp.WymiarPlan /*20161031 HALLOWEEN UPDATE*/ / 3600, ISNULL(prp.WymiarCzasu / 3600, ISNULL(p.EtatL, 1) * 8 / ISNULL(EtatM, 1))) else 0 end) IZ2
    from PlanPracy pp
    left join Zmiany z on z.Id = pp.IdZmianyPlan
    left join Kalendarz k on k.Data = pp.Data
    left join Pracownicy p on p.Id = pp.IdPracownika
    left join PracownicyParametry prp on prp.IdPracownika = p.Id and @do between prp.Od and ISNULL(prp.Do, ''20990909'')
    where pp.Data between dateadd(MONTH, -1, @od3) and isnull(dateadd(D, 0, @do), ''20990909'')
    group by pp.IdPracownika
) lj on lj.IdPracownika = r.IdPracownika
outer apply (select top 1 kpk.Kod, kpk.Opis from rcpKodyPrac kp left join rcpKodyPracKody kpk on kpk.Id = kp.Kod where IdPracownika = P.Id) oak
left join rcpPracownicyTypyOkresow pto on pto.IdPracownika = p.Id and @do between pto.DataOd and ISNULL(pto.DataDo, ''20990909'')
/*left join rcpOkresyRozliczenioweTypy ort on ort.Id = pto.IdTypuOkresu*/
--left join OkresyRozliczeniowe orz on orz.DataOd = @od and orz.Typ = pto.IdTypuOkresu

left join OkresyRozliczeniowe orz on @od between orz.DataOd and orz.DataDo and orz.Typ = pto.IdTypuOkresu

--left join CzasNom cn on cn.IdOkresu = orz.Id and cn.Data = @od

outer apply (select top 1 * from CzasNom where (orz.Id is null or IdOkresu = orz.Id) and Data = @od) cn      -- plomba !!! 



left join PracownicyParametry prp on prp.IdPracownika = p.Id and @do between prp.Od and ISNULL(prp.Do, ''20990909'')
/*left join dbo.rcp_sundays(dateadd(month, -1, @od)) ndls on ndls.IdPracownika = p.Id*/ /* tu jest REM */
where p.Status in (-1,0,1)
and (@stanf is null or ps.IdStanowiska = @stanf)
and (@dzial is null or ps.IdDzialu = @dzial)
and (@klasf = ''null'' or ps.Klasyfikacja = @klasf)
and (@ssearch = ''null'' or (P.Nazwisko + '' '' + p.Imie like ''%'' + @ssearch + ''%'' or P.Imie + '' '' + p.Nazwisko like ''%'' + @ssearch + ''%'' or P.KadryId like @ssearch)) 
						--and (@all = 1 or P.Id in (select IdPracownika from dbo.fn_GetTreeOkres(@UserId, @Od, @Do, @Do))))	
/*and (ps.IdDzialu = @com or @com is null)*/
and (@klasyfikacje is null or (@klasyfikacje = ''all'' and ps.klasyfikacja != ''keeeper'') or ps.Klasyfikacja in (select items from dbo.SplitStr(@klasyfikacje, '','')))

--order by [Dzial||||0], [FirmaSort||||0], p.Nazwisko, p.Imie
' + @order + '

drop table #zzz

'


exec sp_executesql @stmt">

    <SelectParameters>
        <asp:ControlParameter Name="kierId" Type="Int32" ControlID="hidIdKierownika" PropertyName="Value" />
        <asp:ControlParameter Name="od" Type="DateTime" ControlID="hidDataOd" PropertyName="Value" />
        <asp:ControlParameter Name="do" Type="DateTime" ControlID="hidDataDo" PropertyName="Value" />

        <asp:ControlParameter Name="com" Type="Int32" ControlID="hidCommodity" PropertyName="Value" />

        <asp:ControlParameter Name="stan" Type="Int32" ControlID="hidIdStanowiska" PropertyName="Value" DefaultValue="" ConvertEmptyStringToNull="true" />
        <asp:ControlParameter Name="dzial" Type="Int32" ControlID="hidIdDzialu" PropertyName="Value" DefaultValue="" ConvertEmptyStringToNull="true" />
        <asp:ControlParameter Name="klas" Type="String" ControlID="hidIdKlasyfikacji" PropertyName="Value" DefaultValue="" ConvertEmptyStringToNull="true" />

        <asp:ControlParameter Name="search" Type="String" ControlID="hidSearch" PropertyName="Value" DefaultValue="" ConvertEmptyStringToNull="true" />

        <asp:ControlParameter Name="app" Type="String" ControlID="hidApp" PropertyName="Value" DefaultValue="" ConvertEmptyStringToNull="true" />

        <asp:ControlParameter Name="entities" Type="String" ControlID="hidEntities" PropertyName="Value" DefaultValue="" ConvertEmptyStringToNull="true" />

        <asp:ControlParameter Name="klasyfikacje" Type="String" ControlID="hidKlasyfikacje" PropertyName="Value" DefaultValue="" ConvertEmptyStringToNull="true" />
     </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsSave" runat="server" SelectCommand="
declare @pracId int = {0}
declare @data datetime = {1}
declare @zmId int = {2}

declare @accId int = {3}
declare @nadzien datetime = GETDATE()

select
  @pracId IdPracownika
, @data Data
, @zmId IdZmiany
, @nadzien DataZm
, ISNULL(h.Id, 0) OriginalId
into #ccc
from (select 1 x) x
left join PlanPracy h on h.IdPracownika = @pracId and h.Data = @data
where ISNULL(@zmId, -1) != ISNULL(h.IdZmianyPlan, -1)

update PlanPracy set
  IdZmiany = @zmId
, DataZm = @nadzien
, IdKierownikaZm = @accId
from PlanPracy h
inner join #ccc a on a.OriginalId = h.Id

insert into PlanPracy (IdPracownika, Data, IdZmiany, DataZm, IdKierownikaZm, Akceptacja, DataAcc, IdKierownikaAcc)
select a.IdPracownika, a.Data, a.IdZmiany, a.DataZm, @accId, 0, a.DataZm, @accId
from (select 1 x) x
inner join #ccc a on a.OriginalId = 0

drop table #ccc    
" />

<asp:SqlDataSource ID="dsConditions" runat="server" SelectCommand="
select * from rcpWarunki where getdate() between DataOd and isnull(DataDo, '20990909') and Wymagany in (1, {0}) and Grupa = 'PP' order by Kolejnosc
" />


<asp:SqlDataSource ID="dsGetRPN" runat="server" SelectCommand="select dbo.cat(Id, ';', 0) from Zmiany where NazwaEN in ('Z1', 'Z2', 'Z3')" />
