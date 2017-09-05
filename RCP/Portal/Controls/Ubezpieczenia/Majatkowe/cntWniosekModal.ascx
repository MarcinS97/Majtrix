<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWniosekModal.ascx.cs" Inherits="HRRcp.Portal.Controls.WnioskiMajatkowe.cntWniosekModal" %>


<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="cc" TagName="DateEdit" %>
<%@ Register Src="~/Portal/Controls/dbField.ascx" TagPrefix="cc" TagName="dbField" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/Portal/Controls/Ubezpieczenia/Majatkowe/cntWarianty.ascx" TagPrefix="cc" TagName="cntWarianty" %>


<asp:HiddenField ID="hidUserId" runat="server" Visible="false" />

<uc1:cntModal runat="server" ID="cntModal" WidthType="Large" Backdrop="false" CssClass="preview wn-formularz">
    <HeaderTemplate>
        <h3 class="xtext-primary">Wniosek o ubezpieczenie</h3>
        <h3 class="status" id="tStatus" runat="server" visible="false"><asp:Literal ID="ltStatus" runat="server"></asp:Literal></h3>        
    </HeaderTemplate>
    <ContentTemplate>
        <div class="" runat="server" id="divFormularz">
            <label class="text-primary">Dane ubezpieczającego:</label>
            <table class="tbHeader table table-bordered">
                <tr>
                    <td class="col1">
                        <label>Nazwisko i imię zgłaszającego</label>
                    </td>
                    <td>
                        <asp:Label ID="lbZglaszajacy" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>PESEL</label>
                    </td>
                    <td>
                        <asp:Label ID="lblPESEL" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>Nr telefonu</label><span class="star">*</span>
                    </td>
                    <td>
                        <cc:dbField runat="server" ID="NrTel" Type="tb" Rq="true" ValidationGroup="vgSave" />
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>Email</label><span class="star">*</span>
                    </td>
                    <td>
                        <cc:dbField runat="server" ID="Email" Type="tb" Rq="true" ValidationGroup="vgSave" />
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>Pracodawca (nazwa, siedziba)</label><span class="star">*</span>
                    </td>
                    <td>
                        <%--<asp:Label ID="lblPracodawca" runat="server" Visible="false" />--%>
                        <cc:dbField runat="server" ID="Pracodawca" ValueField="PracodawcaText" DataSourceID="dsPracodawca" Type="ddl" Rq="true" ValidationGroup="vgSave" />

                        <asp:SqlDataSource ID="dsPracodawca" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
                            SelectCommand="select null Pracodawca, 'wybierz ...' PracodawcaText, 0 Sort 
                                           union all
                                           select /*Id*/ Nazwa + ' - ' + Adres Pracodawca, Nazwa + ' - ' + Adres PracodawcaText, 1 Sort 
                                            from poPracodawcy order by Sort, PracodawcaText"></asp:SqlDataSource>
                    </td>
                </tr>
            </table>

            <label class="text-primary">Ja niżej podpisany składam wniosek o ubezpieczenie mieszkania zlokalizowanego:</label>
            <table class="tbPracownicy table table-bordered tbAdresLok" runat="server" id="tbAdresUbezp">
                <tr id="trAddrUbezp0" runat="server" class="ddlAdres">
                    <td class="col1">
                        <label>Adres</label><span class="star">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAdresyUbezp" runat="server" DataSourceID="dsAdresy" DataValueField="Value" DataTextField="Text" 
                            CssClass="form-control ddlAdres" OnSelectedIndexChanged="ddlAdresyUbezp_SelectedIndexChanged" AutoPostBack="true" />
                        <asp:RequiredFieldValidator ID="rqDdlAdresyUbezp" runat="server" ControlToValidate="ddlAdresyUbezp" ValidationGroup="vgSave" Display="Dynamic" ErrorMessage="Pole wymagane"  />

                        <%--<cc:dbField ID="AdresUbezpType" runat="server" DataSourceID="dsAdresyUbezp" DataValueField="Value" ValueField="Text" Type="ddl" Rq="true" ValidationGroup="vgSave"
                            CssClass="ddlAdres" OnSelectedIndexChanged="ddlAdresyUbezp_SelectedIndexChanged" />--%>
                    </td>
                </tr>
                <tr id="trAddrUbezp1" runat="server" class="other">
                    <td class="col1">
                        <label>Adres</label><span class="star">*</span>
                    </td>
                    <td class="adres2">

                        <asp:Label ID="lblAdresUbezpUlicaDomLok" runat="server" />


                        <%--<asp:TextBox ID="tbUlica" runat="server" CssClass="form-control" />--%>
                        <%--<cc:dbField runat="server" ID="AdresUlicaDomLok" Type="tb" Rq="true" ValidationGroup="vgSave" />--%>
                        <%--<cc:dbField runat="server" ID="AdresUbezpUlica" Type="tb" Rq="true" ValidationGroup="vgSave" CssClass="inline" Placeholder="Ulica" />
                        <cc:dbField runat="server" ID="AdresUbezpDom" Type="tb" Rq="true" ValidationGroup="vgSave" CssClass="inline" Placeholder="Dom" />
                        <cc:dbField runat="server" ID="AdresUbezpLokal" Type="tb" CssClass="inline" Placeholder="Lokal" />--%>


                        
                        <asp:TextBox ID="tbAdresUbezpUlica" runat="server" CssClass="form-control inline" style="width: auto;" placeholder="Ulica" ValidationGroup="vgSave" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbAdresUbezpUlica" ValidationGroup="vgSave" Display="Dynamic" ErrorMessage="Pole wymagane"  />

                        <asp:TextBox ID="tbAdresUbezpDom" runat="server" CssClass="form-control inline" style="width: auto;" placeholder="Dom" ValidationGroup="vgSave" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbAdresUbezpDom" ValidationGroup="vgSave" Display="Dynamic" ErrorMessage="Pole wymagane"  />


                        <asp:TextBox ID="tbAdresUbezpLokal" runat="server" CssClass="form-control inline" style="width: auto;" placeholder="Lokal" ValidationGroup="vgSave" />


                        


                    </td>
                </tr>
                <tr id="trAddrUbezp2" runat="server" class="other">
                    <td class="col1">
                        <label>Kod, miejscowość</label><span class="star">*</span>
                    </td>
                    <td>
                        <%--<asp:TextBox ID="tbMiejscowosc" runat="server" CssClass="form-control" />--%>

                        <asp:Label ID="lblAdresUbezpKodMiasto" runat="server" />



