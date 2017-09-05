<%@ Page Title="" ValidateRequest="false" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="KierownikForm.aspx.cs" Inherits="HRRcp.KierownikForm" %>
<%@ Register src="~/Controls/PlanPracyZmiany.ascx" tagname="PlanPracyZmiany" tagprefix="uc1" %>
<%@ Register src="~/Controls/PlanPracyAccept.ascx" tagname="PlanPracyAccept" tagprefix="uc1" %>
<%@ Register src="~/Controls/PlanPracyRozliczenie.ascx" tagname="PlanPracyRozliczenie" tagprefix="uc1" %>
<%@ Register src="~/Controls/SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc1" %>
<%@ Register src="~/Controls/RcpControl.ascx" tagname="RcpControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/StrukturaControl.ascx" tagname="StrukturaControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/KierParamsControl.ascx" tagname="KierParams" tagprefix="uc2" %>
<%@ Register src="~/Controls/KierZastepstwa.ascx" tagname="KierZastepstwa" tagprefix="uc2" %>
<%@ Register src="~/Controls/Przypisania/cntPrzesunieciaKier.ascx" tagname="cntPrzesunieciaKier" tagprefix="uc3" %>
<%@ Register src="~/Controls/Portal/paWnioskiUrlopoweKier.ascx" tagname="paWnioskiUrlopoweKier" tagprefix="uc1" %>
<%@ Register src="~/Controls/PlanUrlopow/cntPlanUrlopow.ascx" tagname="cntPlanUrlopow" tagprefix="uc1" %>
<%@ Register src="~/Controls/PodzialLudzi/cntPodzialLudziKier.ascx" tagname="cntPodzialLudziKier" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/print.css" rel="stylesheet" media="print" type="text/css" runat="server" id="headPrint" visible="false"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="caption">
        <tr>
            <td>
                <span class="caption4">
                    <img alt="" src="images/captions/PracNotes.png"/>
                    Panel kierownika
                </span>
            </td>
            <td class="btprint_top" align="right">
                <%--
                <asp:Button ID="btRepPrint1" class="button75" runat="server" Text="Drukuj" OnClientClick="printPreview();return false;" Visible="false"/>
                <asp:Button ID="btRepBack1" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" Visible="false" />
                <asp:Button ID="btRepExcel1" class="button75" runat="server" Text="Excel" OnClientClick="return exportExcelClick();" onclick="btExcel_Click" Visible="false" />
                --%>
            </td>
        </tr>
    </table>     
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Menu ID="tabKierownik" runat="server" Orientation="Horizontal" 
                onmenuitemclick="tabKierownik_MenuItemClick" >
                <StaticMenuStyle CssClass="tabsStrip" />
                <StaticMenuItemStyle CssClass="tabItem" />
                <StaticSelectedStyle CssClass="tabSelected" />
                <StaticHoverStyle CssClass="tabHover" />
                <Items>
                    <asp:MenuItem Text="Plan pracy"             Value="pgPlanPracy|HKPP" Selected="True" ></asp:MenuItem>
                    <asp:MenuItem Text="Akceptacja czasu pracy" Value="pgAkceptacjaCzasu|HKACCPP"></asp:MenuItem>
                    <asp:MenuItem Text="Rozliczanie nadgodzin"  Value="pgRozliczanieNadgodzin|HKROZLNADG"></asp:MenuItem>
                    <asp:MenuItem Text="Pracownicy"             Value="pgStruktura|HKPRAC"></asp:MenuItem>
                    <asp:MenuItem Text="Przesunięcia"           Value="vPrzesuniecia|HDELEG"></asp:MenuItem>
                    <asp:MenuItem Text="Plan urlopów"           Value="pgPlanUrlopow|HKPLANU"></asp:MenuItem>
                    <asp:MenuItem Text="Wnioski urlopowe"       Value="vWnioskiUrlopowe|HWNURLOP"></asp:MenuItem>
                    <asp:MenuItem Text="Podział Ludzi"          Value="vPodzialLudzi|HPODZIALLUDZI"></asp:MenuItem>
                    <asp:MenuItem Text="Zastępstwa"             Value="pgZastepstwa|HZASTEP"></asp:MenuItem>
                    <asp:MenuItem Text="Ustawienia"             Value="pgUstawienia|HKPARAMS"></asp:MenuItem>
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

            <div id="admTabsContent" class="tabsContent paKierownika" style="border-collapse:collapse; background-color:#FFF;">
                <asp:MultiView ID="mvKierownik" runat="server" ActiveViewIndex="0">
                    <asp:View ID="pgPlanPracy" runat="server">
                        <div class="padding">
                            <uc1:PlanPracyZmiany ID="PlanPracyZmiany" Adm="0" OnDataSaved="PlanPracyZmiany_DataSaved" runat="server" />
                        </div>
                    </asp:View>

                    <asp:View ID="pgAkceptacjaCzasu" runat="server" onactivate="pgAkceptacjaCzasu_Activate">
                        <div class="padding">
                            <div runat="server" id="divDostepniKierownicy" visible="false">  
                                <span class="t1">Kierownik:</span>
                                <asp:DropDownList ID="ddlKierownicy2" runat="server" AutoPostBack="true" onselectedindexchanged="ddlKierownicy2_SelectedIndexChanged" />
                                <div class="divider_ppacc"></div>
                            </div>
                            <uc1:PlanPracyAccept ID="PlanPracyAccept" Adm="0" runat="server" />
                        </div>
                    </asp:View>
                    
                    <asp:View ID="pgRozliczanieNadgodzin" runat="server" onactivate="pgRozliczanieNadgodzin_Activate">
                        <div class="padding">
                            <div runat="server" id="div1" visible="false">  
                                <span class="t1">Kierownik:</span>
                                <asp:DropDownList ID="ddlKierownicy3" runat="server" AutoPostBack="true" onselectedindexchanged="ddlKierownicy3_SelectedIndexChanged" />
                                <div class="divider_ppacc"></div>
                            </div>
                            <uc1:PlanPracyRozliczenie ID="PlanPracyRozliczenie" runat="server" />
                        </div>
                    </asp:View>

                    <asp:View ID="pgPlanUrlopow" runat="server" onactivate="pgPlanUrlopow_Activate">
                        <div class="padding">
                            <div runat="server" id="div2" visible="false">  
                                <span class="t1">Kierownik:</span>
                                <asp:DropDownList ID="ddlKierownicy4" runat="server" AutoPostBack="true" onselectedindexchanged="ddlKierownicy4_SelectedIndexChanged" />
                                <div class="divider_ppacc"></div>
                            </div>
                            <uc1:cntPlanUrlopow ID="cntPlanUrlopow" Mode="0" runat="server" />
                        </div>
                    </asp:View>

                    <asp:View ID="pgStruktura" runat="server" onactivate="pgStruktura_Activate">
                        <div class="padding">
                            <table id="tbAdmStruktura" class="table0">
                                <tr>
                                    <td class="tdLeft">
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
                                    <td class="tdRight">
                                        <span class="t5">Dane RCP pracownika:</span> 
                                        <asp:Label ID="lbPracName" runat="server" Text="Proszę wybrać pracownika..."></asp:Label>
                                        <table class="okres_navigator okres_navigator_struktura">
                                            <tr>
                                                <td class="col1">
                                                    <uc1:SelectOkres ID="SelectOkresRCP" ControlID="cntRcp" runat="server" />
                                                </td>
                                                <td class="col2">
                                                    <asp:Button ID="btHide1" class="button" runat="server" Text="Ukryj szczegóły" />
                                                </td>
                                            </tr>
                                        </table>
                                        <uc1:RcpControl ID="cntRcp" StrefaSelect="false" RoundSelect="false" RoundTypeSelect="false" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:View>

                    <asp:View ID="pgUstawienia" runat="server">
                        <div class="padding">
                            <uc2:KierParams ID="cntKierParams" OnChanged="cntKierParams_Changed" runat="server" />
                        </div>
                    </asp:View>

                    <asp:View ID="pgZastepstwa" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <uc2:KierZastepstwa ID="cntZastepstwa" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:View>

                    <asp:View ID="vPrzesuniecia" runat="server" OnActivate="pgPrzesuniecia_Activate">
                        <uc3:cntPrzesunieciaKier ID="cntPrzesunieciaKier1" runat="server" />
                    </asp:View>

                    <asp:View ID="vWnioskiUrlopowe" runat="server" OnActivate="vWnioskiUrlopowe_Activate">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" >
                            <ContentTemplate>
                                <uc1:paWnioskiUrlopoweKier ID="paWnioskiUrlopowe" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:View>
                    
                    <asp:View ID="vPodzialLudzi" runat="server" OnActivate="pgPodzialLudzi_Activate">
                        <uc3:cntPodzialLudziKier ID="cntPodzialLudziKier" runat="server" />
                    </asp:View>

                </asp:MultiView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
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
</asp:Content>
