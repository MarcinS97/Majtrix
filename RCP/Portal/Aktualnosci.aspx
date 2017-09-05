<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="Aktualnosci.aspx.cs" Inherits="HRRcp.Portal.Aktualnosci" %>
<%@ Register src="../Controls/Portal/cntArticles3.ascx" tagname="cntArticles" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page960">
        <div class="spacer16"></div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
            <ContentTemplate>
                <uc1:cntArticles ID="cntArticles" runat="server" Mode="0" Grupa="ARTYKULY"/>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
