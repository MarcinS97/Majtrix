<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.Master" AutoEventWireup="true" CodeBehind="Kalendarz.aspx.cs" Inherits="HRRcp.RCP.Adm.Kalendarz" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register Src="~/RCP/Controls/Adm/cntKalendarz.ascx" TagPrefix="uc1" TagName="cntKalendarz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Kalendarz" SubText1="Roczny kalendarz dni wolnych" />
    <div class="form-page pgUstawienia">
        <uc1:cntKalendarz runat="server" id="cntKalendarz" />
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>