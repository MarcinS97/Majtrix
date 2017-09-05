<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanPracy.ascx.cs" Inherits="HRRcp.Controls.PlanPracy" %>
<%@ Register src="PlanPracyLineHeader.ascx" tagname="PlanPracyLineHeader" tagprefix="uc1" %>
<%@ Register src="PlanPracyInHeader.ascx" tagname="PlanPracyInHeader" tagprefix="uc1" %>
<%@ Register src="PlanPracyLine2.ascx" tagname="PlanPracyLine2" tagprefix="uc1" %>
<%@ Register src="PathControl.ascx" tagname="PathControl" tagprefix="uc1" %>
<%@ Register src="LetterDataPager.ascx" tagname="LetterDataPager" tagprefix="uc1" %>

<asp:HiddenField ID="hidFrom" runat="server" Visible="false" />
<asp:HiddenField ID="hidTo" runat="server" Visible="false" />
<asp:HiddenField ID="hidOkresId" runat="server" Visible="false" />
<asp:HiddenField ID="hidKierId" runat="server" Visible="false" />
<asp:HiddenField ID="hidKId" runat="server" Visible="false" />
<asp:HiddenField ID="hidRootId" runat="server" Visible="false" />
<asp:HiddenField ID="hidZastId" runat="server" Visible="false" />
<asp:HiddenField ID="hidZakres" runat="server" Visible="false" />
<asp:HiddenField ID="hidKIdOkresy" runat="server" Visible="false" />
<%--
<asp:HiddenField ID="hidFrom" runat="server" />
<asp:HiddenField ID="hidTo" runat="server" />
<asp:HiddenField ID="hidOkresId" runat="server" />
<asp:HiddenField ID="hidKierId" runat="server" />
<asp:HiddenField ID="hidKId" runat="server" />
<asp:HiddenField ID="hidMoj" runat="server" />
<asp:HiddenField ID="hidKIdOkresy" runat="server" />
--%>
<asp:HiddenField ID="hidEditMode" runat="server" />
<asp:HiddenField ID="hidSelZmiana" runat="server" />

<asp:HiddenField ID="hidClickPracId" runat="server" />
<asp:HiddenField ID="hidClickDayIndex" runat="server" />


<asp:HiddenField ID="hidPracId" runat="server" Visible="false" />


<uc1:PathControl ID="cntPath" OnSelectPath="OnSelectPath" runat="server" />

<asp:Label ID="lbDebug" runat="server" Visible="false" />

