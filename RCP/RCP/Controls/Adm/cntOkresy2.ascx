<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntOkresy2.ascx.cs" Inherits="HRRcp.RCP.Controls.Adm.cntOkresy2" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagName="DateEdit" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>
<%@ Register Src="~/RCP/Controls/Adm/cntOkresyEdit.ascx" TagPrefix="uc1" TagName="cntOkresyEdit" %>
<%@ Register Src="~/Controls/Adm/cntImportCSV.ascx" TagPrefix="uc1" TagName="cntImportCSV" %>

<%@ Register Assembly="HRRcp" Namespace="HRRcp.Controls.Custom" TagPrefix="cc2" %>


<div id="ctAdmOkresy" runat="server" class="cntAdmOkresy" style="">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Always">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-6">
                    <h3>Zarządzanie okresami rozliczeniowymi
                        <%--<asp:Button ID="btnCloseOkres" runat="server" Text="Zamknij okres rozliczeniowy" CssClass="btn btn-sm btn-primary" OnClick="btnCloseOkres_Click" />--%>

                    </h3>
                    <hr />
                    <div class="top_buttons">
                        <asp:Button ID="btnAddOkresModal" runat="server" OnClick="btnAddOkresModal_Click" CssClass="btn btn-success" Text="Otwórz okres rozliczeniowy" />
                    </div>
                    <asp:ListView ID="lvOkresy" runat="server" DataKeyNames="Id"
                        DataSourceID="SqlDataSource1" InsertItemPosition="None"
                        OnItemCreated="lvOkresy_ItemCreated" OnItemDataBound="lvOkresy_ItemDataBound"
                        OnItemInserting="lvOkresy_ItemInserting"
                        OnItemUpdating="lvOkresy_ItemUpdating"
                        OnItemDeleted="lvOkresy_ItemDeleted"
                        OnItemDeleting="lvOkresy_ItemDeleting">
                        <ItemTemplate>
                            <tr class="it typ<%# Eval("Typ") %>" >
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Type") %>' />
                                </td>
                                <td class="data">
                                    <asp:Label ID="DataOdLabel" runat="server" Text='<%# Eval("DataOd", "{0:d}") %>' />
                                </td>
                                <td class="data">
                                    <asp:Label ID="DataDoLabel" runat="server" Text='<%# Eval("DataDo", "{0:d}") %>' />
                                </td>
                                <%--                        <td class="data">
                                <asp:Label ID="LockedToLabel" runat="server" Text='<%# Eval("DataBlokady", "{0:d}") %>' />
                            </td>
                            <td class="data">
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("DataNaliczenia", "{0:d}") %>' />
                            </td>
                            <td class="num">
                                <asp:Label ID="StawkaNocnaLabel" runat="server" Text='<%# Eval("StawkaNocna", "{0:0.0000}") %>' />
                            </td>--%>
                                <td>
                                    <asp:Label ID="NominalLabel" runat="server" Text='<%# Eval("Nominal") %>' ToolTip='<%# Eval("NominalMies") %>'/>
                                </td>
                                <td>
                                    <asp:Label ID="StatusLabel" runat="server" Text='<%# GetStatus(Eval("Status")) %>' />
                                </td>
                                <%--                        <td>
                                <asp:Label ID="ZamknalNILabel" runat="server" Text='<%# Eval("ZamknalNI") %>' />
                            </td>
                            <td class="data">
                                <asp:Label ID="DataZamknieciaLabel" runat="server" Text='<%# Eval("DataZamkniecia", "{0:d}") %>' />
                            </td>--%>
                                <td class="control">
                                    <asp:Button ID="EditButton" runat="server" Text="Edytuj" OnClick="EditButton_Click" CommandArgument='<%# Eval("Id") %>' />
                                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="Usuń" />

                                    <asp:Button ID="btnCloseOkresConfirm" runat="server" CssClass="btn btn-sm btn-primary" Text="Zamknij" OnClick="btnCloseOkresConfirm_Click"
                                        Visible='<%# (int)Eval("Status") == 0 %>' CommandArgument='<%# Eval("Id") %>' />

                                    
                                    <asp:Button ID="btnOpenOkresConfirm" runat="server" CssClass="btn btn-sm btn-default" Text="Otwórz" OnClick="btnOpenOkresConfirm_Click"
                                        Visible='<%# (int)Eval("Status") == 1 %>' CommandArgument='<%# Eval("Id") %>' />

                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table runat="server" style="">
                                <tr>
                                    <td>Brak danych
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <InsertItemTemplate>
                            <tr class="iit">
                                <td class="data">
                                </td>
                                <td class="data">
                                    <uc1:DateEdit ID="deDataDo" runat="server" Date='<%# Bind("DataDo") %>' />
                                </td>
                                <td class="data">
                                    <uc1:DateEdit ID="deDataBlokady" runat="server" Date='<%# Bind("DataBlokady") %>' Visible="false" />
                                </td>
                                <td class="num">
                                    <asp:TextBox ID="StawkaNocnaTextBox" runat="server" Text='<%# Bind("StawkaNocna", "{0:0.0000}") %>' />
                                    <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" TargetControlID="StawkaNocnaTextBox"
                                        FilterType="Custom"
                                        ValidChars="0123456789,." />
                                    <uc1:DateEdit ID="deDataOd" runat="server" Date='<%# Bind("DataOd") %>' />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStatus" runat="server" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlZamknal" runat="server" Visible="false"
                                        DataSourceID="SqlDataSource2"
                                        DataTextField="Admin"
                                        DataValueField="Id">
                                    </asp:DropDownList>
                                </td>
                                <td class="data">
                                    <uc1:DateEdit ID="deDataZamkniecia" runat="server" Date='<%# Bind("DataZamkniecia") %>' Visible="false" />
                                </td>
                                <td class="control">
                                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Dodaj" />
                                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Czyść" />
                                </td>
                            </tr>
                        </InsertItemTemplate>
                        <LayoutTemplate>
                            <table runat="server" class="xListView1 tbAdmOkresy hoverline">
                                <tr runat="server">
                                    <td runat="server">
                                        <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                                            <tr runat="server" style="">
                                                <th runat="server">Typ okresu</th>
                                                <th runat="server">Data od</th>
                                                <th runat="server">Data do</th>
                                                <%--      <th runat="server">Akceptacja tygodniowa&nbsp;do</th>
                                            <th runat="server">Data naliczenia</th>--%>
                                                <%--<th runat="server">Stawka nocna&nbsp;[zł]</th>--%>
                                                <th runat="server">Nominał (dni)</th>
                                                <th runat="server">Status</th>
                                                <%--<th runat="server">Zamknął</th>--%>
                                                <%--<th runat="server">Data zamknięcia</th>--%>
                                                <th runat="server" class="control"></th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr class="pager">
                                    <td>
                                        <asp:DataPager ID="DataPager1" runat="server" PageSize="20">
                                            <Fields>
                                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="false" ShowNextPageButton="false" FirstPageText="Pierwsza" PreviousPageText="Poprzednia" />
                                                <asp:NumericPagerField ButtonType="Link" />
                                                <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="true" ShowNextPageButton="true" NextPageText="Następna" LastPageText="Ostatnia" />
                                            </Fields>
                                        </asp:DataPager>
                                    </td>
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <EditItemTemplate>
                            <tr class="eit">
                                <td>
                                    <asp:DropDownList ID="ddlTypOkresu" runat="server" DataValueField="Value" DataTextField="Text" DataSourceID="dsTypOkresu" CssClass="form-control" AutoPostBack="true" />
                                    <asp:SqlDataSource ID="dsTypOkresu" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                                        SelectCommand="
    select null Value, 'wybierz ...' Text, 0 Sort
    union all
    select Id Value, Nazwa Text, 1 Sort
    from rcpOkresyRozliczenioweTypy
    order by Sort, Text                    
    " />
                                </td>
                                <td class="data">
                                    <uc1:DateEdit ID="deDataOd" runat="server" Date='<%# Bind("DataOd") %>' />
                                </td>
                                <td class="data">
                                    <uc1:DateEdit ID="deDataDo" runat="server" Date='<%# Bind("DataDo") %>' />
                                </td>
                                <td class="data">
                                    <uc1:DateEdit ID="deDataBlokady" runat="server" Date='<%# Bind("DataBlokady") %>' />

                                    <%--
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("DataBlokady") %>' />
                    <asp:CustomValidator ID="vlDataBlokady" runat="server" 
                        ControlToValidate="TextBox1" 
                        onservervalidate="vlDataBlokady_ServerValidate"
                        CssClass="error"
                        ErrorMessage="Błąd!"
                        Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="vge"
                        ></asp:CustomValidator>
                                    --%>  
                                </td>
                                <td class="data">
                                    <uc1:DateEdit ID="deDataNaliczenia" runat="server" Date='<%# Bind("DataNaliczenia") %>' />
                                </td>
                                <td class="num">
                                    <asp:TextBox ID="StawkaNocnaTextBox" runat="server" Text='<%# Bind("StawkaNocna", "{0:0.0000}") %>' />
                                    <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" Enabled="true" TargetControlID="StawkaNocnaTextBox"
                                        FilterType="Custom"
                                        ValidChars="0123456789,." />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStatus" runat="server" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlZamknal" runat="server" Visible="false"
                                        DataSourceID="SqlDataSource2"
                                        DataTextField="Admin"
                                        DataValueField="Id">
                                    </asp:DropDownList>
                                </td>
                                <td class="data">
                                    <uc1:DateEdit ID="deDataZamkniecia" runat="server" Date='<%# Bind("DataZamkniecia") %>' Visible="false" />
                                </td>
                                <td class="control">
                                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="vge" />
                                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Anuluj" />
                                </td>
                            </tr>
                        </EditItemTemplate>
                    </asp:ListView>
                </div>
                <div class="col-md-6 import">
                    <h3>Import</h3>
                    <hr />
                    <label>Data ostatniego importu:</label>
                    <asp:Label ID="lbLastImport" runat="server" ></asp:Label>
                    <br />
                    <%--<asp:Button ID="btnImport" runat="server" Text="Importuj" CssClass="btn btn-primary" />--%>
                    <uc1:cntImportCSV runat="server" id="cntImportCSV" Typ="9" BtnCssClass="btn btn-primary" OnImportFinished="cntImportCSV_ImportFinished"/>
                    <asp:Button ID="btZwolnij" runat="server" CssClass="btn btn-primary" Text="Aktywni pracownicy" OnClick="btZwolnij_Click"/>
                    <br />
                    <br />
                    <br />
                    <h3>Eksport danych</h3>
                    <hr />
                    <label>Eksportuj dane za miesiąc:</label>
                    <asp:DropDownList ID="ddlMiesiac" runat="server" CssClass="form-control" DataSourceID="dsEksportMies" DataTextField="Text" DataValueField="Value"></asp:DropDownList>
                    <div class="exportfilter">
                        <cc2:cntMultiSelect ID="msEntities" runat="server" SelectionMode="Multiple" DataSourceID="dsEntities" DataValueField="Value" Visible="true"
                                        DataTextField="Text" CssClass="" AutoPostBack="false" OnSelectedIndexChanged="msEntities_SelectedIndexChanged" 
                                        DisableIfEmpty="true" NonSelectedText="jednostka ..." ButtonWidth="320px" Width="320"/>
                        <cc2:cntMultiSelect ID="msKlasyfikacje" runat="server" SelectionMode="Multiple" DataSourceID="dsKlasyfikacje2" DataValueField="Value" Visible="true"
                                        DataTextField="Text" CssClass="" AutoPostBack="false" OnSelectedIndexChanged="msKlasyfikacje_SelectedIndexChanged" OnDataBound="msKlasyfikacje_DataBound"
                                        DisableIfEmpty="true" NonSelectedText="klasyfikacja ..." ButtonWidth="200px" />    
                    </div>
                    <asp:Button ID="btExportCSV" runat="server" CssClass="btn btn-primary" Text="Pobierz plik CSV" OnClick="btExportCSV_Click"/>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="cntImportCSV" />
            <asp:PostBackTrigger ControlID="btExportCSV" />
        </Triggers>
    </asp:UpdatePanel>
