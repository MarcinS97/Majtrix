<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntDicArea.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Przypisania.cntDicArea" %>

<asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" InsertItemPosition="LastItem" 
    onitemcreated="ListView1_ItemCreated" 
    onitemdatabound="ListView1_ItemDataBound" 
    oniteminserting="ListView1_ItemInserting" 
    onitemupdating="ListView1_ItemUpdating">
    <ItemTemplate>
        <tr class="it">
            <td class="area">
                <asp:Label ID="AreaLabel" runat="server" Text='<%# Eval("Area") %>' />
            </td>
            <td class="commodity">
                <asp:Label ID="CommodityIdLabel" runat="server" Text='<%# Eval("Commodity") %>' />
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
    <EditItemTemplate>
        <tr  class="eit">
            <td class="area">
                <asp:TextBox ID="AreaTextBox" runat="server" MaxLength="50" Text='<%# Bind("Area") %>' />
                <asp:RequiredFieldValidator Text="Pole wymagane" ControlToValidate="AreaTextBox" ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="evg" ></asp:RequiredFieldValidator>
            </td>
            <td class="commodity">
                <asp:DropDownList ID="ddlCommodity" runat="server" >
                </asp:DropDownList>                
            </td>
            <td class="check">
                <asp:CheckBox ID="AktywneCheckBox" runat="server" Checked='<%# Bind("Aktywne") %>' />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" ValidationGroup="evg" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
        </tr>
    </EditItemTemplate>
    <InsertItemTemplate>
        <tr  class="iit">
            <td class="area">
                <asp:TextBox ID="AreaTextBox" runat="server" MaxLength="50" Text='<%# Bind("Area") %>' />
                <asp:RequiredFieldValidator Text="Pole wymagane" ControlToValidate="AreaTextBox" ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic" ValidationGroup="ivg" ></asp:RequiredFieldValidator>
            </td>
            <td class="commodity">
                <asp:DropDownList ID="ddlCommodity" runat="server" >
                </asp:DropDownList>                
            </td>
            <td class="check">
                <asp:CheckBox ID="AktywneCheckBox" runat="server" Checked='<%# Bind("Aktywne") %>' />
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
        </tr>
    </InsertItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="tbDicArea">
            <tr runat="server">
                <td runat="server" colspan="2">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">
                                Area</th>
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

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [Area] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Area] ([Area], [CommodityId], [Aktywne]) VALUES (@Area, @CommodityId, @Aktywne)" 
    SelectCommand="SELECT A.*, C.Commodity FROM Area A left outer join Commodity C on C.Id = A.CommodityId ORDER BY Commodity, Area" 
    UpdateCommand="UPDATE [Area] SET [Area] = @Area, [CommodityId] = @CommodityId, [Aktywne] = @Aktywne WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Area" Type="String" />
        <asp:Parameter Name="CommodityId" Type="Int32" />
        <asp:Parameter Name="Aktywne" Type="Boolean" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Area" Type="String" />
        <asp:Parameter Name="CommodityId" Type="Int32" />
        <asp:Parameter Name="Aktywne" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>

