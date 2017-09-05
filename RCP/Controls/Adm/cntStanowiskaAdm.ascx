<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntStanowiskaAdm.ascx.cs" Inherits="HRRcp.Controls.Adm.cntStanowiskaAdm" %>

<div id="paStanowiskaAdm" runat="server" class="cntStanowiskaAdm">
    <div class="filter">
        <span>Filtruj:</span> 
        <asp:DropDownList ID="ddlDzial" runat="server" DataSourceID="SqlDataSource3" DataTextField="Dzial" DataValueField="IdDzialu" AutoPostBack="true">
        </asp:DropDownList>
    </div>
    <asp:ListView ID="lvStanowiska" runat="server" DataSourceID="SqlDataSource1" 
        DataKeyNames="Id" InsertItemPosition="LastItem" 
        onitemdeleting="lvStanowiska_ItemDeleting" 
        onitemcreated="lvStanowiska_ItemCreated" 
        onitemdatabound="lvStanowiska_ItemDataBound">
        <ItemTemplate>
            <tr style="" class="it">
                <td>
                    <asp:Label ID="DzialLabel" runat="server" Text='<%# Eval("Dzial") %>' />
                </td>
                <td>
                    <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
                </td>
                <td class="check">
                    <asp:CheckBox ID="AktywneCheckBox" runat="server" Checked='<%# Eval("Aktywne") %>' Enabled="false" />
                </td>
                <td class="control">
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                </td>
            </tr>
        </ItemTemplate>
        <SelectedItemTemplate>
            <tr style="" class="sit">
                <td>
                    <asp:Label ID="DzialLabel" runat="server" Text='<%# Eval("Dzial") %>' />
                </td>
                <td>
                    <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
                </td>
                <td class="check">
                    <asp:CheckBox ID="AktywneCheckBox" runat="server" Checked='<%# Eval("Aktywne") %>' Enabled="false" />
                </td>
                <td class="control">
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
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
            <tr style="" class="iit">
                <td>
                    <asp:DropDownList ID="ddlDzial" runat="server" 
                        DataSourceID="SqlDataSource2"
                        DataTextField="Dzial"
                        DataValueField="IdDzialu">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="ivg" Enabled="false" 
                        ControlToValidate="ddlDzial" 
                        ErrorMessage="Błąd" >
                    </asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="ivg" 
                        ControlToValidate="NazwaTextBox" 
                        ErrorMessage="Błąd" >
                    </asp:RequiredFieldValidator>
                </td>
                <td class="check">
                    <asp:CheckBox ID="AktywneCheckBox" runat="server" Checked='<%# Bind("Aktywne") %>' />
                </td>
                <td class="control">
                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" ValidationGroup="ivg"/>
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
                </td>
            </tr>
        </InsertItemTemplate>
        <EditItemTemplate>
            <tr style="" class="eit">
                <td>
                    <%--SelectedValue='<%# Bind("IdDzialu") %>'--%>
                    <asp:DropDownList ID="ddlDzial" runat="server"                         
                        DataSourceID="SqlDataSource2"
                        DataTextField="Dzial"
                        DataValueField="IdDzialu">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="evg" Enabled="false"
                        ControlToValidate="ddlDzial" 
                        ErrorMessage="Błąd" >
                    </asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="evg" 
                        ControlToValidate="NazwaTextBox" 
                        ErrorMessage="Błąd" >
                    </asp:RequiredFieldValidator>
                </td>
                <td class="check">
                    <asp:CheckBox ID="AktywneCheckBox" runat="server" Checked='<%# Bind("Aktywne") %>' />
                </td>
                <td class="control">
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" ValidationGroup="evg"/>
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
                </td>
            </tr>
        </EditItemTemplate>
        <LayoutTemplate>
            <table runat="server" class="tbStanowiskaAdm">
                <tr runat="server">
                    <td runat="server">
                        <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr id="Tr2" runat="server" style="">
                                <th id="Th4" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Dzial"   Text="Dział"            ToolTip="Sortuj" /></th>
                                <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="Nazwa"   Text="Nazwa stanowiska" ToolTip="Sortuj" /></th>
                                <th id="Th3" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Aktywne" Text="Aktywne"          ToolTip="Sortuj" /></th>
                                <th id="Th1" class="control" runat="server">
                                </th>
                            </tr>
                            <tr ID="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="pager">
                    <td class="left">
                        <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                                <asp:NumericPagerField ButtonType="Link" />
                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                            </Fields>
                        </asp:DataPager>
                    </td>
                    <td class="right">
                        <span class="count">Pokaż na stronie:&nbsp;&nbsp;&nbsp;</span>
                        <asp:DropDownList ID="ddlLines" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr class="pager">
                    <td class="left">
                    </td>
                    <td class="right">
                        <span class="count">Ilość rekordów:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
    </asp:ListView>
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    DeleteCommand="DELETE FROM [Stanowiska] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Stanowiska] ([IdDzialu], [Nazwa], [Aktywne]) VALUES (@IdDzialu, @Nazwa, @Aktywne)" 
    SelectCommand="
select S.Id, S.IdDzialu, 
--case when D.Status &lt; 0 then '* ' + D.Nazwa else D.Nazwa end as Dzial, 
D.Nazwa as Dzial, 
S.Nazwa, S.Aktywne 
from Stanowiska S
left outer join Dzialy D on D.Id = S.IdDzialu
where (@dzial is null or S.IdDzialu = @dzial)
order by 
case when ISNULL(D.Status,0) &lt; 0 then 1 else 0 end,
D.Nazwa, 
S.Aktywne desc,
S.Nazwa
" 
    UpdateCommand="UPDATE [Stanowiska] SET [IdDzialu] = @IdDzialu, [Nazwa] = @Nazwa, [Aktywne] = @Aktywne WHERE [Id] = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlDzial" Name="dzial" PropertyName="SelectedValue" Type="Int32"/>
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="IdDzialu" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Aktywne" Type="Boolean" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="IdDzialu" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Aktywne" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
SELECT Id as IdDzialu, 
case when Status &lt; 0 then '* ' + Nazwa else Nazwa end as Dzial, 2 as Sort,  
case when Status &lt; 0 then 0 else 1 end as Aktywny
FROM [Dzialy] 
union
select null as IdDzialu, 'wybierz ...' as Dzial, 1 as Sort, 1 as Aktywny
ORDER BY Sort, Aktywny desc, Dzial
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
SELECT Id as IdDzialu, 
case when Status &lt; 0 then '* ' + Nazwa else Nazwa end as Dzial, 2 as Sort,  
case when Status &lt; 0 then 0 else 1 end as Aktywny
FROM [Dzialy] 
union
select null as IdDzialu, 'Pokaż wszystkie działy' as Dzial, 1 as Sort, 1 as Aktywny
ORDER BY Sort, Aktywny desc, Dzial
    ">
</asp:SqlDataSource>


