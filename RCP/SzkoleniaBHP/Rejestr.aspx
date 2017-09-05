<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Rejestr.aspx.cs" Inherits="HRRcp.SzkoleniaBHP.Rejestr" %>
<%@ Register src="~/SzkoleniaBHP/Controls/cntUprawnienia.ascx" tagname="cntUprawnienia" tagprefix="uc1" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="pgSzkoleniaBHP">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:cntUprawnienia ID="cntUprawnienia1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>    
    <asp:UpdateProgress ID="updProgress1" runat="server" DisplayAfter="10">
        <ProgressTemplate>
            <div class="updProgress1">
                <div class="center">
                    <img id="Img1" alt="Indicator" src="~/images/activity.gif" runat="server"/> 
                    <span>Trwa przetwarzanie. Proszę czekać ...</span>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