<%--                        <cc:dbField runat="server" ID="AdresUbezpKod" Type="tb" Rq="true" ValidationGroup="vgSave" CssClass="inline" Placeholder="00-000" />
                        <cc:dbField runat="server" ID="AdresUbezpMiasto" Type="tb" Rq="true" ValidationGroup="vgSave" CssClass="inline" Placeholder="Miasto..." />--%>

                        
                        <asp:TextBox ID="tbAdresUbezpKod" runat="server" CssClass="form-control inline" style="width: auto;" placeholder="00-000" ValidationGroup="vgSave" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbAdresUbezpKod" ValidationGroup="vgSave" Display="Dynamic" ErrorMessage="Pole wymagane"  />

                        <asp:TextBox ID="tbAdresUbezpMiasto" runat="server" CssClass="form-control inline" style="width: auto;" placeholder="Miasto" ValidationGroup="vgSave" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbAdresUbezpMiasto" ValidationGroup="vgSave" Display="Dynamic" ErrorMessage="Pole wymagane"  />



                    </td>
                </tr>
            </table>

            <table class="table table-bordered tbPowierzchnia">
                <tr id="trAddrUbezp3" runat="server" class="">
                    <td class="col1">
                        <label>Powierzchnia lokalu (m2)</label><span class="star">*</span>
                    </td>
                    <td>
                        <%--<asp:TextBox ID="tbPowierzchnia" runat="server" CssClass="form-control" />--%>
                        <cc:dbField runat="server" ID="PowierzchniaLokalu" Type="tb" Rq="true" ValidationGroup="vgSave" MaxLength="6" ValidChars="0123456789" />
                    </td>
                </tr>
            </table>

            <label class="text-primary">Adres korespondencyjny:</label><br />
            <div id="divAddrQuestion" runat="server">
                <span>Czy adres korespondencyjny jest taki sam jak adres mieszkania powyżej?</span><br />
                <asp:RadioButtonList ID="rblKoresp" runat="server" CssClass="rblKoresp" RepeatLayout="Flow" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblKoresp_SelectedIndexChanged">
                    <asp:ListItem Text="Tak" Value="1" Selected="True" />
                    <asp:ListItem Text="Nie" Value="0" />
                </asp:RadioButtonList>
                <br />
            </div>
            <table class="tbPracownicy table table-bordered tbAdresKoresp" runat="server" id="tbAdresKor">
                <tr id="trAddrKor0" runat="server" class="ddlAdres">
                    <td class="col1">
                        <label>Adres</label><span class="star">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAdresyKor" runat="server" DataSourceID="dsAdresy" DataValueField="Value" DataTextField="Text" CssClass="form-control ddlAdres" 
                            OnSelectedIndexChanged="ddlAdresyKor_SelectedIndexChanged" AutoPostBack="true" />
                        <asp:RequiredFieldValidator ID="rqDdlAdresyKor" runat="server" ControlToValidate="ddlAdresyKor" ValidationGroup="vgSave" Display="Dynamic" ErrorMessage="Pole wymagane"  />
                    </td>
                </tr>
                <tr id="trAddrKor1" runat="server" class="other" >
                    <td class="col1">
                        <label>Adres</label><span class="star">*</span>
                    </td>
                    <td class="adres2">
                        <%--<asp:TextBox ID="tbUlica" runat="server" CssClass="form-control" />--%>
                        <%--<cc:dbField runat="server" ID="AdresUlicaDomLok" Type="tb" Rq="true" ValidationGroup="vgSave" />--%>
                      <%--  <cc:dbField runat="server" ID="AdresKorUlica" Type="tb" Rq="true" CssClass="inline" Placeholder="Ulica" />
                        <cc:dbField runat="server" ID="AdresKorDom" Type="tb" Rq="true" CssClass="inline" Placeholder="Dom" />
                        <cc:dbField runat="server" ID="AdresKorLokal" Type="tb" CssClass="inline" Placeholder="Lokal" />--%>

                        <asp:Label ID="lblAdresKorUlicaDomLok" runat="server" />

                        <asp:TextBox ID="tbAdresKorUlica" runat="server" CssClass="form-control inline" style="width: auto;" placeholder="Ulica" ValidationGroup="vgSave" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbAdresKorUlica" ValidationGroup="vgSave" Display="Dynamic" ErrorMessage="Pole wymagane"  />

                        <asp:TextBox ID="tbAdresKorDom" runat="server" CssClass="form-control inline" style="width: auto;" placeholder="Dom" ValidationGroup="vgSave" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="tbAdresKorDom" ValidationGroup="vgSave" Display="Dynamic" ErrorMessage="Pole wymagane"  />

                        <asp:TextBox ID="tbAdresKorLokal" runat="server" CssClass="form-control inline" style="width: auto;" placeholder="Lokal" ValidationGroup="vgSave" />
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbAdresKorLokal" ValidationGroup="vgSave" Display="Dynamic" ErrorMessage="Pole wymagane"  />--%>
                    </td>
                </tr>
                <tr id="trAddrKor2" runat="server" class="other" >
                    <td class="col1">
                        <label>Kod, miejscowość</label><span class="star">*</span>
                    </td>
                    <td>
                        
                        <asp:Label ID="lblAdresKorKodMiasto" runat="server" />

                        <%--<asp:TextBox ID="tbMiejscowosc" runat="server" CssClass="form-control" />--%>

                        <asp:TextBox ID="tbAdresKorKod" runat="server" CssClass="form-control inline" style="width: auto;" placeholder="00-000" ValidationGroup="vgSave" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="tbAdresKorKod" ValidationGroup="vgSave" Display="Dynamic" ErrorMessage="Pole wymagane"  />

                        <asp:TextBox ID="tbAdresKorMiasto" runat="server" CssClass="form-control inline" style="width: auto;" placeholder="Miasto" ValidationGroup="vgSave" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="tbAdresKorMiasto" ValidationGroup="vgSave" Display="Dynamic" ErrorMessage="Pole wymagane"  />

                        <%--<cc:dbField runat="server" ID="AdresKorKod" Type="tb" Rq="true" CssClass="inline" Placeholder="00-000" />
                        <cc:dbField runat="server" ID="AdresKorMiasto" Type="tb" Rq="true" CssClass="inline" Placeholder="Miasto..." />--%>
                    </td>
                </tr>
            </table>


            <label class="text-primary">Lokal mieszklany posiada status (właściwe zaznaczyć):</label>

    
            <table class="tbWlasnosc table table-bordered">
                <tr>
                    <td class="col1">

                        <cc:dbField runat="server" ID="Wlasnosc" ValueField="WlasnoscText" DataSourceID="dsWlasnosc" Type="rbl" OnChanged="Wlasnosc_Changed" Rq="true" 
                            ValidationGroup="vgSave" />

                        <asp:SqlDataSource ID="dsWlasnosc" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
                            SelectCommand="select 0 Wlasnosc, 'spółdzielczego własnościowego prawa do lokalu mieszkalnego' WlasnoscText
                                                    union all
                                                    select 1, 'odrębnej własności'  
                                                    union all
                                                    select -1, 'inny (podać jaki)'
                                                    union all
                                                    select 2, 'brak' 
                                         "></asp:SqlDataSource>


                        <td>
                            <%--<asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" />--%>
                            <%--<asp:Label ID="lblWlasnoscOpis" runat="server" Visible="false" />--%>
                            <cc:dbField runat="server" ID="WlasnoscOpis" Type="tb" Visible="false" TextMode="MultiLine" Rq="true" ValidationGroup="vgSave" />
                        </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>Ubezpieczenie od dnia</label><span class="star">*</span>
                    </td>
                    <td>
                        <%--<cc:DateEdit ID="deOdDnia" runat="server" />--%>

                        <cc:dbField runat="server" ID="DataOd" Type="date" Rq="true" ValidationGroup="vgSave" />
                    </td>
                </tr>
            </table>


            <div>
                <label class="text-primary">Rodzaj nieruchomości:</label>
                <cc:dbField runat="server" ID="Rodzaj" ValueField="RodzajText" DataSourceID="dsRodzaj" Type="rbl" OnChanged="Rodzaj_Changed" Rq="true" 
                            ValidationGroup="vgSave" />

                        <asp:SqlDataSource ID="dsRodzaj" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
                            SelectCommand="select Id Rodzaj, Nazwa RodzajText from poWnioskiMajatkoweLokalRodzaje"></asp:SqlDataSource>

                
