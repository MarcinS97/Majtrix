<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RaportPDF.aspx.cs" Inherits="HRRcp.RaportPDF" 
EnableEventValidation="false"
%>
<%--
<%@ Register Src="~/Controls/Reports/cntReportHeader.ascx" TagName="cntReportHeader" TagPrefix="uc1" %>
--%>
<%@ Register Src="~/Controls/Reports/cntReport2.ascx" TagPrefix="uc1" TagName="cntReport2" %>
<%@ Register src="~/Controls/Reports/cntFilter2.ascx" tagname="cntFilter" tagprefix="uc2" %>
<%@ Register src="~/Controls/Reports/cntSqlReportEdit.ascx" tagname="cntSqlReportEdit" tagprefix="uc3" %>
<%@ Register src="~/Controls/Reports/cntReportScheduler.ascx" tagname="cntReportScheduler" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
    <link href="<%# ResolveUrl("~/styles/print.css") %>" rel="stylesheet" media="print" type="text/css" />
    <script type="text/javascript">
        function exportExcelClick3() {
            return exportExcel('<%=hidReport.ClientID%>');
        }
    </script>
</asp:Content>

<%--
        $(document).ready(function () {
            var ctlTd = $('.GridView1 td');
            if (ctlTd.length > 0) {
                //console.log('Found ctlTd');
                ctlTd.wrapInner('<div class="avoidBreak" />');
            }
        });
--%>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hidReport" runat="server" />
    <asp:HiddenField ID="hidReportTitle" runat="server" />
    <div id="cntReportHeaderM" runat="server" class="caption4 caption RaportF_title printoff">
        <asp:Label ID="lbTitle" CssClass="title" runat="server" Text="Raporty"></asp:Label>
<%--    <asp:LinkButton ID="btnBack" runat="server" Text="Powrót" CssClass="btn btn-sm btn-default pull-right" PostBackUrl="~/MatrycaSzkolen/Ewaluacja.aspx" />--%>
        <div class="rep-buttons pull-right" >
            <asp:Button ID="btEdit" runat="server" CssClass="button button_tech" Text="Edytuj" Visible="false" OnClick="btEdit_Click" />
            <asp:Button ID="btNew" runat="server" CssClass="button button_tech" Text="Dodaj nowy raport" Visible="false" OnClick="btNew_Click" />
            <asp:Button ID="btDelete" runat="server" CssClass="button75 button_tech" Text="Usuń" Visible="false" OnClick="btDelete_Click" />

            <asp:LinkButton ID="btRepBack1" class="icon-white" runat="server" ToolTip="Powrót"
                Text="Powrót" OnClientClick="history.back();return false;" ><i class="fa fa-chevron-circle-left"></i></asp:LinkButton>
            <asp:LinkButton ID="btRepPrint1" class="icon-blue" runat="server" ToolTip="Drukuj"
                Text="Drukuj" OnClientClick="printPreview();return false;" ><i class="fa fa-print"></i></asp:LinkButton>
            <asp:LinkButton ID="btnPDF" class="btn-pdf" runat="server" ToolTip="Pobierz jako PDF" 
                Text="PDF" OnClick="btnPDF_Click"><i class="fa fa-file-pdf-o"></i></asp:LinkButton>
            <asp:LinkButton ID="btRepExcel1" class="btn-excel" runat="server" ToolTip="Pobierz jako Excel"
                Text="Excel" OnClientClick="return exportExcelClick3();" onclick="btExcel_Click" ><i class="fa fa-file-excel-o"></i></asp:LinkButton>
        </div>
    </div>
<%--<uc1:cntReport2 ID="cntReportHeaderM" runat="server" GridVisible="false" CssClass="report_page noborder" Visible="false"  />--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="pdfwrapper" class="report_page RaportF" runat="server" pdf="true" >
<%--                
                <div id="paEdit" runat="server" class="paEdit printoff" visible="false">
                    <div class="buttons">
                        <asp:Label ID="lbInfo" runat="server" CssClass="info" Visible="false" ></asp:Label>
                        <asp:Button ID="btEdit" runat="server" CssClass="button button_tech" Text="Edytuj" OnClick="btEdit_Click" />
                        <asp:Button ID="btNew" runat="server" CssClass="button button_tech" Text="Dodaj nowy raport" OnClick="btNew_Click" />
                        <asp:Button ID="btDelete" runat="server" CssClass="button75 button_tech" Text="Usuń" OnClick="btDelete_Click" />
                    </div>    
                </div>
--%>
<%--
                <uc1:cntReportHeader ID="cntReportHeader" runat="server" Caption="Raport" />
--%>
                <uc1:cntReport2 ID="cntReportHeader" runat="server" GridVisible="false" CssClass="report_page noborder report-header" Visible="true" Title=""  />
                <uc2:cntFilter ID="cntFilter" runat="server" OnClear="cntFilter_Clear" OnEdit="cntFilter_Edit" OnEndEdit="cntFilter_EndEdit" />
                <div class="grid-wrapper">
                    <asp:GridView ID="gvReport" CssClass="GridView1" runat="server" AutoGenerateColumns="true" DataSourceID="SqlDataSource1" PageSize="20" AllowPaging="false" AllowSorting="true" OnDataBound="GridView1_DataBound" >
                    </asp:GridView>
                </div>
<%--
                <uc1:cntReport2 ID="cntReport2" runat="server" Visible="false" PageSize="20" AllowPaging="true" />
--%>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false" OnSelected="SqlDataSource1_Selected" >
                </asp:SqlDataSource>

<%--                
                <div id="paPrintFooter" runat="server" class="print_footer" visible="false" >
                    <asp:Label ID="lbPrintFooter" class="left" runat="server" ></asp:Label>
                    <br />
                    <asp:Label ID="lbPrintTime" class="left" runat="server" ></asp:Label>
                </div>
--%>
            </div>          
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upScheduler" runat="server" Visible="false">
        <ContentTemplate>
            <uc4:cntReportScheduler ID="cntReportScheduler" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upEdit" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc3:cntSqlReportEdit ID="cntSqlReportEdit" runat="server" Visible="false" OnSave="cntSqlReportEdit_Save"/>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:PlaceHolder runat="server" id="jsScards" visible="false">
        <script type="text/javascript">
            function addFaces() {
                $("table.GridView1 td").each(function() {
                    $(this).html($(this).html().replace("_hap", "<img src='Scorecards/images/Happy64px.png' style='border-width:0px; width: 16px;'>"));
                    $(this).html($(this).html().replace("_sad", "<img src='Scorecards/images/Sad64px.png' style='border-width:0px; width: 16px;'>"));
                    $(this).html($(this).html().replace("_spook", "<img src='Scorecards/images/Skeleton64px.png' style='border-width:0px; width: 16px;'>"));
                });
            }

            //function BeginRequestHandler(sender, args) { }
            function EndRequestHandler(sender, args) {
                //addFaces();
            }

            $(document).on("ready", addFaces());
            //Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        </script>
    </asp:PlaceHolder>
</asp:Content>
