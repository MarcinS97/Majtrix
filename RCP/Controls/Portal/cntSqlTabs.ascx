<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSqlTabs.ascx.cs" Inherits="HRRcp.Controls.Portal.cntSqlTabs" %>

<asp:Menu ID="mnTabs" runat="server" Orientation="Horizontal" onmenuitemclick="mnTabs_MenuItemClick" >
    <StaticMenuStyle CssClass="tabsStrip" />
    <StaticMenuItemStyle CssClass="tabItem" />
    <StaticSelectedStyle CssClass="tabSelected" />
    <StaticHoverStyle CssClass="tabHover" />
</asp:Menu>

<asp:SqlDataSource ID="dsGrupa" runat="server"
    SelectCommand="select MenuText from SqlMenu where Grupa = '{0}' and Aktywny = 1 order by Kolejnosc
    "/>
