<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntKalendarz.ascx.cs" Inherits="HRRcp.RCP.Controls.Adm.cntKalendarz" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>

<div id="paKalendarz" runat="server" class="cntKalendarz">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="filter">
                <div id="paFilterKal" runat="server" visible="true" class="paFilterKal">
                    <asp:Label ID="lbRok" runat="server" Text="Rok:"></asp:Label>                
                    <asp:HiddenField ID="hidSwStale" runat="server" Visible="false" />
                    <uc1:DateEdit runat="server" ID="deRok" AutoPostBack="true" OnDateChanged="deRok_DateChanged"/>
                    <asp:Button ID="btGenerate" runat="server" Text="Wygeneruj" CssClass="btn btn-default" OnClick="btGenerate_Click" />
                    <asp:Button ID="btDeleteKal" runat="server" Text="Usuń" CssClass="btn btn-danger" OnClick="btDeleteKal_Click" />
                </div>
                <asp:Button ID="btSwietaStale" runat="server" Text="Święta stałe" CssClass="btn btn-primary" OnClick="btSwietaStale_Click" />
                <asp:Button ID="btSwietaStaleBack" runat="server" Text="Święta stałe - Powrót" CssClass="btn btn-primary" Visible="false" OnClick="btSwietaStaleBack_Click" />
            </div>
            <asp:GridView ID="gvKalendarz" runat="server" CssClass="GridView1" DataSourceID="SqlDataSource1" OnDataBound="gvKalendarz_DataBound" ></asp:GridView>
            <asp:Button ID="gvKalendarzCmd" runat="server" CssClass="button_postback" onclick="gvKalendarzCmd_Click" />
            <asp:HiddenField ID="gvKalendarzCmdPar" runat="server" />             
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <uc1:cntModal runat="server" ID="cntModalEdit" Backdrop="false" Keyboard="false" ShowFooter="true" CloseButtonText="Anuluj" >
        <ContentTemplate>
            <div class="row1">
                <label>Rodzaj dnia:</label>
                <asp:DropDownList ID="ddlRodzaj" CssClass="form-control" runat="server" DataSourceID="dsRodzaj" DataTextField="Text" DataValueField="Value" AutoPostBack="true" OnSelectedIndexChanged="ddlRodzaj_SelectedIndexChanged"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ValidationGroup="vgSave"                     
                    ControlToValidate="ddlRodzaj" 
                    ErrorMessage="pole wymagane" />
            </div>
            <div>
                <label>Opis:</label>
                <asp:TextBox ID="tbOpis" CssClass="form-control" runat="server" MaxLength="50"></asp:TextBox>
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btDelete" runat="server" Text="Usuń" CssClass="btn btn-danger left" OnClick="btDelete_Click" Visible="false"/>
            <asp:Button ID="btSave" runat="server" Text="Zapisz" ValidationGroup="vgSave" OnClick="btSave_Click"/>
        </FooterTemplate>
    </uc1:cntModal>  
</div>

<asp:Literal ID="ltTh" runat="server" Visible="false">
    <div>
        {0}
    </div>
</asp:Literal>
<asp:Literal ID="ltTd" runat="server" Visible="false">
    <div class="day rodzaj{1}" title="{4} {2}">
        <div class="top">
            {0}
        </div>
        <div class="middle">
            {3}
        </div>
        <div class="bottom">

        </div>
    </div>
</asp:Literal>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false" OnSelected="SqlDataSource1_Selected"
    SelectCommand="
declare @od datetime
declare @colnom nvarchar(max)
if ISNULL(@swstale,0) = 1 begin
    set @od = '19000101'
    set @colnom = N''
end
else begin
    set @od = dbo.boy(@rok)
    set @colnom = ', mdays - N.Wolne [Nominał:NS;nominal||dni]'
end

declare @colsH nvarchar(max)
declare @colsD nvarchar(max)

select 
  @colsD = ISNULL(@colsD + ',', '') + '[' + convert(varchar, num) + ']' 
, @colsH = ISNULL(@colsH + ',', '') + '[' + convert(varchar, num) + ']['
    + REPLICATE(' ', num / 7) + -- sql dodaje 1,2.. do zduplikowanych kolumn
	+ case num % 7 
		when 0 then 'Po:;day' 
		when 1 then 'Wt:;day'
		when 2 then 'Śr:;day'
		when 3 then 'Cz:;day'
		when 4 then 'Pi:;day'
		when 5 then 'So:;day so'
		when 6 then 'Ni:;day ni'
		end
    + '|cmd:edit @mies ' + convert(varchar, num) + ']' 
from dbo.GetNum(0, 31+6)

declare @stmt nvarchar(max)
set @stmt = N'
set language polish

