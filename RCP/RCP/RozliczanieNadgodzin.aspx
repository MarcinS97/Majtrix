<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="RozliczanieNadgodzin.aspx.cs" Inherits="HRRcp.RCP.RozliczanieNadgodzin" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register src="~/Controls/PlanPracyRozliczenie.ascx" tagname="PlanPracyRozliczenie" tagprefix="uc1" %>
<%@ Register Src="~/RCP/Controls/cntSelectKier.ascx" TagPrefix="uc1" TagName="cntSelectKier" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Rozliczanie nadgodzin" SubText1="" />
    <div class="form-page pgRozliczenieNadgodzin">
        <div id="paSearch" runat="server" class="search">
            <div>
                <asp:Label ID="Label1" runat="server" Text="Wyszukaj pracownika:" Visible="false"></asp:Label>

                <%-- nie działą Visible="false" --%>
                <asp:TextBox ID="tbSearch" runat="server" CssClass="form-control" MaxLength="250" Placeholder="Wyszukaj pracownika ..." Visible="false" ></asp:TextBox>
                <asp:Button ID="btClear" runat="server" CssClass="btn-default" Text="Czyść" Visible="false" />
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="filter">  
                    <uc1:cntSelectKier runat="server" id="cntSelectKier" OnDataBound="ddlKier_DataBound" OnSelectedIndexChanged="ddlKier_SelectedIndexChanged" />
                </div>
                <asp:Button ID="btSearch" runat="server" Text="Wyszukaj" CssClass="button_postback" onclick="btSearch_Click" /> 
                <uc1:PlanPracyRozliczenie ID="PlanPracyRozliczenie" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>




