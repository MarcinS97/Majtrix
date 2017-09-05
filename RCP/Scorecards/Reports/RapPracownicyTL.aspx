<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Scorecards/Report.Master" AutoEventWireup="true" CodeBehind="RapPracownicyTL.aspx.cs" Inherits="HRRcp.Scorecards.Reports.RapPracownicyTL" %>

<%@ Register Src="~/Controls/EliteReports/cntReport.ascx" TagPrefix="leet" TagName="Report" %>
<%@ Register Src="~/Controls/EliteReports/cntFilter.ascx" TagPrefix="leet" TagName="Filter" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">

    <asp:HiddenField id="hidObserverId" runat="server" Visible="false" />
    
      <%--<leet:Filter ID="fltrDate"
                    runat="server"
                    Type="DateEditRange"
                    Token="w.Data"
                    PostBackButtonId="btnFilter"
                    ReportId="Rep1"
                            />--%>
                            
<%--        <leet:Filter ID="fltrSuperior"
                    runat="server"
                    Type="DropDownList"
                    ValueField="Id"
                    TextField="Name"
                    Token="IdKierownika"
                    ChooseString="false"
                    ReportId="Rep1"
                    PostBackButtonId="btnFilter"
                    />--%>
    <div class="report_page RepCCUprawnienia">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

        <div class="filters">
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


select p.Id, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') as Name
from dbo.fn_GetTree2(@observerId, 1, GETDATE()) t
left join Pracownicy p on p.Id = t.IdPracownika where t.IdPracownika in (select distinct IdKierownika from Przypisania where IdCommodity is not null)">
                    <SelectParameters>
                        <asp:ControlParameter Name="observerId" Type="Int32" ControlID="hidObserverId" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>
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
            SQL="
declare @observerId int = @SQL1
declare @dataod datetime = '@SQL2'
declare @datado datetime = '@SQL3'


select
  p1.Nazwisko + ' ' + p1.Imie + ISNULL(' (' + p1.KadryId + ')', '') --[Pracownik:C]
, p2.Nazwisko + ' ' + p2.Imie + ISNULL(' (' + p2.KadryId + ')', '') --[Kierownik]
, ta.Nazwa --[Arkusz]
, convert(varchar, case when prem.CzasPracy &gt; 0 then round((prem.GodzProd / prem.CzasPracy), 2) * 100 else 0 end) as Produktywnosc --[Produktywność (%):M]
, convert(varchar, case when prem.IloscSztuk &gt; 0 then (convert(float, (prem.IloscSztuk - prem.IloscBledow)) / prem.IloscSztuk) * 100 else 0 end) as FPY --[FPY (%):M]
from scWnioski w
left join scTypyArkuszy ta on ta.Id = w.IdTypuArkuszy
left join scPremie prem on w.Id = prem.IdWniosku and prem._do is null and ((ta.Rodzaj = 4) or (ta.Rodzaj = 5 and prem.IdPracownika &lt; 0))
left join Przypisania prz on w.Data between prz.Od and ISNULL(prz.Do, '20990909') and /*prz.IdCommodity = w.IdTypuArkuszy and*/ ((prem.IdPracownika &gt; 0 and prz.IdPracownika = prem.IdPracownika) or (prem.IdPracownika &lt; 0 and prz.IdCommodity = 0 - prem.IdPracownika))
left join Pracownicy p1 on p1.Id = prz.IdPracownika
left join Pracownicy p2 on p2.Id = prz.IdKierownika
where w.Data between @dataOd and @dataDo and prz.IdPracownika in (select IdPracownika from dbo.fn_GetSubPrzypisania(@observerId, GETDATE())) and (ta.Rodzaj = 4 or ta.Rodzaj = 5)
" />
</ContentTemplate>
</asp:UpdatePanel>

</div>


</asp:Content>
