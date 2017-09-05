<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WniosekWolneZaNadg.aspx.cs" Inherits="HRRcp.WniosekWolneZaNadg" %>
<%@ Register src="~/Controls/Raporty/cntWniosekWolneZaNadg.ascx" tagname="cntWniosekWolneZaNadg" tagprefix="uc1" %>
<%@ Register src="~/Controls/Raporty/cntWniosekWolneZaNadgKier.ascx" tagname="cntWniosekWolneZaNadgKier" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/print.css" rel="stylesheet" media="print" type="text/css" runat="server" id="headPrint" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
    <div class="center">
        <table class="caption">
            <tr>
                <td>
                    <span class="caption4">
                        <img id="Img1" alt="" src="~/images/captions/AnkietaView.png" runat="server"/>
                        Wniosek o wolne za nadgodziny
                    </span>
                </td>
                <td class="btprint_top" align="right">                    
                    <asp:Button ID="btRepPrint1" class="button75" runat="server" Text="Drukuj" OnClientClick="printPrint();return false;" />
                    <asp:Button ID="btRepPrintPrev1" class="button75" runat="server" Text="Podgląd" OnClientClick="printPreview();return false;" />
                    <asp:Button ID="btRepBack1" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" />
                    <%--
                    <asp:Button ID="btRepPrint1" class="button75" runat="server" Text="Drukuj" OnClientClick="printPreview();return false;" Visible="false"/>
                    <asp:Button ID="btRepBack1" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" Visible="false" />
                    <asp:Button ID="btRepExcel1" class="button75" runat="server" Text="Excel" OnClientClick="return exportExcelClick();" onclick="btExcel_Click" Visible="false" />
                    --%>
                </td>
            </tr>
        </table>     
    </div>
    
    <div class="page960 pgWniosekWolneZaNadg">
        <div class="border">
            <uc1:cntWniosekWolneZaNadg ID="cntWniosekWolneZaNadg1" runat="server" />
            <uc1:cntWniosekWolneZaNadgKier ID="cntWniosekWolneZaNadgKier" runat="server" Visible="false" />
        </div>
    </div>

    <div class="center">
        <table class="printoff table0">
            <tr>
                <td class="btprint_bottom2" >
                    <asp:Button ID="btRepPrint2" class="button75" runat="server" Text="Drukuj" OnClientClick="printPrint();return false;" />
                    <asp:Button ID="btRepPrintPrev2" class="button75" runat="server" Text="Podgląd" OnClientClick="printPreview();return false;" />
                    <asp:Button ID="btRepBack2" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" />
                    
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btDelete" class="button75" runat="server" Text="Usuń" onclick="btDelete_Click" />
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
