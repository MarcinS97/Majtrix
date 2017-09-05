<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RepCCNadminuty.aspx.cs" Inherits="HRRcp.Reports.RepCCNadminuty" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<%--
raport - 
p1 - od
p2 - do
p3 - cc lub *
p4 - class lub *
p5 - kierowik id
--%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="select 'Pracowicy z nadminutami' + 
case 
    when '@p3' <> '*' then ' - ' + (select cc + ' ' + Nazwa from CC where cc = '@p3')
    when '@p4' <> '*' then ' - @p4'
    when '@p5' <> '*' then ' - Kierownik: ' + (select Nazwisko + ' ' + Imie from Pracownicy where KadryId = '@p5')
    else ''
end
        "
        Title2="Okres: @p1 do @p2"
        SQL1="
join ccPrawa PR on PR.UserId = @UserId and PR.CC = S.CC
join ccPrawa PC on PC.UserId = @UserId and PC.CC = S.Class
        "
        P2="
,round(D.StawkaGodz, 4) as [Stawka godz.:N0.0000],
ISNULL(round(sum(D.vNadm50  * D.StawkaGodz * 1.5),2),0) as [Kwota 50:N0.00S], 
ISNULL(round(sum(D.vNadm100 * D.StawkaGodz * 2)  ,2),0) as [Kwota 100:N0.00S],
ISNULL(round(sum(D.vNadmNocne * D.StawkaNocna)   ,2),0) as [Kwota Nocne:N0.00S],

ISNULL(round(sum(D.vNadm50  * D.StawkaGodz * 1.5),2),0) +
ISNULL(round(sum(D.vNadm100 * D.StawkaGodz * 2)  ,2),0) as [Koszty 50+100:N0.00S],

ISNULL(round(sum(D.vNadm50  * D.StawkaGodz * 1.5),2),0) +
ISNULL(round(sum(D.vNadm100 * D.StawkaGodz * 2)  ,2),0) +
ISNULL(round(sum(D.vNadmNocne * D.StawkaNocna)   ,2),0) as [Suma kosztów:N0.00S]
        "
        CMD1="select case when '@p5'='*' then ',D.Kierownik as [Kierownik|RepCCNadminuty @p1 @p2 * * @knrew|Wszyscy pracownicy z nadminutami kierownika], D.KierLogo as [Nr ew. ]' else '' end"
        SQL="
declare @dataOd datetime
declare @dataDo datetime
set @dataOd = '@p1'
set @dataDo = '@p2'

select  
	S.Data, 
	S.Logo, S.Pracownik, 
	S.KierLogo, S.Kierownik,
	S.Dzial,
	ISNULL(S.Stanowisko, S.ImpStanowisko) as [Stanowisko],
	S.Class,
	S.StawkaGodz,
	S.StawkaNocna,
	
	round(sum(S.vNadg50),4) as vNadg50, 
	--round(sum(S.vNadg50) - FLOOR(sum(S.vNadg50)),4) as vNadm50,  
	round(sum(S.vNadg50),4) - FLOOR(round(sum(S.vNadg50),4)) as vNadm50,  
	
	round(sum(S.vNadg100),4) as vNadg100, 
	--round(sum(S.vNadg100) - FLOOR(sum(S.vNadg100)),4) as vNadm100,
	round(sum(S.vNadg100),4) - FLOOR(round(sum(S.vNadg100),4)) as vNadm100,
	
	round(sum(S.vNocne),4) as vNocne, 
	--round(sum(S.vNocne) - FLOOR(sum(S.vNocne)),4) as vNadmNocne,
	round(sum(S.vNocne),4) - FLOOR(round(sum(S.vNocne),4)) as vNadmNocne

	/*
	round(sum(S.vNadg50),4) as vNadg50, 
	round(sum(S.vNadg50) - FLOOR(sum(S.vNadg50)),4) as vNadm50,  
	round(sum(S.vNadg100),4) as vNadg100, 
	round(sum(S.vNadg100) - FLOOR(sum(S.vNadg100)),4) as vNadm100,
	round(sum(S.vNocne),4) as vNocne, 
	round(sum(S.vNocne) - FLOOR(sum(S.vNocne)),4) as vNadmNocne
	*/
into #tmpNadminuty
from VrepDaneMPK S
where S.Data between @dataOd and @dataDo  

and S.Typ not in (3,13)  --bez dopełnień
--and S.Typ not in (11,12,17)  --rozdzielonych splitem


and ('@p3' = '*' or '@p3' = S.cc)
--and ('@p4' = '*' or '@p4' = S.Class)
--and ('@p5' = '*' or '@p5' = S.KierLogo)
group by S.Data, S.Logo, S.Pracownik, S.KierLogo, S.Kierownik,S.Dzial,S.Stanowisko,S.ImpStanowisko,S.Class,S.StawkaGodz,S.StawkaNocna
having 
round(sum(S.vNadg50),4) - FLOOR(round(sum(S.vNadg50),4)) <> 0 
or round(sum(S.vNadg100),4) - FLOOR(round(sum(S.vNadg100),4)) <> 0
or round(sum(S.vNocne),4) - FLOOR(round(sum(S.vNocne),4)) <> 0

/*
round(sum(S.vNadg50) - FLOOR(sum(S.vNadg50)),4) <> 0 
or round(sum(S.vNadg100) - FLOOR(sum(S.vNadg100)),4) <> 0
or round(sum(S.vNocne) - FLOOR(sum(S.vNocne)),4) <> 0
*/

