<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepPodzialCC_old.ascx.cs" Inherits="HRRcp.Controls.RepPodzialCC_old" %>
<%@ Register src="Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>

<table runat="server" class="okres_navigator noprint" visible="false">
    <tr>
        <td>
            <asp:RadioButtonList ID="rblClass1" runat="server" AutoPostBack="True" Visible="false"
                onselectedindexchanged="rblClass_SelectedIndexChanged" 
                RepeatDirection="Horizontal"
                CssClass="rbl">
                <asp:ListItem Selected="True" Text="Wszyscy pracownicy" Value="all" />
                <asp:ListItem Text="DL" Value="DL" />
                <asp:ListItem Text="BUIL" Value="BUIL" />
            </asp:RadioButtonList>
            <asp:RadioButtonList ID="rblClass" runat="server" AutoPostBack="True" 
                onselectedindexchanged="rblClass_SelectedIndexChanged" 
                RepeatDirection="Horizontal" CssClass="rbl"
                DataSourceID="SqlDataSource1" DataTextField="Class" DataValueField="Id" >
                <asp:ListItem Selected="True" Text="Wszyscy pracownicy" Value="all" />
                <asp:ListItem Text="DL" Value="DL" />
                <asp:ListItem Text="BUIL" Value="BUIL" />
            </asp:RadioButtonList>
        </td>
    </tr>
</table>

<asp:CheckBoxList ID="cblClass" runat="server" DataSourceID="SqlDataSource1" Visible="false"
    DataTextField="Class" DataValueField="Id" 
    AutoPostBack="true"
    onselectedindexchanged="clbClass_SelectedIndexChanged">
</asp:CheckBoxList>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select 'Wszyscy pracownicy' as Class, 'all' as Id, 1 as Sort
union
select distinct Class, Class as Id, 
case Class
    when 'DL' then 2
    when 'BUIL' then 3 
    else 9
end as Sort 
from PodzialLudziImport
order by Sort, Class">
</asp:SqlDataSource>

<asp:Menu ID="tabWyniki" CssClass="printoff" runat="server" Orientation="Horizontal" Visible="false"
    onmenuitemclick="tabWyniki_MenuItemClick" >
    <StaticMenuStyle CssClass="tabsStrip" />
    <StaticMenuItemStyle CssClass="tabItem" />
    <StaticSelectedStyle CssClass="tabSelected" />
    <StaticHoverStyle CssClass="tabHover" />
    <Items>
        <asp:MenuItem Text="Wszyscy pracownicy" Value="all" Selected="True" ></asp:MenuItem>
        <asp:MenuItem Text="DL" Value="DL" ></asp:MenuItem>
        <asp:MenuItem Text="BUIL" Value="BUIL" ></asp:MenuItem>
    </Items>
    <StaticItemTemplate>
        <div class="tabCaption">
            <div class="tabLeft">
                <div class="tabRight">
                    <asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Text") %>' />
                </div>
            </div>
        </div>
    </StaticItemTemplate>
</asp:Menu>

<uc1:cntReport ID="cntReport1" runat="server" 
    Title="Podział czasu pracy na CC - Okresy rozliczeniowe"
    Title2="select 
case when SUBSTRING(Rights,7,1) = '1' then '' else 'Brak prawa dostępu do raportu; ' end + 
case when SUBSTRING(Rights,8,1) = '1' then 'Uprawnienia: Dostęp do kosztów; ' else 'Uprawnienia: ' end + 
case when SUBSTRING(Rights,9,1) = '1' then 'Dostęp do wszystkich pracowników' else 'Dostęp do pracowników wg klasyfikacji:' end  
from Pracownicy where Id=@UserId
    "
    CssClass="report_page RepPodzialCC"
    SQL1="
join ccPrawa PCC on PCC.UserId = @UserId and PCC.CC = S.cc
join ccPrawa PCL on PCL.UserId = @UserId and PCL.CC = I.Class
    "
    P2="
,ISNULL(round(sum(S.vNadg50 * round(ISNULL(PO.Stawka,P.Stawka)/(CN.DniPrac*8*cast(ISNULL(PO.EtatL,P.EtatL) as float)/ISNULL(PO.EtatM, P.EtatM)),4)*1.5),2),0) as [Kwota 50:N0], 
ISNULL(round(sum(S.vNadg100 * round(ISNULL(PO.Stawka,P.Stawka)/(CN.DniPrac*8*cast(ISNULL(PO.EtatL,P.EtatL) as float)/ISNULL(PO.EtatM, P.EtatM)),4)*2),2),0) as [Kwota 100:N0],
ISNULL(round(sum(S.vNocne * R.StawkaNocna),2),0) as [Kwota Nocne:N0],

ISNULL(round(sum(S.vNadg50 * round(ISNULL(PO.Stawka,P.Stawka)/(CN.DniPrac*8*cast(ISNULL(PO.EtatL,P.EtatL) as float)/ISNULL(PO.EtatM, P.EtatM)),4)*1.5),2),0) +
ISNULL(round(sum(S.vNadg100 * round(ISNULL(PO.Stawka,P.Stawka)/(CN.DniPrac*8*cast(ISNULL(PO.EtatL,P.EtatL) as float)/ISNULL(PO.EtatM, P.EtatM)),4)*2),2),0)
    as [Koszt 50+100:N0],
 
