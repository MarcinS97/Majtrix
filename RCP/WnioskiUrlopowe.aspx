<%@ Page Title="" Language="C#" MasterPageFile="~/Kwitek.Master" AutoEventWireup="true" CodeBehind="WnioskiUrlopowe.aspx.cs" Inherits="HRRcp.WnioskiUrlopoweForm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/Kwitek/Urlop.ascx" tagname="Urlop" tagprefix="uc1" %>
<%@ Register src="~/Controls/Portal/cntWnioskiUrlopowe.ascx" tagname="cntWnioskiUrlopowe" tagprefix="uc1" %>
<%@ Register src="~/Controls/Portal/cntWnioskiUrlopoweSelect2.ascx" tagname="cntWnioskiUrlopoweSelect" tagprefix="uc1" %>
<%@ Register src="~/Controls/Portal/cntWniosekUrlopowy.ascx" tagname="cntWniosekUrlopowy" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="paWnioskiUrlopowe">
        <div class="paWnioskiUrlopowe2">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:Urlop ID="Urlop1" runat="server" />
                    <br />

                    <span class="title">Wypełnij wniosek o:</span>
                    <uc1:cntWnioskiUrlopoweSelect ID="cntWnioskiUrlopoweSelect1" Mode="0" Filter="0" OnSelect="cntWnioskiUrlopoweSelect1_Select" runat="server" />


                    <%--
                    <asp:MultiView ID="mvWnioskiUrlopowe" runat="server" ActiveViewIndex="0"
                        onactiveviewchanged="mvWnioskiUrlopowe_ActiveViewChanged">
                       
                        <asp:View ID="vSelect" runat="server">
                            <span class="title">Wypełnij wniosek o:</span>
                            <uc1:cntWnioskiUrlopoweSelect ID="cntWnioskiUrlopoweSelect1" Mode="0" OnSelect="cntWnioskiUrlopoweSelect1_Select" runat="server" />
                        </asp:View>

                        <asp:View ID="vWniosek" runat="server">
                            <uc2:cntWniosekUrlopowy ID="cntWniosekUrlopowy1a" OnClose="cntWniosekUrlopowy1_Close" runat="server" />
                        </asp:View>
                    </asp:MultiView>
                    --%>
                    
                    
                    <br />
                    <span class="title">Moje wnioski:</span>
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe1" Status="9" OnShow="cntWnioskiUrlopowe1_Show" OnHide="cntWnioskiUrlopowe1_Hide" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btPopup" runat="server" style="display: none;" />
                    <asp:ModalPopupExtender ID="extWniosekPopup" runat="server" 
                        TargetControlID="btPopup"
                        PopupControlID="paWniosekPopup" 
                        BackgroundCssClass="wnModalBackground" >
                        <Animations>
                            <OnShown>
                                <ScriptAction Script="popupEventHandler();" />
                            </OnShown>
                        </Animations>
                    </asp:ModalPopupExtender>                        
                    <asp:Panel ID="paWniosekPopup" runat="server" CssClass="wnModalPopup wniosekPopup" style="display: none;" >
                        <uc2:cntWniosekUrlopowy ID="cntWniosekUrlopowy1" OnClose="cntWniosekUrlopowy1_Close" runat="server" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            
        </div>
    </div>
</asp:Content>
