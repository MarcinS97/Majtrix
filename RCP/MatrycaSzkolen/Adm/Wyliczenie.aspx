<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true" CodeBehind="Wyliczenie.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.Adm.Wyliczenie" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/Adm/cntWyliczenie.ascx" TagPrefix="cc" TagName="cntWyliczenie" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
        <uc1:pagetitle id="PageTitle1" runat="server" title="Naliczenie ocen" SubText1="Formatka do naliczania ocen w datach" />
        <div class="center960 pgKartaZgloszenie">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <cc:cntWyliczenie id="cWyliczenie" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
