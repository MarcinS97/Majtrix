<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PlanUrlopow.aspx.cs" Inherits="HRRcp.PlanUrlopow" %>

<%@ Register Src="~/Controls/PageTitle.ascx" TagName="PageTitle" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/PlanUrlopow/cntPlanRoczny2.ascx" TagName="cntPlanRoczny" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgPlanUrlopow">
        <div class="page-title">
            <asp:Label ID="lbTitle" runat="server" Text="Plan urlopów"></asp:Label>
        </div>
        <div id="paRozlNadg" runat="server" class="border fiszka container wide">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <uc2:cntPlanRoczny ID="cntPlanRoczny" runat="server" Mode="1" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <table class="printoff table0">
            <tr>
                <td class="btprint_bottom2">
                    <%--
                    <asp:Button ID="btRepBack2" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" />
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
            <div class="updProgress1 printoff">
                <div class="center">
                    <img alt="Indicator" src="images/activity.gif" />
                    <span>Trwa przetwarzanie. Proszę czekać ...</span>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
