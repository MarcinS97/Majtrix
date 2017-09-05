<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="NowyOkres.aspx.cs" Inherits="HRRcp.RCP.NowyOkres" %>

<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register Src="~/RCP/Controls/Adm/cntOkresy2.ascx" TagName="AdmOkresy" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Okresy rozliczeniowe" SubText1="Zarządzanie okresami rozliczeniowymi, współpraca z systemem Kadrowo-Płacowym" />
    <div class="form-page pgOkresyRozl">
        <uc2:AdmOkresy ID="cntOkresy" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
