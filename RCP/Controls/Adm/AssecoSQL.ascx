<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssecoSQL.ascx.cs" Inherits="HRRcp.Controls.Adm.AssecoSQL" %>

<asp:HiddenField ID="hidVer" runat="server" Value="1.0"/>
 
<asp:SqlDataSource ID="dsImportUrlopZbior" runat="server"
    SelectCommand="
declare @mode int    
declare @data datetime
declare @dzis datetime
set @mode = {0}
set @dzis = '{1}'
declare @boy datetime
declare @eoy datetime

-- brak dbo.boy,eoy
set @boy = DATEADD(dd, 0, DATEDIFF(dd, 0, @dzis))
set @boy = DATEADD(d, -datepart(dy, @boy) + 1, @boy)
set @eoy = DATEADD(D, -1, DATEADD(YEAR, 1, @boy))
set @data = DATEADD(dd, 0, DATEDIFF(dd, 0, @dzis))
set @data = dateadd(d, -datepart(d, @data), dateadd(m, 13 - datepart(m, @data), @data)) 
--select @boy, @eoy, @data

--drop table RCP..tmpUrlopZbior2013 
--drop table #ppp

select a.LpLogo, a.DataZatrudnienia, a.UmowaNumer, a.PierwszaPracaUrlop, a.EtatNum, a.Zatrudniony, a.Wlasny
, l.Data, l.LimitDniZ, l.LimitDniB, l.LimitDniNom, l.WykDniB, l.WykDniZ, l.LimitGodzinZ, l.LimitGodzinB, l.LimitGodzinNom, l.WykGodzinyB, l.WykGodzinyZ
, m.wymiar
, l2.LimitDniZ l2LimitDniZ
, DZ.DataZwiekszenia
into #ppp
from dbo.lp_fn_BasePracExLow(@dzis) a
outer apply (select * from dbo.lp_fn_LimitUrlopuNaDzien(a.LpLogo, a.UmowaNumer, case when a.DataZwolnienia is not null and a.DataZwolnienia &lt;= @data then a.DataZwolnienia else @data end, 'w')) l	-- koniec roku
outer apply (select top 1 * from dbo.lp_fn_UrlopyLimityEx(a.LpLogo, a.UmowaNumer) where UrlopTyp = 'w' and DataOd &lt;= @data order by DataOd desc) m
--outer apply (select * from dbo.lp_fn_LimitUrlopuNaDzien(a.LpLogo, a.UmowaNumer, case when a.DataZwolnienia is not null and a.DataZwolnienia &lt;= @dzis then a.DataZwolnienia else @dzis end, 'w')) l	-- na dzień
--outer apply (select top 1 * from dbo.lp_fn_UrlopyLimityEx(a.LpLogo, a.UmowaNumer) where UrlopTyp = 'w' and DataOd &lt;= @dzis order by DataOd desc) m
outer apply (select * from dbo.lp_fn_LimitUrlopowLimitowanychNaDzienEx(a.LpLogo, a.UmowaNumer, case when a.DataZatrudnienia &gt; @boy then a.DataZatrudnienia else @boy end) where UrlopTyp = 'w') l2
outer apply (select dbo.lp_fn_UrlopyLimitData26(a.LpLogo, a.UmowaNumer, @dzis, 'Urlopowy', 2) as DataZwiekszenia) DZ
outer apply (select 
      l.LimitGodzinNom urlopnom2
	, l.LimitGodzinZ + case when l.WykDniZ &gt; 0 then l.WykGodzinyZ else 0 end urlopzaleg2		-- fix godz są (błąd) jak dni nie ma (poprawne)
    , l.WykGodzinyB + case when l.WykDniZ &gt; 0 then l.WykGodzinyZ else 0 end WykorzystanyDoDnia) a2
--WHERE year(@data) = year(l.Data)
where l.Data between @boy and @eoy 
	--and a.Zatrudniony = 1 
	and a.Wlasny = 1
	and (a2.urlopnom2 != 0 or a2.urlopzaleg2 != 0 or a2.WykorzystanyDoDnia != 0)
    and a.LpDzial = 'IQOR'  --bez fundacji TC

	--and a.LpLogo = '04591' 

select D.Rok, D.NREWID, 
--urlopnom2, urlopzaleg2, WykorzystanyDoDnia, WymiarRok

