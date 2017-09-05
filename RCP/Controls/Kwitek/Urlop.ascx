<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntUrlop.ascx.cs" Inherits="HRRcp.Controls.Kwitek.Urlop" %>
<%@ Register src="PracUrlop2.ascx" tagname="PracUrlop" tagprefix="uc1" %>
<%@ Register Src="~/Portal/Controls/Social/cntAvatar.ascx" TagPrefix="uc1" TagName="cntAvatar" %>


<div class="pUrlopRamka">
    <table class="kwitek_header table0">
        <tr>
            <td>
                <div class="ramka urlop">
                    <asp:DataList ID="dlHeader" runat="server" DataSourceID="SqlDataSource1" onitemdatabound="dlHeader_ItemDataBound">
                        <ItemTemplate>
                            <div>
                                <div id="paDaneOsobowe" runat="server" class="paDaneOsobowe" >

                                    <uc1:cntAvatar runat="server" ID="cntAvatar" Width="32px" Height="32px" NrEw='<%# Eval("LpLogo") %>' />
                                    <asp:Label ID="LpLogoLabel" runat="server" CssClass="nrew" Text='<%# Eval("LpLogo") %>' />&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="ImieLabel" runat="server" cssclass="nazwisko" Text='<%# Eval("Imie") %>' />
                                    <asp:Label ID="NazwiskoLabel" runat="server" cssclass="nazwisko" Text='<%# Eval("Nazwisko") %>' /><br />
                                    <div id="paPesel" runat="server" visible='<%# IsPeselVisible %>'>
                                        <span class="col1">Pesel:</span> <asp:Label ID="PeselLabel" CssClass="pesel" runat="server" Text='<%# Eval("Pesel") %>' /><br />
                                    </div>
                                </div>
                            
                                <div id="paWymiary" runat="server" class="paWymiary" visible='<%# OldWymiaryVisible %>'>
                                    <hr />
                                    <span class="col1a">Wymiar urlopu:</span>                    <asp:Label ID="Label1" CssClass="col2" runat="server" Text='<%# Eval("UrlopNom") %>' />&nbsp;&nbsp;&nbsp;(wymiar liczony proporcjonalnie do końcowej daty bieżącej umowy)<br />                
                                    <%--
                                    <span class="col1a">Wymiar urlopu w roku:</span>             <asp:Label ID="Label4" CssClass="col2" runat="server" Text='<%# Eval("UrlopNomRok") %>' />&nbsp;&nbsp;&nbsp;(wymiar urlopu przypadający w danym roku)<br />                
                                    --%>
                                    <span class="col1a">Zaległy:</span>                          <asp:Label ID="Label2" CssClass="col2" runat="server" Text='<%# Eval("UrlopZaleg") %>' /><br /> 
                                    <span class="col1a">Naniesiony w systemie:</span>            <asp:Label ID="Label3" CssClass="col2" runat="server" Text='<%# Eval("UrlopWyk") %>' /><br />
                                    <span class="col1a">Pozostały do wybrania:</span>            <asp:Label ID="lbDoWyk" CssClass="col2" runat="server" /><br />
                                    <span class="col1a">Wykorzystany na dzień dzisiejszy:</span> <asp:Label ID="Label5" CssClass="col2" runat="server" Text='<%# Eval("WykDoDn") %>' /><br />
                                    <span class="col1a">W tym na żądanie:</span>                 <asp:Label ID="Label6" CssClass="col2" runat="server" Text='<%# Eval("NaZadanie") %>' /><br />
                                    <span class="col1a">Pozostały na dzień:</span>               <asp:Label ID="lbDoWykNaDzien" CssClass="col2" runat="server" /><br />
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                    <asp:GridView ID="gvLimity" CssClass="GridView1" runat="server" DataSourceID="SqlDataSource2" Visible='<%# AssecoWymiaryVisible %>'>
                    </asp:GridView>
                    <div id="paDataZwiekszenia" runat="server" class="paDataZwiekszenia" visible="false">
                        <span class="label1">Data zwiększenia wymiaru urlopu wypoczynkowego:</span> <asp:Label ID="lbDataZwiekszenia" CssClass="value" runat="server" />
                    </div>
                </div>
            </td>
        </tr>
    </table>   

    <br />

    <div class="paUrlopyLista leftMrg12">
        <asp:Label ID="lbLista" CssClass="t1" runat="server" Text="Urlop wykorzystany:"></asp:Label>
        <uc1:PracUrlop ID="PracUrlop1" runat="server" />
    </div>
</div> 

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" >
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false" 
    SelectCommand="
