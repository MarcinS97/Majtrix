<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSciezkiPlikiEdit.ascx.cs" Inherits="HRRcp.Portal.Controls.Adm.cntSciezkiPlikiEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<div runat="server" class="cntSciezkiPlikiEdit" >
    <asp:ListView ID="ListView1" runat="server" DataKeyNames="ID" DataSourceID="SqlDataSource1" EnableModelValidation="True" InsertItemPosition="none">
        <EditItemTemplate>
            <tr class="edt">
                <td class="num">
                    <asp:TextBox ID="IDTextBox" runat="server" Text='<%# Bind("ID") %>' />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                        ValidationGroup="vge"                     
                        ControlToValidate="IDTextBox" 
                        ErrorMessage="Błąd">
                    </asp:RequiredFieldValidator>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true" 
                    TargetControlID="IDTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789" />
                </td>
                <td class="typ">
                    <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
                </td>
                <td class="path">
                    <asp:TextBox ID="SciezkaTextBox" runat="server" Text='<%# Bind("Sciezka") %>' />
                
                    
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vge"                     
                    ControlToValidate="SciezkaTextBox" 
                    ErrorMessage="Błąd" />
                </td>
                <td class="sql">
                    <asp:TextBox ID="SqlTextBox" runat="server" TextMode="MultiLine" Rows="3" Text='<%# Bind("Sql") %>' />
                </td>
                <td class="sql">
                    <asp:TextBox ID="SqlDateListTextBox" runat="server" TextMode="MultiLine" Rows="3" Text='<%# Bind("SqlDateList") %>' />
                </td>
                <td class="sql">
                    <asp:TextBox ID="SqlFindNrEwTextBox" runat="server" TextMode="MultiLine" Rows="3" Text='<%# Bind("SqlFindNrEw") %>' />
                </td>
                <td class="control">
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
                </td>
            </tr>
        </EditItemTemplate>
        <EmptyDataTemplate>
            <table runat="server" class="edt">
                <tr>
                    <td>Brak danych</td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <tr class="iit">
                <td class="num">
                    <asp:TextBox ID="IDTextBox" runat="server" Text='<%# Bind("ID") %>' />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi"                     
                    ControlToValidate="IDTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true" 
                    TargetControlID="IDTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789" />
                </td>
                <td class="typ">
                    <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
                </td>
                <td class="path">
                    <asp:TextBox ID="SciezkaTextBox" runat="server" Text='<%# Bind("Sciezka") %>' />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi"                     
                    ControlToValidate="SciezkaTextBox" 
                    ErrorMessage="Błąd" />
                </td>
                <td class="sql">
                    <asp:TextBox ID="SqlTextBox" runat="server" TextMode="MultiLine" Rows="3" Text='<%# Bind("Sql") %>' />
                </td>
                <td class="sql">
                    <asp:TextBox ID="SqlDateListTextBox" runat="server" TextMode="MultiLine" Rows="3" Text='<%# Bind("SqlDateList") %>' />
                </td>
                <td class="sql">
                    <asp:TextBox ID="SqlFindNrEwTextBox" runat="server" TextMode="MultiLine" Rows="3" Text='<%# Bind("SqlFindNrEw") %>' />
                </td>
                <td class="control">
                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Clear" />
                </td>
            </tr>
        </InsertItemTemplate>
        <ItemTemplate>
            <tr class="it">
                <td class="num">
                    <asp:Label ID="IDLabel" runat="server" Text='<%# Eval("ID") %>' />
                </td>
                <td class="typ">
                    <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
                </td>
                <td class="path">
                    <asp:Label ID="SciezkaLabel" runat="server" Text='<%# Eval("Sciezka") %>' />
                </td>
                <td class="sql">
                    <asp:Label ID="SqlLabel" runat="server" Text='<%# Eval("Sql") %>' />
                </td>
                <td class="sql">
                    <asp:Label ID="SqlDateListLabel" runat="server" Text='<%# Eval("SqlDateList") %>' />
                </td>
                <td class="sql">
                    <asp:Label ID="SqlFindNrEwLabel" runat="server" Text='<%# Eval("SqlFindNrEw") %>' />
                </td>
                <td class="control">
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
            </tr>
        </ItemTemplate>
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr runat="server" style="">
                                <th runat="server">ID</th>
                                <th runat="server">Typ</th>
                                <th runat="server">Sciezka</th>
                                <th runat="server">Sql</th>
                                <th runat="server">SqlDateList</th>
                                <th runat="server">SqlFindNrEw</th>
                                <th runat="server"></th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj nowy" />
        </LayoutTemplate>       
    </asp:ListView>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
        DeleteCommand="DELETE FROM [PlikiSciezki] WHERE [ID] = @ID" 
        InsertCommand="INSERT INTO [PlikiSciezki] ([ID], [Typ], [Sciezka], [Sql], [SqlDateList], [SqlFindNrEw]) VALUES (@ID, @Typ, @Sciezka, @Sql, @SqlDateList, @SqlFindNrEw)" 
        SelectCommand="SELECT * FROM [PlikiSciezki]" 
        UpdateCommand="UPDATE [PlikiSciezki] SET [Typ] = @Typ, [Sciezka] = @Sciezka, [Sql] = @Sql, [SqlDateList] = @SqlDateList, [SqlFindNrEw] = @SqlFindNrEw WHERE [ID] = @ID"
        >
        <DeleteParameters>
            <asp:Parameter Name="ID" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="ID" Type="Int32" />
            <asp:Parameter Name="Typ" Type="String" />
            <asp:Parameter Name="Sciezka" Type="String" />
            <asp:Parameter Name="Sql" Type="String" />
            <asp:Parameter Name="SqlDateList" Type="String" />
            <asp:Parameter Name="SqlFindNrEw" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Typ" Type="String" />
            <asp:Parameter Name="Sciezka" Type="String" />
            <asp:Parameter Name="Sql" Type="String" />
            <asp:Parameter Name="SqlDateList" Type="String" />
            <asp:Parameter Name="SqlFindNrEw" Type="String" />
            <asp:Parameter Name="ID" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
</div>