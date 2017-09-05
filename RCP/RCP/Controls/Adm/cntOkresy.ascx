<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntOkresy.ascx.cs" Inherits="HRRcp.RCP.Controls.Adm.cntOkresy" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagName="DateEdit" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>

<div id="ctAdmOkresy" runat="server" class="cntAdmOkresy">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="top_buttons">
                <asp:Button ID="btnAddOkresModal" runat="server" OnClick="btnAddOkresModal_Click" CssClass="btn btn-success" Text="Otwórz okres rozliczeniowy" />
            </div>
            <asp:ListView ID="lvOkresy" runat="server" DataKeyNames="Id"
                DataSourceID="SqlDataSource1" InsertItemPosition="None"
                OnItemCreated="lvOkresy_ItemCreated" OnItemDataBound="lvOkresy_ItemDataBound"
                OnItemInserting="lvOkresy_ItemInserting"
                OnItemUpdating="lvOkresy_ItemUpdating"
                OnItemDeleted="lvOkresy_ItemDeleted"
                OnItemDeleting="lvOkresy_ItemDeleting">
                <ItemTemplate>
                    <tr class="it">
                        <td class="data">
                            <asp:Label ID="DataOdLabel" runat="server" Text='<%# Eval("DataOd", "{0:d}") %>' />
                        </td>
                        <td class="data">
                            <asp:Label ID="DataDoLabel" runat="server" Text='<%# Eval("DataDo", "{0:d}") %>' />
                        </td>
                        <td class="data">
                            <asp:Label ID="LockedToLabel" runat="server" Text='<%# Eval("DataBlokady", "{0:d}") %>' />
                        </td>
                        <td class="data">
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("DataNaliczenia", "{0:d}") %>' />
                        </td>
                        <td class="num">
                            <asp:Label ID="StawkaNocnaLabel" runat="server" Text='<%# Eval("StawkaNocna", "{0:0.0000}") %>' />
                        </td>
                        <td>
                            <asp:Label ID="StatusLabel" runat="server" Text='<%# GetStatus(Eval("Status")) %>' />
                        </td>
                        <td>
                            <asp:Label ID="ZamknalNILabel" runat="server" Text='<%# Eval("ZamknalNI") %>' />
                        </td>
                        <td class="data">
                            <asp:Label ID="DataZamknieciaLabel" runat="server" Text='<%# Eval("DataZamkniecia", "{0:d}") %>' />
                        </td>
                        <td class="control">
                            <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
                            <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" /><br />
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <table runat="server" style="">
                        <tr>
                            <td>Brak danych
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <InsertItemTemplate>
                    <tr class="iit">
                        <td class="data">
                            <uc1:DateEdit ID="deDataOd" runat="server" Date='<%# Bind("DataOd") %>' />
                        </td>
                        <td class="data">
                            <uc1:DateEdit ID="deDataDo" runat="server" Date='<%# Bind("DataDo") %>' />
                        </td>
                        <td class="data">
                            <uc1:DateEdit ID="deDataBlokady" runat="server" Date='<%# Bind("DataBlokady") %>' Visible="false" />
                        </td>
                        <td class="num">
                            <asp:TextBox ID="StawkaNocnaTextBox" runat="server" Text='<%# Bind("StawkaNocna", "{0:0.0000}") %>' />
                            <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" TargetControlID="StawkaNocnaTextBox"
                                FilterType="Custom"
                                ValidChars="0123456789,." />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlZamknal" runat="server" Visible="false"
                                DataSourceID="SqlDataSource2"
                                DataTextField="Admin"
                                DataValueField="Id">
                            </asp:DropDownList>
                        </td>
                        <td class="data">
                            <uc1:DateEdit ID="deDataZamkniecia" runat="server" Date='<%# Bind("DataZamkniecia") %>' Visible="false" />
                        </td>
                        <td class="control">
                            <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Dodaj" />
                            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Czyść" />
                        </td>
                    </tr>
                </InsertItemTemplate>
                <LayoutTemplate>
                    <table runat="server" class="xListView1 tbAdmOkresy hoverline">
                        <tr runat="server">
                            <td runat="server">
                                <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                                    <tr runat="server" style="">
                                        <th runat="server">Data od</th>
                                        <th runat="server">Data do</th>
                                        <th runat="server">Akceptacja tygodniowa&nbsp;do</th>
                                        <th runat="server">Data naliczenia</th>
                                        <th runat="server">Stawka nocna&nbsp;[zł]</th>
                                        <th runat="server">Status</th>
                                        <th runat="server">Zamknął</th>
                                        <th runat="server">Data zamknięcia</th>
                                        <th runat="server" class="control"></th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr class="pager">
                            <td>
                                <asp:DataPager ID="DataPager1" runat="server" PageSize="6">
                                    <Fields>
                                        <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                                        <asp:NumericPagerField ButtonType="Link" />
                                        <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                                    </Fields>
                                </asp:DataPager>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
                <EditItemTemplate>
                    <tr class="eit">
                        <td class="data">
                            <uc1:DateEdit ID="deDataOd" runat="server" Date='<%# Bind("DataOd") %>' />
                        </td>
                        <td class="data">
                            <uc1:DateEdit ID="deDataDo" runat="server" Date='<%# Bind("DataDo") %>' />
                        </td>
                        <td class="data">
                            <uc1:DateEdit ID="deDataBlokady" runat="server" Date='<%# Bind("DataBlokady") %>' />

                            <%--
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("DataBlokady") %>' />
                <asp:CustomValidator ID="vlDataBlokady" runat="server" 
                    ControlToValidate="TextBox1" 
                    onservervalidate="vlDataBlokady_ServerValidate"
                    CssClass="error"
                    ErrorMessage="Błąd!"
                    Display="Dynamic"
                    SetFocusOnError="true"
                    ValidationGroup="vge"
                    ></asp:CustomValidator>
                            --%>  
                        </td>
                        <td class="data">
                            <uc1:DateEdit ID="deDataNaliczenia" runat="server" Date='<%# Bind("DataNaliczenia") %>' />
                        </td>
                        <td class="num">
                            <asp:TextBox ID="StawkaNocnaTextBox" runat="server" Text='<%# Bind("StawkaNocna", "{0:0.0000}") %>' />
                            <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" TargetControlID="StawkaNocnaTextBox"
                                FilterType="Custom"
                                ValidChars="0123456789,." />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlZamknal" runat="server" Visible="false"
                                DataSourceID="SqlDataSource2"
                                DataTextField="Admin"
                                DataValueField="Id">
                            </asp:DropDownList>
                        </td>
                        <td class="data">
                            <uc1:DateEdit ID="deDataZamkniecia" runat="server" Date='<%# Bind("DataZamkniecia") %>' Visible="false" />
                        </td>
                        <td class="control">
                            <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vge" />
                            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                        </td>
                    </tr>
                </EditItemTemplate>
            </asp:ListView>
        </ContentTemplate>
    </asp:UpdatePanel>

    <uc1:cntModal runat="server" ID="cntModal" Title="Dodaj okres">
        <ContentTemplate>
            <div class="form-group">
                <label>Data od:</label>
                <uc1:DateEdit ID="deFrom" runat="server" />
                <label>do:</label>
                <uc1:DateEdit ID="deTo" runat="server" />
            </div>
            <div class="form-group">
                <label>Data naliczenia:</label>
                <uc1:DateEdit ID="deCalc" runat="server" />
            </div>
            <div class="form-group form-inline">
                <label>Stawka nocna [zł]:</label>
                <asp:TextBox ID="tbStawkaNoc" runat="server" CssClass="form-control" MaxLength="5" />
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btnAddOkres" runat="server" CssClass="btn btn-success" OnClick="btnAddOkres_Click" Text="Zapisz" />
        </FooterTemplate>
    </uc1:cntModal>
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    DeleteCommand="DELETE FROM [OkresyRozl] WHERE [Id] = @Id"
    InsertCommand="INSERT INTO [OkresyRozl] ([DataOd], [DataDo], [DataBlokady], [StawkaNocna], [Status], [Zamknal], [DataZamkniecia]) VALUES (@DataOd, @DataDo, @DataBlokady, @StawkaNocna, @Status, @Zamknal, @DataZamkniecia)"
    SelectCommand="SELECT O.*, P.Nazwisko + ' ' + P.Imie as ZamknalNI FROM OkresyRozl O left outer join Pracownicy P on O.Zamknal=P.Id ORDER BY [DataOd] desc"
    UpdateCommand="UPDATE [OkresyRozl] SET [DataOd] = @DataOd, [DataDo] = @DataDo, [StawkaNocna] = @StawkaNocna, [Status] = @Status, [Zamknal] = @Zamknal, [DataZamkniecia] = @DataZamkniecia, DataBlokady = @DataBlokady, DataNaliczenia = @DataNaliczenia WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="DataOd" Type="DateTime" />
        <asp:Parameter Name="DataDo" Type="DateTime" />
        <asp:Parameter Name="DataBlokady" Type="DateTime" />
        <asp:Parameter Name="StawkaNocna" Type="Double" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Zamknal" Type="Int32" />
        <asp:Parameter Name="DataZamkniecia" Type="DateTime" />
        <asp:Parameter Name="DataNaliczenia" Type="DateTime" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="DataOd" Type="DateTime" />
        <asp:Parameter Name="DataDo" Type="DateTime" />
        <asp:Parameter Name="DataBlokady" Type="DateTime" />
        <asp:Parameter Name="StawkaNocna" Type="Double" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Zamknal" Type="Int32" />
        <asp:Parameter Name="DataZamkniecia" Type="DateTime" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource2" runat="server"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="SELECT 1 as Sort, Id, Nazwisko + ' ' + Imie as Admin FROM Pracownicy where Admin=1
                   union 
                   select 0 as Sort, null as Id, 'wybierz ...' as Admin
                   ORDER BY Sort, Admin">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsInsert" runat="server" SelectCommand="insert into OkresyRozl (DataOd, DataDo, DataNaliczenia, StawkaNocna, Status) values ({0}, {1}, {2}, {3}, 0)" />
<asp:SqlDataSource ID="dsNewOkres" runat="server" SelectCommand="select top 1 dbo.bom(dateadd(month, 1, DataOd)) DateFrom, dbo.eom(dateadd(month, 1, DataOd)) DateTo from OkresyRozl order by DataOd desc" />    
