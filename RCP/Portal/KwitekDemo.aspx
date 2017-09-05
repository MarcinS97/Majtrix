<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="KwitekDemo.aspx.cs" Inherits="HRRcp.Portal.KwitekDemo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgArticles border">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/portal/3/kwitek_demo.png"  />
    </div>
    <div class="center">
        <table class="printoff table0">
            <tr>
                <td class="btprint_bottom2" >
                    <asp:Button ID="btRepBack2" class="button75" runat="server" Text="Powrót" OnClientClick="history.back();return false;" />
                    <asp:Button ID="btRepPrint2" class="button75" runat="server" Text="Drukuj" OnClientClick="printPreview();return false;" />
                    <asp:Button ID="btRepExcel2" class="button75" runat="server" Text="Excel" OnClientClick="return exportExcelClick();" onclick="btExcel_Click" />
                </td>
            </tr>
        </table>     

        <div class="print_footer">
            <asp:Label ID="lbPrintFooter" class="left" runat="server" Text="Wydrukowano z Portalu Pracownika v."></asp:Label>
            <asp:Label ID="lbPrintVersion" class="left" runat="server" Text="1.0.0.0"></asp:Label>
            <br />
            <asp:Label ID="lbPrintTime" class="left" runat="server" ></asp:Label>
        </div>
    </div>
</asp:Content>
