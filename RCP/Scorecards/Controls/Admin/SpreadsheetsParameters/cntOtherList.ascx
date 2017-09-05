<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntOtherList.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Admin.SpreadsheetsParameters.cntOtherList" %>

<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:HiddenField ID="hidType" runat="server" Visible="false" />
<asp:HiddenField ID="hidScorecardTypeId" runat="server" Visible="false" />
<asp:HiddenField ID="hidTL" runat="server" Visible="false" />
<asp:HiddenField ID="hidPercent" runat="server" Visible="false" />
<asp:HiddenField ID="hidUnit" runat="server" Visible="false" />
<asp:HiddenField ID="hidParametr2Query" runat="server" Visible="false" Value="p.Parametr2" />

<div id="ctOtherList class="cntOtherList">
<%--    <h3><asp:Label ID="lblTitle" runat="server" /></h3>--%>
        
    <asp:Label ID="lblTitle" runat="server" CssClass="title" />
    <asp:Label ID="lblSubtitle" runat="server" CssClass="subTitle" />


    <asp:ListView ID="lvOther" runat="server" DataSourceID="dsOther" DataKeyNames="Id" InsertItemPosition="LastItem" Visible="true" OnItemUpdating="lvOther_ItemUpdating" OnItemInserting="lvOther_ItemInserting" OnItemDataBound="lvOther_ItemDataBound" 
        OnItemCreated="lvOther_ItemCreated" OnLayoutCreated="lvOther_LayoutCreated" >
    <ItemTemplate>
        <tr id="Tr1" style="" runat="server" class="it">
            <td><asp:Label ID="Label2" runat="server" Text='<%# Eval("_Parametr") %>' /></td>
            <td runat="server" visible='<%# IsEmployee() %>'><asp:Label ID="Label4" runat="server" Text='<%# Eval("Employee") %>' /></td>
            <td runat="server" visible='<%# IsCustomDDL() %>'><asp:Label ID="lblCustom" runat="server" Text='<%# Eval("Parametr2") %>' /></td>
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
            <td runat="server">
                <asp:TextBox ID="tbParametr" runat="server" Text='<%# Bind("Parametr") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbParametr" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="tbParameterValidator" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbParametr" ErrorMessage="" ></asp:RequiredFieldValidator>
            </td>
            <td id="Td2" runat="server" visible='<%# IsParametr2() %>'>            
                <asp:DropDownList ID="ddlEmployees" runat="server" DataSourceID="dsEmployees" DataValueField="Id" DataTextField="Name" />
                <asp:RequiredFieldValidator ID="ddlEmployeesValidator" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="ddlEmployees" ErrorMessage="Błąd" ></asp:RequiredFieldValidator>    
                <asp:SqlDataSource ID="dsEmployees" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="
select null as Id, 'wybierz ...' as Name, 0 as Sort 
union all 
select distinct p.Id, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') as Name, 1 as Sort 
from Przypisania prz
left join Pracownicy p on prz.IdKierownika = p.Id
where prz.IdCommodity = @typark
order by Sort, Name
" >
    <SelectParameters>
        <asp:ControlParameter Name="typark" Type="Int32" ControlID="hidScorecardTypeId" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>

                <asp:DropDownList ID="ddlCustom" runat="server" Visible="false" />
                  <asp:RequiredFieldValidator ID="ddlCustomValidator" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="ddlCustom" ErrorMessage="Błąd" ></asp:RequiredFieldValidator>    

            </td>
            <td class="data">
                <uc1:DateEdit ID="deFrom" runat="server" Date='<%# Bind("Od") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("Do") %>' />
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
                <asp:TextBox ID="tbParametr" runat="server" Text='<%# Bind("ParametrEdit") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbParametr" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="tbParameterValidator" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbParametr" ErrorMessage="" ></asp:RequiredFieldValidator>
            </td>
            
            <td runat="server" visible='<%# IsParametr2() %>'>
                <asp:DropDownList ID="ddlEmployees" runat="server" DataSourceID="dsEmployees" DataValueField="Id" DataTextField="Name" Visible='<%# IsEmployee() %>' />
                <asp:SqlDataSource ID="dsEmployees" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="
