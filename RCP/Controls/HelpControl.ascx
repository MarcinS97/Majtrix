<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpControl.ascx.cs" Inherits="HRRcp.Controls.HelpControl" %>

<asp:HiddenField ID="hidContext" runat="server" />
<asp:HiddenField ID="hidStoreContext" runat="server" />

<asp:Panel ID="paHelp" CssClass="help" runat="server">
    <asp:Literal ID="ltHelp" runat="server"></asp:Literal>
    <div id="helpHideButton" >
        <asp:LinkButton ID="btHelpHide" runat="server" onclick="btHelpHide_Click">Ukryj</asp:LinkButton>
    </div>
</asp:Panel>