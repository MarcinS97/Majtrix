<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntReport2.ascx.cs" Inherits="HRRcp.Controls.Reports.cntReport2" %>
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
                ShowFooter="True" 
                PageSize="20" >
                <EmptyDataTemplate>
                    <%= GetNoDataInfo("Brak danych")%>
                </EmptyDataTemplate>
            </asp:GridView>
            <asp:Button ID="btExecute" runat="server" Text="Wykonaj" CssClass="button_postback" onclick="btExecute_Click"/>
            <asp:Button ID="gvReportCmd" runat="server" CssClass="button_postback" Text="Button" onclick="gvReportCmd_Click" />
            <asp:HiddenField ID="gvReportCmdPar" runat="server" />            
            <asp:HiddenField ID="gvReportSelected" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <div id="paDesc" runat="server" class="report_description" visible="false">
        <asp:Literal ID="ltDesc" runat="server"></asp:Literal>
    </div>
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    OnSelecting="SqlDataSource1_Selecting" OnSelected="SqlDataSource1_Selected"
    >
    <SelectParameters>
        <asp:SessionParameter DefaultValue="US" Name="lang" SessionField="LNG" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<%--
                onrowdatabound="gvReport_RowDataBound" 
                onprerender="gvReport_PreRender" 
                ondatabound="gvReport_DataBound" 
--%>

<%--
<script runat="server">
    protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.CommandTimeout = 0;
    }
</script>
--%>