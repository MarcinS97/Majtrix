<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSelectOkres3.ascx.cs" Inherits="HRRcp.Controls.cntSelectOkres3" %>

<asp:HiddenField ID="hidFrom" runat="server" />
<asp:HiddenField ID="hidTo" runat="server" />
<asp:HiddenField ID="hidOkresId" runat="server" />
<asp:HiddenField ID="hidStatus" runat="server" />

<div class="btn-group printoff">
    <asp:Button ID="btBegin" runat="server" Text="|◄" CssClass="button" onclick="btBegin_Click" ToolTip="Początek funkcjonowania"/>
    <asp:Button ID="btMinusQ" runat="server" Text="◄◄" CssClass="button2 button" onclick="btMinusQ_Click" ToolTip="Minus 3 miesiące"/>
    <asp:Button ID="btPrev" runat="server" Text="◄" CssClass="button" onclick="btPrev_Click" ToolTip="Poprzedni miesiąc"/>
    <span class="info">
        <asp:Label ID="lbFrom" runat="server"/><span>&nbsp;...&nbsp;</span><asp:Label ID="lbTo" runat="server"/><asp:Label ID="lbStatus" CssClass="status" runat="server"/>
    </span>
    <asp:Button ID="btNext" runat="server" Text="►" CssClass="button" onclick="btNext_Click" ToolTip="Następny miesiąc"/>
    <asp:Button ID="btPlusQ" runat="server" Text="►►" CssClass="button2 button" onclick="btPlusQ_Click" ToolTip="Plus 3 miesiące"/>
    <asp:Button ID="btEnd" runat="server" Text="►|" CssClass="button" onclick="btEnd_Click" ToolTip="Okres bieżący"/>
</div>
