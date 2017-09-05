<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSqlContent.ascx.cs" Inherits="HRRcp.Portal.Controls.cntSqlContent" %>

<%@ Register src="~/Controls/Reports/cntReport2.ascx" tagname="cntReport" tagprefix="uc1" %>
<%@ Register src="~/Controls/Reports/cntDetails.ascx" tagname="cntDetails" tagprefix="uc2" %>

<%@ Register src="~/Portal/Controls/cntSqlEdit.ascx" tagname="cntSqlEdit" tagprefix="uc3" %>

<div id="paSqlContent" runat="server" class="cntSqlContent">
    <asp:Menu ID="tabContent" runat="server" Orientation="Horizontal"   onmenuitemclick="tabContent_MenuItemClick" >
        <StaticMenuStyle CssClass="tabsStrip" />
        <StaticMenuItemStyle CssClass="tabItem" />
        <StaticSelectedStyle CssClass="tabSelected" />
        <StaticHoverStyle CssClass="tabHover" />
    </asp:Menu>
    <div class="tabsContent">
        <div id="paEdit" runat="server" class="edit xpull-right" visible="false">
            <asp:Button ID="btEdit" runat="server" Text="Edycja" onclick="btEdit_Click" CssClass="btn btn-primary pull-right" style="margin-bottom: 32px;" />            
            <uc3:cntSqlEdit ID="cntSqlEdit1" runat="server" OnClose="cntSqlEdit1_Close" Visible="false"/>            
        </div>    
            
        <div id="paEdit2" runat="server" class="edit2 inline" visible="true">        
            <asp:HiddenField ID="hidTabId" runat="server" Visible="false"/>
            <asp:HiddenField ID="hidTabType" runat="server" Visible="false"/>
            <asp:Button ID="btWniosek" runat="server" Text="Wniosek o zmianę danych" onclick="btWniosek_Click" CssClass="btn btn-default" />
        </div>
    
        <uc1:cntReport ID="cntMasterLines" runat="server" Visible="false"
            AllowPaging="true"
            PageSize="20" 
            AllowQueryString="false"
            GridCssClass="GridView1 table table-striped table-bordered"
        />

        <uc2:cntDetails ID="cntMasterScreen" runat="server" Visible="true"
            AllowQueryString="false" 
        />
    </div>
</div>

<asp:SqlDataSource ID="dsTabs" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
    SelectCommand="
declare @grupa varchar(200)
declare @rights varchar(max)
set @grupa = '{1}'
set @rights = '{2}'

select * from {0}..SqlContent 
where Grupa = @grupa and Aktywny = 1 
and (Rights is null or dbo.CheckRightsExpr(isnull(@rights, ''), Rights) = 1)    
order by Kolejnosc, MenuText
    "/>


