<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPracownicyEditModal.ascx.cs" Inherits="HRRcp.RCP.Controls.Harmonogram.cntPracownicyEditModal" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>



<uc1:cntModal runat="server" ID="cntModal" Backdrop="false" Keyboard="false">
    <ContentTemplate>
        <div class="form-horizontal">
            <div class="form-group">
                <label class="col-sm-4 control-label">Miesiąc:</label>
                <div class="col-sm-5">
                    <asp:DropDownList ID="ddlMiesiac" runat="server" DataSourceID="dsMiesiac" DataValueField="Value" DataTextField="Text" CssClass="form-control"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlMiesiac_SelectedIndexChanged" />
                    <asp:SqlDataSource ID="dsMiesiac" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                        SelectCommand="
declare @od datetime = dbo.bom(GETDATE())
select distinct CONVERT(varchar(10), DataOd, 20) as Text, CONVERT(varchar(10), DataOd, 20) as Value
from OkresyRozliczeniowe 
where Status = 0   -- otwarte, powinienem tylko importować na datę pierwszego 3 miesięcznego 
order by 1 desc
" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-4 control-label">Imię:</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="tbImie" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbImie" ErrorMessage="Pole wymagane" ValidationGroup="vgSave"
                        SetFocusOnError="false" CssClass="error" Display="Dynamic" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-4 control-label">Nazwisko:</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="tbNazwisko" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbNazwisko" ErrorMessage="Pole wymagane" ValidationGroup="vgSave"
                        SetFocusOnError="false" CssClass="error" Display="Dynamic" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-4 control-label">Identyfikator:</label>
                <div class="col-sm-3">
                    <asp:TextBox ID="tbIdentyfikator" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbIdentyfikator" ErrorMessage="Pole wymagane" ValidationGroup="vgSave"
                        SetFocusOnError="false" CssClass="error" Display="Dynamic" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-4 control-label">Dział:</label>
                <div class="col-sm-8">
                    <asp:DropDownList ID="ddlDzial" runat="server" DataSourceID="dsDzial" DataValueField="Value" DataTextField="Text" CssClass="form-control" />
                    <asp:SqlDataSource ID="dsDzial" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                        SelectCommand="
select null Value, 'wybierz ...' Text, 0 Sort 
union all
select Id Value, Nazwa Text, 1 Sort 
from Dzialy
order by Sort, Text
" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlDzial" ErrorMessage="Pole wymagane" ValidationGroup="vgSave"
                        SetFocusOnError="false" CssClass="error" Display="Dynamic" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-4 control-label">Typ okresu:</label>
                <div class="col-sm-8">
                    <asp:DropDownList ID="ddlTypOkresu" runat="server" DataSourceID="dsTypOkresu" DataValueField="Value" DataTextField="Text" CssClass="form-control" />
                    <asp:SqlDataSource ID="dsTypOkresu" runat="server" CancelSelectOnNullParameter="false" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                        SelectCommand="
select null Value, 'wybierz ...' Text, 0 Sort 
union all 
select ort.Id Value, ort.Nazwa Text, 1 Sort 
from rcpOkresyRozliczenioweTypy ort
left join OkresyRozliczeniowe o on o.Typ = ort.Id
where Aktywny = 1
group by ort.Id, ort.Nazwa
having count(o.Id) &gt; 0
order by Sort, Text                    
">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="deNaDzien" Name="nadzien" PropertyName="Date" Type="DateTime" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlTypOkresu" ErrorMessage="Pole wymagane" ValidationGroup="vgSave"
                        SetFocusOnError="false" CssClass="error" Display="Dynamic" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-4 control-label">Klasyfikacja:</label>
                <div class="col-sm-3">
                    <asp:TextBox ID="tbKlasyfikacja" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbKlasyfikacja" ErrorMessage="Pole wymagane" ValidationGroup="vgSave"
                        SetFocusOnError="false" CssClass="error" Display="Dynamic" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-4 control-label">Funkcja:</label>
                <div class="col-sm-3">
                    <asp:TextBox ID="tbFunkcja" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-4 control-label">Kod:</label>
                <div class="col-sm-3">
                    <asp:TextBox ID="tbKod" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-4 control-label">Opis kodu:</label>
                <div class="col-sm-5">
                    <asp:TextBox ID="tbKodOpis" runat="server" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-4 control-label">Data zatrudnienia:</label>
                <div class="col-sm-3">
                    <uc1:DateEdit runat="server" ID="deDataZatrudnienia" />
                </div>
            </div>
            <%--<div class="form-group">
                <label class="col-sm-4 control-label">Data zwolnienia:</label>
                <div class="col-sm-3">
                    <uc1:DateEdit runat="server" ID="deDataZwol" />
                </div>
            </div>--%>
        </div>
    </ContentTemplate>
    <FooterTemplate>
        <asp:Button ID="btnSaveConfirm" runat="server" Text="Zapisz" CssClass="btn btn-success" OnClick="btnSaveConfirm_Click" ValidationGroup="vgSave" />
        <asp:Button ID="btnSave" runat="server" CssClass="hidden" OnClick="btnSave_Click" ValidationGroup="vgSave" />
    </FooterTemplate>
</uc1:cntModal>

<asp:SqlDataSource ID="dsSave" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
delete from bufPracownicyImport
insert into bufPracownicyImport (Nazwisko, Imie, Identyfikator, DzialWydzial, Funkcja, Kod, KodOpis, Agencja, DataZatrudnienia)
values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', {8})

exec dbo.keeeper_pracownicy_import '{9}'

declare @pracId int
set @pracId = (select Id from Pracownicy where KadryId = '{2}')

insert rcpPracownicyTypyOkresow
select @pracId, {10}, '{9}', null

" />

<asp:SqlDataSource ID="dsData" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
declare @nadzien datetime = '{1}'
declare @pracId int = {0}

select 
	Imie
,	Nazwisko
,	KadryId
,	ps.IdDzialu
,	ps.Klasyfikacja
,	d.Nazwa
,	kp.Kod KodId
,	kpk.Kod
,	kpk.Opis
,	ps.Rodzaj
,   p.DataZatr
,	pto.IdTypuOkresu TypOkresu
from Pracownicy p
left join PracownicyStanowiska ps on ps.IdPracownika = p.Id and @nadzien between ps.Od and isnull(ps.Do, '20990909')
left join Dzialy d on d.Id = ps.IdDzialu
left join rcpKodyPrac kp on kp.IdPracownika = p.Id and @nadzien between kp.Od and isnull(kp.Do, '20990909')
left join rcpKodyPracKody kpk on kpk.Id = kp.Kod
left join rcpPracownicyTypyOkresow pto on pto.IdPracownika = p.Id and @nadzien between pto.DataOd and isnull(pto.DataDo, '20990909')
where p.Id = @pracId
" />

<asp:SqlDataSource ID="dsNrEwid" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
select Nazwisko + ' ' + Imie + ' (' + KadryId + ')' Pracownik from Pracownicy where KadryId = '{0}'    
" 
    UpdateCommand="update Pracownicy set KadryId = '{0}' where Id = {1}"
/>