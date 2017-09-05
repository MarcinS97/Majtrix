<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSqlContent2.ascx.cs" Inherits="HRRcp.Controls.Portal.cntSqlContent2" %>

<%@ Register src="~/Controls/Reports/cntReport2.ascx" tagname="cntReport" tagprefix="uc1" %>
<%@ Register src="~/Controls/Reports/cntDetails.ascx" tagname="cntDetails" tagprefix="uc2" %>

<%@ Register src="cntSqlEdit.ascx" tagname="cntSqlEdit" tagprefix="uc3" %>

<div id="paSqlContent" runat="server" class="cntSqlContent">
    <asp:Menu ID="tabContent" runat="server" Orientation="Horizontal"   onmenuitemclick="tabContent_MenuItemClick" >
        <StaticMenuStyle CssClass="tabsStrip" />
        <StaticMenuItemStyle CssClass="tabItem" />
        <StaticSelectedStyle CssClass="tabSelected" />
        <StaticHoverStyle CssClass="tabHover" />
    </asp:Menu>
    <div class="tabsContent">
        <div id="paEdit" runat="server" class="edit" visible="false">
            <asp:Button ID="btEdit" runat="server" CssClass="button100" Text="Edycja" onclick="btEdit_Click" />            
            <uc3:cntSqlEdit ID="cntSqlEdit1" runat="server" OnClose="cntSqlEdit1_Close" Visible="false"/>            
        </div>    
    
        
        <div id="paEdit2" runat="server" class="edit2" visible="true">
        
            <asp:HiddenField ID="hidTabId" runat="server" Visible="false"/>
            <asp:HiddenField ID="hidTabType" runat="server" Visible="false"/>
            <asp:Button ID="btWniosek" runat="server" Text="Wniosek o zmianę danych" onclick="btWniosek_Click" />
        </div>
        
    
    
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
