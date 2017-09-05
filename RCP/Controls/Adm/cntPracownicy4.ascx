<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPracownicy4.ascx.cs" Inherits="HRRcp.Controls.Adm.cntPracownicy4" %>
<%@ Register Src="~/Controls/Portal/cntSqlTabs.ascx" TagPrefix="uc1" TagName="cntSqlTabs" %>
<%@ Register Src="~/Controls/Reports/cntFilter2.ascx" TagPrefix="uc1" TagName="cntFilter2" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>


<div id="paPracownicy4" runat="server" class="cntPracownicy4">
    <asp:HiddenField ID="hidNaDzien" runat="server" Visible="false"/>
    <div id="paFilter" runat="server" class="filter" >
        <div class="row">
            <div class="col-md-4">
                <div class="input-group">
                    <asp:TextBox ID="tbSearch" CssClass="form-control" runat="server" PlaceHolder="Wyszukaj..."></asp:TextBox>
                    <div class="input-group-btn">
                        <asp:Button ID="btClear" runat="server" CssClass="btn btn-default" Text="Czyść" />
                    </div>
                </div>
            </div>
        </div>
        <uc1:cntFilter2 runat="server" ID="cntFilter" ReportId="10000" OnFilter="cntFilter_Filter" />
    </div>    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <asp:Button ID="btSearch" runat="server" CssClass="button_postback" Text="Wyszukaj" onclick="btSearch_Click" />
            <div class="tabs">
                <uc1:cntSqlTabs runat="server" ID="tabFilter" CssClass="left" Grupa="PRACFILTER" OnDataBound="tabFilter_DataBound" Items="
Zatrudnieni|1
BYD|11
ZG|12
CETOR|13
Przełożeni|2
Nieprzypisani|3
BYD|31
ZG|32
Nowi|4
Brak RcpId|5
Zwolnieni|-1
Pomijani|-2
Wszyscy|99
"
                    />
                <uc1:cntSqlTabs runat="server" ID="tabMode" CssClass="right" Items="Przypisanie|1|Uprawnienia|2"/>
            </div>
            <asp:GridView ID="gvPracownicy" CssClass="tbPracownicy GridView1" runat="server" DataSourceID="dsPracownicy" AllowSorting="true" AllowPaging="true" PageSize="20" 
                OnInit="gvPracownicy_Init"
                OnLoad="gvPracownicy_Load"
                OnDataBinding="gvPracownicy_DataBinding"
                OnDataBound="gvPracownicy_DataBound"
                ></asp:GridView>
            <asp:Button ID="gvPracownicyCmd" runat="server" CssClass="button_postback" OnClick="gvPracownicyCmd_Click"/>
            <asp:HiddenField ID="gvPracownicyCmdPar" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <uc1:cntModal runat="server" ID="modalEdit" Backdrop="false" Keyboard="false" CloseButtonText="Anuluj" >
        <ContentTemplate>

        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btSave" runat="server" CssClass="btn btn-default" Text="Zapisz" OnClick="btSave_Click"/>
        </FooterTemplate>
    </uc1:cntModal>
</div>

<asp:SqlDataSource ID="dsPracownicy" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" OnSelected="dsPracownicy_Selected" 
    SelectCommand="
select
  P.Id [id:-]
, 'Wybierz' [:;select|cmd:select @id]
, P1.NazwiskoImie [Pracownik]
, P.KadryId [Nr ewid.]
, P.KadryId2 [Nr ewid.2]
, P.Kierownik [Kier]
, P.Mailing [@]
, P1.StatusNazwa
, P.DataZatr [Data zatrudnienia:D]
, P.DataZwol [Data zwolnienia:D]
/*
, PK.RcpId [RCP;ID]
, PK.NrKarty [RCP;Nr karty]
, S.Nazwa [RCP;Strefa]
, KO.Nazwa [RCP;Algorytm]
*/
, PK.RcpId [RCP ID]
, PK.NrKarty [Nr karty]
, S.Nazwa [Strefa]
, KO.Nazwa [Algorytm]

, K1.Przelozony [Przełożony]
, C.Commodity [Commodity]
, A.Area [Area]
, PO.Position [Position]
, D.Nazwa [Dział]
, T.Nazwa [Stanowisko]
, 'Edycja' [:;control|cmd:edit @id]
from Pracownicy P 
outer apply (select 
    RTRIM(P.Nazwisko) + ' ' + RTRIM(P.Imie) as NazwiskoImie
  , case P.Status 
        when  0 then 'Zatrudniony'
        when  1 then 'Nowy'
        when -1 then 'Zwolniony'
        when -2 then 'Pomiń'
        else 'Inny ' + convert(varchar, P.Status)
        end StatusNazwa 
  , ISNULL(P.Opis, '') Opis) P1

