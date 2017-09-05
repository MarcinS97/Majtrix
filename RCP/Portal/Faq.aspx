<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="Faq.aspx.cs" Inherits="HRRcp.Portal.Faq" ValidateRequest="false" %>

<%@ Register Src="~/Portal/Controls/cntArticles.ascx" TagName="cntArticles" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgFaq border">
        <uc1:cntArticles ID="cntArticles" runat="server" Mode="0" Grupa="FAQ" PageSize="0" Title="FAQ" />
    </div>
</asp:Content>
