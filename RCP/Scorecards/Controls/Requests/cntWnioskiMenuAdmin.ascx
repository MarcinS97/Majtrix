<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWnioskiMenuAdmin.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Requests.cntWnioskiMenuAdmin" %>
<%@ Register src="~/Controls/PodzialLudzi/cntSelectRokMiesiac.ascx" tagname="cntSelectRokMiesiac" tagprefix="uc2" %>

<%--<%@ Register src="cntWnioskiPremiowe.ascx" tagname="cntWnioskiPremiowe" tagprefix="uc1" %>--%>
<%@ Register Src="~/Scorecards/Controls/Requests/cntRequestsAdmin2.ascx" TagPrefix="leet" TagName="Requests" %>
<%@ Register Src="~/Scorecards/Controls/Requests/cntRequestsAdmin.ascx" TagPrefix="leet" TagName="RequestsAdmin" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>


<div id="paWnioskiMenu" runat="server" class="cntWnioskiMenu">
    <asp:HiddenField ID="hidObserverId" runat="server" Visible="false" />

    <table class="tabsContent" >
        <tr>
            <td class="LeftMenu">
                <asp:Menu ID="mnLeft" runat="server" StaticDisplayLevels="1" onmenuitemclick="mnLeft_MenuItemClick" >
                    <StaticMenuStyle CssClass="menu" />
                    <StaticSelectedStyle CssClass="selected" />
                    <StaticMenuItemStyle CssClass="item" />
                    <StaticHoverStyle CssClass="hover" />
                    <Items>
                        <asp:MenuItem Text="Wszystkie wnioski" Value="vALL" ></asp:MenuItem>
                        <asp:MenuItem Text="W przygotowaniu" Value="vNEW" ></asp:MenuItem>
                        <asp:MenuItem Text="Do akceptacji Kierownika" Value="vACCKIER" ></asp:MenuItem>
                        <asp:MenuItem Text="Cofnięte przez Kierownika" Value="vACCDKIER" ></asp:MenuItem>
                        <asp:MenuItem Text="Do akceptacji Zarządu" Value="vACCZARZ" ></asp:MenuItem>
                        <asp:MenuItem Text="Cofnięte przez Zarząd" Value="vACCDZARZ" ></asp:MenuItem>
                        <asp:MenuItem Text="Zaakceptowane - HR" Value="vHR" ></asp:MenuItem>
                        <asp:MenuItem Text="Odrzucone" Value="vREJ" ></asp:MenuItem>
                        <%--<asp:MenuItem Text="Wszystkie" Value="vALL"  ></asp:MenuItem>--%>
                    </Items>
                </asp:Menu>
            </td>                            
            <td class="LeftMenuContent">
                <div class="kryteria">
                    <table class="table0">
                        <tr>
                            <td id="tdAdmin" runat="server" class="left" >
                                <span>Team Leader:</span>
                                <asp:DropDownList ID="ddlSuperior" runat="server" AutoPostBack="True" DataSourceID="dsSuperior" DataTextField="Name" DataValueField="Id" 
                                    onselectedindexchanged="ddlSuperior_SelectedIndexChanged" >
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="dsSuperior" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                                    SelectCommand="
/*--declare @observerId int = 0
select 0 as Id, 'wybierz ...' as Name, 0 as Sort
union all
select distinct p.Id, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') as Name, 3 as Sort 
from scWnioski w
left join Pracownicy p on w.IdPracownika = p.Id
where w.IdPracownika &gt; 0
union
select Id/*Pracownika*/ as Id,
case when Status = -1 then '*' else '' end +
Nazwisko + ' ' + Imie + ISNULL(' (' + KadryId + ')','') as Name,
case 
when Status = -1 then 5 
when Id/*Pracownika*/ = @observerId then 1
else 4 end as Sort
from Pracownicy
--from dbo.fn_GetTree2(0) 
where Kierownik = 1 and Status != -2
order by Sort, Name*/

select prz.IdPracownika as a, prz.IdKierownika as b, prz2.IdKierownika as c
into #ccc
from Przypisania prz
left join scWnioski w on prz.IdPracownika = w.IdPracownika and w.Data between prz.Od and ISNULL(prz.Do, '20990909')
left join Przypisania prz2 on prz.IdKierownika = prz2.IdPracownika
where w.IdPracownika is not null
select 0 as Id, 'Wszyscy przełożeni' as Name, 0 as Sort
union all
select p.Id, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') as Name, 3 as Sort  from Pracownicy p where p.Id in (select a from #ccc) or p.Id in (select b from #ccc) or p.Id in (select c from #ccc)
order by Sort, Name
drop table #ccc
                                ">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="hidObserverId" Name="observerId" PropertyName="Value" Type="Int32" />
                                    </SelectParameters>
                                </asp:SqlDataSource>                                                                   
