<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RepCCPracDni.aspx.cs" Inherits="HRRcp.RepCCPracDni" %>
<%@ Register src="~/Controls/Reports/cntReport2.ascx" tagname="cntReport" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<%--
raport - poszczegolne cc w dniach
p1 - od
p2 - do
p3 - cc lub *
p4 - zakres: 0 - wszystko, 1 - czaszm, 50 - nadg 50, 100 - nadg 100, 150 - nadg łącznie, 152 - nadg łacznie + nocne, 2 - nocne, 3 - wszystkie dopełnienia
p5 - logo pracownika
p6 - 1 019 podzielone, 0 niepodzielone
p7 - 1 dopełnienia, 0 bez dopełnień
--%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="select case when @p4=3 then 'Dopełnienia z zaokrągleń' else 'Czas przepracowany' end + ' - ' + KadryId + ' ' + Nazwisko + ' ' + Imie from Pracownicy where KadryId='@p5'"
        Title2="Okres: @p1 do @p2"
        Title3="select case when '@p3' = '*' then '' else (select 'cc: ' + cc + ' ' + Nazwa from CC where cc = '@p3') end"
        Title4="select
case when @p6=1 then '019 podzielone' else '019 niepodzielone' end +
case when @p7=1 then '' else ', bez dopełnień wynikających z zaokrągleń' end
        "
        SQL1="
join ccPrawa PR on PR.UserId = @UserId and PR.CC = S.CC
join ccPrawa PC on PC.UserId = @UserId and PC.CC = S.Class
        "
        P2="
,round(S.StawkaGodz, 4) as [Stawka godz.:N0.0000],
ISNULL(round(sum(S.vNadg50  * S.StawkaGodz * 1.5),2),0) as [Kwota 50:N0.00S], 
ISNULL(round(sum(S.vNadg100 * S.StawkaGodz * 2)  ,2),0) as [Kwota 100:N0.00S],
ISNULL(round(sum(S.vNocne   * S.StawkaNocna)     ,2),0) as [Kwota Nocne:N0.00S],

ISNULL(round(sum(S.vNadg50  * S.StawkaGodz * 1.5),2),0) +
ISNULL(round(sum(S.vNadg100 * S.StawkaGodz * 2)  ,2),0) as [Koszt 50+100:N0.00S],

ISNULL(round(sum(S.vNadg50  * S.StawkaGodz * 1.5),2),0) +
ISNULL(round(sum(S.vNadg100 * S.StawkaGodz * 2)  ,2),0) +
ISNULL(round(sum(S.vNocne   * S.StawkaNocna)     ,2),0) as [Suma kosztów:N0.00S]

--ISNULL(round(S.vNadg50 * S.StawkaGodz  * 1.5,2),0) as [Kwota 50:N0.00S], 
--ISNULL(round(S.vNadg100 * S.StawkaGodz * 2  ,2),0) as [Kwota 100:N0.00S],
--ISNULL(round(S.vNocne * S.StawkaNocna       ,2),0) as [Kwota Nocne:N0.00S],

--ISNULL(round(S.vNadg50 * S.StawkaGodz  * 1.5,2),0) +
--ISNULL(round(S.vNadg100 * S.StawkaGodz * 2  ,2),0) as [Koszt 50+100:N0.00S],

--ISNULL(round(S.vNadg50 * S.StawkaGodz  * 1.5,2),0) +
--ISNULL(round(S.vNadg100 * S.StawkaGodz * 2  ,2),0) +
--ISNULL(round(S.vNocne * S.StawkaNocna,2)       ,0) as [Suma kosztów:N0.00S],
        "
        CMD1="select case when '@p3'='*' then '' else '''Wszystkie cc'' as [|RepCCPracDni @p1 @p2 * 0 @p5 @p6 @p7|Czas przepracowany łącznie na wszystkie cc],' end"
        SQL="
declare @dataOd datetime
declare @dataDo datetime
set @dataOd = '@p1'
set @dataDo = '@p2'
        
SET LANGUAGE Polish

select 
convert(varchar(10), S.Data, 20) as [dzien:-],        
convert(varchar(10), S.Data, 20) + ' ' + dbo.FirstUpper(substring(DATENAME(DW,S.Data),1,3))
	as [Data|RepCCPracDniCC @dzien @dzien 0 @p5 @p6 @p7|Czas przepracowany na wszystkie cc w danym dniu],

S.CC + ' ' + CC.Nazwa as [CC],

    @CMD1

ISNULL(round(sum(S.vCzasZm), 2),0) as [Czas Zm:N0.00S|RepCCPracDniCC @dzien @dzien 0 @p5 @p6 @p7|Podział na cc w danym dniu], 
ISNULL(round(sum(S.vNadg50), 2),0) as [Nadg 50:N0.00S|RepCCPracDniCC @dzien @dzien 50 @p5 @p6 @p7|Podział nadgodzin 50 na cc w danym dniu], 
ISNULL(round(sum(S.vNadg100),2),0) as [Nadg 100:N0.00S|RepCCPracDniCC @dzien @dzien 100 @p5 @p6 @p7|Podział nadgodzin 100 na cc w danym dniu], 
ISNULL(round(sum(S.vNadg50), 2),0) +
ISNULL(round(sum(S.vNadg100),2),0) as [Nadg 50+100:N0.00S|RepCCPracDniCC @dzien @dzien 150 @p5 @p6 @p7|Podział nadgodzin (50+100) na cc w danym dniu],
ISNULL(round(sum(S.vNocne),  2),0) as [Nocne:N0.00S|RepCCPracDniCC @dzien @dzien 2 @p5 @p6 @p7|Podział czasu pracy w nocy na cc w danym dniu] 
    @SQL2
,S.TypOpis as [Typ]

from VrepDaneMPK S
    @SQL1
left outer join CC on CC.cc = S.CC
where S.Data between @dataOd and @dataDo 
and ('@p3' = '*' or '@p3' = S.cc)
and S.Logo='@p5'
--and (@p4 <> 3 or S.Typ in (3,13))
and (
    (@p4 = 0) or 
--    (@p4 = 3) or
    (@p4 = 1 and S.vCzasZm > 0) or 
    (@p4 = 50 and S.vNadg50 > 0) or 
    (@p4 = 100 and S.vNadg100 > 0) or 
    (@p4 = 150 and (S.vNadg50 > 0 or S.vNadg100 > 0)) or 
    (@p4 = 152 and (S.vNadg50 > 0 or S.vNadg100 > 0 or S.vNocne > 0)) or 
    (@p4 = 2 and S.vNocne > 0) or
    (@p4 = 3 and S.Typ in (3,13))
)
and (
    (@p6=1 and S.cc not in (select cc from CC where GrSplitu is not null))  -- bez 019, podzielone
 or (@p6=0 and S.Typ < 10)                                                  -- z 019, niepodzielone 
)
and (@p7=1 or S.Typ not in (3,13))  -- bez dopelnien

group by S.Data, S.StawkaGodz, S.StawkaNocna, S.CC, CC.Nazwa
, S.TypOpis

order by S.Data
"/>
</asp:Content>

