<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StrukturaControl.ascx.cs" Inherits="HRRcp.Controls.StrukturaControl" %>

<asp:HiddenField ID="hidRootId" runat="server" />
<asp:HiddenField ID="hidOkresId" runat="server" />

<div class="StrukturaControl">
    <asp:Panel ID="paSearch" runat="server" CssClass="paSearch" DefaultButton="btSearch">
        <asp:HiddenField ID="hidPrevSearch" runat="server" />
        <asp:HiddenField ID="hidStartValue" runat="server" />
        <asp:TextBox ID="tbSearch" CssClass="textbox" runat="server"></asp:TextBox>
        <asp:Button ID="btSearch" CssClass="button75" runat="server" Text="Wyszukaj" onclick="btSearch_Click" />
        <asp:Label ID="lbNotFound" runat="server" Text="NotFound" ForeColor="Red" style="text-align: left" Visible="False"></asp:Label>
    </asp:Panel>

    <asp:Panel ID="paStructure" runat="server" ScrollBars="Both" CssClass="tvStructure">
        <asp:TreeView ID="tvStructure" runat="server" Visible="true"
            onselectednodechanged="tvStructure_SelectedNodeChanged" 
            ShowLines="True" 
            ExpandDepth="1" >
            <ParentNodeStyle Font-Names="Verdana" Font-Size="9pt" ForeColor="#0099FF" />
            <SelectedNodeStyle Font-Names="Verdana" Font-Size="9pt" BackColor="#0099FF" ForeColor="White" />
            <RootNodeStyle Font-Bold="True" Font-Names="Verdana" Font-Size="9pt" ForeColor="#0099FF" />
            <NodeStyle Font-Bold="True" ImageUrl="~/images/kierownik.png" />
            <LeafNodeStyle Font-Bold="False" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" ImageUrl="~/images/user.png" />
        </asp:TreeView>
    </asp:Panel>

    <div class="buttons">
        <asp:Button ID="btExpand" class="button75" runat="server" Text="Rozwiń" onclick="btExpand_Click" />
        <asp:Button ID="btCollapse" class="button75" runat="server" Text="Zwiń" onclick="btCollapse_Click" />
    </div>
</div>
