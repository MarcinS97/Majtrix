<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntLinie.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Adm.Mapowanie.cntLinie" %>

<%@ Register Src="~/Scorecards/Controls/Admin/cntSearch.ascx" TagPrefix="leet" TagName="Search" %>


<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="ctTasks" runat="server" class="cntTasks" style="margin-top: 16px;">

            <div class="search-box">
                <asp:TextBox ID="tbSearch" runat="server" CssClass="form-control input-sm" Style="display: inline-block; min-width: 260px; width: auto;" placeholder="Linia" />
                <asp:Button ID="btnSerach" runat="server" Text="Wyszukaj" CssClass="btn btn-sm btn-default" />
                <asp:Button ID="btnClear" runat="server" Text="Czyść" CssClass="btn btn-sm btn-default" OnClick="btnClear_Click" />
            </div>

            <asp:ListView ID="lvLinie" runat="server" DataSourceID="dsLinie" DataKeyNames="Id"
                InsertItemPosition="None" >
                <ItemTemplate>
                    <tr id="trSelect" style="" runat="server" class="it">
                        <td class="check">
                            <asp:HiddenField ID="hidId" runat="server" Value='<%# Eval("Id") %>' />
                            <asp:CheckBox ID="cbSelect" runat="server" AutoPostBack="true" OnCheckedChanged="CheckItem" />
                        </td>
                        <td class="name">
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Name") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <table id="Table1" runat="server" class="table0">
                        <tr class="edt">
                            <td>
                                <asp:Label ID="lbNoData" runat="server" Text="Brak danych" /><br />
                                <br />
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <InsertItemTemplate>
                    <tr class="iit">
                        <td>
                        </td>
                        <td>
                            <asp:TextBox ID="lbZastepowany" runat="server" Text='<%# Bind("Name") %>' />
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Czas") %>' />
                        </td>
                        <td>
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
                    <table runat="server" class="xListView1 xtbZastepstwa xhoverline" style="width: 100%;">
                        <tr id="Tr1" runat="server">
                            <td id="Td1" runat="server">
                                <table id="itemPlaceholderContainer" runat="server" class="lvTasks" style="width: 100%;">
                                    <tr id="Tr2" runat="server" style="">
                                        <th><asp:CheckBox ID="cbAll" runat="server" OnCheckedChanged="CheckAll" AutoPostBack="true" /></th>
                                        <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" Text="Nazwa" CommandName="Sort" CommandArgument="Nazwa" /></th>
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
            <asp:SqlDataSource ID="dsLinie" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                SelectCommand="
select *, Nazwa + case when Aktywny = 0 then ' (nieaktywna)' else '' end Name from msLinie
where (@search is null or Nazwa like '%' + @search + '%')
order by Aktywny desc, Nazwa
"
                DeleteCommand="DELETE FROM scCzynnosci WHERE [Id] = @Id" InsertCommand="INSERT INTO scCzynnosci (Nazwa, Czas, IdCC, Aktywny) VALUES (@Nazwa, @Czas, @IdCC, @Aktywny)"
                UpdateCommand="update scCzynnosci SET Nazwa = @Nazwa, Czas = @Czas, IdCC = @IdCC, Aktywny = @Aktywny WHERE [Id] = @Id">
                <SelectParameters>
                    <asp:ControlParameter Name="search" Type="String" ControlID="tbSearch" PropertyName="Text" />
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
