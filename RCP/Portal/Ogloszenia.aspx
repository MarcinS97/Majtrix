<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="Ogloszenia.aspx.cs" Inherits="HRRcp.Portal.OgloszeniaForm" %>

<%@ Register Src="Controls/cntOgloszeniaBoard.ascx" TagName="cntOgloszeniaBoard" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--
    <link rel="stylesheet" type="text/css" href="<%# ResolveUrl("~/styles/Bootstrap/css/bootstrap.css") %>" />                       
    <link rel="stylesheet" type="text/css" href="<%# ResolveUrl("~/styles/FontAwesome/css/font-awesome.min.css") %>" />
    --%>
    <link rel="stylesheet" type="text/css" href="<%# ResolveUrl("~/Portal/Styles/Ogloszenia.css") %>" />

    <%--    
    <script type="text/javascript" src="<%# ResolveUrl("~/styles/Bootstrap/js/bootstrap.js") %>" ></script>
    --%>
    <script type="text/javascript" src="<%# ResolveUrl("~/Portal/Scripts/ogloszenia.js") %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgOgloszenia" style="margin: 0;">
        <div class="page-title">Ogłoszenia</div>
        <%--<hr />--%>
        <div class="container">
            <uc2:cntOgloszeniaBoard ID="cntOgloszeniaBoard" runat="server" />
            <%-- nie ma leżeć na update panel !!! bo są dodawane class, kontrolki mają swoje update panele --%>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Button ID="btSearch" runat="server" CssClass="button_postback" OnClick="btSearch_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

