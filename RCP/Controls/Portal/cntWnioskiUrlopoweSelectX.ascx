<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWnioskiUrlopoweSelectX.ascx.cs" Inherits="HRRcp.Controls.Portal.cntWnioskiUrlopoweSelectX" %>
<asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" GroupItemCount="3" 
    InsertItemPosition="LastItem">
    <EmptyItemTemplate>
        <td runat="server" />
        </EmptyItemTemplate>
        <ItemTemplate>
            <td runat="server" style="">
                Id:
                <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                <br />
                Typ:
                <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
                <br />
                TypNapis:
                <asp:Label ID="TypNapisLabel" runat="server" Text='<%# Eval("TypNapis") %>' />
                <br />
                Symbol:
                <asp:Label ID="SymbolLabel" runat="server" Text='<%# Eval("Symbol") %>' />
                <br />
                IdKodyAbs:
                <asp:Label ID="IdKodyAbsLabel" runat="server" Text='<%# Eval("IdKodyAbs") %>' />
                <br />
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" 
                    Checked='<%# Eval("Aktywny") %>' Enabled="false" Text="Aktywny" />
                <br />
                Kolejnosc:
                <asp:Label ID="KolejnoscLabel" runat="server" Text='<%# Eval("Kolejnosc") %>' />
                <br />
                Image:
                <asp:Label ID="ImageLabel" runat="server" Text='<%# Eval("Image") %>' />
                <br />
                <asp:CheckBox ID="WypelniaPracownikCheckBox" runat="server" 
                    Checked='<%# Eval("WypelniaPracownik") %>' Enabled="false" 
                    Text="WypelniaPracownik" />
                <br />
                <asp:CheckBox ID="WypelniaKierownikCheckBox" runat="server" 
                    Checked='<%# Eval("WypelniaKierownik") %>' Enabled="false" 
                    Text="WypelniaKierownik" />
                <br />
                Info:
                <asp:Label ID="InfoLabel" runat="server" Text='<%# Eval("Info") %>' />
                <br />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                    Text="Delete" />
                <br />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                <br />
            </td>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <td runat="server" style="">
                Id:
                <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                <br />
                Typ:
                <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
                <br />
                TypNapis:
                <asp:Label ID="TypNapisLabel" runat="server" Text='<%# Eval("TypNapis") %>' />
                <br />
                Symbol:
                <asp:Label ID="SymbolLabel" runat="server" Text='<%# Eval("Symbol") %>' />
                <br />
                IdKodyAbs:
                <asp:Label ID="IdKodyAbsLabel" runat="server" Text='<%# Eval("IdKodyAbs") %>' />
                <br />
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" 
                    Checked='<%# Eval("Aktywny") %>' Enabled="false" Text="Aktywny" />
                <br />
                Kolejnosc:
                <asp:Label ID="KolejnoscLabel" runat="server" Text='<%# Eval("Kolejnosc") %>' />
                <br />
                Image:
                <asp:Label ID="ImageLabel" runat="server" Text='<%# Eval("Image") %>' />
                <br />
                <asp:CheckBox ID="WypelniaPracownikCheckBox" runat="server" 
                    Checked='<%# Eval("WypelniaPracownik") %>' Enabled="false" 
                    Text="WypelniaPracownik" />
                <br />
                <asp:CheckBox ID="WypelniaKierownikCheckBox" runat="server" 
                    Checked='<%# Eval("WypelniaKierownik") %>' Enabled="false" 
                    Text="WypelniaKierownik" />
                <br />
                Info:
                <asp:Label ID="InfoLabel" runat="server" Text='<%# Eval("Info") %>' />
                <br />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                    Text="Delete" />
                <br />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                <br />
            </td>
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
            <td runat="server" style="">
                Id:
                <asp:TextBox ID="IdTextBox" runat="server" Text='<%# Bind("Id") %>' />
                <br />
                Typ:
                <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
                <br />
                TypNapis:
                <asp:TextBox ID="TypNapisTextBox" runat="server" 
                    Text='<%# Bind("TypNapis") %>' />
                <br />
                Symbol:
                <asp:TextBox ID="SymbolTextBox" runat="server" Text='<%# Bind("Symbol") %>' />
                <br />
                IdKodyAbs:
                <asp:TextBox ID="IdKodyAbsTextBox" runat="server" 
                    Text='<%# Bind("IdKodyAbs") %>' />
                <br />
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" 
                    Checked='<%# Bind("Aktywny") %>' Text="Aktywny" />
                <br />
                Kolejnosc:
                <asp:TextBox ID="KolejnoscTextBox" runat="server" 
                    Text='<%# Bind("Kolejnosc") %>' />
                <br />
                Image:
                <asp:TextBox ID="ImageTextBox" runat="server" Text='<%# Bind("Image") %>' />
                <br />
                <asp:CheckBox ID="WypelniaPracownikCheckBox" runat="server" 
                    Checked='<%# Bind("WypelniaPracownik") %>' Text="WypelniaPracownik" />
                <br />
                <asp:CheckBox ID="WypelniaKierownikCheckBox" runat="server" 
                    Checked='<%# Bind("WypelniaKierownik") %>' Text="WypelniaKierownik" />
                <br />
                Info:
                <asp:TextBox ID="InfoTextBox" runat="server" Text='<%# Bind("Info") %>' />
                <br />
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" 
                    Text="Insert" />
                <br />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                    Text="Clear" />
                <br />
            </td>
        </InsertItemTemplate>
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table ID="groupPlaceholderContainer" runat="server" border="0" style="">
                            <tr ID="groupPlaceholder" runat="server">
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
            <td runat="server" style="">
                Id:
                <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
                <br />
                Typ:
                <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
                <br />
                TypNapis:
                <asp:TextBox ID="TypNapisTextBox" runat="server" 
                    Text='<%# Bind("TypNapis") %>' />
                <br />
                Symbol:
                <asp:TextBox ID="SymbolTextBox" runat="server" Text='<%# Bind("Symbol") %>' />
                <br />
                IdKodyAbs:
                <asp:TextBox ID="IdKodyAbsTextBox" runat="server" 
                    Text='<%# Bind("IdKodyAbs") %>' />
                <br />
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" 
                    Checked='<%# Bind("Aktywny") %>' Text="Aktywny" />
                <br />
                Kolejnosc:
                <asp:TextBox ID="KolejnoscTextBox" runat="server" 
                    Text='<%# Bind("Kolejnosc") %>' />
                <br />
                Image:
                <asp:TextBox ID="ImageTextBox" runat="server" Text='<%# Bind("Image") %>' />
                <br />
                <asp:CheckBox ID="WypelniaPracownikCheckBox" runat="server" 
                    Checked='<%# Bind("WypelniaPracownik") %>' Text="WypelniaPracownik" />
                <br />
                <asp:CheckBox ID="WypelniaKierownikCheckBox" runat="server" 
                    Checked='<%# Bind("WypelniaKierownik") %>' Text="WypelniaKierownik" />
                <br />
                Info:
                <asp:TextBox ID="InfoTextBox" runat="server" Text='<%# Bind("Info") %>' />
                <br />
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" 
                    Text="Update" />
                <br />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                    Text="Cancel" />
                <br />
            </td>
        </EditItemTemplate>
        <GroupTemplate>
            <tr ID="itemPlaceholderContainer" runat="server">
                <td ID="itemPlaceholder" runat="server">
                </td>
            </tr>
        </GroupTemplate>
        <SelectedItemTemplate>
            <td runat="server" style="">
                Id:
                <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                <br />
                Typ:
                <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
                <br />
                TypNapis:
                <asp:Label ID="TypNapisLabel" runat="server" Text='<%# Eval("TypNapis") %>' />
                <br />
                Symbol:
                <asp:Label ID="SymbolLabel" runat="server" Text='<%# Eval("Symbol") %>' />
                <br />
                IdKodyAbs:
                <asp:Label ID="IdKodyAbsLabel" runat="server" Text='<%# Eval("IdKodyAbs") %>' />
                <br />
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" 
                    Checked='<%# Eval("Aktywny") %>' Enabled="false" Text="Aktywny" />
                <br />
                Kolejnosc:
                <asp:Label ID="KolejnoscLabel" runat="server" Text='<%# Eval("Kolejnosc") %>' />
                <br />
                Image:
                <asp:Label ID="ImageLabel" runat="server" Text='<%# Eval("Image") %>' />
                <br />
                <asp:CheckBox ID="WypelniaPracownikCheckBox" runat="server" 
                    Checked='<%# Eval("WypelniaPracownik") %>' Enabled="false" 
                    Text="WypelniaPracownik" />
                <br />
                <asp:CheckBox ID="WypelniaKierownikCheckBox" runat="server" 
                    Checked='<%# Eval("WypelniaKierownik") %>' Enabled="false" 
                    Text="WypelniaKierownik" />
                <br />
                Info:
                <asp:Label ID="InfoLabel" runat="server" Text='<%# Eval("Info") %>' />
                <br />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                    Text="Delete" />
                <br />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                <br />
            </td>
        </SelectedItemTemplate>
