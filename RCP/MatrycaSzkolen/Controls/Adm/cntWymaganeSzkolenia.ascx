<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWymaganeSzkolenia.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Adm.cntWymaganeSzkolenia" %>


<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>

<div id="ctSpreadsheetsTasks" runat="server" class="cntSpreadsheetsTasks cntUprawnieniaSzkol">
    <div class="left">
        <asp:Label ID="Label1" runat="server" CssClass="title" Text="Stanowiska" />
        <hr />

        <%--        <cc:Stanowiska ID="Stanowiska" runat="server" OnStanowiskoSelected="StanowiskoSelected" />--%>
        <div id="ctSpreadsheetsList" runat="server" class="cntSpreadsheetsList container" style="margin-top: 12px;">
            <div class="search-box">
                <asp:TextBox ID="tbSearchPrac" runat="server" CssClass="form-control input-sm" Style="display: inline-block; min-width: 260px; width: auto;" placeholder="Pracownik" />
                <asp:Button ID="btnSerachPrac" runat="server" Text="Wyszukaj" CssClass="btn btn-sm btn-default" />
                <asp:Button ID="btnClearPrac" runat="server" Text="Czyść" CssClass="btn btn-sm btn-default" OnClick="btnClearPrac_Click" />
            </div>

            <asp:ListView ID="lvPracownicy" runat="server" DataSourceID="dsPracownicy" DataKeyNames="Id"
                InsertItemPosition="None" OnSelectedIndexChanged="lvPracownicy_SelectedIndexChanged">
                <ItemTemplate>
                    <tr id="Tr1" style="" runat="server" class="it">
                        <td class="name" style="display: none;">
                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("KadryId") %>' /></td>
                        <td class="name">
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Name") %>' /></td>
                        <td id="tdControl" class="control" runat="server">
                            <%--<asp:Button ID="SelectButton" runat="server" CommandName="Select" Text="Wybierz" CssClass="btn-sm" />--%>




                            <asp:LinkButton ID="SelectButton" runat="server" CommandName="Select"><i class="glyphicon glyphicon-unchecked checker"></i></asp:LinkButton>

                            <%--<asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />--%>
                            <%--<asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />--%>
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
                <SelectedItemTemplate>
                    <tr id="Tr2" style="" runat="server" class="sit">
                        <td class="name" style="display: none;">
                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("KadryId") %>' /></td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Name") %>' /></td>
                        <td id="tdControl" class="control" runat="server">

                            <%--<asp:Button ID="SelectButton" runat="server" CommandName="Select" Text="Wybierz" />--%>


                            <asp:LinkButton ID="SelectButton" runat="server" CommandName="Select"><i class="glyphicon glyphicon-check checker checked"></i></asp:LinkButton>

                            <%--<asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />--%>
                            <%--<asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />--%>
                        </td>
                    </tr>
                </SelectedItemTemplate>
                <LayoutTemplate>
                    <table runat="server" class="xListView1 xtbZastepstwa xhoverline">
                        <%--ListView1 tbZastepstwa hoverline--%>
                        <tr id="Tr1" runat="server">
                            <td id="Td1" runat="server">
                                <table id="itemPlaceholderContainer" runat="server" class="lvSpreadsheets">
                                    <%--lvSpreadsheets--%>
                                    <tr id="Tr2" runat="server" style="">
                                        <th id="Th1" runat="server" style="display: none;">
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text="Nr Ewid." CommandName="Sort" CommandArgument="Name" /></th>
                                        <th id="Th2" runat="server">
                                            <asp:LinkButton ID="LinkButton2" runat="server" Text="Nazwa" CommandName="Sort" CommandArgument="Name" /></th>
                                        <th id="thControl" class="control" runat="server"></th>
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

            <asp:SqlDataSource ID="dsPracownicy" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                SelectCommand="
/*select *, Nazwisko + ' ' + Imie as Name from Pracownicy
where Status &gt;= 0 and (@search is null or (Nazwisko + ' ' + Imie) like '%' + @search + '%')
order by Name*/
select *, '' KadryId, Nazwa Name from Stanowiska
where Aktywne = 1 and (@search is null or Nazwa like '%' + @search + '%')
order by Name