ISNULL(D.UrlopNomDni, 0) * 8 * EtatNum UrlopNom,               -- UrlopNom [h]
ISNULL(D.UrlopZalegDni, 0) * 8 * EtatNum UrlopZaleg,             -- UrlopZaleg [h]
ISNULL(
case when NREWID = '02159' then WykorzystanyDoDnia  -- Kristina - z dni głupoty zwraca
else D.UrlopWykDni * 8 * EtatNum
end, 0) UrlopWyk,                                            -- UrlopWyk [h]

D.WymiarRok,                                          -- 20/26
D.UrlopNomDni, D.UrlopZalegDni, D.UrlopWykDni,
D.urlopnom2, D.urlopzaleg2, D.WykorzystanyDoDnia, D.DataZwiekszenia,

ISNULL(K.UrlopNomRok, 
case when D.DataZatrudnienia &gt; @boy then              ------- wyliczenie wymiaru urlopu do końca roku
--    ceiling(cast(WymiarRok as float) * (13 - MONTH(DataZatrudnienia)) / 12)
    ceiling(cast(D.WymiarRok as float) * (case when D.PierwszaPracaUrlop = 1 then 12 else 13 end - MONTH(D.DataZatrudnienia)) / 12) -- odjąć by trzeba jeszcze urlop wykorzystany w innej pracy !!! 20151230
else D.WymiarRok
end) as UrlopNomRok


--,LimitDniZ, LimitDniB, LimitDniNom, WykDniB, WykDniZ, LimitGodzinZ, LimitGodzinB, LimitGodzinNom, WykGodzinyB, WykGodzinyZ
--into RCP..tmpUrlopZbior2014

