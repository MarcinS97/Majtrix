<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntEmployeesZoom.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Spreadsheets.cntEmployeesZoom" %>

<%@ Register Src="~/Controls/Reports/cntReport2.ascx" TagPrefix="leet" TagName="Report" %>


    <asp:HiddenField ID="hidScorecardTypeId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidDate" runat="server" Visible="false" />
    <asp:HiddenField ID="hidObserverId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidTeamLeader" runat="server" Visible="false" />
    <asp:HiddenField ID="hidCol" runat="server" Visible="false" />
    
    
    
<%--        <leet:Report    ID="RepMain"
                        runat="server"
                        Title="Czynności"
                        Pager="false"
                        SQL="
declare @date datetime = @SQL1
declare @ObserverId int = @SQL2
declare @typark int = @SQL3
declare @tl bit = @SQL4
--declare @oj bit = 0

select
  p.Imie + ' ' + p.Nazwisko + ISNULL(' (' + p.KadryId + ')', '') as Pracownik --[eee]
, ppoa.Nominal --[aasd]
, pp2oa.GodzNieob --[qwe]
, ppoa.Nadgodziny50 --[aa]
, ppoa.Nadgodziny100 --[bb]
from Przypisania prz
left join Pracownicy p on prz.IdPracownika = p.Id

left join PlanPracy pp on pp.IdPracownika = prz.IdPracownika and @date = pp.Data
left join Absencja a on prz.IdPracownika &gt; 0 and a.IdPracownika = prz.IdPracownika and @date between a.DataOd and a.DataDo
left join AbsencjaKody ak on a.Kod = ak.Kod
outer apply (select case when ISNULL(ISNULL(pp.IdZmianyKorekta, pp.IdZmiany), -1) != -1 then 8 else 0 end as Nominal, CONVERT(float, ISNULL(pp.n50, 0))/3600 as Nadgodziny50, CONVERT(float, ISNULL(pp.n100, 0))/3600 as Nadgodziny100) ppoa --do zmiany n i s
outer apply (select ppoa.Nominal - (ISNULL(CONVERT(float, pp.CzasZm), 0)/3600) as GodzNieob) pp2oa

where (@date between prz.Od and ISNULL(prz.Do, '20990909') and prz.Status = 1) and prz.IdCommodity = @typark and prz.IdKierownika = @ObserverId                        
                        
"
                        />--%>


                    <asp:Button ID="btClose2" CssClass="button100" runat="server" Text="Test" Visible="false" />


                    <div class="gv_scroll" style="text-align: left !important;">
                    <asp:GridView ID="gvEmployees" runat="server" CssClass="GridView2" DataKeyNames="Id:-" 
                        DataSourceID="dsEmployees" 
                            
                         >
                    </asp:GridView>
                    </div>
        
        
        
<%--
        <div class="bottom_buttons">
            <asp:Button ID="btClose" CssClass="button100" runat="server" Text="Zamknij" />
        </div>
--%>        
        
        
<asp:SqlDataSource ID="dsEmployees" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"  SelectCommand="
--declare @date datetime = '20140901'
--declare @ObserverId int = 2323
--declare @typark int = 7
--declare @tl bit = 0
--declare @oj bit = 0


declare @kanapkaCzas as float = ISNULL(CONVERT(float, (select Kod from Kody where Typ = 'SCCZAS' and Aktywny = 1)) / 60, 6)

--declare @col int = 3
select
  ROW_NUMBER() OVER(ORDER BY p.Nazwisko) AS Lp --[:-] 
