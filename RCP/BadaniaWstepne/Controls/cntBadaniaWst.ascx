<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntBadaniaWst.ascx.cs" Inherits="HRRcp.BadaniaWstepne.Controls.cntBadaniaWst" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>
<%@ Register Src="~/Controls/Portal/cntSqlTabs.ascx" TagPrefix="uc1" TagName="cntSqlTabs" %>
<%@ Register Src="~/Controls/Przypisania/cntSplityWsp.ascx" TagPrefix="uc1" TagName="cntSplityWsp" %>
<%@ Register Src="~/BadaniaWstepne/Controls/cntDateZakr.ascx" TagPrefix="uc1" TagName="cntDateZakr" %>
<%@ Register Src="~/BadaniaWstepne/Controls/cntMenuZakres3.ascx" TagPrefix="uc1" TagName="cntMenuZakres" %>
<%--
<%@ Register Src="~/BadaniaWstepne/Controls/cntMenuZakres.ascx" TagPrefix="uc1" TagName="cntMenuZakres" %>
<%@ Register Src="~/BadaniaWstepne/Controls/cntMenuZakres2.ascx" TagPrefix="uc1" TagName="cntMenuZakres2" %>
--%>

<%@ Import Namespace="HRRcp.BadaniaWstepne.Templates" %>
<%@ Import Namespace="System.Collections.Generic" %>

