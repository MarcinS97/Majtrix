<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepHeader.ascx.cs" Inherits="HRRcp.Controls.RepHeader" %>

<table class="rep_header" name="header"><tr><td class="col1">
    <img id="img1" alt="" src="../images/captions/rep1.png" runat="server" />
    <img id="img2" alt="" src="../images/captions/rep2.png" runat="server" />
</td><td class="col2">
    <asp:Label ID="lbCaption0" runat="server" CssClass="printon" Text="Rejestracja Czasu Pracy<br />"></asp:Label>
    <asp:Label ID="lbCaption" runat="server" ></asp:Label>
    <asp:Label ID="lbCaption1" runat="server"  CssClass="t1" ></asp:Label>
    <br /><asp:Label ID="lbCaption2" runat="server" CssClass="t1" ></asp:Label>
</td><td class="col3">
    <img id="img3" alt="" src="../images/RepLogo.png" runat="server" />
</td></tr></table>
