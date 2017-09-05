<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntHeader.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Spreadsheets.cntHeader" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<div id="ctHeader" runat="server" class="cntHeader">
    <asp:HiddenField ID="hidScorecardTypeId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidEmployeeId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidDate" runat="server" Visible="false" />
    <asp:HiddenField ID="hidObserverId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidTeamLeader" runat="server" Visible="false" />
    <input type="hidden" id="hidData" runat="server" class="hidData" />
    <input type="hidden" id="hidData2" runat="server" class="hidData2" />
    <input type="hidden" id="hidData3" runat="server" class="hidData3" />
    <input type="hidden" id="hidAbs" runat="server" class="hidAbs" />
    <input type="hidden" id="hidPrInnyArkusz" runat="server" class="hidPrInnyArkusz" />
    <div id="info" runat="server" class="info">
        <table id="tbInfo"  class="tbInfo">
            <tr>
                <td>
                    <span>Nr ewid</span>
                </td>
                <td>
                    <asp:Label ID="lblNrEwid" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <span>Imię i Nazwisko</span>
                </td>
                <td>
                    <asp:Label ID="lblName" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <span>Team Leader</span>
                </td>
                <td>
                    <asp:Label ID="lblTeamLeader" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <span>Data zatrudnienia</span>
                </td>
                <td>
                    <asp:Label ID="lblHireDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <span>Okres próbny (P)</span>
                </td>
                <td>
                    <asp:Label ID="lblProbation" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <span>Obszar</span>
                </td>
                <td>
                    <asp:Label ID="lblArea" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <span>QC</span>
                </td>
                <td>
                    <asp:Label ID="lblQC" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div id="infoGroup" runat="server" class="info">
        <table id="tbInfoGroup" class="tbInfo">
            <tr>
                <td>
                    <span>Team Leader</span>
                </td>
                <td>
                    <asp:Label ID="lblTeamLeaderGroup" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <span>QC</span>
                </td>
                <td>
                    <asp:Label ID="lblQCGroup" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <span>Wielkość zespołu</span>
                </td>
                <td class="teamSize">
                    <asp:Label ID="lblTeamSize" runat="server" />
                    <asp:TextBox ID="tbTeamSize" runat="server" MaxLength="3"  />
                    <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" TargetControlID="tbTeamSize" FilterType="Custom" ValidChars="0123456789" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" SetFocusOnError="true" Display="Dynamic" CssClass="error" ControlToValidate="tbTeamSize" ErrorMessage="Pole nie może być puste" ValidationGroup="ivg"></asp:RequiredFieldValidator>
                    <%--<asp:Label ID="lblTeamSize" runat="server" />--%>
                </td>
            </tr>
        </table>
    </div>
    <div id="sums" class="sums">
        <table id="tbSums" class="tbSums">
            <tr>
                <th>
                </th>
                <th>
                    <span>Podstawa</span>
                </th>
                <th>
                    <span>Proporcjonalnie</span>
                </th>
            </tr>
            <tr>
                <td>
                    <span>Średniomiesięczna produktywność</span>
                </td>
                <td class="num">
                    <span class="lblMidProdPodst"></span>
                </td>
                <td>
                    <%--<asp:Label ID="lblMidProdProp" runat="server" /> --%>
                </td>
            </tr>
            <tr>
                <td>
                    <span>Premia za produktywność</span>
                </td>
                <td class="num" >
                    <span class="lblPremiaProdPodst"></span>
                </td>
                <td class="num tdPremiaProd" runat="server" id="tdPremiaProd">
                    <span class="lblPremiaProdProp"></span>
                </td>
            </tr>
            <tr>
                <td>
                    <span>Korekta o współczynnik jakości</span>
                </td>
                <td class="num">
                    <span class="lblKorektaWsplJakPodst"></span>
                </td>
                <td class="num">
                    <span class="lblKorektaWsplJakProp"></span>
                </td>
            </tr>
            <tr id="trDisposition" runat="server" class="absKorekta">
                <td>
                    <span>Korekta premii za dyspozycyjność</span>
                </td>
                <td class="num">
                    <span class="lblKorektaDyspPodst"></span>
                </td>
                <td class="num">
                    <span class="lblKorektaDyspProp"></span>
                </td>
            </tr>
            <tr>
                <td>
                    <span>Premia za pracę na rzecz innego działu</span>
                </td>
                <td class="num">
                    <span class="lblPremiaInnyDzialPodst"></span>
                </td>
                <td class="num">
                    <span class="lblPremiaInnyDzialProp"></span>
                </td>
            </tr>
            <tr>
                <td>
                    <span>Razem premia</span>
                </td>
                <td>
                </td>
                <td colspan="2" class="num">
                    <span class="lblRazemPremia"></span>
                </td>
            </tr>
        </table>
    </div>
    
    
     <div id="absSums" class="absSums">
        <table id="tbAbsSums" class="tbAbsSums">
            <asp:Repeater ID="rpSums" runat="server">
                <HeaderTemplate>
                    <tr>
                        <th colspan="2">
                            Suma absencji
                        </th>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Symbol") %>'  />                            
                        </td>
                        <td>
                            <asp:Label ID="lblIlosc" runat="server" Text='<%# Eval("Ilosc") %>'   />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    
        
        <table class="tbRight">
            <tr>
                <td>
                    <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/RepLogo155x45_Sivantos.png" CssClass="imgLogo" />
                </td>
            </tr>
            <tr>
                <td>
                    <div id="scAccepted" runat="server" class="scAccepted" >
                        ARKUSZ ZAAKCEPTOWANY
                    </div>
                    <div id="scAccpeptedAdmin" runat="server">
                        <asp:Button ID="btnUnacceptConfirm" runat="server" Text="Cofnij akceptację" CssClass="button100" OnClick="UnacceptConfirm" />
                    </div>
                </td>
            </tr>
        </table>
        <asp:Button ID="btnUnnacept" runat="server" Text="" OnClick="Unaccept" style="display: none;" />
        <asp:Button ID="btnUnaccept2" runat="server" Text="" OnClick="Unaccept2" style="display: none;" />
    
