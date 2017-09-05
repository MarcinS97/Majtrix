<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntOkresyTypy.ascx.cs" Inherits="HRRcp.RCP.Controls.Adm.cntOkresyTypy" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>


<div id="ctOkresyTypy" runat="server" class="cntOkresyTypy">
    <asp:ListView ID="lvOkresyTypy" runat="server" DataSourceID="dsOkresyTypy" EnableModelValidation="True" DataKeyNames="Id" InsertItemPosition="LastItem">
        <EditItemTemplate>
            <tr style="">
                <%--<td>
                    <asp:Label Text='<%# Eval("Id") %>' runat="server" ID="IdLabel1" /></td>--%>
                <td>
                    <asp:TextBox Text='<%# Bind("Nazwa") %>' runat="server" ID="NazwaTextBox" CssClass="form-control" MaxLength="200" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="NazwaTextBox" ErrorMessage="Pole wymagane" ValidationGroup="vgUpdate"
                        SetFocusOnError="false" CssClass="error" />
                </td>
                <td>
                    <asp:TextBox Text='<%# Bind("Opis") %>' runat="server" ID="OpisTextBox" CssClass="form-control" MaxLength="500" /></td>
                <td>
                    <asp:TextBox Text='<%# Bind("IloscMiesiecy") %>' runat="server" ID="IloscMiesiecyTextBox" CssClass="form-control" Enabled="false" MaxLength="2" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="IloscMiesiecyTextBox" ErrorMessage="Pole wymagane" ValidationGroup="vgUpdate"
                        SetFocusOnError="false" CssClass="error" />
                        <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" TargetControlID="IloscMiesiecyTextBox"
                        FilterType="Custom"
                        ValidChars="0123456789" />
                </td>
                <td>
                    
                    <uc1:DateEdit runat="server" ID="DateEdit" Date='<%# Bind("StartOd") %>'  />
                    <%--<asp:TextBox Text='<%# Bind("StartOd") %>' runat="server" ID="StartOdTextBox" CssClass="form-control" Enabled="false" />--%></td>
                <td>
                    <asp:CheckBox Checked='<%# Bind("Aktywny") %>' runat="server" ID="AktywnyCheckBox" CssClass="" /></td>
                <td class="control">
                    <asp:Button runat="server" CommandName="Update" Text="Update" ID="UpdateButton" CssClass="btn btn-success" ValidationGroup="vgUpdate" />
                    <asp:Button runat="server" CommandName="Cancel" Text="Cancel" ID="CancelButton" CssClass="btn btn-default" />
                </td>
            </tr>
        </EditItemTemplate>
        <EmptyDataTemplate>
            <table runat="server" style="">
                <tr>
                    <td>No data was returned.</td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <tr style="">
                <%--<td>&nbsp;</td>--%>
                <td>
                    <asp:TextBox Text='<%# Bind("Nazwa") %>' runat="server" ID="NazwaTextBox" CssClass="form-control" MaxLength="200" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="NazwaTextBox" ErrorMessage="Pole wymagane" ValidationGroup="vgInsert"
                        SetFocusOnError="false" CssClass="error" />
                </td>
                <td>
                    <asp:TextBox Text='<%# Bind("Opis") %>' runat="server" ID="OpisTextBox" CssClass="form-control" MaxLength="500" /></td>
                <td>
                    <asp:TextBox Text='<%# Bind("IloscMiesiecy") %>' runat="server" ID="IloscMiesiecyTextBox" CssClass="form-control" MaxLength="2" />
                    <asp:RequiredFieldValidator ID="rqIloscMiesiecy" runat="server" ControlToValidate="IloscMiesiecyTextBox" ErrorMessage="Pole wymagane" ValidationGroup="vgInsert"
                        SetFocusOnError="false" CssClass="error" />
                    <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" TargetControlID="IloscMiesiecyTextBox"
                        FilterType="Custom"
                        ValidChars="0123456789" />
                </td>
                <td>
                    <uc1:DateEdit runat="server" ID="DateEdit" Date='<%# Bind("StartOd") %>' />
                    <%--<asp:TextBox Text='<%# Bind("StartOd") %>' runat="server" ID="StartOdTextBox" CssClass="form-control" />--%></td>
                <td>
                    <asp:CheckBox Checked='<%# Bind("Aktywny") %>' runat="server" ID="AktywnyCheckBox" CssClass="" /></td>
                <td class="control">
                    <asp:Button runat="server" CommandName="Insert" Text="Insert" ID="InsertButton" CssClass="btn btn-success" ValidationGroup="vgInsert" />
                    <asp:Button runat="server" CommandName="Cancel" Text="Clear" ID="CancelButton" CssClass="btn btn-default" />
                </td>
            </tr>
        </InsertItemTemplate>
        <ItemTemplate>
            <tr style="">
                <%-- <td>
                    <asp:Label Text='<%# Eval("Id") %>' runat="server" ID="IdLabel" /></td>--%>
                <td>
                    <asp:Label Text='<%# Eval("Nazwa") %>' runat="server" ID="NazwaLabel" /></td>
                <td>
                    <asp:Label Text='<%# Eval("Opis") %>' runat="server" ID="OpisLabel" /></td>
                <td>
                    <asp:Label Text='<%# Eval("IloscMiesiecy") %>' runat="server" ID="IloscMiesiecyLabel" /></td>
                <td>
                    <asp:Label Text='<%# Eval("Date") %>' runat="server" ID="StartOdLabel" /></td>
                <td>
                    <asp:CheckBox Checked='<%# Eval("Aktywny") %>' runat="server" ID="AktywnyCheckBox" Enabled="false" /></td>
                <td class="control">
                    <asp:Button runat="server" Text="Usuń" ID="btnDeleteConfirm" CssClass="btn btn-danger" OnClick="btnDeleteConfirm_Click" CommandArgument='<%# Eval("Id") %>' />
                    <asp:Button runat="server" CommandName="Edit" Text="Edit" ID="EditButton" CssClass="btn btn-default" />
                </td>
            </tr>
        </ItemTemplate>
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table runat="server" id="itemPlaceholderContainer" style="" border="0">
                            <tr runat="server" style="">
                                <%--<th runat="server">Id</th>--%>
                                <th runat="server">Nazwa</th>
                                <th runat="server">Opis</th>
                                <th runat="server">IloscMiesiecy</th>
                                <th runat="server">StartOd</th>
                                <th runat="server">Aktywny</th>
                                <th runat="server"></th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder"></tr>
                        </table>
                    </td>
                </tr>
                <tr class="pager">
                    <td>
                        <asp:DataPager ID="DataPager1" runat="server" PageSize="20">
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
    </asp:ListView>
    <asp:SqlDataSource runat="server" ID="dsOkresyTypy" ConnectionString='<%$ ConnectionStrings:HRConnectionString %>' 
        DeleteCommand="DELETE FROM [rcpOkresyRozliczenioweTypy] WHERE [Id] = @Id" 
        InsertCommand="INSERT INTO [rcpOkresyRozliczenioweTypy] ([Nazwa], [Opis], [IloscMiesiecy], [StartOd], [Aktywny]) VALUES (@Nazwa, @Opis, @IloscMiesiecy, @StartOd, @Aktywny)" 
        SelectCommand="select *, convert(varchar(10), StartOd, 20) Date from rcpOkresyRozliczenioweTypy" 
        UpdateCommand="UPDATE [rcpOkresyRozliczenioweTypy] SET [Nazwa] = @Nazwa, [Opis] = @Opis, [IloscMiesiecy] = @IloscMiesiecy, [StartOd] = @StartOd, [Aktywny] = @Aktywny WHERE [Id] = @Id">
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32"></asp:Parameter>
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="Nazwa" Type="String"></asp:Parameter>
            <asp:Parameter Name="Opis" Type="String"></asp:Parameter>
            <asp:Parameter Name="IloscMiesiecy" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="StartOd" Type="DateTime"></asp:Parameter>
            <asp:Parameter Name="Aktywny" Type="Boolean"></asp:Parameter>
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Nazwa" Type="String"></asp:Parameter>
            <asp:Parameter Name="Opis" Type="String"></asp:Parameter>
            <asp:Parameter Name="IloscMiesiecy" Type="Int32"></asp:Parameter>
            <asp:Parameter Name="StartOd" Type="DateTime"></asp:Parameter>
            <asp:Parameter Name="Aktywny" Type="Boolean"></asp:Parameter>
            <asp:Parameter Name="Id" Type="Int32"></asp:Parameter>
        </UpdateParameters>
    </asp:SqlDataSource>
</div>

<asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" CssClass="hidden" />

<asp:SqlDataSource ID="dsCheckIfCanDelete" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select case when count(*) > 0 then 0 else 1 end from OkresyRozliczeniowe where Typ = {0}" />

<asp:SqlDataSource ID="dsDelete" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="delete from rcpOkresyRozliczenioweTypy where Id = {0}" />

