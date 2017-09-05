<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="WnioskiUrlopoweKier.aspx.cs" Inherits="HRRcp.Portal.WnioskiUrlopoweKierForm" %>

<%@ Register Src="~/Controls/Kwitek/PracUrlop2.ascx" TagName="PracUrlop" TagPrefix="uc1" %>
<%@ Register Src="~/Portal/Controls/paWnioskiUrlopoweKier.ascx" TagName="paWnioskiUrlopoweKier" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page1200 paWnioskiUrlopowe">
        <div class="page-title">Wnioski</div>
        <div class="container wide">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:paWnioskiUrlopoweKier ID="paWnioskiUrlopowe" Mode="2" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
