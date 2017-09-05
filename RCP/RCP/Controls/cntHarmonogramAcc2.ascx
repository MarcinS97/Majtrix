<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntHarmonogramAcc2.ascx.cs" Inherits="HRRcp.RCP.Controls.cntHarmonogramAcc2" %>
<%@ Register Src="~/Controls/Portal/cntSqlTabs.ascx" TagPrefix="cc" TagName="cntSqlTabs" %>

<%@ Register Src="~/RCP/Controls/cntPlanPracy2.ascx" TagPrefix="uc1" TagName="PlanPracy" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="cc" TagName="cntModal" %>



<div id="paHarmonogramAcc" runat="server" class="cntHarmonogramAcc">
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <cc:cntSqlTabs ID="MenuFilter" runat="server" Visible="true"
                AddCssClass=""
                DataTextField="Name"
                DataValueField="Id"
                SQL="
select 1 as Id, 'Do akceptacji' as Name, 0 as Sort
union all
select 2 as Id, 'Zaakceptowany' as Name, 1 as Sort
union all
select -1 as Id, 'Odrzucony' as Name, 2 as Sort
        "
                OnSelectTab="MenuFilter_SelectTab"
                OnDataBound="MenuFilter_DataBound" />

            <div class="list-wrapper">
                <asp:GridView ID="gvList" runat="server" DataSourceID="dsList" CssClass="table">
                </asp:GridView>

                <asp:Button ID="gvListCmd" runat="server" CssClass="button_postback" Text="Button" OnClick="gvListCmd_Click" />
                <asp:HiddenField ID="gvListCmdPar" runat="server" />
            </div>
            <div class="plan-wrapper">

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    
                <cc:cntModal runat="server" ID="cntModal" Width="1500px">
                    <ContentTemplate>

                        <uc1:PlanPracy runat="server" ID="cntPlanPracy" Mode="0" SubMode="1" PracCheckbox="true" />


                    </ContentTemplate>
                    <FooterTemplate>
                        
                        
                <asp:Button ID="btnAcceptConfirm" runat="server" Text="Zaakceptuj" Visible="false" CssClass="btn btn-success" OnClick="btnAcceptConfirm_Click" />
                <asp:Button ID="btnAccept" runat="server" CssClass="button_postback" OnClick="btnAccept_Click" />

                <asp:Button ID="btnRejectConfirm" runat="server" Text="Odrzuć" Visible="false" CssClass="btn btn-danger" OnClick="btnRejectConfirm_Click" />
                <asp:Button ID="btnReject" runat="server" CssClass="button_postback" OnClick="btnReject_Click" />
                    </FooterTemplate>
                </cc:cntModal>
</div>

<asp:SqlDataSource ID="dsList" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
--declare @filter int = 1

select /*p.Nazwisko + ' ' + p.Imie Pracownik, p.KadryId,  */
convert(varchar(10), ha.Data, 20) [data:-]
, p.KierId [kierId:-]
, p.KierownikNI Przelozony, p2.Nazwisko + ' ' + p2.Imie Wnioskujacy, ha.Data [Data:D], count(*) Ilość
, case when @filter = 1 then 'Pokaż' else '' end [:;control|cmd:edit @kierId @data|Pokaż plan]
from rcpHarmonogramAcc ha
left join VPrzypisaniaNaDzis p on p.Id = ha.IdPracownika
left join Pracownicy p2 on p2.Id = ha.IdTworzacego
where (@filter is null or ha.Status = @filter)
group by p.KierId, p.KierownikNI, p2.Nazwisko, p2.Imie, ha.Data
">
    <SelectParameters>
        <asp:ControlParameter Name="filter" Type="Int32" ControlID="MenuFilter" PropertyName="SelectedValue" />
    </SelectParameters>
</asp:SqlDataSource>


<asp:SqlDataSource ID="dsAccept2" runat="server"
    SelectCommand="
declare @nIds nvarchar(MAX) = {0}

select
  h.*
into #aaa
from rcpHarmonogram h
inner join dbo.SplitInt(@nIds, ',') n on n.items = h.IdNaglowka
/*where h.IdNaglowka = @nId*/

update PlanPracy set
/*select pp.IdZmiany, a.IdZmiany*/
  IdZmiany = a.IdZmiany
from PlanPracy pp
inner join #aaa a on a.IdPracownika = pp.IdPracownika and a.Data = pp.Data

insert into PlanPracy (IdPracownika, Data, IdZmiany, DataZm, IdKierownikaZm, Akceptacja, DataAcc, IdKierownikaAcc)
select
  a.IdPracownika
, a.Data
, a.IdZmiany
, a.DataZm
, a.IdKierownikaZm
, a.Akceptacja
, a.DataAcc
, a.IdKierownikaAcc
from #aaa a
left join PlanPracy pp on pp.IdPracownika = a.IdPracownika and pp.Data = a.Data
where pp.Id is null

update rcpHarmonogramAcc set
  Status = 2
from rcpHarmonogramAcc ha
inner join dbo.SplitInt(@nIds, ',') n on n.items = ha.Id

drop table #aaa
" />

<asp:SqlDataSource ID="dsAccept" runat="server"
    SelectCommand="
declare @nIds nvarchar(MAX) = {0}

select pp.* into #aaa
from dbo.SplitInt(@nIds, ',') n
left join rcpHarmonogramAcc ha on ha.Id = n.items
left join PlanPracy pp on pp.IdPracownika = ha.IdPracownika

update PlanPracy set
  IdZmiany = pp.IdZmianyPlan
from PlanPracy pp
inner join #aaa a on a.Id = pp.Id

    update rcpHarmonogramAcc set
  Status = 2
from rcpHarmonogramAcc ha
inner join dbo.SplitInt(@nIds, ',') n on n.items = ha.Id

drop table #aaa
" />



<asp:SqlDataSource ID="dsReject" runat="server"
    SelectCommand="
declare @nIds nvarchar(MAX) = {0}

update rcpHarmonogramAcc set
  Status = -1
from rcpHarmonogramAcc ha
inner join dbo.SplitInt(@nIds, ',') n on n.items = ha.Id

" />




<%--            <asp:Button ID="btnRedirect" runat="server" Text="Pokaż plan pracy" CssClass="btn btn-sm btn-success" OnClick="btnRedirect_Click" />--%>