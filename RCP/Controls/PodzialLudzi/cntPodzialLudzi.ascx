<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPodzialLudzi.ascx.cs" Inherits="HRRcp.Controls.PodzialLudzi.cntPodzialLudzi" %>
<%--
<%@ Register src="~/Controls/Przypisania/cntSplityWsp.ascx" tagname="cntSplityWsp" tagprefix="uc6" %>
--%>
<%@ Register src="~/Controls/PodzialLudzi/cntSplityWsp2.ascx" tagname="cntSplityWsp" tagprefix="uc6" %>
<%@ Register src="~/Controls/Portal/cntSqlTabs.ascx" tagname="cntSqlTabs" tagprefix="uc3" %>
<%@ Register src="~/Controls/DateEdit.ascx" tagname="DateEdit" tagprefix="uc2" %>

<div id="paPodzialLudzi" runat="server" class="cntPodzialLudzi">
    <asp:HiddenField ID="hidOd" runat="server" Visible="false" />
    <asp:HiddenField ID="hidDo" runat="server" Visible="false" />
    <asp:HiddenField ID="hidKierId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidNoEdit" runat="server" Visible="false" />
    <asp:HiddenField ID="hidClass" runat="server" Visible="false" />
    <asp:HiddenField ID="hidStawki" runat="server" Visible="false" />
    <asp:HiddenField ID="hidStawkiH" runat="server" Visible="false" />
    
    <div id="paFilter" runat="server" class="paFilter" visible="true">
        <div class="left">
            <span class="label">Wyszukaj:</span>
            <asp:TextBox ID="tbSearch" CssClass="search textbox" runat="server" ></asp:TextBox>
            <asp:Button ID="btClear" runat="server" CssClass="button75" Text="Czyść" />
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
            <asp:Button ID="btSearch" runat="server" CssClass="button_postback" Text="Wyszukaj" onclick="btSearch_Click" />

            <div class="kryteria">
                <div id="paKier" runat="server" visible="false">
                    <span>Pracownicy z CC przełożonego:</span>
                    <asp:DropDownList ID="ddlPM" runat="server" AutoPostBack="True" 
                        DataSourceID="SqlDataSource2" DataTextField="Pracownik" DataValueField="Id" 
                        onselectedindexchanged="ddlPM_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                    SelectCommand="
select 'Wszyscy pracownicy' as Pracownik, -99 as Id, 1 as Sort
union all            
select P.Nazwisko + ' ' + P.Imie as Pracownik, P.Id as Id, 2 as Sort 
from Pracownicy P 
where P.Kierownik = 1 and Status &gt;= 0 and 
(dbo.GetRightId(Rights,38) = 1
or P.Id in (select distinct UserId from ccPrawa)) 
order by Sort, Pracownik
                ">
                </asp:SqlDataSource>        
            </div>

            <div class="tabsLine">
                <uc3:cntSqlTabs ID="tabClass" runat="server" OnSelectTab="tabClass_SelectTab" Visible="true" />
