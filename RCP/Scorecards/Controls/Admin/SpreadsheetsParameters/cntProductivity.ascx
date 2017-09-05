<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntProductivity.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Admin.SpreadsheetsParameters.cntProductivity" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>

<asp:HiddenField ID="hidScorecardTypeId" runat="server" Visible="false" />
<asp:HiddenField ID="hidTL" runat="server" Visible="false" />

<div id="ctProductivity" runat="server" class="cntProductivity">

    
    <%--<h3 class="hProd"><asp:Label ID="lblTitle" runat="server"  /></h3>--%>
    <asp:Label ID="lblTitle" runat="server" CssClass="title" />
    <asp:Label ID="lblSubtitle" runat="server" CssClass="subTitle" Text="Współczynnik produktywności, na podstawie którego wyliczana jest premia" />
    
    <asp:DropDownList ID="ddlProductivity" runat="server" DataSourceID="dsDic" DataValueField="Id" DataTextField="Name" AutoPostBack="true" OnSelectedIndexChanged="ddlProductivity_SelectedIndexChanged" CssClass="ddl" />
    <asp:SqlDataSource ID="dsDic" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select Id, Nazwa as Name from scSlowniki where Typ = 'PROD'" />

     <asp:ListView ID="lvProductivity" runat="server" DataSourceID="dsProductivity" DataKeyNames="Id" InsertItemPosition="LastItem" OnItemUpdating="lvProductivity_ItemUpdating" OnItemInserting="lvProductivity_ItemInserting">
        <ItemTemplate>
        <tr id="Tr1" style="" runat="server" class="it">
            <td>
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("_DlaIlu") %>' />
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" Text='<%# Eval("_Ile") %>' />
            </td>
<%--            <td class="check">
                <asp:CheckBox ID="Label3" runat="server" Checked='<%# Eval("OkresProbny") %>' Enabled="false" />
            </td>--%>
            <td>
                <asp:Label ID="Label5" runat="server" Text='<%# Eval("DateFrom") %>' />
            </td>
            <td>
                <asp:Label ID="Label6" runat="server" Text='<%# Eval("DateTo") %>' />
            </td>
            <td id="tdControl" class="control" runat="server">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
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
                <asp:TextBox ID="tbDlaIlu" runat="server" Text='<%# Bind("DlaIlu") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbDlaIlu" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbDlaIlu" ErrorMessage="" ValidationGroup="ivgProd"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:TextBox ID="tbIle" runat="server" Text='<%# Bind("Ile") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbIle" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbIle" ErrorMessage="" ValidationGroup="ivgProd"></asp:RequiredFieldValidator>
            </td>
<%--            <td class="check">
                <asp:CheckBox ID="cbChecked" runat="server" Checked='<%# Bind("OkresProbny") %>' />
            </td>--%>
            <td class="data">
                <uc1:DateEdit ID="deFrom" runat="server" Date='<%# Bind("Od") %>' ValidationGroup="ivgProd" />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("Do") %>' />
            </td>
            <td id="tdControl" class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Zapisz" ValidationGroup="ivgProd" />
                <%--<asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Anuluj" />--%>
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td>
                <asp:TextBox ID="tbDlaIlu" runat="server" Text='<%# Bind("DlaIluEdit") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="tbDlaIlu" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbDlaIlu" ErrorMessage="" ValidationGroup="evgProd"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:TextBox ID="tbIle" runat="server" Text='<%# Bind("Ile") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="tbIle" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbIle" ErrorMessage="" ValidationGroup="evgProd"></asp:RequiredFieldValidator>
            </td>
<%--            <td class="check">
                <asp:CheckBox ID="cbChecked" runat="server" Checked='<%# Bind("OkresProbny") %>' />
            </td>--%>
            <td class="data">
                <uc1:DateEdit ID="deFrom" runat="server" Date='<%# Bind("Od") %>' ValidationGroup="evgProd" />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("Do") %>' />
            </td>
            <td id="tdControl" class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="evgProd" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table id="Table1" runat="server" class="ListView1 tbZastepstwa hoverline">
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table id="itemPlaceholderContainer" runat="server">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" Text="Dla Ilu (%)" CommandName="Sort" CommandArgument="DlaIlu" /></th>
                            <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" Text="Ile (zł)" CommandName="Sort" CommandArgument="Ile" /></th>
<%--                            <th id="Th3" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" Text="Okres próbny" CommandName="Sort" CommandArgument="OkresProbny" /></th>--%>
                            <th id="Th4" runat="server"><asp:LinkButton ID="LinkButton4" runat="server" Text="Od" CommandName="Sort" CommandArgument="Od" /></th>
                            <th id="Th5" runat="server"><asp:LinkButton ID="LinkButton5" runat="server" Text="Do" CommandName="Sort" CommandArgument="Do" /></th>
                            <th id="thControl" class="control" runat="server">
                            </th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trPager" runat="server">
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="dsProductivity" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select p.*
, s.Nazwa as TypeName
, LEFT(CONVERT(varchar, p.Od, 20), 10) as DateFrom
, LEFT(CONVERT(varchar, p.Do, 20), 10) as DateTo
, convert(varchar, p.DlaIlu * 100) + '%' as _DlaIlu
, convert(Varchar, p.Ile) + ' zł' as _Ile
, p.DlaIlu * 100 as DlaIluEdit
from scProduktywnosc p
left join scSlowniki s on s.Id = p.Rodzaj
where p.IdTypuArkuszy = @typark and p.Rodzaj = @genre and p.TL = @tl
"
    DeleteCommand="delete from scProduktywnosc where Id = @Id"
    InsertCommand="INSERT INTO scProduktywnosc (DlaIlu, Ile, OkresProbny, Rodzaj, Od, Do, IdTypuArkuszy, TL) VALUES (@DlaIlu, @Ile, 0, @genre, @Od, @Do, @typark, @tl)" 
    UpdateCommand="update scProduktywnosc SET DlaIlu = @DlaIlu, Ile = @Ile, OkresProbny = 0, Od = @Od, Do = @Do WHERE [Id] = @Id">
    <SelectParameters>
        <asp:ControlParameter Name="typark" Type="Int32" ControlID="hidScorecardTypeId" PropertyName="Value" />
        <asp:ControlParameter Name="genre" Type="Int32" ControlID="ddlProductivity" PropertyName="SelectedValue" />
        <asp:ControlParameter Name="tl" Type="String" ControlID="hidTL" PropertyName="Value" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="DlaIlu" Type="Double" />
        <asp:Parameter Name="Ile" Type="Double" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="DlaIlu" Type="Double" />
        <asp:Parameter Name="Ile" Type="Double" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:ControlParameter Name="genre" Type="Int32" ControlID="ddlProductivity" PropertyName="SelectedValue" />
        <asp:ControlParameter Name="typark" Type="Int32" ControlID="hidScorecardTypeId" PropertyName="Value" />
        <asp:ControlParameter Name="tl" Type="String" ControlID="hidTL" PropertyName="Value" />
    </InsertParameters>
</asp:SqlDataSource>
</div>