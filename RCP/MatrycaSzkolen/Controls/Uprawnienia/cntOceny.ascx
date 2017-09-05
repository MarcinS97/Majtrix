<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntOceny.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Uprawnienia.cntOceny" %>

<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:HiddenField ID="hidCertyfikatId" runat="server" Visible="false" />
<asp:HiddenField ID="hidPracId" runat="server" Visible="false" />
<asp:HiddenField ID="hidUprId" runat="server" Visible="false" />
<asp:HiddenField ID="hidUserId" runat="server" Visible="false" />


<div class="cntOceny">
      <asp:ListView ID="lvOceny" runat="server" DataSourceID="dsOceny" DataKeyNames="Id" InsertItemPosition="LastItem" Visible="true" OnItemInserting="lvOceny_ItemInserting" OnItemUpdating="lvOceny_ItemUpdating" >
    <ItemTemplate>
        <tr id="Tr1" style="" runat="server" class="it">
            <td><asp:Label ID="Label5" runat="server" Text='<%# Eval("DateFrom") %>' /></td>
            <td><asp:Label ID="Label6" runat="server" Text='<%# Eval("DateTo") %>' /></td>
            <td><asp:Label ID="Label7" runat="server" Text='<%# Eval("Ocena") %>' /></td>
            <td><asp:Label ID="Label3" runat="server" Text='<%# Eval("Korektor") %>' /></td>
            <td><asp:Label ID="Label4" runat="server" Text='<%# Eval("Akceptujacy") %>' /></td>
            <td><asp:Label ID="Label8" runat="server" Text='<%# Eval("AcceptDate") %>' /></td>
            <td><asp:Label ID="Label2" runat="server" Text='<%# Eval("Uwagi") %>' /></td>
            <td><asp:Label ID="Label1" runat="server" Text='<%# Eval("StatusName") %>' /></td>
            <td id="tdControl" class="control" runat="server" style="width: 120px;">
                <%--<asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" CssClass="btn btn-sm btn-default" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" CssClass="btn btn-sm btn-default" />--%>
                
                <asp:LinkButton ID="EditButton" runat="server" CommandName="Edit" Visible='<%# CanEdit() && Convert.ToBoolean(Eval("EditVisible")) %>' 
                    CssClass="btn xbtn-sm xbtn-default"><i class="glyphicon glyphicon-edit"></i></asp:LinkButton>
                <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" Visible='<%# CanEdit() && Convert.ToBoolean(Eval("EditVisible")) %>' 
                    CssClass="btn xbtn-sm xbtn-default text-danger"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>

                <asp:LinkButton ID="btnZoom" runat="server" OnClick="btnZoom_Click" Visible='<%# Convert.ToBoolean(Eval("Lupka")) %>' CommandArgument='<%# Eval("IdPracownika") + ";" + Eval("OkresOd") + ";" + Eval("OkresDo") %>' CssClass="btn xbtn-sm xbtn-default text-primary"><i class="glyphicon glyphicon-search"></i></asp:LinkButton>
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
            <td class="data">
                <uc1:DateEdit ID="deOd" runat="server" Date='<%# Bind("DataOd") %>' ValidationGroup="ivg" />
            </td>
            <td class="data">
                <uc1:DateEdit ID="deDo" runat="server" Date='<%# Bind("DataDo") %>'   />
            </td>
            <td class="ocena">
                <asp:TextBox ID="tbOcena" runat="server" Text='<%# Bind("Ocena") %>' ValidationGroup="ivg" MaxLength="1" />
                <asp:RequiredFieldValidator ID="reqOcena" runat="server" ControlToValidate="tbOcena" ErrorMessage="Pole wymagane" Display="Dynamic" ValidationGroup="ivg" />
                <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" TargetControlID="tbOcena" FilterType="Custom" ValidChars="01234" />
            </td>
            <td><asp:Label ID="Label3" runat="server" Text='<%# Eval("Korektor") %>' /></td>
            <td><asp:Label ID="Label4" runat="server" Text='<%# Eval("Akceptujacy") %>' /></td>
            <td><asp:Label ID="Label8" runat="server" Text='<%# Eval("AcceptDate") %>' /></td>
            <td class="ocena">
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Uwagi") %>'  />
            </td>
            <td>

            </td>
            <td id="tdControl" class="control" style="text-align: center;">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Zapisz" CssClass="btn btn-sm btn-success" />
                <%--<asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Anuluj" />--%>
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="data">
                <uc1:DateEdit ID="deOd" runat="server" Date='<%# Bind("DataOd") %>' ValidationGroup="evg"  />
            </td>
            <td class="data">
                <uc1:DateEdit ID="deDo" runat="server" Date='<%# Bind("DataDo") %>'   />
            </td>
            <td class="ocena">
                <asp:TextBox ID="tbOcena" runat="server" Text='<%# Bind("Ocena") %>' MaxLength="1"  />
                <asp:RequiredFieldValidator ID="reqOcena" runat="server" ControlToValidate="tbOcena" ErrorMessage="Pole wymagane" Display="Dynamic" ValidationGroup="evg" />
                <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" TargetControlID="tbOcena" FilterType="Custom" ValidChars="01234" />
            </td>
            <td><asp:Label ID="Label3" runat="server" Text='<%# Eval("Korektor") %>' /></td>
            <td><asp:Label ID="Label4" runat="server" Text='<%# Eval("Akceptujacy") %>' /></td>
            <td><asp:Label ID="Label8" runat="server" Text='<%# Eval("AcceptDate") %>' /></td>
            <td class="ocena">
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Uwagi") %>' />
            </td>
            <td><asp:Label ID="Label1" runat="server" Text='<%# Eval("StatusName") %>' /></td>
            <td id="tdControl" class="control">
