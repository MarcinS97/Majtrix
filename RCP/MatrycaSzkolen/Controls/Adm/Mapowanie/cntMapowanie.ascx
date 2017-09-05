<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntMapowanie.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Adm.cntMapowanie" %>

<%@ Register Src="~/MatrycaSzkolen/Controls/Adm/Mapowanie/cntStanowiska.ascx" TagPrefix="cc" TagName="Stanowiska" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/Adm/Mapowanie/cntLinie.ascx" TagPrefix="cc" TagName="Linie" %>
<%@ Register Src="~/MatrycaSzkolen/Controls/Adm/Mapowanie/cntLinieStanowiska.ascx" TagPrefix="cc" TagName="LinieStanowiska" %>


<div id="ctSpreadsheetsTasks" runat="server" class="cntSpreadsheetsTasks cntMapowanie">
    <div class="left">
        <asp:Label ID="Label1" runat="server" CssClass="title" Text="Szkolenia stanowiskowe" />
        <hr />
        <cc:Stanowiska ID="Stanowiska" runat="server" OnStanowiskoSelected="StanowiskoSelected" />
    </div>
    <div class="middle">
        <asp:Label ID="lblConnection" runat="server" CssClass="title" Text="Powiązania - wybierz szkolenie stanowiskowe" />
        <hr />
        <cc:LinieStanowiska ID="LinieStanowiska" runat="server" OnDeleted="LinieStanowiska_Deleted" OnSelected="LinieStanowiska_Selected" />
    </div>
    <div class="buttons">
        <asp:Button ID="btnAdd" runat="server" Text="◄ Dodaj" CssClass="btn btn-success btn-sm" OnClick="AddTask" Enabled="false" />
        <asp:Button ID="btnDelete" runat="server" Text="► Usuń" CssClass="btn btn-danger btn-sm" OnClick="RemoveTask" Enabled="false" />
    </div>
    <div class="right">
        <asp:Label ID="Label3" runat="server" CssClass="title" Text="Linie" />
        <hr />
        <cc:Linie ID="Linie" runat="server" />
    </div>
</div>
