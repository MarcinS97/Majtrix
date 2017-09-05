<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntScorecardSelect.ascx.cs" Inherits="HRRcp.Scorecards.Controls.cntScorecardSelect" %>
<%@ Register src="~/Controls/PodzialLudzi/cntSelectRokMiesiac.ascx" tagname="cntSelectRokMiesiac" tagprefix="uc2" %>

<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="paScorecardSelect" runat="server" class="cntScorecardSelect">

            <div class="">

            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
