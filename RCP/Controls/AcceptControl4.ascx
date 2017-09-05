<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AcceptControl4.ascx.cs" Inherits="HRRcp.Controls.AcceptControl4" %>
<%@ Register Src="RcpControl.ascx" TagName="RcpControl" TagPrefix="uc1" %>
<%@ Register Src="RcpAnalizeControl.ascx" TagName="RcpAnalizeControl" TagPrefix="uc1" %>
<%@ Register Src="Title3.ascx" TagName="Title3" TagPrefix="uc1" %>
<%@ Register Src="TimeEdit.ascx" TagName="TimeEdit" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%--
<%@ Register src="MPK2.ascx" tagname="MPK" tagprefix="uc2" %>
--%>
<%@ Register Src="MPK4.ascx" TagName="MPK" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/RozliczenieNadg/cntPrzeznaczNadg.ascx" TagName="cntPrzeznaczNadg" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Reports/cntReport2.ascx" TagPrefix="uc1" TagName="cntReport2" %>

<asp:HiddenField ID="hidPPId" runat="server" Visible="false" />
<asp:HiddenField ID="hidPracId" runat="server" Visible="false" />
<asp:HiddenField ID="hidData" runat="server" Visible="false" />
<asp:HiddenField ID="hidAlerty" runat="server" Visible="false" />
<asp:HiddenField ID="hidFrom" runat="server" Visible="false" />
<asp:HiddenField ID="hidTo" runat="server" Visible="false" />

<div id="paAccept4" runat="server" class="cntAccept4">
    <div class="title">
        <table>                
            <tr>
                <td class="colleft">
                    <asp:Label ID="lbData" CssClass="data" runat="server" ></asp:Label>
                    <asp:Label ID="lbDataDzien" runat="server" ></asp:Label>
                </td>
                <td class="colmiddle">
                    <asp:Label ID="lbKadryId" runat="server" ></asp:Label>
                    <asp:Label ID="lbPracownik" CssClass="pracownik" runat="server" ></asp:Label>
                </td>
                <td class="colright">
                    <asp:Label ID="lbStatus" runat="server" ></asp:Label>
                </td>
            </tr>
            <%--        
            <tr class="title">
                <td></td>
                <td class="colmiddle">
                    <asp:Label ID="lbTitle" runat="server" CssClass="title" Text="Akceptacja czasu pracy"></asp:Label>
                </td>
                <td></td>
            </tr>
            --%>
        </table>
        <asp:Label ID="lbTitle" runat="server" CssClass="title" Text="Akceptacja czasu pracy"></asp:Label>
    </div>
    <div class="content">
        <asp:Menu ID="tabAccept" runat="server" Orientation="Horizontal"
            OnMenuItemClick="tabAccept_MenuItemClick">
            <StaticMenuStyle CssClass="tabsStrip" />
            <StaticMenuItemStyle CssClass="tabItem" />
            <StaticSelectedStyle CssClass="tabSelected" />
            <StaticHoverStyle CssClass="tabHover" />
            <Items>
                <asp:MenuItem Text="Dane pracownika"        Value="pgDanePracownika"></asp:MenuItem>
                <asp:MenuItem Text="Czas pracy"             Value="pgCzasPracy" Selected="True"></asp:MenuItem>
                <asp:MenuItem Text="Korekta"                Value="pgKorekta"></asp:MenuItem>
                <asp:MenuItem Text="Wnioski o nadgodziny"   Value="pgWnioskiNadg"></asp:MenuItem>
                <asp:MenuItem Text="Podział kosztów"        Value="pgPodzialKosztow"></asp:MenuItem>
                <asp:MenuItem Text="Rozliczenie nadgodzin"  Value="pgNadgodziny"></asp:MenuItem>
                <asp:MenuItem Text="Odbicia RCP"            Value="pgRCP"></asp:MenuItem>
            </Items>
            <StaticItemTemplate>
                <div class="tabCaption">
                    <div class="tabLeft">
                        <div class="tabRight">
                            <asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Text") %>' />
                        </div>
                    </div>
                </div>
            </StaticItemTemplate>
        </asp:Menu>
        <div class="content-inner">
            <asp:MultiView ID="mvAccept" runat="server" ActiveViewIndex="0" >
                <asp:View ID="pgCzasPracy" runat="server" OnActivate="pgCzasPracy_Activate">
                    <div class="pgCzasPracy">
                        <span class="label1">Zmiana:</span>

                        <asp:Label ID="lbZmiana0" CssClass="zmiana" runat="server" />

                        <asp:DropDownList ID="ddlZmiana0" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlZmiana_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlZmianaTime0" runat="server" CssClass="form-control" AutoPostBack="True" Visible="false" OnSelectedIndexChanged="ddlZmianaTime_SelectedIndexChanged">
                        </asp:DropDownList>

