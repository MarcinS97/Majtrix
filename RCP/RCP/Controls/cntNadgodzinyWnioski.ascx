<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntNadgodzinyWnioski.ascx.cs" Inherits="HRRcp.RCP.Controls.cntNadgodzinyWnioski" %>
<%@ Register Src="~/Controls/SelectOkres.ascx" TagName="SelectOkres" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Portal/cntSqlTabs.ascx" TagPrefix="uc1" TagName="cntSqlTabs" %>
<%@ Register Src="~/RCP/Controls/cntNadgodzinyWnioskiModal.ascx" TagPrefix="uc1" TagName="cntNadgodzinyWnioskiModal" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>


<div id="ctWnioskiNadgodziny" runat="server" class="cntWnioskiNadgodziny">
    <asp:HiddenField ID="hidUserId" runat="server" Visible="false" />
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:cntSqlTabs runat="server" ID="cntSqlTabs" OnSelectTab="cntSqlTabs_SelectTab" OnDataBound="cntSqlTabs_DataBound"
                Items="
Złóż wniosek|-88
Moje wnioski|1
Do akceptacji|2
Zaakceptowane|3
Odrzucone|4
Wszystkie wnioski|-99
                    " />
            <%--<uc1:SelectOkres ID="cntSelectOkres" StoreInSession="true" OnOkresChanged="cntSelectOkres_OkresChanged" runat="server" />--%>

            <div id="filters" class="panel panel-default filter">
                <%--<div class="panel-heading">Filtry</div>--%>
                <div class="panel-body form-inline">
                    <div class="form-group">
                        <label>Przełożony:</label>
                        <asp:DropDownList ID="ddlKierownik" runat="server" DataSourceID="dsKier" DataValueField="Id" DataTextField="Name" AutoPostBack="true"
                            CssClass="form-control" OnSelectedIndexChanged="ddlKierownik_SelectedIndexChanged" OnDataBound="ddlKierownik_DataBound" />
                    </div>
                    <div class="form-group">
                        <label>Pracownik:</label>
                        <asp:DropDownList ID="ddlPracownik" runat="server" DataSourceID="dsPracownik" DataValueField="Id" DataTextField="Name" AutoPostBack="true"
                            CssClass="form-control" OnSelectedIndexChanged="ddlPracownik_SelectedIndexChanged" />
                    </div>
                    <div id="paStatus" runat="server" class="form-group">
                        <label>Status:</label>
                        <asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="dsStatus" DataValueField="Id" AutoPostBack="true" DataTextField="Name" CssClass="form-control" />
                    </div>
                    <div id="paDataOd" runat="server" class="form-group">
                        <label>Data od:</label>
                        <uc1:DateEdit runat="server" ID="deDataOd" AutoPostBack="true" />
                    </div>
                    <div id="paDataDo" runat="server" class="form-group">
                        <label>Data do:</label>
                        <uc1:DateEdit runat="server" ID="deDataDo" AutoPostBack="true" />
                    </div>
                    <div id="paAddRequest" runat="server" class="form-group right">
                        <asp:Button ID="btnAddRequest" runat="server" Text="Dodaj wniosek" OnClick="btnAddRequest_Click" CssClass="btn btn-sm btn-success" />
                    </div>
                </div>
            </div>

            <div class="paTable">
                <asp:GridView ID="gvList" runat="server" DataSourceID="dsList" CssClass="table" GridLines="None" DataKeyNames="Id" OnRowDataBound="gvList_RowDataBound" Visible="false"
                    AutoGenerateColumns="false" AllowPaging="false" AllowSorting="true" EmptyDataRowStyle-CssClass="empty">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectAll_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HiddenField ID="hidStatus" runat="server" Visible="false" Value='<%# Eval("StatusId") %>' />
                                <asp:CheckBox ID="cbSelect" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelect_CheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="NrEwid" HeaderText="Nr Ewid." SortExpression="NrEwid" />
                        <asp:BoundField DataField="Pracownik" HeaderText="Pracownik" SortExpression="Pracownik" />
                        <asp:BoundField DataField="DataWysylki" HeaderText="Data wysyłki" SortExpression="DataWysylki" />
                        <asp:BoundField DataField="Data" HeaderText="Data" SortExpression="Data" />
                        <asp:BoundField DataField="Zmiana" HeaderText="Zmiana" SortExpression="Zmiana" />
                        <asp:BoundField DataField="Nadg50" HeaderText="Nadgodziny 50" SortExpression="Nadg50" />
                        <asp:BoundField DataField="Nadg100" HeaderText="Nadgodziny 100" SortExpression="Nadg100" />
                        <asp:BoundField DataField="Nadg" HeaderText="Nadgodziny" SortExpression="Nadg" />
                        <asp:BoundField DataField="Noc" HeaderText="Noc" SortExpression="Noc" />
                        <asp:BoundField DataField="Rodzaj" HeaderText="Rodzaj" SortExpression="Rodzaj" />
                        <asp:BoundField DataField="Powod" HeaderText="Powód" SortExpression="Powod" />
                        <asp:BoundField DataField="Uwagi" HeaderText="Uwagi" SortExpression="Uwagi" />
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="well well-sm">Brak danych</div>
                    </EmptyDataTemplate>
                </asp:GridView>
                <%--
                <asp:BoundField DataField="WDniu" HeaderText="W dniu" SortExpression="WDniu" />
                --%>    
                <div id="paPracownicy" runat="server" class="paPracownicy">
                    <div class="paSelectAll">
                        <asp:CheckBox ID="gvPracownicySelectAll" runat="server" ToolTip="Zaznacz wszystko" onclick="gvSelectAll(this);" />
                    </div>
                    <asp:GridView ID="gvPracownicy" runat="server" DataSourceID="dsPracownicy" CssClass="table" AllowSorting="true">
                    </asp:GridView>
                    <asp:Button ID="gvPracownicyCmd" runat="server" CssClass="button_postback" Text="Button" OnClick="gvPracownicyCmd_Click" />
                    <asp:HiddenField ID="gvPracownicyCmdPar" runat="server" />
                    <asp:HiddenField ID="gvPracownicySelected" runat="server" />
                </div>
            </div>

            <div runat="server" id="paButtons" class="buttons">
                <div id="divPrint" runat="server" visible="true" class="paButtons">
                    <div class="btn-group">
                        <asp:Button ID="btnPrint" runat="server" Text="Wydruk karty nadgodzin" CssClass="btn btn-default" OnClick="btnPrint_Click" />
                        <asp:Button ID="btnRejectAccepted" runat="server" Text="Odrzuć" CssClass="btn btn-danger" OnClick="btnRejectAccepted_Click" />
                        <%--
                        OnClientClick="javascript:showAjaxProgress();"  T:tylko jak zgasić ?
                        --%>
                    </div>
                </div>

                <div id="divAcc" runat="server" visible="true" class="paButtons">
                    <div class="btn-group">
                        <asp:Button ID="btnAcceptConfirm" runat="server" Text="Zaakceptuj" CssClass="btn btn-success" OnClick="btnAcceptConfirm_Click" />
                        <asp:Button ID="btnAccept" runat="server" CssClass="button_postback" OnClick="btnAccept_Click" />
                        <%--                        <asp:Button ID="btnReject" runat="server" CssClass="button_postback" OnClick="btnReject_Click" />--%>
                        <asp:Button ID="btnRejectConfirm" runat="server" Text="Odrzuć" CssClass="btn btn-danger" OnClick="btnRejectConfirm_Click" />
                    </div>
                </div>

                <%--                <div id="divAdmin" runat="server" visible="false" class="paButtons">
                    <div class="btn-group">
                        <asp:Button ID="btnDeleteConfirm" runat="server" Text="Usuń" CssClass="btn btn-danger" OnClick="btnDeleteConfirm_Click" />
                        <asp:Button ID="btnDelete" runat="server" CssClass="button_postback" OnClick="btnDelete_Click" />
                    </div>
                </div>--%>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPrint" />
        </Triggers>
    </asp:UpdatePanel>

    <uc1:cntNadgodzinyWnioskiModal
        ID="cntNadgodzinyWnioskiModal"
        runat="server"
        OnSent="cntWnioskiNadgodzinyModal_Sent"
        Mode="PostAccept" />

    <uc1:cntModal Title="Powód odrzucenia" runat="server" ID="cntRejectModal">
        <ContentTemplate>
            <div class="form-group">
                <label>Powód odrzucenia</label>
                <asp:TextBox runat="server" ID="tbRejectReason" CssClass="form-control"></asp:TextBox>

            </div>
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btnReject" runat="server" CssClass="btn btn-danger" OnClick="btnReject_Click" Text="Odrzuć"/>
        </FooterTemplate>
    </uc1:cntModal>
