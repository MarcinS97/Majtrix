<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="Pracownicy.aspx.cs" Inherits="HRRcp.RCP.Pracownicy" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register src="~/Controls/Adm/cntPracownicy3.ascx" tagname="cntPracownicy2" tagprefix="uc1" %>

<%--
<%@ Register src="~/Controls/Adm/cntPracownicy4.ascx" tagname="cntPracownicy2" tagprefix="uc1" %>
<%@ Register src="~/Controls/Adm/cntPracownicy3.ascx" tagname="cntPracownicy2" tagprefix="uc1" %>
--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Pracownicy" SubText1="" />
    <div class="form-page pgPracownicy">
        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:cntPracownicy2 ID="cntPracownicy2" OnSelectedChanged="cntPracownicy2_SelectedChanged" OnCommand="cntPracownicy2_Command" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
