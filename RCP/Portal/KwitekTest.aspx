<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="KwitekTest.aspx.cs" Inherits="HRRcp.Portal.KwitekTest" ValidateRequest="false" %>
<%@ Register src="~/Portal/Controls/cntArticles.ascx" tagname="cntArticles" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="xpgArticles border">
        <uc1:cntArticles ID="cntArticles" runat="server" Mode="0" Grupa="KWITEK" PageSize="0" Title="Kwitek płacowy" />
    </div>
</asp:Content>
