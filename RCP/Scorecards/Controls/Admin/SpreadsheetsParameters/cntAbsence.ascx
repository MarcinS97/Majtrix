<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntAbsence.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Admin.SpreadsheetsParameters.cntAbsence" %>

<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:HiddenField ID="hidScorecardTypeId" runat="server" Visible="false" />
<asp:HiddenField ID="hidTL" runat="server" Visible="false" />

<div id="ctAbsence" runat="server" class="cntAbsence">

    
    <asp:Label ID="lblTitle" runat="server" Text="Dyspozycyjność" CssClass="title" />
    <asp:Label ID="lblSubtitle" runat="server" CssClass="subTitle" Text="Ilość dni absencji, na podstawie których wyliczana jest korekta premii"/>
    
    <%--<h3><asp:Label ID="lblTitle" runat="server" Text="Premia za dyspozycyjność" /></h3>--%>

     <asp:ListView ID="lvAbsence" runat="server" DataSourceID="dsAbsence" DataKeyNames="Id" InsertItemPosition="LastItem" OnItemUpdating="lvAbsence_ItemUpdating" OnItemInserting="lvAbsence_ItemInserting">
        <ItemTemplate>
        <tr id="Tr1" style="" runat="server" class="it">
            <td>
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("DlaIlu") %>' />
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" Text='<%# Eval("HowMuch") %>' />
            </td>
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
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbDlaIlu" ErrorMessage="" ValidationGroup="ivgAbs"></asp:RequiredFieldValidator>
            </td>
            <td class="curr">
                <asp:TextBox ID="tbIle" runat="server" Text='<%# Bind("Ile") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbIle" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbIle" ErrorMessage="" ValidationGroup="ivgAbs"></asp:RequiredFieldValidator>
                
                <asp:RadioButtonList ID="rblCurrency" runat="server" CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Value="0" Selected="True" Text="%"></asp:ListItem>
                    <asp:ListItem Value="1" Text="zł"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td class="data">
                <uc1:DateEdit ID="deFrom" runat="server" Date='<%# Bind("Od") %>' ValidationGroup="ivgAbs" />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("Do") %>' />
            </td>
            <td id="tdControl" class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Zapisz" ValidationGroup="ivgAbs" />
                <%--<asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Anuluj" />--%>
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td>
                <asp:TextBox ID="tbDlaIlu" runat="server" Text='<%# Bind("DlaIlu") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="tbDlaIlu" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbDlaIlu" ErrorMessage="" ValidationGroup="evgAbs"></asp:RequiredFieldValidator>
            </td>
            <td class="curr">
                <asp:TextBox ID="tbIle" runat="server" Text='<%# Bind("IleActual") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="tbIle" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbIle" ErrorMessage="" ValidationGroup="evgAbs"></asp:RequiredFieldValidator>
                
                <asp:RadioButtonList ID="rblCurrency" runat="server" CellPadding="0" CellSpacing="0" RepeatDirection="Horizontal" RepeatLayout="Flow" SelectedValue='<%# Eval("Curr") %>' >
                    <asp:ListItem Value="0" Selected="True" Text="%"></asp:ListItem>
                    <asp:ListItem Value="1" Text="zł"></asp:ListItem>
                </asp:RadioButtonList>
                
            </td>
            <td class="data">
                <uc1:DateEdit ID="deFrom" runat="server" Date='<%# Bind("Od") %>' ValidationGroup="evgAbs" />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("Do") %>' />
            </td>
            <td id="tdControl" class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="evgAbs" />
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
                            <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" Text="Dla ilu" CommandName="Sort" CommandArgument="DlaIlu" /></th>
                            <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" Text="Ile" CommandName="Sort" CommandArgument="Ile" /></th>
                            <th id="Th3" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" Text="Od" CommandName="Sort" CommandArgument="Od" /></th>
                            <th id="Th4" runat="server"><asp:LinkButton ID="LinkButton4" runat="server" Text="Do" CommandName="Sort" CommandArgument="Do" /></th>
                            <th id="thControl" class="control" runat="server"></th>
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

<asp:SqlDataSource ID="dsAbsence" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select a.*
, LEFT(CONVERT(varchar, a.Od, 20), 10) as DateFrom
, LEFT(CONVERT(varchar, a.Do, 20), 10) as DateTo
, case when a.Ile &gt;= 1337 then CONVERT(varchar, a.Ile - 1337) + ' zł' else CONVERT(varchar, a.Ile * 100) + '%' end as HowMuch
, case when a.Ile &gt;= 1337 then (a.Ile - 1337) else a.Ile * 100 end as IleActual
, case when a.Ile &gt;= 1337 then 1 else 0 end as Curr
from scAbsencje a
where a.IdTypuArkuszy = @typark and a.TL = @tl
order by Od desc
"
    DeleteCommand="delete from scAbsencje where Id = @Id"
    InsertCommand="INSERT INTO scAbsencje (DlaIlu, Ile, Od, Do, IdTypuArkuszy, TL) VALUES (@DlaIlu, @Ile + @curr, @Od, @Do, @typark, @tl)" 
    UpdateCommand="update scAbsencje SET DlaIlu = @DlaIlu, Ile = @Ile + @curr, Od = @Od, Do = @Do WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <SelectParameters>
        <asp:ControlParameter Name="typark" Type="Int32" ControlID="hidScorecardTypeId" PropertyName="Value" />
        <asp:ControlParameter Name="tl" Type="String" ControlID="hidTL" PropertyName="Value" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="DlaIlu" Type="Double" />
        <asp:Parameter Name="Ile" Type="Double" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="curr" Type="Double" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="DlaIlu" Type="Double" />
        <asp:Parameter Name="Ile" Type="Double" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="curr" Type="Double" />
        <asp:ControlParameter Name="typark" Type="Int32" ControlID="hidScorecardTypeId" PropertyName="Value" />
        <asp:ControlParameter Name="tl" Type="String" ControlID="hidTL" PropertyName="Value" />
    </InsertParameters>
</asp:SqlDataSource>
</div>
