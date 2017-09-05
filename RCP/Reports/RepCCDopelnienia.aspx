<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RepCCDopelnienia.aspx.cs" Inherits="HRRcp.Reports.RepCCDopelnienia" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<%--
raport - 
p1 - od
p2 - do
p3 - cc lub *
p4 - class lub *
p5 - zakres: 0 - wszystko, 1 - czaszm, 50 - nadg 50, 100 - nadg 100, 2 - nocne
--%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="select 'Dopełnienia z zaokrągleń' + 
case when '@p3' = '*' then '' else (select ' - ' + cc + ' ' + Nazwa from CC where cc = '@p3') end + 
case when '@p4' = '*' then '' else ' - @p4' end 
        "
        Title2="Okres: @p1 do @p2"
        SQL1="
join ccPrawa PR on PR.UserId = @UserId and PR.CC = S.CC
join ccPrawa PC on PC.UserId = @UserId and PC.CC = S.Class
        "
        P2="
round(S.StawkaGodz, 4) as [Stawka godz.:N0.0000],
ISNULL(round(sum(S.vNadg50  * S.StawkaGodz * 1.5),2),0) as [Kwota 50:N0.00S], 
ISNULL(round(sum(S.vNadg100 * S.StawkaGodz * 2)  ,2),0) as [Kwota 100:N0.00S],
ISNULL(round(sum(S.vNocne   * S.StawkaNocna)     ,2),0) as [Kwota Nocne:N0.00S],

ISNULL(round(sum(S.vNadg50  * S.StawkaGodz * 1.5),2),0) +
ISNULL(round(sum(S.vNadg100 * S.StawkaGodz * 2)  ,2),0) +
ISNULL(round(sum(S.vNocne   * S.StawkaNocna)     ,2),0) as [Suma kosztów:N0.00S],
        "
        SQL="
declare @dataOd datetime
declare @dataDo datetime
set @dataOd = '@p1'
set @dataDo = '@p2'

select 
S.Logo as [nrew:-], 
S.KierLogo as [knrew:-],
S.Logo as [Nr ew.], 
S.Pracownik as [Pracownik|RepCCPracCC @p1 @p2 * 0 @nrew 1 1|Czas przepracowany na wszystkie cc], 
S.Dzial as [Dział],
ISNULL(S.Stanowisko, S.ImpStanowisko) as [Stanowisko],
S.Class as [Klasyfikacja],

--S.CC + ' ' + CC.Nazwa as [CC],

--ISNULL(round(sum(S.vCzasZm) ,2),0) as [Czas Zm:N0.00S|RepCCPracDni @p1 @p2 @p3 1001 @nrew 1 1|Dopełnienia czasu pracy na wszystkie cc], 
--ISNULL(round(sum(S.vNadg50) ,2),0) as [Nadg 50:N0.00S|RepCCPracDni @p1 @p2 @p3 1050 @nrew 1 1|Dopełnienia nadgodzin 50 na wszystkie cc], 
--ISNULL(round(sum(S.vNadg100),2),0) as [Nadg 100:N0.00S|RepCCPracDni @p1 @p2 @p3 1100 @nrew 1 1|Dopełnienia nadgodzin 100 na wszystkie cc], 
--ISNULL(round(sum(S.vNocne)  ,2),0) as [Nocne:N0.00S|RepCCPracDni @p1 @p2 @p3 1002 @nrew 1 1|Dopełnienia czasu nocnego na wszystkie cc],
/*
ISNULL(round(sum(S.vCzasZm) ,2),0) as [Czas Zm:N0.00S|RepCCPracDni @p1 @p2 @p3 3 @nrew 1 1|Dopełnienia czasu pracy na wszystkie cc], 
ISNULL(round(sum(S.vNadg50) ,2),0) as [Nadg 50:N0.00S|RepCCPracDni @p1 @p2 @p3 3 @nrew 1 1|Dopełnienia czasu pracy na wszystkie cc], 
ISNULL(round(sum(S.vNadg100),2),0) as [Nadg 100:N0.00S|RepCCPracDni @p1 @p2 @p3 3 @nrew 1 1|Dopełnienia czasu pracy na wszystkie cc], 
ISNULL(round(sum(S.vNocne)  ,2),0) as [Nocne:N0.00S|RepCCPracDni @p1 @p2 @p3 3 @nrew 1 1|Dopełnienia czasu pracy na wszystkie cc],
*/
ISNULL(round(sum(S.vCzasZm) ,2),0) as [Czas Zm:N0.00S|RepCCPracDni @p1 @p2 @p3 0 @nrew 1 1|Czas pracy w okresie], 
ISNULL(round(sum(S.vNadg50) ,2),0) as [Nadg 50:N0.00S|RepCCPracDni @p1 @p2 @p3 0 @nrew 1 1|Czas pracy w okresie], 
ISNULL(round(sum(S.vNadg100),2),0) as [Nadg 100:N0.00S|RepCCPracDni @p1 @p2 @p3 0 @nrew 1 1|Czas pracy w okresie], 
ISNULL(round(sum(S.vNocne)  ,2),0) as [Nocne:N0.00S|RepCCPracDni @p1 @p2 @p3 0 @nrew 1 1|Czas pracy w okresie],
    @SQL2
S.Kierownik as [Kierownik|RepCCPracownicy @p1 @p2 * * 3 @knrew 1 1|Wszyscy pracownicy kierownika], 
S.KierLogo as [Nr ew. ]

from VrepDaneMPK S 
left outer join CC on CC.cc = S.CC
    @SQL1
where S.Data between @dataOd and @dataDo 
--and Typ in (3,13)
and Typ in (3)
and ('@p3' = '*' or '@p3' = S.cc)
and ('@p4' = '*' or '@p4' = S.Class)
and (
    (@p5 = 0) or
    (@p5 = 1   and S.vCzasZm > 0) or 
    (@p5 = 50  and S.vNadg50 > 0) or 
    (@p5 = 100 and S.vNadg100 > 0) or 
    (@p5 = 2   and S.vNocne > 0) 
)
group by S.Logo, S.Pracownik, 
		S.Dzial, S.Stanowisko, S.ImpStanowisko,
		S.Class, 
		--S.CC, CC.Nazwa, 
		S.KierLogo, S.Kierownik, S.StawkaGodz 
order by S.Logo
"/>
</asp:Content>

