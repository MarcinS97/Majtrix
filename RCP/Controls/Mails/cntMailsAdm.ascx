<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntMailsAdm.ascx.cs" Inherits="HRRcp.Controls.cntMailsAdm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register src="cntMailsZnaczniki.ascx" tagname="cntMailsZnaczniki" tagprefix="uc1" %>



<!-- group box -->
<table class="GroupBox1" width="100%"><tr><td class="tl" >&nbsp;&nbsp;</td><td class="th" ><div class="title">
    WYSYŁANIE POWIADOMIEŃ MAILOWYCH
    </div></td><td class="tr" >&nbsp;&nbsp;&nbsp;</td></tr><tr><td class="vl"></td><td>
    <!-- group box content -->
    <table class="table3C">
        <tr>
            <td class="col1">
                Serwer SMTP:<br />
                <span class="helpu">Nazwa lub adres ip serwera:</span>
                <br />
                <br />
                <br />
                <span class="helpu">Hasło:</span>
            </td>
            <td class="col2">
                <asp:TextBox ID="tbSMTP1" class="textbox form-control" runat="server" Width="300px" MaxLength="100"></asp:TextBox>
                
            <asp:TextBox ID="tbSMTP2" class="textbox form-control" runat="server" Width="300px" MaxLength="100"></asp:TextBox>
                
            
                <asp:TextBox ID="tbSMTP3" class="textbox form-control" TextMode="Password" runat="server" Width="300px" MaxLength="100"></asp:TextBox>

            </td>
        </tr>
        <tr>
            <td class="col1 oneline">
                Adres e-mail nadawcy:
            </td>
            <td class="col2">
                <asp:TextBox ID="tbEmail" class="textbox form-control" runat="server" Width="300px" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="col1 oneline">
                Adres aplikacji:
            </td>
            <td class="col2">
                <asp:TextBox ID="tbAppAddr" class="textbox form-control" runat="server" Width="300px" MaxLength="100"></asp:TextBox>
                <asp:Button ID="btGetAppAddr" class="button75 btn btn-default" runat="server" Text="Pobierz" onclick="btGetAppAddr_Click" />                
            </td>
        </tr>
        <tr id="trOkres" runat="server" class="lastline">
            <td class="col1">
                Przypomnienie o zakończeniu:<br />
                <span class="t4">na ile dni przed końcem okresu rozliczeniowego wysłać</span>
            </td>
            <td class="col2">
                <asp:TextBox ID="tbMonitIlDni" class="textbox form-control" runat="server" Width="80px" MaxLength="2"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" TargetControlID="tbMonitIlDni" FilterType="Numbers" />
            </td>
        </tr>
    </table>
    <!-- group box end content -->
    </td><td class="vr" >&nbsp;</td></tr><tr><td class="bl"></td><td class="bh"></td><td class="br"></td></tr>
</table>
<table width="100%" cellpadding="0px" cellspacing="0px" style="border-collapse:collapse; vertical-align:top">
    <tr>
        <td id="paParEdit1" runat="server" align="right" style="padding:8px 0px 0px 0px;">
            <asp:Button ID="btEdit" class="button75 btn btn-primary" runat="server" Text="Edycja" onclick="btEdit_Click" />
            <asp:Button ID="btSave" class="button75 btn btn-success" runat="server" Text="Zapisz" Visible="False" onclick="btSave_Click" />
            <asp:Button ID="btCancel" class="button75 btn btn-default" runat="server" Text="Anuluj" Visible="False" onclick="btCancel_Click" />
        </td>
    </tr>
</table>
<br />


<div class="tbMailsFilter">
    <asp:DropDownList ID="ddlFilter" runat="server" DataSourceID="SqlDataSource2" DataTextField="Nazwa" DataValueField="Grupa" AutoPostBack="True" CssClass="form-control">
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
</div>
<br />

