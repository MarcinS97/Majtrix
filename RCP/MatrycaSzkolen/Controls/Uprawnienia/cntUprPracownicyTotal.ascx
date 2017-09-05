<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntUprPracownicyTotal.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Uprawnienia.cntUprPracownicyTotal" %>

<asp:HiddenField ID="hidTyp" runat="server" Visible="false"/>
<asp:HiddenField ID="hidKwal" runat="server" Visible="false"/>
<asp:HiddenField ID="hidStatus" runat="server" Visible="false"/>
<asp:HiddenField ID="hidCC" runat="server" Visible="false"/>
<asp:HiddenField ID="hidData" runat="server" Visible="false"/>
<asp:HiddenField ID="hidMonit" runat="server" Visible="false"/>
<asp:HiddenField ID="hidStrList" runat="server" Visible="false"/>
<asp:HiddenField ID="hidKierId" runat="server" Visible="false"/>
<asp:HiddenField ID="hidShowSub" runat="server" Visible="false"/>

<asp:HiddenField ID="hidUnlimited" runat="server" Visible="false" Value="bezterminowo"/>

<div class="zoomMatryca">
    <asp:Menu ID="tabFilter" runat="server" Orientation="Horizontal" >
        <StaticMenuStyle CssClass="tabsStrip mnFilter" />
        <StaticMenuItemStyle CssClass="tabItem" />
        <StaticSelectedStyle CssClass="tabSelected" />
        <StaticHoverStyle CssClass="tabHover" />
        <Items>
            <asp:MenuItem Text="Aktualne" Value="A" Selected="True"></asp:MenuItem>
            <asp:MenuItem Text="Wygasające" Value="W" ></asp:MenuItem>
            <asp:MenuItem Text="Przeterminowane" Value="P"></asp:MenuItem>
            <asp:MenuItem Text="Wszystkie" Value="9"></asp:MenuItem>            
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
    <div class="scroller">
        <asp:GridView ID="GridView1" runat="server" DataKeyNames="Id:-" BorderStyle="None"
            DataSourceID="SqlDataSource1" ondatabound="GridView1_DataBound">
        </asp:GridView>
        <asp:Button ID="GridView1Cmd" runat="server" CssClass="button_postback" Text="Button" onclick="GridView1Cmd_Click" />
        <asp:HiddenField ID="GridView1CmdPar" runat="server" />
    </div>
    <div class="pager">
        <span class="count"><asp:Literal ID="lbCountLabel" runat="server" Text="Ilość:" /><asp:Label ID="lbCount" runat="server" Text="0" ></asp:Label></span>        
    </div>
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
--declare @monit int = 30
--declare @data datetime = GETDATE()
--declare @status varchar(10) = 'p'
--declare @kierId int = -99
--declare @typ int = -99
--declare @kwal int = -99
--declare @cc int = -99
--declare @awp char = 'A'
--declare @sub bit = 0
--declare @lang varchar(20) = 'PL'
--declare @unlimited nvarchar(50) = 'bez terminu'

declare @dataW datetime 
set @dataW = DATEADD(D, @monit, @data)

