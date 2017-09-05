<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWidelki.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Adm.cntWidelki" %>

<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:HiddenField ID="hidType" runat="server" Visible="false" />
<asp:HiddenField ID="hidScorecardTypeId" runat="server" Visible="false" />
<asp:HiddenField ID="hidTL" runat="server" Visible="false" />
<asp:HiddenField ID="hidPercent" runat="server" Visible="false" />
<asp:HiddenField ID="hidUnit" runat="server" Visible="false" />
<asp:HiddenField ID="hidParametr2Query" runat="server" Visible="false" Value="p.Parametr2" />

<asp:HiddenField ID="hidDateFilter" runat="server" Visible="false" />

<div id="ctOtherList" class="cntWidelki" style="display:inline-block;">
<%--    <h3><asp:Label ID="lblTitle" runat="server" /></h3>--%>
        
    <asp:Label ID="lblTitle" runat="server" CssClass="title" style="display: inline-block; margin-bottom: 0px !important;" />
    <hr />
    <div class="xfilters xinlineb">
            <div class="form-group" style="display: inline-block;" >
                <label>Data:</label>
                <uc1:DateEdit ID="deFilter" runat="server" style="display: block;" OnDateChanged="deFilter_DateChanged" AutoPostBack="true" />
            </div>
        <div class="xfilter form-group pull-right">
            <label>Linia:</label>
            <asp:DropDownList ID="ddlLinie" runat="server" DataSourceID="dsLinie" DataValueField="Id" DataTextField="Name" AutoPostBack="true" CssClass="form-control" />
            <asp:SqlDataSource ID="dsLinie" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select null as Id, 'wybierz ...' as Name, 0 as Sort union all select Id, Nazwa as Name, 1 as Sort from msLinie order by Sort, Name" />
        </div>
    </div>
    <asp:Label ID="lblSubtitle" runat="server" CssClass="subTitle" />


    <asp:ListView ID="lvOther" runat="server" DataSourceID="dsOther" DataKeyNames="Id" InsertItemPosition="LastItem" Visible="true" OnItemUpdating="lvOther_ItemUpdating" OnItemInserting="lvOther_ItemInserting" OnItemDataBound="lvOther_ItemDataBound" 
        OnItemCreated="lvOther_ItemCreated" OnLayoutCreated="lvOther_LayoutCreated" >
    <ItemTemplate>
        <tr id="Tr1" style="" runat="server" class="it">
            <td><asp:Label ID="Label4" runat="server" Text='<%# Eval("Linia") %>' /></td>
            <td><asp:Label ID="Label2" runat="server" Text='<%# Eval("Od") %>' /></td>
            <td><asp:Label ID="Label1" runat="server" Text='<%# Eval("Do") %>' /></td>
            <td><asp:Label ID="Label3" runat="server" Text='<%# Eval("Wynik") %>' /></td>
            <%--<td id="Td1" runat="server" visible='<%# IsEmployee() %>'><asp:Label ID="Label4" runat="server" Text='<%# Eval("Employee") %>' /></td>--%>
            <%--<td id="Td2" runat="server" visible='<%# IsCustomDDL() %>'><asp:Label ID="lblCustom" runat="server" Text='<%# Eval("Parametr2") %>' /></td>--%>
            <td><asp:Label ID="Label5" runat="server" Text='<%# Eval("DateFrom") %>' /></td>
            <td><asp:Label ID="Label6" runat="server" Text='<%# Eval("DateTo") %>' /></td>
            <td id="tdControl" class="control" runat="server">
                <%--<asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" CssClass="btn btn-sm btn-default" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" CssClass="btn btn-sm btn-default" />--%>
                
                <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" CssClass="btn xbtn-sm xbtn-default"><i class="glyphicon glyphicon-edit"></i></asp:LinkButton>
                <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" CssClass="btn xbtn-sm xbtn-default text-danger"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>
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
            <td id="Td2" runat="server">      
                <asp:HiddenField ID="hidIdLinii" runat="server" Visible="false" Value='<%# Eval("IdLinii") %>' />
                      
                <asp:DropDownList ID="ddlLinie" runat="server" DataSourceID="dsLinie" DataValueField="Id" DataTextField="Name" />
                <asp:RequiredFieldValidator ID="ddlLinieValidator" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="ddlLinie" ErrorMessage="Błąd" ></asp:RequiredFieldValidator>    
                <asp:SqlDataSource ID="dsLinie" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select Id, Nazwa as Name from msLinie order by Name" />

              <%-- <asp:DropDownList ID="ddlCustom" runat="server" Visible="false" />
                  <asp:RequiredFieldValidator ID="ddlCustomValidator" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="ddlCustom" ErrorMessage="Błąd" ></asp:RequiredFieldValidator>    --%>

            </td>
            <td id="Td3" runat="server">
                <asp:TextBox ID="tbOd" runat="server" Text='<%# Bind("Od") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbOd" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="tbOdValidator" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbOd" ErrorMessage="" ></asp:RequiredFieldValidator>
            </td>
            <td id="Td5" runat="server">
                <asp:TextBox ID="tbDo" runat="server" Text='<%# Bind("Do") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbDo" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="tbDoValidator" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbDo" ErrorMessage="" ></asp:RequiredFieldValidator>
            </td>
            <td id="Td6" runat="server">
                <asp:TextBox ID="tbWynik" runat="server" Text='<%# Bind("Wynik") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="tbDo" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="tbWynikValidator" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbWynik" ErrorMessage="" ></asp:RequiredFieldValidator>
            </td>
            <td class="data">
                <uc1:DateEdit ID="deFrom" runat="server" Date='<%# Bind("DataOd") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataDo") %>' />
            </td>
            <td id="tdControl" class="control" style="text-align: center;">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Zapisz" CssClass="btn btn-sm btn-success" />
                <%--<asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Anuluj" />--%>
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td id="Td2" runat="server">            
                <asp:HiddenField ID="hidIdLinii" runat="server" Visible="false" Value='<%# Eval("IdLinii") %>' />
            
                <asp:DropDownList ID="ddlLinie" runat="server" DataSourceID="dsLinie" DataValueField="Id" DataTextField="Name" />
                <asp:RequiredFieldValidator ID="ddlLinieValidator" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="ddlLinie" ErrorMessage="Błąd" ></asp:RequiredFieldValidator>    
                <asp:SqlDataSource ID="dsLinie" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select Id, Nazwa as Name from msLinie order by Name" />

              <%-- <asp:DropDownList ID="ddlCustom" runat="server" Visible="false" />
                  <asp:RequiredFieldValidator ID="ddlCustomValidator" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="ddlCustom" ErrorMessage="Błąd" ></asp:RequiredFieldValidator>    --%>

            </td>
            <td>
                <asp:TextBox ID="tbOd" runat="server" Text='<%# Bind("Od") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbOd" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="tbOdValidator" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbOd" ErrorMessage="" ></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:TextBox ID="tbDo" runat="server" Text='<%# Bind("Do") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="tbDo" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="tbDoValidator" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbDo" ErrorMessage="" ></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:TextBox ID="tbWynik" runat="server" Text='<%# Bind("Wynik") %>' MaxLength="10" />
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="tbWynik" FilterType="Custom" ValidChars="0123456789,." />
                <asp:RequiredFieldValidator ID="tbWynikValidator" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbWynik" ErrorMessage="" ></asp:RequiredFieldValidator>
            </td>
            
