<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="PlikiKier.aspx.cs" Inherits="HRRcp.Portal.PlikiKier" %>
<%@ Register src="~/Portal/Controls/cntPliki.ascx" tagname="cntPliki" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgArticles border">
        <uc1:cntPliki ID="cntPliki1" runat="server" Grupa="FILEK"/>        
    </div>
</asp:Content>
