<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPodzialLudziKier.ascx.cs" Inherits="HRRcp.Controls.Przypisania.cntPodzialLudziKier" %>
<%@ Register src="~/Controls/Przypisania/cntDicCC.ascx" tagname="cntDicCC" tagprefix="uc3" %>
<%@ Register src="~/Controls/Przypisania/cntDicCommodity.ascx" tagname="cntDicCommodity" tagprefix="uc4" %>
<%@ Register src="~/Controls/Przypisania/cntDicArea.ascx" tagname="cntDicArea" tagprefix="uc4" %>
<%@ Register src="~/Controls/Przypisania/cntDicPosition.ascx" tagname="cntDicPosition" tagprefix="uc4" %>
<%@ Register src="~/Controls/Przypisania/cntSplity.ascx" tagname="cntSplity" tagprefix="uc3" %>
<%@ Register src="~/Controls/PodzialLudzi/cntPodzialLudziMies.ascx" tagname="cntPodzialLudziMies" tagprefix="uc3" %>
<%@ Register src="~/Controls/PodzialLudzi/cntCCMiesiace2.ascx" tagname="cntCCMiesiace2" tagprefix="uc1" %>

<%@ Register src="cntUprawnieniaLimity.ascx" tagname="cntUprawnieniaLimity" tagprefix="uc2" %>
<%@ Register src="cntRapLimityNN.ascx" tagname="cntRapLimityNN" tagprefix="uc5" %>

<table class="tabsContent cntPodzialLudziKier" >
    <tr>
        <td class="LeftMenu">
            <asp:Menu ID="mnLeft" runat="server" StaticDisplayLevels="1" onmenuitemclick="mnLeft_MenuItemClick" >
                <StaticMenuStyle CssClass="menu" />
                <StaticSelectedStyle CssClass="selected" />
                <StaticMenuItemStyle CssClass="item" />
                <StaticHoverStyle CssClass="hover" />
                <Items>
                    <asp:MenuItem Text="Podział Ludzi" Value="vPodzial" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="Raporty dla PM" Value="vRaportyPM" ></asp:MenuItem>
                    <asp:MenuItem Text="Limit nadgodzin na CC" Value="vRaportLimity" ></asp:MenuItem>
                    
                    <asp:MenuItem Text="Ilość DL na CC" Value="vRaportDL||url:PodzialLudzi/RaportDL.aspx" ></asp:MenuItem>
                    
                    <asp:MenuItem Text="Struktura:" Value="DIC3" Selectable="false" Enabled="false" ></asp:MenuItem>
                    <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Splity" Value="vSplity"></asp:MenuItem>
                    <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Centra kosztowe" Value="vCC"></asp:MenuItem>
                    <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Commodities" Value="vCommodity"></asp:MenuItem>
                    <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Areas" Value="vArea"></asp:MenuItem>
                    <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Positions" Value="vPosition"></asp:MenuItem>
                    <asp:MenuItem Text="Uprawnienia" Value="vUprawnienia" ></asp:MenuItem>
                    <asp:MenuItem Text="Limity nadgodzin na CC" Value="vLimity" ></asp:MenuItem>
                </Items>
            </asp:Menu>
        </td>
        
        <td class="LeftMenuContent">
            <asp:MultiView ID="mvPrzesuniecia" runat="server" ActiveViewIndex="0">
                <asp:View ID="vPodzial" runat="server" >
                    <uc3:cntPodzialLudziMies ID="cntPodzialLudziMies" runat="server" />    <%-- bez UpdatePanel bo ma showDialog --%>
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
                
                <asp:View ID="vLimity" runat="server" >
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>                        
                            <uc1:cntCCMiesiace2 ID="cntCCMiesiace" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>            
                </asp:View>

                <asp:View ID="vLimityPreview" runat="server" >
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>   
                            <uc5:cntRapLimityNN ID="cntRapLimityNN1" Mode="1" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>            
                </asp:View>
            </asp:MultiView>
        </td>
    </tr>
</table>

