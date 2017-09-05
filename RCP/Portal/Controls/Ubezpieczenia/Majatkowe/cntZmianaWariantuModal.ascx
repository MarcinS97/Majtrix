<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntZmianaWariantuModal.ascx.cs" Inherits="HRRcp.Portal.Controls.WnioskiMajatkowe.cntZmianaWariantuModal" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>
<%@ Register Src="~/Portal/Controls/dbField.ascx" TagPrefix="cc" TagName="dbField" %>
<%@ Register Src="~/Portal/Controls/Ubezpieczenia/Majatkowe/cntWarianty.ascx" TagPrefix="uc1" TagName="cntWarianty" %>


<style>
    .wn-formularz .dateedit input {
        width: 200px;
    }
</style>


<uc1:cntModal runat="server" ID="cntModal" Title="Zmiana wariantu ubezpieczenia" WidthType="Large">
    <ContentTemplate>
        <%--<uc1:DateEdit runat="server" ID="DateEdit" OnDateChanged="DateEdit_DateChanged" />--%>
        <div class="wn-formularz">
            <label class="text-primary">Dane ubezpieczającego</label>
            <table class="tbHeader table table-bordered">
                <tr>
                    <td class="col1">
                        <label>Nazwisko i imię zgłaszającego</label>
                    </td>
                    <td>
                        <%--<asp:Label ID="lbZglaszajacy" runat="server"></asp:Label>--%>
                        <cc:dbField runat="server" ID="Zglaszajacy" Type="lb" />
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>PESEL</label>
                    </td>
                    <td>
                        <asp:Label ID="lblPESEL" runat="server"></asp:Label>
                        <%--<cc:dbField runat="server" ID="PESEL" Type="tb" Rq="true" ValidationGroup="vgSave" Enabled="false" />--%>
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>Nr telefonu</label>
                    </td>
                    <td>
                        <%--<asp:TextBox ID="tbNrTel" runat="server" CssClass="form-control" />--%>

                        <cc:dbField runat="server" ID="NrTel" Type="tb" />
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>Email</label>
                    </td>
                    <td>
                        <%--<asp:TextBox ID="tbNrTel" runat="server" CssClass="form-control" />--%>

                        <cc:dbField runat="server" ID="Email" Type="tb" />
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>Pracodawca (nazwa, siedziba)</label>
                    </td>
                    <td>
                        <%--<asp:Label ID="lbPracodawca" runat="server" Text="KDR Solutions Sp. z o.o. Kozietulskiego 4a 85-567 Bydgoszcz"></asp:Label>--%>
                        <%--<cc:dbField runat="server" ID="Pracodawca" Type="tb" />--%>
                        <%--<asp:Label ID="lblPracodawca" runat="server" Visible="true" />--%>
                        <cc:dbField runat="server" ID="Pracodawca" ValueField="PracodawcaText" DataSourceID="dsPracodawca" Type="ddl" />

                        <asp:SqlDataSource ID="dsPracodawca" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
                            SelectCommand="select null Pracodawca, 'wybierz ...' PracodawcaText, 0 Sort 
                                            union all
                                           select /*Id*/ Nazwa + ' - ' + Adres Pracodawca, Nazwa + ' - ' + Adres PracodawcaText, 1 Sort 
                                            from poPracodawcy order by Sort, PracodawcaText"></asp:SqlDataSource>

                    </td>
                </tr>
            </table>
            <label class="text-primary">Obecny wariant:</label><br />
            <label>Data rozpoczęcia:</label>
            <asp:Label ID="lblDate" runat="server" /><br />

            <table id="tbWariantObecny" runat="server" class="tbWarianty table table-bordered">
                <tr>
                    <th colspan="2">
                        <label>Bezpieczny</label>
                    </th>
                    <th colspan="2">
                        <label>Bezpieczny Plus*</label>
                    </th>
                    <th rowspan="2">
                        <label>Składka miesięczna łącznie (zł)</label>
                    </th>
                </tr>
                <tr>
                    <th>
                        <label>Suma ubezpieczenia (zł)</label>
                    </th>
                    <th>
                        <label>Składka miesięczna (zł)</label>
                    </th>
                    <th>
                        <label>Suma ubezpieczenia (zł)</label>
                    </th>
                    <th>
                        <label>Składka miesięczna (zł)</label>
                    </th>
                </tr>
                <tr runat="server">
                    <td>
                        <label>
                            <asp:Literal ID="litSuma" runat="server" /></label>
                    </td>
                    <td class="xtdSkladka">
                        <div class="checkbox">
                            <label>
                                <asp:CheckBox ID="cbSkladka" runat="server" Enabled="false" Checked="true" /><asp:Literal ID="litSkladka" runat="server" /></label>
                        </div>
                    </td>
                    <td>
                        <label>
                            <asp:Literal ID="litSumaPlus" runat="server" /></label>
                    </td>
                    <td class="xtdSkladkaPlus">
                        <div class="checkbox">
                            <label>
                                <asp:CheckBox ID="cbSkladkaPlus" runat="server" Enabled="false" />
                                <%--<input type="checkbox" value="">--%>  +
                                <asp:Literal ID="litSkladkaPlus" runat="server" /></label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </div>
                    </td>
                    <td>
                        <label class="sum">
                            <asp:Literal ID="litSkladkaSum" runat="server" /></label>
                    </td>
                </tr>
            </table>


            <label class="text-primary">Nowy wariant:</label><br />

            <%--<cc:dbField runat="server" ID="DataOd" OnChanged="DateEdit_DateChanged" Type="date" Label="Data rozpoczęcia:" />--%>
            <label>Data rozpoczęcia:</label><span class="star">*</span>
            <uc1:DateEdit ID="DataOd" runat="server" style="width: 200px;" ValidationGroup="vgSave" />
            <uc1:cntWarianty runat="server" ID="cntWarianty" />
        </div>
    </ContentTemplate>
    <FooterTemplate>
        <asp:Button ID="btnSaveConfirm" runat="server" Text="Zapisz" CssClass="btn btn-success" OnClick="btnSaveConfirm_Click" ValidationGroup="vgSave" />
        <asp:Button ID="btnSave" runat="server" CssClass="hidden" OnClick="btnSave_Click" />
    </FooterTemplate>
