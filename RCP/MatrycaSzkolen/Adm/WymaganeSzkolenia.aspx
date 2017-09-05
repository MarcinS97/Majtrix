<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true" CodeBehind="WymaganeSzkolenia.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.Adm.WymaganeSzkolenia" %>

<%@ Register Src="~/MatrycaSzkolen/Controls/Adm/cntWymaganeSzkolenia.ascx" TagPrefix="cc" TagName="cntWymaganeSzkolenia" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="pgUprawnieniaSzkol pgScParametry">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Stanowiska - Szkolenia" SubText1="Wymagane szkolenia" />
        <div class="pageContent">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <cc:cntWymaganeSzkolenia id="CUPrawnienia" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
