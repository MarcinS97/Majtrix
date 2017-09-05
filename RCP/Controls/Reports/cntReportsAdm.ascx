<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntReportsAdm.ascx.cs" Inherits="HRRcp.Controls.cntReportsAdm" %>

<div id="paReportsAdm" runat="server" class="cntReportsAdm">
    
    <div class="tbMailsFilter" runat="server" visible="false">
        <asp:DropDownList ID="ddlFilter" runat="server" DataSourceID="SqlDataSource2" 
            DataTextField="Nazwa" DataValueField="Grupa" AutoPostBack="True" 
            onselectedindexchanged="ddlFilter_SelectedIndexChanged" >
        </asp:DropDownList>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
            SelectCommand="
    select null as Grupa, 'Pokaż wszystkie mailingi' as Nazwa, 1 as Sort
    union all 
    select distinct M.Grupa, ISNULL(G.Opis + ' - ' + M.Grupa, M.Grupa) as Nazwa, 2 as Sort 
    from Mailing M
    left join MailingGrupy G on G.Grupa = M.Grupa
    order by Sort, Grupa
            ">
        </asp:SqlDataSource>
        
        <br />
    </div>

    <asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" DataSourceID="SqlDataSource3" 
        onitemcreated="ListView1_ItemCreated" 
        onitemdatabound="ListView1_ItemDataBound" 
        oniteminserting="ListView1_ItemInserting" 
        onitemupdating="ListView1_ItemUpdating" 
        onitemdeleted="ListView1_ItemDeleted" 
        onitemediting="ListView1_ItemEditing" 
        oniteminserted="ListView1_ItemInserted" 
        onitemupdated="ListView1_ItemUpdated" 
        onselectedindexchanged="ListView1_SelectedIndexChanged" 
        onitemcanceling="ListView1_ItemCanceling" onitemcommand="ListView1_ItemCommand" 
        onselectedindexchanging="ListView1_SelectedIndexChanging" 
        ondatabinding="ListView1_DataBinding" ondatabound="ListView1_DataBound">
        <ItemTemplate>
            <tr class="it">
                <td class="check">
                    <asp:CheckBox ID="cbSelect" runat="server" />
                    <asp:Button ID="btSelect" runat="server" CommandName="Select" Text="Select" CssClass="button_postback" />
                </td>
                <td class="id num">
                    <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                </td>

                <td class="grupa">
                    <asp:Label ID="GrupaLabel" runat="server" Text='<%# Eval("Grupa") %>' />
                </td>

                <td class="nazwa">
                    <asp:Label ID="MenuTextLabel" runat="server" Text='<%# Eval("MenuText") %>' />
                </td>
                <td class="desc">
                    <asp:Label ID="ToolTipLabel" runat="server" Text='<%# Eval("ToolTip") %>' />
                </td>

                <td class="cmd">
                    <asp:Label ID="CommandLabel" runat="server" Text='<%# Eval("Command") %>' />
                </td>
                
                <td class="par">
                    <asp:Label ID="Par1Label" runat="server" Text='<%# Eval("Par1") %>' />
                </td>
                <td class="par">
                    <asp:Label ID="Par2Label" runat="server" Text='<%# Eval("Par2") %>' />
                </td>

                <td class="typ">
<%--                    <asp:Label ID="TypLabel" runat="server" Text='< %# Eval("Typ") %>' />--%>
                    <asp:HiddenField ID="hidSql" runat="server" Visible="false" Value='<%# Eval("Sql") %>' />
                    <asp:HiddenField ID="hidSqlParams" runat="server" Visible="false" Value='<%# Eval("SqlParams") %>' />
                </td>
                <td class="komentarz">
