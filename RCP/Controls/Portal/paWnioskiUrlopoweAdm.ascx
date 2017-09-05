<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="paWnioskiUrlopoweAdm.ascx.cs" Inherits="HRRcp.Controls.Portal.paWnioskiUrlopoweAdm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="../Kwitek/Urlop.ascx" tagname="Urlop" tagprefix="uc1" %>
<%@ Register src="~/Portal/Controls/cntWnioskiUrlopowe.ascx" tagname="cntWnioskiUrlopowe" tagprefix="uc1" %>
<%@ Register src="~/Portal/Controls/cntWnioskiUrlopoweSelect.ascx" tagname="cntWnioskiUrlopoweSelect" tagprefix="uc1" %>
<%@ Register src="~/Portal/Controls/cntWniosekUrlopowy.ascx" tagname="cntWniosekUrlopowy" tagprefix="uc2" %>

<table class="tabsContent" >
    <tr>
        <td class="LeftMenu">
            <asp:Menu ID="mnLeft" runat="server" StaticDisplayLevels="1" onmenuitemclick="mnLeft_MenuItemClick" >
                <StaticMenuStyle CssClass="menu" />
                <StaticSelectedStyle CssClass="selected" />
                <StaticMenuItemStyle CssClass="item" />
                <StaticHoverStyle CssClass="hover" />
                <Items>
                    <asp:MenuItem Text="Do wprowadzenia" Value="vDoWprowadzenia" Selected="true" ></asp:MenuItem>
                    <asp:MenuItem Text="Wprowadzone" Value="vWprowadzone" ></asp:MenuItem>
                    <asp:MenuItem Text="Do akceptacji" Value="vDoAkceptacji" ></asp:MenuItem>
                    <asp:MenuItem Text="Odrzucone" Value="vOdrzucone"></asp:MenuItem>
                    <asp:MenuItem Text="Wypełnij wniosek" Value="vEnter"></asp:MenuItem>
                    <asp:MenuItem Text="Moje wnioski" Value="vMojeWnioski" ></asp:MenuItem>
                    <asp:MenuItem Text="Ustawienia" Value="vSettings" ></asp:MenuItem>
                </Items>
            </asp:Menu>
        </td>
        
        <td class="LeftMenuContent">
            <asp:MultiView ID="mvPrzesuniecia" runat="server" ActiveViewIndex="0" OnActiveViewChanged="mvPrzesuniecia_ActiveViewChanged">

                <asp:View ID="vDoWprowadzenia" runat="server">
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe5" Status="3" Mode="2" Filter="1" Init="0" OnShow="cntWnioskiUrlopowe1_Show" runat="server" />
                    <div class="bottom_buttons">
                        <asp:Button ID="btEnter" CssClass="button250 btn btn-primary pull-right" runat="server" Text="Wprowadź wnioski" onclick="btEnter_Click" />
                    </div>
                </asp:View>

                <asp:View ID="vWprowadzone" runat="server">
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe6" Status="4" Mode="2" Filter="1" Init="0" OnShow="cntWnioskiUrlopowe1_Show" runat="server" />
                </asp:View>

                <asp:View ID="vDoAkceptacji" runat="server">
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe2" Status="1" Mode="2" Filter="1" Init="0" OnShow="cntWnioskiUrlopowe1_Show" runat="server" />
                </asp:View>

                <asp:View ID="vOdrzucone" runat="server">
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe4" Status="2" Mode="2" Filter="1" Init="0" OnShow="cntWnioskiUrlopowe1_Show" runat="server" />
                </asp:View>

                <asp:View ID="vMojeWnioski" runat="server">
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe1" Status="9" Mode="0" Filter="0" Init="0" OnShow="cntWnioskiUrlopoweMoje_Show" runat="server" />
                    <br />                    
<%--
                    <uc1:Urlop ID="Urlop1" runat="server" />
                    <br />
--%>                    
                    <h4 class="title">Wypełnij wniosek o:</h4>
                    <hr />
                    <uc1:cntWnioskiUrlopoweSelect ID="cntWnioskiUrlopoweSelect1" OnSelect="cntWnioskiUrlopoweSelect1_Select" runat="server" />
                    <div class="spacer64"></div>
                    <div class="spacer64"></div>
                </asp:View>

                <asp:View ID="vEnter" runat="server">
                    <h4 class="title">Wypełnij wniosek dla pracownika:</h4>
                    <hr />
                    <uc1:cntWnioskiUrlopoweSelect ID="cntWnioskiUrlopoweSelect2" Mode="2" OnSelect="cntWnioskiUrlopoweSelect2_Select" runat="server" />
                    <br />
                    <h3 class="title">Wnioski:</h3>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                        <ContentTemplate>
                            <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe3" Status="8" Mode="2" Filter="0" Init="0" OnShow="cntWnioskiUrlopowe1_Show" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="spacer64"></div>
                </asp:View>
                
                <asp:View ID="vSettings" runat="server">
                    <uc1:cntWnioskiUrlopoweSelect ID="cntWnioskiUrlopoweSelect3" Mode="3" runat="server" />
                    <div class="spacer64"></div>
                </asp:View>
            </asp:MultiView>
        </td>
    </tr>
</table>

<asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <%--<asp:Button ID="btPopup" runat="server" style="display: none;" />
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
        <asp:Panel ID="paWniosekPopup" runat="server" CssClass="wnModalPopup wniosekPopup" style="display: none;" >--%>
            <uc2:cntWniosekUrlopowy ID="cntWniosekUrlopowy1" OnClose="cntWniosekUrlopowy1_Close" runat="server" />
        <%--</asp:Panel>--%>
    </ContentTemplate>
</asp:UpdatePanel>

