<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntKierPath.ascx.cs" Inherits="HRRcp.SzkoleniaBHP.Controls.cntKierPath" %>

<div id="paKierPath" runat="server" class="cntKierPath">
    <asp:ImageButton ID="ibtUp" runat="server" ImageUrl="~/images/buttons/upitems.png" CssClass="img" CommandName="zoomKierUp" Visible="false" ToolTip="Cofnij" />
    <asp:LinkButton ID="lbtUp" runat="server" Text="Cofnij" CommandName="zoomKierUp" Visible="false" ToolTip="Cofnij" ></asp:LinkButton>
</div>