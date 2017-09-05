<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true" CodeBehind="Raporty.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.Raporty" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc2" %>
<%@ Register src="~/MatrycaSzkolen/Controls/cntSqlReports.ascx" tagname="cntSqlReports" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div id="pgRaportyForm">
        <div class="center" >
            <uc2:PageTitle ID="PageTitle1" runat="server" Title="Raporty i zestawienia" />
        </div>
        <div class="pageContent">
            <div class="center">
                <div id="paMy" runat="server" visible="true">
                  
                    <asp:Label ID="Label1" runat="server" CssClass="title"><i class="fa fa-user" style="margin-right: 6px;"></i>Moje zestawienia</asp:Label>
                    <uc1:cntSqlReports ID="cntSqlReportsMy" Grupa="REPORT" Mode="1" OnSelectedChanged="cntSqlReports_SelectedChanged" runat="server" />
                </div>

                <div id="paAll" runat="server" visible="false">
                    <asp:Label ID="lbAllTitle" runat="server" CssClass="title"><i class="fa fa-users" style="margin-right: 6px;"></i>Zestawienia dla pracowników</asp:Label>
                    <uc1:cntSqlReports ID="cntSqlReportsAll" Grupa="REPORT" Mode="2" OnSelectedChanged="cntSqlReportsAll_SelectedChanged" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
