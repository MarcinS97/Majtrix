<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSqlContent.ascx.cs" Inherits="HRRcp.Controls.Portal.cntSqlContent" %>

<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>
<%@ Register src="~/Controls/Reports/cntDetails.ascx" tagname="cntDetails" tagprefix="uc2" %>

<div id="paSqlContent" runat="server" class="cntSqlContent">
    <asp:Menu ID="tabContent" runat="server" Orientation="Horizontal" onmenuitemclick="tabContent_MenuItemClick" >
        <StaticMenuStyle CssClass="tabsStrip" />
        <StaticMenuItemStyle CssClass="tabItem" />
        <StaticSelectedStyle CssClass="tabSelected" />
        <StaticHoverStyle CssClass="tabHover" />
    </asp:Menu>
    <div class="tabsContent">
        <uc1:cntReport ID="cntMasterLines" runat="server" Visible="false"
            AllowPaging="true"
            PageSize="20"
            AllowQueryString="false"
        />

        <uc2:cntDetails ID="cntMasterScreen" runat="server" Visible="true"
            AllowQueryString="false"
        />
    </div>
</div>
