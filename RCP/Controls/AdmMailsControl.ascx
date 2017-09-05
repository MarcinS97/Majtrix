<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdmMailsControl.ascx.cs" Inherits="HRRcp.Controls.AdmMailsControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

<!-- group box -->
<table class="GroupBox1" width="100%"><tr><td class="tl" >&nbsp;&nbsp;</td><td class="th" ><div class="title">
    WYSYŁANIE POWIADOMIEŃ MAILOWYCH
    </div></td><td class="tr" >&nbsp;&nbsp;&nbsp;</td></tr><tr><td class="vl"></td><td>
    <!-- group box content -->
    <table class="table3C">
        <tr>
            <td class="col1">
                Serwer SMTP:<br />
                <span class="t4">nazwa lub adres ip serwera</span>
            </td>
            <td class="col2">
                <asp:TextBox ID="tbSMTP" class="textbox" runat="server" Width="300px" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="col1 oneline">
                Adres e-mail nadawcy:
            </td>
            <td class="col2">
                <asp:TextBox ID="tbEmail" class="textbox" runat="server" Width="300px" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="col1 oneline">
                Adres aplikacji:
            </td>
            <td class="col2">
                <asp:TextBox ID="tbAppAddr" class="textbox" runat="server" Width="300px" MaxLength="100"></asp:TextBox>
                <asp:Button ID="btGetAppAddr" class="button75" runat="server" Text="Pobierz" onclick="btGetAppAddr_Click" />                
            </td>
        </tr>
        <tr class="lastline">
            <td class="col1">
                Przypomnienie o zakończeniu:<br />
                <span class="t4">na ile dni przed końcem okresu rozliczeniowego wysłać</span>
            </td>
            <td class="col2">
                <asp:TextBox ID="tbMonitIlDni" class="textbox" runat="server" Width="80px" MaxLength="2"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" TargetControlID="tbMonitIlDni" FilterType="Numbers" />
            </td>
        </tr>
    </table>
    <!-- group box end content -->
    </td><td class="vr" >&nbsp;</td></tr><tr><td class="bl"></td><td class="bh"></td><td class="br"></td></tr>
</table>
<!-- group box end -->

<table width="100%" cellpadding="0px" cellspacing="0px" style="border-collapse:collapse; vertical-align:top">
    <tr>
        <td id="paParEdit1" runat="server" align="right" style="padding:8px 0px 0px 0px;">
            <asp:Button ID="btEdit" class="button75" runat="server" Text="Edycja" onclick="btEdit_Click" />
            <asp:Button ID="btSave" class="button75" runat="server" Text="Zapisz" Visible="False" onclick="btSave_Click" />
            <asp:Button ID="btCancel" class="button75" runat="server" Text="Anuluj" Visible="False" onclick="btCancel_Click" />
        </td>
    </tr>
</table>
<div class="spacer16"></div>

<asp:ListView ID="lvMails" runat="server" DataSourceID="dsMailing" DataKeyNames="Typ" 
    onitemcommand="lvMails_ItemCommand" 
    onitemdatabound="lvMails_ItemDataBound" >
    <ItemTemplate>
        <tr class="it" style="">
            <td class="col1">
                <asp:CheckBox ID="AktywnyCheckBox" class="check" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" />
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
                        <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr id="Tr2" runat="server" style="">
                                <th id="Th1" class="col1" runat="server">
                                    Treść powiadomień mailowych
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
    <EditItemTemplate>
        <tr class="eit" style="">
            <td class="col1">
                <asp:CheckBox ID="AktywnyCheckBox" class="check" runat="server" Checked='<%# Bind("Aktywny") %>' />
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
                    <tr class="row3" id="rowZnaczniki" runat="server" visible="true">
                        <td class="col1">
                            <span class="t1">Wstaw:</span><br />
                        </td>
                        <td class="col2">
                            <div class="znaczniki">
                                <asp:PlaceHolder ID="phZnaczniki" runat="server"></asp:PlaceHolder>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" /><br />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
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



