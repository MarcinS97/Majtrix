<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanPracyRozliczenie.ascx.cs" Inherits="HRRcp.Controls.PlanPracyRozliczenie" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register src="~/Controls/RozliczenieNadg/cntSelectOkres.ascx" tagname="SelectOkres" tagprefix="uc1" %>
<%@ Register src="~/Controls/RozliczenieNadg/cntPrzeznaczNadg2Popup.ascx" tagname="cntPrzeznaczNadg2Popup" tagprefix="uc2" %>
<%@ Register src="PlanPracy.ascx" tagname="PlanPracy" tagprefix="uc1" %>
<%@ Register src="DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>

<div class="PlanPracyAccept PlanPracyRozliczenie">
    <table class="okres_navigator">
        <tr>
            <td class="colleft">
                <asp:Label ID="lbPlanQ" runat="server" CssClass="t5" Text="Rozliczenie nadgodzin"></asp:Label>
            </td>
            <td class="colmiddle">
                <uc1:SelectOkres ID="cntSelectOkres" ControlID="cntPlanPracy" OnOkresChanged="cntSelectOkres_Changed" runat="server" />
            </td>
            <td class="colright">
                <asp:Label ID="lbOkresStatus" CssClass="t1" Visible="false" runat="server" ></asp:Label>
                <asp:Button ID="btAccept" runat="server" Text="Akceptuj" CssClass="button75" onclick="btAcceptPP_Click" Visible="false"/>
                <asp:Button ID="btCheck" runat="server" Text="Sprawdź" CssClass="button75" onclick="btCheckPP_Click" Visible="false" />
            </td>
        </tr>
    </table>
    <uc1:PlanPracy ID="cntPlanPracy" Mode="3" OnShowRozliczenie="cntPlanPracy_ShowRozliczenie" runat="server" />
</div>

<%--
<asp:Button ID="tmpBtModalPopup" runat="server" style="display: none;" />
<asp:Panel ID="paAccept" CssClass="modalPopupAcc" Style="display: none;" runat="server" DefaultButton="tmpBtModalPopup">
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

<uc2:cntPrzeznaczNadg2Popup ID="cntPrzeznaczNadg2Popup" OnChanged="cntPrzeznaczNadg_Changed" runat="server" />
--%>
