<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Wnioski_Dane_Osobowe_Adm.ascx.cs"
    Inherits="HRRcp.Controls.WniosekZmianaDanych.Wnioski_Dane_Osobowe_Adm" %>
<%@ Register Src="~/Controls/Portal/WniosekZmianaDanych/cntzoom.ascx" TagName="zoom"
    TagPrefix="muc1" %>
<%@ Register Src="~/Controls/Portal/WniosekZmianaDanych/GetName.ascx" TagName="GetName"
    TagPrefix="muc1" %>
<table class="tabsContent">
    <tr>
        <td class="LeftMenu">
            <asp:Menu ID="mnLeft" runat="server" StaticDisplayLevels="1" OnMenuItemClick="mnLeft_MenuItemClick">
                <StaticMenuStyle CssClass="menu" />
                <StaticSelectedStyle CssClass="selected" />
                <StaticMenuItemStyle CssClass="item" />
                <StaticHoverStyle CssClass="hover" />
                <Items>
                    <asp:MenuItem Text="Do wprowadzenia" Value="vDoWprowadzenia" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="Wprowadzone" Value="vWprowadzone"></asp:MenuItem>
                    <asp:MenuItem Text="Do akceptacji" Value="vDoAkceptacji"></asp:MenuItem>
                    <asp:MenuItem Text="Odrzucone" Value="vOdrzucone"></asp:MenuItem>
                    <asp:MenuItem Text="Robocze" Value="vRobocze"></asp:MenuItem>
                    <asp:MenuItem Text="Wypełnij wniosek" Value="vEnter"></asp:MenuItem>
                    <asp:MenuItem Text="Wszystkie" Value="vWszystkie"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </td>
        <td class="LeftMenuContent">
            <asp:MultiView ID="mvPrzesuniecia" runat="server" ActiveViewIndex="0">
                <asp:View ID="vDoWprowadzenia" runat="server">
                    <muc1:zoom ID="zoom1" Lvl="2" Status="3" runat="server" />
                </asp:View>
                <asp:View ID="vWprowadzone" runat="server">
                    <muc1:zoom ID="zoom2" Lvl="2" Status="4" runat="server" />
                </asp:View>
                <asp:View ID="vDoAkceptacji" runat="server">
                    <muc1:zoom ID="zoom3" Lvl="2" Status="1" runat="server" />
                </asp:View>
                <asp:View ID="vOdrzucone" runat="server">
                    <muc1:zoom ID="zoom4" Lvl="2" Status="2" runat="server" />
                </asp:View>
                <asp:View ID="vRobocze" runat="server">
                    <muc1:zoom ID="zoom5" Lvl="2" Status="0" runat="server" />
                </asp:View>
                <asp:View ID="vWszystkie" runat="server">
                    <muc1:zoom ID="zoom6" Lvl="2" Status="W" runat="server" />
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btExcel" runat="server" Text="Excel" OnClick="btExcel_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btExcel" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:View>
                <asp:View ID="vEnter" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <muc1:GetName ID="GetName1" runat="server" Lvl="2" />
                            <br />
                            <hr />
                            <b>Wnioski pracownika </b>
                            <br />
                            <muc1:zoom ID="zoom7" Visible="false" Lvl="2" StatusVisible="1" Status="AZ" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:View>
            </asp:MultiView>
        </td>
    </tr>
</table>
