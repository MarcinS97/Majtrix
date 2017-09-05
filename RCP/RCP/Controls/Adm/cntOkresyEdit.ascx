<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntOkresyEdit.ascx.cs" Inherits="HRRcp.RCP.Controls.Adm.cntOkresyEdit" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagName="DateEdit" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>

<asp:HiddenField ID="hidOkresType" runat="server" Visible="false" />

<div id="ctOkresyEdit" runat="server" class="cntOkresyEdit">
    <uc1:cntModal runat="server" ID="cntModal" Title="Dodaj okres">
        <ContentTemplate>
            <div id="divTypOkresu" runat="server" class="form-group">
                <label>Typ okresu:</label>
                <asp:DropDownList ID="ddlTypOkresu" runat="server" DataValueField="Value" DataTextField="Text" DataSourceID="dsTypOkresu" OnSelectedIndexChanged="ddlTypOkresu_SelectedIndexChanged"
                    CssClass="form-control" AutoPostBack="true" />
                <asp:SqlDataSource ID="dsTypOkresu" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="
select null Value, 'wybierz ...' Text, 0 Sort, 0 IloscMiesiecy
union all
select convert(varchar,Id) + '|' + convert(varchar, IloscMiesiecy) Value, Nazwa Text, 1 Sort, IloscMiesiecy
from rcpOkresyRozliczenioweTypy
order by Sort, IloscMiesiecy desc, Text                    
                "/>
            </div>
            <div id="divOkres" runat="server" class="form-group" >
                <label>Data początku:</label>
                <uc1:DateEdit runat="server" ID="deStart" ValidationGroup="vgSave" AutoPostBack="true" OnDateChanged="deStart_DateChanged"/>
                <label class="okresdo">Data końca:</label>
                <uc1:DateEdit runat="server" ID="deEnd" ReadOnly="true"/>
            </div>
            <div id="divNom" runat="server" visible="false">
                <asp:HiddenField ID="hidNomSum" runat="server" Visible="false" />
                <asp:Repeater ID="rpNom" runat="server">
                    <HeaderTemplate>
                        <div class="row">
                            <div class="col-sm-6">
                                
                            </div>
                            <div class="col-sm-6">
                                <label></label>
                            </div>
                            <%--
                            <div class="col-sm-3">
                                <label>Domyśl</label>
                            </div>
                            --%>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:HiddenField ID="hidDate" runat="server" Visible="false" Value='<%# Eval("Data") %>' />
                        <div class="row form-group">
                            <div class="col-sm-6">
                                <label>Dni nominalne - <%# Eval("Friendly") %> (<%# Eval("Kalendarz") %>)</label>
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox ID="tbNom" runat="server" CssClass="form-control nom" MaxLength="3" Text='<%# Eval("DniPracValue") %>' Enabled='<%# Eval("IsEnabled") %>' />   <%-- placeholder='<%# Eval("Kalendarz") %>'  --%>
                                <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" TargetControlID="tbNom" FilterType="Custom" ValidChars="0123456789" />
                            </div>
                            <%--<div class="col-sm-3">
                                <span><%# Eval("Nom") %></span>
                            </div>--%>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:SqlDataSource ID="dsNom" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="
declare @mies int = (select IloscMiesiecy from rcpOkresyRozliczenioweTypy where Id = @typ)
;with a as
(
    select 1 b
    union all
    select b + 1 from a where b &lt; @mies
)
select b from a
">
                    <SelectParameters>
                        <asp:ControlParameter Name="typ" Type="Int32" ControlID="ddlTypOkresu" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" OnClick="btnSave_Click" Text="Zapisz" ValidationGroup="vgSave"/>
        </FooterTemplate>
    </uc1:cntModal>

</div>

<asp:SqlDataSource ID="dsNextOkres" runat="server" SelectCommand="select top 1 DATEADD(M,{1},DataOd) from OkresyRozliczeniowe where Typ = {0} order by DataOd desc" />
<asp:SqlDataSource ID="dsInsert" runat="server" SelectCommand="insert into OkresyRozliczeniowe (DataOd, DataDo, Status, Typ) values ({0}, {1}, 0, {2}); select SCOPE_IDENTITY();"></asp:SqlDataSource>
<asp:SqlDataSource ID="dsAddCzas" runat="server" SelectCommand="insert into CzasNom (Data, DniPrac, IdOkresu) values ({0}, {1}, {2})" />
<asp:SqlDataSource ID="dsUpdateCzas" runat="server" SelectCommand="update CzasNom set DniPrac = {0} where Data = {1} and IdOkresu = {2}" />
<asp:SqlDataSource ID="dsNewOkres" runat="server" SelectCommand="select top 1 dbo.bom(dateadd(month, 1, DataOd)) DateFrom, dbo.eom(dateadd(month, 1, DataOd)) DateTo from OkresyRozl order by DataOd desc" />
<asp:SqlDataSource ID="dsGetCzasUpdate" runat="server" SelectCommand="select DniPrac from CzasNom where Data = {0} and IdOkresu = {1}" />
<asp:SqlDataSource ID="dsGetCzasNom" runat="server" SelectCommand="
declare @nadzien datetime = {0}

declare @od datetime = dbo.bom(@nadzien)
declare @do datetime = dbo.eom(@nadzien)

