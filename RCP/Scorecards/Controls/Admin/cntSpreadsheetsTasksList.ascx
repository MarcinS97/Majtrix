<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSpreadsheetsTasksList.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Admin.cntSpreadsheetsTasksList" %>

<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>
<%@ Register Src="~/Scorecards/Controls/Admin/cntSearch.ascx" TagPrefix="leet" TagName="Search" %>



<div id="ctSpreadsheetsTasksList" runat="server" class="cntSpreadsheetsTasksList">
    <asp:HiddenField ID="hidSpreadsheetId" runat="server" />
    
    
    <leet:Search ID="Search" runat="server" TableName="lvSpreadsheets" ColumnName="task" />
    
    <asp:Label ID="lblMustSelect" runat="server" Text="Wybierz arkusz" />
    <asp:ListView ID="lvSpreadsheets" runat="server" DataSourceID="dsSpreadsheets" DataKeyNames="Id"
        InsertItemPosition="None">
        <ItemTemplate>
            <tr id="Tr1" style="" runat="server" class="it">
                <td  class="task"><asp:Label ID="Label1" runat="server" Text='<%# Eval("Task") %>' /></td>
                <td><asp:Label ID="Label2" runat="server" Text='<%# Eval("DateFrom") %>' /></td>
                <td><asp:Label ID="Label3" runat="server" Text='<%# Eval("DateTo") %>' /></td>
                <td class="check"><asp:CheckBox ID="cbQC" runat="server" Checked='<%# Eval("QC") %>' Enabled="false" /></td>
                <td id="tdControl" class="control" runat="server">
                    <asp:Button ID="SelectButton" runat="server" CommandName="Select" Text="Wybierz" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
                    <%--<asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />--%>
                </td>
<%--                <td class="check">
                    <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("Id") %>' />
                    <asp:CheckBox ID="cbSelect" runat="server" AutoPostBack="true" OnCheckedChanged="CheckItem" />
                </td>--%>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table id="Table1" runat="server" class="table0">
                <tr class="edt">
                    <td>
                        <%--<asp:Label ID="lbNoData" runat="server" Text="Brak danych" />--%><br />
                        <br />
                        <%--<asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord" Text="Dodaj" />--%>
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
                    <%--<asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Anuluj" />--%>
                </td>
            </tr>
        </InsertItemTemplate>
        <EditItemTemplate>
            <tr class="eit">
                <td class="task">
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Task") %>' />
                </td>
                <td class="data">
                    <uc1:DateEdit ID="deFrom" runat="server" Date='<%# Bind("Od") %>' ValidationGroup="evg" />
                </td>
                <td class="data">
                    <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("Do") %>' />
                </td>
                <td class="check">
                    <asp:CheckBox ID="cbQC" runat="server" Checked='<%# Bind("QC") %>' />
                </td>
                <td id="tdControl" class="control blockControl">
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                </td>
<%--                <td></td>--%>
            </tr>
        </EditItemTemplate>
        <SelectedItemTemplate>
            <tr id="Tr2" style="" runat="server" class="sit">
                <td  class="task"><asp:Label ID="Label1" runat="server" Text='<%# Eval("Task") %>' /></td>
                <td><asp:Label ID="Label2" runat="server" Text='<%# Eval("DateFrom") %>' /></td>
                <td><asp:Label ID="Label3" runat="server" Text='<%# Eval("DateTo") %>' /></td>
                <td class="check"><asp:CheckBox ID="cbQC" runat="server" Checked='<%# Eval("QC") %>' Enabled="false" /></td>
                <td id="tdControl" class="control" runat="server">
                    <asp:Button ID="SelectButton" runat="server" CommandName="Select" Text="Wybierz" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
                    <%--<asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />--%>
                </td>
<%--                <td class="check">
                    <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("Id") %>' />
                    <asp:CheckBox ID="cbSelect" runat="server" AutoPostBack="true" OnCheckedChanged="CheckItem" />
                </td>--%>
            </tr>
        </SelectedItemTemplate>
        <LayoutTemplate>
            <table id="Table1" runat="server" class="ListView1 tbZastepstwa hoverline">
                <tr id="Tr1" runat="server">
                    <td id="Td1" runat="server">
                        <table id="itemPlaceholderContainer" runat="server" class="lvSpreadsheets">
                            <tr id="Tr2" runat="server" style="">
                                <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" Text="Czynność" CommandName="Sort" CommandArgument="Task" /></th>
                                <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" Text="Od" CommandName="Sort" CommandArgument="Od" /></th>
                                <th id="Th3" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" Text="Do" CommandName="Sort" CommandArgument="Do" /></th>
                                <th id="Th4" runat="server"><asp:LinkButton ID="LinkButton4" runat="server" Text="QC" CommandName="Sort" CommandArgument="QC" /></th>
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
<%--                    <td id="Td2" runat="server" class="pager">
                        <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true"
                                    ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false"
                                    FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                                <asp:NumericPagerField ButtonType="Link" />
                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false"
                                    ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true"
                                    NextPageText="Następna" LastPageText="Ostatnia" />
                            </Fields>
                        </asp:DataPager>
                    </td>--%>
                </tr>
            </table>
        </LayoutTemplate>
    </asp:ListView>
    <asp:SqlDataSource ID="dsSpreadsheets" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
        SelectCommand="
select tac.*, c.Nazwa + ' (' + CC.cc + ')' as Task,  left(Convert(nvarchar, Od, 20), 10) as DateFrom,  left(Convert(nvarchar, Do, 20), 10) as DateTo
from scTypyArkuszyCzynnosci tac
left join scCzynnosci c on c.Id = tac.IdCzynnosci
left join CC on CC.Id = c.IdCC
where tac.IdTypuArkuszy = @SpreadsheetId
order by Id desc
" DeleteCommand="update scTypyArkuszyCzynnosci set Do = GetDate() where Id = @Id" InsertCommand="insert into scTypyArkuszyCzynnosci (Nazwa, Opis, Rodzaj, Aktywny) VALUES (@Nazwa, @Opis, @Rodzaj, @Aktywny)"
        UpdateCommand="update scTypyArkuszyCzynnosci SET Od = @Od, Do = @Do, QC = @QC WHERE [Id] = @Id">
        <SelectParameters>
            <asp:ControlParameter ControlID="hidSpreadsheetId" PropertyName="Value" Name="SpreadsheetId"
                Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Od" Type="DateTime" />
            <asp:Parameter Name="Do" Type="DateTime" />
            <asp:Parameter Name="QC" Type="Boolean" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="Nazwa" Type="String" />
            <asp:Parameter Name="Opis" Type="String" />
            <asp:Parameter Name="Rodzaj" Type="Int32" />
            <asp:Parameter Name="Aktywny" Type="Boolean" />
        </InsertParameters>
    </asp:SqlDataSource>
</div>


<asp:SqlDataSource ID="dsGetAssigned" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select p.Imie + ' ' + p.Nazwisko + ISNULL(' (' + p.KadryId + ')', '') as Pracownik, COUNT(*) as Ilosc from scWartosci w
left join Pracownicy p on w.IdPracownika = p.Id
left join scTypyArkuszyCzynnosci tac on tac.IdCzynnosci = w.IdCzynnosci and w.IdTypuArkuszy = tac.IdTypuArkuszy
where tac.Id = @tacId
group by w.IdPracownika, p.Imie, p.Nazwisko, p.KadryId    
">
    <SelectParameters>
        <asp:Parameter Name="tacId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsRemove" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
delete from scTypyArkuszyCzynnosci where Id = @tacId
">
    <SelectParameters>
        <asp:Parameter Name="tacId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>