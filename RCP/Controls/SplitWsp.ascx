<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SplitWsp.ascx.cs" Inherits="HRRcp.Controls.SplitWsp" %>

<asp:HiddenField ID="hidIdSplitu" runat="server" />
<asp:HiddenField ID="hidGrSplitu" runat="server" />

<asp:ListView ID="lvSplityWsp" runat="server" DataSourceID="SqlDataSource1" 
    InsertItemPosition="LastItem"
    onitemdatabound="lvSplityWsp_ItemDataBound" 
    onitemdeleting="lvSplityWsp_ItemDeleting" 
    oniteminserting="lvSplityWsp_ItemInserting" 
    onitemupdating="lvSplityWsp_ItemUpdating">
    <ItemTemplate>
        <tr class="it">
            <td>
                <asp:Label ID="IdCCLabel" runat="server" Text='<%# Eval("cc") %>' />
            </td>
            <td class="num">
                <asp:Label ID="WspLabel" runat="server" Text='<%# Eval("Wsp") %>' />
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
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
            <td>
                <asp:DropDownList ID="ddlCC" runat="server" 
                    DataSourceID="SqlDataSource2"
                    DataTextField="Nazwa"
                    DataValueField="Id">
                </asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="WspTextBox" runat="server" Text='<%# Bind("Wsp") %>' />
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td>
                <asp:DropDownList ID="ddlCC" runat="server" 
                    DataSourceID="SqlDataSource2"
                    DataTextField="Nazwa"
                    DataValueField="Id">
                </asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="WspTextBox" runat="server" Text='<%# Bind("Wsp") %>' />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="tbSplitWsp hoverline">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0">
                        <tr runat="server" style="">
                            <th runat="server">
                                CC</th>
                            <th runat="server">
                                Współczynnik </th>
                            <th class="control"></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"     
    SelectCommand="SELECT S.Id, S.IdCC, CC.cc + ' - ' + CC.Nazwa as cc, S.Wsp FROM SplityWsp S left outer join CC on CC.Id = S.IdCC where ([IdSplitu] = @IdSplitu) ORDER BY cc" 
    ConflictDetection="CompareAllValues" 
    DeleteCommand="DELETE FROM [SplityWsp] WHERE [Id] = @original_Id AND [IdSplitu] = @original_IdSplitu AND [IdCC] = @original_IdCC AND [Wsp] = @original_Wsp" 
    InsertCommand="INSERT INTO [SplityWsp] ([IdSplitu], [IdCC], [Wsp]) VALUES (@IdSplitu, @IdCC, @Wsp)" 
    OldValuesParameterFormatString="original_{0}" 
    UpdateCommand="UPDATE [SplityWsp] SET [IdSplitu] = @IdSplitu, [IdCC] = @IdCC, [Wsp] = @Wsp WHERE [Id] = @original_Id AND [IdSplitu] = @original_IdSplitu AND [IdCC] = @original_IdCC AND [Wsp] = @original_Wsp">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidIdSplitu" Name="IdSplitu" PropertyName="Value" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="original_Id" Type="Int32" />
        <asp:Parameter Name="original_IdSplitu" Type="Int32" />
        <asp:Parameter Name="original_IdCC" Type="Int32" />
        <asp:Parameter Name="original_Wsp" Type="Double" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:ControlParameter ControlID="hidIdSplitu" Name="IdSplitu" PropertyName="Value" Type="Int32" />
        <asp:Parameter Name="IdCC" Type="Int32" />
        <asp:Parameter Name="Wsp" Type="Double" />
        <asp:Parameter Name="original_Id" Type="Int32" />
        <asp:Parameter Name="original_IdSplitu" Type="Int32" />
        <asp:Parameter Name="original_IdCC" Type="Int32" />
        <asp:Parameter Name="original_Wsp" Type="Double" />
    </UpdateParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="hidIdSplitu" Name="IdSplitu" PropertyName="Value" Type="Int32" />
        <asp:Parameter Name="IdCC" Type="Int32" />
        <asp:Parameter Name="Wsp" Type="Double" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select 0 as Sort, null as Id, 'wybierz ...' as Nazwa union
                   SELECT 1 as Sort, Id, cc + ' - ' + Nazwa as Nazwa FROM CC where ISNULL(AktywneDo, GETDATE()) &gt;= GETDATE() 
                   ORDER BY Sort, Nazwa">
</asp:SqlDataSource>
