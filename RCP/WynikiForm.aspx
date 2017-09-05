<%@ Page Title="" ValidateRequest="false" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WynikiForm.aspx.cs" Inherits="HRRcp.WynikiForm" %>
<%@ Register src="~/Controls/RepPodsumowanie.ascx" tagname="RepPodsumowanie" tagprefix="uc1" %>
<%@ Register src="~/Controls/RepHeader.ascx" tagname="RepHeader" tagprefix="uc1" %>
<%@ Register src="~/Controls/RepKartaRoczna.ascx" tagname="RepKartaRoczna" tagprefix="uc1" %>
<%@ Register src="~/Controls/SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc1" %>



<%@ Register src="~/Controls/RepNadgodziny3.ascx" tagname="RepNadgodziny" tagprefix="uc1" %>



<%@ Register src="~/Controls/AbsencjaLegenda.ascx" tagname="AbsencjaLegenda" tagprefix="uc2" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc2" %>
<%@ Register src="~/Controls/RepUrlop.ascx" tagname="RepUrlopy" tagprefix="uc1" %>



<%@ Register src="~/Controls/RepCzasPracy2.ascx" tagname="RepCzasPracy" tagprefix="uc1" %>



<%@ Register src="~/Controls/RepPodzialCC.ascx" tagname="RepPodzialCC" tagprefix="uc1" %>
<%@ Register src="~/Controls/Raporty/RepStolowka.ascx" tagname="RepStolowka" tagprefix="uc1" %>
<%@ Register src="~/Controls/Raporty/RepSpoznienia.ascx" tagname="RepSpoznienia" tagprefix="uc1" %>
<%@ Register src="~/Controls/Raporty/RepRozlNadg.ascx" tagname="RepRozlNadg" tagprefix="uc1" %>
<%@ Register src="~/Controls/Raporty/repPlanUrlopow.ascx" tagname="repPlanUrlopow" tagprefix="uc1" %>
<%@ Register src="~/Controls/Raporty/repRezerwaUrlopowa.ascx" tagname="repRezerwaUrlopowa" tagprefix="uc1" %>
<%@ Register src="~/Controls/Raporty/repESD.ascx" tagname="repESD" tagprefix="uc1" %>