</div>


<asp:SqlDataSource ID="dsSave" runat="server" SelectCommand="
--ULTYMATYWNA METODA ROZWIAZUJACA WSZYSTKO ZWIAZANE Z WNIOSKAMI PREMIOWYMI
--UPDATE 1 DONE
--UPDATE 2 DONE
declare @wniosekId int
set @wniosekId = (select Id from scWnioski w where w.IdTypuArkuszy = {0} and w.Data = {1} and w.IdPracownika = {2})

    declare @bilans int
    declare @oldwniosekId int
    declare @hajs int

    declare @hco int
    select @hco = HeadcountOverride from scWnioski where Id = @wniosekId

    select @bilans = ISNULL(BilansOtwarcia, 0), @oldwniosekId = Id from scWnioski where Data = DATEADD(m, -1, {1}) and IdPracownika = {2} and IdTypuArkuszy = {0}
    select @hajs = (select top 1 Parametr from scParametry p where {0} = p.IdTypuArkuszy and p.Typ = 'PREM' and DATEADD(m, -1, {1}) between p.Od and ISNULL(p.Do, '20990909') and p.TL = 0)
    select @hajs = @hajs * ISNULL(@hco, (select COUNT(*) from scPremie where _do is null and IdWniosku = @oldwniosekId and IdPracownika != {2} and ISNULL(Czas, ISNULL(CzasPracy, 0)) &gt; 0 and IdPracownika in
        (select IdPracownika from Przypisania where IdKierownika = {2} and dbo.eom(DATEADD(m, -1, {1})) between Od and ISNULL(Do, '20990909'))))

    select @bilans = @bilans + @hajs - (select SUM(ISNULL(PremiaUznaniowa, 0)) from scPremie where IdWniosku = @oldwniosekId and _do is null)
    
if @wniosekId is null begin --pierwsze uruchomienie systemu, powinno odpalic sie tylko raz ew. przy jakims purge'u danych --nie wazne, teraz to jest funkcjonalnosc

	insert into scWnioski (IdTypuArkuszy, IdPracownika, Data, DataWyplaty, BilansOtwarcia, Status, Kacc, Pacc, IloscPracownikow, DataUtworzenia) values ({0}, {2}, {1}, DATEADD(m, 1, {1}), @bilans, 0, -1, -1, {3}, GETDATE())
	set @wniosekId = SCOPE_IDENTITY()
    --if @wniosekId is null set @wniosekId = @@IDENTITY
	end
else 
begin
    update scWnioski set IloscPracownikow = {3}, BilansOtwarcia = @bilans where Id = @wniosekId
