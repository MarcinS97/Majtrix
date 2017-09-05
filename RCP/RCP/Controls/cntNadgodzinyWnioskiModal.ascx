<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntNadgodzinyWnioskiModal.ascx.cs" Inherits="HRRcp.RCP.Controls.cntNadgodzinyWnioskiModal" %>

<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/Controls/TimeEdit.ascx" TagName="TimeEdit" TagPrefix="uc1" %>



<asp:HiddenField ID="hidUserId" runat="server" Visible="false" />

<uc1:cntModal runat="server" ID="cntModal" Title="Utwórz wniosek" CssClass="nadgWnModal">
    <ContentTemplate>
        <div class="form-group">
<%--            <label>Pracownik:</label>--%>
            <%--<asp:DropDownList ID="ddlPracownik" runat="server" DataSourceID="dsPracownik" DataValueField="Id"
                DataTextField="Name" CssClass="form-control" OnSelectedIndexChanged="ddlPracownik_SelectedIndexChanged" />--%>

            <table class="table table-hover emp-Wnioski">
                <tr>
                    <th colspan="2">Pracownik</th>
                </tr>
                <asp:Repeater ID="rpEmployees" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td class="check">
                                <asp:CheckBox ID="cbChecked" runat="server" Checked='<%# Eval("Checked") %>'/>
                                <asp:HiddenField ID="hidId" runat="server" Visible="false" value='<%# Eval("Id") %>'/>
                            </td>
                            <td class="name"><%# Eval("Name") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <hr />
        <div class="row form-group form-inline">
            <div class="col-md-12">
                <label class="label1">Data:</label>
                <uc1:DateEdit runat="server" ID="deDate" OnDateChanged="deDate_DateChanged" ValidationGroup="vgSave" />
            </div>
        </div>
        <div id="divZmiana" runat="server" class="row form-group form-inline" visible="false">
            <div class="col-md-12">
                <label class="label1">Zmiana:</label>
                <asp:DropDownList ID="ddlZmiana" runat="server" DataSourceID="dsZmiany" DataValueField="Id" DataTextField="Nazwa" CssClass="form-control zmiana" Enabled="true" />
            </div>
        </div>
        <div class="row form-group form-inline">
            <div class="col-sm-7 form-inline" runat="server" visible="false">
                <label>Od:</label>
                <asp:DropDownList ID="ddlOd" runat="server" CssClass="form-control" />
                <label>Do:</label>
                <asp:DropDownList ID="ddlDo" runat="server" CssClass="form-control" />
            </div>
            <div id="divNadgPre" class="col-sm-5" runat="server">
                <label>Nadgodziny:</label>
                <asp:TextBox ID="tbNadg" runat="server" CssClass="form-control" Width="80px" />
            </div>
        </div>
        <div id="divNadgPost" runat="server" class="form-group form-inline">
            <label class="label1">Nadg. 50%:</label>
            <uc1:TimeEdit ID="tbNadg50" runat="server" Right="true" InLineCount="4" />

            <label>Nadg. 100%:</label>
            <uc1:TimeEdit ID="tbNadg100" runat="server" Right="true" InLineCount="4" />

            <label>Godz. nocne:</label>
            <uc1:TimeEdit ID="tbNoc" runat="server" Right="true" InLineCount="4" />
        </div>
        <hr />
        <div id="divType" runat="server" >
            <asp:RadioButtonList ID="rblType" runat="server" RepeatLayout="Flow" CssClass="radio" Style="margin-left: 32px;" AutoPostBack="false" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                <asp:ListItem Text="Do wypłaty" Value="1" Selected="True"/>
                <asp:ListItem Text="Do wybrania w dniu" Value="2" />
            </asp:RadioButtonList>
            <uc1:DateEdit ID="deCDay" runat="server" Visible="false" />
        </div>
        <hr />
        <div class="form-group">
            <label>Powód nadgodzin:</label>
            <asp:TextBox ID="tbPowod" runat="server" CssClass="form-control" TextMode="MultiLine" />
        </div>
        <div class="form-group">
            <label>Uwagi:</label>
            <asp:TextBox ID="tbUwagi" runat="server" CssClass="form-control" TextMode="MultiLine" />
        </div>
        <asp:Button ID="btnSendRequest" runat="server" CssClass="button_postback" OnClick="btnSendRequest_Click" ValidationGroup="vgSave" />
    </ContentTemplate>
    <FooterTemplate>
        <asp:Button ID="btnSendRequestConfirm" runat="server" Text="Wyślij do akceptacji" CssClass="btn btn-sm btn-success" OnClick="btnSendRequestConfirm_Click" ValidationGroup="vgSave" />
    </FooterTemplate>
</uc1:cntModal>




<asp:SqlDataSource ID="dsPracownik" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
--declare @kierId int = 11

select null as Id, 'wybierz ...' as Name, 0 as Sort
union all
select IdPracownika Id, Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')', '') Name, 1 as Sort from dbo.fn_GetTree2(@kierId, 0, GETDATE()) 
order by Sort, Name   
">
    <SelectParameters>
        <asp:ControlParameter Name="kierId" Type="Int32" ControlID="hidUserId" PropertyName="Value" DefaultValue="0" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="x_dsZmiana" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select null as Id, 'wybierz ...' as Name, 0 as Sort