<%--
                <asp:Menu ID="tabClass" CssClass="printoff" runat="server" Orientation="Horizontal" Visible="true"
                    onmenuitemclick="tabClass_MenuItemClick" >
                    <StaticMenuStyle CssClass="tabsStrip" />
                    <StaticMenuItemStyle CssClass="tabItem" />
                    <StaticSelectedStyle CssClass="tabSelected" />
                    <StaticHoverStyle CssClass="tabHover" />
                    <Items>
                        <asp:MenuItem Text="DL" Value="DL" Selected="true"></asp:MenuItem>
                        <asp:MenuItem Text="BUIL" Value="BUIL" ></asp:MenuItem>
                        <asp:MenuItem Text="SG&A" Value="SG&A" ></asp:MenuItem>
                        <asp:MenuItem Text="Wszyscy pracownicy" Value="all" ></asp:MenuItem>
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
--%>
                <asp:Menu ID="tabDane" CssClass="printoff" runat="server" Orientation="Horizontal" Visible="true"
                    onmenuitemclick="tabDane_MenuItemClick" >
                    <StaticMenuStyle CssClass="tabsStrip" />
                    <StaticMenuItemStyle CssClass="tabItem" />
                    <StaticSelectedStyle CssClass="tabSelected" />
                    <StaticHoverStyle CssClass="tabHover" />
                    <Items>
                        <asp:MenuItem Text="Pokaż dane" Value="SHOW" Selected="true"></asp:MenuItem>
                        <asp:MenuItem Text="Ukryj dane" Value="HIDE" ></asp:MenuItem>
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
                
                <asp:Menu ID="tabWartosci" CssClass="printoff" runat="server" Orientation="Horizontal" Visible="true"
                    onmenuitemclick="tabWartosci_MenuItemClick" >
                    <StaticMenuStyle CssClass="tabsStrip" />
                    <StaticMenuItemStyle CssClass="tabItem" />
                    <StaticSelectedStyle CssClass="tabSelected" />
                    <StaticHoverStyle CssClass="tabHover" />
                    <Items>
                        <asp:MenuItem Text="Podział * FTE" Value="PLFTE" Selected="true"></asp:MenuItem>
                        <asp:MenuItem Text="Podział" Value="PL" ></asp:MenuItem>
                        <asp:MenuItem Text="Przypisania do cc" Value="CC" ></asp:MenuItem>
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
            </div>

            <div class="border xfiszka" >

                <asp:GridView ID="gvPodzial" runat="server" DataSourceID="SqlDataSource1" 
                    EmptyDataText="Brak danych"
                    onload="gvPodzial_Load" >
                </asp:GridView>
                <div class="pager">
                    <span class="count">Ilość:<asp:Label ID="lbCount" runat="server" ></asp:Label></span>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <span class="count">Pokaż na stronie:</span>
                    <asp:DropDownList ID="ddlLines" runat="server" AutoPostBack="true"    
                        OnChange="showAjaxProgress();"
                        OnSelectedIndexChanged="ddlLines_SelectedIndexChanged">
                        <asp:ListItem Text="25" Value="25" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                        <asp:ListItem Text="WSZYSTKO" Value="all"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <asp:Button ID="gvPodzialCmd" runat="server" CssClass="button_postback" Text="Button" onclick="gvPodzialCmd_Click" />
                <asp:HiddenField ID="gvPodzialCmdPar" runat="server" />
                <asp:HiddenField ID="gvPodzialSelected" runat="server" />
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="paContainer" runat="server">
                <div id="divZoom" style="display:none;" class="modalPopup">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <table id="tbInfo" runat="server" class="info table0" visible="false">
                                <tr>
                                    <th class="selected">
                                        Zaznaczeni pracownicy:
                                    </th>
                                    <th class="napodstawie">
                                        Split na podstawie:
                                    </th>
                                </tr>
                                <tr>
                                    <td class="selected">
                                        <asp:Label ID="lbSelected" runat="server" CssClass="selected" ></asp:Label>
                                    </td>
                                    <td class="napodstawie">
                                        <asp:Label ID="lbNaPodstawie" runat="server" CssClass="napodstawie" ></asp:Label>
                                    </td>
                                </tr>
                            </table>
<%--
                            <div class="top_buttons">
                                <uc2:dateedit ID="deNaDzien" runat="server" ValidationGroup="vgImport" Visible="false"/>
                                <asp:Button ID="btImport" class="button" runat="server" Text="Import splitów na dzień" onclick="btImport_Click" ValidationGroup="vgImport" Visible="false"/>
                            </div>
--%>
                            <uc6:cntSplityWsp ID="cntSplityWsp1" Mode="1" Type="1" runat="server" Visible="true"/>
                            <div class="bottom_buttons">
                                <div class="left">
                                    <uc2:dateedit ID="deNaDzien" runat="server" ValidationGroup="vgImport" Visible="false" CalendarPosition="TopLeft" />
                                    <asp:Button ID="btImport" class="button" runat="server" Text="Import splitów na dzień" onclick="btImport_Click" ValidationGroup="vgImport" Visible="false"/>
                                </div>
                                <div class="right">
                                    <asp:Button ID="btExcel" CssClass="button100" runat="server" Text="Excel" OnClick="btExcel_Click" Visible="false"/>
                                    <asp:Button ID="btSave" CssClass="button100" runat="server" Text="Zapisz" OnClick="btSave_Click" />
                                    <asp:Button ID="btSave2" CssClass="button_postback" runat="server" Text="Zapisz" OnClick="btSave2_Click" />
                                    <asp:Button ID="btClose" CssClass="button100" runat="server" Text="Anuluj" OnClick="btClose_Click" />
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btExcel"/>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" CancelSelectOnNullParameter="false" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    OnSelected="SqlDataSource1_Selected"
    SelectCommand="
/*
declare @od datetime 
declare @do datetime 
declare @noEdit varchar(2)
declare @class varchar(20)
declare @typ varchar(10)
declare @stawki bit 
declare @stawkiH bit 
declare @kid int
declare @selected varchar(max)

set @od = '20150201'
set @do = '20150217'
set @noEdit = ''
set @class = 'all'
set @typ = 'PLFTE'
set @stawki = 1
set @stawkiH = 1
set @kid = -99
set @selected = null
*/

declare @xfte bit
declare @p19 bit   -- podzielone grupy splitów
declare @brutto nvarchar(200)