<script language="C#" type="text/C#" runat="server">

    
    public void Page_Init(object sender, EventArgs e)
    {
        Dictionary<int, int> _PreColsH;
        CrColumnCreator[] _PreColumns;
        int[] _ReqCols;
        
        CreateColumns(out _PreColsH, out _PreColumns, out _ReqCols);
        this.BWCHandle.CreateColumns(_PreColumns, _PreColsH, _ReqCols);
    }




    public void CreateColumns(
        out Dictionary<int, int> _PreColsH,
        out CrColumnCreator[] _PreColumns,
        out int[] _ReqCols)
    {
        UC_DataColumnRightId = 17;
        UC_ZdjecieRightId = 16;
         
        _PreColsH = new Dictionary<int, int>();
        _PreColsH.Add(1, 1);
        var Tb50 = new CrTG0_TextBoxAll(50);
        var Tb50E = new CrTG0_TextBoxAll(50, true);
        var tb200 = new CrTG0_TextBoxAll(200);
        var TbN5 = new CrTG1_TextBoxNum(5, false, false);
        var TbN11 = new CrTG1_TextBoxNum(11, false, false);
        var TbU = new CrTG2_MultiLineTB(200, 3);
        var Cb = new CrTG3_CheckBox();
        var tbN3 = new CrTG1_TextBoxNum(3, false, false);
        var tbF5 = new CrTG1_TextBoxNum(5, false, true);
        //Columns.Add("Kody.Nazwa as KodText");
        //Joins.Add("LEFT JOIN Kody on Kody.id = Main.Class");
        var cl = new CrTG8_NullableDDL("select id [Value], Nazwa [Text] from Kody where Typ = 'PRACGRUPA' order by Lp",
            "KodText",
            "Kody.Nazwa as KodText",
            "LEFT JOIN Kody on Kody.id = Main.Class", null);
        var cc = new CrTG5_CC();
        var de = new CrTG6_DateEdit();
        var de2 = new CrTG11_DateEditZakr();
        var st = new CrTG7_DDLTextBox(@"select Id [Value], Nazwa [Text] from Stanowiska where Status >= 0 order by Nazwa");
        
        var pr = new CrTG8_NullableDDL(@"select Id [Value], Nazwisko + ' ' + Imie + ' (' + KadryId + ')' as [Text] from Pracownicy where Kierownik = 1 and Status >= 0 and KadryId < 80000 order by Nazwisko, Imie",
            "PrzelText",
            "BB.Nazwisko + ' ' + BB.Imie + ' (' + BB.KadryId + ')' as PrzelText",
            "LEFT JOIN Pracownicy BB on Main.PrzelId = BB.Id", null);

        var op = new CrTG8_NullableDDL(@"select Id [Value], Nazwisko + ' ' + Imie + ' (' + KadryId + ')' as [Text] from Pracownicy where dbo.GetRightId(Rights, 67) = 1 and Status >= 0 order by Nazwisko, Imie",
            "OpiekunText",
            "OP.Nazwisko + ' ' + OP.Imie + ' (' + OP.KadryId + ')' as OpiekunText",
            "LEFT JOIN Pracownicy OP on Main.OpiekunId = OP.Id", null);
        var rek = new CrTG8_NullableDDL(@"select Id [Value], Nazwisko + ' ' + Imie [Text] from Pracownicy where dbo.GetRightId(Rights, 64) = 1 and Status >= 0 order by Nazwisko, Imie",
            "RekruterText",
            "ISNULL(AA.Nazwisko + ' ' + AA.Imie, Rekruter) as RekruterText",
            "LEFT JOIN Pracownicy AA on Main.RekruterId = AA.Id", null);
        
        //select ROW_NUMBER() over (order by items desc) as Id, items from dbo.SplitStr('TAK,NIE',',')
        var itt = new CrTG8_NullableDDL(@"select LEFT(items,1) as Value, SUBSTRING(items,2,99) as Text from dbo.SplitStr('1TAK,0NIE',',')",
            "SzkolenieITTText",
            "case SzkolenieITT when 1 then 'TAK' when 0 then 'NIE' else null end as SzkolenieITTText",
            null, null);
        
        var typ = new CrTG12_DDL( 
            "select Kod [Value], Nazwa [Text] from Kody where Typ = 'BWTYP' order by Lp",
            "TypText",
            "K.Nazwa as TypText",
            "LEFT JOIN Kody K on ISNULL(Main.TypId,1) = K.Kod and K.Typ = 'BWTYP'", null);
        
        var ni = new CrTG9_NazwImie();
        //var fu = new CrTG10_FileUpload("~/BadaniaWstepne/images/");
        var fu = new CrTG10_FileUpload("~/Images/photos/");

        _ReqCols = new int[] { 1 };  // { 1,37 } predefiniowane wymagane kolumny
        
        _PreColumns = new CrColumnCreator[] {
            new CrColumnCreator(0, 17, "Data", "Zakr", de2, "data"),   

            new CrColumnCreator(5, 18, "Typ", "TypId", typ, "c312"),

            new CrColumnCreator(9, 1, "Imię", "Imie", Tb50E, "c312"),
            new CrColumnCreator(10, 1, "Nazwisko", "Nazwisko", Tb50E, "c312"),
            new CrColumnCreator(20, 2, "Nr Ewid.", "NrEw", TbN5, "c75"),
            new CrColumnCreator(30, 3, "Uwagi dotyczące badań", "UwagiBadania", TbU, "c312"),
            
            new CrColumnCreator(40, 4, "Numer identyfikatora", "IdOliwia", TbN5, "c75"),
            new CrColumnCreator(45, 14, "Numer RCP ID IT", "NrIT", TbN5, "c75"),

            new CrColumnCreator(50, 5, "Wniosek o zatrudnienie", "Zatrudnienie", Cb, "c75 check"),
            
            //new CrColumnCreator(60, 6, "Rekruter", "Rekruter", Tb50, "c312"),
            new CrColumnCreator(60, 6, "Rekruter", "RekruterId", rek, "c312"),

            new CrColumnCreator(70, 7, "Class", "Class", cl, "c312"),
            new CrColumnCreator(80, 8, "CC", "", cc, "tnw"),
            new CrColumnCreator(90, 9, "Data zatrudnienia", "DataZatr", de, "data"),
            new CrColumnCreator(100, 10, "Stanowisko", "Stanowisko", st, "c312"),
            new CrColumnCreator(110, 11, "Przełożony", "PrzelId", pr, "c312"),
            new CrColumnCreator(120, 12, "Data badań", "DataBadan", de, "data"),
            new CrColumnCreator(130, 13, "PESEL", "PESEL", TbN11, "c312"),
            new CrColumnCreator(150, 15, "Status", "Status", Tb50, "c312"),
            new CrColumnCreator(160, 16, "Zdjęcie", "Zdjecie", fu, "img"),
            
            new CrColumnCreator(1, 1, "Pracownik", "CrTG9NazwImie", ni, "c312"),

            //----- staże i praktyki -----            
            /*
            new CrColumnCreator(400, 19, "Szkoła/Uczelnia", "Szkola", tb200, "c312"),
            new CrColumnCreator(410, 20, "Program", "Program", tb200, "c312"),
            new CrColumnCreator(420, 21, "Rozpoczęcie", "DataRozp", de, "data"),
            new CrColumnCreator(430, 22, "Zakończenie", "DataZak", de, "data"),
            new CrColumnCreator(440, 23, "Czas trwania", "CzasTrwania", TbN5, "c75"),
            new CrColumnCreator(450, 24, "Kierunek", "Kierunek", tb200, "c312"),
            new CrColumnCreator(460, 25, "Dział", "Dzial", tb200, "c312"),
            new CrColumnCreator(470, 26, "Opiekun", "OpiekunId", op, "c312"),
            new CrColumnCreator(480, 27, "Klient", "Klient", tb200, "c312"),
            new CrColumnCreator(490, 28, "Pretest", "Pretest", tbN3, "c75"),
            new CrColumnCreator(500, 29, "Posttest", "Posttest", tbN3, "c75"),
            new CrColumnCreator(510, 30, "Coach", "Coach", tbF5, "c75"),
            new CrColumnCreator(520, 31, "Obszar 1", "Obszar1", tbF5, "c75"),
            new CrColumnCreator(530, 32, "Obszar 2", "Obszar2", tbF5, "c75"),
            new CrColumnCreator(540, 33, "Obszar 3", "Obszar3", tbF5, "c75"),
            new CrColumnCreator(550, 34, "E-mail", "Email", Tb50E, "c312"),
            new CrColumnCreator(560, 35, "Telefon", "Telefon", Tb50E, "c312"),
            new CrColumnCreator(570, 36, "Komentarz", "Komentarz", tb200, "c312"),
            new CrColumnCreator(580, 37, "Nagroda", "Nagroda", Cb, "c75 check")
            */
            new CrColumnCreator(115, 38, "Szkolenie ITT", "SzkolenieITT", itt, "c75"),
        };
    }

    //public const int lastColumnRight = 38; nie może przekroczyć 100
