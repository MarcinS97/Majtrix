<%@ Page Title="" Language="C#" MasterPageFile="~/Scorecards/Scorecards.Master" AutoEventWireup="true" CodeBehind="Scorecard.aspx.cs" Inherits="HRRcp.Scorecards.Scorecard" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc1" %>
<%@ Register Src="~/Scorecards/Controls/Spreadsheets/cntScorecard.ascx" TagPrefix="leet" TagName="Scorecard" %>
<%@ Register Src="~/Scorecards/Controls/Spreadsheets/cntScorecardMenu2.ascx" TagPrefix="leet" TagName="ScorecardMenu2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="HeadFooter" runat="server">
     <script type="text/javascript">
        //hideFooter();
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler2);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler2);

        function BeginRequestHandler2(sender, args) {
            //prepareSums();
            //if (typeof subBeginRequestHandler == 'function') subBeginRequestHandler();
            //alert(1);
        }

        function EndRequestHandler2(sender, args) {
            prepareJS();
            //prepareSums();
            //if (typeof subEndRequestHandler == 'function') subEndRequestHandler();
            //resize();
            //alert(2);
        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="pgScorecards">
        <uc1:PageTitle ID="PageTitle1" runat="server"
            Ico="../images/captions/layout_edit.png"
            Title=""
        />
        <div class="pageContent">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <leet:ScorecardMenu2 ID="ScorecardMenu2" runat="server" OnSaved="SaveScorecard" />
                    <leet:Scorecard ID="ScorecardActual" runat="server" InEdit="true" HeaderVisible="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