<%--                                    
                                <span style="margin-left: 16px;">Data:</span>
                                <uc1:DateEdit ID="deDate" runat="server" AutoPostBack="true" OnDateChanged="ddlSuperior_SelectedIndexChanged" />
--%>
                                <span style="margin-left: 16px;">Miesiąc wypracowania premii:</span>
                                <asp:DropDownList ID="ddlMiesiac" runat="server" AutoPostBack="True" DataSourceID="dsMies" DataTextField="Data" DataValueField="Id" onselectedindexchanged="ddlSuperior_SelectedIndexChanged" >
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="dsMies" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                                    SelectCommand="
select 'wybierz ...' as Data, null as Id, 1 as Sort
union all
select distinct convert(varchar(7), Data, 20) as Data, convert(varchar(7), Data, 20) + '-01' as Id, 2 as Sort 
from scWnioski 
order by Sort, Data desc                                
                                    ">
                                </asp:SqlDataSource>   
                                    
                                    
<%--                                <asp:RadioButtonList ID="rblUpr" runat="server" AutoPostBack="true" Visible="true" RepeatLayout="Flow" RepeatDirection="Horizontal" CellSpacing="15" OnSelectedIndexChanged="rblUpr_SelectedIndexChanged">
                                    <asp:ListItem Value="1" Text="K " Selected="True" />
                                    <asp:ListItem Value="2" Text="Z " />
                                    <asp:ListItem Value="3" Text="HR" />
                                </asp:RadioButtonList>     --%>
                                                                
                                <asp:Button ID="btClear" runat="server" Text="Czyść" CssClass="button100" onclick="btClear_Click" style="margin-left: 16px;"/>
                            </td>
                            <td>
                            
                            </td>
<%--                            <td class="right">
                                <uc2:cntSelectRokMiesiac runat="server" ID="cntSelectRokMiesiac" canBackAll="true" canNextAll="true"
                                                        OnBackAll="cntSelectRokMiesiac_BackAll"
                                                        OnNextAll="cntSelectRokMiesiac_NextAll"
                                                        OnValueChanged="cntSelectRokMiesiac_ValueChanged" />                                    
                            </td>--%>
                        </tr>
                    </table>
                </div>
                <asp:MultiView ID="mvWnioski" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vALL" runat="server" OnActivate="Activate1" >
                        <leet:Requests ID="cntWnioskiPremioweAll" runat="server" Mode="0" Upr="0" OnObserverChanged="ObserverChanged" OnChanged="Changed" />
                    </asp:View>
                    <asp:View ID="vNEW" runat="server" OnActivate="Activate2" >
                        <leet:Requests ID="cntWnioskiPremioweNew" runat="server" Mode="1" Upr="0" OnObserverChanged="ObserverChanged" OnChanged="Changed" />
                    </asp:View>
                    <asp:View ID="vACCKIER" runat="server" OnActivate="Activate3" >
                        <leet:Requests ID="cntWnioskiPremioweAccKier" runat="server" Mode="2" Upr="1" OnObserverChanged="ObserverChanged" OnChanged="Changed"/>                    
                    </asp:View>
                    <asp:View ID="vACCDKIER" runat="server" OnActivate="Activate4" >
                        <leet:Requests ID="cntWnioskiPremioweAccdKier" runat="server" Mode="3" Upr="1" OnObserverChanged="ObserverChanged" OnChanged="Changed"/>                    
                    </asp:View>
                    <asp:View ID="vACCZARZ" runat="server" OnActivate="Activate5" >
                        <leet:Requests ID="cntWnioskiPremioweAccZarz" runat="server" Mode="4" Upr="2" OnObserverChanged="ObserverChanged" OnChanged="Changed"/>                    
                    </asp:View>
                    <asp:View ID="vACCDZARZ" runat="server" OnActivate="Activate6" >
                        <leet:Requests ID="cntWnioskiPremioweAccdZarz" runat="server" Mode="5" Upr="2" OnObserverChanged="ObserverChanged" OnChanged="Changed"/>                    
                    </asp:View>
                    <asp:View ID="vHR" runat="server" OnActivate="Activate7" >
                        <leet:Requests ID="cntWnioskiPremioweHR" runat="server" Mode="6" Upr="3" OnObserverChanged="ObserverChanged" OnChanged="Changed"/>                    
                    </asp:View>
                    <asp:View ID="vREJ" runat="server" OnActivate="Activate8" >
                        <leet:Requests ID="cntWnioskiPremioweRej" runat="server" Mode="7" Upr="0"  OnObserverChanged="ObserverChanged" OnChanged="Changed"/>
                        <%--<leet:RequestsAdmin ID="cntWnioskiPremioweAdmin" runat="server" OnChanged="Changed" />--%>
                    </asp:View>
                </asp:MultiView>
            </td>
        </tr>
    </table>
    
    
    
</div>