</div>

<asp:Button ID="btnCloseOkres" runat="server" CssClass="button_postback" OnClick="btnCloseOkres_Click" />
<asp:Button ID="btnOpenOkres" runat="server" CssClass="button_postback" OnClick="btnOpenOkres_Click" />

<uc1:cntOkresyEdit runat="server" ID="cntOkresyEdit" OnSaved="cntOkresyEdit_Saved" />

<asp:SqlDataSource ID="SqlDataSource1" runat="server"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    DeleteCommand="delete from CzasNom where IdOkresu = @Id; DELETE FROM [OkresyRozliczeniowe] WHERE [Id] = @Id"
    SelectCommand="
set language 'Polish'   -- uwaga - ustawia dla sesji !!!

declare @sp varchar(1), @emp varchar(1), @sep varchar(10), @nodata varchar(50)
set @sp = ' '
set @emp = ''
set @sep = ', '
set @nodata = 'brak'


select
  o.*
, ort.Nazwa Type

--,ISNULL(STUFF((
--select @sep + convert(varchar, C.DniPrac) 
--from CzasNom C
--where C.IdOkresu = o.Id
--order by Data
--for XML PATH('')), 1, 2, @emp), '') as Nominal

--,ISNULL(STUFF((
--select @sep + DATENAME(MONTH, C.Data)
--from CzasNom C
--where C.IdOkresu = o.Id
--order by Data
--for XML PATH('')), 1, 2, @emp), '') as NominalMies