">
                <SelectParameters>
                    <asp:ControlParameter Name="search" Type="String" ControlID="tbSearchPrac" PropertyName="Text" />
                </SelectParameters>
            </asp:SqlDataSource>



        </div>




    </div>
    <div class="middle">
        <asp:Label ID="Label2" runat="server" CssClass="title" Text="Stanowiska - Szkolenia" />
        <hr />

        <div id="ctSpreadsheetsTasksList" runat="server" class="cntSpreadsheetsTasksList">
            <asp:HiddenField ID="hidPracId" runat="server" Visible="false" />



            <%--<leet:Search ID="Search" runat="server" TableName="lvSpreadsheets" ColumnName="task" />--%>

            <asp:Label ID="lblMustSelect" runat="server" Text="Wybierz stanowisko" />
            <asp:ListView ID="lvPracSzkolenia" runat="server" DataSourceID="dsPracSzkolenia" DataKeyNames="Id"
                InsertItemPosition="None">
                <ItemTemplate>
                    <tr id="Tr1" style="" runat="server" class="it">
                        <td style="width: 250px;" class="">
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Szkolenie") %>' /></td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("DateFrom") %>' /></td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("DateTo") %>' /></td>
                        <td id="tdControl" class="control" runat="server">

                            <%--<asp:Button ID="SelectButton" runat="server" CommandName="Select" Text="Wybierz" CssClass="btn-sm" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />--%>

                            <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" CssClass="btn xbtn-sm xbtn-default"><i class="glyphicon glyphicon-edit"></i></asp:LinkButton>
                            <asp:LinkButton ID="SelectButton" runat="server" CommandName="Select" CssClass="btn"><i class="glyphicon glyphicon-unchecked checker"></i></asp:LinkButton>

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
                                <asp:Label ID="lbNoData" runat="server" Text="Stanowisko nie posiada wymaganych szkoleń" CssClass="label" /><br />
                                <br />
                                <%--<asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord" Text="Dodaj" />--%>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <InsertItemTemplate>
                    <tr class="iit">
                        <td>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Szkolenie") %>' />
                        </td>
                        <td>
                            <uc1:DateEdit ID="deFrom" runat="server" Date='<%# Bind("DataOd") %>' ValidationGroup="ivg" />
                        </td>
                        <td>
                            <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataDo") %>' />
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
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Szkolenie") %>' />
                        </td>
                        <td class="data">
                            <uc1:DateEdit ID="deFrom" runat="server" Date='<%# Bind("DataOd") %>' ValidationGroup="evg" />
                        </td>
                        <td class="data">
                            <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataDo") %>' />
                        </td>
                        <td id="tdControl" class="control blockControl">
                            <%--                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />--%>
                            <asp:LinkButton ID="SaveButton" runat="server" CommandName="Update" CssClass="btn xbtn-sm xbtn-default text-success"><i class="glyphicon glyphicon-floppy-disk"></i></asp:LinkButton>
                            <asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel" CssClass="btn xbtn-sm xbtn-default"><i class="glyphicon glyphicon-ban-circle"></i></asp:LinkButton>

                        </td>
                        <%--                <td></td>--%>
                    </tr>
                </EditItemTemplate>
                <SelectedItemTemplate>
                    <tr id="Tr2" style="" runat="server" class="sit">
                        <td class="task">
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Szkolenie") %>' /></td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("DateFrom") %>' /></td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("DateTo") %>' /></td>
                        <td id="tdControl" class="control" runat="server">
                            <%--<asp:Button ID="SelectButton" runat="server" CommandName="Select" Text="Wybierz" />--%>




                            <%--<asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />--%>
                            <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" CssClass="btn xbtn-sm xbtn-default"><i class="glyphicon glyphicon-edit"></i></asp:LinkButton>
                            <asp:LinkButton ID="SelectButton" runat="server" CommandName="Select" CssClass="btn"><i class="glyphicon glyphicon-check checker checked"></i></asp:LinkButton>

                            <%--<asp:LinkButton ID="SelectButton" runat="server" CommandName="Select"><i class="glyphicon glyphicon-check checker checked"></i></asp:LinkButton>--%>
                            <%--<asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />--%>
                        </td>
                        <%--                <td class="check">
                    <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("Id") %>' />
                    <asp:CheckBox ID="cbSelect" runat="server" AutoPostBack="true" OnCheckedChanged="CheckItem" />
                </td>--%>
                    </tr>
                </SelectedItemTemplate>
                <LayoutTemplate>
                    <table runat="server" class="xListView1 xtbZastepstwa xhoverline">
                        <tr id="Tr1" runat="server">
                            <td id="Td1" runat="server">
                                <table id="itemPlaceholderContainer" runat="server" class="lvSpreadsheets">
                                    <tr id="Tr2" runat="server" style="">
                                        <th id="Th1" runat="server">
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text="Szkolenie" CommandName="Sort" CommandArgument="Szkolenie" /></th>
                                        <th id="Th2" runat="server">
                                            <asp:LinkButton ID="LinkButton2" runat="server" Text="Od" CommandName="Sort" CommandArgument="Od" /></th>
                                        <th id="Th3" runat="server">
                                            <asp:LinkButton ID="LinkButton3" runat="server" Text="Do" CommandName="Sort" CommandArgument="Do" /></th>
                                        <%--<th id="Th4" runat="server"><asp:LinkButton ID="LinkButton4" runat="server" Text="QC" CommandName="Sort" CommandArgument="QC" /></th>--%>
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
                                        <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true"
                                            ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false"
                                            FirstPageText="«" PreviousPageText="‹" />
                                        <asp:NumericPagerField ButtonType="Link" />
                                        <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false"
                                            ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true"
                                            NextPageText="›" LastPageText="»" />
                                    </Fields>
                                </asp:DataPager>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
            <asp:SqlDataSource ID="dsPracSzkolenia" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                SelectCommand="