select count(*)
from dbo.getDates2(@od, @do) d
left join Kalendarz k on k.Data = d.Data 
where k.Rodzaj is null
    " />
<asp:SqlDataSource ID="dsLegacyInsert" runat="server" SelectCommand="
with a as
(
	select
	  CONVERT(datetime, {0}) Od
	, dbo.eom(CONVERT(datetime, {0})) Do
	union all
	select
	  DATEADD(MONTH, 1, Od)
	, dbo.eom(DATEADD(MONTH, 1, Od))
	from a
	where a.Od &lt; {1}
)
insert OkresyRozl (DataOd, DataDo, Status, Archiwum)
select 
  a.Od
, a.Do
, 0
, 0
from a
left join OkresyRozl rozl on rozl.DataOd = a.Od and rozl.DataDo = a.Do
where a.Od &lt; {1} and rozl.Id is null
" />
<asp:SqlDataSource ID="dsCreateNewOkres" runat="server" SelectCommand="
declare @data datetime = '2023-01-01'
declare @nom nvarchar(MAX) = '-1,11,-1'
declare @w int = 3

;with m (i, m) as
(
    select 0, @data
    union all
    select i + 1, DATEADD(MONTH, 1, m.m) from m where m.m &lt; DATEADD(MONTH, @w - 1, @data)
)
select i idx, dbo.bom(m.m) Od, dbo.eom(m.m) Do into #aaa from m

insert into OkresyRozliczeniowe (DataOd, DataDo, Status, Typ)
select
  MIN(Od)
, MAX(Do)
, 0
, (select Id from rcpOkresyRozliczenioweTypy ort where ort.IloscMiesiecy = @w and ort.Aktywny = 1)
from #aaa

insert into CzasNom (Data, DniPrac, IdOkresu)
select
  Od
, case when n.items != -1 then n.items else b.b end b

, orr.Id
from #aaa a
left join dbo.SplitIntSort(@nom, ',') n on n.idx = a.idx
outer apply
(
    select
      SUM(case when k.Rodzaj is null then 1 else 0 end) b
    from dbo.GetDates2(a.Od, a.Do) d
    left join Kalendarz k on k.Data = d.Data
    group by dbo.bom(d.Data)
) b
left join OkresyRozliczeniowe orr on orr.DataOd = @data and orr.Typ = (select Id from rcpOkresyRozliczenioweTypy ort where ort.IloscMiesiecy = @w and ort.Aktywny = 1)

insert into OkresyRozliczeniowe (DataOd, DataDo, Status, Typ)
select
  Od
, Do
, 0
, (select Id from rcpOkresyRozliczenioweTypy ort where ort.IloscMiesiecy = 1 and ort.Aktywny = 1)
from #aaa

insert into CzasNom (Data, DniPrac, IdOkresu)
select
  Od
, /*case when n.items != -1 then n.items else*/ b.b /*end*/
, orr.Id
from #aaa a
/*left join dbo.SplitIntSort(@nom, ',') n on n.idx = a.idx*/
outer apply
(
    select
      SUM(case when k.Rodzaj is null then 1 else 0 end) b
    from dbo.GetDates2(a.Od, a.Do) d
    left join Kalendarz k on k.Data = d.Data
    group by dbo.bom(d.Data)
) b
inner join OkresyRozliczeniowe orr on orr.DataOd = a.Od and orr.Typ = 1

drop table #aaa    
" />
<asp:SqlDataSource ID="dsGetOkresData" runat="server" SelectCommand="select O.*, T.IloscMiesiecy from OkresyRozliczeniowe O left join rcpOkresyRozliczenioweTypy T on T.Id = O.Typ where O.Id = {0}" />
<asp:SqlDataSource ID="dsGetOkresCzasyNom" runat="server" SelectCommand="select * from CzasNom where IdOkresu = {0} order by Data" />




<%--
    
                    <label>Okres:</label>
                <asp:DropDownList ID="ddlOkresy" runat="server" DataValueField="Value" DataTextField="Text" DataSourceID="dsOkresy" CssClass="form-control"
                    OnSelectedIndexChanged="ddlOkresy_SelectedIndexChanged" AutoPostBack="true" />
                <asp:SqlDataSource ID="dsOkresy" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="
--declare @typ int = 1;
with o as
(
    select DATEADD(MONTH, 1, ISNULL(MAX(DataOd), dbo.bom(GETDATE()))) Data from OkresyRozliczeniowe where Typ = @typ
    union all
    select DATEADD(MONTH, ort.IloscMiesiecy, Data ) from o inner join rcpOkresyRozliczenioweTypy ort on ort.Id = @typ
)
select top 10
  CONVERT(varchar(10), dbo.bom(Data), 20) + ';' + CONVERT(varchar(10), DATEADD(MONTH, ort.IloscMiesiecy - 1, dbo.eom(Data)), 20) Value
, CONVERT(varchar(10), dbo.bom(Data), 20) + ' - ' + CONVERT(varchar(10), DATEADD(MONTH, ort.IloscMiesiecy - 1, dbo.eom(Data)), 20) Text
from o inner join rcpOkresyRozliczenioweTypy ort on ort.Id = @typ

">
                    <SelectParameters>
                        <asp:ControlParameter Name="typ" Type="Int32" ControlID="ddlTypOkresu" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>


    
    
    --%>