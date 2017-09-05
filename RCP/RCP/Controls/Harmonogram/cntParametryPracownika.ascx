<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntParametryPracownika.ascx.cs" Inherits="HRRcp.RCP.Controls.Harmonogram.cntParametryPracownika" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>



<asp:HiddenField ID="hidEmployeeId" runat="server" Visible="false" />
<asp:HiddenField ID="hidDateFrom" runat="server" Visible="false" />
<asp:HiddenField ID="hidDateTo" runat="server" Visible="false" />

<div id="ctParametryPracownika" runat="server" class="cntParametryPracownika">
    <uc1:cntModal runat="server" ID="cntModal" Title=''>
        <ContentTemplate>
            <div class="form-inline">
                <div class="form-group">
                    <label>Funkcja:</label>
                    <asp:TextBox ID="tbFunk" runat="server" CssClass="form-control" MaxLength="5" Width="100px" Enabled="true" />
                </div>
            </div>
            <div class="form-group">
                <label>Kod:</label>
                <%--<asp:TextBox ID="tbKod" runat="server" CssClass="xform-control" />
                    <asp:AutoCompleteExtender ID="tbKodAutoCompleteExtender" runat="server" TargetControlID="tbKod" 
                        ServicePath="~/main.asmx" ServiceMethod="GetCodes" EnableCaching="true" MinimumPrefixLength="1"></asp:AutoCompleteExtender>--%>

                <asp:UpdatePanel ID="upInner" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>


                        <asp:DropDownList ID="ddlKody" runat="server" DataValueField="Value" AutoPostBack="true"
                            DataTextField="Text" DataSourceID="dsKody" CssClass="form-control ddlKody" OnSelectedIndexChanged="ddlKody_SelectedIndexChanged" />
                        <asp:SqlDataSource ID="dsKody" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                            SelectCommand="select null Value, 'wybierz ...' Text, 0 Sort 
                                        union all 
                                        select Id Value, Kod + isnull(' - ' + Opis, '') Text, 1 Sort 
                                        from rcpKodyPracKody 
                                        where Typ = 0
                                        union all
                                        select -1 Value, 'inny ...' Text, 2 Sort
                                        order by Sort, Text" />

                        <div id="divNewCode" runat="server" class="divNewCode form-group xhidden">
                            <label>Symbol:</label>
                            <asp:TextBox ID="tbNewCode" runat="server" CssClass="form-control" MaxLength="2" />
                            <label>Opis:</label>
                            <asp:TextBox ID="tbNewCodeDescription" runat="server" CssClass="form-control" />

                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <hr />
            <label>Dozwolone dni pracy:</label>
            <div class="form-group well">

                <table class="table">
                    <tr>
                        <asp:Repeater ID="rpSchemeHeader" runat="server">
                            <ItemTemplate>
                                <th>
                                    <%# Eval("ColumnName") %>
                                </th>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tr>

                    <asp:Repeater ID="rpSchemeDataRows" runat="server" OnItemDataBound="rpSchemeDataRows_ItemDataBound" OnDataBinding="rpSchemeDataRows_DataBinding">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hidWeekDay" runat="server" Visible="false" Value='<%# GetValue(Eval("Nazwa"), 0) %>' />
                                    <%# GetValue(Eval("Nazwa"), 1) %>
                                </td>
                                <asp:Repeater ID="rpSchemeDataCells" runat="server">

                                    <ItemTemplate>

                                        <td>
                                            <%--<%# GetValue(Container.DataItem, 1) %>--%>
                                            <asp:HiddenField ID="hidShiftId" runat="server" Visible="false" Value='<%# GetValue(Container.DataItem, 1) %>' />
                                            <asp:CheckBox ID="cbCheck" runat="server" Checked='<%# GetValue(Container.DataItem, 0) == "0" %>' />