<%--                            select 0 Rodzaj, 'lokal mieszkalny' RodzajText
                                                    union all
                                                    select 1, 'dom mieszkalny'  --%>

            </div>



             <div id="divCesjaQuestion" runat="server">
                <label class="text-primary">Cesja:</label><br />
                <asp:RadioButtonList ID="rblCesja" runat="server" CssClass="rblCesja" RepeatLayout="Flow" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblCesja_SelectedIndexChanged">
                    <asp:ListItem Text="Tak" Value="1" />
                    <asp:ListItem Text="Nie" Value="0" Selected="true" />
                </asp:RadioButtonList>
                <br />
            </div>
            <table id="tbCesja" runat="server" class="tbPracownicy table table-bordered tbCesja">
                <tr>
                    <td class="col1">
                        <label>REGON Banku</label><span class="star">*</span>
                    </td>
                    <td>
                        <cc:dbField runat="server" ID="RegonBanku" Type="tb" ValidationGroup="vgSave" />
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>Pełna nazwa banku</label><span class="star">*</span>
                    </td>
                    <td>
                        <cc:dbField runat="server" ID="NazwaBanku" Type="tb"  ValidationGroup="vgSave" />
                    </td>
                </tr>
            </table>


            <label class="text-primary">Wariant polisy:</label>
            <h5 class="">Deklaruję chęć przystapienia do ubezpieczenia wg wybranego wariantu z sumą ubezpieczenia mieszkania lub domu mieszkalnego oraz składką miesięcznie potrącaną z Twojego wynagrodzenia w wysokości zaznaczonej w tabeli (właściwe zaznaczyć)<span class="star">*</span></h5>
            <cc:cntWarianty runat="server" ID="cntWarianty" />
            <h5 class="">* Wykupienie wariantu Bezpieczny Plus jest możliwe wyłacznie jako rozszerzenie wariantu Bezpieczny</h5>
            <label class="text-primary">Oświadczenie wnioskującego:</label>
            <p>
                <cc:dbField runat="server" ID="PermCudzy" Type="check" CssClass="inline" Rq="true" ValidationGroup="vgSave" />
                <span class="star">*</span>
                W przypadku zawarcia umowy ubezpieczenia na cudzy rachunek Ubezpieczający oświadcza, że finansuje w całości koszt składki ubezpieczeniowej i że w terminie 5 dni od zawarcia niniejszej umowy 
                 przekaże ubezpieczonemu OWU wskazane w polisie, w tym informację wymaganą przepisem art. 17 ustawy o działalności ubezpieczeniowej i reasekuracyjnej.
            </p>
            <p>
                <cc:dbField runat="server" ID="PermAssistance" Type="check" CssClass="inline" Rq="true" ValidationGroup="vgSave" />
                <span class="star">*</span>
                Ogólne warunki ubezpieczenia mienia ze składką płatną miesięcznie oraz szczególne warunki 
                ubezpieczenia „Assistance Plus” zatwierdzone uchwałą Zarządu UNIQA Towarzystwo Ubezpieczeń S.A. z dnia 28 grudnia 2015r., otrzymałem.
            </p>
            <p>
                <cc:dbField runat="server" ID="PermAdm" Type="check" CssClass="inline" Rq="true" ValidationGroup="vgSave" />
                <span class="star">*</span>
                Potwierdzam, że zostałem/zostałam poinformowany/poinformowana o tym, że Administratorem moich danych osobowych jest UNIQA Towarzystwo Ubezpieczeń S.A. z siedzibą w Łodzi (90-520) 
                przy ul. Gdańskiej 132, o prawie dostępu do tych danych i ich poprawiania, a także o tym, że dane osobowe będą wykorzystywane przez Administratora w celu obsługi i wykonywania umowy ubezpieczenia.
                
            </p>
            <p>
                <cc:dbField runat="server" ID="PermDane" Type="check" CssClass="inline" />
                Wyrażam zgodę na na przekazanie moich danych osobowych, w zakresie niezbędnym dla celów realizacji niniejszej umowy oraz wypełniania obowiązków reasekuratora przez 
                Mondial Assistance Sp. z. o.o z siedzibą przy ul. Domaniewskiej 50B, 02-672 Warszawa.
            </p>
            <p>
                <cc:dbField runat="server" ID="PermDaneM" Type="check" CssClass="inline" />
                Wyrażam zgodę na przetwarzanie moich danych osobowych w celu marketingu produktów i usług przez UNIQA Towarzystwo Ubezpieczeń na Życie S.A. 
                jak również na przetwarzanie moich danych w tych samych celach przez Administratora po rozwiązaniu umowy.
            </p>
            <p>
                <cc:dbField runat="server" ID="PermDaneR" Type="check" CssClass="inline" />
                Wyrażam zgodę na przesyłanie informacji handlowych przez UNIQA Towarzystwo Ubezpieczeń S.A. oraz UNIQA Towarzystwo Ubezpieczeń na Życie S.A. na wskazany adres poczty elektronicznej, 
                a także telefonicznie, telefaksem lub innym środkiem telekomunikacji elektronicznej, także po rozwiązaniu umowy ubezpieczenia oraz na składanie przez UNIQA Towarzystwo Ubezpieczeń S.A. 
                oświadczeń i przekazywanie informacji związanych z zawarciem i wykonywaniem umów ubezpieczenia, które łączą mnie ze Spółką z wykorzystaniem środków komunikacji elektronicznej.
            </p>
            <p>
                <cc:dbField runat="server" ID="PermBonusClub" Type="check" CssClass="inline" />
                Wnioskuję / nie wnioskuję o przyjęcie do programu lojalnościowego UNIQA Bonus Club. Akceptuję regulamin programu dostępny na stronie <a href="http://www.uniqabonusclub.pl">www.uniqabonusclub.pl</a>
            </p>
            <hr />
            <p>
                <cc:dbField runat="server" ID="ZgodaNaPotracenie" Type="check" CssClass="inline" Rq="true" ValidationGroup="vgSave" />
                <span class="star">*</span>
                Wyrażam zgodę na potrącenie składki polisy z mojego wynagrodzenia.
            </p>
        </div>
    </ContentTemplate>
    <FooterTemplate>
        <asp:Button ID="btnRestoreConfirm" runat="server" Visible="false" CssClass="btn btn-primary" Text="Przywróć" OnClick="btnRestoreConfirm_Click" />
        <asp:Button ID="btnRestore" runat="server" Visible="false" CssClass="hidden" OnClick="btnRestore_Click" />
        <asp:Button ID="btnSaveConfirm" runat="server" Text="Zapisz i wyślij" CssClass="btn btn-success xpull-right" OnClick="btnSaveConfirm_Click" ValidationGroup="vgSave" />
        <asp:Button ID="btnSave" runat="server" CssClass="hidden" OnClick="btnSave_Click" />
        <%--<asp:Button ID="btnPrint" runat="server" Visible="false" CssClass="btn btn-primary pull-right" Text="Drukuj" OnClick="btnPrint_Click" ValidationGroup="vPrint" Style="margin-right: 8px !important; margin-bottom: 16px !important;" />--%>
    </FooterTemplate>
