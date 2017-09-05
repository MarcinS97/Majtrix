<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="SqlMenu.aspx.cs" Inherits="HRRcp.Portal.SqlMenu" %>

<%@ Register Src="~/Portal/Controls/cntSqlMenu.ascx" TagPrefix="uc1" TagName="cntSqlMenu" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

            <uc1:cntSqlMenu runat="server" id="cntSqlMenu" />
 
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
