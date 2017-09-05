<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntWyliczenie.ascx.cs" Inherits="HRRcp.MatrycaSzkolen.Controls.Adm.cntWyliczenie" %>

<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="cc" TagName="DateEdit" %>

<div class="cntWyliczenie">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-inline">
                <div class="form-group">
                    <label>Od:</label>
                    <cc:DateEdit ID="deLeft" runat="server" ValidationGroup="cvg" />
                </div>
                <div class="form-group">
                    <label>Do:</label>
                    <cc:DateEdit ID="deRight" runat="server" />
                </div>
                <asp:Button ID="btnCalculate" runat="server" OnClick="btnCalculate_Click" CssClass="btn btn-sm btn-default" Text="Przelicz" ValidationGroup="cvg" />
            </div>
            <asp:Button ID="btnCalculateConfirm" runat="server" OnClick="btnCalculateConfirm_Click" CssClass="button_postback" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>



<asp:SqlDataSource ID="dsCalculate" runat="server" SelectCommand="
declare @od datetime = {0}
declare @do datetime = {1}

declare @today datetime = dbo.getdate(GETDATE())

select
  w.IdPracownika
, ls.IdStanowiska IdUprawnienia
/*, 0*/
/*, @today*/
, MAX((ISNULL(w0.Wynik, 0) + ISNULL(w1.Wynik, 0) + ISNULL(w2.Wynik, 0)) / 3) Ocena
into #aaa
from dbo.msGetWartosci(@od, @do) w
left join msLinieStanowiska ls on ls.IdLinii = w.IdLinii and @today between ls.Od and ISNULL(ls.Do, '20990909')
left join msWidelki w0 on w0.IdLinii = w.IdLinii and w0.Typ = 0 and @today between w0.DataOd and ISNULL(w0.DataDo, '20990909') and w.IloscGodzin between w0.Od and w0.Do
left join msWidelki w1 on w1.IdLinii = w.IdLinii and w1.Typ = 1 and @today between w1.DataOd and ISNULL(w1.DataDo, '20990909') and w.Wydajnosc between w1.Od and w1.Do
left join msWidelki w2 on w2.IdLinii = w.IdLinii and w2.Typ = 2 and @today between w2.DataOd and ISNULL(w2.DataDo, '20990909') and w.Braki between w2.Od and w2.Do
where ls.IdStanowiska is not null
group by w.IdPracownika, ls.IdStanowiska

update msOceny set
  DataDo = @today
from msOceny o
inner join #aaa a on a.IdPracownika = o.IdPracownika and a.IdUprawnienia = o.IdUprawnienia
where o.Status = 0 and @today between o.DataOd and ISNULL(o.DataDo, '20990909')

update msOceny set
  Status = 1
from msOceny o
inner join #aaa a on a.IdPracownika = o.IdPracownika and a.IdUprawnienia = o.IdUprawnienia
where o.Status = 2 and @today between o.DataOd and ISNULL(o.DataDo, '20990909')

insert into msOceny (IdPracownika, IdUprawnienia, Status, DataOd, Ocena, OkresOd, OkresDo, DataUtworzenia)
select IdPracownika, IdUprawnienia, 0, @today, Ocena, @od, @do, @today from #aaa

drop table #aaa
" />