<asp:ListView ID="lvPlanPracy" runat="server" DataKeyNames="Id" DataSourceID="SqlDataSource1" 
    ondatabound="lvPlanPracy_DataBound" 
    onitemdatabound="lvPlanPracy_ItemDataBound" 
    onunload="lvPlanPracy_Unload" 
    ondatabinding="lvPlanPracy_DataBinding" 
    onitemcreated="lvPlanPracy_ItemCreated" 
    onlayoutcreated="lvPlanPracy_LayoutCreated" 
    onitemcommand="lvPlanPracy_ItemCommand" 
    onprerender="lvPlanPracy_PreRender" 
    onload="lvPlanPracy_Load" oninit="lvPlanPracy_Init" >
    <ItemTemplate>
        <%--
        <tr class="it" id="trLine" runat="server">
        --%>     
        <tr id="trInHeader" runat="server" visible="false" class="header2">
            <th id="thPracownik" class="pracname" runat="server">
                <asp:ImageButton ID="ibtSubItemsBack" class="btupitems" runat="server" 
                    ImageUrl="~/images/buttons/upitems.png" 
                    onclick="ibtSubItemsBack_Click" />
                <span>
                    Pracownik / Nr ew.
                </span>
                <uc1:PlanPracyInHeader ID="cntInHeader" runat="server" />            
            </th>
        </tr>       
        <tr class='<%# GetLineClass(Eval("Ja")) %>' >
            <td id="tdPracName" class="pracname" runat="server">
                <asp:HiddenField ID="hidPracId" runat="server" Value='<%# Eval("Id") %>' />
                <asp:HiddenField ID="hidAlgorytm" runat="server" Value='<%# Eval("Algorytm") %>' />
                <div class="action">
                    <div>
                        <asp:Button ID="btZeruj" CssClass="zeruj round2" runat="server" Text="0" CommandName="ZERUJ" CommandArgument='<%# Eval("Id") %>' ToolTip="Zerowanie minut nadgodzin"/>
                    </div>
                </div>
                <div class="pracname">
                    <asp:Label ID="PracownikLabel" runat="server" CssClass="nazwisko" Text='<%# Eval("NazwiskoImie") %>' />
                    <asp:LinkButton ID="lbtPracownik" runat="server" CssClass="nazwisko" Text='<%# Eval("NazwiskoImie") %>' CommandName="SubItems" CommandArgument='<%# Eval("Id") %>' />
                    <asp:Label ID="lbNrEw" runat="server" CssClass="nrew" Text='<%# Eval("KadryId") %>' />
                </div>
            </td>
            <td id="tdR0" class="tdR0" runat="server" visible="false">
                <asp:Label ID="lbR0" runat="server" />
            </td><td id="tdR1" class="num tdR1" runat="server" visible="false">
                <asp:Label ID="lbR1" runat="server" />
            </td><td id="tdR2" class="num tdR2" runat="server" visible="false">
                <asp:Label ID="lbR2" runat="server" />
            </td><td id="tdR3" class="num tdR3" runat="server" visible="false">
                <asp:Label ID="lbR3" runat="server" />
            </td><td id="tdR4" class="num tdR4" runat="server" visible="false">
                <asp:Label ID="lbR4" runat="server" />
            </td><td id="tdR5" class="num tdR5" runat="server" visible="false">
                <asp:Label ID="lbR5" runat="server" />
            </td><td id="tdR6" class="num tdR6" runat="server" visible="false">
                <asp:Label ID="lbR6" runat="server" />
            </td><td id="tdR7" class="num tdR7" runat="server" visible="false">
                <asp:Label ID="lbR7" runat="server" />
            </td><td id="tdR8" class="num tdR8" runat="server" visible="false">
                <asp:Label ID="lbR8" runat="server" />
            </td><td id="tdR9" class="num tdR9" runat="server" visible="false">
                <asp:Label ID="lbR9" runat="server" />
            </td><td id="tdR10" class="num tdR10" runat="server" visible="false">
                <asp:Label ID="lbR10" runat="server" />
            </td><td id="tdR11" class="num tdR11" runat="server" visible="false">
                <asp:Label ID="lbR11" runat="server" />
            </td><td id="tdR12" class="num tdR12" runat="server" visible="false">
                <asp:Label ID="lbR12" runat="server" />
            </td>
            <td id="tdColSelect" class="colselect" runat="server">
                <div class="colselect">
                    <asp:CheckBox ID="cbPrac" runat="server" />
                </div>
            </td>
            <uc1:PlanPracyLine2 ID="PlanPracyLine" runat="server" />
            <td id="tdRB" class="tdRB control" runat="server" visible="false">
                <asp:LinkButton ID="lbtRozlEdit" runat="server" CommandName="RozlEdit" CommandArgument='<%# Eval("Id") %>' CssClass="btn" ToolTip="Szczegóły" Visible="false"><i class="glyphicon glyphicon-search"></i></asp:LinkButton>
                <asp:LinkButton ID="lbtRozlShow" runat="server" CommandName="RozlShow" CommandArgument='<%# Eval("Id") %>' CssClass="btn" ToolTip="Szczegóły" Visible="false"><i class="glyphicon glyphicon-search"></i></asp:LinkButton>
                <asp:Button ID="btRozlEdit" runat="server" CommandName="RozlEdit" CommandArgument='<%# Eval("Id") %>' Text="Szczegóły" Visible="false"/>
                <asp:Button ID="btRozlShow" runat="server" CommandName="RozlShow" CommandArgument='<%# Eval("Id") %>' Text="Szczegóły" Visible="false" />
                <asp:Button ID="btUrlopEdit" runat="server" CommandName="UrlopEdit" CommandArgument='<%# Eval("Id") %>' Text="Edycja" Visible="false" />
                <asp:Button ID="btUrlopShow" runat="server" CommandName="UrlopEdit" CommandArgument='<%# Eval("Id") %>' Text="Pokaż" ToolTip="Pokaż plan urlopów na cały rok" Visible="false" />
            </td></tr></ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" style="">
            <tr>
                <td>
                    Brak danych
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1" id="lvOuterTable">
            <tr runat="server">
                <td colspan="2" runat="server">
                    <table id="itemPlaceholderContainer" class="tbPlanPracy" >  <%-- runat="server" --%>
                        <tr class="line1">
                            <th id="thPracownik" class="pracname" runat="server">
                                <asp:ImageButton ID="ibtSubItemsBack" class="btupitems" runat="server" 
                                    ImageUrl="~/images/buttons/upitems.png" 
                                    onclick="ibtSubItemsBack_Click" />
                                <span>
                                    Pracownik / Nr ew.
                                </span>
                            </th>
                            <th id="thRT0" rowspan="2" class="thR0" runat="server" visible="false" >Okres rozliczeniowy</th>                                                     
                            <th id="thRT1" colspan="3" class="thR123" runat="server" visible="false" >Czas pracy, nadgodziny [h]</th>                                                     
                            <th id="thRT2" colspan="3" class="thR456" runat="server" visible="false" >Pozostało do rozliczenia [h]</th>                                                     
                            <th id="thRT3" colspan="2" class="thR45" runat="server" visible="false" >Nadg. do wypłaty [h]</th>                                                     
                            <th id="thRT4" colspan="2" class="thR67" runat="server" visible="false" >Nadg. wybrane [h]</th>                                                     
                            <th id="thRT5" colspan="2" class="thR89" runat="server" visible="false" >Odpracowane [h]</th>                                                     
                            <th id="thStanUrlopow" colspan="4" class="thU1234" runat="server" visible="false" >Stan urlopów</th>                                                     
                            <th id="thColSelect" class="colselect" runat="server">
                                <asp:CheckBox ID="cbDniAll" ToolTip="Ustaw dni" runat="server" OnClick="javascript:cbDniAllClickPP(this);" />
                                <asp:CheckBox ID="cbPracAll" ToolTip="Ustaw pracowników" runat="server" OnClick="javascript:cbPracAllClickPP(this);" />
                            </th>
                            <uc1:PlanPracyLineHeader ID="PlanPracyLineHeader" runat="server" />
                            <th id="thRB" rowspan="2" class="thRB control" runat="server" visible="false"></th>                            
                        </tr>
                        <tr id="trHeaderRozl2" runat="server" visible="false" class="line2">
                            <th id="thR1" class="tdR1" runat="server" >Niedomiar</th>                                                     
                            <th id="thR2" class="tdR2" runat="server" >50%</th>                                                     
                            <th id="thR3" class="tdR3" runat="server" >100%</th>                                                     

                            <th id="thR10" class="tdR10" runat="server" >Niedomiar</th>                                                     
                            <th id="thR11" class="tdR11" runat="server" >50%</th>                                                     
                            <th id="thR12" class="tdR12" runat="server" >100%</th>                                                     

                            <th id="thR4" class="tdR4" runat="server" >50%</th>                                                     
                            <th id="thR5" class="tdR5" runat="server" >100%</th>                                                     
                            <th id="thR6" class="tdR6" runat="server" >50%</th>                                                     
                            <th id="thR7" class="tdR7" runat="server" >100%</th>                                                     
                            <th id="thR8" class="tdR8" runat="server" >50%</th>                                                     
                            <th id="thR9" class="tdR9" runat="server" >100%</th>
                        </tr>
                        <tr id="trHeaderStanUrlopow2" runat="server" visible="false">
                            <th id="th1" class="tdU1" runat="server" title="Wymiar w bieżącym roku" >Wym</th>                                                     
                            <th id="th2" class="tdU2" runat="server" title="Zaległy z poprzedniego roku" >Zal</th>                                                     
                            <th id="th3" class="tdU3" runat="server" title="Zaplanowany w roku bieżącym" >Plan</th>                                                     
                            <th id="th4" class="tdU4" runat="server" title="Pozostały do rozplanowania" >Poz</th>                                                     
                            <%--
                            <th id="th1" class="tdU1" runat="server" >Obecny</th>                                                     
                            <th id="th2" class="tdU2" runat="server" >Zaległy</th>                                                     
                            <th id="th3" class="tdU3" runat="server" >Zaplan.</th>                                                     
                            <th id="th4" class="tdU4" runat="server" >Zostaje</th>                                                     
                            --%>    
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                        <tr id="trFooterRozl" runat="server" visible="false" class="rozlnadgfooter">
                            <%--<td colspan="4"></td>--%>
                            <td colspan="14" class="control1">
                                <asp:Button ID="btDoWyplatyAll50" runat="server" CommandName="dowyplatyall50" Text="Nadgodziny 50 -> do wypłaty" Visible="true"/>
                                <asp:Button ID="btDoWyplatyAll100" runat="server" CommandName="dowyplatyall100" Text="Nadgodziny 100 -> do wypłaty" Visible="true"/>
                                <asp:Button ID="btDoWyplatyAll" runat="server" CommandName="dowyplatyall" Text="Nadgodziny -> do wypłaty" Visible="false"/>                                
                            </td>
                            <%--<td colspan="7"></td>--%>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trPager" runat="server">
                <td id="Td2" runat="server" class="pager" style="">
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="10">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                        </Fields>
                    </asp:DataPager>
                    &nbsp;&nbsp;&nbsp;
                    <span class="t1">Pokaż na stronie:</span> 
                    <asp:DropDownList ID="ddlLines" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLines_SelectedIndexChanged">
                        <asp:ListItem Text="10" Value="10" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                        <asp:ListItem Text="WSZYSTKO" Value="all"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="pager" align="right">
                    <uc1:LetterDataPager ID="LetterDataPager1" runat="server" 
                        TbName="Pracownicy" 
                        Letter="LEFT(Nazwisko,1)"
                        Where="IdKierownika = @IdKierownika and Kierownik = 0"
                        Offset="select count(*) from Pracownicy where IdKierownika = @IdKierownika and Kierownik = 1"
                        ParName1="IdKierownika"
                        ParField1="hidKierId"
                        />
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:Button ID="btSelectCell" CssClass="button_postback" runat="server" Text="Select" onclick="btSelectCell_Click" />
<asp:Button ID="btDoWyplaty50" CssClass="button_postback" runat="server" OnClick="btDoWyplaty50_Click"/>
<asp:Button ID="btDoWyplaty100" CssClass="button_postback" runat="server" OnClick="btDoWyplaty100_Click"/>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
SELECT distinct P.Id, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, P.KadryId, 
    ISNULL(PO.Kierownik, P.Kierownik) as Kierownik, 
    PR.RcpStrefaId as StrefaId,      -- na koniec okresu
    PP.RcpAlgorytm as Algorytm, 

    --ISNULL(PK.RcpId, -1) as RcpId, 
    -1 as RcpId, 

    ISNULL(PO.Status, P.Status) as Status,
    P.DataZatr, P.DataZwol,
    0 as Ja 
