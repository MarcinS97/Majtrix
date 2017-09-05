<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepCzasPracy.ascx.cs" Inherits="HRRcp.Controls.RepCzasPracy" %>
<%@ Register src="~/Controls/RepHeader.ascx" tagname="RepHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc1" %>
<%@ Register src="~/Controls/RepNadgodziny3.ascx" tagname="RepNadgodziny" tagprefix="uc1" %>

<table class="okres_navigator printoff">
    <tr>
        <td class="colleft">
            <asp:Label ID="lbKierownik" runat="server" CssClass="t1" Text="Kierownik:"></asp:Label>
            <asp:DropDownList ID="ddlKierownicy" runat="server" AutoPostBack="true" onselectedindexchanged="ddlKierownicy_SelectedIndexChanged" />
        </td>
        <td class="colmiddle">
            <uc1:SelectOkres ID="cntSelectOkres" ControlID="repNadgodziny" OnOkresChanged="cntSelectOkres_Changed" runat="server" />
        </td>
        <td class="colright">
        </td>
    </tr>
</table>
<div class="divider_ppacc printoff"></div>

<uc1:RepHeader ID="repHeader" Caption="" Icon="1" runat="server" />
<uc1:RepNadgodziny ID="repNadgodziny" Mode="4" CCMode="1" runat="server" />
