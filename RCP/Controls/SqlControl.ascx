<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SqlControl.ascx.cs" Inherits="HRRcp.Controls.SqlControl" %>

<asp:TextBox ID="tbSQL" runat="server" Rows="5" TextMode="MultiLine"></asp:TextBox><br />
<asp:Button ID="btExec" runat="server" Text="Wykonaj" onclick="btExec_Click" /><br />
<asp:Literal ID="ltMessage" runat="server"></asp:Literal><br />
<asp:Literal ID="ltResult" runat="server"></asp:Literal>
