<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="Zmiany.aspx.cs" Inherits="HRRcp.RCP.Adm.Zmiany" %>

<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register Src="~/Controls/ZmianyControl3.ascx" TagName="ZmianyControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Zmiany" SubText1="Definiowanie parametrów zmian" />
    <div class="form-page pgZmiany" >
        <div class="pgZmianyContainer">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <uc1:ZmianyControl ID="cntZmiany" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
