<%@ Page Title="" ValidateRequest="false" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AdminForm.aspx.cs" Inherits="HRRcp.AdminForm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/StrukturaControl.ascx" tagname="StrukturaControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/ImportStruktura.ascx" tagname="ImportStruktura" tagprefix="uc1" %>
<%@ Register src="~/Controls/SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc1" %>
<%@ Register src="~/Controls/RcpControl.ascx" tagname="RcpControl" tagprefix="uc1" %>


<%@ Register src="~/Controls/PracownicyList.ascx" tagname="PracownicyList" tagprefix="uc1" %>
<%@ Register src="~/Controls/Adm/cntPracownicy3.ascx" tagname="cntPracownicy2" tagprefix="uc1" %>


<%@ Register src="~/Controls/StrefyControl2.ascx" tagname="StrefyControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/ZmianyControl3.ascx" tagname="ZmianyControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/AdmParamsControl.ascx" tagname="AdmParamsControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/DzialyControl.ascx" tagname="DzialyControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/PlanPracyZmiany.ascx" tagname="PlanPracyZmiany" tagprefix="uc1" %>
<%@ Register src="~/Controls/PlanPracyAccept.ascx" tagname="PlanPracyAccept" tagprefix="uc1" %>
<%@ Register src="~/Controls/PlanPracyRozliczenie.ascx" tagname="PlanPracyRozliczenie" tagprefix="uc1" %>
<%@ Register src="~/Controls/AdmZastepstwa.ascx" tagname="AdmZastepstwa" tagprefix="uc1" %>
<%@ Register src="~/Controls/Przypisania/cntPrzesunieciaAdm.ascx" tagname="cntPrzesunieciaAdm" tagprefix="uc2" %>
<%@ Register src="~/Controls/Portal/paWnioskiUrlopoweAdm.ascx" tagname="paWnioskiUrlopoweAdm" tagprefix="uc1" %>
<%@ Register src="~/Controls/PlanUrlopow/cntPlanUrlopow.ascx" tagname="cntPlanUrlopow" tagprefix="uc1" %>

<%@ Register src="~/Controls/Adm/cntImportALARMUS.ascx" tagname="cntImportALARMUS" tagprefix="uc3" %>
<%@ Register src="~/Controls/Adm/cntImportABSENCJA.ascx" tagname="cntImportABSENCJA" tagprefix="uc3" %>
<%@ Register src="~/Controls/Adm/cntImportABSENCJA2.ascx" tagname="cntImportABSENCJA2" tagprefix="uc2" %>


