<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPrzypisaniaMini.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Przypisania.cntPrzypisaniaMini" %>

<asp:HiddenField ID="hidPracId" runat="server" />
<asp:HiddenField ID="hidDataOd" runat="server" />

<asp:ListView ID="lvPrzypisaniaMini" runat="server" DataKeyNames="Id" 
    DataSourceID="SqlDataSource1" 
    onselectedindexchanged="lvPrzypisaniaMini_SelectedIndexChanged" 
    onitemdatabound="lvPrzypisaniaMini_ItemDataBound" 
    onpagepropertieschanged="lvPrzypisaniaMini_PagePropertiesChanged">
    <ItemTemplate>
        <tr id="trLine" runat="server" class="it">
            <td class="select">
                <asp:Button ID="SelectButton" runat="server" CommandName="Select" Text="Pokaż" />
            </td>
            <td class="data">
                <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od", "{0:d}") %>' />
            </td>
            <td class="data">
                <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do", "{0:d}") %>' />
            </td>
            <td class="nazwisko">
                <asp:Label ID="KierownikLabel" runat="server" Text='<%# Eval("Kierownik") %>' />
            </td>
            <td class="status lastcol">
                <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
            </td>
            <td id="tdControl" runat="server" class="control" visible="false">
                <asp:Button ID="btCopy" runat="server" Text="Ustaw ►" />
            </td>
        </tr>
    </ItemTemplate>
    <SelectedItemTemplate>
        <tr id="trLine" runat="server" class="sit">
            <td class="select"></td>
            <td class="data">
                <asp:Label ID="OdLabel" runat="server" Text='<%# Eval("Od", "{0:d}") %>' />
            </td>
            <td class="data">
                <asp:Label ID="DoLabel" runat="server" Text='<%# Eval("Do", "{0:d}") %>' />
            </td>
            <td class="nazwisko">
                <asp:Label ID="KierownikLabel" runat="server" Text='<%# Eval("Kierownik") %>' />
            </td>
            <td class="status lastcol">
                <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
            </td>
            <td id="tdControl" runat="server" class="control" visible="false">
                <asp:Button ID="btCopy" runat="server" Text="Ustaw ►" />
            </td>
        </tr>
    </SelectedItemTemplate>
    <EmptyDataTemplate>
        <table id="Table1" runat="server" class="tbPrzypisaniaMini ListView1 tbBrowser table0 narrow">
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr id="Tr2" runat="server" style="">
                            <th class="select"></th>
                            <th id="Th1" runat="server">
                                Od</th>
                            <th id="Th2" runat="server">
                                Do</th>
                            <th id="Th3" runat="server">
                                Przełożony</th>
                            <th id="Th4" runat="server" class="lastcol">
                                Status</th>
                        </tr>
                        <tr runat="server">
                            <td colspan="5">
                                <%--
                                Brak zaplanowanych przesunięć
                                --%>
                                Pracownik nieprzypisany
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table runat="server" class="tbPrzypisaniaMini table0 narrow">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th id="thSelect" runat="server" class="select"></th>
                            <th id="Th6" runat="server">
                                Od</th>
                            <th runat="server">
                                Do</th>
                            <th runat="server">
                                Przełożony</th>
                            <th runat="server" class="lastcol">
                                Status</th>
                            <th id="trControl" runat="server" visible="false"></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" class="pager">
                <td runat="server" style="">
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="3">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="false" ShowLastPageButton="false" 
                                NextPageText="►"
                                PreviousPageText="◄"
                                />
                        </Fields>
                    </asp:DataPager>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select R.Id, R.Od, R.Do, ISNULL(K.Nazwisko + ' ' + K.Imie, 'Główny poziom struktury') as Kierownik, S.Status 
from Przypisania R
left outer join Pracownicy K on K.Id = R.IdKierownika
left outer join PrzypisaniaStatusy S on S.Id = R.Status
where R.IdPracownika = @pracId 
    and R.Status in (0, 1)
    --and ((ISNULL(R.Do, '20990909') &gt;= @dataOd and R.Status = 0) or  -- wszystkie oczekujące
	--     (R.Od &gt;= @dataOd and R.Status = 1))	-- zaakceptowane
order by R.Status, R.Od desc
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="pracId" PropertyName="Value" />
        <asp:ControlParameter ControlID="hidDataOd" Name="dataOd" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>


