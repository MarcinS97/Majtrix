<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true" CodeBehind="Oddelegowania.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.Adm.Oddelegowania" %>

<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc2" TagName="PageTitle" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/Przypisania/cntPrzesunieciaAdm.ascx" TagPrefix="cc" TagName="cntPrzesunieciaAdm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <uc2:PageTitle ID="PageTitle1" runat="server" Title="Pracownicy" SubText1="Oddelegowania pracowników - Administracja" />
    <div class="pgPrzesuniecia">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <cc:cntPrzesunieciaAdm ID="Przesuniecia" runat="server"  />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>    
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