</uc1:cntModal>

<%-- OnClientClick="return $('form').valid();"--%>


<asp:SqlDataSource ID="dsData" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
    SelectCommand="
select wm.*    
, P.Nazwisko + ' ' + P.Imie Zglaszajacy 
, case Wlasnosc 
    when 0 then 'spółdzielczego własnościowego prawa do lokalu mieszkalnego'
    when 1 then 'odrębnej własności'
    when 2 then WlasnoscOpis
    else '' end WlasnoscText
, s.StatusNazwa, s.StatusNazwa2
, isnull(wm.AdresUbezpUlica, wm.AdresUbezpMiasto) + isnull(' ' + wm.AdresUbezpDom, '') + isnull(' ' + wm.AdresUbezpLokal, '') AdresUbezpUlicaDomLokal
, isnull(wm.AdresUbezpKod, '') + isnull(' ' + wm.AdresUbezpMiasto, '') AdresUbezpKodMiasto
, isnull(wm.AdresKorUlica, wm.AdresKorMiasto) + isnull(' ' + wm.AdresKorDom, '') + isnull(' ' + wm.AdresKorLokal, '') AdresKorUlicaDomLokal
, isnull(wm.AdresKorKod, '') + isnull(' ' + wm.AdresKorMiasto, '') AdresKorKodMiasto
from poWnioskiMajatkowe wm
left join poWnioskiMajatkoweStatusy s on s.Id = wm.Status
left join Pracownicy P on P.Id = wm.ZglaszajacyId
--right join Pracownicy p on p.Id is not null
where wm.Id = {0} " />



