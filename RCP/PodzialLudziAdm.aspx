<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PodzialLudziAdm.aspx.cs" Inherits="HRRcp.PodzialLudziAdm" %>
<%@ Register src="~/Controls/PodzialLudzi/cntPodzialLudziKier.ascx" tagname="cntPodzialLudziKier" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="caption">
        <tr>
            <td>
                <span class="caption4">
                    <img alt="" src="images/captions/PracNotes.png"/>
                    Podział Ludzi - Administracja
                </span>
            </td>
            <td class="btprint_top" align="right">
                <%--
                <asp:Button ID="btRepPrint1" class="button75" runat="server" Text="Drukuj" OnClientClick="printPreview();return false;" Visible="false"/>
                <asp:Button ID="btRepBack1" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" Visible="false" />
                <asp:Button ID="btRepExcel1" class="button75" runat="server" Text="Excel" OnClientClick="return exportExcelClick();" onclick="btExcel_Click" Visible="false" />
                --%>
            </td>
        </tr>
    </table>     
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="admTabsContent" class="tabsContent paKierownika" style="border-collapse:collapse; background-color:#FFF;">
                <uc3:cntPodzialLudziKier ID="cntPodzialLudziKier" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
