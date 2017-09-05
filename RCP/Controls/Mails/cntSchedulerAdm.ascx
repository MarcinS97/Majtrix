<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSchedulerAdm.ascx.cs" Inherits="HRRcp.Controls.Mails.cntSchedulerAdm" %>

<div id="paSchedulerAdm" runat="server" class="cntSchedulerAdm">
    <asp:HiddenField ID="hidGrupa" runat="server" Visible="false"/>
    <asp:HiddenField ID="hidNotGrupa" runat="server" Visible="false"/>
    <div class="tbMailsFilter">
        <asp:DropDownList ID="ddlFilter" runat="server" DataSourceID="SqlDataSource2" Visible='<%# IsFilterVisible %>' 
            DataTextField="Nazwa" DataValueField="Grupa" AutoPostBack="True" 
            onselectedindexchanged="ddlFilter_SelectedIndexChanged" CssClass="form-control" >
        </asp:DropDownList>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
            SelectCommand="
select null as Grupa, 'Pokaż wszystkie zadania' as Nazwa, 1 as Sort
union all 
select distinct M.Grupa, M.Grupa as Nazwa, 2 as Sort 
from Scheduler M
where (@notgrupa2 is null or M.Grupa != @notgrupa2)
order by Sort, Grupa
            ">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidNotGrupa" Name="notgrupa2" PropertyName="Value" Type="String"/>
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <br />

    <asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" DataSourceID="SqlDataSource1" 
        InsertItemPosition="None" 
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
                <td class="grupa">
                    <asp:Label ID="GrupaLabel" runat="server" Text='<%# Eval("Grupa") %>' />
                </td>
                <td class="typ">
                    <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
                    <asp:HiddenField ID="hidSql" runat="server" Visible="false" Value='<%# Eval("SQL") %>' />
                </td>
                <td class="komentarz">
                    <asp:Label ID="KomentarzLabel" runat="server" Text='<%# Eval("Komentarz") %>' />
                </td>                
<%--
                <td>
                    <asp:Label ID="WersjaLabel" runat="server" Text='<%# Eval("Wersja") %>' />
                </td>
                <td>
                    <asp:Label ID="WersjaDataLabel" runat="server" Text='<%# Eval("WersjaData") %>' />
                </td>
--%>                
                <td class="check">
                    <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" />
                </td>
                <td class="num">
                    <asp:Label ID="KolejnoscLabel" runat="server" Text='<%# Eval("Kolejnosc") %>' />
                </td>
<%--
                <td>
                    <asp:Label ID="SQLLabel" runat="server" Text='<%# Eval("SQL") %>' />
                </td>
                <td>
                    <asp:Label ID="AutorIdLabel" runat="server" Text='<%# Eval("Autor") %>' />
                </td>
--%>
                <td class="control">
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-default" />
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-danger" />
                </td>
            </tr>
        </ItemTemplate>
        <SelectedItemTemplate>
            <tr class="sit">
                <td class="check">
                </td>
                <td class="grupa">
                    <asp:Label ID="GrupaLabel" runat="server" Text='<%# Eval("Grupa") %>' />
                </td>
                <td class="typ">
                    <asp:Label ID="TypLabel" runat="server" Text='<%# Eval("Typ") %>' />
                    <asp:HiddenField ID="hidSql" runat="server" Visible="false" Value='<%# Eval("SQL") %>' />
                </td>
                <td class="komentarz">
                    <asp:Label ID="KomentarzLabel" runat="server" Text='<%# Eval("Komentarz") %>' />
                </td>                
<%--
                <td>
                    <asp:Label ID="WersjaLabel" runat="server" Text='<%# Eval("Wersja") %>' />
                </td>
                <td>
                    <asp:Label ID="WersjaDataLabel" runat="server" Text='<%# Eval("WersjaData") %>' />
                </td>
--%>                
                <td class="check">
                    <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" />
                </td>
                <td class="num">
                    <asp:Label ID="KolejnoscLabel" runat="server" Text='<%# Eval("Kolejnosc") %>' />
                </td>
