<%@ Page Title="" Language="C#" MasterPageFile="~/Kwitek.Master" AutoEventWireup="true" CodeBehind="Urlop.aspx.cs" Inherits="HRRcp.UrlopForm" %>
<%@ Register src="~/Controls/Kwitek/Urlop.ascx" tagname="Urlop" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
            <br />
            <uc1:Urlop ID="Urlop1" runat="server" />
            <br />
            <asp:Button CssClass="button printoff" ID="btPrint" runat="server" Text="Drukuj" OnClientClick="javascript:window.print();"/>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
