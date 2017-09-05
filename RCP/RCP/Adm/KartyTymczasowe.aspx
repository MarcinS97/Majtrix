<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="KartyTymczasowe.aspx.cs" Inherits="HRRcp.KartyTymczasowe" %>

<%@ Register Src="~/RCP/Controls/Adm/cntKartyTymczasowe.ascx" TagPrefix="uc1" TagName="cntKartyTymczasowe" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-title">
        <i class="fa fa-id-card-o"></i>
        Identyfikatory tymczasowe
    </div>
    <%--<hr />--%>
    <div id="pgKartyTmp" class="center bg-white" >
        <uc1:cntKartyTymczasowe runat="server" id="cntKartyTymczasowe" />
    </div>
</asp:Content>