<%--
<%@ Register src="Controls/AdmLogControl.ascx" tagname="AdmLogControl" tagprefix="uc1" %>
<%@ Register src="Controls/AdmInfoControl.ascx" tagname="AdmInfoControl" tagprefix="uc3" %>
<%@ Register src="Controls/AdmMailsControl.ascx" tagname="AdmMailsControl" tagprefix="uc3" %>
<%@ Register src="Controls/AdmSettingsControl.ascx" tagname="AdmSettingsControl" tagprefix="uc3" %>
<%@ Register src="Controls/AdmKodyWyjasnicControl.ascx" tagname="AdmKodyWyjasnicControl" tagprefix="uc3" %>
<%@ Register src="Controls/ProgressBar2.ascx" tagname="ProgressBar2" tagprefix="uc3" %>
<%@ Register src="Controls/PracInfo.ascx" tagname="PracInfo" tagprefix="uc1" %>
--%>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/print.css" rel="stylesheet" media="print" type="text/css" runat="server" id="headPrint" visible="false"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="caption" >
        <tr>
            <td>
                <span class="caption4">
                    <img alt="" src="images/captions/Struktura.png"/>
                    Administracja
                </span>
            </td>
            <td class="btprint_top" align="right">
                <%--
                <asp:Button ID="btRepPrint1" class="button75" runat="server" Text="Drukuj" OnClientClick="printPreview();return false;" Visible="false" />
                <asp:Button ID="btRepBack1" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" Visible="false" />
                <asp:Button ID="btRepExcel1" class="button75" runat="server" Text="Excel" OnClientClick="return exportExcelClick();" onclick="btExcel_Click" Visible="false" />
                --%>
            </td>
        </tr>
    </table>     
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
            <asp:Menu ID="tabAdmin" runat="server" Orientation="Horizontal" 
                onmenuitemclick="tabAdmin_MenuItemClick" >
                <StaticMenuStyle CssClass="tabsStrip tabsStrip_AdminForm" />
                <StaticMenuItemStyle CssClass="tabItem" />
                <StaticSelectedStyle CssClass="tabSelected" />
                <StaticHoverStyle CssClass="tabHover" />
                <Items>
                    <asp:MenuItem Text="Struktura organizacyjna" Value="0" Selected="True"></asp:MenuItem>
                    <asp:MenuItem Text="Pracownicy"         Value="11"></asp:MenuItem>
                    <asp:MenuItem Text="P1"                 Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="Wnioski urlopowe"   Value="10"></asp:MenuItem>
                    <asp:MenuItem Text="Przesunięcia"       Value="9"></asp:MenuItem>
                    <asp:MenuItem Text="Strefy RCP"         Value="2"></asp:MenuItem>                    
                    <%--
                    <asp:MenuItem Text="Działy"             Value="3"></asp:MenuItem>                    
                    --%>
                    <asp:MenuItem Text="Zmiany"             Value="4"></asp:MenuItem>
                    <asp:MenuItem Text="Zastępstwa"         Value="8"></asp:MenuItem>
                    <%--
                    <asp:MenuItem Text="Alerty" Value="5"></asp:MenuItem>
                    --%>
                    <asp:MenuItem Text="Parametry" Value="5"></asp:MenuItem>
                    <asp:MenuItem Text="Plan pracy" Value="6"></asp:MenuItem>
                    <asp:MenuItem Text="Akceptacja czasu pracy" Value="7"></asp:MenuItem>
                    <asp:MenuItem Text="Rozliczenie nadgodzin" Value="12"></asp:MenuItem>
                    <asp:MenuItem Text="Plan urlopów" Value="13"></asp:MenuItem>
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
                
            <asp:Timer ID="ProgressTimer" runat="server" Enabled="False" Interval="1000" 
                ontick="ProgressTimer_Tick">
            </asp:Timer>
            
            <div id="admTabsContent" class="tabsContent paAdmin" style="background-color:#FFF;">
                <asp:MultiView ID="mvAdministracja" runat="server" ActiveViewIndex="0">
                    
                    <asp:View ID="pgStruktura" runat="server" onactivate="pgStruktura_Activate">
                        <div class="padding">
                            <table id="tbAdmStruktura" class="table0" >
                                <tr>
                                    <td class="tdLeft" valign="top">
                                        <span class="t5">Struktura organizacyjna w okresie rozliczeniowym</span> 
                                        <table id="navStruktura" class="okres_navigator okres_navigator_struktura" runat="server">
                                            <tr>
                                                <td class="col1">
                                                    <uc1:SelectOkres ID="cntSelectOkresStruct" ControlID="cntStruktura" ParentID="navStruktura" ForStruktura="true" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                        <uc1:StrukturaControl ID="cntStruktura" OnSelectedChanged="cntStruktura_SelectedChanged" runat="server" />
                                    </td>
                                    <td class="tdRight" valign="top">
                                        <span class="t5">Dane RCP pracownika:</span> 
                                        <asp:Label ID="lbPracName" runat="server" ></asp:Label>
                                        <table class="okres_navigator okres_navigator_struktura">
                                            <tr>
                                                <td class="col1">
                                                    <uc1:SelectOkres ID="cntSelectOkres" ControlID="cntRCPStruct" runat="server" />
                                                </td>
                                                <td class="col2">
                                                    <asp:Button ID="btRefresh1" runat="server" class="button" onclick="btRefresh1_Click" Text="Odśwież" />
                                                    <asp:Button ID="btHide1" runat="server" class="button" Text="Ukryj szczegóły" />
                                                </td>
                                            </tr>
                                        </table>
                                        <uc1:RcpControl ID="cntRCPStruct" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <table class="table0 paImportButtons narrow">
                                <tr>
                                    <td class="left">
                                        <uc3:cntImportABSENCJA ID="cntImportABSENCJA" runat="server" Visible="false" OnImportFinished="cntImportABSENCJA_ImportFinished" />    
                                        <uc2:cntImportABSENCJA2 ID="cntImportABSENCJA2" runat="server" Visible="false" OnImportFinished="cntImportABSENCJA_ImportFinished" Typ="7" />    
                                        <uc2:cntImportABSENCJA2 ID="cntImportUmowy" runat="server" Visible="false" OnImportFinished="cntImportABSENCJA_ImportFinished" Typ="8" />    
                                        <%-- <uc2:cntImportABSENCJA2 ID="cntImportABSENCJA" runat="server" Visible="false" OnImportFinished="cntImportABSENCJA_ImportFinished" Typ="6" />    --%>           
                                        <uc3:cntImportALARMUS ID="cntImportALARMUS1" runat="server" Visible="false" OnImportFinished="cntImportALARMUS1_ImportFinished"/>                
                                        <uc1:ImportStruktura ID="ImportStruktura" OnImportFinished="ImportFinished" runat="server" Visible="false" />
                                    </td>
                                    <td class="right">
                                        <div>
                                            <asp:Label ID="lbInfo" runat="server" ></asp:Label><br />
                                            <asp:Button ID="btWeekClose" runat="server" CssClass="button250" Text="Zamknięcie tygodnia" onclick="btWeekClose_Click" />
                                            <asp:Button ID="btWeekOpen" runat="server" CssClass="button250" Text="Odblokowanie tygodnia" onclick="btWeekOpen_Click" />
                                            <div class="spacer8"></div>
                                            <asp:Button ID="Button3" runat="server" CssClass="button250" Text="Import danych z systemu KP" ToolTip="Import absencji, stawek wynagrodzenia i kalendarza" onclick="Button3_Click" Visible="false"/>
                                            <asp:Button ID="Button2" runat="server" CssClass="button250" Text="Zamknięcie miesiąca" onclick="Button2_Click" />
                                            <asp:Button ID="Button4" runat="server" CssClass="button250" Text="Odblokowanie miesiąca" Enabled="false" onclick="Button4_Click" />
                                            <hr />
                                            <asp:Button ID="Button6" runat="server" CssClass="button250" Text="Import danych z Asseco HR" ToolTip="Import absencji, stawek wynagrodzenia i kalendarza z systemu HR" onclick="Button6_Click" />
                                            <asp:Button ID="Button5" runat="server" CssClass="button250" Text="Eksport do Asseco HR" ToolTip="Eksport danych czasu pracy do systemu kadrowo-płacowego" onclick="Button5_Click" />
                                            <div id="paPodzialLudzi" runat="server" visible="false" style="display: block; ">
                                                <hr />
                                                <asp:Button ID="btPodzial" runat="server" CssClass="button250" Text="Podział Ludzi - Administracja" onclick="btPodzial_Click" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:View>
                    
                    <asp:View ID="pgPracownicy" runat="server" >
                        <div class="padding">
                            <uc1:PracownicyList ID="cntPracownicy" OnSelectedChanged="cntPracownicy_SelectedChanged" runat="server" />
                            <table class="okres_navigator okres_navigator_pracownicy">
                                <tr>
                                    <td class="colleft"></td>
                                    <td class="colmiddle">
                                        <uc1:SelectOkres ID="cntSelectOkres1" ControlID="cntRCPPrac1,cntRCPPrac2" runat="server" />
                                    </td>
                                    <td class="colright">
                                        <asp:Button ID="btRefresh" runat="server" CssClass="button" onclick="btRefresh_Click" Text="Odśwież" />
                                        <asp:Button ID="Button1" runat="server" CssClass="button" Text="Ukryj szczegóły" />
                                    </td>
                                </tr>
                            </table>
                            <table class="table0 pracownicy_rcp"> 
                                <tr>
                                    <td valign="top">
                                        <uc1:RcpControl ID="cntRCPPrac1" runat="server" />
                                    </td>
                                    <td width="8"></td>
                                    <td valign="top">
                                        <uc1:RcpControl ID="cntRCPPrac2" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:View>

                    <asp:View ID="pgStrefyRCP" runat="server">
                        <div class="padding">
                            <uc1:StrefyControl ID="StrefyControl1" runat="server" />
                        </div>    
                    </asp:View>

                    <asp:View ID="pgDzialy" runat="server">
                        <div class="padding">
                            <uc1:DzialyControl ID="Dzialy" runat="server" />
                        </div>    
                    </asp:View>

                    <asp:View ID="pgZmiany" runat="server">
                        <div class="padding">
                            <uc1:ZmianyControl ID="cntZmiany" runat="server" />
                        </div>
                    </asp:View>

                    <asp:View ID="pgParams" runat="server">
                        <uc1:AdmParamsControl ID="cntParams" runat="server" />
                    </asp:View>

                    <asp:View ID="pgPlanPracy" runat="server" onactivate="pgPlanPracy_Activate">
                        <div class="padding">
                            <div class="printoff">
                                <span class="t1">Kierownik:</span>
                                <asp:DropDownList ID="ddlKierownicy" runat="server" AutoPostBack="true" onselectedindexchanged="ddlKierownicy_SelectedIndexChanged" />
                                <div class="divider_ppacc"></div>
                            </div>
                            <uc1:PlanPracyZmiany ID="PlanPracyZmiany" Adm="1" runat="server" />
                        </div>
                    </asp:View>
                    
                    <asp:View ID="pgAkceptacjaCzasu" runat="server" onactivate="pgAkceptacjaCzasu_Activate">
                        <div class="padding">
                            <span class="t1">Kierownik:</span>
                            <asp:DropDownList ID="ddlKierownicy2" runat="server" AutoPostBack="true" onselectedindexchanged="ddlKierownicy2_SelectedIndexChanged" />
                            <div class="divider_ppacc"></div>
                            <uc1:PlanPracyAccept ID="PlanPracyAccept" Adm="1" runat="server" />
                        </div>
                    </asp:View>
                    
                    <asp:View ID="pgZastepstwa" runat="server" onactivate="pgZastepstwa_Activate">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <uc1:AdmZastepstwa ID="cntZastepstwa" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:View>

                    <asp:View ID="pgPrzesuniecia" runat="server" onactivate="pgPrzesuniecia_Activate">
                        <uc2:cntPrzesunieciaAdm ID="cntPrzesunieciaAdm" runat="server" />
                                
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" >
                            <ContentTemplate>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:View>

                    <asp:View ID="pgWnioskiUrlopowe" runat="server" onactivate="pgWnioskiUrlopowe_Activate">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc1:paWnioskiUrlopoweAdm ID="paWnioskiUrlopowe" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:View>

                    <asp:View ID="View1" runat="server" >
                        <div class="padding">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <uc1:cntPracownicy2 ID="cntPracownicy2" OnSelectedChanged="cntPracownicy2_SelectedChanged" OnCommand="cntPracownicy2_Command" runat="server" />

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </asp:View>

                    <asp:View ID="View2" runat="server" onactivate="pgRozliczenieCzasu_Activate">
                        <div class="padding">
                            <span class="t1">Kierownik:</span>
                            <asp:DropDownList ID="ddlKierownicy3" runat="server" AutoPostBack="true" onselectedindexchanged="ddlKierownicy3_SelectedIndexChanged" />
                            <div class="divider_ppacc"></div>
                            <uc1:PlanPracyRozliczenie ID="PlanPracyRozliczenie" runat="server" />
                        </div>
                    </asp:View>

                    <asp:View ID="View3" runat="server" onactivate="pgPlanUrlopow_Activate">
                        <div class="padding">
                            <div class="cntPlanUrlopow_adm">
                                <div class="left">
                                    <span class="t1">Kierownik:</span>
                                    <asp:DropDownList ID="ddlKierownicy4" runat="server" AutoPostBack="true" onselectedindexchanged="ddlKierownicy4_SelectedIndexChanged" />
                                </div>
                                <asp:Button ID="btAbsDl" runat="server" Text="Absencje długotrwałe" CssClass="button" PostBackUrl="~/AbsencjeDlugotrwale.aspx" />