<%--                        <span class="label1">Wymiar:</span>

                        <uc1:TimeEdit ID="teZmiana0" runat="server" Right="true" InLineCount="4" />--%>

                        <uc1:RcpAnalizeControl ID="cntRcpAnalize" runat="server" Visible="true" />
                        <%--
                        <span>Nadgodziny '50: </span><asp:Label ID="lb500" runat="server" /><br />
                        <span>Nadgodziny '100: </span><asp:Label ID="lb1000" runat="server" />
                        --%>
                        <span class="t5">Alerty</span>
                        <table class="table0 tbAccept4">
                            <tr>
                                <td id="tdAlerty" runat="server">
                                    <div id="paAlertyScroll" class="vscrollbox">
                                        <asp:Literal ID="ltAlerty" runat="server"></asp:Literal>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>                    
                </asp:View>

                <asp:View ID="pgKorekta" runat="server" OnActivate="pgKorekta_Activate">
                    <table class="table0 tbAccept3">
                        <tr class="topline">
                            <td class="col1"></td>
                            <td class="col2">Dane RCP:</td>
                            <td class="col3">
                                <asp:Label ID="lbZmienNa" runat="server" Text="Zmień na ..."></asp:Label>
                            </td>
                        </tr>
                        <tr class="divider">
                            <td class="col1">Godzina wejścia:</td>
                            <td class="col2">
                                <asp:Label ID="lbTimeIn" runat="server"></asp:Label></td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teTimeIn" runat="server" Right="true" InLineCount="4" />
                                <asp:Label ID="lbTimeInVal" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col1">Godzina wyjścia:</td>
                            <td class="col2">
                                <asp:Label ID="lbTimeOut" runat="server"></asp:Label></td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teTimeOut" runat="server" Right="true" InLineCount="4" />
                                <asp:Label ID="lbTimeOutVal" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="divider" id="trWorktimeAll" runat="server">
                            <td class="col1">
                                <%--                            
                            Łączny czas pracy
                                --%>
                                <asp:Label ID="lbLacznieBezPrzerw" runat="server" Text="Łącznie bez przerw:"></asp:Label>
                                <asp:Label ID="lbLacznieZPrzerwami" runat="server" Text="Łącznie z przerwą:" Visible="false"></asp:Label>
                            </td>
                            <td class="col2">
                                <asp:Label ID="lbWorktimeAll" runat="server"></asp:Label></td>
                            <td class="col3 przerwy">
                                <asp:TextBox ID="tbWorktimeAll" Visible="false" runat="server"></asp:TextBox>
                                <asp:Label ID="lbWorktimeAllVal" Visible="false" runat="server"></asp:Label>
                                <asp:Label ID="lbPrzerwy" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trWStrefie" runat="server">
                            <td class="col1">Czas przepracowany:
                                <asp:Label ID="lbWStrefieBezPrzerw" runat="server" Text="W strefie bez przerw" Visible="false"></asp:Label>
                                <asp:Label ID="lbWStrefieZPrzerwami" runat="server" Text="W strefie z przerwami" Visible="false"></asp:Label>
                            </td>
                            <td class="col2">
                                <asp:Label ID="lbWStrefie" runat="server"></asp:Label></td>
                            <td class="col3"></td>
                        </tr>
                        <tr class="divider zmiana">
                            <td class="col1">Zmiana:</td>
                            <td class="col3" colspan="2">
                                <asp:Label ID="lbZmiana" runat="server" Visible="false"></asp:Label>
                                <asp:DropDownList ID="ddlZmiana" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlZmiana_SelectedIndexChanged">
                                </asp:DropDownList><br />
                                <asp:DropDownList ID="ddlZmianaTime" runat="server" CssClass="form-control" AutoPostBack="True" Visible="false" OnSelectedIndexChanged="ddlZmianaTime_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:Label ID="lbZmianaZgoda" runat="server" CssClass="zgoda" Visible="false" Text="Zgoda na nadgodziny"></asp:Label>
                                <asp:Label ID="lbZmianaBrak" runat="server" CssClass="brakzgody" Visible="false" Text="Brak zgody na nadgodziny"></asp:Label>
                            </td>
                        </tr>

                        <%--
                        <tr class="divider zmiana">
                            <td class="col1">Wymiar:</td>
                            <td class="col2">
                                <asp:Label ID="lbWymiar" runat="server"></asp:Label>
                            </td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teWymiar" runat="server" Right="true" InLineCount="4" />
                                <asp:Label ID="lbWymiarVal" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        --%>

                        <tr>
                            <td class="col1">Czas na zmianie:</td>
                            <td class="col2">
                                <asp:Label ID="lbWorktime" runat="server"></asp:Label></td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teWorktime" runat="server" Right="true" />
                                <asp:Label ID="lbWorktimeVal" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col1">Nadgodziny w dzień:</td>
                            <td class="col2">
                                <asp:Label ID="lbNadgDzien" runat="server"></asp:Label></td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teNadgDzien" runat="server" Right="true" />
                                <asp:Label ID="lbNadgDzienVal" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col1">Nadgodziny w nocy:</td>
                            <td class="col2">
                                <asp:Label ID="lbNadgNoc" runat="server"></asp:Label></td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teNadgNoc" runat="server" Right="true" />
                                <asp:Label ID="lbNadgNocVal" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
    <%--                        <tr>
                            <td class="col1">Nadgodziny 50</td>
                            <td class="col2">
                                <asp:Label ID="lbNadg50" runat="server"></asp:Label></td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teNadg50" runat="server" Right="true" />
                                <asp:Label ID="lbNadg50Val" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col1">Nadgodziny 100</td>
                            <td class="col2">
                                <asp:Label ID="lbNadg100" runat="server"></asp:Label></td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teNadg100" runat="server" Right="true" />
                                <asp:Label ID="lbNadg100Val" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="col1">W nocy (<asp:Label ID="lbNocneOdDo" runat="server" Text="22-6" />)</td>
                            <td class="col2">
                                <asp:Label ID="lbNocne" runat="server"></asp:Label></td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teNocne" runat="server" Right="true" />
                                <asp:Label ID="lbNocneVal" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="divider absencja">
                            <td class="col1">Powód absencji:</td>
                            <td class="col3" colspan="2">
                                <asp:Label ID="lbAbsencja" runat="server"></asp:Label>
                                <asp:DropDownList ID="ddlAbsencja" CssClass="form-control" runat="server" />
                            </td>
                        </tr>
                        <tr class="uwagi3">
                            <td class="col1">Uwagi:</td>
                            <td class="col3" colspan="2">
                                <asp:TextBox ID="tbUwagi" CssClass="textbox form-control" runat="server" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                <asp:Label ID="lbUwagi" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:View>

                <asp:View ID="pgPodzialKosztow" runat="server" OnActivate="pgPodzialKosztow_Activate">
                    <uc2:MPK ID="cntMPK" runat="server" />
                </asp:View>

                <asp:View ID="pgNadgodziny" runat="server" OnActivate="pgNadgodziny_Activate">
                    <uc3:cntPrzeznaczNadg ID="cntPrzeznaczNadg" runat="server" />
                </asp:View>

                <asp:View ID="pgWnioskiNadg" runat="server" OnActivate="pgWnioskiNadg_Activate">