from -----------
(
SELECT 
YEAR(a.Data) as Rok, a.LpLogo NREWID, 
ISNULL(sum(a.EtatNum), 1) as EtatNum,
sum(ISNULL(a.LimitGodzinNom, 0)) urlopnom2,

--sum(l.LimitGodzinZ + l.WykGodzinyZ) urlopzaleg2,
--sum(l.WykGodzinyB + l.WykGodzinyZ) WykorzystanyDoDnia,
sum(a.LimitGodzinZ + case when a.WykDniZ &gt; 0 then a.WykGodzinyZ else 0 end) urlopzaleg2,     -- fix godz są (błąd) jak dni nie ma (poprawne)
sum(a.WykGodzinyB + case when a.WykDniZ &gt; 0 then a.WykGodzinyZ else 0 end) WykorzystanyDoDnia,

sum(a.LimitDniNom) as UrlopNomDni,
--sum(l.LimitDniZ + l.WykDniZ) as UrlopZalegDni,
sum(a.l2LimitDniZ) as UrlopZalegDni,
sum(a.WykDniB + a.WykDniZ) as UrlopWykDni,

max(a.wymiar) as WymiarRok,
max(a.DataZwiekszenia) as DataZwiekszenia,
min(a.DataZatrudnienia) as DataZatrudnienia,

max(a.PierwszaPracaUrlop) as PierwszaPracaUrlop  --plomba

--,sum(LimitDniZ)LimitDniZ, sum(LimitDniB)LimitDniB, sum(LimitDniNom)LimitDniNom, sum(WykDniB)WykDniB, sum(WykDniZ)WykDniZ	,sum(LimitGodzinZ)LimitGodzinZ, sum(LimitGodzinB)LimitGodzinB, sum(LimitGodzinNom)LimitGodzinNom, sum(WykGodzinyB)WykGodzinyB, sum(WykGodzinyZ)WykGodzinyZ

from #ppp a
--FROM dbo.lp_fn_BasePracExLow(@dzis) a 
--cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(a.LpLogo, a.UmowaNumer, case when a.DataZwolnienia is not null and a.DataZwolnienia &lt;= @data then a.DataZwolnienia else @data end, 'w') l	-- koniec roku

--outer apply (select top 1 * from dbo.lp_fn_UrlopyLimityEx(a.LpLogo, a.UmowaNumer) where UrlopTyp = 'w' and DataOd &lt;= @data order by DataOd desc) m
--.cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(a.LpLogo, a.UmowaNumer, case when a.DataZwolnienia is not null and a.DataZwolnienia &lt;= @dzis then a.DataZwolnienia else @dzis end, 'w') l	-- na dzień
--.outer apply (select top 1 * from dbo.lp_fn_UrlopyLimityEx(a.LpLogo, a.UmowaNumer) where UrlopTyp = 'w' and DataOd &lt;= @dzis order by DataOd desc) m

--outer apply (select * from dbo.lp_fn_LimitUrlopowLimitowanychNaDzienEx(a.LpLogo, a.UmowaNumer, case when a.DataZatrudnienia &gt; @boy then a.DataZatrudnienia else @boy end) where UrlopTyp = 'w') l2
--outer apply (select dbo.lp_fn_UrlopyLimitData26(a.LpLogo, a.UmowaNumer, @dzis, 'Urlopowy', 2) as DataZwiekszenia) DZ

WHERE year(@data) = year(a.Data) and a.Zatrudniony = 1 and a.Wlasny = 1
group by a.LpLogo, YEAR(a.Data)

union 

SELECT 
YEAR(a.Data) as Rok, a.LpLogo NREWID, 
ISNULL(sum(a.EtatNum), 1) as EtatNum,
sum(ISNULL(a.LimitGodzinNom, 0)) urlopnom2,

--sum(l.LimitGodzinZ + l.WykGodzinyZ) urlopzaleg2,
--sum(l.WykGodzinyB + l.WykGodzinyZ) WykorzystanyDoDnia,
sum(a.LimitGodzinZ + case when a.WykDniZ &gt; 0 then a.WykGodzinyZ else 0 end) urlopzaleg2,
sum(a.WykGodzinyB +case when a.WykDniZ &gt; 0 then a.WykGodzinyZ else 0 end) WykorzystanyDoDnia,

sum(a.LimitDniNom) as UrlopNomDni,
--sum(l.LimitDniZ + l.WykDniZ) as UrlopZalegDni,
sum(a.l2LimitDniZ) as UrlopZalegDni,
sum(a.WykDniB + a.WykDniZ) as UrlopWykDni,

max(a.wymiar) as WymiarRok,
max(a.DataZwiekszenia) as DataZwiekszenia,
min(a.DataZatrudnienia) as DataZatrudnienia,

max(a.PierwszaPracaUrlop) as PierwszaPracaUrlop  --plomba

--,sum(LimitDniZ)LimitDniZ, sum(LimitDniB)LimitDniB, sum(LimitDniNom)LimitDniNom, sum(WykDniB)WykDniB, sum(WykDniZ)WykDniZ	,sum(LimitGodzinZ)LimitGodzinZ, sum(LimitGodzinB)LimitGodzinB, sum(LimitGodzinNom)LimitGodzinNom, sum(WykGodzinyB)WykGodzinyB, sum(WykGodzinyZ)WykGodzinyZ

from #ppp a
--FROM dbo.lp_fn_BasePracExLow(@dzis) a 
--cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(a.LpLogo, a.UmowaNumer, case when a.DataZwolnienia is not null and a.DataZwolnienia &lt;= @data then a.DataZwolnienia else @data end, 'w') l	-- koniec roku

--outer apply (select top 1 * from dbo.lp_fn_UrlopyLimityEx(a.LpLogo, a.UmowaNumer) where UrlopTyp = 'w' and DataOd &lt;= @data order by DataOd desc) m
--.cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(a.LpLogo, a.UmowaNumer, case when a.DataZwolnienia is not null and a.DataZwolnienia &lt;= @dzis then a.DataZwolnienia else @dzis end, 'w') l	-- na dzień
--.outer apply (select top 1 * from dbo.lp_fn_UrlopyLimityEx(a.LpLogo, a.UmowaNumer) where UrlopTyp = 'w' and DataOd &lt;= @dzis order by DataOd desc) m
--outer apply (select * from dbo.lp_fn_LimitUrlopowLimitowanychNaDzienEx(a.LpLogo, a.UmowaNumer, case when a.DataZatrudnienia &gt; @boy then a.DataZatrudnienia else @boy end) where UrlopTyp = 'w') l2
--outer apply (select dbo.lp_fn_UrlopyLimitData26(a.LpLogo, a.UmowaNumer, @dzis, 'Urlopowy', 2) as DataZwiekszenia) DZ

WHERE year(@data) = year(a.Data) and a.Zatrudniony = 0 and a.Wlasny = 1
and a.LpLogo not in 
	(
	select aa.LpLogo 
	
	from #ppp aa
	--from dbo.lp_fn_BasePracExLow(@dzis) aa 
	--cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(aa.LpLogo, aa.UmowaNumer, case when aa.DataZwolnienia is not null and aa.DataZwolnienia &lt;= @data then aa.DataZwolnienia else @data end, 'w') ll	-- koniec roku
	
	--cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(aa.LpLogo, aa.UmowaNumer, case when aa.DataZwolnienia is not null and aa.DataZwolnienia &lt;= @dzis then aa.DataZwolnienia else @dzis end, 'w') ll	-- na dzień
	WHERE year(@data) = year(aa.Data) and aa.Zatrudniony = 1 and aa.Wlasny = 1
	)
group by a.LpLogo, YEAR(a.Data)

) D
left join RCP..UrlopZbiorKorekta K on K.Rok = YEAR(@data) and K.KadryId = D.NREWID
where (D.urlopnom2 != 0 or D.urlopzaleg2 != 0 or D.WykorzystanyDoDnia != 0)

