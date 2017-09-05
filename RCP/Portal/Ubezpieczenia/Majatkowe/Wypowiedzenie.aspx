<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="Wypowiedzenie.aspx.cs" Inherits="HRRcp.Portal.Ubezpieczenia.Majatkowe.Wypowiedzenie" %>

<%@ Register Src="~/Portal/Controls/Ubezpieczenia/Majatkowe/cntLista.ascx" TagPrefix="uc1" TagName="cntLista" %>
<%@ Register Src="~/Portal/Controls/Ubezpieczenia/Majatkowe/cntWypowiedzenieModal.ascx" TagPrefix="uc1" TagName="cntWypowiedzenieModal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgMojePolisy">
        <div class="page-title">
            Zakończ polisę
            <div class="sub">Wybierz polisę do zakończenia</div>
        </div>
        <div class="container wide">
            <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:cntLista runat="server" ID="cntLista" OnShow="cntLista_Show" Mode="12" ButtonIcon="glyphicon glyphicon-remove" ButtonClass="btn btn-danger" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <uc1:cntWypowiedzenieModal runat="server" ID="cntWypowiedzenieModal" OnSaved="cntWypowiedzenieModal_Saved" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