union all
select Id, Symbol + ' - ' + Nazwa Name, 1 Sort from Zmiany
order by Sort, Name" />

<asp:SqlDataSource ID="dsZmiany" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
declare @korzm int
declare @korwym int
declare @planzm int
declare @planwym int
declare @selzm int
declare @selwym int -- tylko jeden rekord
declare @wymiar int -- wymiar pracownika

--set @wymiar = 28800
--set @planzm = null
--set @planwym = 28800
--set @korzm = 3
--set @korwym = 3000 
--set @selzm = null
--set @selwym = 19000 
/*
set @planzm  = {0}
set @planwym = {1}
set @korzm   = {2}
set @korwym  = {3} 
set @selzm   = {4}
set @selwym  = {5} 
set @wymiar  = {6}
*/

select null Id, 'nie zmieniaj' Nazwa, 1 Sort, null Kolejnosc, 0 Wymiar
union all
select Id, Nazwa1 + case when @planzm is null and IdA = -1 or ISNULL(@planzm, -1) = IdA and ISNULL(@planwym, 0) = Wymiar then ' - PLAN' else '' end Nazwa, 2, Kolejnosc, Wymiar
from 
( 	
select 'brak zmiany' as Nazwa1, '-1|0' Id, -1 IdA, null Kolor, 4 as Sort, 0 Wymiar, 0 as Kolejnosc
    union 
select Z.Symbol + ISNULL(' - ' + Z.Nazwa, '') 
--   + case when Z.Od != Z.Do then ' ' + LEFT(convert(varchar, Z.Od, 8),5) + ' - ' + LEFT(convert(varchar, Z.Do, 8),5) else '' end
  + case when Z.Od != Z.Do then ' ' + LEFT(convert(varchar, Z.Od, 8),5) + ' - ' + LEFT(convert(varchar, DATEADD(SECOND, Z4.zmCzas, Z.Od), 8),5) else '' end
--  + ISNULL(' (' + REPLACE(dbo.ToTimeHMM(T.items), ':00', '') + ' h)','')

  + ' (' + REPLACE(dbo.ToTimeHMM(Z4.zmCzas), ':00', '') + 'h)'

  + case when Z.HideZgoda = 0 and Z.ZgodaNadg = 1 then ' - ZGODA NADG.' else '' end
    as Nazwa1
  , convert(varchar, Z.Id) + '|' + CONVERT(varchar, ISNULL(T.items, Z3.zmCzas)) Id
  , Z.Id IdA
  , Z.Kolor
  , 2 Sort
  , T.items Wymiar
  , Z.Kolejnosc 
  --, Z.InneCzasy
  --, T.items
from Zmiany Z
outer apply (select DATEDIFF(SECOND, dbo.gettime(Z.Od), dbo.gettime(Z.Do)) zmCzas) Z2
outer apply (select case when Z2.zmCzas &lt; 0 then Z2.zmCzas + 86400 else Z2.zmCzas end /*+ ISNULL(Z.Par1, 0)*/ zmCzas) Z3   --zmniejszenie piątek
outer apply (
	select items from dbo.SplitInt(Z.InneCzasy, ';') where @wymiar != 28800
	union
	select @korwym from (select 1 x) x where @korzm = Z.Id
	union
	select @planwym from (select 1 x) x where @planzm = Z.Id
	union
	select @selwym from (select 1 x) x where @selzm = Z.Id
	union
	select Z3.zmCzas from (select 1 x) x where Z3.zmCzas != 0
) T
outer apply (select ISNULL(T.items, Z3.zmCzas) + case when Z.Typ = 5 then ISNULL(Z.Par1, 0) else 0 end zmCzas) Z4
where Z.Visible=1 and Z.Widoczna=1
) D
where (@selzm is null 
	or @selzm = IdA and (IdA = -1 or @selwym = Wymiar)
	)  
order by Sort, Kolejnosc, Wymiar desc, Nazwa
    ">
</asp:SqlDataSource>




<asp:SqlDataSource ID="dsData" runat="server"
    SelectCommand="
declare @pracId int = {0}
declare @data datetime = {1}

select * 
from PlanPracy 
where 
  IdPracownika = @pracId 
and Data = @data

    
" />
<asp:SqlDataSource ID="dsSend" runat="server"
    SelectCommand="
declare @data datetime = {0}
declare @pracId int = {1}
declare @idZmiany int = {2}
declare @nadg50 float = {3}
declare @nadg100 float = {4}
declare @noc float = {5}
declare @powod nvarchar(500) = {6}
declare @uwagi nvarchar(1000) = {7}
declare @dataUtw datetime = GETDATE()
declare @autorId int = {8}
declare @status int = 1
declare @nadg int = {9}
declare @rodzaj int = {10}
declare @date2 datetime = {11}

insert into rcpNadgodzinyWnioski (Data, IdZmiany, IdPracownika, Nadg50, Nadg100, Noc, Powod, Uwagi, DataUtworzenia, AutorId, Status, Nadg, RodzajId, Data2)
values (@data, @idZmiany, @pracId, @nadg50, @nadg100, @noc, @powod, @uwagi, @dataUtw, @autorId, @status, @nadg, @rodzaj, @date2)

select SCOPE_IDENTITY()
" />

