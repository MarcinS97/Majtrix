<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="Login_x.aspx.cs" Inherits="HRRcp.Kiosk.Login_x" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div>
        Logowanie użytkownika.<br />        
        Przekazane parametry:<br />
        ------------------------------------<br />
        <asp:Literal ID="ltParams" runat="server"></asp:Literal>
        ------------------------------------<br />
    </div>
</asp:Content>
