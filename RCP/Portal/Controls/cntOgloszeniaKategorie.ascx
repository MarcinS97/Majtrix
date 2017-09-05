<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntOgloszeniaKategorie.ascx.cs" Inherits="HRRcp.Portal.Controls.cntOgloszeniaKategorie" %>

<div id="paOgloszeniaKategorie" runat="server" class="cntOgloszeniaKategorie">
    <asp:ListView ID="lvKategorie" runat="server" DataKeyNames="Id" DataSourceID="SqlDataSource1" EnableModelValidation="True" InsertItemPosition="None"
        OnItemCreated="lvKategorie_ItemCreated"
        >
        <ItemTemplate>
            <tr style="">
                <td>
                    <asp:Label ID="NazwaLabel" runat="server" Text='<%# Eval("Kategoria") %>' />
                </td>
                <td class="num">
                    <asp:Label ID="KolejnoscLabel" runat="server" Text='<%# Eval("Kolejnosc") %>' />
                </td>
                <td class="check">
                    <asp:CheckBox ID="cbAktywna" runat="server" Checked='<%# Eval("Aktywna") %>' Enabled="false" />
                </td>
                <td class="control">
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                </td>
            </tr>
        </ItemTemplate>
        <InsertItemTemplate>
            <tr class="iit">
                <td>
                    <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Kategoria") %>' />
                </td>
                <td class="num">
                    <asp:TextBox ID="KolejnoscTextBox" runat="server" Text='<%# Bind("Kolejnosc") %>' />
                </td>
                <td class="check">
                    <asp:CheckBox ID="cbAktywna" runat="server" Checked='<%# Bind("Aktywna") %>' />
                </td>
                <td class="control">
                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Clear" />
                </td>
            </tr>
        </InsertItemTemplate>
        <EditItemTemplate>
            <tr class="eit">
                <td>
                    <asp:TextBox ID="NazwaTextBox" runat="server" Text='<%# Bind("Kategoria") %>' />
                </td>
                <td class="num">
                    <asp:TextBox ID="KolejnoscTextBox" runat="server" Text='<%# Bind("Kolejnosc") %>' />
                </td>
                <td class="check">
                    <asp:CheckBox ID="cbAktywna" runat="server" Checked='<%# Bind("Aktywna") %>' />
                </td>
                <td class="control">
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
                </td>
            </tr>
        </EditItemTemplate>
        <EmptyDataTemplate>
            <table runat="server" class="edt">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Brak danych"></asp:Label>
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <div class="buttons">
                <%-- trick, ten znika i jest znacznikiem, ze ma ListView pokazywać InsetTemplate dopiero po NewRecord --%>                
                <asp:Button ID="btNewRecord" runat="server" CssClass="btn btn-default" CommandName="NewRecord" Text="Dodaj kategorię" />
                <asp:Button ID="InsertButton" runat="server" CssClass="button_postback" CommandName="NewRecord" Text="Dodaj kategorię" />
            </div>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr runat="server" style="">
                                <th runat="server">
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="sort" CommandArgument="Kategoria" >Kategoria</asp:LinkButton>                                
                                </th>
                                <th runat="server">
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="sort" CommandArgument="Kolejnosc" >Kolejność</asp:LinkButton>                                
                                </th>
                                <th runat="server">
                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandName="sort" CommandArgument="Aktywna" >Aktywna</asp:LinkButton>                                
                                </th>
                                <th runat="server">
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
    </asp:ListView>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" 
        DeleteCommand="DELETE FROM [poOgloszeniaKategorie] WHERE [Id] = @Id" 
        InsertCommand="INSERT INTO [poOgloszeniaKategorie] ([Kategoria], [Kolejnosc], [Aktywna]) VALUES (@Kategoria, @Kolejnosc, @Aktywna)" 
        SelectCommand="SELECT * FROM [poOgloszeniaKategorie] ORDER BY Aktywna desc, Kolejnosc, Kategoria" 
        UpdateCommand="UPDATE [poOgloszeniaKategorie] SET [Kategoria] = @Kategoria, [Kolejnosc] = @Kolejnosc, [Aktywna] = @Aktywna WHERE [Id] = @Id"
        >
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="Kategoria" Type="String" />
            <asp:Parameter Name="Kolejnosc" Type="Int32" />
            <asp:Parameter Name="Aktywna" Type="Boolean" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Kategoria" Type="String" />
            <asp:Parameter Name="Kolejnosc" Type="Int32" />
            <asp:Parameter Name="Aktywna" Type="Boolean" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
</div>