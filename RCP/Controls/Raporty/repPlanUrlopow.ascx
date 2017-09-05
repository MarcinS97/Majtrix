<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="repPlanUrlopow.ascx.cs" Inherits="HRRcp.Controls.Raporty.repPlanUrlopow" %>
<%@ Register src="~/Controls/Reports/cntReport2.ascx" tagname="cntReport" tagprefix="uc1" %>
<%@ Register src="~/Controls/SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc1" %>
<%@ Register src="../Reports/cntReportHeader.ascx" tagname="cntReportHeader" tagprefix="uc2" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>
    
        <table class="okres_navigator printoff">
            <tr>
                <td class="colleft">
<%--                    <uc1:SelectOkres ID="cntSelectOkres" OnOkresChanged="cntSelectOkres_Changed" runat="server" StoreInSession="true" />
--%>
                    Rok: 
                    <asp:DropDownList ID="ddlRok" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddlRok_SelectedIndexChanged">
                        <asp:ListItem Selected="True">2015</asp:ListItem>
                        <asp:ListItem >2014</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>

        <div class="divider_ppacc printoff"></div>

        <uc2:cntReportHeader ID="cntReportHeader1" runat="server" 
            Caption="Plan urlopów"
        />

        <asp:Menu ID="tabRaporty" CssClass="printoff" runat="server" Orientation="Horizontal" 
            onmenuitemclick="tabRaporty_MenuItemClick" >
            <StaticMenuStyle CssClass="tabsStrip" />
            <StaticMenuItemStyle CssClass="tabItem" />
            <StaticSelectedStyle CssClass="tabSelected" />
            <StaticHoverStyle CssClass="tabHover" />
            <Items>
                <asp:MenuItem Text="Brak planu"                 Value="vBrak" Selected="True" ></asp:MenuItem>
                <asp:MenuItem Text="Plan niepoprawny"           Value="vError" ></asp:MenuItem>                
                <asp:MenuItem Text="Plan poprawny"              Value="vOK" ></asp:MenuItem>                
                <asp:MenuItem Text="Plan - wszyscy"             Value="vAll" ></asp:MenuItem>                
                <asp:MenuItem Text="Kierownicy do pogonienia"    Value="vPogonic" ></asp:MenuItem>                
                
                <asp:MenuItem Text="Plan - realizacja"          Value="vRealizacja" ></asp:MenuItem>
                <asp:MenuItem Text="Plan - realizacja miesięcznie" Value="vRealizacjaMies" ></asp:MenuItem>
<%--
                <asp:MenuItem Text="Dni - Pracownicy"       Value="vDniPrac" ></asp:MenuItem>
                <asp:MenuItem Text="Szczegóły"              Value="vSzczegoly" ></asp:MenuItem>
                <asp:MenuItem Text="Duplikaty ROGER'ów"     Value="vDuplikatyROGER" ></asp:MenuItem>
--%>                
            </Items>
            <StaticItemTemplate>
                <div class="tabCaption">
                    <div class="tabLeft">
                        <div class="tabRight">
                            <asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Text") %>' />
                        </div>
                    </div>
                </div>
            </StaticItemTemplate>
        </asp:Menu>

        <div class="tabsContentLine" style="border-collapse:collapse; background-color:#FFF;">
            <asp:MultiView ID="mvRaporty" runat="server" ActiveViewIndex="0">
                <%-----------------------------------%>
                <asp:View ID="vBrak" runat="server" >
                <%-----------------------------------%>
                    <uc1:cntReport ID="cntReport1" runat="server" 
                        CssClass="RepPlanUrlopow"
                        HeaderVisible="false"
                        SQL="
