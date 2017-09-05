<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepRozlNadg.ascx.cs" Inherits="HRRcp.Controls.Raporty.RepRozlNadg" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>
<%@ Register src="~/Controls/SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc1" %>
<%@ Register src="../Reports/cntReportHeader.ascx" tagname="cntReportHeader" tagprefix="uc2" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc2" %>

<table class="okres_navigator printoff">
    <tr>
        <%--
        <td class="colmiddle">
            <uc1:SelectOkres ID="cntSelectOkres" OnOkresChanged="cntSelectOkres_Changed" runat="server" StoreInSession="true" />
        </td>
        --%>
        <td class="colleft">
            <div id="paSelectKier" runat="server">
                <span class="t1">Kierownik:</span>                            
                <asp:DropDownList ID="ddlKierownicy" runat="server" AutoPostBack="true" onselectedindexchanged="ddlKierownicy_SelectedIndexChanged" />
            </div>
        </td>
        <td class="colmiddle">
            <span>Okres od:</span> 
            <uc2:DateEdit ID="deOd" runat="server" />&nbsp;&nbsp;&nbsp;
            <span>do:</span>
            <uc2:DateEdit ID="deDo" runat="server" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btExec" runat="server" CssClass="button75" Text="Wykonaj" onclick="btExec_Click" />
        </td>
        <td class="colright">
        </td>
    </tr>
</table>

<div class="divider_ppacc printoff"></div>

<uc2:cntReportHeader ID="cntReportHeader1" runat="server" 
    Caption="Rozliczenie nadgodzin"
/>

<asp:Menu ID="tabRaporty" CssClass="printoff" runat="server" Orientation="Horizontal" 
    onmenuitemclick="tabRaporty_MenuItemClick" >
    <StaticMenuStyle CssClass="tabsStrip" />
    <StaticMenuItemStyle CssClass="tabItem" />
    <StaticSelectedStyle CssClass="tabSelected" />
    <StaticHoverStyle CssClass="tabHover" />
    <Items>
        <%--
        <asp:MenuItem Text="Rozliczenie w okresie"           Value="vRozlOkres" Selected="True" ></asp:MenuItem>
        --%>
        <asp:MenuItem Text="Podsumowanie miesięczne"         Value="vSumyMies" Selected="True" ></asp:MenuItem>
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
        <asp:View ID="vRozlOkres" runat="server" >
        <%-----------------------------------%>
            <uc1:cntReport ID="cntReport1" runat="server" 
                CssClass="RepRozlNadg"
                HeaderVisible="false"
                SQL="
select 'Brak danych' as [Informacja]
            "/>
        </asp:View>

        <%-----------------------------------%>
        <asp:View ID="vSumyMies" runat="server" >
        <%-----------------------------------%>
            <uc1:cntReport ID="cntReport2" runat="server" 
                CssClass="RepRozlNadg"
                HeaderVisible="false"
                SQL="
declare 
    @od datetime,
    @do datetime,
    @kierId int
set @od = dbo.bom(@SQL1)
set @do = dbo.eom(@SQL2)
set @kierId = @SQL3

