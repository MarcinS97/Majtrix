<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RaportLimityNN.aspx.cs" Inherits="HRRcp.RaportLimityNN" %>
<%@ Register src="../Controls/PodzialLudzi/cntRapLimityNN.ascx" tagname="cntRaport" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
    <uc1:cntRaport ID="cntRaport" runat="server" Mode="0" />
</asp:Content>
