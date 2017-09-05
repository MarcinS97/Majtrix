<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="OkresyRozliczenioweTypy.aspx.cs" Inherits="HRRcp.RCP.Adm.OkresyRozliczenioweTypy" %>

<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register Src="~/RCP/Controls/Adm/cntOkresyTypy.ascx" TagPrefix="uc1" TagName="cntOkresyTypy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .pgOkresyRozlTypy {
            padding: 32px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Typy okresów rozliczeniowych" SubText1="" />
    <div class="form-page pgOkresyRozlTypy">
        <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:cntOkresyTypy runat="server" ID="cntOkresyTypy" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

