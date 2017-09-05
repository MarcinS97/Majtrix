<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPracownicyHarm.ascx.cs" Inherits="HRRcp.RCP.Controls.Harmonogram.cntPracownicyHarm" %>

<%@ Register Src="~/Controls/Portal/cntSqlTabs.ascx" TagName="cntSqlTabs" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagName="DateEdit" TagPrefix="uc2" %>

<%@ Register Assembly="HRRcp" Namespace="HRRcp.Controls.Custom" TagPrefix="cc2" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc2" TagName="cntModal" %>
<%@ Register Src="~/RCP/Controls/Harmonogram/cntPracownicyEditModal.ascx" TagPrefix="uc2" TagName="cntPracownicyEditModal" %>



<div id="paPracownicyHarm" runat="server" class="cntPracownicyHarm">
    <div id="paFilter" runat="server" class="filter row" visible="true">
        <div class="col-md-12">
            <asp:Label ID="Label1" runat="server" CssClass="label" Text="Wyszukaj:" Visible="true"></asp:Label>
            <asp:TextBox ID="tbSearch" CssClass="form-control tb-search" MaxLength="250" Width="320" runat="server"></asp:TextBox>
            <%--            
            <div class="hspacer"></div>
            <asp:Button ID="btClear" runat="server" CssClass="button75 btn" Text="Czyść" />
            --%>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="filter2">
                <div class="filter2abs">
                    <asp:Label ID="lblNaDzien" runat="server" Text="Na dzień:" CssClass="label" />
                    <uc2:DateEdit ID="deNaDzien" runat="server" AutoPostBack="true" OnDateChanged="deNaDzien_DateChanged" />
                    <div class="spacer">
                    </div>
                    <cc2:cntMultiSelect ID="msDzial" runat="server" SelectionMode="Multiple" DataSourceID="dsDzial" DataValueField="Value" Visible="true"
                        DataTextField="Text" CssClass="" AutoPostBack="false" OnSelectedIndexChanged="msDzial_SelectedIndexChanged"
                        DisableIfEmpty="true" NonSelectedText="jednostka ..." ButtonWidth="320px" Width="320" />
                    <cc2:cntMultiSelect ID="msFirma" runat="server" SelectionMode="Multiple" DataSourceID="dsFirma" DataValueField="Value" Visible="true"
                        DataTextField="Text" CssClass="" AutoPostBack="false" OnSelectedIndexChanged="msFirma_SelectedIndexChanged" OnDataBound="msFirma_DataBound"
                        DisableIfEmpty="true" NonSelectedText="klasyfikacja ..." ButtonWidth="200px" />
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default btn-search" Visible="true" OnClick="btnSearch_Click" ToolTip="Wyszukaj...">
                        <i class="fa fa-search"></i>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-default btn-search-clear" Visible="true" OnClick="btnClear_Click" ToolTip="Czyść...">
                        <i class="glyphicon glyphicon-erase"></i>
                    </asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnAssignOkresType" runat="server" CssClass="btn btn-primary" Text="Przypisz okres" OnClick="btnAssignOkresType_Click" />
                    <asp:Button ID="btnAddEmployee" runat="server" Text="Dodaj pracownika" CssClass="btn btn-success" OnClick="btnAddEmployee_Click" />
                </div>
            </div>
            <asp:Button ID="btSearch" runat="server" CssClass="button_postback" Text="Wyszukaj" OnClick="btSearch_Click" />
            <div class="tabsLine">
                <asp:Menu ID="tabStatus" CssClass="printoff" runat="server" Orientation="Horizontal" Visible="true"
                    OnMenuItemClick="tabStatus_MenuItemClick">
                    <StaticMenuStyle CssClass="tabsStrip" />
                    <StaticMenuItemStyle CssClass="tabItem" />
                    <StaticSelectedStyle CssClass="tabSelected" />
                    <StaticHoverStyle CssClass="tabHover" />
                    <Items>
                        <asp:MenuItem Text="Wszyscy" Value="-99" Selected="true"></asp:MenuItem>
                        <asp:MenuItem Text="Aktywni" Value="1"></asp:MenuItem>
                        <asp:MenuItem Text="Nieaktywni" Value="-1"></asp:MenuItem>
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
            </div>
            <div class="pracownicy">
                <asp:GridView ID="gvPracownicy" runat="server" DataSourceID="SqlDataSource1" PagerSettings-Mode="NumericFirstLast"
                    EmptyDataText="Brak danych" PageSize="9999"
                    OnLoad="gvPracownicy_Load">
                </asp:GridView>
                <div class="pager">
                    <div class="pagerabs">
                        <span class="count">Ilość:<asp:Label ID="lbCount" runat="server"></asp:Label></span>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <span class="count">Pokaż na stronie:</span>
                        <asp:DropDownList ID="ddlLines" runat="server" AutoPostBack="true" CssClass="form-control"
                            OnChange="showAjaxProgress();"
                            OnSelectedIndexChanged="ddlLines_SelectedIndexChanged">
                            <asp:ListItem Text="25" Value="25"></asp:ListItem>
                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                            <asp:ListItem Text="WSZYSTKO" Value="all" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <asp:Button ID="gvPracownicyCmd" runat="server" CssClass="button_postback" Text="Button" OnClick="gvPracownicyCmd_Click" />
                <asp:HiddenField ID="gvPracownicyCmdPar" runat="server" />
                <asp:HiddenField ID="gvPracownicySelected" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc2:cntModal runat="server" ID="cntModal" Title="Przypisz typ okresu rozliczeniowego do pracowników">
        <ContentTemplate>
            <div class="form-group">
                <label>Przypisz od daty: </label>
                <asp:Label ID="lblNaDzienOkres" runat="server" />
                <br />
                <label>Typ okresu:</label>

                <asp:DropDownList ID="ddlTypOkresu" runat="server" DataSourceID="dsTypOkresu" DataValueField="Value" DataTextField="Text" CssClass="form-control" />
                <asp:SqlDataSource ID="dsTypOkresu" runat="server" CancelSelectOnNullParameter="false" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand=
