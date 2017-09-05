<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Splity.ascx.cs" Inherits="HRRcp.Controls.Splity" %>
<%@ Register src="SplitWsp.ascx" tagname="SplitWsp" tagprefix="uc3" %>
<%@ Register src="DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>
<%@ Register src="LetterDataPager.ascx" tagname="LetterDataPager" tagprefix="uc1" %>

<asp:HiddenField ID="hidGrSplitu" runat="server" />

<asp:ListView ID="lvSplity" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" InsertItemPosition="LastItem">
    <ItemTemplate>
        <tr class="it">
            <td class="select">
                <asp:Button ID="SelectButton" runat="server" CommandName="Select" Text="Select" />
            </td>
            <td>
                <asp:Label ID="GrSplituLabel" runat="server" Text='<%# Eval("GrSplitu") %>' />
            </td>
            <td>
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="DataOdLabel" runat="server" Text='<%# Eval("DataOd", "{0:d}") %>' />
            </td>
            <td>
                <asp:Label ID="DataDoLabel" runat="server" Text='<%# Eval("DataDo", "{0:d}") %>' />
            </td>
            <td>
                <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
            </td>
            <td class="control">
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
            </td>
        </tr>
    </ItemTemplate>
    <SelectedItemTemplate>
        <tr class="sit">
            <td></td>
            <td>
                <asp:Label ID="GrSplituLabel" runat="server" Text='<%# Eval("GrSplitu") %>' />
            </td>
            <td>
                <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Nazwa") %>' />
            </td>
            <td>
                <asp:Label ID="DataOdLabel" runat="server" Text='<%# Eval("DataOd") %>' />
            </td>
            <td>
                <asp:Label ID="DataDoLabel" runat="server" Text='<%# Eval("DataDo") %>' />
            </td>
            <td>
                <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
            </td>
            <td class="control">
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
            </td>
        </tr>
        <tr class="sit">
            <td></td>
            <td colspan="5">
                Split:<br />
                <uc3:SplitWsp ID="cntSplitWsp" runat="server" IdSplitu='<%# Eval("Id") %>' />                
            </td>
            <td></td>
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
        <tr class="iit">
            <td>&nbsp;</td>
            <td>
                <asp:TextBox ID="GrSplituTextBox" runat="server" Text='<%# Bind("GrSplitu") %>' />
            </td>
            <td>
                <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataOd") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit2" runat="server" Date='<%# Bind("DataDo") %>' />
            </td>
            <td>
                <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td>&nbsp;</td>
            <td>
                <asp:TextBox ID="GrSplituTextBox" runat="server" Text='<%# Bind("GrSplitu") %>' />
            </td>
            <td>
                <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Nazwa") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataOd") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit2" runat="server" Date='<%# Bind("DataDo") %>' />
            </td>
            <td>
                <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
        </tr>
        <tr class="eit">
            <td></td>
            <td colspan="5">
                Split:<br />
                <uc3:SplitWsp ID="cntSplitWsp" runat="server" IdSplitu='<%# Eval("Id") %>' />                
            </td>
            <td></td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1 hoverline">
            <tr runat="server">
                <td runat="server" colspan="2">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" class="tbSplity">
                        <tr runat="server" style="">
                            <th runat="server" class="control1">
                            </th>
                            <th runat="server">
                                GrSplitu</th>
                            <th runat="server">
                                Nazwa</th>
                            <th runat="server">
                                DataOd</th>
                            <th runat="server">
                                DataDo</th>
                            <th runat="server">
                                Typ</th>
                            <th id="Th1" runat="server" class="control">
                            </th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="pager">
                <td class="left">
                    <uc1:LetterDataPager ID="LetterDataPager1" runat="server" />
                </td>
                <td class="right">
                    <h3>Pokaż na stronie:&nbsp;&nbsp;&nbsp;</h3>
                    <asp:DropDownList ID="ddlLines" runat="server" AutoPostBack="true" 
                        OnChange="javascript:showAjaxProgress();"
                        OnSelectedIndexChanged="ddlLines_SelectedIndexChanged">
                        <asp:ListItem Text="15" Value="15" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                        <asp:ListItem Text="WSZYSTKO" Value="all"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="pager">
                <td class="left">
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="20">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                </td>
                <td class="right">
                    <span class="count">Ilość rekordów:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConflictDetection="CompareAllValues" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [Splity] WHERE [Id] = @original_Id AND [GrSplitu] = @original_GrSplitu AND (([Nazwa] = @original_Nazwa) OR ([Nazwa] IS NULL AND @original_Nazwa IS NULL)) AND [DataOd] = @original_DataOd AND (([DataDo] = @original_DataDo) OR ([DataDo] IS NULL AND @original_DataDo IS NULL)) AND [Typ] = @original_Typ" 
    InsertCommand="INSERT INTO [Splity] ([GrSplitu], [Nazwa], [DataOd], [DataDo], [Typ]) VALUES (@GrSplitu, @Nazwa, @DataOd, @DataDo, @Typ)" 
    OldValuesParameterFormatString="original_{0}" 
    SelectCommand="SELECT *, case when GrSplitu = 9999 then 1 else 2 end as Sort FROM [Splity] order by Sort, DataOd desc, Nazwa" 
    UpdateCommand="UPDATE [Splity] SET [GrSplitu] = @GrSplitu, [Nazwa] = @Nazwa, [DataOd] = @DataOd, [DataDo] = @DataDo, [Typ] = @Typ WHERE [Id] = @original_Id AND [GrSplitu] = @original_GrSplitu AND (([Nazwa] = @original_Nazwa) OR ([Nazwa] IS NULL AND @original_Nazwa IS NULL)) AND [DataOd] = @original_DataOd AND (([DataDo] = @original_DataDo) OR ([DataDo] IS NULL AND @original_DataDo IS NULL)) AND [Typ] = @original_Typ">
    <DeleteParameters>
        <asp:Parameter Name="original_Id" Type="Int32" />
        <asp:Parameter Name="original_GrSplitu" Type="Int32" />
        <asp:Parameter Name="original_Nazwa" Type="String" />
        <asp:Parameter Name="original_DataOd" Type="DateTime" />
        <asp:Parameter Name="original_DataDo" Type="DateTime" />
        <asp:Parameter Name="original_Typ" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="GrSplitu" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="DataOd" Type="DateTime" />
        <asp:Parameter Name="DataDo" Type="DateTime" />
        <asp:Parameter Name="Typ" Type="Int32" />
        <asp:Parameter Name="original_Id" Type="Int32" />
        <asp:Parameter Name="original_GrSplitu" Type="Int32" />
        <asp:Parameter Name="original_Nazwa" Type="String" />
        <asp:Parameter Name="original_DataOd" Type="DateTime" />
        <asp:Parameter Name="original_DataDo" Type="DateTime" />
        <asp:Parameter Name="original_Typ" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="GrSplitu" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="DataOd" Type="DateTime" />
        <asp:Parameter Name="DataDo" Type="DateTime" />
        <asp:Parameter Name="Typ" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>

