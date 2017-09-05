<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPlanRoczny2.ascx.cs" Inherits="HRRcp.Controls.cntPlanRoczny2" %>
<%@ Register src="../PlanPracyLineHeader.ascx" tagname="PlanPracyLineHeader" tagprefix="uc1" %>
<%@ Register src="../PlanPracyLine2.ascx" tagname="PlanPracyLine2" tagprefix="uc1" %>
<%@ Register src="cntPracInfo2.ascx" tagname="cntPracInfo" tagprefix="uc1" %>
<%@ Register src="cntSelectUrlop.ascx" tagname="cntSelectUrlop" tagprefix="uc1" %>
<%@ Register src="../AbsencjaLegenda.ascx" tagname="AbsencjaLegenda" tagprefix="uc2" %>

<asp:HiddenField ID="hidRokBoy" runat="server" Visible="false" />
<asp:HiddenField ID="hidCzas" runat="server" Visible="false" />
<asp:HiddenField ID="hidPracId" runat="server" Visible="false" />
<asp:HiddenField ID="hidUmowaOd" runat="server" Visible="false" />
<asp:HiddenField ID="hidUmowaDo" runat="server" Visible="false" />

<asp:HiddenField ID="hidSelUrlop" runat="server" />
<asp:HiddenField ID="hidClickPracId" runat="server" />
<asp:HiddenField ID="hidClickDayIndex" runat="server" />

<div id="divSelectZmiana" class="cntSelectZmiana" runat="server">
    <div class="title1">    
        <asp:Label ID="lbZmianyQ" runat="server" CssClass="t5" Text="Typy absencji"></asp:Label>
        <asp:Label ID="lbZmianyE" runat="server" CssClass="t5" Visible="false" Text="1) Wybierz typ urlopu/absencji do naniesienia:"></asp:Label>
    </div>
    <uc1:cntSelectUrlop ID="cntSelectUrlop" OnSelectedChanged="OnSelectUrlop" Mode="1" runat="server" />
</div>

<div id="paAdminOptions" runat="server" visible="false" class="paPlanUrlopowRok_options">
    <div>
        <asp:CheckBox ID="cbKorekta" runat="server" Visible="false" AutoPostBack="True" oncheckedchanged="cbKorekta_CheckedChanged" Text="Korekta"/>
        <span>Pokaż modyfikację z:</span>
        <asp:DropDownList ID="ddlHistory" runat="server" 
            AutoPostBack="True" onselectedindexchanged="ddlHistory_SelectedIndexChanged" 
            DataSourceID="SqlDataSource2" DataTextField="Text" DataValueField="Value" >
        </asp:DropDownList>
    </div>
</div>

<table id="tbNavigator" class="okres_navigator tbPlanUrlopowRok_navigator" runat="server" visible="false" >
    <tr>
        <td class="colleft">
            <asp:Label ID="lbPlanQ" runat="server" CssClass="t5" Text="Plan urlopów"></asp:Label>
            <asp:Label ID="lbPlanE" runat="server" CssClass="t5" Visible="false" Text="2) Kliknij w dzień i ustaw urlop/absencję:"></asp:Label>
        </td>
        <td class="colmiddle">
            <%--
            <uc1:SelectOkres ID="cntSelectOkres" StoreInSession="true" ControlID="cntPlanPracy" OnOkresChanged="cntSelectOkres_Changed" runat="server" />
            --%>
            <asp:Label ID="lbRok" Visible="false" runat="server" ></asp:Label>
        </td>
        <td class="colright">            
            <div>
                <asp:Label ID="lbOkresStatus" CssClass="t1" Visible="false" runat="server" ></asp:Label>
            </div>
            <asp:Button ID="btEditPP" runat="server" Text="Edycja" Visible="false" CssClass="button75" onclick="btEditPP_Click" />
            <asp:Button ID="btCheckPP" runat="server" Text="Sprawdź" Visible="false" CssClass="button75" onclick="btCheckPP_Click" />
            <asp:Button ID="btSavePP" runat="server" Text="Zapisz" Visible="false" CssClass="button75" onclick="btSavePP_Click" />
            <asp:Button ID="btCancelPP" runat="server" Text="Anuluj" Visible="false" CssClass="button75" onclick="btCancelPP_Click" />
            <asp:Button ID="btBack" runat="server" Text="Powrót" Visible="false" CssClass="button75" OnClientClick="showAjaxProgress();history.back();return false;" />
            <%--
            javascrip t:window.setTimeout(function(){showAjaxProgress();}, 1000);return true;
            --%>
        </td>
    </tr>
