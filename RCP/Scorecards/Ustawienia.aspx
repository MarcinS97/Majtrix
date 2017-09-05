<%@ Page Title="" Language="C#" MasterPageFile="~/Scorecards/Scorecards.Master" AutoEventWireup="true" CodeBehind="Ustawienia.aspx.cs" Inherits="HRRcp.Scorecards.Ustawienia" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="pgScUstawienia">
        <uc1:PageTitle ID="PageTitle1" runat="server"
            Ico="../images/captions/layout_edit.png"
            Title="Ustawienia"
        />
        <div class="pageContent">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
