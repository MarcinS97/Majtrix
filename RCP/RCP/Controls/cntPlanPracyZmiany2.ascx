<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntPlanPracyZmiany2.ascx.cs" Inherits="HRRcp.RCP.Controls.cntPlanPracyZmiany2" %>
<%@ Register Src="~/Controls/RepHeader.ascx" TagName="RepHeader" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="cc" TagName="DateEdit" %>
<%@ Register Src="~/Controls/SelectOkres.ascx" TagName="SelectOkres" TagPrefix="uc1" %>
<%@ Register Src="~/RCP/Controls/cntZmianySelect.ascx" TagName="SelectZmiana" TagPrefix="uc1" %>
<%@ Register Src="~/RCP/Controls/cntPlanPracy2.ascx" TagName="PlanPracy" TagPrefix="uc1" %>
<%@ Register Src="~/RCP/Controls/cntSchematy.ascx" TagPrefix="cc" TagName="cntSchematy" %>
<%@ Register Src="~/RCP/Controls/Adm/cntSchematy.ascx" TagPrefix="uc1" TagName="cntSchematy" %>
<%@ Register Src="~/RCP/Controls/cntModal.ascx" TagPrefix="cc" TagName="cntModal" %>
<%@ Register Src="~/Controls/ZmianyControl3.ascx" TagPrefix="cc" TagName="ZmianyControl3" %>





<div id="paPlanPracyZmiany" runat="server" class="cntPlanPracyZmiany">
    <asp:HiddenField ID="hidKierId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidUserId" runat="server" Visible="false" />
    
    <div class="paZmiany">
        <asp:UpdatePanel ID="upBtn" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btnAddOkresModal" runat="server" OnClick="btnAddOkresModal_Click" CssClass="btn btn-success" Text="Otwórz okres rozliczeniowy" style="margin-bottom: 10px; width: 100%;" />
            </ContentTemplate>
        </asp:UpdatePanel>
                        
        <div class="panel-group" id="xaccordion">
            <div id="_paZmianyTools" runat="server" visible="true" class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse" data-parent="#accordion" href="#collapse3">Narzędzia</a>
                    </h4>
                  </div>
                <div id="collapse3" class="panel-collapse collapse in">
                    <div class="panel-body paZmianyTools">
                        <asp:UpdatePanel ID="upTools" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:Button ID="btnCopyFromModal"    runat="server" CssClass="btn btn-default first"    Text="Kopiuj z..."              OnClick="btnCopyFromModal_Click" />    
                                <asp:Button ID="btSetScheme"         runat="server" CssClass="btn btn-default last"     Text="Ustaw schemat"            OnClick="btScheme_Click" />

                               
                                <asp:Button ID="btCheckPP"           runat="server" CssClass="btn btn-default"          Text="Sprawdź"                  OnClick="btCheckPP_Click" />

                            
                                <asp:Button ID="btnPrintHar"         runat="server" CssClass="btn btn-default"          Text="Wydrukuj Harmonogram"     OnClick="btnPrintHar_Click" />
                                <asp:Button ID="btnPrintList"        runat="server" CssClass="btn btn-default last"     Text="Wydrukuj Listę obecności" OnClick="btnPrintList_Click" />
                                <asp:Button ID="btnSendToAcc"        runat="server" CssClass="button_postback"                                          OnClick="btnSendToAcc_Click" />
                                <asp:Button ID="btnEditZmiany"       runat="server" CssClass="btn btn-default"          Text="Edytuj zmiany"            OnClick="btnEditZmiany_Click" />
                                <asp:Button ID="lnkEditSchemesModal" runat="server" CssClass="btn btn-default"          Text="Edytuj schematy"          OnClick="lnkEditSchemesModal_Click" />
                                
                                <%--
                                <asp:Button ID="btScheme" runat="server" Text="Schemat" Visible="false" CssClass="button75" UseSubmitBehavior="false" data-toggle="modal" data-target="#myModal" OnClientClick="return false;" OnClick="btScheme_Click" />
                                <asp:Button ID="btnAccept" runat="server" Text="Zaakceptuj" Visible="false" CssClass="button75 btn-success" OnClick="btnAccept_Click" /><br />
                                <asp:Button ID="btnReject" runat="server" Text="Odrzuć" Visible="false" CssClass="button75 btn-danger" OnClick="btnReject_Click" /><br />
                                --%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
