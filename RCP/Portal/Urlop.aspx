<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="Urlop.aspx.cs" Inherits="HRRcp.Portal.UrlopForm" %>

<%@ Register Src="~/Controls/Kwitek/Urlop.ascx" TagName="Urlop" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page960 pgUrlopy">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="page-title">
                    Urlopy
                </div>
                <div class="container wide">
                    <uc1:Urlop ID="Urlop1" UMode="2" runat="server" />
                    <asp:LinkButton CssClass="button75 printoff btn btn-primary btn-print" ID="btPrint" runat="server" Text="Drukuj" OnClientClick="javascript:window.print();" Visible="false">
                    <i class="fa fa-print"></i>Drukuj
                    </asp:LinkButton>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