,  prz.Id [Id:-]
, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') as Pracownik
, ppoa.Nominal as [Czas Nominalny:N]
--, LEFT(CONVERT(nvarchar, pp.CzasIn, 20), 10) as Wejście
--, LEFT(CONVERT(nvarchar, pp.CzasOut, 20), 10) as Wyjście
, Z.Symbol + ' - ' + Z.Nazwa as [Zmiana]
, ROUND(ppoa.vCzasZm,3) as [Czas zmiany:N]
, ROUND(ppoa.Nadgodziny50,3) as [Nadgodziny 50:N]
, ROUND(ppoa.Nadgodziny100,3) as [Nadgodziny 100:N]
, ROUND(ppoa.Nocne,3) as [Czas nocny:N]
, ROUND(pp2oa.GodzNieob,3) as [Godz. Nieob.:N]
, ak.Symbol + ' - ' + ak.Nazwa as [Absencja]
, round(ISNULL(case when ISNULL(ppoa.Nominal, 0) = ISNULL(pp2oa.GodzNieob, 0) then 0 + ISNULL(ppoa.Nadgodziny50 + ppoa.Nadgodziny100, 0) else ISNULL(ppoa.Nominal, 0) - ISNULL(pp2oa.GodzNieob, 0) - ISNULL(d.PracaInnyArkusz, 0) - ISNULL(d.CzasNieprod, 0) + ISNULL(ppoa.Nadgodziny50 + ppoa.Nadgodziny100, 0) end, 0), 4) as [Czas produktywny:N]
, round(ISNULL(case when ISNULL(ppoa.Nominal, 0) = ISNULL(pp2oa.GodzNieob, 0) then 0 + ISNULL(ppoa.Nadgodziny50 + ppoa.Nadgodziny100, 0) - (ISNULL(s.Parametr, 0) * aoa.Alive) else case when ISNULL(ppoa.Nominal, 0) + ISNULL(ppoa.Nadgodziny50 + Nadgodziny100, 0) - ISNULL(pp2oa.GodzNieob, 0) - ISNULL(d.PracaInnyArkusz, 0) - ISNULL(d.CzasNieprod, 0) - (ISNULL(s.Parametr, 0) * aoa.Alive/*(COUNT(distinct prz.IdPracownika) + SUM(ISNULL(d.Korekta, 0)))*/) &lt; 0 then 0 else ISNULL(ppoa.Nominal, 0) + ISNULL(ppoa.Nadgodziny50 + ppoa.Nadgodziny100, 0) - ISNULL(pp2oa.GodzNieob, 0) - ISNULL(d.PracaInnyArkusz, 0) - ISNULL(d.CzasNieprod, 0) - (ISNULL(s.Parametr, 0) * aoa.Alive/*(COUNT(distinct prz.IdPracownika) + SUM(ISNULL(d.Korekta, 0)))*/) end end, 0), 4) as [Czas produktywny (Bez przerwy do produktywności):N]
from Przypisania prz
left join Pracownicy p on prz.IdPracownika = p.Id
left join PlanPracy pp on pp.IdPracownika = prz.IdPracownika and @date = pp.Data
left join Absencja a on prz.IdPracownika &gt; 0 and a.IdPracownika = prz.IdPracownika and @date between a.DataOd and a.DataDo
left join AbsencjaKody ak on a.Kod = ak.Kod
left join Zmiany Z on Z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
left join scDni d on d.Data = @date and d.IdTypuArkuszy = @typark and d.IdPracownika = @typark * -1
outer apply (select ISNULL(case when Z.Typ in (1,2,3) then 0 else case when Z.Od &gt; Z.Do then 24 else 0 end + DATEDIFF(HOUR, Z.Od, Z.Do) end, 0) as Nominal, ISNULL(case when Z.Typ in (1,2,3) then 0 else cast(pp.CzasZm as float) end, 0) / 3600 as vCzasZm, CONVERT(float, ISNULL(pp.n50, 0))/3600 as Nadgodziny50, CONVERT(float, ISNULL(pp.n100, 0))/3600 as Nadgodziny100, CONVERT(float, ISNULL(pp.Nocne, 0))/3600 as Nocne) ppoa --do zmiany n i s
--outer apply (select ppoa.Nominal - (ISNULL(CONVERT(float, pp.CzasZm), 0)/3600) as GodzNieob) pp2oa
outer apply (select case when ppoa.Nominal &gt; ppoa.vCzasZm then ppoa.Nominal - ppoa.vCzasZm else 0 end as GodzNieob) pp2oa
outer apply (select ppoa.Nominal + ppoa.Nadgodziny50 + ppoa.Nadgodziny100 - pp2oa.GodzNieob as Godziny) goa
outer apply (select case when /*ak.Symbol is null*/ goa.Godziny &gt;= @kanapkaCzas then 1 else 0 end as Alive) as aoa
--outer apply (select case when ak.Symbol is null then 1 else 0 end as Alive) as aoa
left join scParametry s on s.IdTypuArkuszy = @typark and (@date between s.Od and ISNULL(s.Do, '20990909')) and s.Typ = 'SANDWICH' and s.TL = @tl
where (@date between prz.Od and ISNULL(prz.Do, '20990909') and prz.Status = 1) and ((prz.IdCommodity = @typark/* and prz.IdKierownika = @ObserverId*/) or (dbo.GetRight(@observerId, 57)/*@tl*/ = 1 and prz.IdPracownika = @ObserverId))
and
(
	(
		@col = 0
	)
	--or
	--(
	--	@col = 1
	--	and
	--	ppoa.Nominal != 0
	--)
	or
	(
		@col = 2
		and
		pp2oa.GodzNieob != 0
	)
	or
	(
		@col = 3
		and
		(
			ppoa.Nadgodziny50 != 0
			or
			ppoa.Nadgodziny100 != 0
			or
			ppoa.Nocne != 0
		)
	)
	or
	(
		@col = 4
	)
	or
	(
		@col = 5
	)
)
order by Pracownik
" >
    <SelectParameters>
            <asp:ControlParameter Name="typark" ControlID="hidScorecardTypeId" PropertyName="Value" Type="String" />
            <asp:ControlParameter Name="date" ControlID="hidDate" PropertyName="Value" Type="String" />
            <asp:ControlParameter Name="ObserverId" ControlID="hidObserverId" PropertyName="Value" Type="String" />
            <asp:ControlParameter Name="tl" ControlID="hidTeamLeader" PropertyName="Value" Type="String" />
            <asp:ControlParameter Name="col" ControlID="hidCol" PropertyName="Value" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
        
        