<%--            <div id="_paZmianySchematy" runat="server" visible="true" class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse" data-parent="#accordion" href="#collapse2">Schematy</a>
                    </h4>
                </div>
                <div id="collapse2" class="panel-collapse collapse">
                    <div class="panel-body">
                        <asp:UpdatePanel ID="upSchematy" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <cc:cntSchematy ID="Schematy" runat="server" EditMode="true" />                        
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>--%>
            <div class="panel panel-default">
                <div class="panel-heading" >
                    <h4 class="panel-title">
                        <a data-toggle="collapse" data-parent="#accordion" href="#collapse1">Zmiany</a>
                    </h4>
                </div>
                <div id="collapse1" class="panel-collapse collapse in">
                    <div class="panel-body">
                        <asp:UpdatePanel ID="upZmiany" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <uc1:SelectZmiana ID="cntSelectZmiana" OnSelectedChanged="OnSelectZmiana" Mode="1" runat="server" />
                             </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div><div class="paPlan"><%-- nie rozdzielać!!! to są inline-block obok siebie --%>
        <div id="paSearch" runat="server" class="search">
            <div>
                <asp:Label ID="Label1" runat="server" Text="Wyszukaj pracownika:" Visible="false"></asp:Label>
                <asp:TextBox ID="tbSearch" runat="server" CssClass="form-control" MaxLength="250" Placeholder="Wyszukaj pracownika ..." style="display: inline-block;" ></asp:TextBox>
                <asp:Button ID="btClear" runat="server" CssClass="btn-default" Text="Czyść" />
            
            
           
            </div>

       

        </div>
        <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="filter"> 
                    <asp:DropDownList ID="ddlCom" runat="server" CssClass="form-control ddlKier" DataSourceID="dsCom" DataValueField="Value" DataTextField="Text" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlCom_SelectedIndexChanged" Visible="true" />

                    <asp:DropDownList ID="ddlKier" runat="server" CssClass="form-control ddlKier" DataSourceID="dsKier" DataValueField="Value" 
                        DataTextField="Text" AutoPostBack="true" OnSelectedIndexChanged="ddlKier_SelectedIndexChanged" Visible="true" />
                
                   

                      <asp:DropDownList ID="ddlVer" runat="server" DataSourceID="dsVer" DataValueField="Id" DataTextField="Name" CssClass="form-control ddlVer" Visible="false" OnDataBound="ddlVer_DataBound"
                     style="width: 200px;" AutoPostBack="true" OnSelectedIndexChanged="ddlVer_SelectedIndexChanged" />
            <asp:SqlDataSource ID="dsVer" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false" 
                SelectCommand="
select
  distinct h._od Id
, CONVERT(varchar(19), h._od, 20) + ISNULL(' - ' + hac.Nazwa, '') Name
, ha.Status
from
rcpHarmonogram h
left join rcpHarmonogramAcc ha on ha.Id = h.IdNaglowka
left join rcpHarmonogramAccStatus hac on hac.Id = ha.Status
--where h.IdPracownika in (select * from dbo.SplitInt('251', ',')) 
where h.IdPracownika in (select IdPracownika from dbo.fn_GetTree2(@kierId, 0, GETDATE()))
order by h._od desc, ha.Status desc

                              " >
                <SelectParameters>
                    <asp:ControlParameter Name="kierId" Type="Int32" ControlID="ddlKier" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>
            
                      <asp:SqlDataSource ID="dsCom" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false" 
                          SelectCommand="select null Value, 'brygada ...' Text, 0 Sort union all select Id Value, Commodity Text, 1 Sort from Commodity where Aktywne = 1 order by Sort, Text"
                          />


                
                </div>
                <asp:Button ID="btSearch" runat="server" Text="Wyszukaj" CssClass="button_postback" onclick="btSearch_Click" />
                
                <table class="okres_navigator printoff">
                    <tr>
                        <td class="colleft">
                            <div runat="server" visible="false">
                                <asp:Label ID="lbPlanQ" runat="server" CssClass="t5" Text="Plan pracy"></asp:Label>
                                <asp:Label ID="lbPlanE" runat="server" CssClass="t5" Visible="false" Text="2) Kliknij w dzień i ustaw zmianę:"></asp:Label>
                            </div>
                        </td>
                        <td class="colmiddle">
                            <uc1:SelectOkres ID="cntSelectOkres" StoreInSession="true" ControlID="cntPlanPracy" OnOkresChanged="cntSelectOkres_Changed" runat="server" />
                        </td>
                        <td class="colright">
                            <asp:Label ID="lbOkresStatus" CssClass="t1" Visible="false" runat="server"></asp:Label>
                             <asp:Button ID="btnSendToAccConfirm" runat="server" CssClass="button150" style="height: 28px; padding: 0px;"          Text="Wyślij do akceptacji"     OnClick ="btnSendToAccConfirm_Click" />
                            <asp:Button ID="btEditPP" runat="server" Text="Edycja" CssClass="button75" OnClick="btEditPP_Click" />
                            <asp:Button ID="btSavePP" runat="server" Text="Zapisz" Visible="false" CssClass="button75 btn-success" OnClick="btSavePP_Click" />
                            <asp:Button ID="btCancelPP" runat="server" Text="Anuluj" Visible="false" CssClass="button75" OnClick="btCancelPP_Click" />
                            <asp:Button ID="btPrint" class="button75" runat="server" Text="Drukuj" OnClientClick="printPreview();return false;" Visible="false" />
                        </td>
                    </tr>
                </table>
                
                <div id="paTitle" runat="server" visible="false" class="printon">
                    <uc1:RepHeader ID="repHeader" Icon="2" Caption="Plan pracy" runat="server" />
                </div>
                <uc1:PlanPracy ID="cntPlanPracy" Mode="0" runat="server" PracCheckbox="true" StatusVisible="true"  />
                <div id="paLegenda" runat="server" visible="false" class="printon legendaZmiany">
                    <div class="spacer16"></div>
                    Zmiany:
                    <uc1:SelectZmiana ID="cntZmianyLegenda" Mode="1" runat="server" />
                </div>
        
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnPrintHar" />
                <asp:PostBackTrigger ControlID="btnPrintList" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    
    
    <%----- schematy -----%>

    <cc:cntModal runat="server" ID="cntSetSchemesModal" Title="Ustaw schemat">
        <ContentTemplate>
            <asp:HiddenField ID="hidSchemeEmp" runat="server" Visible="false" />
            <asp:HiddenField ID="hidSchemeDays" runat="server" Visible="false" />
            <div class="form-group" runat="server" visible="true">
                <label>Schematy:</label>
                <asp:DropDownList ID="ddlScheme" runat="server" DataValueField="Id" DataTextField="Name" CssClass="form-control" DataSourceID="dsSchemes" />
            </div>
            <div class="form-group">
                <label>Data od:</label>
                <cc:DateEdit ID="deLeft" runat="server" />

                <label>Data do:</label>
                <cc:DateEdit ID="deRight" runat="server" />
            </div>

            <%--<asp:Button ID="btnShowPlan" runat="server" OnClick="btnShowPlan_Click" Text="Pokaż plan" CssClass="btn btn-default" />--%>

        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btnSaveScheme" runat="server" CssClass="btn btn-success" OnClick="btnSaveScheme_Click" Text="Zapisz" />
        </FooterTemplate>
    </cc:cntModal>

