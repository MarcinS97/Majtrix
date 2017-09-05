<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.Master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="Start.aspx.cs" Inherits="HRRcp.Start" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register Src="~/Controls/Reports/cntInfoBoard.ascx" TagPrefix="uc1" TagName="cntInfoBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Start" SubText1="Informacje podsumowujące / zadania do wykonania" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:cntInfoBoard runat="server" id="cntInfoBoard1" Grupa="STARTPAGE"/>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
