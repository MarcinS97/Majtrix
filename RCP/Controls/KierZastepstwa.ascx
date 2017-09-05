<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KierZastepstwa.ascx.cs" Inherits="HRRcp.Controls.KierZastepstwa" %>
<%@ Register src="Zastepstwa.ascx" tagname="Zastepstwa" tagprefix="uc2" %>

<table class="tabsContent" >
    <tr>
        <td class="LeftMenu">
            <asp:Menu ID="mnLeft" runat="server" StaticDisplayLevels="1" onmenuitemclick="mnLeft_MenuItemClick" >
                <StaticMenuStyle CssClass="menu" />
                <StaticSelectedStyle CssClass="selected" />
                <StaticMenuItemStyle CssClass="item" />
                <StaticHoverStyle CssClass="hover" />
                <Items>
                    <asp:MenuItem Text="Kogo zastępuję" Value="0"></asp:MenuItem>
                    <asp:MenuItem Text="Kto mnie zastępuje" Value="1" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="Historia zastępstw" Value="2"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </td>
        
        <td class="LeftMenuContent">
            <asp:MultiView ID="mvSettings" runat="server" ActiveViewIndex="0">
                <asp:View ID="vMoje" runat="server">
                    <uc2:Zastepstwa ID="Zastepstwa2" Mode="0" runat="server" />
                </asp:View>

                <asp:View ID="vMnie" runat="server">
                    <uc2:Zastepstwa ID="Zastepstwa1" Mode="1" runat="server" />
                </asp:View>

                <asp:View ID="vHist" runat="server">
                    <uc2:Zastepstwa ID="ZastepstwaHistoria" Mode="11" runat="server" />
                </asp:View>
            </asp:MultiView>
        </td>
    </tr>
</table>
