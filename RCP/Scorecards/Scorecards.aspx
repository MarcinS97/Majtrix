<%@ Page Title="" EnableEventValidation="false" Language="C#"  MasterPageFile="~/Scorecards/Scorecards.Master" AutoEventWireup="true" CodeBehind="Scorecards.aspx.cs" Inherits="HRRcp.Scorecards.Scorecards" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc1" %>

<%--<%@ Register src="Controls/cntScorecardSelect.ascx" tagname="cntScorecardSelect" tagprefix="uc2" %>--%>

<%@ Register Src="~/Scorecards/Controls/Spreadsheets/cntSpreadsheet.ascx" TagPrefix="leet" TagName="Spreadsheet" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="HeadFooter" runat="server">
     <script type="text/javascript">
        //hideFooter();
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler1);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler1);

        function BeginRequestHandler1(sender, args) {
            //prepareSums();
            //if (typeof subBeginRequestHandler == 'function') subBeginRequestHandler();
            //alert(1);
        }

        function EndRequestHandler1(sender, args) {
            prepareJS();
            //prepareSums();
            //if (typeof subEndRequestHandler == 'function') subEndRequestHandler();
            //resize();
            //alert(2);
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="pgScorecards">
<%--        <uc1:PageTitle ID="PageTitle1" runat="server"
            Ico="../images/captions/layout_edit.png"
            Title="Scorecards"
        />--%>
<%--        <div class="pageContent">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>--%>
                    <leet:Spreadsheet ID="Spreadsheet" runat="server" />
<%--                </ContentTemplate>
            </asp:UpdatePanel>
        </div>--%>
    </div>    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