<%--            <td id="Td4" runat="server" >--%>
<%--                <asp:DropDownList ID="ddlEmployees" runat="server" DataSourceID="dsEmployees" DataValueField="Id" DataTextField="Name" Visible='<%# IsEmployee() %>' />
                <asp:SqlDataSource ID="dsEmployees" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="
select distinct p.Id, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') as Name from Przypisania prz
left join Pracownicy p on prz.IdKierownika = p.Id
where prz.IdCommodity = @typark
" >
    <SelectParameters>
        <asp:ControlParameter Name="typark" Type="Int32" ControlID="hidScorecardTypeId" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>   --%>
<%--                <asp:DropDownList ID="ddlCustom" runat="server" Visible='<%# IsCustomDDL() %>' />
 <asp:RequiredFieldValidator ID="ddlCustomValidator" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="ddlCustom" ErrorMessage="Błąd" ></asp:RequiredFieldValidator> 


            </td>--%>
<%--            <td>
                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Parametr2") %>' MaxLength="200" />
            </td>--%>
            <td class="data">
                <uc1:DateEdit ID="deFrom" runat="server" Date='<%# Bind("DataOd") %>' />
            </td>
            <td class="data">
                <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataDo") %>'  />
            </td>
            <td id="tdControl" class="control">
<%--            <asp:Button ID="SaveButton" runat="server" CommandName="Update" Text="Zapisz" CssClass="btn btn-sm btn-success" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" CssClass="btn btn-sm btn-default" />--%>

                <asp:LinkButton ID="SaveButton" runat="server" CommandName="Update" CssClass="btn xbtn-sm xbtn-default text-success"><i class="glyphicon glyphicon-floppy-disk"></i></asp:LinkButton>
                <asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel" CssClass="btn xbtn-sm xbtn-default"><i class="glyphicon glyphicon-ban-circle"></i></asp:LinkButton>
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="xListView1 xtbZastepstwa xhoverline">
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table id="itemPlaceholderContainer" runat="server">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th0" runat="server"><asp:LinkButton ID="LinkButton0" runat="server" Text="Linia" CommandName="Sort" CommandArgument="Linia" /></th>
                            <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" Text="Od" CommandName="Sort" CommandArgument="Od" /></th>
                            <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" Text="Do" CommandName="Sort" CommandArgument="Do" /></th>
                            <th id="Th3" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" Text="Wynik" CommandName="Sort" CommandArgument="Wynik" /></th>
