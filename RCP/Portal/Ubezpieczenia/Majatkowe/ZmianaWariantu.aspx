<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="ZmianaWariantu.aspx.cs" Inherits="HRRcp.Portal.Ubezpieczenia.Majatkowe.ZmianaWariantu" %>

<%@ Register Src="~/Portal/Controls/Ubezpieczenia/Majatkowe/cntLista.ascx" TagPrefix="uc1" TagName="cntLista" %>
<%@ Register Src="~/Portal/Controls/Ubezpieczenia/Majatkowe/cntZmianaWariantuModal.ascx" TagPrefix="uc1" TagName="cntZmianaModal" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgZmianaWariantu">
        <div class="page-title">
            Zmiana wariantu ubezpieczenia
            <div class="sub">
                Wybierz polisę do zmiany
            </div>
        </div>
        <div class="container wide">
            <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:cntLista runat="server" ID="cntLista" OnShow="cntLista_Show" ButtonIcon="fa fa-exchange" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <uc1:cntZmianaModal runat="server" ID="cntZmianaModal" OnSaved="cntZmianaModal_Saved" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
