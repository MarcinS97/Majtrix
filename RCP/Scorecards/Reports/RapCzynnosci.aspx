<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Scorecards/Report.Master" AutoEventWireup="true" CodeBehind="RapCzynnosci.aspx.cs" Inherits="HRRcp.Scorecards.Reports.RapCzynnosci" %>

<%@ Register Src="~/Controls/EliteReports/cntReport.ascx" TagPrefix="leet" TagName="Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
       <leet:Report
                    ID="Rep1"
                    runat="server"
                    CssClass="none"
                    Title="Czynności"
                    DivClass="report_page RepCCUprawnienia"
                    Pager="false"
                    Charts="All"
                    SQL="
declare @ccId int = @p1


select  c.Id, --[cId:-]
        c.Nazwa, --[Czynność:C|charts`names]
		count(w.Id) as Ilosc, --[Ilość:S] 
        isnull(sum(w.Ilosc), 0) as Czas --[Czas (min):S|charts`values]
from scCzynnosci c
left join scWartosci w on w.IdCzynnosci = c.Id 
where c.Aktywny = 1 and c.IdCC = @ccId
group by c.Id, c.Nazwa
"
 />
 </ContentTemplate>
 </asp:UpdatePanel>
<%-- 
 |RapPracownicy @cId 
|RapPracownicy @cId|charts`values
--%>
</asp:Content>
