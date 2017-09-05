<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KwitekHeader.ascx.cs" Inherits="HRRcp.Controls.Kwitek.KwitekHeader" %>

<asp:HiddenField ID="xhidKadryId" runat="server" />

<asp:DataList ID="dlHeader" runat="server" DataSourceID="SqlDataSource1">
    <ItemTemplate>
        <asp:Label ID="LpLogoLabel" runat="server" Text='<%# Eval("LpLogo") %>' />
        <asp:Label ID="ImieLabel" runat="server" Text='<%# Eval("Imie") %>' />
        <asp:Label ID="NazwiskoLabel" runat="server" Text='<%# Eval("Nazwisko") %>' />
        <br />
        Pesel: <asp:Label ID="PeselLabel" runat="server" Text='<%# Eval("Pesel") %>' /><br />        
        Lista: <asp:Label ID="NrListyLabel" runat="server" Text='<%# Eval("NrListy") %>' />, <asp:Label ID="KategoriaListyLabel" runat="server" Text='<%# Eval("KategoriaListy") %>' /><br />       
        Rok: <asp:Label ID="RokLabel" runat="server" Text='<%# Eval("Rok") %>' /> Miesiac: <asp:Label ID="MiesiacLabel" runat="server" Text='<%# Eval("Miesiac") %>' /><br />
        <br />
        Data: <asp:Label ID="DataLabel" runat="server" Text='<%# Eval("Data", "{0:d}") %>' /><br />
        <br />
        <br />
    </ItemTemplate>
</asp:DataList>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ASSECO %>" >
</asp:SqlDataSource>
