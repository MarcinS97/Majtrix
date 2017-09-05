<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AcceptControl2a.ascx.cs" Inherits="HRRcp.Controls.AcceptControl2a" %>
<%@ Register src="RcpControl.ascx" tagname="RcpControl" tagprefix="uc1" %>
<%@ Register src="RcpAnalizeControl.ascx" tagname="RcpAnalizeControl" tagprefix="uc1" %>
<%@ Register src="Title3.ascx" tagname="Title3" tagprefix="uc1" %>
<%@ Register src="TimeEdit.ascx" tagname="TimeEdit" tagprefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%--
<%@ Register src="MPK2.ascx" tagname="MPK" tagprefix="uc2" %>
--%>
<%@ Register src="MPK3.ascx" tagname="MPK" tagprefix="uc2" %>
<%@ Register src="~/Controls/RozliczenieNadg/cntPrzeznaczNadg.ascx" tagname="cntPrzeznaczNadg" tagprefix="uc3" %>
<%@ Register Src="~/RCP/Controls/cntNadgodzinyWnioskiModal.ascx" TagPrefix="uc1" TagName="cntNadgodzinyWnioskiModal" %>
<%@ Register Src="~/Controls/Reports/cntReport2.ascx" TagPrefix="uc1" TagName="cntReport2" %>


<asp:HiddenField ID="hidPPId" runat="server" Visible="false" />
<asp:HiddenField ID="hidPracId" runat="server" Visible="false" />
<asp:HiddenField ID="hidData" runat="server" Visible="false" />
<asp:HiddenField ID="hidAlerty" runat="server" Visible="false" />
<asp:HiddenField ID="hidFrom" runat="server" Visible="false" />
<asp:HiddenField ID="hidTo" runat="server" Visible="false" />

