<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntReportSchedulerEdit.ascx.cs" Inherits="HRRcp.Controls.Reports.cntReportSchedulerEdit" %>
<%@ Register Src="~/Portal/Controls/dbField.ascx" TagPrefix="uc1" TagName="dbField" %>
<%@ Register Assembly="HRRcp" Namespace="HRRcp.Controls.Portal" TagPrefix="cc1" %>

<div id="paReportSchedulerEdit" runat="server" class="cntReportSchedulerEdit">
    <asp:HiddenField ID="hidGrupa" runat="server" Visible="false" />
    <div class="data">
        <uc1:dbField runat="server" ID="Email" Type="tb" MaxLength="2000" Rq="false" StVisible="3" 
                Label="Wyślij na adres e-mail:"/>
        <uc1:dbField runat="server" ID="cc" Type="tb" MaxLength="2000" Rq="false" StVisible="3" 
                Label="Wyślij do wiadomości (cc):"/>
        <uc1:dbField runat="server" ID="bcc" Type="tb" MaxLength="2000" Rq="false" StVisible="3" 
                Label="Wyślij do ukrytej wiadomości (bcc):"/>

        <uc1:dbField runat="server" ID="IdRaportu" ValueField="Nazwa" Type="ddl" DataSourceID="dsRaporty" Wybierz="true" Rq="true" StVisible="3"  
                Label="Raport:"/>
        <uc1:dbField runat="server" ID="Parametry" Type="tb" StVisible="3" 
                Label="Parametry:"/>

        <uc1:dbField runat="server" ID="DataStartu" Type="date" Rq="true" StVisible="3" 
                Label="Data startu:"/>
        <uc1:dbField runat="server" ID="DataStopu" Type="date" StVisible="3" 
                Label="Data końca:"/>

        <uc1:dbField runat="server" ID="InterwalTyp" Type="ddl" ValueField="Text" DataSourceID="dsInterwaly" Wybierz="true" Rq="true" StVisible="3" 
                Label="Powtarzaj co:" OnChanged="InterwalTyp_Changed" />
        <uc1:dbField runat="server" ID="Interwal" Type="tb" Visible="false" Rq="true" MaxLength="3" Min="0" Max="366" ValidChars="0123456789" StVisible="3" 
                Label="Ilość:"/>

        <uc1:dbField runat="server" ID="Aktywny" Type="check" StVisible="3" 
                Label="Aktywny:"/>
    </div>
    <div id="paButtons" runat="server" class="buttons">
        <cc1:WnButton ID="wbtSave"   CssClass="btn btn-default" runat="server" Text="Zapisz" onclick="wbtSave_Click" StVisible="3" />
        <cc1:WnButton ID="wbtCancel" CssClass="btn btn-default" runat="server" Text="Anuluj" onclick="wbtCancel_Click" StVisible="3" />
        <cc1:WnButton ID="wbtEdit"   CssClass="btn btn-default" runat="server" Text="Edycja" onclick="wbtEdit_Click" StVisible="2" />        
    </div>
</div>

<asp:SqlDataSource ID="dsRaportyScheduler" runat="server" 
    SelectCommand="
select top 1 
S.*, 
--U.LastName + ' ' + U.FirstName as UserName, U.Email as UserEmail,
U.Nazwisko + ' ' + U.Imie as UserName, U.Email as UserEmail,
R.MenuText as RaportNazwa, R.ToolTip as RaportOpis,
case 
when InterwalTyp is null then 'Jednorazowo'
when InterwalTyp in ('HH','HOUR') then 'Godzin: ' + convert(varchar, S.Interwal)
when InterwalTyp in ('D','DD','DAY') then 'Dni: ' + convert(varchar, S.Interwal)
when InterwalTyp in ('WW','WEEK') then 'Tygodnie: ' + convert(varchar, S.Interwal)
when InterwalTyp in ('M','MM','MONTH') then 'Miesiące: ' + convert(varchar, S.Interwal)
when InterwalTyp in ('LM') then 'Ostatniego dnia miesiąca'
else InterwalTyp + ': ' + convert(varchar, S.Interwal)
end as InterwalOpis
from RaportyScheduler S 
--left join AspNetUsers U on U.Id = S.UserId
left join Pracownicy U on U.Id = S.UserId
left join {2}SqlMenu R on R.Id = S.IdRaportu
where {0} = {1}
    "/>

<asp:SqlDataSource ID="dsRaporty" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select Id IdRaportu, MenuText Nazwa 
from SqlMenu 
where Grupa = @grupa
order by Nazwa
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hidGrupa" Name="grupa" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsInterwaly" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="
select 'Jednorazowo' Text, '1' InterwalTyp
union all select 'Godziny', 'HH'
union all select 'Dni', 'DD'
union all select 'Tygodnie', 'WW'
union all select 'Miesiące', 'MM'
union all select 'Ostatniego dnia miesiąca', 'LM'
    "
    UpdateCommand="
declare @next datetime
declare @DataStartu datetime
declare @InterwalTyp varchar(20)
declare @interwal int
set @DataStartu = '{0}'
set @InterwalTyp = '{1}'
set @Interwal = {2}

set @next = @DataStartu
while @next &lt; GETDATE()
	set @next = case ISNULL(@InterwalTyp,'') 
		when 'HH' then DATEADD(HH, @Interwal, @next)
		when 'DD' then DATEADD(DD, @Interwal, @next)
		when 'WW' then DATEADD(WW, @Interwal, @next)
		when 'MM' then DATEADD(MM, @Interwal, @next)
        when 'LM' then case when @next != dbo.eom(@next) then dbo.eom(@next) else DATEADD(MM, 1, @next) end
        else GETDATE()
		end
select @next NextStart
    ">
</asp:SqlDataSource>
