<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true" CodeBehind="RaportDL.aspx.cs" Inherits="HRRcp.RaportDL" %>
<%@ Register src="~/Controls/Reports/cntReport2.ascx" tagname="cntReport2" tagprefix="uc1" %>
<%@ Register src="~/Controls/Reports/cntReportHeader.ascx" tagname="cntReportHeader" tagprefix="uc1" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagName="DateEdit" TagPrefix="uc3" %>


<%@ Register src="../Controls/PodzialLudzi/cntRaportDL.ascx" tagname="cntRaport" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
<%--
    <uc1:cntRaport ID="cntRaport" runat="server" Mode="0" />
--%>

<div class="RaportDL report_page">
    <uc1:cntReportHeader ID="cntReportHeader1" runat="server" 
        Caption="Ilość DL na CC"
    />
    
    <asp:HiddenField ID="hidAll" runat="server" Visible="false"/>
    <asp:HiddenField ID="hidUserId" runat="server" Visible="false"/>
    <asp:HiddenField ID="hidKierId" runat="server" Visible="false"/>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
            <div id="paFilter" runat="server" class="paFilter">
                <table>
                    <tr>                
<%--                    
                        <td class="ico">                        
                        </td>
--%>                    
                        <td class="col1" rowspan="2">
                            <div id="paKier" runat="server" visible="false">
                                <asp:Label ID="Label9" runat="server" class="label" Text="CC Przełożonego:" />
                                <asp:DropDownList ID="ddlPM" runat="server" AutoPostBack="True" 
                                    DataSourceID="SqlDataSourcePM" DataTextField="Pracownik" DataValueField="Id" 
                                    onselectedindexchanged="ddlPM_SelectedIndexChanged">
                                </asp:DropDownList>
                                <br />
                            </div>

                            <asp:Label ID="Label4" runat="server" class="label" Text="Dane za okres:" />
                            <asp:Label ID="Label5" runat="server" Text="Od:" />
                            <uc3:DateEdit ID="deOd" runat="server" />
                            <asp:Label ID="Label6" runat="server" Text="Do:" />
                            <uc3:DateEdit ID="deDo" runat="server" />
                            <asp:Button ID="btPrev" runat="server" CssClass="button" Text="<" OnClick="btPrev_Click" />
                            <asp:Button ID="btNext" runat="server" CssClass="button" Text=">" OnClick="btNext_Click" />                            
                            <br />

                            <asp:Label ID="Label20" runat="server" class="label" Text="CC:" />
                            <asp:DropDownList ID="ddlCC" runat="server" DataSourceID="SqlDataSourceCC" DataTextField="Name" DataValueField="Id" AutoPostBack="true" onselectedindexchanged="ddlCC_SelectedIndexChanged" >
                            </asp:DropDownList><br />
                        </td>
                        <td class="sep"></td>
                        <td class="col2">
                        </td>
                    </tr>
                    <tr>
                        <td></td>    
                        <td class="bottom_buttons">
                            <asp:Button ID="btWyszukaj" runat="server" CssClass="button75" Text="Wyszukaj" OnClick="btWyszukaj_Click" />
                            <asp:Button ID="btClear" runat="server" CssClass="button75" Text="Czyść" OnClick="btClear_Click"/>
                        </td>
                    </tr>
                </table>    
            </div>
            <asp:GridView ID="GridView1" runat="server" CssClass="GridView1" AutoGenerateColumns="true" DataSourceID="SqlDataSource1" 
                ondatabound="GridView1_DataBound" 
                onpageindexchanged="GridView1_PageIndexChanged">
            </asp:GridView>
            <div class="pager">
                <span class="count">Ilość:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <span class="count">Pokaż na stronie:</span>
                <asp:DropDownList ID="ddlLines" runat="server" AutoPostBack="true"    
                    OnChange="showAjaxProgress();"
                    OnSelectedIndexChanged="ddlLines_SelectedIndexChanged">
                    <asp:ListItem Text="20" Value="20" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="50" Value="50"></asp:ListItem>
                    <asp:ListItem Text="100" Value="100"></asp:ListItem>
                    <asp:ListItem Text="WSZYSTKO" Value="all"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>    

<asp:SqlDataSource ID="SqlDataSourcePM" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select 'Wszystkie cc' as Pracownik, -99 as Id, 1 as Sort
union all            
select P.Nazwisko + ' ' + P.Imie as Pracownik, P.Id as Id, 2 as Sort 
from Pracownicy P 
where P.Kierownik = 1 and Status &gt;= 0 and 
(dbo.GetRightId(Rights,40) = 1      -- PM
or P.Id in (select distinct UserId from ccPrawa)) 
order by Sort, Pracownik
">
</asp:SqlDataSource>        