</div>

<asp:SqlDataSource ID="dsList" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
set @dataod = ISNULL(@dataod, '20000101')
set @datado = ISNULL(@datado, '20990909')

select 
   nw.Id Id
, p.KadryId NrEwid
, p.Nazwisko + ' ' + p.Imie Pracownik
, convert(varchar(10), nw.Data, 20) Data
, Powod
, nws.Nazwa Status
, z.Nazwa Zmiana
, dbo.ToTimeHMM(Nadg50) Nadg50
, dbo.ToTimeHMM(Nadg100) Nadg100
, dbo.ToTimeHMM(ISNULL(Nadg, Nadg50 + Nadg100)) Nadg

, case nw.RodzajId 
    when 1 then 'Do wypłaty' 
    when 2 then 'Do wybrania' + ISNULL(' w dniu: ' + convert(varchar(10), nw.Data2, 20), '') 
    else convert(varchar, nw.RodzajId) 
  end [Rodzaj]
--, case nw.RodzajId when 1 then null else convert(varchar(10), nw.Data2, 20) end [WDniu]

, Uwagi
, dbo.ToTimeHMM(Noc) Noc
, nw.Status StatusId
, nw.DataUtworzenia DataWysylki
from rcpNadgodzinyWnioski nw
left join Pracownicy p on p.Id = nw.IdPracownika
left join rcpNadgodzinyWnioskiStatus nws on nws.Id = nw.Status
left join Zmiany z on z.Id = nw.IdZmiany
where 
  --Data between isnull(@od, '19990909') and isnull(@do, '20990909') 
