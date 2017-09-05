<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntZmianaDanychModal.ascx.cs" Inherits="HRRcp.Portal.Controls.Ubezpieczenia.Majatkowe.cntZmianaDanychModal" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>
<%@ Register Src="~/Portal/Controls/dbField.ascx" TagPrefix="uc1" TagName="dbField" %>
<uc1:cntModal runat="server" ID="cntModal" Title="Zmiana danych polisy" CssClass="modalZmianaDanych wn-formularz">
    <ContentTemplate>
        <%--<uc1:DateEdit runat="server" ID="DateEdit" OnDateChanged="DateEdit_DateChanged" />--%>
        <%--<uc1:dbField runat="server" ID="DataOd" OnChanged="DateEdit_DateChanged" Type="date" Label="Data rozpoczęcia:" />--%>

        <label class="text-primary">Obecne dane:</label>

        <table id="Table1" runat="server" class="tbDane table table-bordered tbCesja">
            <tr>
                <td class="col1">
                    <label>Telefon:</label>
                </td>
                <td>
                    <asp:Label ID="lblTelefon" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    <label>Email:</label>
                </td>
                <td>
                    <asp:Label ID="lblEmail" runat="server" />                        
                </td>
            </tr>
            <tr id="divCesja" runat="server">
                <td class="col1">
                    <label>Cesja:</label>
                </td>
                <td>
                    <asp:Label ID="lblCesja" runat="server" />
                </td>
            </tr>
        </table>

        <label class="text-primary">Nowe dane:</label>
        <table id="Table2" runat="server" class="tbDaneNowe table table-bordered tbCesja">
            <tr>
                <td class="col1">
                    <label>Telefon:</label><span class="star">*</span>
                </td>
                <td>
                    <uc1:dbField runat="server" ID="NrTel" Type="tb" Rq="true"/>
                </td>
            </tr>
            <tr>
                <td class="col1">
                    <label>Email:</label><span class="star">*</span>
                </td>
                <td>
                    <uc1:dbField runat="server" ID="Email" Type="tb" Rq="true"/>
                </td>
            </tr>
            <tr id="divCesjaQuestion" runat="server">
                <td class="col1">
                    <label>Cesja:</label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblCesja" runat="server" CssClass="rblCesja" RepeatLayout="Flow" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblCesja_SelectedIndexChanged">
                        <asp:ListItem Text="Tak" Value="1" />
                        <asp:ListItem Text="Nie" Value="0" Selected="true" />
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <table id="tbCesja" runat="server" class="tbPracownicy table table-bordered tbCesja">
            <tr>
                <td class="col1">
                    <label>REGON Banku</label><span class="star">*</span>
                </td>
                <td>
                    <uc1:dbfield runat="server" id="RegonBanku" type="tb" validationgroup="vgSave" />
                </td>
            </tr>
            <tr>
                <td class="col1">
                    <label>Pełna nazwa banku</label><span class="star">*</span>
                </td>
                <td>
                    <uc1:dbfield runat="server" id="NazwaBanku" type="tb" validationgroup="vgSave" />
                </td>
            </tr>
        </table>



    </ContentTemplate>
    <FooterTemplate>
        <asp:Button ID="btnSaveConfirm" runat="server" Text="Zapisz" CssClass="btn btn-success" OnClick="btnSaveConfirm_Click" />
        <asp:Button ID="btnSave" runat="server" CssClass="hidden" OnClick="btnSave_Click" />
    </FooterTemplate>
</uc1:cntModal>

<asp:SqlDataSource ID="dsData" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
    SelectCommand="
select wm.*
    
, P.Nazwisko + ' ' + P.Imie Zglaszajacy 
, case Wlasnosc 
    when 0 then 'spółdzielczego własnościowego prawa do lokalu mieszkalnego'
    when 1 then 'odrębnej własności'
    when 2 then WlasnoscOpis
    else '' end WlasnoscText
, NazwaBanku + ' - ' + RegonBanku CesjaName
from poWnioskiMajatkowe wm
left join Pracownicy P on P.Id = wm.ZglaszajacyId
--right join Pracownicy p on p.Id is not null
where wm.Id = {0} " />



<asp:SqlDataSource ID="dsSave" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" SelectCommand="
declare @fromId int
set @fromId = (select FromId from poWnioskiMajatkowe where Id = {0})

update poWnioskiMajatkowe set DataZakonczenia = (select DataOd from poWnioskiMajatkowe where Id = {0}) where DataZakonczenia is null and FromId = @fromId
    
insert into poWnioskiMajatkowe 
select 
  ZglaszajacyId
, PESEL
, Pracodawca
, {1} NrTel
, PowierzchniaLokalu
, Wlasnosc
, WlasnoscOpis
, DataOd
, ParId
, Plus
, GETDATE() DataUtworzenia
, Status
, PermAdm
, PermDane
, PermDaneM
, PermDaneR
, Imie
, Nazwisko
, Narodowosc
, DataUrodzenia
, AdresUbezpKod
, AdresUbezpMiasto
, AdresUbezpUlica
, AdresUbezpDom
, AdresUbezpLokal
, AdresKorKod
, AdresKorMiasto
, AdresKorUlica
, AdresKorDom
, AdresKorLokal
, {2} Email
, SumaUbezpieczenia
, Skladka
, SumaUbezpieczeniaPlus
, SkladkaPlus
, PrzedmiotWariantuPlus
, StatusName
, {3} Cesja
, {4} RegonBanku
, {5} NazwaBanku
, SumaNNW
, SkladkaNNW
, AdresKorSame
, AdresKorType
, AdresUbezpType
, OdrzProblemyFinansowe
, OdrzZakupInnego
, OdrzBrakSatysfakcji
, OdrzOczekiwania
, OdrzOczekiwaniaUwagi
, OdrzInnaPrzyczyna
, OdrzInnaPrzyczynaUwagi
, ZgodaNaPotracenie
, null DataZakonczenia
, @fromId FromId --(select FromId from poWnioskiMajatkowe where Id = {0}) FromId
, PlusId
, Rodzaj
, PermCudzy
, PermAssistance
, PermBonusClub
, null SkladkaAssecoId
from poWnioskiMajatkowe where Id = {0}    
" />



