<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSchematy.ascx.cs" Inherits="HRRcp.RCP.Adm.cntSchematy" %>

<div class="cntSchematyAdm">

    <asp:ListView ID="lvSchematy" runat="server" DataKeyNames="Id" DataSourceID="dsSchematy" 
        OnItemDataBound="lvSchematy_ItemDataBound" OnItemCommand="lvSchematy_ItemCommand" 
        InsertItemPosition="None" OnItemCreated="lvSchematy_ItemCreated" OnItemDeleted="lvSchematy_ItemDeleted" OnItemUpdated="lvSchematy_ItemUpdated">
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Label ID="Label1" Text='<%# Eval("Nazwa") %>' runat="server" />
                </td>
                <td>
                    <asp:Label ID="Label2" Text='<%# Eval("Opis") %>' runat="server" />
                </td>
<%--                <td>
                    <asp:Label ID="Label3" Text='<%# Eval("Schemat") %>' runat="server" />
                </td>--%>
                <%--                <td>
                    <asp:Label ID="Label4" Text='<%# Eval("SQL") %>' runat="server" />
                </td>
                <td>
                    <asp:Label ID="Label5" Text='<%# Eval("Funkcja") %>' runat="server" />
                </td>--%>
                <td>
                    <asp:CheckBox ID="Checkbox1" Checked='<%# Eval("Aktywny") %>' runat="server" Enabled="false" />
                </td>
                <td>
                    <asp:Label ID="Label6" Text='<%# Eval("Kolejnosc") %>' runat="server" />
                </td>
                <td class="control">
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" Visible='<%# !IsEdit() %>' />
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" CssClass="btn-danger" Visible='<%# !IsEdit() %>' />
                </td>

            </tr>
        </ItemTemplate>
        <EditItemTemplate>
            <tr>
                <td>
                    <asp:HiddenField ID="hidZmiany" runat="server" Visible="false" Value='<%# Bind("Schemat") %>' />
                    <asp:TextBox ID="Label1" Text='<%# Bind("Nazwa") %>' runat="server" CssClass="form-control" />
                </td>
                <td>
                    <asp:TextBox ID="Label2" Text='<%# Bind("Opis") %>' runat="server" CssClass="form-control" />
                </td>
                <td>
                    <asp:CheckBox ID="Checkbox1" Checked='<%# Bind("Aktywny") %>' runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="TextBox1" Text='<%# Bind("Kolejnosc") %>' runat="server" CssClass="form-control" />
                </td>
                <td></td>
            </tr>
            <asp:Repeater ID="rpZmiany" runat="server" DataSourceID="dsZmianyRep">
                <ItemTemplate>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="Label6" Text='<%# Eval("Name") %>' runat="server" />
                        </td>
                        <td style="text-align: right;">
                            <asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click" CssClass="" 
                                CommandArgument='<%# Eval("Id").ToString() + ";" + Container.ItemIndex.ToString()  %>'>
                                <i class="glyphicon glyphicon-remove text-danger"></i></asp:LinkButton> 
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <asp:SqlDataSource ID="dsZmianyRep" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                SelectCommand="
--declare @zmiany nvarchar(MAX) = '182,181'
select z.Id, z.Symbol + ' - ' + z.Nazwa Name
from dbo.SplitIntSort(@zmiany, ',') s
left join Zmiany z on z.Id = s.items
order by s.idx, Name">
                <SelectParameters>
                    <asp:ControlParameter Name="zmiany" Type="String" ControlID="hidZmiany" PropertyName="Value" />
                </SelectParameters>
            </asp:SqlDataSource>



            <tr>
                <td colspan="4">
                    <asp:DropDownList ID="ddlZmiany" runat="server" DataSourceID="dsZmiany" DataValueField="Id" DataTextField="Name" OnSelectedIndexChanged="btnAddZmiana_Click" AutoPostBack="true" />
                    <asp:SqlDataSource ID="dsZmiany" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                        SelectCommand="