, (select dbo.cat(DniPrac, ', ', 0) from 
    (
        select top 360 * from CzasNom where IdOkresu = o.Id order by Data 
    ) a
  ) Nominal
, (select dbo.cat(DATENAME(MONTH, Data), ', ', 0) from 
    (
        select top 360 * from CzasNom where IdOkresu = o.Id order by Data
    ) b
  ) NominalMies 
from OkresyRozliczeniowe o
left join rcpOkresyRozliczenioweTypy ort on ort.Id = o.Typ
order by DataDo desc, IloscMiesiecy desc
        "
    UpdateCommand="UPDATE [OkresyRozliczeniowe] SET [DataOd] = @DataOd, [DataDo] = @DataDo, [Status] = @Status, Typ = @Typ WHERE [Id] = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="DataOd" Type="DateTime" />
        <asp:Parameter Name="DataDo" Type="DateTime" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Typ" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource2" runat="server"
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="SELECT 1 as Sort, Id, Nazwisko + ' ' + Imie as Admin FROM Pracownicy where Admin=1
                   union 
                   select 0 as Sort, null as Id, 'wybierz ...' as Admin
                   ORDER BY Sort, Admin"></asp:SqlDataSource>

<asp:SqlDataSource ID="dsCloseOkres" runat="server" SelectCommand="update OkresyRozliczeniowe set Status = 1 where Id = {0}" />
<asp:SqlDataSource ID="dsOpenOkres" runat="server" SelectCommand="update OkresyRozliczeniowe set Status = 0 where Id = {0}" />
<asp:SqlDataSource ID="dsLastImportDate" runat="server" SelectCommand="select top 1 * from bufPracownicyImport --order by DataImportu desc" />

