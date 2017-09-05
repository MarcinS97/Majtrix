<%@ Page Title="" Language="C#" MasterPageFile="~/Portal3.Master" AutoEventWireup="true" CodeBehind="Kier.aspx.cs" Inherits="HRRcp.Portal.Wnioski.PracaZdalna.Kier" %>

<%@ Register Src="~/Portal/Controls/Wnioski/PracaZdalna/cntWniosekModal.ascx" TagPrefix="uc1" TagName="cntWniosekModal" %>
<%@ Register Src="~/Portal/Controls/Wnioski/PracaZdalna/cntWnioski.ascx" TagPrefix="uc1" TagName="cntWnioski" %>
<%@ Register Src="~/Portal/Controls/cntWniosekUrlopowy.ascx" TagPrefix="uc1" TagName="cntWniosekUrlopowy" %>
<%@ Register Src="~/Portal/Controls/cntWnioskiUrlopowe.ascx" TagPrefix="uc1" TagName="cntWnioskiUrlopowe" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page1200 paWnioskiUrlopowe">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <h1>
                    <i class="fa fa-home"></i>
                    Wnioski o pracę zdalną
                    <asp:LinkButton ID="btnNewRequest" runat="server" CssClass="btn btn-primary pull-right" OnClick="btnNewRequest_Click" >
                        <i class="fa fa-envelope"></i>
                        Złóż wniosek
                    </asp:LinkButton>
                </h1>
                <hr />
                <%--<uc1:cntWnioski runat="server" ID="cntWnioski" />--%>
                
                    <uc1:cntWnioskiUrlopowe ID="cntWnioskiUrlopowe2" Status="8" OnShow="cntWnioskiUrlopowe2_Show" runat="server" Mode="1" Rodzaj="1" />

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:cntWniosekUrlopowy runat="server" ID="cntWniosekUrlopowy1" OnClose="cntWniosekUrlopowy1_Close" Rodzaj="1" />
            <%--<uc1:cntWnioskiUrlopowe runat="server" ID="cntWnioskiUrlopowe" />--%>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
</asp:Content>
