<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPrzypiszRCPHistoria.ascx.cs" Inherits="HRRcp.Controls.Kiosk.cntPrzypiszRCPHistoria" %>

<asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" onitemdeleted="ListView1_ItemDeleted" 
    onitemdeleting="ListView1_ItemDeleting">
    <ItemTemplate>
        <tr style="">
            <td>
                <asp:Label ID="DataLabel" runat="server" Text='<%# Eval("Data", "{0:d}") %>' />
                <asp:Label ID="Label1" runat="server" CssClass="gray" Text='<%# Eval("Data", "{0:T}") %>' />
            </td>
            <td>
                <asp:Label ID="PracownikLabel" runat="server" Text='<%# Eval("Pracownik") %>' />
            </td>
            <td>
                <asp:Label ID="Nr_EwidLabel" runat="server" Text='<%# Eval("Nr_Ewid") %>' />
            </td>
            <td>
                <asp:Label ID="NrKartyLabel" runat="server" Text='<%# Eval("NrKarty") %>' />
            </td>
            <td>
                <asp:Label ID="PrevNrKartyLabel" runat="server" Text='<%# Eval("PrevNrKarty") %>' />
            </td>
            <td>
                <asp:Label ID="AdminLabel" runat="server" Text='<%# Eval("Admin") %>' />
            </td>
            <td class="control">
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" style="">
            <tr>
                <td>
                    Brak przypisań
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <InsertItemTemplate>
        <tr style="">
            <td>
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
            <td>
                <asp:TextBox ID="DataTextBox" runat="server" Text='<%# Bind("Data") %>' />
            </td>
            <td>
                <asp:TextBox ID="PracownikTextBox" runat="server" Text='<%# Bind("Pracownik") %>' />
            </td>
            <td>
                <asp:TextBox ID="Nr_EwidTextBox" runat="server" Text='<%# Bind("Nr_Ewid") %>' />
            </td>
            <td>
                <asp:TextBox ID="NrKartyTextBox" runat="server" Text='<%# Bind("NrKarty") %>' />
            </td>
            <td>
                <asp:TextBox ID="PrevNrKartyTextBox" runat="server" Text='<%# Bind("PrevNrKarty") %>' />
            </td>
            <td>
                <asp:TextBox ID="AdminTextBox" runat="server" Text='<%# Bind("Admin") %>' />
            </td>
        </tr>
    </InsertItemTemplate>
    <LayoutTemplate>
        <table runat="server">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">
                                <asp:LinkButton ID="LinkButton1" Text="Data" runat="server" CommandName="Sort" ToolTip="Sortuj" CommandArgument="Data" />
                            </th>
                            <th runat="server">
                                <asp:LinkButton ID="LinkButton2" Text="Pracownik" runat="server" CommandName="Sort" ToolTip="Sortuj" CommandArgument="Pracownik" />
                            </th>
                            <th runat="server">
                                <asp:LinkButton ID="LinkButton3" Text="Nr Ewid." runat="server" CommandName="Sort" ToolTip="Sortuj" CommandArgument="Nr_Ewid" />
                            </th>
                            <th runat="server">
                                <asp:LinkButton ID="LinkButton4" Text="Nr karty RCP" runat="server" CommandName="Sort" ToolTip="Sortuj" CommandArgument="NrKarty" />
                            </th>
                            <th runat="server">
                                <asp:LinkButton ID="LinkButton5" Text="Poprzedni nr karty RCP" runat="server" CommandName="Sort" ToolTip="Sortuj" CommandArgument="PrevNrKarty" />
                            </th>
                            <th runat="server">
                                <asp:LinkButton ID="LinkButton6" Text="Przypisał" runat="server" CommandName="Sort" ToolTip="Sortuj" CommandArgument="Admin" />
                            </th>
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
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                </td>
                <td class="right">
                    <h3>Pokaż na stronie:&nbsp;&nbsp;&nbsp;</h3>
                    <asp:DropDownList ID="ddlLines" runat="server"></asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <span class="count">Ilość rekordów:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                </td>
            </tr>
            <tr class="pager">
                <td class="left">
                </td>
                <td class="right">
                </td>
            </tr>
        </table>
    </LayoutTemplate>
    <EditItemTemplate>
        <tr style="">
            <td>
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
            <td>
                <asp:TextBox ID="DataTextBox" runat="server" Text='<%# Bind("Data") %>' />
            </td>
            <td>
                <asp:TextBox ID="PracownikTextBox" runat="server" Text='<%# Bind("Pracownik") %>' />
            </td>
            <td>
                <asp:TextBox ID="Nr_EwidTextBox" runat="server" Text='<%# Bind("Nr_Ewid") %>' />
            </td>
            <td>
                <asp:TextBox ID="NrKartyTextBox" runat="server" Text='<%# Bind("NrKarty") %>' />
            </td>
            <td>
                <asp:TextBox ID="PrevNrKartyTextBox" runat="server" Text='<%# Bind("PrevNrKarty") %>' />
            </td>
            <td>
                <asp:TextBox ID="AdminTextBox" runat="server" Text='<%# Bind("Admin") %>' />
            </td>
        </tr>
    </EditItemTemplate>
    <SelectedItemTemplate>
        <tr style="">
            <td>
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
            </td>
            <td>
                <asp:Label ID="DataLabel" runat="server" Text='<%# Eval("Data") %>' />
            </td>
            <td>
                <asp:Label ID="PracownikLabel" runat="server" Text='<%# Eval("Pracownik") %>' />
            </td>
            <td>
                <asp:Label ID="Nr_EwidLabel" runat="server" Text='<%# Eval("Nr_Ewid") %>' />
            </td>
            <td>
                <asp:Label ID="NrKartyLabel" runat="server" Text='<%# Eval("NrKarty") %>' />
            </td>
            <td>
                <asp:Label ID="PrevNrKartyLabel" runat="server" Text='<%# Eval("PrevNrKarty") %>' />
            </td>
            <td>
                <asp:Label ID="AdminLabel" runat="server" Text='<%# Eval("Admin") %>' />
            </td>
        </tr>
    </SelectedItemTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [PrzypisaniaRCP] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [PrzypisaniaRCP] ([IdPracownika], [NrKarty], [Data], [AdminId], [PrevNrKarty], [OtherPracId]) VALUES (@IdPracownika, @NrKarty, @Data, @AdminId, @PrevNrKarty, @OtherPracId)" 
    SelectCommand="
