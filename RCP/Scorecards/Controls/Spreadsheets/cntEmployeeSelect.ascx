<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntEmployeeSelect.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Spreadsheets.cntEmployeeSelect" %>

<%--<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Always">
    <ContentTemplate>--%>
        <div id="ctEmployeeSelect" runat="server" class="cntEmployeeSelect">
        
            <asp:HiddenField ID="hidObserverId" runat="server" Visible="false" />
            <asp:HiddenField ID="hidEmployeeId" runat="server" Visible="false" />
            <asp:HiddenField ID="hidScorecardTypeId" runat="server" Visible="false" />
            <asp:HiddenField ID="hidDate" runat="server" Visible="false" />
            
            <%--<div class="wrapper">--%>
            <table id="tbSuperiorsSelect" class="tbSuperiorsSelect hiddenTable">
                <tr>
                    <td class="head">
                        <span class="fa fa-user-secret"></span>
                        Przełożeni
                    </td>
                </tr>
                <tr class="search">
                    <td>
                        <asp:TextBox ID="tbSearch" runat="server" CssClass="superiorsSearch" placeholder="Wyszukaj..." />
                        <i class="fa fa-search"></i>
                    </td>
                </tr>
                <asp:Repeater ID="rpSuperiors" runat="server" DataSourceID="dsSuperiors">
                    <ItemTemplate>
                        <tr class="items">
                            <td>
                                <asp:LinkButton ID="lnkSuperior" runat="server" CssClass='<%# GetObserverClass("ziomek", Eval("Id").ToString()) %>' Text='<%# Eval("Name") %>' OnClick="ChangeSuperior" CommandArgument='<%# Eval("Id") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <table id="tbEmployeeSelect" class="tbEmployeeSelect hiddenTable">
                <tr>
                    <td class="head">
                        <span class="fa fa-user"></span>
                        Pracownicy
                    </td>
                </tr>
                <tr class="search">
                    <td>
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="superiorsSearch" placeholder="Wyszukaj..." />
                        <i class="fa fa-search"></i>
                    </td>
                </tr>
                <asp:Repeater ID="rpEmployees" runat="server" DataSourceID="dsEmployees">
                    <ItemTemplate>
                        <tr class="items">
                            <td>
                                <asp:LinkButton ID="lnkEmployee" runat="server" CssClass='<%# GetEmployeeClass("ziomek", Eval("Id").ToString(), Eval("TL").ToString()) %>'  OnClick="ChangeEmployee" CommandArgument='<%# Eval("TeamEmployeeId") %>' >
                                    <%# Eval("Name") %>
                                    <i runat="server" style="float: right;" class="fa fa-lock" visible='<%# Eval("Lock").ToString() == "1" %>'></i>
                                    <br />
                                    <span><%# Eval("Description") %></span>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <table id="tbTeamSelect" class="tbGroupsSelect hiddenTable">
                <tr>
                    <td class="head">
                        <span class="fa fa-group"></span>
                        Zespoły
                    </td>
                </tr>
                <tr class="search">
                    <td>
                        <asp:TextBox ID="TextBox2" runat="server" CssClass="superiorsSearch" placeholder="Wyszukaj..." />
                        <i class="fa fa-search"></i>
                    </td>
                </tr>
                <asp:Repeater ID="rpTeams" runat="server" DataSourceID="dsTeams">
                    <ItemTemplate>
                        <tr class="items">
                            <td>
                                <asp:LinkButton ID="lnkTeam" runat="server" CssClass='<%# GetTeamClass("ziomek", Eval("Id").ToString()) %>' OnClick="ChangeTeam" CommandArgument='<%# Eval("TeamEmployeeId") %>' >
                                    <%# Eval("Name") %> 
                                    <i id="I1" runat="server" style="float: right;" class="fa fa-lock" visible='<%# Eval("Lock").ToString() == "1" %>'></i>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <%--</div>--%>
            <div id="showMine" runat="server" class="menuItem showMine" hid="1" visible="true">
                <span class="text">Mój arkusz</span>
                <span class="fa fa-fighter-jet"></span>
            </div>
            <div id="showSuperiors" runat="server" class="menuItem showSuperiors showHiddenTable" hid="1" data-table="tbSuperiorsSelect" visible="false">
                <span class="text">Przełożeni</span>
                <span class="fa fa-user-secret"></span>
            </div>
            <div id="showList" runat="server" class="menuItem showList showHiddenTable" hid="1" data-table="tbEmployeeSelect">
                <span class="text">Pracownicy</span>
                <span class="fa fa-user"></span>
            </div>
            <div id="showGroups" runat="server" class="menuItem showGroups showHiddenTable" hid="1" data-table="tbGroupsSelect">
                <span class="text">Zespoły</span>
                <span class="fa fa-group"></span>
            </div>
            <div id="unaccept" runat="server" class="menuItem unaccept" hid ="1" visible="false">
                <span class="text">Odakceptuj</span>
                <span class="fa fa-unlock-alt"></span>
            </div>
            <div id="accept" runat="server" class="menuItem accept" hid="1">
                <span class="text">Akceptuj</span>
                <span class="fa fa-lock"></span>
            </div>
            <div id="acceptall" runat="server" class="menuItem acceptall" hid="1">
                <span class="text">Akceptuj wszystkie</span>
                <asp:Image ID="imgTdogg" runat="server" style="margin-top: 11px; height: 27px;" ImageUrl="~/Scorecards/images/T-DOGG.png" />
                <%--<span class="fa fa-lock"></span>--%>
            </div>
            <div id="save" runat="server" class="menuItem save toggleAlarmClass" hid="1">
                <span class="text">Zapisz</span>
                <asp:Image ID="imgCassette" runat="server" ImageUrl="~/Scorecards/images/cassette.png" />
                <%--<span class="fa fa-save"></span>--%>
            </div>
            <asp:Button ID="btnSave" runat="server" CssClass="btnSave" OnClick="Save" ValidationGroup="ivg" CausesValidation="true"></asp:Button>
            <asp:Button ID="bntAccept" runat="server" CssClass="btnAccept" OnClick="Accept" ValidationGroup="ivg" CausesValidation="true" ></asp:Button>
            <asp:Button ID="btnAcceptAll" runat="server" CssClass="btnAcceptAll" OnClick="AcceptAll" ValidationGroup="ivg" CausesValidation="true" ></asp:Button>
            <asp:Button ID="btnMine" runat="server" CssClass="btnMine" OnClick="MySpreadsheet" ></asp:Button>
            <asp:Button ID="btnUnAccept" runat="server" CssClass="btnUnAccept" OnClick="UnAccept" />
            <asp:SqlDataSource ID="dsSuperiors" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                SelectCommand=" 