<%--                    <asp:Label ID="KomentarzLabel" runat="server" Text='< %# Eval("Komentarz") %>' />--%>
                </td>                


                <td class="check">
                    <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" />
                </td>
                <td class="num">
                    <asp:Label ID="KolejnoscLabel" runat="server" Text='<%# Eval("Kolejnosc") %>' />
                </td>

                <td class="control">
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                </td>
            </tr>
        </ItemTemplate>
        <SelectedItemTemplate>
            <tr class="sit">
                <td class="check">
                </td>


                <td class="id num">
                    <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                </td>

                <td class="grupa">
                    <asp:Label ID="GrupaLabel" runat="server" Text='<%# Eval("Grupa") %>' />
                </td>

                <td class="nazwa">
                    <asp:Label ID="MenuTextLabel" runat="server" Text='<%# Eval("MenuText") %>' />
                </td>
                <td class="desc">
                    <asp:Label ID="ToolTipLabel" runat="server" Text='<%# Eval("ToolTip") %>' />
                </td>

                <td class="cmd">
                    <asp:Label ID="CommandLabel" runat="server" Text='<%# Eval("Command") %>' />
                </td>

                <td class="par">
                    <asp:Label ID="Par1Label" runat="server" Text='<%# Eval("Par1") %>' />
                </td>
                <td class="par">
                    <asp:Label ID="Par2Label" runat="server" Text='<%# Eval("Par2") %>' />
                </td>
                
                <td class="typ">
<%--                    <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />--%>
                    <asp:HiddenField ID="hidSql" runat="server" Visible="false" Value='<%# Eval("Sql") %>' />
                    <asp:HiddenField ID="hidSqlParams" runat="server" Visible="false" Value='<%# Eval("SqlParams") %>' />
                </td>
                <td class="komentarz">
<%--                    <asp:Label ID="KomentarzLabel" runat="server" Text='<%# Eval("Komentarz") %>' />--%>
                </td>                


                <td class="check">
                    <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" />
                </td>
                <td class="num">
                    <asp:Label ID="KolejnoscLabel" runat="server" Text='<%# Eval("Kolejnosc") %>' />
                </td>

                <td class="control" rowspan="1">
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
                </td>
            </tr>
<%--            <tr class="sit">
                <td class="sql" colspan="12">
                    <asp:Label ID="Label1" runat="server" CssClass="sql" Text='<%# Eval("Sql") %>' />
                </td>
            </tr>--%>
        </SelectedItemTemplate>
        <InsertItemTemplate>
            <tr class="iit">
                <td></td>
                <td></td>

                <td class="grupa">
                    <asp:TextBox ID="GrupaTextBox" runat="server" Text='<%# Bind("Grupa") %>' />
                </td>
                <td class="nazwa">
                    <asp:TextBox ID="MenuTextTextBox" runat="server" Text='<%# Bind("MenuText") %>' />
                </td>
                <td class="opis">
                    <asp:TextBox ID="ToolTipTextBox" runat="server" Text='<%# Bind("ToolTip") %>' />
                </td>
                <td class="cmd">
                    <asp:TextBox ID="CommandTextBox" runat="server" Text='<%# Bind("Command") %>' />
                </td>
                
                
                <td class="par">
                    <asp:TextBox ID="Par1TextBox" runat="server" Text='<%# Bind("Par1") %>' />
                </td>
                <td class="par">
                    <asp:TextBox ID="Par2TextBox" runat="server" Text='<%# Bind("Par2") %>' />
                </td>



                <td class="typ">
<%--                    <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />--%>
                </td>
                <td class="komentarz">
<%--                    <asp:TextBox ID="KomentarzTextBox" runat="server" Text='<%# Bind("Komentarz") %>' />--%>
                </td>
                <td class="check">
                    <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' />
                </td>
                <td>
                    <asp:TextBox ID="KolejnoscTextBox" runat="server" Text='<%# Bind("Kolejnosc") %>' />
                </td>


                <td class="control" rowspan="2">
                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Clear" />
                </td>
            </tr>
            <tr class="iit">
                <td class="sql" colspan="12">
                    <asp:TextBox ID="tbSql" runat="server" Text='<%# Bind("Sql") %>' TextMode="MultiLine" Rows="20" />
                </td>
            </tr>
        </InsertItemTemplate>
        <EditItemTemplate>
            <tr class="eit">
                <td></td>
                <td></td>



                <td class="grupa">
                    <asp:TextBox ID="GrupaTextBox" runat="server" Text='<%# Bind("Grupa") %>' />
                </td>
                <td class="nazwa">
                    <asp:TextBox ID="MenuTextTextBox" runat="server" Text='<%# Bind("MenuText") %>' />
                </td>
                <td class="opis">
                    <asp:TextBox ID="ToolTipTextBox" runat="server" Text='<%# Bind("ToolTip") %>' />
                </td>
                <td class="cmd">
                    <asp:TextBox ID="CommandTextBox" runat="server" Text='<%# Bind("Command") %>' />
                </td>
                
                
                <td class="par">
                    <asp:TextBox ID="Par1TextBox" runat="server" Text='<%# Bind("Par1") %>' />
                </td>
                <td class="par">
                    <asp:TextBox ID="Par2TextBox" runat="server" Text='<%# Bind("Par2") %>' />
                </td>



                <td class="typ">
