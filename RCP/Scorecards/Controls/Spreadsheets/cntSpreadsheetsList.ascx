<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSpreadsheetsList.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Spreadsheets.cntSpreadsheetsList" %>

<asp:HiddenField id="hidObserverId" runat="server" Visible="false" />

<div id="ctSpreadsheets" runat="server" class="cntSpreadsheets" >

     <asp:ListView ID="lvSpreadsheets" runat="server" DataSourceID="dsSpreadsheets" DataKeyNames="" >
    <ItemTemplate>
        <tr id="Tr1" style="" runat="server" class="it">
            <td class="name"><asp:Label ID="Label1" runat="server" Text='<%# Eval("Miesiac") %>' /></td>
            <td><asp:Label ID="Label2" runat="server" Text='<%# Eval("Przelozony") %>' /></td>
            <td><asp:Label ID="Label3" runat="server" Text='<%# Eval("Arkusz") %>' /></td>
            <td><asp:Label ID="Label4" runat="server" Text='<%# Eval("Pracownik") %>' /></td>
            <td id="tdControl" class="control" runat="server">
                <asp:Button ID="lnkShowSpreadsheet" runat="server" OnClick="ShowSpreadsheet" Text="Arkusz" CommandArgument='<%# Eval("ObserverId") + ";" + Eval("EmployeeId") + ";" + Eval("ScorecardTypeId") + ";" + Eval("Date") %>' />
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
    <LayoutTemplate>
        <table id="Table1" runat="server" class="ListView1 tbZastepstwa hoverline">
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" class="lvSpreadsheets">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" Text="Miesiąc" CommandName="Sort" CommandArgument="Miesiac" /></th>
                            <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" Text="Przełożony" CommandName="Sort" CommandArgument="Przelozony" /></th>
                            <th id="Th3" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" Text="Arkusz" CommandName="Sort" CommandArgument="Arkusz" /></th>
                            <th id="Th4" runat="server"><asp:LinkButton ID="LinkButton4" runat="server" Text="Pracownik" CommandName="Sort" CommandArgument="Pracownik" /></th>
                            <th id="thControl" class="control" runat="server">
                            </th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trPager" runat="server">
                <td id="Td2" runat="server" class="pager">
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="5">
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

<asp:SqlDataSource ID="dsSpreadsheets" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
declare @ObserverId int = @obs
declare @date datetime = dbo.bom(GETDATE())

select
  convert(varchar(7),@date,20) as Miesiac
, k.Nazwisko + ' ' + k.Imie as Przelozony
, ta.Nazwa [Arkusz]
, (pr.Nazwisko + ' ' + pr.Imie + isnull(' (' + pr.KadryId + ')', '')) [Pracownik]
--, case when prem.Id is not null then 'Zaakceptowany' else '' end as [Status akceptacji]
, k.Id as ObserverId
, pr.Id as EmployeeId
, ta.Id as ScorecardTypeId
, @date as Date
from dbo.fn_GetTreeOkres(@ObserverId, dbo.bom(@Date), dbo.eom(@Date), @Date) p
left join scTypyArkuszy ta on p.IdCommodity = ta.Id
left join Pracownicy pr on p.IdPracownika = pr.Id
left join scSlowniki s on ta.Rodzaj = s.Id
left join Pracownicy k on k.Id = p.IdKierownika
left join scWnioski w on w.IdTypuArkuszy = ta.Id and w.IdPracownika = k.Id and w.Data = @date
left join scPremie prem on prem.IdWniosku = w.Id and prem.IdPracownika = pr.Id and prem._do is null
--where s.Nazwa2 = 'ARKI'
where s.Nazwa2 = 'ARKI' and ta.Wniosek = 1 and prem.Id is null
union all
select distinct 
  convert(varchar(7),@date,20) [Miesiac]
, k.Nazwisko + ' ' + k.Imie [Przelozony]
, ta.Nazwa [Arkusz]
, ta.Nazwa [Pracownik]
, k.Id as ObserverId
, /*0 - ta.Id*/ null as EmployeeId
, ta.Id as ScorecardTypeId
, @date as Date
--, case when prem.Id is not null then 'Zaakceptowany' else '' end as [Status akceptacji]
from dbo.fn_GetTreeOkres(@ObserverId, dbo.bom(@Date), dbo.eom(@Date), @Date) p
left join scTypyArkuszy ta on p.IdCommodity = ta.Id
left join scSlowniki s on ta.Rodzaj = s.Id
left join Pracownicy k on k.Id = p.IdKierownika
left join scWnioski w on w.IdTypuArkuszy = ta.Id and w.IdPracownika = k.Id and w.Data = @date
left join scPremie prem on prem.IdWniosku = w.Id and prem.IdPracownika = 0 - ta.Id
where s.Nazwa2 = 'ARKZ' and ta.Wniosek = 1 and prem.Id is null
order by 1, 2, 3, 4
"
>
    <SelectParameters>
        <asp:ControlParameter ControlID="hidObserverId" PropertyName="Value" Type="Int32" Name="obs" />
    
    </SelectParameters>
</asp:SqlDataSource>




</div>