<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/print.css" rel="stylesheet" media="print" type="text/css" />
    <script type="text/javascript">
        function exportExcelClick() {
            return exportExcel('<%=hidReport.ClientID%>');
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="pgWynikiForm">
    <asp:HiddenField ID="hidReport" runat="server" />

    <table class="caption printoff" >
        <tr>
            <td>
                <span class="caption4">
                    <img alt="" src="images/captions/AnkietaView.png"/>
                    Raporty i zestawienia
                </span>
            </td>
            <td class="btprint_top" align="right">
                <asp:Button ID="btRepBack1" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" />
                <asp:Button ID="btRepPrint1" class="button75" runat="server" Text="Drukuj" OnClientClick="printPreview();return false;" />
                <asp:Button ID="btRepExcel1" class="button75" runat="server" Text="Excel" OnClientClick="return exportExcelClick();" onclick="btExcel_Click" />
            </td>
        </tr>
    </table>     

    <%--
    <br />&nbsp;&nbsp;&nbsp;&nbsp;
    <a href="http://localhost:50675/Reports/RepCCSplity.aspx?p1=20130401&p2=20130430">Splity test</a>
    <br />
    <br />
    --%>
    
    
    <asp:Menu ID="tabWyniki" CssClass="printoff" runat="server" Orientation="Horizontal" 
        onmenuitemclick="tabWyniki_MenuItemClick" >
        <StaticMenuStyle CssClass="tabsStrip" />
        <StaticMenuItemStyle CssClass="tabItem" />
        <StaticSelectedStyle CssClass="tabSelected" />
        <StaticHoverStyle CssClass="tabHover" />
        <Items>
            <asp:MenuItem Text="Nadgodziny" Value="0" Selected="True" ></asp:MenuItem>
            <asp:MenuItem Text="Nadgodziny - okres" Value="4" ></asp:MenuItem>
            <asp:MenuItem Text="Nadgodziny - projekty" Value="1" ></asp:MenuItem>
            <asp:MenuItem Text="Rozliczenie nadgodzin" Value="9" ></asp:MenuItem>
            <asp:MenuItem Text="Roczna Karta Pracy" Value="2" ></asp:MenuItem>
            <asp:MenuItem Text="Spóźnienia" Value="8" ></asp:MenuItem>
            <asp:MenuItem Text="ESD" Value="11" ></asp:MenuItem>
            <asp:MenuItem Text="Urlopy" Value="3" ></asp:MenuItem>
            <asp:MenuItem Text="Plan Urlopów" Value="10" ></asp:MenuItem>
            <asp:MenuItem Text="Rezerwa urlopowa" Value="12" ></asp:MenuItem>
            <asp:MenuItem Text="Czas pracy" Value="5" ></asp:MenuItem>
            <asp:MenuItem Text="Stołówka" Value="7" ></asp:MenuItem>
            <asp:MenuItem Text="Podział na cc" Value="6" ></asp:MenuItem>
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

    <div class="tabsContent" style="border-collapse:collapse; background-color:#FFF;">
        <asp:MultiView ID="mvWyniki" runat="server" ActiveViewIndex="0">
            <asp:View ID="pgNadgodzinyKier" runat="server" onactivate="pgNadgodziny_Activate" >
                <table class="okres_navigator printoff">
                    <tr>
                        <td class="colleft">
                            <span class="t1">Kierownik:</span>                            
                            <asp:DropDownList ID="ddlKierownicy" runat="server" AutoPostBack="true" onselectedindexchanged="ddlKierownicy_SelectedIndexChanged" />
                        </td>
                        <td class="colmiddle">
                            <uc1:SelectOkres ID="cntSelectOkres" ControlID="repNadgodziny" OnOkresChanged="cntSelectOkres_Changed" runat="server" />
                        </td>
                        <td class="colright">
                        </td>
                    </tr>
                </table>
                <div class="divider_ppacc printoff"></div>
                <uc1:RepHeader ID="repHeader1" Caption="Nadgodziny za miesiąc ........" Icon="1" runat="server" />
                <uc1:RepNadgodziny ID="repNadgodziny" Mode="3" CCMode="1" runat="server" />
            </asp:View>
            
            <asp:View ID="pgNadgodziny7" runat="server" >
                <table class="okres_navigator printoff">
                    <tr>
                        <td class="colleft">
                            <span class="t1">Kierownik:</span>                            
                            <asp:DropDownList ID="ddlKierownicy7" runat="server" AutoPostBack="true" onselectedindexchanged="ddlKierownicy7_SelectedIndexChanged" />
                        </td>
                        <td class="colmiddle">
                            <span>Okres od:</span> 
                            <uc2:DateEdit ID="deOd" runat="server" />&nbsp;&nbsp;&nbsp;
                            <span>do:</span>
                            <uc2:DateEdit ID="deDo" runat="server" />&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btExec7" runat="server" CssClass="button75" Text="Wykonaj" onclick="btExec7_Click" />
                        </td>
                        <td class="colright">
                            <asp:Button ID="btExecCC" runat="server" CssClass="button75" Text="Podział na CC" visible="false" onclick="btExecCC_Click" />
                        </td>
                    </tr>
                </table>
                <div class="divider_ppacc printoff"></div>              
                <uc1:RepHeader ID="repHeader4" Icon="2" Caption="Nadgodziny w okresie od ........ do ........." runat="server" />
                <uc1:RepNadgodziny ID="repNadgodziny7" Mode="3" runat="server" />
            </asp:View>
            
            
            <asp:View ID="pgNadgodzinyProj" runat="server" >
                <table class="okres_navigator printoff">
                    <tr>
                        <td class="colleft">
                            <span class="t1">Projekt:</span>                            
                            <asp:DropDownList ID="ddlProjekty" runat="server" AutoPostBack="true" onselectedindexchanged="ddlProjekty_SelectedIndexChanged" />
                        </td>
                        <td class="colmiddle">
                            <uc1:SelectOkres ID="cntSelectOkresProj" ControlID="repNadgodzinyProj" OnOkresChanged="cntSelectOkresProj_Changed" runat="server" />
                        </td>
                        <td class="colright">
                        </td>
                    </tr>
                </table>
                <div class="divider_ppacc printoff"></div>              
                <uc1:RepHeader ID="repHeader5" Icon="2" Caption="Nadgodziny na projekt" runat="server" />
                <uc1:RepNadgodziny ID="repNadgodzinyProj" Mode="4" runat="server" />



                <%--
                <span class="t1">Rok: </span>
                <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="true" onselectedindexchanged="ddlRok_SelectedIndexChanged"/>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                
                <span class="t1">KierownikPracownik:</span>
                <asp:DropDownList ID="DropDownList4" runat="server" AutoPostBack="true" onselectedindexchanged="ddlPracownicy_SelectedIndexChanged"/>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                
                <asp:Button ID="Button2" runat="server" CssClass="button100" Text="Wykonaj" Visible="false" onclick="btExecute_Click" />

                --%>
            </asp:View>
            <asp:View ID="pgKartaPracy" runat="server" onactivate="pgKartaPracy_Activate" >
                <div class="rep_navigator printoff">
                    <span class="t1">Rok: </span>
                    <asp:DropDownList ID="ddlRok" runat="server" AutoPostBack="true" onselectedindexchanged="ddlRok_SelectedIndexChanged"/>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    
                    <span class="t1">Pracownik:</span>
                    <asp:DropDownList ID="ddlPracownicy" runat="server" AutoPostBack="true" onselectedindexchanged="ddlPracownicy_SelectedIndexChanged"/>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    
                    <asp:Button ID="btExecute" runat="server" CssClass="button100" Text="Wykonaj" Visible="false" onclick="btExecute_Click" />
                </div>
                <div class="divider_ppacc printoff"></div>
                
                <uc1:RepHeader ID="repHeader2" Icon="2" Caption="Karta ewidencji czasu pracy za rok ........" runat="server" />
                <uc1:RepKartaRoczna ID="cntKartaRoczna" runat="server" />
                <uc2:AbsencjaLegenda ID="AbsencjaLegenda" runat="server" />
            </asp:View>

            <asp:View ID="pgUrlopy" runat="server" onactivate="pgNadgodziny_Activate" >
                <table class="okres_navigator printoff">
                    <tr>
                        <td class="colleft">
                            <span class="t1">Kierownik:</span>                            
                            <asp:DropDownList ID="ddlKierUrlopy" runat="server" AutoPostBack="true" onselectedindexchanged="ddlKierUrlopy_SelectedIndexChanged" />
                        </td>
                        <td class="colmiddle">
                            <span>Okres od początku roku</span> 
                            <asp:Label ID="lbUrlopOd" CssClass="dataod" runat="server" ></asp:Label>
                            <span>do:</span>
                            <uc2:DateEdit ID="deUrlopDo" runat="server"  />&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button1" runat="server" CssClass="button75" Text="Wykonaj" onclick="btExecUrlop_Click" />
                        </td>
                        <td class="colright">
                        </td>
                    </tr>
                </table>
                <div class="divider_ppacc printoff"></div>
                
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc1:RepHeader ID="repHeader3" Icon="2" Caption="Wykorzystanie urlopu od ........ do ........." runat="server" />
                        <uc1:RepUrlopy ID="repUrlopy" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                
            </asp:View>

            <asp:View ID="View1" runat="server" >
                <uc1:RepCzasPracy ID="repCzasPracy" runat="server" />
            </asp:View>

            <asp:View ID="vStolowka" runat="server" onactivate="vStolowka_Activate" >
                <uc1:RepStolowka ID="repStolowka" runat="server" />
            </asp:View>

            <asp:View ID="pgPodzialCC" runat="server" 
                onactivate="pgPodzialCC_Activate" 
                ondeactivate="pgPodzialCC_Deactivate" >
                <uc1:RepPodzialCC ID="repPodzialCC" runat="server" />
            </asp:View>

            <asp:View ID="vSpoznienia" runat="server" onactivate="vSpoznienia_Activate" >
                <uc1:RepSpoznienia ID="repSpoznienia" Mode="1" runat="server" />
            </asp:View>

            <asp:View ID="vRozlNadg" runat="server" onactivate="vRozlNadg_Activate" >
                <uc1:RepRozlNadg ID="repRozlNadg" Mode="1" runat="server" />
            </asp:View>

            <asp:View ID="vESD" runat="server" onactivate="vESD_Activate" >
                <uc1:repESD ID="repESD" runat="server" />
            </asp:View>

            <asp:View ID="vPlanUrlopow" runat="server" onactivate="vPlanUrlopow_Activate" >
                <uc1:repPlanUrlopow ID="repPlanUrlopow" runat="server" />
            </asp:View>

            <asp:View ID="vRezerwaUrlopowa" runat="server" onactivate="vRezerwaUrlopowa_Activate" >
                <uc1:repRezerwaUrlopowa ID="repRezerwaUrlopowa" runat="server" />
            </asp:View>

        </asp:MultiView>
    </div>

    <table class="printoff" style="width:100%; border-collapse:collapse; ">
        <tr>
            <td class="btprint_bottom" align="right">
                <asp:Button ID="btRepBack2" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" />
                <asp:Button ID="btRepPrint2" class="button75" runat="server" Text="Drukuj" OnClientClick="printPreview();return false;" />
                <asp:Button ID="btRepExcel2" class="button75" runat="server" Text="Excel" OnClientClick="return exportExcelClick();" onclick="btExcel_Click" />
            </td>
        </tr>
    </table>     

    <div class="print_footer">
        <asp:Label ID="lbPrintFooter" class="left" runat="server" Text="Wydrukowano z systemu Rejestracji Czasu Pracy v."></asp:Label>
        <asp:Label ID="lbPrintVersion" class="left" runat="server" Text="1.0.0.0"></asp:Label>
        <br />
        <asp:Label ID="lbPrintTime" class="left" runat="server" ></asp:Label>
    </div>

    <asp:UpdateProgress ID="updProgress1" runat="server" DisplayAfter="10">
        <ProgressTemplate>
            <div class="updProgress1">
                <div class="center">
                    <img alt="Indicator" src="~/images/activity.gif" runat="server"/> 
                    <span>Trwa przetwarzanie. Proszę czekać ...</span>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>
</asp:Content>