select distinct p.Id, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') as Name from Przypisania prz
left join Pracownicy p on prz.IdKierownika = p.Id
where prz.IdCommodity = @typark
" >
    <SelectParameters>
        <asp:ControlParameter Name="typark" Type="Int32" ControlID="hidScorecardTypeId" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>   
                <asp:DropDownList ID="ddlCustom" runat="server" Visible='<%# IsCustomDDL() %>' />
 <asp:RequiredFieldValidator ID="ddlCustomValidator" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="ddlCustom" ErrorMessage="Błąd" ></asp:RequiredFieldValidator> 


            </td>
<%--            <td>
                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Parametr2") %>' MaxLength="200" />
            </td>--%>
            <td class="data">
                <uc1:DateEdit ID="deFrom" runat="server" Date='<%# Bind("Od") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("Do") %>'  />
            </td>
            <td id="tdControl" class="control">
                <asp:Button ID="SaveButton" runat="server" CommandName="Update" Text="Zapisz" />
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
                            <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" Text="Parametr" CommandName="Sort" CommandArgument="Parametr" /></th>
                            <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" Text="Pracownik" CommandName="Sort" CommandArgument="Parametr2" /></th>
                            <th id="th3" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" Text="Od" CommandName="Sort" CommandArgument="Od" /></th>
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

<asp:SqlDataSource ID="dsOther" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select p.Id, p.Od, p.Do, p.Parametr
, LEFT(CONVERT(varchar, p.Od, 20), 10) as DateFrom
, LEFT(CONVERT(varchar, p.Do, 20), 10) as DateTo 
, case when @percent = 1 then convert(varchar, p.Parametr * 100) + '%' else convert(varchar, p.Parametr) end as _Parametr
, case when @Percent = 1 then p.Parametr * 100 else p.Parametr end as ParametrEdit
, pr.Nazwisko +  ' ' + pr.Imie + isnull(' (' + pr.KadryId + ')', '') as Employee
, p.Parametr2 as Parametr2Edit
, s.Nazwa as Parametr2
from scParametry p
left join Pracownicy pr on pr.Id = p.Parametr2
left join scSlowniki s on s.Id = p.Parametr2 and s.Typ = p.Typ
where p.IdTypuArkuszy = @typark and p.Typ = @typ and TL = @tl
order by s.Nazwa, p.Od
"
    InsertCommand="INSERT INTO scParametry (Typ, Parametr, Parametr2, Od, Do, IdTypuArkuszy, TL) VALUES (@Typ, @Parametr, @Parametr2, @Od, @Do, @typark, @tl)" 
    UpdateCommand="update scParametry SET Parametr = @Parametr, Parametr2 = @Parametr2, Od = @Od, Do = @Do WHERE [Id] = @Id"
    DeleteCommand="delete from scParametry where Id = @Id"
    >
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <SelectParameters>
        <asp:ControlParameter Name="typark" Type="Int32" ControlID="hidScorecardTypeId" PropertyName="Value" />
        <asp:ControlParameter Name="typ" Type="String" ControlID="hidType" PropertyName="Value" />
        <asp:ControlParameter Name="tl" Type="String" ControlID="hidTL" PropertyName="Value" />
        <asp:ControlParameter Name="percent" Type="Int32" ControlID="hidPercent" PropertyName="Value" />
        <%--<asp:ControlParameter Name="par2" Type="SByte" ControlID="hidParametr2Query" PropertyName="Value" />--%>
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="Parametr" Type="Double" />
        <asp:Parameter Name="Parametr2" Type="Double" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Parametr" Type="Double" />
        <asp:Parameter Name="Parametr2" Type="Double" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:ControlParameter Name="typ" Type="String" ControlID="hidType" PropertyName="Value" />
        <asp:ControlParameter Name="typark" Type="Int32" ControlID="hidScorecardTypeId" PropertyName="Value" />
        <asp:ControlParameter Name="tl" Type="String" ControlID="hidTL" PropertyName="Value" />
    </InsertParameters>
</asp:SqlDataSource>

</div>