select 
	D.Logo as [nrew:-], 
	D.KierLogo as [knrew:-],	
	D.Logo            as [Nr ew.],  
	--D.Pracownik     as [Pracownik|RepCCPracDni @p1 @p2 * 0 @nrew 1 1|Czas pracy w poszczególne dni], 
	D.Pracownik       as [Pracownik|RepCCPracCC @p1 @p2 * 0 @nrew 1 1|Czas przepracowany na wszystkie cc],
	D.Dzial           as [Dział],
	D.Stanowisko      as [Stanowisko],
	D.Class           as [Klasyfikacja],
	SUM(D.VNadm50)    as [Nadm 50:N0.00S|RepCCPracDni @p1 @p2 @p3 0 @nrew 1 1|Czas przepracowany],
	SUM(D.vNadm100)   as [Nadm 100:N0.00S|RepCCPracDni @p1 @p2 @p3 0 @nrew 1 1|Czas przepracowany],
	SUM(D.VNadm50) +
	SUM(D.vNadm100)   as [Nadm 50+100:N0.00S|RepCCPracDni @p1 @p2 @p3 0 @nrew 1 1|Czas przepracowany],
	SUM(D.vNadmNocne) as [Nocne:N0.00S|RepCCPracDni @p1 @p2 @p3 0 @nrew 1 1|Czas przepracowany]
    /*
    @SQL2
    */
    @CMD1
from #tmpNadminuty D
where
    ('@p4' = '*' or '@p4' = D.Class)
and ('@p5' = '*' or '@p5' = D.KierLogo)
group by D.Logo, D.Pracownik, D.KierLogo, D.Kierownik,D.Dzial,D.Stanowisko,D.Class,D.StawkaGodz,D.StawkaNocna
order by D.Logo

drop table #tmpNadminuty
"/>
</asp:Content>

<%--
przy zakresie na kierownika masakrycznie wolne

declare @dataOd datetime
declare @dataDo datetime
set @dataOd = '@p1'
set @dataDo = '@p2'

select 
	D.Logo as [nrew:-], 
	D.KierLogo as [knrew:-],	
	D.Logo, 
	D.Pracownik as [Pracownik|RepCCPracDni @p1 @p2 * 0 @nrew 1 1|Czas pracy w poszczególne dni], 
	D.Dzial as [Dział],
	D.Stanowisko as [Stanowisko],
	D.Class as [Klasyfikacja],
	SUM(D.VNadm50)    as [Nadm 50:N0.00S|RepCCPracDni @p1 @p2 @p3 0 @nrew 1 1|Czas przepracowany],
	SUM(D.vNadm100)   as [Nadm 100:N0.00S|RepCCPracDni @p1 @p2 @p3 0 @nrew 1 1|Czas przepracowany],
	SUM(D.VNadm50) +
	SUM(D.vNadm100)   as [Nadm 50+100:N0.00S|RepCCPracDni @p1 @p2 @p3 0 @nrew 1 1|Czas przepracowany],
	SUM(D.vNadmNocne) as [Nocne:N0.00S|RepCCPracDni @p1 @p2 @p3 0 @nrew 1 1|Czas przepracowany],
    /*
    @SQL2
    */
    D.Kierownik as [Kierownik|RepCCNadminuty @p1 @p2 * * @knrew|Wszyscy pracownicy kierownika z nadminutami], 
    D.KierLogo as [Nr ew. ]	
from
(	
select  
	S.Data, 
	S.Logo, S.Pracownik, 
	S.KierLogo, S.Kierownik,
	S.Dzial,
	ISNULL(S.Stanowisko, S.ImpStanowisko) as [Stanowisko],
	S.Class,
	S.StawkaGodz,
	S.StawkaNocna,
	round(sum(S.vNadg50),4) as vNadg50, 
	round(sum(S.vNadg50) - FLOOR(sum(S.vNadg50)),4) as vNadm50,  
	round(sum(S.vNadg100),4) as vNadg100, 
	round(sum(S.vNadg100) - FLOOR(sum(S.vNadg100)),4) as vNadm100,
	round(sum(S.vNocne),4) as vNocne, 
	round(sum(S.vNocne) - FLOOR(sum(S.vNocne)),4) as vNadmNocne
from VrepDaneMPK S
where S.Data between @dataOd and @dataDo  

and S.Typ not in (3,13)  --bez dopełnień

and ('@p3' = '*' or '@p3' = S.cc)
and ('@p4' = '*' or '@p4' = S.Class)
and ('@p5' = '*' or '@p5' = S.KierLogo)
group by S.Data, S.Logo, S.Pracownik, S.KierLogo, S.Kierownik,S.Dzial,S.Stanowisko,S.ImpStanowisko,S.Class,S.StawkaGodz,S.StawkaNocna
having round(sum(S.vNadg50) - FLOOR(sum(S.vNadg50)),4) <> 0 
or round(sum(S.vNadg100) - FLOOR(sum(S.vNadg100)),4) <> 0
or round(sum(S.vNocne) - FLOOR(sum(S.vNocne)),4) <> 0
) as D
group by D.Logo, D.Pracownik, D.KierLogo, D.Kierownik,D.Dzial,D.Stanowisko,D.Class,D.StawkaGodz,D.StawkaNocna
order by D.Logo
--%>