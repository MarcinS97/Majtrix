<%@ Page Title="" Language="C#" MasterPageFile="~/KwitekAdmin.Master" AutoEventWireup="true" CodeBehind="AdminKwitek.aspx.cs" Inherits="HRRcp.AdminKwitekForm" %>
<%@ Register src="~/Controls/Kwitek/Wyplaty.ascx" tagname="Wyplaty" tagprefix="uc1" %>
<%@ Register src="~/Controls/Kwitek/WyplataDetale3.ascx" tagname="WyplataDetale" tagprefix="uc1" %>
<%@ Register src="~/Controls/Kwitek/Urlop.ascx" tagname="Urlop" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <uc1:Wyplaty ID="Wyplaty1" OnSelectedChanged="Wyplaty1_SelectedChanged" Mode="1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <uc1:WyplataDetale ID="WyplataDetale1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" Visible="false">
        <ContentTemplate>
            <uc1:Urlop ID="Urlop1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>
