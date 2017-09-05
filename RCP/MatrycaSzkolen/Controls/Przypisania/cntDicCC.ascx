<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntDicCC.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Przypisania.cntDicCC" %>

<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:ListView ID="lvDic" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" InsertItemPosition="LastItem" 
    onitemdatabound="lvDic_ItemDataBound" 
    oniteminserting="lvDic_ItemInserting" 
    onitemupdating="lvDic_ItemUpdating">
    <ItemTemplate>
        <tr id="trLine" runat="server" class="it">
            <td class="cc">
                <asp:Label ID="ccLabel" runat="server" Text='<%# Eval("cc") %>' />
            </td>
            <td class="nazwa">
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("Surplus") %>' Enabled="false" />
            </td>
            <td class="date">
                <asp:Label ID="lbAktywneOd" runat="server" Text='<%# Eval("AktywneOd", "{0:d}") %>' />
            </td>
            <td class="date">
                <asp:Label ID="lbAktywneDo" runat="server" Text='<%# Eval("AktywneDo", "{0:d}") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="GrupaCheckBox" runat="server" Checked='<%# Eval("Grupa") %>' Enabled="false" />
            </td>
            <td class="check">
                <asp:CheckBox ID="WyborCheckBox" runat="server" Checked='<%# Eval("Wybor") %>' Enabled="false" />
            </td>
            <%--
            <td class="num">
                <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
            </td>
            --%>
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
            <td class="col1 cc">
                <asp:TextBox ID="ccTextBox" runat="server" MaxLength="20" Text='<%# Bind("cc") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    Enabled="false"
                    ValidationGroup="vgi" 
                    ControlToValidate="ccTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td class="col4 nazwa">
                <asp:TextBox ID="NazwaTextBox" runat="server" MaxLength="200" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi" 
                    ControlToValidate="NazwaTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td class="check">
                <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("Surplus") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="deAktywneOd" runat="server" Date='<%# Bind("AktywneOd") %>'  />
            </td>
            <td class="data">
                <uc1:DateEdit ID="deAktywneDo" runat="server" Date='<%# Bind("AktywneDo") %>'  />
            </td>
            <td class="check">
                <asp:CheckBox ID="GrupaCheckBox" runat="server" Checked='<%# Bind("Grupa") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="WyborCheckBox" runat="server" Checked='<%# Bind("Wybor") %>' />
            </td>
            <%--
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
            --%>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" ValidationGroup="vgi" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="col1 cc">
                <asp:TextBox ID="ccTextBox" runat="server" MaxLength="20" Text='<%# Bind("cc") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    Enabled="false"
                    ValidationGroup="vgi" 
                    ControlToValidate="ccTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td class="col4 nazwa">
                <asp:TextBox ID="NazwaTextBox" runat="server" MaxLength="200" Text='<%# Bind("Nazwa") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi" 
                    ControlToValidate="NazwaTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td class="check">
                <asp:CheckBox ID="CheckBox3" runat="server" Checked='<%# Bind("Surplus") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="deAktywneOd" runat="server" Date='<%# Bind("AktywneOd") %>'  />
            </td>
            <td class="data">
                <uc1:DateEdit ID="deAktywneDo" runat="server" Date='<%# Bind("AktywneDo") %>'  />
            </td>
            <td class="check">
                <asp:CheckBox ID="GrupaCheckBox" runat="server" Checked='<%# Bind("Grupa") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="WyborCheckBox" runat="server" Checked='<%# Bind("Wybor") %>' />
            </td>
            <%--
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
            --%>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" ValidationGroup="vge"/>
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="tbCC">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th id="Th1" runat="server">
                                Kod CC</th>
                            <th id="Th6" runat="server">
                                Nazwa</th>
                            <th id="Th7" runat="server">
                                Surplus</th>
                            <th id="Th4" runat="server">
                                Aktywne od</th>
                            <th id="Th5" runat="server">
                                Aktywne do</th>
                            <th id="Th3" runat="server">
                                Grupa</th>
                            <th id="Th2" runat="server">
                                Do wybrania</th>
                            <%--
                            <th runat="server">
                                Status</th>
                            --%>
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
    InsertCommand="
INSERT INTO [CC] ([cc], [Grupa], [Nazwa], [AktywneOd], [AktywneDo], [Wybor], [Status], Surplus) VALUES (@cc, @Grupa, @Nazwa, @AktywneOd, @AktywneDo, @Wybor, @Status, @Surplus)
if @Grupa = 1 begin
--    update CC set GrSplitu = 10000 + Id where Id = (select @@Identity)
    update CC set GrSplitu = ISNULL((select MIN(GrSplitu) - 1 from CC where GrSplitu between 9000 and 9999), 9999) where Id = (select @@Identity)
end
    "     
    SelectCommand="select * from CC order by Wybor desc, cc" 
    UpdateCommand="
UPDATE [CC] SET [cc] = @cc, [Grupa] = @Grupa, [Nazwa] = @Nazwa, [AktywneOd] = @AktywneOd, [AktywneDo] = @AktywneDo, [Wybor] = @Wybor, [Status] = @Status, Surplus = @Surplus,

--GrSplitu = case when @Grupa = 1 then 10000 + Id else null end
GrSplitu = case when @Grupa = 1 then 
    case when GrSplitu is null then ISNULL((select MIN(GrSplitu) - 1 from CC where GrSplitu between 9000 and 9999), 9999)
    else GrSplitu
    end
else null
end

WHERE [Id] = @Id
    ">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="cc" Type="String" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="AktywneOd" Type="datetime" />
        <asp:Parameter Name="AktywneDo" Type="datetime" />        
        <asp:Parameter Name="Wybor" Type="Boolean" />        
        <asp:Parameter Name="Grupa" Type="Boolean" />        
        <asp:Parameter Name="Surplus" Type="Boolean" />        
        <asp:Parameter Name="Status" Type="Int32" DefaultValue="1"/>
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="cc" Type="String" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="AktywneOd" Type="datetime" />
        <asp:Parameter Name="AktywneDo" Type="datetime" />        
        <asp:Parameter Name="Wybor" Type="Boolean" />        
        <asp:Parameter Name="Grupa" Type="Boolean" />        
        <asp:Parameter Name="Surplus" Type="Boolean" />        
        <asp:Parameter Name="Status" Type="Int32" DefaultValue="1"/>
    </InsertParameters>
</asp:SqlDataSource>