select
	/*
	ISNULL(SUM(case when C.Id is not null and (C.DataWaznosci is null or C.DataWaznosci &gt;= @data) then 1 else 0 end), 0) [Aktualne],  
	ISNULL(SUM(case when C.DataWaznosci between @data and @dataW then 1 else 0 end), 0) [Wygasajace],  
	ISNULL(SUM(case when C.DataWaznosci &lt; @data then 1 else 0 end), 0) [Przeterm]
	--,ISNULL(SUM(case when C.Id is null then 1 else 0 end), 0) [Brak]
	*/
	P.Id [Id:-],
	P.Nazwisko + ' ' + P.Imie [Pracownik:;nowrap first],
	P.KadryId [Nr ew.],
	CONVERT(varchar(10), P.DataZatr, 20) as [Data zatrudnienia:;nowrap],
	--CONVERT(varchar(10), P.DataZwol, 20) as [Data zwolnienia:;nowrap],
    ISNULL(I.DataZwolStatus, convert(varchar(10),P.DataZwol,20)) [Data zwolnienia:;nowrap],	


    --dbo.fn_GetPracLastCCPodzialLudzi(P.GrSplitu, @data, 5, ', ') [CC],
    --dbo.fn_GetCC(R.Id, 5, ', ') [CC],
    	
    ST.Nazwa [Stanowisko],

    K.Nazwisko + ' ' + K.Imie + ISNULL(' (' + K.KadryId + ')','') [Przełożony:;nowrap],
    	
	--U.Symbol [Symbol:;nowrap],
	C.Symbol [Symbol:;nowrap],
	C.NazwaCertyfikatu [Nazwa:;nowrap],

	C.Numer [Numer zaświadczenia:;nowrap],
	C.DodatkoweWarunki [Dodatkowe warunki],
	CONVERT(varchar(10), C.DataRozpoczecia, 20) as [Data rozpoczęcia:;nowrap],
	CONVERT(varchar(10), C.DataZakonczenia, 20) as [Data zakończenia:;nowrap],	
	--C.Kategoria, 
	case when C.Id is not null and C.DataWaznosci is null then @unlimited else CONVERT(varchar(10), C.DataWaznosci, 20) end as [Data ważności:;nowrap],
	--CONVERT(varchar(10), C.DataZdobyciaUprawnien, 20) as [Data uzyskania:;nowrap],
	C.Uwagi [Uwagi:;width200]

from (
	select distinct 
	ISNULL(U2.Id, U.Id) as Id,
	U.Typ, U.KwalifikacjeId, U.PoziomId, 
	ISNULL(U2.Grupa, U.Grupa) as Grupa,   
	ISNULL(U2.PoziomPoziom, U.PoziomPoziom) as PoziomPoziom,
	U.Aktywne
	from Uprawnienia U
	outer apply (select top 1 * from Uprawnienia 
				where Typ = U.Typ and KwalifikacjeId = U.KwalifikacjeId 
				and (U.PoziomId is null and Id = U.Id or PoziomId = U.PoziomId) and Aktywne = 1 order by PoziomPoziom asc) U2
	where U.Aktywne = 1
) U 
left join Pracownicy P on P.Status != -2 and P.KadryId between 0 and 80000-1 -- CETOR

--left join PodzialLudziImport I on I.KadryId = P.KadryId and @data between I.OkresOd and ISNULL(I.OkresDo,'20990909')
left join PodzialLudziImport IX on IX.KadryId = P.KadryId and @data between IX.OkresOd and ISNULL(IX.OkresDo,'20990909')
left join PlanUrlopowPomin PU on PU.IdPracownika = P.Id and @data between PU.Od and ISNULL(PU.Do,'20990909')
left join Kody PUK on PUK.Typ = 'ABSDL' and PUK.Kod = PU.PowodKod
--outer apply (select IX.DataZwolStatus) I1
outer apply (select 
	case when PU.Id is null then IX.DataZwolStatus 
	else ISNULL(PUK.Nazwa + ' - ' + PUK.Nazwa2 + ISNULL(' (' + PU.Powod + ')', ''), ' ' + ISNULL(PU.Powod, 'absencja długotrwała')) --spacja żeby nie A!
	end as DataZwolStatus) I



--left join Przypisania R on R.IdPracownika = P.Id and @data between R.Od and ISNULL(R.Do,'20990909') and R.Status = 1
outer apply (select top 1 * from Przypisania where IdPracownika = P.Id and Od &lt;= @data and Status = 1 order by Od desc) R
left join Pracownicy K on K.Id = R.IdKierownika

outer apply (select top 1 * from PracownicyStanowiska where IdPracownika = P.Id and Od &lt;= @data order by Od desc) PS
left join Stanowiska ST on ST.Id = PS.IdStanowiska 

--left join Certyfikaty C on C.IdUprawnienia = U.Id and C.IdPracownika = P.Id and C.Aktualny = 1
outer apply (select top 1 * from VCertyfikatyUprawnienie   --najnowsze z tego samego lub wyższego poziomu
			where IdPracownika = P.Id and Typ = U.Typ and KwalifikacjeId = U.KwalifikacjeId 
			and (U.PoziomId is null and UprId = U.Id or PoziomId = U.PoziomId and PoziomPoziom &gt;= U.PoziomPoziom) and Aktualny = 1 order by DataWaznosci desc) C
