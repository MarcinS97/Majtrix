<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AcceptControl3.ascx.cs" Inherits="HRRcp.Controls.AcceptControl3" %>
<%@ Register src="RcpControl.ascx" tagname="RcpControl" tagprefix="uc1" %>
<%@ Register src="RcpAnalizeControl.ascx" tagname="RcpAnalizeControl" tagprefix="uc1" %>
<%@ Register src="Title3.ascx" tagname="Title3" tagprefix="uc1" %>
<%@ Register src="TimeEdit.ascx" tagname="TimeEdit" tagprefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register src="MPK.ascx" tagname="MPK" tagprefix="uc2" %>

<asp:HiddenField ID="hidPPId" runat="server" />
<asp:HiddenField ID="hidPracId" runat="server" />
<asp:HiddenField ID="hidData" runat="server" />
<asp:HiddenField ID="hidAlerty" runat="server" />
<asp:HiddenField ID="hidFrom" runat="server" />
<asp:HiddenField ID="hidTo" runat="server" />

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
        <td class="colright1">
            <span class="t5">Dane RCP</span>
        </td>
        <td class="colright2">
            <asp:Label ID="lbTimeRound" CssClass="t1" runat="server" >Zaokrąglenia:</asp:Label>
            <asp:DropDownList ID="ddlTimeRound" runat="server" AutoPostBack="true" onselectedindexchanged="ddlTimeRound_SelectedIndexChanged"></asp:DropDownList>
        </td>
    </tr>        
    <tr>
        <td rowspan="2" class="colleft">
        
            <table class="table0 tbAccept2" style="display: none;">
                <tr>
                    <td class="col1">Dział</td>
                    <td class="col2"><asp:Label ID="lbDzial" runat="server" ></asp:Label></td>
                </tr>
                <tr>
                    <td class="col1">Stanowisko</td>
                    <td class="col2"><asp:Label ID="lbStanowisko" runat="server" ></asp:Label></td>
                </tr>
                <tr class="divider">
                    <td class="col1">Strefa RCP</td>
                    <td class="col2"><asp:Label ID="lbStrefaRCP" runat="server" ></asp:Label></td>
                </tr>
                <tr>
                    <td class="col1">Algorytm</td>
                    <td class="col2"><asp:Label ID="lbAlgorytm" runat="server" ></asp:Label></td>
                </tr>
            </table>

            <div class="spacer16"></div>
            
            <span class="t5">Czas pracy</span>
            <table class="table0 tbAccept3">
                <tr class="topline">
                    <td class="col1"></td>
                    <td class="col2">Dane RCP</td>
                    <td class="col3"><asp:Label ID="lbZmienNa" runat="server" Text="Zmień na ..." ></asp:Label></td>
                </tr>

                <tr class="divider zmiana">
                    <td class="col1">Zmiana</td>
                    <td class="col2" colspan="2">
                        <asp:DropDownList ID="ddlZmiana" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="ddlZmiana_SelectedIndexChanged"></asp:DropDownList>
                        <asp:Label ID="lbZmiana" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td class="col1">Godzina wejścia</td>
                    <td class="col2"><asp:Label ID="lbTimeIn" runat="server" ></asp:Label></td>
                    <td class="col3">
                        <uc1:TimeEdit ID="teTimeIn" runat="server" />
                        <asp:Label ID="lbTimeInVal" Visible="false" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="col1">Godzina wyjścia</td>
                    <td class="col2"><asp:Label ID="lbTimeOut" runat="server" ></asp:Label></td>
                    <td class="col3">
                        <uc1:TimeEdit ID="teTimeOut" runat="server" />
                        <asp:Label ID="lbTimeOutVal" Visible="false" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr class="divider">
                    <td class="col1">Łączny czas pracy</td>
                    <td class="col2"><asp:Label ID="lbWorktimeAll" runat="server" ></asp:Label></td>
                    <td class="col3">
                        <asp:TextBox ID="tbWorktimeAll" Visible="false" runat="server"></asp:TextBox>
                        <asp:Label ID="lbWorktimeAllVal" Visible="false" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td class="col1">Czas pracy (zmiana)</td>
                    <td class="col2"><asp:Label ID="lbWorktime" runat="server" ></asp:Label></td>
                    <td class="col3">
                        <uc1:TimeEdit ID="teWorktime" runat="server" />
                        <asp:Label ID="lbWorktimeVal" Visible="false" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td class="col1">Nadgodziny w dzień</td>
                    <td class="col2"><asp:Label ID="lbNadgDzien" runat="server" ></asp:Label></td>
                    <td class="col3">
                        <uc1:TimeEdit ID="teNadgDzien" runat="server" />
                        <asp:Label ID="lbNadgDzienVal" Visible="false" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td class="col1">Nadgodziny w nocy</td>
                    <td class="col2"><asp:Label ID="lbNadgNoc" runat="server" ></asp:Label></td>
                    <td class="col3">
                        <uc1:TimeEdit ID="teNadgNoc" runat="server" />
                        <asp:Label ID="lbNadgNocVal" Visible="false" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td class="col1">Praca w nocy (<asp:Label ID="lbNocneOdDo" runat="server" Text="22-6"></asp:Label>)</td>
                    <td class="col2"><asp:Label ID="lbNocne" runat="server" ></asp:Label></td>
                    <td class="col3">
                        <uc1:TimeEdit ID="teNocne" runat="server" />
                        <asp:Label ID="lbNocneVal" Visible="false" runat="server" ></asp:Label>
                    </td>
                </tr>


                <tr>
                    <td colspan="3">
                            
                            
                        <div id="paRcpScroll" class="vscrollbox">        
        
                            <uc2:MPK ID="cntMPK" runat="server" />
            
                        </div>
                    </td>
                </tr>


                <tr class="divider absencja">
                    <td class="col1">Powód absencji</td>
                    <td class="col3" colspan="2">
                        <asp:Label ID="lbAbsencja" runat="server" ></asp:Label>
                        <asp:DropDownList ID="ddlAbsencja" runat="server" />
                    </td>
                </tr>
                <tr class="uwagi">
                    <td class="col1">Uwagi</td>
                    <td  class="col3" colspan="2">
                        <asp:TextBox ID="tbUwagi" CssClass="textbox" runat="server" Rows="2" TextMode="MultiLine"></asp:TextBox>
                        <asp:Label ID="lbUwagi" Visible="false" runat="server" ></asp:Label>
                    </td>
                </tr>
            </table>

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
        
        </td>
        <td colspan="2" class="colright">
            <div id="paAcceptScroll" class="vscrollbox">
                <uc1:RcpAnalizeControl ID="cntRcpAnalize" runat="server" />                
                <uc1:RcpControl ID="cntRcp" StrefaSelect="false" RoundSelect="false" RoundTypeSelect="false" runat="server" />
            </div>
            <div class="buttons">
                <asp:Button ID="btAccept" CssClass="button100" runat="server" Visible="false" Text="Zaakceptuj" onclick="btAccept_Click" />
                <asp:Button ID="btAccept2" CssClass="button_postback" runat="server" Visible="false" onclick="btAccept2_Click" />
                <asp:Button ID="btSaveAcc" CssClass="button100" runat="server" Text="Zapisz" onclick="btSaveAcc_Click" />
                <asp:Button ID="btCloseAcc" CssClass="button100" runat="server" Text="Anuluj" onclick="btCloseAcc_Click" />
                <asp:Button ID="btUnlock" CssClass="button125" runat="server" Visible="false" Text="Cofnij akceptację" onclick="btUnlock_Click" />
                <asp:Button ID="btCloseAcc1" CssClass="button100" runat="server" Visible="false" Text="Zamknij" onclick="btCloseAcc_Click" />
            </div>
        </td>    
    </tr>
    <tr>
        <td class="buttons">
        </td>
    </tr>
</table>
