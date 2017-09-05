<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="Przypisania.aspx.cs" Inherits="HRRcp.RCP.Przypisania" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register src="~/MatrycaSzkolen/Controls/Przypisania/cntPrzesunieciaKier.ascx" tagname="cntPrzesuniecia" tagprefix="cc" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/Przypisania/cntPrzesunieciaAdm.ascx" TagPrefix="cc" TagName="cntPrzesunieciaAdm" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Przypisania pracowników" SubText1="" />
    <div class="form-page pgPrzypisania">
        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <cc:cntPrzesuniecia ID="cntPrzesuniecia" runat="server" Visible="false" />
                <cc:cntPrzesunieciaAdm runat="server" ID="cntPrzesunieciaAdm" Visible="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
