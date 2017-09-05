<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="HRRcp.Portal.Search" %>

<%@ Register Src="~/Portal/Controls/cntSearch.ascx" TagName="cntSearch" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgPage960_16 border">
        <div class="page-title">Wyniki wyszukiwania:</div>
        <%--<div class="title5">
            <img alt="" src="../images/captions/AnkietaView.png"/>
            <span>Wyniki wyszukiwania</span>        
        </div>--%>
        <div class="container">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <uc1:cntSearch ID="cntSearch" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