<table id="cntPPAccept" class="table0">
    <tr class="title">
        <td colspan="3" class="title">
            <uc1:Title3 ID="Title31" runat="server" 
                Ico="../images/captions/PracNotes.png"
                Title="Akceptacja czasu pracy pracownika" 
                SubText1="Pracownik:"
                SubText2="Data:"/>
            <div class="kierinfo round5">
                <table class="kierinfo table0">
                    <tr>
                        <td class="col1">
                            <asp:Label ID="lbKierAccLabel" CssClass="t1" runat="server" Text="Zaakceptował:"></asp:Label>
                        </td>
                        <td class="col2">
                            <asp:Label ID="lbKierAcc" runat="server" ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="col1">
                            <span class="t1">Data:</span>
                        </td>
                        <td class="col2">
                            <asp:Label ID="lbDataAcc" runat="server" ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="col1">
                            <span class="t1">Status:</span>
                        </td>
                        <td class="col2">
                            <asp:Label ID="lbStatus" runat="server" ></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>            
        </td>
    </tr>
    <tr class="line1">
        <td class="colleft">
            <span class="t5">Dane pracownika</span>        
        </td>
        <%--<td class="colright1">
            <span class="t5">Dane RCP</span>
        </td>--%>
        <td class="colright2">

            <%--        
            <asp:Label ID="lbTimeRound" CssClass="t1" runat="server" >Zaokrąglenia:</asp:Label>
            <asp:DropDownList ID="ddlTimeRound" runat="server" AutoPostBack="true" onselectedindexchanged="ddlTimeRound_SelectedIndexChanged"></asp:DropDownList>
            --%>

        </td>
    </tr>        
    <tr>
        <td rowspan="2" class="colleft">
            <table class="table0 tbAccept2">
                <tr id="tr00" runat="server"><td class="col1">Dział</td>
                    <td class="col2"><asp:Label ID="lbDzial" runat="server" ></asp:Label></td>
                </tr>
                <tr id="tr01" runat="server"><td class="col1">Stanowisko</td>
                    <td class="col2"><asp:Label ID="lbStanowisko" runat="server" ></asp:Label></td>
                </tr>
                <tr id="tr02" runat="server" class="divider"><td class="col1">CC - Projekt</td>
                    <td class="col2"><asp:Label ID="lbCC" runat="server" ></asp:Label></td>
                </tr>
                <tr id="trComm" runat="server" visible="false"><td class="col1 narrow">Commodity Area Position</td>
                    <td class="col2">
                        <asp:Label ID="lbCommodity" runat="server" /> <span class="divider">•</span>
                        <asp:Label ID="lbArea" runat="server" /> <span class="divider">•</span>
                        <asp:Label ID="lbPosition" runat="server" />
                    </td>
                </tr>
                <tr id="trArkusz" runat="server" visible="false"><td class="col1 narrow">Arkusz</td>
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
                <tr class="divider"><td class="col1">Strefa RCP</td>
                    <td class="col2"><asp:Label ID="lbStrefaRCP" runat="server" ></asp:Label></td>
                </tr>
                <tr><td class="col1">Algorytm</td>
                    <td class="col2"><asp:Label ID="lbAlgorytm" runat="server" ></asp:Label></td>
                </tr>
            </table>

            <!--<div class="spacer16"></div>-->
            
            <asp:Menu ID="tabAccept" runat="server" Orientation="Horizontal" 
                onmenuitemclick="tabAccept_MenuItemClick" >
                <StaticMenuStyle CssClass="tabsStrip" />
                <StaticMenuItemStyle CssClass="tabItem" />
                <StaticSelectedStyle CssClass="tabSelected" />
                <StaticHoverStyle CssClass="tabHover" />
                <Items>
                    <asp:MenuItem Text="Czas pracy" Value="0" Selected="True" ></asp:MenuItem>
                    <asp:MenuItem Text="Podział kosztów" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="Klasyfikacja nadgodzin" Value="2"></asp:MenuItem>
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

            <asp:MultiView ID="mvCzasPodzial" runat="server" ActiveViewIndex="0">
                <asp:View ID="pgCzasPracy" runat="server">
                    <table class="table0 tbAccept3">
                        <tr class="topline"><td class="col1"></td>
                            <td class="col2">Dane RCP</td>
                            <td class="col3"><asp:Label ID="lbZmienNa" runat="server" Text="Zmień na ..." ></asp:Label></td>
                        </tr>
                        <tr><td class="col1">Godzina wejścia</td>
                            <td class="col2"><asp:Label ID="lbTimeIn" runat="server" ></asp:Label></td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teTimeIn" runat="server" Right="true" InLineCount="4"/>
                                <asp:Label ID="lbTimeInVal" Visible="false" runat="server" ></asp:Label>
                            </td>
                        </tr>
                        <tr><td class="col1">Godzina wyjścia</td>
                            <td class="col2"><asp:Label ID="lbTimeOut" runat="server" ></asp:Label></td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teTimeOut" runat="server" Right="true" InLineCount="4" />
                                <asp:Label ID="lbTimeOutVal" Visible="false" runat="server" ></asp:Label>
                            </td>
                        </tr>
                        <tr class="divider"><td class="col1">
                            <%--                            
                            Łączny czas pracy
                            --%>                            
                                <asp:Label ID="lbLacznieBezPrzerw" runat="server" Text="Łącznie bez przerw"></asp:Label>
                                <asp:Label ID="lbLacznieZPrzerwami" runat="server" Text="Łącznie z przerwą" Visible="false" ></asp:Label>
                            </td>
                            <td class="col2"><asp:Label ID="lbWorktimeAll" runat="server" ></asp:Label></td>
                            <td class="col3 przerwy">
                                <asp:TextBox ID="tbWorktimeAll" Visible="false" runat="server"></asp:TextBox>
                                <asp:Label ID="lbWorktimeAllVal" Visible="false" runat="server" ></asp:Label>
                                <asp:Label ID="lbPrzerwy" Visible="false" runat="server" ></asp:Label>
                            </td>
                        </tr>
                        <tr id="trWStrefie" runat="server" ><td class="col1">
                                W strefie
                                <asp:Label ID="lbWStrefieBezPrzerw" runat="server" Text="W strefie bez przerw" Visible="false"></asp:Label>
                                <asp:Label ID="lbWStrefieZPrzerwami" runat="server" Text="W strefie z przerwami" Visible="false" ></asp:Label>
                            </td>
                            <td class="col2"><asp:Label ID="lbWStrefie" runat="server" ></asp:Label></td>
                            <td class="col3">
                            </td>
                        </tr>
                        <tr class="divider zmiana"><td class="col1">Zmiana</td>
                            <td class="col3" colspan="2">
                                <asp:Label ID="lbZmiana" runat="server" Visible="false"></asp:Label>
                                <asp:DropDownList ID="ddlZmiana" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="ddlZmiana_SelectedIndexChanged"></asp:DropDownList><br />
                                <asp:Label ID="lbZmianaZgoda" runat="server" CssClass="zgoda" Visible="false" Text="Zgoda na nadgodziny"></asp:Label>
                                <asp:Label ID="lbZmianaBrak" runat="server" CssClass="brakzgody" Visible="false" Text="Brak zgody na nadgodziny"></asp:Label>
                            </td>
                        </tr>
                        
                        <tr><td class="col1">Czas na zmianie</td>
                            <td class="col2"><asp:Label ID="lbWorktime" runat="server" ></asp:Label></td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teWorktime" runat="server" Right="true" />
                                <asp:Label ID="lbWorktimeVal" Visible="false" runat="server" ></asp:Label>
                            </td>
                        </tr>
                        <tr><td class="col1">Nadgodziny w dzień</td>
                            <td class="col2"><asp:Label ID="lbNadgDzien" runat="server" ></asp:Label></td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teNadgDzien" runat="server" Right="true" />
                                <asp:Label ID="lbNadgDzienVal" Visible="false" runat="server" ></asp:Label>
                            </td>
                        </tr>
                        <tr><td class="col1">Nadgodziny w nocy</td>
                            <td class="col2"><asp:Label ID="lbNadgNoc" runat="server" ></asp:Label></td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teNadgNoc" runat="server" Right="true" />
                                <asp:Label ID="lbNadgNocVal" Visible="false" runat="server" ></asp:Label>
                            </td>
                        </tr>
                        <tr><td class="col1">W nocy (<asp:Label ID="lbNocneOdDo" runat="server" Text="22-6" />)</td>
                            <td class="col2"><asp:Label ID="lbNocne" runat="server" ></asp:Label></td>
                            <td class="col3">
                                <uc1:TimeEdit ID="teNocne" runat="server" Right="true" />
                                <asp:Label ID="lbNocneVal" Visible="false" runat="server" ></asp:Label>
                            </td>
                        </tr>
                        <tr class="divider absencja"><td class="col1">Powód absencji</td>
                            <td class="col3" colspan="2">
                                <asp:Label ID="lbAbsencja" runat="server" ></asp:Label>
                                <asp:DropDownList ID="ddlAbsencja" runat="server" />
                            </td>
                        </tr>
                        <tr class="uwagi3"><td class="col1">Uwagi</td>
                            <td  class="col3" colspan="2">
                                <asp:TextBox ID="tbUwagi" CssClass="textbox" runat="server" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                <asp:Label ID="lbUwagi" Visible="false" runat="server" ></asp:Label>
                            </td>
                        </tr>
                    </table>
    
                    <div class="spacer16"></div>

                    <span class="t5">Wnioski o nadgodziny</span>
                    <uc1:cntReport2 runat="server" ID="cntReport2" GridVisible="false" CssClass="report_page noborder report-header" SQL="
