<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPominAdm.ascx.cs" Inherits="HRRcp.Controls.cntPominAdm" %>
<%@ Register src="../DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>

<div class="paFilter_right">
    <div class="left">
<%--
        ɎѼѪẎŸỸỴ¥Ұұ
--%>    
        <asp:DropDownList ID="ddlPracFilter" runat="server" CssClass="form-control" 
            DataSourceID="SqlDataSource2" DataTextField="Pracownik" 
            DataValueField="IdPracownika" AutoPostBack="true" 
            onselectedindexchanged="ddlPracFilter_SelectedIndexChanged">
        </asp:DropDownList>                    
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
            SelectCommand="
select null as IdPracownika, '- pracownik -' as Pracownik, 1 as Sort
union all            
select distinct X.IdPracownika, P.Nazwisko + ' ' + P.Imie + ISNULL(' (' + P.KadryId + ')', '') as Pracownik, 2 as Sort 
from PlanUrlopowPomin X
left join Pracownicy P on P.Id = X.IdPracownika
order by Sort, Pracownik
            ">
        </asp:SqlDataSource>
    </div>
    <div>
        <asp:CheckBox ID="cbNaDzien" runat="server" Text="Pokaż na dzień:" 
            Checked="true" AutoPostBack="true" 
            oncheckedchanged="cbNaDzien_CheckedChanged" />
        <uc1:DateEdit ID="dtNaDzien" runat="server" AutoPostBack="true" OnDateChanged="dtNaDzien_Changed"/>
        <asp:HiddenField ID="hidNaDzien" runat="server" Visible="false"/>
    </div>
</div>

<asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" DataSourceID="SqlDataSource1" 
    InsertItemPosition="None" onitemcreated="ListView1_ItemCreated" 
    onitemdatabound="ListView1_ItemDataBound" 
    oniteminserting="ListView1_ItemInserting" 
    onitemupdating="ListView1_ItemUpdating" 
    onitemcommand="ListView1_ItemCommand">
    <ItemTemplate>
        <tr class="it">
            <td class="pracownik">
                <asp:Label ID="IdPracownikaLabel" runat="server" Text='<%# Eval("Pracownik") %>' />
            </td>
            <td class="nrew">
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("KadryId") %>' />
            </td>
            <td class="data">
                <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od", "{0:d}") %>' />
            </td>
            <td class="data">
                <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do", "{0:d}") %>' />
            </td>
            <td class="powod">
                <asp:Label ID="PowodLabel" runat="server" Text='<%# Eval("PowodNapis") %>' />
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
                    Brak danych<br />
                    <asp:Button ID="InsertButton" runat="server" CssClass="button" CommandName="NewRecord" Text="Dodaj pracownika" />                    
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <InsertItemTemplate>
        <tr class="iit">
            <td colspan="2" class="pracownik">
                <asp:DropDownList ID="ddlPracownik" runat="server">
                </asp:DropDownList>                    
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    CssClass="error"
                    ValidationGroup="ivg"                     
                    ControlToValidate="ddlPracownik" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td class="data">
                <uc1:DateEdit ID="deOd" runat="server" Date='<%# Bind("Od") %>' ValidationGroup="ivg"/>
            </td>
            <td class="data">
                <uc1:DateEdit ID="deDo" runat="server" Date='<%# Bind("Do") %>' />
            </td>
            <td class="powod">
                <asp:DropDownList ID="ddlPowod" runat="server">
                </asp:DropDownList>                    
                <asp:TextBox ID="PowodTextBox" runat="server" Text='<%# Bind("Powod") %>' />
            </td>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" />
                <asp:Button ID="btDoInsert" runat="server" CssClass="button_postback" CommandName="Insert" Text="Insert" />
                <asp:Button ID="CancelButton" runat="server" CommandName="CancelInsert" Text="Clear" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td colspan="2" class="pracownik">
                <asp:DropDownList ID="ddlPracownik" runat="server">
                </asp:DropDownList>                    
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                    CssClass="error"
                    ValidationGroup="evg"                     
                    ControlToValidate="ddlPracownik" 
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            <td class="data">
                <uc1:DateEdit ID="deOd" runat="server" Date='<%# Bind("Od") %>' ValidationGroup="evg"/>
            </td>
            <td class="data">
                <uc1:DateEdit ID="deDo" runat="server" Date='<%# Bind("Do") %>' />
            </td>
            <td class="powod">
                <asp:DropDownList ID="ddlPowod" runat="server">
                </asp:DropDownList>                    
                <asp:TextBox ID="PowodTextBox" runat="server" Text='<%# Bind("Powod") %>' />
            </td>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                <asp:Button ID="btDoUpdate" runat="server" CssClass="button_postback" CommandName="Update" Text="Update" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server"><asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Pracownik">Pracownik</asp:LinkButton></th>
                            <th runat="server"><asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="KadryId">Nr ewid.</asp:LinkButton></th>
                            <th runat="server"><asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Od">Data od</asp:LinkButton></th>
                            <th runat="server"><asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="Do">Data do</asp:LinkButton></th>
                            <th runat="server"><asp:LinkButton ID="LinkButton5" runat="server" CommandName="Sort" CommandArgument="PowodNapis">Powod</asp:LinkButton></th>
                            <th id="Th1" runat="server" class="control">
                                <asp:Button ID="InsertButton" runat="server" CommandName="NewRecord" Text="Insert" />                    
                            </th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="pager">
                <td>
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                </td>
                <td class="right">
                    <span class="count">Ilość:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                    &nbsp;&nbsp;&nbsp;
                    <span class="count">Pokaż na stronie:&nbsp;&nbsp;&nbsp;</span>
                    <asp:DropDownList ID="ddlLines" runat="server" >
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" CancelSelectOnNullParameter="false"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    DeleteCommand="DELETE FROM [PlanUrlopowPomin] WHERE [Id] = @Id" 
    InsertCommand="
