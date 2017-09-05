<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPrzeznaczNadg.ascx.cs" Inherits="HRRcp.Controls.cntPrzeznaczNadg" %>
<%@ Register src="~/Controls/TimeEdit.ascx" tagname="TimeEdit" tagprefix="uc1" %>

<asp:HiddenField ID="hidPlanId" runat="server" />
<asp:HiddenField ID="hidPracId" runat="server" />
<asp:HiddenField ID="hidData" runat="server" />
<asp:HiddenField ID="hidOkresOd" runat="server" />
<asp:HiddenField ID="hidOkresDo" runat="server" />
<asp:HiddenField ID="hidCzasZm" runat="server" />
<asp:HiddenField ID="hidNadgD" runat="server" />
<asp:HiddenField ID="hidNadgN" runat="server" />
<asp:HiddenField ID="hidNocne" runat="server" />

<asp:ListView ID="lvPodzial" runat="server" DataSourceID="SqlDataSource1" 
    DataKeyNames="Id" InsertItemPosition="LastItem" 
    onitemcreated="lvPodzial_ItemCreated" onitemdatabound="lvPodzial_ItemDataBound" 
    oniteminserting="lvPodzial_ItemInserting" onitemupdating="lvPodzial_ItemUpdating" 
    ondatabound="lvPodzial_DataBound" onlayoutcreated="lvPodzial_LayoutCreated" 
    onitemcanceling="lvPodzial_ItemCanceling" onitemediting="lvPodzial_ItemEditing" 
    OnItemCommand="lvPodzial_ItemCommand"
    onitemupdated="lvPodzial_ItemUpdated">
    <ItemTemplate>
        <tr class="it">
            <td class="col1">
                <asp:Label ID="RodzajLabel" runat="server" Text='<%# Eval("Rodzaj") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="ZaDzienLabel" runat="server" Text='<%# Eval("ZaDzien", "{0:d}") %>' />
            </td>
            <td id="tdCzasZm" runat="server" visible="false" class="col2">
                <asp:Label ID="CzasZmLabel" runat="server" Text='<%# Eval("CzasZmT") %>' />
            </td>
            <td class="col2">
                <asp:Label ID="NadgodzinyDzienLabel" runat="server" Text='<%# Eval("NadgodzinyDzienT") %>' />
            </td>
            <td class="col2" id="tdLastCol" runat="server">
                <asp:Label ID="NadgodzinyNocLabel" runat="server" Text='<%# Eval("NadgodzinyNocT") %>' />
            </td>
            <td id="tdNocne" runat="server" visible="false" class="col2">
                <asp:Label ID="NocneLabel" runat="server" Text='<%# Eval("NocneT") %>' />
            </td>
            <td id="tdControl" runat="server" class="control">
                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Delete" />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" class="tbEnterNadgedt">
            <tr>
                <td>
                    Brak klasyfikacji nadgodzin
                    <asp:Button ID="btNewRecord" runat="server" CommandName="NewRecord" Text="Zaklasyfikuj nadgodziny" Visible="false"/>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <InsertItemTemplate>
        <%--
        <tr class="iit">        
            <td class="col1" colspan="2" align="right">
                Pozostało:
            </td>
            <td>
                <asp:Label ID="lbRestD" runat="server" />
            </td>
            <td>
            </td>
        </tr>
        --%>
        <tr class="iit">        
            <td class="col1" colspan="2">
                <asp:DropDownList ID="ddlNadg" runat="server" DataSourceID="SqlDataSource4" DataTextField="Rodzaj" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlNadgInsert_SelectedIndexChanged" ></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic" 
                    ValidationGroup="vgi" 
                    ControlToValidate="ddlNadg"
                    CssClass="t4n error"
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvNadg" runat="server" Display="Dynamic"
                    ValidationGroup="vgi"
                    ControlToValidate="ddlNadg"
                    OnServerValidate="ddlNadg_ValidateInsert"
                    CssClass="t4n error"
                    ErrorMessage="Klasyfikacja już istnieje">
                </asp:CustomValidator>
            </td>
            <td id="tdCzasZm" runat="server" visible="false" class="col2">
                <uc1:TimeEdit ID="teCzasZm" runat="server" Seconds='<%# Bind("CzasZm") %>' />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNadgD" runat="server" Seconds='<%# Bind("NadgodzinyDzien") %>' />
            </td>
            <td class="col2" id="tdLastCol" runat="server">
                <uc1:TimeEdit ID="teNadgN" runat="server" Seconds='<%# Bind("NadgodzinyNoc") %>' />
            </td>
            <td id="tdNocne" runat="server" visible="false" class="col2">
                <uc1:TimeEdit ID="teNocne" runat="server" Seconds='<%# Bind("Nocne") %>' />
            </td>
            <%--
            <td class="col2">
                <asp:DropDownList ID="ddlZaDzien" runat="server" DataSourceID="SqlDataSource3" DataTextField="ZaDzien" DataValueField="Id" ></asp:DropDownList>
                <asp:RequiredFieldValidator ID="rqvZaDzien" runat="server" SetFocusOnError="True" Display="Dynamic" Enabled="false"
                    ValidationGroup="vgi" 
                    ControlToValidate="ddlZaDzien"
                    CssClass="t4n error"
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            --%>
            <td class="control">
                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Insert" ValidationGroup="vgi"/>
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" />
            </td>
        </tr>
    </InsertItemTemplate>
    <EditItemTemplate>
        <tr class="eit">
            <td class="col1" colspan="2">
                <asp:HiddenField ID="hidRodzajId" runat="server" Value='<%# Eval("RodzajId") %>' />
                <asp:HiddenField ID="hidZaDzien" runat="server" Value='<%# Eval("ZaDzien") %>' />
                <asp:DropDownList ID="ddlNadg" DataSourceID="SqlDataSource2" runat="server" DataTextField="Rodzaj" DataValueField="Id"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vge" 
                    ControlToValidate="ddlNadg"
                    CssClass="t4n error"
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvNadg" runat="server" Display="Dynamic"
                    ValidationGroup="vge"
                    ControlToValidate="ddlNadg"
                    OnServerValidate="ddlNadg_ValidateEdit"
                    CssClass="t4n error"
                    ErrorMessage="Klasyfikacja już istnieje">
                </asp:CustomValidator>
            </td>
            <td id="tdCzasZm" runat="server" visible="false" class="col2">
                <uc1:TimeEdit ID="teCzasZm" runat="server" Seconds='<%# Bind("CzasZm") %>' />
            </td>
            <td class="col2">
                <uc1:TimeEdit ID="teNadgD" runat="server" Seconds='<%# Bind("NadgodzinyDzien") %>' />
            </td>
            <td class="col2" id="tdLastCol" runat="server">
                <uc1:TimeEdit ID="teNadgN" runat="server" Seconds='<%# Bind("NadgodzinyNoc") %>'/>
            </td>
            <td id="tdNocne" runat="server" visible="false" class="col2">
                <uc1:TimeEdit ID="teNocne" runat="server" Seconds='<%# Bind("Nocne") %>'/>
            </td>
            <%--
            <td class="col2">
                <asp:DropDownList ID="ddlZaDzien" runat="server" DataSourceID="SqlDataSource3" DataTextField="ZaDzien" DataValueField="Id" ></asp:DropDownList>
                <asp:RequiredFieldValidator ID="rqvZaDzien" runat="server" SetFocusOnError="True" Display="Dynamic" Enabled="false"
                    ValidationGroup="vge" 
                    ControlToValidate="ddlZaDzien"
                    CssClass="t4n error"
                    ErrorMessage="Błąd" >
                </asp:RequiredFieldValidator>
            </td>
            --%>
            <td class="control">
                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vge" />
                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
            </td>
        </tr>
    </EditItemTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1 tbEnterNadg hoverline narrow">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th id="Th2" runat="server">
                                Klasyfikacja</th>
                            <th>
                                Za dzień
                            </th>
                            <th id="Th1" runat="server" visible="false">
                                Zmiana</th>
                            <th runat="server">
                                Nadg.<br />w dzień</th>
                            <th id="thLastCol" runat="server">
                                Nadg.<br />w nocy</th>
                            <th id="xthLastCol" runat="server" visible="false">
                                Praca w<br />nocy (<asp:Label ID="lbNocneOdDo" runat="server" Text="22-6"></asp:Label>)</th>
                            <th id="thControl" class="control" runat="server">&nbsp;</th>
                        </tr>
                        <tr runat="server" class="it total">
                            <td class="col1">Nadgodziny <span class="t4">(hh:mm)</span></td>
                            <td></td>
                            <td class="col2" runat="server" visible="false" ><asp:Label ID="lbCzasZm" runat="server" ></asp:Label></td>
                            <td class="col2"><asp:Label ID="lbNadgD" runat="server" ></asp:Label></td>
                            <td id="thLastCol1" class="col2" runat="server" ><asp:Label ID="lbNadgN" runat="server" ></asp:Label></td>
                            <td id="xthLastCol1" class="col2" runat="server" visible="false" ><asp:Label ID="lbNocne" runat="server" ></asp:Label></td>
                            <td id="thControl1" class="control" runat="server">&nbsp;</td>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"     
    SelectCommand="SELECT P.Id, P.RodzajId, K.Nazwa as Rodzaj, K.Parametr,
                        P.CzasZm, P.NadgodzinyDzien, P.NadgodzinyNoc, P.Nocne,
                        dbo.ToTimeHMM(P.CzasZm) as CzasZmT, 
                        dbo.ToTimeHMM(P.NadgodzinyDzien) as NadgodzinyDzienT, 
                        dbo.ToTimeHMM(P.NadgodzinyNoc) as NadgodzinyNocT, 
                        dbo.ToTimeHMM(P.Nocne) as NocneT, 
                        P.ZaDzien, P.Uwagi 
                    FROM PodzialNadgodzin P
                    left outer join Kody K on K.Typ = 'PODZNADG' and K.Kod = P.RodzajId
                    WHERE ([IdPracownika] = @IdPracownika and [Data] = @Data) ORDER BY P.Id"
    DeleteCommand="DELETE FROM [PodzialNadgodzin] WHERE [Id] = @Id" 
    InsertCommand="INSERT INTO [PodzialNadgodzin] ([IdPlanPracy], [Data], [IdPracownika], [RodzajId], [CzasZm], [NadgodzinyDzien], [NadgodzinyNoc], [Nocne], [Uwagi], ZaDzien) VALUES 
                                                  (@IdPlanPracy, @Data, @IdPracownika, @RodzajId, @CzasZm, @NadgodzinyDzien, @NadgodzinyNoc, @Nocne, @Uwagi, @ZaDzien)" 
    UpdateCommand="UPDATE [PodzialNadgodzin] SET [RodzajId] = @RodzajId, [CzasZm] = @CzasZm, [NadgodzinyDzien] = @NadgodzinyDzien, [NadgodzinyNoc] = @NadgodzinyNoc, [Nocne] = @Nocne, [Uwagi] = @Uwagi, ZaDzien = @ZaDzien WHERE [Id] = @Id">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="datetime" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="RodzajId" Type="Int32" />
        <asp:Parameter Name="CzasZm" Type="Int32" />
        <asp:Parameter Name="NadgodzinyDzien" Type="Int32" />
        <asp:Parameter Name="NadgodzinyNoc" Type="Int32" />
        <asp:Parameter Name="Nocne" Type="Int32" />
        <asp:Parameter Name="Uwagi" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="hidPlanId" Name="IdPlanPracy" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidData" Name="Data" PropertyName="Value" Type="datetime" />
        <asp:Parameter Name="RodzajId" Type="Int32" />
        <asp:Parameter Name="CzasZm" Type="Int32" />
        <asp:Parameter Name="NadgodzinyDzien" Type="Int32" />
        <asp:Parameter Name="NadgodzinyNoc" Type="Int32" />
        <asp:Parameter Name="Nocne" Type="Int32" />
        <asp:Parameter Name="Uwagi" Type="String" />
        <asp:Parameter Name="ZaDzien" Type="DateTime" />
    </InsertParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select null as Id, 'wybierz ...' as Rodzaj, -1 as Sort
