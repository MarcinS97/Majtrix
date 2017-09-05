<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="Kalkulator.aspx.cs" Inherits="HRRcp.PL.Kalkulator" %>

<%@ Register Src="~/Controls/PodzialLudzi/cntKalkulator.ascx" TagPrefix="uc1" TagName="cntKalkulator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<%--        
glyphicon glyphicon-calendar
glyphicon glyphicon-subtitles
--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWide" runat="server">
    <div class="page-title">
        <i class="fa fa-calculator"></i>
        Kalkulator Podziału Ludzi
    </div>
    <%--<hr />--%>
    <div id="pgKalkulatorPL" class="center bg-white xcontainer xwide">
        <uc1:cntKalkulator runat="server" ID="cntKalkulator" />
    </div>
</asp:Content>





