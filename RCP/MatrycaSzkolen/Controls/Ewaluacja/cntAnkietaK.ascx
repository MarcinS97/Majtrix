<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntAnkietaK.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Ewaluacja.cntAnkietaK" %>

<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="cc" TagName="DateEdit" %>


<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="cntKartaZgloszenie cntAnkietaK">
            <h3 class="xtext-primary">
                Ankieta ewaluacyjna przełożonego                 
                <asp:Button ID="btnPrint2" runat="server" CssClass="btn btn-primary btn-sm pull-right" Text="Drukuj" OnClick="btnPrint_Click" ValidationGroup="vPrint" />
            </h3>
            <hr />
            <h4><b>Szkolenie:</b> 
                <asp:Label ID="lblTraining" runat="server" /></h4>
             <h4><b>Status:</b> 
                 <asp:Label ID="lblStatus" runat="server" Text="W trakcie edycji" CssClass="" /> 
             </h4>
            <hr />            
            <table class="tbHeader table table-bordered">
                <tr>
                    <td class="col1">
                        <label>Imię i nazwisko szkolonego pracownika/</label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbPracownik" runat="server" CssClass="form-control" MaxLength="100" />
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>Temat szkolenia</label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbTematSzkolenia" runat="server" CssClass="form-control" />
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
                <tr>
                    <td class="col1">
                        <label>Cel szczegółowy szkolenia</label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbCel" runat="server" CssClass="form-control" />
                    </td>
                </tr>
                <tr>
                    <td class="col1">
                        <label>W jakim czasie od momentu szkolenia można ocenić efektywność szkolenia? (dni)</label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbMonitDni" runat="server" CssClass="form-control" />
                    </td>
                </tr>
            </table>
            <hr />
            <p>
                Skala ocen:
        <br />
                1 – poziom niski (należy ponownie przeszkolić pracownika lub podjąć inne działania, mające na celu uzupełnienie innych kompetencji)
        <br />
                2 – poziom zadowalający (pracownik ma wiedzę, ale nie potrafi jej w pełni wykorzystać)
        <br />
                3 – poziom dobry (pracownik ma wiedzę, potrafi wykorzystać zdobyte informacje, potrzebuje czasu lub pomocy by efektywnie zarządzać i wdrażać zagadnienia ze szkolenia)
        <br />
                4 – poziom wysoki (szkolenie było w pełni efektywne, pracownik wykorzystuje zdobytą wiedze w praktyce)
        <br />
            </p>



            <p>
                <h5>1. Czy zakładany cel szkolenia został zrealizowany?</h5>
                <asp:RadioButtonList ID="rbl1" runat="server" CssClass="">
                    <asp:ListItem Value="0">Tak</asp:ListItem>
                    <asp:ListItem Value="1">Nie</asp:ListItem>
                </asp:RadioButtonList>

                <label>W stopniu:</label>
                <asp:TextBox ID="tbOcena1" runat="server" CssClass="form-control form-inline" Width="100px" />
                <br />
                <label>Uzasadnij</label>
                <asp:TextBox ID="tbTekst1" runat="server" CssClass="form-control form-inline" />
            </p>
            <p>
                <h5>2. Czy pracownik w praktyce wykorzystuje zdobytą wiedzę i umiejętności?</h5>
                <asp:RadioButtonList ID="rbl2" runat="server">
                    <asp:ListItem Value="0">Tak</asp:ListItem>
                    <asp:ListItem Value="1">Nie</asp:ListItem>
                </asp:RadioButtonList>

                <label>W stopniu:</label>
                <asp:TextBox ID="tbOcena2" runat="server" CssClass="form-control form-inline" Width="100px" />
                <br />
                <label>Uzasadnij</label>
                <asp:TextBox ID="tbTekst2" runat="server" CssClass="form-control form-inline"  />
            </p>
            <p>
                <h5>3.  Czy pracownik potrafi przekazać zdobytą wiedzę/umiejętności współpracownikom?</h5>
                <asp:RadioButtonList ID="rbl3" runat="server">
                    <asp:ListItem Value="0">a) Tak</asp:ListItem>
                    <asp:ListItem Value="1">b) Nie</asp:ListItem>
                    <asp:ListItem Value="2">c) Nie jest to wymagane</asp:ListItem>
                </asp:RadioButtonList>

                <%--        <label>W stopniu:</label>
        <asp:TextBox ID="tbOcena3" runat="server" CssClass="form-control form-inline" Width="100px" />--%>

                <label>Uzasadnij</label>
                <asp:TextBox ID="tbTekst3" runat="server" CssClass="form-control form-inline" />
            </p>
            <p>
                <h5>4. W jaki sposób pracownik wykorzystuje wiedzę/umiejętności zdobyte na szkoleniu? (np. konkretne działania, metody, rozwiązania, obowiązki)</h5>
                <asp:TextBox ID="tbOcena3" runat="server" TextMode="MultiLine" CssClass="form-control" />

            </p>

            <div id="divRemove" runat="server" visible="false" style="display: inline-block;">
                <asp:Button ID="btnRemove" runat="server" CssClass="btn btn-danger btn-sm" Text="Usuń" OnClick="btnRemove_Click" />
                <asp:Button ID="btnRemoveConfirm" runat="server" CssClass="button_postback" Text="" OnClick="btnRemoveConfirm_Click" />
            </div>
            <div id="divStatus1" runat="server" visible="false" style="display: inline-block;">
                <asp:Button ID="btnUnlock" runat="server" CssClass="btn btn-default btn-sm" Text="Odblokuj" OnClick="btnUnlock_Click" />
                <asp:Button ID="btnUnlockConfirm" runat="server" CssClass="button_postback" Text="" OnClick="btnUnlockConfirm_Click" />
            </div>
            <div id="divRejected" runat="server" visible="false" style="display: inline-block;" >
                <asp:Button ID="btnRestore" runat="server" CssClass="btn btn-default btn-sm" Text="Przywróc" OnClick="btnRestore_Click" />
                <asp:Button ID="btnRestoreConfirm" runat="server" CssClass="button_postback" Text="" OnClick="btnRestoreConfirm_Click" />
            </div>
            <div class="pull-right">
                <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary btn-sm" Text="Drukuj" OnClick="btnPrint_Click" ValidationGroup="vPrint" />

                <div id="divStatus0" runat="server" visible="false" style="display: inline-block;">
                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success btn-sm" Text="Zapisz" OnClick="btnSave_Click" />
                    <asp:Button ID="btnSaveAndBlock" runat="server" CssClass="btn btn-success btn-sm" Text="Zapisz i zablokuj" OnClick="btnSaveAndBlock_Click" />
                    <asp:Button ID="btnSaveAndBlockConfirm" runat="server" CssClass="button_postback" Text="Zapisz" OnClick="btnSaveAndBlockConfirm_Click" />
                </div>
            </div>

            <%--    <div class="pull-right">
        <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary btn-sm" Text="Drukuj" OnClick="btnPrint_Click" ValidationGroup="vPrint" />
        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success btn-sm" Text="Zapisz" OnClick="btnSave_Click" />
     </div>--%>
        </div>

        <asp:SqlDataSource ID="dsAccept" runat="server" SelectCommand="update msAnkiety set Status = 1, DataWypelnienia = GETDATE() where Id = {0}" />
        <asp:SqlDataSource ID="dsUnlock" runat="server" SelectCommand="update msAnkiety set Status = 0 where Id = {0}" />
        <asp:SqlDataSource ID="dsRemove" runat="server" SelectCommand="delete from msAnkiety where Id = {0}" />
                <asp:SqlDataSource ID="dsRestore" runat="server" SelectCommand="update msAnkiety set Status = 0 where Id = {0}" />

        <asp:SqlDataSource ID="dsData" runat="server" SelectCommand="
            