--and
(
    (@menuFilter = -99)
 or (@menuFilter = 1 and nw.AutorId = @userId)
 or (@menuFilter = 2 and nw.Status = 1 and (((select dbo.GetRightId(Rights, 98) from Pracownicy where Id = @userId) = 1) or @userId = 0))
 or (@menuFilter = 3 /*and nw.IdAkceptujacego = @userId*/ and nw.Status = 2)
 or (@menuFilter = 4 /*and nw.IdAkceptujacego = @userId*/ and nw.Status = -1)
)
and (@pracFilter is null or p.Id = @pracFilter)
and (@kier is null 
  or @kier  = -99 and p.Id in (select IdPracownika from dbo.fn_GetTreeOkres(@kier, @dataod, @datado, GETDATE()))
  or @kier != -99 and p.Id in (select IdPracownika from Przypisania where IdKierownika = @kier and Od &lt; @datado and @dataod &lt; ISNULL(Do,'20990909') and Status = 1)
    )
and (@statusFilter is null or nw.Status = @statusFilter)
and nw.Data between @dataod and @datado
order by Data desc
">
    <SelectParameters>
        <%--<asp:ControlParameter Name="od" Type="DateTime" ControlID="cntSelectOkres" PropertyName="DateFrom" />
        <asp:ControlParameter Name="do" Type="DateTime" ControlID="cntSelectOkres" PropertyName="DateTo" />--%>
        <asp:ControlParameter Name="userId" Type="Int32" ControlID="hidUserId" PropertyName="Value" />
        <asp:ControlParameter Name="menuFilter" Type="Int32" ControlID="cntSqlTabs" PropertyName="SelectedValue" />
        <asp:ControlParameter Name="pracFilter" Type="Int32" ControlID="ddlPracownik" PropertyName="SelectedValue" />
        <asp:ControlParameter Name="kier" Type="Int32" ControlID="ddlKierownik" PropertyName="SelectedValue" />
        <asp:ControlParameter Name="statusFilter" Type="Int32" ControlID="ddlStatus" PropertyName="SelectedValue" />
        <asp:ControlParameter Name="dataod" Type="DateTime" ControlID="deDataOd" PropertyName="Date" />
        <asp:ControlParameter Name="datado" Type="DateTime" ControlID="deDataDo" PropertyName="Date" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsPracownicy" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    InsertCommand="select convert(varchar,Id) + ';' + Nazwisko + ' ' + Imie + ISNULL(' (' + KadryId + ')','') from Pracownicy where Id in ({0}) order by Nazwisko,Imie,KadryId"
    SelectCommand="
