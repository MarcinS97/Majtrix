<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepPracInfo.ascx.cs" Inherits="HRRcp.Controls.RepPracInfo" %>

<asp:HiddenField ID="hidPracId" runat="server" />

<table class="table0 cntRepPracInfo" >
    <tr>
        <td class="col1"><span class="t1">Imię i nazwisko:</span></td>
        <td class="col2">
            <asp:Label ID="lbImieNazwisko" runat="server" ></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="col1"><span class="t1">Numer ewidencyjny:</span></td>
        <td class="col2">
            <asp:Label ID="lbNrEw" runat="server" ></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="col1"><span class="t1">Dział:</span></td>
        <td class="col2">
            <asp:Label ID="lbDzial" runat="server" ></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="col1"><span class="t1">Stanowisko:</span></td>
        <td class="col2">
            <asp:Label ID="lbStanowisko" runat="server" ></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="col1"><span class="t1">Data zatrudnienia:</span></td>
        <td class="col2">
            <asp:Label ID="lbDataZatrudnienia" runat="server" ></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="col1"><span class="t1">Wymiar etatu:</span></td>
        <td class="col2">
            <asp:Label ID="lbWymiarEtatu" runat="server" ></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="col1"><span class="t1">Inne informacje:</span></td>
        <td class="col2">
            <asp:Label ID="lbInformacje" runat="server" ></asp:Label>
        </td>
    </tr>
</table>