<asp:SqlDataSource ID="SqlDataSourceCC" runat="server" CancelSelectOnNullParameter="false" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="                        
select 0 as Sort, 'wszystkie cc' as Name, null as Id, null as cc
union all   
select 
case when GETDATE() between CC.AktywneOd and ISNULL(CC.AktywneDo,'20990909') then 1 else 2 end as Sort,
case when GETDATE() between CC.AktywneOd and ISNULL(CC.AktywneDo,'20990909') then '' else '*' end +
CC.cc + ISNULL(' ' + CC.Nazwa,'') as Name, 
CC.Id, CC.cc
from CC
left join ccPrawa PR on PR.IdCC = CC.Id and PR.UserId = @kid 
where CC.Grupa = 0 and (@kid = -99 or PR.Id is not null)
order by Sort,cc
         ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidKierId" Name="kid" Type="Int32" />
        <asp:ControlParameter ControlID="hidAll" Name="all" Type="Int32" />
        <asp:ControlParameter ControlID="hidUserId" Name="userId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" CancelSelectOnNullParameter="false" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    onselected="SqlDataSource1_Selected"
    SelectCommand="
    /*
declare @cc nvarchar(100) = null
declare @kid int = -99
declare @od datetime = '20141001'
declare @do datetime = dbo.eom(@od)
    */
    
declare @klas nvarchar(100)
set @klas = 'DL'

select CC [CC]
,Nazwa [Nazwa] 
,Data [Data:D]
,case dbo.dow(Data) 
    when 0 then 'pn' 
    when 1 then 'wt' 
    when 2 then 'śr' 
    when 3 then 'czw' 
    when 4 then 'pt' 
    when 5 then 'sb' 
    when 6 then 'ndz' 
 else '' 
 end as [Dzień]
,IloscDL [Ilosć DL:NN2]
,IloscPracujacych [Ilość pracujących DL:NN2]
,IloscUrlopow [Ilość urlopów:NN2]
,IloscChorobowych [Ilość chorobowych:NN2]
,IloscPracujacych + IloscUrlopow + IloscChorobowych [Suma:NN2]
,IloscDL - IloscPracujacych - IloscUrlopow - IloscChorobowych [Różnica:NN2]
from
(
select 
ISNULL(CC1.cc, CC.cc) [CC]
,ISNULL(CC1.Nazwa,CC.Nazwa) [Nazwa] 
,D.Data [Data]
,case dbo.dow(D.Data) 
    when 0 then 'pn' 
    when 1 then 'wt' 
    when 2 then 'śr' 
    when 3 then 'czw' 
    when 4 then 'pt' 
    when 5 then 'sb' 
    when 6 then 'ndz' 
 else '' 
 end as [Dzien]
 /*
,case dbo.dow(D.Data) 
    when 0 then 'Po' 
    when 1 then 'Wt' 
    when 2 then 'Śr' 
    when 3 then 'Cz' 
    when 4 then 'Pt' 
    when 5 then 'So' 
    when 6 then 'Ni' 
 else '' 
 end as [Dzien]
 */
--,P.Nazwisko, P.Imie, P.KadryId, P.Status, R.Od, R.Do, PS.Klasyfikacja, PP.CzasZm, PP.Nadgodzinydzien, PP.NAdgodzinyNoc, PP.Akceptacja, PP.Data, BA.Typ, BA.DataOd, Ba.DataDo, WU.Od, WU.Do, CC.cc, W.Wsp,CC1.cc, W1.Wsp
/*
,count(*) [IloscDL]
,sum(case when ISNULL(PP.CzasZm,0) + ISNULL(PP.NadgodzinyDzien,0) + ISNULL(PP.NadgodzinyNoc,0) &gt; 0 then 1 else 0 end) [Ilość pracujących DL]
,sum(case when BA.Typ = 'U' then 1 else 0 end) [Ilość urlopów]
,sum(case when BA.Typ = 'Z' then 1 else 0 end) [Ilość chorobowych]
*/
,ROUND(sum(ISNULL(W1.Wsp*W.Wsp,ISNULL(W.Wsp,1))),2) [IloscDL]
,ROUND(sum(case when ISNULL(PP.CzasZm,0) + ISNULL(PP.NadgodzinyDzien,0) + ISNULL(PP.NadgodzinyNoc,0) &gt; 0 then ISNULL(W1.Wsp*W.Wsp,ISNULL(W.Wsp,1)) else 0 end),2) [IloscPracujacych]
,ROUND(sum(case when BA.Typ = 'U' and ISNULL(ISNULL(PP.IdZmianyKorekta, PP.IdZmiany),-1) != -1 then ISNULL(W1.Wsp*W.Wsp,ISNULL(W.Wsp,1)) else 0 end),2) [IloscUrlopow]
,ROUND(sum(case when BA.Typ = 'Z' then ISNULL(W1.Wsp*W.Wsp,ISNULL(W.Wsp,1)) else 0 end),2) [IloscChorobowych]

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
and (
	PS.Klasyfikacja = @klas
	)
and (
    @cc is null or ISNULL(CC1.Id, CC.Id) = @cc
    )

--and ISNULL(CC1.cc, CC.cc) = '001'
--and D.Data = '20150701'

group by D.Data, ISNULL(CC1.cc, CC.cc), ISNULL(CC1.Nazwa, CC.Nazwa)
) R
order by 1,3
    ">
    <%--
