<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="zoomUrlopy.ascx.cs" Inherits="HRRcp.Controls.zoomUrlopy" %>
<%@ Register src="PracUrlop.ascx" tagname="PracUrlop" tagprefix="uc1" %>

<div id="divZoom" title="Urlop pracownika" style="display:none;" class="PracUrlopy">
    <asp:UpdatePanel ID="UpdatePanel1a" runat="server">
        <ContentTemplate>
            <div id="divInfo" runat="server" class="info">
                <%--
                <uc1:InfoLabel ID="InfoLabel1" Text="" Value="" runat="server" />
                --%>
            </div>

            <uc1:PracUrlop ID="cntUrlop" runat="server" />

            <div class="buttons right">
                <asp:Button ID="btClose" runat="server" CssClass="button75" Text="Zamknij" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>  