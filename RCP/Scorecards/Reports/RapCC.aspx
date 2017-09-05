<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Scorecards/Report.Master" AutoEventWireup="true" CodeBehind="RapCC.aspx.cs" Inherits="HRRcp.Scorecards.Reports.RapCC" %>

<%@ Register Src="~/Controls/EliteReports/cntReport.ascx" TagPrefix="leet" TagName="Report" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="uc1" TagName="DateEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headReport" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderReport" runat="server">
        <div class="report_page RepCCUprawnienia">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

     <div class="filters">
            <div class="filter">
                <asp:Label ID="lblDataOd" runat="server" Text="Data od: " />
                <uc1:DateEdit ID="deDateLeft" runat="server" />
            </div>
            <div class="filter">
                <asp:Label ID="Label1" runat="server" Text="Data do: " />
                <uc1:DateEdit ID="deDateRight" runat="server" />
            </div>            
            <div class="filter">
                <asp:Label ID="Label3" runat="server" Text="MPK: " />
                <asp:DropDownList ID="ddlCC" runat="server" DataValueField="Id" DataTextField="Name" DataSourceID="dsCC" />
                <asp:SqlDataSource ID="dsCC" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="select 0 as Id, 'wybierz ...' as Name, 0 as Sort union all select Id, cc as Name, 1 as Sort from CC order by Sort, Name desc" />
            </div>
            <asp:Button ID="btnFilter" runat="server" Text="Filtruj" OnClick="btnFilter_Click" CssClass="button100" />
    </div>


   <leet:Report
                    ID="Rep1"
                    runat="server"
                    CssClass="none"
                    Title="Czynności - MPK"
                    DivClass="none"
                    Pager="false"
                    Charts="All"
                    SQL="
/*select  cc.Id, --accId:-] 
        cc.CC, --aCC:C|charts`names]
        cc.Nazwa, --aNazwa]
		cc.AktywneOd, --aAktywne Od]
		cc.AktywneDo, --aAktywne Do]
		count(w.Id) as Ilosc, --aIlość:S|RapCzynnosci @ccId ] 
        isnull(sum(w.Ilosc), 0) as Czas --aCzas:S|RapCzynnosci @ccId |charts`values]
from scCzynnosci c
left join scWartosci w on w.IdCzynnosci = c.Id 
left join CC cc on cc.Id = c.IdCC
where c.Aktywny = 1
group by cc.Id, cc.Nazwa, cc.CC, cc.AktywneOd, cc.AktywneDo*/

declare @od datetime = '@SQL2'
declare @do datetime = '@SQL3'
declare @ccId int = @SQL1

/*
select
  c.Id --cId:-]
, @od --pod:-]
, @do --pdo:-]
, c.Nazwa --Czynność:C|charts`names]
, CC.cc --MPK]
, cc.Nazwa --MPK]
, /*count(w.Id)*/ISNULL(SUM(w.Ilosc), 0) as Ilosc --Ilość:S|RapPracownicy @cId @pod @pdo] 
, isnull(sum(w.Ilosc * c.Czas), 0) as Czas --Czas:S|RapPracownicy @cId @pod @pdo|charts`values]
from scTypyArkuszyCzynnosci tac
left join scCzynnosci c on tac.IdCzynnosci = c.Id
left join CC on CC.Id = c.IdCC
left join scWartosci w on w.IdCzynnosci = c.Id and w.Data between @od and @do
where tac.Od &lt;= @do and @od &lt;= ISNULL(tac.Do, '20990909') and (c.IdCC = @ccId or @ccId = 0)
group by c.Id, c.Nazwa, CC.cc, CC.Nazwa
*/

select
  c.Id --[cId:-]
, @od --[pod:-]
, @do --[pdo:-]
, c.Nazwa --[Czynność:C|charts`names]
, CC.cc --[MPK]
, cc.Nazwa --[Projekt]
, ISNULL(SUM(w.Ilosc), 0) as Ilosc --[Ilość:S|RapPracownicy @cId @pod @pdo] 
, isnull(sum(w.Ilosc * c.Czas), 0) as Czas --[Czas (min):S|RapPracownicy @cId @pod @pdo|charts`values]
from scWartosci w
left join scCzynnosci c on w.IdCzynnosci = c.Id
left join scTypyArkuszyCzynnosci tac on tac.IdCzynnosci = w.IdCzynnosci and tac.IdTypuArkuszy = w.IdTypuArkuszy and tac.Od &lt;= @do and @od &lt;= ISNULL(tac.Do, '20990909')
left join CC on CC.Id = c.IdCC
where (c.IdCC = @ccId or @ccId = 0) and w.Data between @od and @do
group by c.Id, c.Nazwa, CC.cc, CC.Nazwa

"
 />
</ContentTemplate>
</asp:UpdatePanel>
</div>
    


</asp:Content>
