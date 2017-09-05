<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntAnkietaP.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Ewaluacja.cntAnkietaP" %>

<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="cc" TagName="DateEdit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:HiddenField ID="hidPracId" runat="server" Visible="false" />
        <asp:HiddenField ID="hidStanId" runat="server" Visible="false" />
        
        <div class="cntKartaZgloszenie cntAnkietaP">
            <h3 class="xtext-primary">
                Ankieta ewaluacyjna pracownika                 
                <asp:Button ID="btnPrint2" runat="server" CssClass="btn btn-primary btn-sm pull-right" Text="Drukuj" OnClick="btnPrint_Click" ValidationGroup="vPrint" />
            </h3>
            <hr />
            <h4><b>Szkolenie:</b> 
                <asp:Label ID="lblTraining" runat="server" /></h4>
             <h4><b>Status:</b> 
                 <asp:Label ID="lblStatus" runat="server" Text="W trakcie edycji" CssClass="" /> 
             </h4>
            <hr />
            <p class="xtext-primary">
                Prosimy o wypełnienia niniejszej ankiety, w celu uzyskania informacji na temat przeprowadzonego szkolenia.
        <br />
                Prosimy o wpisanie oceny, która wydaję się Pani/Panu najbardziej właściwa według podanej skali ocen.
            </p>
            <table class="tbHeader table table-bordered">
                <tr>
                    <td class="col1">
                        <label>Imię i nazwisko pracownika</label>
                    </td>
                    <td>
                        <%--<asp:DropDownList ID="ddlPracownik" runat="server" CssClass="form-control" DataSourceID="dsPracownik" DataValueField="Id" DataTextField="Name" AutoPostBack="true" OnSelectedIndexChanged="ddlPracownik_SelectedIndexChanged" />
                        <asp:SqlDataSource ID="dsPracownik" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                            SelectCommand="select null as Id, 'wybierz ...' as Name, 0 as Sort union all select Id, Nazwisko + ' ' + Imie as Name, 1 as Sort from Pracownicy order by Sort, Name" />--%>

                        <asp:TextBox ID="tbPracownik" runat="server" CssClass="form-control" MaxLength="100" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>Stanowisko</label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbStanowisko" runat="server" CssClass="form-control" MaxLength="100" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>Temat szkolenia</label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbTematSzkolenia" runat="server" CssClass="form-control" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>Firma / organizator szkolenia</label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbOrganizator" runat="server" CssClass="form-control" />
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>Imię i nazwisko osób prowadzących szkolenie</label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbProwadzacy" runat="server" TextMode="MultiLine" CssClass="form-control" />
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>Miejsce szkolenia (miejscowość, adres)</label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbMiejsce" runat="server" TextMode="MultiLine" CssClass="form-control" />
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>Ilość godzin / dni zajęć</label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbCzasTrwania" runat="server" CssClass="form-control" />
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>Data szkolenia</label>
                    </td>
                    <td>
                        <cc:DateEdit ID="deDataSzkolenia" runat="server" />
                    </td>
                </tr>
            </table>

            <h4 class="text-primary">Skala ocen:</h4>

            <p>
                1 - bardzo źle/zły/zła
        <br />
                2 – źle/zły/zła
        <br />
                3 – dostatecznie/dostateczny/dostateczna
        <br />
                4 – dobrze/dobra/dobre
        <br />
                5 - bardzo dobrze/dobra/dobre
        <br />
            </p>


            <h4 class="text-primary">Pytania ogólne:</h4>



            <table class="tbPracownicy table table-bordered">
                <tr>
                    <th>Pytania ogólne</th>
                    <th>Ocena</th>
                </tr>
                <tr>
                    <td>Jak oceniłbyś swoją satysfakcję z uczestnictwa w szkoleniu?
                    </td>
                    <td style="width: 100px;">
                        <asp:TextBox ID="tbOcena1" runat="server" CssClass="form-control" MaxLength="1" />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbOcena1" FilterType="Custom" ValidChars="12345" />
                    </td>
                </tr>
                <tr>
                    <td>W jakim stopniu zamierzasz wykorzystać treści przekazane na szkoleniu po powrocie do pracy?
                    </td>
                    <td>
                        <asp:TextBox ID="tbOcena2" runat="server" CssClass="form-control" MaxLength="1" />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="tbOcena1" FilterType="Custom" ValidChars="12345" />
                    </td>
                </tr>
            </table>

            <table class="tbPracownicy table table-bordered">
                <tr>
                    <th>Pytania o trenerów</th>
                    <th>Ocena</th>
                </tr>
                <tr>
                    <td colspan="2">Jak oceniasz trenera pod względem:
                    </td>
                </tr>
                <tr>
                    <td>- teoretycznej wiedzy merytorycznej?
                    </td>
                    <td style="width: 100px;">
                        <asp:TextBox ID="tbOcena3" runat="server" CssClass="form-control" MaxLength="1" />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbOcena2" FilterType="Custom" ValidChars="12345" />
                    </td>
                </tr>
                <tr>
                    <td>- praktycznej wiedzy merytorycznej?
                    </td>
                    <td>
                        <asp:TextBox ID="tbOcena4" runat="server" CssClass="form-control" MaxLength="1" />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="tbOcena3" FilterType="Custom" ValidChars="12345" />
                    </td>
                </tr>
                <tr>
                    <td>- komunikatywności?
                    </td>
                    <td>
                        <asp:TextBox ID="tbOcena5" runat="server" CssClass="form-control" MaxLength="1" />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="tbOcena4" FilterType="Custom" ValidChars="12345" />
                    </td>
                </tr>
                <tr>
                    <td>- zaspokojenia oczekiwań w stosunku do tematu?
                    </td>
                    <td>
                        <asp:TextBox ID="tbOcena6" runat="server" CssClass="form-control" MaxLength="1" />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="tbOcena5" FilterType="Custom" ValidChars="12345" />
                    </td>
                </tr>
                <tr>
                    <td>- uzyskania odpowiedzi na zadawane pytania?
                    </td>
                    <td>
                        <asp:TextBox ID="tbOcena7" runat="server" CssClass="form-control" MaxLength="1" />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="tbOcena6" FilterType="Custom" ValidChars="12345" />
                    </td>
                </tr>
            </table>

            <table class="tbPracownicy table table-bordered">
                <tr>
                    <th>Pytania dotyczące materiałów i organizacji szkolenia</th>
                    <th>Ocena</th>
                </tr>
                <tr>
                    <td>Ocena wykładów - czy przekazywane treści były zrozumiałe
                    </td>
                    <td style="width: 100px;">
                        <asp:TextBox ID="tbOcena8" runat="server" CssClass="form-control" MaxLength="1" />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="tbOcena7" FilterType="Custom" ValidChars="12345" />
                    </td>
                </tr>
                <tr>
                    <td>Ocena ćwiczeń - czy prowadzone ćwiczenia będą/są przydatne w dalszej pracy
                    </td>
                    <td>
                        <asp:TextBox ID="tbOcena9" runat="server" CssClass="form-control" MaxLength="1" />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="tbOcena8" FilterType="Custom" ValidChars="12345" />
                    </td>
                </tr>
                <tr>
                    <td>Czy podawane przykłady miały odniesienie do twojej pracy?
                    </td>
                    <td>
                        <asp:TextBox ID="tbOcena10" runat="server" CssClass="form-control" MaxLength="1" />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="tbOcena9" FilterType="Custom" ValidChars="12345" />
                    </td>
                </tr>
                <tr>
                    <td>Jak oceniasz materiały szkoleniowe pod względem czytelności i wygody korzystania?
                    </td>
                    <td>
                        <asp:TextBox ID="tbOcena11" runat="server" CssClass="form-control" MaxLength="1" />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="tbOcena10" FilterType="Custom" ValidChars="12345" />
                    </td>
                </tr>
                <tr>
                    <td>Jak oceniasz materiały szkoleniowe pod względem kompletności w porównaniu do przekazanych treści?
                    </td>
                    <td>
                        <asp:TextBox ID="tbOcena12" runat="server" CssClass="form-control" MaxLength="1" />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="tbOcena11" FilterType="Custom" ValidChars="12345" />
                    </td>
                </tr>
                <tr>
                    <td>Jak oceniasz warunki, w jakich prowadzone było szkolenie?
                    </td>
                    <td>
                        <asp:TextBox ID="tbOcena13" runat="server" CssClass="form-control" MaxLength="1" />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="tbOcena12" FilterType="Custom" ValidChars="12345" />
                    </td>
                </tr>
                <tr>
                    <td>Jak oceniasz przestrzeganie planu zajęć?
                    </td>
                    <td>
                        <asp:TextBox ID="tbOcena14" runat="server" CssClass="form-control" MaxLength="1" />
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="tbOcena13" FilterType="Custom" ValidChars="12345" />
                    </td>
                </tr>
            </table>


            <label>Uwagi:</label>
            <asp:TextBox ID="tbUwagi" runat="server" TextMode="MultiLine" CssClass="form-control" Style="margin-bottom: 6px;" />


            <div id="divRemove" runat="server" visible="false" style="display: inline-block;">
                <asp:Button ID="btnRemove" runat="server" CssClass="btn btn-danger btn-sm" Text="Usuń" OnClick="btnRemove_Click" />
                <asp:Button ID="btnRemoveConfirm" runat="server" CssClass="button_postback" Text="" OnClick="btnRemoveConfirm_Click" />
            </div>
            <div id="divRejected" runat="server" visible="false" style="display: inline-block;" >
                <asp:Button ID="btnRestore" runat="server" CssClass="btn btn-default btn-sm" Text="Przywróc" OnClick="btnRestore_Click" />
                <asp:Button ID="btnRestoreConfirm" runat="server" CssClass="button_postback" Text="" OnClick="btnRestoreConfirm_Click" />
            </div>
            <div id="divStatus1" runat="server" visible="false" style="display: inline-block;">
                <asp:Button ID="btnUnlock" runat="server" CssClass="btn btn-default btn-sm" Text="Odblokuj" OnClick="btnUnlock_Click" />
                <asp:Button ID="btnUnlockConfirm" runat="server" CssClass="button_postback" Text="" OnClick="btnUnlockConfirm_Click" />
            </div>

            <div class="pull-right">
                <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary btn-sm" Text="Drukuj" OnClick="btnPrint_Click" ValidationGroup="vPrint" />

                <div id="divStatus0" runat="server" visible="false" style="display: inline-block;">
                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success btn-sm" Text="Zapisz" OnClick="btnSave_Click" />
                    <asp:Button ID="btnSaveAndBlock" runat="server" CssClass="btn btn-success btn-sm" Text="Zapisz i zablokuj" OnClick="btnSaveAndBlock_Click" />
                    <asp:Button ID="btnSaveAndBlockConfirm" runat="server" CssClass="button_postback" Text="Zapisz" OnClick="btnSaveAndBlockConfirm_Click" />
                </div>
            </div>
        </div>



        <asp:SqlDataSource ID="dsData" runat="server" SelectCommand="
