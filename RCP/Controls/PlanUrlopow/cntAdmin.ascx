<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntAdmin.ascx.cs" Inherits="HRRcp.Controls.cntAdmin" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="../DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>

<asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" InsertItemPosition="LastItem">
    <ItemTemplate>
        <tr class="it">
            <td>
                <asp:Label ID="RokLabel" runat="server" Text='<%# Eval("Rok") %>' />
            </td>
            <td class="data">
                <asp:Label ID="PlanStopLabel" runat="server" Text='<%# Eval("PlanStop", "{0:d}") %>' />
            </td>
            <td class="num">
                <asp:Label ID="Min14Label" runat="server" Text='<%# Eval("Min14") %>' />
            </td>
            <td class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
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
            <td class="data">
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("PlanStop") %>' ValidationGroup="ivg"/>
            </td>
            <td class="num">
                <asp:TextBox ID="Min14TextBox" runat="server" MaxLength="2" Text='<%# Bind("Min14") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd" ValidationGroup="ivg" ControlToValidate="Min14TextBox" SetFocusOnError="True" Display="Dynamic" >
                </asp:RequiredFieldValidator>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true" 
                    TargetControlID="Min14TextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789" />
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="data">
                <asp:Label ID="RokLabel" runat="server" Text='<%# Eval("Rok") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("PlanStop") %>' ValidationGroup="evg"/>
            </td>
            <td class="num">
                <asp:TextBox ID="Min14TextBox" runat="server" MaxLength="2" Text='<%# Bind("Min14") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Błąd" ValidationGroup="evg" ControlToValidate="Min14TextBox" SetFocusOnError="True" Display="Dynamic" >
                </asp:RequiredFieldValidator>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true" 
                    TargetControlID="Min14TextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789" />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">
                                Rok</th>
                            <th runat="server">
                                Koniec okresu planowania</th>
                            <th runat="server">
                                Minimalna ilość urlopu dla weryfikacji</th>
                            <th id="Th1" runat="server">
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

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [PlanUrlopowParam] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [PlanUrlopowParam] ([Rok], [PlanStop], [Min14], [Blokada], [Status], [IdPracownika]) VALUES (dbo.boy(@PlanStop), @PlanStop, @Min14, @Blokada, @Status, @IdPracownika)" 
    SelectCommand="SELECT [Id], YEAR([Rok]) as Rok, [PlanStop], [Min14], [Blokada], [Status], [IdPracownika] FROM [PlanUrlopowParam] ORDER BY [Rok]" 
    UpdateCommand="UPDATE [PlanUrlopowParam] SET [Rok] = dbo.boy(@PlanStop), [PlanStop] = @PlanStop, [Min14] = @Min14, [Blokada] = @Blokada, [Status] = @Status, [IdPracownika] = @IdPracownika WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Rok" Type="DateTime" />
        <asp:Parameter Name="PlanStop" Type="DateTime" />
        <asp:Parameter Name="Min14" Type="Int32" />
        <asp:Parameter Name="Blokada" Type="Boolean" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Rok" Type="DateTime" />
        <asp:Parameter Name="PlanStop" Type="DateTime" />
        <asp:Parameter Name="Min14" Type="Int32" />
        <asp:Parameter Name="Blokada" Type="Boolean" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="IdPracownika" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>

