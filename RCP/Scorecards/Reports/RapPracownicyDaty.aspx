<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Scorecards/Report.Master" AutoEventWireup="true" CodeBehind="RapPracownicyDaty.aspx.cs" Inherits="HRRcp.Scorecards.Reports.RapPracownicyDaty" %>

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
                    Title="select 'Raport czynności - ' + case when @p4 > 0 then (select p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') from Pracownicy p where Id = @p4) else (select 'Zespół ' + ta.Nazwa from scTypyArkuszy ta where ta.Id = 0 - @p4) end + ' - ' + Nazwa from scCzynnosci where Id = @p1"
                    Title2="select 'Okres: ' + left(convert(varchar, '@p2', 20), 10) + ' - ' + left(convert(varchar, '@p3', 20), 10)"
                    DivClass="report_page RepCCUprawnienia"
                    Pager="false"
                    Charts="All"
                    SQL="
declare @od datetime = '@p2'
declare @do datetime = '@p3'

select
  LEFT(CONVERT(varchar, w.Data, 20), 10) as Data --[Data:C|charts`names]
, ISNULL(SUM(w.Ilosc), 0) as Ilosc --[Ilość:S] 
, isnull(sum(w.Ilosc * c.Czas), 0) as Czas --[Czas (min):S|charts`values]
from scWartosci w
left join scCzynnosci c on w.IdCzynnosci = c.Id
left join scTypyArkuszy ta on w.IdTypuArkuszy = ta.Id
left join scTypyArkuszyCzynnosci tac on tac.IdCzynnosci = w.IdCzynnosci and tac.IdTypuArkuszy = w.IdTypuArkuszy and tac.Od &lt;= @do and @od &lt;= ISNULL(tac.Do, '20990909')
left join Pracownicy p on p.Id = w.IdPracownika
where w.Data between @od and @do and c.Id = @p1 and w.IdPracownika = @p4
group by w.Data, w.IdPracownika, p.Nazwisko, p.Imie, p.KadryId, ta.Id, ta.Nazwa
" />
</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>