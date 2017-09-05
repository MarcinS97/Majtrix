<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true"
    CodeBehind="Widelki.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.Adm.Widelki" %>

<%@ Register Src="~/MatrycaSzkolen/Controls/Adm/cntWidelki.ascx" TagPrefix="cc" TagName="cntWidelki" %>
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
    <div class="pgScParametry">
        <uc1:pagetitle id="PageTitle1" runat="server" title="Kryteria oceny" SubText1="Parametry ocen ustawiane w datach" />
        <div class="pageContent">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <cc:cntWidelki ID="WidelkiC" runat="server" Percent="0" Unit="" Type="0" Title="Doświadczenie" Subtitle="Wybierz linię aby zawęzić zakres" ValidationGroup="w0" />
                    <cc:cntWidelki ID="CntWidelki2" runat="server" Percent="0" Unit="" Type="1" Title="Wydajność" Subtitle="Wybierz linię aby zawęzić zakres" ValidationGroup="w0" />
                    <cc:cntWidelki ID="CntWidelki1" runat="server" Percent="0" Unit="" Type="2" Title="Braki" Subtitle="Wybierz linię aby zawęzić zakres" ValidationGroup="w0" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
