<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="Zastepstwa.aspx.cs" Inherits="HRRcp.RCP.Zastepstwa" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register Src="~/Controls/KierZastepstwa.ascx" TagPrefix="cc" TagName="cntZastepstwa" %>
<%@ Register Src="~/Controls/AdmZastepstwa.ascx" TagPrefix="cc" TagName="cntZastepstwaAdm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Zastępstwa" SubText1="" />
    <div class="form-page pgZastepstwa">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <cc:cntZastepstwa id="cZastepstwa" runat="server" />
                <cc:cntZastepstwaAdm id="cZastepstwaAdm" runat="server" Visible="false" />
            </ContentTemplate>            
        </asp:UpdatePanel>
    </div>    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
