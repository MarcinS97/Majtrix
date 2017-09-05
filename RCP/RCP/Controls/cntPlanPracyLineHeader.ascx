<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPlanPracyLineHeader.ascx.cs" Inherits="HRRcp.RCP.Controls.cntPlanPracyLineHeader" %>

<asp:HiddenField ID="hidFrom" runat="server" Visible="false" />
<asp:HiddenField ID="hidTo" runat="server" Visible="false" />
<asp:HiddenField ID="hidHeaderData" runat="server" Visible="false" />
<asp:HiddenField ID="hidSumHeader" runat="server" Visible="false" />

<asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1" 
    onitemdatabound="Repeater1_ItemDataBound" 
    ondatabinding="Repeater1_DataBinding" EnableViewState="True">
    <ItemTemplate>
        <th id="thData" class="day" runat="server" >
            <asp:Image ID="imgAccepted" ImageUrl="../images/ok_small.png" visible="false" runat="server" />
            <asp:Label ID="DataLabel" CssClass="day_date" runat="server" /><br />
            <asp:Label ID="DayLabel" CssClass="day_name" runat="server" />
            <asp:CheckBox ID="cbDay" CssClass="checkbox" runat="server" />
        </th>
    </ItemTemplate>
</asp:Repeater>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="set datefirst 7;
                   select D.Data, DATEPART(WEEKDAY,D.Data) as Day, K.Rodzaj, K.Opis 
                   from GetDates2(@from,@to) D 
                   left outer join Kalendarz K on K.Data = D.Data">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidFrom" Name="from" PropertyName="Value" />
        <asp:ControlParameter ControlID="hidTo" Name="to" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>

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
<%--<th id="thCheck" runat="server" visible="false">
    <asp:CheckBox ID="cbSelectAll" runat="server" Checked="true" OnClick="javascript:cbSelectAllRight(this);" />
</th>--%>


            <%--
            <asp:Label ID="DataLabel" CssClass="day_date" runat="server" Text='<%# Eval("Data", "{0:%d}") %>' /><br />
            --%>

<%--
<asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1" 
    onitemdatabound="Repeater1_ItemDataBound" 
    ondatabinding="Repeater1_DataBinding">
    <HeaderTemplate>
        </th>
    </HeaderTemplate>
    <ItemTemplate>
        <th id="thData" class="day" runat="server" >
            <asp:Label ID="DataLabel" CssClass="day_date" runat="server" Text='<%# Eval("Data", "{0:%d}") %>' /><br />
            <asp:Label ID="DayLabel" CssClass="day_name" runat="server" />
            <asp:CheckBox ID="cbDay" runat="server" />
        </th>
    </ItemTemplate>
    <FooterTemplate>
        <th style="display:none;">
    </FooterTemplate>
</asp:Repeater>
--%>