<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepStolowka.ascx.cs" Inherits="HRRcp.Controls.Raporty.RepStolowka" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>
<%@ Register src="~/Controls/SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc1" %>
<%@ Register src="../Reports/cntReportHeader.ascx" tagname="cntReportHeader" tagprefix="uc2" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>
    
        <table class="okres_navigator printoff">
            <tr>
                <td class="colmiddle">
                    <uc1:SelectOkres ID="cntSelectOkres" OnOkresChanged="cntSelectOkres_Changed" runat="server" StoreInSession="true" />
                </td>
            </tr>
        </table>

        <div class="divider_ppacc printoff"></div>

        <uc2:cntReportHeader ID="cntReportHeader1" runat="server" 
            Caption="Osoby korzystające ze stołówki"
        />

        <asp:Menu ID="tabRaporty" CssClass="printoff" runat="server" Orientation="Horizontal" 
            onmenuitemclick="tabRaporty_MenuItemClick" >
            <StaticMenuStyle CssClass="tabsStrip" />
            <StaticMenuItemStyle CssClass="tabItem" />
            <StaticSelectedStyle CssClass="tabSelected" />
            <StaticHoverStyle CssClass="tabHover" />
            <Items>
                <asp:MenuItem Text="Dni - ilości"           Value="vDni" Selected="True" ></asp:MenuItem>
                <asp:MenuItem Text="Pracownicy - podwójnie" Value="vPodwojnie" ></asp:MenuItem>
                <asp:MenuItem Text="Pracownicy - ilości"    Value="vPracownicy" ></asp:MenuItem>
                <asp:MenuItem Text="Dni - Pracownicy"       Value="vDniPrac" ></asp:MenuItem>
                <asp:MenuItem Text="Szczegóły"              Value="vSzczegoly" ></asp:MenuItem>
                <asp:MenuItem Text="Duplikaty ROGER'ów"     Value="vDuplikatyROGER" ></asp:MenuItem>
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

        <div class="tabsContentLine" style="border-collapse:collapse; background-color:#FFF;">
            <asp:MultiView ID="mvRaporty" runat="server" ActiveViewIndex="0">
                <%-----------------------------------%>
                <asp:View ID="vDni" runat="server" >
                <%-----------------------------------%>
                    <uc1:cntReport ID="cntReport1" runat="server" 
                        CssClass="RepStolowka"
                        HeaderVisible="false"
                        SQL="
declare 
    @dataOd datetime,
    @dataDo datetime
set @dataOd = '@SQL1'
set @dataDo = '@SQL2'

select 
convert(varchar(10), O.Data, 20) as [Data], 
sum(O.ObiadF) as [Fordońska:N0S|Reports/RepStolowkaPrac @Data @Data F @SQL1 @SQL2|Pracownicy korzystający ze stołówki na Fordońskiej], 
sum(O.ObiadS) as [Szajnochy:N0S|Reports/RepStolowkaPrac @Data @Data S @SQL1 @SQL2|Pracownicy korzystający ze stołówki na Szajnochy], 
sum(O.ObiadF) + 
sum(O.ObiadS) as [Suma:N0S|Reports/RepStolowkaPrac @Data @Data FS @SQL1 @SQL2|Pracownicy korzystający ze stołówki], 
sum(O.ITuITu) as [Podwójnie:N0S|Reports/RepStolowkaPrac @Data @Data 2 @SQL1 @SQL2|Pracownicy korzystający ze stołówki podwójnie w dniu]
from VObiadyDzienPracownik O 
where O.Data between @dataOd and @dataDo 
group by O.Data
having sum(O.ObiadF) + sum(O.ObiadS) &gt; 0
order by O.Data
                    "/>
                </asp:View>

                <%-----------------------------------%>
                <asp:View ID="vPracownicy" runat="server" >
                <%-----------------------------------%>
                    <uc1:cntReport ID="cntReport4" runat="server" 
                        CssClass="RepStolowka"
                        HeaderVisible="false"
                        SQL="
declare 
    @dataOd datetime,
    @dataDo datetime
