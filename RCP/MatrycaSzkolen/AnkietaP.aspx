<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true" CodeBehind="AnkietaP.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.AnkietaP" %>

<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc2" TagName="PageTitle" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/Ewaluacja/cntAnkietaP.ascx" TagPrefix="cc" TagName="cntAnkietaP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="caption4 caption">
        <asp:Label ID="lbTitle" CssClass="title" runat="server" Text="Ankieta uczestnika"></asp:Label>
        <asp:LinkButton ID="btnBack" runat="server" Text="Powrót" CssClass="btn btn-sm btn-default pull-right" PostBackUrl="~/MatrycaSzkolen/Ewaluacja.aspx" />
    </div>
    <%--<uc2:PageTitle ID="PageTitle1" runat="server" Title="Ankieta uczestnika" SubText1="Ankieta poszkoleniowa uczestnika"  />--%>
    <div class="pgKartaZgloszenie center960">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <cc:cntAnkietaP ID="cAnkietaP" runat="server" />
            </ContentTemplate>
            <%--<Triggers>
                <asp:PostBackTrigger ControlID="cAnkietaP" />
            </Triggers>--%>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