"
select null Value, 'wybierz ...' Text, 0 Sort 
union all 
select ort.Id Value, ort.Nazwa Text, 1 Sort 
from rcpOkresyRozliczenioweTypy ort
left join OkresyRozliczeniowe o on o.Typ = ort.Id
where Aktywny = 1
group by ort.Id, ort.Nazwa
having count(o.Id) &gt; 0
order by Sort, Text                    
"
                    >
                    <SelectParameters>
                        <asp:ControlParameter ControlID="deNaDzien" Name="nadzien" PropertyName="Date" Type="DateTime" />
                    </SelectParameters>
                </asp:SqlDataSource>


            </div>
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btnAssignOkresConfirm" runat="server" Text="Przypisz" CssClass="btn btn-primary" OnClick="btnAssignOkresConfirm_Click" />
            <asp:Button ID="btnAssignOkres" runat="server" CssClass="hidden" OnClick="btnAssignOkres_Click" />
        </FooterTemplate>
    </uc2:cntModal>
</div>

<uc2:cntPracownicyEditModal runat="server" id="cntPracownicyEditModal" OnSaved="cntPracownicyEditModal_Saved" />


<asp:SqlDataSource ID="SqlDataSource1" runat="server" CancelSelectOnNullParameter="false" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    OnSelected="SqlDataSource1_Selected"
    SelectCommand="
    /*
declare @typ int  -- 1 zatrudnieni, -1 zwolnieni, -99 wszyscy
declare @dzial nvarchar(100)
declare @firma nvarchar(100)
declare @selected varchar(max)
set @status = 1
    */
--declare @data datetime
--set @data = dbo.getdate(GETDATE())
if @dzial is null set @dzial = '-99'
if @firma is null set @firma = '-99'

declare @kodyfirma nvarchar(100)
select @kodyfirma = Nazwa from Kody where Typ = 'CLASS.FIRMA'

declare @TAK nvarchar(50)
declare @NIE nvarchar(50)
set @TAK = 'TAK'
set @NIE = 'NIE'

select 
  P.Id [pracId:-]
