<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdmKodyWyjasnicControl.ascx.cs" Inherits="HRRcp.Controls.AdmKodyWyjasnicControl" %>

<asp:ListView ID="lvKodyWyjasnic" runat="server" DataSourceID="SqlDataSource1" 
    DataKeyNames="Id"
    InsertItemPosition="LastItem" 
    onitemdatabound="lvKodyWyjasnic_ItemDataBound" >
    <ItemTemplate>
        <tr class="it" style="">
            <td class="col1">
                <asp:Label ID="Kod" runat="server" Text='<%# Eval("Kod") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="Nazwa" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edycja" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
            </td>
        </tr>
    </ItemTemplate>
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
        <table id="Table2" class="ListView1 hoverline" runat="server">
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table ID="itemPlaceholderContainer" class="tbKodyWyjasnic" runat="server" border="0" style="">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" class="col1" runat="server">Kod</th>
                            <th id="Th2" class="col2" runat="server">Nazwa</th>
                            <th id="Th3" class="control" runat="server"></th>
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
    <EditItemTemplate>
        <tr class="eit" style="">
            <td class="col1">
                <asp:TextBox ID="KodTextBox" class="textbox" runat="server" MaxLength="10" Text='<%# Bind("Kod") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="KodTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="col2">
                <asp:TextBox ID="NazwaTextBox" class="textbox" runat="server" MaxLength="200" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="NazwaTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="control">
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vgEdit" />
            </td>
        </tr>
    </EditItemTemplate>
    <InsertItemTemplate>
        <tr class="iit" style="">
            <td class="col1">
                <asp:TextBox ID="KodTextBox" class="textbox" runat="server" Text='<%# Bind("Kod") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert" ControlToValidate="KodTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="col2">
                <asp:TextBox ID="NazwaTextBox" class="textbox" runat="server" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert" ControlToValidate="NazwaTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Insert" Text="Dodaj" ValidationGroup="vgInsert" />
            </td>
        </tr>
    </InsertItemTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT * from [Slowniki] WHERE Typ='WYJASNIC' ORDER BY [Kod]" 
    UpdateCommand="UPDATE Slowniki SET [Typ] = 'WYJASNIC', [Kod] = @Kod, [Nazwa] = @Nazwa WHERE [Id] = @Id"
    DeleteCommand="DELETE FROM [Slowniki] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Slowniki] ([Typ], [Kod], [Nazwa], [Opis]) VALUES ('WYJASNIC', @Kod, @Nazwa, NULL)" >
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Typ" Type="String" />
        <asp:Parameter Name="Kod" Type="String" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="Typ" Type="String" />
        <asp:Parameter Name="Kod" Type="String" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>
