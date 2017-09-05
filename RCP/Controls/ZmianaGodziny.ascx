<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ZmianaGodziny.ascx.cs" Inherits="HRRcp.Controls.ZmianaGodziny" %>

<asp:HiddenField ID="hidMode" runat="server" />
<asp:HiddenField ID="hidOvertimes" runat="server" />
<asp:HiddenField ID="hidZmianDo" runat="server" />

<table class="zmianyhours">
    <tr>
        <th class="col1">
            <span class="t1">Nadgodzina</span><br />
        </th>
        <th class="col2">
            <span class="t1">Stawka</span><br />
        </th>
        <th class="control">
            <asp:Button ID="btAdd" runat="server" Text="Dodaj" onclick="btAdd_Click" />
        </th>
    </tr>
    <asp:PlaceHolder ID="phHours" runat="server"></asp:PlaceHolder>
</table>
