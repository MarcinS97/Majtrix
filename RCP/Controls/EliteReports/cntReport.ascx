<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntReport.ascx.cs" Inherits="HRRcp.Controls.EliteReports.cntReport" %>
<%@ Register src="~/Controls/Reports/cntReportHeader.ascx" tagname="cntReportHeader" tagprefix="uc1" %>
<%@ Register Src="~/Controls/EliteReports/cntChart.ascx" TagPrefix="leet" TagName="Chart" %>

<asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="MainDiv" runat="server" class="border">
        
            <asp:HiddenField ID="hidSQL" runat="server" Visible="false"/>
            <asp:HiddenField ID="hidCheck" runat="server" Visible="false"/>
                    
            <uc1:cntReportHeader ID="cntReportHeader1" runat="server" />
            
            <asp:Button ID="btnConvertToPdf" runat="server" Text="PDF" Visible="false" OnClick="ConvertToPDF" />
            
            <div class="ddlCharts">
                <asp:DropDownList 
                    ID="ddlCharts" 
                    runat="server"
                    OnSelectedIndexChanged="ddlCharts_Selected"
                    AutoPostBack="true"
                    CssClass="printoff" 
                    style="margin-bottom: 16px;"
                    onload="ddlCharts_Load"
                    >
                        <asp:ListItem Value="" Text="Wybierz wykres" Selected="True"/>
                        <asp:ListItem Value="0" Text="Liniowy" Enabled="false" />
                        <asp:ListItem Value="1" Text="Słupkowy" Enabled="false" />
                        <asp:ListItem Value="2" Text="Kołowy" Enabled="false" />                
                        <asp:ListItem Value="3" Text="Radar" Enabled="false" />
                        <asp:ListItem Value="4" Text="Polar" Enabled="false" />
                        <asp:ListItem Value="5" Text="Doughnut" Enabled="false" />
                </asp:DropDownList>     
            </div>   
        
            <leet:Chart id="MainChart" runat="server" Width="900" Height="550"  />
<%--
            <div id="paFilter" runat="server" class="cntEliteReport" visible="false">
--%>        
            <div class="cntEliteReport" >
                <asp:Literal ID="MainTable" runat="server" />
            </div>
        
            <asp:PlaceHolder ID="pControls" runat="server" Visible="false" >
            </asp:PlaceHolder>
            <asp:Panel ID="PagerLinks" runat="server" Visible="false">
            </asp:Panel>
            <asp:DropDownList ID="ddlPager" Visible="false" AutoPostBack="true" runat="server" 
                onselectedindexchanged="ddlPager_SelectedIndexChanged">
                <asp:ListItem Value="10">10</asp:ListItem>
                <asp:ListItem Value="20">20</asp:ListItem>
                <asp:ListItem Value="50">50</asp:ListItem>
                <asp:ListItem Value="100">100</asp:ListItem>
                <asp:ListItem Value="all">ALL</asp:ListItem>
            </asp:DropDownList>                 
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="ddlCharts" />
        <asp:PostBackTrigger ControlID="btnConvertToPdf" />
    </Triggers>
</asp:UpdatePanel>