<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPDFReader.ascx.cs" Inherits="HRRcp.Controls.cntPDFReader" %>

<asp:Button ID="PrintButton" CssClass="button printoff prButton" Text="Drukuj" runat="server" OnClientClick="javascript:window.print();" Visible="false" />
<div class="printoff dInC">
    <asp:Label ID="DocGTitle" runat="server" /><br />
    <asp:Label ID="DocTitle" runat="server" />
</div>
<asp:Literal ID="Literal1" runat="server" Visible="true"></asp:Literal>
