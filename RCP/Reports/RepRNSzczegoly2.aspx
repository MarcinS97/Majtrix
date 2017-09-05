<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RepRNSzczegoly2.aspx.cs" Inherits="HRRcp.Reports.RepRNSzczegoly2" %>
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
set @od = '@p1'
set @do = '@p2'
set @typ = '@p3'
set @pracId = @p4

select
    Data     [Data:D],
    /*    
    Niedomiar [Niedomiar:NS],
    N50   [Nadgodziny 50:NS],    -- do wyp³aty (nierozliczone + do wyp³aty)
    N100  [Nadgodziny 100:NS],
    */
    /*
    drN   [Niedomiar do rozliczenia:NS||Niedomiar do rozliczenia],
    P50   [Do wyp³aty 50:NS],    -- do wyp³aty (nierozliczone + do wyp³aty)
    P100  [Do wyp³aty 100:NS],
    Nocne [Czas nocny:NS],
	*/
    drN   [drN:-],
    P50   [P50:-],    -- do wyp³aty (nierozliczone + do wyp³aty)
    P100  [P100:-],
    Nocne [Nocne:-],

	W50   [Wybrane 50:NS],
	W100  [Wybrane 100:NS],
	O50   [Odpracowane 50:NS],
	O100  [Odpracowane 100:NS],
	
    TypNazwa [Typ rozliczenia],
    ZaDzien  [Za dzieñ:D]
from
(    
select  
    Data, TypNazwa, ZaDzien,
    /*
    round(cast(NS.Niedomiar as float) / 3600, 2) as Niedomiar,
    round(cast(NS.N50  as float) / 3600, 2) as N50,
    round(cast(NS.N100 as float) / 3600, 2) as N100,
    */ 
    /*
    round(cast(NS.Niedomiar + NS.WND + NS.OND as float) / 3600, 2) as drN,
    round(cast(NS.N50  - (NS.W50 + NS.O50)   as float) / 3600, 2) as P50,
    round(cast(NS.N100 - (NS.W100 + NS.O100) as float) / 3600, 2) as P100,
	round(cast(NS.Nocne as float) / 3600, 2) as Nocne,
	*/
    0 as drN,
    0 as P50, 
    0 as P100,
    0 as Nocne,

	round(cast(NS.W50   as float) / 3600, 2) as W50,
	round(cast(NS.W100  as float) / 3600, 2) as W100,
	round(cast(NS.O50   as float) / 3600, 2) as O50,
	round(cast(NS.O100  as float) / 3600, 2) as O100
from VRozliczenieNadgodzin NS
where NS.IdPracownika = @pracId 
and NS.Data = @od
and NS.Typ &lt;&gt; -10  -- czas pracy
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
order by Data, ZaDzien
"/>
</asp:Content>
