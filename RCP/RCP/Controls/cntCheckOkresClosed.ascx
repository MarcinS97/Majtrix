<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntCheckOkresClosed.ascx.cs" Inherits="HRRcp.RCP.Controls.cntCheckOkresClosed" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/Controls/Reports/cntReport2.ascx" TagPrefix="uc1" TagName="cntReport2" %>

<div runat="server" id="paCheckOkresClosed" class="cntCheckOkresClosed">
    <uc1:cntModal runat="server" ID="cntModal" Title="Zamknięcie miesiąca niemożliwe" WidthType="Large">
        <ContentTemplate>
            <asp:HiddenField ID="hidData" runat="server" Visible="false"/>
            <center>
                <uc1:cntReport2 runat="server" ID="cntReport" GridVisible="false" CssClass="report_page noborder report-header" />
            </center>
        </ContentTemplate>
        <FooterTemplate>
            <asp:UpdatePanel runat="server" ID="upCSV">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExportCSV" />
                </Triggers>
                <ContentTemplate>
                    <asp:Button runat="server" ID="btnExportCSV" Text="Eksport do CSV" OnClick="btnExportCSV_Click" CssClass="btn btn-success" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </FooterTemplate>
    </uc1:cntModal>
</div>

<asp:SqlDataSource ID="dsGetKierNotClosedNew" runat="server"
        SelectCommand="
declare @od datetime = '{0}'
declare @do datetime = '{1}'
declare @kierId int = {2}

/*
select
  a.Kierownik Przełożony
, a.Email Email
, case when a.Planowanie = 0 then 'BRAK' else '' end [Plan pracy]
, case when a.Akceptacja = 0 then 'BRAK' else '' end [Akceptacja czasu pracy]
from
	(
	select
	  ISNULL(k.Nazwisko + ' ' + k.Imie + ISNULL(' (' + k.KadryId + ')', ''), ' Poziom główny struktury') Kierownik
    , ISNULL(k.Email, '') Email
	, case when COUNT(pp.Id) = 0 then 0 else 1 end Planowanie
	, case when SUM(CAST(ISNULL(pp.Akceptacja, 0) as int)) = COUNT(pp.Id) and COUNT(pp.Id) != 0 then 1 else 0 end Akceptacja

	from Przypisania r
	left join Pracownicy k on k.Id = r.IdKierownika
	left join PlanPracy pp on pp.IdPracownika = r.IdPracownika
	left join PracownicyParametry pr on pr.IdPracownika = r.IdPracownika and @do between pr.Od and ISNULL(pr.Do, '20990909')
	left join PlanUrlopowPomin x on x.IdPracownika = r.IdPracownika and @do between x.Od and ISNULL(x.Do, '20990909')
	where (r.Status = 1 and (@kierId = 0 or (r.IdKierownika = @kierId or r.IdKierownika = 0)) and r.Od &lt;= @do and ISNULL(r.Do, '20990909') &gt;= @od)
	and x.Id is null
	and pr.RcpAlgorytm != 0
	group by k.Id, k.Nazwisko, k.Imie, k.KadryId, k.Email
) a
where a.Planowanie = 0 or a.Akceptacja = 0
order by a.Kierownik
*/

select
  ISNULL(k.Nazwisko + ' ' + k.Imie + ISNULL(' (' + k.KadryId + ')', ''), ' Poziom główny struktury') [Przełożony:C]
