<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPlikiAdm.ascx.cs" Inherits="HRRcp.Controls.Portal.cntPlikiAdm" %>
<%@ Register src="~/Controls/Portal/cntPliki.ascx" tagname="cntPliki" tagprefix="uc1" %>

<table class="tabsContent" >
    <tr>
        <td class="LeftMenu">
            <asp:Menu ID="mnLeft" runat="server" StaticDisplayLevels="1" onmenuitemclick="mnLeft_MenuItemClick" >
                <StaticMenuStyle CssClass="menu" />
                <StaticSelectedStyle CssClass="selected" />
                <StaticMenuItemStyle CssClass="item" />
                <StaticHoverStyle CssClass="hover" />
                <Items>
                    <asp:MenuItem Text="Dla pracowników" Value="FILEP" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="Dla przełożonych" Value="FILEK" ></asp:MenuItem>
                </Items>
            </asp:Menu>
        </td>
        
        <td class="LeftMenuContent">
            <uc1:cntPliki ID="cntPliki" runat="server" Mode="1" Grupa="FILEP"/>                                  
        </td>
    </tr>
</table>
