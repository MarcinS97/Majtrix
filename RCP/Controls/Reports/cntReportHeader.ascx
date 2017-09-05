<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntReportHeader.ascx.cs" Inherits="HRRcp.Controls.cntReportHeader" %>

<table class="rep_header" name="header"><tr><td class="col1">
    <img id="img1" alt="" src="~/images/captions/rep1.png" runat="server" />
    <img id="img2" alt="" src="~/images/captions/rep2.png" runat="server" visible="false"/>
</td><td class="col2">
    <asp:Label ID="lbCaption0" runat="server" CssClass="printon" Text="Rejestracja Czasu Pracy<br />"></asp:Label>
    <asp:Label ID="lbCaption" runat="server" CssClass="title0" name="title" ></asp:Label>
    <asp:Label ID="lbCaption1" runat="server" CssClass="title1" ></asp:Label>
    <asp:Label ID="lbCaption2" runat="server" CssClass="title2" ></asp:Label>
    <asp:Label ID="lbCaption3" runat="server" CssClass="title3" ></asp:Label>
</td><td class="col3">
    <%--
    <img id="img3" alt="" src="~/images/RepLogo.png" runat="server" />
    --%>
    <asp:Image ID="img3" runat="server" ImageUrl="~/images/RepLogo.png" />
</td></tr></table>