select
Logo [Logo:C], 
Pracownik, 
convert(varchar(10),DataZatr,120) [Data zatrudnienia:D],
case when UmowaDo is null then 'bezterminowo' else convert(varchar(10), UmowaDo, 120) end [Data obowiązywania umowy],
Pomin [Pomiń przy planowaniu],
Powod [Powód pominięcia],
--KierLogoStr [Logo P], 
--KierownikStr [Przełożony],
KierLogo [Logo PP], 
Kierownik [Przełożony planujący], 
Email [e-mail], 
Mailing [Mailing],
case PlanOk 
when 0 then 'brak planu'
when 1 then 'plan niepoprawny'
when 2 then 'ok'
else CONVERT(varchar, PlanOk)
end [Status],
WymiarRok [Wymiar w roku:NS],
convert(varchar(10),DataZwiekszenia,120) [Data zwiększenia wymiaru:D],
UrlopNom [Urlop przysługujący do końca umowy/roku:NS],
UrlopNomRok [Urlop przysługujący do końca roku A:NS],
UrlopZaleg [Urlop zaległy B:NS],
UrlopDodatkowy [Urlop dodatkowy C:NS],
Łącznie [Łącznie A+B+C:NS],
Zaplanowany [Zaplanowany:NS],
Pozostalo [Pozostało do zaplanowania:NS]
from VPlanUrlopowNaDzis2 where Logo &lt; 80000
and PlanOk = 0
order by Pomin,Pracownik
                    "/>
                </asp:View>

                <%-----------------------------------%>
                <asp:View ID="vError" runat="server" >
                <%-----------------------------------%>
                    <uc1:cntReport ID="cntReport4" runat="server" 
                        CssClass="RepPlanUrlopow"
                        HeaderVisible="false"
                        SQL="
select
Logo [Logo:C], 
Pracownik, 
convert(varchar(10),DataZatr,120) [Data zatrudnienia:D],
case when UmowaDo is null then 'bezterminowo' else convert(varchar(10), UmowaDo, 120) end [Data obowiązywania umowy],
Pomin [Pomiń przy planowaniu],
Powod [Powód pominięcia],
--KierLogoStr [Logo P], 
--KierownikStr [Przełożony],
KierLogo [Logo PP], 
Kierownik [Przełożony planujący], 
Email [e-mail], 
Mailing [Mailing],
case PlanOk 
when 0 then 'brak planu'
when 1 then 'plan niepoprawny'
when 2 then 'ok'
else CONVERT(varchar, PlanOk)
end [Status],
WymiarRok [Wymiar w roku:NS],
convert(varchar(10),DataZwiekszenia,120) [Data zwiększenia wymiaru:D],
UrlopNom [Urlop przysługujący do końca umowy/roku:NS],
UrlopNomRok [Urlop przysługujący do końca roku A:NS],
UrlopZaleg [Urlop zaległy B:NS],
UrlopDodatkowy [Urlop dodatkowy C:NS],
Łącznie [Łącznie A+B+C:NS],
Zaplanowany [Zaplanowany:NS],
Pozostalo [Pozostało do zaplanowania:NS]
from VPlanUrlopowNaDzis2 where Logo &lt; 80000
and PlanOk = 1
order by Pomin,Pracownik
                "/>
                </asp:View>

                <%-----------------------------------%>
                <asp:View ID="vOK" runat="server" >
                <%-----------------------------------%>
                    <uc1:cntReport ID="cntReport6" runat="server" 
                        CssClass="RepPlanUrlopow"
                        HeaderVisible="false"
                        SQL="
select
Logo [Logo:C], 
Pracownik, 
convert(varchar(10),DataZatr,120) [Data zatrudnienia:D],
case when UmowaDo is null then 'bezterminowo' else convert(varchar(10), UmowaDo, 120) end [Data obowiązywania umowy],
Pomin [Pomiń przy planowaniu],
Powod [Powód pominięcia],
--KierLogoStr [Logo P], 
--KierownikStr [Przełożony],
KierLogo [Logo PP], 
Kierownik [Przełożony planujący], 
Email [e-mail], 
Mailing [Mailing],
case PlanOk 
when 0 then 'brak planu'
when 1 then 'plan niepoprawny'
when 2 then 'ok'
else CONVERT(varchar, PlanOk)
end [Status],
WymiarRok [Wymiar w roku:NS],
convert(varchar(10),DataZwiekszenia,120) [Data zwiększenia wymiaru:D],
UrlopNom [Urlop przysługujący do końca umowy/roku:NS],
UrlopNomRok [Urlop przysługujący do końca roku A:NS],
UrlopZaleg [Urlop zaległy B:NS],
UrlopDodatkowy [Urlop dodatkowy C:NS],
Łącznie [Łącznie A+B+C:NS],
Zaplanowany [Zaplanowany:NS],
Pozostalo [Pozostało do zaplanowania:NS]
from VPlanUrlopowNaDzis2 where Logo &lt; 80000
and PlanOk = 2
order by Pomin,Pracownik
                "/>
                </asp:View>
              
                <%-----------------------------------%>
                <asp:View ID="vAll" runat="server" >
                <%-----------------------------------%>
                    <uc1:cntReport ID="cntReport8" runat="server" 
                        CssClass="RepPlanUrlopow"
                        HeaderVisible="false"
                        SQL="