<%--    <cc:cntModal runat="server" ID="cntModalPlan">
        <ContentTemplate>                
            <uc1:PlanPracy runat="server" ID="PlanPracy1" Mode="0" SubMode="1" PracCheckbox="true" />

        </ContentTemplate>
    </cc:cntModal>--%>



    <cc:cntModal ID="cntCopyModal" runat="server" Title="Kopiuj z">
        <ContentTemplate>
            <asp:HiddenField ID="hidCopyEmp" runat="server" Visible="false" />
            <asp:HiddenField ID="hidCopyDays" runat="server" Visible="false" />

            <div class="form-group">
                <label>Pracownik:</label>
                <asp:DropDownList ID="ddlPrac2" runat="server" DataSourceID="dsPrac2" DataValueField="Id" DataTextField="Name" CssClass="form-control" />
                <asp:SqlDataSource ID="dsPrac2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                    SelectCommand="
                    select null Id, 'wszyscy pracownicy ...' Name, 0 Sort 
                    union all 
                    select IdPracownika Id, Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')', '') Name, 1 Sort 
                    from dbo.fn_GetTree2(@kierId, 0, GETDATE())
                    where IdPracownika not in (select items from dbo.SplitInt(@emp, ',')) 
                    order by Sort, Name
                    ">
                    <SelectParameters>
                        <asp:ControlParameter Name="emp" Type="String" ControlID="hidCopyEmp" PropertyName="Value" />
                        <asp:ControlParameter Name="kierId" Type="Int32" ControlID="ddlKier" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <div class="form-group">
                <label>Data od:</label>
                <cc:DateEdit ID="deCopyLeft" runat="server" />

                <label>Data do:</label>
                <cc:DateEdit ID="deCopyRight" runat="server" />
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btnCopyFrom" runat="server" CssClass="btn btn-success" OnClick="btnCopyFrom_Click" Text="Zapisz" />
        </FooterTemplate>
    </cc:cntModal>

    <%-- edycja schematów --%>

    <cc:cntModal runat="server" ID="cntModal" Title="Schematy">
        <ContentTemplate>
            <uc1:cntSchematy runat="server" ID="cntSchematy" />
        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btnShowSchemeInsert" runat="server" CssClass="btn-success pull-left" OnClick="btnShowSchemeInsert_Click" Text="Dodaj nowy schemat" />
        </FooterTemplate>
    </cc:cntModal>

    <%-- edycja zmian --%>


    <cc:cntModal runat="server" ID="cntModalZmiany" Width="1200px" Title="Edycja zmian">
        <ContentTemplate>
            <cc:ZmianyControl3 runat="server" ID="ZmianyControl3" />
        </ContentTemplate>
    </cc:cntModal>