end

" />


<asp:SqlDataSource ID="dsHeader" runat="server" SelectCommand="
--declare @arkusz as int =
declare @typark as int = {0}--(select IdTypuArkuszy from scArkusze where Id = @arkusz)
declare @pracId as int = {1}--(select IdPracownika from scArkusze where Id = @arkusz)
declare @date as datetime = '{2}'--GETDATE()--(select Miesiac from scArkusze where Id = @arkusz)
declare @observerId int = {3}
declare @qc as int, @prod as int
declare @od as datetime = CONVERT(datetime, Convert(varchar, YEAR(@date)) + '-' + convert(varchar, MONTH(@date)) + '-01' )
declare @do as datetime = DATEADD(D, -1, DATEADD(M,1,@od))
--declare @tl bit = {4}
declare @tl bit = case when @pracId &lt; 0 then {4} else case when {5} = 1 then 1 else {4} end end
select @qc = QC, @prod = Produktywnosc from scTypyArkuszy where Id = @typark

select @prod = case when case when @pracId &lt; 0 then 0 else @tl end = 1 then 9 else 11 end

select KadryId, Nazwisko + ' ' + Imie as Name, LEFT(CONVERT(varchar, DataZatr, 20), 10) as DataZatr from Pracownicy where Id = @pracId

