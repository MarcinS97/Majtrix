<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntDicCommodity.ascx.cs" Inherits="HRRcp.Controls.Przypisania.cntDicCommodity" %>

<asp:ListView ID="ListView1" runat="server" DataSourceID="SqlDataSource1" DataKeyNames="Id" InsertItemPosition="LastItem">
    <ItemTemplate>
        <tr class="it">
            <td class="commodity">
                <asp:Label ID="CommodityLabel" runat="server" Text='<%# Eval("Commodity") %>' />
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
            <td class="commodity">
                <asp:TextBox ID="CommodityTextBox" runat="server" MaxLength="50" Text='<%# Bind("Commodity") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    Enabled="false"
                    ValidationGroup="vgi" 
                    ControlToValidate="CommodityTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td class="check">
                <asp:CheckBox ID="AktywneCheckBox" runat="server" Checked='<%# Bind("Aktywne") %>' />
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" ValidationGroup="vgi"/>
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
        </tr>
    </InsertItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="tbDicCommodity">
            <tr runat="server">
                <td runat="server" colspan="2">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">
                                Commodity</th>
                            <th runat="server">
                                Aktywne</th>
                            <th id="Th1" runat="server">
                            </th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
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
                <td class="right">
                    <span class="count">Ilość:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
    <EditItemTemplate>
        <tr style="">
            <td class="commodity">
                <asp:TextBox ID="CommodityTextBox" runat="server" MaxLength="50" Text='<%# Bind("Commodity") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    Enabled="false"
                    ValidationGroup="vge" 
                    ControlToValidate="CommodityTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td class="check">
                <asp:CheckBox ID="AktywneCheckBox" runat="server" Checked='<%# Bind("Aktywne") %>' />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" ValidationGroup="vge"/>
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
        </tr>
    </EditItemTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" DeleteCommand="DELETE FROM [Commodity] WHERE [Id] = @Id" InsertCommand="INSERT INTO [Commodity] ([Commodity], [Aktywne]) VALUES (@Commodity, @Aktywne)" SelectCommand="SELECT * FROM [Commodity] ORDER BY [Commodity]" UpdateCommand="UPDATE [Commodity] SET [Commodity] = @Commodity, [Aktywne] = @Aktywne WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Commodity" Type="String" />
        <asp:Parameter Name="Aktywne" Type="Boolean" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Commodity" Type="String" />
        <asp:Parameter Name="Aktywne" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>