<%--
                <td>
                    <asp:Label ID="SQLLabel" runat="server" Text='<%# Eval("SQL") %>' />
                </td>
                <td>
                    <asp:Label ID="AutorIdLabel" runat="server" Text='<%# Eval("Autor") %>' />
                </td>
--%>
                <td class="control">
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-default" />
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-danger" />
                </td>
            </tr>
        </SelectedItemTemplate>
        <InsertItemTemplate>
            <tr class="iit">
                <td>
                </td>
                <td class="grupa">
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Grupa") %>' CssClass="form-control" />
<%--
                    <asp:DropDownList ID="ddlGrupa" runat="server" DataSourceID="SqlDataSource1" DataTextField="Nazwa" DataValueField="Grupa" SelectedValue='<%# Bind("Grupa") %>'
                        AutoPostBack="true" 
                        Visible="true"
                        OnSelectedIndexChanged="ddlGrupa_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                        SelectCommand="
                            select distinct ISNULL(M.Grupa, G.Grupa) as Grupa, ISNULL(M.Grupa, G.Grupa) + ISNULL(' - ' + G.Opis,'') as Nazwa from Mailing M
                            full outer join MailingGrupy G on G.Grupa = M.Grupa
                            order by 1
                        ">
                    </asp:SqlDataSource>
                    <asp:TextBox ID="tbGrupa" class="typ textbox" runat="server" onclick="javascript:selectControl(null);" Text='<%# Bind("Grupa") %>' 
                        AutoPostBack="true" 
                        OnTextChanged="tbGrupa_TextChanged"/>
--%>
                </td>
                <td class="typ">
                    <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' CssClass="form-control" />
                </td>
                <td class="komentarz">
                    <asp:TextBox ID="KomentarzTextBox" runat="server" Text='<%# Bind("Komentarz") %>' CssClass="form-control" />
                </td>
                <td class="check">
                    <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>'  />
                </td>
                <td>
                    <asp:TextBox ID="KolejnoscTextBox" runat="server" Text='<%# Bind("Kolejnosc") %>' CssClass="form-control" />
                </td>
<%--
                <td>
                    <asp:TextBox ID="WersjaTextBox" runat="server" Text='<%# Bind("Wersja") %>' />
                </td>
                <td>
                    <asp:TextBox ID="WersjaDataTextBox" runat="server" 
                        Text='<%# Bind("WersjaData") %>' />
                </td>
                <td>
                    <asp:TextBox ID="AutorIdTextBox" runat="server" Text='<%# Bind("AutorId") %>' />
                </td>
--%>
                <td class="control">
                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" CssClass="btn btn-success" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" CssClass="btn btn-default" />
                </td>
            </tr>
        </InsertItemTemplate>
        <EditItemTemplate>
            <tr class="eit">
                <td>
                </td>
                <td class="grupa">
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Grupa") %>' CssClass="form-control" />
<%--
                    <asp:DropDownList ID="ddlGrupa" runat="server" DataSourceID="SqlDataSource1" DataTextField="Nazwa" DataValueField="Grupa" SelectedValue='<%# Bind("Grupa") %>'
                        AutoPostBack="true" 
                        Visible="true"
                        OnSelectedIndexChanged="ddlGrupa_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                        SelectCommand="
                            select distinct ISNULL(M.Grupa, G.Grupa) as Grupa, ISNULL(M.Grupa, G.Grupa) + ISNULL(' - ' + G.Opis,'') as Nazwa from Mailing M
                            full outer join MailingGrupy G on G.Grupa = M.Grupa
                            order by 1
                        ">
                    </asp:SqlDataSource>
                    <asp:TextBox ID="tbGrupa" class="typ textbox" runat="server" onclick="javascript:selectControl(null);" Text='<%# Bind("Grupa") %>' 
                        AutoPostBack="true" 
                        OnTextChanged="tbGrupa_TextChanged"/>
