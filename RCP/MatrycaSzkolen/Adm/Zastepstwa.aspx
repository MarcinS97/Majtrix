<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true" CodeBehind="Zastepstwa.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.Adm.Zastepstwa" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc2" TagName="PageTitle" %>
<%@ Register Src="~/Controls/AdmZastepstwa.ascx" TagPrefix="cc" TagName="cntZastepstwa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <uc2:PageTitle ID="PageTitle1" runat="server" Title="Zastępstwa" SubText1="" />
    <div class="pgZastepstwa">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <cc:cntZastepstwa id="cZastepstwa" runat="server" />
            </ContentTemplate>            
        </asp:UpdatePanel>
    </div>    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
