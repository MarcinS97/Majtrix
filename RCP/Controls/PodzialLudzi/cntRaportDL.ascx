<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntRaportDL.ascx.cs" Inherits="HRRcp.Controls.PodzialLudzi.cntRaportDL" %>
<%@ Register src="~/Controls/Reports/cntReport2.ascx" tagname="cntReport" tagprefix="uc1" %>
<%@ Register src="~/Controls/Reports/cntReportHeader.ascx" tagname="cntReportHeader" tagprefix="uc2" %>
<%@ Register src="~/Controls/Portal/cntSqlTabs.ascx" tagname="cntSqlTabs" tagprefix="uc3" %>
<%@ Register Src="~/Controls/PodzialLudzi/cntSelectRokMiesiac.ascx" TagPrefix="uc1" TagName="cntSelectRokMiesiac" %>


<div id="paRaportDL" runat="server" class="cntRapLimityNN cntRaportDL">

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
where P.Kierownik = 1 and Status &gt;= 0 and 
(dbo.GetRightId(Rights,40) = 1   --PM
or P.Id in (select distinct UserId from ccPrawa)) 
order by Sort, Pracownik
    ">
    </asp:SqlDataSource>        

    <%--
    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="Przekroczenia limitów nadgodzin na CC"
        Title2="select 'Okres: ' + convert(varchar(10), convert(datetime,'@SQL4'), 20) + ' do ' + convert(varchar(10), DATEADD(D, -1, DATEADD(M, 1, '@SQL4')), 20)"
        SQL="
    --%>
    <uc1:cntReport ID="cntReport1" runat="server" 
        Title="Ilość DL na CC"
        Title2="select 'Okres: ' + convert(varchar(10), convert(datetime,'@SQL4'), 20) + ' do ' + convert(varchar(10), dbo.eom('@SQL4'), 20)"
        Title3="select case when @SQL1 = -99 then '' else (select 'Przełożony: ' + Nazwisko + ' ' + Imie from Pracownicy where Id = @SQL1) end"

        SQL="
------------------------------------
declare @od datetime
declare @do datetime
set @od = '@SQL4'
set @do = dbo.eom(@od)

declare @p3 bit, @p4 bit
set @p3 = 1
set @p4 = 1 

declare @kid int
set @kid = '@SQL1'

select D.Data [Data:D]
,ISNULL(CC1.cc, CC.cc) [CC]
--,P.Nazwisko, P.Imie, P.KadryId, P.Status, R.Od, R.Do, PS.Klasyfikacja, PP.CzasZm, PP.Nadgodzinydzien, PP.NAdgodzinyNoc, PP.Akceptacja, PP.Data, BA.Typ, BA.DataOd, Ba.DataDo, WU.Od, WU.Do, CC.cc, W.Wsp,CC1.cc, W1.Wsp

,count(*) [Ilosć DL:NN2S]

/*
,sum(case when ISNULL(PP.CzasZm,0) + ISNULL(PP.NadgodzinyDzien,0) + ISNULL(PP.NadgodzinyNoc,0) &gt; 0 then 1 else 0 end) [Ilość pracujących DL]
,sum(case when BA.Typ = 'U' then 1 else 0 end) [Ilość urlopów]
,sum(case when BA.Typ = 'Z' then 1 else 0 end) [Ilość chorobowych]
*/
,sum(case when ISNULL(PP.CzasZm,0) + ISNULL(PP.NadgodzinyDzien,0) + ISNULL(PP.NadgodzinyNoc,0) &gt; 0 then ISNULL(W1.Wsp*W.Wsp,ISNULL(W.Wsp,1)) else 0 end) [Ilość pracujących DL:NN2S]
,sum(case when BA.Typ = 'U' then ISNULL(W1.Wsp*W.Wsp,ISNULL(W.Wsp,1)) else 0 end) [Ilość urlopów:NN2S]
,sum(case when BA.Typ = 'Z' then ISNULL(W1.Wsp*W.Wsp,ISNULL(W.Wsp,1)) else 0 end) [Ilość chorobowych:NN2S]

from dbo.GetDates2(@od, @do) D
left join Przypisania R on D.Data between R.Od and ISNULL(R.Do,'20990909') and R.Status = 1
left join Pracownicy P on P.Id = R.IdPracownika
left join PracownicyStanowiska PS on PS.IdPracownika = R.IdPracownika and D.Data between PS.Od and ISNULL(PS.Do, '20990909')
left join PlanPracy PP on PP.IdPracownika = R.IdPracownika and PP.Data = D.Data and PP.Akceptacja = 1
left join bufAbsencja BA on BA.LpLogo = P.KadryId and D.Data between BA.DataOd and BA.DataDo
left join poWnioskiUrlopowe WU on WU.IdPracownika = R.IdPracownika and D.Data between WU.Od and WU.Do and WU.StatusId = 3 

left join Splity S on S.GrSplitu = P.GrSplitu and D.Data between S.DataOd and ISNULL(S.DataDo, '20990909')
left join SplityWsp W on W.IdSplitu = S.Id
left join CC on CC.Id = W.IdCC

left join Splity S1 on S1.GrSplitu = CC.GrSplitu and CC.Grupa = 1 and D.Data between S1.DataOd and ISNULL(S1.DataDo, '20990909')
left join SplityWsp W1 on W1.IdSplitu = S1.Id
left join CC CC1 on CC1.Id = W1.IdCC

--left join ccPrawa PR on PR.UserId = @kid and (PR.IdCC = ISNULL(CC1.Id, CC.Id) or PR.IdCC = CC.Id)
left join ccPrawa PR on PR.UserId = @kid and (PR.IdCC = ISNULL(CC1.Id, CC.Id))
where (@kid = -99 or PR.Id is not null)
and PS.Klasyfikacja = 'DL'

--and ISNULL(CC1.cc, CC.cc) = '001'
--and D.Data = '20150701'

group by D.Data, ISNULL(CC1.cc, CC.cc)
order by 1,2
        "/>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

