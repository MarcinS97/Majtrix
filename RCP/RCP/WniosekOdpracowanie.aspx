<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WniosekOdpracowanie.aspx.cs" Inherits="HRRcp.RCP.WniosekOdpracowanie" %>
<%@ Register src="~/Controls/Raporty/cntWniosekOdpracowanie.ascx" tagname="cntWniosekOdpracowanie" tagprefix="uc1" %>
<%@ Register Src="~/RCP/Controls/Raporty/cntWniosekOdpracowanie.ascx" TagPrefix="uc2" TagName="cntWniosekOdpracowanie" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--    <link href="styles/print.css" rel="stylesheet" media="print" type="text/css" runat="server" id="headPrint" />--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--    <div class="center">
        <table class="caption">
            <tr>
                <td>
                    <span class="caption4">
                        <img id="Img1" alt="" src="~/images/captions/AnkietaView.png" runat="server"/>
                        Wniosek o zwolnienie od pracy
                    </span>
                </td>
                <td class="btprint_top" align="right">                    
                   <asp:Button ID="btRepPrint1" class="button75" runat="server" Text="Drukuj" OnClientClick="printPrint();return false;" />
                    <asp:Button ID="btRepPrintPrev1" class="button75" runat="server" Text="Podgląd" OnClientClick="printPreview();return false;" />
                    <asp:Button ID="btRepBack1" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" />
                    
                    <asp:Button ID="btRepPrint1" class="button75" runat="server" Text="Drukuj" OnClientClick="printPreview();return false;" Visible="false"/>
                    <asp:Button ID="btRepBack1" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" Visible="false" />
                    <asp:Button ID="btRepExcel1" class="button75" runat="server" Text="Excel" OnClientClick="return exportExcelClick();" onclick="btExcel_Click" Visible="false" />
                    
                </td>
            </tr>
        </table>     
    </div>
    --%>
    <div class="pgWniosekOdpracownie printoff">
        <uc1:PageTitle runat="server" ID="PageTitle" Title="Wniosek o zwolnienie od pracy" />
        <div class="buttons">
            <asp:Button ID="btRepPrint1" class="button75" runat="server" Text="Drukuj" OnClientClick="printPrint();return false;" />
            <asp:Button ID="btRepPrintPrev1" class="button75" runat="server" Text="Podgląd" OnClientClick="printPreview();return false;" />
            <asp:Button ID="btRepBack1" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" />
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="pgWniosekOdpracownie page960 pgWniosekWolneZaNadg">
        <div class="border">
            <uc1:cntWniosekOdpracowanie ID="cntWniosekOdpracowanie1" runat="server" Visible="false" />
            <uc2:cntWniosekOdpracowanie ID="cntWniosekOdpracowanie2" runat="server" Visible="false" />
        </div>
    </div>

    <div class="pgWniosekOdpracownie center">
        <table class="printoff table0">
            <tr>
                <td class="btprint_bottom2" >
                    <asp:Button ID="btRepPrint2" class="button75" runat="server" Text="Drukuj" OnClientClick="printPrint();return false;" />
                    <asp:Button ID="btRepPrintPrev2" class="button75" runat="server" Text="Podgląd" OnClientClick="printPreview();return false;" />
                    <asp:Button ID="btRepBack2" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" />
                    
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btDelete" class="button75" runat="server" Text="Usuń" onclick="btDelete_Click" visible="false"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--
                    <asp:Button ID="btRepExcel2" class="button75" runat="server" Text="Excel" OnClientClick="return exportExcelClick();" onclick="btExcel_Click" />
                    --%>
                    <br />
                    <br />
                </td>
            </tr>
        </table>     

        <div class="print_footer">
            <asp:Label ID="lbPrintFooter" class="left" runat="server" Text="Wydrukowano z systemu Rejestracji Czasu Pracy v."></asp:Label>
            <asp:Label ID="lbPrintVersion" class="left" runat="server" Text="1.0.0.0"></asp:Label>
            <br />
            <asp:Label ID="lbPrintTime" class="left" runat="server" ></asp:Label>
        </div>
    </div>
</asp:Content>
