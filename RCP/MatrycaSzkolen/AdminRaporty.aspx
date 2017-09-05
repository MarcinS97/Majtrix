<%@ Page Title="" Language="C#" MasterPageFile="~/Scorecards/Scorecards.Master" AutoEventWireup="true" CodeBehind="AdminRaporty.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.AdminRaporty" %>
<%@ Register Src="~/Controls/Reports/cntReportsAdm.ascx" TagPrefix="uc1" TagName="cntReportsAdm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--
    <asp:Label ID="lbTitle" runat="server" Text="Raporty - Administracja"></asp:Label>
    <br />
    --%>
    <h2>Definicje raportów</h2>    
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <uc1:cntReportsAdm runat="server" id="cntReportsAdm" Grupa="REPORT" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