<%--                    <span class="t5">Wnioski o nadgodziny</span>--%>
                    <uc1:cntReport2 runat="server" ID="cntReport2" GridVisible="false" CssClass="report_page noborder wnioskinadg" SQL="
    select
    p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') Wnioskujący
    /*, Nadg [Ilość (h)]*/
    , z.Symbol + ' - ' + z.Nazwa [Zmiana]
    , dbo.ToTimeHMM(Nadg50) ['50]
    , dbo.ToTimeHMM(Nadg100) ['100]
    , dbo.ToTimeHMM(Noc) [Nocne]

    , case nw.RodzajId 
        when 1 then 'Do wypłaty' 
        when 2 then 'Do wybrania' + ISNULL(' w dniu: ' + convert(varchar(10), nw.Data2, 20), '') 
        else convert(varchar, nw.RodzajId) 
      end [Rodzaj]
    --, case nw.RodzajId when 1 then null else nw.Data2 end [W dniu:D]

    , nws.Nazwa [Status]
    from rcpNadgodzinyWnioski nw
    inner join Pracownicy p on p.Id = nw.AutorId
    inner join rcpNadgodzinyWnioskiStatus nws on nws.Id = nw.Status
    left join Zmiany z on z.Id = nw.IdZmiany
    where nw.IdPracownika = '@SQL1' and nw.Data = '@SQL2'
    " />
                    <br />
                    <asp:Button ID="btWniosekNadg" CssClass="button" runat="server" Visible="true" Text="Wniosek o nadgodziny" OnClick="btWniosekNadg_Click" />
                </asp:View>

                <asp:View ID="pgDanePracownika" runat="server" OnActivate="pgDanePracownika_Activate">
                    <table class="table0 tbAccept2">
                        <tr id="trKier" runat="server" class="divider" visible="false">
                            <td class="col1">Przełożony:</td>
                            <td class="col2">
                                <asp:Label ID="lbKier" runat="server"></asp:Label></td>
                        </tr>

                        <tr id="trDzial" class="divider" runat="server">
                            <td class="col1">Dział:</td>
                            <td class="col2">
                                <asp:Label ID="lbDzial" runat="server"></asp:Label></td>
                        </tr>
                        <tr id="trStanowisko" runat="server">
                            <td class="col1">Stanowisko:</td>
                            <td class="col2">
                                <asp:Label ID="lbStanowisko" runat="server"></asp:Label></td>
                        </tr>

                        <tr id="trCC" runat="server" class="divider" visible="false">
                            <td class="col1">CC - Projekt:</td>
                            <td class="col2">
                                <asp:Label ID="lbCC" runat="server"></asp:Label></td>
                        </tr>

                        <tr id="trGrupa" runat="server" class="divider" visible="false">
                            <td class="col1"><asp:Literal ID="ltGrupa" runat="server" Text="Rodzaj:"></asp:Literal></td>
                            <td class="col2">
                                <asp:Label ID="lbGrupa" runat="server"></asp:Label></td>
                        </tr>
                        <tr id="trClass" runat="server" visible="false">
                            <td class="col1"><asp:Literal ID="ltClass" runat="server" Text="Klasyfikacja:"></asp:Literal></td>
                            <td class="col2">
                                <asp:Label ID="lbClass" runat="server"></asp:Label></td>
                        </tr>
                        <tr id="trGrade" runat="server" visible="false">
                            <td class="col1"><asp:Literal ID="ltGrade" runat="server" Text="Grade:"></asp:Literal></td>
                            <td class="col2">
                                <asp:Label ID="lbGrade" runat="server"></asp:Label></td>
                        </tr>
                        
                        <tr id="trComm" runat="server" class="divider" visible="false">
                            <td class="col1 narrow">Commodity Area Position:</td>
                            <td class="col2">
                                <asp:Label ID="lbCommodity" runat="server" />
                                <span class="divider">•</span>
                                <asp:Label ID="lbArea" runat="server" />
                                <span class="divider">•</span>
                                <asp:Label ID="lbPosition" runat="server" />
                            </td>
                        </tr>
                        <tr id="trArkusz" runat="server" class="divider" visible="false">
                            <td class="col1 narrow">Arkusz:</td>
                            <td class="col2">
                                <asp:Label ID="lbArkusz" runat="server" />
                            </td>
                        </tr>
                        <%--
                        <tr class="divider zmiana">
                            <td class="col1">Zmiana</td>
                            <td class="col2">
                                <asp:Label ID="lbZmiana" runat="server" Visible="false"></asp:Label>
                                <asp:DropDownList ID="ddlZmiana" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="ddlZmiana_SelectedIndexChanged"></asp:DropDownList><br />
                                <asp:Label ID="lbZmianaZgoda" runat="server" CssClass="zgoda" Visible="false" Text="Zgoda na nadgodziny"></asp:Label>
                                <asp:Label ID="lbZmianaBrak" runat="server" CssClass="brakzgody" Visible="false" Text="Brak zgody na nadgodziny"></asp:Label>
                            </td>
                        </tr>
                        --%>
                        <tr class="divider">
                            <td class="col1">Strefa RCP:</td>
                            <td class="col2">
                                <asp:Label ID="lbStrefaRCP" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="col1">Algorytm:</td>
                            <td class="col2">
                                <asp:Label ID="lbAlgorytm" runat="server"></asp:Label></td>
                        </tr>

                        <%----%>
                        <tr id="trEtat" runat="server" class="divider" visible="false">
                            <td class="col1">Etat:</td>
                            <td class="col2">
                                <asp:Label ID="lbEtat" runat="server"></asp:Label></td>
                        </tr>
                        <tr id="trWymiar" runat="server" class="xdivider" visible="false">
                            <td class="col1">Wymiar czasu pracy:</td>
                            <td class="col2">
                                <asp:Label ID="lbWymiarT" runat="server"></asp:Label></td>
                        </tr>
                        <tr id="trPrzerwaWliczona" runat="server" class="xdivider" visible="false">
                            <td class="col1">Przerwa wliczona:</td>
                            <td class="col2">
                                <asp:Label ID="lbPrzerwaWliczona" runat="server"></asp:Label></td>
                        </tr>
                        <tr id="trPrzerwaNiewliczona" runat="server" class="xdivider" visible="false">
                            <td class="col1">Przerwa niewliczona:</td>
                            <td class="col2">
                                <asp:Label ID="lbPrzerwaNiewliczona" runat="server"></asp:Label></td>
                        </tr>
                        <%----%>
                    </table>
                </asp:View>

                <asp:View ID="pgRCP" runat="server" OnActivate="pgRCP_Activate">    
                    <uc1:RcpControl ID="cntRcp" StrefaSelect="false" RoundSelect="false" RoundTypeSelect="false" runat="server" Visible="true" />
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
    <div class="buttons">
        <asp:Label ID="lbInfoJa" runat="server" CssClass="error" Text="Edycja i akceptacja danych zablokowana&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Visible="false"></asp:Label>
        <asp:Button ID="btAccept" CssClass="button100" runat="server" Visible="false" Text="Zaakceptuj" OnClick="btAccept_Click" />
        <asp:Button ID="btAccept2" CssClass="button_postback" runat="server" Visible="false" OnClick="btAccept2_Click" />
        <asp:Button ID="btSaveAcc" CssClass="button100" runat="server" Text="Zapisz" OnClick="btSaveAcc_Click" />
        <asp:Button ID="btCloseAcc" CssClass="button100" runat="server" Text="Anuluj" OnClick="btCloseAcc_Click" />
        <asp:Button ID="btUnlock" CssClass="button" runat="server" Visible="false" Text="Cofnij akceptację" OnClick="btUnlock_Click" />
        <asp:Button ID="btCloseAcc1" CssClass="button100" runat="server" Visible="false" Text="Zamknij" OnClick="btCloseAcc_Click" />
    </div>
</div>

<asp:SqlDataSource ID="dsZmiany_1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select 'brak zmiany (plan)' as Nazwa, null as Id, 0 as Sort, 0 as Kolejnosc 
    union 
select 'brak zmiany' as Nazwa, -1 as Id, 1 as Sort, 0 as Kolejnosc
    union 
select Symbol + ISNULL(' - ' + Nazwa, '') + 
    case when Od != Do then ' ' + LEFT(convert(varchar, Od, 8),5) + ' - ' + LEFT(convert(varchar, Do, 8),5) else '' end +
    case when HideZgoda = 0 and ZgodaNadg = 1 then ' - ZGODA NADG.' else '' end
    as Nazwa, Id, 2 as Sort, Kolejnosc 
from Zmiany 
where Visible=1 and Widoczna=1
order by Sort, Kolejnosc, Nazwa
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsZmiany" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
declare @korzm int
declare @korwym int
declare @planzm int
declare @planwym int
declare @selzm int
declare @selwym int -- tylko jeden rekord
declare @wymiar int -- wymiar pracownika

--set @wymiar = 28800
--set @planzm = null
--set @planwym = 28800
--set @korzm = 3
--set @korwym = 3000 
--set @selzm = null
--set @selwym = 19000 

set @planzm  = {0}
set @planwym = {1}
set @korzm   = {2}
set @korwym  = {3} 
set @selzm   = {4}
set @selwym  = {5} 
set @wymiar  = {6}

select Nazwa1 + case when @planzm is null and IdA = -1 or ISNULL(@planzm, -1) = IdA and ISNULL(@planwym, 0) = Wymiar then ' - PLAN' else '' end Nazwa
  , *
from 
( 	
select 'brak zmiany' as Nazwa1, '-1|0' Id, -1 IdA, null Kolor, 4 as Sort, 0 Wymiar, 0 as Kolejnosc
    union 
select Z.Symbol + ISNULL(' - ' + Z.Nazwa, '') 
--   + case when Z.Od != Z.Do then ' ' + LEFT(convert(varchar, Z.Od, 8),5) + ' - ' + LEFT(convert(varchar, Z.Do, 8),5) else '' end
  + case when Z.Od != Z.Do then ' ' + LEFT(convert(varchar, Z.Od, 8),5) + ' - ' + LEFT(convert(varchar, DATEADD(SECOND, Z4.zmCzas, Z.Od), 8),5) else '' end
--  + ISNULL(' (' + REPLACE(dbo.ToTimeHMM(T.items), ':00', '') + ' h)','')

  + ' (' + REPLACE(dbo.ToTimeHMM(Z4.zmCzas), ':00', '') + 'h)'

  + case when Z.HideZgoda = 0 and Z.ZgodaNadg = 1 then ' - ZGODA NADG.' else '' end
    as Nazwa1
  , convert(varchar, Z.Id) + '|' + CONVERT(varchar, ISNULL(T.items, Z3.zmCzas)) Id
  , Z.Id IdA
  , Z.Kolor
  , 2 Sort
  , T.items Wymiar
  , Z.Kolejnosc 
  --, Z.InneCzasy
  --, T.items
from Zmiany Z
outer apply (select DATEDIFF(SECOND, dbo.gettime(Z.Od), dbo.gettime(Z.Do)) zmCzas) Z2
outer apply (select case when Z2.zmCzas &lt; 0 then Z2.zmCzas + 86400 else Z2.zmCzas end /*+ ISNULL(Z.Par1, 0)*/ zmCzas) Z3   --zmniejszenie piątek
outer apply (
	select items from dbo.SplitInt(Z.InneCzasy, ';') where @wymiar != 28800
	union
	select @korwym from (select 1 x) x where @korzm = Z.Id
	union
	select @planwym from (select 1 x) x where @planzm = Z.Id
	union
	select @selwym from (select 1 x) x where @selzm = Z.Id
	union
	select Z3.zmCzas from (select 1 x) x where Z3.zmCzas != 0
) T
outer apply (select ISNULL(T.items, Z3.zmCzas) + case when Z.Typ = 5 then ISNULL(Z.Par1, 0) else 0 end zmCzas) Z4
where Z.Visible=1 and Z.Widoczna=1
) D
where (@selzm is null 
	or @selzm = IdA and (IdA = -1 or @selwym = Wymiar)
	)  
order by Sort, Kolejnosc, Wymiar desc, Nazwa
    ">
</asp:SqlDataSource>