<asp:ListView ID="lvMails" runat="server" DataSourceID="dsMailing" DataKeyNames="Id" 
    InsertItemPosition='<%#InsertItemPosition%>'
    onitemcommand="lvMails_ItemCommand" 
    onitemdatabound="lvMails_ItemDataBound" 
    onitemupdating="lvMails_ItemUpdating" 
    onitemcreated="lvMails_ItemCreated" oniteminserted="lvMails_ItemInserted" 
    oniteminserting="lvMails_ItemInserting" >
    <ItemTemplate>
        <tr class="it" >
            <td class="col1">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" CssClass="check" Checked='<%# Eval("Aktywny") %>' Enabled="false" />
                <asp:Label ID="OpisLabel" class="opis" runat="server" Text='<%# Eval("Opis") %>' /><br />
                <table>
                    <tr class="row1">
                        <td class="col1">
                            <span class="t1">Temat:</span> 
                        </td><td class="col2">
                            <asp:Label ID="TematLabel" class="textbox subject readonly" runat="server" Text='<%# Eval("Temat") %>' />
                        </td>
                    </tr>
                    <tr class="row2">
                        <td class="col1">
                            <span class="t1">Treść:</span> 
                        </td>
                        <td class="col2">
                            <asp:Label ID="TrescLabel" class="textbox body readonly" runat="server" Text='<%# Eval("Tresc") %>' />
                        </td>
                    </tr>
                </table>
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" CssClass="btn btn-default" /><br />
                <asp:Button ID="TestButton" runat="server" CommandName="Send" CommandArgument='<%# Eval("Typ") + "|" + Eval("Grupa") %>' Text="Testuj" CssClass="btn btn-default" />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table id="Table1" class="edt" runat="server" style="">
            <tr>
                <td>
                    Brak danych
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="col1">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" CssClass="check" Checked='<%# Bind("Aktywny") %>' />
                <asp:Label ID="OpisLabel" class="opis" runat="server" Text='<%# Eval("Opis") %>' /><br />
                <table>
                    <tr id="trOpis" runat="server" class="row0" visible="false">
                        <td class="col1">
                            <span class="t1">Opis:</span>
                        </td>
                        <td class="col2">
                            <asp:TextBox ID="tbOpis" class="opis textbox form-control" runat="server" onclick="javascript:selectControl(null);" Text='<%# Bind("Opis") %>' />
                        </td>                        
                    </tr>
                    <tr id="trTyp" runat="server" class="row0" visible="false">
                        <td class="col1">
                            <span class="t1">Typ:</span>
                        </td>
                        <td class="col2">
                            <asp:TextBox ID="tbTyp" class="typ textbox form-control" runat="server" onclick="javascript:selectControl(null);" Text='<%# Bind("Typ") %>' />
                        </td>                        
                    </tr>
                    <tr id="trGrupa" runat="server" class="row0" visible="false">
                        <td class="col1">
                            <span class="t1">Grupa:</span>
                        </td>
                        <td class="col2">
                            <asp:DropDownList ID="ddlGrupa" runat="server" DataSourceID="SqlDataSource1" DataTextField="Nazwa" DataValueField="Grupa" SelectedValue='<%# Bind("Grupa") %>'
                                AutoPostBack="true" 
                                Visible="true" CssClass="form-control"
                                OnSelectedIndexChanged="ddlGrupa_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                                SelectCommand="
                                    select distinct ISNULL(M.Grupa, G.Grupa) as Grupa, ISNULL(M.Grupa, G.Grupa) + ISNULL(' - ' + G.Opis,'') as Nazwa from Mailing M
                                    full outer join MailingGrupy G on G.Grupa = M.Grupa
                                    order by 1
                                ">
                            </asp:SqlDataSource>
                            <asp:TextBox ID="tbGrupa" class="typ textbox form-control" runat="server" onclick="javascript:selectControl(null);" Text='<%# Bind("Grupa") %>' 
                                AutoPostBack="true" 
                                OnTextChanged="tbGrupa_TextChanged"/>


                                <%--
                            SelectCommand="select Grupa + ISNULL(' - ' + Opis,'') as Nazwa, Grupa from MailingGrupy order by Grupa"
                                --%>
                        </td>                        
                    </tr>
                    <tr class="row1">
                        <td class="col1">
                            <span class="t1">Temat:</span><br />
                        </td>
                        <td class="col2">
                            <asp:TextBox ID="TematTextBox" class="subject textbox form-control" runat="server" Text='<%# Bind("Temat") %>' onclick="javascript:selectControl(this);" /><br />
                        </td>
                    </tr>
                    <tr class="row2">
                        <td class="col1">
                            <span class="t1">Treść:</span><br />
                        </td>
                        <td class="col2">
                            <asp:TextBox ID="TrescTextBox" class="body textbox form-control" runat="server" Text='<%# Bind("Tresc") %>' onclick="javascript:selectControl(this);" Rows="15" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr class="row3" id="rowZnaczniki" runat="server" visible="false">
                        <td class="col1">
                            <span class="t1">Wstaw:</span>
                        </td>
                        <td class="col2">
                            <div id="paZnaczniki" class="znaczniki" runat="server" visible='<%# !IsSuperuser %>'>
                                <asp:PlaceHolder ID="phZnaczniki" runat="server"></asp:PlaceHolder>
                            </div>
                            <uc1:cntMailsZnaczniki ID="cntMailsZnaczniki1" runat="server" Visible='<%# IsSuperuser %>' Grupa='<%# Eval("Grupa") %>' />
                        </td>
                    </tr>
                </table>
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" CssClass="btn btn-success" /><br />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" CssClass="btn btn-default" /><br />
                <br />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" CssClass="btn btn-danger" Visible='<%# IsSuperuser %>' />
            </td>
        </tr>
    </EditItemTemplate>
    <InsertItemTemplate>
        <tr class="iit">
            <td class="col1">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" CssClass="check" Checked='<%# Bind("Aktywny") %>' />
                <asp:Label ID="OpisLabel" class="opis" runat="server" Text='<%# Eval("Opis") %>' /><br />
                <table>
                    <tr class="row0" >
                        <td class="col1">
                            <span class="t1">Opis:</span>
                        </td>
                        <td class="col2">
                            <asp:TextBox ID="tbOpis" class="opis textbox form-control" runat="server" onclick="javascript:selectControl(null);" Text='<%# Bind("Opis") %>' />
                        </td>                        
                    </tr>
                    <tr class="row0" >
                        <td class="col1">
                            <span class="t1">Typ:</span>
                        </td>
                        <td class="col2">
                            <asp:TextBox ID="tbTyp" class="typ textbox form-control" runat="server" onclick="javascript:selectControl(null);" Text='<%# Bind("Typ") %>' />
                        </td>                        
                    </tr>
                    <tr class="row0" >
                        <td class="col1">
                            <span class="t1">Grupa:</span>
                        </td>
                        <td class="col2">
                            <asp:DropDownList ID="ddlGrupa" runat="server" DataSourceID="SqlDataSource1" DataTextField="Nazwa" DataValueField="Grupa" 
                                AutoPostBack="true"  CssClass="form-control"
                                Visible="true"
                                OnSelectedIndexChanged="ddlGrupaInsert_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                                SelectCommand="
                                    select distinct ISNULL(M.Grupa, G.Grupa) as Grupa, ISNULL(M.Grupa, G.Grupa) + ISNULL(' - ' + G.Opis,'') as Nazwa from Mailing M
                                    full outer join MailingGrupy G on G.Grupa = M.Grupa
                                    order by 1
                                ">
                            </asp:SqlDataSource>
                            <asp:TextBox ID="tbGrupa" class="typ textbox form-control" runat="server" onclick="javascript:selectControl(null);" Text='<%# Bind("Grupa") %>' 
                                AutoPostBack="true" 
                                OnTextChanged="tbGrupa_TextChanged"/>
                        </td>                        
                    </tr>
                    <tr class="row1">
                        <td class="col1">
                            <span class="t1">Temat:</span><br />
                        </td>
                        <td class="col2">
                            <asp:TextBox ID="TematTextBox" class="subject textbox form-control" runat="server" Text='<%# Bind("Temat") %>' onclick="javascript:selectControl(this);" /><br />
                        </td>
                    </tr>
                    <tr class="row2">
                        <td class="col1">
                            <span class="t1">Treść:</span><br />
                        </td>
                        <td class="col2">
                            <asp:TextBox ID="TrescTextBox" class="body textbox form-control" runat="server" Text='<%# Bind("Tresc") %>' onclick="javascript:selectControl(this);" Rows="15" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr class="row3" id="rowZnaczniki" runat="server" visible="false">
                        <td class="col1">
                            <span class="t1">Wstaw:</span>
                        </td>
                        <td class="col2">
                            <uc1:cntMailsZnaczniki ID="cntMailsZnaczniki1" runat="server" Visible='<%# IsSuperuser %>' Grupa="NEW"/>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Zapisz" CssClass="btn btn-success" /><br />
                <asp:Button ID="CancelButton" runat="server" CommandName="Clear" Text="Czyść" CssClass="btn btn-default" />
            </td>
        </tr>
    </InsertItemTemplate>
    <LayoutTemplate>
        <div id="Mailing">
            <table id="Table2" runat="server" class="ListView1">
                <tr id="Tr1" runat="server">
                    <td id="Td1" runat="server">
                        <table id="itemPlaceholderContainer" runat="server" border="0" style="" class="table">
                            <tr id="Tr2" runat="server" style="">
                                <th id="Th1" class="col1" runat="server">
                                    Wzory wiadomości e-mail
                                </th>
                                <th class="control" id="Th2" runat="server">
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="Tr3" runat="server">
                    <td id="Td2" runat="server" style="">
                    </td>
                </tr>
            </table>
        </div>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="dsMailing" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [Mailing] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [Mailing] ([Typ], [Grupa], [Opis], [Temat], [Tresc], [Aktywny]) VALUES (@Typ, @Grupa, @Opis, @Temat, @Tresc, @Aktywny)" 
    SelectCommand="SELECT * FROM [Mailing] ORDER BY [Opis]" 
    UpdateCommand="UPDATE [Mailing] SET [Typ] = @Typ, [Grupa] = @Grupa, [Opis] = @Opis, [Temat] = @Temat, [Tresc] = @Tresc, [Aktywny] = @Aktywny WHERE [Id] = @Id"
    FilterExpression="'{0}' is null or Grupa='{0}'" >
    <FilterParameters>
        <asp:ControlParameter ControlID="ddlFilter" Name="typ" PropertyName="SelectedValue" />
    </FilterParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Grupa" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Temat" Type="String" />
        <asp:Parameter Name="Tresc" Type="String" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
        <asp:Parameter Name="Typ" Type="String" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Typ" Type="String" />
        <asp:Parameter Name="Grupa" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Temat" Type="String" />
        <asp:Parameter Name="Tresc" Type="String" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>

<%--
    UpdateCommand="UPDATE [Mailing] SET [Typ] = @Typ, [Grupa] = @Grupa, [Opis] = @Opis, [Temat] = @Temat, [Tresc] = @Tresc, [Aktywny] = @Aktywny WHERE [Id] = @Id">
--%>