drop table #ppp
--select * from #ppp
    ">
</asp:SqlDataSource>

<%--
declare @mode int    
declare @data datetime
declare @dzis datetime
set @mode = {0}
set @dzis = '{1}'
declare @boy datetime

-- brak dbo.boy,eoy
set @boy = DATEADD(dd, 0, DATEDIFF(dd, 0, @dzis))
set @boy = DATEADD(d, -datepart(dy, @boy) + 1, @boy)
set @data = DATEADD(dd, 0, DATEDIFF(dd, 0, @dzis))
set @data = dateadd(d, -datepart(d, @data), dateadd(m, 13 - datepart(m, @data), @data)) 

--drop table RCP..tmpUrlopZbior2013 

select Rok, NREWID, 
--urlopnom2, urlopzaleg2, WykorzystanyDoDnia, WymiarRok

ISNULL(UrlopNomDni, 0) * 8 * EtatNum,               -- UrlopNom [h]
ISNULL(UrlopZalegDni, 0) * 8 * EtatNum,             -- UrlopZaleg [h]
ISNULL(
case when NREWID = '02159' then WykorzystanyDoDnia  -- Kristina - z dni głupoty zwraca
else UrlopWykDni * 8 * EtatNum
end, 0),                                            -- UrlopWyk [h]

WymiarRok,                                          -- 20/26
UrlopNomDni, UrlopZalegDni, UrlopWykDni,
urlopnom2, urlopzaleg2, WykorzystanyDoDnia, DataZwiekszenia,

case when DataZatrudnienia &gt; @boy then              ------- wyliczenie wymiaru urlopu do końca roku
--    ceiling(cast(WymiarRok as float) * (13 - MONTH(DataZatrudnienia)) / 12)
    ceiling(cast(WymiarRok as float) * (case when PierwszaPracaUrlop = 1 then 12 else 13 end - MONTH(DataZatrudnienia)) / 12) -- odjąć by trzeba jeszcze urlop wykorzystany w innej pracy !!! 20151230
else WymiarRok
end as UrlopNomRok


--,LimitDniZ, LimitDniB, LimitDniNom, WykDniB, WykDniZ, LimitGodzinZ, LimitGodzinB, LimitGodzinNom, WykGodzinyB, WykGodzinyZ
--into RCP..tmpUrlopZbior2014

