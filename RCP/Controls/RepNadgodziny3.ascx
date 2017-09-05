<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepNadgodziny3.ascx.cs" Inherits="HRRcp.Controls.RepNadgodziny3" %>
<%@ Register src="PathControl.ascx" tagname="PathControl" tagprefix="uc1" %>

<asp:HiddenField ID="hidFrom" runat="server" />
<asp:HiddenField ID="hidTo" runat="server" />
<asp:HiddenField ID="hidKierId" runat="server" />
<asp:HiddenField ID="hidProjId" runat="server" />
<asp:HiddenField ID="hidOkresId" runat="server" />

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
            <td id="_tdDzial" class="col3" runat="server">
                <asp:Label ID="Label2" runat="server" Text='<%# Eval("Dzial") %>' />
            </td>
            <%--
            <td id="td4" class="col4" runat="server">
                <asp:Label ID="Label3" runat="server" Text='<%# Eval("Stanowisko") %>' />
            </td>
            --%>
            <td id="tdClass" class="col4" runat="server" >
                <asp:Label ID="lbClass" runat="server" Text='<%# Eval("Class") %>' />
            </td>

            <td id="tdCC" class="colCC" runat="server" visible="false">
                <asp:Label ID="lbCC" runat="server" Text='<%# Eval("CC") %>' />
            </td>

            <td id="tdKier" class="col5" runat="server" visible="false">
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("KierownikNI") %>' Visible="false"/>
                <asp:LinkButton ID="lbtKierownik" runat="server" Text='<%# Eval("KierownikNI") %>' CommandName="SubItems" CommandArgument='<%# Eval("IdKierownika") %>' />
            </td>                   

            <td id="td16" class="suma lline" runat="server">
                <asp:Label ID="lbNominalny" runat="server" />
            </td>
            <td id="td13" class="suma" runat="server">
                <asp:Label ID="lbZmianyPlan" runat="server" />
            </td>
            <td id="td61" class="suma" runat="server">
                <asp:Label ID="lbSumaryczny" runat="server" />
            </td>
            <td id="td62" class="suma" runat="server" visible="false">
                <asp:Label ID="lbPrzepracowany" runat="server" />
            </td>
            <td id="td15" class="suma" runat="server">
                <asp:Label ID="lbNieprzepracowany" runat="server" />
            </td>            

            <td id="td51" class="suma" runat="server" visible="false">
                <asp:Label ID="lbUrlop" runat="server" />
            </td>
            <td id="td52" class="suma" runat="server" visible="false">
                <asp:Label ID="lbChorobowe" runat="server" />
            </td>
            <td id="td71" class="suma" runat="server" visible="false">
                <asp:Label ID="lbAbsencjeInne" runat="server" />
            </td>

            <td id="td5" class="suma lline" runat="server">
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

            <td id="td53" class="suma lline" runat="server" visible="false">
                <asp:Label ID="lbWolneZaNadg" runat="server" />
            </td>
            <td id="td59" class="suma" runat="server" visible="false">
                <asp:Label ID="lbOdpracowane" runat="server" />
            </td>

            <td id="td54" class="suma" runat="server" visible="false">
                <asp:Label ID="lbWyplata50" runat="server" />
            </td>
            <td id="td55" class="suma" runat="server" visible="false">
                <asp:Label ID="lbWyplata100" runat="server" />
            </td>

            <td id="td4" class="suma" runat="server" visible="false">
                <asp:Label ID="lbDodatek50" runat="server" />
            </td>
            <td id="td14" class="suma" runat="server" visible="false">
                <asp:Label ID="lbDodatek100" runat="server" />
            </td>



            <td id="td56" class="suma lline" runat="server" visible="false">
                <asp:Label ID="lbNierNiedomiar" runat="server" />
            </td>
            <td id="td57" class="suma" runat="server" visible="false">
                <asp:Label ID="lbNierNadg50" runat="server" />
            </td>
            <td id="td58" class="suma" runat="server" visible="false">
                <asp:Label ID="lbNierNadg100" runat="server" />
            </td>
            <%-- kwoty --%>
            <td id="td7" class="suma lline" runat="server">
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

            <td id="td41" class="suma gray" runat="server" visible="false">
                <asp:Label ID="lbRcpSumaryczny" runat="server" />
            </td>
            <td id="td42" class="suma gray" runat="server" visible="false">
                <asp:Label ID="lbRcpNadg50" runat="server" />
            </td>
            <td id="td43" class="suma gray" runat="server" visible="false">
                <asp:Label ID="lbRcpNadg100" runat="server" />
            </td>
            <td id="td44" class="suma gray" runat="server" visible="false">
                <asp:Label ID="lbRcpNocny" runat="server" />
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
        <table runat="server" class="ListView1 hoverline narrow" id="lvOuterTable">
            <tr id="Tr1" runat="server">
                <td id="Td1" colspan="2" runat="server">
                    <table id="itemPlaceholderContainer" name="report" class="tbRepNadgodziny">  <%-- runat="server" --%>
                        <tr>
                            <th class="pracname">Pracownik</th>
                            <th>Nr ew.</th>
                            <th id="thDzial" runat="server">Dział</th>
                            <%--
                            <th>Stanowisko</th>                                                        
                            --%>
                            <th id="thClass" runat="server" >Klasyfikacja</th>                            
                            <th id="thCC" runat="server" visible="false">
                                <asp:Label ID="lbThCC" Text="CC" runat="server" />
                            </th>                            
                            <th id="thKier" runat="server" visible="false">Kierownik</th>                            
                            
                            <th class="lline">Czas nominalny</th>                            
                            <th>Czas zaplan.</th>                            
                            <th id="th61" runat="server" >Łączny przeprac.</th>                            
                            <th id="th62" runat="server" visible="false">Czas przeprac.</th>                            
                            <th><asp:Literal ID="ltCzasNieprz" runat="server" Text="Czas nieprzepr."></asp:Literal><asp:Literal ID="ltCzasNieprzS" runat="server" Text="Godziny nieprzepr." Visible="false"></asp:Literal></th>                            
                            
                            <th id="th51" runat="server" visible="false">Urlopy</th>                            
                            <th id="th52" runat="server" visible="false">Chorob.</th>                            
                            <th id="th71" runat="server" visible="false">Absencje inne</th>                            
                            
                            <th class="lline">Nadg.<br />50</th>                            
                            <th>Nadg.<br />100</th>                            
                            <th>Czas nocny</th>                            
                            <th id="th3" runat="server">Niedziele i święta</th>                            

                            <th id="th53" runat="server" visible="false" class="lline">Wolne za<br />nadg.</th>                            
                            <th id="th59" runat="server" visible="false">Niedomiar<br />odprac.</th>                            
                            <th id="th54" runat="server" visible="false">Do wypłaty<br />nadg. 50</th>                            
                            <th id="th55" runat="server" visible="false">Do wypłaty<br />nadg. 100</th>                            
                            <th id="th4"  runat="server" visible="false">Dodatek<br />50 %</th>                            
                            <th id="th14" runat="server" visible="false">Dodatek<br />100 %</th>                            

                            <th id="th56" runat="server" visible="false" class="lline">Nierozlicz.<br />niedomiar</th>                            
                            <th id="th57" runat="server" visible="false">Nierozlicz.<br />nadg. 50</th>                            
                            <th id="th58" runat="server" visible="false">Nierozlicz.<br />nadg. 100</th>                            
            
                            <th id="th7" runat="server" class="lline">Wynagr. zasadn.</th>                            
                            <th id="th8" runat="server">Stawka godz.[zł]</th>                            
                            <th id="th9" runat="server">Wynagr.<br />50 [zł]</th>                            
                            <th id="th10" runat="server">Wynagr.<br />100 [zł]</th>                            
                            <th id="th11" runat="server">Suma nadgodzin</th>                            
                            <th id="th12" runat="server">Wynagr.<br />Noc [zł]</th>                            
                            
                            <th id="th41" runat="server" visible="false">Rcp<br />Czas&nbsp;łączny</th>                            
                            <th id="th42" runat="server" visible="false">Rcp<br />Nadg.50</th>                            
                            <th id="th43" runat="server" visible="false">Rcp<br />Nadg.100</th>                            
                            <th id="th44" runat="server" visible="false">Rcp<br />Czas&nbsp;nocny</th>                                                        
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>


                        <tr class="it sumy">
                            <th class="pracname"></th>
                            <th></th>
                            <th id="thhDzial" runat="server" visible="false"></th>
                            <%--
                            <th>Stanowisko</th>                                                        
                            --%>
                            <th id="thhClass" runat="server" ></th>                            
                            <th id="thhCC" runat="server" visible="false">
                                <asp:Label ID="Label3" Text="CC" runat="server" />
                            </th>                            
                            <th id="thhKier" runat="server" visible="false"></th>                            
                            
                            <th class="lline">Czas nominalny</th>                            
                            <th>Czas zaplan.</th>                            
                            <th id="thh61" runat="server" visible="false">Łączny przeprac.</th>                            
                            <th id="thh62" runat="server" >Czas przeprac.</th>                            
                            <th><asp:Literal ID="Literal1" runat="server" Visible="false" Text="Czas nieprzepr."></asp:Literal><asp:Literal ID="Literal2" runat="server" Text="Godziny nieprzepr." Visible="true"></asp:Literal></th>                            
                            
                            <th id="thh51" runat="server" visible="true">Urlopy</th>                            
                            <th id="thh52" runat="server" visible="true">Chorob.</th>                            
                            <th id="thh71" runat="server" visible="true">Absencje inne</th>                            
                            
                            <th class="lline">Nadg.<br />50</th>                            
                            <th>Nadg.<br />100</th>                            
                            <th>Czas nocny</th>                            
                            <th id="thh3" runat="server">Niedziele i święta</th>                            

                            <th id="thh53" runat="server" visible="true" class="lline">Wolne za<br />nadg.</th>                            
                            <th id="thh59" runat="server" visible="true">Niedomiar<br />odprac.</th>                            
                            <th id="thh54" runat="server" visible="true">Do wypłaty<br />nadg. 50</th>                            
                            <th id="thh55" runat="server" visible="true">Do wypłaty<br />nadg. 100</th>                            
                            <th id="thh4"  runat="server" visible="false">Dodatek<br />50 %</th>                            
                            <th id="thh14" runat="server" visible="false">Dodatek<br />100 %</th>                            

                            <th id="thh56" runat="server" visible="true" class="lline">Nierozlicz.<br />niedomiar</th>                            
                            <th id="thh57" runat="server" visible="true">Nierozlicz.<br />nadg. 50</th>                            
                            <th id="thh58" runat="server" visible="true">Nierozlicz.<br />nadg. 100</th>                            
            
                            <th id="thh7" runat="server" visible="false" class="lline">Wynagr. zasadn.</th>                            
                            <th id="thh8" runat="server" visible="false" >Stawka godz.[zł]</th>                            
                            <th id="thh9" runat="server" visible="false" >Wynagr.<br />50 [zł]</th>                            
                            <th id="thh10" runat="server" visible="false" >Wynagr.<br />100 [zł]</th>                            
                            <th id="thh11" runat="server" visible="false" >Suma nadgodzin</th>                            
                            <th id="thh12" runat="server" visible="false" >Wynagr.<br />Noc [zł]</th>                            
                            
                            <th id="thh41" runat="server" visible="false">Rcp<br />Czas&nbsp;łączny</th>                            
                            <th id="thh42" runat="server" visible="false">Rcp<br />Nadg.50</th>                            
                            <th id="thh43" runat="server" visible="false">Rcp<br />Nadg.100</th>                            
                            <th id="thh44" runat="server" visible="false">Rcp<br />Czas&nbsp;nocny</th>                                                        
                        </tr>



                        <tr class="it sumy">
                            <td id="tdSumy" colspan="4" runat="server">Sumy:</td>
                            
                            <td id="td16" class="suma lline" runat="server">
                                <asp:Label ID="lbSumNominalny" runat="server" />
                            </td>
                            <td id="td13" class="suma" runat="server">
                                <asp:Label ID="lbSumZmianyPlan" runat="server" />
                            </td>
                            <td id="td61" class="suma" runat="server">
                                <asp:Label ID="lbSumSumaryczny" runat="server" />
                            </td>
                            <td id="td62" class="suma" runat="server" visible="false">
                                <asp:Label ID="lbSPrzepracowany" runat="server" />
                            </td>
                            <td id="td15" class="suma" runat="server">
                                <asp:Label ID="lbSumNieprzepracowany" runat="server" />
                            </td>


                            <td id="td51" class="suma" runat="server" visible="false">
                                <asp:Label ID="lbSUrlop" runat="server" />
                            </td>
                            <td id="td52" class="suma" runat="server" visible="false">
                                <asp:Label ID="lbSChorobowe" runat="server" />
                            </td>
                            <td id="td71" class="suma" runat="server" visible="false">
                                <asp:Label ID="lbSAbsencjeInne" runat="server" />
                            </td>

                            <td id="td5" class="suma lline" runat="server">
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

                            <td id="td53" class="suma lline" runat="server" visible="false">
                                <asp:Label ID="lbSWolneZaNadg" runat="server" />
                            </td>
                            <td id="td59" class="suma" runat="server" visible="false">
                                <asp:Label ID="lbSOdpracowane" runat="server" />
                            </td>

                            <td id="td54" class="suma" runat="server" visible="false">
                                <asp:Label ID="lbSWyplata50" runat="server" />
                            </td>
                            <td id="td55" class="suma" runat="server" visible="false">
                                <asp:Label ID="lbSWyplata100" runat="server" />
                            </td>

                            <td id="td4" class="suma" runat="server" visible="false">
                                <asp:Label ID="lbSDodatek50" runat="server" />
                            </td>
                            <td id="td14" class="suma" runat="server" visible="false">
                                <asp:Label ID="lbSDodatek100" runat="server" />
                            </td>


                            <td id="td56" class="suma lline" runat="server" visible="false">
                                <asp:Label ID="lbSNierNiedomiar" runat="server" />
                            </td>
                            <td id="td57" class="suma" runat="server" visible="false">
                                <asp:Label ID="lbSNierNadg50" runat="server" />
                            </td>
                            <td id="td58" class="suma" runat="server" visible="false">
                                <asp:Label ID="lbSNierNadg100" runat="server" />
                            </td>

                            <td id="td7" class="suma lline" runat="server">
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

                            <td id="td41" class="suma gray" runat="server" visible="false">
                                <asp:Label ID="lbSumRcpSumaryczny" runat="server" />
                            </td>
                            <td id="td42" class="suma gray" runat="server" visible="false">
                                <asp:Label ID="lbSumRcpNadg50" runat="server" />
                            </td>
                            <td id="td43" class="suma gray" runat="server" visible="false">
                                <asp:Label ID="lbSumRcpNadg100" runat="server" />
                            </td>
                            <td id="td44" class="suma gray" runat="server" visible="false">
                                <asp:Label ID="lbSumRcpNocny" runat="server" />
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




<script runat="server">
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        //tabContent.MenuItemClick += new MenuEventHandler(tabContent_MenuItemClick2);
        lvNadgodziny.DataBound += new EventHandler(lvNadgodziny_DataBound2);
        lvNadgodziny.ItemDataBound += new EventHandler<ListViewItemEventArgs>(lvNadgodziny_ItemDataBound2);
    }

    protected void lvNadgodziny_DataBound2(object sender, EventArgs e)
    {
        bool all = KierId == "-100";
        Control c = lvNadgodziny.FindControl("thhKier");
        if (c != null) c.Visible = all;
        switch (Mode)
        {
            case nmoKierStawki:
                c = lvNadgodziny.FindControl("thhDzial");
                if (c != null) c.Visible = true;
                break;                                       
            //case nmoProjStawki:
            //case nmoKier:
            //    c = lvNadgodziny.FindControl("thhDzial");
            //    if (c != null) c.Visible = false;
            //    break;                
        }
    }

    protected void lvNadgodziny_ItemDataBound2(object sender, ListViewItemEventArgs e)
    {
    }
</script>
