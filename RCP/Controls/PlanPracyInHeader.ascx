<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanPracyInHeader.ascx.cs" Inherits="HRRcp.Controls.PlanPracyInHeader" %>

<asp:Literal ID="ltThCheck" runat="server" Visible="false"></asp:Literal>

<asp:Repeater ID="Repeater1" runat="server" 
    onitemdatabound="Repeater1_ItemDataBound" 
    ondatabinding="Repeater1_DataBinding" >
    <ItemTemplate>
        </th><asp:Literal ID="ltThDay" runat="server"></asp:Literal>
            <asp:Image ID="imgAccepted" ImageUrl="../images/ok_small.png" visible="false" runat="server" />
            <asp:Label ID="DataLabel" CssClass="day_date" runat="server" /><br />
            <asp:Label ID="DayLabel" CssClass="day_name" runat="server" />
    </ItemTemplate>
</asp:Repeater>

<asp:Repeater ID="Repeater2" runat="server" Visible="false"
    onitemdatabound="Repeater2_ItemDataBound" 
    ondatabinding="Repeater2_DataBinding" >    
    <ItemTemplate>
        </th><th class='suma<%# GetSumaCss() %>' >
            <asp:Label ID="lb1" CssClass="lb1" runat="server" />
            <asp:Label ID="lb2" CssClass="lb2" runat="server" />        
    </ItemTemplate>
</asp:Repeater>
