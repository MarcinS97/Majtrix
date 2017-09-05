<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="Zwolnienia.aspx.cs" Inherits="HRRcp.Portal.ZwolnieniaForm" %>

<%@ Register Src="~/Controls/Kwitek/PracUrlop2.ascx" TagName="PracUrlop" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Kwitek/cntImieNazwisko.ascx" TagName="ImieNazwisko" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="page-title">
        <i class="fa fa-thermometer-full"></i>
        Zwolnienia lekarskie
    </div>
    <div class="pUrlopRamka container wide">
        <uc2:ImieNazwisko ID="ImieNazwisko1" runat="server" />
        <div class="page960 pgUrlopy">
            <div class="leftMrg12">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <%--<asp:Label ID="lbLista" CssClass="t1" runat="server" Text="Zwolnienia lekarskie:"></asp:Label>--%>
                        <%--<h4>Zwolnienia lekarskie:</h4>--%>
                        <uc1:PracUrlop ID="cntZwolnienia" Mode="1" runat="server" />
                        <asp:LinkButton CssClass="button75 printoff btn btn-primary pull-right" ID="btPrint" OnClientClick="javascript:window.print();" runat="server" Text="Drukuj" Visible="false">
                            <i class="fa fa-print"></i>Drukuj
                        </asp:LinkButton>
                        <div style="clear: both;"></div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
