<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PracUrlop2.ascx.cs" Inherits="HRRcp.Controls.PracUrlop2" %>

<div class="scroller">
    <asp:GridView ID="gvUrlopy" runat="server" 
        AllowSorting="false" CssClass="GridView1 gvUrlopPrac table table-striped table-bordered"
        DataSourceID="SqlDataSource1" 
        AllowPaging="true"
        PageSize="20"
        PagerSettings-Mode="NumericFirstLast"
        GridLines="None">
        <%--<RowStyle BackColor="#EFF3FB" /> ForeColor="#333333"
        <FooterStyle BackColor="#ffffff" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#ffffff" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" />--%>
        <%--<AlternatingRowStyle BackColor="White" />--%>
        <EmptyDataTemplate>
            Brak danych
        </EmptyDataTemplate>
    </asp:GridView>
</div>    

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" >
</asp:SqlDataSource>