</script>

<script type="text/javascript">
    var LastUpdateId;
    var NewDataButtonId;
    function InitNDButton(bID)
    {
        NewDataButtonId = bID;
    }
    function OnCntBadUpdate(newId)
    {
        LastUpdateId = newId;
        $("#" + NewDataButtonId).click();
    }
    function IncrLUI()
    {
        LastUpdateId = (parseInt(LastUpdateId) + 1).toString();
    }

    function initLastUpdateId() {
        callAjax2(svcUrl, 'GetcntBadLastId', {},
            function(msg) {
                LastUpdateId = msg.d;
            }
        );
    }

    $(function () {
        setInterval(function () {
            callAjax2(svcUrl, 'GetcntBadLastId', {},
                function (msg) {
                    if (LastUpdateId != msg.d) {
                        OnCntBadUpdate(msg.d);
                    }
                }
            );
        }, 5000);
    });
</script>

<div class="cntBadaniaWst">
    <%-- panel wyszukiwania --%>
    <div id="paFilter1" runat="server" class="paFilter" visible="true">
        <div class="left">
            <asp:TextBox ID="ImNazFil" CssClass="search textbox" runat="server" MaxLength="100"/>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="caption">
                <tr>
                    <td class="left">
                        <span class="caption4">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/captions/layout_edit.png"/>
                            Badania wstępne
                        </span>
                    </td>
                    <td class="left1">
                        <asp:Label ID="Label9" runat="server" class="label" Text="Wyszukaj:" />
                    </td>
                    <td class="middle">
                        <div id="FilterBasic" visible="true" class="paFilter3" runat="server">
                            <div class="filterspacer"></div>
                            <asp:Button ID="btClear" CssClass="button75" Text="Czyść" runat="server" />
                            <asp:Button ID="buttSAdv" CssClass="button" Text="Zaawansowane" runat="server" OnClick="buttSAdv_Click" />
                            <asp:Button ID="btExcel" CssClass="button75" Text="Excel" runat="server" OnClick="btExcel_Click" />
                        </div>
                        <div id="FilterAdv" visible="false" class="paFilter2 ItemContainer" runat="server" >
                            <asp:PlaceHolder ID="PHFilter" runat="server" />
                            <div class="bottom_buttons">
                                <asp:Button ID="btSearchAdv" CssClass="button75" Text="Szukaj" runat="server" OnClick="btSearchAdv_Click" />
                                <asp:Button ID="Button1" CssClass="button75" Text="Czyść" runat="server" OnClick="btClear_Click" />
                                <asp:Button ID="buttSBasic" CssClass="button75" Text="Ukryj" runat="server" OnClick="buttSBasic_Click" />
                            </div>
                        </div>
                    </td>
                    <td class="right">
                        <asp:Button ID="btImportAsk" CssClass="button100" Text="Import" runat="server" OnClick="btImport_Click" />
                        <asp:Button ID="btImportAsk2" CssClass="button_postback" runat="server" OnClick="btImport_Click" />
                        <asp:Button ID="btImportBig" CssClass="button_postback" runat="server" OnClick="btImport_Click" />
                        <asp:Button ID="btImport" CssClass="button_postback" runat="server" OnClick="btImport_Click" />
                        <asp:Button ID="btAdmin" CssClass="button100" Text="Administracja" runat="server" OnClick="btAdmin_Click" />
                    </td>
                </tr>
            </table>
    <%--------------------------%>
    <%-- old --
    <asp:TextBox CssClass="search textbox" ID="ImNazFil" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="FilterBasic" visible="true" class="paFilter3" runat="server">
                <span class="label">Wyszukaj pracownika:</span>
                <asp:Button ID="btClear" CssClass="button75" Text="Czysc" OnClick="btClear_Click" runat="server" />
                <asp:Button ID="buttSAdv" CssClass="button75" Text="Filtr" runat="server" OnClick="buttSAdv_Click" />
            </div>
            <div id="FilterAdv" visible="false" class="paFilter2 ItemContainer" runat="server" >
                <asp:PlaceHolder ID="PHFilter" runat="server" />
                <asp:Button ID="btSearchAdv" CssClass="button75" Text="Szukaj" runat="server" OnClick="btSearchAdv_Click" />
                <asp:Button CssClass="button75" Text="Czysc" OnClick="btClear_Click" runat="server" />
                <asp:Button ID="buttSBasic" CssClass="button75" Text="Ukryj" runat="server" OnClick="buttSBasic_Click" />
            </div>
    --%>

            <div style="display: none;">
                <asp:Label runat="server" ID="imgLoader" CssClass="fileupload" style="display:none;" ><img alt="" src="../../images/uploading.gif" /></asp:Label> 
                <asp:AsyncFileUpload ID="AsyncFileUpload2" runat="server" CssClass="fileupload" 
                    ToolTip="Wybierz plik" 
                    UploadingBackColor="#8FDB3C" CompleteBackColor="#8FDB3C"
                    UploaderStyle="Traditional" ThrobberID="imgLoader"
                    />
            </div>

            <asp:Button ID="btSearch" runat="server" CssClass="button_postback" Text="Wyszukaj" onclick="btSearch_Click" />
            <asp:Button ID="btNewData" CssClass="button_postback" runat="server" OnLoad="btNewData_Load" OnClick="btNewData_Click" />
    

            <%-- page --%>
            <div class="pageContent">
                <div class="padding">
            
            <div>
                <asp:Button ID="btAdd" Text="Dodaj pracownika" CssClass="btAdd button150" runat="server" OnClick="btAdd_Click"/>
                
            </div>
            <uc1:cntMenuZakres id="MenuZakres1" DefOffset="-2" ZakrType="Day" VisibleItems="8" OnSelectedItemChanged="MenuZakres1_SelectedItemChanged" runat="server" />
            <asp:ListView ID="ListView1" DataKeyNames="id" DataSourceID="SqlDataSource1" runat="server"
                            OnItemUpdating="ListView1_ItemUpdating" 
                            OnItemInserting="ListView1_ItemInserting"
                            OnItemDeleting="ListView1_ItemDeleting"
                            OnItemEditing="ListView1_ItemEditing"
                            OnItemDataBound="ListView1_ItemDataBound"
                            OnItemUpdated="ListView1_ItemUpdated"
                            OnItemInserted="ListView1_ItemInserted"
                            OnItemDeleted="ListView1_ItemDeleted"
                            OnItemCommand="ListView1_ItemCommand"
                            OnSorting="ListView1_Sorting"
                            OnSorted="ListView1_Sorted"
                            OnDataBound="ListView1_DataBound"
                            >
                <%--OnLayoutCreated="ListView1_LayoutCreated" Nie moze tu byc tego eventu musze go dodac recznie tak by byl po Tools.PrepareDicListView--%>
                <EmptyDataTemplate>
                    <table class="empty table0">
                        <tr id="DynCols" style="display: none;" oninit="DynCols_Init" runat="server" /> <%-- Bez LinkButton'ow PrepareSorting traci sortowana kolumne --%>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Brak danych" />
                                <%--<asp:Button ID="EDButtAdd" Text="Dodaj pracownika" CssClass="button150" CommandName="NewRecord" runat="server" />--%>
                            </td>
