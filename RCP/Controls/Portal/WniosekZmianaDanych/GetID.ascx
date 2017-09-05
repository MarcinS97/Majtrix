<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GetID.ascx.cs" Inherits="HRRcp.Controls.WnioseZmianaDanych.GetID" %>
<%@ Register Src="~/Controls/Portal/WniosekZmianaDanych/cntSqlContent2.ascx" TagName="SqlContent" TagPrefix="muc1" %>
<div id="cntGetID" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
    
<muc1:SqlContent ID="sqlcontent1"   runat="server" />
</ContentTemplate>
    </asp:UpdatePanel>
</div>
</div>

    
</div>