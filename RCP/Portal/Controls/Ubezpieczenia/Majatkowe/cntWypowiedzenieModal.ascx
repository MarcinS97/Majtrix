<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWypowiedzenieModal.ascx.cs" Inherits="HRRcp.Portal.Controls.Ubezpieczenia.Majatkowe.cntWypowiedzenieModal" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/Portal/Controls/dbField.ascx" TagPrefix="uc1" TagName="dbField" %>



<uc1:cntModal runat="server" ID="cntModal" Title="Wypowiedzenie polisy" CssClass="cntWypowiedzenieModal">
    <ContentTemplate>
       <h4>Data zakończenia polisy:</h4>
        <uc1:dbField runat="server" ID="DataZakonczenia" Type="date" CssClass="inline" />
        <h4>Powód wypowiedzenia umowy ubezpieczenia:</h4>
        <hr />
        


        <uc1:dbField runat="server" ID="OdrzProblemyFinansowe" Type="check" Label="Problemy finansowe (spłata kredytu, pożyczki, choroba, utrata pracy, inne)" />
        <uc1:dbField runat="server" ID="OdrzZakupInnego" Type="check" Label="Zakup innego ubezpieczenia (oferta konkurencji)" />
        <uc1:dbField runat="server" ID="OdrzBrakSatysfakcji" Type="check" Label="Brak satysfakcji z obsługi UNIQUA" />
        <uc1:dbField runat="server" ID="OdrzOczekiwania" Type="check" Label="Produkt nie spełnia oczekiwań Klienta. Jeżeli tak, prosimy podać dlaczego?" />
        <uc1:dbField runat="server" ID="OdrzOczekiwaniaUwagi" Type="tb" TextMode="MultiLine" CssClass="hidden" />
        <uc1:dbField runat="server" ID="OdrzInnaPrzyczyna" Type="check" Label="Inna przyczyna. Proszę podać jaka?" />
        <uc1:dbField runat="server" ID="OdrzInnaPrzyczynaUwagi" Type="tb" TextMode="MultiLine" CssClass="hidden" />
    </ContentTemplate>
    <FooterTemplate>
        <asp:Button ID="btnSaveConfirm" runat="server" Text="Zapisz i zamknij" CssClass="btn btn-danger" OnClick="btnSaveConfirm_Click"  />
        <asp:Button ID="btnSave" runat="server" CssClass="hidden" OnClick="btnSave_Click" />
    </FooterTemplate>
</uc1:cntModal>

<asp:SqlDataSource ID="dsData" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
    SelectCommand="
select wm.*
from poWnioskiMajatkowe wm
left join Pracownicy P on P.Id = wm.ZglaszajacyId
where wm.Id = {0}" />

