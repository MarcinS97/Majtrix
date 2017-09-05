<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Rejestr.aspx.cs" Inherits="HRRcp.BadaniaWstepne.Rejestr" %>
<%@ Register src="Controls/cntBadaniaWst.ascx" tagname="cntBadaniaWst" tagprefix="uc1" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="pgBadniaWst">
        <uc1:cntBadaniaWst ID="cntBadaniaWst1" runat="server" />
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

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
