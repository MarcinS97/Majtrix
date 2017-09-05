<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="MojePolisy.aspx.cs" Inherits="HRRcp.Portal.Ubezpieczenia.Majatkowe.MojePolisy" %>

<%@ Register Src="~/Portal/Controls/Ubezpieczenia/Majatkowe/cntLista.ascx" TagPrefix="uc1" TagName="cntLista" %>
<%@ Register Src="~/Portal/Controls/Ubezpieczenia/Majatkowe/cntWniosekModal.ascx" TagPrefix="uc1" TagName="cntWniosekModal" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgMojePolisy">
        <div class="page-title">Moje polisy</div>
        <div class="container wide">
            <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:cntLista runat="server" ID="cntLista" OnShow="cntLista_Show" Mode="99" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <uc1:cntWniosekModal runat="server" ID="cntWniosekModal" OnSaved="cntWniosekModal_Saved" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
