<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanPracyZmiany.ascx.cs" Inherits="HRRcp.Controls.PlanPracyZmiany" %>
<%@ Register src="SelectZmiana2.ascx" tagname="SelectZmiana" tagprefix="uc1" %>
<%@ Register src="SelectOkres.ascx" tagname="SelectOkres" tagprefix="uc1" %>
<%@ Register src="PlanPracy.ascx" tagname="PlanPracy" tagprefix="uc1" %>
<%@ Register src="~/Controls/RepHeader.ascx" tagname="RepHeader" tagprefix="uc1" %>

<%@ Register Src="~/Controls/DateEdit.ascx" TagPrefix="cc" TagName="DateEdit" %>

<asp:HiddenField ID="hidKierId" runat="server" Visible="false" />

<div class="PlanPracyZmiany">

            <asp:DropDownList ID="ddlKier" runat="server" DataSourceID="dsKier" DataValueField="Value" DataTextField="Text" AutoPostBack="true" OnSelectedIndexChanged="ddlKier_SelectedIndexChanged" Visible="false"/>
            <asp:SqlDataSource ID="dsKier" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
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
"
                >
            </asp:SqlDataSource>

<%--
                <SelectParameters>
                    <asp:ControlParameter Name="kierId" Type="Int32" ControlID="hidKierId" PropertyName="Value" />
                </SelectParameters>
    --%>

            <asp:DropDownList ID="ddlPrac" runat="server" DataSourceID="dsPrac" DataValueField="Id" DataTextField="Name" AutoPostBack="true" OnSelectedIndexChanged="ddlPrac_SelectedIndexChanged" />

            <asp:SqlDataSource ID="dsPrac" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                SelectCommand="
select null Id, 'wszyscy pracownicy ...' Name, 0 Sort 
union all 
select IdPracownika Id, Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')', '') Name, 1 Sort 
from dbo.fn_GetTree2(@kierId, 0, GETDATE())
order by Sort, Name
"
            >
                <SelectParameters>
                    <asp:ControlParameter Name="kierId" Type="Int32" ControlID="hidKierId" PropertyName="Value" />
                </SelectParameters>
            </asp:SqlDataSource>
            
    <div id="divSelectZmiana" class="cntSelectZmiana printoff" runat="server">
        <div class="title1">
            
            
            

            
            
            
                
            <asp:Label ID="lbZmianyQ" runat="server" CssClass="t5" Text="Zmiany"></asp:Label>
            <asp:Label ID="lbZmianyE" runat="server" CssClass="t5" Visible="false" Text="1) Wybierz zmianę do naniesienia - zaplanuj zmianę udzielając lub nie udzielając zgody na nadgodziny:"></asp:Label>
            
            <a id="aShowAll" onclick="javascript:$('#aShowAll').fadeOut('slow');$('#paZmianyOld').show('slow',function(){resize();});$('#aHideAll').fadeIn('slow');" class="button">Pokaż nieużywane</a> 
            <a id="aHideAll" onclick="javascript:$('#aHideAll').fadeOut('slow');$('#paZmianyOld').hide('slow',function(){resize();});$('#aShowAll').fadeIn('slow');" class="button" style="display: none;" >Ukryj nieużywane</a> 
            
