<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntZastepstwaMenu.ascx.cs" Inherits="HRRcp.Scorecards.Controls.cntZastepstwaMenu" %>
<%@ Register src="cntZastepstwa.ascx" tagname="cntZastepstwa" tagprefix="uc2" %>

<table class="tabsContent" >
    <tr>
        <td class="LeftMenu">
            <asp:Menu ID="mnLeft" runat="server" StaticDisplayLevels="1" onmenuitemclick="mnLeft_MenuItemClick" >
                <StaticMenuStyle CssClass="menu" />
                <StaticSelectedStyle CssClass="selected" />
                <StaticMenuItemStyle CssClass="item" />
                <StaticHoverStyle CssClass="hover" />
                <Items>
                    <asp:MenuItem Text="Zastępstwa bieżące" Value="0" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="Historia zastępstw" Value="1" ></asp:MenuItem>
                </Items>
            </asp:Menu>
        </td>
        
        <td class="LeftMenuContent">
            <asp:MultiView ID="mvSettings" runat="server" ActiveViewIndex="0">
                <asp:View ID="vZastepstwa" runat="server" >
                    <uc2:cntZastepstwa ID="cntZastepstwa" Mode="2" Sort="1" runat="server" />
                </asp:View>

                <asp:View ID="vHistoria" runat="server" >
                    <uc2:cntZastepstwa ID="cntZastepstwaHist" Mode="21" Sort="1" runat="server" />
                </asp:View>
            </asp:MultiView>
        </td>
    </tr>
</table>