select null Id, 'wybierz zmianę ...' Name, 0 Sort
union all
select Id, Symbol + ' - ' + Nazwa Name, 1 Sort
from Zmiany 
order by Sort, Name
" />
                </td>
                <td style="text-align: right;">
                    <%--<asp:Button ID="btnAddZmiana" runat="server" CssClass="btn btn-sm btn-success" Text="Dodaj" OnClick="btnAddZmiana_Click" />--%>
                    <%--<asp:LinkButton ID="btnAddZmiana" runat="server" OnClick="btnAddZmiana_Click"><i class="glyphicon glyphicon-plus text-success"></i></asp:LinkButton>--%>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                </td>
                <td class="control">
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" CssClass="btn-success" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                </td>
            </tr>
        </EditItemTemplate>
        <InsertItemTemplate>
                    <tr>
                <td>
                    <asp:HiddenField ID="hidZmiany" runat="server" Visible="false" Value='<%# Bind("Schemat") %>' />
                    <asp:TextBox ID="Label1" Text='<%# Bind("Nazwa") %>' runat="server" CssClass="form-control" />
                </td>
                <td>
                    <asp:TextBox ID="Label2" Text='<%# Bind("Opis") %>' runat="server" CssClass="form-control" />
                </td>
                <td>
                    <asp:CheckBox ID="Checkbox1" Checked='<%# Bind("Aktywny") %>' runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="TextBox1" Text='<%# Bind("Kolejnosc") %>' runat="server" CssClass="form-control" />
                </td>
                <td></td>
            </tr>
            <asp:Repeater ID="rpZmiany" runat="server" DataSourceID="dsZmianyRep">
                <ItemTemplate>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="Label6" Text='<%# Eval("Name") %>' runat="server" />
                        </td>
                        <td style="text-align: right;">
                            <asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click" CssClass="" 
                                CommandArgument='<%# Eval("Id").ToString() + ";" + Container.ItemIndex.ToString() %>'>
                                <i class="glyphicon glyphicon-remove text-danger"></i></asp:LinkButton> 
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <asp:SqlDataSource ID="dsZmianyRep" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                SelectCommand="
--declare @zmiany nvarchar(MAX) = '182,181'
select z.Id, z.Symbol + ' - ' + z.Nazwa Name
from dbo.SplitIntSort(@zmiany, ',') s
left join Zmiany z on z.Id = s.items
order by s.idx, Name">
                <SelectParameters>
                    <asp:ControlParameter Name="zmiany" Type="String" ControlID="hidZmiany" PropertyName="Value" />
                </SelectParameters>
            </asp:SqlDataSource>



            <tr>
                <td colspan="4">
                    <asp:DropDownList ID="ddlZmiany" runat="server" DataSourceID="dsZmiany" DataValueField="Id" DataTextField="Name" OnSelectedIndexChanged="btnAddZmiana_Click" AutoPostBack="true" />
                    <asp:SqlDataSource ID="dsZmiany" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                        SelectCommand="
select null Id, 'wybierz zmianę ...' Name, 0 Sort
union all
select Id, Symbol + ' - ' + Nazwa Name, 1 Sort
from Zmiany 
order by Sort, Name
" />
                </td>
                <td style="text-align: right;">
                    <%--<asp:Button ID="btnAddZmiana" runat="server" CssClass="btn btn-sm btn-success" Text="Dodaj" OnClick="btnAddZmiana_Click" />--%>
                    <%--<asp:LinkButton ID="btnAddZmiana" runat="server" OnClick="btnAddZmiana_Click"><i class="glyphicon glyphicon-plus text-success"></i></asp:LinkButton>--%>
                </td>
            </tr>
            <tr>
                <td colspan="4">

                </td>
                <td class="control">
                    <asp:Button ID="InsertButton1" runat="server" CommandName="Insert" Text="Zapisz" CssClass="btn-success" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Anuluj" />
                </td>
            </tr>
        </InsertItemTemplate>
        <EmptyDataTemplate>
            <table runat="server" style="">
                <tr>
                    <td>Brak danych
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table runat="server" class="table0">
                <tr runat="server">
                    <td colspan="2" runat="server">
                        <table id="itemPlaceholderContainer" class="table">
                            <%-- runat="server" --%>
                            <tr>
                                <th id="th1" runat="server">Nazwa</th>
                                <th id="th2" runat="server">Opis</th>
                                <%--<th id="th3" runat="server">Schemat</th>--%>
                                <%--                                <th id="th4" runat="server">SQL</th>
                                <th id="th5" runat="server">Funkcja</th>--%>
                                <th id="th6" runat="server">Aktywny</th>
                                <th id="th7" runat="server">Kolejność</th>
                                <th id="thRB" class="control" runat="server" visible="true"></th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="trPager" runat="server" visible="false">
                    <td id="Td2" runat="server" class="pager" style="">
                        <asp:DataPager ID="DataPager1" runat="server" PageSize="100">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                                <asp:NumericPagerField ButtonType="Link" />
                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                            </Fields>
                        </asp:DataPager>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
    </asp:ListView>


</div>


<asp:SqlDataSource ID="dsSchematy" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="select * from rcpSchematy"
    UpdateCommand="update rcpSchematy set Nazwa = @Nazwa, Opis = @Opis, Aktywny = @Aktywny, Kolejnosc = @Kolejnosc, Schemat = @Schemat where Id = @Id"
    InsertCommand="insert into rcpSchematy (Nazwa, Opis, Aktywny, Kolejnosc, Schemat) values (@Nazwa, @Opis, @Aktywny, @Kolejnosc, @Schemat)"
    DeleteCommand="delete from rcpSchematy where Id = @Id"
>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
        <asp:Parameter Name="Kolejnosc" Type="Int32" />
        <asp:Parameter Name="Schemat" Type="String" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
        <asp:Parameter Name="Kolejnosc" Type="Int32" />
        <asp:Parameter Name="Schemat" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>