<%--
                            <th class="control">
                                <asp:Button ID="EDButtAdd" Text="Dodaj" CommandName="NewRecord" runat="server" />
                            </th>
--%>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <table runat="server">
                        <tr>
                            <td>
                                <table id="itemPlaceholderContainer" runat="server">
                                    <tr id="DynCols" oninit="DynCols_Init" runat="server" /> <%-- Wszystkie th z nazwami kolumn + columna z przyciskiem dodaj --%>
                                    <tr id="itemPlaceHolder" runat="server" />                                    
                                </table>
                            </td>
                        </tr>
                        <tr class="pager">
                            <td class="left">
                                <asp:DataPager ID="DataPager1" OnLoad="DataPager1_Load" runat="server" PageSize="20">
                                    <Fields>
                                        <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="true" ShowLastPageButton="false" ShowPreviousPageButton="true" ShowNextPageButton="false" 
                                            PreviousPageText="Poprzednia" FirstPageText="Pierwsza"/>
                                        <asp:NumericPagerField ButtonType="Link" />
                                        <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="nav" ShowFirstPageButton="false" ShowLastPageButton="true" ShowPreviousPageButton="false" ShowNextPageButton="true" 
                                            NextPageText="Następna" LastPageText="Ostatnia"/>
                                    </Fields>
                                </asp:DataPager>
                            </td>
                            <td class="right">
                                <span class="count">Ilość:<asp:Label id="lbCount" runat="server" /></span>
                                &nbsp;&nbsp;&nbsp;
                                <span class="count">Pokaż na stronie:</span>
                                <asp:DropDownList ID="ddlLines" OnSelectedIndexChanged="ddlLines_SelectedIndexChanged" runat="server" />
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
            <asp:Literal ID="LNoCols" Visible="false" Text="Nie posiadasz uprawnień do wyświetlenia danych. Skontaktuj się z Administatorem." runat="server" />


                </div>
            </div>


        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btExcel" />
        </Triggers>
    </asp:UpdatePanel>
    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
        OnInserted="SqlDataSource1_Inserted"
        SelectCommand="SELECT {3} FROM (SELECT {0},Main.* FROM BadaniaWst as Main {1}) Main {2}"
        OnFiltering="SqlDataSource1_Filtering"
        >
        <InsertParameters>
            <asp:Parameter Name="LID" Direction="Output" Type="Int32" DefaultValue="0" />
        </InsertParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSourceImport" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
        SelectCommand="
