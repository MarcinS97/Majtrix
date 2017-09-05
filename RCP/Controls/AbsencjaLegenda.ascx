<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AbsencjaLegenda.ascx.cs" Inherits="HRRcp.Controls.AbsencjaLegenda" %>

<div class="legendabox">
    <span class="t5">Kody absencji</span><br />
    <div class="legenda AbsencjaLegenda">
        <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1" >
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
                <span class="item">
                    <asp:Label ID="lbSymbol" CssClass="symbol" runat="server" Text='<%# Eval("Symbol") %>'></asp:Label> -
                    <asp:Label ID="lbOpis" CssClass="nazwa" runat="server" Text='<%# Prepare(Eval("Nazwa")) %>'></asp:Label>&nbsp;&nbsp;&nbsp;<br />
                </span>
            </ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT distinct Symbol, Nazwa FROM [AbsencjaKody] WHERE Widoczny = 1 ORDER BY [Symbol], [Nazwa]">
</asp:SqlDataSource>