ISNULL(round(sum(S.vNadg50 * round(ISNULL(PO.Stawka,P.Stawka)/(CN.DniPrac*8*cast(ISNULL(PO.EtatL,P.EtatL) as float)/ISNULL(PO.EtatM, P.EtatM)),4)*1.5),2),0) +
ISNULL(round(sum(S.vNadg100 * round(ISNULL(PO.Stawka,P.Stawka)/(CN.DniPrac*8*cast(ISNULL(PO.EtatL,P.EtatL) as float)/ISNULL(PO.EtatM, P.EtatM)),4)*2),2),0) +
ISNULL(round(sum(S.vNocne * R.StawkaNocna),2),0) as [Suma kosztów:N0]
    "
    SQL3="and S.cc not in (select cc from CC where GrSplitu is not null)"
    P3="and S.Typ < 10"
    SQL="
SET LANGUAGE Polish

select
convert(varchar(10), R.DataOd, 120) as [Od:-],    
convert(varchar(10), ISNULL(R.DataBlokady, R.DataDo), 120) as [Do:-],    

convert(varchar(10), R.DataDo, 120) as [DoX:-],    

dbo.FirstUpper(DATENAME(MONTH, R.DataDo)) + ' ' + convert(varchar,DATEPART(YEAR, R.DataDo)) as [MiesRok:-], 
dbo.FirstUpper(DATENAME(MONTH, R.DataDo)) + ' ' + convert(varchar,DATEPART(YEAR, R.DataDo)) 
as [Okres rozliczeniowy|Reports/RepCCTygodnie @Od @Do @MiesRok|Podział tygodniowy],

convert(varchar(10), R.DataOd, 20) as [Od],
convert(varchar(10), ISNULL(R.DataBlokady, R.DataDo), 20) as [Do],

'cc+019'   as [Raporty|Reports/RepCC @Od @Do 0 1|Podział na cc w okresie rozliczeniowym bez podzielonego 019],
'cc'       as [|Reports/RepCC @Od @Do 1 1|Podział na cc w okresie rozliczeniowym z 019 podzielonym],
'klas'     as [|Reports/RepCCKlasyfikacja @Od @Do 1 1|Podział wg klasyfikacji pracowników],
'dop'      as [|Reports/RepCCDopelnienia @Od @Do * * 0|Pracownicy mający dopełnienia z zaokrągleń],
'nadmin'   as [|Reports/RepCCNadminuty @Od @Do * * *|Pracownicy mający nadminuty],
'splity'   as [|Reports/RepCCSplity2 @Od @Do|Podział ludzi na cc w okresie rozliczeniowym],
'prac+019' as [|Reports/RepCCPracownicyCC @Od @Do 0 1 * *|Podział czasu pracy ludzi na cc w okresie rozliczeniowym bez podzielonego 019],
'prac'     as [|Reports/RepCCPracownicyCC @Od @Do 1 1 * *|Podział czasu pracy ludzi na cc w okresie rozliczeniowym],

ISNULL(round(sum(S.vCzasZm),0),0)  as [Czas Zm:N0|Reports/RepCCPracownicy @Od @Do * * 0 * 1 1|Pracownicy], 
ISNULL(round(sum(S.vNadg50),0),0)  as [Nadg 50:N0|Reports/RepCCPracownicy @Od @Do * * 50 * 1 1|Pracownicy z nadgodzinami 50], 
ISNULL(round(sum(S.vNadg100),0),0) as [Nadg 100:N0|Reports/RepCCPracownicy @Od @Do * * 100 * 1 1|Pracownicy z nadgodzinami 100],

ISNULL(round(sum(S.vNadg50),0),0)  +
ISNULL(round(sum(S.vNadg100),0),0) as [Nadg 50+100:N0|Reports/RepCCPracownicy @Od @Do * * 150 * 1 1|Pracownicy z nadgodzinami (50+100)],

ISNULL(round(sum(S.vNocne),0),0)   as [Nocne:N0|Reports/RepCCPracownicy @Od @Do * * 2 * 1 1|Pracownicy pracujący w czasie nocnym]
@SQL2
from DaneMPK S
left outer join OkresyRozl R on S.Data BETWEEN R.DataOd AND R.DataDo
left outer join PracownicyOkresy PO on PO.IdOkresu = R.Id and PO.Id = S.IdPracownika
left outer join Pracownicy P on P.Id = case when R.Archiwum = 1 then -1 else S.IdPracownika end
left outer join PodzialLudziImport I on I.KadryId = S.NR_EW and S.Data between I.OkresOd and ISNULL(I.OkresDo, '20990909')
@SQL1
left outer join CzasNom CN on CN.Data = DATEADD(dd, -DAY(R.DataDo) + 1, R.DataDo)
where R.DataOd &gt;= '20121121'

and S.cc not in (select cc from CC where GrSplitu is not null)

group by R.DataOd, R.DataDo, R.DataBlokady
order by R.DataOd
"/>

<!--
Opis raportu
- pokazuje dane za miesiąc (od początku okresu do końca lub ostatniego zatrzaśniętego tygodnia
- pomija wartości na 019 bo są one rozłożone na inne cc splitem 019i na jedno wycjodzi
- pokazuje z dopełnieniami do pełnych godzin - stąd wartości powinny być zgodne z raportem nadgodzin
-->
<br />
<asp:Button ID="btUprawnienia" runat="server" Visible="false" CssClass="button100" PostBackUrl="~/Reports/RepCCUprawnienia.aspx" Text="Uprawnienia" />