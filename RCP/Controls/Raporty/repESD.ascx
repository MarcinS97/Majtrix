<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="repESD.ascx.cs" Inherits="HRRcp.Controls.Raporty.repESD" %>
<%@ Register src="~/Controls/Reports/cntReport2.ascx" tagname="cntReport" tagprefix="uc1" %>
<%@ Register src="~/Controls/SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc1" %>
<%@ Register src="../Reports/cntReportHeader.ascx" tagname="cntReportHeader" tagprefix="uc2" %>

<%@ Register src="../DateEdit.ascx" tagname="DateEdit" tagprefix="uc3" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    </ContentTemplate>
</asp:UpdatePanel>
    
        <table class="okres_navigator printoff">
            <tr>
                <td class="colleft">
<%--                    <uc1:SelectOkres ID="cntSelectOkres" OnOkresChanged="cntSelectOkres_Changed" runat="server" StoreInSession="true" />
--%>
                    Data: 
                    <uc3:DateEdit ID="deData" runat="server" AutoPostBack="true" OnDateChanged="deData_Changed"/>
                </td>
            </tr>
        </table>

        <div class="divider_ppacc printoff"></div>

        <uc2:cntReportHeader ID="cntReportHeader1" runat="server" 
            Caption="Testy ESD"
        />

        <asp:Menu ID="tabRaporty" CssClass="printoff" runat="server" Orientation="Horizontal" 
            onmenuitemclick="tabRaporty_MenuItemClick" >
            <StaticMenuStyle CssClass="tabsStrip" />
            <StaticMenuItemStyle CssClass="tabItem" />
            <StaticSelectedStyle CssClass="tabSelected" />
            <StaticHoverStyle CssClass="tabHover" />
            <Items>
                <asp:MenuItem Text="Pracownicy"                 Value="vPracownicy" Selected="True" ></asp:MenuItem>

<%--
                <asp:MenuItem Text="Plan niepoprawny"           Value="vError" ></asp:MenuItem>                
                <asp:MenuItem Text="Plan poprawny"              Value="vOK" ></asp:MenuItem>                
                <asp:MenuItem Text="Plan - wszyscy"             Value="vAll" ></asp:MenuItem>                
                <asp:MenuItem Text="Kierownicy do pogonienia"    Value="vPogonic" ></asp:MenuItem>                
               
                <asp:MenuItem Text="Plan - realizacja"          Value="vRealizacja" ></asp:MenuItem>
                <asp:MenuItem Text="Dni - Pracownicy"       Value="vDniPrac" ></asp:MenuItem>
                <asp:MenuItem Text="Szczegóły"              Value="vSzczegoly" ></asp:MenuItem>
                <asp:MenuItem Text="Duplikaty ROGER'ów"     Value="vDuplikatyROGER" ></asp:MenuItem>
--%>                
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

        <div class="tabsContentLine" style="border-collapse:collapse; background-color:#FFF;">
            <asp:MultiView ID="mvRaporty" runat="server" ActiveViewIndex="0">
                <%-----------------------------------%>
                <asp:View ID="vPracownicy" runat="server" >
                <%-----------------------------------%>
                    <uc1:cntReport ID="cntReport1" runat="server" 
                        CssClass="RepESD"
                        HeaderVisible="false"
                        SQL="
declare @od datetime
declare @do datetime
set @od = '@SQL1'
set @do = DATEADD(D, 1, @od)

declare @str int
select top 1 @str = Id from Strefy where Typ = 1

select 
--convert(varchar(10),dbo.getdate(R.Czas),120) [Data:DC], 
R.Czas [Czas:DT], 
R.ECUserId, 
R.ECReaderId, 
R.ECCode, 
--R.ECUniqueId,

P.KadryId [NrEw], 
P.Nazwisko + ' ' + P.Imie [Pracownik], 
--P.Id as IdPracownika,

case R.ECCode when 1 then 'ok' when 2 then 'odmowa' else convert(varchar,R.ECCode) end [Status],  --1 ok, 2 - odmowa
RR.Name, 
RR.Zone,
Y.Nazwisko + ' ' + Y.Imie [Kierownik], 
Y.KadryId [NrEwK]

from RCP R
left join Readers RR on RR.Id = R.ECReaderId
left join PracownicyKarty K on K.RcpId = R.ECUserId and R.Czas between K.Od and ISNULL(K.Do, '20990909')
left join Pracownicy P on P.Id = K.IdPracownika
left join Przypisania X on X.IdPracownika = K.IdPracownika and R.Czas between X.Od and ISNULL(X.Do, '20990909') and X.Status = 1
left join Pracownicy Y on Y.Id = X.IdKierownika

where R.ECReaderId in 
(
select items from dbo.SplitInt((select top 1 Readers from StrefyReaders where IdStrefy = @str and DataOd &lt;= @od order by DataOd desc), ',') 
)
and R.Czas between @od and @do
and P.Id is not null
order by Pracownik, Czas
                    "/>
                </asp:View>
                    
            </asp:MultiView>
        </div>

<%--
select * from Strefy
insert into Strefy values ('ESD Lista - Techniczna', 1, 0)
select * from StrefyReaders

declare @esd varchar(max)
select @esd = isnull(@esd + ',', '') + 
CONVERT(varchar,Id) 
from Readers 
where Name like '%ESD%' or Zone like '%ESD%'
order by Id

insert into StrefyReaders values (34, '20140101', @esd)
--%>