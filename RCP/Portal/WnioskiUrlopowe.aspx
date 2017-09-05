<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="WnioskiUrlopowe.aspx.cs" Inherits="HRRcp.Portal.WnioskiUrlopoweForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/Kwitek/Urlop.ascx" TagName="Urlop" TagPrefix="uc1" %>
<%@ Register Src="~/Portal/Controls/cntWnioskiUrlopowe3.ascx" TagName="cntWnioskiUrlopowe" TagPrefix="uc1" %>
<%--<%@ Register Src="~/Controls/Portal/cntWnioskiUrlopoweSelect2.ascx" TagName="cntWnioskiUrlopoweSelect" TagPrefix="uc1" %>--%>
<%@ Register Src="~/Portal/Controls/cntWnioskiUrlopoweSelect3.ascx" TagName="cntWnioskiUrlopoweSelect" TagPrefix="uc1" %>
<%@ Register Src="~/Portal/Controls/cntWniosekUrlopowy3.ascx" TagName="cntWniosekUrlopowy" TagPrefix="uc2" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-title">
        Wnioski urlopowe
    </div>
    <div class="paWnioskiUrlopowe container wide">
        <div class="paWnioskiUrlopowe2">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:Urlop ID="Urlop1" runat="server" />
                    <h2><i class="fa fa-pencil icon-fa"></i>Wypełnij wniosek o:</h2>
                    <hr />
                    <uc1:cntWnioskiUrlopoweSelect ID="cntWnioskiUrlopoweSelect1" Mode="0" OnSelect="cntWnioskiUrlopoweSelect1_Select" runat="server" />
                    <br />
                    <h2 class="title"><i class="fa fa-user icon-fa"></i>Moje wnioski:</h2>
                    <hr />
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe1" Status="9" OnShow="cntWnioskiUrlopowe1_Show" OnHide="cntWnioskiUrlopowe1_Hide" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <%--<asp:Button ID="btPopup" runat="server" Style="display: none;" />
                    <asp:ModalPopupExtender ID="extWniosekPopup" runat="server"
                        TargetControlID="btPopup"
                        PopupControlID="paWniosekPopup"
                        BackgroundCssClass="wnModalBackground">
                        <Animations>
                            <OnShown>
                                <ScriptAction Script="popupEventHandler();" />
                            </OnShown>
                        </Animations>
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="paWniosekPopup" runat="server" CssClass="wnModalPopup wniosekPopup" Style="display: none;">
                        <uc2:cntWniosekUrlopowy ID="cntWniosekUrlopowy1" OnClose="cntWniosekUrlopowy1_Close" runat="server" />
                    </asp:Panel>--%>

                        <uc2:cntWniosekUrlopowy ID="cntWniosekUrlopowy2" OnClose="cntWniosekUrlopowy1_Close" runat="server" />



                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>
</asp:Content>