<%--                    <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' />--%>
                </td>
                <td class="komentarz">
<%--                    <asp:TextBox ID="KomentarzTextBox" runat="server" Text='<%# Bind("Komentarz") %>' />--%>
                </td>
                <td class="check">
                    <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' />
                </td>
                <td>
                    <asp:TextBox ID="KolejnoscTextBox" runat="server" Text='<%# Bind("Kolejnosc") %>' />
                </td>




                <td class="control" rowspan="2">
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
                </td>
            </tr>
            <tr class="eit">
                <td class="sql" colspan="12">
                    <asp:TextBox ID="tbSql" runat="server" Text='<%# Bind("Sql") %>' TextMode="MultiLine" Rows="20" />
                </td>
            </tr>
        </EditItemTemplate>
        <EmptyDataTemplate>
            <table id="Table1" runat="server" style="">
                <tr>
                    <td>
                        Brak danych<br />
                        <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj" CssClass="button100" />
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table runat="server" >
                <tr runat="server">
                    <td runat="server" class="list">
                        <table ID="itemPlaceholderContainer" runat="server" border="0" style="" class="table table-hover">
                            <tr runat="server" style="">
                                <th runat="server"></th>
                                <th><asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Id" ToolTip="Sortuj" Text="Id" /></th>
                                <th><asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="Grupa" ToolTip="Sortuj" Text="Grupa" /></th>
                                <th><asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="MenuText" ToolTip="Sortuj" Text="Nazwa" /></th>
                                <th><asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="ToolTip" ToolTip="Sortuj" Text="Opis" /></th>
                                <th><asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="Command" ToolTip="Sortuj" Text="Komenda" /></th>
                                <th><asp:LinkButton ID="LinkButton8" runat="server" CommandName="Sort" CommandArgument="Par1" ToolTip="Sortuj" Text="Par1" /></th>
                                <th><asp:LinkButton ID="LinkButton9" runat="server" CommandName="Sort" CommandArgument="Par2" ToolTip="Sortuj" Text="Par2" /></th>
                                <th><asp:LinkButton ID="LinkButton6" runat="server" CommandName="Sort" CommandArgument="Typ" ToolTip="Sortuj" Text="Typ" /></th>
                                <th><asp:LinkButton ID="LinkButton7" runat="server" CommandName="Sort" CommandArgument="Comment" ToolTip="Sortuj" Text="Komentarz" /></th>
                                <th><asp:LinkButton ID="LinkButton10" runat="server" CommandName="Sort" CommandArgument="Aktywny" ToolTip="Sortuj" Text="Aktywny" /></th>
                                <th><asp:LinkButton ID="LinkButton11" runat="server" CommandName="Sort" CommandArgument="Kolejnosc" ToolTip="Sortuj" Text="Kolejność" /></th>
                                <th id="Th1" runat="server" class="control">
                                    <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Insert" />
                                </th>
                            </tr>
                            <tr ID="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
    </asp:ListView>

    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
        DeleteCommand="DELETE FROM [SqlMenu] WHERE [Id] = @Id" 
        InsertCommand="INSERT INTO [SqlMenu] ([Grupa], [ParentId], [MenuText], [MenuTextEN], [ToolTip], [ToolTipEN], [Command], [Kolejnosc], [Aktywny], [Image], [Rights], [Par1], [Par2], [Sql], [SqlParams], [Mode]) VALUES (@Grupa, @ParentId, @MenuText, @MenuTextEN, @ToolTip, @ToolTipEN, @Command, @Kolejnosc, @Aktywny, @Image, @Rights, @Par1, @Par2, @Sql, @SqlParams, @Mode)" 
        SelectCommand="SELECT * FROM [SqlMenu] WHERE ([Grupa] = @Grupa) ORDER BY [Id]" 
        UpdateCommand="UPDATE [SqlMenu] SET [Grupa] = @Grupa, [ParentId] = @ParentId, [MenuText] = @MenuText, [MenuTextEN] = @MenuTextEN, [ToolTip] = @ToolTip, [ToolTipEN] = @ToolTipEN, [Command] = @Command, [Kolejnosc] = @Kolejnosc, [Aktywny] = @Aktywny, [Image] = @Image, 
            --[Rights] = @Rights, 
            [Par1] = @Par1, [Par2] = @Par2, [Sql] = @Sql, [SqlParams] = @SqlParams, [Mode] = @Mode WHERE [Id] = @Id" OnInserted="SqlDataSource3_Inserted">
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="Grupa" Type="String" />
            <asp:Parameter Name="ParentId" Type="Int32" />
            <asp:Parameter Name="MenuText" Type="String" />
            <asp:Parameter Name="MenuTextEN" Type="String" />
            <asp:Parameter Name="ToolTip" Type="String" />
            <asp:Parameter Name="ToolTipEN" Type="String" />
            <asp:Parameter Name="Command" Type="String" />
            <asp:Parameter Name="Kolejnosc" Type="Int32" />
            <asp:Parameter Name="Aktywny" Type="Boolean" />
            <asp:Parameter Name="Image" Type="String" />
            <asp:Parameter Name="Rights" Type="String" />
            <asp:Parameter Name="Par1" Type="String" />
            <asp:Parameter Name="Par2" Type="String" />
            <asp:Parameter Name="Sql" Type="String" />
            <asp:Parameter Name="SqlParams" Type="String" />
            <asp:Parameter Name="Mode" Type="Int32" />
        </InsertParameters>
        <SelectParameters>
            <asp:Parameter DefaultValue="REPORT" Name="Grupa" Type="String" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="Grupa" Type="String" />
            <asp:Parameter Name="ParentId" Type="Int32" />
            <asp:Parameter Name="MenuText" Type="String" />
            <asp:Parameter Name="MenuTextEN" Type="String" />
            <asp:Parameter Name="ToolTip" Type="String" />
            <asp:Parameter Name="ToolTipEN" Type="String" />
            <asp:Parameter Name="Command" Type="String" />
            <asp:Parameter Name="Kolejnosc" Type="Int32" />
            <asp:Parameter Name="Aktywny" Type="Boolean" />
            <asp:Parameter Name="Image" Type="String" />
            <asp:Parameter Name="Rights" Type="String" />
            <asp:Parameter Name="Par1" Type="String" />
            <asp:Parameter Name="Par2" Type="String" />
            <asp:Parameter Name="Sql" Type="String" />
            <asp:Parameter Name="SqlParams" Type="String" />
            <asp:Parameter Name="Mode" Type="Int32" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>

