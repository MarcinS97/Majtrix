<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntTasks.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Spreadsheets.cntTasks" %>

<%@ Register Src="~/Scorecards/Controls/Admin/cntSearch.ascx" TagPrefix="leet" TagName="Search" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<div id="ctTasks" runat="server" class="cntTasks" >
    
    <leet:Search ID="Search" runat="server" TableName="lvTasks" ColumnName="name" />
    
    
    <asp:ListView ID="lvTasks" runat="server" DataSourceID="dsTasks" 
    DataKeyNames="Id" InsertItemPosition="LastItem" 
    OnItemInserting="lvTasks_ItemInserting"
    OnItemUpdating="lvTasks_ItemUpdating"
    OnItemCreated="lvTasks_ItemCreated"
    >
    <ItemTemplate>
        <tr id="Tr1" style="" runat="server" class="it">
            <td class="name"><asp:Label ID="Label1" runat="server" Text='<%# Eval("Nazwa") %>' /></td>
            <td><asp:Label ID="Label2" runat="server" Text='<%# Eval("Czas") %>' /></td>
            <td><asp:Label ID="Label3" runat="server" Text='<%# Eval("cc") %>' /></td>
            <%--<td class="check"><asp:CheckBox ID="cbQC" runat="server" Checked='<%# Eval("QC") %>' Enabled="false" /></td>--%>
            <td class="check"><asp:CheckBox ID="cbAktywny" runat="server" Checked='<%# Eval("Aktywny") %>' Enabled="false" /></td>
            <td id="tdControl" class="control" runat="server">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
                <%--<asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />--%>
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table id="Table1" runat="server" class="table0">
            <tr class="edt">
                <td>
                    <asp:Label ID="lbNoData" runat="server" Text="Brak danych" /><br /><br />
                    <asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord" Text="Dodaj" />
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <InsertItemTemplate>
        <tr class="iit">
            <td>
                <asp:TextBox ID="tbNazwa" runat="server" Text='<%# Bind("Nazwa") %>' MaxLength="500" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbNazwa" ErrorMessage="" ValidationGroup="ivg"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:TextBox ID="tbCzas" runat="server" Text='<%# Bind("Czas") %>' MaxLength="10" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbCzas" ErrorMessage="" ValidationGroup="ivg"></asp:RequiredFieldValidator>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbCzas" FilterType="Custom" ValidChars="0123456789,." />
            </td>
            <td>
                <asp:DropDownList ID="ddlCC" runat="server" DataSourceID="dsCC" DataValueField="Id" DataTextField="Name" />
                <asp:SqlDataSource ID="dsCC" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select null as Id, 'wybierz ...' as Name, 0 as Sort union all select Id, cc as Name, 1 as Sort from CC where (GETDATE() between AktywneOd and ISNULL(AktywneDo, '20990909')) order by Sort, Name" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="ddlCC" ErrorMessage="" ValidationGroup="ivg"></asp:RequiredFieldValidator>
            </td>
<%--            <td class="check">
                <asp:CheckBox ID="cbQC" runat="server" Checked='<%# Bind("QC") %>' />
            </td>--%>
            <td class="check">
                <asp:CheckBox ID="cbActive" runat="server" Checked='<%# Bind("Aktywny") %>' />
            </td>
            <td id="tdControl" class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Zapisz" />
                <%--<asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Anuluj" />--%>
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td>
                <asp:TextBox ID="tbNazwa" runat="server" Text='<%# Bind("Nazwa") %>' MaxLength="500" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbNazwa" ErrorMessage="" ValidationGroup="evg"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:TextBox ID="tbCzas" runat="server" Text='<%# Bind("Czas") %>' MaxLength="10" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbCzas" ErrorMessage="" ValidationGroup="evg"></asp:RequiredFieldValidator>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbCzas" FilterType="Custom" ValidChars="0123456789,." />
            </td>
            <td>
                <asp:DropDownList ID="ddlCC" runat="server" DataSourceID="dsCC" DataValueField="Id" DataTextField="Name" SelectedValue='<%# Eval("IdCC") %>' />
                <asp:SqlDataSource ID="dsCC" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select null as Id, 'wybierz ...' as Name, 0 as Sort union all select Id, cc as Name, 1 as Sort from CC where (GETDATE() between AktywneOd and ISNULL(AktywneDo, '20990909')) order by Sort, Name" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="ddlCC" ErrorMessage="" ValidationGroup="evg"></asp:RequiredFieldValidator>
            </td>
<%--            <td class="check">
                <asp:CheckBox ID="cbQC" runat="server" Checked='<%# Bind("QC") %>' />
            </td>--%>
            <td class="check">
                <asp:CheckBox ID="TextBox3" runat="server" Checked='<%# Bind("Aktywny") %>' />
            </td>
            
            <td id="tdControl" class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table id="Table1" runat="server" class="ListView1 tbZastepstwa hoverline">
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" class="lvTasks">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" Text="Nazwa" CommandName="Sort" CommandArgument="Nazwa" /></th>
                            <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" Text="Czas nominalny" CommandName="Sort" CommandArgument="Czas" /></th>
                            <th id="Th3" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" Text="CC" CommandName="Sort" CommandArgument="IdCC" /></th>
<%--                            <th id="Th4" runat="server"><asp:LinkButton ID="LinkButton4" runat="server" Text="QC" CommandName="Sort" CommandArgument="QC" /></th>--%>
                            <th id="Th4" runat="server"><asp:LinkButton ID="LinkButton4" runat="server" Text="Aktywny" CommandName="Sort" CommandArgument="Aktywny" /></th>
                            <th id="thControl" class="control" runat="server"></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trPager" runat="server" >
            </tr>
           <%-- <tr id="tr3" runat="server" >
                <td class="bottom_buttons">
                    <asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord" Text="Dodaj zastępstwo" />
                </td>
            </tr>--%>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="dsTasks" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="select c.*, cc.cc
from scCzynnosci c
left join CC cc on cc.Id = c.IdCC
order by Nazwa
"
    DeleteCommand="DELETE FROM scCzynnosci WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO scCzynnosci (Nazwa, Czas, IdCC, Aktywny) VALUES (@Nazwa, @Czas, @IdCC, @Aktywny)" 
    UpdateCommand="update scCzynnosci SET Nazwa = @Nazwa, Czas = @Czas, IdCC = @IdCC, Aktywny = @Aktywny WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Czas" Type="String" />
        <asp:Parameter Name="IdCC" Type="Int32" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Nazwa" Type="String" />
        <asp:Parameter Name="Czas" Type="String" />
        <asp:Parameter Name="IdCC" Type="Int32" />
        <asp:Parameter Name="Aktywny" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>

</div>