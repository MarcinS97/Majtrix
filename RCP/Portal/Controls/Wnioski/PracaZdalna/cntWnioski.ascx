<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWnioski.ascx.cs" Inherits="HRRcp.Portal.Controls.Wnioski.PracaZdalna.cntWnioski" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/Kwitek/Urlop.ascx" tagname="Urlop" tagprefix="uc1" %>
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
                    <asp:MenuItem Text="Do akceptacji" Value="vDoAkceptacji" Selected="true" ></asp:MenuItem>
                    <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Zaakceptowane" Value="vZaakceptowane" ></asp:MenuItem>
                    <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Odrzucone" Value="vOdrzucone"></asp:MenuItem>
                    <%--<asp:MenuItem Text="Wypełnij wniosek" Value="vEnter" ></asp:MenuItem>                    
                    <asp:MenuItem Text="Moje wnioski" Value="vMojeWnioski" ></asp:MenuItem>--%>
                </Items>
            </asp:Menu>
        </td>
        
        <td class="LeftMenuContent">
            <asp:MultiView ID="mvPrzesuniecia" runat="server" ActiveViewIndex="0">

                <asp:View ID="vDoAkceptacji" runat="server">
                    <%--<h3>Do akceptacji:</h3><hr />--%>
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe2" Status="1" Mode="1337" Filter="0" OnShow="cntWnioskiUrlopowe1_Show" runat="server" Rodzaj="1" />
                </asp:View>

                <asp:View ID="vZaakceptowane" runat="server">
                    <%--<h3>Zaakceptowane:</h3><hr />--%>
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe3" Status="4" Mode="1337" Filter="0" OnShow="cntWnioskiUrlopowe1_Show" runat="server" Rodzaj="1" />
                </asp:View>

                <asp:View ID="vOdrzucone" runat="server">
                    <%--<h3>Odrzucone:</h3><hr />--%>
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe4" Status="2" Mode="1337" Filter="0" OnShow="cntWnioskiUrlopowe1_Show" runat="server" Rodzaj="1" />
                </asp:View>

            </asp:MultiView>
        </td>
    </tr>
</table>

<asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <uc2:cntWniosekUrlopowy ID="cntWniosekUrlopowy1" OnClose="cntWniosekUrlopowy1_Close" runat="server" Rodzaj="1" />
    </ContentTemplate>
</asp:UpdatePanel>
