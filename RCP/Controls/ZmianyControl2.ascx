<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ZmianyControl2.ascx.cs" 
Inherits="HRRcp.Controls.ZmianyControl2" %>
<%@ Register src="ZmianaGodziny2.ascx" tagname="ZmianaGodziny" tagprefix="uc1" %>

<asp:ListView ID="lvZmiany" runat="server" DataSourceID="SqlDataSource1" DataKeyNames="Id" 
    onselectedindexchanged="lvZmiany_SelectedIndexChanged" 
    onitemdatabound="lvZmiany_ItemDataBound" 
    onitemcommand="lvZmiany_ItemCommand" onitemcreated="lvZmiany_ItemCreated" 
    oniteminserting="lvZmiany_ItemInserting" 
    onitemupdating="lvZmiany_ItemUpdating" 
    onitemediting="lvZmiany_ItemEditing" onprerender="lvZmiany_PreRender" >
    <ItemTemplate>
        <div class="zmiana it round5">
            <table class="zmiana">
                <tr class="firstline">
                    <td class="col1">
                        <asp:Label ID="SymbolLabel" runat="server" CssClass="symbol round2" ToolTip="Symbol zmiany" Text='<%# Eval("Symbol") %>' BackColor='<%# GetColorNull(Eval("Kolor").ToString()) %>' />
                    </td>
                    <td colspan="3">
                        <asp:Label ID="NazwaLabel" runat="server" CssClass="nazwa" Text='<%# Eval("Nazwa") %>' /><br />
                        <asp:CheckBox ID="VisibleCheckBox" runat="server" CssClass="checkbox" Checked='<%# Eval("Visible") %>' Enabled="false" Text="Widoczna" />
                    </td>
                </tr>
                <tr class="title">
                    <td class="col1"></td>
                    <td class="col2"><span class="t1">Godziny</span></td>
                    <td class="col3"><span class="t1">Stawka</span></td>
                    <td class="col4"></td>
                </tr>
                <tr class="godziny">
                    <td class="col1"></td>
                    <td class="col2">
                        <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od", "{0:t}") %>' /> -
                        <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do", "{0:t}") %>' />
                    </td>
                    <td class="col3">
                        <asp:Label ID="StawkaLabel" runat="server" Text='<%# Eval("Stawka") %>' />%
                    </td>
                    <td class="col4"></td>
                </tr>
                <tr class="nadgodziny">
                    <td class="col1"></td>
                    <td colspan="2">
                        <uc1:ZmianaGodziny ID="cntNadgodziny" Overtimes='<%# Eval("Nadgodziny") %>' Editable="false" runat="server" />
                    </td>
                    <td class="col4"></td>
                </tr>
            </table>
            <div class="buttons">
                <asp:Button ID="DupButton" runat="server" Visible="false" CommandName="Duplicate" Text="Duplikuj" />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
            </div>
        </div>
    </ItemTemplate>
    <EmptyDataTemplate>
        <span>Brak danych</span>
    </EmptyDataTemplate>
    <InsertItemTemplate>
        <div class="fullline">
            <div class="zmiana eit iit round5" >
                <table class="zmiana">
                    <tr class="firstline">
                        <td class="col1">
                            <asp:Label ID="SymbolLabel" runat="server" CssClass="symbol round2" ToolTip="Symbol zmiany" Text='<%# Eval("Symbol") %>' BackColor='<%# GetColorNull(Eval("Kolor").ToString()) %>' />
                        </td>
                        <td colspan="3">
                            <asp:HiddenField ID="hidColor" runat="server" Value='<%# Bind("Kolor") %>'/>
                            <span class="label">Symbol:</span><asp:TextBox ID="TextBox1" CssClass="symbol" runat="server" Text='<%# Bind("Symbol") %>' />
                            <asp:ImageButton ID="KolorImageButton" CssClass="palette" runat="Server" 
                                ImageUrl="~/images/buttons/palette.png" 
                                ToolTip="Kliknij aby zmienić kolor zmiany"/>
                            <br />
                            <span class="label">Nazwa:</span><asp:TextBox ID="TextBox2" CssClass="nazwa" runat="server" Text='<%# Bind("Nazwa") %>' /><br />
                            <span class="label"></span><asp:CheckBox ID="VisibleCheckBox" CssClass="checkbox" runat="server" Checked='<%# Bind("Visible") %>' Text="Widoczna" />
                        </td>
                    </tr>
                    <tr class="title">
                        <td class="col1"></td>
                        <td class="col2"><span class="t1">Godziny</span></td>
                        <td class="col3"><span class="t1">Stawka</span></td>
                        <td class="col4"></td>
                    </tr>
                    <tr class="godziny">
                        <td class="col1"></td>
                        <td class="col2">
                            <asp:DropDownList ID="ddlCzasOd" runat="server" ></asp:DropDownList> -
                            <asp:DropDownList ID="ddlCzasDo" runat="server" ></asp:DropDownList>                        
                        </td>
                        <td class="col3">
                            <asp:DropDownList ID="ddlStawka" runat="server" ></asp:DropDownList>                        
                        </td>
                        <td class="col4"></td>
                    </tr>
                    <uc1:ZmianaGodziny ID="cntNadgodziny" Overtimes='<%# Eval("Nadgodziny") %>' Editable="true" runat="server" />
                </table>
                <div class="buttons">
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Insert" Text="Zapisz" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Anuluj" />
                </div>
            </div>
        </div>
    </InsertItemTemplate>
    <EditItemTemplate>
        <div class="zmiana eit round5" >
            <table class="zmiana">
                <tr class="firstline">
                    <td class="col1">
                        <asp:Label ID="SymbolLabel" runat="server" CssClass="symbol round2" ToolTip="Symbol zmiany" Text='<%# Eval("Symbol") %>' BackColor='<%# GetColorNull(Eval("Kolor").ToString()) %>' />
                    </td>
                    <td colspan="3">
                        <asp:HiddenField ID="hidColor" runat="server" Value='<%# Bind("Kolor") %>'/>
                        <span class="label">Symbol:</span><asp:TextBox ID="TextBox1" CssClass="symbol" runat="server" Text='<%# Bind("Symbol") %>' />
                        <asp:ImageButton ID="KolorImageButton" CssClass="palette color" runat="Server" 
                            ImageUrl="~/images/buttons/palette.png" 
                            ToolTip="Kliknij aby zmienić kolor zmiany" />
                        <br />
                        <span class="label">Nazwa:</span><asp:TextBox ID="TextBox2" CssClass="nazwa" runat="server" Text='<%# Bind("Nazwa") %>' /><br />
                        <span class="label"></span><asp:CheckBox ID="VisibleCheckBox" CssClass="checkbox" runat="server" Checked='<%# Bind("Visible") %>' Text="Widoczna" />
                    </td>
                </tr>
                <tr class="title">
                    <td class="col1"></td>
                    <td class="col2"><span class="t1">Godziny</span></td>
                    <td class="col3"><span class="t1">Stawka</span></td>
                    <td class="col4"></td>
                </tr>
                <tr class="godziny">
                    <td class="col1"></td>
                    <td class="col2">
                        <asp:DropDownList ID="ddlCzasOd" runat="server" ></asp:DropDownList> -
                        <asp:DropDownList ID="ddlCzasDo" runat="server" ></asp:DropDownList>                        
                    </td>
                    <td class="col3">
                        <asp:DropDownList ID="ddlStawka" runat="server" ></asp:DropDownList>                        
                    </td>
                    <td class="col4"></td>
                </tr>
                <uc1:ZmianaGodziny ID="cntNadgodziny" Overtimes='<%# Eval("Nadgodziny") %>' Editable="true" runat="server" />
            </table>
            <div class="buttons">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
            </div>
        </div>
    </EditItemTemplate>
    <LayoutTemplate>
        <div class="zmiany_buttons">
            <asp:Button ID="InsertButton" runat="server" class="button100" Text="Nowa zmiana" CommandName="NewRecord" />
        </div>
        <div ID="itemPlaceholderContainer" runat="server" style="" class="zmiany">
            <div ID="itemPlaceholder" runat="server" />
        </div>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT *, 
                       LEFT(convert(varchar, Od, 8),5) as CzasOd,
                       LEFT(convert(varchar, Do, 8),5) as CzasDo                   
                   FROM [Zmiany] ORDER BY [Symbol], [Visible] desc" 
    DeleteCommand="DELETE FROM [Zmiany] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Zmiany] ([Symbol], [Nazwa], [Od], [Do], [Stawka], [Visible], [Kolor], [Nadgodziny]) VALUES (@Symbol, @Nazwa, @Od, @Do, @Stawka, @Visible, @Kolor, @Nadgodziny)" 
    UpdateCommand="UPDATE [Zmiany] SET [Symbol] = @Symbol, [Nazwa] = @Nazwa, [Od] = @Od, [Do] = @Do, [Stawka] = @Stawka, [Visible] = @Visible, [Kolor] = @Kolor, [Nadgodziny] = @Nadgodziny WHERE [Id] = @Id" >
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Symbol" Type="String" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="Visible" Type="Boolean" />
        <asp:Parameter Name="Kolor" Type="String" />
        <asp:Parameter Name="Nadgodziny" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Symbol" Type="String" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="Visible" Type="Boolean" />
        <asp:Parameter Name="Kolor" Type="String" />
        <asp:Parameter Name="Nadgodziny" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>
