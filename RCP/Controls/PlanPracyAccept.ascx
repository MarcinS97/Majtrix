<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanPracyAccept.ascx.cs" Inherits="HRRcp.Controls.PlanPracyAccept" %>
<%@ Register src="SelectZmiana.ascx" tagname="SelectZmiana" tagprefix="uc1" %>
<%@ Register src="SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc1" %>
<%@ Register src="PlanPracy.ascx" tagname="PlanPracy" tagprefix="uc1" %>
<%@ Register src="AcceptControl2a.ascx" tagname="AcceptControl2" tagprefix="uc1" %>
<%@ Register src="DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register src="AbsencjaLegenda.ascx" tagname="AbsencjaLegenda" tagprefix="uc2" %>
<%@ Register src="PPDayLegenda.ascx" tagname="PPDayLegenda" tagprefix="uc2" %>
<%@ Register src="PPSumyLegenda.ascx" tagname="PPSumyLegenda" tagprefix="uc2" %>

<div class="PlanPracyAccept">
    <table class="okres_navigator">
        <tr>
            <td class="colleft">
                <asp:Label ID="lbPlanQ" runat="server" CssClass="t5" Text="Akceptacja czasu pracy"></asp:Label>
            </td>
            <td class="colmiddle">
                <uc1:SelectOkres ID="cntSelectOkres" ControlID="cntPlanPracy" StoreInSession="true" OnOkresChanged="cntSelectOkres_Changed" runat="server" />
            </td>
            <td class="colright">
                <asp:Label ID="lbOkresStatus" CssClass="t1" Visible="false" runat="server" ></asp:Label>
                <asp:Label ID="lbAccept" runat="server" Text="Akceptuj do:"></asp:Label>
                <uc1:DateEdit ID="deDataAccept" runat="server" />
                <asp:Button ID="btAccept" runat="server" Text="Akceptuj" CssClass="button75" onclick="btAcceptPP_Click" />
                <asp:Button ID="btCheck" runat="server" Text="Sprawdź" Visible="false" CssClass="button75" onclick="btCheckPP_Click" />
            </td>
        </tr>
    </table>
    <uc1:PlanPracy ID="cntPlanPracy" Mode="1" OnSelectDay="OnSelectDay" runat="server" />
</div>

<div class="legenda_col1">
    <uc2:PPDayLegenda ID="PPDayLegenda" runat="server" />
    <br />
    <uc2:PPSumyLegenda ID="PPSumyLegenda" runat="server" />
</div>
<uc2:AbsencjaLegenda ID="AbsencjaLegenda" runat="server" />

<asp:Button ID="tmpBtModalPopup" runat="server" style="display: none;" />
<asp:Panel ID="paAccept" CssClass="modalPopupAcc" Style="display: none;" runat="server" DefaultButton="tmpBtModalPopup">
    <uc1:AcceptControl2 ID="cntAccept2" 
        OnAcceptChanges="cntAccept_AcceptChanges" 
        OnCancelChanges="cntAccept_CancelChanges"
        runat="server" />
</asp:Panel>

<asp:ModalPopupExtender ID="paAccept_ModalPopupExtender" runat="server" 
    DynamicServicePath="" 
    Enabled="true" 
    BackgroundCssClass="modalBackground"    
    PopupControlID="paAccept"
    TargetControlID="tmpBtModalPopup"
    BehaviorID="mpe">    
    <Animations>
        <OnShown>
            <ScriptAction Script="setRcpScrollPos();" />
        </OnShown>    
    </Animations>
</asp:ModalPopupExtender>

