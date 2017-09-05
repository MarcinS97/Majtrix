<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="ListaPlacowek.aspx.cs" Inherits="HRRcp.Portal.Ubezpieczenia.ListaPlacowek" %>
<%@ Register Src="~/Controls/Reports/cntReport2.ascx" TagPrefix="uc1" TagName="cntReport2" %>
<%@ Register src="~/Controls/Reports/cntFilter2.ascx" tagname="cntFilter" tagprefix="uc2" %>
<%@ Register src="~/Controls/Reports/cntSqlReportEdit.ascx" tagname="cntSqlReportEdit" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .viewer{
            width: 100%;
            min-height: 800px;
             }
        .pgWarunkiUbezpieczenia .buttons { text-align: right; margin-top: 16px; }
        .paPDFViewer .paEditButton .btn { margin-bottom: 16px; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-title">
        Wykaz placówek medycznych
    </div>
    <div class="pgListaPlacowek container wide">
        <asp:UpdatePanel ID="upEdit" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc3:cntSqlReportEdit ID="cntSqlReportEdit" runat="server" Visible="false" OnSave="cntSqlReportEdit_Save"/>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional" > 
            <ContentTemplate>
                <uc2:cntFilter ID="cntFilter" runat="server" OnClear="cntFilter_Clear" OnEdit="cntFilter_Edit" OnEndEdit="cntFilter_EndEdit" />
                <asp:Button ID="btEdit" runat="server" Text="Edytuj raport" OnClick="btEdit_Click" Visible="false" />
                <div class="grid-wrapper">
                    <asp:GridView ID="gvReport" CssClass="GridView1" runat="server" AutoGenerateColumns="true" DataSourceID="SqlDataSource1" PageSize="50" AllowPaging="true" AllowSorting="true" OnDataBound="GridView1_DataBound" >
                    </asp:GridView>
                </div>
    <%--
                <uc1:cntReport2 ID="cntReport2" runat="server" Visible="false" PageSize="20" AllowPaging="true" />
    --%>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false" OnSelected="SqlDataSource1_Selected" >
                </asp:SqlDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
