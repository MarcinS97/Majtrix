<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntRozlNadgSuma.ascx.cs" Inherits="HRRcp.Controls.RozliczenieNadg.cntRozlNadgSuma" %>

<asp:HiddenField ID="hidPracId" runat="server" Visible="false"/>
<asp:HiddenField ID="hidOkresOd" runat="server" Visible="false"/>
<asp:HiddenField ID="hidOkresDo" runat="server" Visible="false"/>

<asp:ListView ID="lvSumy" runat="server" DataSourceID="SqlDataSource1">
    <ItemTemplate>
        <tr class='<%# "it " + GetRowClass(Eval("Sort")) %>' >
            <td id="td0" class="tdR0" runat="server" >
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("Opis") %>' />
            </td>
            <td id="tdR1" class="num tdR1" runat="server" >
                <asp:Label ID="NiedomiarLabel" runat="server" Text='<%# Eval("Niedomiar") %>' ToolTip='<%# Eval("tNiedomiar") %>'/>
            </td>
            <td id="tdR2" class="num tdR2" runat="server" >
                <asp:Label ID="N50Label" runat="server" Text='<%# Eval("N50") %>' ToolTip='<%# Eval("tN50") %>'/>
            </td>
            <td id="tdR3" class="num tdR3" runat="server" >
                <asp:Label ID="N100Label" runat="server" Text='<%# Eval("N100") %>' ToolTip='<%# Eval("tN100") %>'/>
            </td>                                                                   

            <td id="td1" class="num tdR14" runat="server" >
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("drNiedomiar") %>' ToolTip='<%# Eval("tdrNiedomiar") %>'/>
            </td>
            <td id="td2" class="num tdR15" runat="server" >
                <asp:Label ID="Label2" runat="server" Text='<%# Eval("dr50") %>' ToolTip='<%# Eval("tdr50") %>'/>
            </td>
            <td id="td3" class="num tdR16" runat="server" >
                <asp:Label ID="Label3" runat="server" Text='<%# Eval("dr100") %>' ToolTip='<%# Eval("tdr100") %>'/>
            </td>                                                                   

            <td id="tdR4" class="num tdR4" runat="server" >          
                <asp:Label ID="P50Label" runat="server" Text='<%# Eval("P50") %>'   ToolTip='<%# Eval("tP50") %>' />
            </td>                                                                   
            <td id="tdR5" class="num tdR5" runat="server" >          
                <asp:Label ID="P100Label" runat="server" Text='<%# Eval("P100") %>' ToolTip='<%# Eval("tP100") %>'/>
            </td>                                                                   
            <td id="tdR6" class="num tdR6" runat="server" >          
                <asp:Label ID="W50Label" runat="server" Text='<%# Eval("W50") %>'   ToolTip='<%# Eval("tW50") %>' />
            </td>                                                                   
            <td id="tdR7" class="num tdR7" runat="server" >          
                <asp:Label ID="W100Label" runat="server" Text='<%# Eval("W100") %>' ToolTip='<%# Eval("tW100") %>'/>
            </td>                                                                   
            <td id="tdR8" class="num tdR8" runat="server" >          
                <asp:Label ID="O50Label" runat="server" Text='<%# Eval("O50") %>'   ToolTip='<%# Eval("tO50") %>' />
            </td>                                                                   
            <td id="tdR9" class="num tdR9 lastcol" runat="server" >          
                <asp:Label ID="O100Label" runat="server" Text='<%# Eval("O100") %>' ToolTip='<%# Eval("tO100") %>'/>
            </td>
        </tr>
    </ItemTemplate>
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
        <table runat="server" class="ListView1 table0 tbRozlNadgSumy">
            <tr runat="server">
                <td runat="server">
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr>
                            <th id="th0" rowspan="2" class="thR0" runat="server" >Miesiąc</th>                                                     
                            <th id="thR11" colspan="3" class="thR123" runat="server" >Czas pracy, nadgodziny [h]</th>                                                     
                            <th id="thR15" colspan="3" class="thR456" runat="server" >Pozostało do rozliczenia [h]</th>                                                     
                            <th id="thR12" colspan="2" class="thR45" runat="server" >Nadg. do wypłaty [h]</th>                                                     
                            <th id="thR13" colspan="2" class="thR67" runat="server" >Nadg. wybrane [h]</th>                                                     
                            <th id="thR14" colspan="2" class="thR89 lastcol" runat="server" >Odpracowane [h]</th>                                                     
                        </tr>
                        <tr id="trHeaderRozl2" runat="server" >
                            <th id="thR1" class="tdR1" runat="server" >Niedomiar</th>                                                     
                            <th id="thR2" class="tdR2" runat="server" >50%</th>                                                     
                            <th id="thR3" class="tdR3" runat="server" >100%</th>                                                     

                            <th id="thR21" class="tdR14" runat="server" >Niedomiar</th>                                                     
                            <th id="thR22" class="tdR15" runat="server" >50%</th>                                                     
                            <th id="thR23" class="tdR16" runat="server" >100%</th>                                                     

                            <th id="thR4" class="tdR4" runat="server" >50%</th>                                                     
                            <th id="thR5" class="tdR5" runat="server" >100%</th>                                                     
                            <th id="thR6" class="tdR6" runat="server" >50%</th>                                                     
                            <th id="thR7" class="tdR7" runat="server" >100%</th>                                                     
                            <th id="thR8" class="tdR8" runat="server" >50%</th>                                                     
                            <th id="thR9" class="tdR9 lastcol" runat="server" >100%</th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server">
                <td runat="server" style="">
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select
    'Suma:' as Opis,
    0 as Sort,
    
    ROUND(NS.Niedomiar, 4) as Niedomiar,
    ROUND(NS.N50,  4) as N50, 
    ROUND(NS.N100, 4) as N100,
    
    --NS.Niedomiar + NS.W50 + NS.W100 + NS.O50 + NS.O100 as drNiedomiar,
    ROUND(NS.Niedomiar + NS.WND + NS.OND,          4) as drNiedomiar,
    ROUND(NS.N50  - (NS.P50 + NS.W50 + NS.O50),    4) as dr50,
    ROUND(NS.N100 - (NS.P100 + NS.W100 + NS.O100), 4) as dr100,
    
    --dbo.ToTimeHMM(NS.sNiedomiar + NS.sW50 + NS.sW100 + NS.sO50 + NS.sO100) as tdrNiedomiar,
    dbo.ToTimeHMM(NS.sNiedomiar + NS.sWND + NS.sOND) as tdrNiedomiar,
    dbo.ToTimeHMM(NS.sN50 - (NS.sP50 + NS.sW50 + NS.sO50)) as tdr50,
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