select 
--  Data, LpLogo, A.Nazwisko, A.Imie
--, UmowaNumer, NaDzien
--, UrlopTyp
  YEAR(Data) [Rok]
, UrlopNazwa [Typ urlopu]

--, LimitDniZ [Zaległy;Dni]
, ROUND(cast(LimitDniZZgodzin as float),2) [Urlop przysługujący;Zaległy;Dni:N]
, ROUND(cast(LimitGodzinZ as float),2) [Urlop przysługujący;Zaległy;Godz.:N]

--, LimitDniB [Bieżące;D]
, ROUND(cast(LimitDniBZgodzin as float),2) [Urlop przysługujący;Bieżący;Dni:N]
, ROUND(cast(LimitGodzinB as float),2) [Urlop przysługujący;Bieżący;Godz.:N]

--, LimitDniRazem [Razem;D]
, ROUND(cast(LimitDniRazemZGodzin as float),2) [Urlop przysługujący;Razem;Dni:N;bold]
, ROUND(cast(LimitGodzinRazem as float),2) [Urlop przysługujący;Razem;Godz.:N;bold]


--, U1.UrlopNom + U1.UrlopZaleg - U1.UrlopWyk [UrlopZbior]
--, U1.UrlopNom, U1.UrlopZaleg, U1.UrlopWyk

--, ROUND(cast(WykOdPDRokuDni as float),2) [Wykorzystany od początku roku;Dni:N]
, ROUND(cast(WykOdPDRokuDniZGodz as float),2) [Wykorzystany od początku roku;Dni:N]
, ROUND(cast(WykOdPDRokuGodz as float),2) [Wykorzystany od początku roku;Godz.:N]

, ROUND(cast(PozostaleDni as float),2) [Aktualnie biegnący;Pozostały;Dni:N]
, ROUND(cast(PozostaleGodziny as float),2) [Aktualnie biegnący;Pozostały;Godz.:N]
, ROUND(cast(LimitDniRazemzPozostaleDni as float),2) [Aktualnie biegnący;Limit po zakończeniu;Dni:N]
, ROUND(cast(LimitGodzinRazemzPozostaleGodziny as float),2) [Aktualnie biegnący;Limit po zakończeniu;Godz.:N]

--, BilansDni [Bilans kon.roku;D]
, ROUND(cast(BilansDniZgodzin as float),2) [Bilans na koniec roku;Dni:N]
, ROUND(cast(BilansGodziny as float),2) [Bilans na koniec roku;Godz.:N]
/*
, JednostkaZliczania [Limit;Jednostka]
, ROUND(cast(LimitZintZ as float),2) [Limit;Zaległy:N]
, ROUND(cast(LimitZintB as float),2) [Limit;Bieżący:N]
, ROUND(cast(LimitZintRazem as float),2) [Limit;Razem:N]
, ROUND(cast(BilansZint as float),2) [Limit;Bilans:N]
*/
/*
, ROUND(cast(LimitDniNom as float),2) [Limit w roku;Dni:N]
, ROUND(cast(LimitGodzinNom as float),2) [Limit w roku;Godz.:N] 
*/
/*
, WykOdPDRokuDni, WykOdPDRokuGodz, WykOdPDRokuDniZGodz, LimitUrlopuNazadanie, LimitUrlopNaGodziny
, PozostaleZint, LimitZintRazemzPozostale, LimitDniZalZEtatu, LimitDniBiezZEtatu, LimitDniRazemZEtatu, BilansZEtatu
, EtatNumeric, NormaGodzinowa
, WykDniB, WykGodzinyB, WykDniZ, WykGodzinyZ
, LimitDoPierwszyDzienRokuDni, LimitDoPierwszyDzienRokuGodziny, UrlopyDoPierwszyDzienRokuDni, UrlopyDoPierwszyDzienRokuDniKal, UrlopyDoPierwszyDzienRokuGodziny
*/

from UrlopZbiorAsseco A

/*
left join Pracownicy P on P.KadryId = A.LpLogo
left join UrlopZbior U on U.Rok = 2016 and U.KadryId = A.LpLogo
outer apply (select
    round(U.UrlopNom / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNom, 
    round(U.UrlopZaleg / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopZaleg,
    round(U.UrlopWyk / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopWyk
) U1   
*/

where A.LpLogo = @KadryId

--order by LpLogo
order by Data, UrlopTyp
    ">
    <SelectParameters>
        <asp:Parameter Name="KadryId" Type="String" />
    </SelectParameters>            
</asp:SqlDataSource>        

<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false" 
    SelectCommand="

    /*
     * SQLDS dla CO - bez urlopu aktualnie biegnacego
     *
     */


