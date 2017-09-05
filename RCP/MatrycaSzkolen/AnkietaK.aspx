<%@ Page Title="" Language="C#" MasterPageFile="~/MatrycaSzkolen/MS.Master" AutoEventWireup="true" CodeBehind="AnkietaK.aspx.cs" Inherits="HRRcp.MatrycaSzkolen.AnkietaK" %>
<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc2" TagName="PageTitle" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/Ewaluacja/cntAnkietaK.ascx" TagPrefix="cc" TagName="cntAnkietaK" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    
    <div class="caption4 caption">
        <asp:Label ID="lbTitle" CssClass="title" runat="server" Text="Ankieta przełożonego"></asp:Label>
        <asp:LinkButton ID="btnBack" runat="server" Text="Powrót" CssClass="btn btn-sm btn-default pull-right" PostBackUrl="~/MatrycaSzkolen/Ewaluacja.aspx" />
    </div>

    <%--<uc2:PageTitle ID="PageTitle1" runat="server" Title="Akceptacje" SubText1="" />--%>
    <div class="pgKartaZgloszenie center960">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <cc:cntAnkietaK id="cAnkietaK" runat="server" />
            </ContentTemplate>            
            <Triggers>
                <%--<asp:PostBackTrigger ControlID="cAnkietaK" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </div>    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="HeadFooter" runat="server">
</asp:Content>