<%--        
            <a id="aShowAll" onclick="javascript:$('#paZmianyOld').hide();" class="button">Pokaż nieużywane</a> 
            <a id="aHideAll" href="javascript:$('#aHideAll').fadeOut('slow');$('#paZmianyOld').hide('slow',function(){resize();});$('#aShowAll').fadeIn('slow');" class="button" style="display: none;" >Ukryj nieużywane</a> 

            <a id="aShowAll" href="javascript:$('#aShowAll').hide();$('#paZmianyOld').show(function(){resize();});$('#aHideAll').show();" class="button">Pokaż nieużywane</a> 
            <a id="aHideAll" href="javascript:$('#aHideAll').fadeOut('slow');$('#paZmianyOld').hide('slow',function(){resize();});$('#aShowAll').fadeIn('slow');" class="button" style="display: none;" >Ukryj nieużywane</a> 



            cos sie na ie sypie
            <a id="aShowAll" href="javascript:$('#aShowAll').fadeOut('slow');$('#paZmianyOld').show('slow',function(){resize();});$('#aHideAll').fadeIn('slow');" class="button">Pokaż nieużywane</a> 
            <a id="aHideAll" href="javascript:$('#aHideAll').fadeOut('slow');$('#paZmianyOld').hide('slow',function(){resize();});$('#aShowAll').fadeIn('slow');" class="button" style="display: none;" >Ukryj nieużywane</a> 


            <a id="aShowAll" href="javascript:$('#aShowAll').hide('slow');$('#paZmianyOld').show('slow');$('#aHideAll').show('slow');" class="button">Pokaż nieużywane</a> 
            <a id="aHideAll" href="javascript:$('#aHideAll').hide('slow');$('#paZmianyOld').hide('slow');$('#aShowAll').show('slow');" class="button" style="display: none;" >Ukryj nieużywane</a> 
    
            <a id="a1" href="javascript:$('#aShowAll').hide('slow');$('#paZmianyOld').show('slow');$('#aHideAll').show('slow');" class="button">Pokaż nieużywane</a> 
            <a id="a2" href="javascript:$('#aShowAll').show('slow');$('#paZmianyOld').hide('slow');$('#aHideAll').hide('slow');" class="button" style="display: none;" >Ukryj nieużywane</a> 
--%>                       
            <asp:LinkButton ID="lbtShowZmianyOld" runat="server" OnClientClick="javascript:$('#paZmianyOld').show('slow');return false;" Visible="false" >Pokaż nieużywane</asp:LinkButton>            
            <asp:LinkButton ID="lbtHideZmianyOld" runat="server" OnClientClick="javascript:$('#paZmianyOld').hide('slow');return false;" Visible="false" >Ukryj nieużywane</asp:LinkButton>            
        </div>
        <uc1:SelectZmiana ID="cntSelectZmiana" OnSelectedChanged="OnSelectZmiana" Mode="1" runat="server" />
        <div id="paZmianyOld" style="display: none;">
            <uc1:SelectZmiana ID="cntZmianyOld" Mode="2" runat="server" />        
        </div>
    </div>
    <table class="okres_navigator printoff">
        <tr>
            <td class="colleft">
                <asp:Label ID="lbPlanQ" runat="server" CssClass="t5" Text="Plan pracy"></asp:Label>
                <asp:Label ID="lbPlanE" runat="server" CssClass="t5" Visible="false" Text="2) Kliknij w dzień i ustaw zmianę:"></asp:Label>
            </td>
            <td class="colmiddle">
                <uc1:SelectOkres ID="cntSelectOkres" StoreInSession="true" ControlID="cntPlanPracy" OnOkresChanged="cntSelectOkres_Changed" runat="server" />
            </td>
            <td class="colright">
                <asp:Label ID="lbOkresStatus" CssClass="t1" Visible="false" runat="server" ></asp:Label>
                <asp:Button ID="btEditPP" runat="server" Text="Edycja" CssClass="button75" onclick="btEditPP_Click" />
                <asp:Button ID="btScheme" runat="server" Text="Schemat" Visible="false" CssClass="button75" UseSubmitBehavior="false" 
                    data-toggle="modal" data-target="#myModal" OnClientClick="return false;" />
                <asp:Button ID="btCheckPP" runat="server" Text="Sprawdź" Visible="false" CssClass="button75" onclick="btCheckPP_Click" />
                <asp:Button ID="btSavePP" runat="server" Text="Zapisz" Visible="false" CssClass="button75" onclick="btSavePP_Click" />
                <asp:Button ID="btCancelPP" runat="server" Text="Anuluj" Visible="false" CssClass="button75" onclick="btCancelPP_Click" />
                <asp:Button ID="btPrint" class="button75" runat="server" Text="Drukuj" OnClientClick="printPreview();return false;" Visible="false"/>

            </td>
        </tr>
    </table>

    <div id="paTitle" runat="server" visible="false" class="printon">
        <uc1:RepHeader ID="repHeader" Icon="2" Caption="Plan pracy" runat="server" />
    </div>                
    <uc1:PlanPracy ID="cntPlanPracy" Mode="0" runat="server" />
    <div id="paLegenda" runat="server" visible="false" class="printon legendaZmiany">
        <div class="spacer16"></div>
        Zmiany:
        <uc1:SelectZmiana ID="cntZmianyLegenda" Mode="1" runat="server" />
    </div>        
