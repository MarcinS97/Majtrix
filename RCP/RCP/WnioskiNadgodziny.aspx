<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="WnioskiNadgodziny.aspx.cs" Inherits="HRRcp.RCP.WnioskiNadgodziny" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register Src="~/RCP/Controls/cntNadgodzinyWnioski.ascx" TagPrefix="uc1" TagName="cntNadgodzinyWnioski" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Wnioski o nadgodziny" />
    <div class="form-page pgWnioskiNadgodziny">
        <uc1:cntNadgodzinyWnioski runat="server" ID="cntNadgodzinyWnioski" />
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>



