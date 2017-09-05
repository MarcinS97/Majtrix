<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="PrzypiszRCP.aspx.cs" Inherits="HRRcp.Kiosk.PrzypiszRCP" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc1" %>
<%@ Register src="~/Controls/Kiosk/cntPrzypiszRCP.ascx" tagname="cntPrzypiszRCP" tagprefix="uc2" %>
<%@ Register src="~/Controls/Kiosk/cntPrzypiszRCPHistoria.ascx" tagname="cntPrzypiszRCPHistoria" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-title">
        <i class="fa fa-id-card-o"></i>
        Przypisywanie numerów kart RCP

    </div>
    <%--<hr />--%>
    <div id="pgPrzypiszRCP" class="center bg-white container wide" >
        <%--<uc1:PageTitle ID="PageTitle2" runat="server"
            Ico="../images/captions/PracNotes.png"
            Title="Przypisywanie numerów kart RCP"
        />
        --%>
        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Menu ID="tabPrzypisz" runat="server" Orientation="Horizontal" 
                    onmenuitemclick="tabPrzypisz_MenuItemClick" >
                    <StaticMenuStyle CssClass="tabsStrip" />
                    <StaticMenuItemStyle CssClass="tabItem" />
                    <StaticSelectedStyle CssClass="tabSelected" />
                    <StaticHoverStyle CssClass="tabHover" />
                    <Items>
                        <asp:MenuItem Text="Przypisywanie" Value="vPrzypisz|HPRZYPISZRCP" Selected="true"></asp:MenuItem>
                        <asp:MenuItem Text="Historia" Value="vHistoria|HHISTORIARCP"></asp:MenuItem>
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

                <div id="tabsContent" runat="server" class="tabsContent border" >
                    <asp:MultiView ID="mvPrzypisz" runat="server" ActiveViewIndex="0">

                        <asp:View ID="vPrzypisz" runat="server" onactivate="vPrzypisz_Activate" 
                            ondeactivate="vPrzypisz_Deactivate">
                            <asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>                                

                                    <uc2:cntPrzypiszRCP ID="cntPrzypiszRCP1" runat="server" />

                                </ContentTemplate>
                            </asp:UpdatePanel>            
                        </asp:View>

                        <asp:View ID="vHistoria" runat="server">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>                                

                                    <uc3:cntPrzypiszRCPHistoria ID="cntPrzypiszRCPHistoria1" OnDeleted="cntPrzypiszRCPHistoria1_Deleted" runat="server" />

                                </ContentTemplate>
                            </asp:UpdatePanel>            
                        </asp:View>

                    </asp:MultiView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>
