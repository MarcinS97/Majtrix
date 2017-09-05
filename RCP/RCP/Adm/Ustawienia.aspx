<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="Ustawienia.aspx.cs" Inherits="HRRcp.RCP.Adm.UstawieniaForm" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register src="~/Controls/Mails/cntMailsAdm.ascx" tagname="cntMailsAdm" tagprefix="uc3" %>
<%@ Register src="~/Controls/Mails/cntSchedulerAdm.ascx" tagname="cntSchedulerAdm" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Ustawienia" SubText1="Ustawienia systemu, mailingi i scheduler" />
    <div class="form-page pgUstawienia">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Menu ID="tabUstawienia" runat="server" Orientation="Horizontal" 
                onmenuitemclick="tabUstawienia_MenuItemClick" >
                <StaticMenuStyle CssClass="tabsStrip" />
                <StaticMenuItemStyle CssClass="tabItem" />
                <StaticSelectedStyle CssClass="tabSelected" />
                <StaticHoverStyle CssClass="tabHover" />
                <Items>
                    <asp:MenuItem Text="Powiadomienia mailowe" Value="vMails" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="Powiadomienia mailowe - Scheduler" Value="vScheduler"></asp:MenuItem>
                </Items>
                <%--
                    <asp:MenuItem Text="Teksty" Value="vTeksty"></asp:MenuItem>
                    <asp:MenuItem Text="Ustawienia systemu" Value="vSettings"></asp:MenuItem>
                --%>
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

            <div class="tabsContent" style="border-collapse:collapse; background-color:#FFF;">
                
                <div class="content">

                <asp:MultiView ID="mvUstawienia" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vMails" runat="server">
                        <uc3:cntMailsAdm ID="cntMailsAdm" runat="server" />
                    </asp:View>

                    <asp:View ID="vScheduler" runat="server">
                        <uc3:cntSchedulerAdm ID="cntSchedulerAdm" runat="server" />
                    </asp:View>

                    <%--
                    <asp:View ID="vTeksty" runat="server">
                        <uc3:AdmInfoControl ID="AdmInfoControl" runat="server" />
                        <div class="bottom_buttons" >
                            <span>
                                Wybierz plik do importu:&nbsp;&nbsp;
                            </span>
                            <asp:FileUpload ID="FileUpload1" width="500px" CssClass="fileupload" runat="server" />
                            <asp:Button ID="btImportTeksty" runat="server" CssClass="button100" Text="Import" onclick="btImportTeksty_Click" OnClientClick="javascript:return checkFile();" />
                            <asp:Button ID="btExportTeksty" runat="server" CssClass="button100" Text="Eksport" onclick="btExportTeksty_Click" />
                        </div>
                    </asp:View>

                    <asp:View ID="vMails" runat="server">
                        <uc3:AdmMailsControl ID="AdmMailsControl" runat="server" />
                    </asp:View>

                    <asp:View ID="vUstawienia" runat="server">                    
                        <asp:Button ID="btUprawnienia" runat="server" Visible="false" CssClass="button100" PostBackUrl="~/Reports/RepCCUprawnienia.aspx" Text="Uprawnienia" />
                    </asp:View>

        <Triggers>
            <asp:PostBackTrigger ControlID="btExportTeksty" />
            <asp:PostBackTrigger ControlID="btImportTeksty" />
        </Triggers>
                    --%>
                </asp:MultiView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
