<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Uprawnienia.aspx.cs" Inherits="HRRcp.UprawnieniaPL" %>
<%@ Register src="~/Controls/PodzialLudzi/cntUprawnienia.ascx" tagname="cntUprawnienia" tagprefix="uc1" %>
<%--
<%@ Register src="~/Controls/PodzialLudzi/cntUprawnienia.ascx" tagname="cntUprawnienia" tagprefix="uc1" %>
2 zak³adki: Podzia³ + Limity
<%@ Register src="~/Controls/PodzialLudzi/cntUprawnienia2.ascx" tagname="cntUprawnienia" tagprefix="uc1" %> 
--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function exportExcelClick() {
            return exportExcel('<%=hidReport.ClientID%>');
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">    
    <div id="pgUprawnieniaPL">
        <div class="center">
            <table class="caption" >
                <tr>
                    <td class="left">
                        <span class="caption4">
                            <img alt="" src="~/images/captions/layout_edit.png" runat="server"/>
                            <asp:Label ID="lbTitle" runat="server" Text="Podzia³ Ludzi - Uprawnienia"></asp:Label>
                        </span>
                    </td>
                    <td class="middle">
                    </td>
                    <td class="right" align="right">
                        <asp:Button ID="btRepBack1" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" />
                        <asp:Button ID="btRepPrint1" class="button75" runat="server" Text="Drukuj" OnClientClick="printPreview();return false;" Visible="false"/>
                        <asp:Button ID="btRepExcel1" class="button75" runat="server" Text="Excel" OnClientClick="return exportExcelClick();" onclick="btExcel_Click" />
                    </td>
                </tr>
            </table>     
        </div>
        
        <div class="center_wide3">        
            <div class="border">
                <asp:HiddenField ID="hidReport" runat="server" />
                <uc1:cntUprawnienia ID="cntUprawnienia" runat="server" />        
            </div>
        </div>
        
        <div class="center">
            <table class="printoff table0">
                <tr>
                    <td class="btprint_bottom2" >
                        <asp:Button ID="btRepBack2" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" />
                        <asp:Button ID="btRepPrint2" class="button75" runat="server" Text="Drukuj" OnClientClick="printPreview();return false;" Visible="false"/>
                        <asp:Button ID="btRepExcel2" class="button75" runat="server" Text="Excel" OnClientClick="return exportExcelClick();" onclick="btExcel_Click" />
                    </td>
                </tr>
            </table>       
        </div>
        <asp:UpdateProgress ID="updProgress1" runat="server" 
            DisplayAfter="10">
            <ProgressTemplate>
                <div class="updProgress1">
                    <div class="center">
                        <img alt="Indicator" src="~/images/activity.gif" runat="server"/> 
                        <span>Trwa przetwarzanie. Proszê czekaæ ...</span>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>    
</asp:Content>
