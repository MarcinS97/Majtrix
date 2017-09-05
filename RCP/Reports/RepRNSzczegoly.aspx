<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RepRNSzczegoly.aspx.cs" Inherits="HRRcp.Reports.RepRNSzczegoly" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<%--
raport - pracownicy w zakresie dat
p1 - od
p2 - do
p3 - typ 
p4 - pracId
--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">    
    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="select 'Rozliczenie nadgodzin' + 
case '@p3' 
    when 'drN'  then ' - Niedomiar do rozliczenia' 
    when 'P50'  then ' - Nadgodziny 50 do wyp³aty' 
    when 'P100' then ' - Nadgodziny 100 do wyp³aty' 
    when 'NOC'  then ' - Czas pracy w nocy' 
    when 'W50'  then ' - Nadgodziny 50 wybrane' 
    when 'W100' then ' - Nadgodziny 100 wybrane' 
    when 'O50'  then ' - Nadgodziny 50 odpracowane' 
    when 'O100' then ' - Nadgodziny 100 odpracowane' 
    else             '' 
end"
        Title2="select 'Pracownik: ' + KadryId + ' ' + Nazwisko + ' ' + Imie from Pracownicy where Id = @p4"
        Title3="select case when '@p1' = '@p2' then 'Dzieñ: @p1' else 'Okres: @p1 do @p2' end"
        SQL="
declare 
    @od datetime,
    @do datetime,
    @pracId int,
    @typ varchar(10)
set @od = dbo.bom('@p1')
set @do = dbo.eom('@p2')
set @typ = '@p3'
set @pracId = @p4

select
    @pracId [pracId:-],
    convert(varchar(10), Data, 20) [Data:D|RepRNSzczegoly2 @Data @Data * @pracId|Poka¿ wszystko z dnia],
    /*    
    Niedomiar [Niedomiar:NS|Niedomiar],
    N50   [Nadgodziny 50:NS],    -- do wyp³aty (nierozliczone + do wyp³aty)
    N100  [Nadgodziny 100:NS],
    */
    drN   [Niedomiar do rozliczenia:NS||Niedomiar do rozliczenia],
    P50   [Do wyp³aty 50:NS],    -- do wyp³aty (nierozliczone + do wyp³aty)
    P100  [Do wyp³aty 100:NS],
    Nocne [Czas nocny:NS],
	W50   [Wybrane 50:NS|RepRNSzczegoly2 @Data @Data W50 @pracId|Nadgodziny 50 wybrane],
	W100  [Wybrane 100:NS|RepRNSzczegoly2 @Data @Data W100 @pracId|Nadgodziny 100 wybrane],
	O50   [Odpracowane 50:NS|RepRNSzczegoly2 @Data @Data O50 @pracId|Nadgodziny 50 - odpracowanie],
	O100  [Odpracowane 100:NS|RepRNSzczegoly2 @Data @Data O50 @pracId|Nadgodziny 100 - odpracowanie]
from
(    
select  
    NS.Data,
    /*
    round(cast(sum(NS.Niedomiar) as float) / 3600, 2) as Niedomiar,
    round(cast(sum(NS.N50)  as float) / 3600, 2) as N50,
    round(cast(sum(NS.N100) as float) / 3600, 2) as N100,
    */ 
    round(cast(sum(NS.Niedomiar + NS.WND + NS.OND) as float) / 3600, 2) as drN,
    round(cast(sum(NS.N50  - (NS.W50 + NS.O50))   as float) / 3600, 2) as P50,
    round(cast(sum(NS.N100 - (NS.W100 + NS.O100)) as float) / 3600, 2) as P100,
	round(cast(sum(NS.Nocne) as float) / 3600, 2) as Nocne,
	round(cast(sum(NS.W50)   as float) / 3600, 2) as W50,
	round(cast(sum(NS.W100)  as float) / 3600, 2) as W100,
	round(cast(sum(NS.O50)   as float) / 3600, 2) as O50,
	round(cast(sum(NS.O100)  as float) / 3600, 2) as O100
from VRozliczenieNadgodzin NS
where NS.IdPracownika = @pracId 
and NS.Data between @od and @do
group by Data
) D
where (
    (@typ = 'drN'  and drN   &lt;&gt; 0) or
    (@typ = 'P50'  and P50   &lt;&gt; 0) or
    (@typ = 'P100' and P100  &lt;&gt; 0) or  
    (@typ = 'NOC'  and Nocne &lt;&gt; 0) or
    (@typ = 'W50'  and W50   &lt;&gt; 0) or
    (@typ = 'W100' and W100  &lt;&gt; 0) or
    (@typ = 'O50'  and O50   &lt;&gt; 0) or
    (@typ = 'O100' and O100  &lt;&gt; 0) or
    (@typ = '*' and (
        drN   &lt;&gt; 0 or
        P50   &lt;&gt; 0 or
        P100  &lt;&gt; 0 or
        Nocne &lt;&gt; 0 or
        W50   &lt;&gt; 0 or
        W100  &lt;&gt; 0 or
        O50   &lt;&gt; 0 or
        O100  &lt;&gt; 0
    ))
)
order by Data
"/>
</asp:Content>
