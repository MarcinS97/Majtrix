<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="AkceptacjaCzasuPracy.aspx.cs" Inherits="HRRcp.RCP.AkceptacjaCzasuPracy" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register src="~/RCP/Controls/cntPlanPracyAccept.ascx" tagname="cntPlanPracyAccept" tagprefix="uc1" %>

<%--
    ValidateRequest="false"
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Akceptacja czasu pracy" SubText1="Zaakceptuj czas pracy pracowników" />
    <div class="form-page pgAkceptacjaCzasuPracy">
        <uc1:cntPlanPracyAccept ID="cntPlanPracyAccept" runat="server" />
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript">
        $(document).on("ready", function () {

            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler1);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler1);

            function BeginRequestHandler1(sender, args) {
                ct420 = $('#paAcceptScroll').is(":visible")
            }

            function EndRequestHandler1(sender, args) {
                if (ct420)
                {
                    $('#paAcceptScroll').show()
                }
            }
        });
    </script>
</asp:Content>