<asp:SqlDataSource ID="dsPrac" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"
    SelectCommand="
select 
   Nazwisko + ' ' + Imie Pracownik
, Nick PESEL
from Pracownicy 
where Id = {0} 
    " />


<%--<asp:SqlDataSource ID="dsVariantParameters" runat="server" SelectCommand="select * from poWnioskiMajatkoweParametry where Id = {0}" ConnectionString="<%$ ConnectionStrings:PORTAL %>" />--%>
<asp:SqlDataSource ID="dsVariantParameters" runat="server" SelectCommand=
"
select wmp.Suma, wmp.Skladka, isnull(oa.SumaPlus, 0) SumaPlus, isnull(oa.SkladkaPlus, 0) SkladkaPlus
from poWnioskiMajatkoweParametry wmp 
outer apply (select * from poWnioskiMajatkoweParametry where Id = {1}) oa
where wmp.Id = {0}
" 
ConnectionString="<%$ ConnectionStrings:PORTAL %>" />



<asp:SqlDataSource ID="dsAdres" runat="server" SelectCommand="select * from poPracownicyAdresy where Id = {0}" ConnectionString="<%$ ConnectionStrings:PORTAL %>" />

<%--<asp:SqlDataSource ID="dsAdresyUbezp" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" SelectCommand="
select null AdresUbezpType, 'wybierz ...' Text, 0 Sort
union all
select Id AdresUbezpType, AdresUlica + ' - ' + AdresDom + '/' + AdresLokal + ' ' + AdresKod + ' ' + AdresMiasto Text, 1 Sort from poPracownicyAdresy
union all
select -1 AdresUbezpType, 'Inny' Text, 2 Sort
order by Sort, Text      
" />--%>

