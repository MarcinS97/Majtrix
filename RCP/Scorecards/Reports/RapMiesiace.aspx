<%@ Page Title="" Language="C#" EnableEventValidation="false"  MasterPageFile="~/Scorecards/Report.Master" AutoEventWireup="true" CodeBehind="RapMiesiace.aspx.cs" Inherits="HRRcp.Scorecards.Reports.RapMiesiace" %>

<%@ Register Src="~/Controls/Reports/cntReport2.ascx" TagPrefix="leet" TagName="Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

      <leet:Report
                    ID="Rep1"
                    runat="server"
                    CssClass="none"
                    Title="Miesiące"
                    DivClass="report_page RepCCUprawnienia"
                    Pager="true"
                    Pages="12"
                    SQL="
select 
DataOd [data:-]
, convert(varchar(7), DataOd, 20) as [Miesiac|RapRozdzielnik @data]
from OkresyRozl
order by DataOd desc
"
 />
 </ContentTemplate>
 </asp:UpdatePanel>

</asp:Content>
