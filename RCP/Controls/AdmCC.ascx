<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdmCC.ascx.cs" Inherits="HRRcp.Controls.AdmCC" %>

<%@ Register src="DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<div class="tbCCfilter">
    &nbsp;&nbsp;<asp:Label ID="lbFilter" runat="server" Text="Pokaż na dzień: " />&nbsp;&nbsp;&nbsp;
    <uc1:DateEdit ID="deNaDzien" runat="server" />&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btFilter" runat="server" CssClass="button75" Text="Filtruj" onclick="btFilter_Click" />
</div>

<asp:ListView ID="lvDic" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" InsertItemPosition="LastItem" 
    onitemdatabound="lvDic_ItemDataBound" oniteminserting="lvDic_ItemInserting" 
    onitemupdating="lvDic_ItemUpdating">
    <ItemTemplate>
        <tr id="trLine" runat="server" class="it">
            <td>
                <asp:Label ID="ccLabel" runat="server" Text='<%# Eval("cc") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="GrupaCheckBox" runat="server" Checked='<%# Eval("Grupa") %>' Enabled="false" />
            </td>
            <td class="check">
                <asp:CheckBox ID="WyborCheckBox" runat="server" Checked='<%# Eval("Wybor") %>' Enabled="false" />
            </td>
            <td>
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td class="date">
                <asp:Label ID="lbAktywneOd" runat="server" Text='<%# Eval("AktywneOd", "{0:d}") %>' />
            </td>
            <td class="date">
                <asp:Label ID="lbAktywneDo" runat="server" Text='<%# Eval("AktywneDo", "{0:d}") %>' />
            </td>
            <td>
                <asp:Label ID="GrupaLabel" runat="server" Text='<%# Eval("GrupaNazwa") %>' />
            </td>
            <td class="num">
                <asp:Label ID="UdzialLabel" runat="server" Text='<%# Eval("Udzial") %>' />
            </td>
            <td class="num">
                <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
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
            <td class="col1">
                <asp:TextBox ID="ccTextBox" runat="server" MaxLength="20" Text='<%# Bind("cc") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    Enabled="false"
                    ValidationGroup="vgi" 
                    ControlToValidate="ccTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td class="check">
                <asp:CheckBox ID="GrupaCheckBox" runat="server" Checked='<%# Bind("Grupa") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="WyborCheckBox" runat="server" Checked='<%# Bind("Wybor") %>' />
            </td>
            <td class="col4">
                <asp:TextBox ID="NazwaTextBox" runat="server" MaxLength="200" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi" 
                    ControlToValidate="NazwaTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td class="data">
                <uc1:DateEdit ID="deAktywneOd" runat="server" Date='<%# Bind("AktywneOd") %>'  />
            </td>
            <td class="data">
                <uc1:DateEdit ID="deAktywneDo" runat="server" Date='<%# Bind("AktywneDo") %>'  />
            </td>
            
            <td class="col7">
                <asp:DropDownList ID="ddlGrupa" runat="server"
                    DataSourceID="SqlDataSource2"
                    DataTextField="Nazwa"
                    DataValueField="Id">
                </asp:DropDownList>
            </td>
            <td class="num">
                <asp:TextBox ID="UdzialTextBox" runat="server" MaxLength="3" Text='<%# Bind("Udzial", "{0.0}") %>' />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true" 
                    TargetControlID="StatusTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789.," />
            </td>
            <td class="num">
                <asp:TextBox ID="StatusTextBox" runat="server" MaxLength="3" Text='<%# Bind("Status") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi" 
                    ControlToValidate="StatusTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" 
                    TargetControlID="StatusTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789-" />
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" ValidationGroup="vgi" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="col1">
                <asp:TextBox ID="ccTextBox" runat="server" MaxLength="20" Text='<%# Bind("cc") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    Enabled="false"
                    ValidationGroup="vgi" 
                    ControlToValidate="ccTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td class="check">
                <asp:CheckBox ID="GrupaCheckBox" runat="server" Checked='<%# Bind("Grupa") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="WyborCheckBox" runat="server" Checked='<%# Bind("Wybor") %>' />
            </td>
            <td class="col4">
                <asp:TextBox ID="NazwaTextBox" runat="server" MaxLength="200" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi" 
                    ControlToValidate="NazwaTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td class="data">
                <uc1:DateEdit ID="deAktywneOd" runat="server" Date='<%# Bind("AktywneOd") %>'  />
            </td>
            <td class="data">
                <uc1:DateEdit ID="deAktywneDo" runat="server" Date='<%# Bind("AktywneDo") %>'  />
            </td>
            
            <td class="col7">
                <asp:DropDownList ID="ddlGrupa" runat="server"
                    DataSourceID="SqlDataSource2"
                    DataTextField="Nazwa"
                    DataValueField="Id">
                </asp:DropDownList>
            </td>
            <td class="num">
                <asp:TextBox ID="UdzialTextBox" runat="server" MaxLength="3" Text='<%# Bind("Udzial", "{0:0.0}") %>' />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true" 
                    TargetControlID="StatusTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789.," />
            </td>
            <td class="num">
                <asp:TextBox ID="StatusTextBox" runat="server" MaxLength="3" Text='<%# Bind("Status") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi" 
                    ControlToValidate="StatusTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" 
                    TargetControlID="StatusTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789-" />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" ValidationGroup="vge"/>
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="tbCC narrow">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th id="Th1" runat="server">
                                Kod CC</th>
                            <th id="Th6" runat="server">
                                Grupa</th>
                            <th id="Th3" runat="server">
                                Wybór</th>
                            <th runat="server">
                                Nazwa</th>
                            <th runat="server">
                                Aktywne od</th>
                            <th id="Th2" runat="server">
                                Aktywne do</th>
                            <th id="Th4" runat="server">
                                Nazwa grupy</th>
                            <th id="Th5" runat="server">
                                Udział %<br /><span class="t4n">(0.0-1.0)</span></th>
                            <th runat="server">
                                Status</th>
                            <th runat="server" class="control">
                                <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj" Visible="false" />
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
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [CC] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [CC] ([cc], [Grupa], [Nazwa], [AktywneOd], [AktywneDo], [IdGrupy], [Udzial], [Wybor], [Status]) VALUES (@cc, @Grupa, @Nazwa, @AktywneOd, @AktywneDo, @IdGrupy, @Udzial, @Wybor, @Status)" 
    SelectCommand="
        select C.*, G.Id as GrupaId, G.Nazwa as GrupaNazwa, 
        case when C.AktywneDo is null then 1 
        else case when (select max(ISNULL(AktywneDo, '2999-01-01')) from CC where CC.cc = C.cc) = ISNULL(C.AktywneDo,'2999-01-01') then 1
        else 0 end end as Aktualne,
        ISNULL(C.AktywneDo, '2999-01-01') as SortData  
        from CC C
        left outer join CC G on G.Id = C.IdGrupy 
        order by cc, SortData desc" 
    UpdateCommand="UPDATE [CC] SET [cc] = @cc, [Grupa] = @Grupa, [Nazwa] = @Nazwa, [AktywneOd] = @AktywneOd, [AktywneDo] = @AktywneDo, [IdGrupy] = @IdGrupy, [Udzial] = @Udzial, [Wybor] = @Wybor, [Status] = @Status WHERE [Id] = @Id"
    FilterExpression="AktywneOd <= '{0}' and '{0}' <= ISNULL(AktywneDo, '{0}')">
    <FilterParameters>
        <asp:ControlParameter Name="Data" ControlId="deNaDzien" PropertyName="DateStr"/>
    </FilterParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="cc" Type="String" />
        <asp:Parameter Name="Grupa" Type="Boolean" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="AktywneOd" Type="datetime" />
        <asp:Parameter Name="AktywneDo" Type="datetime" />        
        <asp:Parameter Name="IdGrupy" Type="Int32" />
        <asp:Parameter Name="Udzial" Type="Double" />
        <asp:Parameter Name="Wybor" Type="Boolean" />        
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="cc" Type="String" />
        <asp:Parameter Name="Grupa" Type="Boolean" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="AktywneOd" Type="datetime" />
        <asp:Parameter Name="AktywneDo" Type="datetime" />        
        <asp:Parameter Name="IdGrupy" Type="Int32" />
        <asp:Parameter Name="Udzial" Type="Double" />
        <asp:Parameter Name="Wybor" Type="Boolean" />        
        <asp:Parameter Name="Status" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select 'wybierz ...' as Nazwa, null as Id, 0 as Sort union
                   SELECT Nazwa, Id, 1 as Sort FROM [CC] where Grupa=1 ORDER BY Sort, Nazwa" >
</asp:SqlDataSource>