select 
--  Data, LpLogo, A.Nazwisko, A.Imie
--, UmowaNumer, NaDzien
--, UrlopTyp
  YEAR(Data) [Rok]
, UrlopNazwa [Typ urlopu]

--, LimitDniZ [Zaległy;Dni]
, ROUND(cast(LimitDniZZgodzin as float),2) [Urlop przysługujący;Zaległy;Dni:N]
, ROUND(cast(LimitGodzinZ as float),2) [Urlop przysługujący;Zaległy;Godz.:N]

--, LimitDniB [Bieżące;D]
, ROUND(cast(LimitDniBZgodzin as float),2) [Urlop przysługujący;Bieżący;Dni:N]
, ROUND(cast(LimitGodzinB as float),2) [Urlop przysługujący;Bieżący;Godz.:N]

--, LimitDniRazem [Razem;D]
, ROUND(cast(LimitDniRazemZGodzin as float),2) [Urlop przysługujący;Razem;Dni:N;bold]
, ROUND(cast(LimitGodzinRazem as float),2) [Urlop przysługujący;Razem;Godz.:N;bold]


--, U1.UrlopNom + U1.UrlopZaleg - U1.UrlopWyk [UrlopZbior]
--, U1.UrlopNom, U1.UrlopZaleg, U1.UrlopWyk

--, ROUND(cast(WykOdPDRokuDni as float),2) [Wykorzystany od początku roku;Dni:N]
, ROUND(cast(WykOdPDRokuDniZGodz as float),2) [Wykorzystany od początku roku;Dni:N]
, ROUND(cast(WykOdPDRokuGodz as float),2) [Wykorzystany od początku roku;Godz.:N]
    /*
, ROUND(cast(PozostaleDni as float),2) [Aktualnie biegnący;Pozostały;Dni:N]
, ROUND(cast(PozostaleGodziny as float),2) [Aktualnie biegnący;Pozostały;Godz.:N]
, ROUND(cast(LimitDniRazemzPozostaleDni as float),2) [Aktualnie biegnący;Limit po zakończeniu;Dni:N]
, ROUND(cast(LimitGodzinRazemzPozostaleGodziny as float),2) [Aktualnie biegnący;Limit po zakończeniu;Godz.:N]
    */
--, BilansDni [Bilans kon.roku;D]
, ROUND(cast(BilansDniZgodzin as float),2) [Bilans na koniec roku;Dni:N]
, ROUND(cast(BilansGodziny as float),2) [Bilans na koniec roku;Godz.:N]
/*
, JednostkaZliczania [Limit;Jednostka]
, ROUND(cast(LimitZintZ as float),2) [Limit;Zaległy:N]
, ROUND(cast(LimitZintB as float),2) [Limit;Bieżący:N]
, ROUND(cast(LimitZintRazem as float),2) [Limit;Razem:N]
, ROUND(cast(BilansZint as float),2) [Limit;Bilans:N]
*/
/*
, ROUND(cast(LimitDniNom as float),2) [Limit w roku;Dni:N]
, ROUND(cast(LimitGodzinNom as float),2) [Limit w roku;Godz.:N] 
*/
/*
, WykOdPDRokuDni, WykOdPDRokuGodz, WykOdPDRokuDniZGodz, LimitUrlopuNazadanie, LimitUrlopNaGodziny
, PozostaleZint, LimitZintRazemzPozostale, LimitDniZalZEtatu, LimitDniBiezZEtatu, LimitDniRazemZEtatu, BilansZEtatu
, EtatNumeric, NormaGodzinowa
, WykDniB, WykGodzinyB, WykDniZ, WykGodzinyZ
, LimitDoPierwszyDzienRokuDni, LimitDoPierwszyDzienRokuGodziny, UrlopyDoPierwszyDzienRokuDni, UrlopyDoPierwszyDzienRokuDniKal, UrlopyDoPierwszyDzienRokuGodziny
*/

from UrlopZbiorAsseco A

/*
left join PRacownicy P on P.KadryId = A.LpLogo
left join UrlopZbior U on U.Rok = 2016 and U.KadryId = A.LpLogo
outer apply (select
    round(U.UrlopNom / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopNom, 
    round(U.UrlopZaleg / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopZaleg,
    round(U.UrlopWyk / ((cast(P.EtatL as float) / P.EtatM) * 8),2) as UrlopWyk
) U1   
*/

where A.LpLogo = @KadryId

--order by LpLogo
order by Data, UrlopTyp
    ">
    <SelectParameters>
        <asp:Parameter Name="KadryId" Type="String" />
    </SelectParameters>            
</asp:SqlDataSource>        

