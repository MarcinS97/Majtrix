<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="Start.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.Start" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register Src="~/Controls/Reports/cntInfoBoard.ascx" TagPrefix="uc1" TagName="cntInfoBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
<%--    
    <div class="pgStart">
     <uc1:PageTitle ID="PageTitle1" runat="server" Ico="../images/captions/layout_edit.png" Title="Start" SubText1="Formatka startowa" />
        <div class="pageContent inline">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  asd
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>    
--%>
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Start" SubText1="Informacje podsumowujące / zadania do wykonania" />
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:cntInfoBoard runat="server" id="cntInfoBoard1" Grupa="STARTPAGE"/>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
