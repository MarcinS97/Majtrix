<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepNadgodziny2.ascx.cs" Inherits="HRRcp.Controls.RepNadgodziny2" %>
<%@ Register src="PathControl.ascx" tagname="PathControl" tagprefix="uc1" %>
<%@ Register src="CCHeader.ascx" tagname="CCHeader" tagprefix="uc2" %>

<asp:HiddenField ID="hidFrom" runat="server" />
<asp:HiddenField ID="hidTo" runat="server" />
<asp:HiddenField ID="hidKierId" runat="server" />
<asp:HiddenField ID="hidProjId" runat="server" />
<asp:HiddenField ID="hidOkresId" runat="server" />

zzz
<table id="tbInfo" runat="server" class="tbRepWarning" name="report" visible="false">
    <tr>
        <td>
            * Uwaga! Zestawienie wykonane na podstawie zaakceptowanych, ale niekompletnych danych
        </td>
    </tr>
</table>        
<uc1:PathControl ID="cntPath" OnSelectPath="OnSelectPath" runat="server" />
<asp:ListView ID="lvNadgodziny" runat="server" DataSourceID="SqlDatasource1" DataKeyNames="Id"
    ondatabound="lvPlanPracy_DataBound" 
    onitemdatabound="lvPlanPracy_ItemDataBound" 
    onunload="lvPlanPracy_Unload" 
    ondatabinding="lvPlanPracy_DataBinding" 
    onitemcreated="lvPlanPracy_ItemCreated" 
    onlayoutcreated="lvPlanPracy_LayoutCreated" 
    onitemcommand="lvPlanPracy_ItemCommand" 
    onprerender="lvPlanPracy_PreRender" >
    <ItemTemplate>
        <tr class="it">
            <td id="tdPracName" class="col1" runat="server">
                <asp:Label ID="PracownikLabel" runat="server" Text='<%# Eval("NazwiskoImie") %>' />
                <asp:LinkButton ID="lbtPracownik" runat="server" Text='<%# Eval("NazwiskoImie") %>' CommandName="SubItems" CommandArgument='<%# Eval("Id") %>' />
                <asp:Label ID="lbWarning" runat="server" CssClass="warning" Text="*" Visible="false"></asp:Label>
            </td>
            <td id="td2" class="col2" runat="server">
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("KadryId") %>' />
            </td>
            <td id="tdDzial" class="col3" runat="server">
                <asp:Label ID="Label2" runat="server" Text='<%# Eval("Dzial") %>' />
            </td>
            <%--
            <td id="td4" class="col4" runat="server">
                <asp:Label ID="Label3" runat="server" Text='<%# Eval("Stanowisko") %>' />
            </td>
            <td id="td12" class="col4" runat="server">
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("KierownikNI") %>' />
            </td>
            --%>
            <td id="td16" class="suma" runat="server">
                <asp:Label ID="lbNominalny" runat="server" />
            </td>
            <td id="td13" class="suma" runat="server">
                <asp:Label ID="lbZmianyPlan" runat="server" />
            </td>
            <td id="td14" class="suma" runat="server">
                <asp:Label ID="lbSumaryczny" runat="server" />
            </td>
            <td id="td15" class="suma" runat="server">
                <asp:Label ID="lbNieprzepracowany" runat="server" />
            </td>            
            <td id="td5" class="suma" runat="server">
                <asp:Label ID="lbNadg50" runat="server" />
            </td>
            <td id="td6" class="suma" runat="server">
                <asp:Label ID="lbNadg100" runat="server" />
            </td>
            <td id="td17" class="suma" runat="server">
                <asp:Label ID="lbNocny" runat="server" />
            </td>
            <td id="td3" class="suma" runat="server">
                <asp:Label ID="lbNiedzieleSwieta" runat="server" />
            </td>

            <td id="td7" class="suma" runat="server">
                <asp:Label ID="lbStawka" runat="server" />
            </td>
            <td id="td8" class="suma" runat="server">
                <asp:Label ID="lbStawkaGodz" runat="server" />
            </td>
            
            <td id="td9" class="suma" runat="server">
                <asp:Label ID="lbWynagr50" runat="server" />
            </td>
            <td id="td10" class="suma" runat="server">
                <asp:Label ID="lbWynagr100" runat="server" />
            </td>
            <td id="td11" class="suma" runat="server">
                <asp:Label ID="lbWynagrNadgSuma" runat="server" />
            </td>
            <td id="td12" class="suma" runat="server">
                <asp:Label ID="lbWynagrNoc" runat="server" />
            </td>
            
            <td id="tdCC0" class="suma" runat="server" visible="false">
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table id="Table1" runat="server" style="">
            <tr>
                <td>
                    Brak danych
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1 hoverline" id="lvOuterTable">
            <tr id="Tr1" runat="server">
                <td id="Td1" colspan="2" runat="server">
                    <table id="itemPlaceholderContainer" name="report" class="tbRepNadgodziny">  <%-- runat="server" --%>
                        <tr>
                            <th class="pracname">Pracownik</th>
                            <th>Nr ew.</th>
                            <th id="thDzial" runat="server">Dział</th>
                            <%--
                            <th>Stanowisko</th>                                                        
                            <th>Kierownik</th>                            
                            --%>
                            <th>Czas nominalny</th>                            
                            <th>Czas zaplan.</th>                            
                            <th>Łączny przeprac.</th>                            
                            <th>Czas nieprzepr.</th>                            
                            <th>Nadg.<br />50</th>                            
                            <th>Nadg.<br />100</th>                            
                            <th>Czas nocny</th>                            
                            <th>Niedziele i święta</th>                            
            
                            <th id="th7" runat="server">Wynagr. zasadn.</th>                            
                            <th id="th8" runat="server">Stawka godz.[zł]</th>                            
                            <th id="th9" runat="server">Wynagr.<br />50 [zł]</th>                            
                            <th id="th10" runat="server">Wynagr.<br />100 [zł]</th>                            
                            <th id="th11" runat="server">Suma nadgodzin</th>                            
                            <th id="th12" runat="server">Wynagr.<br />Noc [zł]</th>                            

                            <th id="thCC0" runat="server" visible="false">
                                <uc2:CCHeader ID="CCHeader1" runat="server" />
                            </th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                        <tr class="it sumy">
                            <td id="tdSumy" colspan="3" runat="server">Sumy:</td>
                            
                            <td id="td16" class="suma" runat="server">
                                <asp:Label ID="lbSumNominalny" runat="server" />
                            </td>
                            <td id="td13" class="suma" runat="server">
                                <asp:Label ID="lbSumZmianyPlan" runat="server" />
                            </td>
                            <td id="td14" class="suma" runat="server">
                                <asp:Label ID="lbSumSumaryczny" runat="server" />
                            </td>
                            <td id="td15" class="suma" runat="server">
                                <asp:Label ID="lbSumNieprzepracowany" runat="server" />
                            </td>
                            <td id="td5" class="suma" runat="server">
                                <asp:Label ID="lbSumNadg50" runat="server" />
                            </td>
                            <td id="td6" class="suma" runat="server">
                                <asp:Label ID="lbSumNadg100" runat="server" />
                            </td>
                            <td id="td18" class="suma" runat="server">
                                <asp:Label ID="lbSumNocny" runat="server" />
                            </td>
                            <td id="td3" class="suma" runat="server">
                                <asp:Label ID="lbSumNiedzieleSwieta" runat="server" />
                            </td>

                            <td id="td7" class="suma" runat="server">
                                <asp:Label ID="lbSumStawka" runat="server" Text='<%# Eval("Stawka", "{0:0.00}" ) %>' />
                            </td>
                            <td id="td8" class="suma" runat="server">
                                <asp:Label ID="lbSumStawkaGodz" runat="server" />
                            </td>
                            <td id="td9" class="suma" runat="server">
                                <asp:Label ID="lbSumWynagr50" runat="server" />
                            </td>
                            <td id="td10" class="suma" runat="server">
                                <asp:Label ID="lbSumWynagr100" runat="server" />
                            </td>
                            <td id="td11" class="suma" runat="server">
                                <asp:Label ID="lbSumWynagrNadgSuma" runat="server" />
                            </td>
                            <td id="td12" class="suma" runat="server">
                                <asp:Label ID="lbSumWynagrNoc" runat="server" />
                            </td>
                            
                            <td id="tdCCsuma0" runat="server" visible="false">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<table id="tbStawkaNocna" runat="server" name="report">
    <tr>
        <td>
            Nominalny czas pracy w miesiącu <asp:Label ID="lbNomMies" runat="server" ></asp:Label> [godz.]:
        </td>
        <td>
            <asp:Label ID="lbNomCzasMies" runat="server" ></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            Stawka dodatku za czas przepracowany w nocy [zł/godz.]:
        </td>
        <td>
            <asp:Label ID="lbStawkaNocne" runat="server" ></asp:Label>
        </td>
    </tr>    
</table>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" >
</asp:SqlDataSource>


<%--
    SelectCommand="SELECT P.Id, 
                        P.Nazwisko + ' ' + P.Imie as NazwiskoImie, 
                        P.KadryId,
                        P.Stawka,
                        P.RcpId, 
                        P.Kierownik
                        --,D.Nazwa as Dzial
                        --,S.Nazwa as Stanowisko
                        --,K.Nazwisko + ' ' + K.Imie as KierownikNI
                   FROM Pracownicy P 
                   --LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu
                   --LEFT OUTER JOIN Stanowiska S ON S.Id = P.IdStanowiska
                   --LEFT OUTER JOIN Pracownicy K ON K.Id = P.IdKierownika
                   ORDER BY NazwiskoImie" >
--%>