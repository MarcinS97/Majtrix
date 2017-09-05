<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RepStolowkaPrac.aspx.cs" Inherits="HRRcp.Reports.RepStolowkaPrac" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<%--
raport - pracownicy w zakresie dat
p1 - od
p2 - do
p3 - F S FS 2 *
p4 - okres od
p5 - okres do
--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">    
    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="select 'Osoby korzystaj¹ce ze sto³ówki: ' + case '@p3' 
when 'F' then 'Fordoñska' 
when 'S' then 'Szajnochy' 
when 'FS' then 'Fordoñska + Szajnochy' 
when '2' then 'Fordoñska + Szajnochy PODWÓJNIE' 
else '' end"
        Title2="select case when '@p1' = '@p2' then 'Dzieñ: @p1' else 'Okres: @p1 do @p2' end"
        SQL1=""
        P2=""
        SQL="
declare 
	@dataOd datetime,
	@dataDo datetime
set @dataOd = '@p1'
set @dataDo = '@p2'

select 
--convert (varchar(10), O.Data, 20) as [Data|RepStolowkaDetale @Data @Data * @id|Szczegó³y pracownika / karty w dniu],
--convert (varchar(10), O.Data, 20) as [Data],
convert (varchar(10), O.Data, 20) as 
    [Data|RepStolowkaSzczegoly @Data @Data * @Logo @ECUserId|Poka¿ godziny i szczegó³y w dniu],
ISNULL(O.Pracownik, '- karta: ' + convert(varchar, ECUserId) + ' -') as 
    [Pracownik|RepStolowkaPracownik @p4 @p5 * @Logo @ECUserId|Poka¿ dni], 
O.Logo, 
O.ECUserId as 
    [ECUserId:N|RepStolowkaSzczegoly @p4 @p5 * @Logo @ECUserId|Poka¿ godziny i szczegó³y w okresie],
O.ObiadF as 
    --[Fordoñska:N0S|RepStolowkaSzczegoly @Data @Data F @Logo @ECUserId|Posi³ek - Fordoñska],
    [Fordoñska:N0S|RepStolowkaSzczegoly @Data @Data * @Logo @ECUserId|Poka¿ godziny i szczegó³y w dniu],
--ObiadFsum as [Fordoñska(suma):N0S], 
O.OdmowaFsum as 
    --[Odmowa Ford.:N0S|RepStolowkaSzczegoly @Data @Data OF @Logo @ECUserId|Odmowa - Fordoñska], 
    [Odmowa Ford.:N0S|RepStolowkaSzczegoly @Data @Data * @Logo @ECUserId|Poka¿ godziny i szczegó³y w dniu], 
O.ObiadS as 
    --[Szajnochy:N0S|RepStolowkaSzczegoly @Data @Data S @Logo @ECUserId|Posi³ek - Szajnochy],
    [Szajnochy:N0S|RepStolowkaSzczegoly @Data @Data * @Logo @ECUserId|Poka¿ godziny i szczegó³y w dniu],
--ObiadSsum as [Szajnochy(suma):N0S], 
OdmowaSsum as 
    --[Odmowa Szajn.:N0S|RepStolowkaSzczegoly @Data @Data OS @Logo @ECUserId|Odmowa - Szajnochy], 
    [Odmowa Szajn.:N0S|RepStolowkaSzczegoly @Data @Data * @Logo @ECUserId|Poka¿ godziny i szczegó³y w dniu], 
--ObiadF + ObiadS as [Fordoñska+Szajnochy:N0S],
case when ObiadF &gt; 0 and ObiadS &gt; 0 then 1 else 0 end as  
    --[Podwójnie:N0S|RepStolowkaSzczegoly @Data @Data 2 @Logo @ECUserId|Posi³ki podwójnie] 
    [Podwójnie:N0S|RepStolowkaSzczegoly @Data @Data * @Logo @ECUserId|Poka¿ godziny i szczegó³y w dniu] 
from VObiadyDzienPracownik O 
where O.Data between @dataOd and @dataDo 
and (
'@p3' = 'F' and (ObiadF &gt; 0) or
'@p3' = 'S' and (ObiadS &gt; 0) or
'@p3' = 'FS' and (ObiadF &gt; 0 or ObiadS &gt; 0) or
'@p3' = '2' and (ObiadF &gt; 0 and ObiadS &gt; 0) or
'@p3' = '*' and (ObiadF &gt; 0 or ObiadS &gt; 0 or OdmowaFsum &gt; 0 or OdmowaSsum &gt; 0)
)
order by O.Data, O.Pracownik, O.ECUserId
"/>
</asp:Content>

