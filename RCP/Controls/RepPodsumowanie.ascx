<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepPodsumowanie.ascx.cs" Inherits="HRRcp.Controls.RepPodsumowanie" %>

<table class="table3B" id="wyniki_podsumowanie" name="report">
    <tr>
        <td class="col1">Czas pracy</td>
        <td class="col2"><asp:LinkButton ID="LinkButton1" runat="server" onclick="zoom_Click" ></asp:LinkButton></td>
    </tr>
    <tr>
        <td class="col1">Czas pracy pracownika</td>
        <td class="col2"><asp:LinkButton ID="LinkButton9" runat="server" onclick="zoom_Click" ></asp:LinkButton></td>
    </tr>
    <tr>
        <td class="col1">Roczna karta pracy pracowika</td>
        <td class="col2"><asp:LinkButton ID="LinkButton2" runat="server" onclick="zoom_Click" ></asp:LinkButton></td>
    </tr>
    <tr>
        <td class="col1">Czas pracy i nadgodziny - sumy</td>
        <td class="col2"><asp:LinkButton ID="LinkButton4" runat="server" onclick="zoom_Click" ></asp:LinkButton></td>
    </tr>
    <tr>
        <td class="col1">Alerty</td>
        <td class="col2"><asp:LinkButton ID="LinkButton3" runat="server" onclick="zoom_Click" ></asp:LinkButton></td>
    </tr>
</table>
