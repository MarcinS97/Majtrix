<%@ Page Title="" Language="C#" MasterPageFile="~/Kwitek.Master" AutoEventWireup="true" CodeBehind="Kwitek.aspx.cs" Inherits="HRRcp.KwitekForm" %>
<%@ Register src="~/Controls/Kwitek/Wyplaty.ascx" tagname="Wyplaty" tagprefix="uc1" %>
<%@ Register src="~/Controls/Kwitek/WyplataDetale3.ascx" tagname="WyplataDetale" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:Wyplaty ID="Wyplaty1" OnSelectedChanged="Wyplaty1_SelectedChanged" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <uc1:WyplataDetale ID="WyplataDetale1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>