where (@typ = -99 or not U.Typ & @typ = 0) and (@kwal = -99 or U.KwalifikacjeId = @kwal) and U.Grupa = 0 and U.Aktywne = 1


and P.Status != -2 and P.KadryId between 0 and 80000-1


and (
    @awp = '9' or
    @awp = 'A' and C.Id is not null and (C.DataWaznosci is null or C.DataWaznosci &gt;= @data) or
    @awp = 'W' and C.DataWaznosci between @data and @dataW or
    @awp = 'P' and C.DataWaznosci &lt; @data or
    @awp = 'B'
    )



and (
    @status = 'p'  and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &gt; @data or	--zatrudnieni
    @status = 'pa' and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &gt; @data and LEFT(ISNULL(I.DataZwolStatus,'A'),1) in ('A','2') or	--zatrudnieni
    @status = 'k'  and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &gt; @data and P.Kierownik = 1 or	--zatrudnieni
    --@status = 'n' and (P.Status = 1 or P.Status = 0 and P.DataZwol is null) or			--nowi/nieprzypisani
    @status = 'n'  and (P.Status = 1 or P.Status = 0 and (R.Id is null or ISNULL(R.Do,'20990909') &lt; @data)) or			--nowi/nieprzypisani
    @status = 'z'  and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &lt; @data or	--zwolnieni
	@status = 'all' and P.Status != -2		--wszyscy, oprócz pomijanych
    )
and (
    @kierId = -99 or
	@sub = 0 and R.IdKierownika = @kierId or
	@sub = 1 and R.IdKierownika in (select IdPracownika from dbo.fn_GetTree2(@kierId, 1, GETDATE()) where Kierownik = 1)
    )
and (
    @awp = 'B' and C.Id is null
 or @awp != 'B' and C.Id is not null
    )    
--and (@cc = -99 or R.Id in (select IdPrzypisania from SplityWspP where IdCC = @cc))
order by 2
    " onselected="SqlDataSource1_Selected">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidKierId" Name="kierId" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidShowSub" Name="sub" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidStatus" Name="status" PropertyName="Value" Type="String"/>
        <asp:ControlParameter ControlID="tabFilter" Name="awp" PropertyName="SelectedValue" Type="String"/>
        <asp:ControlParameter ControlID="hidCC" Name="cc" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidData" Name="data" PropertyName="Value" Type="DateTime"/>
        <asp:ControlParameter ControlID="hidMonit" Name="monit" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidTyp" Name="typ" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidKwal" Name="kwal" PropertyName="Value" Type="Int32"/>
        <asp:SessionParameter DefaultValue="PL" Name="lang" SessionField="LNG" Type="String" />
        <asp:ControlParameter ControlID="hidUnlimited" Name="unlimited" PropertyName="Value" Type="String"/>
    </SelectParameters>
</asp:SqlDataSource>

<%--
        <asp:ControlParameter ControlID="hidStrList" Name="strList" PropertyName="Value" Type="String"/>
--%>


<%--
--declare @monit int = 30
--declare @data datetime = GETDATE()
--declare @status varchar(10) = 'p'
--declare @kierId int = -99
--declare @typ int = 1024
--declare @kwal int = 23
--declare @sub bit = 1
--declare @lang varchar(20) = 'PL'
--declare @awp char = 'P'
--declare @uprId int = 1000082949
--declare @unlimited nvarchar(50) = 'bez terminu'

declare @dataW datetime
set @dataW = DATEADD(D, @monit, @data)
    
