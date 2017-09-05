<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HRRcp.Kiosk.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        Logowanie użytkownika.<br />        
        Przekazane parametry:<br />
        ------------------------------------<br />
        <asp:Literal ID="ltParams" runat="server"></asp:Literal>
        ------------------------------------<br />
    </div>
</asp:Content>
