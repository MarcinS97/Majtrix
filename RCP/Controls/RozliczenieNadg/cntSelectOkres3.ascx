<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntSelectOkres3.ascx.cs" Inherits="HRRcp.Controls.RozliczenieNadg.cntSelectOkres3" %>

<asp:HiddenField ID="hidFrom" runat="server" />
<asp:HiddenField ID="hidTo" runat="server" />
<asp:HiddenField ID="hidOkresId" runat="server" />
<asp:HiddenField ID="hidStatus" runat="server" />

<div class="SelectOkres printoff">
    <asp:Button ID="btBegin" runat="server" Text="|◄" CssClass="button" OnClick="btBegin_Click" ToolTip="Początek funkcjonowania"/>
    <asp:Button ID="btMinusQ" runat="server" Text="◄◄" CssClass="button2 button" OnClick="btMinusQ_Click" ToolTip="Minus 3 okresy rozliczeniowe"/>
    <asp:Button ID="btPrev" runat="server" Text="◄" CssClass="button" OnClick="btPrev_Click" ToolTip="Poprzedni okres rozliczeniowy"/>
    <span class="info">
        <asp:Label ID="lbFrom" runat="server"/><span>&nbsp;...&nbsp;</span><asp:Label ID="lbTo" runat="server"/><asp:Label ID="lbStatus" CssClass="status" runat="server"/>
    </span>
    <asp:Button ID="btNext" runat="server" Text="►" CssClass="button" OnClick="btNext_Click" ToolTip="Następny okres rozliczeniowy"/>
    <asp:Button ID="btPlusQ" runat="server" Text="►►" CssClass="button2 button" OnClick="btPlusQ_Click" ToolTip="Plus 3 okresy rozliczeniowe"/>
    <asp:Button ID="btEnd" runat="server" Text="►|" CssClass="button" OnClick="btEnd_Click" ToolTip="Bieżący okres rozliczeniowy"/>
</div>

<asp:SqlDataSource ID="dsOkres" runat="server" 
    SelectCommand="
declare @data datetime
declare @pracId int
declare @cnt int
set @cnt = {2}
set @data = DATEADD(M, @cnt, '{0}')
set @pracId = {1}

select 
  ok.Id
, ISNULL(ISNULL(ok.DataOd, ok1.DataOd), ok2.DataOd) DataOd
, ISNULL(ISNULL(ok.DataDo, ok1.DataDo), ok2.DataDo) DataDo
, ISNULL(ISNULL(ok.Status, ok1.Status), ok2.Status) Status
, ISNULL(ok.Zamknal, ok1.Zamknal) Zamknal
, ISNULL(ok.DataZamkniecia, ok1.DataZamkniecia) DataZamkniecia
, pto.IdTypuOkresu
, ot.IloscMiesiecy
from (select dbo.bom(@data) DataOd, dbo.eom(@data) DataDo, 0 Status) ok2
left join rcpPracownicyTypyOkresow pto on pto.IdPracownika = @pracId and @data between pto.DataOd and ISNULL(pto.DataDo, '20990909')
left join rcpOkresyRozliczenioweTypy ot on ot.Id = pto.IdTypuOkresu
left join OkresyRozliczeniowe ok on @data between ok.DataOd and ok.DataDo and ok.Typ = pto.IdTypuOkresu
left join OkresyRozl ok1 on @data between ok1.DataOd and ok1.DataDo and ok.Id is null
    "/>

<%--
--select * from OkresyRozliczeniowe where @data between DataOd and DataDo

select 
  ok.Id
, ISNULL(ISNULL(ok.DataOd, ok1.DataOd), ok2.DataOd) DataOd
, ISNULL(ISNULL(ok.DataDo, ok1.DataDo), ok2.DataDo) DataDo
, ISNULL(ISNULL(ok.Status, ok1.Status), ok2.Status) Status
, ISNULL(ok.Zamknal, ok1.Zamknal) Zamknal
, ISNULL(ok.DataZamkniecia, ok1.DataZamkniecia) DataZamkniecia
, pto.IdTypuOkresu
, ot.IloscMiesiecy
from rcpPracownicyTypyOkresow pto
left join rcpOkresyRozliczenioweTypy ot on ot.Id = pto.IdTypuOkresu
left join OkresyRozliczeniowe ok on @data between ok.DataOd and ok.DataDo and ok.Typ = pto.IdTypuOkresu
left join OkresyRozl ok1 on @data between ok1.DataOd and ok1.DataDo and ok.Id is null
outer apply (select dbo.bom(@data) DataOd, dbo.eom(@data) DataDo, 0 Status) ok2
where pto.IdPracownika = @pracId and @data between pto.DataOd and ISNULL(pto.DataDo, '20990909')




<asp:SqlDataSource ID="dsOkresLastBefore" runat="server" 
    SelectCommand="
declare @data datetime
declare @pracId int
set @data = '{0}'
set @pracId = {1}

select top 1 * from OkresyRozliczeniowe where DataDo &lt; '{0}' order by DataOd desc 
    "/>

<asp:SqlDataSource ID="dsOkresFirstAfter" runat="server" 
    SelectCommand="
declare @data datetime
declare @pracId int
set @data = '{0}'
set @pracId = {1}

select top 1 * from OkresyRozliczeniowe where DataOd &gt; '{0}' order by DataOd
    "/>

<asp:SqlDataSource ID="dsOkresPrev" runat="server" 
    SelectCommand="
declare @data datetime
declare @pracId int
set @data = '{0}'
set @pracId = {2}

select top {1} * from OkresyRozliczeniowe where DataDo &lt; '{0}' order by DataOd desc
    "/>

<asp:SqlDataSource ID="dsOkresNext" runat="server" 
    SelectCommand="
declare @data datetime
declare @pracId int
set @data = '{0}'
set @pracId = {2}

select top {1} * from OkresyRozliczeniowe where DataOd &gt; '{0}' order by DataOd
    "/>
--%>