<asp:SqlDataSource ID="dsEksportMies" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
declare @od datetime = dbo.bom(GETDATE())
select distinct CONVERT(varchar(7), DataOd, 20) as Text, CONVERT(varchar(10), DataOd, 20) as Value
from OkresyRozliczeniowe 
where Status = 0   -- otwarte
order by 1 desc
    ">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsEksportCSV" runat="server" SelectCommand="
declare @od datetime
declare @do datetime
set @od = dbo.bom('{0}')
set @do = dbo.eom(@od)

declare @dzial nvarchar(max)	-- id jednostki
declare @firma nvarchar(max)	-- nazwa apt
set @dzial = '{1}'
set @firma = '{2}'

if ISNULL(@dzial, '') = '' set @dzial = '-99'
if ISNULL(@firma, '') = '' set @firma = '-99'

declare @kodyfirma nvarchar(100)
select @kodyfirma = Nazwa from Kody where Typ = 'CLASS.FIRMA'

-- header --
select 'GRAFIK PRACY NA MIESIĄC ' + convert(varchar(10), @od, 20) + ' DO ' + convert(varchar(10), @do, 20)
select ''    

-- zmiany --
select '', 'Symbole', 'Od', 'Do'
;with Z(Lp,Symbol,Od,Do) as 
(
    select 
      ROW_NUMBER() over (order by Z1.Od) Lp
    , Z.Symbol, Z1.Od, Z1.Do       
    from Zmiany Z
    outer apply (select DATEPART(HOUR, Od) Od, DATEPART(HOUR, Do) Do) Z1
    where Z.TypZmiany = 1
)
select '', ISNULL(Symbol, ''), ISNULL(convert(varchar,Od), ''), ISNULL(convert(varchar,Do), '') 
from dbo.GetNum(1,6) N
left join Z on Z.Lp = N.num
select ''    

