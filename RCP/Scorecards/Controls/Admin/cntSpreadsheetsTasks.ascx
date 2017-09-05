<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSpreadsheetsTasks.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Admin.cntSpreadsheetsTasks" %>

<%@ Register Src="~/Scorecards/Controls/Admin/cntTasksList.ascx" TagPrefix="leet" TagName="TasksList" %>
<%@ Register Src="~/Scorecards/Controls/Admin/cntSpreadsheetsList.ascx" TagPrefix="leet" TagName="SpreadsheetsList" %>
<%@ Register Src="~/Scorecards/Controls/Admin/cntSpreadsheetsTasksList.ascx" TagPrefix="leet" TagName="SpreadsheetsTasksList" %>


<div id="ctSpreadsheetsTasks" runat="server" class="cntSpreadsheetsTasks" >
    <div class="left">
        <leet:SpreadsheetsList Id="SpreadsheetsList" runat="server" OnSheetSelected="SheetSelected"  />
    </div>
    <div class="middle">
        <leet:SpreadsheetsTasksList Id="SpreadsheetsTasksList" runat="server" />
    </div>
    <div class="buttons">
        <asp:Button ID="btnAdd" runat="server" Text="◄ Dodaj" CssClass="button75" OnClick="AddTask" />
        <asp:Button ID="btnDelete" runat="server" Text="► Usuń" CssClass="button75" OnClick="RemoveTask" />
    </div>
    <div class="right">
        <leet:TasksList ID="TaskList" runat="server" />
    </div>
</div>