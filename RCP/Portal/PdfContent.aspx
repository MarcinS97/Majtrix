<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="PdfContent.aspx.cs" Inherits="HRRcp.Portal.PdfContent" %>

<%@ Register Src="~/Portal/Controls/cntPDFViewer.ascx" TagPrefix="uc1" TagName="cntPDFViewer" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .viewer{
            width: 100%;
            min-height: 800px;
             }
        .paPDFViewer .paEditButton .btn { margin-bottom: 16px; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgPdfContent">
        <div class="page-title">Podgląd PDF</div>
        <div class="container">
            <uc1:cntPDFViewer runat="server" id="cntPDFViewer" />
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