-- nagłówek --
declare @stmt nvarchar(max)
set @stmt = N'select ''LP'',''Nazwisko Imię'','''''	
select @stmt = @stmt + ',' + convert(varchar, DATEPART(DAY, Data)) from dbo.GetDates2(@od, @do)
exec sp_executesql @stmt

-- harmonogramy --
declare @colsH nvarchar(max)
declare @colsD nvarchar(max)

select 
  @colsD = ISNULL(@colsD + ',', '') + '[' + convert(varchar, num) + ']' 
, @colsH = ISNULL(@colsH + ',', '') + '[' + convert(varchar, num) + ']'
from dbo.GetDates2(@od, @do) D
outer apply (select DATEPART(DAY, D.Data) num) D1

declare @stmt2 nvarchar(max)
set @stmt2 = N'
declare @od datetime
declare @do datetime
declare @dzial nvarchar(max)	-- id jednostki
declare @firma nvarchar(max)	-- nazwa apt
declare @kodyfirma nvarchar(100)

set @od = ''' + convert(varchar(10), @od, 20) + '''
set @do = ''' + convert(varchar(10), @do, 20) + '''
set @dzial = ''' + @dzial + '''
set @firma = ''' + @firma + '''
set @kodyfirma = ''' + @kodyfirma + '''

select ROW_NUMBER() OVER(ORDER BY Pracownik) AS Lp
--select ROW_NUMBER() OVER(ORDER BY Info, Pracownik) AS Lp
, Pracownik

, '''' [ ]
--, Info [ ]   -- jezeli format eksportu sie nie rozsypie, to mozna dać 

