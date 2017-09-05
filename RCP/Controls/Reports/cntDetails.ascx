<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntDetails.ascx.cs" Inherits="HRRcp.Controls.Reports.cntDetails" %>
<%@ Register src="~/Controls/Reports/cntReportHeader.ascx" tagname="cntReportHeader" tagprefix="uc1" %>

<div id="paDetails" runat="server" class="report_page">
    <uc1:cntReportHeader ID="cntReportHeader1" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        
            <asp:DetailsView ID="dvDetails" runat="server" CssClass="DetailsView1 table table-striped table-bordered"
                DataSourceID="SqlDataSource1"                  
                PagerStyle-CssClass="pager"
                PagerSettings-Mode="NumericFirstLast"
                AutoGenerateRows="True" 
                OnDataBound="dvDetails_DataBound" 
                OnPreRender="dvDetails_PreRender" 
                ShowFooter="True" 
                EmptyDataText="Brak danych" 
                GridLines="None" >
                <PagerSettings FirstPageText="|◄" LastPageText="►|" Mode="NumericFirstLast" NextPageText="" PreviousPageText="◄" />
                <FooterStyle CssClass="footer" />
                <CommandRowStyle CssClass="control" />
                <FieldHeaderStyle CssClass="label" />
                <EmptyDataRowStyle CssClass="edt" />
                <PagerStyle CssClass="pager" />
                <HeaderStyle CssClass="header" />
                <InsertRowStyle CssClass="iit" />
                <EditRowStyle CssClass="eit" />
                <AlternatingRowStyle CssClass="alt" />
            </asp:DetailsView>
        
            
        
        
        
        <%--
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
            --%>
            
            <%--            <asp:Button ID="btExecute" runat="server" Text="Wykonaj" CssClass="button_postback" onclick="btExecute_Click"/> 
--%>
        </ContentTemplate>
    </asp:UpdatePanel>


    <div id="paDesc" runat="server" class="report_description" visible="false">
        <asp:Literal ID="ltDesc" runat="server"></asp:Literal>
    </div>
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" >
</asp:SqlDataSource>


