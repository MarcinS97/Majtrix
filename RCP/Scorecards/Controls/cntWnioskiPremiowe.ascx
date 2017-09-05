<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWnioskiPremiowe.ascx.cs" Inherits="HRRcp.Scorecards.Controls.cntWnioskiPremiowe" %>

<div id="paWnioskiPremiowe" runat="server" class="cntWnioskiPremiowe">
    <div class="demo_table">
        Lista wniosków premiowych, po wybraniu następuje przejście na wniosek<br />
        <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/Scorecards/WniosekPremiowy.aspx">Wniosek 1</asp:LinkButton><br />
        <asp:LinkButton ID="LinkButton2" runat="server" PostBackUrl="~/Scorecards/WniosekPremiowy.aspx">Wniosek 2</asp:LinkButton><br />
        <asp:LinkButton ID="LinkButton3" runat="server" PostBackUrl="~/Scorecards/WniosekPremiowy.aspx">Wniosek 3</asp:LinkButton><br />
        <asp:LinkButton ID="LinkButton4" runat="server" PostBackUrl="~/Scorecards/WniosekPremiowy.aspx">Wniosek 4</asp:LinkButton><br />
        <asp:LinkButton ID="LinkButton5" runat="server" PostBackUrl="~/Scorecards/WniosekPremiowy.aspx">Wniosek 5</asp:LinkButton><br />
    </div>
</div>
