<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Scorecards/Report.Master" AutoEventWireup="true" CodeBehind="RapAbsZoom.aspx.cs" Inherits="HRRcp.Scorecards.Reports.RapAbsZoom" %>

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
                    Title="select 'Raport wskaźnik absencji - ' + (select Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')', '') Name from Pracownicy where Id = @p1)"
                    Title2="select left(convert(varchar, '@p2', 20), 10) + ' - ' + left(convert(varchar, '@p3', 20), 10) "
                    DivClass="report_page RepCCUprawnienia"
                    Pager="false"
                    Charts="All"
                    SQL="
declare @pracId int = @p1
declare @od datetime = '@p2'
declare @do datetime = '@p3'

select
  LEFT(CONVERT(varchar, dz.Data, 20), 10) Data --[Data:C|charts`names]
, ak.Symbol --[Symbol]
, ak.Nazwa --[Nazwa]
, aoa.AbsGodz --[Godz. absencji:S|charts`values]
from dbo.GetDates2(@od, @do) dz
left join Pracownicy p on p.Id = @pracId
left join PlanPracy pp on pp.IdPracownika = p.Id and dz.Data = pp.Data
outer apply (select k.Parametr from Kody k where k.Aktywny = 1 and k.Typ = /*'SCABSENCJE'*/'RCHOROBOWE') oa
left join Absencja a on a.IdPracownika = p.Id and a.Kod in (select items from dbo.SplitInt(oa.Parametr, ',')) and dz.Data between a.DataOd and a.DataDo
left join AbsencjaKody ak on a.Kod = ak.Kod
left join Zmiany Z on Z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
outer apply (select case when a.Kod is not null and Z.Id is not null and Z.Typ not in (1,2,3) then cast(8 as float) * p.EtatL / p.EtatM /*else 0 end*/ else 0 end as AbsGodz) aoa
where a.Id is not null
"
 />
 </ContentTemplate>
 </asp:UpdatePanel>
<%-- 
 |RapPracownicy @cId 
|RapPracownicy @cId|charts`values
--%>
</asp:Content>
