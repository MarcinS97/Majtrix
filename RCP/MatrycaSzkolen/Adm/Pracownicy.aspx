<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true" CodeBehind="Pracownicy.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.Adm.Pracownicy" %>

<%@ Register Src="~/MatrycaSzkolen/Controls/Adm/cntPracownicy3.ascx" TagPrefix="cc" TagName="Pracownicy" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc2" TagName="PageTitle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <uc2:PageTitle ID="PageTitle1" runat="server" Title="Pracownicy" SubText1="Pracownicy - Uprawnienia" />
    <div class="pgPracownicy  pgPracownicy4 xcenter960">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <cc:Pracownicy ID="Prac" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>    
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
