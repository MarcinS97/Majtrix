<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ZmianyControl.ascx.cs" Inherits="HRRcp.Controls.ZmianyControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="ZmianaGodziny.ascx" tagname="ZmianaGodziny" tagprefix="uc1" %>

<asp:ListView ID="lvZmiany" runat="server" DataSourceID="SqlDataSource1" 
    onselectedindexchanged="lvZmiany_SelectedIndexChanged" DataKeyNames="Id" onitemdatabound="lvZmiany_ItemDataBound" 
    onitemediting="lvZmiany_ItemEditing" oniteminserting="lvZmiany_ItemInserting" 
    onitemupdating="lvZmiany_ItemUpdating" 
    onitemcommand="lvZmiany_ItemCommand" onitemcreated="lvZmiany_ItemCreated">
    <ItemTemplate>
        <tr style="" class="it">
            <td>
                <asp:Label ID="SymbolLabel" runat="server" Text='<%# Eval("Symbol") %>' />
            </td>
            <td>
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od", "{0:t}") %>' />
            </td>
            <td>
                <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do", "{0:t}") %>' />
            </td>
            <td>
                <asp:Label ID="StawkaLabel" runat="server" Text='<%# StawkaText(Eval("Stawka").ToString()) %>' />
            </td>
            <td>
                <%--
                <asp:Label ID="KolorLabel" runat="server" Text='<%# Eval("Kolor") %>' />
                --%>
                <asp:Panel ID="KolorSample" CssClass="colorpicker" BackColor='<%# GetColorNull(Eval("Kolor").ToString()) %>' runat="server" />
            </td>
            <td>
                <asp:CheckBox ID="VisibleCheckBox" runat="server" Checked='<%# Eval("Visible") %>' Enabled="false" />
            </td>
            <%--
            <td>
                <asp:Label ID="NadgodzinyLabel" runat="server" Text='<%# Eval("Nadgodziny") %>' />
            </td>
            --%>
            <td class="control" rowspan="2">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
            </td>
        </tr>
        <tr class="it">
            <td colspan="7" class="overtimes">
                <uc1:ZmianaGodziny ID="cntNadgodziny" Overtimes='<%# Eval("Nadgodziny") %>' Editable="false" runat="server" />
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
        <tr class="iit">
            <td>
                <asp:TextBox ID="SymbolTextBox" MaxLength="10" runat="server" Text='<%# Bind("Symbol") %>' />
            </td>
            <td>
                <asp:TextBox ID="NazwaTextBox" MaxLength="200" runat="server" Text='<%# Bind("Nazwa") %>' />
            </td>
            <td>
                <asp:DropDownList ID="ddlCzasOd" runat="server" ></asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlCzasDo" runat="server" ></asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="StawkaDropDownList" runat="server" ></asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="KolorTextBox" CssClass="colorpicker"
                    Enabled="false"
                    AutoCompleteType="None" 
                    MaxLength="6" runat="server" Text='<%# GetColorNoHash(Eval("Kolor").ToString()) %>'/>
                <asp:Panel ID="KolorSample" CssClass="colorpicker" runat="server" ToolTip="Kliknij aby zmienić kolor" />
                <asp:ColorPickerExtender ID="ColorPicker" runat="server" 
                    TargetControlID="KolorTextBox" 
                    PopupButtonID="KolorSample" 
                    SampleControlID="KolorSample"/>
            </td>
            <td>
                <asp:CheckBox ID="VisibleCheckBox" runat="server" Checked='<%# Bind("Visible") %>' />
            </td>
            <td class="control" rowspan="2">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Insert" Text="Zapisz" />
                <asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Anuluj" />
            </td>
        </tr>
        <tr class="it">
            <td colspan="7" class="overtimes">
                <uc1:ZmianaGodziny ID="cntNadgodziny" Overtimes='<%# Eval("Nadgodziny") %>' Editable="true" runat="server" />
            </td>
        </tr>
    </InsertItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" class="tbZmiany" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">Symbol</th>
                            <th runat="server">Nazwa</th>
                            <th runat="server">Od</th>
                            <th runat="server">Do</th>
                            <th runat="server">Stawka</th>
                            <th runat="server">Kolor</th>
                            <th runat="server">Widoczna</th>
                            <th runat="server" class="control">
                                <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj" ToolTip="Dodaj nowy rekord"/>                            
                            </th>
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
        <tr style="" class="eit">
            <td>
                <asp:TextBox ID="SymbolTextBox" MaxLength="10" runat="server" Text='<%# Bind("Symbol") %>' />
            </td>
            <td>
                <asp:TextBox ID="NazwaTextBox" MaxLength="200" runat="server" Text='<%# Bind("Nazwa") %>' />
            </td>
            <td>
                <asp:DropDownList ID="ddlCzasOd" runat="server" ></asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlCzasDo" runat="server" ></asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="StawkaDropDownList" runat="server" ></asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="KolorTextBox" CssClass="colorpicker"
                    Enabled="true"
                    AutoCompleteType="None" 
                    MaxLength="6" 
                    runat="server" 
                    Text='<%# GetColorNoHash(Eval("Kolor").ToString()) %>'/>
                
                <asp:Panel ID="KolorSample" CssClass="colorpicker" runat="server" ToolTip="Kliknij aby zmienić kolor" />
                
                <asp:ColorPickerExtender ID="ColorPicker" runat="server" 
                    
                    OnClientColorSelectionChanged="changeColorCPE" 
                    
                    TargetControlID="KolorTextBox" 
                    PopupButtonID="KolorSample" 
                    SampleControlID="KolorSample" 
                    />
            </td>
            <%--
            
            SelectedColor='<%# Eval("Kolor") %>'
                    
            
            
            <td>
                <asp:TextBox ID="IkonaTextBox" runat="server" Text='<%# Bind("Ikona") %>' />
            </td>
            <td>
                <asp:TextBox ID="NadgodzinyTextBox" runat="server" Text='<%# Bind("Nadgodziny") %>' />
            </td>
            --%>
            <td>
                <asp:CheckBox ID="VisibleCheckBox" runat="server" Checked='<%# Bind("Visible") %>' />
            </td>
            <td class="control" rowspan="2">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
            </td>
        </tr>
        <tr class="it">
            <td colspan="7" class="overtimes">
                <uc1:ZmianaGodziny ID="cntNadgodziny" Overtimes='<%# Eval("Nadgodziny") %>' Editable="true" runat="server" />
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
                <asp:Label ID="SymbolLabel" runat="server" Text='<%# Eval("Symbol") %>' />
            </td>
            <td>
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od") %>' />
            </td>
            <td>
                <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do") %>' />
            </td>
            <td>
                <asp:CheckBox ID="VisibleCheckBox" runat="server" Checked='<%# Eval("Visible") %>' Enabled="false" />
            </td>
            <td>
                <asp:Label ID="IkonaLabel" runat="server" Text='<%# Eval("Ikona") %>' />
            </td>
            <td>
                <asp:Label ID="KolorLabel" runat="server" Text='<%# Eval("Kolor") %>' />
            </td>
            <td>
                <asp:Label ID="NadgodzinyLabel" runat="server" Text='<%# Eval("Nadgodziny") %>' />
            </td>
        </tr>
    </SelectedItemTemplate>
