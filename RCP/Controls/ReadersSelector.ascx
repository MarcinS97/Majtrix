<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReadersSelector.ascx.cs" Inherits="HRRcp.Controls.ReadersSelector" %>

<asp:HiddenField ID="hidReaders" runat="server" />

<asp:ListView ID="lvReaders" runat="server" 
    DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" onitemdatabound="lvReaders_ItemDataBound">
    <ItemTemplate>
        <tr class="it">
            <td class="check">
                <asp:CheckBox ID="Select" runat="server" />            
                <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("Id") %>' />
            </td>
            <td>
                <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
            </td>
            <td>
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Name") %>' />
            </td>
            <td>
                <asp:Label ID="ZoneLabel" runat="server" Text='<%# Eval("Zone") %>' />
            </td>
            <td>
                <asp:Label ID="InOutLabel" runat="server" Text='<%# GetInOutText(Eval("InOut")) %>' />
                <asp:DropDownList ID="ddlInOut" runat="server" Visible="false">
                    <asp:ListItem Text="IN" Value="0"></asp:ListItem>
                    <asp:ListItem Text="OUT" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" style="" class="ListView1 hoverline">
            <tr>
                <td>
                    <table ID="itemPlaceholderContainer" class="tbReadersSelector" runat="server" border="0" style="">
                        <tr id="Tr1" runat="server" style="">
                            <th id="Th1" runat="server"></th>
                            <th id="Th2" runat="server">#</th>
                            <th id="Th3" runat="server">Nazwa</th>
                            <th id="Th4" runat="server">Zone</th>
                            <th id="Th5" runat="server">Typ</th>
                        </tr>
                        <tr runat="server">
                            <td colspan="5">
                                Brak danych
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1 hoverline narrow">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" class="tbReadersSelector" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server"></th>
                            <th runat="server">#</th>
                            <th runat="server">Nazwa</th>
                            <th runat="server">Zone</th>
                            <th runat="server">Typ</th>
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
    SelectCommand="SELECT * FROM [Readers] ORDER BY [Id]">
</asp:SqlDataSource>

