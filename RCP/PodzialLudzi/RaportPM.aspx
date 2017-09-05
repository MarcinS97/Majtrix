<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RaportPM.aspx.cs" Inherits="HRRcp.RaportPM" %>
<%@ Register src="../Controls/PodzialLudzi/cntRaportPM.ascx" tagname="cntRaportPM" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
    <uc1:cntRaportPM ID="cntRaportPM1" runat="server" Mode="0" />
</asp:Content>
