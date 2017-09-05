<%@ Page Title="" Language="C#" MasterPageFile="~/IPO/IPO.Master" AutoEventWireup="true"
    CodeBehind="IPO.aspx.cs" Inherits="HRRcp.IPO.IPO" ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/IPO/Controls/cntZamowienia.ascx" TagName="cntZamowienia" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
    <style type="text/css">
        table.portal td.content div.paAdmin { width: auto; }
        .selectedRole 
        {
            background-color: #00ff00;
        }
        .caption input 
        {
            width: 100px;
            height: 25px;
        }
    </style>
    <table class="caption">
        <tr>
            <td>
                <span class="caption4">
                    <img alt="" src="/IPO/images/IPO_icon.png" />
                    iPO </span>
            </td>
            <td align="right">
                <asp:HiddenField ID="selectedRole" runat="server" Value="0" />
                <asp:Button ID="zamawiajacyButton" runat="server" Text="Zamawiający" OnClick="zamawiajacy_OnClick"/>
                <asp:Button ID="akceptujacyButton" runat="server" Text="Akceptujący" OnClick="akceptujacy_OnClick"/>
                <asp:Button ID="kupiecButton" runat="server" Text="Kupiec" OnClick="kupiec_OnClick"/>
                <asp:Button ID="notyfikowanyButton" runat="server" Text="Notyfikowany" OnClick="notyfikowany_OnClick"/>
                <asp:Button ID="administratorButton" runat="server" Text="Administrator" OnClick="administrator_OnClick"/>
                
            </td>
        </tr>
    </table>
            <asp:Menu ID="zamawiajacyMenu" runat="server" Orientation="Horizontal" OnMenuItemClick="tabAdmin_MenuItemClick"
                Visible="false">
                <StaticMenuStyle CssClass="tabsStrip" />
                <StaticMenuItemStyle CssClass="tabItem" />
                <StaticSelectedStyle CssClass="tabSelected" />
                <StaticHoverStyle CssClass="tabHover" />
                <StaticItemTemplate>
                    <div class="tabCaption">
                        <div class="tabLeft">
                            <div class="tabRight">
                                <asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Text") %>' />
                            </div>
                        </div>
                    </div>
                </StaticItemTemplate>
                <Items>
                    <asp:MenuItem Text="W przygotowaniu" Value="zamawiajacy1" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="Akceptacje" Value="zamawiajacy2"></asp:MenuItem>
                    <asp:MenuItem Text="Do realizacji" Value="zamawiajacy3"></asp:MenuItem>
                    <asp:MenuItem Text="Do odbioru" Value="zamawiajacy4"></asp:MenuItem>
                    <asp:MenuItem Text="Zamknięte" Value="zamawiajacy5"></asp:MenuItem>
                    <asp:MenuItem Text="Odrzucone" Value="zamawiajacy6"></asp:MenuItem>
                </Items>
            </asp:Menu>
            <asp:Menu ID="akceptujacyMenu" runat="server" Orientation="Horizontal" OnMenuItemClick="tabAdmin_MenuItemClick"
                Visible="false">
                <StaticMenuStyle CssClass="tabsStrip" />
                <StaticMenuItemStyle CssClass="tabItem" />
                <StaticSelectedStyle CssClass="tabSelected" />
                <StaticHoverStyle CssClass="tabHover" />
                <StaticItemTemplate>
                    <div class="tabCaption">
                        <div class="tabLeft">
                            <div class="tabRight">
                                <asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Text") %>' />
                            </div>
                        </div>
                    </div>
                </StaticItemTemplate>
                <Items>
                    <asp:MenuItem Text="Do zaakceptowania" Value="akceptujacy1" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="Zaakceptowane" Value="akceptujacy2"></asp:MenuItem>
                    <asp:MenuItem Text="Zamknięte" Value="akceptujacy3"></asp:MenuItem>
                    <asp:MenuItem Text="Odrzucone" Value="akceptujacy4"></asp:MenuItem>
                </Items>
            </asp:Menu>
            <asp:Menu ID="kupiecMenu" runat="server" Orientation="Horizontal" OnMenuItemClick="tabAdmin_MenuItemClick"
                Visible="false">
                <StaticMenuStyle CssClass="tabsStrip" />
                <StaticMenuItemStyle CssClass="tabItem" />
                <StaticSelectedStyle CssClass="tabSelected" />
                <StaticHoverStyle CssClass="tabHover" />
                <StaticItemTemplate>
                    <div class="tabCaption">
                        <div class="tabLeft">
                            <div class="tabRight">
                                <asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Text") %>' />
                            </div>
                        </div>
                    </div>
                </StaticItemTemplate>
                <Items>
                    <asp:MenuItem Text="W przygotowaniu" Value="kupiec1" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="Do akceptacji cen" Value="kupiec2"></asp:MenuItem>
                    <asp:MenuItem Text="Do realizacji" Value="kupiec3"></asp:MenuItem>
                    <asp:MenuItem Text="Do odbioru" Value="kupiec4"></asp:MenuItem>
                    <asp:MenuItem Text="Zamknięte" Value="kupiec5"></asp:MenuItem>
                    <asp:MenuItem Text="Odrzucone" Value="kupiec6"></asp:MenuItem>
                </Items>
            </asp:Menu>
            <asp:Menu ID="notyfikowanyMenu" runat="server" Orientation="Horizontal" OnMenuItemClick="tabAdmin_MenuItemClick"
                Visible="false">
                <StaticMenuStyle CssClass="tabsStrip" />
                <StaticMenuItemStyle CssClass="tabItem" />
                <StaticSelectedStyle CssClass="tabSelected" />
                <StaticHoverStyle CssClass="tabHover" />
                <StaticItemTemplate>
                    <div class="tabCaption">
                        <div class="tabLeft">
                            <div class="tabRight">
                                <asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Text") %>' />
                            </div>
                        </div>
                    </div>
                </StaticItemTemplate>
                <Items>
                    <asp:MenuItem Text="Akceptacje" Value="notyfikowany1" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="Do realizacji" Value="notyfikowany2"></asp:MenuItem>
                    <asp:MenuItem Text="Do odbioru" Value="notyfikowany3"></asp:MenuItem>
                    <asp:MenuItem Text="Zamknięte" Value="notyfikowany4"></asp:MenuItem>
                    <asp:MenuItem Text="Odrzucone" Value="notyfikowany5"></asp:MenuItem>
                </Items>
            </asp:Menu>
            <asp:Menu ID="administratorMenu" runat="server" Orientation="Horizontal" OnMenuItemClick="tabAdmin_MenuItemClick"
                Visible="false">
                <StaticMenuStyle CssClass="tabsStrip" />
                <StaticMenuItemStyle CssClass="tabItem" />
                <StaticSelectedStyle CssClass="tabSelected" />
                <StaticHoverStyle CssClass="tabHover" />
                <StaticItemTemplate>
                    <div class="tabCaption">
                        <div class="tabLeft">
                            <div class="tabRight">
                                <asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Text") %>' />
                            </div>
                        </div>
                    </div>
                </StaticItemTemplate>
                <Items>
                    <asp:MenuItem Text="W przygotowaniu" Value="administrator1" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="W akceptacji" Value="administrator2"></asp:MenuItem>
                    <asp:MenuItem Text="Do realizacji" Value="administrator3"></asp:MenuItem>
                    <asp:MenuItem Text="W trakcie realizacji" Value="administrator4"></asp:MenuItem>
                    <asp:MenuItem Text="Do odbioru" Value="administrator5"></asp:MenuItem>
                    <asp:MenuItem Text="Zamknięte" Value="administrator6"></asp:MenuItem>
                    <asp:MenuItem Text="Odrzucone" Value="administrator7"></asp:MenuItem>
                </Items>
            </asp:Menu>
            <div id="admTabsContent" class="tabsContent paAdmin" style="background-color: #FFF;">
                <asp:MultiView ID="zakladkiMultiView" runat="server" ActiveViewIndex="0">
                    <asp:View ID="zamawiajacy1" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia1" Zakladka="1" Rola="1" Edycja="true" Dodawanie="true" Wysylanie="true"
                            runat="server" />
                    </asp:View>
                    <asp:View ID="zamawiajacy2" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia2" Zakladka="2" Rola="1" Edycja="false" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="zamawiajacy3" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia3" Zakladka="3" Rola="1" Edycja="false" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="zamawiajacy4" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia4" Zakladka="4" Rola="1" Edycja="false" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="zamawiajacy5" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia5" Zakladka="5" Rola="1" Edycja="false" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="zamawiajacy6" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia6" Zakladka="6" Rola="1" Edycja="false" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="akceptujacy1" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia7" Zakladka="1" Rola="2" Edycja="false" Dodawanie="false" Wysylanie="false" Akceptacja="true" runat="server" />
                    </asp:View>
                    <asp:View ID="akceptujacy2" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia8" Zakladka="2" Rola="2" Edycja="false" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="akceptujacy3" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia9" Zakladka="3" Rola="2" Edycja="false" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="akceptujacy4" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia10" Zakladka="4" Rola="2" Edycja="false" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="kupiec1" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia11" Zakladka="1" Rola="3" Edycja="false" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="kupiec2" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia12" Zakladka="2" Rola="3" Edycja="true"  Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="kupiec3" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia13" Zakladka="3" Rola="3" Edycja="true" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="kupiec4" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia14" Zakladka="4" Rola="3" Edycja="true" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="kupiec5" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia15" Zakladka="5" Rola="3" Edycja="false" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="kupiec6" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia16" Zakladka="6" Rola="3" Edycja="false" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="notyfikowany1" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia17" Zakladka="1" Rola="4" Edycja="false" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="notyfikowany2" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia18" Zakladka="2" Rola="4" Edycja="false" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="notyfikowany3" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia19" Zakladka="3" Rola="4" Edycja="false" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="notyfikowany4" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia20" Zakladka="4" Rola="4" Edycja="false" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="notyfikowany5" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia21" Zakladka="5" Rola="4" Edycja="false" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="administrator1" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia22" Zakladka="1" Rola="5" Edycja="true" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="administrator2" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia23" Zakladka="2" Rola="5" Edycja="true" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="administrator3" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia24" Zakladka="3" Rola="5" Edycja="true" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="administrator4" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia25" Zakladka="4" Rola="5" Edycja="true" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="administrator5" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia26" Zakladka="5" Rola="5" Edycja="true" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="administrator6" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia27" Zakladka="6" Rola="5" Edycja="true" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                    <asp:View ID="administrator7" runat="server" OnActivate="TabOnActivate">
                        <uc1:cntZamowienia ID="CntZamowienia28" Zakladka="7" Rola="5" Edycja="true" Dodawanie="false" Wysylanie="false" Akceptacja="false" runat="server" />
                    </asp:View>
                </asp:MultiView>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="updProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
        DisplayAfter="10">
        <ProgressTemplate>
            <div class="updProgress1">
                <div class="center">
                    <img alt="Indicator" src="../../images/activity.gif" />
                    <span>Trwa przetwarzanie. Proszę czekać ...</span>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:ModalPopupExtender ID="updProgressBlocker" runat="server" TargetControlID="updProgress1"
        BackgroundCssClass="updProgress1back" PopupControlID="updProgress1">
    </asp:ModalPopupExtender>
</asp:Content>
