<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntEmployeeList.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Requests.cntEmployeeList" %>

<asp:HiddenField ID="hidObserverId" runat="server" Visible="false" />

        
         <asp:ListView ID="lvEmployees" runat="server" DataSourceID="dsEmployees" DataKeyNames="Id" >
    <ItemTemplate>
        <tr id="trSelect" style="" runat="server" class="it">
            <td class="name">
                <asp:HiddenField ID="hidId" runat="server" Visible="false" Value='<%# Eval("Id") %>' />
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Nazwisko") %>' /></td>
            <td><asp:Label ID="Label2" runat="server" Text='<%# Eval("Imie") %>' /></td>
            <td><asp:Label ID="Label3" runat="server" Text='<%# Eval("KadryId") %>' /></td>
            <td class="check"><asp:CheckBox ID="cbSelect" runat="server" OnCheckedChanged="CheckItem"  AutoPostBack="true" /></td>
            <%--<td id="tdControl" class="control" runat="server">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edytuj" />
            </td>--%>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table id="Table1" runat="server" class="table0">
            <tr class="edt">
                <td>
                    <asp:Label ID="lbNoData" runat="server" Text="Brak danych" /><br /><br />
                    <%--<asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord" Text="Dodaj" />--%>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table id="Table1" runat="server" class="ListView1 tbZastepstwa hoverline" style="width: 600px;">
            <tr id="Tr1" runat="server">
                <td id="Td1" runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" class="lvTasks">
                        <tr id="Tr2" runat="server" style="">
                            <th id="Th1" runat="server"><asp:LinkButton ID="LinkButton1" runat="server" Text="Nazwisko" CommandName="Sort" CommandArgument="Nazwisko" /></th>
                            <th id="Th2" runat="server"><asp:LinkButton ID="LinkButton2" runat="server" Text="Imie" CommandName="Sort" CommandArgument="Imie" /></th>
                            <th id="Th3" runat="server"><asp:LinkButton ID="LinkButton3" runat="server" Text="KadryId" CommandName="Sort" CommandArgument="KadryId" /></th>
                            <th id="Th4" runat="server"><asp:CheckBox ID="cbAll" runat="server" OnCheckedChanged="CheckAll" AutoPostBack="true" /></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trPager" runat="server" >
            </tr>
           <%-- <tr id="tr3" runat="server" >
                <td class="bottom_buttons">
                    <asp:Button ID="btNewRecord" CssClass="button margin0" runat="server" CommandName="NewRecord" Text="Dodaj zastępstwo" />
                </td>
            </tr>--%>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="dsEmployees" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="select pr.Id, pr.Imie, pr.Nazwisko, pr.KadryId from Przypisania p
left join Pracownicy pr on p.IdPracownika = pr.Id
where p.IdKierownika = @ObserverId and GETDATE() between p.Od and ISNULL(p.Do, '20990909')
">
    <SelectParameters>
        <asp:ControlParameter Name="ObserverId" Type="Int32" ControlID="hidObserverId" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>
    
    
    <div class="bottom_buttons">
        <asp:Button ID="btnCreate" runat="server" Text="Utwórz" OnClick="Create" CssClass="button100" />
        <asp:Button ID="btnClose" runat="server" Text="Zamknij" CssClass="button100" Visible="false" />
    </div>
    

<asp:SqlDataSource ID="dsCreateRequest" runat="server" SelectCommand="insert into scWnioski (IdTypuArkuszy, IdPracownika, Data, DataWyplaty, BilansOtwarcia, DataAkceptacji, IdAkceptujacego, IloscPracownikow, Status, Kacc, Pacc, DataUtworzenia) values (-1337, {0}, {1}, {2}, -1, null, null, null, 0, -1, -1, GETDATE())" />
<asp:SqlDataSource ID="dsCreateBonus" runat="server" SelectCommand="insert into scPremie (Id, {0},  )" />

