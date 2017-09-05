<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntRapLimityNN.ascx.cs" Inherits="HRRcp.Controls.PodzialLudzi.cntRapLimityNN" %>
<%@ Register src="~/Controls/Reports/cntReport2.ascx" tagname="cntReport" tagprefix="uc1" %>
<%@ Register src="~/Controls/Reports/cntReportHeader.ascx" tagname="cntReportHeader" tagprefix="uc2" %>
<%@ Register src="~/Controls/Portal/cntSqlTabs.ascx" tagname="cntSqlTabs" tagprefix="uc3" %>
<%@ Register Src="~/Controls/PodzialLudzi/cntSelectRokMiesiac.ascx" TagPrefix="uc1" TagName="cntSelectRokMiesiac" %>


<div id="paRapLimityNN" runat="server" class="cntRapLimityNN">

<%--
    <uc2:cntReportHeader ID="cntReportHeader1" runat="server" Visible="false"
        Caption="Przekroczenia limitów nadgodzin na CC"    
    />
--%>    
   
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

    <div class="kryteria printoff">
        <table class="table0">
            <tr>
                <td id="tdKier" runat="server" class="left" visible="false">
                    <span>CC Przełożonego:</span>
                    <asp:DropDownList ID="ddlPM" runat="server" AutoPostBack="True" 
                        DataSourceID="SqlDataSource1" DataTextField="Pracownik" DataValueField="Id" 
                        onselectedindexchanged="ddlPM_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="right">
                    <uc1:cntSelectRokMiesiac runat="server" ID="cntSelectRokMiesiac" canBackAll="true" canNextAll="true"
                        OnBackAll="cntSelectRokMiesiac_BackAll"
                        OnNextAll="cntSelectRokMiesiac_NextAll"
                        OnValueChanged="cntSelectRokMiesiac_ValueChanged" />
                </td>
            </tr>
        </table>
    </div>

    <%--    
    <div id="paKierWr" class="kryteria" runat="server">
        <div id="paKier" runat="server" class="left" visible="false">
            <span>CC Przełożonego:</span>
            <asp:DropDownList ID="ddlPM" runat="server" AutoPostBack="True" 
                DataSourceID="SqlDataSource1" DataTextField="Pracownik" DataValueField="Id" 
                onselectedindexchanged="ddlPM_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div class="right">
            <uc1:cntSelectRokMiesiac runat="server" ID="cntSelectRokMiesiac" canBackAll="true" canNextAll="true"
                OnBackAll="cntSelectRokMiesiac_BackAll"
                OnNextAll="cntSelectRokMiesiac_NextAll"
                OnValueChanged="cntSelectRokMiesiac_ValueChanged" />
        </div>
    </div>
    --%>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
        SelectCommand="
select 'Wszystkie cc' as Pracownik, -99 as Id, 1 as Sort
union all            
select P.Nazwisko + ' ' + P.Imie as Pracownik, P.Id as Id, 2 as Sort 
from Pracownicy P 
where P.Kierownik = 1 and Status &gt;= 0 
and dbo.GetRightId(Rights,52) = 1
and P.Id in (select distinct UserId from ccPrawa)
order by Sort, Pracownik
    ">
    </asp:SqlDataSource>        

    <%--
