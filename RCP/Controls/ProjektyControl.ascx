<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjektyControl.ascx.cs" Inherits="HRRcp.Controls.ProjektyControl" %>

<asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" InsertItemPosition="LastItem">
    <ItemTemplate>
        <tr style="">
            <td>
                <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
            </td>
            <td>
                <asp:Label ID="NR_EWLabel" runat="server" Text='<%# Eval("NR_EW") %>' />
            </td>
            <td>
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="PROJ_DATELabel" runat="server" Text='<%# Eval("PROJ_DATE") %>' />
            </td>
            <td>
                <asp:Label ID="PROJ_ACTLabel" runat="server" Text='<%# Eval("PROJ_ACT") %>' />
            </td>
            <td>
                <asp:Label ID="IdStrefyLabel" runat="server" Text='<%# Eval("IdStrefy") %>' />
            </td>
            <td class="control">
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
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
    <InsertItemTemplate>
        <tr style="">
            <td>
                <asp:TextBox ID="IdTextBox" runat="server" Text='<%# Bind("Id") %>' />
            </td>
            <td>
                <asp:TextBox ID="NR_EWTextBox" runat="server" Text='<%# Bind("NR_EW") %>' />
            </td>
            <td>
                <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
            </td>
            <td>
                <asp:TextBox ID="PROJ_DATETextBox" runat="server" Text='<%# Bind("PROJ_DATE") %>' />
            </td>
            <td>
                <asp:TextBox ID="PROJ_ACTTextBox" runat="server" Text='<%# Bind("PROJ_ACT") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdStrefyTextBox" runat="server" Text='<%# Bind("IdStrefy") %>' />
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
        </tr>
    </InsertItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1 hoverline">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" class="tbProjekty" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">
                                Id</th>
                            <th runat="server">
                                NR_EW</th>
                            <th runat="server">
                                Nazwa</th>
                            <th runat="server">
                                PROJ_DATE</th>
                            <th runat="server">
                                PROJ_ACT</th>
                            <th runat="server">
                                IdStrefy</th>
                            <th runat="server"></th>
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
                <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
            </td>
            <td>
                <asp:TextBox ID="NR_EWTextBox" runat="server" Text='<%# Bind("NR_EW") %>' />
            </td>
            <td>
                <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
            </td>
            <td>
                <asp:TextBox ID="PROJ_DATETextBox" runat="server" Text='<%# Bind("PROJ_DATE") %>' />
            </td>
            <td>
                <asp:TextBox ID="PROJ_ACTTextBox" runat="server" Text='<%# Bind("PROJ_ACT") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdStrefyTextBox" runat="server" Text='<%# Bind("IdStrefy") %>' />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
        </tr>
    </EditItemTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [Projekty] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Projekty] ([Id], [NR_EW], [Nazwa], [PROJ_DATE], [PROJ_ACT], [IdStrefy]) VALUES (@Id, @NR_EW, @Nazwa, @PROJ_DATE, @PROJ_ACT, @IdStrefy)" 
    SelectCommand="SELECT * FROM [Projekty] ORDER BY [Nazwa]" 
    UpdateCommand="UPDATE [Projekty] SET [NR_EW] = @NR_EW, [Nazwa] = @Nazwa, [PROJ_DATE] = @PROJ_DATE, [PROJ_ACT] = @PROJ_ACT, [IdStrefy] = @IdStrefy WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="NR_EW" Type="String" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="PROJ_DATE" Type="DateTime" />
        <asp:Parameter Name="PROJ_ACT" Type="Int32" />
        <asp:Parameter Name="IdStrefy" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="NR_EW" Type="String" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="PROJ_DATE" Type="DateTime" />
        <asp:Parameter Name="PROJ_ACT" Type="Int32" />
        <asp:Parameter Name="IdStrefy" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>


