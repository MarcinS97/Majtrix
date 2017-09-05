<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true" CodeBehind="Akceptacje.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.Akceptacje" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc2" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/cntAkceptacje.ascx" TagPrefix="cc" TagName="cntAkceptacje" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <uc2:PageTitle ID="PageTitle1" runat="server" Title="Akceptacje" SubText1="Akceptacje certyfikatów" />
    <div class="pgAkceptacje xcenter960">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <cc:cntAkceptacje id="cAkceptacje" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