select 'Wszystkie cc' as Pracownik, -99 as Id, 1 as Sort
union all            
select P.Nazwisko + ' ' + P.Imie as Pracownik, P.Id as Id, 2 as Sort 
from Pracownicy P 
where P.Kierownik = 1 and Status &gt;= 0 and 
--(dbo.GetRightId(Rights,38) = 1
(dbo.GetRightId(Rights,52) = 1
or P.Id in (select distinct UserId from ccPrawa)) 
order by Sort, Pracownik


    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="Przekroczenia limitów nadgodzin na CC"
        Title2="select 'Okres: ' + convert(varchar(10), convert(datetime,'@SQL4'), 20) + ' do ' + convert(varchar(10), DATEADD(D, -1, DATEADD(M, 1, '@SQL4')), 20)"
        SQL="

        Title2="select 'Okres: ' + convert(varchar(10), convert(datetime,'@SQL4'), 20) + ' do ' + convert(varchar(10), dbo.eom('@SQL4'), 20)"
    --%>
    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="Przekroczenia limitów nadgodzin na CC"
        Title2="select 'Okres: ' + convert(varchar(10), convert(datetime,'@SQL4'), 20) + ' do ' + convert(varchar(10), ISNULL((select DataImportu from OkresyRozl where DataOd = '@SQL4' and DataImportu &gt;= '@SQL4'), dbo.eom('@SQL4')), 20)"
        Title3="select case when @SQL1 = -99 then '' else (select 'Przełożony: ' + Nazwisko + ' ' + Imie from Pracownicy where Id = @SQL1) end"

        P1="
,ISNULL(Nadg50,0) [Nadg 50:NN2]
,ISNULL(Nadg100,0) [Nadg 100:NN2]
,ISNULL(Nadg,0) [Nadg 50+100:NN2]
        "
        P3="
,ISNULL(Nadg50,0) [Nadg 50:NN2|RepCCPracownicy @dod @ddo @cc * 50 * 1 1|Pracownicy z nadgodzinami 50]
,ISNULL(Nadg100,0) [Nadg 100:NN2|RepCCPracownicy @dod @ddo @cc * 100 * 1 1|Pracownicy z nadgodzinami 100]
,ISNULL(Nadg,0) [Nadg 50+100:NN2|RepCCPracownicy @dod @ddo @cc * 150 * 1 1|Pracownicy z nadgodzinami 50+100]
        "
        SQL="
------------------------------------
declare @dod datetime
declare @ddo datetime
set @dod = '@SQL4'
--set @ddo = dbo.eom(@dod)
set @ddo = ISNULL((select DataImportu from OkresyRozl where DataOd = @dod and DataImportu &gt;= @dod), dbo.eom(@dod))  --wg ustaleń Iwona

declare @margin int 
set @margin = 10  --[%]

declare @p3 bit, @p4 bit
set @p3 = 1
set @p4 = 1 

declare @kwoty bit, @kid int
set @kwoty = 0
set @kid = '@SQL1'

/*
select CC,
ROUND(SUM(FTEcc),2) as FTE,
--ROUND(SUM(Stawkacc),2) as Stawka
ROUND(SUM(Stawkacc * FTE),2) as Stawka
into #pp1
from VPodzialLudziRapPM
where Miesiac = @dod
    --and ('-99' = '-99' or TypImport = '@SQL3')
group by CC
*/

select CC,
ROUND(SUM(vNadg50),2) as Nadg50,
ROUND(SUM(vNadg100),2) as Nadg100,
ROUND(SUM(vNadg50 + vNadg100),2) as Nadg,
ISNULL(round(sum(vNadg50 * StawkaGodz * 1.5),2),0) as NadgKoszt50,
ISNULL(round(sum(vNadg100 * StawkaGodz * 2),2),0) as NadgKoszt100,
ISNULL(round(sum(vNadg50 * StawkaGodz * 1.5),2),0) +
ISNULL(round(sum(vNadg100 * StawkaGodz * 2),2),0) as NadgKoszt
into #pp2
from VrepDaneMPKv2
where Data between @dod and @ddo
	and (
		(@p3=1 and cc not in (select cc from CC where Grupa = 1))  -- bez 019, podzielone   
	 or (@p3=0 and Typ &lt; 10)    -- z 019, niepodzielone 
	)
	and (@p4=1 or Typ not in (3,13))  -- bez dopelnien
	--and ('@SQL3' = '-99' or TypImport = '@SQL3')
group by CC

select
CC.cc [cc],
CC.Id [ccId:-],
convert(varchar(10),@dod,20) [dod:-],
convert(varchar(10),@ddo,20) [ddo:-],
CC.Nazwa + case when @ddo between CC.AktywneOd and ISNULL(CC.AktywneDo, '20990909') then '' else ' (nieaktywne)' end [Nazwa:C]

--,ISNULL(FTE,0) [FTE:NF2S]
--,ISNULL(Nadg50,0) [Nadg 50:NF2S;c2]
--,ISNULL(Nadg100,0) [Nadg 100:NF2S;c3]
--,ISNULL(Nadg,0) [Nadg 50+100:NF2S;c2]
--,ISNULL(Stawka,0) [Koszt brutto:NN2S]
--,ISNULL(NadgKoszt50,0) [Koszt 50:NN2S;c2]
--,ISNULL(NadgKoszt100,0) [Koszt 100:NN2S;c3]
--,ISNULL(NadgKoszt,0) [Koszt 50+100:NN2S;c2]


@SQL2
/*
,ISNULL(Nadg50,0) [Nadg 50:NN2]
,ISNULL(Nadg100,0) [Nadg 100:NN2]
,ISNULL(Nadg,0) [Nadg 50+100:NN2]
*/

,ISNULL(L.Limit, 0) [Limit:NN2]
,case when round(ISNULL(Nadg,0) - ISNULL(L.Limit,0),2) &lt;= 0 then -round(ISNULL(Nadg,0) - ISNULL(L.Limit,0),2) else 0 * ISNULL(L.Limit,0) end [Pozostało:NN2;green]
,case when round(ISNULL(Nadg,0) - ISNULL(L.Limit,0),2) &gt; 0 then round(ISNULL(Nadg,0) - ISNULL(L.Limit,0),2) else 0 * ISNULL(L.Limit,0) end [Przekroczenie:NN2;red]
,case when ISNULL(Nadg,0) &gt; ISNULL(L.Limit,0) then 'Przekroczenie !!!' 
when ISNULL(Nadg,0) &gt; ISNULL(L.Limit,0) * (100 - @margin) / 100 then 'Możliwe przekroczenie (margines ' + convert(varchar, @margin) + '%)'
else '' end as [Informacja:;width200]
/*
,ISNULL(convert(varchar,L.Limit), 'brak limitu') [Limit:NN2]
,case when round(ISNULL(Nadg,0) - L.Limit,2) &lt;= 0 then -round(ISNULL(Nadg,0) - L.Limit,2) else 0 * L.Limit end [Pozostało:NN2;green]
,case when round(ISNULL(Nadg,0) - L.Limit,2) &gt; 0 then round(ISNULL(Nadg,0) - L.Limit,2) else 0 * L.Limit end [Przekroczenie:NN2;red]
,case when ISNULL(Nadg,0) &gt; L.Limit then 'Przekroczenie !!!' 
when ISNULL(Nadg,0) &gt; L.Limit * (100 - @margin) / 100 then 'Możliwe przekroczenie (margines ' + convert(varchar, @margin) + '%)'
else '' end as [Informacja:;width200]
*/

from CC 
--left join #pp1 A on A.CC = CC.cc
left join #pp2 B on B.CC = CC.cc
left join ccPrawa R on R.UserId = @kid and R.IdCC = CC.Id
left join ccLimity L on L.Miesiac = @dod and L.ccId = CC.Id and L.isLast = 1
where (AktywneOd &lt;= @ddo and @dod &lt;= ISNULL(AktywneDo, '20990909') 
    and CC.GrSplitu is null
	--or A.FTE != 0
	--or A.Stawka != 0
	or B.Nadg != 0)
and (@kid = -99 or R.Id is not null)

order by CC.cc
        "/>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>


<%--
declare @od datetime
declare @do datetime
declare @userId int
select @od = '@SQL4', @do = DATEADD(D, -1, DATEADD(M, 1, @od)), @userId = @SQL1

select A.cc, Nazwa [Nazwa:C]
    ,round(Nadg50,  2) [Nadg 50:N]
    ,round(Nadg100, 2) [Nadg 100:N]
    ,round(Nadg,    2) [Nadg 50+100:N]
    ,ISNULL(convert(varchar, Limit), 'brak') [Limit:N]
,case
	when Limit is null or Limit &gt; Nadg then 'brak'
	else CONVERT(varchar, ROUND(Nadg - Limit, 2))
	end [Przekroczenie:N]
,case 
	when Limit is NULL then 'Brak Limitu'
	when ISNULL(Nadg, 0) &gt; ISNULL(Limit, 0)       then 'Przekroczenie o ' + convert(varchar, pr-100) + '% !!!'
	when ISNULL(Nadg, 0) &gt; ISNULL(Limit, 0) * 0.9 then 'Wykorzystano ' + convert(varchar, pr) + '% (Możliwe przekroczenie)'
	else                                                  'Wykorzystano ' + convert(varchar, pr) + '%' 
	end as Informacja
from
(
    select CC.cc, CC.Id as ccId
	,CC.Nazwa + case when @do between CC.AktywneOd and ISNULL(CC.AktywneDo, '20990909') then '' else ' (zamknięte)' end Nazwa
	,ISNULL(vNadg50, 0) Nadg50, ISNULL(vNadg100, 0) Nadg100, ISNULL(vNadg50, 0) + ISNULL(vNadg100, 0) Nadg, Limit
	,round(case when Limit is null then -1 when Limit = 0 then 0 else ((ISNULL(vNadg50, 0) + ISNULL(vNadg100, 0)) / Limit) end * 100, 2) as pr
	from CC
	left join (
	    select CC, Od, Do, SUM(vNadg50) as vNadg50, SUM(vNadg100) as vNadg100 
	    from tmpVrepDaneMPK2
		where (vNadg50 != 0 OR vNadg100 != 0) AND (@od between Od and Do)
		group by CC, Od, Do) NN 
		on NN.CC = CC.cc
	left join ccLimity LL on LL.isLast = 1 and LL.ccId = CC.Id and LL.miesiac = @od
	left join ccUprawnienia UU on UU.IdCC = CC.id and UU.userId = @userId
	where CC.Grupa != 1 AND (@userId = -99 OR UU.id IS NOT NULL)
) A
order by pr desc, A.cc
--%>