declare @Typ int
set @Typ = 0

/*declare @kierId int
set @kierId = 0

declare @Data datetime
set @Data = GETDATE()
-----

select IdPracownika as Id, Pracownik + case when Rola != '' then ' - ' + Rola else '' end as Name, 
--convert(varchar, IdPracownika) + '|' + case when Rola = '' then 'poza' when TL = 1 then 'acc' else '' end as Id,   -- css
SortPath
from 
(
select *
,SUBSTRING(
case when ADM	= 1 then ',ADM'   else '' end +	--rScorecardsAdmin
case when TL	= 1 then ',TL'	  else '' end +	--rScorecardsTL   
case when KIER	= 1 then ',KIER'  else '' end +	--rScorecardsKier 
case when ZARZ	= 1 then ',ZARZ'  else '' end +	--rScorecardsPrez 
case when WNACC = 1 then ',WNACC' else '' end	--rScorecardsWnAcc
,2,999) as Rola
from
(
select 
--REPLICATE(' ',Hlevel * -4 * @Typ) + 
REPLICATE('&nbsp;', Hlevel * -4 * @Typ) +
S.Nazwisko + ' ' + S.Imie + ISNULL(' (' + S.KadryId + ')','') as Pracownik,
S.IdPracownika, S.SortPath
,dbo.GetRightId(Rights, 56) as ADM	--rScorecardsAdmin
,dbo.GetRightId(Rights, 57) as TL	--rScorecardsTL   
,dbo.GetRightId(Rights, 58) as KIER	--rScorecardsKier 
,dbo.GetRightId(Rights, 59) as ZARZ	--rScorecardsPrez 
,dbo.GetRightId(Rights, 60) as WNACC--rScorecardsWnAcc
from dbo.fn_GetTree2(@kierId, 1, @Data) S
left join Pracownicy P on P.Id = S.IdPracownika
) D1
where @Typ = -1 or (ADM = 1 or TL = 1 or KIER = 1 or ZARZ = 1 or WNACC = 1)
) D
order by SortPath*/




declare @kierId int
set @kierId = 0
declare @Data datetime
set @Data = GETDATE()
-----

