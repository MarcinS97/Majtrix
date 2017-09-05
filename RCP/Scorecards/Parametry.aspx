<%@ Page Title="" Language="C#" MasterPageFile="~/Scorecards/Scorecards.Master" AutoEventWireup="true" CodeBehind="Parametry.aspx.cs" Inherits="HRRcp.Scorecards.Parametry" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc1" %>

<%@ Register Src="~/Scorecards/Controls/Admin/cntParameters.ascx" TagPrefix="leet" TagName="Parameters" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="pgScParametry">
        <uc1:PageTitle ID="PageTitle1" runat="server"
            Ico="../images/captions/layout_edit.png"
            Title="Scorecard'y - Parametry"
        />
        <div class="pageContent">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <leet:Parameters ID="Parameters" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