--declare @dataod datetime
--declare @datado datetime
--declare @selected varchar(max)
--declare @kier int 
--declare @pracFilter int 

declare @od datetime
declare @do datetime
set @od = ISNULL(@dataod, '20000101')
set @do = ISNULL(@datado, '20990909')

select 
  P.Id [pracId:-]
, SEL.items [Zaznacz:CB;check|@pracId|Zaznacz]
, P.KadryId [Nr Ewid]
, P.Nazwisko + ' ' + P.Imie [Pracownik:C]
, PS.Grupa [Rodzaj]
, PS.Klasyfikacja [Firma]
, D.Nazwa [Dział]
, S.Nazwa [Stanowisko]
, K.Nazwisko + ' ' + K.Imie [Przełożony]
, P.DataZatr [Data zatrudnienia:D]
, P.DataZwol [Data zwolnienia:D]
--,*
from Pracownicy P
outer apply (select items from dbo.SplitInt(@selected,',') where items = p.Id) SEL
outer apply (select top 1 * from Przypisania where IdPracownika = P.Id and Od &lt;= @do and Status = 1 order by OD desc) R
--outer apply (select top 1 * from PracownicyParametry where IdPracownika = P.Id and Od &lt;= @do order by OD desc) PP
outer apply (select top 1 * from PracownicyStanowiska where IdPracownika = P.Id and Od &lt;= @do order by OD desc) PS
left join Dzialy D on D.Id = PS.IdDzialu
left join Stanowiska S on S.Id = PS.IdStanowiska
left join Pracownicy K on K.Id = R.IdKierownika
where 
    (@pracFilter is null or p.Id = @pracFilter)
and (@kier is null 
  or @kier  = -99 and p.Id in (select IdPracownika from dbo.fn_GetTreeOkres(@kier, @od, @do, GETDATE()))
  or @kier != -99 and p.Id in (select IdPracownika from Przypisania where IdKierownika = @kier and Od &lt;= @do and @od &lt;= ISNULL(Do,'20990909') and Status = 1)
    )
and p.Status != -2
order by P.Nazwisko, P.Imie, P.KadryId    
    ">
    <SelectParameters>
        <%--<asp:ControlParameter Name="od" Type="DateTime" ControlID="cntSelectOkres" PropertyName="DateFrom" />
        <asp:ControlParameter Name="do" Type="DateTime" ControlID="cntSelectOkres" PropertyName="DateTo" />--%>
        <asp:ControlParameter Name="userId" Type="Int32" ControlID="hidUserId" PropertyName="Value" />
        <asp:ControlParameter Name="menuFilter" Type="Int32" ControlID="cntSqlTabs" PropertyName="SelectedValue" />
        <asp:ControlParameter Name="pracFilter" Type="Int32" ControlID="ddlPracownik" PropertyName="SelectedValue" />
        <asp:ControlParameter Name="kier" Type="Int32" ControlID="ddlKierownik" PropertyName="SelectedValue" />
        <asp:ControlParameter Name="statusFilter" Type="Int32" ControlID="ddlStatus" PropertyName="SelectedValue" />
        <asp:ControlParameter Name="dataod" Type="DateTime" ControlID="deDataOd" PropertyName="Date" />
        <asp:ControlParameter Name="datado" Type="DateTime" ControlID="deDataDo" PropertyName="Date" />
        <asp:ControlParameter ControlID="gvPracownicySelected" Name="selected" PropertyName="Value" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsPrint" runat="server" SelectCommand="
select 
  nw.IdPracownika
