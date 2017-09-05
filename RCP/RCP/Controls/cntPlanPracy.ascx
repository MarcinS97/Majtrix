<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPlanPracy.ascx.cs" Inherits="HRRcp.RCP.Controls.cntPlanPracy" %>
<%@ Register src="~/RCP/Controls/cntPlanPracyLineHeader.ascx" tagname="PlanPracyLineHeader" tagprefix="uc1" %>
<%@ Register src="~/RCP/Controls/cntPlanPracyInHeader.ascx" tagname="PlanPracyInHeader" tagprefix="uc1" %>
<%@ Register src="~/RCP/Controls/cntPlanPracyLine2.ascx" tagname="PlanPracyLine2" tagprefix="uc1" %>
<%@ Register src="~/Controls/PathControl.ascx" tagname="PathControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/LetterDataPager.ascx" tagname="LetterDataPager" tagprefix="uc1" %>

<asp:HiddenField ID="hidFrom" runat="server" Visible="false" />
<asp:HiddenField ID="hidTo" runat="server" Visible="false" />
<asp:HiddenField ID="hidOkresId" runat="server" Visible="false" />
<asp:HiddenField ID="hidKierId" runat="server" Visible="false" />
<asp:HiddenField ID="hidKId" runat="server" Visible="false" />
<asp:HiddenField ID="hidRootId" runat="server" Visible="false" />
<asp:HiddenField ID="hidZastId" runat="server" Visible="false" />
<asp:HiddenField ID="hidZakres" runat="server" Visible="false" />
<asp:HiddenField ID="hidKIdOkresy" runat="server" Visible="false" />
<asp:HiddenField ID="hidDzial" runat="server" Visible="false" />
<asp:HiddenField ID="hidStanowisko" runat="server" Visible="false" />
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
<asp:HiddenField ID="hidSearch" runat="server" Visible="false" />
<asp:HiddenField ID="hidUserId" runat="server" Visible="false" />
<asp:HiddenField ID="hidAll"    runat="server" Visible="false" />
<asp:HiddenField ID="hidUserZastId" runat="server" Visible="false" />

<asp:HiddenField ID="hidStatusFilter" runat="server" Visible="false" />
<asp:HiddenField ID="hidVersion" runat="server" Visible="false" />