union 
SELECT Kod, Nazwa, Lp FROM Kody 
WHERE Typ = 'PODZNADG' ORDER BY Sort">
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select null as Id, 'wybierz ...' as ZaDzien, null as Data
    union 
select 
    convert(varchar, PP.Id) + '|' + convert(varchar(10), PP.Data, 20) + '|' + convert(varchar, ISNULL(R.WymiarCzasu, 28800) - PP.CzasZm) as Id,
    convert(varchar(10), PP.Data, 20) + ' (' + dbo.ToTimeHMM(ISNULL(R.WymiarCzasu, 28800) - PP.CzasZm) + ')' as ZaDzien,
    PP.Data
    --PP.Data, R.WymiarCzasu - PP.CzasZm as Niedomiar, dbo.ToTimeHMM(R.WymiarCzasu - PP.CzasZm) as NiedomiarHMM,
    --PP.Czas, PP.CzasZm, dbo.ToTimeHMM(PP.Czas), dbo.ToTimeHMM(PP.CzasZm) ,*
from PlanPracy PP 
left outer join PracownicyParametry R on R.IdPracownika = PP.IdPracownika and PP.Data between R.Od and ISNULL(R.Do, '20990909')
left outer join Absencja A on A.IdPracownika = PP.IdPracownika and PP.Data between A.DataOd and A.DataDo
where 
PP.IdPracownika = @IdPracownika and
PP.Akceptacja = 1 and A.Id is null and CzasZm &gt;= 0 and ISNULL(NadgodzinyDzien, 0) = 0 and ISNULL(NadgodzinyNoc, 0) = 0 and R.WymiarCzasu - CzasZm &gt; 0
and PP.Data between @dOd and @dDo
and ISNULL(PP.IdZmianyKorekta, PP.IdZmiany) is not null
ORDER BY Data">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidOkresOd" Name="dOd" PropertyName="Value" Type="datetime" />
        <asp:ControlParameter ControlID="hidOkresDo" Name="dDo" PropertyName="Value" Type="datetime" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select null as Id, 'wybierz ...' as Rodzaj, -1 as Sort, null as Data
    union 