set @dataOd = '@SQL1'
set @dataDo = '@SQL2'

select 
ISNULL(O.Pracownik, '- karta: ' + convert(varchar, ECUserId) + ' -') as 
    [Pracownik|Reports/RepStolowkaPracownik @SQL1 @SQL2 * @Logo @ECUserId|Pokaż dni], 
O.Logo, 
O.ECUserId as 
    [ECUserId:N|Reports/RepStolowkaSzczegoly @SQL1 @SQL2 * @Logo @ECUserId|Pokaż godziny i szczegóły],
sum(O.ObiadF) as 
    [Fordońska:N0S|Reports/RepStolowkaSzczegoly @SQL1 @SQL2 F @Logo @ECUserId|Posiłki - Fordońska],
--O.ObiadFsum as [Fordońska(suma):N0S], 
sum(O.OdmowaFsum) as 
    [Odmowa Ford.:N0S|Reports/RepStolowkaSzczegoly @SQL1 @SQL2 OF @Logo @ECUserId|Odmowa - Fordońska], 
sum(ObiadS) as 
    [Szajnochy:N0S|Reports/RepStolowkaSzczegoly @SQL1 @SQL2 S @Logo @ECUserId|Posiłki - Szajnochy],
--O.ObiadSsum as [Szajnochy(suma):N0S], 
sum(O.OdmowaSsum) as 
    [Odmowa Szajn.:N0S|Reports/RepStolowkaSzczegoly @SQL1 @SQL2 OS @Logo @ECUserId|Odmowa - Szajnochy], 
sum(O.ObiadF) + sum(O.ObiadS) as 
    [Fordońska+Szajnochy:N0S|Reports/RepStolowkaSzczegoly @SQL1 @SQL2 FS @Logo @ECUserId|Posiłki - Fordońska+Szajnochy],
sum(case when O.ObiadF &gt; 0 and O.ObiadS &gt; 0 then 1 else 0 end) as 
    [Podwójnie:N0S|Reports/RepStolowkaSzczegoly @SQL1 @SQL2 2 @Logo @ECUserId|Posiłki podwójnie] 
from VObiadyDzienPracownik O 
where O.Data between @dataOd and @dataDo 
and (O.ObiadF &gt; 0 or O.ObiadS &gt; 0 or O.OdmowaFsum &gt; 0 or O.OdmowaSsum &gt; 0)
group by O.Pracownik, O.Logo, O.ECUserId
order by O.Pracownik, O.ECUserId
                "/>
                </asp:View>

                <%-----------------------------------%>
                <asp:View ID="vPodwojnie" runat="server" >
                <%-----------------------------------%>     <%-- na podstawie RepStolowkaPrac --%> 
                    <uc1:cntReport ID="cntReport5" runat="server"   
                        CssClass="RepStolowka"
                        HeaderVisible="false"
                        SQL="
declare 
	@dataOd datetime,
	@dataDo datetime
set @dataOd = '@SQL1'
set @dataDo = '@SQL2'

select 
convert (varchar(10), O.Data, 20) as 
    [Data|Reports/RepStolowkaSzczegoly @Data @Data * @Logo @ECUserId|Pokaż godziny i szczegóły w dniu],
ISNULL(O.Pracownik, '- karta: ' + convert(varchar, ECUserId) + ' -') as 
    [Pracownik|Reports/RepStolowkaPracownik @SQL1 @SQL2 * @Logo @ECUserId|Pokaż dni], 
O.Logo, 
O.ECUserId as 
    [ECUserId:N|Reports/RepStolowkaSzczegoly @SQL1 @SQL2 * @Logo @ECUserId|Pokaż godziny i szczegóły w okresie],
O.ObiadF as 
    [Fordońska:N0S|Reports/RepStolowkaSzczegoly @Data @Data * @Logo @ECUserId|Pokaż godziny i szczegóły w dniu],
O.OdmowaFsum as 
    [Odmowa Ford.:N0S|Reports/RepStolowkaSzczegoly @Data @Data * @Logo @ECUserId|Pokaż godziny i szczegóły w dniu], 