declare @od datetime
declare @swstale int
set @od = ''' + REPLACE(CONVERT(varchar(10), @od, 20),'-','') + '''
set @swstale = ''' + convert(varchar, @swstale) + '''
select 
  MiesNazwa [Miesiąc \ Dzień:;miesiac]
--, mdays - N.Wolne [Nominał:;nominal||dni]
' + @colnom + '
, convert(varchar(10), bom, 20) [mies:-]
, ' + @colsH + ' 
from 
(
	select 
	  M.num + 1 Mies
	, DATENAME(MONTH, M1.bom) MiesNazwa
    , M1.bom
    , M2.mdays 
	, D.num Dzien	 
	, case when D.Dzien between 1 and M2.mdays then 
		          convert(varchar, D.Dzien)                 --0
		+ ''|'' + ISNULL(convert(varchar, K.Rodzaj), '''')  --1
		+ ''|'' + ISNULL(convert(varchar, K.Opis), '''')    --2
		+ ''|'' + case when K.Rodzaj is null then '''' else case K.Rodzaj when 0 then ''S'' when 1 then ''N'' when 2 then ''ŚW'' else ''W'' end end    --3
		+ ''|'' + case when @swstale = 1 then ''ROK'' + SUBSTRING(D2.DataStr, 5, 6) else D2.DataStr end    --4
		else ''|-99|||'' end Rodzaj
	--, K.Opis
    --, case when D.Dzien between 1 and M2.mdays and K.Data is null then 1 else 0 end Roboczy
	from dbo.GetNum(0, 12) M
	outer apply (select DATEADD(M, M.num, @od) bom) M1	
	outer apply (select dbo.dow(M1.bom) dowbom, DAY(dbo.eom(M1.bom)) mdays) M2
	outer apply (select num, num - dowbom ofs, num - dowbom + 1 Dzien from dbo.GetNum(0, 31+6)) D
	outer apply (select DATEADD(D, D.ofs, bom) Data) D1
	outer apply (select convert(varchar(10),D1.Data,20) DataStr) D2
	left join Kalendarz K on K.Data = D1.Data
) D
pivot
(
	max(D.Rodzaj) for D.Dzien in (' + @colsD + ')
) PV
outer apply (select count(*) Wolne from Kalendarz where Data between bom and dbo.eom(bom)) N
order by Mies
'
--select @stmt
exec sp_executesql @stmt
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="deRok" PropertyName="Date" Name="rok" Type="DateTime"/>
        <asp:ControlParameter ControlID="hidSwStale" PropertyName="Value" Name="swstale" Type="Int32"/>
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsEdit" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
declare @mies datetime = '{0}'
declare @idx int = {1}

declare @mdays int, @ofs int, @addday int, @date datetime 
set @mdays = DATEDIFF(DAY, @mies, dbo.eom(@mies)) -- wraca -1 i dobrze, bo porównanie 0..
set @ofs = dbo.dow(@mies)
set @addday = @idx - @ofs
set @date = DATEADD(D, @addday, @mies)

--select @mdays, @ofs, @addday, @date

select 
  @date Data
, K.Rodzaj
, K.Opis
, dbo.dow(@date) dow
from (select 1 X) X
left join Kalendarz K on K.Data = @date
where @addday between 0 and @mdays
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsRodzaj" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select null Value, 'wybierz...' Text, 0 Sort
union all
select '2|Św|Święto' Value, 'Święto' Text, 1 Sort
union all
select '0|S|Sobota', 'Sobota', 2
union all
select '1|N|Niedziela', 'Niedziela', 3
union all
select '3|W|Wolne za święto', 'Wolne za święto', 4
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsGenerate" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
declare @rok datetime
set @rok = dbo.boy('{0}')

declare @drok int
set @drok = YEAR(@rok) - 1900  -- święta stałe

--select * from Kalendarz where Data between DATEADD(YEAR, -@drok, @rok) and dbo.eoy(DATEADD(YEAR, -@drok, @rok))
--delete from Kalendarz where Data between @rok and dbo.eoy(@rok)

insert into Kalendarz
select 
  D.Data
, ISNULL(K.Rodzaj, case dow when 5 then 0 when 6 then 1 end) Rodzaj
, ISNULL(K.Opis, case dow when 5 then 'Sobota' when 6 then 'Niedziela' end) Opis  
--, DATEADD(YEAR, -@drok, D.Data), K.Data, K.Rodzaj, K.Opis
from dbo.GetDates2(@rok, dbo.eoy(@rok)) D
outer apply (select dbo.dow(D.Data) dow) D1
left join Kalendarz K on K.Data = DATEADD(YEAR, -@drok, D.Data) and K.Rodzaj = 2  -- święta stałe
where D1.dow in (5,6) or K.Data is not null
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsDelete" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
declare @rok datetime
set @rok = dbo.boy('{0}')
delete from Kalendarz where Data between @rok and dbo.eoy(@rok)
    ">
</asp:SqlDataSource>
