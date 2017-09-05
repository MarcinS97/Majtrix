<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="RozliczenieNadgodzin.aspx.cs" Inherits="HRRcp.RozliczenieNadgodzin" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc1" %>
<%@ Register src="~/Controls/RozliczenieNadg/cntRozlNadg.ascx" tagname="cntRozlNadg" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="x_page960 pgRozliczenieNadgodzin">
        <table class="caption" >
            <tr>
                <td>
                    <span class="caption4">
                        <img alt="" src="images/captions/layout_edit.png"/>
                        <asp:Label ID="lbTitle" runat="server" Text="Rozliczenie nadgodzin"></asp:Label>
                    </span>
                </td>
                <td align="right">
                    <asp:Button ID="btRepBack1" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" />
                </td>
            </tr>
        </table>     
        <div id="paRozlNadg" runat="server" class="border fiszka" >
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <uc2:cntRozlNadg ID="cntRozlNadg" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <table class="printoff table0">
            <tr>
                <td class="btprint_bottom2" >
                    <asp:Button ID="btRepBack2" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" />
                    <%--
                    <asp:Button ID="btRepPrint2" class="button75" runat="server" Text="Drukuj" OnClientClick="printPreview();return false;" />
                    <asp:Button ID="btRepExcel2" class="button75" runat="server" Text="Excel" OnClientClick="return exportExcelClick();" onclick="btExcel_Click" />
                    --%>
                </td>
            </tr>
        </table>       
    </div>
    <%--
        AssociatedUpdatePanelID="UpdatePanel1" 
    --%>    
    <asp:UpdateProgress ID="updProgress1" runat="server" 
        DisplayAfter="10">
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

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