</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" CancelSelectOnNullParameter="false" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [Scheduler] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Scheduler] ([Typ], [Grupa], [SQL], [Aktywny], [Komentarz], [Kolejnosc], [Wersja], [WersjaData], [AutorId]) VALUES (@Typ, @Grupa, @SQL, @Aktywny, @Komentarz, @Kolejnosc, @Wersja, @WersjaData, @AutorId)" 
    SelectCommand="
SELECT * FROM [Scheduler] 
where @typ is null or Typ = @typ
ORDER BY [Kolejnosc]
        " 
    UpdateCommand="UPDATE [Scheduler] SET [Typ] = @Typ, [Grupa] = @Grupa, [SQL] = @SQL, [Aktywny] = @Aktywny, [Komentarz] = @Komentarz, [Kolejnosc] = @Kolejnosc, [Wersja] = @Wersja, [WersjaData] = @WersjaData, [AutorId] = @AutorId WHERE [Id] = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlFilter" Name="typ" PropertyName="SelectedValue" Type="String"/>
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Typ" Type="String" />
        <asp:Parameter Name="Grupa" Type="String" />
        <asp:Parameter Name="SQL" Type="String" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
        <asp:Parameter Name="Komentarz" Type="String" />
        <asp:Parameter Name="Kolejnosc" Type="Int32" />
        <asp:Parameter Name="Wersja" Type="String" />
        <asp:Parameter Name="WersjaData" Type="DateTime" />
        <asp:Parameter Name="AutorId" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Typ" Type="String" />
        <asp:Parameter Name="Grupa" Type="String" />
        <asp:Parameter Name="SQL" Type="String" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
        <asp:Parameter Name="Komentarz" Type="String" />
        <asp:Parameter Name="Kolejnosc" Type="Int32" />
        <asp:Parameter Name="Wersja" Type="String" />
        <asp:Parameter Name="WersjaData" Type="DateTime" />
        <asp:Parameter Name="AutorId" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>