<%--            <asp:Button ID="SaveButton" runat="server" CommandName="Update" Text="Zapisz" CssClass="btn btn-sm btn-success" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" CssClass="btn btn-sm btn-default" />--%>

                <asp:LinkButton ID="SaveButton" runat="server" CommandName="Update" ValidationGroup="evg" CssClass="btn xbtn-sm xbtn-default text-success"><i class="glyphicon glyphicon-floppy-disk"></i></asp:LinkButton>
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
                            <th id="th0" runat="server"><asp:LinkButton ID="LinkButton4" runat="server" Text="Data od" CommandName="Sort" CommandArgument="DataOd" /></th>
                            <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton5" runat="server" Text="Data do" CommandName="Sort" CommandArgument="DataDo" /></th>
                            <th id="Th3" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" Text="Ocena" CommandName="Sort" CommandArgument="Ocena" /></th>
                            <th id="Th5" runat="server"><asp:LinkButton ID="LinkButton6" runat="server" Text="Wnoszący o korektę" CommandName="Sort" CommandArgument="Korektor" /></th>
                            <th id="Th6" runat="server"><asp:LinkButton ID="LinkButton7" runat="server" Text="Akceptujący" CommandName="Sort" CommandArgument="Akceptujacy" /></th>
                            <th id="Th7" runat="server"><asp:LinkButton ID="LinkButton8" runat="server" Text="Data akceptacji" CommandName="Sort" CommandArgument="AcceptDate" /></th>
                            <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" Text="Uwagi" CommandName="Sort" CommandArgument="Uwagi" /></th>
                            <th id="Th4" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" Text="Status" CommandName="Sort" CommandArgument="StatusName" /></th>
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

<asp:SqlDataSource ID="dsOceny" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    CancelSelectOnNullParameter="false"
    SelectCommand="
select 
	*
, os.Nazwa as StatusName
, convert(varchar(10), DataOd, 20) DateFrom
, convert(varchar(10), DataDo, 20) DateTo
, convert(varchar(10), o.DataAkceptacji, 20) AcceptDate
, case when OkresDo is null and OkresOd is null then 0 else 1 end Lupka
, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') Korektor
, p2.Nazwisko + ' ' + p2.Imie + ISNULL(' (' + p2.KadryId + ')', '') Akceptujacy
, case when o.Status = 0 then 0 else 1 end EditVisible
from msOceny o
left join msOcenyStatus os on os.Id = o.Status
left join Pracownicy p on p.Id = o.IdKorektora
left join Pracownicy p2 on p2.Id = o.IdAkceptujacego
where o.IdPracownika = @pracId and o.IdUprawnienia = @uprId
"
    InsertCommand="INSERT INTO msOceny (IdPracownika, IdUprawnienia, DataOd, DataDo, Ocena, Status, Uwagi, DataUtworzenia, DataModyfikacji, IdKorektora) VALUES (@pracId, @uprId, @DataOd, @DataDo, @Ocena, -2, @Uwagi, GETDATE(), GETDATE(), @userId)" 
    UpdateCommand="update msOceny SET DataOd = @DataOd, DataDo = @DataDo, Ocena = @Ocena, Status = -2, Uwagi = @Uwagi, DataModyfikacji = GETDATE(), IdKorektora = @userId WHERE [Id] = @Id"
    DeleteCommand="delete from msOceny where Id = @Id"
    >
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <SelectParameters>
        <asp:ControlParameter Name="pracId" Type="Int32" ControlID="hidPracId" PropertyName="Value" />
        <asp:ControlParameter Name="uprId" Type="Int32" ControlID="hidUprId" PropertyName="Value" />
        <%--<asp:ControlParameter Name="linia" Type="Int32" ControlID="ddlLinie" PropertyName="SelectedValue" />--%>
        <%--<asp:ControlParameter Name="par2" Type="SByte" ControlID="hidParametr2Query" PropertyName="Value" />--%>
    </SelectParameters>
    <UpdateParameters>
        <asp:ControlParameter Name="userId" Type="Int32" ControlID="hidUserId" PropertyName="Value" />
        <asp:Parameter Name="DataOd" Type="DateTime" />
        <asp:Parameter Name="DataDo" Type="DateTime" />
        <asp:Parameter Name="Ocena" Type="Double" />
        <asp:Parameter Name="Uwagi" Type="String" />
    </UpdateParameters>
    <InsertParameters>
        <asp:ControlParameter Name="pracId" Type="Int32" ControlID="hidPracId" PropertyName="Value" />
        <asp:ControlParameter Name="uprId" Type="Int32" ControlID="hidUprId" PropertyName="Value" />
        <asp:ControlParameter Name="userId" Type="Int32" ControlID="hidUserId" PropertyName="Value" />
        <asp:Parameter Name="DataOd" Type="DateTime" />
        <asp:Parameter Name="DataDo" Type="DateTime" />
        <asp:Parameter Name="Ocena" Type="Double" />
        <asp:Parameter Name="Uwagi" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>
</div>

<asp:SqlDataSource ID="dsCorrect" runat="server"
    SelectCommand=
"
declare @pid int = {0}
declare @uid int = {1}
declare @from datetime = {2}
declare @to datetime = {3}
declare @id int = {4}

select case when COUNT(Id) &gt; 0 then 0 else 1 end 
from msOceny o 
where (o.Status &gt; 0 or o.Status = -2) and o.IdPracownika = @pid and o.IdUprawnienia = @uid and o.DataOd &lt; ISNULL(@to, '20990909') and @from &lt; o.DataDo and (@id is null or o.Id != @id)
    
" />
    