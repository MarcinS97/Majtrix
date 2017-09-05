<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="WnioskiUrlopoweAkceptacja.aspx.cs" Inherits="HRRcp.Portal.WnioskiUrlopoweAkceptacja" %>
<%@ Register src="~/Controls/Kwitek/PracUrlop2.ascx" tagname="PracUrlop" tagprefix="uc1" %>
<%@ Register src="~/Portal/Controls/paWnioskiUrlopoweKier3.ascx" tagname="paWnioskiUrlopoweKier" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page1200 pgWnioskiUrlopoweAkceptacja" >
        <div class="page-title">
            <i class="fa fa-check"></i>
            Akceptacje
        </div>
        <div class="container wide">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:paWnioskiUrlopoweKier ID="paWnioskiUrlopowe" Mode="1" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
