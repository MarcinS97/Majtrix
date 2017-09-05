<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWnioskiMenu.ascx.cs" Inherits="HRRcp.Scorecards.Controls.cntWnioskiMenu" %>
<%@ Register src="~/Controls/PodzialLudzi/cntSelectRokMiesiac.ascx" tagname="cntSelectRokMiesiac" tagprefix="uc2" %>

<%--<%@ Register src="cntWnioskiPremiowe.ascx" tagname="cntWnioskiPremiowe" tagprefix="uc1" %>--%>
<%@ Register Src="~/Scorecards/Controls/Requests/cntRequests.ascx" TagPrefix="leet" TagName="Requests" %>
<%@ Register Src="~/Scorecards/Controls/Requests/cntRequestsAdmin.ascx" TagPrefix="leet" TagName="RequestsAdmin" %>



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
                        <asp:MenuItem Text="Wnioski do akceptacji" Value="vTOACC" ></asp:MenuItem>
                        <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Zaakceptowane" Value="vACC" ></asp:MenuItem>
                        <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Do wyjaśnienia" Value="vWYJ" ></asp:MenuItem>
                        <asp:MenuItem Text="&nbsp;&nbsp;&nbsp;Odrzucone" Value="vREJ" ></asp:MenuItem>
                        <asp:MenuItem Text="Wszystkie" Value="vALL" ></asp:MenuItem>
                        <asp:MenuItem Text="Moje wnioski" Value="vMY" ></asp:MenuItem>
                    </Items>
                </asp:Menu>
            </td>                            
            <td class="LeftMenuContent">
                <div class="kryteria">
                    <table class="table0">
                        <tr>
                            <td id="tdAdmin" runat="server" class="left" visible="false">
                                <span>Przełożony:</span>
                                <asp:DropDownList ID="ddlSuperior" runat="server" AutoPostBack="True" 
                                    DataSourceID="dsSuperior" DataTextField="Name" DataValueField="Id" 
                                    onselectedindexchanged="ddlSuperior_SelectedIndexChanged" style="margin-bottom: 8px;">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="dsSuperior" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
                                    SelectCommand="
--declare @observerId int = 0
select 0 as Id, 'Wszyscy pracownicy' as Name, 0 as Sort
union all
select distinct p.Id, p.Nazwisko + ' ' + p.Imie + ISNULL(' (' + p.KadryId + ')', '') as Name, 3 as Sort 
from scWnioski w
left join Pracownicy p on w.IdPracownika = p.Id
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
order by Sort, Name
                                ">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="hidObserverId" Name="observerId" PropertyName="Value" Type="Int32" />
                                    </SelectParameters>
                                </asp:SqlDataSource>   
                                
                                <asp:RadioButtonList ID="rblUpr" runat="server" AutoPostBack="true" Visible="true" RepeatLayout="Flow" RepeatDirection="Horizontal" CellSpacing="15" OnSelectedIndexChanged="rblUpr_SelectedIndexChanged">
                                    <%--<asp:ListItem Value="0" Text="TL " Selected="True" />--%>
                                    <asp:ListItem Value="1" Text="K " Selected="True" />
                                    <asp:ListItem Value="2" Text="Z " />
                                    <asp:ListItem Value="3" Text="HR" />
                                </asp:RadioButtonList>     
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
                    <asp:View ID="vMY" runat="server" OnActivate="Activate1" >
                        <leet:Requests ID="cntWnioskiPremioweMy" runat="server" Mode="0" OnObserverChanged="ObserverChanged" OnChanged="Changed" />
                    </asp:View>
                    <asp:View ID="vTOACC" runat="server" OnActivate="Activate2" >
                        <leet:Requests ID="cntWnioskiPremioweToAcc" runat="server" Mode="1" OnObserverChanged="ObserverChanged" OnChanged="Changed" />
                    </asp:View>
                    <asp:View ID="vACC" runat="server" OnActivate="Activate3" >
                        <leet:Requests ID="cntWnioskiPremioweAcc" runat="server" Mode="2"  OnObserverChanged="ObserverChanged" OnChanged="Changed"/>                    
                    </asp:View>
                    <asp:View ID="vWYJ" runat="server" OnActivate="Activate6" >
                        <leet:Requests ID="cntWnioskiPremioweWyj" runat="server" Mode="3"  OnObserverChanged="ObserverChanged" OnChanged="Changed"/>                    
                    </asp:View>
                    <asp:View ID="vREJ" runat="server" OnActivate="Activate4" >
                        <leet:Requests ID="cntWnioskiPremioweRej" runat="server" Mode="4"  OnObserverChanged="ObserverChanged" OnChanged="Changed"/>                    
                    </asp:View>
                    <asp:View ID="vALL" runat="server" OnActivate="Activate5" >
                        <%--<leet:Requests ID="cntWnioskiPremioweAll" runat="server" Mode="99"  OnObserverChanged="ObserverChanged" OnChanged="Changed"/>--%>
                        <leet:RequestsAdmin ID="cntWnioskiPremioweAdmin" runat="server" OnChanged="Changed" />
                    </asp:View>
                </asp:MultiView>
            </td>
        </tr>
    </table>
    
    <%--<leet:Requests id="Requests" runat="server" Mode="1" />--%>
    
    
</div>