</table>

<asp:ListView ID="lvPlanPracy" runat="server" DataSourceID="SqlDataSource1" DataKeyNames="MiesiacOd"
    ondatabound="lvPlanPracy_DataBound" 
    onitemdatabound="lvPlanPracy_ItemDataBound" 
    onunload="lvPlanPracy_Unload" 
    ondatabinding="lvPlanPracy_DataBinding" 
    onitemcreated="lvPlanPracy_ItemCreated" 
    onlayoutcreated="lvPlanPracy_LayoutCreated" 
    onitemcommand="lvPlanPracy_ItemCommand" 
    onprerender="lvPlanPracy_PreRender" >
    <ItemTemplate>
        <tr class="it">
            <td id="tdMonth" class="month" runat="server">
                <asp:Label ID="MonthLabel" runat="server" />
            </td>
            <uc1:PlanPracyLine2 ID="PlanPracyLine" Mode="5" runat="server" />                            
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table runat="server" style="">
            <tr>
                <td>
                    Brak danych
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <table runat="server" class="ListView1" id="lvOuterTable">
            <tr runat="server">
                <td colspan="2" runat="server">
                    <table id="itemPlaceholderContainer" class="tbPlanPracy tbKartaRoczna tbPlanUrlopowRok" name="report4">  <%-- runat="server" --%>
                        <tr>
                            <th id="thHeader1" runat="server" class="header">
                                <uc1:cntPracInfo ID="cntPracInfo" runat="server" />
                            </th>
                            <th rowspan="2" class="suma"><div><span class="rotate90L">Suma w miesiącu</span></div></th>                            
                            <th rowspan="2" class="suma"><div><span class="rotate90L">Narastająco</span></div></th>                            
                        </tr>
                        <tr>
                            <th class="month">
                                <asp:Label ID="MonthLabel" runat="server" Text="Miesiąc"/>
                            </th>
                            <uc1:PlanPracyLineHeader ID="PlanPracyLineHeader" Mode="5" runat="server" />
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
</asp:ListView>

<asp:Button ID="btSelectCell" CssClass="button_postback" runat="server" Text="Select" onclick="btSelectCell_Click" />

<uc2:AbsencjaLegenda ID="AbsencjaLegenda" runat="server" />

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
declare 
    @colsH nvarchar(max), 
    @colsD nvarchar(max)
    ,@stmt1 nvarchar(max)    
    ,@stmt2 nvarchar(max)    
    
	,@umowaOd datetime
    ,@umowaDo datetime
    ,@dataZwol datetime
/*
  ,@boy datetime
  ,@czas datetime
  ,@pracid int
set @boy = '20160101'
set @czas = GETDATE()
set @pracid = 3010
*/

select top 1 
    --@umowaOd = ISNULL(DataZatrudnienia, dbo.boy(@czas)), 
    --@umowaDo = ISNULL(UmowaDo, dbo.eoy(@czas)),         
    
    @umowaOd = ISNULL(DataZatrudnienia, @boy), 
    --@umowaDo = ISNULL(UmowaDo, '20990909'), 

    --@umowaOd = @boy, 
    @umowaDo = '20990909', 

    @dataZwol = DataZwolnienia
from PracownicyUmowy 
where IdPracownika = @pracid and 
	UmowaOd &lt;= dbo.eoy(@czas) 	-- na dzień, nie wprzód
	order by UmowaOd desc 

if @dataZwol is not null 
    if @dataZwol between dbo.boy(@czas) and @umowaDo
        set @umowaDo = @dataZwol
        
