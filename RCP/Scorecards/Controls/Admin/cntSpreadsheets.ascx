<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSpreadsheets.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Admin.cntSpreadsheets" %>

<%@ Register Src="~/Scorecards/Controls/Admin/cntSearch.ascx" TagPrefix="leet" TagName="Search" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<div id="ctSpreadsheets" runat="server" class="cntSpreadsheets" >
    <leet:Search ID="Search" runat="server" TableName="lvSpreadsheets" ColumnName="name" />

     <asp:ListView ID="lvSpreadsheets" runat="server" DataSourceID="dsSpreadsheets" DataKeyNames="Id" InsertItemPosition="LastItem" OnItemInserting="lvSpreadsheets_ItemInserting" OnItemUpdating="lvSpreadsheets_ItemUpdating" OnItemCreated="lvSpreadsheets_ItemCreated" OnItemDataBound="lvSpreadsheets_ItemDataBound">
    <ItemTemplate>
        <tr id="Tr1" style="" runat="server" class="it">
            <td class="name"><asp:Label ID="Label1" runat="server" Text='<%# Eval("Nazwa") %>' /></td>
            <td><asp:Label ID="Label2" runat="server" Text='<%# Eval("Opis") %>' /></td>
            <td><asp:Label ID="Label3" runat="server" Text='<%# Eval("Genre") %>' /></td>
            <td><asp:Label ID="Label4" runat="server" Text='<%# Eval("QCName") %>' /></td>
<%--            <td><asp:Label ID="Label5" runat="server" Text='<%# Eval("ProdName") %>' /></td>--%>
            <td class="check"><asp:CheckBox ID="cbRequest" runat="server" Checked='<%# Eval("Wniosek") %>' Enabled="false" ToolTip="Generuj wniosek premiowy" /></td>
            <td class="check"><asp:CheckBox ID="cbActive" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" /></td>
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
                <asp:TextBox ID="tbNazwa" runat="server" Text='<%# Bind("Nazwa") %>' MaxLength="200" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbNazwa" ErrorMessage="" ValidationGroup="ivg"></asp:RequiredFieldValidator>
            </td>
            <td style="min-width: 200px;">
                <asp:TextBox ID="tbOpis" runat="server" Text='<%# Bind("Opis") %>' MaxLength="500" />
            </td>
            <td>
                <asp:DropDownList ID="ddlGenre" runat="server" DataSourceID="dsGenre" DataValueField="Id" DataTextField="Name" />
                <asp:SqlDataSource ID="dsGenre" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select null as Id, 'wybierz ...' as Name, 0 as Sort union all select Id, Nazwa as Name, 1 as Sort from scSlowniki where Typ = 'ARK' and Aktywny = 1 order by Sort" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="ddlGenre" ErrorMessage="" ValidationGroup="ivg"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:DropDownList ID="ddlQC" runat="server" DataSourceID="dsQC" DataValueField="Id" DataTextField="Name" />
                <asp:SqlDataSource ID="dsQC" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select null as Id, 'wybierz ...' as Name, 0 as Sort union all select Id, Nazwa as Name, 1 as Sort from scSlowniki where Typ = 'QC' and Aktywny = 1 order by Sort" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="ddlQC" ErrorMessage="" ValidationGroup="ivg"></asp:RequiredFieldValidator>
            </td>
 <%--           <td>
                <asp:DropDownList ID="ddlProd" runat="server" DataSourceID="dsProd" DataValueField="Id" DataTextField="Name" />
                <asp:SqlDataSource ID="dsProd" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select null as Id, 'Wybierz...' as Name, 0 as Sort union all select Id, Nazwa as Name, 1 as Sort from scSlowniki where Typ = 'PROD' and Aktywny = 1 order by Sort" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="ddlProd" ErrorMessage="" ValidationGroup="ivg"></asp:RequiredFieldValidator>
            </td>--%>
            <td class="check">
                <asp:CheckBox ID="cbRequest" runat="server" Checked='<%# Bind("Wniosek") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="cbActive" runat="server" Checked='<%# Bind("Aktywny") %>' />
            </td>
            <td id="tdControl" class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Zapisz" />
                <%--<asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Anuluj" />--%>
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td>
                <asp:TextBox ID="tbNazwa" runat="server" Text='<%# Bind("Nazwa") %>' MaxLength="200" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbNazwa" ErrorMessage="" ValidationGroup="evg"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Opis") %>' MaxLength="500" />
            </td>
            <td>
                <asp:DropDownList ID="ddlGenre" runat="server" DataSourceID="dsGenre" DataValueField="Id" DataTextField="Name" />
                <asp:SqlDataSource ID="dsGenre" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select null as Id, 'wybierz ...' as Name, 0 as Sort union all select Id, Nazwa as Name, 1 as Sort from scSlowniki where Typ = 'ARK' and Aktywny = 1 order by Sort" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="ddlGenre" ErrorMessage="" ValidationGroup="evg"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:DropDownList ID="ddlQC" runat="server" DataSourceID="dsQC" DataValueField="Id" DataTextField="Name" />
                <asp:SqlDataSource ID="dsQC" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select null as Id, 'wybierz ...' as Name, 0 as Sort union all select Id, Nazwa as Name, 1 as Sort from scSlowniki where Typ = 'QC' and Aktywny = 1 order by Sort" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="ddlQC" ErrorMessage="" ValidationGroup="evg"></asp:RequiredFieldValidator>
            </td>