select P.Nazwisko + ' ' + P.Imie + ISNULL(' (' + P.KadryId + ')', '') as Pracownik, B.Id, P.Id as IdPracownika, P.DataZatr, B.PrzelId, B.NrIT
from VPrzypisaniaNaDzis P 
inner join BadaniaWst B on B.Pesel = P.Nick
where P.KierId is null and P.Status &gt;= 0 and P.DataZwol is null
and B.PrzelId is not null        
        "
        UpdateCommand="
BEGIN TRANSACTION;

BEGIN TRY
------------------------
declare @lastid int, @cnt int, @step int 
set @cnt = 0

set @step = 1
select B.Id, P.Id as IdPracownika, P.DataZatr, B.PrzelId, B.NrIT
into #bbb
from VPrzypisaniaNaDzis P 
inner join BadaniaWst B on B.Pesel = P.Nick
where P.KierId is null and P.Status &gt;= 0 and P.DataZwol is null
and B.PrzelId is not null 
set @cnt = @@ROWCOUNT

set @step = 2
select @lastid = max(Id) from Przypisania

set @step = 3

insert into Przypisania
select IdPracownika, DataZatr, null, 
PrzelId,
null, null, null, null, null, null, null, null, GETDATE(), convert(varchar,Id), 1, 1, null, null, null, null
from #bbb

