<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSqlContent2.ascx.cs" Inherits="HRRcp.Controls.Portal.cntSqlContent3" %>

<%@ Register src="~/Controls/Portal/WniosekZmianaDanych/cntReport2.ascx" tagname="cntReport" tagprefix="uc1" %>




<div id="paSqlContent" runat="server" class="cntSqlContent">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>

    
        <uc1:cntReport ID="cntMasterLines" runat="server" Visible="false"
            AllowPaging="true"
            PageSize="20"
            AllowQueryString="false"
        />

        </ContentTemplate>
    </asp:UpdatePanel>
</div>