SELECT convert(varchar, Kod) as Id, Nazwa, Lp as Sort, null as Data FROM Kody 
WHERE Typ = 'PODZNADG' and Kod in (1,2) 
    union
select 
    convert(varchar, K.Kod) + '|' +
    convert(varchar(10), PP.Data, 20) + '|' + 
    convert(varchar, PP.Id) + '|' + 
    convert(varchar, ISNULL(R.WymiarCzasu, 28800) - PP.CzasZm - N.MinusNadg) + '|' +
    dbo.ToTimeHMM(ISNULL(R.WymiarCzasu, 28800) - PP.CzasZm - N.MinusNadg) as Id,
    K.Nazwa + ': ' + convert(varchar(10), PP.Data, 20) + ' (' + 
        case when N.MinusNadg = 0 then '' else 
            dbo.ToTimeHMM(ISNULL(R.WymiarCzasu, 28800) - PP.CzasZm - N.MinusNadg) + '/'
        end +  
        dbo.ToTimeHMM(ISNULL(R.WymiarCzasu, 28800) - PP.CzasZm) +         
        ')' as Rodzaj,
    999999 as Sort, PP.Data
    --,PP.Data, R.WymiarCzasu - PP.CzasZm as Niedomiar, dbo.ToTimeHMM(R.WymiarCzasu - PP.CzasZm) as NiedomiarHMM,
    --,PP.Czas, PP.CzasZm, dbo.ToTimeHMM(PP.Czas), dbo.ToTimeHMM(PP.CzasZm) ,*
	--,MinusNadg
