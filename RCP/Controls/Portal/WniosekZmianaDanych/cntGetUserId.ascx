<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntGetUserId.ascx.cs" Inherits="HRRcp.Controls.WniosekZmianaDanych.cntGetUserId" %>
<div id="cntGetUserId">
<asp:DropDownList DataSourceID="SqlDataSource1" ID="DropDownList1" runat="server">
    
</asp:DropDownList>
</div>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="select Id,Imie [Imię],Nazwisko [Nazwisko] from Pracownicy where Admin!=1 and Kierownik!=1"></asp:SqlDataSource>
