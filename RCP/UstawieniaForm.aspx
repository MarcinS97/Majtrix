<%@ Page Title="" ValidateRequest="false" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="UstawieniaForm.aspx.cs" Inherits="HRRcp.UstawieniaForm" %>
<%@ Register src="~/Controls/AdmLogControl.ascx" tagname="AdmLogControl" tagprefix="uc1" %>
<%--
<%@ Register src="Controls/AdmPracownicyControl.ascx" tagname="AdmPracownicyControl" tagprefix="uc2" %>
--%>
<%@ Register src="~/Controls/AdmInfoControl.ascx" tagname="AdmInfoControl" tagprefix="uc3" %>
<%@ Register src="~/Controls/AdmMailsControl.ascx" tagname="AdmMailsControl" tagprefix="uc3" %>
<%@ Register src="~/Controls/Mails/cntMailsAdm.ascx" tagname="cntMailsAdm" tagprefix="uc3" %>
<%@ Register src="~/Controls/Mails/cntSchedulerAdm.ascx" tagname="cntSchedulerAdm" tagprefix="uc3" %>
<%@ Register src="~/Controls/AdmSettingsControl.ascx" tagname="AdmSettingsControl" tagprefix="uc3" %>
<%@ Register src="~/Controls/AdmKodyWyjasnicControl.ascx" tagname="AdmKodyWyjasnicControl" tagprefix="uc3" %>
<%@ Register src="~/Controls/ProgressBar2.ascx" tagname="ProgressBar2" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function checkFile() {
            fu = document.getElementById('<%=FileUpload1.ClientID%>');
            if (fu != null) {
                if (!fu.value) {
                    alert("Brak pliku do importu.");
                    return false;
                }
            }
            return true;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="caption">
        <tr>
            <td>
                <span class="caption4">
                    <img alt="" src="images/captions/layout_edit.png"/>
                    Ustawienia systemu
                </span>
            </td>
            <td align="right">
            </td>
        </tr>
    </table>     

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Menu ID="tabUstawienia" runat="server" Orientation="Horizontal" 
                onmenuitemclick="tabUstawienia_MenuItemClick" >
                <StaticMenuStyle CssClass="tabsStrip" />
                <StaticMenuItemStyle CssClass="tabItem" />
                <StaticSelectedStyle CssClass="tabSelected" />
                <StaticHoverStyle CssClass="tabHover" />
                <Items>
                    <asp:MenuItem Text="Podgląd zdarzeń" Value="0" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="Teksty" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="Powiadomienia mailowe" Value="4"></asp:MenuItem>
                    <asp:MenuItem Text="Powiadomienia mailowe - Scheduler" Value="5"></asp:MenuItem>
                    <%--<asp:MenuItem Text="Powiadomienia mailowe old" Value="2"></asp:MenuItem>--%>
                    <asp:MenuItem Text="Ustawienia systemu" Value="3"></asp:MenuItem>
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

            <asp:Timer ID="ProgressTimer" runat="server" Enabled="False" Interval="1000" 
                ontick="ProgressTimer_Tick">
            </asp:Timer>

            <div class="tabsContent" style="border-collapse:collapse; background-color:#FFF;">
                <asp:MultiView ID="mvPracownicy" runat="server" ActiveViewIndex="0">

                    <asp:View ID="pgLog" runat="server">
                        <uc1:AdmLogControl ID="AdmLogControl" runat="server" />
                    </asp:View>
                
                    <asp:View ID="pgTeksty" runat="server">
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

                    <asp:View ID="pgMails" runat="server">
                        <uc3:AdmMailsControl ID="AdmMailsControl" runat="server" />
                    </asp:View>

                    <asp:View ID="pgUstawienia" runat="server">                    
                        
                        <asp:Button ID="btUprawnienia" runat="server" Visible="false" CssClass="button100" PostBackUrl="~/Reports/RepCCUprawnienia.aspx" Text="Uprawnienia" />
                        
                        <%-- 
                        <uc3:AdmSettingsControl ID="AdmSettingsControl1" runat="server" />
                        <div class="spacer16"></div>
                        <!-- group box -->
                        <table class="GroupBox1" width="100%"><tr><td class="tl" >&nbsp;&nbsp;</td><td class="th" ><div class="title">
                            KODY - UWAGI KONTROLERA DO POZYCJI ANKIETY
                            </div></td><td class="tr" >&nbsp;&nbsp;&nbsp;</td></tr><tr><td class="vl"></td><td>
                            <!-- group box content -->
                            <table class="table3C">
                                <tr>
                                    <td class="col1">
                                        <uc3:AdmKodyWyjasnicControl ID="AdmKodyWyjasnicControl1" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <div class="spacer8"></div>
                            <!-- group box end content -->
                            </td><td class="vr" >&nbsp;</td></tr><tr><td class="bl"></td><td class="bh"></td><td class="br"></td></tr>
                        </table>
                        <!-- group box end -->
                        --%>
                    </asp:View>

                    <asp:View ID="View1" runat="server">
                        <uc3:cntMailsAdm ID="cntMailsAdm" runat="server" />
                    </asp:View>

                    <asp:View ID="View2" runat="server">
                        <uc3:cntSchedulerAdm ID="cntSchedulerAdm" runat="server" />
                    </asp:View>
                </asp:MultiView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btExportTeksty" />
            <asp:PostBackTrigger ControlID="btImportTeksty" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="updProgress1" runat="server" 
        AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="500">
        <ProgressTemplate>
            <div class="updProgress1">
                <div class="center">
                    <img alt="Indicator" src="images/activity.gif" /> 
                    <span>Trwa przetwarzanie. Proszę czekać ...</span>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
