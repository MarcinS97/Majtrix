<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntTasksList.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Admin.cntTasksList" %>

<%@ Register Src="~/Scorecards/Controls/Admin/cntSearch.ascx" TagPrefix="leet" TagName="Search" %>

<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="ctTasks" runat="server" class="cntTasks">
        
            
            <leet:Search ID="Search" runat="server" TableName="lvTasks" ColumnName="name" />
        
            <asp:ListView ID="lvTasks" runat="server" DataSourceID="dsTasks" DataKeyNames="Id"
                InsertItemPosition="None" >
                <ItemTemplate>
                    <tr id="trSelect" style="" runat="server" class="it">
                        <td class="check">
                            <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("Id") %>' />
                            <asp:CheckBox ID="cbSelect" runat="server" AutoPostBack="true" OnCheckedChanged="CheckItem" />
                        </td>
                        <td class="name">
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Nazwa") %>' />
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("Czas") %>' />
                        </td>
                        <td class="control">
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("cc") %>' />
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
                <InsertItemTemplate>
                    <tr class="iit">
                        <td>
                        </td>
                        <td>
                            <asp:TextBox ID="lbZastepowany" runat="server" Text='<%# Bind("Nazwa") %>' />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Czas") %>' />
                        </td>
                        <td>
                            <%--<asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("IdCC") %>' />--%>
                            <asp:DropDownList ID="ddlCC" runat="server" DataSourceID="dsCC" DataValueField="Id"
                                DataTextField="Name" />
                            <asp:SqlDataSource ID="dsCC" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                                SelectCommand="
select Id, cc as Name
from CC
where (GETDATE() between AktywneOd and ISNULL(AktywneDo, '20990909'))
order by cc" />
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
                        </td>
                        <td>
                            <asp:TextBox ID="lbZastepowany" runat="server" Text='<%# Bind("Nazwa") %>' />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Czas") %>' />
                        </td>
                        <td>
                            <%--<asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("IdCC") %>' />--%>
                            <asp:DropDownList ID="ddlCC" runat="server" DataSourceID="dsCC" DataValueField="Id"
                                DataTextField="Name" SelectedValue='<%# Eval("IdCC") %>'>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="dsCC" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                                SelectCommand="
select Id, cc as Name
from CC
where (GETDATE() between AktywneOd and ISNULL(AktywneDo, '20990909'))
order by cc" />
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
                                <table id="itemPlaceholderContainer" runat="server" class="lvTasks">
                                    <tr id="Tr2" runat="server" style="">
                                        <th><asp:CheckBox ID="cbAll" runat="server" OnCheckedChanged="CheckAll" AutoPostBack="true" /></th>
                                        <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" Text="Nazwa" CommandName="Sort" CommandArgument="Nazwa" /></th>
                                        <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" Text="Czas" CommandName="Sort" CommandArgument="Czas" /></th>
                                        <th id="Th3" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" Text="CC" CommandName="Sort" CommandArgument="IdCC" /></th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trPager" runat="server">
<%--                            <td id="Td2" runat="server" class="pager">
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
                        <%-- <tr id="tr3" runat="server" >
                <td class="bottom_buttons">
                    <asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord" Text="Dodaj zastępstwo" />
                </td>
            </tr>--%>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
            <asp:SqlDataSource ID="dsTasks" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                SelectCommand="select c.*, cc.cc
from scCzynnosci c
left join CC cc on cc.Id = c.IdCC
" DeleteCommand="DELETE FROM scCzynnosci WHERE [Id] = @Id" InsertCommand="INSERT INTO scCzynnosci (Nazwa, Czas, IdCC, Aktywny) VALUES (@Nazwa, @Czas, @IdCC, @Aktywny)"
                UpdateCommand="update scCzynnosci SET Nazwa = @Nazwa, Czas = @Czas, IdCC = @IdCC, Aktywny = @Aktywny WHERE [Id] = @Id">
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
