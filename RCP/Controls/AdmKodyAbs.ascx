<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdmKodyAbs.ascx.cs" Inherits="HRRcp.Controls.AdmKodyAbs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
                
<%--
<asp:DropDownList ID="ddlTyp2" runat="server" SelectedItem='<%# Bind("Typ2") %>'>
    <asp:ListItem Text="wybierz ..." Value=""></asp:ListItem>
    <asp:ListItem Text="Urlop" Value="U"></asp:ListItem>
    <asp:ListItem Text="Zasiłek" Value="Z"></asp:ListItem>
</asp:DropDownList>
--%>                

<asp:Menu ID="tabFilter" runat="server" Orientation="Horizontal" 
    onmenuitemclick="tabFilter_MenuItemClick" >
    <StaticMenuStyle CssClass="tabsStrip" />
    <StaticMenuItemStyle CssClass="tabItem" />
    <StaticSelectedStyle CssClass="tabSelected" />
    <StaticHoverStyle CssClass="tabHover" />
    <Items>
        <asp:MenuItem Text="Asseco HR" Value="0" Selected="True"></asp:MenuItem>
        <asp:MenuItem Text="System KP" Value="1"></asp:MenuItem>
        <asp:MenuItem Text="Wszystkie" Value="2"></asp:MenuItem>                    
    </Items>
    <StaticItemTemplate>
        <div class="tabCaption">
            <div class="tabLeft">
                <div class="tabRight">
                    <asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Text") %>' />
                </div>
            </div>
        </div>
    </StaticItemTemplate>
</asp:Menu>

