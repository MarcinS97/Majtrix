<%@ Page Title="" Language="C#" MasterPageFile="~/IPO/IPO.Master" AutoEventWireup="true" CodeBehind="AdministracjaIPO.aspx.cs" Inherits="HRRcp.IPO.IPOAdmin" ValidateRequest="false" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>




<%@ Register src="~/IPO/Controls/cntUprawnienia.ascx" tagname="cntUprawnienia" tagprefix="uc1" %>
<%@ Register src="~/IPO/Controls/cntAkceptujacyAdmin.ascx" tagname="cntAkceptujacyAdmin" tagprefix="uc1" %>
<%@ Register src="~/IPO/Controls/cntKonfig.ascx" tagname="cntKonfig" tagprefix="uc1" %>
<%@ Register src="~/IPO/Controls/cntProdukty.ascx" tagname="cntProdukty" tagprefix="uc1" %>
<%@ Register src="~/IPO/Controls/cntDostawcy.ascx" tagname="cntDostawcy" tagprefix="uc1" %>
<%@ Register src="~/IPO/Controls/cntUprawnieniaMod.ascx" tagname="cntUprawnieniaMod" tagprefix="uc1" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table class="caption" >
        <tr>
            <td>
                <span class="caption4">

                    <asp:Image ID="Image3" runat="server" ImageUrl="~/IPO/images/Struktura.png" /> 
                    <asp:Label ID="Label13" runat="server" Text='Administracja IPO' />
                </span>
            </td>
            <td align="right">
            </td>
        </tr>
    </table>     

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
            <asp:Menu ID="IPOTabAdmin" runat="server" Orientation="Horizontal" 
                onmenuitemclick="IPOtabAdmin_MenuItemClick" >
                <StaticMenuStyle CssClass="tabsStrip" />
                <StaticMenuItemStyle CssClass="tabItem" />
                <StaticSelectedStyle CssClass="tabSelected" />
                <StaticHoverStyle CssClass="tabHover" />
                <Items>
                    <asp:MenuItem Text="Zamawiający" Value="pgZamawiajacy" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="Notyfikowani" Value="pgNotyfikowani"></asp:MenuItem>
                    <asp:MenuItem Text="Kupcy" Value="pgKupcy"></asp:MenuItem>
                    <asp:MenuItem Text="Magazynier" Value="pgMagazynier"></asp:MenuItem>
                    <asp:MenuItem Text="Moderator" Value="pgModerator"></asp:MenuItem> 
                    <asp:MenuItem Text="Akceptujący" Value="pgAkceptujacy"></asp:MenuItem>            
                    <asp:MenuItem Text="Administratorzy" Value="pgAdministratorzy"></asp:MenuItem>
                    <asp:MenuItem Text="Centra Kosztowe" Value="pgCC"></asp:MenuItem>  
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
            
            
             <asp:Menu ID="Config" runat="server" Orientation="Horizontal" 
                onmenuitemclick="IPOtabAdmin_MenuItemClick" >
                <StaticMenuStyle CssClass="tabsStrip" />
                <StaticMenuItemStyle CssClass="tabItem" />
                <StaticSelectedStyle CssClass="tabSelected" />
                <StaticHoverStyle CssClass="tabHover" />
                <Items>
                    <asp:MenuItem Text="Konfiguracja" Value="pgConfig"></asp:MenuItem>
                    <asp:MenuItem Text="Produkty" Value="pgProdukty"></asp:MenuItem>
                    <asp:MenuItem Text="Dostawcy" Value="pgDostawcy"></asp:MenuItem>
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

            
            <div id="IPOadmTabsContent" class="tabsContent paAdmin" style="background-color:#FFF; width:auto !important">
                <asp:MultiView ID="mvAdministracja" runat="server" ActiveViewIndex="0">
                    
           

                    <asp:View ID="pgZamawiajacy" runat="server" OnActivate="TabOnActivate">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc1:cntUprawnienia Rola="Zamawiający" ID="cntUprawnienia" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:View>
                    <asp:View ID="pgNotyfikowani" runat="server" OnActivate="TabOnActivate">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc1:cntUprawnienia Rola="Notyfikowany" ID="cntUprawnienia1" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:View>
                    <asp:View ID="pgKupcy" runat="server" OnActivate="TabOnActivate">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc1:cntUprawnienia Rola="Kupiec" ID="cntUprawnienia2" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:View>
                    <asp:View ID="pgMagazynier" runat="server" OnActivate="TabOnActivate">
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc1:cntUprawnienia Rola="Magazynier" ID="cntUprawnienia3" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:View>
                    <asp:View ID="pgAdministratorzy" runat="server" OnActivate="TabOnActivate">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc1:cntUprawnienia Rola="Administrator" ID="cntUprawnienia4" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:View>
                    <asp:View ID="pgAkceptujacy" runat="server" OnActivate="TabOnActivate">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc1:cntAkceptujacyAdmin ID="cntUprawnieniaAkceptujacy" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:View>
                    
                    <asp:View ID="pgConfig" runat="server" OnActivate="TabOnActivate">
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc1:cntKonfig ID="cntKonfig" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:View>
                    <asp:View ID="pgProdukty" runat="server" OnActivate="TabOnActivate">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc1:cntProdukty ID="cntProdukty" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:View>
                    <asp:View ID="pgDostawcy" runat="server" OnActivate="TabOnActivate">
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc1:cntDostawcy ID="cntDostawcy" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:View>
                     <asp:View ID="pgCC" runat="server" OnActivate="TabOnActivate">
                        <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc1:cntUprawnieniaMod ID="cntUprawnienia5" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:View>
                    <asp:View ID="pgModerator" runat="server" OnActivate="TabOnActivate">
                        <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc1:cntUprawnienia Rola="Moderator" ID="cntUprawnienia6" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/IPO/images/activity.gif" AlternateText="Loading ..." /> 
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

</asp:Content>
