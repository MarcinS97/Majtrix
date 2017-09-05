<%@ Page Title="" Language="C#" MasterPageFile="~/KwitekAdmin.Master" AutoEventWireup="true" CodeBehind="AdminUrlop.aspx.cs" Inherits="HRRcp.AdminUrlopForm" %>
<%@ Register src="~/Controls/Kwitek/Urlop.ascx" tagname="Urlop" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:Urlop ID="Urlop1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
