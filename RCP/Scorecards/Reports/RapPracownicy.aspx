<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Scorecards/Report.Master" AutoEventWireup="true" CodeBehind="RapPracownicy.aspx.cs" Inherits="HRRcp.Scorecards.Reports.RapPracownicy" %>

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
                    Title="select 'Raport czynności - ' + Nazwa from scCzynnosci where Id = @p1"
                    Title2="select 'Okres: ' + left(convert(varchar, '@p2', 20), 10) + ' - ' + left(convert(varchar, '@p3', 20), 10) "
                    DivClass="report_page RepCCUprawnienia"
                    Pager="false"
                    Charts="All"
                    SQL="
/*declare @cId int = @p1

select  p.Id, --qpid:-]
        p.Nazwisko, --qPracownik:C]
		count(w.Id) as Ilosc --qIlość:S] 
from Pracownicy p
left join scWartosci w on w.IdPracownika = p.Id and w.IdCzynnosci = @cId
where Ilosc &gt; 0
group by p.Id, p.Nazwisko
order by Nazwisko*/

declare @od datetime = '@p2'
declare @do datetime = '@p3'
/*
select
  p.KadryId --Nr Ewid.]
, p.Nazwisko + ' ' + p.Imie /*+ ISNULL(' (' + p.KadryId + ')', '')*/ --Pracownik]
, ta.Nazwa --Arkusz]
, /*count(w.Id)*/SUM(w.Ilosc) as Ilosc --Ilość:S]
, isnull(sum(w.Ilosc * c.Czas), 0) as Czas --Czas:S]
from Pracownicy p
left join scWartosci w on w.IdPracownika = p.Id
left join scCzynnosci c on w.IdCzynnosci = c.Id
left join scTypyArkuszyCzynnosci tac on tac.IdCzynnosci = w.IdCzynnosci and tac.IdTypuArkuszy = w.IdTypuArkuszy
left join scTypyArkuszy ta on ta.Id = tac.IdTypuArkuszy
where tac.Od &lt;= @do and @od &lt;= ISNULL(tac.Do, '20990909') and c.Id = @p1 and w.Ilosc is not null
group by p.Id, p.Nazwisko, p.Imie, p.KadryId, ta.Id, ta.Nazwa*/

select
  @p1 --[cid:-]
, @od --[pod:-]
, @do --[pdo:-]
, w.IdPracownika --[pid:-]
, ISNULL(p.KadryId, '') --[Nr Ewid.:C]
, case when w.IdPracownika &gt; 0 then p.Nazwisko + ' ' + p.Imie /*+ ISNULL(' (' + p.KadryId + ')', '')*/ else 'Zespół ' + ta.Nazwa end --[Pracownik|charts`names]
, ta.Nazwa --[Arkusz]
, ISNULL(SUM(w.Ilosc), 0) as Ilosc --[Ilość:S|RapPracownicyDaty @cid @pod @pdo @pid] 
, isnull(sum(w.Ilosc * c.Czas), 0) as Czas --[Czas (min):S|RapPracownicyDaty @cid @pod @pdo @pid|charts`values]
from scWartosci w
left join scCzynnosci c on w.IdCzynnosci = c.Id
left join scTypyArkuszy ta on w.IdTypuArkuszy = ta.Id
left join scTypyArkuszyCzynnosci tac on tac.IdCzynnosci = w.IdCzynnosci and tac.IdTypuArkuszy = w.IdTypuArkuszy and tac.Od &lt;= @do and @od &lt;= ISNULL(tac.Do, '20990909')
left join Pracownicy p on p.Id = w.IdPracownika
where w.Data between @od and @do and c.Id = @p1
group by w.IdPracownika, p.Nazwisko, p.Imie, p.KadryId, ta.Id, ta.Nazwa

" />
</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>