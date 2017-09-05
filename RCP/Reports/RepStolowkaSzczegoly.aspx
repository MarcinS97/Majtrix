<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RepStolowkaSzczegoly.aspx.cs" Inherits="HRRcp.Reports.RepStolowkaSzczegoly" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<%--
raport - pracownicy w zakresie dat
p1 - od
p2 - do
p3 - F S FS 2 *
p4 - Logo
p5 - ECUserId
--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">    
    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="select 
case when '@p4' = ''  
    then 'Korzystanie ze sto³ówki - szczegó³y: karta nr @p5, pracownik nieokreœlony' 
    else 'Korzystanie ze sto³ówki - szczegó³y: ' + (select KadryId + ' ' + Nazwisko + ' ' + Imie from Pracownicy where KadryId = '@p4')
end"
        Title2="select 
case '@p3' 
    when 'F'  then 'Sto³ówka: Fordoñska' 
    when 'S'  then 'Sto³ówka: Szajnochy' 
    when 'OF' then 'Odmowa: Fordoñska' 
    when 'OS' then 'Odmowa: Szajnochy' 
    when 'FS' then 'Sto³ówka: Fordoñska + Szajnochy' 
    when '2'  then 'Sto³ówka: Fordoñska + Szajnochy PODWÓJNIE' 
    when '*'  then ''
    else           'Sto³ówka: @p3' 
end"
        Title3="select case when '@p1' = '@p2' then 'Dzieñ: @p1' else 'Okres: @p1 do @p2' end"
        SQL3="
join (
	select Data, ECUserId from VObiadyDzienPracownik O 
	where O.Data between @dataOd and @dataDo 
	and (O.ObiadF &gt; 0 and O.ObiadS &gt; 0)
) A on A.Data = O.Data and A.ECUserId = O.ECUserId
        "    
        SQL4="
and (
'@p3' = 'F'  and (ObiadF &gt; 0) or
'@p3' = 'S'  and (ObiadS &gt; 0) or
'@p3' = 'OF' and (OdmowaF &gt; 0) or
'@p3' = 'OS' and (OdmowaS &gt; 0) or
'@p3' = 'FS' and (ObiadF &gt; 0 or ObiadS &gt; 0) or
'@p3' = '*'  and (ObiadF &gt; 0 or ObiadS &gt; 0 or OdmowaF &gt; 0 or OdmowaS &gt; 0)
)
        "
        SQL="
declare 
    @dataOd datetime,
    @dataDo datetime
set @dataOd = '@p1'
set @dataDo = '@p2'

select 
convert (varchar(10), O.Data, 20) as [Data:-], 
O.Czas as [Czas], 
ISNULL(O.Pracownik, '- karta: ' + convert(varchar, O.ECUserId) + ' -') as [Pracownik|RepStolowkaPracownik @p1 @p2 * @Logo @ECUserId|Poka¿ dni], 
O.Logo, 
O.ECUserId as [ECUserId:N], 
O.ECUniqueId as [ECUniqueId:N],  
O.ECReaderId as [ECReaderId:N], 
O.ECCode as [ECCode:N],
O.ObiadF as [Fordoñska:N0S], 
O.OdmowaF as [Odmowa Ford.:N0S], 
O.ObiadS as [Szajnochy:N0S], 
O.OdmowaS as [Odmowa Szajn.:N0S]
from VObiady O 
@SQL3
where O.Data between @dataOd and @dataDo and O.ECCode in (1,2)
and (O.ObiadF &gt; 0 or O.ObiadS &gt; 0 or O.OdmowaF &gt; 0 or O.OdmowaS &gt; 0)
and (
'@p4' &lt;&gt; '' and O.Logo = '@p4' or
'@p4' = '' and O.ECUserId = @p5
)
@SQL4
--order by O.Pracownik, O.ECUserId, O.Czas
order by O.Czas
"/>
</asp:Content>
