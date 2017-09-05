<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="Acc.aspx.cs" Inherits="HRRcp.Portal.Wnioski.PracaZdalna.Acc" %>

<%@ Register Src="~/Portal/Controls/Wnioski/PracaZdalna/cntWniosekModal.ascx" TagPrefix="uc1" TagName="cntWniosekModal" %>
<%@ Register Src="~/Portal/Controls/Wnioski/PracaZdalna/cntWnioski.ascx" TagPrefix="uc1" TagName="cntWnioski" %>
<%@ Register Src="~/Portal/Controls/cntWniosekUrlopowy.ascx" TagPrefix="uc1" TagName="cntWniosekUrlopowy" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page1200 paWnioskiUrlopowe">
        <div class="page-title">
            <i class="fa fa-home"></i>
            Akceptacja wniosków o pracę zdalną
        </div>
        <div class="container wide">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:cntWnioski runat="server" ID="cntWnioski" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