FROM Przypisania R 
left outer join Pracownicy P on P.Id = R.IdPracownika
left outer join OkresyRozl O on @Do between O.DataOd and O.DataDo 
left outer join PracownicyOkresy PO on PO.IdOkresu = O.Id and PO.Id = R.IdPracownika
left outer join PracownicyParametry PP on PP.IdPracownika = R.IdPracownika and @Do between PP.Od and ISNULL(PP.Do, '20990909')

--left outer join PracownicyKarty PK on PK.IdPracownika = R.IdPracownika and @Do between PK.Od and ISNULL(PK.Do, '20990909')

left outer join Przypisania PR on PR.IdPracownika = R.IdPracownika and PR.Status = 1 and @Do between PR.Od and ISNULL(PR.Do, '20990909')
WHERE R.IdKierownika = @IdKierownika
and R.Status = 1 and ISNULL(R.Do, '20990909') &gt;= @Od and R.Od &lt;= @Do 
and ISNULL(PO.Status, P.Status) &gt;= -1  
ORDER BY Kierownik desc, NazwiskoImie
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidFrom" Name="Od" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidTo" Name="Do" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidKId" Name="IdKierownika" PropertyName="Value" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<%--
distinct tu jest dlatego, że moze miec 2 karty rcp (powinien jeden numer RcpId !!!)
--%>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"  CancelSelectOnNullParameter="false"
    SelectCommand="
