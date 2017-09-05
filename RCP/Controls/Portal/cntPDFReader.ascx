<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPDFReader.ascx.cs" Inherits="HRRcp.Controls.cntPDFReader" %>

<table class="PDFRHeader">
    <tr>
        <td>
            <asp:Button runat="server" CssClass="button btn btn-primary" Text="Cofnij" onclick="Unnamed1_Click" />
        </td>
        <td style="text-align: right;">
            <h3><asp:Label ID="DocGTitle" runat="server" /></h3>
        </td>
        <td>
            <asp:Button ID="PrintButton" CssClass="button" Text="Drukuj" 
                runat="server" Visible="false" 
                onclick="PrintButton_Click" />
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <h4><asp:Label ID="DocTitle" runat="server" /></h4>
        </td>
    </tr>
</table>
<asp:Literal ID="Literal1" runat="server" Visible="true"></asp:Literal>
