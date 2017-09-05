<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntDicPosition.ascx.cs" Inherits="HRRcp.Controls.Przypisania.cntDicPosition" %>

<asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" InsertItemPosition="LastItem" 
    onitemcreated="ListView1_ItemCreated" onitemdatabound="ListView1_ItemDataBound">
    <ItemTemplate>
        <tr class="it">
            <td class="position">
                <asp:Label ID="PositionLabel" runat="server" Text='<%# Eval("Position") %>' />
            </td>
            <td class="commodity">
                <asp:Label ID="CommodityIdLabel" runat="server" Text='<%# Eval("Commodity") %>' />
            </td>
            <td class="area">
                <asp:Label ID="AreaIdLabel" runat="server" Text='<%# Eval("Area") %>' />
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
            <td class="position">
                <asp:TextBox ID="PositionTextBox" runat="server" Text='<%# Bind("Position") %>' />
            </td>
            <td class="commodity">
                <asp:DropDownList ID="ddlCommodity" runat="server" OnSelectedIndexChanged="ddlCommodity_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>            
            </td>
            <td class="area">
                <asp:DropDownList ID="ddlArea" runat="server" >
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
    <EditItemTemplate>
        <tr class="eit">
            <td class="position">
                <asp:TextBox ID="PositionTextBox" runat="server" Text='<%# Bind("Position") %>' />
            </td>
            <td class="commodity">
                <asp:DropDownList ID="ddlCommodity" runat="server" OnSelectedIndexChanged="ddlCommodity_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>            
            </td>
            <td class="area">
                <asp:DropDownList ID="ddlArea" runat="server" >
                </asp:DropDownList>            
            </td>
            <td class="check">
                <asp:CheckBox ID="AktywneCheckBox" runat="server" Checked='<%# Bind("Aktywne") %>' />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">
                                Position</th>
                            <th runat="server">
                                Commodity</th>
                            <th runat="server">
                                Area</th>
                            <th runat="server">
                                Aktywne</th>
                            <th runat="server" class="control">
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
    DeleteCommand="DELETE FROM [Position] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Position] ([Position], [CommodityId], [AreaId], [Aktywne]) VALUES (@Position, @CommodityId, @AreaId, @Aktywne)" 
    SelectCommand="
select P.*, C.Commodity, A.Area from Position P
left outer join Commodity C on C.Id = P.CommodityId
left outer join Area A on A.Id = P.AreaId 
ORDER BY Commodity, Area, Position" 
    UpdateCommand="UPDATE [Position] SET [Position] = @Position, [CommodityId] = @CommodityId, [AreaId] = @AreaId, [Aktywne] = @Aktywne WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Position" Type="String" />
        <asp:Parameter Name="CommodityId" Type="Int32" />
        <asp:Parameter Name="AreaId" Type="Int32" />
        <asp:Parameter Name="Aktywne" Type="Boolean" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Position" Type="String" />
        <asp:Parameter Name="CommodityId" Type="Int32" />
        <asp:Parameter Name="AreaId" Type="Int32" />
        <asp:Parameter Name="Aktywne" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>