SELECT distinct P.Id, P.Nazwisko + ' ' + P.Imie 

    --+ case when R.IdPracownika = @RootId or R.IdPracownika = @ZastId then ' JA' else '' end

    as NazwiskoImie, P.KadryId, 
    ISNULL(PO.Kierownik, P.Kierownik) as Kierownik, 
    PR.RcpStrefaId as StrefaId,      -- na koniec okresu
    PP.RcpAlgorytm as Algorytm, 

    --ISNULL(PK.RcpId, -1) as RcpId, 
    -1 as RcpId, 

    ISNULL(PO.Status, P.Status) as Status,
    P.DataZatr, P.DataZwol,
    case when R.IdPracownika = @RootId then 1 else 0 end as Ja
    --,case when R.IdPracownika = @RootId or R.IdPracownika = @ZastId then 1 else 0 end as Ja
    --,ISNULL(DK.Pracownicy, 0) as SubPrac,
    --,ISNULL(DK.Edycja, 0) as Edycja     
FROM Przypisania R 
left join Pracownicy P on P.Id = R.IdPracownika
left join OkresyRozl O on @Do between O.DataOd and O.DataDo 
left join PracownicyOkresy PO on PO.IdOkresu = O.Id and PO.Id = R.IdPracownika
left join PracownicyParametry PP on PP.IdPracownika = R.IdPracownika and @Do between PP.Od and ISNULL(PP.Do, '20990909')

