<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntChart.ascx.cs" Inherits="HRRcp.Controls.EliteReports.cntChart" %>

<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="cntChart">
            <div id="hidWrapper" style="display: none;">
                <asp:HiddenField ID="hidImg" runat="server" />
            </div>
            <asp:Literal ID="ChartCanvas" runat="server" />
            <asp:Image id="imgCanvas" runat="server" CssClass="imgCanvas printon" style="display: none;" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
