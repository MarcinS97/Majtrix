<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSelectRokMiesiac.ascx.cs" Inherits="HRRcp.Controls.cntSelectRokMiesiac" %>

<asp:UpdatePanel runat="server">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="BTYB" />
        <asp:AsyncPostBackTrigger ControlID="BTYN" />
        <asp:AsyncPostBackTrigger ControlID="BTMB" />
        <asp:AsyncPostBackTrigger ControlID="BTMN" />
    </Triggers>
    <ContentTemplate>
        <asp:HiddenField ID="HFMVal" runat="server" Visible="false" />
        <asp:HiddenField ID="HFYVal" runat="server" Visible="false" />

        <div class="SelectOkres cntSelectRokMiesiac printoff">
            <asp:Button ID="BTAB" runat="server" Text="|◄" CssClass="button" OnClick="BTA_Click" Visible='<%# canBackAll %>' CommandArgument="0" />
            <asp:Button ID="BTYB" runat="server" Text="◄◄" CssClass="button2 button" OnClick="btSelectMY_Click" ToolTip='<%# ToolTipPrev %>' CommandName="Y" CommandArgument="0" />
            <asp:Button ID="BTMB" runat="server" Text="◄" CssClass="button" OnClick="btSelectMY_Click" ToolTip="Poprzedni miesiąc" CommandName="M" CommandArgument="0" />
            <span class="info">
                <asp:Label ID="LabelM" Text='<%# MiesiacName %>' runat="server"/> <asp:Label ID="LabelY" Text='<%# Rok %>' runat="server"/>
            </span>
            <asp:Button ID="BTMN" runat="server" Text="►" CssClass="button" OnClick="btSelectMY_Click" ToolTip="Następny miesiąc" CommandName="M" CommandArgument="1" />
            <asp:Button ID="BTYN" runat="server" Text="►►" CssClass="button2 button" OnClick="btSelectMY_Click" ToolTip='<%# ToolTipNext %>' CommandName="Y" CommandArgument="1" />
            <asp:Button ID="BTAN" runat="server" Text="►|" CssClass="button" OnClick="BTA_Click" Visible='<%# canNextAll %>' CommandArgument="1" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