</div>








<div id="myModal" class="modal fade" role="dialog">
  <div class="modal-dialog">

    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Ustaw schemat</h4>
      </div>
      <div class="modal-body">
          <div class="form-group">
              <label>Schemat:</label>
              <asp:DropDownList ID="ddlSchemes" runat="server" DataSourceID="dsSchemes" DataValueField="Id" DataTextField="Name" CssClass="form-control" />
              <asp:SqlDataSource ID="dsSchemes" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                  SelectCommand="
select null Id, 'wybierz ...' Name, 0 Sort union all
select Id, Nazwa Name, 1 Sort from rcpSchematy
order by Sort, Name "

                  />
          </div>
          <div class="form-group">
            <label>Pracownik:</label>
            <asp:DropDownList ID="ddlPrac2" runat="server" DataSourceID="dsPrac2" DataValueField="Id" DataTextField="Name" CssClass="form-control" />
            <asp:SqlDataSource ID="dsPrac2" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" CancelSelectOnNullParameter="false"
                SelectCommand="
select null Id, 'wszyscy pracownicy ...' Name, 0 Sort 
union all 
select IdPracownika Id, Nazwisko + ' ' + Imie + isnull(' (' + KadryId + ')', '') Name, 1 Sort 
from dbo.fn_GetTree2(@kierId, 0, GETDATE())
order by Sort, Name
"
            >
                <SelectParameters>
                    <asp:ControlParameter Name="kierId" Type="Int32" ControlID="hidKierId" PropertyName="Value" />
                </SelectParameters>
            </asp:SqlDataSource>
          </div>
          <div class="form-group">
              <label>Data od:</label>
              <cc:DateEdit ID="deLeft" runat="server" />
          </div>
          <div class="form-group">
              <label>Data do:</label>
              <cc:DateEdit ID="deRight" runat="server" />
          </div>
      </div>
      <div class="modal-footer">
        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" OnClick="btnSave_Click"  />
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>

<asp:SqlDataSource ID="dsApplyScheme" runat="server" SelectCommand=
"
declare @schId int = {0}
declare @pracId int = {1}
declare @od datetime = {2}
declare @do datetime = {3}
declare @accId int = -1
declare @nadzien datetime = GETDATE()

declare @sch nvarchar(512)
declare @dow int

select @sch = Schemat from rcpSchematy where Id = @schId
select @dow = dbo.dow(@od)

select
  @pracId IdPracownika
, d.Data Data
, s.items IdZmiany
, @nadzien DataZm
into #aaa
from dbo.GetDates2(@od, @do) d
outer apply (select (d.Lp + @dow) / 7 % (select COUNT(items) from dbo.SplitIntSort(@sch, ',')) c) oa
left join dbo.SplitIntSort(@sch, ',') s on s.idx = oa.c
left join Kalendarz k on k.Data = d.Data
where ISNULL(k.Rodzaj, -1) not in (0, 1, 2)

update PlanPracy set
  IdZmiany = a.IdZmiany
, DataZm = a.DataZm
, DataAcc = a.DataZm
from PlanPracy pp
inner join #aaa a on a.IdPracownika = pp.IdPracownika and a.Data = pp.Data

insert into PlanPracy (IdPracownika, Data, IdZmiany, DataZm, IdKierownikaZm, Akceptacja, DataAcc, IdKierownikaAcc)
select a.IdPracownika, a.Data, a.IdZmiany, a.DataZm, @accId, 0, a.DataZm, @accId
from #aaa a
inner join PlanPracy pp on pp.IdPracownika = a.IdPracownika and pp.Data = a.Data
where pp.Id is null

drop table #aaa
" />