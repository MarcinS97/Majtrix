<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RepCCPracDniCC.aspx.cs" Inherits="HRRcp.Reports.RepCCPracDniCC" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<%--
raport - 
p1 - od
p2 - do; p2 = p1 = w dniu
p3 - zakres: 0 - wszystko, 1 - czaszm, 50 - nadg 50, 100 - nadg 100, 150 - nadg łącznie, 2 - nocne, 3 - wszystkie dopełnienia
p4 - logo pracownika
p5 - split 019: 1 podzielony, 0 niepodzielony
p6 - dopełnienia: 1 z dopełnieniami, 0 bez dopełnień (na razie tylko 1)
--%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="select 'Czas przepracowany na CC - ' + KadryId + ' ' + Nazwisko + ' ' + Imie from Pracownicy where KadryId='@p4'"
        Title2="select
case when '@p1' = '@p2' then 
        'Dzień: @p1'
else    'Okres: @p1 do @p2'
end"
        Title3="select
case when @p5=1 then '019 podzielone' else '019 niepodzielone' end +
case when @p6=1 then '' else ', bez dopełnień wynikających z zaokrągleń' end
        "
        SQL1="
join ccPrawa PR on PR.UserId = @UserId and PR.CC = S.CC
join ccPrawa PC on PC.UserId = @UserId and PC.CC = S.Class
        "
        P2="
round(S.StawkaGodz, 4) as [Stawka godz.:N0.0000],
ISNULL(round(S.vNadg50  * S.StawkaGodz * 1.5,2),0) as [Kwota 50:N0.00S], 
ISNULL(round(S.vNadg100 * S.StawkaGodz * 2,  2),0) as [Kwota 100:N0.00S],
ISNULL(round(S.vNocne   * S.StawkaNocna,     2),0) as [Kwota Nocne:N0.00S],

ISNULL(round(S.vNadg50  * S.StawkaGodz * 1.5,2),0) +
ISNULL(round(S.vNadg100 * S.StawkaGodz * 2,  2),0) as [Koszt 50+100:N0.00S],

ISNULL(round(S.vNadg50  * S.StawkaGodz * 1.5,2),0) +
ISNULL(round(S.vNadg100 * S.StawkaGodz * 2,  2),0) +
ISNULL(round(S.vNocne   * S.StawkaNocna,     2),0) as [Suma kosztów:N0.00S],
        "
        SQL="
declare @dataOd datetime
declare @dataDo datetime
set @dataOd = '@p1'
set @dataDo = '@p2'

SET LANGUAGE Polish

select 
convert(varchar(10), S.Data, 20) + ' ' + 
	UPPER(substring(DATENAME(DW,S.Data),1,1)) +
	LOWER(substring(DATENAME(DW,S.Data),2,2)) as [Data],

S.cc, CC.Nazwa, 
S.vCzasZm as [Czas Zm:N0.00S],
S.vNadg50 as [Nadg 50:N0.00S], 
S.vNadg100 as [Nadg 100:N0.00S], 
S.vNadg50 +
S.vNadg100 as [Nadg 50+100:N0.00S], 
S.vNocne as [Nocne:N0.00S],
    @SQL2
S.TypOpis as [Typ]
from VrepDaneMPK S
left outer join CC on CC.cc = S.CC
    @SQL1
where S.Data between @dataOd and @dataDo and S.Logo='@p4'
and (
    (@p3 = 0) or 
    (@p3 = 1 and S.vCzasZm > 0) or 
    (@p3 = 50 and S.vNadg50 > 0) or 
    (@p3 = 100 and S.vNadg100 > 0) or 
    (@p3 = 150 and (S.vNadg50 > 0 or S.vNadg100 > 0)) or 
    (@p3 = 2 and S.vNocne > 0) or
    (@p3 = 3 and S.Typ in (3,13))
)
and (
    (@p5=1 and S.cc not in (select cc from CC where GrSplitu is not null)) or   -- bez 019, podzielone
    (@p5=0 and S.Typ < 10)                                                      -- z 019, niepodzielone 
)
and (@p6=1 or S.Typ not in (3,13))  -- bez dopelnien
order by S.Data
"/>
</asp:Content>