select
Logo [Logo:C], 
Pracownik, 
convert(varchar(10),DataZatr,120) [Data zatrudnienia:D],
case when UmowaDo is null then 'bezterminowo' else convert(varchar(10), UmowaDo, 120) end [Data obowiązywania umowy],
Pomin [Pomiń przy planowaniu],
Powod [Powód pominięcia],
--KierLogoStr [Logo P], 
--KierownikStr [Przełożony],
KierLogo [Logo PP], 
Kierownik [Przełożony planujący], 
Email [e-mail], 
Mailing [Mailing],
case PlanOk 
when 0 then 'brak planu'
when 1 then 'plan niepoprawny'
when 2 then 'ok'
else CONVERT(varchar, PlanOk)
end [Status],
WymiarRok [Wymiar w roku:NS],
convert(varchar(10),DataZwiekszenia,120) [Data zwiększenia wymiaru:D],
UrlopNom [Urlop przysługujący do końca umowy/roku:NS],
UrlopNomRok [Urlop przysługujący do końca roku A:NS],
UrlopZaleg [Urlop zaległy B:NS],
UrlopDodatkowy [Urlop dodatkowy C:NS],
Łącznie [Łącznie A+B+C:NS],
Zaplanowany [Zaplanowany:NS],
Pozostalo [Pozostało do zaplanowania:NS]
from VPlanUrlopowNaDzis2 where Logo &lt; 80000
order by Pomin,Pracownik
                "/>
                </asp:View>
             
                <%-----------------------------------%>
                <asp:View ID="vPogonic" runat="server" >
                <%-----------------------------------%>
                    <uc1:cntReport ID="cntReport9" runat="server" 
                        CssClass="RepPlanUrlopow"
                        HeaderVisible="false"
                        SQL="
select
KierLogo [Logo PP:C], 
ISNULL(Kierownik, 'Administrator') [Przełożony planujący], 
Email [e-mail], 
Mailing [Mailing],
sum(case when PlanOk = 0 then 1 else 0 end) as [Brak planu:NS],
sum(case when PlanOk = 1 then 1 else 0 end) as [Plan niepoprawny:NS]
from VPlanUrlopowNaDzis2 where Logo &lt; 80000
and Pomin = 0
group by KierLogo, Kierownik, Email, Mailing
order by Kierownik
                "/>
                </asp:View>
              
                <%-----------------------------------%>
                <asp:View ID="vRealizacja" runat="server" >
                <%-----------------------------------%>     <%-- na podstawie RepStolowkaPrac --%> 
                    <uc1:cntReport ID="cntReport5" runat="server"   
                        CssClass="RepPlanUrlopow"
                        HeaderVisible="false"
                        SQL="
declare @nadzien datetime
    /*
declare @rok datetime
set @rok = '@SQL1-12-31'
if @rok = dbo.eoy(GETDATE())
    set @nadzien = dbo.getdate(GETDATE())
else
    set @nadzien = @rok
    */

set @nadzien = dbo.getdate(GETDATE())

select
Logo [Logo:C], 
@nadzien [xxx],
Pracownik, 
convert(varchar(10),DataZatr,120) [Data zatrudnienia:D],
case when UmowaDo is null then 'bezterminowo' else convert(varchar(10), UmowaDo, 120) end [Data obowiązywania umowy],
Pomin [Pomiń przy planowaniu],
Powod [Powód pominięcia],
--KierLogoStr [Logo P], 
--KierownikStr [Przełożony],
KierLogo [Logo PP], 
Kierownik [Przełożony planujący], 
Email [e-mail], 
Mailing [Mailing],
case PlanOk 
when 0 then 'brak planu'
when 1 then 'plan niepoprawny'
when 2 then 'ok'
else CONVERT(varchar, PlanOk)
end [Status],
WymiarRok [Wymiar w roku:NS],
convert(varchar(10),DataZwiekszenia,120) [Data zwiększenia wymiaru:D],
UrlopNom [Urlop przysługujący do końca umowy/roku:NS],
UrlopNomRok [Urlop przysługujący do końca roku A:NS],
UrlopZaleg [Urlop zaległy B:NS],
UrlopDodatkowy [Urlop dodatkowy C:NS],
Łącznie [Łącznie A+B+C:NS],
Zaplanowany [Zaplanowany:NS],
Pozostalo [Pozostało do zaplanowania:NS],