<%--                            <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" Text="Pracownik" CommandName="Sort" CommandArgument="Parametr2" /></th>--%>
                            <th id="th4" runat="server"><asp:LinkButton ID="LinkButton4" runat="server" Text="Data od" CommandName="Sort" CommandArgument="DataOd" /></th>
                            <th id="Th5" runat="server"><asp:LinkButton ID="LinkButton5" runat="server" Text="Data do" CommandName="Sort" CommandArgument="DataDo" /></th>
                            <th id="thControl" class="control" runat="server">
                            </th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trPager" runat="server">
                <td id="Td2" runat="server" class="pager">
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="«" PreviousPageText="‹" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="›" LastPageText="»" />
                        </Fields>
                    </asp:DataPager>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="dsOther" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    CancelSelectOnNullParameter="false"
    SelectCommand="
select w.Id, w.Od, w.Do, w.Wynik, w.DataOd, w.DataDo, l.Nazwa as Linia, w.IdLinii
, LEFT(CONVERT(varchar, w.DataOd, 20), 10) as DateFrom
, LEFT(CONVERT(varchar, w.DataDo, 20), 10) as DateTo 
, case when @percent = 1 then convert(varchar, w.Od * 100) + '%' else convert(varchar, w.Od) end as _Od
, case when @percent = 1 then convert(varchar, w.Do * 100) + '%' else convert(varchar, w.Do) end as _Do
, case when @percent = 1 then w.Od * 100 else w.Od end as OdEdit
, case when @percent = 1 then w.Do * 100 else w.Do end as DoEdit
, w.Wynik as WynikEdit
from msWidelki w
left join msLinie l on l.Id = w.IdLinii
where 
w.Typ = @typ and (w.IdLinii = @linia or @linia is null)
and (@dateFilter between w.DataOd and isnull(w.DataDo, '20990909') or @dateFilter is null)
order by Linia, w.Od
"
    InsertCommand="INSERT INTO msWidelki (Typ, Od, Do, Wynik, DataOd, DataDo, IdLinii) VALUES (@typ, @Od, @Do, @Wynik, @DataOd, @DataDo, @IdLinii)" 
    UpdateCommand="update msWidelki SET Od = @Od, Do = @Do, Wynik = @Wynik, DataOd = @DataOd, DataDo = @DataDo, IdLinii = @IdLinii WHERE [Id] = @Id"
    DeleteCommand="delete from msWidelki where Id = @Id"
    >
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <SelectParameters>
        <asp:ControlParameter Name="typ" Type="String" ControlID="hidType" PropertyName="Value" />
        <asp:ControlParameter Name="percent" Type="Int32" ControlID="hidPercent" PropertyName="Value" />
        <asp:ControlParameter Name="linia" Type="Int32" ControlID="ddlLinie" PropertyName="SelectedValue" />
        <asp:ControlParameter Name="dateFilter" Type="DateTime" ControlID="hidDateFilter" PropertyName="Value" />
        <%--<asp:ControlParameter Name="par2" Type="SByte" ControlID="hidParametr2Query" PropertyName="Value" />--%>
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="Od" Type="Double" />
        <asp:Parameter Name="Do" Type="Double" />
        <asp:Parameter Name="Wynik" Type="Double" />
        <asp:Parameter Name="DataOd" Type="DateTime" />
        <asp:Parameter Name="DataDo" Type="DateTime" />
        <asp:Parameter Name="IdLinii" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Od" Type="Double" />
        <asp:Parameter Name="Do" Type="Double" />
        <asp:Parameter Name="Wynik" Type="Double" />
        <asp:Parameter Name="DataOd" Type="DateTime" />
        <asp:Parameter Name="DataDo" Type="DateTime" />
        <asp:Parameter Name="IdLinii" Type="Int32" />
        <asp:ControlParameter Name="typ" Type="String" ControlID="hidType" PropertyName="Value" />
    </InsertParameters>
</asp:SqlDataSource>

</div>
<hr />