outer apply (select top 1 * from PracownicyKarty where IdPracownika = P.Id and 
    Od &lt;= dbo.MaxDate2(P.DataZatr, @NaDzien)
    order by Od desc) as PK
outer apply (select top 1 * from PracownicyParametry where IdPracownika = P.Id and 
    Od &lt;= dbo.MaxDate2(P.DataZatr, @NaDzien)
    order by Od desc) as PP
outer apply (select top 1 * from PracownicyStanowiska where IdPracownika = P.Id and 
    Od &lt;= dbo.MaxDate2(P.DataZatr, @NaDzien)
    order by Od desc) as PS
outer apply (select top 1 * from Przypisania where IdPracownika = P.Id and Status = 1 and 
    Od &lt;= dbo.MaxDate2(P.DataZatr, @NaDzien)
    order by Od desc) as R
outer apply (select 
    ISNULL(R.Do, R.DoMonit) as PrzypisanieDo
  , ISNULL(R.Do, ISNULL(R.DoMonit,'20990909')) as PrzypisanieDo2) R1

left outer join Pracownicy K on K.Id = R.IdKierownika
outer apply (select
    case when R.IdKierownika = 0 then 'Główny poziom struktury'
    else RTRIM(K.Nazwisko) + ' ' + RTRIM(K.Imie) end as Przelozony
) K1

left outer join Stanowiska T on T.Id = PS.IdStanowiska
left outer join Dzialy D on D.Id = PS.IdDzialu
left outer join Commodity C on C.id = R.IdCommodity
left outer join Area A on A.Id = R.IdArea
left outer join Position PO on PO.Id = R.IdPosition
left outer join Strefy S on S.Id = R.RcpStrefaId
left outer join Kody KO on KO.Typ = 'ALG' and KO.Kod = PP.RcpAlgorytm

--outer apply (select top 1 * from PodzialLudziImport where KadryId = P.KadryId order by Miesiac desc) as PL
where 
    (
    (@filter =  1 and P.Status in (0,1))     --Zatrudnieni
 or (@filter =  2 and P.Status in (0,1) and P.Kierownik = 1)        --Przełożeni
 or (@filter =  3 and P.Status in (0,1) and (R.Id is null or R1.PrzypisanieDo2 &lt; P.DataZatr))    --Nieprzypisani
 or (@filter =  4 and P.Status = 1)         --Nowi
 or (@filter =  5 and P.Status in (0,1) and PK.RcpId is null)   --Brak RcpId
 or (@filter = -1 and P.Status = -1)        --Zwolnieni
 or (@filter = -2 and P.Status = -2)        --Pomijani
 or (@filter = 99)
{0}
    )
order by P.Nazwisko, P.Imie
    " OnSelecting="dsPracownicy_Selecting" OnLoad="dsPracownicy_Load"
    
    FilterExpression="Pracownik like 'd%'"
    
    >
    <SelectParameters>
        <asp:ControlParameter ControlID="hidNaDzien" Name="NaDzien" PropertyName="Value" Type="DateTime" />
        <asp:ControlParameter ControlID="tabFilter" Name="filter" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsFilterIQOR" runat="server" 
    SelectCommand="
 or (@filter = 11 and P.Status in (0,1) and P.KadryId between 0 and 80000 - 1 and P1.Opis != 'Zielona Góra')   --BYD
 or (@filter = 12 and P.Status in (0,1) and P.KadryId between 0 and 80000 - 1 and P1.Opis  = 'Zielona Góra')   --ZG
 or (@filter = 12 and P.KadryId between 80000 and 90000 - 1)        --CETOR
 or (@filter = 31 and P.Status in (0,1) and (R.Id is null or R1.PrzypisanieDo2 &lt; P.DataZatr) and P1.Opis != 'Zielona Góra')    --BYD
 or (@filter = 32 and P.Status in (0,1) and (R.Id is null or R1.PrzypisanieDo2 &lt; P.DataZatr) and P1.Opis  = 'Zielona Góra')    --ZG
 or (@filter = 41 and P.Status = 1 and P1.Opis != 'Zielona Góra')   --BYD
 or (@filter = 42 and P.Status = 1 and P1.Opis  = 'Zielona Góra')   --ZG
    ">
</asp:SqlDataSource>
