<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JMJ_Mailing.aspx.cs" Inherits="HRRcp.JMJ.JMJ_Mailing" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .btn-mail { padding: 60px 120px; position: fixed; left: 50%; top: 40%; transform: translate(-50%, -50%); font-size: 32px; }
        .btn-mail:hover { cursor: pointer; opacity: 0.8; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnSendMail" runat="server" OnClick="btnSendMail_Click" Text="Wyślij maile" CssClass="btn-mail" />
    </div>
    </form>
</body>
</html>
