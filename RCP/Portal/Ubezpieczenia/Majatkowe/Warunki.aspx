<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="Warunki.aspx.cs" Inherits="HRRcp.Portal.Ubezpieczenia.Majatkowe.Warunki" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .viewer{
            width: 100%;
            min-height: 800px;
             }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        
    <asp:Literal id="litData" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