</asp:ListView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
        DeleteCommand="DELETE FROM [poWnioskiUrlopoweTypy] WHERE [Id] = @Id" 
        InsertCommand="INSERT INTO [poWnioskiUrlopoweTypy] ([Id], [Typ], [TypNapis], [Symbol], [IdKodyAbs], [Aktywny], [Kolejnosc], [Image], [WypelniaPracownik], [WypelniaKierownik], [Info]) VALUES (@Id, @Typ, @TypNapis, @Symbol, @IdKodyAbs, @Aktywny, @Kolejnosc, @Image, @WypelniaPracownik, @WypelniaKierownik, @Info)" 
        SelectCommand="SELECT * FROM [poWnioskiUrlopoweTypy]" 
        UpdateCommand="UPDATE [poWnioskiUrlopoweTypy] SET [Typ] = @Typ, [TypNapis] = @TypNapis, [Symbol] = @Symbol, [IdKodyAbs] = @IdKodyAbs, [Aktywny] = @Aktywny, [Kolejnosc] = @Kolejnosc, [Image] = @Image, [WypelniaPracownik] = @WypelniaPracownik, [WypelniaKierownik] = @WypelniaKierownik, [Info] = @Info WHERE [Id] = @Id">
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Typ" Type="String" />
            <asp:Parameter Name="TypNapis" Type="String" />
            <asp:Parameter Name="Symbol" Type="String" />
            <asp:Parameter Name="IdKodyAbs" Type="Int32" />
            <asp:Parameter Name="Aktywny" Type="Boolean" />
            <asp:Parameter Name="Kolejnosc" Type="Int32" />
            <asp:Parameter Name="Image" Type="String" />
            <asp:Parameter Name="WypelniaPracownik" Type="Boolean" />
            <asp:Parameter Name="WypelniaKierownik" Type="Boolean" />
            <asp:Parameter Name="Info" Type="String" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="Id" Type="Int32" />
            <asp:Parameter Name="Typ" Type="String" />
            <asp:Parameter Name="TypNapis" Type="String" />
            <asp:Parameter Name="Symbol" Type="String" />
            <asp:Parameter Name="IdKodyAbs" Type="Int32" />
            <asp:Parameter Name="Aktywny" Type="Boolean" />
            <asp:Parameter Name="Kolejnosc" Type="Int32" />
            <asp:Parameter Name="Image" Type="String" />
            <asp:Parameter Name="WypelniaPracownik" Type="Boolean" />
            <asp:Parameter Name="WypelniaKierownik" Type="Boolean" />
            <asp:Parameter Name="Info" Type="String" />
        </InsertParameters>
    </asp:SqlDataSource>

