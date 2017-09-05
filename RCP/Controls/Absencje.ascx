<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Absencje.ascx.cs" Inherits="HRRcp.Controls.Absencje" %>

<asp:HiddenField ID="hidKierId" runat="server" />
<asp:HiddenField ID="hidDataOd" runat="server" />

<asp:ListView ID="lvAbsencje" runat="server" DataSourceID="SqlDataSource1">
    <ItemTemplate>
        <tr style="">
            <td>
                <asp:Label ID="DataOdLabel" runat="server" Text='<%# Eval("DataOd") %>' />
            </td>
            <td>
                <asp:Label ID="DataDoLabel" runat="server" Text='<%# Eval("DataDo") %>' />
            </td>
            <td>
                <asp:Label ID="SymbolLabel" runat="server" Text='<%# Eval("Symbol") + " - " + Eval("Nazwa") %>' />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" style="">
            <tr>
                <td>
                    Brak danych
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table runat="server">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">
                                Od</th>
                            <th runat="server">
                                Do</th>
                            <th runat="server">
                                Rodzaj absencji</th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server">
                <td runat="server" style="">
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT Absencja.DataOd, Absencja.DataDo, AbsencjaKody.Symbol, AbsencjaKody.Nazwa FROM Absencja INNER JOIN AbsencjaKody ON Absencja.Kod = AbsencjaKody.Kod">
</asp:SqlDataSource>