<div class="cntPlanPracy"><%-- nie może być runat server --%>
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
                    <asp:HiddenField ID="hidIdNaglowka" runat="server" Value='<%# Eval("IdNaglowka") %>' />
                    <div class="action">
                        <div>
                            <asp:Button ID="btZeruj" CssClass="zeruj round2" runat="server" Text="0" CommandName="ZERUJ" CommandArgument='<%# Eval("Id") %>' ToolTip="Zerowanie minut nadgodzin"/>
                        </div>
                    </div>
                    <div class="pracname">
                        <asp:Label ID="PracownikLabel" runat="server" CssClass="nazwisko" Text='<%# Eval("NazwiskoImie") %>' />
                        <asp:LinkButton ID="lbtPracownik" runat="server" CssClass="nazwisko" Text='<%# Eval("NazwiskoImie") %>' CommandName="SubItems" CommandArgument='<%# Eval("Id") %>' />
                        <asp:Label ID="lbNrEw" runat="server" CssClass="line2 nrew" Text='<%# Eval("KadryId") %>' />
                    </div>
                </td>
                <td id="tdR1" class="num tdR1" runat="server" visible="false"><asp:Label ID="lbR1" runat="server" />
                </td><td id="tdR2" class="num tdR2" runat="server" visible="false"><asp:Label ID="lbR2" runat="server" />
                </td><td id="tdR3" class="num tdR3" runat="server" visible="false"><asp:Label ID="lbR3" runat="server" />
                </td><td id="tdR4" class="num tdR4" runat="server" visible="false"><asp:Label ID="lbR4" runat="server" />
                </td><td id="tdR5" class="num tdR5" runat="server" visible="false"><asp:Label ID="lbR5" runat="server" />
                </td><td id="tdR6" class="num tdR6" runat="server" visible="false"><asp:Label ID="lbR6" runat="server" />
                </td><td id="tdR7" class="num tdR7" runat="server" visible="false"><asp:Label ID="lbR7" runat="server" />
                </td><td id="tdR8" class="num tdR8" runat="server" visible="false"><asp:Label ID="lbR8" runat="server" />
                </td><td id="tdR9" class="num tdR9" runat="server" visible="false"><asp:Label ID="lbR9" runat="server" />
                </td><td id="tdR10" class="num tdR10" runat="server" visible="false"><asp:Label ID="lbR10" runat="server" />
                </td><td id="tdR11" class="num tdR11" runat="server" visible="false"><asp:Label ID="lbR11" runat="server" />
                </td><td id="tdR12" class="num tdR12" runat="server" visible="false"><asp:Label ID="lbR12" runat="server" />
                </td>
                <td id="tdColSelect" class="colselect" runat="server">
                    <div class="colselect">
                        <asp:CheckBox ID="cbPrac" runat="server" Visible='<%# Eval("AccStatusId").ToString() != "1" %>' />
                    </div>
                </td>
                <uc1:PlanPracyLine2 ID="PlanPracyLine" runat="server" />
                <td id="tdStatus" class="status" runat="server" visible="true" title='<%# Eval("StatusName") %>'>
                    <%--<asp:Label ID="lblStatus" runat="server" Text='<%# Eval("StatusName") %>' />--%>
                    <i class='<%# Eval("StatusClass") %>'></i>
                </td>
                <td id="tdRB" class="tdRB control" runat="server" visible="false">
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
                            <tr>
                                <th id="thPracownik" class="pracname" runat="server">
                                    <asp:ImageButton ID="ibtSubItemsBack" class="btupitems" runat="server" 
                                        ImageUrl="~/images/buttons/upitems.png" 
                                        onclick="ibtSubItemsBack_Click" />
                                    <span>
                                        Pracownik / Nr ew.
                                    </span>
                                </th>
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
                            <tr id="trHeaderRozl2" runat="server" visible="false">
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
                    </td>
                    <td class="pager right" >
                        <span class="count">Ilość:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                        &nbsp;&nbsp;&nbsp;
                        <span class="t1">Pokaż na stronie:</span> 
                        <asp:DropDownList ID="ddlLines" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLines_SelectedIndexChanged">
                            <asp:ListItem Text="10" Value="10" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                            <asp:ListItem Text="WSZYSTKO" Value="all"></asp:ListItem>
                        </asp:DropDownList>
                        <%--
                        <uc1:LetterDataPager ID="LetterDataPager1" runat="server" 
                            TbName="Pracownicy" 
                            Letter="LEFT(Nazwisko,1)"
                            Where="IdKierownika = @IdKierownika and Kierownik = 0"
                            Offset="select count(*) from Pracownicy where IdKierownika = @IdKierownika and Kierownik = 1"
                            ParName1="IdKierownika"
                            ParField1="hidKierId"
                            />
                        --%>
                    </td>
                </tr>
                <%--
                <tr id="trPager2" runat="server" class="pager" visible="false">
                    <td class="left" >
                    </td>
                    <td class="right">
                        <span class="count">Ilość:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                    </td>
                </tr>
                --%>
            </table>
        </LayoutTemplate>
    </asp:ListView>

    <asp:Button ID="btSelectCell" CssClass="button_postback" runat="server" Text="Select" onclick="btSelectCell_Click" />
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
SELECT distinct null IdNaglowka, P.Id, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, P.KadryId, 
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
        <asp:ControlParameter ControlID="hidDzial"      Name="dzial" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidStanowisko" Name="stan" PropertyName="Value" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<%--
distinct tu jest dlatego, że moze miec 2 karty rcp (powinien jeden numer RcpId !!!)
--%>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"  CancelSelectOnNullParameter="false" OnSelected="SqlDataSource2_Selected"
    SelectCommand="
--declare @IdKierownika int = 11
--declare @RootId int = -1
--declare @pracId int
--declare @toAcc int
--declare @Od datetime = '20160601'
--declare @Do datetime = dbo.eom(@Od)
--declare @s1 nvarchar(200)
--declare @s2 nvarchar(200)
--declare @Search nvarchar(200) = ''
--declare @all int = 0
--declare @UserId int = @Idkierownika

declare @ssearch nvarchar(250)
set @ssearch = LTRIM(RTRIM(@Search))
set @ssearch = NULLIF(@ssearch, '') + '%'

SELECT distinct ha.Id IdNaglowka, P.Id, P.Nazwisko + ' ' + P.Imie 

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
    , hac.Nazwa StatusName
    , hac.Class StatusClass
    , isnull(ha.Status, 0) AccStatusId
FROM Przypisania R 

