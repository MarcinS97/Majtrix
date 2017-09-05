<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSpreadsheetsList.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Admin.cntSpreadsheetsList" %>

<%@ Register Src="~/Scorecards/Controls/Admin/cntSearch.ascx" TagPrefix="leet" TagName="Search" %>


<div id="ctSpreadsheetsList" runat="server" class="cntSpreadsheetsList">
    <leet:Search ID="Search" runat="server" TableName="lvSpreadsheets" ColumnName="name" />
    <asp:ListView ID="lvSpreadsheets" runat="server" DataSourceID="dsSpreadsheets" DataKeyNames="Id"
        InsertItemPosition="None" OnSelectedIndexChanged="lvSpreadsheets_SelectedIndexChanged">
        <ItemTemplate>
        <tr  style="" runat="server" class="it">
            <td class="name"><asp:Label ID="Label1" runat="server" Text='<%# Eval("Nazwa") %>' /></td>
            <td><asp:Label ID="Label2" runat="server" Text='<%# Eval("Opis") %>' /></td>
            <td><asp:Label ID="Label3" runat="server" Text='<%# Eval("Genre") %>' /></td>
            <td id="tdControl" class="control" runat="server">
                <asp:Button ID="SelectButton" runat="server" CommandName="Select" Text="Wybierz" />
                <%--<asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />--%>
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
                <asp:TextBox ID="lbZastepowany" runat="server" Text='<%# Bind("Nazwa") %>' />
            </td>
            <td>
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Opis") %>' />
            </td>
            <td>
                <asp:DropDownList ID="ddlGenre" runat="server" DataSourceID="dsGenre" DataValueField="Id" DataTextField="Name" />
                <asp:SqlDataSource ID="dsGenre" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select Id, Nazwa as Name from scSlowniki where Typ = 'ARK' and Aktywny = 1" />              
            </td>
            <td class="check">
                <asp:CheckBox ID="cbActive" runat="server" Checked='<%# Bind("Aktywny") %>' />
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
                <asp:TextBox ID="lbZastepowany" runat="server" Text='<%# Bind("Nazwa") %>' />
            </td>
            <td>
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Opis") %>' />
            </td>
            <td>
                <asp:DropDownList ID="ddlGenre" runat="server" DataSourceID="dsGenre" DataValueField="Id" DataTextField="Name" SelectedValue='<%# Eval("Rodzaj") %>' />
                <asp:SqlDataSource ID="dsGenre" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select Id, Nazwa as Name from scSlowniki where Typ = 'ARK' and Aktywny = 1" />
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
     <tr style="" runat="server" class="sit">
            <td><asp:Label ID="Label1" runat="server" Text='<%# Eval("Nazwa") %>' /></td>
            <td><asp:Label ID="Label2" runat="server" Text='<%# Eval("Opis") %>' /></td>
            <td>
                <asp:Label ID="Label3" runat="server" Text='<%# Eval("Genre") %>' />
            </td>
            <td id="tdControl" class="control" runat="server">
            
                <asp:Button ID="SelectButton" runat="server" CommandName="Select" Text="Wybierz" />
                <%--<asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />--%>
                <%--<asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />--%>
            </td>
        </tr>
    </SelectedItemTemplate>
    <LayoutTemplate>
        <table id="Table1" runat="server" class="ListView1 tbZastepstwa hoverline">
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" class="lvSpreadsheets">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" Text="Nazwa" CommandName="Sort" CommandArgument="Nazwa" /></th>
                            <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" Text="Opis" CommandName="Sort" CommandArgument="Opis" /></th>
                            <th id="Th3" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" Text="Rodzaj" CommandName="Sort" CommandArgument="Rodzaj" /></th>
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
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="dsSpreadsheets" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="select ta.*, s.Nazwa as Genre
from scTypyArkuszy ta
left join scSlowniki s on s.Id = ta.Rodzaj
where ta.Aktywny = 1
order by Nazwa
"
    DeleteCommand="DELETE FROM scTypyArkuszy WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO scTypyArkuszy (Nazwa, Opis, Rodzaj, Aktywny) VALUES (@Nazwa, @Opis, @Rodzaj, @Aktywny)" 
    UpdateCommand="update scTypyArkuszy SET Nazwa = @Nazwa, Opis = @Opis, Rodzaj = @Rodzaj, Aktywny = @Aktywny WHERE [Id] = @Id">
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