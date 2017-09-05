<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RepCCPracownicy.aspx.cs" Inherits="HRRcp.Reports.RepCCPracownicy" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<%--
raport - 
p1 - od
p2 - do
p3 - cc lub *
p4 - class lub *
p5 - zakres: 0 - wszystko, 1 - czaszm, 50 - nadg 50, 100 - nadg 100, 150 - nadg łącznie, 2 - nocne, 3 - wszystkie dopełnienia
p6 - nrew kierownika lub *
p7 - 1 019 podzielone, 0 niepodzielone
p8 - 1 dopełnienia, 0 bez dopełnień
--%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="select 
case 
    when @p5 = 1   then 'Pracownicy pracujący na zmianie'
    when @p5 = 50  then 'Pracownicy z nadgodzinami 50'
    when @p5 = 100 then 'Pracownicy z nadgodzinami 100'
    when @p5 = 150 then 'Pracownicy z nadgodzinami (50+100)'
    when @p5 = 2   then 'Pracownicy pracujący w czasie nocnym'
    when @p5 = 3   then 'Pracownicy z dopełnieniami do pełnej godziny'
    else                'Pracownicy'
end +   
case when '@p3' = '*' then '' else ' - cc: ' + (select cc + ' ' + Nazwa from CC where cc = '@p3') end + 
case when '@p4' = '*' then '' else ' - klasyfikacja: @p4' end +
case when '@p6' = '*' then '' else ' - kierownik: ' + (select KadryId + ' ' + Nazwisko + ' ' + Imie from Pracownicy where KadryId='@p6') end 
        "
        Title2="Okres: @p1 do @p2"
        Title3="select
case when @p7=1 then '019 podzielone' else '019 niepodzielone' end +
case when @p8=1 then '' else ', bez dopełnień wynikających z zaokrągleń' end
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
        "
        CMD1="
select case when '@p6'='*' then 
--',S.Kierownik as [Kierownik|RepCCPracownicy @p1 @p2 * * 0 @knrew @p7 @p8|Wszyscy pracownicy kierownika], S.KierLogo as [Nr ew. ]' 
',S.Kierownik as [Kierownik], S.KierLogo as [Nr ew. ]' 
else '' end
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
S.Pracownik as [Pracownik|RepCCPracCC @p1 @p2 * 0 @nrew @p7 @p8|Czas przepracowany na wszystkie cc], 
S.Dzial as [Dział],
ISNULL(S.Stanowisko, S.ImpStanowisko) as [Stanowisko],
S.Class as [Klasyfikacja],
--S.CC + ' ' + CC.Nazwa as [CC],

ISNULL(round(sum(S.vCzasZm) ,2),0) as [Czas Zm:N0.00S|RepCCPracDni @p1 @p2 @p3 0 @nrew @p7 @p8|Dni przepracowane na cc], 
ISNULL(round(sum(S.vNadg50) ,2),0) as [Nadg 50:N0.00S|RepCCPracDni @p1 @p2 @p3 50 @nrew @p7 @p8|Dni z nadgodzinami 50 przepracowanymi na cc], 
ISNULL(round(sum(S.vNadg100),2),0) as [Nadg 100:N0.00S|RepCCPracDni @p1 @p2 @p3 100 @nrew @p7 @p8|Dni z nadgodzinami 100 przepracowanymi na cc], 
ISNULL(round(sum(S.vNadg50) ,2),0) + 
ISNULL(round(sum(S.vNadg100),2),0) as [Nadg 50+100:N0.00S|RepCCPracDni @p1 @p2 @p3 150 @nrew @p7 @p8|Dni z nadgodzinami (50+100) przepracowanymi na cc], 
ISNULL(round(sum(S.vNocne)  ,2),0) as [Nocne:N0.00S|RepCCPracDni @p1 @p2 @p3 2 @nrew @p7 @p8|Dni z przepracowane w nocy na cc]
    @SQL2
    @CMD1
from VrepDaneMPK S 
left outer join CC on CC.cc = S.CC
    @SQL1
where S.Data between @dataOd and @dataDo 
and ('@p3' = '*' or '@p3' = S.cc)
and ('@p4' = '*' or '@p4' = S.Class)
and (@p5 <> 3 or S.Typ in (3,13))
and (
    (@p5 = 0) or (@p5 = 3) or
    (@p5 = 1   and S.vCzasZm > 0) or 
    (@p5 = 50  and S.vNadg50 > 0) or 
    (@p5 = 100 and S.vNadg100 > 0) or 
    (@p5 = 150 and (S.vNadg50 > 0 or S.vNadg100 > 0)) or 
    (@p5 = 2   and S.vNocne > 0) 
)
and ('@p6' = '*' or '@p6' = S.KierLogo)

and (
    (@p7=1 and S.cc not in (select cc from CC where GrSplitu is not null)) or   -- bez 019, podzielone
    (@p7=0 and S.Typ < 10)                                                      -- z 019, niepodzielone 
)
and (@p8=1 or S.Typ not in (3,13))  -- bez dopelnien

group by S.Logo, S.Pracownik, 
		S.Dzial, S.Stanowisko, S.ImpStanowisko,
		S.Class, 
		--S.CC, CC.Nazwa, 
		S.KierLogo, S.Kierownik, S.StawkaGodz 
order by S.Logo
"/>
</asp:Content>