O.ObiadS as 
    [Szajnochy:N0S|Reports/RepStolowkaSzczegoly @Data @Data * @Logo @ECUserId|Pokaż godziny i szczegóły w dniu],
OdmowaSsum as 
    [Odmowa Szajn.:N0S|Reports/RepStolowkaSzczegoly @Data @Data * @Logo @ECUserId|Pokaż godziny i szczegóły w dniu], 
case when ObiadF &gt; 0 and ObiadS &gt; 0 then 1 else 0 end as  
    [Podwójnie:N0S|Reports/RepStolowkaSzczegoly @Data @Data * @Logo @ECUserId|Pokaż godziny i szczegóły w dniu] 
from VObiadyDzienPracownik O 
where O.Data between @dataOd and @dataDo 
and (O.ObiadF &gt; 0 and O.ObiadS &gt; 0)
order by O.Data, O.Pracownik, O.ECUserId
                    "/>
                    
                    <table name="report">
                        <tr><td>&nbsp;</td></tr>
                        <tr><td>
                                Szczegóły
                        </td></tr>
                    </table>
                    <uc1:cntReport ID="cntReport6" runat="server"   
                        CssClass="RepStolowka"
                        HeaderVisible="false"
                        SQL="
declare 
    @dataOd datetime,
    @dataDo datetime
set @dataOd = '@SQL1'
set @dataDo = '@SQL2'

select 
convert (varchar(10), O.Data, 20) as [Data:-], 
O.Czas as [Czas], 
ISNULL(O.Pracownik, '- karta: ' + convert(varchar, O.ECUserId) + ' -') as 
    [Pracownik|Reports/RepStolowkaPracownik @SQL1 @SQL2 * @Logo @ECUserId|Pokaż dni], 
O.Logo, 
O.ECUserId as [ECUserId:N], 
O.ECUniqueId as [ECUniqueId:N],  
O.ECReaderId as [ECReaderId:N], 
O.ECCode as [ECCode:N],
O.ObiadF as [Fordońska:N0S], 
O.OdmowaF as [Odmowa Ford.:N0S], 
O.ObiadS as [Szajnochy:N0S], 
O.OdmowaS as [Odmowa Szajn.:N0S]
from VObiady O 
join (
	select Data, ECUserId from VObiadyDzienPracownik O 
	where O.Data between @dataOd and @dataDo 
	and (O.ObiadF &gt; 0 and O.ObiadS &gt; 0)
) A on A.Data = O.Data and A.ECUserId = O.ECUserId
where O.ECCode in (1,2)
order by O.Data, O.Pracownik, O.ECUserId, O.Czas                    
                    "/>
                </asp:View>

                <%-----------------------------------%>
                <asp:View ID="vDniPrac" runat="server" >
                <%-----------------------------------%>
                    <uc1:cntReport ID="cntReport2" runat="server" 
                        CssClass="RepStolowka"
                        HeaderVisible="false"
                        PageSize="20"
                        AllowPaging="true"
                        SQL="
declare 
    @dataOd datetime,
    @dataDo datetime
set @dataOd = '@SQL1'
set @dataDo = '@SQL2'

select 
convert (varchar(10), O.Data, 20) as [Data],
ISNULL(O.Pracownik, '- karta: ' + convert(varchar, ECUserId) + ' -') as 
    --[Pracownik|Reports/RepStolowkaPracownik @SQL1 @SQL2 * @Logo @ECUserId|Pokaż w całym okresie], 
    [Pracownik],
O.Logo, 
O.ECUserId as 
    --[ECUserId|Reports/RepStolowkaSzczegoly @SQL1 @SQL2 * @Logo @ECUserId|Pokaż szczegóły w całym okresie],
    [ECUserId],
O.ObiadF as [Fordońska:N0],

O.ObiadFsum as [Fordońska(suma):N0], 

O.OdmowaFsum as [Odmowa Ford.:N0], 
O.ObiadS as [Szajnochy:N0],

O.ObiadSsum as [Szajnochy(suma):N0], 

