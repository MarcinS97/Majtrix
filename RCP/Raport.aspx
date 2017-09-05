<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="Raport.aspx.cs" Inherits="HRRcp.Raport" %>
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

                <uc1:cntReportHeader ID="cntReportHeader" runat="server" Caption="Raport" />

                <div id="paFilter" runat="server" class="paFilter" visible="false">
                    <table>
                        <tr>
                            <td class="col2">
                                <asp:Label ID="Label2" runat="server" Text="Program:" CssClass="label" />
                                <leet:Filter ID="fltrProgram"
                                    runat="server"
                                    PostBackButtonId="btnFilter"
                                    ValueField="Id"
                                    Type="DropDownList"
                                    TextField="Name"
                                    Token="KO.IdProgramu"
                                    ReportId="Rep1"
                                    ChooseString="false"
                                    Command="
--select *, Id, left(CONVERT(VARCHAR, DataOcenyOd, 20), 10) + ' - ' + left(CONVERT(VARCHAR, DataOcenyDo, 20), 10) as Name, case when Status = 2 then 0 else 1 end as Ord
--from Programy
--order by Ord, DataOcenyOd desc
                                " />
                                <br />

                                <asp:Label ID="Label1" runat="server" Text="Struktura organizacyjna:" class="label" CssClass="label" />
                                <asp:DropDownList ID="ddlStructureFilter" runat="server" /><br />
                                <asp:Label ID="Label10" runat="server" Text="Lider:" class="label" CssClass="label" />
                                <leet:Filter ID="fltrLeaders"
                                    runat="server"
                                    PostBackButtonId="btnFilter"
                                    ValueField="Id"
                                    Type="DropDownList"
                                    TextField="Name"
                                    Token="PA.IdPrzelozonego"
                                    ReportId="Rep1"
                                    Command="
--select Id_Przelozeni as Id, case when Status = -1 then '* ' else '' end + Nazwisko + ' ' + Imie + ' ' + isnull(' (' + Nr_Ewid + ')', '') as Name, case when Status = -1 then 1 else 0 end as Ord
--from Przelozeni
--where Status &gt; -2         
--order by Ord, Name
" />

                            </td>
                            <td class="sep"></td>
                            <td class="bottom_buttons">
                                <asp:Button ID="btnFilter" runat="server" Text="Filtruj" OnClick="btnFilter_Click" CssClass="button100" />
                                <%--<asp:Button ID="btnClear" runat="server" CssClass="button75" Text="Czyść" OnClick="btnClear_Click" />--%>
                            </td>
                        </tr>
                    </table>
                </div>

                <leet:cntReport2 runat="server" ID="cntReport2" />

<%--                <leet:Report
                    ID="cntReport"
                    runat="server"
                    DivClass="none"
                    Pager="false"
                    Charts="Radar"
                    ChartsColors="rgba(0,0,0,0)|rgba(0,0,0,0)|rgba(0,0,0,0);rgba(0,0,0,0)|rgba(0,0,0,0)|rgba(0,0,0,0);rgba(0,0,0,0)|rgba(0,0,0,0)|rgba(0,0,0,0);rgba(0,0,0,0)|rgba(0,0,0,0)|rgba(0,0,0,0);rgba(0,0,0,0)|rgba(0,0,0,0)|rgba(0,0,0,0);rgba(0,0,0,0)|rgba(0,0,0,0)|rgba(0,0,0,0);rgba(0, 0, 192, 0.0)|#507CD1"
                    ChartsOptions="pointLabelFontSize: 16, datasetStrokeWidth : 6, angleLineColor : 'black', scaleLineColor: 'black', showTooltips: false"
                 />
    --%>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
