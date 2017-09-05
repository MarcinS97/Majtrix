<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Scorecards/Report.Master" AutoEventWireup="true" CodeBehind="RapAbs2.aspx.cs" Inherits="HRRcp.Scorecards.Reports.RapAbs2" %>

<%@ Register Src="~/Controls/EliteReports/cntReport.ascx" TagPrefix="leet" TagName="Report" %>
<%@ Register Src="~/Controls/EliteReports/cntFilter.ascx" TagPrefix="leet" TagName="Filter" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">

<asp:HiddenField id="hidObserverId" runat="server" Visible="false" />
    
    <div class="report_page RepCCUprawnienia">
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
/*                    
select * from
(
	select 0 as Id, 'Wszyscy pracownicy...' as Name, 0 as Sort, 1 as R
	union all
	select p.Id, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') as Name, 1 as Sort, 0 as R
	from dbo.fn_GetTree2(@observerId, 1, GETDATE()) t
	left join Pracownicy p on p.Id = t.IdPracownika
	where p.Kierownik = 1 /*t.IdPracownika in (select distinct IdKierownika from Przypisania where IdCommodity is not null)*/
) asd where (asd.R = 0) or ((asd.R = 0 or asd.R = 1) and (dbo.GetRight(@observerId, 68) = 1 or dbo.GetRight(@observerId, 56) = 1))
order by Sort, Name
*/

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
                <asp:Label ID="Label3" runat="server" Text="MPK: " />
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
            Charts="Line,Bar" 
            Title="Wskaźnik absencji"
            SQL="
declare @observerId int = @SQL1
declare @data datetime = GETDATE()
declare @od datetime = '@SQL2'
declare @do datetime = '@SQL3'
declare @ccId int = @SQL4

select 
  p.Id --[pid:-]
, p.KadryId as NrEwid --[Nr Ewid.]
, p.Nazwisko + ' ' + p.Imie as Pracownik --[Pracownik|charts`names]
, k.Nazwisko + ' ' + k.Imie + isnull(' (' + k.KadryId + ')', '') as Kierownik --[Kierownik]
, LEFT(CONVERT(varchar, p.DataZatr, 20), 10) dza --[Data zatrudnienia]
, LEFT(CONVERT(varchar, p.DataZwol, 20), 10) dzb --[Data zwolnienia]
, soa.cc --[MPK]
, sum(/*pp2oa.GodzNieob*/aoa.AbsGodz) as GodzNieob --[Godz. Nieob.:N|RapAbsZoom @pid @SQL2 @SQL3]
, sum(ppoa.Nominal) as Nominal --[Nomniał:N]
, (case when sum(ppoa.Nominal) = 0 then 0 else round(sum(aoa.AbsGodz) / sum(ppoa.Nominal) * 100, 2) end) as WskaznikAbs --[Wskaźnik abs.:N|charts`values]
from dbo.GetDates2(@od, @do) dz
--left join dbo.fn_GetSubPrzypisania(@observerId, @) prz on prz.IdCommodity is not null /*PRODUKCYJNI*/
outer apply (select * from dbo.fn_GetSubPrzypisania(@observerId, dz.Data) prz where /*prz.IdCommodity is not null*/prz.IdPracownika in
    (select distinct IdPracownika from Przypisania q where q.IdCommodity is not null and @do between q.Od and ISNULL(q.Do, '20990909'))) prz
left join Pracownicy p on prz.IdPracownika = p.Id
outer apply (select top 1 * from Przypisania where IdPracownika = p.Id and Status = 1 and Od &lt;= @do order by Od desc) przk
left join Pracownicy k on przk.IdKierownika = k.Id
left join PlanPracy pp on pp.IdPracownika = prz.IdPracownika and dz.Data = pp.Data
outer apply (select k.Parametr from Kody k where k.Aktywny = 1 and k.Typ = /*'SCABSENCJE'*/'RCHOROBOWE') oa
--left join Absencja a on /*@pracId &gt; 0 and */a.IdPracownika = /*@pracId*/prz.IdPracownika and a.Kod in (select items from dbo.SplitInt(oa.Parametr, ',')) and a.DataOd &lt; @do and a.DataDo &gt; @od --dz.Data between a.DataOd and a.DataDo
left join Absencja a on /*@pracId &gt; 0 and */a.IdPracownika = /*@pracId*/prz.IdPracownika and a.Kod in (select items from dbo.SplitInt(oa.Parametr, ',')) and dz.Data between a.DataOd and a.DataDo
left join AbsencjaKody ak on a.Kod = ak.Kod
left join Zmiany Z on Z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
outer apply (select ISNULL(case when Z.Typ in (1,2,3) then 0 else case when Z.Od &gt; Z.Do then 24 else 0 end + DATEDIFF(HOUR, Z.Od, Z.Do) end, 0) as Nominal, ISNULL(case when Z.Typ in (1,2,3) then 0 else cast(pp.CzasZm as float) end, 0) / 3600 as vCzasZm, CONVERT(float, ISNULL(pp.n50, 0) + ISNULL(pp.n100, 0))/3600 as Nadgodziny) ppoa 
outer apply (select case when ppoa.Nominal &gt; ppoa.vCzasZm then ppoa.Nominal - ppoa.vCzasZm else 0 end as GodzNieob) pp2oa
outer apply (select ppoa.Nominal + ppoa.Nadgodziny - pp2oa.GodzNieob as Godziny) goa
outer apply (select case when a.Kod is not null and Z.Id is not null and Z.Typ not in (1,2,3) then /*case when ppoa.Nominal &gt; 0 then*/ cast(8 as float) * p.EtatL / p.EtatM /*else 0 end*/ else 0 end as AbsGodz) aoa
outer apply (select CC.cc from SplityWspP s left join CC on s.IdCC = CC.Id where IdPrzypisania = prz.Id) soa
outer apply (select top 1 IdCC from SplityWspP where IdPrzypisania = prz.Id) soa2
where (soa2.IdCC = @ccId or @ccId = 0) and prz.Id is not null
group by prz.IdPracownika, p.Id, p.Nazwisko, p.Imie, p.KadryId, k.Nazwisko, k.Imie, k.KadryId, soa.cc, p.DataZatr, p.DataZwol
order by p.Nazwisko

