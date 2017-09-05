<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RepCCSplity.aspx.cs" Inherits="HRRcp.Reports.RepCCSplity" %>
<%@ Register src="~/Controls/Reports/cntReport.ascx" tagname="cntReport" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<%--
raport - 
p1 - od
p2 - do
--%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">    
    <div class="report_page">
    <uc1:cntReport ID="cntReport2" runat="server" Browser="true"
        Title="Splity obowiązujące w okresie: @p1 - @p2"
        CssClass=""
        SQL="
declare 
    @colsH nvarchar(max), 
    @colsD nvarchar(max), 
    @stmt nvarchar(max)

select @colsH = isnull(@colsH + ',', '') + '[' + wCC.cc + ']',
       @colsD = isnull(@colsD + ', ', '') + 'ISNULL(ROUND([' + wCC.cc + '],4),0) as [' + wCC.cc + ':N||' + wCC.Nazwa + ']'  
    from CC
    left outer join Splity S on S.GrSplitu = CC.GrSplitu and '@p2' between S.DataOd and ISNULL(S.DataDo, '20990909')
    left outer join SplityWsp W on W.IdSplitu = S.Id
    left outer join CC wCC on wCC.Id = W.IdCC
    where CC.GrSplitu is not null and '@p2' between CC.AktywneOd and ISNULL(CC.AktywneDo, '20990909')
    order by CC.cc, wCC.cc 

select @stmt = '
declare @dataDo datetime
declare @dataOd datetime
set @dataOd = ''@p1''
set @dataDo = ''@p2''

SELECT grCC, grNazwa, ' + @colsD +
'FROM
(
	select CC.cc as grCC, CC.Nazwa as grNazwa, wCC.cc, W.Wsp
	from CC
	left outer join Splity S on S.GrSplitu = CC.GrSplitu and @dataDo between S.DataOd and ISNULL(S.DataDo, @dataDo)
	left outer join SplityWsp W on W.IdSplitu = S.Id
	left outer join CC wCC on wCC.Id = W.IdCC
	where CC.GrSplitu is not null and @dataDo between CC.AktywneOd and ISNULL(CC.AktywneDo, @dataDo)
) as D
PIVOT
(
	sum(Wsp) FOR D.cc IN (' + @colsH + ')
) as PV
order by grCC'

exec sp_executesql @stmt
    "/>
    <br />
    <br />
    <uc1:cntReport ID="cntReport1" runat="server" Browser="true"
        Title="select 'Split obowiązujący w okresie: ' + convert(varchar(10),'@p1',20) + ' - ' + convert(varchar(10),'@p2',20)"
        CssClass=""
        HeaderVisible="false"
        SQL1="
join ccPrawa PC on PC.UserId = @UserId and PC.CC = ISNULL(I.Class, PC.CC)
        "
        SQL3="
join ccPrawa PR on PR.UserId = @UserId and PR.CC = CC.cc
        "
        SQL="
declare 
    @colsH nvarchar(max), 
    @colsD nvarchar(max), 
    @stmt nvarchar(max)

select 
    @colsH = isnull(@colsH + ',', '') + '[' + CC.cc + ']',
    @colsD = isnull(@colsD + ', ', '') + 'ISNULL(ROUND([' + CC.cc + '],4),0) as [' + CC.cc + ':N||' + CC.Nazwa + ']'  
    from CC 
    @SQL3
    where '@p2' between AktywneOd and ISNULL(AktywneDo, '20990909') 
    order by CC.cc

select @stmt = '
declare @dataDo datetime
declare @dataOd datetime
set @dataOd = ''@p1''
set @dataDo = ''@p2''

SELECT 
    P.KadryId as [nrew:-], 
    P.KadryId as [Nr ew.], 
    P.Nazwisko + '' '' + P.Imie as [Pracownik|RepCCPracCC @p1 @p2 * 0 @nrew 1 1|Czas przepracowany na wszystkie cc], 
	D.Nazwa as [Dział], ST.Nazwa as Stanowisko, I.Class as Klasyfikacja,
	K.KadryId as [Nr ew. K], K.Nazwisko + '' '' + K.Imie as Kierownik, ' +
	@colsD +
'FROM
(
	SELECT S.GrSplitu, CC.cc, W.Wsp
	FROM Splity S
	left outer join SplityWsp W on W.IdSplitu = S.Id
	left outer join CC on CC.Id = W.IdCC
	@SQL3
	where @dataDo between S.DataOd and ISNULL(S.DataDo, ''20990909'')
) as D
PIVOT
(
	sum(Wsp) FOR D.cc IN (' + @colsH + ')
) as PV
left outer join Pracownicy P on P.GrSplitu = PV.GrSplitu
left outer join PodzialLudziImport I on I.KadryId = P.KadryId and @dataDo between I.OkresOd and ISNULL(I.OkresDo, ''20990909'')
@SQL1
left outer join Pracownicy K on K.Id = P.IdKierownika
left outer join Dzialy D on D.Id = P.IdDzialu
left outer join Stanowiska ST on ST.Id = P.IdStanowiska
where I.Id is not null'

exec sp_executesql @stmt
    "/>
    </div>
</asp:Content>


<%--
        GEN="select --top 3
'ISNULL(ROUND((select W.Wsp from Splity S join SplityWsp W on W.IdSplitu = S.Id where @ddo between S.DataOd and ISNULL(S.DataDo, @ddo) and S.GrSplitu = P.GrSplitu and W.IdCC = ' + CONVERT(varchar, Id) + '), 4), 0) as [' + cc + ':N||' + cc + ']' 
from CC where '@p2' between AktywneOd and ISNULL(AktywneDo, '20990909') order by cc"
        SQL="
declare @dod datetime
declare @ddo datetime
set @dod = '@p1'
set @ddo = '@p2'

select 
	P.KadryId as [Nr ew.], P.Nazwisko + ' ' + P.Imie as Pracownik, 
	D.Nazwa as Dzial, ISNULL(S.Nazwa, PL.Stanowisko) as Stanowisko, PL.Class as Klasyfikacja,
	K.KadryId as [Nr ew. K], 
	K.Nazwisko + ' ' + K.Imie as Kierownik,
	@GEN
from PodzialLudziImport PL 
left outer join Pracownicy P on P.KadryId = PL.KadryId
left outer join Pracownicy K on K.Id = P.IdKierownika
left outer join Dzialy D on D.Id = P.IdDzialu
left outer join Stanowiska S on S.Id = P.IdStanowiska
where @ddo between PL.OkresOd and ISNULL(PL.OkresDo, @ddo);
--%>

