<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdmSettingsControl.ascx.cs" Inherits="HRRcp.Controls.AdmSettingsControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

<!-- group box -->
<table class="GroupBox1" width="100%"><tr><td class="tl" >&nbsp;&nbsp;</td><td class="th" ><div class="title">
    POBIERANIE DANYCH PRACOWNIKÓW
    </div></td><td class="tr" >&nbsp;&nbsp;&nbsp;</td></tr><tr><td class="vl"></td><td>
    <!-- group box content -->
    <table class="table3C">
        <tr>
            <td class="col1">
                Kontroler Active Directory:<br />
                <span class="t4">(nazwa lub adres ip serwera)</span>
            </td>
            <td class="col2">
                <asp:TextBox ID="tbADController" class="textbox" runat="server" Width="300px" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr class="lastline">
            <td class="col1">
                Ścieżka organizacyjna:<br />
                <span class="t4">(miejsce w strukturze organizacyjnej AD)</span>
            </td>
            <td class="col2">
                <asp:TextBox ID="tbADPath" class="textbox" runat="server" Width="600px" MaxLength="500"></asp:TextBox>
            </td>
        </tr>
    </table>
    <!-- group box end content -->
    </td><td class="vr" >&nbsp;</td></tr><tr><td class="bl"></td><td class="bh"></td><td class="br"></td></tr>
</table>
<!-- group box end -->
<table width="100%" cellpadding="0px" cellspacing="0px" style="border-collapse:collapse; vertical-align:top">
    <tr>
        <td id="paParEdit1" runat="server" align="right" style="padding:8px 0px 0px 0px;">
            <asp:Button ID="btEdit" class="button75" runat="server" Text="Edycja" onclick="btEdit_Click" />
            <asp:Button ID="btSave" class="button75" runat="server" Text="Zapisz" Visible="False" onclick="btSave_Click" />
            <asp:Button ID="btCancel" class="button75" runat="server" Text="Anuluj" Visible="False" onclick="btCancel_Click" />
        </td>
    </tr>
</table>
