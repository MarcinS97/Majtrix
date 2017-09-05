<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntStrukturaAdm.ascx.cs" Inherits="HRRcp.Controls.Adm.cntStrukturaAdm" %>

<%@ Register src="../Przypisania/cntStruktura.ascx" tagname="cntStruktura" tagprefix="uc1" %>
<%@ Register src="~/Controls/SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc1" %>
<%@ Register src="~/Controls/RcpControl.ascx" tagname="RcpControl" tagprefix="uc1" %>

<div id="paStrukturaAdm" runat="server" class="cntStrukturaAdm">
    <table id="tbAdmStruktura" class="table0" >
        <tr>
            <td class="tdLeft" valign="top">
                <span class="t5">Struktura organizacyjna</span> 
                <table id="navStruktura" class="okres_navigator okres_navigator_struktura" runat="server">
                    <tr>
                        <td class="col1">
                            <uc1:selectokres ID="cntSelectOkresStruct" ControlID="cntStruktura" 
                                ParentID="navStruktura" ForStruktura="true" runat="server" />
                        </td>
                    </tr>
                </table>
                <uc1:cntStruktura ID="cntStruktura" runat="server" OnSelectedChanged="cntStruktura_SelectedChanged" />
            </td>
            <td class="tdRight" valign="top">
                <span class="t5">Dane RCP pracownika:</span> 
                <asp:Label ID="lbPracName" runat="server" ></asp:Label>
                <table class="okres_navigator okres_navigator_struktura">
                    <tr>
                        <td class="col1">
                            <uc1:selectokres ID="cntSelectOkres" ControlID="cntRCPStruct" runat="server" />
                        </td>
                        <td class="col2">
                            <asp:Button ID="btRefresh1" runat="server" class="button" onclick="btRefresh1_Click" Text="Odśwież" />
                            <asp:Button ID="btHide1" runat="server" class="button" Text="Ukryj szczegóły" />
                        </td>
                    </tr>
                </table>
                <uc1:rcpcontrol ID="cntRCPStruct" runat="server" />
            </td>
        </tr>
    </table>
</div>