from -----------
(
SELECT 
YEAR(l.Data) as Rok, a.LpLogo NREWID, 
ISNULL(sum(a.EtatNum), 1) as EtatNum,
sum(ISNULL(l.LimitGodzinNom, 0)) urlopnom2,

--sum(l.LimitGodzinZ + l.WykGodzinyZ) urlopzaleg2,
--sum(l.WykGodzinyB + l.WykGodzinyZ) WykorzystanyDoDnia,
sum(l.LimitGodzinZ + case when l.WykDniZ &gt; 0 then l.WykGodzinyZ else 0 end) urlopzaleg2,     -- fix godz są (błąd) jak dni nie ma (poprawne)
sum(l.WykGodzinyB + case when l.WykDniZ &gt; 0 then l.WykGodzinyZ else 0 end) WykorzystanyDoDnia,

sum(l.LimitDniNom) as UrlopNomDni,
--sum(l.LimitDniZ + l.WykDniZ) as UrlopZalegDni,
sum(l2.LimitDniZ) as UrlopZalegDni,
sum(l.WykDniB + l.WykDniZ) as UrlopWykDni,

max(m.wymiar) as WymiarRok,
max(DZ.DataZwiekszenia) as DataZwiekszenia,
min(a.DataZatrudnienia) as DataZatrudnienia,

max(a.PierwszaPracaUrlop) as PierwszaPracaUrlop  --plomba

--,sum(LimitDniZ)LimitDniZ, sum(LimitDniB)LimitDniB, sum(LimitDniNom)LimitDniNom, sum(WykDniB)WykDniB, sum(WykDniZ)WykDniZ	,sum(LimitGodzinZ)LimitGodzinZ, sum(LimitGodzinB)LimitGodzinB, sum(LimitGodzinNom)LimitGodzinNom, sum(WykGodzinyB)WykGodzinyB, sum(WykGodzinyZ)WykGodzinyZ
FROM dbo.lp_fn_BasePracExLow(@dzis) a 
cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(a.LpLogo, a.UmowaNumer, case when a.DataZwolnienia is not null and a.DataZwolnienia &lt;= @data then a.DataZwolnienia else @data end, 'w') l	-- koniec roku
outer apply (select top 1 * from dbo.lp_fn_UrlopyLimityEx(a.LpLogo, a.UmowaNumer) where UrlopTyp = 'w' and DataOd &lt;= @data order by DataOd desc) m
--cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(a.LpLogo, a.UmowaNumer, case when a.DataZwolnienia is not null and a.DataZwolnienia &lt;= @dzis then a.DataZwolnienia else @dzis end, 'w') l	-- na dzień
--outer apply (select top 1 * from dbo.lp_fn_UrlopyLimityEx(a.LpLogo, a.UmowaNumer) where UrlopTyp = 'w' and DataOd &lt;= @dzis order by DataOd desc) m
outer apply (select * from dbo.lp_fn_LimitUrlopowLimitowanychNaDzienEx(a.LpLogo, a.UmowaNumer, case when a.DataZatrudnienia &gt; @boy then a.DataZatrudnienia else @boy end) where UrlopTyp = 'w') l2

outer apply (select dbo.lp_fn_UrlopyLimitData26(a.LpLogo, a.UmowaNumer, @dzis, 'Urlopowy', 2) as DataZwiekszenia) DZ

WHERE year(@data) = year(l.Data) and a.Zatrudniony = 1 and a.Wlasny = 1
group by a.LpLogo, YEAR(l.Data)

union 

SELECT 
YEAR(l.Data) as Rok, a.LpLogo NREWID, 
ISNULL(sum(a.EtatNum), 1) as EtatNum,
sum(ISNULL(l.LimitGodzinNom, 0)) urlopnom2,

--sum(l.LimitGodzinZ + l.WykGodzinyZ) urlopzaleg2,
--sum(l.WykGodzinyB + l.WykGodzinyZ) WykorzystanyDoDnia,
sum(l.LimitGodzinZ + case when l.WykDniZ &gt; 0 then l.WykGodzinyZ else 0 end) urlopzaleg2,
sum(l.WykGodzinyB +case when l.WykDniZ &gt; 0 then l.WykGodzinyZ else 0 end) WykorzystanyDoDnia,

sum(l.LimitDniNom) as UrlopNomDni,
--sum(l.LimitDniZ + l.WykDniZ) as UrlopZalegDni,
sum(l2.LimitDniZ) as UrlopZalegDni,
sum(l.WykDniB + l.WykDniZ) as UrlopWykDni,

max(m.wymiar) as WymiarRok,
max(DZ.DataZwiekszenia) as DataZwiekszenia,
min(a.DataZatrudnienia) as DataZatrudnienia,

max(a.PierwszaPracaUrlop) as PierwszaPracaUrlop  --plomba

--,sum(LimitDniZ)LimitDniZ, sum(LimitDniB)LimitDniB, sum(LimitDniNom)LimitDniNom, sum(WykDniB)WykDniB, sum(WykDniZ)WykDniZ	,sum(LimitGodzinZ)LimitGodzinZ, sum(LimitGodzinB)LimitGodzinB, sum(LimitGodzinNom)LimitGodzinNom, sum(WykGodzinyB)WykGodzinyB, sum(WykGodzinyZ)WykGodzinyZ
FROM dbo.lp_fn_BasePracExLow(@dzis) a 
cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(a.LpLogo, a.UmowaNumer, case when a.DataZwolnienia is not null and a.DataZwolnienia &lt;= @data then a.DataZwolnienia else @data end, 'w') l	-- koniec roku
outer apply (select top 1 * from dbo.lp_fn_UrlopyLimityEx(a.LpLogo, a.UmowaNumer) where UrlopTyp = 'w' and DataOd &lt;= @data order by DataOd desc) m
--cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(a.LpLogo, a.UmowaNumer, case when a.DataZwolnienia is not null and a.DataZwolnienia &lt;= @dzis then a.DataZwolnienia else @dzis end, 'w') l	-- na dzień
--outer apply (select top 1 * from dbo.lp_fn_UrlopyLimityEx(a.LpLogo, a.UmowaNumer) where UrlopTyp = 'w' and DataOd &lt;= @dzis order by DataOd desc) m
outer apply (select * from dbo.lp_fn_LimitUrlopowLimitowanychNaDzienEx(a.LpLogo, a.UmowaNumer, case when a.DataZatrudnienia &gt; @boy then a.DataZatrudnienia else @boy end) where UrlopTyp = 'w') l2

outer apply (select dbo.lp_fn_UrlopyLimitData26(a.LpLogo, a.UmowaNumer, @dzis, 'Urlopowy', 2) as DataZwiekszenia) DZ

WHERE year(@data) = year(l.Data) and a.Zatrudniony = 0 and a.Wlasny = 1
and a.LpLogo not in (
	select aa.LpLogo 
	from dbo.lp_fn_BasePracExLow(@dzis) aa 
	cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(aa.LpLogo, aa.UmowaNumer, case when aa.DataZwolnienia is not null and aa.DataZwolnienia &lt;= @data then aa.DataZwolnienia else @data end, 'w') ll	-- koniec roku
	--cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(aa.LpLogo, aa.UmowaNumer, case when aa.DataZwolnienia is not null and aa.DataZwolnienia &lt;= @dzis then aa.DataZwolnienia else @dzis end, 'w') ll	-- na dzień
	WHERE year(@data) = year(ll.Data) and aa.Zatrudniony = 1 and aa.Wlasny = 1)
group by a.LpLogo, YEAR(l.Data)

) D
where (urlopnom2 != 0 or urlopzaleg2 != 0 or WykorzystanyDoDnia != 0)
--%>