select mu.*, u.Nazwa as Szkolenie, left(Convert(nvarchar, mu.DataOd, 20), 10) as DateFrom,  left(Convert(nvarchar, mu.DataDo, 20), 10) as DateTo
from msMapaWymaganychSzkolen mu
left join Uprawnienia u on u.Id = mu.IdUprawnienia
where mu.IdStanowiska = @pracId
order by Id desc
"
                DeleteCommand="update msLinieStanowiska set Do = GetDate() where Id = @Id"
                InsertCommand="insert into scTypyArkuszyCzynnosci (Nazwa, Opis, Rodzaj, Aktywny) VALUES (@Nazwa, @Opis, @Rodzaj, @Aktywny)"
                UpdateCommand="update msMapaWymaganychSzkolen SET DataOd = @DataOd, DataDo = @DataDo WHERE [Id] = @Id">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hidPracId" PropertyName="Value" Name="pracId"
                        Type="Int32" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="Id" Type="Int32" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="DataOd" Type="DateTime" />
                    <asp:Parameter Name="DataDo" Type="DateTime" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="Nazwa" Type="String" />
                    <asp:Parameter Name="Opis" Type="String" />
                    <asp:Parameter Name="Aktywny" Type="Boolean" />
                </InsertParameters>
            </asp:SqlDataSource>
        </div>




    </div>
    <div class="buttons">
        <asp:Button ID="btnAdd" runat="server" Text="◄ Dodaj" CssClass="btn btn-success btn-sm" OnClick="AddTask" />
        <asp:Button ID="btnDelete" runat="server" Text="► Usuń" CssClass="btn btn-danger btn-sm" OnClick="RemoveTask" />
    </div>
    <div class="right">
        <asp:Label ID="Label3" runat="server" CssClass="title" Text="Szkolenia" />
        <hr />
        <%--<cc:Linie id="Linie" runat="server" />--%>

        <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="ctTasks" runat="server" class="cntTasks" style="margin-top: 12px;">


                    <div class="search-box">
                        <asp:TextBox ID="tbSearchSzkol" runat="server" CssClass="form-control input-sm" Style="display: inline-block; min-width: 260px; width: auto;" placeholder="Szkolenie" />
                        <asp:Button ID="btnSerachSzkol" runat="server" Text="Wyszukaj" CssClass="btn btn-sm btn-default" />
                        <asp:Button ID="btnClearSzkol" runat="server" Text="Czyść" CssClass="btn btn-sm btn-default" OnClick="btnClearSzkol_Click" />
                    </div>

                    <%--<leet:Search ID="Search" runat="server" TableName="lvTasks" ColumnName="name" />--%>

                    <asp:ListView ID="lvSzkolenia" runat="server" DataSourceID="dsSzkolenia" DataKeyNames="Id"
                        InsertItemPosition="None">
                        <ItemTemplate>
                            <tr id="trSelect" style="" runat="server" class="it">
                                <td class="check">
                                    <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("Id") %>' />
                                    <asp:CheckBox ID="cbSelect" runat="server" AutoPostBack="true" OnCheckedChanged="CheckItem" />
                                </td>
                                <td class="name">
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Nazwa") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table id="Table1" runat="server" class="table0">
                                <tr class="edt">
                                    <td>
                                        <asp:Label ID="lbNoData" runat="server" Text="Brak danych" /><br />
                                        <br />
                                        <asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord"
                                            Text="Dodaj" />
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table runat="server" class="xListView1 xtbZastepstwa xhoverline">
                                <tr id="Tr1" runat="server">
                                    <td id="Td1" runat="server">
                                        <table id="itemPlaceholderContainer" runat="server" class="lvTasks">
                                            <tr id="Tr2" runat="server" style="">
                                                <th>
                                                    <asp:CheckBox ID="cbAll" runat="server" OnCheckedChanged="CheckAll" AutoPostBack="true" /></th>
                                                <th id="Th1" runat="server">
                                                    <asp:LinkButton ID="LinkButton1" runat="server" Text="Nazwa" CommandName="Sort" CommandArgument="Nazwa" /></th>
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
                                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true"
                                                    ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false"
                                                    FirstPageText="«" PreviousPageText="‹" />
                                                <asp:NumericPagerField ButtonType="Link" />
                                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false"
                                                    ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true"
                                                    NextPageText="›" LastPageText="»" />
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
                    <asp:SqlDataSource ID="dsSzkolenia" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                        SelectCommand="
                select uk.Nazwa + ' - ' + u.Nazwa, u.*, u.Id
