<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RepCCTygodnie.aspx.cs" Inherits="HRRcp.Reports.RepCCTygodnie" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">    
    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="Podzia³ czasu pracy na CC - Podzia³ tygodniowy"
        Title2="Okres rozliczeniowy: @p3"
        Title3="019 niepodzielone, bez dope³nieñ wynikaj¹cych z zaokr¹gleñ"
        Description=""
        SQL1="
join ccPrawa PCC on PCC.UserId = @UserId and PCC.CC = S.cc
join ccPrawa PCL on PCL.UserId = @UserId and PCL.CC = S.Class
    "
        P2="
,ISNULL(round(sum(S.vNadg50 * S.StawkaGodz * 1.5),2),0) as [Kwota 50:N0.00S], 
ISNULL(round(sum(S.vNadg100 * S.StawkaGodz * 2  ),2),0) as [Kwota 100:N0.00S],
ISNULL(round(sum(S.vNocne   * S.StawkaNocna     ),2),0) as [Kwota Nocne:N0.00S],
ISNULL(round(sum(S.vNadg50  * S.StawkaGodz * 1.5),2),0) +
ISNULL(round(sum(S.vNadg100 * S.StawkaGodz * 2  ),2),0) as [Koszt 50+100:N0.00S],
ISNULL(round(sum(S.vNadg50  * S.StawkaGodz * 1.5),2),0) +
ISNULL(round(sum(S.vNadg100 * S.StawkaGodz * 2  ),2),0) +  
ISNULL(round(sum(S.vNocne   * S.StawkaNocna     ),2),0) as [Suma kosztów:N0.00S]
    "
        SQL="
declare @dataOd datetime
declare @dataDo datetime
set @dataOd = '@p1'
set @dataDo = '@p2'
        
declare @df int 
set @df = @@DATEFIRST
set datefirst 1 --pon

select
DATEPART(wk,S.Data) as [Tydzieñ],
case when DATEADD(DAY, -DATEPART(dw, @dataOd) + 1 + (DATEPART(wk, Data) - DATEPART(wk, @dataOd)) * 7, @dataOd) < @dataOd then convert(varchar(10), @dataOd, 20) else convert(varchar(10), 
          DATEADD(DAY, -DATEPART(dw, @dataOd) + 1 + (DATEPART(wk, Data) - DATEPART(wk, @dataOd)) * 7, @dataOd), 20) end 
as Od, 
case when DATEADD(DAY, -DATEPART(dw, @dataOd) + (DATEPART(wk, Data) - DATEPART(wk, @dataOd) + 1) * 7, @dataOd) > @dataDo then convert(varchar(10), @dataDo, 20) else convert(varchar(10), 
	      DATEADD(DAY, -DATEPART(dw, @dataOd) + (DATEPART(wk, Data) - DATEPART(wk, @dataOd) + 1) * 7, @dataOd), 20) end
as Do, 
'cc+019' as [|RepCC @Od @Do 0 0|Podzia³ na cc w tygodniu bez podzielonego 019],
'cc' as [|RepCC @Od @Do 1 0|Podzia³ na cc w tygodniu z 019 podzielonym],
'klas' as [|RepCCKlasyfikacja @Od @Do 0 0|Podzia³ czasu pracy wg klasyfikacji pracowników],
ISNULL(round(sum(S.vCzasZm), 2),0) as [Czas Zm:N0.00S|RepCCPracownicy @Od @Do * * 0 * 0 0|Pracownicy], 
ISNULL(round(sum(S.vNadg50), 2),0) as [Nadg 50:N0.00S|RepCCPracownicy @Od @Do * * 50 * 0 0|Pracownicy z nadgodzinami 50], 
ISNULL(round(sum(S.vNadg100),2),0) as [Nadg 100:N0.00S|RepCCPracownicy @Od @Do * * 100 * 0 0|Pracownicy z nadgodzinami 100],
ISNULL(round(sum(S.vNadg50), 2),0) +
ISNULL(round(sum(S.vNadg100),2),0) as [Nadg 50+100:N0.00S|RepCCPracownicy @Od @Do * * 100 * 0 0|Pracownicy z nadgodzinami (50+100)],
ISNULL(round(sum(S.vNocne),  2),0) as [Nocne:N0.00S|RepCCPracownicy @Od @Do * * 2 * 0 0|Pracownicy pracuj¹cy w czasie nocnym]
    --@SQL2
from VrepDaneMPK S
	--@SQL1
where S.Data between @dataOd and @dataDo  

and S.Typ < 10 and S.Typ <> 3  --13

group by DATEPART(wk,S.Data)
order by DATEPART(wk,S.Data)

set datefirst @df
"/>
</asp:Content>

<%--
Opis raportu:
- bez wartoœci podzielonych splitem 019
- wszystko co na 019
- bez dope³nieñ do pe³nych godzin


    <table name="report" class="rep_opis">
        <tr>
            <th>
                Opis raportu:
            </th>
        </tr>
        <tr>
            <td>
                - bez wartoœci podzielonych splitem 019<br />
                - wszystko co na 019<br />
                - bez dope³nieñ do pe³nych godzin<br />
            </td>
        </tr>
    </table>
--%>