select IdPracownika as Id, Pracownik + case when Rola != '' then ' - ' + Rola else '' end as Name, 
--convert(varchar, IdPracownika) + '|' + case when Rola = '' then 'poza' when TL = 1 then 'acc' else '' end as Id,   -- css
SortPath
from 
(
select *
,SUBSTRING(
case when ADM	= 1 then ',ADM'   else '' end +	--rScorecardsAdmin
case when TLP	= 1 then ',TLP'	  else '' end +	--rScorecardsTL   
case when TLNP	= 1 then ',TLNP'  else '' end +	--rScorecardsTL   
case when KIER	= 1 then ',KIER'  else '' end +	--rScorecardsKier 
case when ZARZ	= 1 then ',ZARZ'  else '' end +	--rScorecardsPrez 
case when WNACC = 1 then ',WNACC' else '' end +	--rScorecardsWnAcc
case when HR    = 1 then ',HR'    else '' end +	--rScorecardsHR
case when CNTR  = 1 then ',CNTR'  else '' end	--rScorecardsControlling
,2,999) as Rola,
case 
when ZARZ = 1 then 1
when KIER = 1 then 2
when TLP  = 1 or TLNP = 1 then 3
when HR   = 1 then 4
when CNTR = 1 then 5
when ADM  = 1 then 6
else 7
end as RolaSort
from
(
select 
--REPLICATE(' ',Hlevel * -4 * @Typ) + 
REPLICATE('&nbsp;', Hlevel * -4 * @Typ) +
S.Nazwisko + ' ' + S.Imie + ISNULL(' (' + S.KadryId + ')','') as Pracownik,
S.IdPracownika, S.SortPath
,dbo.GetRightId(Rights, 56) as ADM	--rScorecardsAdmin
,dbo.GetRightId(Rights, 57) as TLP	--rScorecardsTLProd   
,dbo.GetRightId(Rights, 65) as TLNP	--rScorecardsTLNieprod   
,dbo.GetRightId(Rights, 58) as KIER	--rScorecardsKier 
,dbo.GetRightId(Rights, 59) as ZARZ	--rScorecardsPrez 
,dbo.GetRightId(Rights, 60) as WNACC--rScorecardsWnAcc
,dbo.GetRightId(Rights, 68) as HR   --rScorecardsHR
,dbo.GetRightId(Rights, 73) as CNTR --rScorecardsControlling
from dbo.fn_GetTree2(@kierId, 1, @Data) S
left join Pracownicy P on P.Id = S.IdPracownika
) D1
where @Typ = -1 or (TLP = 1 or TLNP = 1 or KIER = 1 or ZARZ = 1 or WNACC = 1)
) D
--order by Sort,SortPath,Pracownik
order by SortPath,RolaSort,Pracownik

">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="dsEmployees" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                SelectCommand="
select pr.Id, p.IdCommodity, CONVERT(varchar, p.IdCommodity) + ';' + CONVERT(varchar, pr.Id) + ';' + CONVERT(varchar, k.Id) as TeamEmployeeId
, (pr.Nazwisko + ' ' + pr.Imie + isnull(' (' + pr.KadryId + ')', ''))  as Name
, case when k.Id != @ObserverId then 1 else 0 end as Sort
, ta.Nazwa COLLATE SQL_Polish_CP1250_CI_AS + case when k.Id != @ObserverId then (', TL: ' + k.Nazwisko + ' ' + k.Imie) else '' end as Description
, case when k.Id = @ObserverId then 1 else 0 end as TL
, case when prem.Id is null then 0 else 1 end as Lock
from dbo.fn_GetTreeOkres(@ObserverId, dbo.bom(@Date), dbo.eom(@Date), @Date) p
left join scTypyArkuszy ta on p.IdCommodity = ta.Id
left join Pracownicy pr on p.IdPracownika = pr.Id
left join scSlowniki s on ta.Rodzaj = s.Id
left join Pracownicy k on k.Id = p.IdKierownika
left join scWnioski w on w.IdTypuArkuszy = ta.Id and w.Data = @Date and w.IdPracownika = k.Id
left join scPremie prem on prem.IdWniosku = w.Id and prem.IdPracownika = pr.Id and prem._do is null
where s.Nazwa2 = 'ARKI' 
order by TL desc, ta.Nazwa, Sort, pr.Nazwisko, pr.Imie
">
                <SelectParameters>
                    <asp:ControlParameter Name="Date" ControlID="hidDate" PropertyName="Value" Type="String" />
                    <asp:ControlParameter Name="ObserverId" ControlID="hidObserverId" PropertyName="Value" Type="Int32" />
                    
                </SelectParameters>
            </asp:SqlDataSource>
                        <asp:SqlDataSource ID="dsTeams" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                SelectCommand="
select distinct
  IdCommodity as Id
, CONVERT(varchar, IdCommodity) + ';' + CONVERT(varchar, k.Id) as TeamEmployeeId
, ta.Nazwa + case when k.Id != @ObserverId then (', TL: ' + k.Nazwisko + ' ' + k.Imie) else '' end as Name
, case when prem.Id is null then 0 else 1 end as Lock
from dbo.fn_GetTreeOkres(@ObserverId, dbo.bom(@Date), dbo.eom(@Date), @Date) p
left join scTypyArkuszy ta on p.IdCommodity = ta.Id
left join scSlowniki s on ta.Rodzaj = s.Id
left join Pracownicy k on k.Id = p.IdKierownika
left join scWnioski w on w.IdTypuArkuszy = ta.Id and w.Data = @Date and w.IdPracownika = k.Id
left join scPremie prem on prem.IdWniosku = w.Id and prem.IdPracownika = 0 - ta.Id and prem._do is null
where s.Nazwa2 = 'ARKZ'
">
                <SelectParameters>
                    <asp:ControlParameter Name="ObserverId" ControlID="hidObserverId" PropertyName="Value" Type="Int32" />
                    <asp:ControlParameter Name="Date" ControlID="hidDate" PropertyName="Value" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>