<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntTypOkresuRozl.ascx.cs" Inherits="HRRcp.Controls.Adm.cntTypOkresuRozl" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="../DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>
<%@ Register src="../TimeEdit.ascx" tagname="TimeEdit" tagprefix="uc1" %>

<asp:HiddenField ID="hidPracId" runat="server" />

<asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" InsertItemPosition="None" 
    onitemcreated="ListView1_ItemCreated" 
    onitemdatabound="ListView1_ItemDataBound" onitemdeleted="ListView1_ItemDeleted" 
    oniteminserted="ListView1_ItemInserted" onitemupdated="ListView1_ItemUpdated" 
    onlayoutcreated="ListView1_LayoutCreated" 
    oniteminserting="ListView1_ItemInserting" 
    onitemupdating="ListView1_ItemUpdating" ondatabound="ListView1_DataBound">
    <ItemTemplate>
        <tr class="it">
            <td class="date">
                <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od", "{0:d}") %>' />
            </td>
            <td class="date">
                <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do", "{0:d}") %>' />
            </td>
            <td id="tdOkres" runat="server" class="okres">
                <asp:Label ID="OkresLabel" runat="server" Text='<%# Eval("Okres") %>' />
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
                    <br />
                    <br />
                    <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj" />                  
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
            <td id="tdOkres" runat="server" class="okres" >
                <asp:DropDownList ID="ddlOkres" runat="server" 
                    DataSourceID="SqlDataSource3"
                    DataTextField="Nazwa"
                    DataValueField="IdTypuOkresu">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi"                     
                    ControlToValidate="ddlOkres" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" ValidationGroup="vgi"/>
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
                <asp:Button ID="btCancelInsert" runat="server" CommandName="CancelInsert" Text="Anuluj" />
            </td>
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
            <td id="tdOkres" runat="server" class="okres">
                <asp:DropDownList ID="ddlOkres" runat="server" 
                    DataSourceID="SqlDataSource3"
                    DataTextField="Nazwa"
                    DataValueField="IdTypuOkresu">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vge"                     
                    ControlToValidate="ddlOkres" 
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
        <table runat="server" class="tbPracParametry tbAlgorytmyRCP">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">
                                Od</th>
                            <th runat="server">
                                Do</th>
                            <th id="thOkres" runat="server">
                                Okres rozliczeniowy</th>
                            <th runat="server" class="control">
                                <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Dodaj" />                  
                            </th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trPager1" runat="server" class="pager">
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
                    <span class="count">Ilość rekordów:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                    <%--
                    <span class="count">Pokaż na stronie:&nbsp;&nbsp;&nbsp;</span>
                    <asp:DropDownList ID="ddlLines" runat="server" ></asp:DropDownList>
                    --%>
                </td>
            </tr>
            <%--                    
            <tr id="trPager2" runat="server" class="pager">
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

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
SELECT 
  o.DataOd Od
, o.DataDo Do
, o.*
, t.Nazwa Okres
FROM rcpPracownicyTypyOkresow o 
left join rcpOkresyRozliczenioweTypy t on t.Id = o.IdTypuOkresu
WHERE o.IdPracownika = @IdPracownika
ORDER BY o.DataOd DESC" 
    DeleteCommand="DELETE FROM [rcpPracownicyTypyOkresow] WHERE [Id] = @Id" 
    InsertCommand="
INSERT INTO [rcpPracownicyTypyOkresow] ([IdPracownika], [DataOd], [DataDo], [IdTypuOkresu]) VALUES (@IdPracownika, @Od, @Do, @IdTypuOkresu) 
-- nie ma triggera :(
update rcpPracownicyTypyOkresow set DataDo = DATEADD(D, -1, @Od) 
where Id = (select top 1 Id from rcpPracownicyTypyOkresow where IdPracownika = @IdPracownika and DataOd &lt; @Od order by DataOd desc) -- poprzedni wpis
    and DataDo is null   -- tylko jak pusto
    " 
    UpdateCommand="
UPDATE [rcpPracownicyTypyOkresow] SET [DataOd] = @Od, [DataDo] = @Do, [IdTypuOkresu] = @IdTypuOkresu WHERE [Id] = @Id
    ">
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
        <asp:Parameter Name="IdTypuOkresu" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="IdTypuOkresu" Type="Int32" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select 0 Sort, null IdTypuOkresu, 'wybierz ...' Nazwa, 0 Aktywny, 0 IloscMiesiecy 
from (select 1 x) x
outer apply (select count(*) Cnt from rcpOkresyRozliczenioweTypy where Aktywny = 1) o
where o.Cnt != 1    -- brak lub więcej niż 1
union all
select 1 Sort, Id IdTypuOkresu, Nazwa + case when Aktywny = 0 then ' (nieaktywny)' else '' end Nazwa, Aktywny, IloscMiesiecy
from rcpOkresyRozliczenioweTypy 
order by Sort, Aktywny desc, IloscMiesiecy
    " />

