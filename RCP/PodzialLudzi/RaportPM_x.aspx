<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RaportPM_x.aspx.cs" Inherits="HRRcp.RaportPM_x" %>
<%@ Register src="~/Controls/Reports/cntReport2.ascx" tagname="cntReport" tagprefix="uc1" %>
<%@ Register src="~/Controls/Reports/cntReportHeader.ascx" tagname="cntReportHeader" tagprefix="uc2" %>
<%@ Register src="~/Controls/Portal/cntSqlTabs.ascx" tagname="cntSqlTabs" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<%--
raport - 
p1 - od
p2 - do
p3 - 1 019 podzielone, 0 niepodzielone
p4 - 1 dope³nienia, 0 bez dope³nieñ
--%>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">    
    <uc2:cntReportHeader ID="cntReportHeader1" runat="server" Visible="false"
        Caption="Podzia³ czasu pracy na CC"    
    />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

    <div class="RaportPM_kryteria">
        <div id="paKier" runat="server" visible="false">
            <span>CC Prze³o¿onego:</span>
            <asp:DropDownList ID="ddlPM" runat="server" AutoPostBack="True" 
                DataSourceID="SqlDataSource1" DataTextField="Pracownik" DataValueField="Id" 
                onselectedindexchanged="ddlPM_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div>
            <span>Na dzieñ:</span>
            <asp:DropDownList ID="ddlNaDzien" runat="server" AutoPostBack="True" 
                DataSourceID="SqlDataSource3" DataTextField="NaDzien" DataValueField="Data" 
                OnSelectedIndexChanged="ddlNaDzien_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
<%--
        <div>
            <span>Week:</span>
            <asp:DropDownList ID="ddlWeek" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2" DataTextField="Week" DataValueField="Id">
            </asp:DropDownList>
        </div>
--%>        
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
            SelectCommand="
select 'Wszystkie cc' as Pracownik, -99 as Id, 1 as Sort
union all            
select P.Nazwisko + ' ' + P.Imie as Pracownik, P.Id as Id, 2 as Sort 
from Pracownicy P 
where P.Kierownik = 1 and Status &gt;= 0 and dbo.GetRightId(Rights,38) = 1 
order by Sort, Pracownik
        ">
        </asp:SqlDataSource>        

        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
            SelectCommand="
select top 12 CONVERT(varchar(10), DataImportu, 20) as NaDzien, 
convert(varchar, Id) + '|' + CONVERT(varchar(10), dbo.bom(DataOd), 20) + '|' + CONVERT(varchar(10), DataImportu, 20) as Data 
from OkresyRozl 
where DataImportu is not null 
order by DataOd desc
        ">
        </asp:SqlDataSource>        




<%--
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
            SelectCommand="
declare @boy datetime, @eoy datetime
set @boy = dbo.boy(GETDATE())  
set @eoy = dbo.eoy(@boy)
  
select 
CONVERT(varchar, Lp + 1) + ' (' +
CONVERT(varchar(10), dbo.MaxDate2(@boy, B.bow), 20) + '...' + 
RIGHT(CONVERT(varchar(10), dbo.MinDate2(@eoy, B.bow + 6), 20), 2) + ')' as Week,
CONVERT(varchar, Lp + 1) + '|' +
CONVERT(varchar(10), dbo.MaxDate2(@boy, B.bow), 20) + '|' + 
CONVERT(varchar(10), dbo.MinDate2(@eoy, B.bow + 6), 20) + '|' + 
case when GETDATE() between B.bow and (B.bow + 6) then 'today' else '' end as Id,
Lp + 1 as WeekNo,
dbo.MaxDate2(@boy, B.bow) bow,
dbo.MinDate2(@eoy, B.bow + 6) as eow
from dbo.GetDates2('19000101','19000224') A
outer apply (select dateadd(dd,(A.Lp) * 7, dateadd(dd, -dbo.dow(@boy), @boy)) bow) B
where B.bow &lt; dbo.eoy(@boy)
            ">
        </asp:SqlDataSource>        





"select 
case when SUBSTRING(Rights,7,1) = '1' then '' else 'Brak prawa dostêpu do raportu; ' end + 
case when SUBSTRING(Rights,8,1) = '1' then 'Uprawnienia: Dostêp do kosztów; ' else 'Uprawnienia: ' end + 
case when SUBSTRING(Rights,9,1) = '1' then 'Dostêp do wszystkich pracowników' else 'Dostêp do pracowników wg klasyfikacji:' end  
from Pracownicy where Id=@UserId


--%>
    </div>

    <uc3:cntSqlTabs ID="cntSqlTabs1" runat="server" OnSelectTab="cntSqlSelectTabs1_SelectTab" />
    
    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="Podzia³ Ludzi"
        Title2="select 'Okres: ' + convert(varchar(10), convert(datetime,'@SQL4'), 20) + ' do ' + convert(varchar(10), convert(datetime,'@SQL5'), 20)"
  
        P1="
ISNULL(FTE,0) [FTE:NS],
ISNULL(Nadg,0) [Iloœæ nadgodzin:NS],
        "
        P2="
ISNULL(FTE,0) [FTE:NS],
ISNULL(Stawka,0) [Koszt brutto:NS],
ISNULL(Nadg,0) [Iloœæ nadgodzin:NS],
ISNULL(NadgKoszt,0) [Koszt nadgodzin:NS]
        "
        SQL="
declare @dod datetime
declare @ddo datetime
set @dod = '@SQL4'
set @ddo = '@SQL5'

declare @p3 bit, @p4 bit
set @p3 = 1
set @p4 = 1 

declare @kwoty bit, @kid int
set @kwoty = 1
set @kid = @SQL1

select CC,
ROUND(SUM(FTEcc),2) as FTE,
ROUND(SUM(Stawkacc),2) as Stawka
into #pp1
from VPodzialLudziRapPM
where Miesiac = @dod
    and ('@SQL3' = '-99' or TypImport = '@SQL3')
group by CC

select CC,
ROUND(SUM(vNadg50 + vNadg100),2) as Nadg,
ISNULL(round(sum(vNadg50 * StawkaGodz * 1.5),2),0) +
ISNULL(round(sum(vNadg100 * StawkaGodz * 2),2),0) as NadgKoszt
into #pp2
from VrepDaneMPKv2
where Data between @dod and @ddo
	and (
		(@p3=1 and cc not in (select cc from CC where GrSplitu is not null))  -- bez 019, podzielone
	 or (@p3=0 and Typ &lt; 10)    -- z 019, niepodzielone 
	)
	and (@p4=1 or Typ not in (3,13))  -- bez dopelnien
	and ('@SQL3' = '-99' or TypImport = '@SQL3')
group by CC


select
CC.cc [cc],
CC.Nazwa + case when @ddo between CC.AktywneOd and ISNULL(CC.AktywneDo, '20990909') then '' else ' (zamkniête)' end [Nazwa:C],

@SQL2

from CC 
left join #pp1 A on A.CC = CC.cc
left join #pp2 B on B.CC = CC.cc
left join ccPrawa R on R.UserId = @kid and R.IdCC = CC.Id
where (AktywneOd &lt;= @ddo and @dod &lt;= ISNULL(AktywneDo, '20990909') 
    and CC.GrSplitu is null
	or A.FTE != 0
	or A.Stawka != 0
	or B.Nadg != 0)
and (@kid = -99 or R.Id is not null)

order by cc
"/>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
