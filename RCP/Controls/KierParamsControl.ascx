<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KierParamsControl.ascx.cs" Inherits="HRRcp.Controls.KierParamsControl" %>

<asp:HiddenField ID="hidKierId" runat="server" />

<div class="cntKierParams">
    <!-- group box -->
    <table class="GroupBox1" width="100%"><tr><td class="tl" >&nbsp;&nbsp;</td><td class="th" ><div class="title">
        Indywidualne Ustawienia Kierownika
        </div></td><td class="tr" >&nbsp;&nbsp;&nbsp;</td></tr><tr><td class="vl"></td><td>
        <!-- group box content -->
        <table class="table0">
            <tr>
                <td class="col1">
                    Długość przerwy w czasie zmiany:<br />
                    <span class="t4">dodawana do czasu pracy (odejmowana od zmiany)</span>
                </td>
                <td class="col2">
                    <asp:DropDownList ID="ddlPrzerwa" runat="server" ></asp:DropDownList>
                    <span class="t4">(minuty)</span>
                </td>
            </tr>
            <tr>
                <td class="col1">
                    Długość przerwy w czasie nadgodzin:<br />
                    <span class="t4">dodawana do czasu nadgodzin</span>
                </td>
                <td class="col2">
                    <asp:DropDownList ID="ddlPrzerwa2" runat="server" ></asp:DropDownList>
                    <span class="t4">(minuty)</span>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="divider">
                    <div></div>
                </td>
            </tr>
            <tr class="lastline">
                <td class="col1">
                    Margines ostrzegania :<br />
                    <span class="t4">czas pracy krótszy niż zmiany bez przerwy</span>
                </td>
                <td class="col2">
                    <asp:DropDownList ID="ddlMargin" runat="server" ></asp:DropDownList>
                    <span class="t4">(minuty)</span>
                </td>
            </tr>
        </table>
        <!-- group box end content -->
        </td><td class="vr" >&nbsp;</td></tr><tr><td class="bl"></td><td class="bh">
        </td><td class="br"></td></tr>
    </table>
    <!-- group box end -->

    <asp:Button ID="btEdit" runat="server" Text="Edycja" CssClass="button75" onclick="btEdit_Click" />
    <asp:Button ID="btSave" runat="server" Text="Zapisz" Visible="false" CssClass="button75" onclick="btSave_Click" />
    <asp:Button ID="btCancel" runat="server" Text="Anuluj" Visible="false" CssClass="button75" onclick="btCancel_Click" />
</div>