--left join PracownicyKarty PK on PK.IdPracownika = R.IdPracownika and @Do between PK.Od and ISNULL(PK.Do, '20990909')

left join Przypisania PR on PR.IdPracownika = R.IdPracownika and PR.Status = 1 and @Do between PR.Od and ISNULL(PR.Do, '20990909')

--left join DostepniKierownicy DK on DK.IdKierownika = @RootId and DK.IdKierDostepny = R.IdPracownika and DK.Od &lt;= @Do and ISNULL(DK.Do, '20990909') &gt;= @Od

WHERE (
    R.IdKierownika = @IdKierownika or 
    (@IdKierownika = @RootId and R.IdPracownika = @IdKierownika)
    --(@Zakres = 0 and R.IdKierownika = @IdKierownika) or
    --(@Zakres = 1 and (R.IdKierownika = @IdKierownika or (@IdKierownika = @RootId and R.IdPracownika = @IdKierownika))) or
    --(@Zakres = 2 and R.IdPracownika in (select IdKierDostepny from DostepniKierownicy where IdKierownika = @RootId and Od &lt;= @Do and ISNULL(Do, '20990909') &gt;= @Od))
)
and R.Status = 1 and ISNULL(R.Do, '20990909') &gt;= @Od and R.Od &lt;= @Do 
and ISNULL(PO.Status, P.Status) &gt;= -1  
and (@pracId is null or R.IdPracownika = @pracId)
ORDER BY Ja desc, Kierownik desc, NazwiskoImie
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidFrom" Name="Od" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidTo" Name="Do" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidKId" Name="IdKierownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidRootId" Name="RootId" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidZastId" Name="ZastId" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidZakres" Name="Zakres" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidPracId" Name="PracId" PropertyName="Value" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourceRozl" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
SELECT distinct 
  P.Id PracId
, convert(varchar, P.Id) + '|' + convert(varchar(10), NS.DataOd, 20) Id   -- do arg

, P.Nazwisko + ' ' + P.Imie NazwiskoImie
, P.KadryId, 
    ISNULL(PO.Kierownik, P.Kierownik) as Kierownik, 
    PR.RcpStrefaId as StrefaId,      -- na koniec okresu
    PP.RcpAlgorytm as Algorytm, 

    --ISNULL(PK.RcpId, -1) as RcpId, 
    -1 as RcpId, 

    ISNULL(PO.Status, P.Status) as Status,
    P.DataZatr, P.DataZwol,
    0 as Ja,
    
    ROUND(NS.Niedomiar, 4) as Niedomiar,
    ROUND(NS.N50, 4) as N50, 
    ROUND(NS.N100, 4) as N100,
    
    --NS.Niedomiar + NS.W50 + NS.W100 + NS.O50 + NS.O100 as drNiedomiar,
    ROUND(NS.Niedomiar + NS.WND + NS.OND,          4) as drNiedomiar,
    ROUND(NS.N50  - (NS.P50 + NS.W50 + NS.O50),    4) as dr50,
    ROUND(NS.N100 - (NS.P100 + NS.W100 + NS.O100), 4) as dr100,
    
    --dbo.ToTimeHMM(NS.sNiedomiar + NS.sW50 + NS.sW100 + NS.sO50 + NS.sO100) as tdrNiedomiar,
    dbo.ToTimeHMM(NS.sNiedomiar + NS.sWND + NS.sOND) as tdrNiedomiar,
    dbo.ToTimeHMM(NS.sN50  - (NS.sP50 + NS.sW50 + NS.sO50)) as tdr50,
    dbo.ToTimeHMM(NS.sN100 - (NS.sP100 + NS.sW100 + NS.sO100)) as tdr100,
    
    ROUND(NS.P50,  4) as P50, 
    ROUND(NS.P100, 4) as P100,
    ROUND(NS.W50,  4) as W50, 
    ROUND(NS.W100, 4) as W100,
    ROUND(NS.O50,  4) as O50, 
    ROUND(NS.O100, 4) as O100,    
    
    NS.tNiedomiar,
    NS.tN50, NS.tN100,
    NS.tP50, NS.tP100,
    NS.tW50, NS.tW100,
    NS.tO50, NS.tO100    

