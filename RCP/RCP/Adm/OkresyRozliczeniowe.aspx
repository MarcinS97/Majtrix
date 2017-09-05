<%@ Page Title="" Language="C#" MasterPageFile="~/RCP/RCP.master" AutoEventWireup="true" CodeBehind="OkresyRozliczeniowe.aspx.cs" Inherits="HRRcp.RCP.Adm.OkresyRozliczeniowe" %>

<%@ Register Src="~/Controls/PageTitle.ascx" TagPrefix="uc1" TagName="PageTitle" %>
<%@ Register Src="~/RCP/Controls/Adm/cntOkresy.ascx" TagName="AdmOkresy" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Adm/cntImportALARMUS.ascx" TagName="cntImportALARMUS" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Adm/cntImportABSENCJA.ascx" TagName="cntImportABSENCJA" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Adm/cntImportABSENCJA2.ascx" TagName="cntImportABSENCJA2" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/ImportStruktura.ascx" TagName="ImportStruktura" TagPrefix="uc1" %>
<%@ Register Src="~/RCP/Controls/cntCheckOkresClosed.ascx" TagPrefix="uc1" TagName="cntCheckOkresClosed" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="uc1" TagName="cntModal" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:PageTitle ID="PageTitle1" runat="server" Title="Okresy rozliczeniowe" SubText1="Zarządzanie okresami rozliczeniowymi, współpraca z systemem Kadrowo-Płacowym" />
    <div class="form-page pgOkresyRozl">
        <uc2:AdmOkresy ID="cntOkresy" runat="server" OnChanged="cntOkresy_Changed"/>

        <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table class="table0 paImportButtons narrow">
                    <tr>
                        <td class="left">
                            <uc3:cntImportABSENCJA ID="cntImportABSENCJA" runat="server" Visible="false" OnImportFinished="cntImportABSENCJA_ImportFinished" />
                            <uc2:cntImportABSENCJA2 ID="cntImportABSENCJA2" runat="server" Visible="false" OnImportFinished="cntImportABSENCJA_ImportFinished" Typ="7" />
                            <uc2:cntImportABSENCJA2 ID="cntImportUmowy" runat="server" Visible="false" OnImportFinished="cntImportABSENCJA_ImportFinished" Typ="8" />
                            <%-- <uc2:cntImportABSENCJA2 ID="cntImportABSENCJA" runat="server" Visible="false" OnImportFinished="cntImportABSENCJA_ImportFinished" Typ="6" />    --%>
                            <uc3:cntImportALARMUS ID="cntImportALARMUS1" runat="server" Visible="false" OnImportFinished="cntImportALARMUS1_ImportFinished" />
                            <uc1:ImportStruktura ID="ImportStruktura" OnImportFinished="ImportFinished" runat="server" Visible="false" />
                        </td>
                        <td class="right">
                            <div>
                                <asp:Label ID="lbInfo" runat="server"></asp:Label><br />
                                <asp:Button ID="btWeekClose" runat="server" CssClass="button250" Text="Zamknięcie tygodnia" OnClick="btWeekClose_Click" />
                                <asp:Button ID="btWeekOpen" runat="server" CssClass="button250" Text="Odblokowanie tygodnia" OnClick="btWeekOpen_Click" />
                                <div class="spacer8"></div>
                                <asp:Button ID="Button3" runat="server" CssClass="button250" Text="Import danych z systemu KP" ToolTip="Import absencji, stawek wynagrodzenia i kalendarza" OnClick="Button3_Click" Visible="false" />
                                <asp:Button ID="Button2" runat="server" CssClass="button250" Text="Zamknięcie miesiąca" OnClick="Button2_Click" />
                                <asp:Button ID="Button4" runat="server" CssClass="button250" Text="Odblokowanie miesiąca" Enabled="false" OnClick="Button4_Click" />
                                <hr />
                                <asp:Button ID="Button6" runat="server" CssClass="button250" Text="Import danych z systemu KP" ToolTip="Import danych pracowników, parametrów i ustawień z systemu Kadrowo-Płacowego" OnClick="Button6_Click" />
                                <asp:Button ID="Button1" runat="server" CssClass="button250" Text="Eksport planu pracy do systemu KP" ToolTip="Eksport danych planu pracy do systemu Kadrowo-Płacowego" OnClick="Button1_Click" />
                                <asp:Button ID="Button5" runat="server" CssClass="button250" Text="Eksport czasu pracy do systemu KP" ToolTip="Eksport danych czasu pracy do systemu Kadrowo-Płacowego" OnClick="Button5_Click" />
                                <div id="paPodzialLudzi" runat="server" visible="false" style="display: block;">
                                    <hr />
                                    <asp:Button ID="btPodzial" runat="server" CssClass="button250" Text="Podział Ludzi - Administracja" OnClick="btPodzial_Click" />
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <uc1:cntModal runat="server" ID="modalConfirmOkres" CloseButtonText="Anuluj" CssClass="modalConfirmOkres">
        <HeaderTemplate>
            <h4>
                <asp:Literal ID="lbExpZaOkresPlan" runat="server" Visible="false" Text="Potwierdź eksport harmonogramów czasu pracy za okres:"></asp:Literal>
                <asp:Literal ID="lbExpZaOkresAcc" runat="server" Visible="false" Text="Potwierdź eksport danych czasu pracy za okres:"></asp:Literal>
            </h4>
        </HeaderTemplate>
        <ContentTemplate>
            <asp:DropDownList ID="ddlExpZaOkres" runat="server" CssClass="form-control" DataSourceID="dsExpZaOkres" DataTextField="Text" DataValueField="Value" ></asp:DropDownList>
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btExpPlan" runat="server" CssClass="btn btn-default" Text="Eksportuj" Visible="false" OnClick="btExpPlan_Click" />
            <asp:Button ID="btExpAcc" runat="server" CssClass="btn btn-default" Text="Eksportuj" Visible="false" OnClick="btExpAcc_Click" />
        </FooterTemplate>
    </uc1:cntModal>

    <uc1:cntCheckOkresClosed runat="server" id="cntCheckOkresClosed" />

    <asp:SqlDataSource ID="dsGetKierNotClosedOld" runat="server"
        SelectCommand="
