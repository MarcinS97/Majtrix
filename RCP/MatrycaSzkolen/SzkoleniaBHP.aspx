<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true" CodeBehind="SzkoleniaBHP.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.SzkoleniaBHP" %>

<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc2" TagName="PageTitle" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/cntBadania.ascx" TagPrefix="cc" TagName="ctBadania" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/Uprawnienia/cntUprawnienia2.ascx" TagPrefix="cc" TagName="cntBadania" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="pgSzkoleniaBHP xpgKartaZgloszenie xcenter960">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <cc:cntBadania runat="server" id="cntBadania" Typ="8192" Title="Szkolenia BHP" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>    
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
