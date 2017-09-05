<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntDicMarginesy.ascx.cs" Inherits="HRRcp.Controls.Adm.cntDicMarginesy" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:ListView ID="lvMarginesy" runat="server" DataSourceID="SqlDataSource1" 
    DataKeyNames="Id"
    InsertItemPosition="LastItem" 
    onitemdatabound="lvMarginesy_ItemDataBound" >
    <ItemTemplate>
        <tr class="it">
            <td class="num">
                <asp:Label ID="Lp" runat="server" Text='<%# Eval("Lp") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="Nazwa" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td class="num">
                <asp:Label ID="Kod" runat="server" Text='<%# Eval("Kod") %>' />
            </td>
            <td class="col4">
                <asp:Label ID="Parametr" runat="server" Text='<%# Eval("Parametr") %>' />
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
        <table class="tbMarginesy hoverline" runat="server">
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" class="col1" runat="server">Lp</th>
                            <th id="Th2" class="col2" runat="server">Margines</th>
                            <th id="Th4" class="col1" runat="server">Wartość [minuty]</th>
                            <th id="Th5" class="col1" runat="server">Parametr</th>
                            <th id="Th3" class="control" runat="server"></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="num">
                <asp:TextBox ID="LpTextBox" class="textbox" runat="server" MaxLength="10" Text='<%# Bind("Lp") %>' />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="true" 
                    TargetControlID="LpTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789" />
            </td>
            <td class="col2">
                <asp:TextBox ID="NazwaTextBox" class="textbox" runat="server" MaxLength="200" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="NazwaTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="num">
                <asp:TextBox ID="KodTextBox" class="textbox" runat="server" MaxLength="10" Text='<%# Bind("Kod") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="KodTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true" 
                    TargetControlID="KodTextBox" 
                    FilterType="Custom" 
                    ValidChars="-0123456789" />
            </td>
            <td class="col1">
                <asp:TextBox ID="ParametrTextBox" class="textbox" runat="server" MaxLength="10" Text='<%# Bind("Parametr") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="ParametrTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="control">
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vgEdit" />
            </td>
        </tr>
    </EditItemTemplate>
    <InsertItemTemplate>
        <tr class="iit">
            <td class="num">
                <asp:TextBox ID="LpTextBox" class="textbox" runat="server" Text='<%# Bind("Lp") %>' />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="true" 
                    TargetControlID="LpTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789" />
            </td>
            <td class="col2">
                <asp:TextBox ID="NazwaTextBox" class="textbox" runat="server" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert" ControlToValidate="NazwaTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="num">
                <asp:TextBox ID="KodTextBox" class="textbox" runat="server" Text='<%# Bind("Kod") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert" ControlToValidate="KodTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true" 
                    TargetControlID="KodTextBox" 
                    FilterType="Custom" 
                    ValidChars="-0123456789" />
            </td>
            <td class="col1">
                <asp:TextBox ID="ParametrTextBox" class="textbox" runat="server" Text='<%# Bind("Parametr") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert" ControlToValidate="ParametrTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Insert" Text="Dodaj" ValidationGroup="vgInsert" />
            </td>
        </tr>
    </InsertItemTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT * from [Kody] WHERE Typ='MARGINES' ORDER BY [Lp]" 
    UpdateCommand="UPDATE Kody SET [Typ] = 'MARGINES', [Lp] = @Lp, [Nazwa] = @Nazwa, [Kod] = @Kod, [Parametr] = @Parametr WHERE [Id] = @Id"
    DeleteCommand="DELETE FROM [Kody] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Kody] ([Typ], [Lp], [Nazwa], [Kod], [Parametr]) VALUES ('MARGINES', @Lp, @Nazwa, @Kod, @Parametr)" >
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Typ" Type="String" />
        <asp:Parameter Name="Lp" Type="String" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Kod" Type="String" />
        <asp:Parameter Name="Parametr" Type="String" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="Typ" Type="String" />
        <asp:Parameter Name="Lp" Type="String" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Kod" Type="String" />
        <asp:Parameter Name="Parametr" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>
