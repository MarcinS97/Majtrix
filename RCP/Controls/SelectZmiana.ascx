<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectZmiana.ascx.cs" Inherits="HRRcp.Controls.SelectZmiana" %>

<asp:HiddenField ID="hidEditMode" runat="server" />

<asp:ListView ID="lvZmiany" runat="server" DataSourceID="SqlDataSource1" DataKeyNames="Id" 
    onselectedindexchanged="lvZmiany_SelectedIndexChanged" 
    ondatabound="lvZmiany_DataBound" 
    onitemdatabound="lvZmiany_ItemDataBound" >
    <ItemTemplate>
        <tr class="it">
            <td id="tdSelect" runat="server" class="control">
                <asp:Button ID="btSelect" runat="server" CommandName="Select" Text="Wybierz" />
            </td>
            <td>
                <asp:Panel ID="KolorSample" CssClass="colorpicker" BackColor='<%# GetColorNull(Eval("Kolor").ToString()) %>' runat="server">
                    <asp:Label ID="SymbolLabel" runat="server" Text='<%# Eval("Symbol") %>' />
                </asp:Panel>
                <%--
                <asp:HiddenField ID="hidColor" Value='<%# GetColorNull(Eval("Kolor").ToString()) %>' runat="server" />
                --%>
            </td>
            <td>
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("CzasOd") %>' />
            </td>
            <td>
                <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("CzasDo") %>' />
            </td>
            <td>
                <asp:Label ID="StawkaLabel" runat="server" Text='<%# StawkaText(Eval("Stawka").ToString()) %>' />
            </td>
            <td>
                <asp:Label ID="NadgodzinyLabel" runat="server" Text='<%# Eval("Nadgodziny") %>' />
            </td>
        </tr>
    </ItemTemplate>
    <SelectedItemTemplate>
        <tr class="sit">
            <td class="control">
            </td>
            <td>
                <asp:Panel ID="KolorSample" CssClass="colorpicker" BackColor='<%# GetColorNull(Eval("Kolor").ToString()) %>' runat="server">
                    <asp:Label ID="SymbolLabel" runat="server" Text='<%# Eval("Symbol") %>' />
                </asp:Panel>
                <%--
                <asp:HiddenField ID="hidColor" Value='<%# GetColorNull(Eval("Kolor").ToString()) %>' runat="server" />
                --%>
            </td>
            <td>
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("CzasOd") %>' />
            </td>
            <td>
                <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("CzasDo") %>' />
            </td>
            <td>
                <asp:Label ID="StawkaLabel" runat="server" Text='<%# StawkaText(Eval("Stawka").ToString()) %>' />
            </td>
            <td>
                <asp:Label ID="NadgodzinyLabel" runat="server" Text='<%# Eval("Nadgodziny") %>' />
            </td>
        </tr>
    </SelectedItemTemplate>
    <EmptyDataTemplate>
        <table id="Table1" runat="server" style="">
            <tr>
                <td>
                    Brak danych
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table id="Table2" runat="server" class="ListView1 ZmianySelect" >
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table ID="itemPlaceholderContainer" class="tbZmianySelect" runat="server" border="0" style="">
                        <tr id="Tr2" runat="server" style="">
                            <th id="thSelect" runat="server" class="control"></th>
                            <th id="Th2" runat="server">Symbol</th>
                            <th id="Th3" runat="server">Nazwa</th>
                            <th id="Th4" runat="server">Od</th>
                            <th id="Th5" runat="server">Do</th>
                            <th id="Th6" runat="server">Stawka</th>
                            <th id="Th7" runat="server">Nadgodziny</th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="Tr3" runat="server">
                <td id="Td2" runat="server" style="">
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT *, 
                        LEFT(convert(varchar, Od, 8),5) as CzasOd,
                        LEFT(convert(varchar, Do, 8),5) as CzasDo
                   FROM [Zmiany] 
                   WHERE [Visible]=1
                   ORDER BY [Symbol]" >
</asp:SqlDataSource>