, ' + @colsH + '
from
(
	select 
      P.Nazwisko + '' '' + P.Imie + ISNULL('' ('' + P.KadryId + '')'','''') Pracownik 
	, ISNULL(DZ.Nazwa,'''') + '', '' + ISNULL(PS.Klasyfikacja, '''') Info
	, ISNULL(Z.Symbol,'''') Zmiana
	
	, D1.Dzien

	from Pracownicy P
	outer apply (select * from dbo.GetDates2(@od, @do)) D
	outer apply (select DAY(D.Data) Dzien) D1
	left join PlanPracy PP on PP.Data = D.Data and PP.IdPracownika = P.Id
	left join Zmiany Z on Z.Id = PP.IdZmianyPlan and TypZmiany = 1  -- !!! do weryfikacji
	outer apply (select top 1 * from PracownicyStanowiska where IdPracownika = P.Id and Od &lt;= @do order by Od desc) PS  -- bo moze byc juz zwolniony
	left join Dzialy DZ on DZ.Id = PS.IdDzialu
	where P.Id in (select distinct IdPracownika from Przypisania where Status = 1 and Od &lt;= @do and @od &lt;= ISNULL(Do, ''20990909''))
		and (@dzial = ''-99'' or PS.IdDzialu     in (select items from dbo.SplitInt(@dzial, '','')))
		--and (@firma = ''all'' or PS.Klasyfikacja in (select items from dbo.SplitStr(@firma, '','')))
        and (
            @firma = ''-99'' 
         or RIGHT(@firma, 3) = ''all'' and PS.Klasyfikacja != @kodyfirma  --wszystkie APT, może być po przecinku z innymi opcjami
         or PS.Klasyfikacja in (select items from dbo.SplitStr(@firma, '',''))
            )
) D
pivot
(
	max(Zmiana) for Dzien in (' + @colsD + ')
) PV
'
exec sp_executesql @stmt2
        " />

<%--
<asp:SqlDataSource ID="dsEksportCSV" runat="server" SelectCommand="
declare @od datetime
declare @do datetime
set @od = dbo.bom('{0}')
set @do = dbo.eom(@od)

-- header --
select 'GRAFIK PRACY NA MIESIĄC ' + convert(varchar(10), @od, 20) + ' DO ' + convert(varchar(10), @do, 20)
select ''    

-- zmiany --
;with Z(Lp,Symbol,Od,Do) as 
(
    select 
      ROW_NUMBER() over (order by Z1.Od) Lp
    , Z.Symbol, Z1.Od, Z1.Do       
    from Zmiany Z
    outer apply (select DATEPART(HOUR, Od) Od, DATEPART(HOUR, Do) Do) Z1
    where Z.TypZmiany = 0
)
select '', ISNULL(Symbol, ''), ISNULL(convert(varchar,Od), ''), ISNULL(convert(varchar,Do), '') 
from dbo.GetNum(1,6) N
left join Z on Z.Lp = N.num
select ''    

-- nagłówek --
declare @stmt nvarchar(max)
set @stmt = N'select ''LP'',''Nazwisko Imię'','''''	
select @stmt = @stmt + ',' + convert(varchar, DATEPART(DAY, Data)) from dbo.GetDates2(@od, @do)
exec sp_executesql @stmt

-- harmonogramy --
declare @colsH nvarchar(max)
declare @colsD nvarchar(max)

select 
  @colsD = ISNULL(@colsD + ',', '') + '[' + convert(varchar, num) + ']' 
, @colsH = ISNULL(@colsH + ',', '') + '[' + convert(varchar, num) + ']'
from dbo.GetDates2(@od, @do) D
outer apply (select DATEPART(DAY, D.Data) num) D1

declare @stmt2 nvarchar(max)
set @stmt2 = N'
declare @od datetime
declare @do datetime
set @od = ''' + convert(varchar(10), @od, 20) + '''
set @do = ''' + convert(varchar(10), @do, 20) + '''

select ROW_NUMBER() OVER(ORDER BY Pracownik) AS Lp
, Pracownik
, '''' [ ]
, ' + @colsH + '
from
(
	select 
      P.Nazwisko + '' '' + P.Imie + ISNULL('' ('' + P.KadryId + '')'','''') Pracownik 
	, ISNULL(Z.Symbol,'''') Zmiana
	
	, D1.Dzien

	from Pracownicy P
	outer apply (select * from dbo.GetDates2(@od, @do)) D
	outer apply (select DAY(D.Data) Dzien) D1
	left join PlanPracy PP on PP.Data = D.Data and PP.IdPracownika = P.Id
	left join Zmiany Z on Z.Id = PP.IdZmianyPlan and TypZmiany = 0  -- !!! do weryfikacji
	where P.Id in (select distinct IdPracownika from Przypisania where Status = 1 and Od &lt;= @do and @od &lt;= ISNULL(Do, ''20990909''))
) D
pivot
(
	max(Zmiana) for Dzien in (' + @colsD + ')
) PV
'
exec sp_executesql @stmt2
        " />
--%>



<asp:SqlDataSource ID="dsEntities" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="select Id Value, Nazwa Text, 1 Sort from Dzialy where Status >= 0 order by Sort, Text" />
<asp:SqlDataSource ID="dsKlasyfikacje2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
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
    "/>