set DATEFIRST 1
select @colsH = isnull(@colsH + ',', '') + '[' + convert(varchar, Lp) + ']',
       --@colsD = isnull(@colsD + ',', '') + '[' + convert(varchar, Lp) + '] as [' + convert(varchar, Lp) + '|' + convert(varchar, DATEPART(DW, DATEADD(D, Lp, '20010101'))) + ']'  
       @colsD = isnull(@colsD + ',', '') + '[' + convert(varchar, Lp) + ']'  
    from dbo.GetDates2(0,36 + 2)

--select @colsH, @colsD

select @stmt1 = N'
declare @boy datetime
declare @czas datetime
declare @pracid int
declare @umowaOd datetime
declare @umowaDo datetime
set @boy = ''' + convert(varchar(10), @boy, 20) + '''
set @czas = ''' + convert(varchar, ISNULL(@czas, GETDATE()), 20) + '''
set @pracid = ' + convert(varchar, @pracid) + '
set @umowaOd = ''' + convert(varchar(10), ISNULL(@umowaOd, dbo.boy(ISNULL(@czas, GETDATE()))), 20) + '''
set @umowaDo = ''' + convert(varchar(10), ISNULL(@umowaDo, dbo.eoy(ISNULL(@czas, GETDATE()))), 20) + '''
set DATEFIRST 1

