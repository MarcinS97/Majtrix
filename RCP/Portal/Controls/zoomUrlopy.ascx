<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="zoomUrlopy.ascx.cs" Inherits="HRRcp.Portal.Controls.zoomUrlopy" %>
<%@ Register Src="~/Controls/PracUrlop.ascx" TagName="PracUrlop" TagPrefix="uc1" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>

<uc1:cntModal runat="server" ID="cntModal" Title="Urlopy">
    <ContentTemplate>
        <div id="divZoom" title="Urlop pracownika" style="" class="PracUrlopy">
            <div id="divInfo" runat="server" class="info">
                <%--
                <uc1:InfoLabel ID="InfoLabel1" Text="" Value="" runat="server" />
                --%>
            </div>
            <uc1:PracUrlop ID="cntUrlop" runat="server" />
            <%--<div class="buttons right">
                <asp:Button ID="btClose" runat="server" CssClass="button75" Text="Zamknij" />
            </div>--%>
        </div>
    </ContentTemplate>
</uc1:cntModal>
