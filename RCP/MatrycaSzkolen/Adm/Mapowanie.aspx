<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true" CodeBehind="Mapowanie.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.Adm.Mapowanie" %>

<%@ Register Src="~/MatrycaSzkolen/Controls/Adm/Mapowanie/cntMapowanie.ascx" TagPrefix="cc" TagName="cntMapowanie" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="pgScParametry pgMapowanie">
        <uc1:PageTitle ID="PageTitle1" runat="server" Title="Matryca Szkoleń - Mapowanie" SubText1="Mapowanie szkoleń i linii" />
        <div class="pageContent">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <cc:cntMapowanie id="Map" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