select a.*
, s.Nazwa as StatusName
, p.Nazwisko + ' ' + p.Imie as PracownikCert
, st.Nazwa StanowiskoPrac
, c.NazwaCertyfikatu as Szkolenie
, case 
    when a.Status = -1 then 'text-danger'
    when a.Status = 2 then 'text-success'
    else 'text-primary'
end as StatusColor
, p.Id as IdPracownikaCert
from msAnkiety a
left join msAnkietyStatus s on s.Id = a.Status
left join Certyfikaty c on c.Id = a.IdCertyfikatu
left join Pracownicy p on p.Id = c.IdPracownika
left join PracownicyStanowiska ps on ps.IdPracownika = p.Id and getdate() between ps.Od and isnull(ps.Do, '20990909')
left join Stanowiska st on st.Id = ps.IdStanowiska

where a.Id = {0}
            " />
        <asp:SqlDataSource ID="dsSave" runat="server"
            SelectCommand="
update msAnkiety set 
    Pracownik = {0}
    , Stanowisko = {1}
    , TematSzkolenia = {2}
    , Organizator = {3}
    , ProwadzacySzkolenie = {4}
    , MiejsceSzkolenia = {5}
    , CzasTrwania = {6}
    , DataSzkolenia = {7}
    , Odp1 = {8}
    , Odp2 = {9}
    , Odp3 = {10}
    , Odp4 = {11}
    , Odp5 = {12}
    , Odp6 = {13}
    , Odp7 = {14}
    , Odp8 = {15}
    , Odp9 = {16}
    , Odp10 = {17}
    , Odp11 = {18}
    , Odp12 = {19}
    , Odp13 = {20}
    , Odp14 = {21}
    , Uwagi = {22}
    , IdPracownika = {23}
    , IdStanowiska = {24}
    where Id = {25}
    " />

        <asp:SqlDataSource ID="dsAccept" runat="server" SelectCommand="update msAnkiety set Status = 1 where Id = {0}" />
        <asp:SqlDataSource ID="dsUnlock" runat="server" SelectCommand="update msAnkiety set Status = 0 where Id = {0}" />
        <asp:SqlDataSource ID="dsRemove" runat="server" SelectCommand="delete from msAnkiety where Id = {0}" />
        <asp:SqlDataSource ID="dsRestore" runat="server" SelectCommand="update msAnkiety set Status = 0 where Id = {0}" />



    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnPrint" />
        <asp:PostBackTrigger ControlID="btnPrint2" />
        <asp:PostBackTrigger ControlID="btnSave" />
        <asp:PostBackTrigger ControlID="btnUnlockConfirm" />
        <asp:PostBackTrigger ControlID="btnSaveAndBlockConfirm" />
    </Triggers>
</asp:UpdatePanel>

