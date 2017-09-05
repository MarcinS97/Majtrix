<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true" CodeBehind="UprawnieniaSzkol.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.Adm.UprawnieniaSzkol" %>

<%@ Register Src="~/MatrycaSzkolen/Controls/Adm/cntUprawnieniaSzkol.ascx" TagPrefix="cc" TagName="cntUprawnieniaSzkol" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="pgUprawnieniaSzkol pgScParametry">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Szkolenia - Uprawnienia trenerów" SubText1="Uprawnienia do szkoleń" />
        <div class="pageContent">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <cc:cntUprawnieniaSzkol id="CUPrawnienia" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