declare @od datetime
declare @do datetime
set @do = '{0}'
set @od = dbo.bom(@do)

select distinct R.IdKierownika as Id, ISNULL(K.Nazwisko + ' ' + K.Imie, ' Poziom główny struktury') as KierownikNI, K.Mailing, K.Email, K.Status  
from Przypisania R
left join Pracownicy K on K.Id = R.IdKierownika
left join PlanPracy PP on PP.IdPracownika = R.IdPracownika and PP.Data between dbo.MaxDate2(@od, R.Od) and dbo.MinDate2(@do, ISNULL(R.Do, '20990909'))
where R.Od &lt;= @do and @od &lt;= ISNULL(R.Do,'20990909') and R.Status = 1 and R.IdPracownika in (
	select distinct IdPracownika from PlanPracy where Data between @od and @do
)
and ISNULL(PP.Akceptacja, 0) = 0

order by KierownikNI                
        "></asp:SqlDataSource>

    <asp:SqlDataSource ID="dsGetKierNotClosed" runat="server"
        SelectCommand="
declare @od datetime
declare @do datetime
set @do = '{0}'
set @od = dbo.bom(@do)

select distinct R.IdKierownika as Id, ISNULL(K.Nazwisko + ' ' + K.Imie, ' Poziom główny struktury') as KierownikNI, K.Mailing, K.Email, K.Status  
from Przypisania R
left join Pracownicy K on K.Id = R.IdKierownika
left join PlanPracy PP on PP.IdPracownika = R.IdPracownika and PP.Data between dbo.MaxDate2(@od, R.Od) and dbo.MinDate2(@do, ISNULL(R.Do, '20990909'))
where R.Od &lt;= @do and @od &lt;= ISNULL(R.Do,'20990909') and R.Status = 1 and R.IdPracownika in (
	select distinct IdPracownika from PlanPracy where Data between @od and @do
)
and ISNULL(PP.Akceptacja, 0) = 0

union
----- brak PP ------
select --P.*,PR.RcpAlgorytm 
-- P.KadryId as Logo, P.Nazwisko + ' ' + P.Imie as Pracownik, P.KierKadryId, P.KierownikNI as Kierownik, P.Status, P.Opis, P.DataZatr, P.DataZwol
distinct R.IdKierownika as Id, ISNULL(K.Nazwisko + ' ' + K.Imie, ' Poziom główny struktury') as KierownikNI, K.Mailing, K.Email, K.Status  
from Pracownicy P
left join Przypisania R on R.IdPracownika = P.Id and @do between R.Od and ISNULL(R.Do,'20990909') and R.Status = 1 
left join Pracownicy K on K.Id = R.IdKierownika
left join PracownicyParametry PR on PR.IdPracownika = P.Id and @do between PR.Od and ISNULL(PR.Do, '20990909')
left outer join PlanPracy PP on PP.IdPracownika = P.Id and PP.Data between dbo.bom(@do) and dbo.eom(@do) and ISNULL(IdZmianyKorekta, IdZmiany) is not null
left join PlanUrlopowPomin X on X.IdPracownika = P.Id and @do between X.Od and ISNULL(X.Do, '20990909')
where PP.Id is null and P.status in (0,1) and P.KadryId between 0 and 80000-1
and X.Id is null
and R.IdKierownika is not null
and PR.RcpAlgorytm != 0
--order by Opis,Kierownik, Pracownik
--------------------
order by KierownikNI                
        "></asp:SqlDataSource>

    <asp:SqlDataSource ID="dsExpZaOkres" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
        SelectCommand="
select 
--  convert(varchar, Id) + '|' + O1.Od + '|' + O1.Do Value
  Id Value
/*
, case when MONTH(O.DataOd) = MONTH(O.DataDo) then 
       O1.Od + ' - ' + RIGHT(O1.Do, 2) 
  else O1.Od + ' - ' + RIGHT(O1.Do, 5) 
  end 
*/
, O1.Od + ' - ' + O1.Do
+ case when O.Status = 1 then ' (zamknięty)' else '' end
  Text
--, *
from OkresyRozl O
outer apply (select convert(varchar(10), DataOd, 20) Od, CONVERT(varchar(10), DataDo, 20) Do) O1
--where O.Status = 0
order by DataOd desc 
        ">
    </asp:SqlDataSource>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

