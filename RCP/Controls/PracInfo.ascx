<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PracInfo.ascx.cs" Inherits="HRRcp.Controls.PracInfo" %>

<!-- group box -->
<table class="GroupBox1" width="100%"><tr><td class="tl" >&nbsp;&nbsp;</td><td class="th" ><div class="title">    
    <asp:Label ID="lbTitle" runat="server" Text="DANE PRACOWNIKA"></asp:Label>
    </div></td><td class="tr" >&nbsp;&nbsp;&nbsp;</td></tr><tr><td class="vl"></td><td>
    <!-- group box content -->
    <table class="table3A tbAccept2">
        <tr>
            <td>Data</td>
            <td><asp:Label ID="lbDay" runat="server" ></asp:Label></td>
        </tr>
        <tr>
            <td>Pracownik</td>
            <td><asp:Label ID="lbPracownik" runat="server" ></asp:Label></td>
        </tr>
        <tr>
            <td>Dział</td>
            <td><asp:Label ID="lbDzial" runat="server" ></asp:Label></td>
        </tr>
        <tr>
            <td>Stanowisko</td>
            <td><asp:Label ID="lbStanowisko" runat="server" ></asp:Label></td>
        </tr>
        <tr>
            <td>Zmiana</td>
            <td><asp:Label ID="lbZmiana" runat="server" ></asp:Label></td>
        </tr>
        <tr>
            <td>Strefa RCP</td>
            <td><asp:Label ID="lbStrefaRCP" runat="server" ></asp:Label></td>
        </tr>
        <tr>
            <td>Algorytm</td>
            <td><asp:Label ID="lbAlgorytm" runat="server" ></asp:Label></td>
        </tr>
    </table>
    <!-- group box end content -->
    </td><td class="vr" >&nbsp;</td></tr><tr><td class="bl"></td><td class="bh"></td><td class="br"></td></tr>
</table>
<!-- group box end -->


