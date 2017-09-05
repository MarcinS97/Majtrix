<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RepCC2.aspx.cs" Inherits="HRRcp.Reports.RepCC2" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<%--
raport - tylko u¿yte cc
p1 - od
p2 - do
p3 - class lub *
--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">    
    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="select 'Podzia³ czasu pracy na CC' + case when '@p3' = '*' then '' else ' - @p3'"
        Title2="Okres: @p1 do @p2"
        SQL1="
join ccPrawa PR on PR.UserId = @UserId and PR.IdCC = CC.Id
join ccPrawa PC on PC.UserId = @UserId and PC.CC = S.Class
        "
        P2="
,ISNULL(round(sum(S.vNadg50 * S.StawkaGodz * 1.5),2),0) as [Kwota 50:N0.00S], 
ISNULL(round(sum(S.vNadg100 * S.StawkaGodz * 2),2),0) as [Kwota 100:N0.00S],
ISNULL(round(sum(S.vNocne * S.StawkaNocna),2),0) as [Kwota Nocne:N0.00S],
ISNULL(round(sum(S.vNadg50 * S.StawkaGodz * 1.5),2),0) +
ISNULL(round(sum(S.vNadg100 * S.StawkaGodz * 2),2),0) +  
ISNULL(round(sum(S.vNocne * S.StawkaNocna),2),0) as [Suma kosztów:N0.00S]
        "
        SQL="
declare @dod datetime
declare @ddo datetime
set @dod = '@p1'
set @ddo = '@p2'

select CC.cc as [cc|RepCCPracownicy @p1 @p2 @cc @p3 0 *|Pracownicy pracuj¹cy na cc],  
CC.Nazwa,
ISNULL(round(sum(S.vCzasZm), 2),0) as [Czas Zm:N0.00S|RepCCPracownicy @p1 @p2 @cc @p3 1 *|Pracownicy pracuj¹cy na zmianie na cc], 
ISNULL(round(sum(S.vNadg50), 2),0) as [Nadg 50:N0.00S|RepCCPracownicy @p1 @p2 @cc @p3 50 *|Pracownicy z nadgodzinami 50 na cc], 
ISNULL(round(sum(S.vNadg100),2),0) as [Nadg 100:N0.00S|RepCCPracownicy @p1 @p2 @cc @p3 100 *|Pracownicy z nadgodzinami 100 na cc], 
ISNULL(round(sum(S.vNocne),  2),0) as [Nocne:N0.00S|RepCCPracownicy @p1 @p2 @cc @p3 2 *|Pracownicy pracuj¹cy w czasie nocnym na cc] 
    @SQL2
from VrepDaneMPK S 
left outer join CC on CC.cc = S.CC
    @SQL1
where S.Data BETWEEN @dod AND @ddo
and ('@p3' = '*' or '@p3' = S.Class)
group by S.cc, CC.Nazwa
order by S.cc
"/>
</asp:Content>
