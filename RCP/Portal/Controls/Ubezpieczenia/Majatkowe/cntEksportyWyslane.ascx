<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntEksportyWyslane.ascx.cs" Inherits="HRRcp.Portal.Controls.Ubezpieczenia.Majatkowe.cntEksportyWyslane" %>

<div id="paEksportyWyslane" runat="server" class="cntEksportyWyslane">
    <asp:Button ID="btExport" runat="server" CssClass="btn btn-primary" Text="Wyślij aktualne zestawienie" OnClick="btExport_Click" />
    <asp:GridView ID="gvEksporty" CssClass="GridView1 table" runat="server" DataSourceID="dsEksporty" AllowSorting="true" AllowPaging="true" PageSize="5" PagerSettings-Mode="NumericFirstLast"
        ></asp:GridView>
    <asp:Button ID="gvEksportyCmd" runat="server" CssClass="button_postback" OnClick="gvEksportyCmd_Click"/>
    <asp:HiddenField ID="gvEksportyCmdPar" runat="server" />
</div>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Button ID="btDownload" runat="server" CssClass="button_postback" Text="Pobierz" OnClick="btDownload_Click"/>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btDownload"/>
    </Triggers>
</asp:UpdatePanel>

<asp:SqlDataSource ID="dsEksporty" runat="server" ConnectionString="<%$ ConnectionStrings:PORTAL %>"  
    SelectCommand="
select 
  R.Id [id:-]
, R.Data [Data wysyłki:DT]
, R.Email [Adres e-mail]
, R.Nazwa [Plik:control;|cmd:download @id|Pobierz plik]
, 'wyślij ponownie' [Akcja:control;|cmd:resend @id|Wyślij ponownie]      --glyphicon glyphicon-envelope
from RaportyWysylki R
left join RaportyScheduler S on S.Id = R.IdRaportyScheduler
where S.Typ = 50
order by R.Data desc, S.IdPracownika
    " >
    <SelectParameters>
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsResend" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"  
    SelectCommand="
select top 1
-- mail --
12,
0 as IdPracownika, 
null Nazwisko,
null Imie, 
1 Mailing, 
R.Email Email, 
null as cc, 
null as bcc, 
null as Zalaczniki, 
M.Typ,
M.Aktywny,
M.Temat, 
M.Tresc
, 1 as ReportId
, R.Nazwa ReportName
, '~/Export' ReportPath
--, R.Plik
, convert(varchar(7), Data, 20) MIESIAC
from {1}..RaportyWysylki R  -- DB_PORTAL
inner join Mailing M on M.Grupa = 'UBEZPIECZENIA' and M.Typ = 'EXPORT' 
where R.Id = {0}
    " >
    <SelectParameters>
    </SelectParameters>
</asp:SqlDataSource>