--FROM  VRozliczenieNadgodzinOkresy NS 
FROM  VRozliczenieNadgodzinOkresy3 NS 

where NS.IdPracownika = @IdPracownika and NS.DataOd = @Od
/*ukrywam jeden*/ and
(
    (select COUNT(*) from VRozliczenieNadgodzinMies NS 
    where NS.IdPracownika = @IdPracownika and NS.DataOd between @Od and @Do) &gt; 1
) /**/
union all
select
    --convert(varchar, DATEPART(MONTH, DataOd)) 
    case DATEPART(MONTH, DataOd)
        when 1 then 'styczeń'
        when 2 then 'luty'
        when 3 then 'marzec'
        when 4 then 'kwiecień'
        when 5 then 'maj'
        when 6 then 'czerwiec'
        when 7 then 'lipiec'
        when 8 then 'sierpień'
        when 9 then 'wrzesień'
        when 10 then 'październik'
        when 11 then 'listopad'
        when 12 then 'grudzień'
    end
    as Opis,
    DATEPART(MONTH, DataOd) as Sort,
    
    ROUND(NS.Niedomiar, 4) as Niedomiar,
    ROUND(NS.N50,  4) as N50, 
    ROUND(NS.N100, 4) as N100,
    
    --NS.Niedomiar + NS.W50 + NS.W100 + NS.O50 + NS.O100 as drNiedomiar,
    ROUND(NS.Niedomiar + NS.WND + NS.OND,          4) as drNiedomiar,
    ROUND(NS.N50 - (NS.P50 + NS.W50 + NS.O50),     4) as dr50,
    ROUND(NS.N100 - (NS.P100 + NS.W100 + NS.O100), 4) as dr100,
    
    --dbo.ToTimeHMM(NS.sNiedomiar + NS.sW50 + NS.sW100 + NS.sO50 + NS.sO100) as tdrNiedomiar,
    dbo.ToTimeHMM(NS.sNiedomiar + NS.sWND + NS.sOND) as tdrNiedomiar,
    dbo.ToTimeHMM(NS.sN50 - (NS.sP50 + NS.sW50 + NS.sO50)) as tdr50,
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
FROM  VRozliczenieNadgodzinMies NS 
where NS.IdPracownika = @IdPracownika and NS.DataOd between @Od and @Do
order by Sort

    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="IdPracownika" PropertyName="Value" />
        <asp:ControlParameter ControlID="hidOkresOd" Name="Od" PropertyName="Value" />
        <asp:ControlParameter ControlID="hidOkresDo" Name="Do" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>

