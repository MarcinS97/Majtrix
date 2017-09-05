<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntRightSummary.ascx.cs" Inherits="HRRcp.RCP.Controls.Harmonogram.cntRightSummary" %>

<asp:HiddenField ID="hidDataOd" runat="server" Visible="false" />
<asp:HiddenField ID="hidDataDo" runat="server" Visible="false" />
<asp:HiddenField ID="hidIdDzialu" runat="server" Visible="false" />

<asp:HiddenField ID="hidEntities" runat="server" Visible="false" />

<div id="rightSummary" runat="server" class="right-summary xxxh420 side-popout">
    <i class="popout-trigger fa fa-calculator"></i>
    <table class="tbSummary">
        <tr>
            <th colspan="4" style="padding-bottom: 0;">
                <h4>
                    <i class="glyphicon glyphicon-th-list"></i>

                    <asp:Label ID="lblRightSummaryTitle" runat="server"></asp:Label>
                </h4>

            </th>
        </tr>
        <asp:Repeater ID="rpSummary" runat="server" DataSourceID="dsRightSummary">
            <HeaderTemplate>
                <tr>
                    <th>Dzień</th>
                    <%--<th title="Ilość osób na zmianie R">R</th>--%>
                    <th title="Ilość osób na zmianie R (bez funkcyjnych)">RO</th>
                    <%--<th title="Ilość osób na zmianie P">P</th>--%>
                    <th title="Ilość osób na zmianie P (bez funkcyjnych)">PO</th>
                    <%--<th title="Ilość osób na zmianie N">N</th>--%>
                    <th title="Ilość osób na zmianie N (bez funkcyjnych)">NO</th>
                </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr data-row-index='<%# Container.ItemIndex %>'>
                    <td class="day">
                        <asp:Label ID="lblDay" runat="server" Text='<%# Eval("DayName") %>' />
                    </td>
                    <%--<td class="rsum" data-value='<%# Eval("R") %>'>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("R") %>' />
                    </td>--%>
                    <td class="r2sum" data-value='<%# Eval("R0") %>' style='<%# "background-color: " + Eval("RColor")  %>' >
                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("R0") %>' />
                    </td>
                    <%--<td class="psum" data-value='<%# Eval("P") %>'>
                        <asp:Label ID="Label4" runat="server" Text='<%# Eval("P") %>' />
                    </td>--%>
                    <td class="p2sum" data-value='<%# Eval("P0") %>' style='<%# "background-color: " + Eval("PColor")  %>'>
                        <asp:Label ID="Label5" runat="server" Text='<%# Eval("P0") %>' />
                    </td>
                    <%--<td class="nsum" data-value='<%# Eval("N") %>'>
                        <asp:Label ID="Label6" runat="server" Text='<%# Eval("N") %>' />
                    </td>--%>
                    <td class="n2sum" data-value='<%# Eval("N0") %>' style='<%# "background-color: " + Eval("NColor")  %>'>
                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("N0") %>' />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <asp:SqlDataSource ID="dsRightSummary" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
        SelectCommand="
declare @nadzien datetime = GETDATE()

--declare @od datetime = dbo.bom(@nadzien)
--declare @do datetime = dbo.eom(@nadzien)

declare @colsH nvarchar(MAX)
declare @colsD nvarchar(MAX)
declare @stmt nvarchar(MAX)
    
set @entities = nullif(@entities, '')
declare @isCalc bit
set @isCalc = (select case when count(*) > 0 then 1 else 0 end from dbo.SplitInt(@entities, ',') inner join Dzialy d on d.Id = items and d.NazwaEN = 'CALC')


select
  CONVERT(varchar, DAY(d.Data)) 
  + ' - ' +
  case dbo.dow(d.Data)
    when 0 then 'Pn'
    when 1 then 'Wt'
    when 2 then 'Śr'
    when 3 then 'Cz'
    when 4 then 'Pi'
    when 5 then 'So'
    when 6 then 'Ni'
  end  
  DayName
