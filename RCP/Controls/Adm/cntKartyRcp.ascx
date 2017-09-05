<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntKartyRcp.ascx.cs" Inherits="HRRcp.Controls.Adm.cntKartyRcp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="../DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>

<asp:HiddenField ID="hidPracId" runat="server" />

<asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" InsertItemPosition="None" 
    onitemcreated="ListView1_ItemCreated" 
    onitemdatabound="ListView1_ItemDataBound" onitemdeleted="ListView1_ItemDeleted" 
    oniteminserted="ListView1_ItemInserted" onitemupdated="ListView1_ItemUpdated" 
    onlayoutcreated="ListView1_LayoutCreated" 
    oniteminserting="ListView1_ItemInserting" 
    onitemupdating="ListView1_ItemUpdating" 
    onitemcommand="ListView1_ItemCommand">
    <ItemTemplate>
        <tr class="it">
            <td class="date">
                <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od", "{0:d}") %>' />
            </td>
            <td class="date">
                <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do", "{0:d}") %>' />
            </td>
            <td id="tdRcpId" runat="server" class="rcpid">
                <asp:Label ID="RcpIdLabel" runat="server" Text='<%# Eval("RcpId") %>' />
            </td>
            <td id="tdNrKarty" runat="server" class="nrkarty">
                <asp:Label ID="NrKartyLabel" runat="server" Text='<%# Eval("NrKarty") %>' />
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
                    Brak przypisanej karty<br />
                    <br />
                    <asp:Button ID="InsertButton" CssClass="button100" runat="server" CommandName="NewRecord" Text="Dodaj" />
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <InsertItemTemplate>
        <tr style="" class="iit">
            <td class="date">
                <uc1:DateEdit ID="deOd" runat="server" Date='<%# Bind("Od") %>' ValidationGroup="vgi" />
            </td>
            <td class="date">
                <uc1:DateEdit ID="deDo" runat="server" Date='<%# Bind("Do") %>' />
            </td>
            <td id="tdRcpId" runat="server" class="rcpid" >
                <asp:TextBox ID="RcpIdTextBox" runat="server" Text='<%# Bind("RcpId") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    CssClass="error"
                    ValidationGroup="vgi"                     
                    ControlToValidate="RcpIdTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" 
                    TargetControlID="RcpIdTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789" />
            </td>
            <td id="tdNrKarty" runat="server" class="nrkarty">
                <asp:TextBox ID="NrKartyTextBox" runat="server" Text='<%# Bind("NrKarty") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    CssClass="error"
                    ValidationGroup="vgi" 
                    ControlToValidate="NrKartyTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" ValidationGroup="vgi"/>
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
                <asp:Button ID="btCancelInsert" runat="server" CommandName="CancelInsert" Text="Anuluj" />
            </td>
        </tr>
        <tr class="iit iit2">
            <td colspan="2">
                <asp:CheckBox ID="cbClosePrev" runat="server" CssClass="check" Checked='<%# Bind("ClosePrev") %>' Text="Zamknij poprzednie przypisanie" />
            </td>
            <td></td>
            <td></td>
            <td class="control"></td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr style="" class="eit">
            <td class="date">
                <uc1:DateEdit ID="deOd" runat="server" Date='<%# Bind("Od") %>' ValidationGroup="vge"/>
            </td>
            <td class="date">
                <uc1:DateEdit ID="deDo" runat="server" Date='<%# Bind("Do") %>' />
            </td>
            <td id="tdRcpId" runat="server" class="rcpid">
                <asp:TextBox ID="RcpIdTextBox" runat="server" Text='<%# Bind("RcpId") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    CssClass="error"
                    ValidationGroup="vge" 
                    ControlToValidate="RcpIdTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" 
                    TargetControlID="RcpIdTextBox" 
                    FilterType="Custom" 
                    ValidChars="0123456789" />
            </td>
            <td id="tdNrKarty" runat="server" class="nrkarty">
                <asp:TextBox ID="NrKartyTextBox" runat="server" Text='<%# Bind("NrKarty") %>' />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    CssClass="error"
                    ValidationGroup="vge" 
                    ControlToValidate="NrKartyTextBox" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" ValidationGroup="vge"/>
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="tbPracParametry tbKartyRcp">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">
                                Od</th>
                            <th runat="server">
                                Do</th>
                            <th id="thRcpId" runat="server">                                
                                <asp:Literal ID="ltRcpId" runat="server" Text="Rcp Id / Numer karty"></asp:Literal>
                                <asp:Literal ID="ltRcpId1" runat="server" Text="Rcp Id" Visible="false"></asp:Literal>
                            </th>
                            <th id="thNrKarty" runat="server">                                
                                <asp:Literal ID="ltNrKarty" runat="server" Text="Rcp Id (Numer karty)" ></asp:Literal>
                                <asp:Literal ID="ltNrKarty1" runat="server" Text="Numer karty RCP" Visible="false"></asp:Literal>
                            </th>
                            <th runat="server" class="control">
                                <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj" />
                            </th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <%--
            <tr class="pager">
                <td class="left">
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="5">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                </td>
                <td class="right">
                    <span class="count">Pokaż na stronie:&nbsp;&nbsp;&nbsp;</span>
                    <asp:DropDownList ID="ddlLines" runat="server" ></asp:DropDownList>
                </td>
            </tr>
            <tr class="pager">
                <td class="left">
                </td>
                <td class="right">
                    <span class="count">Ilość rekordów:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                </td>
            </tr>
            --%>
        </table>
    </LayoutTemplate>
</asp:ListView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [PracownicyKarty] WHERE [Id] = @Id" 
    InsertCommand="
if @ClosePrev = 0
    DISABLE TRIGGER PracownicyKarty_Insert ON PracownicyKarty        

INSERT INTO [PracownicyKarty] ([IdPracownika], [Od], [Do], [RcpId], [NrKarty]) VALUES (@IdPracownika, @Od, @Do, @RcpId, @NrKarty)

if @ClosePrev = 0
    ENABLE TRIGGER PracownicyKarty_Insert ON PracownicyKarty
        " 
    SelectCommand="SELECT *, cast(1 as bit) as ClosePrev FROM [PracownicyKarty] WHERE ([IdPracownika] = @IdPracownika) ORDER BY [Od] DESC" 
    UpdateCommand="UPDATE [PracownicyKarty] SET [IdPracownika] = @IdPracownika, [Od] = @Od, [Do] = @Do, [RcpId] = @RcpId, [NrKarty] = @NrKarty WHERE [Id] = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="RcpId" Type="Int32" />
        <asp:Parameter Name="NrKarty" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="RcpId" Type="Int32" />
        <asp:Parameter Name="NrKarty" Type="String" />
        <asp:Parameter Name="ClosePrev" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>