</div>

<asp:SqlDataSource ID="dsSchemes" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
    SelectCommand="
select null Id, 'wybierz schemat ...' Name, 0 Sort
union all
select Id, Nazwa Name, Kolejnosc Sort
from rcpSchematy 
where Aktywny = 1 
order by Sort, Name" />

<asp:SqlDataSource ID="dsKier" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
declare @od datetime
set @od = dbo.getdate(GETDATE())
select P.IdPracownika as Value, 
REPLICATE('&nbsp;', Hlevel * 4) +
P.Nazwisko + ' ' + P.Imie + ISNULL(' (' + P.KadryId + ')', '') as Text, P.Nazwisko, P.Imie, P1.Sort 
from dbo.fn_GetTree2(@userId, 1, @od) P
outer apply (select case when P.IdPracownika = @userId then 1 else 2 end Sort) P1
where Kierownik = 1
--order by Sort, Nazwisko, Imie
order by P.SortPath
">
    <SelectParameters>
        <asp:ControlParameter Name="userId" Type="Int32" ControlID="hidUserId" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>

<%--
UWAGA: póki co "na dziś" kierownicy powinni się pokazać na dzień planowania - może lepsza byłaby fn_GetTreeOkres
        <asp:ControlParameter Name="od" ControlID="hidFrom" PropertyName="Value" Type="DateTime" />

declare @data datetime
set @data = dbo.getdate(GETDATE())
select null as Value, 'wybierz przełożonego ...' as Text, null as Nazwisko, null as Imie, -1 as Sort
union all
select P.Id as Value, case when K1.Aktywny = 2 then '*' else '' end + P.Nazwisko + ' ' + P.Imie as Text, P.Nazwisko, P.Imie, K1.Aktywny as Sort
from Pracownicy P 
outer apply (select top 1 * from Przypisania where IdKierownika = P.Id and Status = 1 and Od &lt;= @data order by Od desc) K
outer apply (select case when @data between ISNULL(K.Od, '19000101') and ISNULL(K.Do,'20990909') then 1 else 2 end Aktywny) K1
where K.Id is not null
union all
select 0 as Value, 'Poziom główny struktury' as Text, null as Nazwisko, null as Imie, 3 as Sort 
order by Sort, Nazwisko, Imie
--%>

<asp:SqlDataSource ID="dsKierAll" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
declare @data datetime
set @data = dbo.getdate(GETDATE())
select null as Value, 'wybierz przełożonego ...' as Text, null as Nazwisko, null as Imie, -1 as Sort
union all
select P.Id as Value, case when K1.Aktywny = 2 then '*' else '' end + P.Nazwisko + ' ' + P.Imie as Text, P.Nazwisko, P.Imie, K1.Aktywny as Sort
from Pracownicy P 
outer apply (select top 1 * from Przypisania where IdKierownika = P.Id and Status = 1 and Od &lt;= @data order by Od desc) K
outer apply (select case when @data between ISNULL(K.Od, '19000101') and ISNULL(K.Do,'20990909') then 1 else 2 end Aktywny) K1
where K.Id is not null
union all
select 0 as Value, 'Poziom główny struktury' as Text, null as Nazwisko, null as Imie, 3 as Sort 
order by Sort, Nazwisko, Imie
">
    <SelectParameters>
        <asp:ControlParameter Name="userId" Type="Int32" ControlID="hidUserId" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>


<%-- lista pracowników
<asp:DropDownList ID="ddlPrac" runat="server" DataSourceID="dsPrac" DataValueField="Id" DataTextField="Name" AutoPostBack="true" OnSelectedIndexChanged="ddlPrac_SelectedIndexChanged" />
<asp:SqlDataSource ID="dsPrac" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
    SelectCommand="
select null Id, 'wszyscy pracownicy ...' Name, 0 Sort 
union all 
select IdPracownika Id, Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')', '') Name, 1 Sort 
from dbo.fn_GetTree2(@kierId, 0, GETDATE())
order by Sort, Name
">
    <SelectParameters>
        <asp:ControlParameter Name="kierId" Type="Int32" ControlID="hidKierId" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>
--%>







<asp:SqlDataSource ID="dsApplyScheme" runat="server" SelectCommand="
declare @prac_list nvarchar(MAX) = {2} 
--declare @date_list nvarchar(341) = {3}

--select items into #ddd from dbo.SplitStr(@date_list, ',')

declare @schId int = {0}
declare @od datetime = {3}
declare @do datetime = {4}
declare @accId int = {1}
declare @nadzien datetime = GETDATE()

declare @sch nvarchar(512)
declare @dow int

