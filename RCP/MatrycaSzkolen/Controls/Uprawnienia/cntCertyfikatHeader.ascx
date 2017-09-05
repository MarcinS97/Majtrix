<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntCertyfikatHeader.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Uprawnienia.cntCertyfikatHeader" %>

<table class="cntCertyfikatHeader table0" name="report" >
    <tr class="title1">
        <td id="tdUprawnienie" runat="server" colspan="2">
            <h3><asp:Label ID="Label1" runat="server" Text="Uprawnienie"></asp:Label></h3>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="lbUprawnienie" runat="server" />
        </td>
    </tr>
    <tr id="trUpr2" runat="server" visible="false">
        <td class="label">
            <asp:Label ID="Label5" runat="server" Text="Poziom:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lbPoziom" runat="server" />
        </td>
    </tr>
    <tr id="trUpr3" runat="server" visible="false">
        <td class="label">
            <asp:Label ID="Label7" runat="server" Text="Symbol:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lbSymbol" runat="server" />
        </td>
    </tr>

    <tr class="title2">
        <td colspan="2">
            <h3><asp:Label ID="Label2" runat="server" Text="Dane pracownika"></asp:Label></h3>
        </td>
    </tr>
    <tr>
        <td class="label">
            <asp:Label ID="Label3" runat="server" Text="Pracownik:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lbPracownik" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="label">
            <asp:Label ID="Label6" runat="server" Text="Numer ewidencyjny:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lbNrEw" CssClass="value" runat="server" ></asp:Label>
        </td>
    </tr>
    <tr id="trPrac2" runat="server" visible="false">
        <td class="label">
            <asp:Label ID="Label8" runat="server" Text="Symbol spawacza:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lbSymbolSpawacza" CssClass="value" runat="server" ></asp:Label>
        </td>
    </tr>
    
    <tr class="title3">
        <td colspan="2">
            <h3><asp:Label ID="Label4" runat="server" Text="Certyfikaty"></asp:Label></h3>
        </td>
    </tr>
</table>