<%--
                                <asp:Button ID="Button7" runat="server" Text="Absencje długotrwałe" CssClass="button" OnClick="btAbsDl_Click" />
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/AbsencjeDlugotrwale.aspx">HyperLink</asp:HyperLink>
                                <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/AbsencjeDlugotrwale.aspx">LinkButton</asp:LinkButton>
--%>
                                <div class="divider_ppacc"></div>
                            </div>
                            <uc1:cntPlanUrlopow ID="cntPlanUrlopow" Mode="1" runat="server" />
                        </div>
                    </asp:View>

                </asp:MultiView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlKierownicy" EventName="SelectedIndexChanged" />
            <asp:PostBackTrigger ControlID="ImportStruktura" />
            <asp:PostBackTrigger ControlID="cntImportALARMUS1" />
            <asp:PostBackTrigger ControlID="cntImportABSENCJA" />
            <asp:PostBackTrigger ControlID="cntImportABSENCJA2" />
            <asp:PostBackTrigger ControlID="cntImportUmowy" />
        </Triggers>
    </asp:UpdatePanel>






    <asp:SqlDataSource ID="dsGetKierNotClosedOld" runat="server"
        SelectCommand="
declare @od datetime
declare @do datetime
set @do = '{0}'
set @od = dbo.bom(@do)

