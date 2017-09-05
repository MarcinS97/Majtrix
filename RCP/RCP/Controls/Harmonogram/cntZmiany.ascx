<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntZmiany.ascx.cs" Inherits="HRRcp.RCP.Controls.Harmonogram.cntZmiany" %>

<asp:HiddenField ID="hidMin" runat="server" Visible="false" />
<asp:HiddenField ID="hidMax" runat="server" Visible="false" />

<div class="xpanel xpanel-default">
    <div id="ctZmiany" runat="server" class="cntZmiany xpanel-body" >
        <div id="carousel-example-generic" class="carousel slide" data-ride="carousel" data-interval="false" style="display: inline-block;">
            <a class="xleft xcarousel-control chevron chevron-right" href="#carousel-example-generic" role="button" data-slide="prev" runat="server" id="leftChevron">
                <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
                <%--<span class="sr-only">Previous</span>--%>
            </a>
            <div class="carousel-inner toolbox cntZmianySelect clearfix" role="listbox">

                <asp:Repeater ID="rpShiftGroups" runat="server" DataSourceID="dsShiftGroups">
                    <ItemTemplate>
                        <div class='<%# Eval("Class") %>' style="xpadding: 0px 22px;">

                            <asp:HiddenField ID="hidPar" runat="server" Visible="true" Value='<%# Eval("Par") %>' />

                            <asp:SqlDataSource ID="dsShifts" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                                OnSelected="dsShifts_Selected"
                                SelectCommand="
select z.* 
--, right(convert(varchar(5), Od, 20), 8) TimeOd, right(convert(varchar(5), Do, 20), 8) TimeDo 
, case when TypZmiany != 2 then left(right(convert(varchar, Od, 20), 8), 5) else null end TimeOd
, case when TypZmiany != 2 then left(right(convert(varchar, Do, 20), 8), 5) else null end TimeDo 
, case when TypZmiany != 2 then Margines else null end MarginesT
--, case when (z.WymiarOd = z.WymiarDo and z.WymiarOd &lt;= 0) then '' else '8' end as Time
, case when TypZmiany = 2 then 0 else 8 end Time
, case when TypZmiany != 2 then '8' else '' end TimeText
from Zmiany z 
inner join (select items from dbo.SplitInt(@par, ',')) a on a.items = z.Id
order by Kolejnosc
                                ">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hidPar" PropertyName="Value" Type="String" Name="par" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <%--data-content='Godz. rozpoczęcia: <%# Eval("TimeOd") %> &lt;br /&gt; Godz. zakończenia: <%# Eval("TimeDo") %> &lt;br /&gt; Margines: <%# Eval("MarginesT") %>  '--%>

                            <asp:Repeater ID="rpShifts" runat="server" DataSourceID="dsShifts">
                                <ItemTemplate>
                                    <div id="shiftToggler" runat="server" style="display: inline-block; position: relative;" class="shifts-toggler">
                                        <button type="button" class="btn btn-sm zmiana main-shift" data-id='<%# Eval("Id") %>' data-color='<%# Eval("Kolor") %>'
                                            data-name='<%# Eval("Symbol") %>' style='background-color: <%# Eval("Kolor") %>' title='<%# Eval("Nazwa") %>'
                                            aria-haspopup="true" aria-expanded="false" data-time='<%# Eval("Time") %>' data-toggle="popover" 
                                            data-html="true" data-trigger="focus"  data-placement="bottom">
                                            <span class="name"><%# Eval("Symbol") %></span>
                                            <span class="no"><%# Eval("TimeText") %></span>
                                        </button>

                                        <asp:HiddenField ID="hidZid" runat="server" Visible="false" Value='<%# Eval("Id") %>' />

                                        <%--<a data-toggle="dropdown" class="dropdown-toggle" href="#">asdasdas</a>--%>
                                        <ul class="dropdown-menu shift-list" style="display: none;">
                                            <asp:Repeater ID="rpItems" runat="server" DataSourceID="dsItems">
                                                <ItemTemplate>
                                                    <li>
                                                        <button type="button" class="btn btn-sm zmiana" data-id='<%# Eval("Id") %>' 
                                                            data-color='<%# Eval("Kolor") %>' data-name='<%# Eval("Symbol") %>'
                                                            style='background-color: <%# Eval("Kolor") %>' title='<%# Eval("Nazwa") %>' 
                                                            data-time='<%# Eval("idx") %>'>
                                                            <span class="name"><%# Eval("Symbol") %></span>
                                                            <span class="no"><%# Eval("idx") %></span>
                                                        </button>

                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>

                                        <asp:SqlDataSource ID="dsItems" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
                                            SelectCommand="
/*with nums(a) as
(
    select isnull((select WymiarOd from Zmiany where Id = @zid), @od)
    union all
    select a + 1 from nums where a &lt; isnull((select WymiarDo from Zmiany where Id = @zid), @do)
)
select a idx, z.* from nums
left join Zmiany z on z.Id = @zid
where z.WymiarOd != z.WymiarDo*/

select
  a.items / 3600 idx
, z.*
from dbo.SplitInt((select '28800;' + z.InneCzasy from Zmiany z where z.Id = @zid ), ';') a
outer apply (select top 1 * from Zmiany z where z.Id = @zid) z
">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="hidMin" PropertyName="Value" Name="od" Type="Int32" />
                                                <asp:ControlParameter ControlID="hidMax" PropertyName="Value" Name="do" Type="Int32" />
                                                <asp:ControlParameter ControlID="hidZid" PropertyName="Value" Name="zid" Type="Int32" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>





                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>




            <!-- Controls -->
            
            <a class="xright xcarousel-control chevron chevron-right" href="#carousel-example-generic" role="button" data-slide="next" runat="server" id="rightChevron">
                <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
                <%--<span class="sr-only">Next</span>--%>
            </a>



        </div>
        <%--    <div style="margin-left: 12px; overflow-y: auto;">
        <asp:Repeater ID="rpShifts" runat="server" DataSourceID="dsShifts">
            <ItemTemplate>
                <button type="button" class="btn btn-sm zmiana" data-id='<%# Eval("Id") %>' data-color='<%# Eval("Kolor") %>' data-name='<%# Eval("Symbol") %>'
                    style='background-color: <%# Eval("Kolor") %>' title='<%# Eval("Nazwa") %>'>
                    <%# Eval("Symbol") %></button>
            </ItemTemplate>
        </asp:Repeater>
        </div>
        <asp:SqlDataSource ID="dsShifts" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
        SelectCommand="select * from Zmiany" />
        --%>

        <div class="checkbox" style="display: inline-block; position: static;">
            <label>
                <input id="cbFillFree" runat="server" type="checkbox" class="cbFillFree"> <%--checked--%>
                Wypełniaj dni wolne
            </label>
        </div>
        <asp:SqlDataSource ID="dsShiftGroups" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
            SelectCommand="
declare @mod float = 6

select
  dbo.cat(Id, ',', 0) Par
, case when a.a = 1 then 'item active dropdown' else 'item dropdown' end Class                                    
from
    (
    select
      CEILING(CAST(ROW_NUMBER() over (order by Kolejnosc) as float) / @mod) a
    , Id
    from Zmiany
) a
group by a.a" />



    </div>
</div>
