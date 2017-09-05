<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RapAbsencje.aspx.cs" Inherits="HRRcp.RapAbsencje" 
    EnableEventValidation="false" %>
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

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hidReport" runat="server" />
    <asp:HiddenField ID="hidReportTitle" runat="server" />

    <div id="cntReportHeaderM" runat="server" class="caption4 caption RaportF_title printoff" >
        <h1>
            <i class="fa fa-bar-chart"></i>
            Raporty
        </h1>
        <div class="rep-buttons pull-right" >
            <asp:Button ID="btEdit" runat="server" CssClass="button button_tech" Text="Edytuj" Visible="false" OnClick="btEdit_Click" />
            <asp:Button ID="btNew" runat="server" CssClass="button button_tech" Text="Dodaj nowy raport" Visible="false" OnClick="btNew_Click" />
            <asp:Button ID="btDelete" runat="server" CssClass="button75 button_tech" Text="Usuń" Visible="false" OnClick="btDelete_Click" />

            <asp:LinkButton ID="btRepBack1"  CssClass="icon-white" runat="server" ToolTip="Powrót"             Text="Powrót" OnClientClick="history.back();return false;" ><i class="fa fa-chevron-circle-left"></i></asp:LinkButton>
            <asp:LinkButton ID="btRepPrint1" CssClass="icon-blue"  runat="server" ToolTip="Drukuj"             Text="Drukuj" OnClientClick="printPreview();return false;" ><i class="fa fa-print"></i></asp:LinkButton>
            <asp:LinkButton ID="btnPDF"      CssClass="btn-pdf"    runat="server" ToolTip="Pobierz jako PDF"   Text="PDF"    OnClick="btnPDF_Click"><i class="fa fa-file-pdf-o"></i></asp:LinkButton>
            <asp:LinkButton ID="btRepExcel1" CssClass="btn-excel"  runat="server" ToolTip="Pobierz jako Excel" Text="Excel"  OnClientClick="return exportExcelClick3();" OnClick="btExcel_Click" ><i class="fa fa-file-excel-o"></i></asp:LinkButton>
        </div>
    </div>

    <hr />

    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="pdfwrapper" class="report_page RaportF" runat="server" pdf="true" >
                <uc1:cntReport2 ID="cntReportHeader" runat="server" GridVisible="false" CssClass="report_page noborder report-header" Visible="true" Title=""  />
                <uc2:cntFilter ID="cntFilter" runat="server" OnClear="cntFilter_Clear" OnEdit="cntFilter_Edit" OnEndEdit="cntFilter_EndEdit" />
                <div class="grid-wrapper">
                    <asp:GridView ID="gvReport" CssClass="GridView1" runat="server" AutoGenerateColumns="true" DataSourceID="SqlDataSource1" PageSize="20" AllowPaging="false" AllowSorting="true" OnDataBound="GridView1_DataBound" >
                    </asp:GridView>
                </div>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false" OnSelected="SqlDataSource1_Selected" >
                </asp:SqlDataSource>
            </div>          
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upEdit" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc3:cntSqlReportEdit ID="cntSqlReportEdit" runat="server" Visible="false" OnSave="cntSqlReportEdit_Save"/>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%-- Absencja PDF --%>
    <asp:Repeater ID="rpPracownicy" runat="server" DataSourceID="dsPracownicy" OnItemDataBound="rpPracownicy_ItemDataBound">
        <ItemTemplate>
            <div class="list-page break rap-absencja">
                <div class="header">
                    Absencje w okresie <%# Eval("od") %> - <%# Eval("do") %><br />
                    Pracownik: <%# Eval("prac") %> <%# Eval("KadryId") %>
                </div>
                <div class="report">
                    <asp:HiddenField ID="hidPracId" runat="server" Visible="false" Value='<%# Eval("Id") %>'/>
                    <asp:GridView ID="gvAbsencjePrac" CssClass="GridView1" runat="server" AutoGenerateColumns="true" DataSourceID="dsAbsencjePrac" PageSize="20" AllowPaging="false" AllowSorting="true" OnDataBound="GridView1_DataBound" >
                    </asp:GridView>
                    <asp:SqlDataSource ID="dsAbsencjePrac" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false" OnSelected="dsAbsencjePrac_Selected" 
                        SelectCommand="
                select top 10 * from Absencja where IdPracownika = @pracId
                        ">
                        <SelectParameters>
                            <asp:ControlParameter Name="pracId" ControlID="hidPracId" PropertyName="Value" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <div class="footer">
                    Potwierdzam: ...................................
                    Data: <%# Eval("dzis", "{0:D}") %>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>


    <asp:SqlDataSource ID="dsPracownicy" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false" OnSelected="dsPracownicy_Selected" 
        SelectCommand="
declare @od datetime
declare @do datetime
set @od = '20120101'
set @do = '20121231'
declare @dzis datetime
set @dzis = dbo.getdate(GETDATE())

select top 10 
  @od [od]
, @do [do]
, @dzis [dzis]
, Nazwisko + ' ' + Imie [prac]
, * 
from Pracownicy 
where Status in (0,1)
        " >
    </asp:SqlDataSource>




</asp:Content>

