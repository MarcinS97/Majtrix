<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="NadgodzinyOdbior.aspx.cs" Inherits="HRRcp.RCP.NadgodzinyOdbior" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register Src="~/RCP/Controls/cntNadgodzinyOdbior.ascx" TagPrefix="uc1" TagName="cntNadgodzinyOdbior" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Odbiór nadgodzin" />
    <div class="form-page pgWnioskiNadgodzinyOdbior">
        <uc1:cntNadgodzinyOdbior runat="server" ID="cntNadgodzinyOdbior" />
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>



