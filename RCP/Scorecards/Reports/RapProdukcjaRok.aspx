<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Scorecards/Report.Master" AutoEventWireup="true" CodeBehind="RapProdukcjaRok.aspx.cs" Inherits="HRRcp.Scorecards.Reports.RapProdukcjaRok" %>

<%@ Register Src="~/Controls/EliteReports/cntReport.ascx" TagPrefix="leet" TagName="Report" %>
<%@ Register Src="~/Controls/EliteReports/cntFilter.ascx" TagPrefix="leet" TagName="Filter" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">


    <asp:HiddenField id="hidObserverId" runat="server" Visible="false" />
    
    <div class="report_page RepCCUprawnienia">
        <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <div class="filters">
            <div class="filter">
                <asp:Label ID="lblDataOd" runat="server" Text="Rok: " />
                <asp:DropDownList ID="ddlYears" runat="server" DataValueField="Id" DataTextField="Name" DataSourceID="dsYears" />
                <asp:SqlDataSource ID="dsYears" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="select distinct YEAR(DataOd) as Id, YEAR(DataOd) as Name from OkresyRozl order by Id desc" />
                
            </div>
            <div class="filter">
                <asp:Label ID="Label2" runat="server" Text="Przełożony: " />
                <asp:DropDownList ID="ddlSuperiors" runat="server" DataValueField="Id" DataTextField="Name"
                    DataSourceID="dsSuperiors" />
                <asp:SqlDataSource ID="dsSuperiors" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="
select 0 as Id, 'Wszyscy przełożeni' as Name, 0 as Sort where @ObserverId = 0
union all
select p.Id, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') as Name, 3 as Sort
from dbo.fn_GetTree2(@observerId, 1, GETDATE()) t
left join Pracownicy p on p.Id = t.IdPracownika where t.IdPracownika in (select distinct IdKierownika from Przypisania where IdCommodity is not null) order by Sort">
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
            Title="Raport Produktywność / FPY na rok"
            SQL="
declare @observerId int = @SQL1
declare @CC int = @SQL3

