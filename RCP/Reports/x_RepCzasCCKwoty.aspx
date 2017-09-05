<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="x_RepCzasCCKwoty.aspx.cs" Inherits="HRRcp.Reports.RepCzasCCKwoty" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">    
    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="Podzia³ godzin czasu pracy na cc"
        Title2="select 'Okres: ' + 
convert(varchar(10), convert(datetime, '@p1'),20) + ' do ' +
convert(varchar(10), convert(datetime, '@p2'),20)"

        SQL1="
join ccPrawa PR on PR.UserId = @UserId and PR.IdCC = CC.Id
join ccPrawa PC on PC.UserId = @UserId and PC.CC = S.Class
        "
        SQL2 = "
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

select CC.cc as [cc|RepCCPracownicy @p1 @p2 @cc 0 *|Pracownicy pracuj¹cy na cc],  
CC.Nazwa,
ISNULL(round(sum(S.vCzasZm),2),0) as [CzasZm:N0.00S|RepCCPracownicy @p1 @p2 @cc 1 *|Pracownicy pracuj¹cy na zmianie na cc], 
ISNULL(round(sum(S.vNadg50),2),0) as [Nadg 50:N0.00S|RepCCPracownicy @p1 @p2 @cc 50 *|Pracownicy z nadgodzinami 50 na cc], 
ISNULL(round(sum(S.vNadg100),2),0) as [Nadg 100:N0.00S|RepCCPracownicy @p1 @p2 @cc 100 *|Pracownicy z nadgodzinami 100 na cc], 
ISNULL(round(sum(S.vNocne),2),0) as [Nocne:N0.00S|RepCCPracownicy @p1 @p2 @cc 2 *|Pracownicy pracuj¹cy w czasie nocnym na cc] 
    @SQL2
from CC 
left outer join VrepDaneMPK S on S.cc = CC.cc and S.Data BETWEEN @dod AND @ddo
    @SQL1
where @ddo between CC.AktywneOd and ISNULL(CC.AktywneDo, '20990909')
group by CC.cc, CC.Nazwa
order by CC.cc
"/>
</asp:Content>