select distinct R.IdKierownika as Id, ISNULL(K.Nazwisko + ' ' + K.Imie, ' Poziom główny struktury') as KierownikNI, K.Mailing, K.Email, K.Status  
from Przypisania R
left join Pracownicy K on K.Id = R.IdKierownika
left join PlanPracy PP on PP.IdPracownika = R.IdPracownika and PP.Data between dbo.MaxDate2(@od, R.Od) and dbo.MinDate2(@do, ISNULL(R.Do, '20990909'))
where R.Od &lt;= @do and @od &lt;= ISNULL(R.Do,'20990909') and R.Status = 1 and R.IdPracownika in (
	select distinct IdPracownika from PlanPracy where Data between @od and @do
)
and ISNULL(PP.Akceptacja, 0) = 0

order by KierownikNI                
        ">
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="dsGetKierNotClosed" runat="server"
        SelectCommand="
declare @od datetime
declare @do datetime
set @do = '{0}'
set @od = dbo.bom(@do)

select distinct R.IdKierownika as Id, ISNULL(K.Nazwisko + ' ' + K.Imie, ' Poziom główny struktury') as KierownikNI, K.Mailing, K.Email, K.Status  
from Przypisania R
left join Pracownicy K on K.Id = R.IdKierownika
left join PlanPracy PP on PP.IdPracownika = R.IdPracownika and PP.Data between dbo.MaxDate2(@od, R.Od) and dbo.MinDate2(@do, ISNULL(R.Do, '20990909'))
where R.Od &lt;= @do and @od &lt;= ISNULL(R.Do,'20990909') and R.Status = 1 and R.IdPracownika in (
	select distinct IdPracownika from PlanPracy where Data between @od and @do
)
and ISNULL(PP.Akceptacja, 0) = 0

