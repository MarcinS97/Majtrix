<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntTypes.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Adm.cntTypes" %>

<div id="ctTypes" runat="server" class="cntTypes">

    <h3 id="lblTitle" runat="server" cssclass="xtitle"></h3>
    <h5 id="lblSubtitle" runat="server" cssclass="xsubTitle"></h5>

    <asp:HiddenField ID="hidTableName" runat="server" Visible="false" />



    <asp:ListView ID="lvTypes" runat="server" DataSourceID="dsTypes" DataKeyNames="Id" InsertItemPosition="LastItem">
        <ItemTemplate>
            <tr id="Tr1" style="" runat="server" class="it">
                <td>
                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("Id") %>' />
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Nazwa") %>' />
                </td>
                <td class="check">
                    <asp:CheckBox ID="TextBox3" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" />
                </td>
                <td id="tdControl" class="control" runat="server">
    <%--            <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" CssClass="btn btn-sm btn-default" />
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />--%>

                    <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" CssClass="btn xbtn-sm xbtn-default"><i class="glyphicon glyphicon-edit"></i></asp:LinkButton>
                    <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" CssClass="btn xbtn-sm xbtn-default text-danger"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table id="Table1" runat="server" class="table0">
                <tr class="edt">
                    <td>
                        <asp:Label ID="lbNoData" runat="server" Text="Brak danych" /><br />
                        <br />
                        <asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord" Text="Dodaj" />
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <tr class="iit">
                <td></td>
                <td>
                    <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Nazwa") %>' />
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
                <td></td>
                <td>
                    <asp:TextBox ID="lbZastepowany" runat="server" Text='<%# Bind("Nazwa") %>' />
                </td>
                <td class="check">
                    <asp:CheckBox ID="TextBox3" runat="server" Checked='<%# Bind("Aktywny") %>' />
                </td>

                <td id="tdControl" class="control">
                    <%--    <asp:Button ID="btSave" runat="server" CommandName="Update" Text="Zapisz" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />--%>

                    <asp:LinkButton ID="SaveButton" runat="server" CommandName="Update" CssClass="btn xbtn-sm xbtn-default text-success"><i class="glyphicon glyphicon-floppy-disk"></i></asp:LinkButton>
                    <asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel" CssClass="btn xbtn-sm xbtn-default"><i class="glyphicon glyphicon-ban-circle"></i></asp:LinkButton>
                </td>
            </tr>
        </EditItemTemplate>
        <LayoutTemplate>
            <table runat="server" class="xListView1 xtbZastepstwa xhoverline">
                <tr id="Tr1" runat="server">
                    <td id="Td1" runat="server">
                        <table id="itemPlaceholderContainer" runat="server">
                            <tr id="Tr2" runat="server" style="">
                                <th id="th4" runat="server">
                                    <asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="Typ">
                               Id</asp:LinkButton></th>
                                <th id="thZastepujacy" runat="server">
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="Nazwa">
                                Nazwa</asp:LinkButton></th>
                                  <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="Aktywny">
                                Aktywny</asp:LinkButton></th>
                                
                                <th id="thControl" class="control" runat="server">
                                    <%--<asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Dodaj" ToolTip="Dodaj zastępstwo"/>--%>
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr id="trPager" runat="server">
                    <td id="Td2" runat="server" class="pager">
                        <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="«" PreviousPageText="‹" />
                                <asp:NumericPagerField ButtonType="Link" />
                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="›" LastPageText="»" />
                            </Fields>
                        </asp:DataPager>
                    </td>
                </tr>
                <%-- <tr id="tr3" runat="server" >
                <td class="bottom_buttons">
                    <asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord" Text="Dodaj zastępstwo" />
                </td>
            </tr>--%>
            </table>
        </LayoutTemplate>
    </asp:ListView>

    <asp:SqlDataSource ID="dsTypes" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" OnDeleting="dsTypes_Deleting"
        SelectCommand="exec('select * from ' + @TableName)"
        DeleteCommand="
exec
(
'delete from ' + @TableName + ' where Id = ' + @Id
)"
        InsertCommand="
exec
(
'insert into ' + @TableName 
+ ' (Nazwa, Aktywny) values (''' + @Nazwa + ''', ''' + @Aktywny + ''')'
)
"
        UpdateCommand="
exec
(
'update ' + @TableName + ' set Nazwa = ''' + @Nazwa + ''', Aktywny = ''' + @Aktywny + ''' where Id = ' + @Id
)">
        <SelectParameters>
            <asp:ControlParameter ControlID="hidTableName" Name="TableName" PropertyName="Value" Type="String" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
            <asp:ControlParameter ControlID="hidTableName" Name="TableName" PropertyName="Value" Type="String" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Id" Type="Int32" />
            <asp:Parameter Name="Nazwa" Type="String" />
            <asp:Parameter Name="Aktywny" Type="Boolean" />
            <asp:ControlParameter ControlID="hidTableName" Name="TableName" PropertyName="Value" Type="String" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="Nazwa" Type="String" />
            <asp:Parameter Name="Aktywny" Type="Boolean" />
            <asp:ControlParameter ControlID="hidTableName" Name="TableName" PropertyName="Value" Type="String" />
        </InsertParameters>
    </asp:SqlDataSource>

</div>