<asp:SqlDataSource ID="dsImportUpdateZalegDod" runat="server"
    SelectCommand="">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsImportUmowy" runat="server"
    SelectCommand="
select lp_UmowyId, LpLogo, UmowaNumer, DataOd, DataDo, UmowaOd, UmowaDo, DataZatrudnienia, DataZwolnienia, UmowaTyp, DataRozpoczeciaPracy, DataRozwUmowy,
-1, -1
from lp_Umowy
        ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsImport" runat="server"
    SelectCommand="
BEGIN TRANSACTION;
BEGIN TRY
-------------------------------
-------------------------------

-------------------------------
-------------------------------
END TRY
BEGIN CATCH
	select -1 as Error, ERROR_MESSAGE() AS ErrorMessage, @step as Step
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
    select 0 as Error, null as ErrorMessage, @step as Step
END    
    ">
</asp:SqlDataSource>

<%-----%>
<asp:SqlDataSource ID="dsExportAfterRCP" runat="server"
    SelectCommand="
----- aktualizacja delegacji ----    
declare @mode int
declare @od datetime
declare @do datetime
set @mode = {0}
set @od = '{1}'
set @do = '{2}'
set @do = dbo.eom(@od)  -- nadpisuję
--1000090081

update JGBHR01.RCP.dbo.DaneRCP set CzasZm = P1.CzasZm, Uwagi = 'Delegacja - autouzupełnienie'
--select P1.*, D.*
--into #ppp
from JGBHR01.RCP.dbo.DaneRCP D
left join Pracownicy P on P.KadryId = D.NR_EW
outer apply (select dbo.ToTime(P.EtatL * 8 * 3600 / P.EtatM) CzasZm) P1
inner join Absencja A on A.IdPRacownika = P.Id and A.Kod = 1000090081 and D.Data between A.DataOd and A.DataDo
where D.Data between @od and @do
and D.IdZmiany is not null
--and dbo.TimeToSec(ISNULL(D.CzasZm,'0:00')) = 0
and (D.CzasZm is null or D.CzasZm = '0:00:00')  --szybsze
and (D.Nadg50 is null or D.Nadg50 = '0:00:00')
and (D.Nadg100 is null or D.Nadg100 = '0:00:00')
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsExportAfterAsseco" runat="server"
    SelectCommand="
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsImportFindPrac" runat="server"
    SelectCommand="
declare 
    @logo varchar(50)
  , @pesel varchar(50)
  , @nrdok varchar(50)
  , @typdok varchar(50)
set @logo = '{0}'
set @pesel = '{1}'
set @nrdok = '{2}'
set @typdok = '{3}'

select Id,Nick,Status 
from Pracownicy 
where KadryId = @logo 
 or Status = -6 and (Nick = @pesel or @pesel is null and @nrdok like '%' + Nick + '%')  -- Nick dla -6 może być nr paszportu bez liter  
    ">
</asp:SqlDataSource>