outer apply (select top 1 * from PracownicyStanowiska where IdPracownika = R.IdPracownika and Od &lt;= @Do order by Od desc) ps

left join Pracownicy P on P.Id = R.IdPracownika
left join OkresyRozl O on @Do between O.DataOd and O.DataDo 
left join PracownicyOkresy PO on PO.IdOkresu = O.Id and PO.Id = R.IdPracownika
left join PracownicyParametry PP on PP.IdPracownika = R.IdPracownika and @Do between PP.Od and ISNULL(PP.Do, '20990909')

--left join PracownicyKarty PK on PK.IdPracownika = R.IdPracownika and @Do between PK.Od and ISNULL(PK.Do, '20990909')

left join Przypisania PR on PR.IdPracownika = R.IdPracownika and PR.Status = 1 and @Do between PR.Od and ISNULL(PR.Do, '20990909')
--left join DostepniKierownicy DK on DK.IdKierownika = @RootId and DK.IdKierDostepny = R.IdPracownika and DK.Od &lt;= @Do and ISNULL(DK.Do, '20990909') &gt;= @Od
--outer apply (select top 1 ha.*, _od from rcpHarmonogramAcc ha outer apply (select top 1 * from rcpHarmonogram h where h.IdNaglowka = ha.Id) oa where _od &lt;= @version and ha.IdPracownika = p.Id order by _od desc) ha
--outer apply (select top 1 ha.* from rcpHarmonogram h left join rcpHarmonogramAcc ha on ha.Id = h.IdNaglowka where h._od &lt;= @version and h.IdPracownika = p.Id order by _od desc) ha
outer apply ( select top 1 * from rcpHarmonogramAcc where IdPracownika = R.IdPracownika and Data = @Od order by DataUtworzenia desc) ha
left join rcpHarmonogramAccStatus hac on hac.Id = isnull(ha.Status, 0)
where (@toAcc is null or @toAcc = 0) 
and
(
	--(@ss is not null and @all = 1 or @all = 1 or (
	
	(@all = 1 and @IdKierownika = -1 or (		
		R.IdKierownika = @IdKierownika or 
		(@IdKierownika = @RootId and R.IdPracownika = @IdKierownika)
		--(@Zakres = 0 and R.IdKierownika = @IdKierownika) or
		--(@Zakres = 1 and (R.IdKierownika = @IdKierownika or (@IdKierownika = @RootId and R.IdPracownika = @IdKierownika))) or
		--(@Zakres = 2 and R.IdPracownika in (select IdKierDostepny from DostepniKierownicy where IdKierownika = @RootId and Od &lt;= @Do and ISNULL(Do, '20990909') &gt;= @Od))
		)
	)

	and (@ssearch is null or (P.Nazwisko like @ssearch or P.Imie like @ssearch or P.KadryId like @ssearch) 
						and (@all = 1 or P.Id in (select IdPracownika from dbo.fn_GetTreeOkres(@UserId, @Od, @Do, @Do))))	
    
    and (@dzial is null or ps.IdDzialu = @dzial)
    and (@stan is null or ps.IdStanowiska = @stan)

	and R.Status = 1 and ISNULL(R.Do, '20990909') &gt;= @Od and R.Od &lt;= @Do 
	and ISNULL(PO.Status, P.Status) &gt;= -1  
	and (@pracId is null or R.IdPracownika = @pracId)
)
or (@toAcc = 1 and P.Id in (select IdPracownika from rcpHarmonogramAcc where Status = 1 and Data = @Od))
ORDER BY Ja desc, Kierownik desc, NazwiskoImie
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidFrom"   Name="Od" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidTo"     Name="Do" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidKId"    Name="IdKierownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidRootId" Name="RootId" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidZastId" Name="ZastId" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidZakres" Name="Zakres" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidPracId" Name="PracId" PropertyName="Value" Type="Int32" />
        <asp:QueryStringParameter Name="toAcc" Type="Int32" QueryStringField="p" />
        <asp:ControlParameter ControlID="hidUserId" Name="UserId" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidSearch" Name="Search" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="hidAll"    Name="all" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidVersion" Name="version" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidDzial"      Name="dzial" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidStanowisko" Name="stan" PropertyName="Value" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<%--
        <asp:ControlParameter ControlID="hidUserIdZast" Name="UserIdZast" PropertyName="Value" Type="Int32" />


SELECT distinct null IdNaglowka, P.Id, P.Nazwisko + ' ' + P.Imie 

    --+ case when R.IdPracownika = @RootId or R.IdPracownika = @ZastId then ' JA' else '' end

    as NazwiskoImie, P.KadryId, 
    ISNULL(PO.Kierownik, P.Kierownik) as Kierownik, 
    PR.RcpStrefaId as StrefaId,      -- na koniec okresu
    PP.RcpAlgorytm as Algorytm, 
    ISNULL(PK.RcpId, -1) as RcpId, 
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
left join PracownicyKarty PK on PK.IdPracownika = R.IdPracownika and @Do between PK.Od and ISNULL(PK.Do, '20990909')
left join Przypisania PR on PR.IdPracownika = R.IdPracownika and PR.Status = 1 and @Do between PR.Od and ISNULL(PR.Do, '20990909')

--left join DostepniKierownicy DK on DK.IdKierownika = @RootId and DK.IdKierDostepny = R.IdPracownika and DK.Od &lt;= @Do and ISNULL(DK.Do, '20990909') &gt;= @Od

WHERE 
(@toAcc = 0 or @toAcc is null) 
and
(
    (
        R.IdKierownika = @IdKierownika or 
        (@IdKierownika = @RootId and R.IdPracownika = @IdKierownika)
        --(@Zakres = 0 and R.IdKierownika = @IdKierownika) or
        --(@Zakres = 1 and (R.IdKierownika = @IdKierownika or (@IdKierownika = @RootId and R.IdPracownika = @IdKierownika))) or
        --(@Zakres = 2 and R.IdPracownika in (select IdKierDostepny from DostepniKierownicy where IdKierownika = @RootId and Od &lt;= @Do and ISNULL(Do, '20990909') &gt;= @Od))
    )
    and R.Status = 1 and ISNULL(R.Do, '20990909') &gt;= @Od and R.Od &lt;= @Do 
    and ISNULL(PO.Status, P.Status) &gt;= -1  
    and (@pracId is null or R.IdPracownika = @pracId)
)
or (@toAcc = 1 and P.Id in (select IdPracownika from rcpHarmonogramAcc where Status = 1 and Data = @Od))
ORDER BY Ja desc, Kierownik desc, NazwiskoImie

    --%>

<asp:SqlDataSource ID="SqlDataSourceRozl" runat="server" CancelSelectOnNullParameter="false"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
SELECT distinct null IdNaglowka, P.Id, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, P.KadryId, 
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
FROM Przypisania R 
left join Pracownicy P on P.Id = R.IdPracownika
left join OkresyRozl O on @Do between O.DataOd and O.DataDo     -- koniec okresu
left join PracownicyOkresy PO on PO.IdOkresu = O.Id and PO.Id = R.IdPracownika
left join PracownicyParametry PP on PP.IdPracownika = R.IdPracownika and @Do between PP.Od and ISNULL(PP.Do, '20990909')

--left join PracownicyKarty PK on PK.IdPracownika = R.IdPracownika and @Do between PK.Od and ISNULL(PK.Do, '20990909')

left join Przypisania PR on PR.IdPracownika = R.IdPracownika and PR.Status = 1 and @Do between PR.Od and ISNULL(PR.Do, '20990909')

left join VRozliczenieNadgodzinOkresy NS on NS.IdPracownika = R.IdPracownika and NS.DataOd = @Od

WHERE R.IdKierownika = @IdKierownika and R.Status = 1 and ISNULL(R.Do, '20990909') &gt;= @Od and R.Od &lt;= @Do 
and ISNULL(PO.Status, P.Status) &gt;= -1  

--and PP.RcpAlgorytm != 0

ORDER BY Kierownik desc, NazwiskoImie
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidFrom" Name="Od" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidTo" Name="Do" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidKId" Name="IdKierownika" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="hidDzial"      Name="dzial" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidStanowisko" Name="stan" PropertyName="Value" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSourcePlanUrlopow" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
SELECT distinct null IdNaglowka, P.Id, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, P.KadryId, 
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

<asp:SqlDataSource ID="dsUpdate" runat="server" 
    SelectCommand="
declare @pracId int = {0}
declare @data datetime = '{1}'
declare @zmId int = {2}

declare @accId int = {3}
declare @nadzien datetime = {4}

select
  @pracId IdPracownika
, @data Data
, @zmId IdZmiany
, @nadzien DataZm
, h.Id OriginalId
into #ccc
from (select 1 x) x
left join rcpHarmonogram h on h.IdPracownika = @pracId and h.Data = @data
left join rcpHarmonogramAcc ha on ha.Id = h.IdNaglowka
where h._do is null and ((ISNULL(@zmId, -1) != ISNULL(h.IdZmiany, -1) and ha.Status is null) or ha.Status is not null)

update rcpHarmonogram set
  _do = @nadzien
from rcpHarmonogram h
inner join #ccc a on a.IdPracownika = h.IdPracownika and a.Data = h.Data

insert into rcpHarmonogram (Id, _od, IdPracownika, Data, IdZmiany, DataZm, IdKierownikaZm, Akceptacja, DataAcc, IdKierownikaAcc)
select OriginalId, @nadzien, a.IdPracownika, a.Data, a.IdZmiany, a.DataZm, @accId, 0, a.DataZm, @accId
from #ccc a

drop table #ccc
"
    UpdateCommand="
    update PlanPracy set /*IdZmiany*/IdZmianyPlan = {2}, DataZm = case when {4} = {5} then DataZm else GETDATE() end,  
                                                    IdKierownikaZm = case when {4} = {5} then IdKierownikaZm else {3} end, 
                                                    Akceptacja = case when {4} = {5} then Akceptacja else 0 end, 
                                                    DataAcc = case when {4} = {5} then DataAcc else GETDATE() end, 
                                                    IdKierownikaAcc = case when {4} = {5} then IdKierownikaAcc else {3} end
                                                where IdPracownika = {0} and Data = '{1}'
    " 
    InsertCommand="insert into PlanPracy (IdPracownika, Data, /*IdZmiany*/IdZmianyPlan, DataZm, IdKierownikaZm) values ({0},'{1}',{2},GETDATE(),{3})"
     />

<asp:SqlDataSource ID="dsSelect" runat="server" SelectCommand="
select D.Lp, D.Data, /*P.IdZmiany*/P.IdZmianyPlan IdZmiany, P.Id as ppId, Z.*, R.Od as PrzOd, R.Do as PrzDo, R.IdKierownika as PrzKierId, R.DoMonit, R.Id as PrzId
from GetDates2('{1}','{2}') D
/*left join
(
    select P.* from rcpHarmonogram P
    inner join
    (
    	select
    	  h0.Id
    	, MAX(h0._od) _od
    	from rcpHarmonogram h0
    	where h0.IdPracownika = {0} and h0._od &lt;= {3}
    	group by h0.Id
    ) h1 on h1.Id = P.Id and h1._od = P._od
) P on D.Data = P.Data and P.IdPracownika = {0} /*and P._do is null*/*/
/*left join PlanPracy P on D.Data = P.Data and P.IdPracownika = {0}*/
    left join PlanPracy P on D.Data = P.Data and P.IdPracownika = {0}
    left join Zmiany Z on Z.Id = P.IdZmianyPlan
left join Przypisania R on R.IdPracownika = {0} and R.Status = 1 and D.Data between R.Od and ISNULL(R.Do, '20990909') --20140513, niezależnie od przełożonego !!!
order by Data /* czemu tego nie bylo? albo to, albo niech to jest przypisywane jakos lepiej, a nie po kolei jak leci... */
    " />

<asp:SqlDataSource ID="dsSelectAcc" runat="server" SelectCommand="
select D.Lp, D.Data, P.IdZmianyPlan IdZmiany, P.Id as ppId, Z.*, R.Od as PrzOd, R.Do as PrzDo, R.IdKierownika as PrzKierId, R.DoMonit, R.Id as PrzId
from GetDates2('{1}','{2}') D
--left join rcpHarmonogram P on D.Data = P.Data and P.IdPracownika = {0} /*and P._do is null*/ and P.IdNaglowka = {3}
/*left join PlanPracy P on D.Data = P.Data and P.IdPracownika = {0}*/
left join PlanPracy P on D.Data = P.Data and P.IdPracownika = {0}
/*left join Zmiany Z on Z.Id = P.IdZmiany*/
left join Zmiany Z on Z.Id = P.IdZmianyPlan
left join Przypisania R on R.IdPracownika = {0} and R.Status = 1 and D.Data between R.Od and ISNULL(R.Do, '20990909') --20140513, niezależnie od przełożonego !!!
order by Data /* czemu tego nie bylo? albo to, albo niech to jest przypisywane jakos lepiej, a nie po kolei jak leci... */
    " />


<asp:SqlDataSource ID="dsAccList" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"  CancelSelectOnNullParameter="false"
    SelectCommand="
SELECT distinct ha.Id IdNaglowka, P.Id, P.Nazwisko + ' ' + P.Imie

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
    , '' StatusName
    , '' StatusClass
    , 0 AccStatusId
FROM rcpHarmonogramAcc ha
left join Przypisania R on R.IdPracownika = ha.IdPracownika and R.Status = 1 and ISNULL(R.Do, '20990909') &gt;= @Od and R.Od &lt;= @Do 
left join Pracownicy P on P.Id = R.IdPracownika
left join OkresyRozl O on @Do between O.DataOd and O.DataDo 
left join PracownicyOkresy PO on PO.IdOkresu = O.Id and PO.Id = R.IdPracownika
left join PracownicyParametry PP on PP.IdPracownika = R.IdPracownika and @Do between PP.Od and ISNULL(PP.Do, '20990909')

--left join PracownicyKarty PK on PK.IdPracownika = R.IdPracownika and @Do between PK.Od and ISNULL(PK.Do, '20990909')

left join Przypisania PR on PR.IdPracownika = R.IdPracownika and PR.Status = 1 and @Do between PR.Od and ISNULL(PR.Do, '20990909')

--left join DostepniKierownicy DK on DK.IdKierownika = @RootId and DK.IdKierDostepny = R.IdPracownika and DK.Od &lt;= @Do and ISNULL(DK.Do, '20990909') &gt;= @Od

where R.IdKierownika = @IdKierownika and (ha.Status = @StatusFilter)
/*WHERE 
(@toAcc = 0 or @toAcc is null) 
and
(
    (
        R.IdKierownika = @IdKierownika or 
        (@IdKierownika = @RootId and R.IdPracownika = @IdKierownika)
        --(@Zakres = 0 and R.IdKierownika = @IdKierownika) or
        --(@Zakres = 1 and (R.IdKierownika = @IdKierownika or (@IdKierownika = @RootId and R.IdPracownika = @IdKierownika))) or
        --(@Zakres = 2 and R.IdPracownika in (select IdKierDostepny from DostepniKierownicy where IdKierownika = @RootId and Od &lt;= @Do and ISNULL(Do, '20990909') &gt;= @Od))
    )
    and R.Status = 1 and ISNULL(R.Do, '20990909') &gt;= @Od and R.Od &lt;= @Do 
    and ISNULL(PO.Status, P.Status) &gt;= -1  
    and (@pracId is null or R.IdPracownika = @pracId)
)
or (@toAcc = 1 and P.Id in (select IdPracownika from rcpHarmonogramAcc where Status = 1 and Data = @Od))*/

ORDER BY Ja desc, Kierownik desc, NazwiskoImie
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidFrom" Name="Od" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidTo" Name="Do" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidKId" Name="IdKierownika" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidRootId" Name="RootId" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidZastId" Name="ZastId" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidZakres" Name="Zakres" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidPracId" Name="pracId" PropertyName="Value" Type="Int32" />
        <asp:QueryStringParameter Name="toAcc" Type="Int32" QueryStringField="p" />
        <asp:ControlParameter ControlID="hidUserId" Name="UserId" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidSearch" Name="Search" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidStatusFilter" Name="StatusFilter" PropertyName="Value" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsUpdateKorekta" runat="server" 
    SelectCommand=
"
declare @pracId int = {0}
declare @od datetime = {1}    
declare @do datetime = {2}
declare @userId int = {3}


insert into rcpHarmonogramAcc (IdPracownika, Data, Status, DataUtworzenia, IdTworzacego)
select top 1 oa.IdPracownika, @od, 4, GETDATE(), @userId 
from PlanPracy pp
outer apply ( select top 1 * from rcpHarmonogramAcc ha where ha.IdPracownika = pp.IdPracownika and Data = @od order by ha.DataUtworzenia desc) oa
where pp.IdPracownika = @pracId and pp.Data between @od and @do and isnull(IdZmiany, 0) != isnull(IdZmianyPlan, 0) and oa.Status in (2, -1)


    
/*declare @zmiana bit = 
(
    select case when 
    (
        select top 1 Id from PlanPracy pp
        inner join rcpHarmonogramAcc ha 
        where IdPracownika = @pracId and Data between @od and @do and IdZmiany != IdZmianyPlan
    ) is null then 0 else 1 end
)
*/

       
"

    />
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