<%--                                            GetValue(Container.DataItem, 0) %>--%>
                                        </td>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>



                </table>

                <%--             <label>Dni dozwolone</label>
                <asp:Repeater ID="rpDays" runat="server">
                    <ItemTemplate>
                        <div class="checkbox">
                            <label>
                                <asp:CheckBox ID="cbDay" runat="server" Checked='<%# Eval("Checked") %>' />
                                <%# Eval("Name") %>
                            </label>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>--%>
            </div>
            <hr />
            <div class="form-group">
                <label>Typ okresu rozliczeniowego</label>
                <asp:DropDownList ID="ddlTypyOkresow" runat="server" DataValueField="Value" DataTextField="Text" DataSourceID="dsTypyOkresow" CssClass="form-control" />
                <asp:SqlDataSource ID="dsTypyOkresow" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="select null Value, 'wybierz ...' Text, 0 Sort union all select Id Value, Nazwa Text, 1 Sort from rcpOkresyRozliczenioweTypy where Aktywny = 1" />
            </div>

        </ContentTemplate>
        <FooterTemplate>

            <asp:Button ID="bntSave" runat="server" Text="Zapisz" CssClass="btn btn-sm btn-success" OnClick="bntSave_Click" />
        </FooterTemplate>
    </uc1:cntModal>
</div>


<asp:SqlDataSource ID="dsParametry" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
declare @nadzien datetime = '{2}' --getdate()

select 
  p.Id Id
, p.Nazwisko + ' ' + p.Imie Name
, ps.Rodzaj Func
, kp.Kod Code
--, sp.Schemat AllowedDays
, pto.IdTypuOkresu PeriodType
from Pracownicy p
left join PracownicyStanowiska ps on ps.IdPracownika = p.Id and @nadzien between ps.Od and ISNULL(ps.Do, '20990909')
left join rcpKodyPrac kp on kp.IdPracownika = p.Id and @nadzien between kp.Od and ISNULL(ps.Do, '20990909')
left join rcpKodyPracKody kpk on kpk.Id = kp.Kod
--left join rcpSchematPracy sp on @nadzien between sp.DataOd and isnull(sp.DataDo, '20990909') and sp.IdPracownika = p.Id
left join rcpPracownicyTypyOkresow pto on @nadzien between pto.DataOd and isnull(pto.DataDo, '20990909') and pto.IdPracownika = p.Id

where p.Id = {0}
    
"></asp:SqlDataSource>


<%--<asp:SqlDataSource ID="dsSaveAllowedDays" runat="server" SelectCommand="
declare @allowedDays nvarchar(7) = {1} 
declare @pracId int = {0}

declare @nadzien datetime = getdate()

update rcpSchematPracy set
Schemat = @allowedDays
from rcpSchematPracy sp
where @nadzien between sp.DataOd and isnull(sp.DataDo, '20990909') and sp.IdPracownika = @pracId

insert into rcpSchematPracy (IdPracownika, DataOd, DataDo, Schemat)
select @pracId, GETDATE(), null, @allowedDays
from (select 1 x) x
left join rcpSchematPracy sp on @nadzien between sp.DataOd and isnull(sp.DataDo, '20990909') and sp.IdPracownika = @pracId
where sp.Id is null
" />--%>

<asp:SqlDataSource ID="dsSaveCode" runat="server" SelectCommand="
declare @pracId int = {0}
declare @code int = {1} 

declare @nadzien datetime = '{2}'--getdate()

update rcpKodyPrac set
Kod = @code
from rcpKodyPrac kp
where @nadzien between kp.Od and isnull(kp.Do, '20990909') and kp.IdPracownika = @pracId

insert into rcpKodyPrac (IdPracownika, Kod, Od, Do)
select @pracId, @code, /*GETDATE()*/@nadzien, null
from (select 1 x) x
left join rcpKodyPrac kp on @nadzien between kp.Od and isnull(kp.Do, '20990909') and kp.IdPracownika = @pracId
where kp.Id is null
" />


<asp:SqlDataSource ID="dsSavePeriodType" runat="server" SelectCommand="
declare @pracId int = {0}
declare @periodType int = {1} 

declare @nadzien datetime = '{2}' --getdate()

--update rcpPracownicyTypyOkresow set
--IdTypuOkresu = @periodType
--from rcpPracownicyTypyOkresow pto
--where @nadzien between pto.DataOd and isnull(pto.DataDo, '20990909') and pto.IdPracownika = @pracId

