<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="x_PlanPracyLine.ascx.cs" Inherits="HRRcp.Controls.PlanPracyLine" %>

<asp:HiddenField ID="hidFrom" runat="server" />
<asp:HiddenField ID="hidTo" runat="server" />
<asp:HiddenField ID="hidPracId" runat="server" />

<asp:HiddenField ID="hidData1" runat="server" />

<asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1" 
    onitemcreated="Repeater1_ItemCreated">
    <HeaderTemplate>
        </td>
    </HeaderTemplate>
    <ItemTemplate>
        <td class="day" runat="server" onclick="javascript:selectCellPP(this);" ID="tdPP">
            <asp:Label ID="lbZmSymbol" runat="server" Text='<%# Eval("Symbol") %>'></asp:Label>
            <asp:HiddenField ID="hidZmId" runat="server" />
            
        </td>
    </ItemTemplate>
    <FooterTemplate>
        <td style="display: none;">
    </FooterTemplate>
</asp:Repeater>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select D.Data, P.IdZmiany, Z.*
                   from GetDates2(@from,@to) D 
                       left outer join PlanPracy P on P.Data=D.Data and P.IdPracownika=@pracId
                       left outer join Zmiany Z on Z.Id=P.IdZmiany">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidFrom" Name="from" PropertyName="Value" />
        <asp:ControlParameter ControlID="hidTo" Name="to" PropertyName="Value" />
        <asp:ControlParameter ControlID="hidPracId" Name="pracId" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>