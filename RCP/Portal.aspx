<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="Portal.aspx.cs" Inherits="HRRcp.PortalPrac"  ValidateRequest="false" EnableEventValidation="false"   %>

<%--
    ValidateRequest="true" 
--%>

<%@ Register Src="~/Portal/Controls/cntArticles.ascx" TagName="cntArticles" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <%-- <script type="text/javascript">
        function pageLoad() {
            $(function () {
                prepareTinyMce('.editor');
            });
        }
    </script>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pgArticles border">
        <uc1:cntArticles ID="cntArticles" runat="server" Grupa="ARTYKULY" Title="Artykuły" />
    </div>
</asp:Content>