SELECT MiesiacOd, MiesiacDo, DATEPART(M, MiesiacDo) as Month, ' + @colsD +
'FROM
(
	select C.MiesiacOd, C.MiesiacDo, C.Lp, 
		case 
			--when C.Lp = 37 then convert(varchar, (select count(*) from PlanUrlopow where IdPracownika = @pracid and Data between C.MiesiacOd and C.MiesiacDo and @czas between Od and ISNULL(Do, ''20990909'')))
			--when C.Lp = 38 then convert(varchar, (select count(*) from PlanUrlopow where IdPracownika = @pracid and Data between @boy and C.MiesiacDo and @czas between Od and ISNULL(Do, ''20990909'')))
			when C.Lp = 37 then convert(varchar, (select count(*) from PlanUrlopow where IdPracownika = @pracid and Data between dbo.MaxDate2(C.MiesiacOd, @umowaOd) and dbo.MinDate2(C.MiesiacDo, @umowaDo) and @czas between Od and ISNULL(Do, ''20990909'')))
			when C.Lp = 38 then convert(varchar, (select count(*) from PlanUrlopow where IdPracownika = @pracid and Data between dbo.MaxDate2(@boy, @umowaOd) and dbo.MinDate2(C.MiesiacDo, @umowaDo) and @czas between Od and ISNULL(Do, ''20990909'')))
		else convert(varchar(10), C.Day, 20) + ''|'' +
		     --isnull(convert(varchar, K.Rodzaj), '''') + ''|'' + 
		     --isnull(K.Opis, '''') + ''|'' + 
		     
		     case when AK2.Symbol = ''OD'' then ''2'' else isnull(convert(varchar, K.Rodzaj), '''') end + ''|'' + 
		     case when AK2.Symbol = ''OD'' then AK2.Nazwa else isnull(AK2.Opis, '''') end + ''|'' + 
		     
             --isnull(convert(varchar, PU.KodUrlopu), '''') + ''|'' + 
			 case when C1.dtyp = 1 then	     
				isnull(convert(varchar, PU.KodUrlopu), '''') 
				else ''''
				end + ''|'' + 

		     isnull(case when AK.PokazSymbolPU = 1 then AK.Symbol else '''' end, '''') + ''|'' + 
		     isnull(AK.KolorPU, '''') + ''|'' + 
		     isnull(AK.Nazwa, '''') + ''|'' + 
		     
		     convert(varchar, ISNULL(PU.Korekta, 0)) + ''|'' +
		     
		     --isnull(AK2.Symbol, '''') + ''|'' +
		     --isnull(AK2.Kolor, '''') + ''|'' + 
		     --isnull(AK2.Nazwa, '''') + ''|'' +
		     
		     
             --ISNULL(ISNULL(AK2.Symbol, WT.Symbol), '''')  + ''|'' +
             ISNULL(case when A.Kod is null and K.Rodzaj is null then WT.Symbol else AK2.Symbol end, '''')  + ''|'' +
		     ISNULL(AK2.Kolor, '''') + ''|'' + 		     
             --ISNULL(ISNULL(AK2.Nazwa, WT.Typ), '''') + ''|'' +
             ISNULL(case when A.Kod is null and K.Rodzaj is null then WT.Typ else AK2.Nazwa end, '''') + ''|'' +
		     
		     
		     convert(varchar, ISNULL(AK2.WyborPU, 0)) + ''|'' +
		     --case when U.lp_UmowyId is null then ''0'' else ''1'' end 
		     --''1''

		     --case when C.Day between @umowaOd and @umowaDo then ''1'' else ''0'' end 
		     /*
			 case 
		        when C.Day &lt; @umowaOd then ''0''
		        when C.Day &lt;= @umowaDo then ''1'' 
		     else ''2'' 
		     end + ''|'' +
			 */
			 convert(varchar, C1.dtyp) + ''|'' + 
		     
             ISNULL(case when A.Kod is null and K.Rodzaj is null then convert(varchar, W.Id) else null end, '''')

		end as DayData 
	from 
	(
	select 
		DATEADD(M, A.Lp, @boy) as MiesiacOd, 
		DATEADD(D, -1, DATEADD(M, A.Lp + 1, @boy)) as MiesiacDo, 
		B.Lp, 
		--B.Data, DATEPART(DW, B.Data) as DOW, DATENAME(DW, B.Data) as DName,
		case when B.Data between DATEADD(M, A.Lp, @boy) and DATEADD(D, -1, DATEADD(M, A.Lp + 1, @boy)) then B.Data else null end as Day 
	from dbo.GetDates2(1, 12) A
	outer apply dbo.GetDates2(
		DATEADD(D, -DATEPART(DW, DATEADD(M, A.Lp, @boy)) + 1, DATEADD(M, A.Lp, @boy)), 
		DATEADD(D, -DATEPART(DW, DATEADD(M, A.Lp, @boy)) + 31 + 6 + 2, DATEADD(M, A.Lp, @boy))
		) B
	) as C
	'
set @stmt2 = N'
	outer apply (
		select case 
			when C.Day &lt; @umowaOd then 0
		    when C.Day &lt;= @umowaDo then 1 
		    else 2 end dtyp) C1
	left join Kalendarz K on K.Data = C.Day
	left join PlanUrlopow PU on PU.IdPracownika = @pracid and PU.Data = C.Day and @czas between Od and ISNULL(Do, ''20990909'')
	left join AbsencjaKody AK on AK.Kod = PU.KodUrlopu
	left join Absencja A on A.IdPracownika = @pracid and C.Day between A.DataOd and A.DataDo 
	left join AbsencjaKody AK2 on AK2.Kod = A.Kod and (K.Rodzaj is null or AK2.DniWolne = 1)
	
    left join poWnioskiUrlopowe W on C.Day between W.Od and W.Do and W.IdPracownika = @pracId and W.StatusId in (3,4)  --bez odrzuconych i czekających na akceptację
    left join poWnioskiUrlopoweTypy WT on WT.Id = W.TypId 
	
	--outer apply (select top 1 * from PracownicyUmowy where IdPracownika = @pracid and C.Day between UmowaOd and ISNULL(UmowaDo, ''20990909'')) as U

) as D
PIVOT
(
	max(D.DayData) FOR D.Lp IN (' + @colsH + ')
) as PV
order by MiesiacOd'

--select @stmt1 + @stmt2
declare @stmt nvarchar(max) 
set @stmt = @stmt1 + @stmt2
exec sp_executesql @stmt 
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidRokBoy" Name="boy" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidCzas" Name="czas" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidPracId" Name="pracid" PropertyName="Value" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<%--
declare 
    @colsH nvarchar(max), 
    @colsD nvarchar(max), 
    @stmt nvarchar(max)    
--  ,@boy datetime
--  ,@czas datetime
--  ,@pracid int
--set @boy = '20130101'
--set @czas = GETDATE()
--set @pracid = 943
    ,@umowaOd datetime
    ,@umowaDo datetime
    ,@dataZwol datetime


select top 1 
    --@umowaOd = ISNULL(DataZatrudnienia, dbo.boy(@czas)), 
    --@umowaDo = ISNULL(UmowaDo, dbo.eoy(@czas)),         
    
    @umowaOd = ISNULL(DataZatrudnienia, @boy), 
    --@umowaDo = ISNULL(UmowaDo, '20990909'), 

    --@umowaOd = @boy, 
    @umowaDo = '20990909', 

    @dataZwol = DataZwolnienia
from PracownicyUmowy 
where IdPracownika = @pracid and 
	UmowaOd &lt;= dbo.eoy(@czas) 	-- na dzień, nie wprzód
	order by UmowaOd desc 

if @dataZwol is not null 
    if @dataZwol between dbo.boy(@czas) and @umowaDo
        set @umowaDo = @dataZwol
        
set DATEFIRST 1
select @colsH = isnull(@colsH + ',', '') + '[' + convert(varchar, Lp) + ']',
       --@colsD = isnull(@colsD + ',', '') + '[' + convert(varchar, Lp) + '] as [' + convert(varchar, Lp) + '|' + convert(varchar, DATEPART(DW, DATEADD(D, Lp, '20010101'))) + ']'  
       @colsD = isnull(@colsD + ',', '') + '[' + convert(varchar, Lp) + ']'  
    from dbo.GetDates2(0,36 + 2)

--select @colsH, @colsD

select @stmt = '
declare @boy datetime
declare @czas datetime
declare @pracid int
declare @umowaOd datetime
declare @umowaDo datetime
set @boy = ''' + convert(varchar(10), @boy, 20) + '''
set @czas = ''' + convert(varchar, ISNULL(@czas, GETDATE()), 20) + '''
set @pracid = ' + convert(varchar, @pracid) + '
set @umowaOd = ''' + convert(varchar(10), ISNULL(@umowaOd, dbo.boy(ISNULL(@czas, GETDATE()))), 20) + '''
set @umowaDo = ''' + convert(varchar(10), ISNULL(@umowaDo, dbo.eoy(ISNULL(@czas, GETDATE()))), 20) + '''
set DATEFIRST 1

SELECT MiesiacOd, MiesiacDo, DATEPART(M, MiesiacDo) as Month, ' + @colsD +
'FROM
(
	select C.MiesiacOd, C.MiesiacDo, C.Lp, 
		case 
			when C.Lp = 37 then convert(varchar, (select count(*) from PlanUrlopow where IdPracownika = @pracid and Data between C.MiesiacOd and C.MiesiacDo and @czas between Od and ISNULL(Do, ''20990909'')))
			when C.Lp = 38 then convert(varchar, (select count(*) from PlanUrlopow where IdPracownika = @pracid and Data between @boy and C.MiesiacDo and @czas between Od and ISNULL(Do, ''20990909'')))
		else convert(varchar(10), C.Day, 20) + ''|'' +
		     --isnull(convert(varchar, K.Rodzaj), '''') + ''|'' + 
		     --isnull(K.Opis, '''') + ''|'' + 
		     
		     case when AK2.Symbol = ''OD'' then ''2'' else isnull(convert(varchar, K.Rodzaj), '''') end + ''|'' + 
		     case when AK2.Symbol = ''OD'' then AK2.Nazwa else isnull(AK2.Opis, '''') end + ''|'' + 
		     
		     
			 isnull(convert(varchar, PU.KodUrlopu), '''') + ''|'' + 
		     isnull(case when AK.PokazSymbolPU = 1 then AK.Symbol else '''' end, '''') + ''|'' + 
		     isnull(AK.KolorPU, '''') + ''|'' + 
		     isnull(AK.Nazwa, '''') + ''|'' + 
		     
		     convert(varchar, ISNULL(PU.Korekta, 0)) + ''|'' +
		     
		     --isnull(AK2.Symbol, '''') + ''|'' +
		     --isnull(AK2.Kolor, '''') + ''|'' + 
		     --isnull(AK2.Nazwa, '''') + ''|'' +
		     
		     
             --ISNULL(ISNULL(AK2.Symbol, WT.Symbol), '''')  + ''|'' +
             ISNULL(case when A.Kod is null and K.Rodzaj is null then WT.Symbol else AK2.Symbol end, '''')  + ''|'' +
		     ISNULL(AK2.Kolor, '''') + ''|'' + 		     
             --ISNULL(ISNULL(AK2.Nazwa, WT.Typ), '''') + ''|'' +
             ISNULL(case when A.Kod is null and K.Rodzaj is null then WT.Typ else AK2.Nazwa end, '''') + ''|'' +
		     
		     
		     convert(varchar, ISNULL(AK2.WyborPU, 0)) + ''|'' +
		     --case when U.lp_UmowyId is null then ''0'' else ''1'' end 
		     --''1''

		     --case when C.Day between @umowaOd and @umowaDo then ''1'' else ''0'' end 
		     case 
		        when C.Day &lt; @umowaOd then ''0''
		        when C.Day &lt;= @umowaDo then ''1'' 
		     else ''2'' 
		     end + ''|'' +
		     
             ISNULL(case when A.Kod is null and K.Rodzaj is null then convert(varchar, W.Id) else null end, '''')

		end as DayData 
	from 
	(
	select 
		DATEADD(M, A.Lp, @boy) as MiesiacOd, 
		DATEADD(D, -1, DATEADD(M, A.Lp + 1, @boy)) as MiesiacDo, 
		B.Lp, 
		--B.Data, DATEPART(DW, B.Data) as DOW, DATENAME(DW, B.Data) as DName,
		case when B.Data between DATEADD(M, A.Lp, @boy) and DATEADD(D, -1, DATEADD(M, A.Lp + 1, @boy)) then B.Data else null end as Day 
	from dbo.GetDates2(1, 12) A
	outer apply dbo.GetDates2(
		DATEADD(D, -DATEPART(DW, DATEADD(M, A.Lp, @boy)) + 1, DATEADD(M, A.Lp, @boy)), 
		DATEADD(D, -DATEPART(DW, DATEADD(M, A.Lp, @boy)) + 31 + 6 + 2, DATEADD(M, A.Lp, @boy))
		) B
	) as C	
	left join Kalendarz K on K.Data = C.Day
	left join PlanUrlopow PU on PU.IdPracownika = @pracid and PU.Data = C.Day and @czas between Od and ISNULL(Do, ''20990909'')
	left join AbsencjaKody AK on AK.Kod = PU.KodUrlopu
	left join Absencja A on A.IdPracownika = @pracid and C.Day between A.DataOd and A.DataDo 
	left join AbsencjaKody AK2 on AK2.Kod = A.Kod and (K.Rodzaj is null or AK2.DniWolne = 1)
	
    left join poWnioskiUrlopowe W on C.Day between W.Od and W.Do and W.IdPracownika = @pracId and W.StatusId in (3,4)  --bez odrzuconych i czekających na akceptację
    left join poWnioskiUrlopoweTypy WT on WT.Id = W.TypId 
	
	--outer apply (select top 1 * from PracownicyUmowy where IdPracownika = @pracid and C.Day between UmowaOd and ISNULL(UmowaDo, ''20990909'')) as U

) as D
PIVOT
(
	max(D.DayData) FOR D.Lp IN (' + @colsH + ')
) as PV
order by MiesiacOd'

exec sp_executesql @stmt
--%>




<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
select distinct 
    ISNULL(DATEADD(ms, -DATEPART(ms, P.Od) + 1000, P.Od), GETDATE()) as Value,
	ISNULL(convert(varchar, DATEADD(ms, -DATEPART(ms, P.Od), P.Od), 20) + case when AutorId = 0 then ' (korekta automatyczna)' else '' end, 
	'brak danych') as Text
from (select 1 X) X
left join PlanUrlopow P on P.IdPracownika = @pracId and P.Data between @boy and dbo.eoy(@boy)
order by 1 desc
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidRokBoy" Name="boy" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidPracId" Name="pracid" PropertyName="Value" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<%--
select distinct 
    DATEADD(ms, -DATEPART(ms, Od) + 1000, Od) as Value,
	DATEADD(ms, -DATEPART(ms, Od), Od) as Text
from PlanUrlopow 
where IdPracownika = @pracId and Data between @boy and dbo.eoy(@boy)
order by 1 desc    
--%>

<%--
        <asp:ControlParameter ControlID="hidUmowaOd" Name="umowaOd" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidUmowaDo" Name="umowaDo" PropertyName="Value" Type="DateTime" />
--%>