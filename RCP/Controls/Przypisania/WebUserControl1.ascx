<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserControl1.ascx.cs" Inherits="HRRcp.Controls.Przypisania.WebUserControl1" %>
<asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" DataSourceID="SqlDataSource1" InsertItemPosition="LastItem">
    <ItemTemplate>
        <tr style="">
            <td>
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
            </td>
            <td>
                <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
            </td>
            <td>
                <asp:Label ID="IdPracownikaLabel" runat="server" Text='<%# Eval("IdPracownika") %>' />
            </td>
            <td>
                <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od") %>' />
            </td>
            <td>
                <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do") %>' />
            </td>
            <td>
                <asp:Label ID="IdKierownikaLabel" runat="server" Text='<%# Eval("IdKierownika") %>' />
            </td>
            <td>
                <asp:Label ID="IdCCLabel" runat="server" Text='<%# Eval("IdCC") %>' />
            </td>
            <td>
                <asp:Label ID="IdCommodityLabel" runat="server" Text='<%# Eval("IdCommodity") %>' />
            </td>
            <td>
                <asp:Label ID="IdAreaLabel" runat="server" Text='<%# Eval("IdArea") %>' />
            </td>
            <td>
                <asp:Label ID="IdPositionLabel" runat="server" Text='<%# Eval("IdPosition") %>' />
            </td>
            <td>
                <asp:Label ID="IdKierownikaRqLabel" runat="server" Text='<%# Eval("IdKierownikaRq") %>' />
            </td>
            <td>
                <asp:Label ID="DataRqLabel" runat="server" Text='<%# Eval("DataRq") %>' />
            </td>
            <td>
                <asp:Label ID="UwagiRqLabel" runat="server" Text='<%# Eval("UwagiRq") %>' />
            </td>
            <td>
                <asp:Label ID="IdKierownikaAccLabel" runat="server" Text='<%# Eval("IdKierownikaAcc") %>' />
            </td>
            <td>
                <asp:Label ID="DataAccLabel" runat="server" Text='<%# Eval("DataAcc") %>' />
            </td>
            <td>
                <asp:Label ID="UwagiAccLabel" runat="server" Text='<%# Eval("UwagiAcc") %>' />
            </td>
            <td>
                <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
            </td>
            <td>
                <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
            </td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr style="">
            <td>
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
            </td>
            <td>
                <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
            </td>
            <td>
                <asp:Label ID="IdPracownikaLabel" runat="server" Text='<%# Eval("IdPracownika") %>' />
            </td>
            <td>
                <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od") %>' />
            </td>
            <td>
                <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do") %>' />
            </td>
            <td>
                <asp:Label ID="IdKierownikaLabel" runat="server" Text='<%# Eval("IdKierownika") %>' />
            </td>
            <td>
                <asp:Label ID="IdCCLabel" runat="server" Text='<%# Eval("IdCC") %>' />
            </td>
            <td>
                <asp:Label ID="IdCommodityLabel" runat="server" Text='<%# Eval("IdCommodity") %>' />
            </td>
            <td>
                <asp:Label ID="IdAreaLabel" runat="server" Text='<%# Eval("IdArea") %>' />
            </td>
            <td>
                <asp:Label ID="IdPositionLabel" runat="server" Text='<%# Eval("IdPosition") %>' />
            </td>
            <td>
                <asp:Label ID="IdKierownikaRqLabel" runat="server" Text='<%# Eval("IdKierownikaRq") %>' />
            </td>
            <td>
                <asp:Label ID="DataRqLabel" runat="server" Text='<%# Eval("DataRq") %>' />
            </td>
            <td>
                <asp:Label ID="UwagiRqLabel" runat="server" Text='<%# Eval("UwagiRq") %>' />
            </td>
            <td>
                <asp:Label ID="IdKierownikaAccLabel" runat="server" Text='<%# Eval("IdKierownikaAcc") %>' />
            </td>
            <td>
                <asp:Label ID="DataAccLabel" runat="server" Text='<%# Eval("DataAcc") %>' />
            </td>
            <td>
                <asp:Label ID="UwagiAccLabel" runat="server" Text='<%# Eval("UwagiAcc") %>' />
            </td>
            <td>
                <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
            </td>
            <td>
                <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
            </td>
        </tr>
    </AlternatingItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" style="">
            <tr>
                <td>
                    No data was returned.</td>
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
                &nbsp;</td>
            <td>
                <asp:TextBox ID="IdPracownikaTextBox" runat="server" Text='<%# Bind("IdPracownika") %>' />
            </td>
            <td>
                <asp:TextBox ID="OdTextBox" runat="server" Text='<%# Bind("Od") %>' />
            </td>
            <td>
                <asp:TextBox ID="DoTextBox" runat="server" Text='<%# Bind("Do") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdKierownikaTextBox" runat="server" Text='<%# Bind("IdKierownika") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdCCTextBox" runat="server" Text='<%# Bind("IdCC") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdCommodityTextBox" runat="server" Text='<%# Bind("IdCommodity") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdAreaTextBox" runat="server" Text='<%# Bind("IdArea") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdPositionTextBox" runat="server" Text='<%# Bind("IdPosition") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdKierownikaRqTextBox" runat="server" Text='<%# Bind("IdKierownikaRq") %>' />
            </td>
            <td>
                <asp:TextBox ID="DataRqTextBox" runat="server" Text='<%# Bind("DataRq") %>' />
            </td>
            <td>
                <asp:TextBox ID="UwagiRqTextBox" runat="server" Text='<%# Bind("UwagiRq") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdKierownikaAccTextBox" runat="server" Text='<%# Bind("IdKierownikaAcc") %>' />
            </td>
            <td>
                <asp:TextBox ID="DataAccTextBox" runat="server" Text='<%# Bind("DataAcc") %>' />
            </td>
            <td>
                <asp:TextBox ID="UwagiAccTextBox" runat="server" Text='<%# Bind("UwagiAcc") %>' />
            </td>
            <td>
                <asp:TextBox ID="StatusTextBox" runat="server" Text='<%# Bind("Status") %>' />
            </td>
            <td>
                <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
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
                            </th>
                            <th runat="server">
                                Id</th>
                            <th runat="server">
                                IdPracownika</th>
                            <th runat="server">
                                Od</th>
                            <th runat="server">
                                Do</th>
                            <th runat="server">
                                IdKierownika</th>
                            <th runat="server">
                                IdCC</th>
                            <th runat="server">
                                IdCommodity</th>
                            <th runat="server">
                                IdArea</th>
                            <th runat="server">
                                IdPosition</th>
                            <th runat="server">
                                IdKierownikaRq</th>
                            <th runat="server">
                                DataRq</th>
                            <th runat="server">
                                UwagiRq</th>
                            <th runat="server">
                                IdKierownikaAcc</th>
                            <th runat="server">
                                DataAcc</th>
                            <th runat="server">
                                UwagiAcc</th>
                            <th runat="server">
                                Status</th>
                            <th runat="server">
                                Typ</th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server">
                <td runat="server" style="">
                    <asp:DataPager ID="DataPager1" runat="server">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True" />
                        </Fields>
                    </asp:DataPager>
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
                <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdPracownikaTextBox" runat="server" Text='<%# Bind("IdPracownika") %>' />
            </td>
            <td>
                <asp:TextBox ID="OdTextBox" runat="server" Text='<%# Bind("Od") %>' />
            </td>
            <td>
                <asp:TextBox ID="DoTextBox" runat="server" Text='<%# Bind("Do") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdKierownikaTextBox" runat="server" Text='<%# Bind("IdKierownika") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdCCTextBox" runat="server" Text='<%# Bind("IdCC") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdCommodityTextBox" runat="server" Text='<%# Bind("IdCommodity") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdAreaTextBox" runat="server" Text='<%# Bind("IdArea") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdPositionTextBox" runat="server" Text='<%# Bind("IdPosition") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdKierownikaRqTextBox" runat="server" Text='<%# Bind("IdKierownikaRq") %>' />
            </td>
            <td>
                <asp:TextBox ID="DataRqTextBox" runat="server" Text='<%# Bind("DataRq") %>' />
            </td>
            <td>
                <asp:TextBox ID="UwagiRqTextBox" runat="server" Text='<%# Bind("UwagiRq") %>' />
            </td>
            <td>
                <asp:TextBox ID="IdKierownikaAccTextBox" runat="server" Text='<%# Bind("IdKierownikaAcc") %>' />
            </td>
            <td>
                <asp:TextBox ID="DataAccTextBox" runat="server" Text='<%# Bind("DataAcc") %>' />
            </td>
            <td>
                <asp:TextBox ID="UwagiAccTextBox" runat="server" Text='<%# Bind("UwagiAcc") %>' />
            </td>
            <td>
                <asp:TextBox ID="StatusTextBox" runat="server" Text='<%# Bind("Status") %>' />
            </td>
            <td>
                <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />
            </td>
        </tr>
    </EditItemTemplate>
    <SelectedItemTemplate>
        <tr style="">
            <td>
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
            </td>
            <td>
                <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
            </td>
            <td>
                <asp:Label ID="IdPracownikaLabel" runat="server" Text='<%# Eval("IdPracownika") %>' />
            </td>
            <td>
                <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od") %>' />
            </td>
            <td>
                <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do") %>' />
            </td>
            <td>
                <asp:Label ID="IdKierownikaLabel" runat="server" Text='<%# Eval("IdKierownika") %>' />
            </td>
            <td>
                <asp:Label ID="IdCCLabel" runat="server" Text='<%# Eval("IdCC") %>' />
            </td>
            <td>
                <asp:Label ID="IdCommodityLabel" runat="server" Text='<%# Eval("IdCommodity") %>' />
            </td>
            <td>
                <asp:Label ID="IdAreaLabel" runat="server" Text='<%# Eval("IdArea") %>' />
            </td>
            <td>
                <asp:Label ID="IdPositionLabel" runat="server" Text='<%# Eval("IdPosition") %>' />
            </td>
            <td>
                <asp:Label ID="IdKierownikaRqLabel" runat="server" Text='<%# Eval("IdKierownikaRq") %>' />
            </td>
            <td>
                <asp:Label ID="DataRqLabel" runat="server" Text='<%# Eval("DataRq") %>' />
            </td>
            <td>
                <asp:Label ID="UwagiRqLabel" runat="server" Text='<%# Eval("UwagiRq") %>' />
            </td>
            <td>
                <asp:Label ID="IdKierownikaAccLabel" runat="server" Text='<%# Eval("IdKierownikaAcc") %>' />
            </td>
            <td>
                <asp:Label ID="DataAccLabel" runat="server" Text='<%# Eval("DataAcc") %>' />
            </td>
            <td>
                <asp:Label ID="UwagiAccLabel" runat="server" Text='<%# Eval("UwagiAcc") %>' />
            </td>
            <td>
                <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
            </td>
            <td>
                <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
            </td>
        </tr>
    </SelectedItemTemplate>
