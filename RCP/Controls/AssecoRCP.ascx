<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssecoRCP.ascx.cs" Inherits="HRRcp.Controls.AssecoRCP" %>

<asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
    AllowSorting="True" CellPadding="4" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None">
    <RowStyle BackColor="#EFF3FB" />
    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <EditRowStyle BackColor="#2461BF" />
    <AlternatingRowStyle BackColor="White" />
</asp:GridView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:ASSECO_RCP %>" 
    SelectCommand="SELECT * FROM [DaneRCP]"></asp:SqlDataSource>

