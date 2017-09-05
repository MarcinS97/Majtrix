<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPlanPracyLine2.ascx.cs" Inherits="HRRcp.RCP.Controls.cntPlanPracyLine2" %>
<asp:Repeater ID="Repeater1" runat="server" 
    onitemdatabound="Repeater1_ItemDataBound" onprerender="Repeater1_PreRender" ><ItemTemplate><td class="day" runat="server" ID="tdPP"><div class="bck" runat="server" ID="bckPP"></div><div class="day" runat="server" ID="divPP"><asp:Label ID="lbOverlap" CssClass="overlaptext" runat="server"></asp:Label><asp:Label ID="lbTop" CssClass="toptext" runat="server"></asp:Label><asp:Label ID="lbBottom" CssClass="bottomtext" runat="server"></asp:Label></div></td></ItemTemplate></asp:Repeater><asp:Repeater ID="Repeater2" runat="server" 
    onitemdatabound="Repeater2_ItemDataBound" onprerender="Repeater2_PreRender" ><ItemTemplate><td runat="server" ID="tdSuma" class="suma"><div class="suma" runat="server" ID="divPP"><asp:Label ID="lb0" CssClass="lb0" runat="server"></asp:Label><asp:Label ID="lb1" CssClass="lb1" runat="server"></asp:Label><asp:Label ID="lb2" CssClass="lb2" runat="server"></asp:Label><asp:Label ID="lb3" CssClass="lb3" runat="server"></asp:Label></div></td></ItemTemplate></asp:Repeater>
<%--<td id="tdCheck" runat="server" visible="false">
    <asp:CheckBox ID="cbSelect" runat="server" Checked="true" CssClass="cbSelectRight" />
</td>--%>

<asp:HiddenField ID="hidPracId" runat="server" />
<asp:HiddenField ID="hidZmiany" runat="server" />
<asp:HiddenField ID="hidSumy" runat="server" Visible="false" />
