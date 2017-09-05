<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntScorecardMenu2.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Spreadsheets.cntScorecardMenu2" %>


        <div id="ctScorecardMenu2" runat="server" class="cntScorecardMenu2">
            <div id="save" runat="server" class="menuItem save toggleAlarmClass" hid="1">
                <span class="text">Zapisz</span>
                <asp:Image ID="imgCassette" runat="server" ImageUrl="~/Scorecards/images/cassette.png" Height="18" />
            </div>
            <div id="accept" runat="server" class="menuItem back" hid="1">
                <span class="text">Powrót</span>
                <span class="fa fa-hand-o-left"></span>
            </div>
            <asp:Button ID="btnSave" runat="server" CssClass="btnSave" OnClick="Save"></asp:Button>
            <asp:Button ID="btnBack" runat="server" CssClass="btnBack" OnClick="Back"  ></asp:Button>
        </div>
