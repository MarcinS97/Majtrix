<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntRejestracjaRCP.ascx.cs" Inherits="HRRcp.Controls.cntRejestracjaRCP" %>

<center>
    <div style="margin-top:10%">
        <div style="background-color:rgb(234,234,234) ; margin-left:15%; margin-right:15%; padding-bottom:25px; padding-top:25px; margin-top:15%;">
            <p style="margin-top:1%;">
                <asp:Label Font-Size="X-Large" runat="server" Text="Witaj" ID="Label1"></asp:Label>
            </p>
            <p>
                <asp:Label Font-Size="X-Large" runat="server" Text="Naciśnij przycisk poniżej aby zarejestrować się w systemie RCP." ID="Label2"></asp:Label>
            </p>    
            <asp:Button ID="Button1" runat="server" Text="Zarejestruj" OnClick="Button1_Click" CssClass="btn btn-primary btn-lg btn-rejestracja" />
        </div>
    </div>
</center>

<asp:SqlDataSource ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" ID="dsRejestruj" runat="server" SelectCommand="
declare @pracId int = {0}
--declare @nadzien datetime = dbo.getdate(DATEADD(HOUR, -6, GETDATE()))
declare @nadzien datetime = '{1}'

/*select
  DATEADD(SECOND, DATEDIFF(SECOND, dbo.getdate(z.Od), z.Od), @nadzien)
, DATEADD(SECOND, DATEDIFF(SECOND, dbo.getdate(z.Od), z.Od) + pp.Wymiar, @nadzien)
from PlanPracy pp
inner join Zmiany z on z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
where pp.IdPracownika = @pracId and pp.Data = @nadzien*/

insert RCP (Czas, ECCode, ECUserId, ECReaderId, ECDoorType, InOut, Duty)
select
  case x.x when 0 then DATEADD(SECOND, DATEDIFF(SECOND, dbo.getdate(z.Od), z.Od), @nadzien) when 1 then DATEADD(SECOND, ISNULL(DATEDIFF(SECOND, dbo.getdate(z.Od), z.Od) + pp.Wymiar, DATEDIFF(SECOND,  dbo.getdate(z.Do), z.Do)), @nadzien) end Czas
, 1 ECCode
, pk.RcpId ECUserId
, 1000 + 128 * x.x ECReaderId
, 32 ECDoorType
, x.x InOut
, 0 Duty
from PlanPracy pp
inner join Zmiany z on z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
inner join PracownicyKarty pk on @nadzien between pk.Od and ISNULL(pk.Do, '20990909') and pk.IdPracownika = @pracId
cross join (select 0 x union select 1 x) x
where pp.IdPracownika = @pracId and pp.Data = @nadzien
order by x.x"
    InsertCommand="
    declare @pracId int = {0}
--declare @nadzien datetime = dbo.getdate(DATEADD(HOUR, -6, GETDATE()))
declare @nadzien datetime = '{1}'
select  r.Czas, r.ECCode, r.ECUserId, r.ECReaderId, r.ECDoorType, r.InOut, r.Duty from Rcp r
left join PlanPracy pp on r.ECUserId = pp.IdPracownika
inner join Zmiany z on z.Id = ISNULL(pp.IdZmianyKorekta, pp.IdZmiany)
inner join PracownicyKarty pk on @nadzien between pk.Od and ISNULL(pk.Do, '20990909') and pk.IdPracownika = @pracId
cross join (select 0 x union select 1 x) x
where pp.IdPracownika = @pracId and pp.Data = @nadzien
    "
    />
