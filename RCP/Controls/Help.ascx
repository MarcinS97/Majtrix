<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Help.ascx.cs" Inherits="HRRcp.Controls.Help" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

<asp:HiddenField ID="hidContext" runat="server" />
<asp:HiddenField ID="hidStoreContext" runat="server" />

<asp:Panel ID="paHelp" CssClass="help" runat="server">
    <asp:Literal ID="ltHelp" runat="server"></asp:Literal>
    <div id="paEditor" runat="server" class="paEditor" visible="false">
        <span class="label">Typ:</span>
        <asp:Label ID="lbTyp" CssClass="typ" runat="server" ></asp:Label><br />
        <span class="label1">Opis (w administracji):</span><br />
        <asp:TextBox ID="tbOpis" CssClass="textbox" runat="server"></asp:TextBox><br />
        <span class="label1">Tekst pomocy:</span><br />
        <cc1:Editor ID="edHelp" NoUnicode="true" runat="server" />
        <div class="buttons" >
            <asp:Button ID="btSave" CssClass="button75" runat="server" Text="Zapisz" onclick="btSave_Click" />
            <asp:Button ID="btCancel" CssClass="button75" runat="server" Text="Anuluj" onclick="btCancel_Click" />
        </div>
    </div>
    <div id="helpHideButton" runat="server" class="helpHideButton" >
        <asp:LinkButton ID="btEdit" runat="server" onclick="btEdit_Click" Visible="false">Edytuj</asp:LinkButton>
        <asp:LinkButton ID="btHelpHide" runat="server" onclick="btHelpHide_Click">Ukryj</asp:LinkButton>
    </div>
</asp:Panel>