from Uprawnienia u
left join UprawnieniaKwalifikacje uk on uk.Id = u.KwalifikacjeId
where uk.Szkolenie = 1 and (@search is null or u.Nazwa like '%' + @search + '%')
"
                        DeleteCommand="DELETE FROM scCzynnosci WHERE [Id] = @Id" InsertCommand="INSERT INTO scCzynnosci (Nazwa, Czas, IdCC, Aktywny) VALUES (@Nazwa, @Czas, @IdCC, @Aktywny)"
                        UpdateCommand="update scCzynnosci SET Nazwa = @Nazwa, Czas = @Czas, IdCC = @IdCC, Aktywny = @Aktywny WHERE [Id] = @Id">
                        <SelectParameters>
                            <asp:ControlParameter Name="search" Type="String" ControlID="tbSearchSzkol" PropertyName="Text" />
                        </SelectParameters>
                        <DeleteParameters>
                            <asp:Parameter Name="Id" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="Id" Type="Int32" />
                            <asp:Parameter Name="Nazwa" Type="String" />
                            <asp:Parameter Name="Czas" Type="String" />
                            <asp:Parameter Name="IdCC" Type="Int32" />
                            <asp:Parameter Name="Aktywny" Type="Boolean" />
                        </UpdateParameters>
                        <InsertParameters>
                            <asp:Parameter Name="Nazwa" Type="String" />
                            <asp:Parameter Name="Czas" Type="String" />
                            <asp:Parameter Name="IdCC" Type="Int32" />
                            <asp:Parameter Name="Aktywny" Type="Boolean" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


    </div>
</div>


<asp:SqlDataSource ID="dsRemove" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
delete from msMapaWymaganychSzkolen where Id = {0}
" />

<asp:SqlDataSource ID="dsInsert" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
insert into msMapaWymaganychSzkolen (IdUprawnienia, IdStanowiska, DataOd) values ({0}, {1}, {2})
" />

