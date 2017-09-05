<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Portal3.Master" CodeBehind="RejestracjaRCP.aspx.cs" Inherits="HRRcp.Portal.RejestracjaRCP" %>

<%@ Register Src="~/Controls/cntRejestracjaRCP.ascx" TagPrefix="uc1" TagName="cntRejestracja" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/fullcalendar.min.css" rel="stylesheet" />
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:cntRejestracja runat="server" id="cntRejestracja" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
