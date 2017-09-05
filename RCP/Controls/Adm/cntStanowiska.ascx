<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntStanowiska.ascx.cs" Inherits="HRRcp.Controls.Adm.cntStanowiska" %>

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
    onitemupdating="ListView1_ItemUpdating" ondatabound="ListView1_DataBound">
    <ItemTemplate>
        <tr class="it">
            <td class="date">
                <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od", "{0:d}") %>' />
            </td>
            <td class="date">
                <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do", "{0:d}") %>' />
            </td>
            <td id="tdDzial" runat="server" class="dzial">
                <asp:Label ID="DzialLabel" runat="server" Text='<%# Eval("Dzial") %>' />
            </td>
            <td id="tdStanowisko" runat="server" class="dzial">
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Stanowisko") %>' />
            </td>
            <td id="td1" runat="server" class="class">
                <asp:Label ID="Label2" runat="server" Text='<%# Eval("Grupa") %>' />
            </td>
            <td id="td2" runat="server" class="class">
                <asp:Label ID="Label3" runat="server" Text='<%# Eval("Klasyfikacja") %>' />
            </td>
            <td id="td5" runat="server" class="class">
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("Grade") %>' />
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
                    Brak przypisań<br />
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

            <td id="tdDzial" runat="server" class="dzial_stanowisko" colspan="2" >
                <asp:DropDownList ID="ddlStanowisko" runat="server" 
                    DataSourceID="SqlDataSource3"
                    DataTextField="Stanowisko"
                    DataValueField="IdStanowiskaX">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi"                     
                    ControlToValidate="ddlStanowisko" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>

            <td id="tdDzial2" runat="server" class="dzial" visible="false">
                <asp:DropDownList ID="ddlDzial2" runat="server" 
                    DataSourceID="dsDzialy"
                    DataTextField="Dzial"
                    DataValueField="Id">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" SetFocusOnError="True" Display="Dynamic" Enabled="false"
                    ValidationGroup="vgi"                     
                    ControlToValidate="ddlDzial2" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>

            <td id="tdStanowisko2" runat="server" class="stanowisko" visible="false">
                <asp:DropDownList ID="ddlStanowisko2" runat="server" 
                    DataSourceID="dsStanowiska"
                    DataTextField="Stanowisko"
                    DataValueField="Id">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi"                     
                    ControlToValidate="ddlStanowisko2" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>

            <td id="td3" runat="server" class="class" >
                <asp:DropDownList ID="ddlGrupa" runat="server" 
                    DataSourceID="SqlDataSource4"
                    DataTextField="SymGrupa"
                    DataValueField="Grupa">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi"                     
                    Enabled="false"
                    ControlToValidate="ddlGrupa" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td id="td4" runat="server" class="class" >
                <asp:DropDownList ID="ddlKlasyfikacja" runat="server" 
                    DataSourceID="SqlDataSource5"
                    DataTextField="SymKlasyfikacja"
                    DataValueField="Klasyfikacja">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" SetFocusOnError="True" Display="Dynamic"
                    Enabled="false"
                    ValidationGroup="vgi"                     
                    ControlToValidate="ddlKlasyfikacja" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td id="td6" runat="server" class="class" >
                <asp:DropDownList ID="ddlGrade" runat="server" DataValueField="Grade">
                    <asp:ListItem Text="wybierz..." Value="NULL"></asp:ListItem>
                    <asp:ListItem Text="" Value=""></asp:ListItem>
                    <asp:ListItem Text="A" Value="A"></asp:ListItem>
                    <asp:ListItem Text="B" Value="B"></asp:ListItem>
                    <asp:ListItem Text="C" Value="C"></asp:ListItem>
                    <asp:ListItem Text="D" Value="D"></asp:ListItem>
                </asp:DropDownList>
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
            <td id="tdDzial" runat="server" class="dzial_stanowisko" colspan="2" >
                <asp:DropDownList ID="ddlStanowisko" runat="server" 
                    DataSourceID="SqlDataSource3"
                    DataTextField="Stanowisko"
                    DataValueField="IdStanowiskaX">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vge"                     
                    ControlToValidate="ddlStanowisko" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>

            <td id="tdDzial2" runat="server" class="dzial" visible="false">
                <asp:DropDownList ID="ddlDzial2" runat="server" 
                    DataSourceID="dsDzialy"
                    DataTextField="Dzial"
                    DataValueField="Id">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" SetFocusOnError="True" Display="Dynamic" Enabled="false"
                    ValidationGroup="vgi"                     
                    ControlToValidate="ddlDzial2" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>

            <td id="tdStanowisko2" runat="server" class="stanowisko" visible="false">
                <asp:DropDownList ID="ddlStanowisko2" runat="server" 
                    DataSourceID="dsStanowiska"
                    DataTextField="Stanowisko"
                    DataValueField="Id">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgi"                     
                    ControlToValidate="ddlStanowisko2" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>

            <td id="td3" runat="server" class="class" >
                <%--            
                <asp:ComboBox ID="cbGrupa" runat="server" 
                    DataSourceID="SqlDataSource4"
                    DataTextField="SymGrupa"
                    DataValueField="Grupa" >
                </asp:ComboBox>

                --%>
                <%--
                <asp:TextBox ID="tbGrupa" runat="server" Text='<%# Bind("Grupa") %>' ValidationGroup="vge"></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" SetFocusOnError="True" Display="Dynamic"
                    Enabled="false"
                    ValidationGroup="vge"                     
                    ControlToValidate="tbGrupa" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                --%>
                <asp:DropDownList ID="ddlGrupa" runat="server" 
                    DataSourceID="SqlDataSource4"
                    DataTextField="SymGrupa"
                    DataValueField="Grupa">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    Enabled="false"
                    ValidationGroup="vge"                     
                    ControlToValidate="ddlGrupa" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td id="td4" runat="server" class="class" >
                <%--
                <asp:TextBox ID="tbKlasyfikacja" runat="server" Text='<%# Bind("Klasyfikacja") %>' ValidationGroup="vge"></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" SetFocusOnError="True" Display="Dynamic"
                    Enabled="false"
                    ValidationGroup="vge"                     
                    ControlToValidate="tbKlasyfikacja" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                --%>
                <asp:DropDownList ID="ddlKlasyfikacja" runat="server" 
                    DataSourceID="SqlDataSource5"
                    DataTextField="SymKlasyfikacja"
                    DataValueField="Klasyfikacja">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" SetFocusOnError="True" Display="Dynamic"
                    Enabled="false"
                    ValidationGroup="vge"                     
                    ControlToValidate="ddlKlasyfikacja" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td id="td7" runat="server" class="class" >
                <asp:DropDownList ID="ddlGrade" runat="server" DataValueField="Grade">
                    <asp:ListItem Text="wybierz..." Value="NULL"></asp:ListItem>
                    <asp:ListItem Text="" Value=""></asp:ListItem>
                    <asp:ListItem Text="A" Value="A"></asp:ListItem>
                    <asp:ListItem Text="B" Value="B"></asp:ListItem>
                    <asp:ListItem Text="C" Value="C"></asp:ListItem>
                    <asp:ListItem Text="D" Value="D"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" ValidationGroup="vge"/>
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="tbPracParametry tbStanowiskaOdDo">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">
                                Od</th>
                            <th runat="server">
                                Do</th>
                            <th id="thDzial" runat="server">
                                Dzial</th>
                            <th id="th1" runat="server">
                                Stanowisko</th>
                            <th id="th2" runat="server">
                                Grupa</th>
                            <th id="th3" runat="server">
                                Klasyfikacja</th>
                            <th id="th4" runat="server">
                                Grade</th>
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

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [PracownicyStanowiska] WHERE [Id] = @Id" 
    InsertCommand="
