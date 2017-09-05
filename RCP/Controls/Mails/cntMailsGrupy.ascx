<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntMailsGrupy.ascx.cs" Inherits="HRRcp.Controls.Mails.cntMailsGrupy" %>

<asp:ListView ID="lvMailsGrupy" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" InsertItemPosition="LastItem">
    <ItemTemplate>
        <tr style="">
            <td>
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
            </td>
            <td>
                <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
            </td>
            <td>
                <asp:Label ID="GrupaLabel" runat="server" Text='<%# Eval("Grupa") %>' />
            </td>
            <td>
                <asp:Label ID="OpisLabel" runat="server" Text='<%# Eval("Opis") %>' />
            </td>
            <td>
                <asp:Label ID="ZnacznikiSqlLabel" runat="server" Text='<%# Eval("ZnacznikiSql") %>' />
            </td>
            <td>
                <asp:Label ID="ZnacznikiSqlTestLabel" runat="server" Text='<%# Eval("ZnacznikiSqlTest") %>' />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" style="">
            <tr>
                <td>
                    No data was returned.</td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <InsertItemTemplate>
        <tr style="">
            <td>
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" 
                    Text="Insert" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                    Text="Clear" />
            </td>
            <td>
                &nbsp;</td>
            <td>
                <asp:TextBox ID="GrupaTextBox" runat="server" Text='<%# Bind("Grupa") %>' />
            </td>
            <td>
                <asp:TextBox ID="OpisTextBox" runat="server" Text='<%# Bind("Opis") %>' />
            </td>
            <td>
                <asp:TextBox ID="ZnacznikiSqlTextBox" runat="server" Text='<%# Bind("ZnacznikiSql") %>' />
            </td>
            <td>
                <asp:TextBox ID="ZnacznikiSqlTestTextBox" runat="server" Text='<%# Bind("ZnacznikiSqlTest") %>' />
            </td>
        </tr>
    </InsertItemTemplate>
    <LayoutTemplate>
        <table runat="server">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">
                            </th>
                            <th runat="server">
                                Id</th>
                            <th runat="server">
                                Grupa</th>
                            <th runat="server">
                                Opis</th>
                            <th runat="server">
                                ZnacznikiSql</th>
                            <th runat="server">
                                ZnacznikiSqlTest</th>
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
    <EditItemTemplate>
        <tr style="">
            <td>
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
            <td>
                <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
            </td>
            <td>
                <asp:TextBox ID="GrupaTextBox" runat="server" Text='<%# Bind("Grupa") %>' />
            </td>
            <td>
                <asp:TextBox ID="OpisTextBox" runat="server" Text='<%# Bind("Opis") %>' />
            </td>
            <td>
                <asp:TextBox ID="ZnacznikiSqlTextBox" runat="server" Text='<%# Bind("ZnacznikiSql") %>' />
            </td>
            <td>
                <asp:TextBox ID="ZnacznikiSqlTestTextBox" runat="server" Text='<%# Bind("ZnacznikiSqlTest") %>' />
            </td>
        </tr>
    </EditItemTemplate>
    <SelectedItemTemplate>
        <tr style="">
            <td>
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
            </td>
            <td>
                <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
            </td>
            <td>
                <asp:Label ID="GrupaLabel" runat="server" Text='<%# Eval("Grupa") %>' />
            </td>
            <td>
                <asp:Label ID="OpisLabel" runat="server" Text='<%# Eval("Opis") %>' />
            </td>
            <td>
                <asp:Label ID="ZnacznikiSqlLabel" runat="server" Text='<%# Eval("ZnacznikiSql") %>' />
            </td>
            <td>
                <asp:Label ID="ZnacznikiSqlTestLabel" runat="server" Text='<%# Eval("ZnacznikiSqlTest") %>' />
            </td>
        </tr>
    </SelectedItemTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString%>" 
    DeleteCommand="DELETE FROM [MailingGrupy] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [MailingGrupy] ([Grupa], [Opis], [ZnacznikiSql], [ZnacznikiSqlTest]) VALUES (@Grupa, @Opis, @ZnacznikiSql, @ZnacznikiSqlTest)" 
    SelectCommand="SELECT * FROM [MailingGrupy] ORDER BY [Grupa]" 
    UpdateCommand="UPDATE [MailingGrupy] SET [Grupa] = @Grupa, [Opis] = @Opis, [ZnacznikiSql] = @ZnacznikiSql, [ZnacznikiSqlTest] = @ZnacznikiSqlTest WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Grupa" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="ZnacznikiSql" Type="String" />
        <asp:Parameter Name="ZnacznikiSqlTest" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Grupa" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="ZnacznikiSql" Type="String" />
        <asp:Parameter Name="ZnacznikiSqlTest" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>

