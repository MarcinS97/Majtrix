<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="paWnioskiUrlopoweKier3.ascx.cs" Inherits="HRRcp.Portal.Controls.paWnioskiUrlopoweKier3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/Kwitek/Urlop.ascx" tagname="Urlop" tagprefix="uc1" %>
<%@ Register src="~/Portal/Controls/cntWnioskiUrlopowe3.ascx" tagname="cntWnioskiUrlopowe" tagprefix="uc1" %>
<%@ Register src="~/Portal/Controls/cntWnioskiUrlopoweSelect3.ascx" tagname="cntWnioskiUrlopoweSelect" tagprefix="uc1" %>
<%@ Register src="~/Portal/Controls/cntWniosekUrlopowy3.ascx" tagname="cntWniosekUrlopowy" tagprefix="uc2" %>

<table class="tabsContent" >
    <tr>
        <td class="LeftMenu">
            <asp:Menu ID="mnLeft" runat="server" StaticDisplayLevels="1" onmenuitemclick="mnLeft_MenuItemClick" >
                <StaticMenuStyle CssClass="menu" />
                <StaticSelectedStyle CssClass="selected" />
                <StaticMenuItemStyle CssClass="item" />
                <StaticHoverStyle CssClass="hover" />
                <Items>
                    <asp:MenuItem Text="Do akceptacji" Value="vDoAkceptacji" Selected="true" ></asp:MenuItem>
                    <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Zaakceptowane" Value="vZaakceptowane" ></asp:MenuItem>
                    <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Odrzucone" Value="vOdrzucone"></asp:MenuItem>
                    <asp:MenuItem Text="Wypełnij wniosek" Value="vEnter" ></asp:MenuItem>                    
                    <asp:MenuItem Text="Moje wnioski" Value="vMojeWnioski" ></asp:MenuItem>
                </Items>
            </asp:Menu>
        </td>
        
        <td class="LeftMenuContent">
            <asp:MultiView ID="mvPrzesuniecia" runat="server" ActiveViewIndex="0">

                <asp:View ID="vDoAkceptacji" runat="server">
                    <%--<h3>Do akceptacji:</h3><hr />--%>
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe2" Status="1" Mode="1" Filter="0" OnShow="cntWnioskiUrlopowe1_Show" runat="server" />
                </asp:View>

                <asp:View ID="vZaakceptowane" runat="server">
                    <%--<h3>Zaakceptowane:</h3><hr />--%>
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe3" Status="34" Mode="1" Filter="0" OnShow="cntWnioskiUrlopowe1_Show" runat="server" />
                </asp:View>

                <asp:View ID="vOdrzucone" runat="server">
                    <%--<h3>Odrzucone:</h3><hr />--%>
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe4" Status="2" Mode="1" Filter="0" OnShow="cntWnioskiUrlopowe1_Show" runat="server" />
                </asp:View>

                <asp:View ID="vMojeWnioski" runat="server">
                    <uc1:Urlop ID="Urlop1" runat="server" Mode="1"/>
                    <br />
                    <%--<span class="title">Wypełnij wniosek o:</span>--%>
                      <h3 class="title">Wypełnij wniosek o:</h3>
                    <uc1:cntWnioskiUrlopoweSelect ID="cntWnioskiUrlopoweSelect1" OnSelect="cntWnioskiUrlopoweSelect1_Select" runat="server" />
                    <br />
                    <%--<span class="title">Wnioski:</span>--%>
                    <h3 class="title">Wnioski:</h3>
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe1" Status="9" Mode="0" Filter="0" OnShow="cntWnioskiUrlopoweMoje_Show" runat="server" />
                </asp:View>

                <asp:View ID="vEnter" runat="server">
                    <%--<span class="title">Wypełnij wniosek dla pracownika:</span>--%>
                    <h3 class="">Wypełnij wniosek dla pracownika:</h3>
                    <hr />
                    <uc1:cntWnioskiUrlopoweSelect ID="cntWnioskiUrlopoweSelect2" Mode="1" OnSelect="cntWnioskiUrlopoweSelect2_Select" runat="server" />
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                        <ContentTemplate>
                            <%--<span class="title">Wnioski:</span>--%>
                            <h3 class="title">Wnioski:</h3>
                            <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe5" Status="8" Mode="1" Filter="0" OnShow="cntWnioskiUrlopowe1_Show" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:View>

            </asp:MultiView>
        </td>
    </tr>
</table>

<asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Button ID="btPopup" runat="server" style="display: none;" />
     <%--   <asp:ModalPopupExtender ID="extWniosekPopup" runat="server" 
            TargetControlID="btPopup"
            PopupControlID="paWniosekPopup" 
            BackgroundCssClass="wnModalBackground" >
            <Animations><OnShown><ScriptAction Script="popupEventHandler();" /></OnShown></Animations>
        </asp:ModalPopupExtender>                        --%>
        <%--<asp:Panel ID="paWniosekPopup" runat="server" CssClass="wnModalPopup wniosekPopup" style="display: none;" >
            <uc2:cntWniosekUrlopowy ID="cntWniosekUrlopowy1" OnClose="cntWniosekUrlopowy1_Close" runat="server" />
        </asp:Panel>--%>

        <uc2:cntWniosekUrlopowy ID="cntWniosekUrlopowy1" OnClose="cntWniosekUrlopowy1_Close" runat="server" />

    </ContentTemplate>
</asp:UpdatePanel>




<%--
    TargetControlID="xbtShow"
    CancelControlID="btPopClose" 
    OkControlID="btPopOk"
--%>