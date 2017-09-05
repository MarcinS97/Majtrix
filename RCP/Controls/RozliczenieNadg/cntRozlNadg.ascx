<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntRozlNadg.ascx.cs" Inherits="HRRcp.Controls.RozliczenieNadg.cntRozlNadg" %>

<%@ Register src="~/Controls/RozliczenieNadg/cntSelectOkres3.ascx" tagname="cntSelectOkres" tagprefix="uc1" %>

<%@ Register src="~/Controls/RozliczenieNadg/cntPrzeznaczNadg2.ascx" tagname="cntPrzeznaczNadg2" tagprefix="uc2" %>
<%@ Register src="~/Controls/RozliczenieNadg/cntRozlNadgSuma.ascx" tagname="cntRozlNadgSuma" tagprefix="uc3" %>

<div id="paRozlNadg" class="cntRozlNadg" runat="server">
    <table class="okres_navigator">
        <tr>
            <td class="colleft">
                <asp:Label ID="lbPlanQ" runat="server" CssClass="t5" Text="Rozliczenie nadgodzin" Visible="false"></asp:Label>
            </td>
            <td class="colmiddle">
                <uc1:cntSelectOkres ID="cntSelectOkres" StoreInSession="true" OnOkresChanged="cntSelectOkres_Changed" runat="server" />
            </td>
            <td class="colright">
                <asp:Label ID="lbOkresStatus" CssClass="t1" Visible="false" runat="server" ></asp:Label>
                <%--
                <asp:Button ID="btAccept" runat="server" Text="Akceptuj" CssClass="button75" onclick="btAcceptPP_Click" />
                <asp:Button ID="btCheck" runat="server" Text="Sprawdź" CssClass="button75" onclick="btCheckPP_Click" />
                --%>
            </td>
        </tr>
    </table>
    
    <span class="t5">Podsumowanie:</span>
    <uc3:cntRozlNadgSuma ID="cntRozlNadgSuma" runat="server" />
    <br />
    <asp:Menu ID="tabFilter" runat="server" Orientation="Horizontal" 
        onmenuitemclick="tabFilter_MenuItemClick" >
        <StaticMenuStyle CssClass="tabsStrip mnFilter" />
        <StaticMenuItemStyle CssClass="tabItem" />
        <StaticSelectedStyle CssClass="tabSelected" />
        <StaticHoverStyle CssClass="tabHover" />
        <Items>
            <asp:MenuItem Text="Wszystko" Value="1" Selected="True"></asp:MenuItem>
            <asp:MenuItem Text="Do rozliczenia" Value="8" ToolTip="Niedomiar i nadgodziny do rozliczenia (rozliczone i nierozliczone)" ></asp:MenuItem>
            <asp:MenuItem Text="• niedomiar" Value="3" ToolTip="Niedomiar do rozliczenia (rozliczony i nierozliczony)"></asp:MenuItem>
            <asp:MenuItem Text="• nadgodziny" Value="4" ToolTip="Nadgodziny do rozliczenia (rozliczone i nierozliczone)"></asp:MenuItem>            
            <asp:MenuItem Text="Nierozliczone" Value="2" ToolTip="Niedomiar i nadgodziny nierozliczone" ></asp:MenuItem>
            <asp:MenuItem Text="• niedomiar" Value="21" ToolTip="Niedomiar nierozliczony"></asp:MenuItem>
            <asp:MenuItem Text="• nadgodziny" Value="22" ToolTip="Nadgodziny nierozliczone"></asp:MenuItem>
            <asp:MenuItem Text="Do wypłaty" Value="5" ToolTip="Nadgodziny zaklasyfikowane do wypłaty" ></asp:MenuItem> 
            <asp:MenuItem Text="Wolne za nadgodziny" Value="7"></asp:MenuItem>
            <asp:MenuItem Text="Do odpracownania" Value="6" ToolTip="Niedomiar zaklasyfikowany do odpracowania" ></asp:MenuItem>
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
    <uc2:cntPrzeznaczNadg2 ID="cntPrzeznaczNadg" OnChanged="cntPrzeznaczNadg_Changed" runat="server" />

</div>
