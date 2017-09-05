<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntRequestsAdmin.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Requests.cntRequestsAdmin" %>

<%@ Register Src="~/Scorecards/Controls/Requests/cntRequest.ascx" TagPrefix="leet" TagName="Request" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>

<div id="ctRequestsAdmin" runat="server" class="cntRequestsAdmin">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        
        
            <div id="divZoom" style="display: none;" class="modalPopup">
                <asp:UpdatePanel ID="upZoom" runat="server">
                    <ContentTemplate>
                        <leet:Request ID="Request" runat="server" OnSomethingChanged="Request_SomethingChanged" Visible="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        
            
             <asp:ListView ID="lvRequests" runat="server" DataSourceID="dsRequests" DataKeyNames="Id"
                InsertItemPosition="LastItem" OnItemInserting="lvRequests_ItemInserting" OnItemUpdating="lvRequests_ItemUpdating" OnItemCreated="lvRequests_ItemCreated" OnItemDataBound="lvRequests_ItemDataBound" >
                <ItemTemplate>
                    <tr id="" class="it">
                        <td class="">
                            <asp:Label ID="Label11" runat="server" Text='<%# Eval("Id") %>' />
                        </td>
                        <td class="">
                            <asp:HiddenField ID="hidStatus" runat="server" Visible="false" Value='<%# Eval("Status") %>' />
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("Name") %>' />
                        </td>
                        <td class="">
                            <asp:Label ID="Label6" runat="server" Text='<%# Eval("Owner") %>' />
                        </td>
                        <td class="data">
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Date") %>' />
                        </td>
                        <td class="data">
                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("DataWyplaty") %>' />
                        </td>
                        <td class="num">
                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("BilansOtwarciaName") %>' />
                        </td>
                        <td class="num">
                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("IloscPracownikow") %>' />
                        </td>
                        <td>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# GetStatus((int)Eval("Status"), (int)Eval("Kacc"), (int)Eval("Pacc")) %>' />
                        </td>
                        <td>
                            <asp:Label ID="lblKacc" runat="server" Text='<%# GetAcc((int)Eval("Kacc")) %>' />
                        </td>
                        <td>
                            <asp:Label ID="lblPacc" runat="server" Text='<%# GetAcc((int)Eval("Pacc")) %>' />
                        </td>
                        <td class="control">
                            <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
                            <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />
                            <asp:Button ID="btnShow" runat="server" Text="Pokaż" OnClick="ShowRequest" style="display: block; margin: 0 auto;" CommandArgument='<%# Eval("IdWniosku") + ";" + Eval("Status") + ";" + Eval("Type") + ";" + Eval("Rodzaj") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <table id="Table1" runat="server" class="table0">
                        <tr class="edt">
                            <td>
                                <asp:Label ID="lbNoData" runat="server" Text="Brak danych" /><br />
                                <br />
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <InsertItemTemplate>
                    <tr class="iit">
                        <td>
                        </td>
                        <td class="type">
                            <asp:RadioButtonList ID="rblType" runat="server" RepeatLayout="Table" RepeatDirection="Vertical" CellPadding="0" CellSpacing="0" BorderStyle="None" CssClass="rbl">
                                <asp:ListItem Value="1" Text="Własny" Selected="True" />
                                <asp:ListItem Value="0" Text="Arkusz" />
                            </asp:RadioButtonList>
                            <div>
                                <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Nazwa") %>' CssClass="tbName"  />
                                <asp:DropDownList ID="ddlSpreadsheets" runat="server" DataSourceID="dsSpreadsheets" DataValueField="Id" DataTextField="Name" CssClass="ddlSpreadsheets" Enabled="false"  />
                                <asp:SqlDataSource ID="dsSpreadsheets" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                                    SelectCommand="select Id, Nazwa as Name from scTypyArkuszy where Aktywny = 1" />
                                
                            </div>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOwner" runat="server" DataSourceID="dsOwner" DataValueField="Id" DataTextField="Name" />
                            <asp:SqlDataSource ID="dsOwner" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                                SelectCommand="select Id, Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')','') as Name from Pracownicy" />
                        </td>               
                        <td class="data">
                            <uc1:DateEdit ID="deDate" runat="server" Date='<%# Bind("Data") %>' />
                        </td>
                        <td class="data">
                            <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataWyplaty") %>' />
                        </td>
                        <td>
                            <asp:TextBox ID="tbBilansOtwarcia" runat="server" Text='<%# Bind("BilansOtwarcia") %>' />
                        </td>
                        <td>
                            <asp:TextBox ID="tbIloscPracownikow" runat="server" Text='<%# Bind("IloscPracownikow") %>' />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server">
                                <asp:ListItem Value="-1" Text="Odrzucony" />
                                <asp:ListItem Value="0" Text="U twórcy" />
                                <asp:ListItem Value="1" Text="U kierownika" />
                                <asp:ListItem Value="2" Text="W zarządzie" />
                                <asp:ListItem Value="3" Text="W HR" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlKacc" runat="server">
                                <asp:ListItem Value="-1" Text="Do akceptacji" />
                                <asp:ListItem Value="0" Text="Odrzucony" />
                                <asp:ListItem Value="1" Text="Zaakceptowany" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPacc" runat="server">
                                <asp:ListItem Value="-1" Text="Do akceptacji" />
                                <asp:ListItem Value="0" Text="Odrzucony" />
                                <asp:ListItem Value="1" Text="Zaakceptowany" />
                            </asp:DropDownList>
                        </td>
                        <td id="tdControl" class="control">
                            <asp:Button ID="btSave" runat="server" CommandName="Insert" Text="Zapisz" />
                        </td>
                    </tr>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <tr class="eit">
                      <td>
                        </td>
                        <td class="type">
                            <asp:RadioButtonList ID="rblType" runat="server" RepeatLayout="Table" RepeatDirection="Vertical" CellPadding="0" CellSpacing="0" BorderStyle="None" CssClass="rbl">
                                <asp:ListItem Value="1" Text="Własny" Selected="True" />
                                <asp:ListItem Value="0" Text="Arkusz" />
                            </asp:RadioButtonList>
                            <div>
                                <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Nazwa") %>' CssClass="tbName"  />
                                <asp:DropDownList ID="ddlSpreadsheets" runat="server" DataSourceID="dsSpreadsheets" DataValueField="Id" DataTextField="Name" CssClass="ddlSpreadsheets" Enabled="false"  />
                                <asp:SqlDataSource ID="dsSpreadsheets" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                                    SelectCommand="select Id, Nazwa as Name from scTypyArkuszy where Aktywny = 1" />
                                
                            </div>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOwner" runat="server" DataSourceID="dsOwner" DataValueField="Id" DataTextField="Name" />
                            <asp:SqlDataSource ID="dsOwner" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                                SelectCommand="select Id, Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')','') as Name from Pracownicy" />
                        </td>               
                        <td class="data">
                            <uc1:DateEdit ID="deDate" runat="server" Date='<%# Bind("Data") %>' />
                        </td>
                        <td class="data">
                            <uc1:DateEdit ID="DateEdit1" runat="server" Date='<%# Bind("DataWyplaty") %>' />
                        </td>
                        <td>
                            <asp:TextBox ID="tbBilansOtwarcia" runat="server" Text='<%# Bind("BilansOtwarcia") %>' />
                        </td>
                        <td>
                            <asp:TextBox ID="tbIloscPracownikow" runat="server" Text='<%# Bind("IloscPracownikow") %>' />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server">
                                <asp:ListItem Value="-1" Text="Odrzucony" />
                                <asp:ListItem Value="0" Text="U twórcy" />
                                <asp:ListItem Value="1" Text="U kierownika" />
                                <asp:ListItem Value="2" Text="W zarządzie" />
                                <asp:ListItem Value="3" Text="W HR" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlKacc" runat="server">
                                <asp:ListItem Value="-1" Text="Do akceptacji" />
                                <asp:ListItem Value="0" Text="Odrzucony" />
                                <asp:ListItem Value="1" Text="Zaakceptowany" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPacc" runat="server">
                                <asp:ListItem Value="-1" Text="Do akceptacji" />
                                <asp:ListItem Value="0" Text="Odrzucony" />
                                <asp:ListItem Value="1" Text="Zaakceptowany" />
                            </asp:DropDownList>
                        </td>
                        <td id="tdControl" class="control">
                            <asp:Button ID="btSave" runat="server" CommandName="Update" Text="Zapisz" />
                            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                        </td>
                    </tr>
                </EditItemTemplate>
                <LayoutTemplate>
                    <table id="Table1" runat="server" class="ListView1 ListView4 tbZastepstwa tbRequests hoverline table0">
                        <tr id="Tr1" runat="server">
                            <td id="Td1" runat="server">
                                <table id="itemPlaceholderContainer" runat="server">
                                    <tr id="Tr2" runat="server" style="">
                                        <th id="th1" runat="server"><asp:LinkButton ID="LinkButton0" runat="server" Text="Id" CommandName="Sort" CommandArgument="Id" /></th>
                                        <th id="th5" runat="server"><asp:LinkButton ID="LinkButton5" runat="server" Text="Nazwa / Arkusz" CommandName="Sort" CommandArgument="Name" /></th>
                                        <th id="th10" runat="server"><asp:LinkButton ID="LinkButton10" runat="server" Text="Wnioskujący" CommandName="Sort" CommandArgument="Owner" /></th>
                                        <th id="th20" runat="server"><asp:LinkButton ID="LinkButton20" runat="server" Text="Data" CommandName="Sort" CommandArgument="Date" /></th>
                                        <th id="th25" runat="server"><asp:LinkButton ID="LinkButton25" runat="server" Text="Miesiąc wypłaty" CommandName="Sort" CommandArgument="DataWyplaty" /></th>
                                        <th id="th30" runat="server"><asp:LinkButton ID="LinkButton30" runat="server" Text="Bilans Otwarcia" CommandName="Sort" CommandArgument="BilansOtwarcia" /></th>
                                        <th id="th35" runat="server"><asp:LinkButton ID="LinkButton35" runat="server" Text="Ilość Pracowników" CommandName="Sort" CommandArgument="IloscPracownikow" /></th>
                                        <th id="th40" runat="server"><asp:LinkButton ID="LinkButton40" runat="server" Text="Status" CommandName="Sort" CommandArgument="Status" /></th>
                                        <th id="th45" runat="server"><asp:LinkButton ID="LinkButton45" runat="server" Text="Kierownik" CommandName="Sort" CommandArgument="Kacc" /></th>
                                        <th id="th50" runat="server"><asp:LinkButton ID="LinkButton50" runat="server" Text="Zarząd" CommandName="Sort" CommandArgument="Pacc" /></th>
                                        <th></th>
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
                                        <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true"
                                            ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false"
                                            FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                                        <asp:NumericPagerField ButtonType="Link" />
                                        <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false"
                                            ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true"
                                            NextPageText="Następna" LastPageText="Ostatnia" />
                                    </Fields>
                                </asp:DataPager>
                            </td>
                        </tr>
                        <%-- <tr id="tr3" runat="server" >
                <td class="bottom_buttons">
                    <asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord" Text="Dodaj zastępstwo" />
                </td>
            </tr>--%>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
            
            
                        <asp:SqlDataSource ID="dsRequests" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                SelectCommand="