, isnull(lj.R, 0) R
, isnull(lj.R2, 0) R0
, isnull(lj.P, 0) P
, isnull(lj.P2, 0) P0 
, isnull(lj.N, 0) N 
, isnull(lj.N2, 0) N0
, (select top 1 Kolor from Zmiany where Id = 188) RColor
, (select top 1 Kolor from Zmiany where Id = 189) PColor
, (select top 1 Kolor from Zmiany where Id = 190) NColor  
from dbo.getdates2(@od, @do) d
left join
(
    select
      pp.Data
    , SUM(case when pp.IdZmianyPlan = 188 then 1 else 0 end) R
    , SUM(case when pp.IdZmianyPlan = 188 and ps.Rodzaj is null then 1 else 0 end) R2
    , SUM(case when pp.IdZmianyPlan = 189 then 1 else 0 end) P
    , SUM(case when pp.IdZmianyPlan = 189 and ps.Rodzaj is null then 1 else 0 end) P2
    , SUM(case when pp.IdZmianyPlan = 190 then 1 else 0 end) N
    , SUM(case when pp.IdZmianyPlan = 190 and ps.Rodzaj is null then 1 else 0 end) N2
    from PlanPracy pp
    inner join Przypisania r on r.IdPracownika = pp.IdPracownika and r.Status = 1 and pp.Data between r.Od and ISNULL(r.Do, '20990909')
    left join PracownicyStanowiska ps on ps.IdPracownika = pp.IdPracownika and pp.Data between ps.Od and ISNULL(ps.Do, '20990909')
	inner join Dzialy d on d.Id = ps.IdDzialu and (@isCalc = 1 and d.NazwaEN = 'CALC' and (d.Id not in (select items from dbo.SplitInt(@entities, ','))) or @entities is null)
        --and d.NazwaEN = 'CALC' and (d.Id not in (select items from dbo.SplitInt(@entities, ',')))  --and d.Id != @dzialId
    left join Zmiany z on z.Id = pp.IdZmianyPlan  
    where pp.Data between @od and @do
    group by pp.Data
) lj on lj.Data = d.Data
">
        <SelectParameters>
            <asp:ControlParameter Name="od" Type="DateTime" ControlID="hidDataOd" PropertyName="Value" />
            <asp:ControlParameter Name="do" Type="DateTime" ControlID="hidDataDo" PropertyName="Value" />
            <%--<asp:ControlParameter Name="dzialId" Type="Int32" ControlID="hidIdDzialu" PropertyName="Value" />--%>
            <asp:ControlParameter Name="entities" Type="String" ControlID="hidEntities" PropertyName="Value" />
        </SelectParameters>
    </asp:SqlDataSource>

<%--    <asp:SqlDataSource ID="dsRightSummary" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
        SelectCommand="
declare @nadzien datetime = GETDATE()

--declare @od datetime = dbo.bom(@nadzien)
--declare @do datetime = dbo.eom(@nadzien)

declare @colsH nvarchar(MAX)
declare @colsD nvarchar(MAX)
declare @stmt nvarchar(MAX)

--declare @com int = 3
--declare @kierId int = 11

select
  CONVERT(varchar, DAY(d.Data)) 
  + ' - ' +
  case dbo.dow(d.Data)
    when 0 then 'Pn'
    when 1 then 'Wt'
    when 2 then 'Śr'
    when 3 then 'Cz'
    when 4 then 'Pi'
    when 5 then 'So'
    when 6 then 'Ni'
    end  
            Day
, isnull(lj.R, 0) R
, isnull(lj.R2, 0) R2
, isnull(lj.P, 0) P
, isnull(lj.P2, 0) P2 
, isnull(lj.N, 0) N 
, isnull(lj.N2, 0) N2
from dbo.getdates2(@od, @do) d
left join
(
    select
      pp.Data
    , SUM(case when pp.IdZmianyPlan = 188 then 1 else 0 end) R
    , SUM(case when pp.IdZmianyPlan = 188 and ps.Rodzaj is not null then 1 else 0 end) R2
    , SUM(case when pp.IdZmianyPlan = 189 then 1 else 0 end) P
    , SUM(case when pp.IdZmianyPlan = 189 and ps.Rodzaj is not null then 1 else 0 end) P2
    , SUM(case when pp.IdZmianyPlan = 190 then 1 else 0 end) N
    , SUM(case when pp.IdZmianyPlan = 190 and ps.Rodzaj is not null then 1 else 0 end) N2
    from PlanPracy pp
    inner join Przypisania r on r.IdPracownika = pp.IdPracownika and r.Status = 1 and pp.Data between r.Od and ISNULL(r.Do, '20990909') and (r.IdKierownika = @kierId or r.IdCommodity = @com) --r.IdKierownika = @kierId
    left join PracownicyStanowiska ps on ps.IdPracownika = pp.IdPracownika and pp.Data between ps.Od and ISNULL(ps.Do, '20990909')
    where pp.Data between @od and @do
    and (@stan is null or ps.IdStanowiska = @stan)
    and (@klas is null or ps.Klasyfikacja = @klas)

    group by pp.Data
) lj on lj.Data = d.Data
                
">
        <SelectParameters>
            <asp:ControlParameter Name="kierId" Type="Int32" ControlID="hidIdKierownika" PropertyName="Value" />
            <asp:ControlParameter Name="od" Type="DateTime" ControlID="hidDataOd" PropertyName="Value" />
            <asp:ControlParameter Name="do" Type="DateTime" ControlID="hidDataDo" PropertyName="Value" />

            <asp:ControlParameter Name="stan" Type="Int32" ControlID="hidIdStanowiska" PropertyName="Value" DefaultValue="" ConvertEmptyStringToNull="true" />
            <asp:ControlParameter Name="klas" Type="String" ControlID="hidIdKlasyfikacji" PropertyName="Value" DefaultValue="" ConvertEmptyStringToNull="true" />

            <asp:ControlParameter Name="com" Type="Int32" ControlID="hidCommodity" PropertyName="Value" />
        </SelectParameters>
    </asp:SqlDataSource>--%>

</div>