union
----- brak PP ------
select --P.*,PR.RcpAlgorytm 
-- P.KadryId as Logo, P.Nazwisko + ' ' + P.Imie as Pracownik, P.KierKadryId, P.KierownikNI as Kierownik, P.Status, P.Opis, P.DataZatr, P.DataZwol
distinct R.IdKierownika as Id, ISNULL(K.Nazwisko + ' ' + K.Imie, ' Poziom główny struktury') as KierownikNI, K.Mailing, K.Email, K.Status  
from Pracownicy P
left join Przypisania R on R.IdPracownika = P.Id and @do between R.Od and ISNULL(R.Do,'20990909') and R.Status = 1 
left join Pracownicy K on K.Id = R.IdKierownika
left join PracownicyParametry PR on PR.IdPracownika = P.Id and @do between PR.Od and ISNULL(PR.Do, '20990909')
left outer join PlanPracy PP on PP.IdPracownika = P.Id and PP.Data between dbo.bom(@do) and dbo.eom(@do) and ISNULL(IdZmianyKorekta, IdZmiany) is not null
left join PlanUrlopowPomin X on X.IdPracownika = P.Id and @do between X.Od and ISNULL(X.Do, '20990909')
where PP.Id is null and P.status in (0,1) and P.KadryId between 0 and 80000-1
and X.Id is null
and R.IdKierownika is not null
and PR.RcpAlgorytm != 0
--order by Opis,Kierownik, Pracownik
--------------------
order by KierownikNI                
        ">
    </asp:SqlDataSource>



<%--
declare @od datetime
declare @do datetime
set @do = '{0}'
set @od = dbo.bom(@do)

select distinct R.IdKierownika as Id, ISNULL(K.Nazwisko + ' ' + K.Imie, ' Poziom główny struktury') as KierownikNI, K.Mailing, K.Email, K.Status  
from Przypisania R
left join Pracownicy K on K.Id = R.IdKierownika
left join PlanPracy PP on PP.IdPracownika = R.IdPracownika and PP.Data between dbo.MaxDate2(@od, R.Od) and dbo.MinDate2(@do, ISNULL(R.Do, '20990909'))
where R.Od &lt;= @do and @od &lt;= ISNULL(R.Do,'20990909') and R.Status = 1 and R.IdPracownika in (
	select distinct IdPracownika from PlanPracy where Data between @od and @do
)
and ISNULL(PP.Akceptacja, 0) = 0
order by KierownikNI                
--%>









    <asp:UpdateProgress ID="updProgress1" runat="server" 
        AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="10">
        <ProgressTemplate>
            <div class="updProgress1">
                <div class="center">
                    <img alt="Indicator" src="images/activity.gif" /> 
                    <span>Trwa przetwarzanie. Proszę czekać ...</span>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:ModalPopupExtender ID="updProgressBlocker" runat="server" 
        TargetControlID="updProgress1" 
        BackgroundCssClass="updProgress1back" 
        PopupControlID="updProgress1">
    </asp:ModalPopupExtender>

    <script type="text/javascript" language="javascript">
        /*
        var ModalProgress = '< % = updProgressBlocker.ClientID %>';

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginReq);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);

        function beginReq(sender, args) {    // shows the Popup
            $find(ModalProgress).show();
        }

        function endReq(sender, args) {     //  shows the Popup
            $find(ModalProgress).hide();
        } 
        */    
    </script>     
</asp:Content>