, p.Nazwisko + ' ' + p.Imie + isnull(' (' + p.KadryId + ')', '') Pracownik
, month(nw.Data) MonthNo
, convert(varchar(10), nw.Data, 20) Data
, case dbo.dow(Data)
    when 0 then 'pn'
    when 1 then 'wt'
    when 2 then 'śr'
    when 3 then 'czw'
    when 4 then 'pt'
    when 5 then 'sb'
    when 6 then 'nd'
    else 'aoe' end WeekDay
, z.Symbol Zmiana
, nw.Od
, nw.Do
, nw.Nadg50 / 3600 Nadg50
, nw.Nadg100 / 3600 Nadg100
, nw.Noc / 3600 Noc
, nw.Powod
, nw.Uwagi
from rcpNadgodzinyWnioski nw
left join Pracownicy p on p.Id = nw.IdPracownika
left join Zmiany z on z.Id = nw.IdZmiany
where nw.Id in (select items from dbo.SplitInt({0}, ',')) 
order by IdPracownika
" />

<asp:SqlDataSource ID="dsAccept" runat="server"
    SelectCommand="
declare @wid int
set @wid = {2}

update rcpNadgodzinyWnioski set IdAkceptujacego = {0}, IdAkceptujacegoZast = {1}, DataAkceptacji = GETDATE(), Status = 2 where Id = @wid

-- aktualizacja zmiany i wymiaru w PP na podstawie wniosku jeżeli ustawione
-- brak zmiany
insert into PlanPracy (IdPracownika, Data, Akceptacja, IdZmianyKorekta, WymiarKorekta)
select nw.IdPracownika, nw.Data, 0, nw.IdZmiany, pr.WymiarCzasu 
from rcpNadgodzinyWnioski nw
left join PlanPracy pp on pp.IdPracownika = nw.IdPracownika and nw.Data = pp.Data
left join PracownicyParametry pr on pr.IdPracownika = pp.IdPracownika and pp.Data between pr.Od and ISNULL(pr.Do, '20990909')
left join Zmiany z on z.Id = nw.IdZmiany
where nw.Id = @wid and pp.Id is null and nw.IdZmiany is not null and z.Id is not null

-- aktuallizacja
update PlanPracy set IdZmianyKorekta = nw.IdZmiany, WymiarKorekta = pr.WymiarCzasu, Akceptacja = 0, Uwagi = LEFT(ISNULL(pp.Uwagi + '; ', ''), 200) + 'zmiana na podstawie wniosku o nadgodziny'   -- cofamy akceptację dnia jeżeli zmieniamy zmianę
--select pp.Data, pp.Akceptacja, pp.Id, pp.IdZmiany, pp.IdZmianyKorekta, pp.WymiarKorekta, nw.IdZmiany, pr.WymiarCzasu, pp.Uwagi 
from PlanPracy pp
inner join rcpNadgodzinyWnioski nw on nw.IdPracownika = pp.IdPracownika and nw.Data = pp.Data and nw.IdZmiany is not null and nw.IdZmiany != 0 --and nw.Id = @wid
left join PracownicyParametry pr on pr.IdPracownika = pp.IdPracownika and pp.Data between pr.Od and ISNULL(pr.Do, '20990909')
where (ISNULL(pp.IdZmianyKorekta, pp.IdZmiany) is null 
    or pp.Akceptacja = 0 and ISNULL(pp.IdZmianyKorekta, pp.IdZmiany) != nw.IdZmiany
    )   -- tylko jak nic nie jest wpisane lub nie jest zaakceptowane
and nw.Id = @wid

/*
declare @pracId int = (select IdPracownika from rcpNadgodzinyWnioski where Id = {2})
declare @data datetime = (select Data from rcpNadgodzinyWnioski where Id = {2})
declare @userId int = {0}
declare @accepted bit

set @accepted = (select Akceptacja from PlanPracy where IdPracownika = @pracId and Data = @data)

if @accepted = 1
    insert into PodzialNadgodzin (IdPracownika, Data, RodzajId, Uwagi, DataWpisu, AutorId, CzasZm, NadgodzinyDzien, NadgodzinyNoc, Nocne, n50, n100, IdWnioskuNadg) 
    select nw.IdPracownika, nw.Data, nw.RodzajId, nw.Uwagi, GETDATE(), @userId, CzasZm, NadgodzinyDzien, NadgodzinyNoc, Nocne
      , case when nw.Nadg50 = pp.n50 then pp.n50
		 when nw.Nadg50 &lt; pp.n50 then nw.Nadg50
		 when nw.Nadg50 &gt; pp.n50 then pp.n50
		else 0 end
      , case when nw.Nadg100 = pp.n100 then pp.n100
		 when nw.Nadg100 &lt; pp.n100 then nw.Nadg100
		 when nw.Nadg100 &gt; pp.n100 then pp.n100
		else 0 end
      , nw.Id
    from rcpNadgodzinyWnioski nw
    left join PlanPracy pp on pp.Data = nw.Data and pp.IdPracownika = nw.IdPracownika
    where nw.Id = {2} and nw.RodzajId = 1
*/

