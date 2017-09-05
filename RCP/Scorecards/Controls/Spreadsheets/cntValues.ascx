<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cntValues.ascx.cs" Inherits="HRRcp.Scorecards.Controls.Spreadsheets.cntValues" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%--<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
<div id="ctValues" runat="server" class="cntValues selectable cnt cnt2">
    <asp:HiddenField ID="hidScorecardTypeId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidEmployeeId" runat="server" Visible="false" />
    <asp:HiddenField ID="hidDate" runat="server" Visible="false" />
    <asp:HiddenField ID="hidOutsideJob" runat="server" Visible="false" />
    <table class="tbValues">
        <asp:Repeater ID="rpDays" runat="server" OnItemDataBound="rpDays_ItemDataBound" OnDataBinding="rpDays_DataBinding" >
            <ItemTemplate>
                <tr>
                    <asp:HiddenField ID="hidKonkret" runat="server" Value='<%# Eval("Date") %>' Visible="false" />
                    <asp:HiddenField ID="hidDayExists" runat="server" Value='<%# Eval("Bang") %>' Visible="false" />
                    
                    
                    
                    
                    
                    <asp:Repeater ID="rpValues" runat="server" >
                        <ItemTemplate>
                            <td id="tdValue" runat="server" data-value='<%# GetValue(Container.DataItem, 1) %>' >
                                <asp:HiddenField ID="hidState" runat="server" Visible="false" />
                                <asp:HiddenField ID="hidTaskId" runat="server" Value='<%# GetValue(Container.DataItem, 0) %>' Visible="false" />
                                <asp:HiddenField ID="hidOldValue" runat="server" Value='<%# GetValue(Container.DataItem, 1) %>' Visible="false" />
                                <asp:Label ID="lblValue" runat="server" Text='<%# GetValue(Container.DataItem, 1) %>' Visible='<%# !IsInEdit() %>' />
                                <asp:TextBox ID="tbValue" runat="server" Text='<%# GetValue(Container.DataItem, 1) %>' MaxLength="6" autocomplete="off" CssClass="tbValue" />
                                <asp:FilteredTextBoxExtender ID="tbFilter" runat="server" TargetControlID="tbValue" FilterType="Custom" ValidChars="0123456789" />
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                    
                    
                    
                    
                    
                    
                    
                    
                    
<%--                    <asp:SqlDataSource ID="dsValues" runat="server" OnSelecting="dsValues_Selecting"
                        ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" SelectCommand="
declare @od as datetime = CONVERT(datetime, Convert(varchar, YEAR(@date)) + '-' + convert(varchar, MONTH(@date)) + '-01' )
declare @do as datetime = DATEADD(D, -1, DATEADD(M,1,@od))
select distinct c.Nazwa, w.Ilosc, c.Id as TaskId, case when @oj = 1 then @bang else (case when ISNULL(p.IdCommodity, -1) = @typark then 1 else 0 end) end as State
from scTypyArkuszyCzynnosci tac
left join scWartosci w on tac.IdCzynnosci = w.IdCzynnosci and w.IdTypuArkuszy = @typark and w.IdPracownika = @pracId and w.Data = @konkret
left join scCzynnosci c on c.Id = tac.IdCzynnosci
left join Przypisania p on (@konkret between p.Od and ISNULL(p.Do, '20990909')) and ((p.IdPracownika = @pracId and IdCommodity = @typark) or (@pracId &lt; 0 and IdCommodity = @typark))
where tac.IdTypuArkuszy = @typark and tac.Od &lt;= @do and @od &lt;= ISNULL(tac.Do, '20990909')
order by c.Nazwa
">
                        <SelectParameters>
                            <asp:Parameter Name="typark" Type="Int32" />
                            <asp:Parameter Name="pracId" Type="Int32" />
                            <asp:Parameter Name="date" Type="DateTime" />
                            <asp:ControlParameter Name="konkret" Type="String" ControlID="hidKonkret" PropertyName="Value" />
                            <asp:ControlParameter Name="bang" Type="String" ControlID="hidDayExists" PropertyName="Value" />
                            <asp:Parameter Name="oj" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>--%>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
<%--    <asp:SqlDataSource ID="dsDays" runat="server" ConnectionString="<%$ ConnectionStrings:HRConnectionString %>"
        SelectCommand="
declare @pierwszy as datetime = CONVERT(datetime, Convert(varchar, YEAR(@date)) + '-' + convert(varchar, MONTH(@date)) + '-01' );
with Daiz
as
(
	select @pierwszy  as tDate
	union all
	select DATEADD(D, 1, tDate) from Daiz where DATEADD(D, 1, tDate) &lt; DATEADD(M, 1, @pierwszy)
)
select dz.tDate, case when d.Id is not null then 1 else 0 end as Bang from Daiz dz
left join scDni d on dz.tDate = d.Data and d.IdTypuArkuszy = @typark and d.IdPracownika = @pracId
">
        <SelectParameters>
            <asp:ControlParameter Name="date" Type="DateTime" ControlID="hidDate" PropertyName="Value" />
            <asp:ControlParameter Name="typark" Type="Int32" ControlID="hidScorecardTypeId" PropertyName="Value" />
            <asp:ControlParameter Name="pracId" Type="Int32" ControlID="hidEmployeeId" PropertyName="Value" />
        </SelectParameters>
    </asp:SqlDataSource>--%>
</div>
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>