</asp:ListView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" DeleteCommand="DELETE FROM [Przypisania] WHERE [Id] = @Id" 

InsertCommand="INSERT INTO [Przypisania] ([IdPracownika], [Od], [Do], [IdKierownika], [IdCC], [IdCommodity], [IdArea], [IdPosition], [IdKierownikaRq], [DataRq], [UwagiRq], [IdKierownikaAcc], [DataAcc], [UwagiAcc], [Status], [Typ]) VALUES (@IdPracownika, @Od, @Do, @IdKierownika, @IdCC, @IdCommodity, @IdArea, @IdPosition, @IdKierownikaRq, @DataRq, @UwagiRq, @IdKierownikaAcc, @DataAcc, @UwagiAcc, @Status, @Typ)" 
SelectCommand="SELECT * FROM [Przypisania]" UpdateCommand="UPDATE [Przypisania] SET [IdPracownika] = @IdPracownika, [Od] = @Od, [Do] = @Do, [IdKierownika] = @IdKierownika, [IdCC] = @IdCC, [IdCommodity] = @IdCommodity, [IdArea] = @IdArea, [IdPosition] = @IdPosition, [IdKierownikaRq] = @IdKierownikaRq, [DataRq] = @DataRq, [UwagiRq] = @UwagiRq, [IdKierownikaAcc] = @IdKierownikaAcc, [DataAcc] = @DataAcc, [UwagiAcc] = @UwagiAcc, [Status] = @Status, [Typ] = @Typ WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="IdKierownika" Type="Int32" />
        <asp:Parameter Name="IdCC" Type="Int32" />
        <asp:Parameter Name="IdCommodity" Type="Int32" />
        <asp:Parameter Name="IdArea" Type="Int32" />
        <asp:Parameter Name="IdPosition" Type="Int32" />
        <asp:Parameter Name="IdKierownikaRq" Type="Int32" />
        <asp:Parameter Name="DataRq" Type="DateTime" />
        <asp:Parameter Name="UwagiRq" Type="String" />
        <asp:Parameter Name="IdKierownikaAcc" Type="Int32" />
        <asp:Parameter Name="DataAcc" Type="DateTime" />
        <asp:Parameter Name="UwagiAcc" Type="String" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Typ" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="IdKierownika" Type="Int32" />
        <asp:Parameter Name="IdCC" Type="Int32" />
        <asp:Parameter Name="IdCommodity" Type="Int32" />
        <asp:Parameter Name="IdArea" Type="Int32" />
        <asp:Parameter Name="IdPosition" Type="Int32" />
        <asp:Parameter Name="IdKierownikaRq" Type="Int32" />
        <asp:Parameter Name="DataRq" Type="DateTime" />
        <asp:Parameter Name="UwagiRq" Type="String" />
        <asp:Parameter Name="IdKierownikaAcc" Type="Int32" />
        <asp:Parameter Name="DataAcc" Type="DateTime" />
        <asp:Parameter Name="UwagiAcc" Type="String" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Typ" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>