W.Wypoczynkowy [Wykorzystany wypoczynkowy UW:NS],
W.NaZadanie [Na żądanie UŻ:NS],
W.Dodatkowy [Dodatkowy UD:NS],
W.Wypoczynkowy + W.NaZadanie + W.Dodatkowy [Łącznie UW+UŻ+UD:NS],
W.Opieka [Opieka art.188KP:NS]

from VPlanUrlopowNaDzis2 P
outer apply
(
select 
sum(case when AK.Symbol = 'UW' then 1 else 0 end) as Wypoczynkowy,
sum(case when AK.Symbol = 'UŻ' then 1 else 0 end) as NaZadanie,
sum(case when AK.Symbol = 'UD' then 1 else 0 end) as Dodatkowy,
sum(case when AK.Symbol = 'O2' then 1 else 0 end) as Opieka
from dbo.GetDates2(dbo.boy(@nadzien), dbo.eoy(@nadzien)) D
left join Kalendarz K on K.Data = D.Data
left join Absencja A on A.IdPracownika = P.IdPracownika and D.Data between A.DataOd and A.DataDo
left join AbsencjaKody AK on AK.Kod = A.Kod
where K.Data is null
) W
where Logo &lt; 80000
--and PlanOk = 2
and Pomin = 0
order by Pracownik
                    "/>
                    




<%--                    <table name="report">
                        <tr><td>&nbsp;</td></tr>
                        <tr><td>
                                Szczegóły
                        </td></tr>
                    </table>
                    <uc1:cntReport ID="cntReport6" runat="server"   
                        CssClass="RepStolowka"
                        HeaderVisible="false"
                        SQL="
declare 
    @dataOd datetime,
    @dataDo datetime
set @dataOd = '@SQL1'
set @dataDo = '@SQL2'

select 
convert (varchar(10), O.Data, 20) as [Data:-], 
O.Czas as [Czas], 
ISNULL(O.Pracownik, '- karta: ' + convert(varchar, O.ECUserId) + ' -') as 
    [Pracownik|Reports/RepStolowkaPracownik @SQL1 @SQL2 * @Logo @ECUserId|Pokaż dni], 
O.Logo, 
O.ECUserId as [ECUserId:N], 
O.ECUniqueId as [ECUniqueId:N],  
O.ECReaderId as [ECReaderId:N], 
O.ECCode as [ECCode:N],
O.ObiadF as [Fordońska:N0S], 
O.OdmowaF as [Odmowa Ford.:N0S], 
O.ObiadS as [Szajnochy:N0S], 
O.OdmowaS as [Odmowa Szajn.:N0S]
from VObiady O 
join (
	select Data, ECUserId from VObiadyDzienPracownik O 
	where O.Data between @dataOd and @dataDo 
	and (O.ObiadF &gt; 0 and O.ObiadS &gt; 0)
) A on A.Data = O.Data and A.ECUserId = O.ECUserId
where O.ECCode in (1,2)
order by O.Data, O.Pracownik, O.ECUserId, O.Czas                    
                    "/>
--%>
                </asp:View>




<%--
                        CMD1="select convert(varchar(10),DATEADD(dd, -dbo.dow(GETDATE())-1, dbo.getdate(GETDATE())),20)"
                        CMD1="select case when '@SQL1' = '' or '@SQL1-12-31' = dbo.eoy(GETDATE()) then convert(varchar(10),DATEADD(dd, -dbo.dow(GETDATE())-1, dbo.getdate(GETDATE())),20) else '@SQL1-12-31' end"
set @rok = case when '@SQL1' = '' then dbo.eoy(GETDATE()) else '@SQL1-12-31' end
--%>

                <%-----------------------------------%>
                <asp:View ID="vRealizacjaMies" runat="server" >
                <%-----------------------------------%>     
                    <uc1:cntReport ID="cntReport2" runat="server"   
                        CssClass="RepPlanUrlopow"
                        HeaderVisible="false"
                        CMD1="select case when '@SQL1-12-31' = dbo.eoy(GETDATE()) then convert(varchar(10),DATEADD(dd, -dbo.dow(GETDATE())-1, dbo.getdate(GETDATE())),20) else '@SQL1-12-31' end"
                        SQL="
