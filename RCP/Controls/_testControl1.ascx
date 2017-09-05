<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="_testControl1.ascx.cs" Inherits="HRRcp.Controls._testControl1" %>

<asp:HiddenField ID="hidSesId" runat="server"></asp:HiddenField>

<asp:ListView ID="ListView1" runat="server" 
    DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" InsertItemPosition="LastItem">
    <ItemTemplate>
        <tr style="">
            <td>
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
            </td>
            <td>
                <asp:Label ID="sesIdLabel" runat="server" Text='<%# Eval("sesId") %>' />
            </td>
            <td>
                <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
            </td>
            <td>
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
            </td>
            <td>
                <asp:CheckBox ID="AktywnaCheckBox" runat="server" 
                    Checked='<%# Eval("Aktywna") %>' Enabled="false" />
            </td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr style="">
            <td>
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
            </td>
            <td>
                <asp:Label ID="sesIdLabel" runat="server" Text='<%# Eval("sesId") %>' />
            </td>
            <td>
                <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
            </td>
            <td>
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
            </td>
            <td>
                <asp:CheckBox ID="AktywnaCheckBox" runat="server" 
                    Checked='<%# Eval("Aktywna") %>' Enabled="false" />
            </td>
        </tr>
    </AlternatingItemTemplate>
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
                <asp:TextBox ID="sesIdTextBox" runat="server" Text='<%# Bind("sesId") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdTextBox" runat="server" Text='<%# Bind("Id") %>' />
            </td>
            <td>
                <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
            </td>
            <td>
                <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
            </td>
            <td>
                <asp:CheckBox ID="AktywnaCheckBox" runat="server" 
                    Checked='<%# Bind("Aktywna") %>' />
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
                                sesId</th>
                            <th runat="server">
                                Id</th>
                            <th runat="server">
                                Nazwa</th>
                            <th runat="server">
                                Typ</th>
                            <th runat="server">
                                Aktywna</th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server">
                <td runat="server" style="">
                    <asp:DataPager ID="DataPager1" runat="server">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" 
                                ShowLastPageButton="True" />
                        </Fields>
                    </asp:DataPager>
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
                <asp:TextBox ID="sesIdTextBox" runat="server" Text='<%# Bind("sesId") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdTextBox" runat="server" Text='<%# Bind("Id") %>' />
            </td>
            <td>
                <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
            </td>
            <td>
                <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
            </td>
            <td>
                <asp:CheckBox ID="AktywnaCheckBox" runat="server" 
                    Checked='<%# Bind("Aktywna") %>' />
            </td>
        </tr>
    </EditItemTemplate>
    <SelectedItemTemplate>
        <tr style="">
            <td>
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                    Text="Delete" />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
            </td>
            <td>
                <asp:Label ID="sesIdLabel" runat="server" Text='<%# Eval("sesId") %>' />
            </td>
            <td>
                <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
            </td>
            <td>
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
            </td>
            <td>
                <asp:CheckBox ID="AktywnaCheckBox" runat="server" 
                    Checked='<%# Eval("Aktywna") %>' Enabled="false" />
            </td>
        </tr>
    </SelectedItemTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [tmpStrefy] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [tmpStrefy] ([Nazwa]) VALUES (@Nazwa)" 
    SelectCommand="SELECT * FROM [tmpStrefy] WHERE [sesId] = @sesId ORDER BY [Nazwa]" 
    UpdateCommand="UPDATE [tmpStrefy] SET [Nazwa] = @Nazwa WHERE [Id] = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidSesId" Name="sesId" PropertyName="Value" Type="String" />    
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>