O.OdmowaSsum as [Odmowa Szajn.:N0], 
case when ObiadF &gt; 0 and ObiadS &gt; 0 then 1 else 0 end as [Podwójnie:N0] 
from VObiadyDzienPracownik O 
where O.Data between @dataOd and @dataDo 
and (ObiadF &gt; 0 or ObiadS &gt; 0 or OdmowaFsum &gt; 0 or OdmowaSsum &gt; 0)
order by O.Data, O.Pracownik, O.ECUserId
                "/>
                </asp:View>

                <%-----------------------------------%>
                <asp:View ID="vSzczegoly" runat="server" >
                <%-----------------------------------%>
                    <uc1:cntReport ID="cntReport3" runat="server" 
                        CssClass="RepStolowka"
                        HeaderVisible="false"
                        PageSize="20"
                        AllowPaging="true"
                        SQL="
declare 
    @dataOd datetime,
    @dataDo datetime
set @dataOd = '@SQL1'
set @dataDo = '@SQL2'

select 
convert (varchar(10), O.Data, 20) as [Data:-], 
O.Czas as [Czas], 
O.ECUniqueId, --as [:-], 
O.ECReaderId, --as [:-], 
O.ECCode, --as [:-], 
ISNULL(O.Pracownik, '- karta nieprzypisana -') as [Pracownik], 
O.Logo, 
O.ECUserId, 
case when O.Logo is null then 'P' else 'K' end as [typ:-],
ISNULL(O.Logo, convert(varchar, ECUserId)) as [id:-],
O.ObiadF as [Fordońska:N0], 
O.OdmowaF as [Odmowa Ford.:N0], 
O.ObiadS as [Szajnochy:N0], 
O.OdmowaS as [Odmowa Szajn.:N0]
from VObiady O 
where O.Data between @dataOd and @dataDo and O.ECCode in (1,2)
and (O.ObiadF &gt; 0 or O.ObiadS &gt; 0 or O.OdmowaF &gt; 0 or O.OdmowaS &gt; 0)
--order by O.Pracownik, O.ECUserId, O.Czas
order by O.Czas
                "/>
                </asp:View>

                <%-----------------------------------%>
                <asp:View ID="vDuplikatyROGER" runat="server" >
                <%-----------------------------------%>     
                    <uc1:cntReport ID="cntReport7" runat="server"   
                        CssClass="RepStolowka"
                        HeaderVisible="false"
                        SQL="
declare 
    @dataOd datetime,
    @dataDo datetime
set @dataOd = '@SQL1'
set @dataDo = '@SQL2'

select 
convert (varchar(10), O.Data, 20) as [Data:-], 
O.Czas as [Czas], 
ISNULL(O.Pracownik, '- karta: ' + convert(varchar, O.ECUserId) + ' -') as 
    [Pracownik|Reports/RepStolowkaPracownik @SQL1 @SQL2 * @Logo @ECUserId|Pokaż dni], 
O.Logo, 
O.ECUserId as [ECUserId:N], 
O.ECUniqueId as [ECUniqueId:N],  
O.ECReaderId as [ECReaderId:N], 
O.ECCode as [ECCode:N],
O.ObiadF as [Fordońska:N0S], 
O.OdmowaF as [Odmowa Ford.:N0S], 
O.ObiadS as [Szajnochy:N0S], 
O.OdmowaS as [Odmowa Szajn.:N0S]
from VObiady O 
join (
	select Data, ECUserId, ECReaderId, ECCode, COUNT(*) as ilosc
	from VObiady
	where Data between @dataOd and @dataDo and ECCode in (1,2)
	and (ObiadF &gt; 0 or ObiadS &gt; 0)
	group by Data, ECUserId, ECReaderId, ECCode 
	having COUNT(*) &gt; 1
) A on A.Data = O.Data and A.ECUserId = O.ECUserId and A.ECCode = O.ECCode and A.ECReaderId = O.ECReaderId
                    "/>
                </asp:View>
            </asp:MultiView>
        </div>

    
    