INSERT INTO [PracownicyStanowiska] ([IdPracownika], [Od], [Do], IdDzialu, IdStanowiska, Grupa, Klasyfikacja, Grade) 
    VALUES (@IdPracownika, @Od, @Do
    , case when @IdDzialu = -9 then (select IdDzialu from Stanowiska where Id = @IdStanowiska) else @IdDzialu end
    --, case when @IdDzialu2 = -9 then @IdStanowiska else @IdStanowiska2 end 
    , @IdStanowiska
    , @Grupa, @Klasyfikacja, @Grade)" 
    SelectCommand="
select R.*, D.Nazwa as Dzial, S.Nazwa as Stanowisko 
from PracownicyStanowiska R 
left outer join Dzialy D on D.Id = R.IdDzialu
left outer join Stanowiska S on S.Id = R.IdStanowiska
WHERE R.IdPracownika = @IdPracownika
ORDER BY R.Od DESC" 
    UpdateCommand="
UPDATE [PracownicyStanowiska] SET [IdPracownika] = @IdPracownika, [Od] = @Od, [Do] = @Do
    , IdDzialu = case when @IdDzialu = -9 then (select IdDzialu from Stanowiska where Id = @IdStanowiska) else @IdDzialu end
    --, IdStanowiska = case when @IdDzialu2 = -9 then @IdStanowiska else @IdStanowiska2 end 
    , IdStanowiska = @IdStanowiska
    , Grupa = @Grupa, Klasyfikacja = @Klasyfikacja, Grade = @Grade WHERE [Id] = @Id">
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
        <asp:Parameter Name="IdStanowiska" Type="Int32" />
        <asp:Parameter Name="IdDzialu" Type="Int32" />
        <asp:Parameter Name="Grupa" Type="String" />
        <asp:Parameter Name="Klasyfikacja" Type="String" />
        <asp:Parameter Name="Grade" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="IdStanowiska" Type="Int32" />
        <asp:Parameter Name="IdDzialu" Type="Int32" />
        <asp:Parameter Name="Grupa" Type="String" />
        <asp:Parameter Name="Klasyfikacja" Type="String" />
        <asp:Parameter Name="Grade" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select 0 as Sort, 1 as Aktywne, null as IdStanowiskaX, 'wybierz ...' as Stanowisko 