declare @ankietaId int = {0}

select a.*
, s.Nazwa as StatusName
, p.Nazwisko + ' ' + p.Imie as PracownikCert
, c.NazwaCertyfikatu as Szkolenie
, case 
    when a.Status = -1 then 'text-danger'
    when a.Status = 2 then 'text-success'
    else 'text-primary'
end as StatusColor
, u.EwaluacjaMonitDniEdycja MonitDniEdit
, isnull(a.MonitDni, u.EwaluacjaKierDni) MonitDniActual
from msAnkiety a
left join msAnkietyStatus s on s.Id = a.Status
left join Certyfikaty c on c.Id = a.IdCertyfikatu
left join Pracownicy p on p.Id = c.IdPracownika
left join Uprawnienia u on u.Id = c.IdUPrawnienia
where a.Id = @ankietaId" />
        <asp:SqlDataSource ID="dsSave" runat="server"
            SelectCommand="
update msAnkiety set 
    Pracownik = {0}
    , TematSzkolenia = {1}
    , DataSzkolenia = {2}
    , CelSzkolenia = {3}
    , MonitDni = {4}
    , Odp1 = {5}
    , Odp2 = {6}
    , Odp3 = {7}
    , Odp4 = {8}
    , Odp5 = {9}
    , Odp6 = {10}
    , Tekst1 = {11}
    , Tekst2 = {12}
    , Tekst3 = {13}
    where Id = {14}
    " />
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnPrint" />
        <asp:PostBackTrigger ControlID="btnPrint2" />
        <asp:PostBackTrigger ControlID="btnSave" />
        <asp:PostBackTrigger ControlID="btnSaveAndBlockConfirm" />
    </Triggers>
</asp:UpdatePanel>