select
p1.KadryId --[^Nr Ewid.]
, p1.Nazwisko + ' ' + p1.Imie --[^Pracownik]
, p2.Nazwisko + ' ' + p2.Imie + ISNULL(' (' + p2.KadryId + ')', '') --[^Przełożony]
, soa.cc --[^MPK]
--, w.Nazwa 
, [1], [-1], [2], [-2], [3], [-3], [4], [-4], [5], [-5], [6], [-6], [7], [-7], [8], [-8], [9], [-9], [10], [-10], [11], [-11], [12], [-12]
--[Styczeń^Prod.][Styczeń^FPY][Luty^Prod.][Luty^FPY][Marzec^Prod.][Marzec^FPY][Kwiecień^Prod.][Kwiecień^FPY][Maj^Prod.][Maj^FPY][Czerwiec^Prod.][Czerwiec^FPY][Lipiec^Prod.][Lipiec^FPY][Sierpień^Prod.][Sierpień^FPY][Wrzesień^Prod.][Wrzesień^FPY][Październik^Prod.][Październik^FPY][Listopad^Prod.][Listopad^FPY][Grudzień^Prod.][Grudzień^FPY]
from 
(
  select * from 
  (
  select
  --w.Id, 
  --w.Data
  YEAR(w.Data) as Rok,
  MONTH(w.Data) * q.asd as aoe
  , prz.Id
  , prz.IdPracownika
  , prz.IdKierownika
  --, ta.Rodzaj
  --, ta.Nazwa
  --, oa.Produktywnosc
  --, oa.FPY
  --, case when oa.Produktywnosc &gt;= oa2.CelProd then '&lt;span&gt;' + CONVERT(varchar, round(oa.Produktywnosc, 2)) + '% ' + '&lt;/span&gt;' else '&lt;span style=''color: red;''&gt;' + CONVERT(varchar, round(oa.Produktywnosc, 2)) + '% ' + '&lt;/span&gt;' end + '/ ' + case when oa.FPY &gt;= oa2.CelJak then '&lt;span&gt;' + CONVERT(varchar, round(oa.FPY, 2)) + '%' + '&lt;/span&gt;' else '&lt;span style=''color: red;''&gt;' + CONVERT(varchar, round(oa.FPY, 2)) + '%' + '&lt;/span&gt;' end as cat
  , case when q.asd = 1 then case when oa.Produktywnosc &gt;= oa2.CelProd then '&lt;span&gt;' + CONVERT(varchar, round(oa.Produktywnosc, 2)) + '% ' + '&lt;/span&gt;' else '&lt;span style=''color: red;''&gt;' + CONVERT(varchar, round(oa.Produktywnosc, 2)) + '% ' + '&lt;/span&gt;' end else case when oa.FPY &gt;= oa2.CelJak then '&lt;span&gt;' + CONVERT(varchar, round(oa.FPY, 2)) + '%' + '&lt;/span&gt;' else '&lt;span style=''color: red;''&gt;' + CONVERT(varchar, round(oa.FPY, 2)) + '%' + '&lt;/span&gt;' end end as cat
  from scWnioski w
  left join scTypyArkuszy ta on ta.Id = w.IdTypuArkuszy
  left join scPremie prem on w.Id = prem.IdWniosku and prem._do is null and ((ta.Rodzaj = 4) or (ta.Rodzaj = 5 and prem.IdPracownika &lt; 0))
  left join scParametry prod on prod.IdTypuArkuszy = ta.Id and (w.Data between prod.Od and ISNULL(prod.Do, '20990909')) and prod.Typ = 'PROD' and prod.TL = case when w.IdPracownika = prem.IdPracownika then 1 else 0 end
  left join scParametry qc on qc.IdTypuArkuszy = ta.Id and (w.Data between qc.Od and ISNULL(qc.Do, '20990909')) and qc.Typ = 'QC' and qc.TL = case when w.IdPracownika = prem.IdPracownika then 1 else 0 end
  left join Przypisania prz on w.Data between prz.Od and ISNULL(prz.Do, '20990909') and ((prem.IdPracownika &gt; 0 and prz.IdPracownika = prem.IdPracownika) or (prem.IdPracownika &lt; 0 and prz.IdCommodity = 0 - prem.IdPracownika))
  left join (select 1 asd union select -1 asd) q on 1 = 1
  outer apply (select case when prem.CzasPracy &gt; 0 then round((prem.GodzProd / prem.CzasPracy), 2) * 100 else 0 end as Produktywnosc, case when prem.IloscSztuk &gt; 0 then (convert(float, (prem.IloscSztuk - prem.IloscBledow)) / prem.IloscSztuk) * 100 else 0 end as FPY) oa
  outer apply (select ISNULL(case when prem.GodzProd &gt; 0 then prod.Parametr else 0 end, 0) * 100 as CelProd, ISNULL(case when prem.IloscBledow &gt; 0 then qc.Parametr else 0 end, 0) * 100 as CelJak) oa2
  where (ta.Rodzaj = 4 or ta.Rodzaj = 5)
  ) as w
  PIVOT
  (
  MAX(w./*Produktywnosc*/cat) for w.aoe in ([1], [-1], [2], [-2], [3], [-3], [4], [-4], [5], [-5], [6], [-6], [7], [-7], [8], [-8], [9], [-9], [10], [-10], [11], [-11], [12], [-12])
  ) as PV
) w
left join Pracownicy p1 on p1.Id = w.IdPracownika
left join Pracownicy p2 on p2.Id = w.IdKierownika
outer apply (select dbo.cat(CC.cc, ', ', 0) as cc from SplityWspP s left join CC on s.IdCC = CC.Id where IdPrzypisania = w.Id) soa
outer apply (select top 1 IdCC from SplityWspP where IdPrzypisania = w.Id) soa2
where w.IdPracownika in (select IdPracownika from dbo.fn_GetSubPrzypisania(@observerId, GETDATE())) and 
--(w.Rodzaj = 4 or w.Rodzaj = 5) and 
Rok = @SQL2
and (soa2.IdCC = @CC or @CC = 0)
--YEAR(w.Data) = 2015
order by 1

" />
</ContentTemplate>
</asp:UpdatePanel>

</div>


</asp:Content>
