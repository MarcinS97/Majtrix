<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true" CodeBehind="Zgloszenie2.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.Zgloszenie2" %>

<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc2" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/cntKartaZgloszenie2.ascx" TagPrefix="cc" TagName="cntZgloszenie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <uc2:PageTitle ID="PageTitle1" runat="server" Title="Zgłoszenie szkolenia" SubText1="Zgłoszenie pracowników na szkolenie" />
    <div class="pgKartaZgloszenie center960">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <cc:cntZgloszenie id="cZgloszenie" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
