<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntFooter.ascx.cs" Inherits="HRRcp.Controls.cntFooter" %>

<div id="footer" class="footer printoff">
	<div class="center">
		<asp:Label ID="lbAppName" CssClass="appname" runat="server" ></asp:Label> v. <asp:Label ID="lbVersion" runat="server" Text="1.0.0.0"></asp:Label>
        <asp:Label ID="lbCopyright" CssClass="copyright" runat="server" Visible="false"></asp:Label>
        <asp:HyperLink ID="aCopyright" CssClass="copyright" runat="server" ></asp:HyperLink>
        <asp:LinkButton ID="btRegulamin" CssClass="regulamin" runat="server" onclick="btRegulamin_Click" Visible="false" >Regulamin</asp:LinkButton>
	</div>
</div>
