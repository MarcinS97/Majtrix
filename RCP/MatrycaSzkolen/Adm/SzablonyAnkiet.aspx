<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true" CodeBehind="SzablonyAnkiet.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.Adm.SzablonyAnkiet" %>

<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="pgUprawnieniaSzkol pgScParametry">
        <uc1:pagetitle id="PageTitle1" runat="server" title="Szablony ankiet" subtext1=" " />
        <div class="pageContent">

                        <span>Puste szablony ankiet:</span>
                        <asp:Button ID="btnPrintEmpTemplate" runat="server" OnClick="PrintEmployeeSurveyTemplate" CssClass="btn btn-sm btn-default" Text="Ankieta pracownika" />
                        <asp:Button ID="btnPrintSupTemplate" runat="server" OnClick="PrintSuperiorSurveyTemplate" CssClass="btn btn-sm btn-default" Text="Ankieta kierownika" />

        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