, P.Nazwisko [nn:-]
, P.Imie [ii:-]
, P.KadryId [nrew:-]
, case when R1.Aktywny = 1 then 'aktywny' else 'nieaktywny' end [row:class]

--, @status [Filtr;Status]
--, @dzial [Filtr;Dział]
--, @firma [Filtr;Firma]
--, @selected [Selected]
, SEL.items [Zaznacz:CBH;check|@pracId|Zaznacz osobę]
--, 1 [Lp:Lp]  --paginator :(

--, P.Nazwisko + ' ' + P.Imie + ISNULL(' (' + P.KadryId + ')', '') [Pracownik]
, P.Nazwisko + ' ' + P.Imie [Pracownik]

, P.KadryId [Nr ewid.]
, DZ.Nazwa [Jednostka]
, isnull(ort.Nazwa, 'NIEPRZYPISANY') [Typ okresu]
, PS.Klasyfikacja [Klasyfikacja]
, P.DataZatr [Data zatrudnienia:D]
--, P.DataZwol [Data zwolnienia:D]

--, R.Od [Od:D]
--, R.Do [Do:D]
--, P.Status     

, P.DataImportu [Data importu:D]
, P.LastDataZwol [Poprzednio zwolniony:D]
, R1.Aktywny [act:-]
--, case R1.Aktywny when 1 then @TAK else @NIE + ISNULL(' ' + convert(varchar(10), R.Do, 20), '') end [Aktywny (Data wyłączenia):;control|js:ajaxHarmonogram(this 'gvPracownicy' 1 @pracId @act 1);|Zmień status aktywności] 
--, case R1.Aktywny when 1 then @TAK else @NIE end [Aktywny:;control|js:ajaxHarmonogram(this 'gvPracownicy' 1 @pracId @act 2);|Zmień status aktywności] 
, case R1.Aktywny when 1 then @TAK else @NIE end [Aktywny:;control|cmd:status @pracId @act|Zmień status aktywności] 
, P.DataZwol [Data wyłączenia:D]
, 'Edytuj' [Edytuj:;control|cmd:edit @pracId|Edytuj]
from Pracownicy P
outer apply (select items from dbo.SplitInt(@selected,',') where items = P.Id) SEL
outer apply (select top 1 * from PracownicyStanowiska where IdPracownika = P.Id and Od &lt;= /*@data*/@nadzien order by Od desc) PS  -- bo moze byc juz zwolniony

--left join Przypisania R on R.IdPracownika = P.Id and R.Status = 1 and @data between R.Od and ISNULL(R.Do, '20990909') 
--outer apply (select case when R.Id is null then 0 else 1 end Aktywny) R1
outer apply (select top 1 * from Przypisania where IdPracownika = P.Id and Status = 1 and Od &lt;= /*@data*/@nadzien order by Od desc) R
outer apply (select case when /*@data*/@nadzien between R.Od and ISNULL(R.Do, '20990909') then 1 else 0 end Aktywny) R1

left join Dzialy DZ on DZ.Id = PS.IdDzialu
left join rcpPracownicyTypyOkresow pto on pto.IdPracownika = P.Id and /*@data*/@nadzien between pto.DataOd and isnull(pto.DataDo, '20990909')
left join rcpOkresyRozliczenioweTypy ort on ort.Id = pto.IdTypuOkresu
where P.Status in (-1,0,1)
and ( 
    @status = -99
-- or @status = -1 and R.Id is null
-- or @status =  1 and R.Id is not null
 or @status = -1 and R1.Aktywny = 0
 or @status =  1 and R1.Aktywny = 1
	)
and (@dzial = '-99' or PS.IdDzialu     in (select items from dbo.SplitStr(@dzial, ',')))
and (
    @firma = '-99' 
 or RIGHT(@firma, 3) = 'all' and PS.Klasyfikacja != @kodyfirma  --wszystkie APT, może być po przecinku z innymi opcjami
 or PS.Klasyfikacja in (select items from dbo.SplitStr(@firma, ','))
    )
order by P.Nazwisko, P.Imie, P.KadryId
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="tabStatus" Name="status" PropertyName="SelectedValue" Type="String" />
        <asp:ControlParameter ControlID="msDzial" Name="dzial" PropertyName="SelectedItems" Type="String" />
        <asp:ControlParameter ControlID="msFirma" Name="firma" PropertyName="SelectedItems" Type="String" />
        <asp:ControlParameter ControlID="gvPracownicySelected" Name="selected" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="deNaDzien" Name="nadzien" PropertyName="Date" Type="DateTime" />
    </SelectParameters>
</asp:SqlDataSource>

<%-- to samo co w HarmonogramSQL, optymalniej tu przenieść, HarmonogramSQL - testy, jako opcja do wykorzystania--%>
<asp:SqlDataSource ID="dsSetPracAktywny" runat="server"
    SelectCommand="
declare @pid int
declare @act int
declare @data datetime
declare @bom datetime
set @pid = {0}
set @act = {1}
set @bom = dbo.bom('{2}')

set @data = DATEADD(D,-1,@bom)  -- zwalniam żeby nie pokazywal sie w calym miesiacu
    
--set @data = '{2}'
--set @data = DATEADD(D,-1,@data)  -- zwalniam żeby nie pokazywal sie w calym miesiacu


delete from Przypisania where IdPracownika = @pid and Od &gt; @data

declare @rid int
select top 1 @rid = Id from Przypisania where IdPracownika = @pid and Status = 1 and Od &lt;= @data order by Od desc

if @act = 1 begin
    if @rid is null
        insert into Przypisania (IdPracownika, Od, Do, IdKierownika, Status, Typ) values (@pid, @bom, null, 0, 1, 1)
    else begin
        update Przypisania set Do = null where Id = @rid
    end
    update Pracownicy set Status = 0, DataZwol = null where Id = @pid
end
else begin
    update Przypisania set Do = @data where Id = @rid
    update Pracownicy set Status = -1, DataZwol = @data where Id = @pid
end
    "></asp:SqlDataSource>

<asp:SqlDataSource ID="dsDzial" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
select Id Value, Nazwa Text, 1 Sort from Dzialy where Status &gt;= 0 order by Sort, Text
    " />

<asp:SqlDataSource ID="dsFirma" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
/* poniżej inna metoda
-- zeby nie trzeba bylo pamietac przy pierwszym odpaleniu..., potem usunąć i pamiętać...
if not exists (select Nazwa from Kody where Typ = 'CLASS.FIRMA')
    insert into Kody (Typ, Nazwa, Kod, Aktywny) values ('CLASS.FIRMA', 'keeeper', 1, 1)
*/

declare @firma nvarchar(100)
select @firma = Nazwa from Kody where Typ = 'CLASS.FIRMA'

if @firma is null begin     -- docelowo usunąć i pamiętać, żeby dodawać w kodach...
	set @firma = 'keeeper'  
	insert into Kody (Typ, Nazwa, Kod, Aktywny) values ('CLASS.FIRMA', @firma, 1, 1)
end

select distinct Klasyfikacja Value, Klasyfikacja Text, case when Klasyfikacja = @firma then 0 else 1 end Sort 
from PracownicyStanowiska PS
union all 
select 'all' Value, 'Wszystkie APT' Text, 2 Sort    
order by Sort, Text
" />

<asp:SqlDataSource ID="dsAssignOkres" runat="server" SelectCommand="
declare @pracIds nvarchar(max)
declare @okresId int
declare @bom datetime

set @pracIds = '{0}'
set @okresId = {1}
set @bom = '{2}'


--select * from rcpPracownicyTypyOkresow
--update rcpPracownicyTypyOkresow set 
--	DataDo = dateadd(DAY, -1, /*dbo.getdate(getdate())*/@bom)
--from rcpPracownicyTypyOkresow pto
--where pto.DataDo is null and pto.IdPracownika in (select items from dbo.splitInt(@pracIds, ','))

insert rcpPracownicyTypyOkresow
select items, @okresId, /*dbo.getdate(getdate())*/@bom, null
from dbo.splitInt(@pracIds, ',')
" />
