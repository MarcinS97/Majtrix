<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntImportCSV.ascx.cs" Inherits="HRRcp.Controls.cntImportCSV" %>

<script type="text/javascript">
    function checkFileCSV_<%=Typ%>() {
        fu = document.getElementById('<%=FileUpload1.ClientID%>');
        if (fu != null) {
            if (!fu.value) {
                alert("Brak pliku do importu.");
                return false;
            }
        }
        return true;
    }
</script>

<div id="paImportCSV" runat="server" class="cntImportCSV">
    <asp:FileUpload ID="FileUpload1" CssClass="fileupload" runat="server"  />
    <asp:Button ID="btImport" runat="server" CssClass="button200" Text="Import" onclick="btImport_Click" OnClientClick='javascript:if (checkFileCSV_{0}()) {{showAjaxProgress();return true;}} else return false;' />
    <asp:Label ID="lblInfo" runat="server" CssClass="info" Text="Import za rok:" />
    <asp:Label ID="lbInfo9" runat="server" CssClass="info" Text="Import do okresu rozliczeniowego:" Visible="false"/>
    <asp:DropDownList ID="ddlMiesiac" runat="server" DataSourceID="SqlDataSource1" DataTextField="Data" DataValueField="Value"></asp:DropDownList>
</div> 

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
declare @today datetime = GETDATE()
select CONVERT(varchar(4), rok, 20) as Data, CONVERT(varchar(10), dbo.boy(rok), 20) + '|' + CONVERT(varchar(10), dbo.eoy(rok), 20) as Value
from
(
select DATEADD(YEAR, -Lp, dbo.boy(dbo.getdate(@today))) as rok from dbo.GetDates2('20000101',DATEADD(D, YEAR(@today) - 2015, '20000101'))
) D
    "/>

<asp:SqlDataSource ID="dsListaPlac" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
declare @today datetime 
set @today = DATEADD(D,-15,GETDATE())
select M.Data, M.Data + '-01' as Value from dbo.GetDates2(@today, @today + 1) D
outer apply (select convert(varchar(7),DATEADD(M,-Lp,@today),20) as Data) M
    "/>

<asp:SqlDataSource ID="dsOkresyRozl" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
declare @od datetime = dbo.bom(GETDATE())
select distinct CONVERT(varchar(10), DataOd, 20) as Data, CONVERT(varchar(10), DataOd, 20) as Value
from OkresyRozliczeniowe 
where Status = 0   -- otwarte, powinienem tylko importować na datę pierwszego 3 miesięcznego 
order by 1 desc
    "/>

<%--InsertCommand="Lp.;Nazwisko;Imię (pierwsze);Identyfikator;Stanowisko;Dział Wydział;Oddział;Kod funkcyjny;AGENCJA;Rodzaj zmiany"--%>
<asp:SqlDataSource ID="dsOKTPracownicy" runat="server"    SelectCommandType="StoredProcedure"
    InsertCommand="LP;NazwiskoImie;Jednostka;Funkcja;Kod;KodOpis;Agencja;DataZatrudnienia"
    SelectCommand="exec dbo.keeeper_pracownicy_import '{0}'"/>









<%--<asp:SqlDataSource ID="SqlDataSource2" runat="server"     
    InsertCommand="LP;NazwiskoImie;Jednostka;Funkcja;Kod;KodOpis;Agencja"
    SelectCommand="
BEGIN TRANSACTION;

BEGIN TRY
------------------------
declare @okresOd datetime  -- pierwszy dzień okresu 
--set @okresOd = '20160901'
set @okresOd = '{0}'
    

-- czyszczenie tabeli importu
delete from bufPracownicyImport where Imie = '' and Nazwisko = ''

-- ustawianie nr ewid dla APT
declare @lastEwid int = (select isnull(max(convert(int, right(KadryId, len(KadryId) - 1))), 0) from Pracownicy where left(KadryId, 1) = 'T')

update x
set x.Identyfikator = x.nrEw
from
(
	select Identyfikator, (('T' + right('00000' + convert(varchar, row_number() over (order by a.Nazwisko, a.Imie) + @lastEwid), 5))) nrEw
	from bufPracownicyImport a 
	where A.Identyfikator is null or A.Identyfikator = ''
) x

-- zwalniamy wszystkich 20170113 juz nie, bo importy moga być po kawalku, zwalnianie na formatce z reki
--update Pracownicy set Status = -1 where Status != -2

-- aktualizacja pracowników i słowników
insert into Pracownicy (Imie, Nazwisko, KadryId, Status, DataZatr, DataImportu)
select A.Imie, A.Nazwisko, A.Identyfikator, 1, @okresOd, getdate()
from bufPracownicyImport A
left join Pracownicy P on P.KadryId = A.Identyfikator
where P.Id is null and A.Identyfikator is not null and A.Identyfikator != ''

insert into Dzialy (Nazwa, Status)
select distinct DzialWydzial, 0 
from bufPracownicyImport A
left join Dzialy D on D.Nazwa = A.DzialWydzial
where D.Id is null and A.DzialWydzial is not null and A.DzialWydzial != ''

