<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdmInfoControl.ascx.cs" Inherits="HRRcp.Controls.AdmInfoControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

<asp:ListView ID="lvTexts" runat="server" DataSourceID="dsInformacje" 
    DataKeyNames="Typ" 
    onitemcommand="lvTexts_ItemCommand" 
    ondatabound="lvTexts_DataBound" >
    <ItemTemplate>
        <tr class="it" style="">
            <td class="col1">
                <asp:Label ID="OpisLabel" class="opis" runat="server" Text='<%# Eval("Opis") %>' />
                <br />
                <asp:Panel ID="paTekst1" class="textbox readonly form-control" runat="server" ScrollBars="Auto" Height="100px" runat="server">
                    <asp:Literal ID="Tekst1Literal" runat="server" Text='<%# Eval("Tekst") %>'>
                    </asp:Literal>
                </asp:Panel>
            </td>
            <td id="control" runat="server" class="control">
                <asp:Button ID="EditButton" runat="server" CssClass="btn btn-default" CommandName="Edit" Text="Edytuj" /><br />
                <asp:Button ID="TestButton" runat="server" CssClass="btn btn-primary" CommandName="Test" CommandArgument='<%# Eval("Typ") %>' Text="Testuj" />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table id="Table1" class="edt" runat="server" style="">
            <tr>
                <td>Brak danych </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <div id="Informacje">
            <table id="Table2" runat="server" class="ListView1">
                <tr id="Tr1" runat="server">
                    <td id="Td1" runat="server">
                        <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr id="Tr2" runat="server" style="">
                                <th id="Th2" class="col1" runat="server">Teksty informacji i pomocy</th>
                                <th class="control" id="h_control" runat="server"></th>
                            </tr>
                            <tr ID="itemPlaceholder" runat="server">
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
                <asp:Label ID="OpisLabel" class="opis" runat="server" Text='<%# Eval("Opis") %>' />
                <br />
                <div>
                    <cc1:Editor ID="Tekst1Editor" runat="server" NoUnicode="True" Content = '<%# Bind("Tekst") %>'/>
                </div>
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" /><br />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
            </td>
        </tr>
    </EditItemTemplate>
</asp:ListView>

<asp:SqlDataSource ID="dsInformacje" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT * FROM [Teksty] ORDER BY [Opis]" 
    UpdateCommand="UPDATE [Teksty] SET [Tekst] = @Tekst WHERE [Typ] = @Typ">
    <UpdateParameters>
        <asp:Parameter Name="Tekst" Type="String" />
        <asp:Parameter Name="Typ" Type="String" />
    </UpdateParameters>
</asp:SqlDataSource>