--%>
                </td>
                <td class="typ">
                    <asp:TextBox ID="TypTextBox" runat="server" Text='<%# Bind("Typ") %>' CssClass="form-control" />
                    <asp:HiddenField ID="hidSql" runat="server" Visible="false" Value='<%# Eval("SQL") %>' />
                </td>
                <td class="komentarz">
                    <asp:TextBox ID="KomentarzTextBox" runat="server" Text='<%# Bind("Komentarz") %>' CssClass="form-control" />
                </td>
                <td class="check">
                    <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' />
                </td>
                <td>
                    <asp:TextBox ID="KolejnoscTextBox" runat="server" Text='<%# Bind("Kolejnosc") %>' CssClass="form-control" />
                </td>
<%--
                <td>
                    <asp:TextBox ID="WersjaTextBox" runat="server" Text='<%# Bind("Wersja") %>' />
                </td>
                <td>
                    <asp:TextBox ID="WersjaDataTextBox" runat="server" 
                        Text='<%# Bind("WersjaData") %>' />
                </td>
                <td>
                    <asp:TextBox ID="AutorIdTextBox" runat="server" Text='<%# Bind("AutorId") %>' />
                </td>
--%>
                <td class="control">
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" CssClass="btn btn-success" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-default" />
                </td>
            </tr>
        </EditItemTemplate>
        <EmptyDataTemplate>
            <table id="Table1" runat="server" style="">
                <tr>
                    <td>
                        Brak danych<br />
                        <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj" CssClass="btn btn-primary" />
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table runat="server" >
                <tr runat="server">
                    <td runat="server" class="list">
                        <table ID="itemPlaceholderContainer" runat="server" border="0" style="" class="table">
                            <tr runat="server" style="">
                                <th runat="server"></th>
                                <th runat="server">
                                    Grupa</th>
                                <th runat="server">
                                    Typ</th>
                                <th runat="server">
                                    Komentarz</th>                     
<%--
                                <th runat="server">
                                    Wersja</th>
                                <th runat="server">
                                    Data</th>
--%>                     
                                <th runat="server">
                                    Aktywny</th>
                                <th runat="server">
                                    Kolejnosc</th>
                                <th id="Th1" runat="server" class="control">
                                    <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Insert" CssClass="btn btn-primary" />
                                </th>
                            </tr>
                            <tr ID="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="trSql" runat="server" visible="false">
                    <td id="tdSql" runat="server" class="sql">
                        <asp:TextBox ID="tbSql" Rows="50" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
    </asp:ListView>

</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" CancelSelectOnNullParameter="false" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [Scheduler] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Scheduler] ([Typ], [Grupa], [SQL], [Aktywny], [Komentarz], [Kolejnosc], [Wersja], [WersjaData], [AutorId]) VALUES (@Typ, @Grupa, @SQL, @Aktywny, @Komentarz, @Kolejnosc, @Wersja, @WersjaData, @AutorId)" 
    SelectCommand="
SELECT * FROM [Scheduler] 
where (@grupa is null or Grupa = @grupa)
  and (@grupa2 is null or Grupa = @grupa2)
  and (@notgrupa2 is null or Grupa != @notgrupa2)
ORDER BY Grupa,Kolejnosc
        " 
    UpdateCommand="UPDATE [Scheduler] SET [Typ] = @Typ, [Grupa] = @Grupa, [SQL] = @SQL, [Aktywny] = @Aktywny, [Komentarz] = @Komentarz, [Kolejnosc] = @Kolejnosc, [Wersja] = @Wersja, [WersjaData] = @WersjaData, [AutorId] = @AutorId WHERE [Id] = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlFilter" Name="grupa" PropertyName="SelectedValue" Type="String"/>
        <asp:ControlParameter ControlID="hidGrupa" Name="grupa2" PropertyName="Value" Type="String"/>
        <asp:ControlParameter ControlID="hidNotGrupa" Name="notgrupa2" PropertyName="Value" Type="String"/>
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

