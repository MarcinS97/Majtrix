<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntReport.ascx.cs" Inherits="HRRcp.Controls.Reports.cntReport" %>
<%@ Register src="~/Controls/Reports/cntReportHeader.ascx" tagname="cntReportHeader" tagprefix="uc1" %>

<div id="divReport" runat="server" class="report_page">
    <uc1:cntReportHeader ID="cntReportHeader1" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvReport" runat="server" CssClass="GridView1" 
                DataSourceID="SqlDataSource1" 
                PagerStyle-CssClass="pager"
                PagerSettings-Mode="NumericFirstLast"
                AllowSorting="True" 
                AutoGenerateColumns="True" 
                onrowdatabound="gvReport_RowDataBound" 
                onprerender="gvReport_PreRender" 
                ondatabound="gvReport_DataBound" 
                ShowFooter="True" 
                PageSize="20" >
                <EmptyDataTemplate>
                    <%= GetNoDataInfo("Brak danych")%>
                </EmptyDataTemplate>
            </asp:GridView>
            <asp:Button ID="btExecute" runat="server" Text="Wykonaj" CssClass="button_postback" onclick="btExecute_Click"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <div id="paDesc" runat="server" class="report_description" visible="false">
        <asp:Literal ID="ltDesc" runat="server"></asp:Literal>
    </div>
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    onselecting="SqlDataSource1_Selecting" onselected="SqlDataSource1_Selected" 
    >
</asp:SqlDataSource>

