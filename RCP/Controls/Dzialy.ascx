<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Dzialy.ascx.cs" Inherits="HRRcp.Controls.Dzialy" %>


<asp:ListView ID="lvDzialy" runat="server" DataKeyNames="Id" DataSourceID="SqlDataSource1" 
    InsertItemPosition="None" 
    onselectedindexchanged="lvDzialy_SelectedIndexChanged" 
    onitemcommand="lvDzialy_ItemCommand">
    <ItemTemplate>
        <tr class="it">
            <td class="col1">
                <asp:LinkButton ID="NazwaLinkButton" runat="server" Text='<%# Eval("Nazwa") %>' CommandName="Select" CommandArgument='<%# Eval("Id") %>' ></asp:LinkButton>
            </td>
            <td class="col2">
                <asp:Label ID="StatusLabel" runat="server" Text='<%# GetStatus(Eval("Status")) %>' />
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
                <%--
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
                --%>
            </td>
        </tr>
    </ItemTemplate>
    <SelectedItemTemplate>
        <tr class="sit">
            <td class="col1">
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="StatusLabel" runat="server" Text='<%# GetStatus(Eval("Status")) %>' />
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
                <%--
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
                --%>
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
    <InsertItemTemplate>
        <tr class="iit">
            <td class="col1">
                <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
            </td>
            <td class="col2"></td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Dodaj" /><br />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Wyczyść" />
            </td>
        </tr>
    </InsertItemTemplate>
    <LayoutTemplate>
        <table id="Table2" runat="server" class="ListView1">
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table ID="itemPlaceholderContainer" class="tbDzialy" runat="server" border="0" style="">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" runat="server">Nazwa</th>
                            <th id="Th2" runat="server">Status</th>
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
        <tr class="eit">
            <td class="col1">
                <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="StatusLabel" runat="server" Text='<%# GetStatus(Eval("Status")) %>' />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" /><br />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
            </td>
        </tr>
    </EditItemTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [Dzialy] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Dzialy] ([Nazwa], [Status]) VALUES (@Nazwa, @Status)" 
    SelectCommand="SELECT [Nazwa], [Status], [Id] FROM [Dzialy] ORDER BY [Nazwa]" 
    
    UpdateCommand="UPDATE [Dzialy] SET [Nazwa] = @Nazwa, [Status] = @Status WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Status" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>
