<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="Administracja.aspx.cs" Inherits="HRRcp.Portal.Ubezpieczenia.Majatkowe.UbezpAdministracja" %>

<%@ Register Src="~/Portal/Controls/Ubezpieczenia/Majatkowe/cntEksportyWyslane.ascx" TagPrefix="uc1" TagName="cntEksportyWyslane" %>
<%@ Register Src="~/Portal/Controls/Ubezpieczenia/cntUbezpieczeniaParametry.ascx" TagPrefix="uc1" TagName="cntUbezpieczeniaParametry" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgUbezpAdministracja">
        <div class="page-title">Ubezpieczenia majątkowe - Administracja</div>
        <%--<hr />--%>
        <div class="page-box container wide">
            <h3>Eksport danych do ubezpieczyciela</h3>
            <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:cntEksportyWyslane runat="server" ID="cntEksportyWyslane" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <hr />
            <h3>Ustawienia</h3>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:cntUbezpieczeniaParametry runat="server" ID="cntUbezpieczeniaParametry" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
