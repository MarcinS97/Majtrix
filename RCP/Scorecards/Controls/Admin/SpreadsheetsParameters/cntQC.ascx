<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntQC.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Admin.SpreadsheetsParameters.cntQC" %>

<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:HiddenField ID="hidScorecardTypeId" runat="server" Visible="false" />
<asp:HiddenField ID="hidTL" runat="server" Visible="false" />

<div id="ctQC" runat="server" class="cntQC">
  <%--  <h3><asp:Label ID="lblTitle" runat="server" /></h3>--%>
  
    
    <asp:Label ID="lblTitle" runat="server" CssClass="title" />
    <asp:Label ID="lblSubtitle" runat="server" CssClass="subTitle" Text="Współczynnik jakości, na podstawie którego wyliczana jest korekta premii" />
  
    <asp:DropDownList ID="ddlProductivity" runat="server" DataSourceID="dsDic" DataValueField="Id" DataTextField="Name" AutoPostBack="true" OnSelectedIndexChanged="ddlProductivity_SelectedIndexChanged" CssClass="ddl" />
    <asp:SqlDataSource ID="dsDic" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select Id, Nazwa as Name from scSlowniki where Typ = 'QC'" />

     <asp:ListView ID="lvQC" runat="server" DataSourceID="dsQC" DataKeyNames="Id" InsertItemPosition="LastItem" OnItemUpdating="lvQC_ItemUpdating" OnItemInserting="lvQC_ItemInserting">
    <ItemTemplate>
        <tr id="Tr1" style="" runat="server" class="it">
            <td><asp:Label ID="Label1" runat="server" Text='<%# Eval("_DlaIlu") %>' /></td>
            <td><asp:Label ID="Label2" runat="server" Text='<%# Eval("_Ile") %>' /></td>
            <td><asp:Label ID="Label5" runat="server" Text='<%# Eval("DateFrom") %>' /></td>
            <td><asp:Label ID="Label6" runat="server" Text='<%# Eval("DateTo") %>' /></td>
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
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbDlaIlu" ErrorMessage="" ValidationGroup="ivgQC"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:TextBox ID="tbIle" runat="server" Text='<%# Bind("Ile") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbIle" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbIle" ErrorMessage="" ValidationGroup="ivgQC"></asp:RequiredFieldValidator>
            </td>
            <td class="data">
                <uc1:DateEdit ID="deFrom" runat="server" Date='<%# Bind("Od") %>' ValidationGroup="ivgQC" />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("Do") %>' />
            </td>
            <td id="tdControl" class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Zapisz" ValidationGroup="ivgQC" />
                <%--<asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Anuluj" />--%>
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td>
                <asp:TextBox ID="tbDlaIlu" runat="server" Text='<%# Bind("DlaIluEdit") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="tbDlaIlu" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbDlaIlu" ErrorMessage="" ValidationGroup="evgQC"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:TextBox ID="tbIle" runat="server" Text='<%# Bind("IleEdit") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="tbIle" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbIle" ErrorMessage="" ValidationGroup="evgQC"></asp:RequiredFieldValidator>
            </td>
            <td class="data">
                <uc1:DateEdit ID="deFrom" runat="server" Date='<%# Bind("Od") %>' ValidationGroup="evgQC" />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("Do") %>' />
            </td>
            <td id="tdControl" class="control">
                <asp:Button ID="SaveButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="evgQC" />
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
                            <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" Text="Dla ilu (%)" CommandName="Sort" CommandArgument="DlaIlu" /></th>
                            <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" Text="Ile (%)" CommandName="Sort" CommandArgument="Ile" /></th>
                            <th id="Th3" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" Text="Od" CommandName="Sort" CommandArgument="Od" /></th>
                            <th id="Th4" runat="server"><asp:LinkButton ID="LinkButton4" runat="server" Text="Do" CommandName="Sort" CommandArgument="Do" /></th>
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

<asp:SqlDataSource ID="dsQC" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select qc.*
, LEFT(CONVERT(varchar, qc.Od, 20), 10) as DateFrom
, LEFT(CONVERT(varchar, qc.Do, 20), 10) as DateTo
, convert(varchar, qc.DlaIlu * 100) + '%' as _DlaIlu
, convert(varchar, qc.Ile * 100) + '%' as _Ile 
, qc.DlaIlu * 100 as DlaIluEdit
, qc.Ile * 100 as IleEdit 
from scQC qc
where qc.IdTypuArkuszy = @typark and qc.Rodzaj = @genre and qc.TL = @tl
"
    DeleteCommand="delete from scQC where Id = @Id"
    InsertCommand="INSERT INTO scQC (DlaIlu, Ile, Rodzaj, Od, Do, IdTypuArkuszy, TL) VALUES (@DlaIlu, @Ile, @genre, @Od, @Do, @typark, @tl)" 
    UpdateCommand="update scQC SET DlaIlu = @DlaIlu, Ile = @Ile, Od = @Od, Do = @Do WHERE [Id] = @Id">
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