select 
	P.Id [Id:-],
	P.Nazwisko + ' ' + P.Imie [Pracownik:;nowrap first],
	P.KadryId [Nr ew.],
	CONVERT(varchar(10), P.DataZatr, 20) as [Data zatrudnienia:;nowrap],
	--CONVERT(varchar(10), P.DataZwol, 20) as [Data zwolnienia:;nowrap],
    ISNULL(I.DataZwolStatus, convert(varchar(10),P.DataZwol,20)) [Data zwolnienia:;nowrap],	


    --dbo.fn_GetPracLastCCPodzialLudzi(P.GrSplitu, @data, 5, ', ') [CC],
    dbo.fn_GetCC(R.Id, 5, ', ') [CC],
    	
    K.Nazwisko + ' ' + K.Imie + ISNULL(' (' + K.KadryId + ')','') [Przełożony:;nowrap],
    	
	--U.Symbol [Symbol:;nowrap],
	C.Symbol [Symbol:;nowrap],
	C.NazwaCertyfikatu [Nazwa:;nowrap],

	C.Numer [Numer zaświadczenia:;nowrap],
	C.DodatkoweWarunki [Dodatkowe warunki],
	CONVERT(varchar(10), C.DataRozpoczecia, 20) as [Data rozpoczęcia:;nowrap],
	CONVERT(varchar(10), C.DataZakonczenia, 20) as [Data zakończenia:;nowrap],	
	--C.Kategoria, 
	case when C.Id is not null and C.DataWaznosci is null then @unlimited else CONVERT(varchar(10), C.DataWaznosci, 20) end as [Data ważności:;nowrap],
	--CONVERT(varchar(10), C.DataZdobyciaUprawnien, 20) as [Data uzyskania:;nowrap],
	C.Uwagi [Uwagi:;width200]
from Pracownicy P
left join PodzialLudziImport I on I.KadryId = P.KadryId and @data between I.OkresOd and ISNULL(I.OkresDo,'20990909')
outer apply (select top 1 * from Przypisania where IdPracownika = P.Id and Od &lt;= @data and Status = 1 order by Od desc) R
left join Pracownicy K on K.Id = R.IdKierownika


--inner join Certyfikaty C on C.IdPracownika = P.Id
inner join VCertyfikatyUprawnienie3 C on C.IdPracownika = P.Id --and (U.PoziomId is null and C.IdUprawnienia = @uprId or C.PoziomId = U.PoziomId and C.PoziomPoziom &gt;= U.PoziomPoziom)
    and (@awp = '9' or C.Aktualny = 1) 
    and (
        @awp = '9' or
        @awp = 'A' and (C.DataWaznosci is null or C.DataWaznosci &gt;= @data) or
        @awp = 'W' and C.DataWaznosci between @data and @dataW or
        @awp = 'P' and C.DataWaznosci2 &lt; @data or
        @awp = 'B'
        )
inner join Uprawnienia U on U.Id = C.IdUprawnienia 
    and (@typ = -99 or U.Typ & @typ = @typ) 
    and (@kwal = -99 or U.KwalifikacjeId = @kwal)


where P.Status != -2 and P.KadryId between 0 and 80000-1



and (
    @status = 'p'  and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &gt; @data or	--zatrudnieni
    @status = 'pa' and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &gt; @data and LEFT(ISNULL(I.DataZwolStatus,'A'),1) in ('A','2') or	--zatrudnieni
    @status = 'k'  and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &gt; @data and P.Kierownik = 1 or	--kierownicy
    --@status = 'n' and (P.Status = 1 or P.Status = 0 and P.DataZwol is null) or			--nowi/nieprzypisani
    @status = 'n'  and (P.Status = 1 or P.Status = 0 and (R.Id is null or ISNULL(R.Do,'20990909') &lt; @data)) or			--nowi/nieprzypisani
    @status = 'z'  and P.Status != -2 and ISNULL(P.DataZwol,'20990909') &lt; @data or	--zwolnieni
	@status = 'all' and P.Status != -2		--wszyscy, oprócz pomijanych
    )
and (
    @kierId = -99 or
	@sub = 0 and R.IdKierownika = @kierId or
	@sub = 1 and R.IdKierownika in (select IdPracownika from dbo.fn_GetTree2(@kierId, 1, @data) where Kierownik = 1)
    )
and (
    @awp = 'B' and C.Id is null
 or @awp != 'B' and C.Id is not null
    )    
and (
    @cc = -99 or 
    R.Id in (select IdPrzypisania from SplityWspP where IdCC = @cc)
    )
order by 2
--%>