<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="Dane.aspx.cs" Inherits="HRRcp.Portal.PortalDane" %>
<%@ Register Src="~/Portal/Controls/cntSqlContent.ascx" TagName="cntSqlContent" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWide" runat="server">
    <div id="pgPortalDane">
        <div class="page-title">
            <i class="fa fa-user"></i>
            Dane pracownika
        </div>
        <div class="container wide page-content">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <uc1:cntSqlContent ID="cntSqlContent1" runat="server" Grupa="DANEP" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

