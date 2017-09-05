<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepCzasPracy2.ascx.cs" Inherits="HRRcp.Controls.RepCzasPracy2" %>
<%@ Register src="~/Controls/RepHeader.ascx" tagname="RepHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc1" %>
<%@ Register src="~/Controls/RepNadgodziny3.ascx" tagname="RepNadgodziny" tagprefix="uc1" %>
<%@ Register src="~/Controls/RcpControl2.ascx" tagname="RcpControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>

<table class="okres_navigator printoff">
    <tr>
        <td class="colleft">
            <asp:Label ID="lbKierownik" runat="server" CssClass="t1" Text="Kierownik:"></asp:Label>
            <asp:DropDownList ID="ddlKierownicy" runat="server" AutoPostBack="true" onselectedindexchanged="ddlKierownicy_SelectedIndexChanged" />
        </td>
        <td class="colmiddle">
            <uc1:SelectOkres ID="cntSelectOkres" ControlID="repNadgodziny" OnOkresChanged="cntSelectOkres_Changed" runat="server" />
        </td>
        <td class="colright">
        </td>
    </tr>
</table>
<div class="divider_ppacc printoff"></div>

<uc1:RepHeader ID="repHeader" Caption="" Icon="1" runat="server" />

<asp:Menu ID="tabRaporty" CssClass="printoff" runat="server" Orientation="Horizontal" 
    onmenuitemclick="tabRaporty_MenuItemClick" >
    <StaticMenuStyle CssClass="tabsStrip" />
    <StaticMenuItemStyle CssClass="tabItem" />
    <StaticSelectedStyle CssClass="tabSelected" />
    <StaticHoverStyle CssClass="tabHover" />
    <Items>
        <asp:MenuItem Text="Czas pracy - łączenie" Value="vCzasSuma" Selected="True" ></asp:MenuItem>
        <asp:MenuItem Text="Czas pracy - dni" Value="vCzasDni" ></asp:MenuItem>
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
        <asp:View ID="vCzasSuma" runat="server" >
        <%-----------------------------------%>
            <uc1:RepNadgodziny ID="repNadgodziny" Mode="4" CCMode="1" runat="server" />
        </asp:View>

        <%-----------------------------------%>
        <asp:View ID="vCzasDni" runat="server" >
        <%-----------------------------------%>
            <uc1:RcpControl ID="repDni" runat="server" />
        </asp:View>

    </asp:MultiView>
</div>



