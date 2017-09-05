<%@ Page Title="" Language="C#" MasterPageFile="~/IPO/IPO.Master" AutoEventWireup="true"
    CodeBehind="BazaProduktow.aspx.cs" Inherits="HRRcp.IPO.BazaProduktow" ValidateRequest="false"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/IPO/Controls/cntProdukty.ascx" TagName="cntProdukty" TagPrefix="uc1" %>
<%@ Register Src="~/IPO/Controls/cntDostawcy.ascx" TagName="cntDostawcy" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="caption">
        <tr>
            <td>
                <span class="caption4">
                    <img alt="" src="/images/captions/Struktura.png" />
                    Baza Produktów i Dostawców </span>
            </td>
            <td align="right">
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Menu ID="IPOBazaProduktow" runat="server" Orientation="Horizontal" OnMenuItemClick="IPOtabAdmin_MenuItemClick">
                <StaticMenuStyle CssClass="tabsStrip" />
                <StaticMenuItemStyle CssClass="tabItem" />
                <StaticSelectedStyle CssClass="tabSelected" />
                <StaticHoverStyle CssClass="tabHover" />
                <Items>
                    <asp:MenuItem Text="Produkty" Value="pgProdukty" Selected="true"></asp:MenuItem>
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
            <div id="BazaProduktowTabsContent" class="tabsContent paAdmin" style="background-color: #FFF; width:auto;">
                <asp:MultiView ID="mvBazaProduktow" runat="server" ActiveViewIndex="0">
                    <asp:View ID="pgProdukty" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc1:cntProdukty ID="cntProdukty" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:View>
                    <asp:View ID="pgDostawcy" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <uc1:cntDostawcy ID="cntDostawcy" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:View>
                </asp:MultiView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="updProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
        DisplayAfter="10">
        <ProgressTemplate>
            <div class="updProgress1">
                <div class="center">
                    <img alt="Indicator" src="/images/activity.gif" />
                    <span>Trwa przetwarzanie. Proszę czekać ...</span>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:ModalPopupExtender ID="updProgressBlocker" runat="server" TargetControlID="updProgress1"
        BackgroundCssClass="updProgress1back" PopupControlID="updProgress1">
    </asp:ModalPopupExtender>
</asp:Content>