, ISNULL(k.Email, '') [Email]
, ISNULL(case when pv.[1] = 1 then 'x' else '' end, 0) [Brak planu pracy]
, ISNULL(case when pv.[0] = 1 then 'x' else '' end, 0) [Brak akceptacji czasu pracy]
, ISNULL(case when pv.[2] = 1 then 'x' else '' end, 0) [Wnioski o nadgodziny do akceptacji]
, ISNULL(case when pv.[3] = 1 then 'x' else '' end, 0) [Niezbilansowane rozliczenie nadgodzin]
from
(
	select distinct
	  R.IdKierownika as Id
	, 0 c
	, 1 v
	from Przypisania R
	left join Pracownicy K on K.Id = R.IdKierownika
	left join PlanPracy PP on PP.IdPracownika = R.IdPracownika and PP.Data between dbo.MaxDate2(@od, R.Od) and dbo.MinDate2(@do, ISNULL(R.Do, '20990909'))
	where R.Od &lt;= @do and @od &lt;= ISNULL(R.Do,'20990909') and R.Status = 1 /*and R.IdPracownika in (
		select distinct IdPracownika from PlanPracy where Data between @od and @do
	)*/
	and ISNULL(PP.Akceptacja, 0) = 0

	union all
	----- brak PP ------
	select --P.*,PR.RcpAlgorytm 
	-- P.KadryId as Logo, P.Nazwisko + ' ' + P.Imie as Pracownik, P.KierKadryId, P.KierownikNI as Kierownik, P.Status, P.Opis, P.DataZatr, P.DataZwol
	distinct
	  R.IdKierownika as Id
	, 1 c
	, 1 v
	from Pracownicy P
	left join Przypisania R on R.IdPracownika = P.Id and @do between R.Od and ISNULL(R.Do,'20990909') and R.Status = 1 
	left join Pracownicy K on K.Id = R.IdKierownika
	left join PracownicyParametry PR on PR.IdPracownika = P.Id and @do between PR.Od and ISNULL(PR.Do, '20990909')
	left outer join PlanPracy PP on PP.IdPracownika = P.Id and PP.Data between dbo.bom(@do) and dbo.eom(@do) and ISNULL(IdZmianyKorekta, IdZmiany) is not null
	left join PlanUrlopowPomin X on X.IdPracownika = P.Id and @do between X.Od and ISNULL(X.Do, '20990909')
	where PP.Id is null and P.status in (0,1) /*and P.KadryId between 0 and 80000-1*/
	and X.Id is null
	and R.IdKierownika is not null
	and PR.RcpAlgorytm != 0
	--order by Opis,Kierownik, Pracownik
	union all
	select distinct
	  p.Id
	, 2 c
	, 1 v
	from Pracownicy p
	where dbo.GetRightId(p.Rights, 98) = 1
	and p.Status in (0, 1)
	and (select COUNT(*) from rcpNadgodzinyWnioski nw where nw.Data between @od and @do and nw.Status = 1) &gt; 0
	union all

	select
	  r.IdKierownika
	/****************************************************************\
	, ROUND(NS.Niedomiar + NS.WND + NS.OND,          4) as drNiedomiar
	, ROUND(NS.N50  - (NS.P50 + NS.W50 + NS.O50),    4) as dr50
	, ROUND(NS.N100 - (NS.P100 + NS.W100 + NS.O100), 4) as dr100
	\****************************************************************/
	, 3 c
	, 1 v
	from Przypisania r
	inner join VRozliczenieNadgodzinOkresy ns on ns.IdPracownika = r.IdPracownika
	where (r.IdKierownika = @kierId or @kierId = 0) and @do between R.Od and ISNULL(R.Do,'20990909') and R.Status = 1
	and (ns.DataOd = dbo.bom(@do) or ns.DataDo = DATEADD(MONTH, -1, dbo.bom(@do)))
	and
	(
		/*ROUND(NS.Niedomiar + NS.WND + NS.OND,          4) != 0
		or*/
		ROUND(NS.N50  - (NS.P50 + NS.W50 + NS.O50),    4) != 0
		or
		ROUND(NS.N100 - (NS.P100 + NS.W100 + NS.O100), 4) != 0
	)
    and @do = dbo.eom(@do)
) d
PIVOT
(
	MAX(d.v) for d.c in ([0], [1], [2], [3])
) pv
left join Pracownicy k on k.Id = pv.Id
    order by [Przełożony:C]

"></asp:SqlDataSource>
