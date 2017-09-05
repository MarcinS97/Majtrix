<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepSpoznienia.ascx.cs" Inherits="HRRcp.Controls.Raporty.RepSpoznienia" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>
<%@ Register src="~/Controls/SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc1" %>
<%@ Register src="../Reports/cntReportHeader.ascx" tagname="cntReportHeader" tagprefix="uc2" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>
    
        <table class="okres_navigator printoff">
            <tr>
                <td class="colleft">
                    <div id="paSelectKier" runat="server">
                        <span class="t1">Kierownik:</span>                            
                        <asp:DropDownList ID="ddlKierownicy" runat="server" AutoPostBack="true" onselectedindexchanged="ddlKierownicy_SelectedIndexChanged" />
                    </div>
                </td>
                <td class="colmiddle">
                    <uc1:SelectOkres ID="cntSelectOkres" OnOkresChanged="cntSelectOkres_Changed" runat="server" StoreInSession="true" />
                </td>
                <td class="colright">
                </td>
            </tr>
        </table>

        <div class="divider_ppacc printoff"></div>

        <uc2:cntReportHeader ID="cntReportHeader1" runat="server" 
            Caption="Spóźnienia"
        />

        <asp:Menu ID="tabRaporty" CssClass="printoff" runat="server" Orientation="Horizontal" 
            onmenuitemclick="tabRaporty_MenuItemClick" >
            <StaticMenuStyle CssClass="tabsStrip" />
            <StaticMenuItemStyle CssClass="tabItem" />
            <StaticSelectedStyle CssClass="tabSelected" />
            <StaticHoverStyle CssClass="tabHover" />
            <Items>
                <asp:MenuItem Text="Łączny czas spóźnień"    Value="vCzas" Selected="True" ></asp:MenuItem>
                <asp:MenuItem Text="Pracownicy - spóźnienia" Value="vSpoznienia" ></asp:MenuItem>
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
                <asp:View ID="vCzas" runat="server" >
                <%-----------------------------------%>
                    <uc1:cntReport ID="cntReport1" runat="server" 
                        CssClass="RepSpoznienia"
                        HeaderVisible="false"
                        SQL="
declare 
    @od datetime,
    @do datetime,
    @kierId int
set @od = '@SQL1'
set @do = '@SQL2'
set @kierId = @SQL3

select 
S.Pracownik, 
S.KadryId as [Nr ew.],
K.Nazwisko + ' ' + K.Imie as Kierownik, 
K.KadryId as [Nr ew. K],
dbo.ToTime(sum(S.SpoznienieSec)) as [Łącznie],
count(*) as [Ilość],
dbo.ToTime(MAX(S.SpoznienieSec)) as [Największe]
from VSpoznienia S
outer apply (select top 1 * from Przypisania where IdPracownika = S.IdPracownika and Od &lt; @do and Status = 1 order by Od desc) R
left join Pracownicy K on K.Id = R.IdKierownika
where S.Data between @od and @do and S.SpoznienieSec &gt; 0 and S.Akceptacja = 1
and (
	(@kierId = -100) or
	(@kierId &gt;= 0 and S.IdPracownika in (select distinct IdPracownika from Przypisania where Od &lt;= @do and ISNULL(Do, '20990909') &gt;= @od and Status = 1 and IdKierownika = @kierId)) 
)	
group by S.Pracownik, S.KadryId, K.Nazwisko, K.Imie, K.KadryId
order by S.Pracownik
                    "/>
                </asp:View>

                <%-----------------------------------%>
                <asp:View ID="vSpoznienia" runat="server" >
                <%-----------------------------------%>
                    <uc1:cntReport ID="cntReport4" runat="server" 
                        CssClass="RepSpoznienia"
                        HeaderVisible="false"
                        SQL="
declare 
    @od datetime,
    @do datetime,
    @kierId int
set @od = '@SQL1'
set @do = '@SQL2'
set @kierId = @SQL3

select 
S.Data as [Data:D], 
S.Pracownik, 
S.KadryId as [Nr ew.],
K.Nazwisko + ' ' + K.Imie as Kierownik, 
K.KadryId as [Nr ew. K],
S.Symbol + ' - ' + S.Nazwa as [Zmiana],
S.Zmiana as [Początek zmiany],
S.Przjscie as [Przyjście],
S.Spoznienie as [Spóźnienie],
S.Uwagi as [Uwagi:UU]
from VSpoznienia S
left join Przypisania R on R.IdPracownika = S.IdPracownika and S.Data between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1
left join Pracownicy K on K.Id = R.IdKierownika
where S.Data between @od and @do and S.SpoznienieSec &gt; 0 and S.Akceptacja = 1
and (
	(@kierId = -100) or
	(@kierId &gt;= 0 and S.IdPracownika in (select distinct IdPracownika from Przypisania where Od &lt;= @do and ISNULL(Do, '20990909') &gt;= @od and Status = 1 and IdKierownika = @kierId))
)	
order by Data, Pracownik
                "/>
                </asp:View>

            </asp:MultiView>
        </div>

    
    