select 
    P.Id [Id:-],
    convert(varchar(10), NS.DataOd, 20) [DataOd:-],
    convert(varchar(10), NS.DataDo, 20) [DataDo:-],
    P.KadryId as [Nr Ew.], 
    P.Nazwisko + ' ' + P.Imie as [Pracownik], 
	case 
	    when P.Status = -1 then 'Zwolniony'
	    when P.Status in (0,1) and RR.Id is not null then 'Zatrudniony'
	    when P.Status in (0,1) and RR.Id is null then 'Zatrudniony - brak przypisania'
    else ''
    end as [Status],
    /*
	case 
	    when P.Status = -1 then 'Zwolniony' + ISNULL(' (' + convert(varchar(10), P.DataZwol, 20) + ')', '')
	    when P.Status in (0,1) and RR.Id is not null then 'Zatrudniony'
	    when P.Status in (0,1) and RR.Id is null then 'Zatrudniony - brak przypisania' + ISNULL(' (' + convert(varchar(10), R.Do, 20) + ')', '')
    else ''
    end as [Status],
    */

	case 
	    when P.Status = -1 then convert(varchar(10), P.DataZwol, 20)
	    when P.Status in (0,1) and RR.Id is null then convert(varchar(10), R.Do, 20)
    else ''
    end as [Data zwolnienia],

	dbo.fn_GetPracLastCC(P.Id, @do, 2, ',') as [MPK],
    
    case 
		when R.Id is null then ''
		when R.IdKierownika = 0 then 'Poziom główny struktury'
	else K.Nazwisko + ' ' + K.Imie 
	end as [Przełożony], 
	
    NS.Niedomiar + NS.WND + NS.OND as [Niedomiar:NS|Reports/RepRNSzczegoly @DataOd @DataDo drN @Id|Niedomiar do rozliczenia],
    NS.N50 - (NS.W50 + NS.O50)     as [Do wypłaty 50:NS|Reports/RepRNSzczegoly @DataOd @DataDo P50 @Id|Nadgodziny 50 do wypłaty],    -- do wypłaty (nierozliczone + do wypłaty)
    NS.N100 - (NS.W100 + NS.O100)  as [Do wypłaty 100:NS|Reports/RepRNSzczegoly @DataOd @DataDo P100 @Id|Nadgodziny 100 do wypłaty],
    --(select ROUND(CONVERT(float, ISNULL(SUM(Nocne), 0)) / 3600, 2, 0) from PlanPracy where IdPracownika = P.Id and Data between NS.DataOd and NS.DataDo) as [Czas nocny:NS],
    NS.Nocne as [Czas nocny:NS|Reports/RepRNSzczegoly @DataOd @DataDo NOC @Id|Czas pracy w nocy],
	NS.W50   as [Wybrane 50:NS|Reports/RepRNSzczegoly @DataOd @DataDo W50 @Id|Nadgodziny 50 wybrane],
	NS.W100  as [Wybrane 100:NS|Reports/RepRNSzczegoly @DataOd @DataDo W100 @Id|Nadgodziny 100 wybrane],
	NS.O50   as [Odpracowane 50:NS|Reports/RepRNSzczegoly @DataOd @DataDo O50 @Id|Nadgodziny 50 - odpracowanie],
	NS.O100  as [Odpracowane 100:NS|Reports/RepRNSzczegoly @DataOd @DataDo O100 @Id|Nadgodziny 100 - odpracowanie],
    convert(varchar(7), NS.DataOd, 20) as [Miesiąc:D|Reports/RepRNSzczegoly @DataOd @DataDo * @Id|Wszystkie wartości w miesiącu]	
	/*
    ISNULL(NS.Niedomiar + NS.WND + NS.OND, 0) as [Niedomiar:NS||Niedomiar do rozliczenia],
    ISNULL(NS.N50 - (NS.W50 + NS.O50), 0) as [Do wypłaty 50:NS],    -- do wypłaty (nierozliczone + do wypłaty)
    ISNULL(NS.N100 - (NS.W100 + NS.O100), 0) as [Do wypłaty 100:NS],
    (select ROUND(CONVERT(float, ISNULL(SUM(Nocne), 0)) / 3600, 2, 0) from PlanPracy where IdPracownika = P.Id and Data between NS.DataOd and NS.DataDo) as [Czas nocny:NS],
	ISNULL(NS.W50, 0)  as [Wybrane 50:NS],
	ISNULL(NS.W100, 0) as [Wybrane 100:NS],
	ISNULL(NS.O50, 0)  as [Odpracowane 50:NS],
	ISNULL(NS.O100, 0) as [Odpracowane 100:NS],
    convert(varchar(7), NS.DataOd, 20) as [Miesiąc:D]	
    */
from Pracownicy P
left join VRozliczenieNadgodzinMies NS on NS.IdPracownika = P.Id and NS.DataOd between @od and @do
left join Przypisania RR on RR.IdPRacownika = P.Id and @do between RR.Od and ISNULL(RR.Do, '20990909') and RR.Status = 1    -- aktualne
outer apply (select top 1 * from Przypisania where IdPracownika = P.Id and Od &lt;= @do and Status = 1 order by Od desc) R  -- ostatnie
left join Pracownicy K on K.Id = R.IdKierownika
where @od is not null and @do is not null and @kierId is not null
and P.Status &gt;= -1
and (
	(@kierId = -100 and P.Id in (select IdPracownika from Przypisania where Od &lt;= @do and ISNULL(Do, '20990909') &gt;= @od and Status = 1)) or
    (@kierId &gt;= 0 and P.Id in (select IdPracownika from dbo.fn_GetTreeOkres(@kierId, @od, @do, @do) where HLevel = 1))
    )
order by NS.DataOd, Pracownik
        "/>
        </asp:View>

    </asp:MultiView>
</div>