set @xfte = case when @typ = 'PLFTE' then 1 else 0 end
set @p19 = case when @typ = 'CC' then 0 else 1 end
--set @brutto = case when @stawki = 1 then 'PV.Brutto [Brutto:N;dane1],' else '' end
set @brutto = case when @stawki = 1 then 
    case when @stawkiH = 1 then 
        'PV.Brutto as [Brutto:N;dane1],'
    else
        'case when dbo.GetRightId(P.Rights,47) = 1 then null else PV.Brutto end as [Brutto:N;dane1],' 
    end
else '' 
end

declare 
    @colsH nvarchar(max), 
    @colsD nvarchar(max), 
    @sumWsp nvarchar(max),
    @stmt nvarchar(max)

select 
    @sumWsp = isnull(@sumWsp + '+', '') + 'ISNULL(ROUND([' + CC.cc + '],4),0)',
    @colsH = isnull(@colsH + ',', '') + '[' + CC.cc + ']',
    @colsD = isnull(@colsD + ', ', '') + 'ISNULL(ROUND([' + CC.cc + '],4),0) as [' + CC.cc + ':N;left' + 
                                            case when CC.Surplus = 1 then ' s' else '' end + 
                                            case when CC.Grupa = 1 then ' g' else '' end + 
                                            case when @kid != -99 and R.Id is null then ' x' else '' end + 
                                            '||' + CC.cc + ' ' + CC.Nazwa + ']'
from CC 
left join ccPrawa R on R.UserId = @kid and R.IdCC = CC.Id
where @do between AktywneOd and ISNULL(AktywneDo, '20990909') 
and (@p19 = 0 or CC.Grupa = 0)
--and (@kid = -99 or R.Id is not null)  -- ograniczenie wyswietlanych cc
order by 
    case 
        when CC.Surplus = 1 then 0 
        when CC.Grupa = 1 then 2
    else 1 
    end, 
    CC.cc


set @stmt = '
declare @dataDo datetime
declare @dataOd datetime
declare @class varchar(50)
declare @xfte bit
declare @p19 bit
declare @kid int
declare @selected varchar(max)
set @dataOd = ''' + convert(varchar(10), @od, 20) + '''
set @dataDo = ''' + convert(varchar(10), @do, 20) + '''
set @class = ''' + @class + '''
set @xfte = ' + CONVERT(varchar, @xfte) + '
set @p19 = ' + CONVERT(varchar, @p19) + '
set @kid = ' + CONVERT(varchar, @kid) + '
set @selected = ' + case when @selected is null then 'null' else '''' + @selected + '''' end + '
 
SELECT 
	P.Nazwisko [nn:-],
	P.Imie [ii:-],
    P.Id [pracId:-],
    P.KadryId [nrew:-], 
    @dataOd [od:-],
    @dataDo [do:-],
    PV.IdSplitu [splitId:-],
    
    ' + case when @noEdit != 1 then '''Edycja'' [:;control|cmd:edit @pracId @splitId @do 1|Edytuj split],' else '' end + '

    --xP.Id [:CB;check],
    --xcase when SEL.items is not null then ''1'' else ''0'' end + ''|'' + convert(varchar, P.Id) [:CB;check],
    --xcase when SEL.items is not null then ''1'' else ''0'' end [:CB;check|js:gvRowSelect(this @pracId);],    
    
    
    
    SEL.items [Zaznacz:CB;check|@pracId|Zaznacz osobę],
    --SEL.items [:CBH;check|@pracId|Zaznacz osobę],
    
    
    
    P.KadryId as [Nr ew.], 
    --P.Nazwisko + '' '' + P.Imie as [Pracownik: C|Reports/RepCCPracCC @od @do * 0 @nrew 1 1|Czas przepracowany na wszystkie cc], 
    P.Nazwisko + '' '' + P.Imie as [Pracownik], 
    P.Opis as [Lokalizacja],
    
	--I.DataZatr [Data zatrudnienia;left],
	P.DataZatr [Data zatrudnienia:D;left dane1],
	--P.DataZwol [Data zwolnienia:D;left dane1],
	
	PV.DataZwolStatus [Data zwolnienia:;left dane1],
	
	--D.Nazwa as [Dział], 
	ST.Nazwa as [Stanowisko:;dane1], 
	--I.Stanowisko [Stanowisko],
	
	C.Commodity [Commodity:;dane1],		
	A.Area [Area:;dane1],
	PO.Position [Position:;dane1],
	
	K.KadryId as [Nr ew. P:;dane1], K.Nazwisko + '' '' + K.Imie [Przełożony:;dane1], 
	--K.KadryId as [Nr ew.-1:;dane1], 
	K1.Nazwisko + '' '' + K1.Imie [Przełożony-1:;dane1], 
	
	PV.TypImport [Rodzaj pracownika:;dane1],
	PV.Class [Class],
	PV.Grade [Grade:;dane1],' + 

	@brutto + '
	
	PV.Head [Head:N;left dane1],
	PV.FTE [FTE:N;left],
	PV.OstatniDzienWKosztach [Ostatni dzień w kosztach:D;left dane1],
	
	cast(PV.CHwKosztach as int) [CH w kosztach],
	
	null [CC:;dane1],
	
	' + @colsD + 
	',' + @sumWsp + ' [Suma:N;left]' +
	 
	case when @noEdit != 1 then ',''Edycja'' [:;control|cmd:edit @pracId @splitId @do 2|Edytuj split]' else '' end + '