insert into Stanowiska (Nazwa, Aktywne)
select distinct Stanowisko, 1
from bufPracownicyImport A
left join Stanowiska S on S.Nazwa = A.Stanowisko
where S.Id is null and A.Stanowisko is not null and A.Stanowisko != ''

-- aktualizacja Id
update bufPracownicyImport set 
  Pole1 = P.Id  --pracownik
, Pole2 = D.Id	--dział
, Pole3 = S.Id	--stanowisko
--select * 
from bufPracownicyImport A
left join Pracownicy P on P.KadryId = A.Identyfikator
left join Stanowiska S on S.Nazwa = A.Stanowisko
left join Dzialy D on D.Nazwa = A.DzialWydzial
where P.Id is not null

-- czysczenie przypisań w przód, prócz pomijanych
-- !!! nie usuwam przypisań bieżących bo nie mam danych !!!
--delete from Przypisania where Od &gt; @okresOd and IdPracownika not in (select Id from Pracownicy where Status = -2)
delete from PracownicyStanowiska where Od &gt; @okresOd and IdPracownika not in (select Id from Pracownicy where Status = -2)

-- zamknięcie aktualnych przypisań, nie załapie to przypisań od dnia co będzie znacznikiem kto jest zatrudniony, a kto zwolniony; po pierwszym imporcie wszyscy będą zwolnieni, kolejne importy na tą datę uzupełnią stan zatrudnienia 
--update Przypisania set Do = DATEADD(D, -1, @okresOd) where Od &lt; @okresOd and IdPracownika not in (select Id from Pracownicy where Status = -2)
update PracownicyStanowiska set Do = DATEADD(D, -1, @okresOd) where Od &lt; @okresOd and IdPracownika not in (select Id from Pracownicy where Status = -2)

-- aktualizacja danych Pracowników
update Pracownicy set Imie = A.Imie, Nazwisko = A.Nazwisko, Status = 0, DataImportu = getdate(), LastDataZwol = (case when Status = -1 then DataZwol else null end)
from Pracownicy P 
inner join bufPracownicyImport A on A.Identyfikator = P.KadryId

insert into PracownicyStanowiska (IdPracownika, Od, Do, IdDzialu, IdStanowiska, Grupa, Klasyfikacja, grade, Rodzaj, Import)
select A.Pole1, @okresOd, null, A.Pole2, A.Pole3
, A.Oddzial --. Grupa, nie wiem czy dobrze
, A1.Agencja --. Klasyfikacja
, null
, A.KodFunkcyjny, 1
from bufPracownicyImport A
outer apply (select case when ISNULL(A.Agencja,'') = '' then 'keeeper' else A.Agencja end Agencja) A1
left join PracownicyStanowiska PS on PS.IdPracownika = A.Pole1 and PS.Od = @okresOd
where PS.Id is null and a.Pole1 is not null

-- aktualizacja już istniejących (np jak ponowny import danych na ten sam dzień)
update PracownicyStanowiska set IdDzialu = A.Pole2, IdStanowiska = A.Pole3, Grupa = A.Oddzial, Klasyfikacja = A1.Agencja, Rodzaj = A.KodFunkcyjny
from bufPracownicyImport A
outer apply (select case when ISNULL(A.Agencja,'') = '' then 'keeeper' else A.Agencja end Agencja) A1
left join PracownicyStanowiska PS on PS.IdPracownika = A.Pole1 and PS.Od = @okresOd
where PS.Id is not null

insert into Przypisania (IdPracownika, Od, Do, IdKierownika, Status, Typ)
select p.Id, @okresOd, null, 0, 1, 1
from Pracownicy p
left join Przypisania r on r.IdPracownika = p.Id and r.Status = 1 and @okresOd between r.Od and isnull(r.Do, '20990909')
where r.Id is null and p.Status in (0, 1)



/*
select * from Commodity
select * from bufPracownicyImport
select * from PracownicyStanowiska
select * from Kody

select * from Przypisania
select * into Przypisania_20160909 from Przypisania
select * from Przypisania where Od &gt; '20160101'

select * from bufPracownicyImport A
left join Pracownicy P on P.KadryId = A.Identyfikator
where P.Id is nnull

select * from Pracownicy
select * from Pracownicy where Status = -1
select * from Stanowiska
*/


------------------------
END TRY
BEGIN CATCH
	select -3 as Error, ERROR_MESSAGE() AS ErrorMessage;
	/*
    SELECT 
         ERROR_NUMBER() AS ErrorNumber
        ,ERROR_SEVERITY() AS ErrorSeverity
        ,ERROR_STATE() AS ErrorState
        ,ERROR_PROCEDURE() AS ErrorProcedure
        ,ERROR_LINE() AS ErrorLine
        ,ERROR_MESSAGE() AS ErrorMessage;
	*/
    IF @@TRANCOUNT &gt; 0
        ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT &gt; 0 BEGIN
    COMMIT TRANSACTION;
    select 0 as Error, null as ErrorMessage
END    
    "/>--%>