select
  p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') Wnioskujący
/*, Nadg [Ilość (h)]*/
, Nadg50 [50 (h)]
, Nadg100 [100 (h)]
, nws.Nazwa [Status]
from rcpNadgodzinyWnioski nw
inner join Pracownicy p on p.Id = nw.AutorId
inner join rcpNadgodzinyWnioskiStatus nws on nws.Id = nw.Status
where nw.IdPracownika = '@SQL1' and nw.Data = '@SQL2'
"/>
                    <br />
                    <asp:Button ID="btWniosekNadg" CssClass="button" runat="server" Visible="true" Text="Wniosek o nadgodziny" onclick="btWniosekNadg_Click" />

                    <div class="spacer16"></div>
                    
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
                </asp:View>

                <asp:View ID="pgPodzialKosztow" runat="server" onactivate="pgPodzialKosztow_Activate" >
                    <uc2:MPK ID="cntMPK" runat="server" />
                </asp:View>

                <asp:View ID="pgNadgodziny" runat="server" onactivate="pgNadgodziny_Activate" >
                    <uc3:cntPrzeznaczNadg ID="cntPrzeznaczNadg" runat="server" />
                </asp:View>
                
            </asp:MultiView>
        </td>
        <td xcolspan="2" class="colright">
            <div id="paAcceptScroll" class="vscrollbox vscrollboxright">
                <uc1:RcpAnalizeControl ID="cntRcpAnalize" runat="server" />                
                <uc1:RcpControl ID="cntRcp" StrefaSelect="false" RoundSelect="false" RoundTypeSelect="false" runat="server" />
            </div>
        </td>
        <td>
            <div class="buttons">
                <asp:Label ID="lbInfoJa" runat="server" CssClass="error" Text="Edycja i akceptacja danych zablokowana&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Visible="false"></asp:Label>
                
                <asp:Button ID="btAccept" CssClass="button100" runat="server" Visible="false" Text="Zaakceptuj" onclick="btAccept_Click" />
                <asp:Button ID="btAccept2" CssClass="button_postback" runat="server" Visible="false" onclick="btAccept2_Click" />
                <asp:Button ID="btSaveAcc" CssClass="button100" runat="server" Text="Zapisz" onclick="btSaveAcc_Click" />
                <asp:Button ID="btCloseAcc" CssClass="button100" runat="server" Text="Anuluj" onclick="btCloseAcc_Click" />
                <asp:Button ID="btUnlock" CssClass="button125" runat="server" Visible="false" Text="Cofnij akceptację" onclick="btUnlock_Click" />
                <asp:Button ID="btCloseAcc1" CssClass="button100" runat="server" Visible="false" Text="Zamknij" onclick="btCloseAcc_Click" />

                <asp:Button ID="btnShow" runat="server" CssClass="btn btn-default" style="width: 100px;" onclientclick="$('#paAcceptScroll').toggle('medium'); return false" text="Szczegóły" UseSubmitBehavior="false"/>
            </div>
        </td>
    </tr>
</table>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <uc1:cntNadgodzinyWnioskiModal runat="server" ID="cntNadgodzinyWnioskiModal" Mode="PostAccept" />
    </ContentTemplate>
</asp:UpdatePanel>

