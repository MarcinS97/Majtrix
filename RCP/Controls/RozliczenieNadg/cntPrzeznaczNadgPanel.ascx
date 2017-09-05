<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPrzeznaczNadgPanel.ascx.cs" Inherits="HRRcp.Controls.cntPrzeznaczNadgPanel" %>

<%@ Register src="Przypisania/cntStruktura.ascx" tagname="cntStruktura" tagprefix="uc1" %>
<%@ Register src="cntPrzeznaczNadg2.ascx" tagname="cntPrzeznaczNadg2" tagprefix="uc2" %>

<div id="paPrzeznaczNadg" runat="server" class="cntPrzeznaczNadg">
    <table class="tbPrzeznaczNadg table0">
        <tr>
            <td>
                <uc1:cntStruktura ID="cntStruktura1" runat="server" OnSelectedChanged="cntStruktura1_SelectedChanged" />
            </td>
            <td>
                <uc2:cntPrzeznaczNadg2 ID="cntKartotekaNadg" runat="server" />
            </td>
        </tr>
    </table>
</div>