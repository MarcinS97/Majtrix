<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FooterControl.ascx.cs" Inherits="HRRcp.Controls.FooterControl" %>

<div id="footer" class="printoff footer navbar-fixed-bottom">
	<div class="center">
		<asp:Label ID="lbProgram" runat="server" Text="Rejestracja Czasu Pracy"></asp:Label> v. <asp:Label ID="lbVersion" runat="server" Text="1.0.0.0"></asp:Label>
		&nbsp;&nbsp;&nbsp;<asp:Label ID="lbJabil" runat="server" CssClass="lblApp" ></asp:Label>
        &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="btRegulamin" CssClass="regulamin" runat="server" onclick="btRegulamin_Click">Regulamin</asp:LinkButton>
	</div>
</div>
