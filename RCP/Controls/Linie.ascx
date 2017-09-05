<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Linie.ascx.cs" Inherits="HRRcp.Controls.Linie" %>
<%@ Register src="DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>

<asp:ListView ID="lvLinie" runat="server" DataSourceID="SqlDataSource1" 
    DataKeyNames="Id" InsertItemPosition="LastItem" 
    onitemdatabound="lvLinie_ItemDataBound" oniteminserting="lvLinie_ItemInserting" 
    onitemupdating="lvLinie_ItemUpdating" 
    onitemdeleting="lvLinie_ItemDeleting">
    <ItemTemplate>
        <tr class="it">
            <td>
                <asp:Label ID="SymbolLabel" runat="server" Text='<%# Eval("Symbol") %>' />
            </td>
            <td>
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="DataOdLabel" runat="server" Text='<%# Eval("DataOd", "{0:d}") %>' />
            </td>
            <td>
                <asp:Label ID="DataDoLabel" runat="server" Text='<%# Eval("DataDo", "{0:d}") %>' />
            </td>
            <td>
                <asp:Label ID="GrSplituPLabel" runat="server" Text='<%# Eval("SplitP") %>' />
            </td>
            <td>
                <asp:Label ID="GrSplituKLabel" runat="server" Text='<%# Eval("SplitK") %>' />
            </td>
            <td class="control">
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
            </td>
        </tr>
    </ItemTemplate>
    <SelectedItemTemplate>
        <tr class="sit">
            <td>
                <asp:Label ID="SymbolLabel" runat="server" Text='<%# Eval("Symbol") %>' />
            </td>
            <td>
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="DataOdLabel" runat="server" Text='<%# Eval("DataOd") %>' />
            </td>
            <td>
                <asp:Label ID="DataDoLabel" runat="server" Text='<%# Eval("DataDo") %>' />
            </td>
            <td>
                <asp:Label ID="GrSplituPLabel" runat="server" Text='<%# Eval("SplitP") %>' />
            </td>
            <td>
                <asp:Label ID="GrSplituKLabel" runat="server" Text='<%# Eval("SplitK") %>' />
            </td>
            <td class="control">
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
            </td>
        </tr>
    </SelectedItemTemplate>
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
                <asp:TextBox ID="SymbolTextBox" runat="server" Text='<%# Bind("Symbol") %>' />
            </td>
            <td>
                <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataOd") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit2" runat="server" Date='<%# Bind("DataDo") %>' />
            </td>
            <td>
                <asp:DropDownList ID="ddlSplitP" runat="server" 
                    DataSourceID="SqlDataSource7"
                    DataTextField="Nazwa"
                    DataValueField="Id">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlSplitK" runat="server" 
                    DataSourceID="SqlDataSource7"
                    DataTextField="Nazwa"
                    DataValueField="Id">
                </asp:DropDownList>
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td>
                <asp:TextBox ID="SymbolTextBox" runat="server" Text='<%# Bind("Symbol") %>' />
            </td>
            <td>
                <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataOd") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit2" runat="server" Date='<%# Bind("DataDo") %>' />
            </td>
            <td>
                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Bind("GrSplituP") %>'/>                
                <asp:DropDownList ID="ddlSplitP" runat="server" 
                    DataSourceID="SqlDataSource7"
                    DataTextField="Nazwa"
                    DataValueField="Id">
                </asp:DropDownList>
            </td>
            <td>
                <asp:HiddenField ID="HiddenField2" runat="server" Value='<%# Bind("GrSplituK") %>'/>                
                <asp:DropDownList ID="ddlSplitK" runat="server" 
                    DataSourceID="SqlDataSource7"
                    DataTextField="Nazwa"
                    DataValueField="Id">
                </asp:DropDownList>
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1 hoverline narrow">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" class="tbLinie">
                        <tr runat="server" style="">
                            <th runat="server">
                                Symbol</th>
                            <th runat="server">
                                Nazwa</th>
                            <th runat="server">
                                DataOd</th>
                            <th runat="server">
                                DataDo</th>
                            <th runat="server">
                                Split pracowników</th>
                            <th runat="server">
                                Split kierowników</th>
                            <th id="Th1" runat="server">
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
    ConflictDetection="CompareAllValues" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [Linie] WHERE [Id] = @original_Id AND (([Symbol] = @original_Symbol) OR ([Symbol] IS NULL AND @original_Symbol IS NULL)) AND (([Nazwa] = @original_Nazwa) OR ([Nazwa] IS NULL AND @original_Nazwa IS NULL)) AND [DataOd] = @original_DataOd AND (([DataDo] = @original_DataDo) OR ([DataDo] IS NULL AND @original_DataDo IS NULL)) AND (([GrSplituP] = @original_GrSplituP) OR ([GrSplituP] IS NULL AND @original_GrSplituP IS NULL)) AND (([GrSplituK] = @original_GrSplituK) OR ([GrSplituK] IS NULL AND @original_GrSplituK IS NULL))" 
    InsertCommand="INSERT INTO [Linie] ([Symbol], [Nazwa], [DataOd], [DataDo], [GrSplituP], [GrSplituK]) VALUES (@Symbol, @Nazwa, @DataOd, @DataDo, @GrSplituP, @GrSplituK)" 
    OldValuesParameterFormatString="original_{0}" 
    SelectCommand="SELECT L.*,SP.Nazwa as SplitP,SK.Nazwa as SplitK FROM Linie L
                   left outer join Splity SP on SP.GrSplitu = L.GrSplituP and ISNULL(SP.DataDo, GETDATE()) &gt;= GETDATE() 
                   left outer join Splity SK on SK.GrSplitu = L.GrSplituK and ISNULL(SK.DataDo, GETDATE()) &gt;= GETDATE() 
                   ORDER BY Symbol" 
    UpdateCommand="UPDATE [Linie] SET [Symbol] = @Symbol, [Nazwa] = @Nazwa, [DataOd] = @DataOd, [DataDo] = @DataDo, [GrSplituP] = @GrSplituP, [GrSplituK] = @GrSplituK 
                   WHERE [Id] = @original_Id AND (([Symbol] = @original_Symbol) OR ([Symbol] IS NULL AND @original_Symbol IS NULL)) AND (([Nazwa] = @original_Nazwa) OR ([Nazwa] IS NULL AND @original_Nazwa IS NULL)) AND [DataOd] = @original_DataOd AND (([DataDo] = @original_DataDo) OR ([DataDo] IS NULL AND @original_DataDo IS NULL)) AND (([GrSplituP] = @original_GrSplituP) OR ([GrSplituP] IS NULL AND @original_GrSplituP IS NULL)) AND (([GrSplituK] = @original_GrSplituK) OR ([GrSplituK] IS NULL AND @original_GrSplituK IS NULL))">
    <DeleteParameters>
        <asp:Parameter Name="original_Id" Type="Int32" />
        <asp:Parameter Name="original_Symbol" Type="String" />
        <asp:Parameter Name="original_Nazwa" Type="String" />
        <asp:Parameter Name="original_DataOd" Type="DateTime" />
        <asp:Parameter Name="original_DataDo" Type="DateTime" />
        <asp:Parameter Name="original_GrSplituP" Type="Int32" />
        <asp:Parameter Name="original_GrSplituK" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Symbol" Type="String" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="DataOd" Type="DateTime" />
        <asp:Parameter Name="DataDo" Type="DateTime" />
        <asp:Parameter Name="GrSplituP" Type="Int32" />
        <asp:Parameter Name="GrSplituK" Type="Int32" />
        <asp:Parameter Name="original_Id" Type="Int32" />
        <asp:Parameter Name="original_Symbol" Type="String" />
        <asp:Parameter Name="original_Nazwa" Type="String" />
        <asp:Parameter Name="original_DataOd" Type="DateTime" />
        <asp:Parameter Name="original_DataDo" Type="DateTime" />
        <asp:Parameter Name="original_GrSplituP" Type="Int32" />
        <asp:Parameter Name="original_GrSplituK" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Symbol" Type="String" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="DataOd" Type="DateTime" />
        <asp:Parameter Name="DataDo" Type="DateTime" />
        <asp:Parameter Name="GrSplituP" Type="Int32" />
        <asp:Parameter Name="GrSplituK" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource7" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select 0 as Sort, null as Id, 'wybierz ...' as Nazwa union
                   SELECT 1 as Sort, GrSplitu as Id, Nazwa FROM Splity where ISNULL(DataDo, GETDATE()) &gt;= GETDATE() 
                   ORDER BY Sort, Nazwa">
</asp:SqlDataSource>

