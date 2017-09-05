<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntStanowiska.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Adm.Mapowanie.cntStanowiska" %>

<%@ Register Src="~/Scorecards/Controls/Admin/cntSearch.ascx" TagPrefix="leet" TagName="Search" %>

<div id="ctSpreadsheetsList" runat="server" class="cntSpreadsheetsList" style="margin-top: 16px;">
    <div class="search-box form-group">
        <asp:TextBox ID="tbSearch" runat="server" CssClass="form-control input-sm" Style="display: inline-block; min-width: 260px; width: auto;" placeholder="Szkolenie" />
        <asp:Button ID="btnSerach" runat="server" Text="Wyszukaj" CssClass="btn btn-sm btn-default" />
        <asp:Button ID="btnClear" runat="server" Text="Czyść" CssClass="btn btn-sm btn-default" OnClick="btnClear_Click" />
    </div>
    <div class="form-group">
        <asp:Label ID="lblShow" runat="server" CssClass="label">Pokaż tylko nieprzypisane:</asp:Label>
        <asp:CheckBox ID="cbShow" runat="server" AutoPostBack="true" OnCheckedChanged="cbShow_CheckedChanged" />
    </div>
    <asp:ListView ID="lvStanowiska" runat="server" DataSourceID="dsStanowiska" DataKeyNames="Id"
        InsertItemPosition="None" OnSelectedIndexChanged="lvSpreadsheets_SelectedIndexChanged">
        <ItemTemplate>
            <tr id="Tr1" style="" runat="server" class="it">
                <td class="name">
                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Nazwa") %>' />
                </td>
                <td id="tdControl" class="control" runat="server">
                    <asp:LinkButton ID="SelectButton" runat="server" CommandName="Select"><i class="glyphicon glyphicon-unchecked checker"></i></asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table id="Table1" runat="server" class="table0">
                <tr class="edt">
                    <td>
                        <asp:Label ID="lbNoData" runat="server" Text="Brak danych" /><br />
                        <br />
                        <%--<asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord" Text="Dodaj" />--%>
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <tr class="iit">
                <td>
                    <asp:TextBox ID="lbZastepowany" runat="server" Text='<%# Bind("Nazwa") %>' />
                </td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Opis") %>' />
                </td>
                <td>
                    <asp:DropDownList ID="ddlGenre" runat="server" DataSourceID="dsGenre" DataValueField="Id" DataTextField="Name" />
                    <asp:SqlDataSource ID="dsGenre" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                        SelectCommand="select Id, Nazwa as Name from scSlowniki where Typ = 'ARK' and Aktywny = 1" />
                </td>
                <td class="check">
                    <asp:CheckBox ID="cbActive" runat="server" Checked='<%# Bind("Aktywny") %>' />
                </td>
                <td id="tdControl" class="control">
                    <asp:Button ID="btSave" runat="server" CommandName="Insert" Text="Zapisz" />
                </td>
            </tr>
        </InsertItemTemplate>
        <EditItemTemplate>
            <tr class="eit">
                <td>
                    <asp:TextBox ID="lbZastepowany" runat="server" Text='<%# Bind("Nazwa") %>' />
                </td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Opis") %>' />
                </td>
                <td>
                    <asp:DropDownList ID="ddlGenre" runat="server" DataSourceID="dsGenre" DataValueField="Id" DataTextField="Name" SelectedValue='<%# Eval("Rodzaj") %>' />
                    <asp:SqlDataSource ID="dsGenre" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                        SelectCommand="select Id, Nazwa as Name from scSlowniki where Typ = 'ARK' and Aktywny = 1" />
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
        <SelectedItemTemplate>
            <tr id="Tr2" style="" runat="server" class="sit">
                <td>
                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Nazwa") %>' />
                </td>
                <td id="tdControl" class="control" runat="server">
                    <asp:LinkButton ID="SelectButton" runat="server" CommandName="Select"><i class="glyphicon glyphicon-check checker checked"></i></asp:LinkButton>
                </td>
            </tr>
        </SelectedItemTemplate>
        <LayoutTemplate>
            <table runat="server" class="xListView1 xtbZastepstwa xhoverline">
                <tr id="Tr1" runat="server">
                    <td id="Td1" runat="server">
                        <table id="itemPlaceholderContainer" runat="server" class="lvSpreadsheets">
                            <tr id="Tr2" runat="server" style="">
                                <th id="Th1" runat="server">
                                    <asp:LinkButton ID="LinkButton1" runat="server" Text="Nazwa" CommandName="Sort" CommandArgument="Nazwa" /></th>
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

    <asp:SqlDataSource ID="dsStanowiska" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
        SelectCommand="
select 
 u.* 
from Uprawnienia u
left join UprawnieniaKwalifikacje uk on uk.Id = u.KwalifikacjeId
where 
u.Typ = 2048 
and uk.NazwaEN = 'STAN'
and (@search is null or u.Nazwa like '%' + @search + '%')
and ((@showna = 1 and u.Id not in (select IdStanowiska from msLinieStanowiska)) or @showna = 0)
order by u.Nazwa
    "
        DeleteCommand="DELETE FROM scTypyArkuszy WHERE [Id] = @Id"
        InsertCommand="INSERT INTO scTypyArkuszy (Nazwa, Opis, Rodzaj, Aktywny) VALUES (@Nazwa, @Opis, @Rodzaj, @Aktywny)"
        UpdateCommand="update scTypyArkuszy SET Nazwa = @Nazwa, Opis = @Opis, Rodzaj = @Rodzaj, Aktywny = @Aktywny WHERE [Id] = @Id">
        <SelectParameters>
            <asp:ControlParameter Name="search" Type="String" ControlID="tbSearch" PropertyName="Text" />
            <asp:ControlParameter Name="showna" Type="Boolean" ControlID="cbShow" PropertyName="Checked" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Id" Type="Int32" />
            <asp:Parameter Name="Nazwa" Type="String" />
            <asp:Parameter Name="Opis" Type="String" />
            <asp:Parameter Name="Rodzaj" Type="Int32" />
            <asp:Parameter Name="Aktywny" Type="Boolean" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="Nazwa" Type="String" />
            <asp:Parameter Name="Opis" Type="String" />
            <asp:Parameter Name="Rodzaj" Type="Int32" />
            <asp:Parameter Name="Aktywny" Type="Boolean" />
        </InsertParameters>
    </asp:SqlDataSource>



</div>
