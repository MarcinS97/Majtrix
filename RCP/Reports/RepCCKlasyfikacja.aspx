<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RepCCKlasyfikacja.aspx.cs" Inherits="HRRcp.Reports.RepCCKlasyfikacja" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<%--
raport - 
p1 - od
p2 - do
p3 -  - zawsze 019 podzielone
p4 - dope³nienia: 1 z dope³nieniami, 0 bez dope³nieñ (na razie tylko 1)
--%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">    
    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="Podzia³ czasu pracy wg klasyfikacji pracowników"
        Title2="Okres: @p1 do @p2"
        Title3="select '019 podzielone' + case when @p4=1 then '' else ', bez dope³nieñ wynikaj¹cych z zaokr¹gleñ' end"
        SQL1="
join ccPrawa PR on PR.UserId = @UserId and PR.CC = S.CC
join ccPrawa PC on PC.UserId = @UserId and PC.CC = S.Class
        "
        P2="
,ISNULL(round(sum(S.vNadg50 * S.StawkaGodz * 1.5),0),0) as [Kwota 50:N0S], 
ISNULL(round(sum(S.vNadg100 * S.StawkaGodz * 2  ),0),0) as [Kwota 100:N0S],
ISNULL(round(sum(S.vNocne * S.StawkaNocna)       ,0),0) as [Kwota Nocne:N0S],

ISNULL(round(sum(S.vNadg50 * S.StawkaGodz * 1.5),0),0) +
ISNULL(round(sum(S.vNadg100 * S.StawkaGodz * 2  ),0),0) as [Koszt 50+100:N0S],

ISNULL(round(sum(S.vNadg50 * S.StawkaGodz * 1.5) ,0),0) +
ISNULL(round(sum(S.vNadg100 * S.StawkaGodz * 2)  ,0),0) +  
ISNULL(round(sum(S.vNocne * S.StawkaNocna)       ,0),0) as [Suma kosztów:N0S]
        "
        SQL="
declare @dod datetime
declare @ddo datetime
set @dod = '@p1'
set @ddo = '@p2'

select 
S.Class as [class:-],
S.Class as [Klasyfikacja|RepCCPracownicy @p1 @p2 * @class 0 * 1 @p4|Pracownicy z danej klasyfikacji],  
ISNULL(round(sum(S.vCzasZm), 0),0) as [Czas Zm:N0S|RepCCPracownicy @p1 @p2 * @class 1 * 1 @p4|Pracownicy pracuj¹cy na zmianie wg klasyfikacji], 
ISNULL(round(sum(S.vNadg50), 0),0) as [Nadg 50:N0S|RepCCPracownicy @p1 @p2 * @class 50 * 1 @p4|Pracownicy z nadgodzinami 50 wg klasyfikacji], 
ISNULL(round(sum(S.vNadg100),0),0) as [Nadg 100:N0S|RepCCPracownicy @p1 @p2 * @class 100 * 1 @p4|Pracownicy z nadgodzinami 100 wg klasyfikacji], 

ISNULL(round(sum(S.vNadg50), 0),0) +
ISNULL(round(sum(S.vNadg100),0),0) as [Nadg 50+100:N0S|RepCCPracownicy @p1 @p2 * @class 150 * 1 @p4|Pracownicy z nadgodzinami (50+100) wg klasyfikacji], 

ISNULL(round(sum(S.vNocne),  0),0) as [Nocne:N0S|RepCCPracownicy @p1 @p2 * @class 2 * 1 @p4|Pracownicy pracuj¹cy w czasie nocnym wg klasyfikacji] 
    @SQL2
from VrepDaneMPK S 
    @SQL1
where S.Data BETWEEN @dod AND @ddo

and S.cc not in (select cc from CC where GrSplitu is not null)  -- bez 019, podzielone - zawsze!
and (@p4=1 or S.Typ not in (3,13))  -- bez dopelnien

group by S.Class
order by S.Class
"/>
</asp:Content>
