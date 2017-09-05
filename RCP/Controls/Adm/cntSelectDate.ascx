<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSelectDate.ascx.cs" Inherits="HRRcp.Controls.Adm.cntSelectDate" %>

<%@ Register src="../DateEdit.ascx" tagname="DateEdit" tagprefix="uc1" %>

<asp:HiddenField ID="hidFrom" runat="server" />
<asp:HiddenField ID="hidTo" runat="server" />
<asp:HiddenField ID="hidOkresId" runat="server" />
<asp:HiddenField ID="hidStatus" runat="server" />

<div id="paSelectDate" runat="server" class="cntSelectDate SelectOkres printoff">
    <asp:Button ID="btBegin" runat="server" Text="|◄" onclick="btBegin_Click" ToolTip="Początek funkcjonowania"/>
    <asp:Button ID="btMinusQ" runat="server" Text="◄◄" CssClass="button2" onclick="btMinusQ_Click" ToolTip="Minus 3 miesiące"/>
    <asp:Button ID="btPrev" runat="server" Text="◄" onclick="btPrev_Click" ToolTip="Poprzedni miesiąc"/>
    <span class="info">
        <asp:Label ID="lbFrom" runat="server"/><span>&nbsp;...&nbsp;</span><asp:Label ID="lbTo" runat="server"/><asp:Label ID="lbStatus" CssClass="status" runat="server"/>

        
        <uc1:DateEdit ID="deNaDzien" runat="server" />
    </span>
    <asp:Button ID="btNext" runat="server" Text="►" onclick="btNext_Click" ToolTip="Następny miesiąc"/>
    <asp:Button ID="btPlusQ" runat="server" Text="►►" CssClass="button2" onclick="btPlusQ_Click" ToolTip="Plus 3 miesiące"/>
    <asp:Button ID="btEnd" runat="server" Text="►|" onclick="btEnd_Click" ToolTip="Okres bieżący"/>
</div>
