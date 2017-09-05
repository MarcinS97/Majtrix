<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanPracySumHeader.ascx.cs" Inherits="HRRcp.Controls.PlanPracySumHeader" %>

<asp:HiddenField ID="x_hidHeaderData" runat="server" />
<asp:HiddenField ID="hidSumHeader" runat="server" />

<asp:Repeater ID="Repeater2" runat="server" 
    onitemdatabound="Repeater2_ItemDataBound" 
    onprerender="Repeater2_PreRender" >    
    <ItemTemplate>
        <th id="thSuma" class="suma" runat="server" >
            <asp:Label ID="lb1" CssClass="lb1" runat="server" />
            <asp:Label ID="lb2" CssClass="lb2" runat="server" />
        </th>
    </ItemTemplate>
</asp:Repeater>