"/>

<asp:SqlDataSource ID="dsReject" runat="server" SelectCommand="update rcpNadgodzinyWnioski set IdAkceptujacego = {0}, IdAkceptujacegoZast = {1}, DataAkceptacji = GETDATE(), Status = -1, Powod = {3} where Id = {2}"
    DeleteCommand="update rcpNadgodzinyWnioski set Status = -1 where Id = {0}" />

<asp:SqlDataSource ID="dsPracownik" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
--declare @kierId int = 11

select null as Id, 'wybierz ...' as Name, 0 as Sort
union all
select P.IdPRacownika Id, P.Nazwisko + ' ' + P.Imie + isnull(' (' + P.KadryId + ')', '') Name, 1 as Sort 
from dbo.fn_GetTree2(case 
    when @kierId is null then @userId
    when @kierId = -99 then @userId
    else @kierId
    end, 0, GETDATE()) P
where 
    (
    @tab  = -88 
 or @tab != -88 and IdPracownika in (select IdPracownika from rcpNadgodzinyWnioski)
    )
order by Sort, Name   
">
    <SelectParameters>
        <asp:ControlParameter Name="userId" Type="Int32" ControlID="hidUserId" PropertyName="Value" DefaultValue="0" />
        <asp:ControlParameter Name="kierId" Type="Int32" ControlID="ddlKierownik" PropertyName="SelectedValue" />
        <asp:ControlParameter Name="tab" Type="Int32" ControlID="cntSqlTabs" PropertyName="SelectedValue" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsKier" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
--declare @kierId int = 11

select null as Id, 'wybierz ...' as Name, 0 as Sort
union all
select Id, Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')', '') Name, 1 as Sort     
from Pracownicy where Id in 
(
select distinct IdKierownika from dbo.fn_GetTree2(@kierId, 0, GETDATE()) 
)
union all
select -99 as Id, 'Wszyscy pracownicy przełożonych' as Name, 2 as Sort
order by Sort, Name   
">
    <SelectParameters>
        <asp:ControlParameter Name="kierId" Type="Int32" ControlID="hidUserId" PropertyName="Value" DefaultValue="0" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsStatus" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select null as Id, 'wybierz ...' as Name, 0 as Sort
union all
select Id, Nazwa Name, 1 as Sort from rcpNadgodzinyWnioskiStatus
order by Sort, Name   
"></asp:SqlDataSource>

<asp:SqlDataSource ID="dsDelete" runat="server" SelectCommand="
delete from rcpNadgodzinyWnioski where Id = {0}
delete from PodzialNadgodzin where IdWnioskuNadg = {0}
" />

<asp:SqlDataSource ID="dsAccError" runat="server" SelectCommand="
declare @ids nvarchar(MAX) = {0}

select 
  p.Nazwisko + ' ' + p.Imie + isnull(' (' + p.KadryId + ')', '') + ' w dniu: ' + convert(varchar(10), nw.Data, 20) Error
from rcpNadgodzinyWnioski nw
left join PlanPracy pp on pp.IdPracownika = nw.IdPracownika and pp.Data = nw.Data and pp.Akceptacja = 1
left join Pracownicy p on p.Id = nw.IdPracownika
where nw.Id in (select items from dbo.SplitInt(@ids, ',')) and (isnull(nw.Nadg, 0) * 36000 &gt; (isnull(pp.n50, 0) + isnull(pp.n100, 0)) and pp.Id is not null)
" />