</asp:ListView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT *, 
                        LEFT(convert(varchar, Od, 8),5) as CzasOd,
                        LEFT(convert(varchar, Do, 8),5) as CzasDo
                   FROM [Zmiany] ORDER BY [Visible], [Symbol]" 
    DeleteCommand="DELETE FROM [Zmiany] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Zmiany] ([Symbol], [Nazwa], [Od], [Do], [Stawka], [Visible], [Ikona], [Kolor], [Nadgodziny]) VALUES (@Symbol, @Nazwa, @Od, @Do, @Stawka, @Visible, @Ikona, @Kolor, @Nadgodziny)" 
    UpdateCommand="UPDATE [Zmiany] SET [Symbol] = @Symbol, [Nazwa] = @Nazwa, [Od] = @Od, [Do] = @Do, [Stawka] = @Stawka, [Visible] = @Visible, [Ikona] = @Ikona, [Kolor] = @Kolor, [Nadgodziny] = @Nadgodziny WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Symbol" Type="String" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="Stawka" Type="Int32" />
        <asp:Parameter Name="Visible" Type="Boolean" />
        <asp:Parameter Name="Ikona" Type="String" />
        <asp:Parameter Name="Kolor" Type="String" />
        <asp:Parameter Name="Nadgodziny" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Symbol" Type="String" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="Stawka" Type="Int32" />
        <asp:Parameter Name="Visible" Type="Boolean" />
        <asp:Parameter Name="Ikona" Type="String" />
        <asp:Parameter Name="Kolor" Type="String" />
        <asp:Parameter Name="Nadgodziny" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>



<%--
<table>
    <tr>
        <td>
            <asp:Button ID="btAdd" runat="server" class="button75" Text="Dodaj" />
            <asp:Button ID="btEdit" runat="server" class="button75" Text="Edycja" />
            <asp:Button ID="btDelete" runat="server" class="button75" Text="Usuń" />
            <asp:Button ID="btSave" runat="server" class="button75" Text="Zapisz" />
            <asp:Button ID="btCancel" runat="server" class="button75" Text="Anuluj" />
        </td>
        <td>
        </td>
    </tr>        
</table>
--%>