INSERT INTO [PlanUrlopowPomin] ([Rok], [IdPracownika], [Powod], [PowodKod], [AutorId], [DataWpisu], [Od], [Do]) VALUES (dbo.boy(@Od), @IdPracownika, @Powod, @PowodKod, @AutorId, GETDATE(), @Od, @Do)
        " 
    SelectCommand="
select PP.*, 
	P.Nazwisko + ' ' + P.Imie as Pracownik, P.KadryId,
	P.KierownikNI as Kierownik,
	ISNULL(K.Nazwa + ' (' + K.Nazwa2 + ')', '') as PowodKodNapis, 
	ISNULL(K.Nazwa + ' (' + K.Nazwa2 + ')', '') + 
	case when K.Kod is not null and PP.Powod is not null then ' - ' else '' end +
	ISNULL(PP.Powod, '') as PowodNapis  
from PlanUrlopowPomin PP
left join VPrzypisaniaNaDzis P on P.Id = PP.IdPracownika 
left join Kody K on K.Typ = 'ABSDL' and K.Kod = PP.PowodKod
where (@nadzien is null or (@nadzien between PP.Od and ISNULL(PP.Do, '20990909'))) 
  and (@prac is null or PP.IdPracownika = @prac)
order by Pracownik
        " 
    UpdateCommand="
UPDATE [PlanUrlopowPomin] SET [Powod] = @Powod, [PowodKod] = @PowodKod, [AutorId] = @AutorId, [DataWpisu] = GETDATE(), [Od] = @Od, [Do] = @Do WHERE [Id] = @Id
        ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidNaDzien" Name="nadzien" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="ddlPracFilter" Name="prac" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Powod" Type="String" />
        <asp:Parameter Name="PowodKod" Type="Int32" />
        <asp:Parameter Name="AutorId" Type="Int32" />
        <asp:Parameter Name="DataWpisu" Type="DateTime" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
        <asp:Parameter Name="Rok" Type="DateTime" />
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Rok" Type="DateTime" />
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Powod" Type="String" />
        <asp:Parameter Name="PowodKod" Type="Int32" />
        <asp:Parameter Name="AutorId" Type="Int32" />
        <asp:Parameter Name="DataWpisu" Type="DateTime" />
        <asp:Parameter Name="Od" Type="DateTime" />
        <asp:Parameter Name="Do" Type="DateTime" />
    </InsertParameters>
</asp:SqlDataSource>