, NS.DataOd OkresOd
, NS.DataDo OkresDo
, NS.NazwaOkresu
, NS.IloscMiesiecy

, --convert(varchar, NS.IloscMiesiecy) + ' mies., ' +
  convert(varchar(7), NS.DataOd, 20) 
+ case when NS.IloscMiesiecy != 1 then ' do ' 
+ substring(convert(varchar(7), NS.DataDo, 20), 6, 5) else '' end OkresRozl  -- 3 mies., 2017-05-01 do 08-31
, NS.NazwaOkresu OkresRozlHint

FROM Przypisania R 
left join Pracownicy P on P.Id = R.IdPracownika
left join OkresyRozl O on @Do between O.DataOd and O.DataDo     -- koniec okresu
left join PracownicyOkresy PO on PO.IdOkresu = O.Id and PO.Id = R.IdPracownika
left join PracownicyParametry PP on PP.IdPracownika = R.IdPracownika and @Do between PP.Od and ISNULL(PP.Do, '20990909')

--left join PracownicyKarty PK on PK.IdPracownika = R.IdPracownika and @Do between PK.Od and ISNULL(PK.Do, '20990909')

left join Przypisania PR on PR.IdPracownika = R.IdPracownika and PR.Status = 1 and @Do between PR.Od and ISNULL(PR.Do, '20990909')

--left join VRozliczenieNadgodzinOkresy NS on NS.IdPracownika = R.IdPracownika and NS.DataOd = @Od
left join VRozliczenieNadgodzinOkresy3 NS on NS.IdPracownika = R.IdPracownika and NS.DataOd between @Od and @Do

WHERE R.IdKierownika = @IdKierownika and R.Status = 1 and ISNULL(R.Do, '20990909') &gt;= @Od and R.Od &lt;= @Do 
and ISNULL(PO.Status, P.Status) &gt;= -1  

--and PP.RcpAlgorytm != 0

ORDER BY Kierownik desc, NazwiskoImie
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidFrom" Name="Od" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidTo" Name="Do" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidKId" Name="IdKierownika" PropertyName="Value" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourcePlanUrlopow" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
SELECT distinct P.Id, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, P.KadryId, 
    ISNULL(PO.Kierownik, P.Kierownik) as Kierownik, 
    99 as Algorytm,
    ISNULL(PO.Status, P.Status) as Status,
    P.DataZatr, P.DataZwol,

    case when R.IdPracownika = @RootId then 1 else 0 end as JaSort,
    case when R.IdPracownika = @RootId or R.IdPracownika = @ZastId then 1 else 0 end as Ja,

    U.UrlopNom as UrlopNomH,
    U.UrlopZaleg as UrlopZalegH,
    round(U.UrlopNom / ((cast(ISNULL(PO.EtatL, P.EtatL) as float) / ISNULL(PO.EtatM, P.EtatM)) * 8),2) as UrlopNom, 
    round(U.UrlopZaleg / ((cast(ISNULL(PO.EtatL, P.EtatL) as float) / ISNULL(PO.EtatM, P.EtatM)) * 8),2) as UrlopZaleg, 
    ISNULL(UL.Wymiar, 0) as UrlopDod,
    U.UrlopNomRok,  -- do końca roku
    
    (select count(*) from PlanUrlopow where IdPracownika = R.IdPracownika and Data between dbo.boy(@Od) and dbo.eoy(@Od) and Do is null and KodUrlopu is not null) as Zaplanowany,
    (select count(*) from PlanUrlopow where IdPracownika = R.IdPracownika and Data between @Od and @Do and Do is null and KodUrlopu is not null) as Mies,
    (select count(*) from PlanUrlopow where IdPracownika = R.IdPracownika and Data between dbo.boy(@Od) and @Do and Do is null and KodUrlopu is not null) as Narastajaco
      
