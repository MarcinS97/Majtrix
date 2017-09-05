<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="AbsencjeDlugotrwale.aspx.cs" Inherits="HRRcp.AbsencjeDlugotrwale" %>
<%@ Register src="~/Controls/PageTitle.ascx" tagname="PageTitle" tagprefix="uc1" %>
<%@ Register src="~/Controls/PlanUrlopow/cntPominAdm.ascx" tagname="cntPominAdm" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle runat="server" ID="PageTitle" Title="Absencje długotrwałe" />
    <div class="form-page pgAbsencjeDlugotrwale">
        <div id="paRozlNadg" runat="server" class="xborder xfiszka" >
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <uc2:cntPominAdm ID="cntPominAdm" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
