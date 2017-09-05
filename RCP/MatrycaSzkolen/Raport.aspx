<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Scorecards/Report.Master" AutoEventWireup="true" CodeBehind="Raport.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.Raport" %>
<%@ Register Src="~/Controls/Reports/cntReportHeader.ascx" TagName="cntReportHeader" TagPrefix="uc1" %>

<%@ Register Src="~/Controls/EliteReports/cntReport.ascx" TagPrefix="leet" TagName="Report" %>
<%@ Register Src="~/Controls/EliteReports/cntFilter.ascx" TagPrefix="leet" TagName="Filter" %>
<%@ Register Src="~/Controls/Reports/cntReport2.ascx" TagPrefix="leet" TagName="cntReport2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">

    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="report_page">
                <leet:cntReport2 runat="server" ID="cntReportHeader" GridVisible="false" CssClass="report_page noborder"  />
                <leet:cntReport2 runat="server" ID="cntReport2" CssClass="report_page noborder" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