--select * from dbo.fn_GetSubPrzypisania(

select top 1 Imie + ' ' + Nazwisko as TeamLeader from Przypisania p left join Pracownicy pr on p.IdKierownika = pr.Id where ((p.IdPracownika = @pracId and IdCommodity = @typark) or (@pracId &lt; 0 and IdCommodity = @typark)) and (p.Od &lt; dbo.eom(@date)) order by Od desc

select Nazwa as QC from scSlowniki s where s.Id = @qc and s.Typ = 'QC' and s.Aktywny = 1

--te dwa selecty sa wazne
declare @t1 as nvarchar(512), @t2 as nvarchar(512), @t3 as nvarchar(512)
select @t2 = COALESCE(@t2 + ';', '') + CONVERT(varchar, DlaIlu) + '|' + CONVERT(varchar, Ile) from scQC where IdTypuArkuszy = @typark and Rodzaj = @qc and (@date between Od and ISNULL(Do, '20990909')) and TL = case when @pracId &lt; 0 then 0 else @tl end order by DlaIlu
select @t1 = COALESCE(@t1 + ';', '') + CONVERT(varchar, DlaIlu) + '|' + CONVERT(varchar, Ile) from scProduktywnosc where IdTypuArkuszy = @typark and Rodzaj = @prod and (@date between Od and ISNULL(Do, '20990909')) and OkresProbny = 0 and TL = case when @pracId &lt; 0 then 0 else @tl end order by DlaIlu --TU CO INNEGO POTEM
select @t3 = COALESCE(@t3 + ';', '') + CONVERT(varchar, DlaIlu) + '|' + CONVERT(varchar, Ile) from scAbsencje where IdTypuArkuszy = @typark and (@date between Od and ISNULL(Do, '20990909')) and TL = case when @pracId &lt; 0 then 0 else @tl end order by DlaIlu
select @t1 as Table1, @t2 as Table2, @t3 as Table3, isnull((select Parametr from scParametry where IdTypuArkuszy = @typark and (@date between Od and ISNULL(Do, '20990909')) and Typ = 'PGIO' and TL = case when @pracId &lt; 0 then 0 else @tl end), 0) as PrInnyArkusz

select ak.Symbol, SUM(
	DATEDIFF(DAY,
		case when @od &lt; a.DataOd then a.DataOd else @od end,
		case when @do &gt; a.DataDo then a.DataDo else @do end
	) + 1
) as Ilosc into #abs from Absencja a
left join AbsencjaKody ak on ak.Kod = a.Kod
outer apply (select k.Parametr from Kody k where k.Aktywny = 1 and k.Typ = 'SCABSENCJE') oa
where a.IdPracownika = @pracId and a.DataOd &lt;= @do and a.DataDo &gt;= @od and ak.Kod in (select items from dbo.SplitInt(oa.Parametr, ','))
group by ak.Symbol


--select ISNULL(SUM(GodzNieob), 0) as GodzNieob from scDni d where d.IdTypuArkuszy = @typark and d.IdPracownika = @pracId and KodNieob in (select 0)
--select COUNT(*) as GodzNieob from Absencja a where a.IdPracownika = @pracId and a.DataOd &lt;= @do and a.DataDo &gt;= @od
select SUM(Ilosc) as GodzNieob from #abs

select COUNT(*) as HeadCount from Przypisania p where p.IdCommodity = @typark and p.Od &lt;= @do and @od &lt;= ISNULL(p.Do, '20990909') and p.Status = 1 and IdKierownika = @observerId

select * from #abs

declare @wniosekId int
declare @ziomkip int
set @wniosekId = (select Id from scWnioski w where w.IdTypuArkuszy = @typark and w.Data = @date and w.IdPracownika = @observerId) 
select @ziomkip = w.IloscPracownikow
from scWnioski w
where w.Id = @wniosekId

select ISNULL(@ziomkip, 
	(
		select COUNT(*) as ziomkip from
	(
		select * from Przypisania prz where prz.Status = 1 and ((prz.IdPracownika = @pracId and IdCommodity = @typark) or (@pracId &lt; 0 and IdCommodity = @typark and prz.IdKierownika = @ObserverId))
		union all
		select * from Przypisania prz2 where prz2.Status = 1 and @pracId &lt; 0 and prz2.IdPracownika = @ObserverId and dbo.GetRight(@observerId, 57)/*@tl*/ = 1
	) prz3 where (@do between prz3.Od and ISNULL(prz3.Do, '20990909') /*and prz3.Status = 1*/) --and w.IdTypuArkuszy = prz.IdCommodity
	)
) as IloscPracownikow

select case when @observerId = IdKierownika then 1 else 0 end as Cosmos from Przypisania where dbo.eom(@date) between Od and isnull(Do, '20990909') and IdPracownika = @pracId

select SUM(Nominal) as Nominal from dbo.GetDates2(@od, @do) dz
left join PlanPracy pp on pp.IdPracownika = @pracId and dz.Data = pp.Data
left join Zmiany Z on Z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
outer apply (select ISNULL(case when Z.Typ in (1,2,3) then 0 else case when Z.Od &gt; Z.Do then 24 else 0 end + DATEDIFF(HOUR, Z.Od, Z.Do) end, 0) as Nominal, ISNULL(case when Z.Typ in (1,2,3) then 0 else cast(pp.CzasZm as float) end, 0) / 3600 as vCzasZm, CONVERT(float, ISNULL(pp.n50, 0) + ISNULL(pp.n100, 0))/3600 as Nadgodziny) ppoa

drop table #abs
" />


<asp:SqlDataSource ID="dsUnAccept" runat="server" SelectCommand="
declare @wniosekId int
set @wniosekId = (select Id from scWnioski w where w.IdTypuArkuszy = {0} and w.Data = {1} and w.IdPracownika = {2})
delete from scPremie where IdWniosku = @wniosekId and IdPracownika = {3}
" 
/>

<asp:SqlDataSource ID="dsUnAcceptTeam" runat="server" SelectCommand="
declare @wniosekId int
set @wniosekId = (select Id from scWnioski w where w.IdTypuArkuszy = {0} and w.Data = {1} and w.IdPracownika = {2})
delete from scPremie where IdWniosku = @wniosekId
" 
/>

<asp:SqlDataSource ID="dsTeamLeader" runat="server" SelectCommand="
declare @date datetime = {0}
declare @pracId int = {1}
declare @typark int = {2}

declare @WPI float

declare @sanity bit = case when (select COUNT(*) from Przypisania where IdCommodity = @typark and IdKierownika = @pracId and @date between Od and ISNULL(Do, '20990909')) = 0 then 0 else 1 end

select @WPI = Parametr from /*Przypisania p
left join*/ scParametry par /*on p.IdCommodity = par.IdTypuArkuszy and @date between par.Od and ISNULL(par.Do, '20990909')*/
where /*p.IdKierownika = @pracId and @date between p.Od and ISNULL(p.Do, '20990909') and p.Status = 1 and*/ par.Typ = 'WPI' and par.TL = 1 and par.IdTypuArkuszy = @typark and par.Parametr2 = @pracId and par.Parametr2 = @pracId and @date between par.Od and ISNULL(par.Do, '20990909')

select case when ISNULL(@WPI, 0) &gt; 0 and @sanity = 1 then 1 else 0 end
" />