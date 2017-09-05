<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="Warunki2.aspx.cs" Inherits="HRRcp.Portal.Ubezpieczenia.Majatkowe.Warunki2" %>

<%@ Register Src="~/Portal/Controls/cntPDFViewer.ascx" TagPrefix="uc1" TagName="cntPDFViewer" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .viewer{
            width: 100%;
            min-height: 800px;
             }
        .pgWarunkiUbezpieczenia .buttons { text-align: right; margin-top: 16px; }
        .paPDFViewer .paEditButton .btn { margin-bottom: 16px; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField ID="hidTypy" runat="server" Visible="false" Value="OWU|SWU|OWU_ZDROW|OWU_ZYCIE"/>

    <%--<asp:Literal id="litData" runat="server" />--%>
    <div class="page-title">
        Warunki ubezpieczenia
    </div>
    <div class="pgWarunkiUbezpieczenia container wide">
        <uc1:cntPDFViewer runat="server" id="cntPDFViewer" />
        <div class="buttons">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btSendMail" runat="server" CssClass="btn btn-default" Text="Wyślij na mój adres e-mail" OnClick="btSendMail_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
