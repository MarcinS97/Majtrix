<%@ Page Title="" Language="C#" MasterPageFile="~/Scorecards/Scorecards.Master" AutoEventWireup="true" CodeBehind="Administracja.aspx.cs" Inherits="HRRcp.Scorecards.AdminForm" ValidateRequest="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc1" %>
<%@ Register src="~/Scorecards/Controls/cntZastepstwaMenu.ascx" tagname="AdmZastepstwa" tagprefix="uc1" %>
<%@ Register src="~/Controls/Adm/cntPracownicy3.ascx" tagname="cntPracownicy" tagprefix="uc1" %>
<%@ Register src="~/Controls/AdmLogControl.ascx" tagname="AdmLogControl" tagprefix="uc1" %>
<%@ Register src="~/Controls/AdmInfoControl.ascx" tagname="AdmInfoControl" tagprefix="uc3" %>
<%@ Register src="~/Controls/Mails/cntMailsAdm.ascx" tagname="cntMailsAdm" tagprefix="uc3" %>
<%@ Register src="~/Controls/Mails/cntSchedulerAdm.ascx" tagname="cntSchedulerAdm" tagprefix="uc3" %>

<%@ Register Src="~/Scorecards/Controls/Admin/cntParameters.ascx" TagPrefix="leet" TagName="Parameters" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
<%--
    <table class="caption" >
        <tr>
            <td>
                <span class="caption4">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/captions/Struktura.png"/>
                    Administracja
                </span>
            </td>
            <td align="right">
            </td>
        </tr>
    </table>     
    --%>
    <div class="pgScAdministracja">
        <uc1:PageTitle ID="PageTitle1" runat="server"
            Ico="../images/captions/Struktura.png"
            Title="Administracja"
        />
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
                            <asp:MenuItem Text="Pracownicy" Value="pgPracownicy|HAPRAC"></asp:MenuItem>
                            <asp:MenuItem Text="Zastępstwa" Value="pgZastepstwa|HAZASTEP"></asp:MenuItem>
                            <asp:MenuItem Text="Teksty" Value="pgTeksty|HATEKSTY"></asp:MenuItem>
                            <asp:MenuItem Text="Powiadomienia mailowe" Value="pgMails|HAMAILE"></asp:MenuItem>
                            <asp:MenuItem Text="Powiadomienia mailowe - Scheduler" Value="pgScheduler|HASCHEDULER"></asp:MenuItem>
                            <asp:MenuItem Text="Podgląd zdarzeń" Value="pgLog|HALOG" Selected="true"></asp:MenuItem>
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
                    <asp:MultiView ID="mvAdministracja" runat="server" ActiveViewIndex="0">
                        
                        <asp:View ID="pgPracownicy" runat="server" >
                            <div class="padding paPracownicy">
                                <uc1:cntPracownicy ID="cntPracownicy" runat="server" />
                            </div>
                        </asp:View>

                        <asp:View ID="pgParams" runat="server">
                            <leet:Parameters id="Parameters" runat="server" />
                        </asp:View>

                        <asp:View ID="pgZastepstwa" runat="server" >
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <uc1:AdmZastepstwa ID="cntZastepstwa" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:View>

                        <asp:View ID="pgLog" runat="server">
                            <div class="padding paLog">
                                <uc1:AdmLogControl ID="AdmLogControl" runat="server" />
                            </div>
                        </asp:View>
                    
                        <asp:View ID="pgTeksty" runat="server">
                            <div class="padding paTeksty">
                                <uc3:AdmInfoControl ID="AdmInfoControl" runat="server" />
                                <div class="bottom_buttons" >
                                    <span>
                                        Wybierz plik do importu:&nbsp;&nbsp;
                                    </span>
                                    <asp:FileUpload ID="FileUpload1" width="500px" CssClass="fileupload" runat="server" />
                                    <asp:Button ID="btImportTeksty" runat="server" CssClass="button100" Text="Import" onclick="btImportTeksty_Click" OnClientClick="javascript:return checkFile();" />
                                    <asp:Button ID="btExportTeksty" runat="server" CssClass="button100" Text="Eksport" onclick="btExportTeksty_Click" />
                                </div>
                            </div>
                        </asp:View>

                        <asp:View ID="pgMails" runat="server">
                            <div class="padding paMails">
                                <uc3:cntMailsAdm ID="cntMailsAdm" runat="server" />
                            </div>
                        </asp:View>

                        <asp:View ID="pgScheduler" runat="server">
                            <div class="padding paScheduler">
                                <uc3:cntSchedulerAdm ID="cntSchedulerAdm" runat="server" />
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </div>    
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btExportTeksty" />
                <asp:PostBackTrigger ControlID="btImportTeksty" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
