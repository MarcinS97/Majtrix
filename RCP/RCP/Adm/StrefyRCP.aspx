<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="StrefyRCP.aspx.cs" Inherits="HRRcp.RCP.Adm.StrefyRCP" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register src="~/Controls/StrefyControl2.ascx" tagname="StrefyControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Strefy RCP" SubText1="Definiowanie stref rejestracji czasu pracy i czytników" />
    <div class="form-page pgStrefyRCP">
        <uc1:StrefyControl ID="StrefyControl1" runat="server" />
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
