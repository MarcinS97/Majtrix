<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RepCC.aspx.cs" Inherits="HRRcp.Reports.RepCC" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<%--
raport - 
p1 - od
p2 - do
p3 - 1 019 podzielone, 0 niepodzielone
p4 - 1 dope³nienia, 0 bez dope³nieñ
--%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">    
    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="Podzia³ czasu pracy na CC"
        Title2="Okres: @p1 do @p2"
        Title3="select
case when @p3=1 then '019 podzielone' else '019 niepodzielone' end +
case when @p4=1 then '' else ', bez dope³nieñ wynikaj¹cych z zaokr¹gleñ' end
        "
        SQL1="
join ccPrawa PR on PR.UserId = @UserId and PR.IdCC = CC.Id
join ccPrawa PC on PC.UserId = @UserId and PC.CC = ISNULL(S.Class, PC.CC)
        "
        P2="
,ISNULL(round(sum(S.vNadg50 * S.StawkaGodz * 1.5),2),0) as [Kwota 50:N0.00S], 
ISNULL(round(sum(S.vNadg100 * S.StawkaGodz * 2),2),0) as [Kwota 100:N0.00S],
ISNULL(round(sum(S.vNocne * S.StawkaNocna),2),0) as [Kwota Nocne:N0.00S],

ISNULL(round(sum(S.vNadg50 * S.StawkaGodz * 1.5),2),0) +
ISNULL(round(sum(S.vNadg100 * S.StawkaGodz * 2),2),0) as [Koszt 50+100:N0.00S],

ISNULL(round(sum(S.vNadg50 * S.StawkaGodz * 1.5),2),0) +
ISNULL(round(sum(S.vNadg100 * S.StawkaGodz * 2),2),0) +  
ISNULL(round(sum(S.vNocne * S.StawkaNocna),2),0) as [Suma kosztów:N0.00S]
        "
        SQL="
declare @dod datetime
declare @ddo datetime
set @dod = '@p1'
set @ddo = '@p2'

select CC.cc as [cc|RepCCPracownicy @p1 @p2 @cc * 0 * @p3 @p4|Pracownicy pracuj¹cy na cc],  
CC.Nazwa,
ISNULL(round(sum(S.vCzasZm), 2),0) as [Czas Zm:N0.00S|RepCCPracownicy @p1 @p2 @cc * 1 * @p3 @p4|Pracownicy pracuj¹cy na zmianie na cc], 
ISNULL(round(sum(S.vNadg50), 2),0) as [Nadg 50:N0.00S|RepCCPracownicy @p1 @p2 @cc * 50 * @p3 @p4|Pracownicy z nadgodzinami 50 na cc], 
ISNULL(round(sum(S.vNadg100),2),0) as [Nadg 100:N0.00S|RepCCPracownicy @p1 @p2 @cc * 100 * @p3 @p4|Pracownicy z nadgodzinami 100 na cc], 
ISNULL(round(sum(S.vNadg50), 2),0) +
ISNULL(round(sum(S.vNadg100),2),0) as [Nadg 50+100:N0.00S|RepCCPracownicy @p1 @p2 @cc * 150 * @p3 @p4|Pracownicy z nadgodzinami (50+100) na cc],
ISNULL(round(sum(S.vNocne),  2),0) as [Nocne:N0.00S|RepCCPracownicy @p1 @p2 @cc * 2 * @p3 @p4|Pracownicy pracuj¹cy w czasie nocnym na cc] 
    @SQL2
from CC 
left outer join VrepDaneMPK S on S.cc = CC.cc and S.Data BETWEEN @dod AND @ddo

and (
    (@p3=1 and S.cc not in (select cc from CC where GrSplitu is not null))  -- bez 019, podzielone
 or (@p3=0 and S.Typ &lt; 10)    -- z 019, niepodzielone 
)
and (@p4=1 or S.Typ not in (3,13))  -- bez dopelnien

    @SQL1
where @ddo between CC.AktywneOd and ISNULL(CC.AktywneDo, '20990909')
group by CC.cc, CC.Nazwa
order by CC.cc
"/>
</asp:Content>