<asp:ListView ID="lvAbsencjaKody" runat="server" DataSourceID="SqlDataSource2" DataKeyNames="Kod"
    InsertItemPosition="LastItem"
    onitemdatabound="lvAbsencjaKody_ItemDataBound" 
    onlayoutcreated="lvAbsencjaKody_LayoutCreated" 
    onsorting="lvAbsencjaKody_Sorting" 
    onitemcreated="lvAbsencjaKody_ItemCreated" 
    onitemdeleting="lvAbsencjaKody_ItemDeleting" 
    oniteminserting="lvAbsencjaKody_ItemInserting" 
    onitemupdating="lvAbsencjaKody_ItemUpdating">
    <ItemTemplate>
        <tr class="it">
            <td class="num">
                <asp:Label ID="KodLabel" runat="server" Text='<%# Eval("Kod") %>' />
            </td>
            <td>
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="SymbolLabel" runat="server" Text='<%# Eval("Symbol") %>' />
            </td>
            <td class="num">
                <asp:Label ID="GodzinPracyLabel" runat="server" Text='<%# Eval("GodzinPracy") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="DniWolneCheckBox" runat="server" Checked='<%# Eval("DniWolne") %>' Enabled="false" />
            </td>
            <td class="check">
                <asp:CheckBox ID="WidocznyCheckBox" runat="server" Checked='<%# Eval("Widoczny") %>' Enabled="false" />
            </td>
            <td>
                <asp:Label ID="StatusLabel" runat="server" Text='<%# GetStatus(Eval("Status")) %>' />
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Typ2Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" Text='<%# Eval("Kod2") %>' />
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edycja" />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table id="Table7" runat="server" style="">
            <tr>
                <td>
                    Brak danych
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table id="Table8" runat="server" class="ListView1 hoverline narrow">
            <tr id="Tr10" runat="server">
                <td id="Td7" runat="server">
                    <table ID="itemPlaceholderContainer" class="tbAbsencjaKody" runat="server" border="0" style="">
                        <tr id="Tr11" runat="server" style="">
                            <th><asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Kod" Text="Kod" ToolTip="Kod - Sortuj" /></th>
                            <th><asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="Nazwa" Text="Nazwa" ToolTip="Nazwa - Sortuj" /></th>
                            <th><asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Symbol" Text="Symbol" ToolTip="Symbol - Sortuj" /></th>
                            <th><asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="GodzinPracy" Text="Godzin pracy" ToolTip="Godzin pracy - Sortuj" /></th>
                            <th><asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="DniWolne" Text="Pokaż w dni wolne" ToolTip="Pokaż w dni wolne - Sortuj" /></th>
                            <th><asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="Widoczny" Text="Widoczny" ToolTip="Widoczny - Sortuj" /></th>
                            <th><asp:LinkButton ID="LinkButton7" runat="server" CommandName="Sort" CommandArgument="Status" Text="Status" ToolTip="Status - Sortuj" /></th>
                            <th><asp:LinkButton ID="LinkButton8" runat="server" CommandName="Sort" CommandArgument="Typ2Nazwa" Text="Typ" ToolTip="Typ - Sortuj" /></th>
                            <th><asp:LinkButton ID="LinkButton9" runat="server" CommandName="Sort" CommandArgument="Kod2" Text="Kod2" ToolTip="Kod2 - Sortuj" /></th>
                            <th class="control1"></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="Tr12" runat="server">
                <td id="Td8" runat="server" style="">
                </td>
            </tr>
        </table>
    </LayoutTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="num">
                <asp:TextBox ID="KodTextBox" runat="server" Text='<%# Bind("Kod") %>' MaxLength="12" Visible="false" />
                <asp:Label ID="KodLabel" runat="server" Text='<%# Eval("Kod") %>' />
            </td>
            <td>
                <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' MaxLength="100" Visible="false" />
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:TextBox ID="SymbolTextBox" runat="server" Text='<%# Bind("Symbol") %>' MaxLength="10" />
            </td>
            <td class="num">
                <asp:TextBox ID="GodzinPracyTextBox" runat="server" Text='<%# Bind("GodzinPracy") %>' MaxLength="2" Width="50px" />
                <asp:FilteredTextBoxExtender ID="feNumber" runat="server" Enabled="true" TargetControlID="GodzinPracyTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789" />                
            </td>
            <td class="check">
                <asp:CheckBox ID="DniWolneCheckBox1" runat="server" Checked='<%# Bind("DniWolne") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="WidocznyCheckBox" runat="server" Checked='<%# Bind("Widoczny") %>' />
            </td>
            <td>
                <asp:TextBox ID="StatusTextBox" runat="server" Text='<%# Bind("Status") %>' MaxLength="3" Visible="false"/>
                <asp:DropDownList ID="ddlStatus" runat="server" >
                </asp:DropDownList>
                <asp:Label ID="StatusLabel" runat="server" Text='<%# GetStatus(Eval("Status")) %>' Visible="false"/>
            </td>
            <td>
                <asp:DropDownList ID="ddlTyp2" runat="server" >
                    <asp:ListItem Text="wybierz ..." Value=""></asp:ListItem>
                    <asp:ListItem Text="Urlop" Value="U"></asp:ListItem>
                    <asp:ListItem Text="Zasiłek" Value="Z"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="num">
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Kod2") %>' MaxLength="20" Width="50px" />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                <br />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
            </td>
        </tr>
    </EditItemTemplate>
    <InsertItemTemplate>
        <tr class="iit">
            <td>
                <asp:TextBox ID="KodTextBox" runat="server" Text='<%# Bind("Kod") %>' />
            </td>
            <td>
                <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
            </td>
            <td>
                <asp:TextBox ID="SymbolTextBox" runat="server" Text='<%# Bind("Symbol") %>' />
            </td>
            <td>
                <asp:TextBox ID="GodzinPracyTextBox" runat="server" Text='<%# Bind("GodzinPracy") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="DniWolneCheckBox" runat="server" Checked='<%# Bind("DniWolne") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="WidocznyCheckBox" runat="server" Checked='<%# Bind("Widoczny") %>' />
            </td>
            <td>
                <asp:DropDownList ID="ddlStatus" runat="server" >
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlTyp2" runat="server" >
                    <asp:ListItem Text="wybierz ..." Value=""></asp:ListItem>
                    <asp:ListItem Text="Urlop" Value="U"></asp:ListItem>
                    <asp:ListItem Text="Zasiłek" Value="Z"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="num">
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Kod2") %>' MaxLength="20" Width="50px" />
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Dodaj" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Czyść" />
            </td>
        </tr>
    </InsertItemTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT *, case Typ2 when 'U' then 'Urlop' when 'Z' then 'Zasiłek' else null end as Typ2Nazwa FROM [AbsencjaKody] ORDER BY Nazwa" 
    UpdateCommand="UPDATE [AbsencjaKody] SET [Nazwa] = @Nazwa, [Symbol] = @Symbol, [GodzinPracy] = @GodzinPracy, [Status] = @Status, [Widoczny] = @Widoczny, [DniWolne] = @DniWolne, Typ2 = @Typ2, Kod2 = @Kod2 WHERE [Kod] = @Kod"
    DeleteCommand="DELETE FROM [AbsencjaKody] WHERE [Kod] = @Kod" 
    InsertCommand="INSERT INTO [AbsencjaKody] ([Kod], [Nazwa], [Symbol], [GodzinPracy], [Status], [Widoczny], [DniWolne], Typ2, Kod2) VALUES (@Kod, @Nazwa, @Symbol, @GodzinPracy, @Status, @Widoczny, @DniWolne, @Typ2, @Kod2)" >
    <UpdateParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Symbol" Type="String" />
        <asp:Parameter Name="GodzinPracy" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Kod" Type="Int32" />
        <asp:Parameter Name="Widoczny" Type="Boolean" />
        <asp:Parameter Name="DniWolne" Type="Boolean" />
        <asp:Parameter Name="Typ2" Type="String" />
        <asp:Parameter Name="Kod2" Type="String" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Kod" Type="Int32" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="Kod" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Symbol" Type="String" />
        <asp:Parameter Name="GodzinPracy" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Widoczny" Type="Boolean" />
        <asp:Parameter Name="DniWolne" Type="Boolean" />
        <asp:Parameter Name="Typ2" Type="String" />
        <asp:Parameter Name="Kod2" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>