declare @rok datetime
declare @nadzien datetime
declare @boy datetime
declare @eoy datetime	
declare @eom datetime	-- bieżący
--set @nadzien = dbo.getdate(GETDATE())

set @rok = '@SQL1-12-31'
if @rok = dbo.eoy(GETDATE())
    set @nadzien = DATEADD(dd, -dbo.dow(GETDATE())-1, dbo.getdate(GETDATE()))   -- niedziela
else
    set @nadzien = @rok
        
set @boy = dbo.boy(@nadzien)
set @eoy = dbo.eoy(@nadzien)
set @eom = dbo.eom(@nadzien)

select 
P.KadryId, 
P.Nazwisko + ' ' + P.Imie as Pracownik,
--'' as Dział,
P.KierownikNI as Przełożony,

U.Pomin [Pomiń przy planowaniu],
U.Powod [Powód pominięcia],

U.UrlopNom [Urlop przysługujący do końca umowy/roku:NS],
U.UrlopNomRok [Urlop przysługujący do końca roku A:NS],
U.UrlopZaleg [Urlop zaległy B:NS],
U.UrlopDodatkowy [Urlop dodatkowy C:NS],
U.UrlopNomRok + U.UrlopZaleg + U.UrlopDodatkowy [Łącznie A+B+C:NS],

sum(case when D.Data &lt;= @eom and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end) as [Wykorzystany:NS], 
sum(case when MONTH(D.Data) = 1  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end) as [01:NS],--[styczeń],
sum(case when MONTH(D.Data) = 2  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end) as [02:NS],--[luty],
sum(case when MONTH(D.Data) = 3  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end) as [03:NS],--[marzec],
sum(case when MONTH(D.Data) = 4  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end) as [04:NS],--[kwiecień],
sum(case when MONTH(D.Data) = 5  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end) as [05:NS],--[maj],
sum(case when MONTH(D.Data) = 6  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end) as [06:NS],--[czerwiec],
sum(case when MONTH(D.Data) = 7  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end) as [07:NS],--[lipiec],
sum(case when MONTH(D.Data) = 8  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end) as [08:NS],--[sierpień],
sum(case when MONTH(D.Data) = 9  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end) as [09:NS],--[wrzesień],
sum(case when MONTH(D.Data) = 10 and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end) as [10:NS],--[październik],
sum(case when MONTH(D.Data) = 11 and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end) as [11:NS],--[listopad],
sum(case when MONTH(D.Data) = 12 and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end) as [12:NS],--[grudzień],
U.UrlopNomRok + U.UrlopZaleg + U.UrlopDodatkowy - 
sum(case when D.Data &lt;= @eom and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end) as [Pozostały do wykorzystania:NS],

sum(case when PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end) as [Zaplanowany:NS], 
sum(case when MONTH(D.Data) = 1  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end) as [ 01:NS],--[styczeń],
sum(case when MONTH(D.Data) = 2  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end) as [ 02:NS],--[luty],
sum(case when MONTH(D.Data) = 3  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end) as [ 03:NS],--[marzec],
sum(case when MONTH(D.Data) = 4  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end) as [ 04:NS],--[kwiecień],
sum(case when MONTH(D.Data) = 5  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end) as [ 05:NS],--[maj],
sum(case when MONTH(D.Data) = 6  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end) as [ 06:NS],--[czerwiec],
sum(case when MONTH(D.Data) = 7  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end) as [ 07:NS],--[lipiec],
sum(case when MONTH(D.Data) = 8  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end) as [ 08:NS],--[sierpień],
sum(case when MONTH(D.Data) = 9  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end) as [ 09:NS],--[wrzesień],
sum(case when MONTH(D.Data) = 10 and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end) as [ 10:NS],--[październik],
sum(case when MONTH(D.Data) = 11 and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end) as [ 11:NS],--[listopad],
sum(case when MONTH(D.Data) = 12 and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end) as [ 12:NS],--[grudzień],
U.Pozostalo [Pozostało do zaplanowania:NS],