select @sch = Schemat from rcpSchematy where Id = @schId

--select @od = MIN(items) from #ddd
--select @do = MAX(items) from #ddd

select @dow = dbo.dow(@od)

select
  /*@pracId*/ poa.items IdPracownika
, d.Data Data
, s.items IdZmiany
, @nadzien DataZm
into #aaa
from dbo.GetDates2(@od, @do) d
outer apply (select (d.Lp + @dow) / 7 % (select COUNT(items) from dbo.SplitIntSort(@sch, ',')) c) oa
left join dbo.SplitIntSort(@sch, ',') s on s.idx = oa.c
left join Kalendarz k on k.Data = d.Data
outer apply (select items from dbo.SplitInt(@prac_list, ',')) poa
--where d.Data in (select items from #ddd)
where ISNULL(k.Rodzaj, -1) not in (0, 1, 2)

/*
select a.*, h.Id OriginalId into #bbb
from #aaa a
left join rcpHarmonogram h on h.IdPracownika = a.IdPracownika and h.Data = a.Data
where h._do is null and a.IdZmiany != ISNULL(h.IdZmiany, -1)
*/

select a.*, /*h*/pp.Id OriginalId into #bbb
from #aaa a
/*left join rcpHarmonogram h on h.IdPracownika = a.IdPracownika and h.Data = a.Data*/
left join PlanPracy pp on pp.IdPracownika = a.IdPracownika and pp.Data = a.Data
/*where h._do is null and a.IdZmiany != ISNULL(h.IdZmiany, -1)*/
where ISNULL(a.IdZmiany, -1) != ISNULL(pp.IdZmianyPlan, -1)

/*
update rcpHarmonogram set
  _do = @nadzien
from rcpHarmonogram h
inner join #bbb a on /*a.IdPracownika = h.IdPracownika and a.Data = h.Data*/ a.OriginalId = h.Id and h._do is null
*/

update PlanPracy set
  IdZmianyPlan = b.IdZmiany
from PlanPracy pp
inner join #bbb b on b.OriginalId = pp.Id

/*
insert into rcpHarmonogram (Id, _od, IdPracownika, Data, IdZmiany, DataZm, IdKierownikaZm, Akceptacja, DataAcc, IdKierownikaAcc)
select OriginalId, @nadzien, a.IdPracownika, a.Data, a.IdZmiany, a.DataZm, @accId, 0, a.DataZm, @accId
from #bbb a
*/

insert into PlanPracy (IdPracownika, Data, IdZmianyPlan, DataZm, IdKierownikaZm, Akceptacja, DataAcc, IdKierownikaAcc)
select a.IdPracownika, a.Data, a.IdZmiany, a.DataZm, @accId, 0, a.DataZm, @accId
from #bbb a
where a.OriginalId is null


drop table #aaa
drop table #bbb
--drop table #ddd

" />


<asp:SqlDataSource ID="dsCopyFrom" runat="server" SelectCommand="
declare @prac_list nvarchar(MAX) = {2}
--declare @date_list nvarchar(341) = {3}

declare @od datetime = {3}
declare @do datetime = {4}

--select items into #ddd from dbo.SplitStr(@date_list, ',')

declare @accId int = {1}
declare @nadzien datetime = GETDATE()

declare @pracId int = {0}
    /*
select
  /*@pracId*/ poa.items IdPracownika
, h.Data
, h.IdZmiany
, @nadzien DataZm
into #aaa
from rcpHarmonogram h
outer apply (select items from dbo.SplitInt(@prac_list, ',')) poa
where h.Data /*in (select items from #ddd) */ between @od and @do and h.IdPracownika = @pracId and h._do is null
    */

select
  /*@pracId*/ poa.items IdPracownika
, /*h*/pp.Data
, /*h*/pp.IdZmianyPlan IdZmiany
, @nadzien DataZm
into #aaa
/*from rcpHarmonogram h*/
from PlanPracy pp
outer apply (select items from dbo.SplitInt(@prac_list, ',')) poa
/*where h.Data /*in (select items from #ddd) */ between @od and @do and h.IdPracownika = @pracId and h._do is null*/
where pp.Data between @od and @do and pp.IdPracownika = @pracId

/*
select
  /*@pracId*/ poa.items IdPracownika
, d.Data Data
, s.items IdZmiany
, @nadzien DataZm
into #aaa
from dbo.GetDates2(@od, @do) d
outer apply (select (d.Lp + @dow) / 7 % (select COUNT(items) from dbo.SplitIntSort(@sch, ',')) c) oa
left join dbo.SplitIntSort(@sch, ',') s on s.idx = oa.c
/*left join Kalendarz k on k.Data = d.Data*/
outer apply (select items from dbo.SplitInt(@prac_list, ',')) poa
/*where ISNULL(k.Rodzaj, -1) not in (0, 1, 2)*/
*/
 
select a.*, /*h*/pp.Id OriginalId into #bbb
from #aaa a
/*left join rcpHarmonogram h on h.IdPracownika = a.IdPracownika and h.Data = a.Data*/
left join PlanPracy pp on pp.IdPracownika = a.IdPracownika and pp.Data = a.Data
/*where h._do is null and a.IdZmiany != ISNULL(h.IdZmiany, -1)*/
where ISNULL(a.IdZmiany, -1) != ISNULL(pp.IdZmianyPlan, -1)

/*
update rcpHarmonogram set
  _do = @nadzien
from rcpHarmonogram h
inner join #bbb a on /*a.IdPracownika = h.IdPracownika and a.Data = h.Data*/ a.OriginalId = h.Id and h._do is null
*/

update PlanPracy set
  IdZmianyPlan = b.IdZmiany
from PlanPracy pp
inner join #bbb b on b.OriginalId = pp.Id

/*
insert into rcpHarmonogram (Id, _od, IdPracownika, Data, IdZmiany, DataZm, IdKierownikaZm, Akceptacja, DataAcc, IdKierownikaAcc)
select OriginalId, @nadzien, a.IdPracownika, a.Data, a.IdZmiany, a.DataZm, @accId, 0, a.DataZm, @accId
from #bbb a
*/

insert into PlanPracy (IdPracownika, Data, IdZmianyPlan, DataZm, IdKierownikaZm, Akceptacja, DataAcc, IdKierownikaAcc)
select a.IdPracownika, a.Data, a.IdZmiany, a.DataZm, @accId, 0, a.DataZm, @accId
from #bbb a
where a.OriginalId is null

drop table #aaa
drop table #bbb
--drop table #ddd
" />



<asp:SqlDataSource ID="dsPrint" runat="server" 
    SelectCommand="
/*
 * DBW - PLAN PRACY
 *
 */

declare @kierId varchar(12) = {0}

declare @colsH nvarchar(MAX)
declare @colsD nvarchar(MAX)
declare @colsA nvarchar(MAX)
declare @colsA2 nvarchar(MAX)

declare @stmt nvarchar(MAX)

declare @od datetime = {1}
declare @do datetime = {2}

declare @pracIds nvarchar(MAX) = {3}

select
  @colsH = isnull(@colsH + ',', '') + 'ISNULL([' + convert(varchar(10), d.Data, 20) + '], ' + case when k.Rodzaj in (0, 1, 2) then '''|holiday''' else '''''' end + ') [' + convert(varchar, DAY(d.Data)) + ']'
, @colsD = isnull(@colsD + ',', '') + '[' + convert(varchar(10), d.Data, 20) + ']'
, @colsA = isnull(@colsA + ',', '') + '''' + case dbo.dow(d.Data)
    when 0 then 'pn'
    when 1 then 'wt'
    when 2 then 'śr'
    when 3 then 'czw'
    when 4 then 'pt'
    when 5 then 'sb'
    when 6 then 'nd'
    else 'aoe'
end + '|holiday'''
, @colsA2 = isnull(@colsA2 + ',', '') + '''' + convert(varchar, DAY(d.Data)) + '|holiday'' [' + convert(varchar, DAY(d.Data)) + ']'
from dbo.GetDates2(@od, @do) d
left join Kalendarz k on k.Data = d.Data

select @stmt = '
declare @kierId int = ' + @kierId +  '
declare @pracIds nvarchar(MAX) = ''' + @pracIds + '''

select
'''' RowClass
, ''|col1'' [0]
, ' + @colsA2 + '
, ''|last-col'' [-1]
union all
select
'''' RowClass
, ''PRZEŁOŻONY|col1''
, ' + @colsA + '
, ''Data i podpis pracownika|last-col''
union all
select
'''' RowClass
,  p.Nazwisko + '' '' + p.Imie + ''|col1'' [MISTRZ]
, ' + @colsH + '
, ''|last-col'' [Data i podpis pracownika]
from Pracownicy p
left join
(
    select * from
    (
        select pp.IdPracownika, z.Symbol + case when k.Rodzaj in (0, 1, 2) then ''|holiday'' else '''' end Symbol, pp.Data
        from PlanPracy pp
        left join Zmiany z on z.Id = pp.IdZmiany
        left join Kalendarz k on k.Data = pp.Data
    ) pp
    PIVOT
    (
        MAX(pp.Symbol) for pp.Data in (' + @colsD + ')
    ) PV
) t on t.IdPracownika = p.Id
where p.Id = @kierId


select
  '''' RowClass
,  p.Nazwisko + '' '' + p.Imie + ''|col1'' [Pracownik]
, ' + @colsH + '
, ''|last-col'' [Data i podpis pracownika]
/*from dbo.fn_GetTree2(@kierId, 0, GETDATE())*/ from dbo.SplitInt(@pracIds, '','') tr
left join Pracownicy p on p.Id = tr./*IdPracownika*/ items
left join
(
    select * from
    (
        select pp.IdPracownika, z.Symbol + case when k.Rodzaj in (0, 1, 2) then ''|holiday'' else '''' end Symbol, pp.Data
        from PlanPracy pp
        left join Zmiany z on z.Id = pp.IdZmiany
        left join Kalendarz k on k.Data = pp.Data
    ) pp
    PIVOT
    (
        MAX(pp.Symbol) for pp.Data in (' + @colsD + ')
    ) PV
) t on t.IdPracownika = tr./*IdPracownika*/items
order by [Pracownik]
'

exec sp_executesql @stmt

" />


<asp:SqlDataSource ID="dsPrint2" runat="server"
SelectCommand=
"
declare @od datetime = {0}
declare @do datetime = {1}
declare @pracId int = {2}


select 
  'header' RowClass
, 'lp.|lp' Lp
, 'data|data' Data
, 'ZMIANA|zmiana' Zmiana
, 'godzina rozpoczęcia pracy|godzrozp' GodzRozp
, 'godzina zakończenia pracy|godzzak' GodzZak
, 'suma godzin|sumagodz' SumaGodz
, 'podpis pracownika  lub rodzaj nieobecności|podpis' Podpis
, 'ilość godzin normalnych|godznorm' GodzNorm
, 'ilość godzin nocnych|godznoc' GodzNoc
, 'ilość godzin nadliczbowych 50%|nad50' GodzNadliczb50
, 'ilość godzin nadliczbowych 100%|nad100' GodzNadliczb100
union all
select 
  case when k.Data is null then '' else 'holiday' end RowClass
, convert(varchar, Lp + 1) + '|lp' Lp
, case dbo.dow(d.Data)
    when 0 then 'pn'
    when 1 then 'wt'
    when 2 then 'śr'
    when 3 then 'czw'
    when 4 then 'pt'
    when 5 then 'sb'
    when 6 then 'nd'
    else 'aoe' end + '|data' Data
, isnull(z.Symbol,'') + '|zmiana' + '^background-color: ' + isnull(z.Kolor, '#fff') Zmiana
, '|godzrozp' GodzRozp
, '|godzzak' GodzZak
, '|sumagodz' SumaGodz
, '|podpis' Podpis
, '|godznorm' GodzNorm
, '|godznoc' GodzNoc
, '|nad50' GodzNadliczb50
, '|nad100' GodzNadliczb100
from dbo.GetDates2(@od, @do) d
left join PlanPracy pp on pp.Data = d.Data and pp.IdPracownika = @pracId
left join Zmiany z on z.Id = pp.IdZmianyPlan
left join Kalendarz k on k.Data = d.Data
"/>

<asp:SqlDataSource ID="dsPrint2Prac" runat="server" SelectCommand="select Imie + ' ' + Nazwisko + ISNULL(' (' + KadryId + ')','') Pracownik from Pracownicy where Id = {0}" />
<asp:SqlDataSource ID="dsPrint2Comp" runat="server" SelectCommand="select isnull(Klasyfikacja, '') Klasyfikacja from PracownicyStanowiska where IdPracownika = {0} and {1} between Od and ISNULL(Do, '20990909')" />

<asp:SqlDataSource ID="dsSendToAcc2" runat="server" 
    SelectCommand=
"
declare @pracIds nvarchar(MAX) = {0}

declare @date datetime = {1}
declare @kierId int = {2}

declare @nadzien datetime = GETDATE()

/*declare @nId int*/

insert into rcpHarmonogramAcc (IdPracownika, Data, Status, DataUtworzenia, IdTworzacego) select items, @date, 1, @nadzien, @kierId from dbo.SplitInt(@pracIds, ',')

/*set @nId = SCOPE_IDENTITY()*/

update rcpHarmonogram set
  IdNaglowka = ha.Id
/*select **/
from rcpHarmonogram h
inner join dbo.SplitInt(@pracIds, ',') p on p.items = h.IdPracownika
left join rcpHarmonogramAcc ha on ha.Data = dbo.bom(h.Data) and ha.IdPracownika = h.IdPracownika and ha.Status = 1
/*inner join rcpHarmonogramAcc ha on ha.IdPracownika = h.IdPracownika and ha.Data = dbo.bom(h.Data) and ha.Status = 1*/
where h._do is null and h.IdNaglowka is null and dbo.bom(h.Data) = @date 
"
    />


<asp:SqlDataSource ID="dsSendToAcc" runat="server" 
    SelectCommand=
"
declare @pracIds nvarchar(MAX) = {0}

declare @date datetime = {1}
declare @kierId int = {2}

declare @nadzien datetime = GETDATE()
insert into rcpHarmonogramAcc (IdPracownika, Data, Status, DataUtworzenia, IdTworzacego) select items, @date, 1, @nadzien, @kierId from dbo.SplitInt(@pracIds, ',')
"
    />

<asp:SqlDataSource ID="dsToAccList" runat="server"
    SelectCommand="
select Id from rcpHarmonogramAcc where Status = 1 and Data = {0}
" />


<asp:SqlDataSource ID="dsAccept" runat="server"
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



  <cc:cntModal runat="server" ID="cntModalOkres" Title="Dodaj okres">
        <ContentTemplate>
            <div class="form-group">
                <label>Typ okresu:</label>
                <asp:DropDownList ID="ddlTypOkresu" runat="server" DataValueField="Value" DataTextField="Text" DataSourceID="dsTypOkresu" OnSelectedIndexChanged="ddlTypOkresu_SelectedIndexChanged"
                    CssClass="form-control" AutoPostBack="true" />
                <asp:SqlDataSource ID="dsTypOkresu" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="
select null Value, 'wybierz ...' Text, 0 Sort
union all
select Id Value, Nazwa Text, 1 Sort
from rcpOkresyRozliczenioweTypy
order by Sort, Text                    
" />
            </div>
            <div class="form-group">
                <label>Okres:</label>
                <asp:DropDownList ID="ddlOkresy" runat="server" DataValueField="Value" DataTextField="Text" DataSourceID="dsOkresy" CssClass="form-control"
                    OnSelectedIndexChanged="ddlOkresy_SelectedIndexChanged" AutoPostBack="true" />
                <asp:SqlDataSource ID="dsOkresy" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="
--declare @typ int = 1;
with o as
(
    select DATEADD(MONTH, 1, ISNULL(MAX(DataOd), dbo.bom(GETDATE()))) Data from OkresyRozliczeniowe where Typ = @typ
    union all
    select DATEADD(MONTH, ort.IloscMiesiecy, Data ) from o inner join rcpOkresyRozliczenioweTypy ort on ort.Id = @typ
)
select top 10
  CONVERT(varchar(10), dbo.bom(Data), 20) + ';' + CONVERT(varchar(10), DATEADD(MONTH, ort.IloscMiesiecy - 1, dbo.eom(Data)), 20) Value
, CONVERT(varchar(10), dbo.bom(Data), 20) + ' - ' + CONVERT(varchar(10), DATEADD(MONTH, ort.IloscMiesiecy - 1, dbo.eom(Data)), 20) Text
from o inner join rcpOkresyRozliczenioweTypy ort on ort.Id = @typ

">
                    <SelectParameters>
                        <asp:ControlParameter Name="typ" Type="Int32" ControlID="ddlTypOkresu" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <div>
                <asp:Repeater ID="rpNom" runat="server">
                    <ItemTemplate>
                        <asp:HiddenField ID="hidDate" runat="server" Visible="false" Value='<%# Eval("Date") %>' />
                        <div class="row form-group">
                            <div class="col-sm-6">
                                <label>Czas nominalny - <%# Eval("Friendly") %></label>
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox ID="tbNom" runat="server" CssClass="form-control" Width="90px" MaxLength="5" />

                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:SqlDataSource ID="dsNom" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                    SelectCommand="
declare @mies int = (select IloscMiesiecy from rcpOkresyRozliczenioweTypy where Id = @typ)
;with a as
(
    select 1 b
    union all
    select b + 1 from a where b &lt; @mies
)
select b from a
">
                    <SelectParameters>
                        <asp:ControlParameter Name="typ" Type="Int32" ControlID="ddlTypOkresu" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

        </ContentTemplate>
        <FooterTemplate>
            <asp:Button ID="btnAddOkres" runat="server" CssClass="btn btn-success" OnClick="btnAddOkres_Click" Text="Zapisz" />
        </FooterTemplate>
    </cc:cntModal>

<asp:SqlDataSource ID="dsInsert" runat="server" SelectCommand="insert into OkresyRozliczeniowe (DataOd, DataDo, Status, Typ) values ({0}, {1}, 0, {2}); select SCOPE_IDENTITY();"></asp:SqlDataSource>

<asp:SqlDataSource ID="dsAddCzas" runat="server" SelectCommand="insert into CzasNom (Data, DniPrac, IdOkresu) values ({0}, {1}, {2})" />


<asp:SqlDataSource ID="dsNewOkres" runat="server" SelectCommand="select top 1 dbo.bom(dateadd(month, 1, DataOd)) DateFrom, dbo.eom(dateadd(month, 1, DataOd)) DateTo from OkresyRozl order by DataOd desc" />
