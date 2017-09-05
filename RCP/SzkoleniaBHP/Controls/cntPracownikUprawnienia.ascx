<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPracownikUprawnienia.ascx.cs" Inherits="HRRcp.SzkoleniaBHP.Controls.cntPracownikUprawnienia" %>

<asp:HiddenField ID="hidPracId" runat="server" Visible="false"/>
<asp:HiddenField ID="hidTyp" runat="server" Visible="false"/>
<asp:HiddenField ID="hidKwal" runat="server" Visible="false"/>
<asp:HiddenField ID="hidData" runat="server" Visible="false"/>
<asp:HiddenField ID="hidMonit" runat="server" Visible="false"/>

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
        <asp:GridView ID="GridView1" runat="server" DataKeyNames="Id:-" DataSourceID="SqlDataSource1">
        </asp:GridView>
        <asp:Button ID="GridView1Cmd" runat="server" CssClass="button_postback" Text="Button" onclick="GridView1Cmd_Click" />
        <asp:HiddenField ID="GridView1CmdPar" runat="server" />
    </div>
</div>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
--declare @monit int = 30
--declare @data datetime = GETDATE()
--declare @status2 varchar(10) = 'p'
--declare @kierId int = -99
--declare @typ int = -99
--declare @kwal int = -99
--declare @sub bit = 0
--declare @lang varchar(20) = 'PL'
--declare @ucount2 int = 3
--declare @unlimited nvarchar(100) = 'bez terminu'
--declare @awp char 
--set @awp = '9'
--set @awp = 'A'
----set @awp = 'W'
----set @awp = 'P'
--declare @pracId int = 48

declare @dataW datetime
set @dataW = DATEADD(D, @monit, @data)

select 
    C.Id [Id:-], 
	C.Symbol, 
    --case when @lang = 'PL' then U.Nazwa else U.NazwaEN end [Nazwa], 
    --case when @lang = 'PL' then U.Poziom else U.PoziomEN end [Poziom:;nowrap],
	--C.Aktualny,	
	C.NazwaCertyfikatu [Nazwa],
	C.Numer [Numer zaświadczenia:;nowrap],
	--C.Kategoria, 
	C.DodatkoweWarunki [Dodatkowe warunki:;width200],	
	CONVERT(varchar(10), C.DataRozpoczecia, 20) as [Data rozpoczęcia:;nowrap],
	CONVERT(varchar(10), C.DataZakonczenia, 20) as [Data zakończenia:;nowrap],
	case when C.DataWaznosci is null then @unlimited else CONVERT(varchar(10), C.DataWaznosci, 20) end as [Data ważności:;nowrap],
	--CONVERT(varchar(10), C.DataZdobyciaUprawnien, 20) as [Data uzyskania:;nowrap],	
	C.Uwagi [Uwagi:;width200]
from VCertyfikatyUprawnienie3 C
where C.IdPracownika = @pracId 
    and (@typ = -99 or C.Typ & @typ = @typ) 
    and (@kwal = -99 or C.KwalifikacjeId = @kwal)
    and (@awp = '9' or C.Aktualny = 1) 
    and (
        @awp = '9' or
        @awp = 'A' and (C.DataWaznosci is null or C.DataWaznosci &gt;= @data) or    --tylko faktycznie aktualne
        @awp = 'W' and C.DataWaznosci between @data and @dataW or				    --faktycznie wygasające
        @awp = 'P' and C.DataWaznosci2 &lt; @data									--jak wygasł
        )     
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidPracId" Name="pracId" PropertyName="Value" Type="Int32" />
        <asp:ControlParameter ControlID="tabFilter" Name="awp" PropertyName="SelectedValue" Type="String"/>
        <asp:ControlParameter ControlID="hidData"   Name="data" PropertyName="Value" Type="DateTime"/>
        <asp:ControlParameter ControlID="hidMonit"  Name="monit" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidTyp"    Name="typ" PropertyName="Value" Type="Int32"/>
        <asp:ControlParameter ControlID="hidKwal"   Name="kwal" PropertyName="Value" Type="Int32"/>
        <asp:SessionParameter DefaultValue="PL"     Name="lang" SessionField="LNG" Type="String" />
        <asp:ControlParameter ControlID="hidUnlimited" Name="unlimited" PropertyName="Value" Type="String"/>
    </SelectParameters>
</asp:SqlDataSource>

<%--
declare @dataW datetime
set @dataW = DATEADD(D, @monit, @data)
    
select 
    C.Id [Id:-],
    U.Symbol, 
    --case when @lang = 'PL' then U.Nazwa else U.NazwaEN end [Nazwa], 
    --case when @lang = 'PL' then U.Poziom else U.PoziomEN end [Poziom:;nowrap],
	--C.Aktualny,
	
	C.NazwaCertyfikatu [Nazwa],
	C.Numer [Numer zaświadczenia:;nowrap],
	--C.Kategoria, 
	C.DodatkoweWarunki [Dodatkowe warunki:;width200],	
	CONVERT(varchar(10), C.DataRozpoczecia, 20) as [Data rozpoczęcia:;nowrap],
	CONVERT(varchar(10), C.DataZakonczenia, 20) as [Data zakończenia:;nowrap],
	case when C.DataWaznosci is null then @unlimited else CONVERT(varchar(10), C.DataWaznosci, 20) end as [Data ważności:;nowrap],
	--CONVERT(varchar(10), C.DataZdobyciaUprawnien, 20) as [Data uzyskania:;nowrap],	
	C.Uwagi [Uwagi:;width200]
from Certyfikaty C
inner join Uprawnienia U on U.Id = C.IdUprawnienia
    and (@typ = -99 or U.Typ & @typ = @typ) 
    and (@kwal = -99 or U.KwalifikacjeId = @kwal)
where C.IdPracownika = @pracId 
    and (@typ = -99 or U.Typ & @typ = @typ) 
    and (@kwal = -99 or U.KwalifikacjeId = @kwal)
    and (@awp = '9' or C.Aktualny = 1) 
    and (
        @awp = '9' or
        @awp = 'A' and (C.DataWaznosci is null or C.DataWaznosci &gt;= @data) or
        @awp = 'W' and C.DataWaznosci between @data and @dataW or
        @awp = 'P' and C.DataWaznosci &lt; @data
        )     
--%>