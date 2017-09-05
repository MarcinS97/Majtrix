<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectOkres.ascx.cs" Inherits="HRRcp.Controls.SelectOkres" %>

<asp:HiddenField ID="hidFrom" runat="server" />
<asp:HiddenField ID="hidTo" runat="server" />
<asp:HiddenField ID="hidOkresId" runat="server" />
<asp:HiddenField ID="hidStatus" runat="server" />

<div class="SelectOkres printoff">
    <div class="btn-group">
        <asp:Button ID="btBegin" runat="server" Text="|◄" CssClass="btn btn-default button" OnClick="btBegin_Click" ToolTip="Początek funkcjonowania" data-toggle="tooltip" data-container="body" />
        <asp:Button ID="btMinusQ" runat="server" Text="◄◄" CssClass="btn btn-default button2 button" OnClick="btMinusQ_Click" ToolTip="Minus 3 miesiące" data-toggle="tooltip" data-container="body" />
        <asp:Button ID="btPrev" runat="server" Text="◄" CssClass="btn btn-default button glyphicon-triangle-right" OnClick="btPrev_Click" ToolTip="Poprzedni miesiąc" data-toggle="tooltip" data-container="body" />
    </div>
    <span class="info">
        <asp:Label ID="lbFrom" runat="server" /><span>&nbsp;...&nbsp;</span><asp:Label ID="lbTo" runat="server" /><asp:Label ID="lbStatus" CssClass="status" runat="server" />
    </span>
    <div class="btn-group">
        <asp:Button ID="btNext" runat="server" Text="►" CssClass="btn btn-default button" OnClick="btNext_Click" ToolTip="Następny miesiąc" data-toggle="tooltip" data-container="body" />
        <asp:Button ID="btPlusQ" runat="server" Text="►►" CssClass="btn btn-default button2 button" OnClick="btPlusQ_Click" ToolTip="Plus 3 miesiące" data-toggle="tooltip" data-container="body" />
        <asp:Button ID="btEnd" runat="server" Text="►|" CssClass="btn btn-default button" OnClick="btEnd_Click" ToolTip="Okres bieżący" data-toggle="tooltip" data-container="body" />
    </div>
</div>


<%--
    <asp:LinkButton ID="Button1" runat="server" Text="" CssClass="button" onclick="btPrev_Click" ToolTip="Poprzedni miesiąc">
    <i class="glyphicon glyphicon-triangle-right"></i>
        </asp:LinkButton>
--%>