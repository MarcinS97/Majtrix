<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="Content.aspx.cs" Inherits="HRRcp.Portal.ContentForm" ValidateRequest="false" %>
<%@ Register src="~/Portal/Controls/cntArticles.ascx" tagname="cntArticles" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="xpgArticles pgContent border">
        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
                <uc1:cntArticles ID="cntArticles" runat="server" Mode="0" PageSize="0"/>
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
</asp:Content>