select 
  '' as NrEwid
, '' as Pracownik
, '' as Kierownik
, ''
, ''
, ''
, sum(/*pp2oa.GodzNieob*/aoa.AbsGodz) as GodzNieob
, sum(ppoa.Nominal) as Nominal
, (case when sum(ppoa.Nominal) = 0 then 0 else round(sum(aoa.AbsGodz) / sum(ppoa.Nominal) * 100, 2) end) as WskaznikAbs
from dbo.GetDates2(@od, @do) dz
--left join dbo.fn_GetSubPrzypisania(@observerId, @) prz on prz.IdCommodity is not null /*PRODUKCYJNI*/
--outer apply (select * from dbo.fn_GetSubPrzypisania(@observerId, dz.Data) prz /*where prz.IdCommodity is not null*/) prz
outer apply (select * from dbo.fn_GetSubPrzypisania(@observerId, dz.Data) prz where /*prz.IdCommodity is not null*/prz.IdPracownika in
    (select distinct IdPracownika from Przypisania q where q.IdCommodity is not null and @do between q.Od and ISNULL(q.Do, '20990909'))) prz
left join Pracownicy p on prz.IdPracownika = p.Id
outer apply (select top 1 * from Przypisania where IdPracownika = p.Id and Status = 1 and Od &lt;= @do order by Od desc) przk
left join Pracownicy k on przk.IdKierownika = k.Id
left join PlanPracy pp on pp.IdPracownika = prz.IdPracownika and dz.Data = pp.Data
outer apply (select k.Parametr from Kody k where k.Aktywny = 1 and k.Typ = /*'SCABSENCJE'*/'RCHOROBOWE') oa
--left join Absencja a on /*@pracId &gt; 0 and */a.IdPracownika = /*@pracId*/prz.IdPracownika and a.Kod in (select items from dbo.SplitInt(oa.Parametr, ',')) and a.DataOd &lt; @do and a.DataDo &gt; @od --dz.Data between a.DataOd and a.DataDo
left join Absencja a on /*@pracId &gt; 0 and */a.IdPracownika = /*@pracId*/prz.IdPracownika and a.Kod in (select items from dbo.SplitInt(oa.Parametr, ',')) and dz.Data between a.DataOd and a.DataDo
left join AbsencjaKody ak on a.Kod = ak.Kod
left join Zmiany Z on Z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
outer apply (select ISNULL(case when Z.Typ in (1,2,3) then 0 else case when Z.Od &gt; Z.Do then 24 else 0 end + DATEDIFF(HOUR, Z.Od, Z.Do) end, 0) as Nominal, ISNULL(case when Z.Typ in (1,2,3) then 0 else cast(pp.CzasZm as float) end, 0) / 3600 as vCzasZm, CONVERT(float, ISNULL(pp.n50, 0) + ISNULL(pp.n100, 0))/3600 as Nadgodziny) ppoa 
outer apply (select case when ppoa.Nominal &gt; ppoa.vCzasZm then ppoa.Nominal - ppoa.vCzasZm else 0 end as GodzNieob) pp2oa
outer apply (select ppoa.Nominal + ppoa.Nadgodziny - pp2oa.GodzNieob as Godziny) goa
outer apply (select case when a.Kod is not null and Z.Id is not null and Z.Typ not in (1,2,3) then /*case when ppoa.Nominal &gt; 0 then*/ cast(8 as float) * p.EtatL / p.EtatM /*else 0 end*/ else 0 end as AbsGodz) aoa
outer apply (select CC.cc from SplityWspP s left join CC on s.IdCC = CC.Id where IdPrzypisania = prz.Id) soa
outer apply (select top 1 IdCC from SplityWspP where IdPrzypisania = prz.Id) soa2
where (soa2.IdCC = @ccId or @ccId = 0) and prz.Id is not null
" />


</div>





</asp:Content>