select 
ISNULL(CC1.cc, CC.cc) [CC]
,ISNULL(CC1.Nazwa,CC.Nazwa) [Nazwa] 
,D.Data [Data:D]
,case dbo.dow(D.Data) 
    when 0 then 'pn' 
    when 1 then 'wt' 
    when 2 then 'śr' 
    when 3 then 'czw' 
    when 4 then 'pt' 
    when 5 then 'sb' 
    when 6 then 'ndz' 
 else '' 
 end as [Dzień]
--,P.Nazwisko, P.Imie, P.KadryId, P.Status, R.Od, R.Do, PS.Klasyfikacja, PP.CzasZm, PP.Nadgodzinydzien, PP.NAdgodzinyNoc, PP.Akceptacja, PP.Data, BA.Typ, BA.DataOd, Ba.DataDo, WU.Od, WU.Do, CC.cc, W.Wsp,CC1.cc, W1.Wsp
,count(*) [Ilosć DL:NN2]

/*
,sum(case when ISNULL(PP.CzasZm,0) + ISNULL(PP.NadgodzinyDzien,0) + ISNULL(PP.NadgodzinyNoc,0) &gt; 0 then 1 else 0 end) [Ilość pracujących DL]
,sum(case when BA.Typ = 'U' then 1 else 0 end) [Ilość urlopów]
,sum(case when BA.Typ = 'Z' then 1 else 0 end) [Ilość chorobowych]
*/

,sum(case when ISNULL(PP.CzasZm,0) + ISNULL(PP.NadgodzinyDzien,0) + ISNULL(PP.NadgodzinyNoc,0) &gt; 0 then ISNULL(W1.Wsp*W.Wsp,ISNULL(W.Wsp,1)) else 0 end) [Ilość pracujących DL:NN2]
,sum(case when BA.Typ = 'U' and ISNULL(ISNULL(PP.IdZmianyKorekta, PP.IdZmiany),-1) != -1 then ISNULL(W1.Wsp*W.Wsp,ISNULL(W.Wsp,1)) else 0 end) [Ilość urlopów:NN2]
,sum(case when BA.Typ = 'Z' then ISNULL(W1.Wsp*W.Wsp,ISNULL(W.Wsp,1)) else 0 end) [Ilość chorobowych:NN2]

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

and (
    @cc is null or ISNULL(CC1.cc, CC.cc) = @cc
    )

--and ISNULL(CC1.cc, CC.cc) = '001'
--and D.Data = '20150701'

group by D.Data, ISNULL(CC1.cc, CC.cc), ISNULL(CC1.Nazwa, CC.Nazwa)
order by 1,3


/*
where (
   (@str is not null or @all = 1) and LZ.Id_Str_Org in (select Id from dbo.fn_GetStrOrgTree(ISNULL(@str,0), 1, GETDATE())) or
    @str is null and 
        (
        @all = 1 and (LZ.Id_Str_Org is null or LZ.Id_Str_Org = 0) or
        @all = 0 and (LZ.Id_Str_Org = 0 or LZ.Id_Str_Org in (select Id from #strorg))
        )
    )
and (
    @waga is null or Z.Waga = @waga
    )
and (
    @zad is null or Z.Id_Zadania = @zad
    )    
and (
    @prac is null or O.Id_Pracownicy = @prac
    )    
and (
    @kier is null or O.Id_Przelozony = @kier
    )    
and	(
	@od is null or @od &lt;= O.DataOceny
	)
and (
	@do is null or O.DataOceny &lt;= @do
	)
and (
    @ocena is null or ISNULL(O.Ocena,-1) = @ocena
    )    
    
order by 1,2,4,6

OPTION (OPTIMIZE FOR (
@str	= null,
@waga	= null,
@zad	= null,
@prac	= null,
@kier	= null,
@ocena  = null,
@od	= null,
@do	= null
))
*/ 
    --%>

    <%--
        <asp:ControlParameter ControlID="tbSearch" Name="search" Type="String" />
        <asp:ControlParameter ControlID="ddlPM" Name="kid_" PropertyName="SelectedValue" Type="Int32" />
    --%>
    <SelectParameters>
        <asp:ControlParameter ControlID="hidKierId" Name="kid" Type="Int32" />
        <asp:ControlParameter ControlID="ddlCC" Name="cc" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter ControlID="deOd" Name="od" Type="DateTime" PropertyName="Date" />
        <asp:ControlParameter ControlID="deDo" Name="do" Type="DateTime" PropertyName="Date" />
        <asp:ControlParameter ControlID="hidAll" Name="all" Type="Int32" />
        <asp:ControlParameter ControlID="hidUserId" Name="userId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource7" runat="server" CancelSelectOnNullParameter="false" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="                        
select null as Id, 'wybierz ...' as Name, null as Kolejnosc
union all   
select ISNULL(Ocena,-1) as Id, ISNULL(convert(varchar,Ocena) + ' - ','') + Nazwa as Name, Kolejnosc 
from OcenyNazwy
order by Kolejnosc, Id, Name
         ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidUserId" Name="UserId" Type="Int32"/>
        <asp:ControlParameter ControlID="hidAll" Name="all" Type="Int32"/>
    </SelectParameters>
</asp:SqlDataSource>
    
</asp:Content>