/*
sum(case when MONTH(D.Data) = 1  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end)  as [01 wyk.:NS],--[styczeń wykorzystany],
sum(case when MONTH(D.Data) = 1  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end)      as [01 plan:NS],--[styczeń plan],

sum(case when MONTH(D.Data) = 2  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end)  as [02 wyk.:NS],--[luty wykorzystany],
sum(case when MONTH(D.Data) = 2  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end)      as [02 plan:NS],--[luty plan],

sum(case when MONTH(D.Data) = 3  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end)  as [03 wyk.:NS],--[marzec wykorzystany],
sum(case when MONTH(D.Data) = 3  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end)      as [03 plan:NS],--[marzec plan],

sum(case when MONTH(D.Data) = 4  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end)  as [04 wyk.:NS],--[kwiecień wykorzystany],
sum(case when MONTH(D.Data) = 4  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end)      as [04 plan:NS],--[kwiecień plan],

sum(case when MONTH(D.Data) = 5  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end)  as [05 wyk.:NS],--[maj wykorzystany],
sum(case when MONTH(D.Data) = 5  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end)      as [05 plan:NS],--[maj plan],

sum(case when MONTH(D.Data) = 6  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end)  as [06 wyk.:NS],--[czerwiec wykorzystany],
sum(case when MONTH(D.Data) = 6  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end)      as [06 plan:NS],--[czerwiec plan],

sum(case when MONTH(D.Data) = 7  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end)  as [07 wyk.:NS],--[lipiec wykorzystany],
sum(case when MONTH(D.Data) = 7  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end)      as [07 plan:NS],--[lipiec plan],

sum(case when MONTH(D.Data) = 8  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end)  as [08 wyk.:NS],--[sierpień wykorzystany],
sum(case when MONTH(D.Data) = 8  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end)      as [08 plan:NS],--[sierpień plan],

sum(case when MONTH(D.Data) = 9  and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end)  as [09 wyk.:NS],--[wrzesień wykorzystany],
sum(case when MONTH(D.Data) = 9  and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end)      as [09 plan:NS],--[wrzesień plan],

sum(case when MONTH(D.Data) = 10 and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end)  as [10 wyk.:NS],--[październik wykorzystany],
sum(case when MONTH(D.Data) = 10 and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end)      as [10 plan:NS],--[październik plan],

sum(case when MONTH(D.Data) = 11 and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end)  as [11 wyk.:NS],--[listopad wykorzystany],
sum(case when MONTH(D.Data) = 11 and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end)      as [11 plan:NS],--[listopad plan],

sum(case when MONTH(D.Data) = 12 and K.Symbol in ('UW', 'UŻ', 'UD') then 1 else 0 end)  as [12 wyk.:NS],--[grudzień wykorzystany],
sum(case when MONTH(D.Data) = 12 and PUK.Symbol in ('UW', 'UŻ') then 1 else 0 end)      as [12 plan:NS] --[grudzień plan]
*/

null as [Wykorzystany / Zaplanowany],

convert(varchar, sum(case when MONTH(D.Data) = 1  and K.Symbol   in ('UW', 'UŻ', 'UD') then 1 else 0 end)) + '|' +
convert(varchar, sum(case when MONTH(D.Data) = 1  and PUK.Symbol in ('UW', 'UŻ')       then 1 else 0 end)) as [01],

convert(varchar, sum(case when MONTH(D.Data) = 2  and K.Symbol   in ('UW', 'UŻ', 'UD') then 1 else 0 end)) + '|' +
convert(varchar, sum(case when MONTH(D.Data) = 2  and PUK.Symbol in ('UW', 'UŻ')       then 1 else 0 end)) as [02],

convert(varchar, sum(case when MONTH(D.Data) = 3  and K.Symbol   in ('UW', 'UŻ', 'UD') then 1 else 0 end)) + '|' +
convert(varchar, sum(case when MONTH(D.Data) = 3  and PUK.Symbol in ('UW', 'UŻ')       then 1 else 0 end)) as [03],

convert(varchar, sum(case when MONTH(D.Data) = 4  and K.Symbol   in ('UW', 'UŻ', 'UD') then 1 else 0 end)) + '|' +
convert(varchar, sum(case when MONTH(D.Data) = 4  and PUK.Symbol in ('UW', 'UŻ')       then 1 else 0 end)) as [04],

convert(varchar, sum(case when MONTH(D.Data) = 5  and K.Symbol   in ('UW', 'UŻ', 'UD') then 1 else 0 end)) + '|' +
convert(varchar, sum(case when MONTH(D.Data) = 5  and PUK.Symbol in ('UW', 'UŻ')       then 1 else 0 end)) as [05],

convert(varchar, sum(case when MONTH(D.Data) = 6  and K.Symbol   in ('UW', 'UŻ', 'UD') then 1 else 0 end)) + '|' +
convert(varchar, sum(case when MONTH(D.Data) = 6  and PUK.Symbol in ('UW', 'UŻ')       then 1 else 0 end)) as [06],