<asp:SqlDataSource ID="dsAdresy" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>" CancelSelectOnNullParameter="false" SelectCommand="
select null Value, 'wybierz ...' Text, 0 Sort
union all
select Id Value, isnull(AdresUlica, '') + ' - ' + isnull(AdresDom, '') + '/' + isnull(AdresLokal, '') + ' ' + isnull(AdresKod, '') + ' ' + isnull(AdresMiasto, '') Text, 1 Sort from poPracownicyAdresy
where IdPracownika = @pracId
union all
select -1 Value, 'Inny' Text, 2 Sort
order by Sort, Text      
" >
    <SelectParameters>
        <asp:ControlParameter Name="pracId" ControlID="hidUserId" PropertyName="Value" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>


<asp:SqlDataSource ID="dsCosmos" runat="server" SelectCommand="update poWnioskiMajatkowe set FromId = {0} where Id = {0}" ConnectionString="<%$ ConnectionStrings:PORTAL %>" />

<asp:SqlDataSource ID="dsRestore" runat="server" SelectCommand="update poWnioskiMajatkowe set Status = 0, DataZakonczenia = null where Id = {0}" ConnectionString="<%$ ConnectionStrings:PORTAL %>" />

<asp:SqlDataSource ID="dsCheckIfAlreadyExists" runat="server" SelectCommand="
select * from poWnioskiMajatkowe 
where DataOd = '{0}'
and AdresUbezpUlica = '{1}'
and AdresUbezpDom = '{2}'
and AdresUbezpLokal = '{3}'
and AdresUbezpKod = '{4}'
and AdresUbezpMiasto = '{5}'
and Status &gt; -1
and ZglaszajacyId = {6} 
" ConnectionString="<%$ ConnectionStrings:PORTAL %>" />


<asp:SqlDataSource ID="dsCheckIfNext" runat="server" 
    SelectCommand=
"
declare @zglaszajacyId int
declare @dataOd datetime
declare @justCreatedId int

set @zglaszajacyId = {0}
set @dataOd = '{1}'
set @justCreatedId = {2}

select * from poWnioskiMajatkowe 
where 
  ZglaszajacyId = @zglaszajacyId 
and DataOd = @dataOd 
and Status &gt; -1
and DataOd in (select top 1 DataOd from poWnioskiMajatkowe where ZglaszajacyId = @zglaszajacyId and Status &gt; -1)
and Id != @justCreatedId
"
ConnectionString="<%$ ConnectionStrings:PORTAL %>" />