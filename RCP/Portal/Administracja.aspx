<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="Administracja.aspx.cs" Inherits="HRRcp.Portal.PortalAdmin" ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/PracownicyList.ascx" TagName="PracownicyList" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/AdmZastepstwa.ascx" TagName="AdmZastepstwa" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Portal/paWnioskiUrlopoweAdm.ascx" TagName="paWnioskiUrlopoweAdm" TagPrefix="uc1" %>

<%@ Register Src="~/Controls/Adm/cntPracownicy3portal.ascx" TagName="cntPracownicy2" TagPrefix="uc1" %>

<%@ Register Src="~/Controls/Portal/cntPlikiAdm.ascx" TagName="cntPlikiAdm" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Portal/cntArticles3.ascx" TagName="cntArticles" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Portal/cntGazetkaAdm.ascx" TagName="cntGazetkaAdm" TagPrefix="uc1" %>

<%@ Register Src="~/Controls/AdmLogControl.ascx" TagName="AdmLogControl" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/AdmInfoControl.ascx" TagName="AdmInfoControl" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Mails/cntMailsAdm.ascx" TagName="cntMailsAdm" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Mails/cntSchedulerAdm.ascx" TagName="cntSchedulerAdm" TagPrefix="uc3" %>

<%--
<%@ Register Src="~/Controls/Portal/WniosekZmianaDanych/Wnioski_Dane_Osobowe_Adm.ascx" TagName="paWnioskiDaneOsobowe" TagPrefix="muc1" %>
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <table class="caption" >
        <tr>
            <td>
                <span class="caption4">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/captions/Struktura.png"/>
                    Administracja
                </span>
            </td>
            <td align="right">
            </td>
        </tr>
    </table>     --%>

    <%--<h1>Administracja</h1><hr />--%>
    <div class="page-title">
        <i class="fa fa-cog"></i>
        Administracja
    </div>
    <div class="container wide">




        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table class="tabsContent">
                    <tr>
                        <td class="LeftMenu">
                            <asp:Menu ID="tabAdmin" runat="server" Orientation="Vertical"
                                OnMenuItemClick="tabAdmin_MenuItemClick">
                                <%--<StaticMenuStyle CssClass="tabsStrip" />
                                <StaticMenuItemStyle CssClass="tabItem" />
                                <StaticSelectedStyle CssClass="tabSelected" />
                                <StaticHoverStyle CssClass="tabHover" />--%>
                                       <StaticMenuStyle CssClass="menu" />
                <StaticSelectedStyle CssClass="selected" />
                <StaticMenuItemStyle CssClass="item" />
                <StaticHoverStyle CssClass="hover" />
                                <Items>
                                    <asp:MenuItem Text="Pracownicy" Value="pgPracownicy"></asp:MenuItem>
                                    <asp:MenuItem Text="Wnioski urlopowe" Value="pgWnioskiUrlopowe"></asp:MenuItem>
                                    <asp:MenuItem Text="Zastępstwa" Value="pgZastepstwa"></asp:MenuItem>
                                    <%--
                    <asp:MenuItem Text="Parametry" Value="pgParams"></asp:MenuItem>
                                    --%>
                                    <asp:MenuItem Text="Pliki" Value="pgPliki"></asp:MenuItem>
                                    <asp:MenuItem Text="Artykuły" Value="pgArtykuly"></asp:MenuItem>
                                    <asp:MenuItem Text="Newsletter" Value="pgGazetka"></asp:MenuItem>

                                    <asp:MenuItem Text="Teksty" Value="pgTeksty"></asp:MenuItem>
                                    <asp:MenuItem Text="Powiadomienia mailowe" Value="pgMails"></asp:MenuItem>
                                    <asp:MenuItem Text="Scheduler" Value="pgScheduler"></asp:MenuItem>
                                    <asp:MenuItem Text="Podgląd zdarzeń" Value="pgLog" Selected="true"></asp:MenuItem>


                                    <asp:MenuItem Text="Wnioski Zmiana Danych" Value="pgWnioskiZmianaDanych" Selected="true"></asp:MenuItem>
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
                                OnTick="ProgressTimer_Tick">
                            </asp:Timer>

                        </td>

                        <td class="LeftMenuContent">

                            <div id="admTabsContent" class="xtabsContent paAdmin" style="background-color: #FFF;">
                                <asp:MultiView ID="mvAdministracja" runat="server" ActiveViewIndex="0">

                                    <asp:View ID="pgPracownicy" runat="server">
                                        <div class="xpadding" style="xpadding: 24px;">
                                            <uc1:cntPracownicy2 ID="cntPracownicy2" OnSelectedChanged="cntPracownicy2_SelectedChanged" OnCommand="cntPracownicy2_Command" runat="server" />
                                        </div>
                                    </asp:View>

                                    <asp:View ID="pgParams" runat="server">
                                    </asp:View>

                                    <asp:View ID="pgZastepstwa" runat="server" OnActivate="pgZastepstwa_Activate">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <uc1:AdmZastepstwa ID="cntZastepstwa" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:View>

                                    <asp:View ID="pgWnioskiUrlopowe" runat="server" OnActivate="pgWnioskiUrlopowe_Activate">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <uc1:paWnioskiUrlopoweAdm ID="paWnioskiUrlopowe" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:View>


                                    <asp:View ID="pgWnioskiDaneOsobowe" runat="server">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <%--
                                <muc1:paWnioskiDaneOsobowe ID="paWnioskiDaneOsobowe1" runat="server" />
                                                --%>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:View>


                                    <asp:View ID="pgPliki" runat="server" OnActivate="pgPliki_Activate">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <uc1:cntPlikiAdm ID="cntPlikiAdm" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:View>

                                    <asp:View ID="pgArtykuly" runat="server" OnActivate="pgArtykuly_Activate">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <uc1:cntArticles ID="cntArticles" runat="server" Mode="1" Grupa="ARTYKULY" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:View>

                                    <asp:View ID="pgGazetka" runat="server" OnActivate="pgGazetka_Activate">
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <uc1:cntGazetkaAdm ID="cntGazetkaAdm" runat="server" Mode="1" Grupa="GAZETKA" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:View>





                                    <asp:View ID="pgLog" runat="server">
                                        <div class="padding">
                                            <uc1:AdmLogControl ID="AdmLogControl" runat="server" />
                                        </div>
                                    </asp:View>

                                    <asp:View ID="pgTeksty" runat="server">
                                        <div class="padding">
                                            <uc3:AdmInfoControl ID="AdmInfoControl" runat="server" />
                                            <div class="bottom_buttons">
                                                <span>Wybierz plik do importu:&nbsp;&nbsp;
                                                </span>
                                                <asp:FileUpload ID="FileUpload1" Width="500px" CssClass="fileupload" runat="server" />
                                                <asp:Button ID="btImportTeksty" runat="server" CssClass="button100 btn btn-default" Text="Import" OnClick="btImportTeksty_Click" OnClientClick="javascript:return checkFile();" />
                                                <asp:Button ID="btExportTeksty" runat="server" CssClass="button100 btn btn-default" Text="Eksport" OnClick="btExportTeksty_Click" />
                                            </div>
                                        </div>
                                    </asp:View>

                                    <asp:View ID="pgMails" runat="server">
                                        <div class="padding">
                                            <uc3:cntMailsAdm ID="cntMailsAdm" runat="server" />
                                        </div>
                                    </asp:View>

                                    <asp:View ID="pgScheduler" runat="server">
                                        <div class="padding">
                                            <uc3:cntSchedulerAdm ID="cntSchedulerAdm" runat="server" />
                                        </div>
                                    </asp:View>




                                    <asp:View ID="pgWnioskiZmianaDanych" runat="server">
                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <%--
                                <muc1:paWnioskiDaneOsobowe ID="paWnioskiDaneOsobowe2" runat="server" />
                                                --%>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:View>

                                </asp:MultiView>
                            </div>

                        </td>
                    </tr>
                </table>


            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btExportTeksty" />
                <asp:PostBackTrigger ControlID="btImportTeksty" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <%-- <asp:UpdateProgress ID="updProgress1" runat="server" 
        AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="10">
        <ProgressTemplate>
            <div class="updProgress1">
                <div class="center">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/images/activity.gif"/>
                    <span>Trwa przetwarzanie. Proszę czekać ...</span>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>

    <asp:ModalPopupExtender ID="updProgressBlocker" runat="server"
        TargetControlID="updProgress1"
        BackgroundCssClass="updProgress1back"
        PopupControlID="updProgress1">
    </asp:ModalPopupExtender>

</asp:Content>
