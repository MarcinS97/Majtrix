<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="HarmonogramAkceptacja.aspx.cs" Inherits="HRRcp.RCP.HarmonogramAkceptacja" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register Src="~/RCP/Controls/cntHarmonogramAcc.ascx" TagPrefix="uc1" TagName="HarmonogramAcc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Akceptacja planu pracy" SubText1="Proszę zaakceptować lub odrzucić przygotowane harmonogramy" />
    <div class="form-page pgHarmonogramAkceptacja">
      <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
                <uc1:HarmonogramAcc runat="server" ID="HarmAcc" />
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