convert(varchar, sum(case when MONTH(D.Data) = 7  and K.Symbol   in ('UW', 'UŻ', 'UD') then 1 else 0 end)) + '|' +
convert(varchar, sum(case when MONTH(D.Data) = 7  and PUK.Symbol in ('UW', 'UŻ')       then 1 else 0 end)) as [07],

convert(varchar, sum(case when MONTH(D.Data) = 8  and K.Symbol   in ('UW', 'UŻ', 'UD') then 1 else 0 end)) + '|' +
convert(varchar, sum(case when MONTH(D.Data) = 8  and PUK.Symbol in ('UW', 'UŻ')       then 1 else 0 end)) as [08],

convert(varchar, sum(case when MONTH(D.Data) = 9  and K.Symbol   in ('UW', 'UŻ', 'UD') then 1 else 0 end)) + '|' +
convert(varchar, sum(case when MONTH(D.Data) = 9  and PUK.Symbol in ('UW', 'UŻ')       then 1 else 0 end)) as [09],

convert(varchar, sum(case when MONTH(D.Data) = 10 and K.Symbol   in ('UW', 'UŻ', 'UD') then 1 else 0 end)) + '|' +
convert(varchar, sum(case when MONTH(D.Data) = 10 and PUK.Symbol in ('UW', 'UŻ')       then 1 else 0 end)) as [10],

convert(varchar, sum(case when MONTH(D.Data) = 11 and K.Symbol   in ('UW', 'UŻ', 'UD') then 1 else 0 end)) + '|' +
convert(varchar, sum(case when MONTH(D.Data) = 11 and PUK.Symbol in ('UW', 'UŻ')       then 1 else 0 end)) as [11],

convert(varchar, sum(case when MONTH(D.Data) = 12 and K.Symbol   in ('UW', 'UŻ', 'UD') then 1 else 0 end)) + '|' +
convert(varchar, sum(case when MONTH(D.Data) = 12 and PUK.Symbol in ('UW', 'UŻ')       then 1 else 0 end)) as [12], 


convert(varchar, sum(case when D.Data &lt;= @nadzien and K.Symbol   in ('UW', 'UŻ', 'UD') then 1 else 0 end)) + '|' +
convert(varchar, sum(case when D.Data &lt;= @nadzien and PUK.Symbol in ('UW', 'UŻ')       then 1 else 0 end)) as [Na dzień @CMD1:N],

sum(case when D.Data &lt;= @nadzien and K.Symbol   in ('UW', 'UŻ', 'UD') then 1 else 0 end) -
sum(case when D.Data &lt;= @nadzien and PUK.Symbol in ('UW', 'UŻ')       then 1 else 0 end) as [Różnica na dzień:NS] 


from dbo.GetDates2(@boy, @eoy) D
left join Kalendarz KL on KL.Data = D.Data
cross join VPrzypisaniaNaDzis P

outer apply (select * from dbo.fn_GetPlanUrlopow2(P.Id, @eom)) as U

left join Absencja A on A.IdPracownika = P.Id and D.Data between A.DataOd and A.DataDo
left join AbsencjaKody K on K.Kod = A.Kod
left join PlanUrlopow PU on PU.IdPracownika = P.Id and PU.Data = D.Data and PU.Do is null
left join AbsencjaKody PUK on PUK.Kod = PU.KodUrlopu
--left join PlanUrlopowPomin X on X.IdPracownika = P.Id and @nadzien between X.Od and ISNULL(X.Do, '20990909')
where P.Status &gt;= 0 and P.IdPrzypisania is not null and P.KadryId &lt; 80000
--and X.Id is null	-- bez pomijanych osób na dziś

--and U.Pomin = 0		-- 0 - bez pomijanych osób na dzień / 1 - pominięci

and (KL.Data is null or KL.Rodzaj = -1)   -- dzień roboczy

group by P.KadryId, P.Nazwisko, P.Imie, P.KierownikNI, U.UrlopNom, U.UrlopNomRok, U.UrlopZaleg, U.UrlopDodatkowy, U.Zaplanowany, U.Pozostalo, U.Pomin, U.Powod
order by P.Nazwisko, P.Imie, P.KadryId

                        
                        "/>
                </asp:View>



            </asp:MultiView>
        </div>

    
    
