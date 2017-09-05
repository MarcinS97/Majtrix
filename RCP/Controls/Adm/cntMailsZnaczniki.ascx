<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntMailsZnaczniki.ascx.cs" Inherits="HRRcp.Controls.Mails.cntMailsZnaczniki" %>

<asp:HiddenField ID="hidGrupa" runat="server" />

<asp:ListView ID="lvZnaczniki" runat="server" DataKeyNames="Id" InsertItemPosition="LastItem"
    DataSourceID="SqlDataSource1" onitemdatabound="lvZnaczniki_ItemDataBound" 
    oniteminserting="lvZnaczniki_ItemInserting" 
    onitemupdating="lvZnaczniki_ItemUpdating">
    <ItemTemplate>
        <tr style="">
            <td class="znacznik">
                <asp:Label ID="ZnacznikLabel" runat="server" Text='<%# Eval("Znacznik") %>' />
            </td>
            <td>
                <asp:Label ID="OpisLabel" runat="server" Text='<%# Eval("Opis") %>' />
            </td>
            <td class="num">
                <asp:Label ID="KolejnoscLabel" runat="server" Text='<%# Eval("Kolejnosc") %>' />
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                <asp:Button ID="btSelect" runat="server" Text="Wstaw" />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
    </EmptyDataTemplate>
    <InsertItemTemplate>
        <tr style="">
            <td class="znacznik">
                <asp:TextBox ID="ZnacznikTextBox" runat="server" Text='<%# Bind("Znacznik") %>' />
            </td>
            <td>
                <asp:TextBox ID="OpisTextBox" runat="server" Text='<%# Bind("Opis") %>' />
            </td>
            <td class="num">
                <asp:TextBox ID="KolejnoscTextBox" runat="server" Text='<%# Bind("Kolejnosc") %>' />
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
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
                                Znacznik</th>
                            <th runat="server">
                                Opis</th>
                            <th runat="server">
                                Kolejność</th>
                            <th class="control" runat="server" >
                            </th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
    <EditItemTemplate>
        <tr style="">
            <td class="znacznik">
                <asp:Label ID="ZnacznikLabel" runat="server" Text='<%# Eval("Znacznik") %>' Visible="false"/>
                <asp:TextBox ID="ZnacznikTextBox" runat="server" Text='<%# Bind("Znacznik") %>' Visible="true"/>
            </td>
            <td>
                <asp:TextBox ID="OpisTextBox" runat="server" Text='<%# Bind("Opis") %>' />
            </td>
            <td class="num">
                <asp:TextBox ID="KolejnoscTextBox" runat="server" Text='<%# Bind("Kolejnosc") %>' />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
        </tr>
    </EditItemTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [MailingZnaczniki] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [MailingZnaczniki] ([Grupa], [Znacznik], [Opis], [Kolejnosc]) VALUES (@Grupa, @Znacznik, @Opis, @Kolejnosc)" 
    SelectCommand="SELECT [Znacznik], [Opis], [Kolejnosc], [Id] FROM [MailingZnaczniki] WHERE ([Grupa] = @Grupa) ORDER BY [Kolejnosc], [Znacznik]" 
    UpdateCommand="UPDATE [MailingZnaczniki] SET [Grupa] = @Grupa, [Znacznik] = @Znacznik, [Opis] = @Opis, [Kolejnosc] = @Kolejnosc WHERE [Id] = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidGrupa" Name="Grupa" PropertyName="Value" Type="String" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:ControlParameter ControlID="hidGrupa" Name="Grupa" PropertyName="Value" Type="String" />
        <asp:Parameter Name="Znacznik" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Kolejnosc" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="hidGrupa" Name="Grupa" PropertyName="Value" Type="String" />
        <asp:Parameter Name="Znacznik" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Kolejnosc" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>

