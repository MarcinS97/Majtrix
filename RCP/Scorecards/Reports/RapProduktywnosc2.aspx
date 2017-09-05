<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Scorecards/Report.Master" AutoEventWireup="true" CodeBehind="RapProduktywnosc2.aspx.cs" Inherits="HRRcp.Scorecards.Reports.RapProduktywnosc2" %>

<%@ Register Src="~/Controls/Reports/cntReport2.ascx" TagPrefix="leet" TagName="Report" %>
<%@ Register Src="~/Controls/EliteReports/cntFilter.ascx" TagPrefix="leet" TagName="Filter" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">


 <asp:HiddenField id="hidObserverId" runat="server" Visible="false" />
    
    <div class="report_page RepCCUprawnienia">
        <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <div class="filters printoff">
            <div class="filter">
                <asp:Label ID="lblDataOd" runat="server" Text="Data od: " />
                <uc1:DateEdit ID="deDateLeft" runat="server" />
            </div>
            <div class="filter">
                <asp:Label ID="Label1" runat="server" Text="Data do: " />
                <uc1:DateEdit ID="deDateRight" runat="server" />
            </div>
            <div class="filter">
                <asp:Label ID="Label2" runat="server" Text="Przełożony: " />
                <asp:DropDownList ID="ddlSuperiors" runat="server" DataValueField="Id" DataTextField="Name"
                    DataSourceID="dsSuperiors" />
                <asp:SqlDataSource ID="dsSuperiors" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="
/*select p.Id, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') as Name
from dbo.fn_GetTree2(@observerId, 1, GETDATE()) t
left join Pracownicy p on p.Id = t.IdPracownika where t.IdPracownika in (select distinct IdKierownika from Przypisania where IdCommodity is not null)*/

select 0 as Id, 'Wszyscy przełożeni' as Name, 0 as Sort where @ObserverId = 0
union all
select p.Id, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') as Name, 3 as Sort
from dbo.fn_GetTree2(@observerId, 1, GETDATE()) t
left join Pracownicy p on p.Id = t.IdPracownika where t.IdPracownika in (select distinct IdKierownika from Przypisania where IdCommodity is not null) order by Sort
">
                    <SelectParameters>  
                        <asp:ControlParameter Name="observerId" Type="Int32" ControlID="hidObserverId" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <div class="filter">
                <asp:Label ID="Label3" runat="server" Text="MPK M: " />
                <asp:DropDownList ID="ddlCC" runat="server" DataValueField="Id" DataTextField="Name" DataSourceID="dsCC" />
                <asp:SqlDataSource ID="dsCC" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select 0 as Id, 'wybierz ...' as Name, 0 as Sort union all select Id, cc as Name, 1 as Sort from CC order by Sort, Name desc" />
            </div>
            <asp:Button ID="btnFilter" runat="server" Text="Filtruj" OnClick="btnFilter_Click" CssClass="button100" />
            <asp:LinkButton ID="btnPrintAll" runat="server" CssClass="button75" Text="PDF" OnClick="PrintAll"><i class="fa fa-file-pdf-o"></i>Wydruk pracowników</asp:LinkButton>
            
            <%--<asp:Button ID="btnPrintAll" runat="server" Text="Wydruk" OnClick="PrintAll" CssClass="button100" />--%>
        </div>
        <leet:Report ID="Rep1" 
        runat="server" 
        CssClass="none" 
        DivClass="none"
        Pager="false"
            Charts="None" 
            Title="Produktywność pracowników"
            SQL="
--select * from dbo.GetProdQCAbs(19, 2175, 2356, '2015-09-01', '2015-09-11')
--declare @typark int = 19
--declare @pracId int = 2175
declare @observerId int = @SQL1
declare @date datetime = GETDATE()

declare @aparaty int = 7

declare @od as datetime = '@SQL2'
declare @do as datetime = '@SQL3'
declare @CC as int = @SQL4

select IdPracownika, SUM(Ilosc)/COUNT(Ilosc) Srednia into #aparaty from scWartosci where Data between @od and @do and IdCzynnosci = @aparaty and Ilosc is not null and Ilosc != 0
group by IdPracownika