<%--            <td>
                <asp:DropDownList ID="ddlProd" runat="server" DataSourceID="dsProd" DataValueField="Id" DataTextField="Name" />
                <asp:SqlDataSource ID="dsProd" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select null as Id, 'Wybierz...' as Name, 0 as Sort union all select Id, Nazwa as Name, 1 as Sort from scSlowniki where Typ = 'PROD' and Aktywny = 1 order by Sort" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="ddlProd" ErrorMessage="" ValidationGroup="evg"></asp:RequiredFieldValidator>
            </td>--%>
            <td class="check">
                <asp:CheckBox ID="cbRequest" runat="server" Checked='<%# Bind("Wniosek") %>' />
            </td>
            <td class="check">
                <asp:CheckBox ID="cbActive" runat="server" Checked='<%# Bind("Aktywny") %>' />
            </td>
            
            <td id="tdControl" class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table id="Table1" runat="server" class="ListView1 tbZastepstwa hoverline">
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" class="lvSpreadsheets">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" Text="Nazwa" CommandName="Sort" CommandArgument="Nazwa" /></th>
                            <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" Text="Opis" CommandName="Sort" CommandArgument="Opis" /></th>
                            <th id="Th3" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" Text="Rodzaj" CommandName="Sort" CommandArgument="Rodzaj" /></th>
                            <th id="Th4" runat="server"><asp:LinkButton ID="LinkButton4" runat="server" Text="QC" CommandName="Sort" CommandArgument="QCName" /></th>
<%--                            <th id="Th5" runat="server"><asp:LinkButton ID="LinkButton5" runat="server" Text="Produktywność" CommandName="Sort" CommandArgument="ProdName" /></th>--%>
                            <th id="Th6" runat="server"><asp:LinkButton ID="LinkButton6" runat="server" Text="Wniosek" CommandName="Sort" CommandArgument="Wniosek" ToolTip="Generuj wniosek premiowy" /></th>
                            <th id="Th7" runat="server"><asp:LinkButton ID="LinkButton7" runat="server" Text="Aktywny" CommandName="Sort" CommandArgument="Aktywny" /></th>
                            <th id="thControl" class="control" runat="server">
                            </th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trPager" runat="server" >
            </tr>
           <%-- <tr id="tr3" runat="server" >
                <td class="bottom_buttons">
                    <asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord" Text="Dodaj zastępstwo" />
                </td>
            </tr>--%>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="dsSpreadsheets" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
juan: select ta.*, s.Nazwa as Genre, ss.Nazwa as QCName, sss.Nazwa as ProdName
from scTypyArkuszy ta
left join scSlowniki s on s.Id = ta.Rodzaj
left join scSlowniki ss on ss.Id = ta.QC
left join scSlowniki sss on sss.Id = ta.Produktywnosc
order by Nazwa
"
    DeleteCommand="DELETE FROM scTypyArkuszy WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO scTypyArkuszy (Nazwa, Opis, Rodzaj, Aktywny, QC, Wniosek, Produktywnosc) VALUES (@Nazwa, @Opis, @Rodzaj, @Aktywny, @QC, @Wniosek, -1)" 
    UpdateCommand="update scTypyArkuszy SET Nazwa = @Nazwa, Opis = @Opis, Rodzaj = @Rodzaj, Aktywny = @Aktywny, QC = @QC, Wniosek = @Wniosek WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Rodzaj" Type="Int32" />
        <asp:Parameter Name="Wniosek" Type="Boolean" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
        <asp:Parameter Name="QC" Type="Int32" />
        <asp:Parameter Name="Produktywnosc" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Rodzaj" Type="Int32" />
        <asp:Parameter Name="Wniosek" Type="Boolean" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
        <asp:Parameter Name="QC" Type="Int32" />
        <asp:Parameter Name="Produktywnosc" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>



</div>