select 
    R.Id, R.Data, R.IdPracownika,
	P.Nazwisko + ' ' + P.Imie as Pracownik, P.KadryId as Nr_Ewid,
	R.NrKarty, R.PrevNrKarty,
	A.Nazwisko + ' ' + A.Imie as Admin
FROM PrzypisaniaRCP R
left outer join Pracownicy P on P.Id = R.IdPracownika
left outer join Pracownicy A on A.Id = R.AdminId
ORDER BY R.Data DESC"
    UpdateCommand="UPDATE [PrzypisaniaRCP] SET [IdPracownika] = @IdPracownika, [NrKarty] = @NrKarty, [Data] = @Data, [AdminId] = @AdminId, [PrevNrKarty] = @PrevNrKarty, [OtherPracId] = @OtherPracId WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="NrKarty" Type="String" />
        <asp:Parameter Name="Data" Type="DateTime" />
        <asp:Parameter Name="AdminId" Type="Int32" />
        <asp:Parameter Name="PrevNrKarty" Type="String" />
        <asp:Parameter Name="OtherPracId" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="NrKarty" Type="String" />
        <asp:Parameter Name="Data" Type="DateTime" />
        <asp:Parameter Name="AdminId" Type="Int32" />
        <asp:Parameter Name="PrevNrKarty" Type="String" />
        <asp:Parameter Name="OtherPracId" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>

