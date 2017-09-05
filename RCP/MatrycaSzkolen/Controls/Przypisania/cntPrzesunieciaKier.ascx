<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPrzesunieciaKier.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Przypisania.cntPrzesunieciaKier" %>
<%@ Register src="cntPrzypisaniaEdit.ascx" tagname="cntPrzypisaniaEdit" tagprefix="uc1" %>
<%@ Register src="cntPrzypisania.ascx" tagname="cntPrzypisania" tagprefix="uc2" %>
<%@ Register src="cntDicCC.ascx" tagname="cntDicCC" tagprefix="uc3" %>
<%@ Register src="cntDicCommodity.ascx" tagname="cntDicCommodity" tagprefix="uc4" %>
<%@ Register src="cntDicArea.ascx" tagname="cntDicArea" tagprefix="uc4" %>
<%@ Register src="cntDicPosition.ascx" tagname="cntDicPosition" tagprefix="uc4" %>
<%@ Register src="cntSplity.ascx" tagname="cntSplity" tagprefix="uc3" %>

<table class="tabsContent" >
    <tr>
        <td class="LeftMenu">
            <asp:Menu ID="mnLeft" runat="server" StaticDisplayLevels="1" onmenuitemclick="mnLeft_MenuItemClick" >
                <StaticMenuStyle CssClass="menu" />
                <StaticSelectedStyle CssClass="selected" />
                <StaticMenuItemStyle CssClass="item" />
                <StaticHoverStyle CssClass="hover" />
                <Items>
                    <asp:MenuItem Text="Przesuń pracownika" Value="vPrzesun" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="Przesunięcia" Value="vPrzesuniecia" ></asp:MenuItem>
                    <asp:MenuItem Text="Moje wnioski" Value="vMojeWnioski" ></asp:MenuItem>
                    <asp:MenuItem Text="Do akceptacji" Value="vDoAkceptacji" ></asp:MenuItem>
                    <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Zaakceptowane" Value="vZaakceptowane" ></asp:MenuItem>
                    <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Odrzucone" Value="vOdrzucone"></asp:MenuItem>
                    
                    <asp:MenuItem Text="Struktura:" Value="DIC3" Selectable="false" Enabled="false" ></asp:MenuItem>
                    <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Splity" Value="vSplity"></asp:MenuItem>
                    <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Centra kosztowe" Value="vCC"></asp:MenuItem>
                    <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Commodities" Value="vCommodity"></asp:MenuItem>
                    <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Areas" Value="vArea"></asp:MenuItem>
                    <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Positions" Value="vPosition"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </td>
        
        <td class="LeftMenuContent">
            <asp:MultiView ID="mvPrzesuniecia" runat="server" ActiveViewIndex="0">
                <asp:View ID="vPrzesun" runat="server" onactivate="vPrzesun_Activate">
                    <uc1:cntPrzypisaniaEdit ID="cntPrzypisaniaEdit" Mode="KIER" runat="server" />
                </asp:View>

                <asp:View ID="vPrzesuniecia" runat="server">
                    <uc2:cntPrzypisania ID="cntPrzypisania5" Status="88" Mode="0" Filter="1" runat="server" />
                </asp:View>

                <asp:View ID="vMojeWnioski" runat="server">
                    <uc2:cntPrzypisania ID="cntPrzypisania1" Status="9" Mode="0" Filter="2" runat="server" />
                </asp:View>

                <asp:View ID="vDoAkceptacji" runat="server">
                    <uc2:cntPrzypisania ID="cntPrzypisania2" Status="0" Mode="0" Filter="0" runat="server" />
                </asp:View>

                <asp:View ID="vZaakceptowane" runat="server">
                    <uc2:cntPrzypisania ID="cntPrzypisania3" Status="1" Mode="0" Filter="2" runat="server" />
                </asp:View>

                <asp:View ID="vOdrzucone" runat="server">
                    <uc2:cntPrzypisania ID="cntPrzypisania4" Status="2" Mode="0" Filter="2" runat="server" />
                </asp:View>

                <%-- adm ----%>
                <asp:View ID="vSplity" runat="server" >
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>                        
                            <uc3:cntSplity ID="cntSplity1" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>            
                </asp:View>

                <asp:View ID="vCC" runat="server" >
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>                        
                            <uc3:cntDicCC ID="cntDicCC" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>            
                </asp:View>

                <asp:View ID="vCommodity" runat="server" >
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>                        
                            <uc4:cntDicCommodity ID="cntDicCommodity1" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>            
                </asp:View>
                
                <asp:View ID="vArea" runat="server" >
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>                        
                            <uc4:cntDicArea ID="cntDicArea" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>            
                </asp:View>
                
                <asp:View ID="vPosition" runat="server" >
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>                        
                            <uc4:cntDicPosition ID="cntDicPosition" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>            
                </asp:View>
            </asp:MultiView>
        </td>
    </tr>
</table>