/*select 
  pr.Id [pid:-]
, @observerId [przid:-]
, @od [pod:-]
, @do [pdo:-]
, pr.KadryId [Nr ewid.:C]
, (pr.Nazwisko + ' ' + pr.Imie) as [Pracownik|../Raport 16 @przid @pod @pdo @pid]
, (k.Nazwisko + ' ' + k.Imie) as [Przełożony]
, ta.Nazwa as  [Arkusz]
, aoe.sum_prod [Godz. prod.:NN2]
, aoe.sum_sand [Czas prod.:NN2]
, case when ta.Rodzaj = 4 then round(aoe.Prod * 100, 2) else round(aoe.Prod * 100, 2) end as [Produktywność (%):N]
, aoe.sum_count [Ilość:N]
, aoe.sum_err [Błędy:N]
, case when ta.Rodzaj = 4 then round(aoe.QC * 100, 2) else round(aoe2.QC * 100, 2) end as [FPY (%):N]
, aoe.Abs [Absencja (Dni):N]
from dbo.fn_GetSubPrzypisania(@ObserverId, @Date) p
left join Pracownicy pr on p.IdPracownika = pr.Id
left join Pracownicy k on k.Id = p.IdKierownika
left join scTypyArkuszy ta on ta.Id = p.IdCommodity
outer apply (select * from dbo.GetProdQCAbs(p.IdCommodity, pr.Id, @observerId, @od, @do)) aoe
outer apply (select * from dbo.GetProdQCAbs(p.IdCommodity, 0 - p.IdCommodity, @observerId, @od, @do)) aoe2
where IdCommodity is not null
order by pr.Nazwisko*/

select distinct
  case when ta.Rodzaj = 4 then pr.Id else 0 - p.IdCommodity end [pid:-]
, @observerId [przid:-]
, @od [pod:-]
, @do [pdo:-]
, pr.KadryId [Nr ewid.:C]
, ISNULL((pr.Nazwisko + ' ' + pr.Imie), 'Zespół ' + ta.Nazwa) as [Pracownik|../Raport 16 @przid @pod @pdo @pid]
, (k.Nazwisko + ' ' + k.Imie) as [Przełożony]
, ta.Nazwa as  [Arkusz]
, soa.cc [MPK M]
, aoe.sum_prod [Godz. prod.:NN2]
, aoe.sum_sand [Czas prod.:NN2]
, /*case when ta.Rodzaj = 4 then*/ round(aoe.Prod * 100, 2) /*else round(aoe.Prod * 100, 2) end*/ as [Produktywność (%):N]
, aoe.sum_count [Ilość:N]
, aoe.sum_err [Błędy:N]
, /*case when ta.Rodzaj = 4 then*/ round(aoe.QC * 100, 2) /*else round(aoe2.QC * 100, 2) end*/ as [FPY (%):N]
, aoe.Abs [Absencja (Dni):N]
, a.Srednia [Średnia ilość aparatów:N]
from dbo.fn_GetSubPrzypisania(@ObserverId, @Date) p
left join scTypyArkuszy ta on ta.Id = p.IdCommodity
left join Pracownicy pr on p.IdPracownika = pr.Id and ta.Rodzaj = 4
left join Pracownicy k on k.Id = p.IdKierownika
left join #aparaty a on a.IdPracownika = p.IdPracownika
outer apply (select * from dbo.GetProdQCAbs(p.IdCommodity, case when ta.Rodzaj = 4 then pr.Id else 0 - p.IdCommodity end, @observerId, @od, @do)) aoe
outer apply (select dbo.cat(CC.cc, ', ', 0) as cc from SplityWspP s left join CC on s.IdCC = CC.Id where IdPrzypisania = p.Id and pr.Id is not null) soa
outer apply (select top 1 IdCC from SplityWspP where IdPrzypisania = p.Id and pr.Id is not null) soa2
where IdCommodity is not null
and (soa2.IdCC = @CC or @CC = 0)

drop table #aparaty

" />
</ContentTemplate>
<Triggers>
    <asp:PostBackTrigger ControlID="btnPrintAll" />
</Triggers>
</asp:UpdatePanel>

</div>

  <leet:Report ID="RepPDF" 
        runat="server" 
        CssClass="break" 
        Pager="false"
            Charts="None" 
            Title="select 'Produktywność / QC - ' + case when @SQL4 > 0 then (select Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')', '') Name from Pracownicy where Id = @SQL4) else (select Nazwa from scTypyArkuszy where Id = 0 - @SQL4) end + ' ' + left(convert(varchar, '@SQL2', 20), 10) + ' - ' + left(convert(varchar, '@SQL3', 20), 10) "
            
                        
            
            FooterSql = "select 
case when @4 = 0 then 0 else @3 * 100 / @4 end [6], 
case when @7 = 0 then 0 else 100 - @8 * 100 / @7 end [10]"
            
            
            
            SQL="declare @observerId int = @SQL1
declare @date datetime = GETDATE()
declare @pracId int = @SQL4

declare @od as datetime = '@SQL2'
declare @do as datetime = '@SQL3'

select
  pr.Id [pid:-]