set @step = 4

insert into SplityWspP
select R.Id, W.IdCC, W.Wsp 
from Przypisania R
left join SplityWspB W on W.IdPRzypisania = R.UwagiAcc
where R.Id &gt; @lastid and R.UwagiAcc in (select convert(varchar,Id) from #bbb)

set @step = 5

insert into PracownicyKarty (IdPracownika, Od, Do, RcpId, NrKarty)
select P.IdPracownika, P.DataZatr, null, B.NrIT, null 
from #bbb P
left join BadaniaWst B on B.Id = P.Id
where B.NrIT is not null

drop table #bbb

------------------------
END TRY
BEGIN CATCH
	select -1 as Error, ERROR_MESSAGE() AS ErrorMessage, @step as Step, @cnt as PracCount, @lastid as PrzLastId;
	/*
    SELECT 
         ERROR_NUMBER() AS ErrorNumber
        ,ERROR_SEVERITY() AS ErrorSeverity
        ,ERROR_STATE() AS ErrorState
        ,ERROR_PROCEDURE() AS ErrorProcedure
        ,ERROR_LINE() AS ErrorLine
        ,ERROR_MESSAGE() AS ErrorMessage;
	*/
    IF @@TRANCOUNT &gt; 0
        ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT &gt; 0 BEGIN
    COMMIT TRANSACTION;
    select 0 as Error, null as ErrorMessage, @step as Step, @cnt as PracCount, @lastid as PrzLastId
END    
        ">
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="SqlDataSourceDupRcpId" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
        SelectCommand="
-- zduplikowane RcpId
select
P.NAzwisko + ' ' + P.Imie as Pracownik, P.Status,
P.KadryId, P.Opis, PK.RcpID, PK.Od, PK.Do
from Pracownicykarty PK 
left join PRacownicy P on P.Id = PK.IdPracownika
where GETDATE() between PK.Od and ISNULL(PK.Do, '20990909')
and PK.RcpId in 
(
	select B.RcpId from
	(
		select distinct A.RcpId,A.IdPracownika from PracownicyKarty A
		inner join Pracownicy P on P.Id = A.IdPracownika and P.Status &gt;= 0
		where A.Do is null
	) B
	group by B.RcpId
	having count(*) &gt; 1
) 
order by RcpId, Pracownik, PK.Od
        ">
    </asp:SqlDataSource>
    
</div>