FROM
(
	SELECT I.*, S.Id as IdSplitu,
	--CC.cc as CCcc, 	
	ISNULL(C1.cc, CC.cc) as CCcc, 
	case when @xfte = 1 then ISNULL(W1.Wsp * W.Wsp, W.Wsp) * I.FTE else ISNULL(W1.Wsp * W.Wsp, W.Wsp) end as Wsp

	FROM PodzialLudziImport I
	left join Splity S on S.GrSplitu = I.KadryId and @dataDo between s.DataOd and ISNULL(S.DataDo, ''20990909'')
	left join SplityWsp W on W.IdSplitu = S.Id
	left join CC on CC.Id = W.IdCC
	
	left join Splity S1 on @p19 = 1 and CC.Grupa = 1 and S1.GrSplitu = CC.GrSplitu and @dataDo between S1.DataOd and ISNULL(S1.DataDo, ''20990909'')
	left join SplityWsp W1 on W1.IdSplitu = S1.Id
	left join CC C1 on C1.Id = W1.IdCC	
	
	where @dataDo between I.OkresOd and ISNULL(I.OkresDo, ''20990909'')
	and (@kid = -99 or I.KadryId in    -- tylko pracownicy, którzy cos maja na cc kierownika
	(
        select distinct P.KadryId from Pracownicy P
        inner join Splity S on S.GrSplitu = P.GrSplitu and @dataDo between S.DataOd and ISNULL(S.DataDo, ''20990909'')
        inner join SplityWsp W on W.IdSplitu = S.Id
        
        
        left join CC on CC.Id = W.IdCC
        left join Splity S1 on S1.GrSplitu = CC.GrSplitu and @dataDo between S1.DataOd and ISNULL(S1.DataDo, ''20990909'')
        left join SplityWsp W1 on W1.IdSplitu = S1.Id
        left join CC CC1 on CC1.Id = W1.IdCC
        inner join ccPrawa R on R.UserId = @kid and R.IdCC = isNULL(W1.IdCC, W.IdCC)
        
        
        --inner join ccPrawa R on R.UserId = @kid and R.IdCC = W.IdCC
	))
) as D
PIVOT
(
	sum(Wsp) FOR D.CCcc IN (' + @colsH + ')
) as PV
left join Pracownicy P on P.KadryId = PV.KadryId
--left join PodzialLudziImport I on I.KadryId = P.KadryId and @dataDo between I.OkresOd and ISNULL(I.OkresDo, ''20990909'')
outer apply (select top 1 * from Przypisania where IdPracownika = P.Id and Od &lt;= @dataDo and Status = 1 order by Od desc) R
outer apply (select top 1 * from Przypisania where IdPracownika = R.IdKierownika and Od &lt;= @dataDo and Status = 1 order by Od desc) R1
left join Pracownicy K on K.Id = R.IdKierownika
left join Pracownicy K1 on K1.Id = R1.IdKierownika
left join Commodity C on C.Id = R.IdCommodity
left join Area A on A.Id = R.IdArea
left join Position PO on PO.Id = R.IdPosition


--@SQL1

left join PracownicyStanowiska PS on PS.IdPracownika = P.Id and @dataDo between PS.Od and ISNULL(PS.Do, ''20990909'')
left outer join Dzialy D on D.Id = PS.IdDzialu
left outer join Stanowiska ST on ST.Id = PS.IdStanowiska

outer apply (select items from dbo.SplitInt(@selected,'','') where items = P.Id) SEL

where 
(
    @class = ''all'' or
    @class = ''noclass'' and PV.TypImport is null or
    @class = PV.TypImport
)
order by 1,2
'

exec sp_executesql @stmt
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidOd" Name="od" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidDo" Name="do" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="hidKierId" Name="kid" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="hidClass" Name="class" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="hidStawki" Name="stawki" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="hidStawkiH" Name="stawkiH" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="tabClass" Name="xxclass" PropertyName="SelectedValue" Type="String" />
        <asp:ControlParameter ControlID="tabWartosci" Name="typ" PropertyName="SelectedValue" Type="String" />
        <asp:ControlParameter ControlID="hidNoEdit" Name="noEdit" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="gvPodzialSelected" Name="selected" PropertyName="Value" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>
