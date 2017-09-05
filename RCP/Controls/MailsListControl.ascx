<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailsListControl.ascx.cs" Inherits="HRRcp.Controls.MailsListControl" %>


<asp:ListView ID="lvMails" runat="server" DataSourceID="dsMailing" 
    DataKeyNames="Typ" onitemcommand="lvMails_ItemCommand" 
    onitemdatabound="lvMails_ItemDataBound" >
    <ItemTemplate>
        <tr class="it" style="">
            <td class="col1">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" />
                <asp:Label ID="OpisLabel" class="opis" runat="server" Text='<%# Eval("Opis") %>' /><br />
                <table>
                    <tr class="row1">
                        <td class="col1">
                            <span class="t1">Temat:&nbsp;&nbsp;</span> 
                        </td>
                        <td class="col2">
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
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" /><br />
                <asp:Button ID="TestButton" runat="server" CommandName="Send" CommandArgument='<%# Eval("Typ") + "|" + Eval("Grupa") %>' Text="Testuj" />
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
    <LayoutTemplate>
        <div id="Mailing">
            <table id="Table2" runat="server" class="ListView1">
                <tr id="Tr1" runat="server">
                    <td id="Td1" runat="server">
                        <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr id="Tr2" runat="server" style="">
                                <th id="Th1" class="col1" runat="server">Wzory wiadomości e-mail </th>
                                <th class="control" id="Th2" runat="server"></th>
                            </tr>
                            <tr ID="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="Tr3" runat="server">
                    <td id="Td2" runat="server" style=""></td>
                </tr>
            </table>
        </div>
    </LayoutTemplate>
    <EditItemTemplate>
        <tr class="eit" style="">
            <td class="col1">
                <asp:CheckBox ID="AktywnyCheckBox" runat="server" Checked='<%# Bind("Aktywny") %>' />
                <asp:Label ID="OpisLabel" class="opis" runat="server" Text='<%# Eval("Opis") %>' /><br />
                <table>
                    <tr class="row1">
                        <td class="col1">
                            <span class="t1">Temat:&nbsp;&nbsp;</span><br />
                        </td>
                        <td class="col2">
                            <asp:TextBox ID="TematTextBox" class="subject textbox" runat="server" Text='<%# Bind("Temat") %>' onclick="selectControl(this)" /><br />
                        </td>
                    </tr>
                    <tr class="row2">
                        <td class="col1">
                            <span class="t1">Treść:</span><br />
                        </td>
                        <td class="col2">
                            <asp:TextBox ID="TrescTextBox" class="body textbox" runat="server" Text='<%# Bind("Tresc") %>' onclick="selectControl(this)" Rows="6" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr class="row3" id="rowZnaczniki" runat="server">
                        <td class="col1">
                            <span class="t1">Wstaw:</span><br />
                        </td>
                        <td class="col2">
                            <div class="znaczniki">
                                <asp:PlaceHolder ID="phZnaczniki" runat="server">
                                </asp:PlaceHolder>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" /><br />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" /><br />
            </td>
        </tr>
    </EditItemTemplate>
</asp:ListView>

<asp:SqlDataSource ID="dsMailing" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [Mailing] WHERE [Typ] = @Typ" 
    InsertCommand="INSERT INTO [Mailing] ([Typ], [Opis], [Temat], [Tresc], [Aktywny]) VALUES (@Typ, @Opis, @Temat, @Tresc, @Aktywny)" 
    SelectCommand="SELECT * FROM [Mailing] ORDER BY [Opis]" 
    UpdateCommand="UPDATE [Mailing] SET [Temat] = @Temat, [Tresc] = @Tresc, [Aktywny] = @Aktywny WHERE [Typ] = @Typ">
    <DeleteParameters>
        <asp:Parameter Name="Typ" Type="String" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Temat" Type="String" />
        <asp:Parameter Name="Tresc" Type="String" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
        <asp:Parameter Name="Typ" Type="String" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Typ" Type="String" />
        <asp:Parameter Name="Opis" Type="String" />
        <asp:Parameter Name="Temat" Type="String" />
        <asp:Parameter Name="Tresc" Type="String" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>
