<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.Master" AutoEventWireup="true" CodeBehind="Harmonogram2.aspx.cs" Inherits="HRRcp.RCP.Harmonogram2" %>

<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register Src="~/RCP/Controls/cntPlanPracyZmiany2.ascx" TagPrefix="uc1" TagName="cntPlanPracyZmiany" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Plan pracy" SubText1="Planowanie czasu pracy" />
    <div class="form-page pgHarmonogram">
        <uc1:cntPlanPracyZmiany runat="server" ID="cntPlanPracyZmiany" />
    </div>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler1);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler1);

        function BeginRequestHandler1(sender, args) {
        }

        function EndRequestHandler1(sender, args) {
            preparePP();
        }
    </script>
</asp:Content>


<%--
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
--%>

