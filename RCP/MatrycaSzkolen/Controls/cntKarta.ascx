<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntKarta.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.cntKarta" %>


<div class="cntKarta">

</div>

<asp:SqlDataSource ID="dsRow" runat="server" SelectCommand=
"
select 
Nazwisko + ' ' + Imie Pracownik 
from Pracownicy where Id = {0}
" 
/>


<asp:SqlDataSource ID="dsTable" runat="server" SelectCommand="
declare @pracId int = {0}
    
select
  u.Nazwa
, CONVERT(varchar(10), c.DataRozpoczecia, 20) DataOd
, CONVERT(varchar(10), c.DataZakonczenia, 20) DataDo
, p.Imie + ' ' + p.Nazwisko Trener
, null [Podpis pracownika*]
, null [Podpis kierownika komórki organizacyjnej **]
from Uprawnienia u
left join Certyfikaty c on c.IdUprawnienia = u.Id and c.IdPracownika = @pracId
left join UprawnieniaKwalifikacje uk on uk.Id = u.KwalifikacjeId
left join Pracownicy p on p.Id = c.IdAutora
where u.Typ = 2048 and uk.NazwaEN = 'STAN'
order by u.Kolejnosc     
" />