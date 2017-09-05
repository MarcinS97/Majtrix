<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntModalPopup2.ascx.cs" Inherits="HRRcp.Controls.cntModalPopup2" %>

<%--<div id="popupBg" class="popupBg" runat="server" style="display: none;"></div>--%>
<div id="popup" class="popup" runat="server" style="display: none;">
    <div id="header">
        <asp:Label ID="lblTitle" runat="server" CssClass="title" />
        <asp:PlaceHolder ID="phHeader" runat="server"></asp:PlaceHolder>
        <asp:LinkButton ID="lnkPopupCloser" runat="server" CssClass="fa fa-close popupCloser" PostBackUrl="javascript:" ></asp:LinkButton>
    </div>
    <div id="content">
        <asp:PlaceHolder ID="phContent" runat="server"></asp:PlaceHolder>
    </div>
    <div id="footer">
        <asp:PlaceHolder ID="phFooter" runat="server"></asp:PlaceHolder>
    </div>
</div>


<%--
OnClick="lnkPopupCloser_Click"--%>