union
SELECT 1 as Sort, 
    case when ISNULL(D.Status, 1) &gt;= 0 and S.Aktywne = 1 then 1 else 0 end as Aktywne,
    S.Id,
    --convert(varchar, S.Id) + '|' + convert(varchar, D.Id) as IdStanowiska, 

    --case when ISNULL(D.Status, 1) &gt;= 0 and S.Aktywne = 1 then '' else '* ' end +
    ISNULL(D.Nazwa + ' - ', '- ') + S.Nazwa  
    + case when ISNuLL(D.Status, 1) &gt;= 0 and S.Aktywne = 1 then '' else ' (nieaktualne)' end as Stanowisko 
FROM Stanowiska S 
left outer join Dzialy D on D.Id = S.IdDzialu
ORDER BY Sort, Aktywne desc, Stanowisko
                    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsDzialy" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select 0 as Sort, null as Aktywny, null as Id, 'wybierz ...' as Dzial
union
SELECT 1 as Sort
, case when ISNULL(D.Status, 1) &gt;= 0 then 1 else 0 end
, D.Id
, D.Nazwa + case when ISNuLL(D.Status, 1) &gt;= 0 then '' else ' (nieaktualne)' end as Stanowisko 
FROM Dzialy D
ORDER BY Sort, Aktywny desc, Dzial
                    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsStanowiska" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select 0 as Sort, null as Aktywne, null as Id, 'wybierz ...' as Stanowisko 
union
SELECT 1 as Sort
, case when S.Aktywne = 1 then 1 else 0 end as Aktywne
, S.Id as IdS
, S.Nazwa + case when S.Aktywne = 1 then '' else ' (nieaktualne)' end
FROM Stanowiska S 
ORDER BY Sort, Aktywne desc, Stanowisko
                    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource4" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select 0 as Sort, null as Grupa, 'wybierz ...' as SymGrupa
union
select distinct 1 as Sort, TypImport as DdlGrupa, TypImport as SymGrupa from PodzialLudziImport
union
--select distinct 1 as Sort, DdlGrupa as Grupa, Grupa as SymGrupa from PracownicyStanowiska where Grupa is not null
select 1 as Sort, Nazwa as Grupa, Nazwa as SymGrupa from Kody where Typ = 'PRACGRUPA' 
order by Sort, SymGrupa">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource5" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select 0 as Sort, null as Klasyfikacja, 'wybierz ...' as SymKlasyfikacja
union                   
select distinct 1 as Sort, Class as Klasyfikacja, Class as SymKlasyfikacja from PodzialLudziImport 
union
--select distinct 1 as Sort, Klasyfikacja as DdlKlasyfikacja, Klasyfikacja as SymKlasyfikacja from PracownicyStanowiska where Klasyfikacja is not null
select 1 as Sort, Nazwa as Klasyfikacja, Nazwa as SymKlasyfikacja from Kody where Typ = 'PRACKLAS' 
order by Sort, SymKlasyfikacja">
</asp:SqlDataSource>