select
  w.Id
, ISNULL(w.Nazwa, ta.Nazwa) as Name
, LEFT(CONVERT(varchar, w.Data, 20), 10) as Date
, w.Id as IdWniosku
, w.Status
, w.Kacc
, w.Pacc
, w.Nazwa
, w.IdPracownika
, w.IdTypuArkuszy
, w.Data
, w.BilansOtwarcia
, LEFT(CONVERT(varchar, w.DataWyplaty, 20), 10) as DataWyplaty
, convert(varchar, w.BilansOtwarcia) + ' zł' as BilansOtwarciaName
, oa.HeadCount as IloscPracownikow --, w.IloscPracownikow
, case when ta.Rodzaj = 4 then 0 else 1 end as Rodzaj
, case when w.IdTypuArkuszy = -1337 then 1 else 0 end as Type
, p.Nazwisko + ' ' + p.Imie + isnull(' (' + p.KadryId + ')', '') + ISNULL(' [' + p3.Nazwisko + p3.Imie + ']', '') as Owner
, p2.Nazwisko + ' ' + p2.Imie + isnull(' (' + p2.KadryId + ')', '') as Kierownik
from scWnioski w
left join scTypyArkuszy ta on w.IdTypuArkuszy = ta.Id
left join Pracownicy p on p.Id = w.IdPracownika
left join Przypisania prz on prz.IdPracownika = w.IdPracownika and prz.Status = 1 and w.Data between prz.Od and isnull(prz.Do, '20990909')
left join Pracownicy p2 on p2.Id = prz.IdKierownika
left join Pracownicy p3 on p3.Id = w.IdAkceptujacego and w.IdAkceptujacego != w.IdPracownika
outer apply (select COUNT(*) as HeadCount from scPremie prem where prem.IdWniosku = w.Id and prem._do is null and prem.IdPracownika != w.IdPracownika) oa
" 
UpdateCommand="update scWnioski set IdTypuArkuszy = @IdTypuArkuszy, IdPracownika = @IdPracownika, Data = @Data, DataWyplaty = @DataWyplaty, BilansOtwarcia = @BilansOtwarcia, IloscPracownikow = @IloscPracownikow, Status = @Status, Kacc = @Kacc, Pacc = @Pacc, Nazwa = @Nazwa where Id = @Id"
InsertCommand="insert into scWnioski (IdTypuArkuszy, IdPracownika, Data, DataWyplaty, BilansOtwarcia, IloscPracownikow, Status, Kacc, Pacc, Nazwa) values (@IdTypuArkuszy, @IdPracownika, @Data, @DataWyplaty, @BilansOtwarcia, @IloscPracownikow, @Status, @Kacc, @Pacc, @Nazwa)"
DeleteCommand="delete from scPremie where IdWniosku = @Id; delete from scWnioski where Id = @Id"
>
    <InsertParameters>
        <asp:Parameter Name="IdTypuArkuszy" Type="Int32" />
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Data" Type="DateTime" />
        <asp:Parameter Name="DataWyplaty" Type="DateTime" />
        <asp:Parameter Name="BilansOtwarcia" Type="Double" />
        <asp:Parameter Name="IloscPracownikow" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Kacc" Type="Int32" />
        <asp:Parameter Name="Pacc" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="IdTypuArkuszy" Type="Int32" />
        <asp:Parameter Name="IdPracownika" Type="Int32" />
        <asp:Parameter Name="Data" Type="DateTime" />
        <asp:Parameter Name="DataWyplaty" Type="DateTime" />
        <asp:Parameter Name="BilansOtwarcia" Type="Double" />
        <asp:Parameter Name="IloscPracownikow" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Kacc" Type="Int32" />
        <asp:Parameter Name="Pacc" Type="Int32" />
        <asp:Parameter Name="Nazwa" Type="String" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
</asp:SqlDataSource>
     
        </ContentTemplate>
    </asp:UpdatePanel>
</div>