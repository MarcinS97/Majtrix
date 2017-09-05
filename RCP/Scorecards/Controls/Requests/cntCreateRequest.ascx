<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntCreateRequest.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Requests.cntCreateRequest" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>


<asp:HiddenField ID="hidObserverId" runat="server" Visible="false" />


<div id="ctCreateRequest" class="cntCreateRequest">
    <asp:Label ID="lblName" runat="server" Text="Nazwa: " />
    <asp:TextBox ID="tbName" runat="server" CssClass="tbName" MaxLength="80" /><br />
    <asp:Label ID="lblData" runat="server" Text="Data: " />
    <uc1:DateEdit ID="deDate" runat="server" ValidationGroup="ivg" />
    <br />
    <asp:Label ID="lblDataWyp" runat="server" Text="Data wypłaty: " />
    <uc1:DateEdit ID="deDateW" runat="server" ValidationGroup="ivg"  />
    <br />
    <div class="bottom_buttons">
        <asp:Button ID="btnCreate" runat="server" Text="Utwórz" CssClass="button100" OnClick="Create" ValidationGroup="ivg" CausesValidation="true"  />
        <asp:Button ID="btnClose" runat="server" Text="Zamknij" CssClass="button100" />
    </div>
    <asp:SqlDataSource ID="dsCreateRequest" runat="server" SelectCommand="insert into scWnioski (IdTypuArkuszy, IdPracownika, Data, DataWyplaty, BilansOtwarcia, DataAkceptacji, IdAkceptujacego, IloscPracownikow, Status, Kacc, Pacc, Nazwa, DataUtworzenia) values (-1337, {0}, {1}, {2}, null, null, null, null, 0, -1, -1, {3}, GETDATE())" />
</div>