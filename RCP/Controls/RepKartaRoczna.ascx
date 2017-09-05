<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepKartaRoczna.ascx.cs" Inherits="HRRcp.Controls.RepKartaRoczna" %>
<%@ Register src="SelectZmiana2.ascx" tagname="SelectZmiana" tagprefix="uc1" %>
<%@ Register src="SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc1" %>
<%@ Register src="RepPlanPracy.ascx" tagname="RepPlanPracy" tagprefix="uc1" %>

<div class="PlanPracyZmiany">
    <uc1:RepPlanPracy ID="cntPlanPracy" Mode="2" runat="server" />
</div>