FROM Przypisania R 
left join Pracownicy P on P.Id = R.IdPracownika
left join OkresyRozl O on @Do between O.DataOd and O.DataDo 
left join PracownicyOkresy PO on PO.IdOkresu = O.Id and PO.Id = R.IdPracownika
left join UrlopZbior U on U.KadryId = P.KadryId and U.Rok = DATEPART(YEAR, @Od)
--outer apply (select top 1 * from UrlopLimity where IdPracownika = P.Id and DataOd &lt;= dbo.eoy(@Do) and UrlopTyp = 'd' order by DataOd desc) UL
outer apply (select top 1 * from UrlopLimity where IdPracownika = P.Id and DataOd between dbo.boy(@Do) and dbo.eoy(@Do) and UrlopTyp = 'd' order by DataOd desc) UL
WHERE (
    @IdKierownika = -1 or 
    R.IdKierownika = @IdKierownika or 
    (@IdKierownika = @RootId and R.IdPracownika = @IdKierownika)
) 
and R.Status = 1 and ISNULL(R.Do, '20990909') &gt;= @Od and R.Od &lt;= @Do 
and ISNULL(PO.Status, P.Status) &gt;= -1  
ORDER BY JaSort desc, Kierownik desc, NazwiskoImie
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidFrom" Name="Od" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidTo" Name="Do" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidKId" Name="IdKierownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidRootId" Name="RootId" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidZastId" Name="ZastId" PropertyName="Value" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>




<%--
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT P.Id, P.RcpId, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, P.Kierownik, P.Status, P.KadryId,
                        ISNULL(RcpStrefaId, case when P.Kierownik=1 then D.KierStrefaId else D.PracStrefaId end) as StrefaId,
                        ISNULL(RcpAlgorytm, case when P.Kierownik=1 then D.KierAlgorytm else D.PracAlgorytm end) as Algorytm
                   FROM Pracownicy P 
                   LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu
                   WHERE P.IdKierownika = @IdKierownika and (P.Status >= 0 or P.Status = -2 and P.Kierownik = 1)
                  
                   union all
                   
                   SELECT O.Id, O.RcpId, O.Nazwisko + ' ' + O.Imie as NazwiskoImie, O.Kierownik, O.Status, O.KadryId,
                        O.RcpStrefaId as StrefaId,
                        O.RcpAlgorytm as Algorytm
                   FROM PracownicyOkresy O 
                   WHERE O.IdOkresu = @IdOkresu and O.IdKierownika = @IdKierownikaOkresy and (O.Status >= 0 or O.Status = -2 and O.Kierownik = 1)
                   
                   ORDER BY Kierownik desc, NazwiskoImie" 
    >
    <SelectParameters>
        <asp:ControlParameter ControlID="hidOkresId" Name="IdOkresu" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="hidKId" Name="IdKierownika" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="hidKIdOkresy" Name="IdKierownikaOkresy" PropertyName="Value" Type="String" />
    </SelectParameters>
    
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
SELECT distinct P.Id, P.RcpId, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, P.Kierownik, P.Status, P.KadryId,
    --ISNULL(P.RcpStrefaId, case when P.Kierownik=1 then D.KierStrefaId else D.PracStrefaId end) as StrefaId,
    
    
    --R.RcpStrefaId as StrefaId,
    --ISNULL(P.RcpAlgorytm, case when P.Kierownik=1 then D.KierAlgorytm else D.PracAlgorytm end) as Algorytm
    
    
FROM Przypisania R 
left outer join Pracownicy P on P.Id = R.IdPracownika
--LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu
WHERE R.IdKierownika = @IdKierownika and R.Status = 1 and ISNULL(R.Do, '20990909') &gt;= @Od and R.Od &lt;= @Do and
    (P.Status &gt;= 0 or P.Status = -2 and P.Kierownik = 1)
ORDER BY Kierownik desc, NazwiskoImie
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidFrom" Name="Od" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidTo" Name="Do" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidKId" Name="IdKierownika" PropertyName="Value" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>
--%>