insert into rcpPracownicyTypyOkresow (IdPracownika, IdTypuOkresu, DataOd, DataDo)
select @pracId, @periodType, /*GETDATE()*/@nadzien, null
--from (select 1 x) x
--left join rcpPracownicyTypyOkresow pto on @nadzien between pto.DataOd and isnull(pto.DataDo, '20990909') and pto.IdPracownika = @pracId
--where pto.Id is null
" />

<asp:SqlDataSource ID="dsSaveFunc" runat="server" SelectCommand="
declare @pracId int = {0}
declare @func nvarchar(10) = {1} 

declare @nadzien datetime = '{2}' --getdate()

update PracownicyStanowiska set
Rodzaj = @func
from PracownicyStanowiska ps
where @nadzien between ps.Od and isnull(ps.Do, '20990909') and ps.IdPracownika = @pracId

/*
    insert into rcpPracownicyTypyOkresow (IdPracownika, IdTypuOkresu, DataOd, DataDo)
    select @pracId, @periodType, GETDATE(), null
    from (select 1 x) x
    left join rcpPracownicyTypyOkresow pto on @nadzien between pto.Od and isnull(pto.Do, '20990909') and pto.IdPracownika = @pracId
where pto.Id is null
*/
" />

<asp:SqlDataSource ID="dsCodes" runat="server" SelectCommand="select distinct Kod from rcpKodyPracKody order by Kod" />
<asp:SqlDataSource ID="dsNewCode" runat="server" SelectCommand="insert into rcpKodyPracKody (Kod, Opis, Typ) values ({0}, {1}, 0)" />
<asp:SqlDataSource ID="dsDeleteCode" runat="server" SelectCommand="delete from rcpKodyPrac where IdPracownika = {0}" />


<asp:SqlDataSource ID="dsEmployeeScheme" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
--declare @pracId int = 1529

declare @colsH nvarchar(MAX)
declare @colsD nvarchar(MAX)
declare @stmt nvarchar(MAX)

select
  @colsH = ISNULL(@colsH + ',', '') + 'ISNULL([' + CONVERT(varchar, z.Id) + '], ''0|' + CONVERT(varchar, z.Id) + ''')' + '[' + z.Symbol + ']'
, @colsD = ISNULL(@colsD + ',', '') + '[' + CONVERT(varchar, z.Id) + ']'
from Zmiany z
where z.NazwaEN in ('Z1', 'Z2', 'Z3')

set @stmt = '
declare @pracId int = ' + CONVERT(varchar, @pracId) + '

select
  
  CONVERT(varchar, d.Dzien)
+ ''|'' + d.Nazwa Nazwa
, ' + @colsH + '
from
(
    select 0 Dzien, ''Niedziela'' Nazwa
    union
    select 1, ''Poniedziałek''
    union
    select 2, ''Wtorek''
    union
    select 3, ''Środa''
    union
    select 4, ''Czwartek''
    union
    select 5, ''Piątek''
    union
    select 6, ''Sobota''

) d
left join
(
    select * from
    (
        select
          sp.DzienTygodnia
        , sp.IdZmiany
        , ''1|'' + CONVERT(varchar, sp.IdZmiany) x
        from rcpSchematyPracy sp
        where sp.IdPracownika = @pracId
    ) sp
    pivot
    (
        MAX(sp.x) for sp.IdZmiany in (' + @colsD + ')
    ) pv
) v on v.DzienTygodnia = d.Dzien
'

exec sp_executesql @stmt 
" >
    <SelectParameters>
        <asp:ControlParameter Name="pracId" Type="Int32" ControlID="hidEmployeeId" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsEmployeeSchemeDeleteOrInsert" runat="server" 
    SelectCommand=
"
    
" />

<asp:SqlDataSource ID="dsGetEmployeeScheme" runat="server" SelectCommand="select * from rcpSchematyPracy where IdPracownika = {0} and IdZmiany = {1} and DzienTygodnia = {2}" />
<asp:SqlDataSource ID="dsRemoveEmployeeScheme" runat="server" SelectCommand="delete from rcpSchematyPracy where Id = {0}" />
<asp:SqlDataSource ID="dsAddEmployeeScheme" runat="server" SelectCommand="insert into rcpSchematyPracy (IdPracownika, IdZmiany, DzienTygodnia) values ({0}, {1}, {2})" />