<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="LogForm.aspx.cs" Inherits="HRRcp.LogForm" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc2" TagName="PageTitle" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/Adm/cntLog.ascx" TagPrefix="cc" TagName="cntLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc2:PageTitle ID="PageTitle1" runat="server" Title="Podgląd zdarzeń" SubText1="" />
    <div class="pgPracownicy xcenter960">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <cc:cntLog ID="Log1" runat="server"  />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>  
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
