<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="Pliki.aspx.cs" Inherits="HRRcp.Portal.Pliki" %>

<%@ Register Src="~/Portal/Controls/cntPliki.ascx" TagName="cntPliki" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgArticles">
        <uc1:cntPliki ID="cntPliki1" runat="server" Grupa="FILEP" />
    </div>
</asp:Content>
