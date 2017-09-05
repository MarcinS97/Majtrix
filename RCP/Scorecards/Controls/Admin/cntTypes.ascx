<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntTypes.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Admin.cntTypes" %>

<div id="ctTypes" runat="server" class="cntTypes" >


    <asp:DropDownList ID="ddlTypes" CssClass="ddlTypes" runat="server" AutoPostBack="true"  >
        <asp:ListItem Value="ARK" Text="Arkusze" />
        <asp:ListItem Value="QC" Text="QC" />
        <asp:ListItem Value="PROD" Text="Produktywność" />
        <asp:ListItem Value="QUATRO" Text="Dodatki kwartalne" />
    </asp:DropDownList>
    
    
    
    
    <asp:ListView ID="lvTypes" runat="server" DataSourceID="dsTypes" 
    DataKeyNames="_id" InsertItemPosition="LastItem" >
    <ItemTemplate>
        <tr id="Tr1" style="" runat="server" class="it">
            <td>
                <asp:Label ID="lbZastepowany" runat="server" Text='<%# Eval("Typ") %>' />
            </td>
            <td>
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("Id") %>' />
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" Text='<%# Eval("Nazwa2") %>' />
            </td>
            <td>
                <asp:Label ID="Label3" runat="server" Text='<%# Eval("Opis") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="TextBox3" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" />
            </td>
            <td id="tdControl" class="control" runat="server">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
                <%--<asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />--%>
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table id="Table1" runat="server" class="table0">
            <tr class="edt">
                <td>
                    <asp:Label ID="lbNoData" runat="server" Text="Brak danych" /><br /><br />
                    <asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord" Text="Dodaj" />
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <InsertItemTemplate>
        <tr class="iit">
            <td>
            </td>
            <td>
                <asp:TextBox ID="lbZastepowany" runat="server" Text='<%# Bind("Id") %>' />
            </td>
            <td>
                <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Nazwa") %>' />
            </td>
            <td>
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Nazwa2") %>' />
            </td>
            <td>
                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Opis") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="TextBox3" runat="server" Checked='<%# Bind("Aktywny") %>' />
            </td>
            <td id="tdControl" class="control">
                <asp:Button ID="btSave" runat="server" CommandName="Insert" Text="Zapisz" />
                <%--<asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Anuluj" />--%>
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td>
            </td>
            <td>
                <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("Id") %>' />
            </td>
            <td>
                <asp:TextBox ID="lbZastepowany" runat="server" Text='<%# Bind("Nazwa") %>' />
            </td>
            <td>
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Nazwa2") %>' />
            </td>
            <td>
                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Opis") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="TextBox3" runat="server" Checked='<%# Bind("Aktywny") %>' />
            </td>
            
            <td id="tdControl" class="control">
                <asp:Button ID="btSave" runat="server" CommandName="Update" Text="Zapisz" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table id="Table1" runat="server" class="ListView1 tbZastepstwa hoverline">
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table ID="itemPlaceholderContainer" runat="server">
                        <tr id="Tr2" runat="server" style="">
                            <th id="thZastepowany" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Typ">
                               Typ</asp:LinkButton></th>
                            <th id="th4" runat="server"><asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="Typ">
                               Id</asp:LinkButton></th>
                            <th id="thZastepujacy" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="Nazwa">
                                Nazwa</asp:LinkButton></th>
                            <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Nazwa2">
                                Nazwa2</asp:LinkButton></th>
                            <th id="Th3" runat="server"><asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="Opis">
                                Opis</asp:LinkButton></th>
                            <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="Aktywny">
                                Aktywny</asp:LinkButton></th>
                            <th id="thControl" class="control" runat="server">
                                <%--<asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Dodaj" ToolTip="Dodaj zastępstwo"/>--%>
                            </th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trPager" runat="server" >
<%--                <td id="Td2" runat="server" class="pager">
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                </td>--%>
            </tr>
           <%-- <tr id="tr3" runat="server" >
                <td class="bottom_buttons">
                    <asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord" Text="Dodaj zastępstwo" />
                </td>
            </tr>--%>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="dsTypes" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="select * from scSlowniki where Typ = @Typ"
    DeleteCommand="DELETE FROM [Zastepstwa] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO scSlowniki (Id, Typ, Nazwa, Nazwa2, Opis, Aktywny) VALUES (@Id, @Typ, @Nazwa, @Nazwa2, @Opis, @Aktywny)" 
    UpdateCommand="update scSlowniki SET Id = @Id, Nazwa = @Nazwa, Nazwa2 = @Nazwa2, Opis = @Opis, Aktywny = @Aktywny WHERE [_id] = @_id">
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlTypes" Name="Typ" PropertyName="SelectedValue" Type="String" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="_id" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Nazwa2" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:ControlParameter ControlID="ddlTypes" Name="Typ" PropertyName="SelectedValue" Type="String" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Nazwa2" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>

</div>