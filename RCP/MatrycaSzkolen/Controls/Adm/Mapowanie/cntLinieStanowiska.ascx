<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntLinieStanowiska.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Adm.Mapowanie.cntLinieStanowiska" %>

<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>
<%@ Register Src="~/Scorecards/Controls/Admin/cntSearch.ascx" TagPrefix="leet" TagName="Search" %>

<div id="ctSpreadsheetsTasksList" runat="server" class="cntSpreadsheetsTasksList">
    <asp:HiddenField ID="hidStanowiskoId" runat="server" />
    <asp:Label ID="lblMustSelect" runat="server" Text="Wybierz szkolenie stanowiskowe" Visible="false" />
    <asp:ListView ID="lvLinieStanowiska" runat="server" DataSourceID="dsLinieStanowiska" DataKeyNames="Id"
        InsertItemPosition="None" OnItemDeleted="lvLinieStanowiska_ItemDeleted" OnSelectedIndexChanged="lvLinieStanowiska_SelectedIndexChanged" >
        <ItemTemplate>
            <tr id="Tr1" style="" runat="server" class="it">
                <td>
                    <asp:LinkButton ID="SelectButton" runat="server" CommandName="Select" CssClass="btn text-default" ToolTip="Zaznacz">
                        <i class="glyphicon glyphicon-unchecked checker"></i></asp:LinkButton>
                </td>
                <td class="task">
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Linia") %>' />
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("DateFrom") %>' />
                </td>
                <td>
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("DateTo") %>' />
                </td>
                <td id="tdControl" class="control" runat="server">
                    <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" CssClass="btn" ToolTip="Edytuj">
                        <i class="glyphicon glyphicon-edit"></i></asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table id="Table1" runat="server" class="table0">
                <tr class="edt">
                    <td>
                        <div class="well well-sm">
                            <asp:Label ID="lbNoData" runat="server" Text="Brak danych" /><br />
                        </div>
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <tr class="iit">
                <td>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("TaskName") %>' />
                </td>
                <td>
                    <uc1:DateEdit ID="deFrom" runat="server" Date='<%# Bind("Od") %>' ValidationGroup="ivg" />
                </td>
                <td>
                    <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("Od") %>' />
                </td>
                <td id="tdControl" class="control">
                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Zapisz" />
                </td>
            </tr>
        </InsertItemTemplate>
        <EditItemTemplate>
            <tr class="eit">
                <td></td>
                <td class="task">
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Linia") %>' />
                </td>
                <td class="data">
                    <uc1:DateEdit ID="deFrom" runat="server" Date='<%# Bind("Od") %>' ValidationGroup="evg" />
                </td>
                <td class="data">
                    <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("Do") %>' />
                </td>
                <td id="tdControl" class="control blockControl">
                    <asp:LinkButton ID="SaveButton" runat="server" CommandName="Update" CssClass="btn text-success" ToolTip="Zapisz"><i class="glyphicon glyphicon-floppy-disk"></i></asp:LinkButton>
                    <asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel" CssClass="btn" ToolTip="Anuluj"><i class="glyphicon glyphicon-ban-circle"></i></asp:LinkButton>
                </td>
            </tr>
        </EditItemTemplate>
        <SelectedItemTemplate>
            <tr id="Tr2" style="" runat="server" class="sit">
                <td>
                    <asp:LinkButton ID="LinkButton4" runat="server" CommandName="Select" CssClass="btn"><i class="glyphicon glyphicon-check checker checked"></i></asp:LinkButton>
                </td>
                <td class="task">
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Linia") %>' />
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("DateFrom") %>' />
                </td>
                <td>
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("DateTo") %>' />
                </td>
                <td id="tdControl" class="control" runat="server">
                    <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" CssClass="btn xbtn-sm xbtn-default"><i class="glyphicon glyphicon-edit"></i></asp:LinkButton>
                </td>
            </tr>
        </SelectedItemTemplate>
        <LayoutTemplate>
            <table runat="server" class="xListView1 xtbZastepstwa xhoverline" style="width: 100%;">
                <tr id="Tr1" runat="server">
                    <td id="Td1" runat="server">
                        <table id="itemPlaceholderContainer" runat="server" class="lvSpreadsheets">
                            <tr id="Tr2" runat="server" style="">
                                <th id="Th4" runat="server">
                                </th>
                                <th id="Th1" runat="server">
                                    <asp:LinkButton ID="LinkButton1" runat="server" Text="Linia" CommandName="Sort" CommandArgument="Linia" />
                                </th>
                                <th id="Th2" runat="server">
                                    <asp:LinkButton ID="LinkButton2" runat="server" Text="Od" CommandName="Sort" CommandArgument="Od" />
                                </th>
                                <th id="Th3" runat="server">
                                    <asp:LinkButton ID="LinkButton3" runat="server" Text="Do" CommandName="Sort" CommandArgument="Do" />
                                </th>
                                <th id="thControl" class="control" runat="server">
                                    <%--<asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Dodaj" ToolTip="Dodaj zastępstwo"/>--%>
                                </th>
                                <%--                                <th>
                                    Zaznacz
                                </th>--%>
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
            </table>
        </LayoutTemplate>
    </asp:ListView>
    <asp:SqlDataSource ID="dsLinieStanowiska" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
        SelectCommand="
select ls.*, l.Nazwa as Linia,  left(Convert(nvarchar, Od, 20), 10) as DateFrom,  left(Convert(nvarchar, Do, 20), 10) as DateTo
from msLinieStanowiska ls
left join msLinie l on l.Id = ls.IdLinii
where ls.IdStanowiska = @StanowiskoId
order by Id desc
"
        DeleteCommand="update msLinieStanowiska set Do = GetDate() where Id = @Id"
        UpdateCommand="update msLinieStanowiska SET Od = @Od, Do = @Do WHERE [Id] = @Id">
        <SelectParameters>
            <asp:ControlParameter ControlID="hidStanowiskoId" PropertyName="Value" Name="StanowiskoId"
                Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Od" Type="DateTime" />
            <asp:Parameter Name="Do" Type="DateTime" />
        </UpdateParameters>
    </asp:SqlDataSource>
</div>

<asp:SqlDataSource ID="dsRemove" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
delete from msLinieStanowiska where Id = {0}
" />

<asp:SqlDataSource ID="dsInsert" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
insert into msLinieStanowiska (IdStanowiska, IdLinii, Od) values ({0}, {1}, {2})
" />

