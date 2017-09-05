<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="Kontakty.aspx.cs" Inherits="HRRcp.Portal.Kontakty" ValidateRequest="false" %>

<%@ Register Src="~/Portal/Controls/cntArticles.ascx" TagName="cntArticles" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgKontakty border">
        <uc1:cntArticles ID="cntArticles" runat="server" Mode="0" Grupa="KONTAKTY" PageSize="0" Title="Kontakty" />
    </div>
</asp:Content>