</uc1:cntModal>
<%--<asp:SqlDataSource ID="dsData" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
    SelectCommand="
select wm.*
    
, P.Nazwisko + ' ' + P.Imie Zglaszajacy 
, case Wlasnosc 
    when 0 then 'spółdzielczego własnościowego prawa do lokalu mieszkalnego'
    when 1 then 'odrębnej własności'
    when 2 then WlasnoscOpis
    else '' end WlasnoscText

,  Skladka + case when Plus = 1 then SkladkaPlus else 0 end SkladkaSum
, convert(varchar(10), DataOd, 20) CurrentDate
from poWnioskiMajatkowe wm
left join Pracownicy P on P.Id = wm.ZglaszajacyId
--right join Pracownicy p on p.Id is not null
where wm.Id = {0} " />--%>

<asp:SqlDataSource ID="dsData" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
    SelectCommand="
select wm.*
    
, P.Nazwisko + ' ' + P.Imie Zglaszajacy 
, case Wlasnosc 
    when 0 then 'spółdzielczego własnościowego prawa do lokalu mieszkalnego'
    when 1 then 'odrębnej własności'
    when 2 then WlasnoscOpis
    else '' end WlasnoscText

,  wm.Skladka + case when wm.PlusId is not null then oa.SkladkaPlus else 0 end SkladkaSum
, convert(varchar(10), DataOd, 20) CurrentDate
from poWnioskiMajatkowe wm
left join Pracownicy P on P.Id = wm.ZglaszajacyId
outer apply (select * from poWnioskiMajatkoweParametry where Id = wm.PlusId) oa
--right join Pracownicy p on p.Id is not null
where wm.Id = {0} " />





<%--<asp:SqlDataSource ID="dsVariantParameters" runat="server" SelectCommand="select * from poWnioskiMajatkoweParametry where Id = {0}" ConnectionString="<%$ ConnectionStrings:PORTAL %>" />--%>
<asp:SqlDataSource ID="dsVariantParameters" runat="server" SelectCommand=
"
select wmp.Suma, wmp.Skladka, isnull(oa.SumaPlus, 0) SumaPlus, isnull(oa.SkladkaPlus, 0) SkladkaPlus
from poWnioskiMajatkoweParametry wmp 
outer apply (select * from poWnioskiMajatkoweParametry where Id = {1}) oa
where wmp.Id = {0}
" 
ConnectionString="<%$ ConnectionStrings:PORTAL %>" />

<asp:SqlDataSource ID="dsSave" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" SelectCommand="
declare @fromId int
set @fromId = (select FromId from poWnioskiMajatkowe where Id = {0})

update poWnioskiMajatkowe set DataZakonczenia = {7} where DataZakonczenia is null and FromId = @fromId

insert into poWnioskiMajatkowe 
select 
  ZglaszajacyId
, PESEL
, Pracodawca
, NrTel
, PowierzchniaLokalu
, Wlasnosc
, WlasnoscOpis
, {7} DataOd
, {1} ParId
, {2} Plus
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
, Email
, {3} SumaUbezpieczenia
, {4} Skladka
, {5} SumaUbezpieczeniaPlus
, {6} SkladkaPlus
, PrzedmiotWariantuPlus
, StatusName
, Cesja
, RegonBanku
, NazwaBanku
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
, @fromId FromId
, {8} PlusId
, Rodzaj
, PermCudzy
, PermAssistance
, PermBonusClub
, null SkladkaAssecoId
from poWnioskiMajatkowe where Id = {0}  
" />

<asp:SqlDataSource ID="dsCheckIfAlreadyExists" runat="server" SelectCommand="
select * 
from poWnioskiMajatkowe wm
outer apply (select top 1 * from poWnioskiMajatkowe where Id = {1} and Status &gt; -1 and ZglaszajacyId = {2}) oa
where wm.DataOd = '{0}'
and wm.AdresUbezpUlica = oa.AdresUbezpUlica
and wm.AdresUbezpDom = oa.AdresUbezpDom
and wm.AdresUbezpLokal = oa.AdresUbezpLokal
and wm.AdresUbezpKod = oa.AdresUbezpKod
and wm.AdresUbezpMiasto = oa.AdresUbezpMiasto
and wm.ZglaszajacyId = {2}
and wm.Status &gt; -1
" ConnectionString="<%$ ConnectionStrings:PORTAL %>" />


<asp:SqlDataSource ID="dsCheckIfNext" runat="server" 
    SelectCommand=
"
declare @zglaszajacyId int
declare @dataOd datetime

set @zglaszajacyId = {0}
set @dataOd = '{1}'


select * from poWnioskiMajatkowe 
where 
  ZglaszajacyId = @zglaszajacyId 
and DataOd = @dataOd 
and Status &gt; -1
and DataOd in (select top 1 DataOd from poWnioskiMajatkowe where ZglaszajacyId = @zglaszajacyId and Status &gt; -1)       
"
ConnectionString="<%$ ConnectionStrings:PORTAL %>" />
