<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrePrint.aspx.cs" Inherits="HRRcp.PrePrint" %>

<%@ Register src="~/Controls/Portal/cntPDFReader.ascx" tagname="cntPDFReader" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<link href="~/styles/PDFReader.css" rel="stylesheet" type="text/css" />
<script src="<%# ResolveUrl("~/scripts/jquery-1.7.1/jquery.min.js") %>" type="text/javascript"></script>
<script type="text/javascript">
    window.onload = function() {
        window.onafterprint = function() {
            setTimeout(function() {
                document.getElementById('<%= Button2.ClientID %>').click();
            }, 1000);
        }
        setTimeout(function() {
            window.print();
        }, 1000);
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="crPrint" runat="server" />
    <asp:HiddenField ID="crPrintMax" runat="server" />
    <asp:HiddenField ID="crPrintName" runat="server" />
    <asp:Button ID="Button2" style="display: none" runat="server" onclick="Unnamed1_Click" />
    <div class="printoff" style="text-align: center">
    Drukowanie<br />
    <asp:Label ID="CPr" runat="server" />/<asp:Label ID="MPr" runat="server" /><br />
    <asp:Button runat="server" ID="Button1" Text="Przerwij" OnClick="Button1_Click" />
    </div>
    <div class="printon">
        <asp:Image ID="IMGPRT" runat="server" />
    </div>
    </form>
</body>
</html>
