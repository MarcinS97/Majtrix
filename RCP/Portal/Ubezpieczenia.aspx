<%@ Page Title="" ValidateRequest="false" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="Ubezpieczenia.aspx.cs" Inherits="HRRcp.Portal.UbezpieczeniaForm" %>

<%@ Register Src="~/Portal/Controls/cntUbezpieczenia.ascx" TagPrefix="uc1" TagName="cntUbezpieczenia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgUbezpieczenia">
        <uc1:cntUbezpieczenia runat="server" ID="cntUbezpieczenia" />
    </div>
</asp:Content>

