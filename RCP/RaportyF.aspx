<%@ Page Title="" Language="C#" MasterPageFile="~/WAdmin/WAdmin.Master" AutoEventWireup="true" CodeBehind="RaportyF.aspx.cs" Inherits="PreciToolB2BInit.WAdmin.RaportyF" %>
<%@ Register Src="~/WAdmin/Controls/Reports/cntSqlReports.ascx" TagPrefix="uc1" TagName="cntSqlReports" %>
<%@ Register src="~/WAdmin/Controls/Reports/cntSqlReportEdit.ascx" tagname="cntSqlReportEdit" tagprefix="uc3" %>
<%@ Register src="~/WAdmin/Controls/Reports/cntReportScheduler.ascx" tagname="cntReportScheduler" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="paEdit" runat="server" class="paEdit printoff" visible="false">
                <div class="buttons">
                    <asp:Button ID="btNew" runat="server" CssClass="button button_tech" Text="Dodaj nowy raport" OnClick="btNew_Click" />
                </div>    
            </div>    
            <h2>Raporty</h2>
            <uc1:cntSqlReports runat="server" ID="cntSqlReports" Grupa="RAPORTY" OnSelectedChanged="cntSqlReports_SelectedChanged" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upEdit" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc3:cntSqlReportEdit ID="cntSqlReportEdit" runat="server" Visible="false" OnSave="cntSqlReportEdit_Save"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upScheduler" runat="server" Visible="false" >
        <ContentTemplate>
            <uc4:cntReportScheduler ID="cntReportScheduler" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