from PlanPracy PP 
left outer join PracownicyParametry R on R.IdPracownika = PP.IdPracownika and PP.Data between R.Od and ISNULL(R.Do, '20990909')
left outer join Absencja A on A.IdPracownika = PP.IdPracownika and PP.Data between A.DataOd and A.DataDo
inner join Kody K on K.Typ = 'PODZNADG' and K.Kod = 3      --zeby mozna bylo wylaczyc
outer apply (select ISNULL(SUM(ISNULL(NadgodzinyDzien, 0) + ISNULL(NadgodzinyNoc, 0)), 0) as MinusNadg from PodzialNadgodzin where IdPracownika = PP.IdPracownika     --minus co juz odpracowane
	and ZaDzien = PP.Data
	and RodzajId = 3) as N
where 
PP.IdPracownika = @IdPracownika and
PP.Akceptacja = 1 and A.Id is null and PP.CzasZm &gt;= 0 and ISNULL(PP.NadgodzinyDzien, 0) = 0 and ISNULL(PP.NadgodzinyNoc, 0) = 0 and ISNuLL(R.WymiarCzasu, 28800) - PP.CzasZm &gt; 0
and PP.Data between @dOd and @dDo
and ISNULL(PP.IdZmianyKorekta, PP.IdZmiany) is not null
and ISNULL(R.WymiarCzasu, 28800) - PP.CzasZm - N.MinusNadg &gt; 0
ORDER BY Sort, Data">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidOkresOd" Name="dOd" PropertyName="Value" Type="datetime" />
        <asp:ControlParameter ControlID="hidOkresDo" Name="dDo" PropertyName="Value" Type="datetime" />
    </SelectParameters>
</asp:SqlDataSource>
