<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSelectOkres.ascx.cs" Inherits="HRRcp.Controls.RozliczenieNadg.cntSelectOkres" %>

<asp:HiddenField ID="hidFrom" runat="server" />
<asp:HiddenField ID="hidTo" runat="server" />
<asp:HiddenField ID="hidOkresId" runat="server" />
<asp:HiddenField ID="hidStatus" runat="server" />

<div class="SelectOkres printoff">
    <asp:Button ID="btBegin" runat="server" Text="|◄" CssClass="button" OnClick="btBegin_Click" ToolTip="Początek funkcjonowania"/>
    <asp:Button ID="btMinusQ" runat="server" Text="◄◄" CssClass="button2 button" OnClick="btMinusQ_Click" ToolTip="Minus 3 okresy rozliczeniowe"/>
    <asp:Button ID="btPrev" runat="server" Text="◄" CssClass="button" OnClick="btPrev_Click" ToolTip="Poprzedni okres rozliczeniowy"/>
    <span class="info">
        <asp:Label ID="lbFrom" runat="server"/><span>&nbsp;...&nbsp;</span><asp:Label ID="lbTo" runat="server"/><asp:Label ID="lbStatus" CssClass="status" runat="server"/>
    </span>
    <asp:Button ID="btNext" runat="server" Text="►" CssClass="button" OnClick="btNext_Click" ToolTip="Następny okres rozliczeniowy"/>
    <asp:Button ID="btPlusQ" runat="server" Text="►►" CssClass="button2 button" OnClick="btPlusQ_Click" ToolTip="Plus 3 okresy rozliczeniowe"/>
    <asp:Button ID="btEnd" runat="server" Text="►|" CssClass="button" OnClick="btEnd_Click" ToolTip="Bieżący okres rozliczeniowy"/>
</div>