, LEFT(CONVERT(varchar, gd.Data, 20), 10) as [Data:C]
--, pr.KadryId [Nr ewid.:C]
--, (pr.Nazwisko + ' ' + pr.Imie) as [Pracownik]
--, (k.Nazwisko + ' ' + k.Imie) as [Przełożony]
, ta.Nazwa as  [Arkusz]
, aoe.sum_prod [Godz prod.:NN2S]
, aoe.sum_sand [Czas prod.:NN2S]
, case when aoe.sum_sand &gt; 0 then case when aoe.Prod &gt;= raosda.Parametr then '&lt;img src=''../Scorecards/images/Happy64px.png'' style=''border-width:0px; width: 16px;''&gt;' else '&lt;img src=''../Scorecards/images/Sad64px.png'' style=''border-width:0px; width: 16px;''&gt;' end else '&lt;img src=''../Scorecards/images/Skeleton64px.png'' style=''border-width:0px; width: 16px;''&gt;' end [ ]
, round(aoe.Prod * 100, 2) as [Produktywność (%):NN2]
, aoe.sum_count [Ilość:NS]
, aoe.sum_err [Błędy:NS]
, case when aoe.sum_count &gt; 0 then case when aoe.QC &gt;= raosda2.Parametr then '&lt;img src=''../Scorecards/images/Happy64px.png'' style=''border-width:0px; width: 16px;''&gt;' else '&lt;img src=''../Scorecards/images/Sad64px.png'' style=''border-width:0px; width: 16px;''&gt;' end else '&lt;img src=''../Scorecards/images/Skeleton64px.png'' style=''border-width:0px; width: 16px;''&gt;' end [  ]
, round(aoe.QC * 100, 2) as [FPY (%):NN2]
, aoe.Abs [Absencja (Dni):NS]
from GetDates2(@od, @do) gd
--left join dbo.fn_GetSubPrzypisania(@ObserverId, @Date) p on 
left join Pracownicy pr on /*p.IdPracownika*/@pracId = pr.Id
left join Przypisania p on p.IdPracownika = pr.Id and p.Status = 1 and @date between p.Od and ISNULL(p.Do, '20990909')
left join Pracownicy k on k.Id = p.IdKierownika
left join scTypyArkuszy ta on ta.Id = p.IdCommodity
left join scParametry raosda on raosda.IdTypuArkuszy = case when @pracId &lt; 0 then abs(@pracId) else p.IdCommodity end and raosda.Typ = 'PROD' and gd.Data between raosda.Od and ISNULL(raosda.Do, '20990909') and raosda.TL = isnull(dbo.GetRight(pr.Id, 57), 0)
left join scParametry raosda2 on raosda2.IdTypuArkuszy = case when @pracId &lt; 0 then abs(@pracId) else p.IdCommodity end and raosda2.Typ = 'QC' and gd.Data between raosda2.Od and ISNULL(raosda2.Do, '20990909') and raosda2.TL = isnull(dbo.GetRight(pr.Id, 57), 0)
--outer apply (select * from dbo.GetProdQCAbs(p.IdCommodity, pr.Id, @observerId, gd.Data , gd.Data)) aoe
outer apply (select * from dbo.GetProdQCAbs(case when @pracId &lt; 0 then 0 - @pracId else p.IdCommodity end, /*pr.Id*/@pracId, @observerId, gd.Data , gd.Data)) aoe
--where IdCommodity is not null
--order by pr.Nazwisko"
            Visible="false"
            />


<asp:SqlDataSource ID="dsAllEmployees" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
declare @observerId int = {0}
declare @date datetime = GETDATE()
declare @od as datetime = '{1}'
declare @do as datetime = '{2}'
declare @CC int = {3}

select distinct
  case when ta.Rodzaj = 4 then pr.Id else 0 - p.IdCommodity end [pid:-]
, k.Id as kid
from dbo.fn_GetSubPrzypisania(@ObserverId, @Date) p
left join scTypyArkuszy ta on ta.Id = p.IdCommodity
left join Pracownicy pr on p.IdPracownika = pr.Id and ta.Rodzaj = 4
left join Pracownicy k on k.Id = p.IdKierownika
outer apply (select * from dbo.GetProdQCAbs(p.IdCommodity, case when ta.Rodzaj = 4 then pr.Id else 0 - p.IdCommodity end, @observerId, @od, @do)) aoe
outer apply (select top 1 IdCC from SplityWspP where IdPrzypisania = p.Id and pr.Id is not null) soa2
where IdCommodity is not null 
and (soa2.IdCC = @CC or @CC = 0)"
/>


</asp:Content>
