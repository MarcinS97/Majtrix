<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPlanUrlopow.ascx.cs" Inherits="HRRcp.Controls.cntPlanUrlopow" %>
<%@ Register src="cntSelectUrlop.ascx" tagname="cntSelectUrlop" tagprefix="uc1" %>
<%@ Register src="../SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc1" %>
<%@ Register src="../PlanPracy.ascx" tagname="PlanPracy" tagprefix="uc1" %>
<%@ Register src="../AbsencjaLegenda.ascx" tagname="AbsencjaLegenda" tagprefix="uc2" %>

<div class="cntPlanUrlopow PlanPracyZmiany">
    <div id="divSelectZmiana" class="cntSelectZmiana" runat="server">
        <div class="title1">    
            <asp:Label ID="lbZmianyQ" runat="server" CssClass="t5" Text="Typy absencji"></asp:Label>
            <asp:Label ID="lbZmianyE" runat="server" CssClass="t5" Visible="false" Text="1) Wybierz typ urlopu/absencji do naniesienia:"></asp:Label>
        </div>
        <uc1:cntSelectUrlop ID="cntSelectZmiana" OnSelectedChanged="OnSelectZmiana" Mode="1" runat="server" />
    </div>
    <table class="okres_navigator">
        <tr>
            <td class="colleft">
                <asp:Label ID="lbPlanQ" runat="server" CssClass="t5" Text="Plan urlopów"></asp:Label>
                <asp:Label ID="lbPlanE" runat="server" CssClass="t5" Visible="false" Text="2) Kliknij w dzień i ustaw urlop/absencję:"></asp:Label>
            </td>
            <td class="colmiddle">
                <uc1:SelectOkres ID="cntSelectOkres" StoreInSession="true" ControlID="cntPlanPracy" OnOkresChanged="cntSelectOkres_Changed" runat="server" />
            </td>
            <td class="colright">
                <asp:Label ID="lbOkresStatus" CssClass="t1" Visible="false" runat="server" ></asp:Label>
                <asp:Button ID="btEditPP" runat="server" Text="Edycja" Visible="false" CssClass="button75" onclick="btEditPP_Click" />
                <asp:Button ID="btCheckPP" runat="server" Text="Sprawdź" Visible="false" CssClass="button75" onclick="btCheckPP_Click" />
                <asp:Button ID="btSavePP" runat="server" Text="Zapisz" Visible="false" CssClass="button75" onclick="btSavePP_Click" />
                <asp:Button ID="btCancelPP" runat="server" Text="Anuluj" Visible="false" CssClass="button75" onclick="btCancelPP_Click" />
            </td>
        </tr>
    </table>
    <uc1:PlanPracy ID="cntPlanPracy" Mode="4" OnShowPlanUrlopow="cntPlanPracy_ShowPlanUrlopow" runat="server" />
    <uc2:AbsencjaLegenda ID="AbsencjaLegenda" runat="server" />
</div>
