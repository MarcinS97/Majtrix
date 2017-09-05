<%@ Page Title="" Language="C#" MasterPageFile="~/Scorecards/Scorecards.Master" AutoEventWireup="true" CodeBehind="Arkusze.aspx.cs" Inherits="HRRcp.Scorecards.Arkusze" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc1" %>

<%@ Register Src="~/Scorecards/Controls/Admin/cntTasks.ascx" TagPrefix="leet" TagName="Tasks" %>
<%@ Register Src="~/Scorecards/Controls/Admin/cntSpreadsheetsTasks.ascx" TagPrefix="leet" TagName="SpreadsheetsTasks" %>
<%@ Register Src="~/Scorecards/Controls/Admin/cntSpreadsheets.ascx" TagPrefix="leet" TagName="Spreadsheets" %>
<%@ Register Src="~/Scorecards/Controls/Admin/cntSpreadsheetsParameters.ascx" TagPrefix="leet" TagName="SpreadsheetsParameters" %>


<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>


<asp:Content ID="Content5" ContentPlaceHolderID="HeadFooter" runat="server">
     <script type="text/javascript">
        //hideFooter();
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler2);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler2);

        function BeginRequestHandler2(sender, args) {
        }

        function EndRequestHandler2(sender, args) {
            //prepareSearch();
        }
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="pgScAdministracja">
        <uc1:PageTitle ID="PageTitle1" runat="server"
            Ico="../images/captions/Struktura.png"
            Title="Arkusze"
        />
        
        
                <uc1:DateEdit ID="deFrom" runat="server" Visible="false" />
        
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="tabsMenu">
                    <asp:Menu ID="tabAdmin" runat="server" Orientation="Horizontal"  
                        onmenuitemclick="tabAdmin_MenuItemClick" >
                        <StaticMenuStyle CssClass="tabsStrip" />
                        <StaticMenuItemStyle CssClass="tabItem" />
                        <StaticSelectedStyle CssClass="tabSelected" />
                        <StaticHoverStyle CssClass="tabHover" />
                        <Items>
                            <asp:MenuItem Text="Arkusze" Value="vArkusze|HAARKUSZE"></asp:MenuItem>
<%--                            <asp:MenuItem Text="Przełożeni" Value="vKier|HAARKUSZE"></asp:MenuItem>
                            <asp:MenuItem Text="Pracownicy" Value="vPrac|HAARKUSZE"></asp:MenuItem>--%>
                            <asp:MenuItem Text="Czynności" Value="vCzynnosci|HAARKUSZE"></asp:MenuItem>
                            <asp:MenuItem Text="Arkusze - Czynności" Value="vCzynnosciArkusze|HAARKUSZE"></asp:MenuItem>
                            <asp:MenuItem Text="Parametry" Value="vParametry|HAARKUSZE"></asp:MenuItem>
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
                </div>            
                <div class="pageContent">
                    <asp:MultiView ID="mvArkusze" runat="server" ActiveViewIndex="0">
                        <asp:View ID="vArkusze" runat="server" >
                            <div class="padding">
                                <leet:Spreadsheets id="Spreadsheets" runat="server" />
                            </div>
                        </asp:View>
                        <asp:View ID="vKier" runat="server" >
                            <div class="padding">
                            </div>
                        </asp:View>
                        <asp:View ID="vPrac" runat="server" >
                            <div class="padding">
                            </div>
                        </asp:View>
                        <asp:View ID="vCzynnosci" runat="server" >
                            <div class="padding">
                                <leet:Tasks id="Tasks" runat="server" />
                            </div>
                        </asp:View>
                        <asp:View ID="vCzynnosciArkusze" runat="server" OnActivate="ActivateCzynnosciArkusze" >
                            <div class="padding">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <leet:SpreadsheetsTasks ID="SpreadsheetsTasks" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </asp:View>
                        <asp:View ID="vParametry" runat="server" OnActivate="ActivateParametry" >
                            <div class="padding">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <leet:SpreadsheetsParameters ID="SpreadsheetsParameters" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </div>    
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
