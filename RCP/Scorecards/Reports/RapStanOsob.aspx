<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Scorecards/Report.Master" AutoEventWireup="true" CodeBehind="RapStanOsob.aspx.cs" Inherits="HRRcp.Scorecards.Reports.RapStanOsob" %>

<%@ Register Src="~/Controls/EliteReports/cntReport.ascx" TagPrefix="leet" TagName="Report" %>
<%@ Register Src="~/Controls/EliteReports/cntFilter.ascx" TagPrefix="leet" TagName="Filter" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
    <div class="report_page RepCCUprawnienia">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

    <asp:HiddenField id="hidObserverId" runat="server" Visible="false" />
    <div class="filters">
         <div class="filter">
                <asp:Label ID="lblData" runat="server" Text="Data: " />
                <uc1:DateEdit ID="deDate" runat="server" />
            </div>
            <div class="filter">
                <asp:Label ID="Label2" runat="server" Text="Przełożony: " />
                <asp:DropDownList ID="ddlSuperiors" runat="server" DataValueField="Id" DataTextField="Name"
                    DataSourceID="dsSuperiors" />
                <asp:SqlDataSource ID="dsSuperiors" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="
select 0 as Id, 'Wszyscy przełożeni' as Name, 0 as Sort where @ObserverId = 0
union all
select p.Id, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') as Name, 1 as Sort
from dbo.fn_GetTree2(@observerId, 1, GETDATE()) t
left join Pracownicy p on p.Id = t.IdPracownika where t.IdPracownika in (select distinct IdKierownika from Przypisania where IdCommodity is not null)">
                    <SelectParameters>
                        <asp:ControlParameter Name="observerId" Type="Int32" ControlID="hidObserverId" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <div class="filter">
                <asp:Label ID="Label3" runat="server" Text="MPK Macierzyste: " />
                <asp:DropDownList ID="ddlCC" runat="server" DataValueField="Id" DataTextField="Name" DataSourceID="dsCC" />
                <asp:SqlDataSource ID="dsCC" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select 0 as Id, 'wybierz ...' as Name, 0 as Sort union all select Id, cc as Name, 1 as Sort from CC order by Sort, Name desc" />
            </div>
            <asp:Button ID="btnFilter" runat="server" Text="Filtruj" OnClick="btnFilter_Click" CssClass="button100" />
    </div>
    
     <leet:Report ID="Rep1" 
        runat="server" 
        CssClass="none" 
        DivClass="none" 
        Pager="false"
            Charts="None" 
            Title="Pracownicy"
            SQL=
"
declare @observerId int = @SQL1
declare @date datetime = '@SQL2'
declare @ccId int = @SQL3

select
p.KadryId --[Nr Ewid.:C]
, p.Nazwisko + ' ' + p.Imie as Pracownik --[Pracownik]
, ISNULL(K.Nazwisko + ' ' + K.Imie + isnull(' (' + k.KadryId + ')', ''), ' Główny poziom struktury') as [Przełożony] --[Przełożony]
, d.Nazwa as Dzial --[Dział]
, S.Nazwa as Stanowisko --[Stanowisko] 
, HR_DB.dbo.fn_GetPracLastCC(prz.IdPracownika, @date, 1, ',') as CC --[MPK M]
, convert(varchar(10),p.DataZatr,20) --[Data zatrudnienia]
, convert(varchar(10),p.DataZwol, 20) --[Data zwolnienia]
, convert(varchar(10),p.OkresProbnyDo,20) --[Okres próbny do]
, case P.Status 
when -2 then 'Pomiń'
when -1 then 'Zwolniony'
when 0 then 'Zatrudniony'
when 1 then 'Nowy'
end as Status --[Status]
, TA.Nazwa as Arkusz --[Arkusz]
--,R.IdCommodity
--,* 
, cast(p.EtatL as float) / p.EtatM as Etat --[Etat]
from dbo.fn_GetSubPrzypisania(@observerId, GETDATE()) prz
left join Pracownicy p on p.Id = prz.IdPracownika
left join Pracownicy K on K.Id = prz.IdKierownika
left join PracownicyStanowiska ps on ps.IdPracownika = p.Id and @date between ps.Od and ISNULL(ps.Do, '20990910')
left join Dzialy d on d.Id = ps.IdDzialu
left join Stanowiska S on S.Id = ps.IdStanowiska
left join scTypyArkuszy TA on TA.Id = prz.IdCommodity
outer apply (select top 1 IdCC from SplityWspP where IdPrzypisania = prz.Id) soa2
where ISNULL(p.DataZwol,'20990909') &gt; @date and p.Status != -2 and (soa2.IdCC = @ccId or @ccId = 0) and prz.IdCommodity is not null
order by p.Nazwisko
" />






        </ContentTemplate>
    </asp:UpdatePanel>
</div>

</asp:Content>
