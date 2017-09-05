<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntDicKody.ascx.cs" Inherits="HRRcp.Controls.Adm.cntDicKody" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<div class="tbKodyFilter">
    <asp:DropDownList ID="ddlFilter" runat="server" DataSourceID="SqlDataSource2" DataTextField="Nazwa" DataValueField="Typ" AutoPostBack="True">
    </asp:DropDownList>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
        SelectCommand="
select null as Typ, 'Pokaż wszystkie kody' as Nazwa, 1 as Sort
union all 
select distinct Typ, Typ as Nazwa, 2 as Sort from Kody 
order by Sort, Typ
        ">
    </asp:SqlDataSource>
</div>
<br />
<asp:ListView ID="lvKody" runat="server" DataSourceID="SqlDataSource1" 
    DataKeyNames="Id"
    InsertItemPosition="LastItem" 
    onitemdatabound="lvKody_ItemDataBound" >
    <ItemTemplate>
        <tr class="it">
            <td class="col2">
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Typ") %>' />
            </td>
            <td class="num">
                <asp:Label ID="Kod" runat="server" Text='<%# Eval("Kod") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="Nazwa" runat="server" Text='<%# Eval("Nazwa") %>' /><br />
                <asp:Label ID="Label2" runat="server" CssClass="line2" Text='<%# Eval("Nazwa2") %>' />
            </td>
            <td class="col4">
                <asp:Label ID="Parametr" runat="server" Text='<%# Eval("Parametr") %>' />
            </td>
            <td>
                <asp:Label ID="Par1Label" runat="server" Text='<%# Eval("Par1") %>' />
            </td>
            <td>
                <asp:Label ID="Par2Label" runat="server" Text='<%# Eval("Par2") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" />
            </td>
            <td class="num">
                <asp:Label ID="Lp" runat="server" Text='<%# Eval("Lp") %>' />
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
                            <th id="Th1" class="col1" runat="server">Typ</th>
                            <th id="Th4" class="col1" runat="server">Kod</th>
                            <th id="Th2" class="col2" runat="server">Nazwa / Nazwa2</th>
                            <th id="Th5" class="col1" runat="server">Parametr</th>
                            <th id="Th7" runat="server">Par1</th>
                            <th id="Th8" runat="server">Par2</th>
                            <th id="Th9" class="check" runat="server">Aktywny</th>                            
                            <th id="Th6" class="col1" runat="server">Kolejność</th>
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
            <td class="col2">
                <asp:TextBox ID="TypTextBox" class="textbox" runat="server" MaxLength="10" Text='<%# Bind("Typ") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="TypTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="num">
                <asp:TextBox ID="KodTextBox" class="textbox" runat="server" MaxLength="10" Text='<%# Bind("Kod") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="KodTextBox" SetFocusOnError="True" Display="Dynamic" Enabled="false"></asp:RequiredFieldValidator>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true" 
                    TargetControlID="KodTextBox" 
                    FilterType="Custom" 
                    ValidChars="-0123456789" />
            </td>
            <td class="col2">
                <asp:TextBox ID="NazwaTextBox" class="textbox" runat="server" MaxLength="200" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="NazwaTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                <br />
                <asp:TextBox ID="Nazwa2TextBox" class="textbox" runat="server" MaxLength="200" Text='<%# Bind("Nazwa2") %>' />
            </td>
            <td class="col1">
                <asp:TextBox ID="ParametrTextBox" class="textbox" runat="server" MaxLength="500" Text='<%# Bind("Parametr") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Błąd" ValidationGroup="vgEdit" ControlToValidate="ParametrTextBox" SetFocusOnError="True" Display="Dynamic" Enabled="false"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:TextBox ID="Par1TextBox" runat="server" Text='<%# Bind("Par1") %>' />
            </td>
            <td>
                <asp:TextBox ID="Par2TextBox" runat="server" Text='<%# Bind("Par2") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' />
            </td>
            <td class="num">
                <asp:TextBox ID="TextBox1" class="textbox" runat="server" MaxLength="10" Text='<%# Bind("Lp") %>' />
            </td>
            <td class="control">
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vgEdit" />
            </td>
        </tr>
    </EditItemTemplate>
    <InsertItemTemplate>
        <tr class="iit">
            <td class="col2">
                <asp:TextBox ID="TypTextBox" class="textbox" runat="server" Text='<%# Bind("Typ") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert" ControlToValidate="TypTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td class="num">
                <asp:TextBox ID="KodTextBox" class="textbox" runat="server" Text='<%# Bind("Kod") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert" ControlToValidate="KodTextBox" SetFocusOnError="True" Display="Dynamic" Enabled="false"></asp:RequiredFieldValidator>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true" 
                    TargetControlID="KodTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789" />
            </td>
            <td class="col2">
                <asp:TextBox ID="NazwaTextBox" class="textbox" runat="server" MaxLength="200" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert" ControlToValidate="NazwaTextBox" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                <br />
                <asp:TextBox ID="Nazwa2TextBox" class="textbox" runat="server" MaxLength="200" Text='<%# Bind("Nazwa2") %>' />
            </td>
            <td class="col1">
                <asp:TextBox ID="ParametrTextBox" class="textbox" runat="server" MaxLength="500" Text='<%# Bind("Parametr") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Błąd" ValidationGroup="vgInsert" ControlToValidate="ParametrTextBox" SetFocusOnError="True" Display="Dynamic" Enabled="false"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:TextBox ID="Par1TextBox" runat="server" Text='<%# Bind("Par1") %>' />
            </td>
            <td>
                <asp:TextBox ID="Par2TextBox" runat="server" Text='<%# Bind("Par2") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' />
            </td>
            <td class="num">
                <asp:TextBox ID="LpTextBox" class="textbox" runat="server" Text='<%# Bind("Lp") %>' />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="true" 
                    TargetControlID="LpTextBox" 
                    FilterType="Custom" 
                    ValidChars="-0123456789" />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Insert" Text="Dodaj" ValidationGroup="vgInsert" />
            </td>
        </tr>
    </InsertItemTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT * from [Kody] ORDER BY Typ, Lp" 
    UpdateCommand="UPDATE [Kody] SET [Typ] = @Typ, [Lp] = @Lp, [Nazwa] = @Nazwa, [Kod] = @Kod, [Parametr] = @Parametr, [Nazwa2] = @Nazwa2, [Par1] = @Par1, [Par2] = @Par2, [Aktywny] = @Aktywny WHERE [Id] = @Id"
    DeleteCommand="DELETE FROM [Kody] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Kody] ([Typ], [Lp], [Nazwa], [Kod], [Parametr], [Nazwa2], [Par1], [Par2], [Aktywny]) VALUES (@Typ, @Lp, @Nazwa, @Kod, @Parametr, @Nazwa2, @Par1, @Par2, @Aktywny)" 
    FilterExpression="'{0}' is null or Typ='{0}'" >
    <FilterParameters>
        <asp:ControlParameter ControlID="ddlFilter" Name="typ" PropertyName="SelectedValue" />
    </FilterParameters>
    <UpdateParameters>
        <asp:Parameter Name="Typ" Type="String" />
        <asp:Parameter Name="Lp" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Kod" Type="Int32" />
        <asp:Parameter Name="Parametr" Type="String" />
        <asp:Parameter Name="Nazwa2" Type="String" />
        <asp:Parameter Name="Par1" Type="Int32" />
        <asp:Parameter Name="Par2" Type="Int32" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="Typ" Type="String" />
        <asp:Parameter Name="Lp" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Kod" Type="Int32" />
        <asp:Parameter Name="Parametr" Type="String" />
        <asp:Parameter Name="Nazwa2" Type="String" />
        <asp:Parameter Name="Par1" Type="Int32" />
        <asp:Parameter Name="Par2" Type="Int32" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>

