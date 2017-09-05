<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="PDFViewer.aspx.cs" Inherits="HRRcp.Portal.PDFViewer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="../Controls/Portal/cntPDFReader.ascx" TagName="cPDFReader" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .content { padding-top: 12px !important; }
        .border { padding-left: 0 !important; padding-right: 0 !important; padding-bottom: 0 !important; margin-bottom: 12px !important; height: 875px !important; }
            .border .pdfviewer { width: 100%; height: 100%; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-title">
        <i class="fa fa-file-pdf-o"></i>
        Podgl¹d PDF
    </div>
    <div class="border container wide">
        <uc2:cPDFReader ID="cPDFReader1" runat="server" />

    </div>
    <br /><br /><br /><br /><br /><br /><br />
</asp:Content>
